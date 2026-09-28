Imports Oasis_Common

''' <summary>
''' EpisodeContexteDao contre la base de test. La conclusion médicale d'épisode du
''' client lourd rattache, liste et détache les contextes : tout tourne sous
''' oasis_client, y compris la suppression, que
''' docs/migrations/2026-09-27-suppression-client-complement.sql accorde à ce compte.
''' </summary>
<TestClass()> Public Class EpisodeContexteDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New EpisodeContexteDao

    Private Const LienAbsent As Integer = 987654321

    Private Shared Function Contextes(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("contexte_id"))).ToArray()
    End Function

    <TestMethod()> Public Sub UnContexteRattacheEstRelueAvecSonAuteur()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim idContexte = CreerContextePourEpisode(idPatient, idUtilisateur, "Diabete de type 2")

        Assert.IsTrue(dao.CreateEpisodeContexte(New EpisodeContexte With {
            .EpisodeId = idEpisode, .PatientId = idPatient, .ContexteId = idContexte
        }, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}))

        Dim idLien = CLng(Scalaire("SELECT episode_contexte_id FROM oasis.oa_episode_contexte WHERE episode_id = @p0", idEpisode))
        Dim relu = dao.GetEpisodeById(CInt(idLien))
        Assert.AreEqual(idLien, relu.EpisodeContexteId)
        Assert.AreEqual(idEpisode, relu.EpisodeId)
        Assert.AreEqual(idPatient, relu.PatientId)
        Assert.AreEqual(idContexte, relu.ContexteId)
        Assert.AreEqual(idUtilisateur, relu.UserCreation)
        Assert.AreEqual(Date.Today, relu.DateCreation.Date)
    End Sub

    <TestMethod()> Public Sub UnLienInexistantLeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetEpisodeById(LienAbsent))
        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub RattacherDeuxFoisLeMemeContexteEstRefuse()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim idContexte = CreerContextePourEpisode(idPatient, idUtilisateur, "HTA")
        LierContexteEpisode(idEpisode, idPatient, idContexte, idUtilisateur)

        Dim erreur = Assert.ThrowsException(Of Exception)(
            Sub() LierContexteEpisode(idEpisode, idPatient, idContexte, idUtilisateur))

        StringAssert.Contains(erreur.Message, "Collision")
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_contexte WHERE episode_id = @p0", idEpisode)))
    End Sub

    <TestMethod()> Public Sub LeMemeContexteSurDeuxEpisodesEstAccepte()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim premier = CreerEpisode(idPatient, idUtilisateur)
        CloturerEpisode(premier, idUtilisateur)
        Dim second = CreerEpisode(idPatient, idUtilisateur)
        Dim idContexte = CreerContextePourEpisode(idPatient, idUtilisateur, "HTA")

        Dim lien1 = LierContexteEpisode(premier, idPatient, idContexte, idUtilisateur)
        Dim lien2 = LierContexteEpisode(second, idPatient, idContexte, idUtilisateur)

        Assert.AreNotEqual(lien1, lien2)
    End Sub

    <TestMethod()> Public Sub LaListeDonneLesContextesDeLEpisodeParDateDeLiaison()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim idAutreEpisode = CreerEpisode(idAutrePatient, idUtilisateur)
        Dim diabete = CreerContextePourEpisode(idPatient, idUtilisateur, "Diabete")
        Dim hta = CreerContextePourEpisode(idPatient, idUtilisateur, "HTA")
        Dim asthme = CreerContextePourEpisode(idAutrePatient, idUtilisateur, "Asthme")
        DaterContexteEpisode(LierContexteEpisode(idEpisode, idPatient, diabete, idUtilisateur), New Date(2026, 3, 1))
        DaterContexteEpisode(LierContexteEpisode(idEpisode, idPatient, hta, idUtilisateur), New Date(2026, 1, 1))
        LierContexteEpisode(idAutreEpisode, idAutrePatient, asthme, idUtilisateur)

        Dim table = dao.GetAllEpisodeContexteByEpisodeId(idEpisode)

        CollectionAssert.AreEqual(New Long() {hta, diabete}, Contextes(table))
        Assert.AreEqual("HTA", CStr(table.Rows(0)("oa_antecedent_description")))
        Assert.AreEqual("Diabete", CStr(table.Rows(1)("oa_antecedent_description")))
        Assert.AreEqual(idUtilisateur, CLng(table.Rows(0)("user_creation")))
    End Sub

    <TestMethod()> Public Sub UnEpisodeSansContexteDonneUneTableVide()
        Dim idEpisode = CreerEpisode(CreerPatient(), CreerUtilisateur(avecCle:=False))
        Assert.AreEqual(0, dao.GetAllEpisodeContexteByEpisodeId(idEpisode).Rows.Count)
    End Sub

    <TestMethod()> Public Sub LaSuppressionDetacheUnSeulContexte()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim diabete = CreerContextePourEpisode(idPatient, idUtilisateur, "Diabete")
        Dim hta = CreerContextePourEpisode(idPatient, idUtilisateur, "HTA")
        Dim lienDiabete = LierContexteEpisode(idEpisode, idPatient, diabete, idUtilisateur)
        LierContexteEpisode(idEpisode, idPatient, hta, idUtilisateur)

        Assert.IsTrue(dao.SuppressionEpisodeContexteById(lienDiabete))

        CollectionAssert.AreEqual(New Long() {hta}, Contextes(dao.GetAllEpisodeContexteByEpisodeId(idEpisode)))
        ' Le contexte lui-même reste dans le dossier du patient.
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_antecedent WHERE oa_antecedent_id = @p0", diabete)))
    End Sub

    <TestMethod()> Public Sub SupprimerUnLienInexistantNeLevePasDErreur()
        Assert.IsTrue(dao.SuppressionEpisodeContexteById(LienAbsent))
    End Sub

End Class
