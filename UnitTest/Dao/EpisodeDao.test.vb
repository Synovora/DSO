Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne d'épisode par EpisodeDao.BuildBean. Un épisode porte
''' les conclusions médicales : un mauvais défaut ici change ce que le
''' prochain intervenant lit dans le dossier.
''' </summary>
<TestClass()> Public Class TestEpisodeDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "episode_id", "patient_id", "type", "type_activite", "type_profil",
        "description_activite", "commentaire", "observation_medical", "observation_paramedical",
        "decision", "conclusion_ide_type", "conclusion_med_consigne_drc_id",
        "conclusion_med_consigne_denomination", "conclusion_med_contexte1_drc_id",
        "conclusion_med_contexte1_antecedent_id", "conclusion_med_contexte2_drc_id",
        "conclusion_med_contexte2_antecedent_id", "conclusion_med_contexte3_drc_id",
        "conclusion_med_contexte3_antecedent_id", "user_creation", "date_creation",
        "user_modification", "date_modification", "etat", "inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim episode = EpisodeDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"episode_id", 100L}, {"patient_id", 42L}, {"type", "C"},
            {"type_activite", "Suivi pathologie chronique"}, {"type_profil", "MEDICAL"},
            {"description_activite", "Diabète"}, {"commentaire", "RAS"},
            {"observation_medical", "Glycémie stable"}, {"observation_paramedical", "Pansement refait"},
            {"decision", "Poursuite"}, {"conclusion_ide_type", "Surveillance"},
            {"conclusion_med_consigne_drc_id", 11L}, {"conclusion_med_consigne_denomination", "Consigne"},
            {"conclusion_med_contexte1_drc_id", 21L}, {"conclusion_med_contexte1_antecedent_id", 31L},
            {"conclusion_med_contexte2_drc_id", 22L}, {"conclusion_med_contexte2_antecedent_id", 32L},
            {"conclusion_med_contexte3_drc_id", 23L}, {"conclusion_med_contexte3_antecedent_id", 33L},
            {"user_creation", 5L}, {"date_creation", New Date(2024, 1, 2)},
            {"user_modification", 6L}, {"date_modification", New Date(2024, 3, 4)},
            {"etat", "En cours"}, {"inactif", True}}))

        Assert.AreEqual(100L, episode.Id)
        Assert.AreEqual(42L, episode.PatientId)
        Assert.AreEqual("C", episode.Type)
        Assert.AreEqual("Suivi pathologie chronique", episode.TypeActivite)
        Assert.AreEqual("MEDICAL", episode.TypeProfil)
        Assert.AreEqual("Diabète", episode.DescriptionActivite)
        Assert.AreEqual("RAS", episode.Commentaire)
        Assert.AreEqual("Glycémie stable", episode.ObservationMedical)
        Assert.AreEqual("Pansement refait", episode.ObservationParamedical)
        Assert.AreEqual("Poursuite", episode.Decision)
        Assert.AreEqual("Surveillance", episode.ConclusionIdeType)
        Assert.AreEqual(11L, episode.ConclusionMedConsigneDrcId)
        Assert.AreEqual("Consigne", episode.ConclusionMedConsigneDenomination)
        Assert.AreEqual(21L, episode.ConclusionMedContexte1DrcId)
        Assert.AreEqual(31L, episode.ConclusionMedContexte1AntecedentId)
        Assert.AreEqual(22L, episode.ConclusionMedContexte2DrcId)
        Assert.AreEqual(32L, episode.ConclusionMedContexte2AntecedentId)
        Assert.AreEqual(23L, episode.ConclusionMedContexte3DrcId)
        Assert.AreEqual(33L, episode.ConclusionMedContexte3AntecedentId)
        Assert.AreEqual(5L, episode.UserCreation)
        Assert.AreEqual(New Date(2024, 1, 2), episode.DateCreation)
        Assert.AreEqual(6L, episode.UserModification)
        Assert.AreEqual(New Date(2024, 3, 4), episode.DateModification)
        Assert.AreEqual("En cours", episode.Etat)
        Assert.IsTrue(episode.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim episode = EpisodeDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {{"episode_id", 1L}}))

        Assert.AreEqual(1L, episode.Id)
        For Each texte In {episode.Type, episode.TypeActivite, episode.TypeProfil, episode.DescriptionActivite,
                           episode.Commentaire, episode.ObservationMedical, episode.ObservationParamedical,
                           episode.Decision, episode.ConclusionIdeType, episode.ConclusionMedConsigneDenomination,
                           episode.Etat}
            Assert.AreEqual("", texte)
        Next
        For Each entier In {episode.PatientId, episode.ConclusionMedConsigneDrcId,
                            episode.ConclusionMedContexte1DrcId, episode.ConclusionMedContexte1AntecedentId,
                            episode.ConclusionMedContexte2DrcId, episode.ConclusionMedContexte2AntecedentId,
                            episode.ConclusionMedContexte3DrcId, episode.ConclusionMedContexte3AntecedentId,
                            episode.UserCreation, episode.UserModification}
            Assert.AreEqual(0L, entier)
        Next
        Assert.AreEqual(Date.MinValue, episode.DateCreation)
        Assert.AreEqual(Date.MinValue, episode.DateModification)
        Assert.IsFalse(episode.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        EpisodeDao.BuildBean(LigneDeTest.Ligne(Colonnes, Nothing))
    End Sub

End Class
