Imports Oasis_Common

''' <summary>
''' EpisodeObservationDao contre la base de test. Seule la fiche épisode du client
''' lourd l'appelle : tout tourne sous oasis_client.
''' </summary>
<TestClass()> Public Class EpisodeObservationDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New EpisodeObservationDao

    Private Const ObservationAbsente As Integer = 987654321

    Private Shared Function IdsObservations(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("episode_observation_id"))).ToArray()
    End Function

    <TestMethod()> Public Sub UneObservationCreeeEstRelueAvecSesValeurs()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)

        Dim idObservation = CreerObservationEpisode(idEpisode, idPatient, idUtilisateur, "Patient fatigue",
                                                    natureObservation:="SPECIFIQUE", typeObservation:="PARAMEDICAL")

        Dim relue = dao.GetEpisodeObservationById(CInt(idObservation))
        Assert.AreEqual(idObservation, relue.Id)
        Assert.AreEqual(idEpisode, relue.EpisodeId)
        Assert.AreEqual(idPatient, relue.PatientId)
        Assert.AreEqual(idUtilisateur, relue.UserCreation)
        Assert.AreEqual("PARAMEDICAL", relue.TypeObservation)
        Assert.AreEqual("SPECIFIQUE", relue.NatureObservation)
        Assert.AreEqual("PRESENTIEL", relue.NaturePresence)
        Assert.AreEqual("Patient fatigue", relue.Observation)
        Assert.AreEqual(Date.Today, relue.DateCreation.Date)
        Assert.AreEqual(Date.MinValue, relue.DateModification, "jamais modifiée : NULL")
        Assert.IsFalse(relue.Inactif)
    End Sub

    <TestMethod()> Public Sub UneObservationInexistanteLeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetEpisodeObservationById(ObservationAbsente))
        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub LaCreationRenvoieVrai()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)

        Assert.IsTrue(dao.CreateEpisodeObservation(New EpisodeObservation With {
            .EpisodeId = idEpisode, .PatientId = idPatient, .UserCreation = idUtilisateur,
            .TypeObservation = "MEDICAL", .NatureObservation = "LIBRE", .NaturePresence = "DISTANT",
            .Observation = "Teleconsultation"
        }))
        Assert.AreEqual("DISTANT", CStr(Scalaire("SELECT nature_presence FROM oasis.oa_episode_observation WHERE episode_id = @p0", idEpisode)))
    End Sub

    <TestMethod()> Public Sub LesObservationsLibresActivesDeLEpisodeVontDeLaPlusRecenteALaPlusAncienne()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim idAutreEpisode = CreerEpisode(idAutrePatient, idUtilisateur)
        Dim premiere = CreerObservationEpisode(idEpisode, idPatient, idUtilisateur, "Premiere")
        CreerObservationEpisode(idEpisode, idPatient, idUtilisateur, "Specifique", natureObservation:="SPECIFIQUE")
        CreerObservationEpisode(idEpisode, idPatient, idUtilisateur, "Annulee", inactif:=True)
        Dim seconde = CreerObservationEpisode(idEpisode, idPatient, idUtilisateur, "Seconde", typeObservation:="PARAMEDICAL")
        CreerObservationEpisode(idAutreEpisode, idAutrePatient, idUtilisateur, "Ailleurs")

        Dim table = dao.GetEpisodeObservationLibreByEpisode(CInt(idEpisode))

        CollectionAssert.AreEqual(New Long() {seconde, premiere}, IdsObservations(table))
        Assert.AreEqual("Seconde", CStr(table.Rows(0)("observation")))
    End Sub

    <TestMethod()> Public Sub UnEpisodeSansObservationLibreDonneUneTableVide()
        Dim idEpisode = CreerEpisode(CreerPatient(), CreerUtilisateur(avecCle:=False))
        Assert.AreEqual(0, dao.GetEpisodeObservationLibreByEpisode(CInt(idEpisode)).Rows.Count)
    End Sub

    <TestMethod()> Public Sub LaModificationEnregistreChaqueChampEtHorodate()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idAutreUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim idObservation = CreerObservationEpisode(idEpisode, idPatient, idUtilisateur, "Avant")
        Dim lue = dao.GetEpisodeObservationById(CInt(idObservation))
        Dim creation As New Date(2026, 2, 3, 14, 0, 0)
        lue.Observation = "Apres"
        lue.TypeObservation = "PARAMEDICAL"
        lue.NatureObservation = "SPECIFIQUE"
        lue.NaturePresence = "DISTANT"
        lue.UserCreation = idAutreUtilisateur
        lue.DateCreation = creation
        lue.Inactif = True

        Assert.IsTrue(dao.ModificationEpisodeObservation(lue))

        Dim relue = dao.GetEpisodeObservationById(CInt(idObservation))
        Assert.AreEqual("Apres", relue.Observation)
        Assert.AreEqual("PARAMEDICAL", relue.TypeObservation)
        Assert.AreEqual("SPECIFIQUE", relue.NatureObservation)
        Assert.AreEqual("DISTANT", relue.NaturePresence)
        Assert.AreEqual(idAutreUtilisateur, relue.UserCreation)
        Assert.AreEqual(creation, relue.DateCreation)
        Assert.AreEqual(Date.Today, relue.DateModification.Date)
        Assert.IsTrue(relue.Inactif)
        Assert.AreEqual(0, dao.GetEpisodeObservationLibreByEpisode(CInt(idEpisode)).Rows.Count)
    End Sub

    ' --- Compare : sans base, mais sur des observations relues ---------------------------

    <TestMethod()> Public Sub UneObservationRelueEstEgaleASaCopie()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim lue = dao.GetEpisodeObservationById(CInt(CreerObservationEpisode(idEpisode, idPatient, idUtilisateur, "Texte")))
        Dim copie = lue.Clone()

        Assert.IsTrue(dao.Compare(lue, copie))

        ' Comportement actuel : les dates ne comptent qu'au jour près, et inactif est ignoré.
        copie.DateCreation = lue.DateCreation.Date.AddHours(23)
        copie.Inactif = Not lue.Inactif
        Assert.IsTrue(dao.Compare(lue, copie))

        copie.Observation = "Autre texte"
        Assert.IsFalse(dao.Compare(lue, copie))
    End Sub

    <TestMethod()> Public Sub ChaqueChampComparePeutRendreDeuxObservationsDifferentes()
        Dim reference As New EpisodeObservation With {
            .Id = 1, .EpisodeId = 2, .PatientId = 3, .UserCreation = 4, .TypeObservation = "MEDICAL",
            .NatureObservation = "LIBRE", .NaturePresence = "PRESENTIEL", .Observation = "Texte",
            .DateCreation = New Date(2026, 1, 1), .DateModification = New Date(2026, 1, 2)
        }
        Dim variantes As New List(Of System.Action(Of EpisodeObservation)) From {
            Sub(o) o.Id = 9, Sub(o) o.EpisodeId = 9, Sub(o) o.PatientId = 9, Sub(o) o.UserCreation = 9,
            Sub(o) o.TypeObservation = "PARAMEDICAL", Sub(o) o.NatureObservation = "SPECIFIQUE",
            Sub(o) o.NaturePresence = "DISTANT", Sub(o) o.Observation = "Autre",
            Sub(o) o.DateCreation = New Date(2026, 1, 5), Sub(o) o.DateModification = New Date(2026, 1, 6)
        }
        Assert.IsTrue(dao.Compare(reference, reference.Clone()))
        For Each variante In variantes
            Dim modifiee = reference.Clone()
            variante(modifiee)
            Assert.IsFalse(dao.Compare(reference, modifiee))
        Next
    End Sub

End Class
