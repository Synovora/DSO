Imports System.Net
Imports System.Net.Http
Imports Oasis_Common
Imports Oasis_Web

''' <summary>
''' /api/motdepasse : le serveur calcule et écrit l'empreinte, pour le compte qui
''' vient de prouver son mot de passe, ou pour un tiers si l'appelant est
''' administrateur.
''' </summary>
<TestClass()> Public Class TestControleurMotDePasse
    Inherits TestIntegration

    Private Const MotDePasseSuivant As String = "Nouveau#2027a"

    <TestInitialize>
    Public Sub PreparerServeur()
        UtiliserCompte(Compte.Web)
        OuvrirContexteHttp()
    End Sub

    <TestCleanup>
    Public Sub FermerServeur()
        FermerContexteHttp()
    End Sub

    Private Shared Function DemanderChangement(login As String, motDePasseActuel As String,
                                               demande As MotDePasseRequest) As HttpResponseMessage
        Return AppelerApi(Of MotDePasseController)(EnteteBasic(login, motDePasseActuel), "Changer",
                                                   Function(c) c.Changer(demande))
    End Function

    Private Shared Function EmpreinteEnBase(idUtilisateur As Long) As String
        Return CStr(Scalaire("SELECT oa_password FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idUtilisateur)).Trim()
    End Function

    Private Shared Function UsageUnique(idUtilisateur As Long) As Boolean
        Return CBool(Scalaire("SELECT COALESCE(oa_utilisateur_password_is_unique_usage, 0) FROM oasis.oa_utilisateur" &
                              " WHERE oa_utilisateur_id = @p0", idUtilisateur))
    End Function

    <TestMethod()> Public Sub LeTitulaireChangeSonMotDePasse()
        Dim idUtilisateur = CreerUtilisateur("mdp.titulaire")

        Dim reponse = DemanderChangement("mdp.titulaire", MotDePasseParDefaut,
                                         New MotDePasseRequest With {.NouveauMotDePasse = MotDePasseSuivant})

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        ' Comportement actuel : le corps est la chaîne JSON "true", pas un booléen.
        Assert.AreEqual("true", LireJson(Of String)(reponse))
        Dim empreinte = EmpreinteEnBase(idUtilisateur)
        Assert.IsTrue(empreinte.StartsWith("PBKDF2$"), empreinte)
        Assert.IsTrue(MotDePasse.Verifier(MotDePasseSuivant, empreinte))
        Assert.IsFalse(UsageUnique(idUtilisateur), "changé par son titulaire, le mot de passe n'est pas à usage unique")

        Assert.AreEqual(HttpStatusCode.Accepted, SeConnecter("mdp.titulaire", MotDePasseSuivant).StatusCode)
        Assert.AreEqual(HttpStatusCode.Unauthorized, SeConnecter("mdp.titulaire", MotDePasseParDefaut).StatusCode)
    End Sub

    <TestMethod()> Public Sub UnMauvaisMotDePasseActuelEstRefuse()
        Dim idUtilisateur = CreerUtilisateur("mdp.usurpe")
        Dim empreinteAvant = EmpreinteEnBase(idUtilisateur)

        Dim reponse = DemanderChangement("mdp.usurpe", "Mauvais!2026",
                                         New MotDePasseRequest With {.NouveauMotDePasse = MotDePasseSuivant})

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual(empreinteAvant, EmpreinteEnBase(idUtilisateur))
        Assert.AreEqual(HttpStatusCode.Accepted, SeConnecter("mdp.usurpe", MotDePasseParDefaut).StatusCode)
    End Sub

    <TestMethod()> Public Sub SansAuthentificationLeChangementEstRefuse()
        Dim idUtilisateur = CreerUtilisateur("mdp.anonyme")
        Dim empreinteAvant = EmpreinteEnBase(idUtilisateur)

        Dim reponse = AppelerApi(Of MotDePasseController)(Nothing, "Changer",
            Function(c) c.Changer(New MotDePasseRequest With {.UtilisateurId = CInt(idUtilisateur),
                                                              .NouveauMotDePasse = MotDePasseSuivant}))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual(empreinteAvant, EmpreinteEnBase(idUtilisateur))
    End Sub

    <TestMethod()> Public Sub UnMotDePasseHorsPolitiqueEstRefuse()
        Dim idUtilisateur = CreerUtilisateur("mdp.faible")
        Dim empreinteAvant = EmpreinteEnBase(idUtilisateur)

        ' Politique de isValidePassword : 8 caractères, une majuscule, une minuscule,
        ' un chiffre, un caractère spécial.
        Dim refuses = New String() {"Co!1a", "sansmajuscule!1", "SANSMINUSCULE!1", "SansChiffre!!", "SansSpecial12", "", Nothing}
        For Each candidat In refuses
            Dim reponse = DemanderChangement("mdp.faible", MotDePasseParDefaut,
                                             New MotDePasseRequest With {.NouveauMotDePasse = candidat})
            Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode, "refus attendu pour " & If(candidat, "Nothing"))
            Assert.AreEqual("Mot de passe trop faible", CorpsDe(reponse))
        Next

        Assert.AreEqual(empreinteAvant, EmpreinteEnBase(idUtilisateur))
    End Sub

    <TestMethod()> Public Sub ReutiliserLeMotDePasseActuelEstAccepte()
        Dim idUtilisateur = CreerUtilisateur("mdp.identique")
        Dim empreinteAvant = EmpreinteEnBase(idUtilisateur)

        Dim reponse = DemanderChangement("mdp.identique", MotDePasseParDefaut,
                                         New MotDePasseRequest With {.NouveauMotDePasse = MotDePasseParDefaut})

        ' Comportement actuel : aucune règle n'interdit de reprendre le mot de passe
        ' en cours. Seul le sel change.
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.AreNotEqual(empreinteAvant, EmpreinteEnBase(idUtilisateur))
        Assert.AreEqual(HttpStatusCode.Accepted, SeConnecter("mdp.identique", MotDePasseParDefaut).StatusCode)
    End Sub

    <TestMethod()> Public Sub UneRequeteIllisibleEstRefusee()
        CreerUtilisateur("mdp.corps")

        ' Web API remet Nothing à l'action quand le JSON ne se lit pas.
        Dim reponse = DemanderChangement("mdp.corps", MotDePasseParDefaut, Nothing)

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.AreEqual("Requete incomplete", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UnNonAdministrateurNePeutPasChangerLeMotDePasseDUnAutre()
        CreerUtilisateur("mdp.appelant")
        Dim idCible = CreerUtilisateur("mdp.cible")
        Dim empreinteAvant = EmpreinteEnBase(idCible)

        Dim reponse = DemanderChangement("mdp.appelant", MotDePasseParDefaut,
                                         New MotDePasseRequest With {.UtilisateurId = CInt(idCible), .NouveauMotDePasse = MotDePasseSuivant})

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual(empreinteAvant, EmpreinteEnBase(idCible))
        Assert.AreEqual(HttpStatusCode.Accepted, SeConnecter("mdp.cible", MotDePasseParDefaut).StatusCode)
    End Sub

    <TestMethod()> Public Sub UnAdministrateurPoseUnMotDePasseAUsageUnique()
        Dim idAdmin = CreerUtilisateur("mdp.admin", admin:=True)
        Dim idCible = CreerUtilisateur("mdp.administre")

        Dim reponse = DemanderChangement("mdp.admin", MotDePasseParDefaut,
                                         New MotDePasseRequest With {.UtilisateurId = CInt(idCible), .NouveauMotDePasse = MotDePasseSuivant})

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.IsTrue(UsageUnique(idCible))
        Assert.IsFalse(UsageUnique(idAdmin), "le mot de passe de l'administrateur ne bouge pas")
        Assert.AreEqual(HttpStatusCode.Unauthorized, SeConnecter("mdp.administre", MotDePasseParDefaut).StatusCode)

        Dim connexion = SeConnecter("mdp.administre", MotDePasseSuivant)
        Assert.AreEqual(HttpStatusCode.Accepted, connexion.StatusCode)
        ' Comportement actuel : le serveur ouvre la session et se contente de signaler
        ' l'usage unique. C'est le client lourd qui impose le changement.
        Assert.IsTrue(LireJson(Of LoginResponse)(connexion).Utilisateur.IsPasswordUniqueUsage)
    End Sub

    <TestMethod()> Public Sub SeDesignerParSonIdentifiantNEstPasUnChangementPourAutrui()
        Dim idUtilisateur = CreerUtilisateur("mdp.soimeme")

        Dim reponse = DemanderChangement("mdp.soimeme", MotDePasseParDefaut,
                                         New MotDePasseRequest With {.UtilisateurId = CInt(idUtilisateur), .NouveauMotDePasse = MotDePasseSuivant})

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.IsFalse(UsageUnique(idUtilisateur))
    End Sub

    <TestMethod()> Public Sub LeChangementParUnAdministrateurLeveLeVerrou()
        CreerUtilisateur("mdp.deverrouille", admin:=True)
        Dim idCible = CreerUtilisateur("mdp.verrouille")
        Executer("UPDATE oasis.oa_utilisateur SET oa_utilisateur_tentatives = 5," &
                 " oa_utilisateur_verrou_jusqua = DATEADD(minute, 10, SYSDATETIME()) WHERE oa_utilisateur_id = @p0", idCible)

        Dim reponse = DemanderChangement("mdp.deverrouille", MotDePasseParDefaut,
                                         New MotDePasseRequest With {.UtilisateurId = CInt(idCible), .NouveauMotDePasse = MotDePasseSuivant})

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.AreEqual(0, CInt(Scalaire("SELECT COALESCE(oa_utilisateur_tentatives, 0) FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idCible)))
        Assert.IsTrue(IsDBNull(Scalaire("SELECT oa_utilisateur_verrou_jusqua FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idCible)))
        Assert.AreEqual(HttpStatusCode.Accepted, SeConnecter("mdp.verrouille", MotDePasseSuivant).StatusCode)
    End Sub

    <TestMethod()> Public Sub LeChangementEstJournalise()
        Dim idUtilisateur = CreerUtilisateur("mdp.journal")

        DemanderChangement("mdp.journal", MotDePasseParDefaut,
                           New MotDePasseRequest With {.NouveauMotDePasse = MotDePasseSuivant})

        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_action WHERE utilisateur_id = @p0" &
                                         " AND action = @p1", idUtilisateur,
                                         "MODIFICATION : Changement de mot de passe du compte " & idUtilisateur)))
    End Sub

    <TestMethod()> Public Sub UnCompteInexistantEchoueEn500()
        CreerUtilisateur("mdp.fantome", admin:=True)

        Dim reponse = DemanderChangement("mdp.fantome", MotDePasseParDefaut,
                                         New MotDePasseRequest With {.UtilisateurId = 999999999, .NouveauMotDePasse = MotDePasseSuivant})

        ' Comportement actuel : UpdateMotDePasse lève ArgumentException, rendue en 500
        ' et non en 404.
        Assert.AreEqual(HttpStatusCode.InternalServerError, reponse.StatusCode)
        Assert.AreEqual("Erreur interne au serveur", CorpsDe(reponse))
    End Sub

End Class
