Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' UserDao contre la base, sous le compte qui exécute chaque méthode en production.
'''
''' Le client lourd appelle Create, UpdateSansChangerEtatEtDates, GetUserById,
''' GetTableUtilisateurForGrid, ActivationOuDesactivation et ACleSignature (et
''' addFonctions par GetUserById) : tous tournent ici sous Compte.Client, filet
''' contre le retour d'un SELECT * ou d'une écriture de colonne refusée. Le serveur
''' seul appelle getUserByLoginPassword, UpdateMotDePasse, UpdateEmpreinteMotDePasse
''' et EnregistrerCleSignature : ceux-là tournent sous Compte.Web, et l'on vérifie en
''' plus que le client en est empêché par la base.
''' </summary>
<TestClass()> Public Class TestUserDao
    Inherits TestIntegration

    Private ReadOnly dao As New UserDao

    Private Shared Function ValeurUtilisateur(colonne As String, id As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)
    End Function

    Private Shared Sub VerifierRefusSql(erreur As SqlException)
        Assert.IsTrue(erreur.Number = 229 OrElse erreur.Number = 230,
                      "refus de permission attendu, erreur " & erreur.Number & " : " & erreur.Message)
    End Sub

    Private Shared Function IdsDe(table As DataTable) As List(Of Long)
        Dim ids As New List(Of Long)
        For Each ligne As DataRow In table.Rows
            ids.Add(CLng(ligne("oa_utilisateur_id")))
        Next
        Return ids
    End Function

    ' ---------------------------------------------------------------------
    ' Create (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub Create_SousClient_EnregistreUneFicheActive()
        AssurerProfil(ProfilDeTest)
        Dim empreinte = MotDePasse.Hacher(MotDePasseParDefaut)
        Dim identifiant = NouveauLogin()
        Dim nouveau As New Utilisateur With {
            .UtilisateurLogin = identifiant, .UtilisateurNom = "DURAND", .UtilisateurPrenom = "Anne",
            .UtilisateurProfilId = ProfilDeTest, .UtilisateurAdmin = True,
            .UtilisateurTelephone = "0102030405", .UtilisateurFax = "0102030406",
            .UtilisateurMail = "anne.durand@exemple.fr", .UtilisateurRPPS = "10009876543",
            .Password = empreinte, .IsPasswordUniqueUsage = True}

        Assert.IsTrue(dao.Create(nouveau))

        Dim id As Long = nouveau.UtilisateurId
        Assert.IsTrue(id > 0, "l'id attribué doit revenir sur le bean")
        Assert.AreEqual(identifiant, CStr(ValeurUtilisateur("oa_utilisateur_login", id)))
        Assert.AreEqual("DURAND", CStr(ValeurUtilisateur("oa_utilisateur_nom", id)))
        Assert.AreEqual("Anne", CStr(ValeurUtilisateur("oa_utilisateur_prenom", id)))
        Assert.AreEqual(ProfilDeTest, CStr(ValeurUtilisateur("oa_utilisateur_profil_id", id)))
        Assert.IsTrue(CBool(ValeurUtilisateur("oa_utilisateur_admin", id)))
        Assert.AreEqual("anne.durand@exemple.fr", CStr(ValeurUtilisateur("oa_utilisateur_mail", id)))
        Assert.AreEqual("10009876543", CStr(ValeurUtilisateur("oa_utilisateur_rpps", id)))
        Assert.IsTrue(CBool(ValeurUtilisateur("oa_utilisateur_password_is_unique_usage", id)))
        Assert.AreEqual("A", CStr(ValeurUtilisateur("oa_utilisateur_etat", id)).Trim())
        Assert.AreEqual(2999, CDate(ValeurUtilisateur("oa_utilisateur_date_sortie", id)).Year)
        Assert.AreEqual(Date.Today, CDate(ValeurUtilisateur("oa_utilisateur_date_entree", id)).Date)
        ' Limite connue de la migration : l'INSERT de l'empreinte reste permis au poste.
        Assert.AreEqual(empreinte, EmpreinteUtilisateur(id))
    End Sub

    <TestMethod()> Public Sub Create_SousClient_IdentifiantsDeRattachementNulsDonnentNull()
        AssurerProfil(ProfilDeTest)
        Dim nouveau As New Utilisateur With {
            .UtilisateurLogin = NouveauLogin(), .UtilisateurNom = "DURAND", .UtilisateurPrenom = "Anne",
            .UtilisateurProfilId = ProfilDeTest, .Password = MotDePasse.Hacher(MotDePasseParDefaut)}

        dao.Create(nouveau)

        Assert.AreEqual(DBNull.Value, ValeurUtilisateur("oa_utilisateur_site_id", nouveau.UtilisateurId))
        Assert.AreEqual(DBNull.Value, ValeurUtilisateur("oa_utilisateur_unite_sanitaire_id", nouveau.UtilisateurId))
        Assert.AreEqual(DBNull.Value, ValeurUtilisateur("oa_utilisateur_siege_id", nouveau.UtilisateurId))
    End Sub

    <TestMethod()> Public Sub Create_SousClient_NeRenseigneAucuneCleDeSignature()
        ' La paire est générée ensuite par /api/signature/cle, jamais sur le poste.
        Dim id = CreerUtilisateur(avecCle:=False)
        Assert.IsNull(ClePriveeUtilisateur(id))
        Assert.IsNull(AdresseUtilisateur(id))
    End Sub

    ' ---------------------------------------------------------------------
    ' UpdateSansChangerEtatEtDates (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub UpdateSansChangerEtatEtDates_SousClient_ModifieLaFicheSansToucherAuxSecrets()
        Dim id = CreerUtilisateur()
        Dim empreinteAvant = EmpreinteUtilisateur(id)
        Dim cleAvant = ClePriveeUtilisateur(id)
        Dim adresseAvant = AdresseUtilisateur(id)
        Dim sortieAvant = CDate(ValeurUtilisateur("oa_utilisateur_date_sortie", id))

        ' Comme l'écran : la fiche relue par le poste, sans secrets, puis modifiée.
        Dim fiche = dao.GetUserById(id)
        fiche.UtilisateurNom = "MARTIN"
        fiche.UtilisateurPrenom = "Paul"
        fiche.UtilisateurMail = "paul.martin@exemple.fr"
        fiche.UtilisateurTelephone = "0600000000"
        fiche.UtilisateurRPPS = "10000000001"
        fiche.UtilisateurAdmin = True

        Assert.IsTrue(dao.UpdateSansChangerEtatEtDates(fiche))

        Assert.AreEqual("MARTIN", CStr(ValeurUtilisateur("oa_utilisateur_nom", id)))
        Assert.AreEqual("Paul", CStr(ValeurUtilisateur("oa_utilisateur_prenom", id)))
        Assert.AreEqual("paul.martin@exemple.fr", CStr(ValeurUtilisateur("oa_utilisateur_mail", id)))
        Assert.AreEqual("0600000000", CStr(ValeurUtilisateur("oa_utilisateur_telephone", id)))
        Assert.AreEqual("10000000001", CStr(ValeurUtilisateur("oa_utilisateur_rpps", id)))
        Assert.IsTrue(CBool(ValeurUtilisateur("oa_utilisateur_admin", id)))
        Assert.AreEqual(empreinteAvant, EmpreinteUtilisateur(id))
        Assert.AreEqual(cleAvant, ClePriveeUtilisateur(id))
        Assert.AreEqual(adresseAvant, AdresseUtilisateur(id))
        Assert.AreEqual("A", CStr(ValeurUtilisateur("oa_utilisateur_etat", id)).Trim())
        Assert.AreEqual(sortieAvant, CDate(ValeurUtilisateur("oa_utilisateur_date_sortie", id)))
    End Sub

    <TestMethod()> Public Sub UpdateSansChangerEtatEtDates_UtilisateurInexistant_Echoue()
        AssurerProfil(ProfilDeTest)
        Dim fantome As New Utilisateur With {.UtilisateurId = -1, .UtilisateurLogin = NouveauLogin(), .UtilisateurProfilId = ProfilDeTest}
        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.UpdateSansChangerEtatEtDates(fantome))
        StringAssert.Contains(erreur.Message, "(0)")
    End Sub

    ' ---------------------------------------------------------------------
    ' getUserByLoginPassword (serveur)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_BonMotDePasse_RenvoieLUtilisateurEtSaClePrivee()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)

        Dim connecte = dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut)

        Assert.AreEqual(CInt(id), connecte.UtilisateurId)
        Assert.AreEqual(identifiant, connecte.UtilisateurLogin)
        Assert.AreEqual(ClePriveeUtilisateur(id), connecte.UtilisateurClePrivee, "le serveur signe : il doit recevoir la clé")
        Assert.AreEqual(AdresseUtilisateur(id), connecte.UtilisateurAddress)
        Assert.IsNull(connecte.Password, "l'empreinte ne reste pas sur le bean après vérification")
        Assert.AreEqual("MEDICAL", connecte.TypeProfil)
        Assert.IsNotNull(connecte.LstFonction)
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousClient_EstRefuseParLaBase()
        ' Côté poste, la requête d'authentification lit oa_password et cle_privee :
        ' la base doit la refuser en bloc.
        Dim identifiant = NouveauLogin()
        CreerUtilisateur(login:=identifiant)
        Dim erreur = Assert.ThrowsException(Of SqlException)(Sub() dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut))
        VerifierRefusSql(erreur)
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_MauvaisMotDePasse_EchoueEtCompteLEchec()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(identifiant, "Mauvais!2026"))

        Assert.AreEqual(1, CInt(ValeurUtilisateur("oa_utilisateur_tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurUtilisateur("oa_utilisateur_verrou_jusqua", id))
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_CinqEchecs_VerrouillentLeCompte()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)

        For i = 1 To MAX_TRY - 1
            Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(identifiant, "Mauvais!2026"))
        Next
        Assert.AreEqual(MAX_TRY - 1, CInt(ValeurUtilisateur("oa_utilisateur_tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurUtilisateur("oa_utilisateur_verrou_jusqua", id), "pas de verrou avant le seuil")

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(identifiant, "Mauvais!2026"))
        Assert.AreEqual(MAX_TRY, CInt(ValeurUtilisateur("oa_utilisateur_tentatives", id)))
        Assert.IsTrue(CDate(ValeurUtilisateur("oa_utilisateur_verrou_jusqua", id)) > Date.Now)

        ' Même le bon mot de passe est refusé tant que le verrou court.
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut))
        StringAssert.Contains(erreur.Message, "verrouill")
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_VerrouEchu_AutoriseEtRemetLeCompteurAZero()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)
        PoserVerrou(id, MAX_TRY, Date.Now.AddMinutes(-1))

        Dim connecte = dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut)

        Assert.AreEqual(CInt(id), connecte.UtilisateurId)
        Assert.AreEqual(0, CInt(ValeurUtilisateur("oa_utilisateur_tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurUtilisateur("oa_utilisateur_verrou_jusqua", id))
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_SuccesApresEchecs_RemetLeCompteurAZero()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)
        PoserVerrou(id, 3, Nothing)

        dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut)

        Assert.AreEqual(0, CInt(ValeurUtilisateur("oa_utilisateur_tentatives", id)))
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_IdentifiantInconnu_Echoue()
        UtiliserCompte(Compte.Web)
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(NouveauLogin(), MotDePasseParDefaut))
        StringAssert.Contains(erreur.Message, "erroné")
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_UtilisateurInactif_Echoue()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)
        dao.ActivationOuDesactivation(CInt(id), True)

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut))
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_ProfilInactif_Echoue()
        UtiliserCompte(Compte.Web)
        CreerProfil("IT_INACTIF", inactif:=True)
        Dim identifiant = NouveauLogin()
        CreerUtilisateur(login:=identifiant, profilId:="IT_INACTIF")

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut))
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_AncienneEmpreinte_EstAccepteePuisMigree()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)
        PoserEmpreinte(id, Utilisateur.CryptePwd(identifiant, MotDePasseParDefaut))

        dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut)

        Dim empreinte = EmpreinteUtilisateur(id)
        Assert.IsTrue(MotDePasse.EstFormatPbkdf2(empreinte), "l'empreinte SHA-1 doit être remplacée à la connexion")
        Assert.IsTrue(MotDePasse.Verifier(MotDePasseParDefaut, empreinte))
    End Sub

    <TestMethod()> Public Sub GetUserByLoginPassword_SousWeb_AncienneEmpreinteEtMauvaisMotDePasse_NeMigrePas()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)
        Dim ancienne = Utilisateur.CryptePwd(identifiant, MotDePasseParDefaut)
        PoserEmpreinte(id, ancienne)

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(identifiant, "Mauvais!2026"))

        Assert.AreEqual(ancienne, EmpreinteUtilisateur(id))
    End Sub

    ' ---------------------------------------------------------------------
    ' GetUserById et addFonctions (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetUserById_SousClient_LitLaFicheSansSecrets()
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)

        Dim lu = dao.GetUserById(CInt(id))

        Assert.AreEqual(CInt(id), lu.UtilisateurId)
        Assert.AreEqual(identifiant, lu.UtilisateurLogin)
        Assert.AreEqual("TEST", lu.UtilisateurNom)
        Assert.AreEqual("Utilisateur", lu.UtilisateurPrenom)
        Assert.AreEqual("0102030405", lu.UtilisateurTelephone)
        Assert.AreEqual("0102030406", lu.UtilisateurFax)
        Assert.AreEqual("utilisateur.test@exemple.fr", lu.UtilisateurMail)
        Assert.AreEqual("10001234567", lu.UtilisateurRPPS)
        Assert.AreEqual(ProfilDeTest, lu.UtilisateurProfilId)
        Assert.AreEqual("MEDICAL", lu.TypeProfil)
        Assert.AreEqual(1, lu.UtilisateurNiveauAcces)
        Assert.IsFalse(lu.UtilisateurAdmin)
        Assert.IsFalse(lu.IsPasswordUniqueUsage)
        Assert.AreEqual(0, lu.Tentatives)
        Assert.IsFalse(lu.VerrouJusqua.HasValue)
        Assert.AreEqual(AdresseUtilisateur(id), lu.UtilisateurAddress)
        Assert.IsNull(lu.Password)
        Assert.AreEqual("", lu.UtilisateurClePrivee)
    End Sub

    <TestMethod()> Public Sub GetUserById_SousClient_ChargeLesFonctionsActivesDuProfil()
        AssurerProfil(ProfilDeTest)
        Dim associee = CreerFonction("IT fonction associee")
        Dim inactive = CreerFonction("IT fonction inactive", inactif:=True)
        Dim etrangere = CreerFonction("IT fonction etrangere")
        AssocierFonction(ProfilDeTest, associee)
        AssocierFonction(ProfilDeTest, inactive)
        Dim id = CreerUtilisateur()

        Dim lu = dao.GetUserById(CInt(id))

        Assert.IsTrue(lu.IsFonctionIdPossible(associee))
        Assert.IsFalse(lu.IsFonctionIdPossible(inactive))
        Assert.IsFalse(lu.IsFonctionIdPossible(etrangere))
    End Sub

    <TestMethod()> Public Sub GetUserById_SousClient_ColonnesNulles_DonnentLesValeursParDefaut()
        Dim id = CreerUtilisateur()
        ViderColonnesFacultativesUtilisateur(id)
        PoserVerrou(id, 2, New Date(2030, 1, 1, 12, 0, 0))

        Dim lu = dao.GetUserById(CInt(id))

        Assert.AreEqual("", lu.UtilisateurTelephone)
        Assert.AreEqual("", lu.UtilisateurFax)
        Assert.AreEqual("", lu.UtilisateurMail)
        Assert.AreEqual("", lu.UtilisateurRPPS)
        Assert.AreEqual("", lu.UtilisateurAddress, "une clé absente reste absente, sans valeur de repli")
        Assert.AreEqual(0, lu.UtilisateurSiteId)
        Assert.AreEqual(0, lu.UtilisateurUniteSanitaireId)
        Assert.AreEqual(0, lu.UtilisateurSiegeId)
        Assert.AreEqual(2, lu.Tentatives)
        Assert.AreEqual(New Date(2030, 1, 1, 12, 0, 0), lu.VerrouJusqua.Value)
    End Sub

    <TestMethod()> Public Sub GetUserById_SousClient_ProfilInactif_EstQuandMemeLu()
        ' Contrairement à l'authentification, la lecture d'une fiche ne filtre pas
        ' sur l'état du profil : l'historique d'un dossier doit rester affichable.
        CreerProfil("IT_ANCIEN", typeProfil:="PARAMEDICAL", inactif:=True, niveauAcces:=2)
        Dim id = CreerUtilisateur(profilId:="IT_ANCIEN")

        Dim lu = dao.GetUserById(CInt(id))

        Assert.AreEqual("IT_ANCIEN", lu.UtilisateurProfilId)
        Assert.AreEqual("PARAMEDICAL", lu.TypeProfil)
        Assert.AreEqual(2, lu.UtilisateurNiveauAcces)
    End Sub

    <TestMethod()> Public Sub GetUserById_Inconnu_Echoue()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetUserById(-1))
        StringAssert.Contains(erreur.Message, "non retrouvé")
    End Sub

    <TestMethod()> Public Sub AddFonctions_SousClient_RemplitLaListeDuProfil()
        CreerProfil("IT_SECRET", typeProfil:="GESTION")
        Dim premiere = CreerFonction("IT accueil")
        Dim seconde = CreerFonction("IT secretariat")
        AssocierFonction("IT_SECRET", premiere)
        AssocierFonction("IT_SECRET", seconde)
        Dim porteur As New Utilisateur With {.UtilisateurProfilId = "IT_SECRET"}

        dao.addFonctions(porteur)

        Assert.AreEqual(2, porteur.LstFonction.Count)
        Assert.IsTrue(porteur.IsFonctionIdPossible(premiere))
        Assert.IsTrue(porteur.IsFonctionIdPossible(seconde))
    End Sub

    <TestMethod()> Public Sub AddFonctions_ProfilSansFonction_DonneUneListeVide()
        CreerProfil("IT_VIDE")
        Dim porteur As New Utilisateur With {.UtilisateurProfilId = "IT_VIDE"}

        dao.addFonctions(porteur)

        Assert.IsNotNull(porteur.LstFonction)
        Assert.AreEqual(0, porteur.LstFonction.Count)
    End Sub

    ' ---------------------------------------------------------------------
    ' UpdateMotDePasse et UpdateEmpreinteMotDePasse (serveur)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub UpdateMotDePasse_SousWeb_EnregistreLEmpreinteEtLeveLeVerrou()
        UtiliserCompte(Compte.Web)
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)
        PoserVerrou(id, MAX_TRY, Date.Now.AddMinutes(10))

        dao.UpdateMotDePasse(CInt(id), MotDePasse.Hacher("Nouveau!2026"), True)

        Assert.IsTrue(MotDePasse.Verifier("Nouveau!2026", EmpreinteUtilisateur(id)))
        Assert.IsTrue(CBool(ValeurUtilisateur("oa_utilisateur_password_is_unique_usage", id)))
        Assert.AreEqual(0, CInt(ValeurUtilisateur("oa_utilisateur_tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurUtilisateur("oa_utilisateur_verrou_jusqua", id))

        Dim connecte = dao.getUserByLoginPassword(identifiant, "Nouveau!2026")
        Assert.IsTrue(connecte.IsPasswordUniqueUsage, "le titulaire devra changer ce mot de passe")
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUserByLoginPassword(identifiant, MotDePasseParDefaut))
    End Sub

    <TestMethod()> Public Sub UpdateMotDePasse_SousWeb_PourSoiMeme_NEstPasAUsageUnique()
        UtiliserCompte(Compte.Web)
        Dim id = CreerUtilisateur()

        dao.UpdateMotDePasse(CInt(id), MotDePasse.Hacher("Nouveau!2026"), False)

        Assert.IsFalse(CBool(ValeurUtilisateur("oa_utilisateur_password_is_unique_usage", id)))
    End Sub

    <TestMethod()> Public Sub UpdateMotDePasse_SousClient_EstRefuseParLaBase()
        Dim id = CreerUtilisateur()
        Dim avant = EmpreinteUtilisateur(id)

        Dim erreur = Assert.ThrowsException(Of SqlException)(Sub() dao.UpdateMotDePasse(CInt(id), MotDePasse.Hacher("Choisi!2026"), False))

        VerifierRefusSql(erreur)
        Assert.AreEqual(avant, EmpreinteUtilisateur(id))
    End Sub

    <TestMethod()> Public Sub UpdateMotDePasse_SousWeb_UtilisateurInconnu_Echoue()
        UtiliserCompte(Compte.Web)
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.UpdateMotDePasse(-1, MotDePasse.Hacher("Nouveau!2026"), False))
    End Sub

    <TestMethod()> Public Sub UpdateEmpreinteMotDePasse_SousWeb_RemplaceLEmpreinteSeule()
        UtiliserCompte(Compte.Web)
        Dim id = CreerUtilisateur()
        PoserVerrou(id, 2, Nothing)
        Dim empreinte = MotDePasse.Hacher("Nouveau!2026")

        dao.UpdateEmpreinteMotDePasse(CInt(id), empreinte)

        Assert.AreEqual(empreinte, EmpreinteUtilisateur(id))
        Assert.AreEqual(2, CInt(ValeurUtilisateur("oa_utilisateur_tentatives", id)), "le compteur d'échecs n'est pas touché")
        Assert.IsFalse(CBool(ValeurUtilisateur("oa_utilisateur_password_is_unique_usage", id)))
    End Sub

    <TestMethod()> Public Sub UpdateEmpreinteMotDePasse_SousClient_EstRefuseParLaBase()
        Dim id = CreerUtilisateur()
        Dim avant = EmpreinteUtilisateur(id)

        Dim erreur = Assert.ThrowsException(Of SqlException)(Sub() dao.UpdateEmpreinteMotDePasse(CInt(id), MotDePasse.Hacher("Choisi!2026")))

        VerifierRefusSql(erreur)
        Assert.AreEqual(avant, EmpreinteUtilisateur(id))
    End Sub

    ' ---------------------------------------------------------------------
    ' GetTableUtilisateurForGrid et ActivationOuDesactivation (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetTableUtilisateurForGrid_SousClient_AvecTrue_ListeLesActifs()
        ' Le paramètre s'appelle isInactif mais l'écran lui passe l'état du bouton
        ' « Actifs » : True exclut l'état I, donc liste les comptes actifs.
        Dim actif = CreerUtilisateur()
        Dim inactif = CreerUtilisateur()
        dao.ActivationOuDesactivation(CInt(inactif), True)

        Dim ids = IdsDe(dao.GetTableUtilisateurForGrid(True))

        Assert.IsTrue(ids.Contains(actif))
        Assert.IsFalse(ids.Contains(inactif))
    End Sub

    <TestMethod()> Public Sub GetTableUtilisateurForGrid_SousClient_ParDefaut_ListeLesInactifs()
        Dim actif = CreerUtilisateur()
        Dim inactif = CreerUtilisateur()
        dao.ActivationOuDesactivation(CInt(inactif), True)

        Dim ids = IdsDe(dao.GetTableUtilisateurForGrid())

        Assert.IsTrue(ids.Contains(inactif))
        Assert.IsFalse(ids.Contains(actif))
    End Sub

    <TestMethod()> Public Sub GetTableUtilisateurForGrid_SousClient_PorteLeLibelleDuProfil()
        CreerProfil("IT_GRILLE", designation:="Profil grille")
        Dim id = CreerUtilisateur(profilId:="IT_GRILLE")

        Dim table = dao.GetTableUtilisateurForGrid(True)

        Dim trouvee As DataRow = Nothing
        For Each ligne As DataRow In table.Rows
            If CLng(ligne("oa_utilisateur_id")) = id Then trouvee = ligne
        Next
        Assert.IsNotNull(trouvee)
        Assert.AreEqual("Profil grille", CStr(trouvee("oa_r_profil_designation")))
        Assert.AreEqual("IT_GRILLE", CStr(trouvee("oa_utilisateur_profil_id")))
        Assert.AreEqual("A", CStr(trouvee("oa_utilisateur_etat")).Trim())
        Assert.AreEqual(DBNull.Value, trouvee("oa_site_description"), "aucun site rattaché")
    End Sub

    <TestMethod()> Public Sub ActivationOuDesactivation_SousClient_DesactiveLeCompte()
        Dim id = CreerUtilisateur()

        dao.ActivationOuDesactivation(CInt(id), True)

        Assert.AreEqual("I", CStr(ValeurUtilisateur("oa_utilisateur_etat", id)).Trim())
        Assert.AreEqual(Date.Today, CDate(ValeurUtilisateur("oa_utilisateur_date_sortie", id)).Date)
    End Sub

    <TestMethod()> Public Sub ActivationOuDesactivation_SousClient_ReactiveLeCompte()
        Dim id = CreerUtilisateur()
        dao.ActivationOuDesactivation(CInt(id), True)

        dao.ActivationOuDesactivation(CInt(id), False)

        Assert.AreEqual("A", CStr(ValeurUtilisateur("oa_utilisateur_etat", id)).Trim())
        Assert.AreEqual(New Date(2999, 12, 31), CDate(ValeurUtilisateur("oa_utilisateur_date_sortie", id)).Date)
    End Sub

    <TestMethod()> Public Sub ActivationOuDesactivation_DejaDansLEtatDemande_SignaleUneCollision()
        Dim id = CreerUtilisateur()
        dao.ActivationOuDesactivation(CInt(id), True)

        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.ActivationOuDesactivation(CInt(id), True))
        StringAssert.Contains(erreur.Message, "Collision")
    End Sub

    <TestMethod()> Public Sub ActivationOuDesactivation_UtilisateurInconnu_Echoue()
        Assert.ThrowsException(Of Exception)(Sub() dao.ActivationOuDesactivation(-1, True))
    End Sub

    ' ---------------------------------------------------------------------
    ' ACleSignature (client) et EnregistrerCleSignature (serveur)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ACleSignature_SousClient_AvecCle_RenvoieVrai()
        Dim id = CreerUtilisateur(avecCle:=True)
        Assert.IsTrue(dao.ACleSignature(CInt(id)))
    End Sub

    <TestMethod()> Public Sub ACleSignature_SousClient_SansCle_RenvoieFaux()
        Dim id = CreerUtilisateur(avecCle:=False)
        Assert.IsFalse(dao.ACleSignature(CInt(id)))
    End Sub

    <TestMethod()> Public Sub ACleSignature_SousClient_UtilisateurInconnu_RenvoieFaux()
        Assert.IsFalse(dao.ACleSignature(-1))
    End Sub

    <TestMethod()> Public Sub EnregistrerCleSignature_SousWeb_EcritLaCleEtLAdresse()
        UtiliserCompte(Compte.Web)
        Dim id = CreerUtilisateur(avecCle:=False)

        dao.EnregistrerCleSignature(CInt(id), "0xcle-de-test", "0x00000000000000000000000000000000000000aa")

        Assert.AreEqual("0xcle-de-test", ClePriveeUtilisateur(id))
        Assert.AreEqual("0x00000000000000000000000000000000000000aa", AdresseUtilisateur(id))
        Assert.IsTrue(dao.ACleSignature(CInt(id)))
    End Sub

    <TestMethod()> Public Sub EnregistrerCleSignature_SousClient_EstRefuseParLaBase()
        Dim id = CreerUtilisateur()
        Dim cleAvant = ClePriveeUtilisateur(id)
        Dim adresseAvant = AdresseUtilisateur(id)

        Dim erreur = Assert.ThrowsException(Of SqlException)(
            Sub() dao.EnregistrerCleSignature(CInt(id), "0xcle-choisie", "0x00000000000000000000000000000000000000bb"))

        VerifierRefusSql(erreur)
        Assert.AreEqual(cleAvant, ClePriveeUtilisateur(id))
        Assert.AreEqual(adresseAvant, AdresseUtilisateur(id))
    End Sub

    <TestMethod()> Public Sub EnregistrerCleSignature_SousWeb_UtilisateurInconnu_Echoue()
        UtiliserCompte(Compte.Web)
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.EnregistrerCleSignature(-1, "0xcle", "0xadresse"))
    End Sub

End Class
