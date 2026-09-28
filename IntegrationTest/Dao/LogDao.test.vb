Imports Oasis_Common

''' <summary>
''' LogDao contre la base, et les deux points d'entrée qui journalisent.
'''
''' outils.CreateLog (Oasis_Common/Module/outils.vb) est appelé par une vingtaine
''' d'écrans du client lourd : LogDao.CreateLog tourne donc sous oasis_client.
''' GetLogById n'a aucun appelant ; il est relu sous le même compte.
'''
''' JournalAcces (Oasis_Common/Module/JournalAcces.vb) n'écrit pas dans oa_log
''' mais dans oa_action, par ActionDao, et seulement depuis Oasis_Web : ses tests
''' tournent sous oasis_web.
''' </summary>
<TestClass()> Public Class LogDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New LogDao

    Private Shared Function DernierLog(description As String) As Long
        Dim id = Scalaire("SELECT MAX(id) FROM oasis.oa_log WHERE description = @p0", description)
        Assert.IsFalse(id Is Nothing OrElse id Is DBNull.Value, "aucune trace « " & description & " » dans oa_log")
        Return CLng(id)
    End Function

    Private Shared Function ValeurLog(colonne As String, id As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_log WHERE id = @p0", id)
    End Function

    Private Shared Function NombreDeTraces(utilisateurId As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_action WHERE utilisateur_id = @p0", utilisateurId))
    End Function

    Private Shared Function DerniereTrace(colonne As String, utilisateurId As Long) As Object
        Return Scalaire("SELECT TOP 1 " & colonne & " FROM oasis.oa_action WHERE utilisateur_id = @p0 ORDER BY horodatage DESC",
                        utilisateurId)
    End Function

    ' ---------------------------------------------------------------------
    ' LogDao.CreateLog et GetLogById (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreateLog_SousClient_EnregistreLaTraceALInstantPresent()
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim description = "Trace de test " & Guid.NewGuid().ToString("N")
        dao.CreateLog(New Oasis_Common.Log With {
            .Description = description, .TypeLog = "ERREUR", .Origine = "RadFTest",
            .UserLog = New Utilisateur With {.UtilisateurId = CInt(utilisateurId)},
            .DateLog = New Date(2001, 1, 1)})

        Dim id = DernierLog(description)
        Assert.AreEqual("ERREUR", CStr(ValeurLog("type_log", id)))
        Assert.AreEqual("RadFTest", CStr(ValeurLog("origine", id)))
        Assert.AreEqual(utilisateurId, CLng(ValeurLog("user_creation", id)))
        ' La date est celle de l'écriture, pas DateLog du bean.
        Dim horodatage = CDate(ValeurLog("date_creation", id))
        Assert.AreEqual(Date.Today, horodatage.Date)
        Assert.IsTrue(horodatage <= Date.Now.AddSeconds(5))
    End Sub

    <TestMethod()> Public Sub CreateLog_SansUtilisateur_LeveNullReferenceException()
        ' Comportement actuel : UserLog.UtilisateurId est lu sans contrôle, hors du Try.
        Dim description = "Trace sans utilisateur " & Guid.NewGuid().ToString("N")
        Assert.ThrowsException(Of NullReferenceException)(
            Sub() dao.CreateLog(New Oasis_Common.Log With {.Description = description, .TypeLog = "INFO", .Origine = "X"}))
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_log WHERE description = @p0", description)))
    End Sub

    <TestMethod()> Public Sub GetLogById_SousClient_RelitLaTrace()
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim description = "Trace relue " & Guid.NewGuid().ToString("N")
        dao.CreateLog(New Oasis_Common.Log With {
            .Description = description, .TypeLog = "INFO", .Origine = "Synthese",
            .UserLog = New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}})
        Dim id = DernierLog(description)

        Dim lu = dao.GetLogById(CInt(id))

        Assert.AreEqual(id, lu.Id)
        Assert.AreEqual(description, lu.Description)
        Assert.AreEqual("INFO", lu.TypeLog)
        Assert.AreEqual("Synthese", lu.Origine)
        Assert.AreEqual(CInt(utilisateurId), lu.UserLog.UtilisateurId)
        Assert.AreEqual(CDate(ValeurLog("date_creation", id)), lu.DateLog)
    End Sub

    <TestMethod()> Public Sub GetLogById_ColonnesNulles_DonnentDesValeursParDefaut()
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim description = "Trace videe " & Guid.NewGuid().ToString("N")
        dao.CreateLog(New Oasis_Common.Log With {
            .Description = description, .TypeLog = "INFO", .Origine = "Synthese",
            .UserLog = New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}})
        Dim id = DernierLog(description)
        Executer("UPDATE oasis.oa_log SET description = NULL, origine = NULL, type_log = NULL, user_creation = NULL WHERE id = @p0", id)

        Dim lu = dao.GetLogById(CInt(id))

        Assert.AreEqual("", lu.Description)
        Assert.AreEqual("", lu.Origine)
        Assert.AreEqual("", lu.TypeLog)
        Assert.AreEqual(0, lu.UserLog.UtilisateurId)
    End Sub

    <TestMethod()> Public Sub GetLogById_Inexistant_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetLogById(987654321))
        StringAssert.Contains(erreur.Message, "Log inexistante")
    End Sub

    ' ---------------------------------------------------------------------
    ' outils.CreateLog (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub OutilsCreateLog_SousClient_EcritDansOaLog()
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim description = "Paramètre application 'Test' non trouvé ! " & Guid.NewGuid().ToString("N")

        outils.CreateLog(description, "RadFEpisodeDetail", Oasis_Common.Log.EnumTypeLog.ERREUR.ToString(),
                         New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})

        Dim id = DernierLog(description)
        Assert.AreEqual("ERREUR", CStr(ValeurLog("type_log", id)))
        Assert.AreEqual("RadFEpisodeDetail", CStr(ValeurLog("origine", id)))
        Assert.AreEqual(utilisateurId, CLng(ValeurLog("user_creation", id)))
    End Sub

    <TestMethod()> Public Sub OutilsCreateLog_TypeInconnu_DevientInfo()
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim description = "Trace de type inconnu " & Guid.NewGuid().ToString("N")

        outils.CreateLog(description, "Ecran", "AVERTISSEMENT", New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})

        Assert.AreEqual("INFO", CStr(ValeurLog("type_log", DernierLog(description))))
    End Sub

    <TestMethod()> Public Sub OutilsCreateLog_DeuxAppels_DonnentDeuxTraces()
        ' outils garde une seule instance de Log d'un appel à l'autre : chaque appel
        ' doit tout de même écrire sa propre ligne.
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim premiere = "Premiere " & Guid.NewGuid().ToString("N")
        Dim seconde = "Seconde " & Guid.NewGuid().ToString("N")

        outils.CreateLog(premiere, "A", "INFO", New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
        outils.CreateLog(seconde, "B", "ERREUR", New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})

        Assert.AreEqual("A", CStr(ValeurLog("origine", DernierLog(premiere))))
        Assert.AreEqual("B", CStr(ValeurLog("origine", DernierLog(seconde))))
        Assert.AreEqual("ERREUR", CStr(ValeurLog("type_log", DernierLog(seconde))))
    End Sub

    ' ---------------------------------------------------------------------
    ' JournalAcces (serveur, oa_action)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub JournalAcces_SousWeb_ChaqueNatureEcritSonPrefixe()
        UtiliserCompte(Compte.Web)
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim fonctionId = CreerFonction("IT journal")
        Dim patientId = CreerPatient()
        Dim appelant As New Utilisateur With {
            .UtilisateurId = CInt(utilisateurId), .UtilisateurProfilId = ProfilDeTest, .FonctionParDefautId = fonctionId}

        JournalAcces.Consultation(appelant, patientId, "document 12")
        JournalAcces.Modification(appelant, patientId, "document 13")
        JournalAcces.Sortie(appelant, patientId, "courriel vers a@exemple.fr")
        JournalAcces.AccesRefuse(appelant, patientId, "document 14")

        For Each libelle In {"CONSULTATION : document 12", "MODIFICATION : document 13",
                             "SORTIE : courriel vers a@exemple.fr", "REFUS : document 14"}
            Dim filtre = " FROM oasis.oa_action WHERE utilisateur_id = @p0 AND action = @p1"
            Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*)" & filtre, utilisateurId, libelle)), libelle)
            Assert.AreEqual(patientId, CLng(Scalaire("SELECT patient_id" & filtre, utilisateurId, libelle)))
            Assert.AreEqual(ProfilDeTest, CStr(Scalaire("SELECT fonction" & filtre, utilisateurId, libelle)))
            Assert.AreEqual(fonctionId, CLng(Scalaire("SELECT fonction_id" & filtre, utilisateurId, libelle)))
        Next
        Assert.AreEqual(4, NombreDeTraces(utilisateurId))
    End Sub

    <TestMethod()> Public Sub JournalAcces_LibelleTropLong_EstTronqueA400Caracteres()
        UtiliserCompte(Compte.Web)
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim appelant As New Utilisateur With {
            .UtilisateurId = CInt(utilisateurId), .UtilisateurProfilId = ProfilDeTest,
            .FonctionParDefautId = CreerFonction("IT journal")}

        JournalAcces.Consultation(appelant, 0, New String("x"c, 600))

        Dim enregistre = CStr(DerniereTrace("action", utilisateurId))
        Assert.AreEqual(400, enregistre.Length)
        Assert.IsTrue(enregistre.StartsWith("CONSULTATION : xxx"))
    End Sub

    <TestMethod()> Public Sub JournalAcces_ProfilAbsent_EcritUneFonctionVide()
        UtiliserCompte(Compte.Web)
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim appelant As New Utilisateur With {
            .UtilisateurId = CInt(utilisateurId), .UtilisateurProfilId = Nothing,
            .FonctionParDefautId = CreerFonction("IT journal")}

        JournalAcces.Modification(appelant, 0, "cle de signature")

        Assert.AreEqual("", CStr(DerniereTrace("fonction", utilisateurId)))
        Assert.AreEqual(0L, CLng(DerniereTrace("patient_id", utilisateurId)))
    End Sub

    <TestMethod()> Public Sub JournalAcces_SansUtilisateur_NEcritRien()
        UtiliserCompte(Compte.Web)
        Dim avant = CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_action"))

        JournalAcces.Consultation(Nothing, 1, "anonyme")

        Assert.AreEqual(avant, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_action")))
    End Sub

    <TestMethod()> Public Sub JournalAcces_EcritureRefusee_NeFaitPasEchouerLAppelant()
        ' Le journal absorbe ses erreurs : un refus d'écriture ne remonte pas. Le
        ' refus est posé pour ce seul test ; la restauration de l'instantané l'efface.
        Executer("DENY INSERT ON oasis.oa_action TO oasis_web")
        UtiliserCompte(Compte.Web)
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim appelant As New Utilisateur With {
            .UtilisateurId = CInt(utilisateurId), .UtilisateurProfilId = ProfilDeTest,
            .FonctionParDefautId = CreerFonction("IT journal")}

        JournalAcces.Sortie(appelant, 0, "courriel")

        Assert.AreEqual(0, NombreDeTraces(utilisateurId))
    End Sub

End Class
