Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par EpisodeActeParamedicalDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestEpisodeActeParamedicalDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_episode_acte_paramedical_id", "patient_id", "episode_id", "drc_id", "observation",
        "type_observation", "user_id", "date_saisie_observation", "date_modification_observation",
        "inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = EpisodeActeParamedicalDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_episode_acte_paramedical_id", 101L},
            {"patient_id", 102L},
            {"episode_id", 103L},
            {"drc_id", 104L},
            {"observation", "valeur_5"},
            {"type_observation", "valeur_6"},
            {"user_id", 107L},
            {"date_saisie_observation", New Date(2024, 9, 9)},
            {"date_modification_observation", New Date(2024, 10, 10)},
            {"inactif", True}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.PatientId)
        Assert.AreEqual(103L, b.EpisodeId)
        Assert.AreEqual(104L, b.DrcId)
        Assert.AreEqual("valeur_5", b.Observation)
        Assert.AreEqual("valeur_6", b.TypeObservation)
        Assert.AreEqual(107L, b.UserId)
        Assert.AreEqual(New Date(2024, 9, 9), b.DateObservation)
        Assert.AreEqual(New Date(2024, 10, 10), b.DateModification)
        Assert.AreEqual(True, b.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = EpisodeActeParamedicalDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_episode_acte_paramedical_id", 101L}}))

        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual(0L, b.EpisodeId)
        Assert.AreEqual(0L, b.DrcId)
        Assert.AreEqual("", b.Observation)
        Assert.AreEqual("", b.TypeObservation)
        Assert.AreEqual(0L, b.UserId)
        Assert.AreEqual(Date.MinValue, b.DateObservation)
        Assert.AreEqual(Date.MinValue, b.DateModification)
        Assert.AreEqual(False, b.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' oa_episode_acte_paramedical_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_episode_acte_paramedical_id", 101L}}
        valeurs.Remove("oa_episode_acte_paramedical_id")
        EpisodeActeParamedicalDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
