Imports System.Text.RegularExpressions
Imports System.Web.Mvc
Imports System.Web.Security
Imports Oasis_Common
Imports Oasis_Web.Models
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' AuthController : connexion au portail patient, verrouillage après échecs,
''' récupération du mot de passe par lien à usage unique, déconnexion. Tout tourne
''' sous oasis_web, le compte du serveur. On teste le résultat de l'action et ce
''' qu'elle écrit en base, jamais le rendu de la vue.
''' </summary>
<TestClass()> Public Class AuthControllerTest
    Inherits TestIntegration

    Private Const MessageRefus As String = "Identifiant et/ou mot de passe erroné !"
    Private Const MessageVerrou As String = "Compte temporairement verrouillé suite à plusieurs échecs. Réessayez plus tard."
    Private Const MessageErreur As String = "Une erreur est survenue, veuillez réessayer."
    Private Const MessageLienExpire As String = "Lien de récupération invalide ou expiré."
    Private Const MessageLienMalforme As String = "Le lien de récupération n'est pas valide."
    Private Const MessageCleMalformee As String = "La recovery key n'est pas valide."
    Private Const NouveauMotDePasse As String = "Nouveau!Mdp2027"

    <TestInitialize>
    Public Sub OuvrirPortail()
        OuvrirHttpContextPortail()
    End Sub

    <TestCleanup>
    Public Sub FermerPortail()
        FermerHttpContextPortail()
    End Sub

    ' --- Aides ------------------------------------------------------------------------

    Private Shared Function AdresseDeTest() As String
        Return NouveauLogin() & "@exemple.fr"
    End Function

    Private Shared Function Connecter(email As String, motDePasseSaisi As String,
                                      Optional retour As String = Nothing,
                                      Optional seSouvenir As Boolean = False) As ActionResult
        Dim controleur = ControleurPortailAnonyme(Of AuthController)("POST")
        Return controleur.Login(New UserLogin With {.Email = email, .Password = motDePasseSaisi, .RememberMe = seSouvenir}, retour)
    End Function

    Private Shared Function MessageDe(resultat As ActionResult) As String
        Return CStr(VuePortail(resultat).ViewData("Message"))
    End Function

    Private Shared Function Tentatives(idInternaute As Long) As Integer
        Return CInt(Scalaire("SELECT COALESCE(tentatives, 0) FROM oasis.oa_internaute WHERE id = @p0", idInternaute))
    End Function

    Private Shared Function EstVerrouille(idInternaute As Long) As Boolean
        Return CInt(Scalaire("SELECT CASE WHEN verrou_jusqua > SYSDATETIME() THEN 1 ELSE 0 END FROM oasis.oa_internaute WHERE id = @p0",
                             idInternaute)) = 1
    End Function

    Private Shared Sub Verrouiller(idInternaute As Long, nbEchecs As Integer, minutes As Integer)
        Executer("UPDATE oasis.oa_internaute SET tentatives = @p0, verrou_jusqua = DATEADD(minute, @p1, SYSDATETIME()) WHERE id = @p2",
                 nbEchecs, minutes, idInternaute)
    End Sub

    Private Shared Function NombreConnexions(idInternaute As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_internaute_connection WHERE internaute = @p0", idInternaute))
    End Function

    Private Shared Function Colonne(nomColonne As String, idInternaute As Long) As Object
        Return Scalaire("SELECT " & nomColonne & " FROM oasis.oa_internaute WHERE id = @p0", idInternaute)
    End Function

    Private Shared Function Empreinte(idInternaute As Long) As String
        Return ChaineLue(Colonne("password", idInternaute))
    End Function

    ''' <summary>Clé de récupération au format attendu : 64 chiffres hexadécimaux majuscules.</summary>
    Private Shared Function NouvelleCle() As String
        Return (Guid.NewGuid().ToString("N") & Guid.NewGuid().ToString("N")).ToUpperInvariant()
    End Function

    ''' <summary>
    ''' Demande de récupération enregistrée par InternauteDao.UpdateRecovery, le DAO
    ''' qu'appelle Forgot, sous le compte Web.
    ''' </summary>
    Private Shared Sub PoserCle(idInternaute As Long, cle As String, expiration As Date)
        UtiliserCompte(Compte.Web)
        Dim dao As New InternauteDao
        dao.UpdateRecovery(CInt(idInternaute), cle, expiration, Nothing)
    End Sub

    Private Shared Function ReinitialiserMotDePasse(cle As String, saisie As String, Optional confirmation As String = Nothing) As ActionResult
        Dim controleur = ControleurPortailAnonyme(Of AuthController)("POST")
        Return controleur.Recover(New UserRecover With {
            .Recovery = cle, .Password = saisie, .PasswordBis = If(confirmation, saisie)})
    End Function

    Private Shared Function OuvrirLien(cle As String) As ViewResult
        Return VuePortail(ControleurPortailAnonyme(Of AuthController)().Recover(cle))
    End Function

    ''' <summary>Retire modèle et paramètres SMTP : aucun courriel ne peut partir pendant le test.</summary>
    Private Shared Sub InterdireCourriel()
        Executer("DELETE FROM oasis.oa_r_mail_parameter WHERE type_mail_param IN (@p0, @p1)",
                 ParametreMail.TypeMailParams.INTERNAUTE_RESET.ToString(),
                 ParametreMail.TypeMailParams.SMTP_PARAMETERS.ToString())
    End Sub

    ' --- Pages sans traitement -------------------------------------------------------

    <TestMethod()> Public Sub LesPagesSimplesRendentLeurVue()
        Dim controleur = ControleurPortailAnonyme(Of AuthController)()
        Assert.AreEqual("login", VuePortail(controleur.Index()).ViewName)
        Assert.AreEqual("", VuePortail(controleur.Login()).ViewName)
        Assert.AreEqual("", VuePortail(controleur.Forgot()).ViewName)
        Assert.AreEqual("", VuePortail(controleur.Register()).ViewName)
        Assert.AreEqual("", VuePortail(controleur.LockScreen()).ViewName)
    End Sub

    <TestMethod()> Public Sub ToutSaufLaDeconnexionEstOuvertAuxAnonymes()
        Dim controleur = ControleurPortailAnonyme(Of AuthController)()
        Assert.IsNull(AutorisationPortail(controleur, "Index"))
        Assert.IsNull(AutorisationPortail(controleur, "Login"))
        Assert.IsNull(AutorisationPortail(controleur, "Login", GetType(UserLogin), GetType(String)))
        Assert.IsNull(AutorisationPortail(controleur, "Forgot"))
        Assert.IsNull(AutorisationPortail(controleur, "Forgot", GetType(UserForgot)))
        Assert.IsNull(AutorisationPortail(controleur, "Recover", GetType(String)))
        Assert.IsNull(AutorisationPortail(controleur, "Recover", GetType(UserRecover)))
        Assert.IsNull(AutorisationPortail(controleur, "Register"))
        Assert.IsNull(AutorisationPortail(controleur, "LockScreen"))
        VerifierNonAuthentifiePortail(AutorisationPortail(controleur, "Logout"))
    End Sub

    <TestMethod()> Public Sub LaDeconnexionEstPermiseAUnInternauteAuthentifie()
        Dim idInternaute = CreerComptePortail(CreerPatient())
        Assert.IsNull(AutorisationPortail(ControleurPortail(Of AuthController)(idInternaute), "Logout"))
    End Sub

    ' --- Connexion --------------------------------------------------------------------

    <TestMethod()> Public Sub UnBonMotDePasseOuvreLaSessionDeLInternaute()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)

        Dim resultat = Connecter(adresse, MotDePasseParDefaut)

        VerifierRedirectionPortail(resultat, "Dashboard", "Index")
        Dim cookie = CookieFormsPortail()
        Assert.IsNotNull(cookie, "cookie d'authentification posé")
        Dim ticket = FormsAuthentication.Decrypt(cookie.Value)
        Assert.AreEqual(CStr(idInternaute), ticket.Name, "le ticket ne porte que l'id de l'internaute")
        Assert.IsFalse(ticket.IsPersistent)
        Assert.IsTrue(cookie.HttpOnly)
        Assert.AreEqual(1, NombreConnexions(idInternaute))
        Assert.AreEqual(0, Tentatives(idInternaute))
    End Sub

    <TestMethod()> Public Sub SeSouvenirDeMoiRendLeTicketPersistant()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        CreerComptePortail(CreerPatient(), adresse)

        Connecter(adresse, MotDePasseParDefaut, seSouvenir:=True)

        Assert.IsTrue(FormsAuthentication.Decrypt(CookieFormsPortail().Value).IsPersistent)
    End Sub

    <TestMethod()> Public Sub LaConnexionEnregistreLAdresseDeLAppelant()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)

        Connecter(adresse, MotDePasseParDefaut)

        Assert.AreEqual(AdresseAppelantPortail,
                        CStr(Scalaire("SELECT TOP 1 ip FROM oasis.oa_internaute_connection WHERE internaute = @p0 ORDER BY id DESC", idInternaute)).Trim())
    End Sub

    <TestMethod()> Public Sub DerriereUnProxyLaPremiereAdresseTransmiseEstEnregistree()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Dim controleur = ControleurPortailAnonyme(Of AuthController)("POST")
        ContextePortail(controleur).FausseRequete.ServerVariables("HTTP_X_FORWARDED_FOR") = " 203.0.113.7 , 10.0.0.1"

        VerifierRedirectionPortail(controleur.Login(New UserLogin With {.Email = adresse, .Password = MotDePasseParDefaut}, Nothing),
                                   "Dashboard", "Index")

        ' Comportement actuel : l'en-tête X-Forwarded-For est pris tel quel, sans
        ' vérifier qu'il vient d'un proxy de confiance ; un client peut y écrire
        ' l'adresse de son choix.
        Assert.AreEqual("203.0.113.7",
                        CStr(Scalaire("SELECT TOP 1 ip FROM oasis.oa_internaute_connection WHERE internaute = @p0 ORDER BY id DESC", idInternaute)).Trim())
    End Sub

    <TestMethod()> Public Sub UneAdresseDeRetourLocaleEstSuivie()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        CreerComptePortail(CreerPatient(), adresse)

        Dim resultat = Connecter(adresse, MotDePasseParDefaut, "/Resultats/Index")

        Assert.IsInstanceOfType(resultat, GetType(RedirectResult))
        Assert.AreEqual("/Resultats/Index", DirectCast(resultat, RedirectResult).Url)
    End Sub

    <TestMethod()> Public Sub UneAdresseDeRetourExterneEstIgnoree()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        CreerComptePortail(CreerPatient(), adresse)

        For Each retour In {"https://exemple.invalid/piege", "//exemple.invalid/piege", "/\exemple.invalid", "javascript:alert(1)"}
            VerifierRedirectionPortail(Connecter(adresse, MotDePasseParDefaut, retour), "Dashboard", "Index")
        Next
    End Sub

    <TestMethod()> Public Sub UnMauvaisMotDePasseEstRefuseEtCompte()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)

        Dim resultat = Connecter(adresse, "Faux!MotDePasse1")

        Assert.AreEqual(MessageRefus, MessageDe(resultat))
        Assert.AreEqual("", VuePortail(resultat).ViewName)
        Assert.AreEqual(1, Tentatives(idInternaute))
        Assert.IsNull(CookieFormsPortail(), "aucun ticket pour un échec")
        Assert.AreEqual(0, NombreConnexions(idInternaute))
    End Sub

    <TestMethod()> Public Sub UneAdresseInconnueRecoitLaMemeReponse()
        Dim resultat = Connecter(AdresseDeTest(), MotDePasseParDefaut)

        Assert.AreEqual(MessageRefus, MessageDe(resultat))
        Assert.IsNull(CookieFormsPortail())
    End Sub

    <TestMethod()> Public Sub SansEmpreinteEnregistreeAucunMotDePasseNePasse()
        ' Compte créé sans mot de passe, en attente de son premier lien.
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Executer("UPDATE oasis.oa_internaute SET password = NULL WHERE id = @p0", idInternaute)

        Assert.AreEqual(MessageRefus, MessageDe(Connecter(adresse, "")))
        Assert.AreEqual(MessageRefus, MessageDe(Connecter(adresse, MotDePasseParDefaut)))
        Assert.AreEqual(2, Tentatives(idInternaute))
    End Sub

    <TestMethod()> Public Sub QuatreEchecsNeVerrouillentPas()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)

        For i = 1 To 4
            Connecter(adresse, "Faux!MotDePasse" & i)
        Next

        Assert.AreEqual(4, Tentatives(idInternaute))
        Assert.IsFalse(EstVerrouille(idInternaute))
    End Sub

    <TestMethod()> Public Sub CinqEchecsVerrouillentLeComptePourUnQuartDHeure()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)

        For i = 1 To 5
            Assert.AreEqual(MessageRefus, MessageDe(Connecter(adresse, "Faux!MotDePasse" & i)))
        Next

        Assert.AreEqual(5, Tentatives(idInternaute))
        Dim secondes = CInt(Scalaire("SELECT DATEDIFF(second, SYSDATETIME(), verrou_jusqua) FROM oasis.oa_internaute WHERE id = @p0",
                                     idInternaute))
        Assert.IsTrue(secondes > 14 * 60 AndAlso secondes <= 15 * 60, "verrou de 15 minutes, reste " & secondes & " s")
    End Sub

    <TestMethod()> Public Sub UnCompteVerrouilleEstRefuseMemeAvecLeBonMotDePasse()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Verrouiller(idInternaute, 5, 10)

        Dim resultat = Connecter(adresse, MotDePasseParDefaut)

        ' Comportement actuel : le message diffère de celui d'un mauvais mot de passe.
        ' Il révèle qu'un compte existe à cette adresse une fois qu'il est verrouillé.
        Assert.AreEqual(MessageVerrou, MessageDe(resultat))
        Assert.IsNull(CookieFormsPortail())
        Assert.AreEqual(0, NombreConnexions(idInternaute))
        Assert.AreEqual(5, Tentatives(idInternaute), "un essai pendant le verrou ne compte pas")
    End Sub

    <TestMethod()> Public Sub UnVerrouEchuLaisseEntrerEtRemetLeCompteurAZero()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Verrouiller(idInternaute, 5, -1)

        VerifierRedirectionPortail(Connecter(adresse, MotDePasseParDefaut), "Dashboard", "Index")

        Assert.AreEqual(0, Tentatives(idInternaute))
        Assert.AreEqual(DBNull.Value, Colonne("verrou_jusqua", idInternaute))
    End Sub

    <TestMethod()> Public Sub ApresUnVerrouEchuUnSeulEchecReverrouille()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Verrouiller(idInternaute, 5, -1)

        Connecter(adresse, "Faux!MotDePasse1")

        ' Comportement actuel : le compteur n'est pas remis à zéro à l'échéance du
        ' verrou ; le sixième échec reverrouille aussitôt.
        Assert.AreEqual(6, Tentatives(idInternaute))
        Assert.IsTrue(EstVerrouille(idInternaute))
    End Sub

    <TestMethod()> Public Sub UneAncienneEmpreinteEstMigreeALaConnexion()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Dim ancienne = Internaute.CryptePwd(adresse, MotDePasseParDefaut)
        Executer("UPDATE oasis.oa_internaute SET password = @p0 WHERE id = @p1", ancienne, idInternaute)

        VerifierRedirectionPortail(Connecter(adresse, MotDePasseParDefaut), "Dashboard", "Index")

        Dim apres = Empreinte(idInternaute)
        Assert.IsTrue(Oasis_Common.MotDePasse.EstFormatPbkdf2(apres), apres)
        Assert.IsTrue(Oasis_Common.MotDePasse.Verifier(MotDePasseParDefaut, apres))
    End Sub

    <TestMethod()> Public Sub UnInternauteSansPatientSeConnecteQuandMeme()
        ExigerChiffrementFormsPortail()
        Dim adresse = AdresseDeTest()
        CreerInternaute(adresse)

        ' Comportement actuel : l'absence de permission n'est vérifiée qu'ensuite, par
        ' chaque écran (403), pas à la connexion.
        VerifierRedirectionPortail(Connecter(adresse, MotDePasseParDefaut), "Dashboard", "Index")
    End Sub

    ' --- Mot de passe oublié ----------------------------------------------------------

    <TestMethod()> Public Sub UneAdresseInconnueRedirigeSansRienEcrire()
        InterdireCourriel()
        Dim idAutre = CreerComptePortail(CreerPatient(), AdresseDeTest())

        Dim resultat = ControleurPortailAnonyme(Of AuthController)("POST").Forgot(New UserForgot With {.Email = AdresseDeTest()})

        VerifierRedirectionPortail(resultat, "Auth", "Login")
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_internaute WHERE recovery_expiration IS NOT NULL")))
        Assert.AreEqual("cle-recuperation-test", ChaineLue(Colonne("recovery", idAutre)))
    End Sub

    <TestMethod()> Public Sub UneDemandeEnregistreUneCleDUneHeureSansToucherAuMotDePasse()
        InterdireCourriel()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Dim empreinteAvant = Empreinte(idInternaute)

        Dim resultat = ControleurPortailAnonyme(Of AuthController)("POST").Forgot(New UserForgot With {.Email = adresse})

        Dim cle = ChaineLue(Colonne("recovery", idInternaute))
        Assert.IsTrue(Regex.IsMatch(cle, "^[A-F0-9]{64}$"), "clé au format que Recover accepte : " & cle)
        Dim secondes = CInt(Scalaire("SELECT DATEDIFF(second, SYSDATETIME(), recovery_expiration) FROM oasis.oa_internaute WHERE id = @p0",
                                     idInternaute))
        Assert.IsTrue(secondes > 59 * 60 AndAlso secondes <= 60 * 60, "expire dans une heure, reste " & secondes & " s")
        Assert.AreEqual(DBNull.Value, Colonne("code", idInternaute))
        Assert.AreEqual(empreinteAvant, Empreinte(idInternaute), "le compte reste utilisable")

        ' Comportement actuel : sans modèle de courriel (aucun dans la base de test),
        ' la clé reste enregistrée et l'internaute voit un message d'erreur générique
        ' au lieu de la redirection.
        Assert.AreEqual(MessageErreur, MessageDe(resultat))
    End Sub

    <TestMethod()> Public Sub UneNouvelleDemandeRemplaceLaPrecedente()
        InterdireCourriel()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)

        ControleurPortailAnonyme(Of AuthController)("POST").Forgot(New UserForgot With {.Email = adresse})
        Dim premiere = ChaineLue(Colonne("recovery", idInternaute))
        ControleurPortailAnonyme(Of AuthController)("POST").Forgot(New UserForgot With {.Email = adresse})
        Dim seconde = ChaineLue(Colonne("recovery", idInternaute))

        Assert.AreNotEqual(premiere, seconde)
        Assert.AreEqual("Internaute introuvable.", CStr(OuvrirLien(premiere).ViewData("Message")))
    End Sub

    ' --- Lien de récupération (GET) ---------------------------------------------------

    <TestMethod()> Public Sub UnLienValideOuvreLeFormulaire()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))

        Dim vue = OuvrirLien(cle)

        Assert.IsNull(vue.ViewData("Message"))
        Assert.AreEqual(cle, CStr(vue.ViewData("Recovery")))
        Dim affiche = DirectCast(vue.ViewData("Internaute"), Internaute)
        Assert.AreEqual(CInt(idInternaute), affiche.Id)
        Assert.AreEqual(adresse, affiche.Email.Trim())
    End Sub

    <TestMethod()> Public Sub UneCleMalformeeEstRejeteeAvantLaBase()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest())
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))

        For Each malformee In New String() {Nothing, "", "abc", cle.ToLowerInvariant(), cle & "0", cle.Substring(1), " " & cle}
            Dim vue = OuvrirLien(malformee)
            Assert.AreEqual(MessageCleMalformee, CStr(vue.ViewData("Message")), "clé : " & If(malformee, "Nothing"))
            Assert.IsNull(vue.ViewData("Internaute"))
        Next
    End Sub

    <TestMethod()> Public Sub UneCleInconnueEstSignalee()
        Dim vue = OuvrirLien(NouvelleCle())

        Assert.AreEqual("Internaute introuvable.", CStr(vue.ViewData("Message")))
        Assert.IsNull(vue.ViewData("Internaute"))
    End Sub

    <TestMethod()> Public Sub UnLienExpireEstRefuse()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest())
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddMinutes(-1))

        Dim vue = OuvrirLien(cle)

        Assert.AreEqual(MessageLienExpire, CStr(vue.ViewData("Message")))
        Assert.IsNull(vue.ViewData("Internaute"))
    End Sub

    <TestMethod()> Public Sub UneCleSansDateDExpirationResteValable()
        ' Le client lourd crée le compte portail avec une clé et sans date
        ' d'expiration (RadFPatientDetailEdit.BtnCreateInternaute_Click).
        Dim cle = NouvelleCle()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest(), cle)

        Dim vue = OuvrirLien(cle)

        ' Comportement actuel : RecoveryExpiration à Nothing rend la comparaison
        ' « < Now » indéterminée, donc fausse ; le lien de création n'expire jamais.
        Assert.IsNull(vue.ViewData("Message"))
        Assert.AreEqual(CInt(idInternaute), DirectCast(vue.ViewData("Internaute"), Internaute).Id)
    End Sub

    ' --- Réinitialisation (POST) ------------------------------------------------------

    <TestMethod()> Public Sub UneCleValideChangeLeMotDePasseEtSeConsume()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))

        Dim resultat = ReinitialiserMotDePasse(cle, NouveauMotDePasse)

        VerifierRedirectionPortail(resultat, "Auth", "Login")
        Dim apres = Empreinte(idInternaute)
        Assert.IsTrue(Oasis_Common.MotDePasse.EstFormatPbkdf2(apres))
        Assert.IsTrue(Oasis_Common.MotDePasse.Verifier(NouveauMotDePasse, apres))
        Assert.IsFalse(Oasis_Common.MotDePasse.Verifier(MotDePasseParDefaut, apres))
        Assert.AreEqual(DBNull.Value, Colonne("recovery", idInternaute))
        Assert.AreEqual(DBNull.Value, Colonne("recovery_expiration", idInternaute))
        Assert.AreEqual(DBNull.Value, Colonne("code", idInternaute))
    End Sub

    <TestMethod()> Public Sub UneCleNeSertQuUneFois()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest())
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))
        ReinitialiserMotDePasse(cle, NouveauMotDePasse)
        Dim empreinteApresPremier = Empreinte(idInternaute)

        Dim resultat = ReinitialiserMotDePasse(cle, "Autre!Mdp2028")

        Assert.AreEqual(MessageLienExpire, MessageDe(resultat))
        Assert.AreEqual(empreinteApresPremier, Empreinte(idInternaute))
    End Sub

    <TestMethod()> Public Sub UneMauvaiseCleNeTouchePasAuCompte()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest())
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))
        Dim empreinteAvant = Empreinte(idInternaute)

        Dim resultat = ReinitialiserMotDePasse(NouvelleCle(), NouveauMotDePasse)

        Assert.AreEqual(MessageLienExpire, MessageDe(resultat))
        Assert.AreEqual(empreinteAvant, Empreinte(idInternaute))
        Assert.AreEqual(cle, ChaineLue(Colonne("recovery", idInternaute)), "la vraie clé reste utilisable")
    End Sub

    <TestMethod()> Public Sub UneCleExpireeEstRefuseeEtConservee()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest())
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddMinutes(-1))
        Dim empreinteAvant = Empreinte(idInternaute)

        Assert.AreEqual(MessageLienExpire, MessageDe(ReinitialiserMotDePasse(cle, NouveauMotDePasse)))
        Assert.AreEqual(empreinteAvant, Empreinte(idInternaute))
        Assert.AreEqual(cle, ChaineLue(Colonne("recovery", idInternaute)))
    End Sub

    <TestMethod()> Public Sub UneCleMalformeeEstRefuseeAvantLaBase()
        For Each malformee In New String() {Nothing, "", "abc", NouvelleCle().ToLowerInvariant()}
            Assert.AreEqual(MessageLienMalforme, MessageDe(ReinitialiserMotDePasse(malformee, NouveauMotDePasse)),
                            "clé : " & If(malformee, "Nothing"))
        Next
    End Sub

    <TestMethod()> Public Sub DeuxSaisiesDifferentesSontRefuseesSansConsommerLaCle()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest())
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))

        Assert.AreEqual("Les deux mots de passe ne correspondent pas.",
                        MessageDe(ReinitialiserMotDePasse(cle, NouveauMotDePasse, "Autre!Mdp2028")))
        Assert.AreEqual(cle, ChaineLue(Colonne("recovery", idInternaute)))
    End Sub

    <TestMethod()> Public Sub UnModeleInvalideEstRefuseCommeDeuxSaisiesDifferentes()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest())
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))
        Dim controleur = ControleurPortailAnonyme(Of AuthController)("POST")
        controleur.ModelState.AddModelError("Password", "Password required")

        Dim resultat = controleur.Recover(New UserRecover With {.Recovery = cle, .Password = NouveauMotDePasse, .PasswordBis = NouveauMotDePasse})

        Assert.AreEqual("Les deux mots de passe ne correspondent pas.", MessageDe(resultat))
    End Sub

    <TestMethod()> Public Sub UnMotDePasseFaibleEstRefuseSansConsommerLaCle()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest())
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))

        For Each faible In {"Ab!1xyz", "sansmajuscule!1", "SANSMINUSCULE!1", "SansChiffre!!", "SansSpecial12"}
            Assert.AreEqual("Le mot de passe est trop faible : " & messageFormatPassword,
                            MessageDe(ReinitialiserMotDePasse(cle, faible)), faible)
        Next
        Assert.AreEqual(cle, ChaineLue(Colonne("recovery", idInternaute)))
    End Sub

    <TestMethod()> Public Sub LeLienDeCreationDuCompteSansExpirationFixeLeMotDePasse()
        Dim cle = NouvelleCle()
        Dim idInternaute = CreerComptePortail(CreerPatient(), AdresseDeTest(), cle)

        ' Comportement actuel : accepté, faute de date d'expiration (voir
        ' UneCleSansDateDExpirationResteValable).
        VerifierRedirectionPortail(ReinitialiserMotDePasse(cle, NouveauMotDePasse), "Auth", "Login")
        Assert.IsTrue(Oasis_Common.MotDePasse.Verifier(NouveauMotDePasse, Empreinte(idInternaute)))
    End Sub

    <TestMethod()> Public Sub LaReinitialisationNeLevePasLeVerrou()
        Dim adresse = AdresseDeTest()
        Dim idInternaute = CreerComptePortail(CreerPatient(), adresse)
        Dim cle = NouvelleCle()
        PoserCle(idInternaute, cle, Date.Now.AddHours(1))
        Verrouiller(idInternaute, 5, 10)

        VerifierRedirectionPortail(ReinitialiserMotDePasse(cle, NouveauMotDePasse), "Auth", "Login")

        ' Comportement actuel : InternauteDao.Update ne touche ni au compteur ni au
        ' verrou ; le nouveau mot de passe reste refusé jusqu'à l'échéance.
        Assert.AreEqual(5, Tentatives(idInternaute))
        Assert.IsTrue(EstVerrouille(idInternaute))
        Assert.AreEqual(MessageVerrou, MessageDe(Connecter(adresse, NouveauMotDePasse)))
    End Sub

    ' --- Déconnexion ------------------------------------------------------------------

    <TestMethod()> Public Sub LaDeconnexionEffaceLeTicketLaSessionEtLesAnciensCookies()
        Dim idInternaute = CreerComptePortail(CreerPatient())
        Dim controleur = ControleurPortail(Of AuthController)(idInternaute, "POST")

        Dim resultat = controleur.Logout()

        VerifierRedirectionPortail(resultat, "Auth", "Login")
        Dim contexte = ContextePortail(controleur)
        Assert.IsTrue(contexte.FausseSession.Abandonnee)
        For Each nom In {"patientId", "internauteId"}
            Dim ancien = contexte.FausseReponse.Cookies(nom)
            Assert.IsNotNull(ancien, nom)
            Assert.AreEqual("", ancien.Value, nom)
            Assert.IsTrue(ancien.Expires < Date.Now, nom & " expiré")
        Next
        Dim ticket = CookieFormsPortail()
        Assert.IsNotNull(ticket, "SignOut repose un cookie Forms vide")
        Assert.IsTrue(ticket.Expires < Date.Now)
    End Sub

End Class
