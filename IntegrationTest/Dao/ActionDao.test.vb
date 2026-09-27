Imports Oasis_Common

''' <summary>
''' ActionDao contre la base, sous le compte du client lourd : traces d'action
''' écrites depuis la synthèse et l'épisode, relues dans la liste des actions d'un
''' utilisateur. JournalAcces écrit dans la même table côté serveur.
''' </summary>
<TestClass()> Public Class TestActionDao
    Inherits TestIntegration

    Private ReadOnly dao As New ActionDao

    ' Oasis_Common.Action, et non System.Action : les deux sont visibles ici.
    Private Shared Function NouvelleTrace(utilisateurId As Long, patientId As Long, libelle As String,
                                          fonctionId As Long) As Oasis_Common.Action
        Return New Oasis_Common.Action With {
            .UtilisateurId = utilisateurId,
            .PatientId = patientId,
            .Action = libelle,
            .Fonction = "MEDECIN",
            .FonctionId = fonctionId
        }
    End Function

    ' ---------------------------------------------------------------------
    ' CreationAction
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreationAction_SousClient_EnregistreLaTraceALInstantPresent()
        Dim utilisateurId = CreerUtilisateur()
        Dim patientId = CreerPatient()
        Dim fonctionId = CreerFonction("IT medecin")
        Dim avant = Date.Now.AddSeconds(-5)

        Assert.IsTrue(dao.CreationAction(NouvelleTrace(utilisateurId, patientId, "Consultation de la synthèse", fonctionId)))

        Dim filtre = " FROM oasis.oa_action WHERE utilisateur_id = @p0 AND action = @p1"
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*)" & filtre, utilisateurId, "Consultation de la synthèse")))
        Assert.AreEqual(patientId, CLng(Scalaire("SELECT patient_id" & filtre, utilisateurId, "Consultation de la synthèse")))
        Assert.AreEqual("MEDECIN", CStr(Scalaire("SELECT fonction" & filtre, utilisateurId, "Consultation de la synthèse")))
        Assert.AreEqual(fonctionId, CLng(Scalaire("SELECT fonction_id" & filtre, utilisateurId, "Consultation de la synthèse")))
        Dim horodatage = CDate(Scalaire("SELECT horodatage" & filtre, utilisateurId, "Consultation de la synthèse"))
        Assert.IsTrue(horodatage >= avant AndAlso horodatage <= Date.Now.AddSeconds(5),
                      "l'horodatage est celui de l'écriture, pas celui porté par le bean")
    End Sub

    <TestMethod()> Public Sub CreationAction_SansPatient_EstEnregistree()
        ' JournalAcces trace ainsi, avec le patient 0, les actes sans dossier
        ' (génération d'une clé de signature par exemple).
        Dim utilisateurId = CreerUtilisateur()
        Dim fonctionId = CreerFonction("IT medecin")

        dao.CreationAction(NouvelleTrace(utilisateurId, 0, "MODIFICATION : sans dossier", fonctionId))

        Assert.AreEqual(0L, CLng(Scalaire("SELECT patient_id FROM oasis.oa_action WHERE utilisateur_id = @p0", utilisateurId)))
    End Sub

    ' ---------------------------------------------------------------------
    ' getAllActionByUser
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetAllActionByUser_RenvoieSesTracesDeLaPlusRecenteALaPlusAncienne()
        Dim utilisateurId = CreerUtilisateur()
        Dim autreId = CreerUtilisateur()
        Dim patientId = CreerPatient(nom:="ITNOM", prenom:="Itprenom")
        CreerAction(utilisateurId, patientId, New Date(2026, 3, 1, 9, 0, 0), "ancienne")
        CreerAction(utilisateurId, patientId, New Date(2026, 3, 2, 9, 0, 0), "recente")
        CreerAction(autreId, patientId, New Date(2026, 3, 3, 9, 0, 0), "d'un autre")

        Dim table = dao.getAllActionByUser(utilisateurId)

        Assert.AreEqual(2, table.Rows.Count)
        Assert.AreEqual("recente", CStr(table.Rows(0)("action")))
        Assert.AreEqual("ancienne", CStr(table.Rows(1)("action")))
        Assert.AreEqual(New Date(2026, 3, 2, 9, 0, 0), CDate(table.Rows(0)("horodatage")))
        Assert.AreEqual("ITNOM", CStr(table.Rows(0)("oa_patient_nom")))
        Assert.AreEqual("Itprenom", CStr(table.Rows(0)("oa_patient_prenom")))
    End Sub

    <TestMethod()> Public Sub GetAllActionByUser_SansTrace_DonneUneTableVide()
        Dim utilisateurId = CreerUtilisateur()
        Assert.AreEqual(0, dao.getAllActionByUser(utilisateurId).Rows.Count)
    End Sub

    ' ---------------------------------------------------------------------
    ' getAllActionByUserAndDate
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetAllActionByUserAndDate_NeRenvoieQueLeJourDemande()
        Dim utilisateurId = CreerUtilisateur()
        Dim patientId = CreerPatient()
        CreerAction(utilisateurId, patientId, New Date(2026, 3, 14, 23, 59, 59), "veille")
        CreerAction(utilisateurId, patientId, New Date(2026, 3, 15, 0, 0, 0), "debut du jour")
        CreerAction(utilisateurId, patientId, New Date(2026, 3, 15, 23, 59, 59), "fin du jour")
        CreerAction(utilisateurId, patientId, New Date(2026, 3, 16, 0, 0, 0), "lendemain")

        ' L'heure passée est ignorée : seul le jour compte.
        Dim table = dao.getAllActionByUserAndDate(utilisateurId, New Date(2026, 3, 15, 14, 30, 0))

        Assert.AreEqual(2, table.Rows.Count)
        Assert.AreEqual("fin du jour", CStr(table.Rows(0)("action")))
        Assert.AreEqual("debut du jour", CStr(table.Rows(1)("action")))
    End Sub

    <TestMethod()> Public Sub GetAllActionByUserAndDate_PorteLePatientEtLaFonction()
        Dim utilisateurId = CreerUtilisateur()
        Dim patientId = CreerPatient(nom:="ITNOM", prenom:="Itprenom")
        Dim fonctionId = CreerFonction("IT medecin")
        CreerAction(utilisateurId, patientId, New Date(2026, 3, 15, 10, 0, 0), "acte", "MEDECIN", fonctionId)

        Dim table = dao.getAllActionByUserAndDate(utilisateurId, New Date(2026, 3, 15))

        Assert.AreEqual(1, table.Rows.Count)
        Dim ligne = table.Rows(0)
        Assert.AreEqual(patientId, CLng(ligne("patient_id")))
        Assert.AreEqual("ITNOM", CStr(ligne("oa_patient_nom")))
        Assert.AreEqual("MEDECIN", CStr(ligne("fonction")))
        Assert.AreEqual(fonctionId, CLng(ligne("fonction_id")))
    End Sub

    <TestMethod()> Public Sub GetAllActionByUserAndDate_JourSansTrace_DonneUneTableVide()
        Dim utilisateurId = CreerUtilisateur()
        CreerAction(utilisateurId, CreerPatient(), New Date(2026, 3, 15, 10, 0, 0), "acte")

        Assert.AreEqual(0, dao.getAllActionByUserAndDate(utilisateurId, New Date(2026, 3, 17)).Rows.Count)
    End Sub

    ' ---------------------------------------------------------------------
    ' getTraitementById
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetTraitementById_SousClient_LitLaTrace()
        ' La requête filtre sur oa_action_id quand buildBean lit action_id : l'un
        ' des deux noms ne correspond pas au schéma. Ce test tranchera.
        Dim utilisateurId = CreerUtilisateur()
        Dim patientId = CreerPatient()
        Dim fonctionId = CreerFonction("IT medecin")
        Dim id = CreerAction(utilisateurId, patientId, New Date(2026, 3, 15, 10, 0, 0), "acte", "MEDECIN", fonctionId)

        Dim lue = dao.getTraitementById(id)

        Assert.AreEqual(id, lue.ActionId)
        Assert.AreEqual(utilisateurId, lue.UtilisateurId)
        Assert.AreEqual(patientId, lue.PatientId)
        Assert.AreEqual(New Date(2026, 3, 15, 10, 0, 0), lue.Horodatage)
        Assert.AreEqual("acte", lue.Action)
        Assert.AreEqual("MEDECIN", lue.Fonction)
        Assert.AreEqual(fonctionId, lue.FonctionId)
    End Sub

    <TestMethod()> Public Sub GetTraitementById_Inconnue_Echoue()
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getTraitementById(-1))
    End Sub

End Class
