Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' InternauteDao contre la base : compte du portail patient.
'''
''' Le client lourd (RadFPatientDetailEdit) appelle Create, Update et
''' ExisteInternautePourEmail : ces tests tournent sous Compte.Client. Tout le
''' reste (connexion, verrouillage, clé de récupération) tourne dans Oasis_Web
''' (AuthController) et donc sous Compte.Web. Les méthodes qui font un SELECT *
''' sur oa_internaute lisent password et recovery : la base doit les refuser au
''' client, ce que des tests vérifient ici au niveau du DAO (les refus de colonne
''' en SQL brut sont dans RestrictionsColonnes).
''' </summary>
<TestClass()> Public Class InternauteDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New InternauteDao

    ' Constantes privées d'InternauteDao.ControlPassword.
    Private Const SeuilVerrouPortail As Integer = 5
    Private Const DureeVerrouPortail As Integer = 15

    Private Const CleRecuperation As String = "A1B2C3D4E5F60718293A4B5C6D7E8F90A1B2C3D4E5F60718293A4B5C6D7E8F90"

    Private Shared Sub VerifierRefusSql(erreur As SqlException)
        Assert.IsTrue(erreur.Number = 229 OrElse erreur.Number = 230,
                      "refus de permission attendu, erreur " & erreur.Number & " : " & erreur.Message)
    End Sub

    Private Shared Function NouvelleAdresse() As String
        Return NouveauLogin() & "@exemple.fr"
    End Function

    ''' <summary>Vérifie que la clé du compte expire dans le délai par défaut (72 heures).</summary>
    Private Shared Sub VerifierExpirationParDefaut(id As Long)
        Dim secondes = CInt(Scalaire("SELECT DATEDIFF(second, SYSDATETIME(), recovery_expiration) FROM oasis.oa_internaute WHERE id = @p0", id))
        Dim attendu = Internaute.DureeLienPosteParDefautHeures * 3600
        Assert.IsTrue(secondes > attendu - 60 AndAlso secondes <= attendu, "expire dans 72 heures, reste " & secondes & " s")
    End Sub

    ' ---------------------------------------------------------------------
    ' Create (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub Create_SousClient_EnregistreLeCompteAvecUneEmpreintePbkdf2()
        Dim adresse = NouvelleAdresse()
        Dim fiche As New Internaute With {
            .Username = "DURAND", .Email = adresse, .Password = MotDePasseParDefaut,
            .Recovery = CleRecuperation, .Code = "0000"}

        Dim id = dao.Create(fiche)

        Assert.IsTrue(id > 0)
        Assert.AreEqual("DURAND", CStr(ValeurInternaute("username", id)))
        Assert.AreEqual(adresse, CStr(ValeurInternaute("email", id)))
        Assert.AreEqual(CleRecuperation, CStr(ValeurInternaute("recovery", id)))
        Assert.AreEqual("0000", CStr(ValeurInternaute("code", id)))
        Dim empreinte = CStr(ValeurInternaute("password", id))
        Assert.IsTrue(MotDePasse.EstFormatPbkdf2(empreinte), "le mot de passe ne doit jamais partir en clair")
        Assert.IsTrue(MotDePasse.Verifier(MotDePasseParDefaut, empreinte))
        Assert.AreEqual(empreinte, fiche.Password, "Create remplace le mot de passe du bean par son empreinte")
        Assert.AreEqual(0, CInt(ValeurInternaute("tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("verrou_jusqua", id))
        VerifierExpirationParDefaut(id)
        Assert.IsTrue(fiche.RecoveryExpiration.HasValue, "Create reporte l'expiration retenue sur le bean")
    End Sub

    <TestMethod()> Public Sub Create_SousClient_CodeEtRecoveryAbsents_DonnentNull()
        Dim id = dao.Create(New Internaute With {
            .Username = "DURAND", .Email = NouvelleAdresse(), .Password = MotDePasseParDefaut})

        Assert.AreEqual(DBNull.Value, ValeurInternaute("recovery", id))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("code", id))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("recovery_expiration", id), "sans clé, pas d'expiration")
    End Sub

    <TestMethod()> Public Sub Create_SousClient_SansMotDePasse_EnregistreLeCompteSansMotDePasseAvecUnLienQuiExpire()
        ' Comme le bouton « Créer compte internaute » de RadFPatientDetailEdit.
        Dim adresse = NouvelleAdresse()
        Dim fiche As New Internaute With {
            .Email = adresse, .Recovery = CleRecuperation, .Code = "0000", .Username = "DURAND"}

        Dim id = dao.Create(fiche)

        Assert.IsTrue(id > 0)
        Assert.AreEqual(adresse, CStr(ValeurInternaute("email", id)))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("password", id), "le compte ne s'ouvre que par le lien")
        Assert.IsNull(fiche.Password)
        Assert.AreEqual(CleRecuperation, CStr(ValeurInternaute("recovery", id)))
        Assert.AreEqual("0000", CStr(ValeurInternaute("code", id)))
        VerifierExpirationParDefaut(id)
    End Sub

    <TestMethod()> Public Sub Create_SousClient_MotDePasseVide_DonneNull()
        Dim id = dao.Create(New Internaute With {
            .Username = "DURAND", .Email = NouvelleAdresse(), .Password = "", .Recovery = CleRecuperation})

        Assert.AreEqual(DBNull.Value, ValeurInternaute("password", id))
    End Sub

    <TestMethod()> Public Sub Create_SousClient_ExpirationFournie_EstConservee()
        Dim expiration = New Date(2031, 5, 6, 7, 8, 9)

        Dim id = dao.Create(New Internaute With {
            .Username = "DURAND", .Email = NouvelleAdresse(), .Recovery = CleRecuperation,
            .RecoveryExpiration = expiration})

        Assert.AreEqual(expiration, CDate(ValeurInternaute("recovery_expiration", id)))
    End Sub

    ' ---------------------------------------------------------------------
    ' Update (client : réinitialisation depuis la fiche patient ; serveur :
    ' fin de récupération)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub Update_SousClient_CommeLaFichePatient_EffaceLeMotDePasseEtPoseUneCleQuiExpire()
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        PoserRecuperationInternaute(id, Nothing, Nothing)

        ' Comme BtnInitInternaute_Click : pas de mot de passe, nouvelle clé, code 0000.
        Dim retour = dao.Update(New Internaute With {
            .Id = CInt(id), .Recovery = CleRecuperation, .Code = "0000"})

        Assert.AreEqual(id, retour)
        Assert.AreEqual(DBNull.Value, ValeurInternaute("password", id), "l'ancien mot de passe ne sert plus")
        Assert.AreEqual(CleRecuperation, CStr(ValeurInternaute("recovery", id)))
        Assert.AreEqual("0000", CStr(ValeurInternaute("code", id)))
        VerifierExpirationParDefaut(id)
        ' Colonnes que l'UPDATE ne touche pas.
        Assert.AreEqual(adresse, CStr(ValeurInternaute("username", id)))
        Assert.AreEqual(adresse, CStr(ValeurInternaute("email", id)))
    End Sub

    <TestMethod()> Public Sub Update_SousWeb_FinDeRecuperation_EnregistreLEmpreinteEtEffaceLaCle()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        dao.UpdateRecovery(CInt(id), CleRecuperation, Date.Now.AddHours(1), "1234")

        ' Comme AuthController.Recover (POST).
        Dim fiche = dao.GetInternauteByRecoveryKey(CleRecuperation)
        fiche.Password = "Nouveau!2026"
        fiche.CryptePwd()
        fiche.Recovery = Nothing
        fiche.Code = Nothing
        fiche.RecoveryExpiration = Nothing
        dao.Update(fiche)

        Assert.IsTrue(MotDePasse.Verifier("Nouveau!2026", CStr(ValeurInternaute("password", id))))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("recovery", id))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("code", id))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("recovery_expiration", id))
        Assert.AreEqual(adresse, CStr(ValeurInternaute("email", id)))
        Assert.IsNull(dao.GetInternauteByRecoveryKey(CleRecuperation), "la clé est à usage unique")
    End Sub

    <TestMethod()> Public Sub Update_SousWeb_ExpirationRenseignee_EstEnregistree()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        Dim expiration = New Date(2031, 5, 6, 7, 8, 9)

        dao.Update(New Internaute With {
            .Id = CInt(id), .Password = MotDePasse.Hacher(MotDePasseParDefaut), .Recovery = CleRecuperation,
            .RecoveryExpiration = expiration})

        Assert.AreEqual(expiration, CDate(ValeurInternaute("recovery_expiration", id)))
    End Sub

    <TestMethod()> Public Sub Update_CompteInexistant_NeModifieRienEtRenvoieLIdDuBean()
        ' Comportement actuel : Update renvoie l'id du bean sans regarder le nombre de
        ' lignes touchées ; la fiche patient croit alors la réinitialisation faite.
        Dim id = CreerInternaute()
        Dim empreinte = CStr(ValeurInternaute("password", id))

        Dim retour = dao.Update(New Internaute With {.Id = 987654321, .Password = "", .Recovery = CleRecuperation})

        Assert.AreEqual(987654321L, retour)
        Assert.AreEqual(empreinte, CStr(ValeurInternaute("password", id)))
    End Sub

    ' ---------------------------------------------------------------------
    ' ExisteInternautePourEmail (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ExisteInternautePourEmail_SousClient_CompteExistant_RenvoieVrai()
        Dim adresse = NouvelleAdresse()
        CreerInternaute(email:=adresse)

        Assert.IsTrue(dao.ExisteInternautePourEmail(adresse))
    End Sub

    <TestMethod()> Public Sub ExisteInternautePourEmail_SousClient_AdresseInconnue_RenvoieFaux()
        CreerInternaute()
        Assert.IsFalse(dao.ExisteInternautePourEmail(NouvelleAdresse()))
    End Sub

    <TestMethod()> Public Sub ExisteInternautePourEmail_AdresseVideOuAbsente_RenvoieFaux()
        Assert.IsFalse(dao.ExisteInternautePourEmail(Nothing))
        Assert.IsFalse(dao.ExisteInternautePourEmail(""))
        Assert.IsFalse(dao.ExisteInternautePourEmail("   "))
    End Sub

    ' ---------------------------------------------------------------------
    ' Lecture du compte complet : réservée au serveur
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetInternauteByEmail_SousWeb_RelitLeCompteSecretsCompris()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        Dim expiration = New Date(2030, 1, 2, 3, 4, 5)
        PoserRecuperationInternaute(id, CleRecuperation, expiration)
        PoserVerrouInternaute(id, 2, Nothing)

        Dim lu = dao.GetInternauteByEmail(adresse)

        Assert.AreEqual(CInt(id), lu.Id)
        Assert.AreEqual(adresse, lu.Email)
        Assert.AreEqual(adresse, lu.Username)
        Assert.IsTrue(MotDePasse.Verifier(MotDePasseParDefaut, lu.Password))
        Assert.AreEqual(CleRecuperation, lu.Recovery)
        Assert.IsNull(lu.Code)
        Assert.AreEqual(expiration, lu.RecoveryExpiration.Value)
        Assert.AreEqual(2, lu.Tentatives)
        Assert.IsFalse(lu.VerrouJusqua.HasValue)
    End Sub

    <TestMethod()> Public Sub GetInternauteByEmail_SousWeb_AdresseInconnue_RenvoieNothing()
        UtiliserCompte(Compte.Web)
        CreerInternaute()
        Assert.IsNull(dao.GetInternauteByEmail(NouvelleAdresse()))
    End Sub

    <TestMethod()> Public Sub GetInternauteByEmail_SousClient_EstRefuseParLaBase()
        ' SELECT * lit password et recovery : la base doit refuser la requête entière.
        Dim adresse = NouvelleAdresse()
        CreerInternaute(email:=adresse)
        VerifierRefusSql(Assert.ThrowsException(Of SqlException)(Sub() dao.GetInternauteByEmail(adresse)))
    End Sub

    <TestMethod()> Public Sub GetInternauteById_SousWeb_RelitLeCompte()
        ' Aucun appelant aujourd'hui ; un SELECT * ne peut servir qu'au serveur.
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)

        Dim lu = dao.GetInternauteById(id)

        Assert.AreEqual(CInt(id), lu.Id)
        Assert.AreEqual(adresse, lu.Email)
        Assert.IsNull(dao.GetInternauteById(987654321))
    End Sub

    <TestMethod()> Public Sub GetInternauteById_SousClient_EstRefuseParLaBase()
        Dim id = CreerInternaute()
        VerifierRefusSql(Assert.ThrowsException(Of SqlException)(Sub() dao.GetInternauteById(id)))
    End Sub

    <TestMethod()> Public Sub GetInternauteByNIR_SousWeb_NirInconnu()
        ' Seul appelant : du code commenté d'AuthController (inscription). Le bean ne
        ' connaît pas de colonne nir ; si la table n'en a pas non plus, la requête
        ' échoue sur un nom de colonne invalide (erreur 207).
        UtiliserCompte(Compte.Web)
        CreerInternaute()
        Dim colonneNir = Scalaire("SELECT COL_LENGTH('oasis.oa_internaute', 'nir')")
        If colonneNir Is Nothing OrElse colonneNir Is DBNull.Value Then
            Dim erreur = Assert.ThrowsException(Of SqlException)(Sub() dao.GetInternauteByNIR("2700197600123"))
            Assert.AreEqual(207, erreur.Number)
        Else
            Assert.IsNull(dao.GetInternauteByNIR("2700197600123"))
        End If
    End Sub

    ' ---------------------------------------------------------------------
    ' Clé de récupération (serveur)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub UpdateRecovery_SousWeb_PoseCleExpirationEtCodeSansToucherAuMotDePasse()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        Dim empreinte = CStr(ValeurInternaute("password", id))
        Dim expiration = New Date(2030, 6, 7, 8, 9, 10)

        Dim lignes = dao.UpdateRecovery(CInt(id), CleRecuperation, expiration, "4321")

        Assert.AreEqual(1L, lignes)
        Assert.AreEqual(CleRecuperation, CStr(ValeurInternaute("recovery", id)))
        Assert.AreEqual(expiration, CDate(ValeurInternaute("recovery_expiration", id)))
        Assert.AreEqual("4321", CStr(ValeurInternaute("code", id)))
        Assert.AreEqual(empreinte, CStr(ValeurInternaute("password", id)), "une demande de récupération ne bloque pas le compte")
    End Sub

    <TestMethod()> Public Sub UpdateRecovery_SousWeb_CodeAbsent_DonneNull()
        ' Comme AuthController.Forgot, qui passe Nothing pour le code.
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        Executer("UPDATE oasis.oa_internaute SET code = '0000' WHERE id = @p0", id)

        dao.UpdateRecovery(CInt(id), CleRecuperation, Date.Now.AddHours(1), Nothing)

        Assert.AreEqual(DBNull.Value, ValeurInternaute("code", id))
    End Sub

    <TestMethod()> Public Sub UpdateRecovery_CompteInexistant_RenvoieZero()
        UtiliserCompte(Compte.Web)
        Assert.AreEqual(0L, dao.UpdateRecovery(987654321, CleRecuperation, Date.Now.AddHours(1), Nothing))
    End Sub

    <TestMethod()> Public Sub GetInternauteByRecoveryKey_SousWeb_CleValide_RelitLeCompteEtSonExpiration()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        Dim expiration = Date.Now.AddHours(1)
        dao.UpdateRecovery(CInt(id), CleRecuperation, expiration, Nothing)

        Dim lu = dao.GetInternauteByRecoveryKey(CleRecuperation)

        Assert.AreEqual(CInt(id), lu.Id)
        Assert.AreEqual(adresse, lu.Email)
        Assert.AreEqual(CleRecuperation, lu.Recovery)
        Assert.IsTrue(lu.RecoveryExpiration.HasValue)
        Assert.IsTrue(Math.Abs((lu.RecoveryExpiration.Value - expiration).TotalSeconds) < 1)
    End Sub

    <TestMethod()> Public Sub GetInternauteByRecoveryKey_SousWeb_CleExpiree_EstQuandMemeRenvoyee()
        ' Le DAO ne filtre pas l'expiration : c'est AuthController.Recover qui compare
        ' RecoveryExpiration à l'heure courante. La date revient donc intacte.
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        Dim expiree = Date.Now.AddMinutes(-5)
        PoserRecuperationInternaute(id, CleRecuperation, expiree)

        Dim lu = dao.GetInternauteByRecoveryKey(CleRecuperation)

        Assert.AreEqual(CInt(id), lu.Id)
        Assert.IsTrue(lu.RecoveryExpiration.Value < Date.Now, "la date d'expiration passée doit revenir telle quelle")
    End Sub

    <TestMethod()> Public Sub GetInternauteByRecoveryKey_SousWeb_CleSansExpiration_RevientSansDateEtNestPasValide()
        ' Clé sans date, comme en posait le poste avant correction : le DAO la
        ' renvoie telle quelle, c'est CleRecuperationValide qui la refuse.
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute(recovery:=CleRecuperation)
        PoserRecuperationInternaute(id, CleRecuperation, Nothing)

        Dim lu = dao.GetInternauteByRecoveryKey(CleRecuperation)

        Assert.AreEqual(CInt(id), lu.Id)
        Assert.IsFalse(lu.RecoveryExpiration.HasValue)
        Assert.IsFalse(lu.CleRecuperationValide(Date.Now))
    End Sub

    <TestMethod()> Public Sub GetInternauteByRecoveryKey_SousWeb_ClePoseeParLePoste_EstValide()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute(recovery:=CleRecuperation)

        Dim lu = dao.GetInternauteByRecoveryKey(CleRecuperation)

        Assert.AreEqual(CInt(id), lu.Id)
        Assert.IsTrue(lu.CleRecuperationValide(Date.Now))
        Assert.IsFalse(lu.CleRecuperationValide(Date.Now.AddHours(Internaute.DureeLienPosteParDefautHeures + 1)))
    End Sub

    <TestMethod()> Public Sub GetInternauteByRecoveryKey_SousWeb_CleInconnueVideOuAbsente_RenvoieNothing()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute(recovery:=CleRecuperation)
        ' Un compte dont la clé est une chaîne vide ne doit pas répondre à une clé vide.
        Dim autre = CreerInternaute(recovery:=Nothing)
        Executer("UPDATE oasis.oa_internaute SET recovery = '' WHERE id = @p0", autre)

        Assert.IsNull(dao.GetInternauteByRecoveryKey("FFFF" & CleRecuperation.Substring(4)))
        Assert.IsNull(dao.GetInternauteByRecoveryKey(""))
        Assert.IsNull(dao.GetInternauteByRecoveryKey(Nothing))
    End Sub

    <TestMethod()> Public Sub GetInternauteByRecoveryKey_SousClient_EstRefuseParLaBase()
        CreerInternaute(recovery:=CleRecuperation)
        VerifierRefusSql(Assert.ThrowsException(Of SqlException)(Sub() dao.GetInternauteByRecoveryKey(CleRecuperation)))
    End Sub

    ' ---------------------------------------------------------------------
    ' Compteur d'échecs (serveur)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub EnregistrerEchec_SousLeSeuil_IncrementeSansVerrouiller()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()

        dao.EnregistrerEchec(CInt(id), 3, 15)
        dao.EnregistrerEchec(CInt(id), 3, 15)

        Assert.AreEqual(2, CInt(ValeurInternaute("tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("verrou_jusqua", id))
    End Sub

    <TestMethod()> Public Sub EnregistrerEchec_AuSeuil_VerrouillePourLaDureeDemandee()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        PoserVerrouInternaute(id, 2, Nothing)
        Dim avant = Date.Now

        dao.EnregistrerEchec(CInt(id), 3, 20)

        Assert.AreEqual(3, CInt(ValeurInternaute("tentatives", id)))
        Dim verrou = CDate(ValeurInternaute("verrou_jusqua", id))
        Assert.IsTrue(verrou >= avant.AddMinutes(19) AndAlso verrou <= Date.Now.AddMinutes(21),
                      "verrou attendu vers maintenant + 20 min, lu " & verrou.ToString("o"))
    End Sub

    <TestMethod()> Public Sub EnregistrerEchec_AuDelaDuSeuil_RepousseLeVerrou()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        PoserVerrouInternaute(id, 7, Date.Now.AddMinutes(1))

        dao.EnregistrerEchec(CInt(id), 5, 15)

        Assert.AreEqual(8, CInt(ValeurInternaute("tentatives", id)))
        Assert.IsTrue(CDate(ValeurInternaute("verrou_jusqua", id)) > Date.Now.AddMinutes(14))
    End Sub

    <TestMethod()> Public Sub EnregistrerEchec_SousLeSeuil_GardeUnVerrouExistant()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        Dim verrouPose = New Date(2030, 1, 1, 12, 0, 0)
        PoserVerrouInternaute(id, 0, verrouPose)

        dao.EnregistrerEchec(CInt(id), 5, 15)

        Assert.AreEqual(1, CInt(ValeurInternaute("tentatives", id)))
        Assert.AreEqual(verrouPose, CDate(ValeurInternaute("verrou_jusqua", id)))
    End Sub

    <TestMethod()> Public Sub EnregistrerEchec_NeToucheQueLeCompteVise()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        Dim autre = CreerInternaute()

        dao.EnregistrerEchec(CInt(id), 5, 15)

        Assert.AreEqual(0, CInt(ValeurInternaute("tentatives", autre)))
    End Sub

    <TestMethod()> Public Sub ReinitialiserEchecs_RemetCompteurEtVerrouAZero()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute()
        PoserVerrouInternaute(id, 6, Date.Now.AddMinutes(10))

        dao.ReinitialiserEchecs(CInt(id))

        Assert.AreEqual(0, CInt(ValeurInternaute("tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("verrou_jusqua", id))
    End Sub

    <TestMethod()> Public Sub UpdateEmpreinteMotDePasse_RemplaceSeulementLEmpreinte()
        UtiliserCompte(Compte.Web)
        Dim id = CreerInternaute(recovery:=CleRecuperation)
        Dim empreinte = MotDePasse.Hacher("Autre!2026")

        dao.UpdateEmpreinteMotDePasse(CInt(id), empreinte)

        Assert.AreEqual(empreinte, CStr(ValeurInternaute("password", id)))
        Assert.AreEqual(CleRecuperation, CStr(ValeurInternaute("recovery", id)))
    End Sub

    ' ---------------------------------------------------------------------
    ' GetInternauteByLoginPassword (serveur)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousWeb_BonMotDePasse_RenvoieLeCompteSansMotDePasse()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        PoserVerrouInternaute(id, 3, Nothing)

        Dim connecte = dao.GetInternauteByLoginPassword(adresse, MotDePasseParDefaut)

        Assert.AreEqual(CInt(id), connecte.Id)
        Assert.AreEqual(adresse, connecte.Email)
        Assert.IsNull(connecte.Password, "l'empreinte ne reste pas sur le bean")
        Assert.AreEqual(0, CInt(ValeurInternaute("tentatives", id)), "le succès remet le compteur à zéro")
    End Sub

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousWeb_MauvaisMotDePasse_EchoueEtCompteLEchec()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetInternauteByLoginPassword(adresse, "Mauvais!2026"))

        StringAssert.Contains(erreur.Message, "erroné")
        Assert.AreEqual(1, CInt(ValeurInternaute("tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("verrou_jusqua", id))
    End Sub

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousWeb_CinqEchecs_VerrouillentMemeLeBonMotDePasse()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)

        For i = 1 To SeuilVerrouPortail - 1
            Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetInternauteByLoginPassword(adresse, "Mauvais!2026"))
        Next
        Assert.AreEqual(DBNull.Value, ValeurInternaute("verrou_jusqua", id), "pas de verrou avant le seuil")

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetInternauteByLoginPassword(adresse, "Mauvais!2026"))
        Assert.AreEqual(SeuilVerrouPortail, CInt(ValeurInternaute("tentatives", id)))
        Dim verrou = CDate(ValeurInternaute("verrou_jusqua", id))
        Assert.IsTrue(verrou > Date.Now.AddMinutes(DureeVerrouPortail - 1) AndAlso verrou <= Date.Now.AddMinutes(DureeVerrouPortail + 1))

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetInternauteByLoginPassword(adresse, MotDePasseParDefaut))
        StringAssert.Contains(erreur.Message, "verrouill")
        ' Un essai pendant le verrou n'est pas compté.
        Assert.AreEqual(SeuilVerrouPortail, CInt(ValeurInternaute("tentatives", id)))
    End Sub

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousWeb_VerrouEchu_AutoriseEtRemetAZero()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        PoserVerrouInternaute(id, SeuilVerrouPortail, Date.Now.AddMinutes(-1))

        Dim connecte = dao.GetInternauteByLoginPassword(adresse, MotDePasseParDefaut)

        Assert.AreEqual(CInt(id), connecte.Id)
        Assert.AreEqual(0, CInt(ValeurInternaute("tentatives", id)))
        Assert.AreEqual(DBNull.Value, ValeurInternaute("verrou_jusqua", id))
    End Sub

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousWeb_AdresseInconnue_Echoue()
        UtiliserCompte(Compte.Web)
        CreerInternaute()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(
            Sub() dao.GetInternauteByLoginPassword(NouvelleAdresse(), MotDePasseParDefaut))
        StringAssert.Contains(erreur.Message, "erroné")
    End Sub

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousWeb_AncienneEmpreinte_EstAccepteePuisMigree()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        PoserEmpreinteInternaute(id, Internaute.CryptePwd(adresse, MotDePasseParDefaut))

        dao.GetInternauteByLoginPassword(adresse, MotDePasseParDefaut)

        Dim empreinte = CStr(ValeurInternaute("password", id))
        Assert.IsTrue(MotDePasse.EstFormatPbkdf2(empreinte), "l'empreinte SHA-1 doit être remplacée à la connexion")
        Assert.IsTrue(MotDePasse.Verifier(MotDePasseParDefaut, empreinte))
    End Sub

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousWeb_MotDePasseVideApresReinitialisation_EstRefuse()
        ' Après « réinitialiser » depuis la fiche patient, password vaut '' : aucun
        ' mot de passe ne passe tant que le patient n'a pas suivi son lien.
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        PoserEmpreinteInternaute(id, "")

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetInternauteByLoginPassword(adresse, ""))
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetInternauteByLoginPassword(adresse, MotDePasseParDefaut))
        Assert.AreEqual(2, CInt(ValeurInternaute("tentatives", id)))
    End Sub

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousWeb_MotDePasseNull_EstRefuse()
        UtiliserCompte(Compte.Web)
        Dim adresse = NouvelleAdresse()
        Dim id = CreerInternaute(email:=adresse)
        PoserEmpreinteInternaute(id, Nothing)

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetInternauteByLoginPassword(adresse, MotDePasseParDefaut))
        Assert.AreEqual(1, CInt(ValeurInternaute("tentatives", id)))
    End Sub

    <TestMethod()> Public Sub GetInternauteByLoginPassword_SousClient_EstRefuseParLaBase()
        Dim adresse = NouvelleAdresse()
        CreerInternaute(email:=adresse)
        VerifierRefusSql(Assert.ThrowsException(Of SqlException)(
            Sub() dao.GetInternauteByLoginPassword(adresse, MotDePasseParDefaut)))
    End Sub

End Class
