Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par EpisodeObservationDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestEpisodeObservationDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "episode_observation_id", "episode_id", "patient_id", "type_observation",
        "nature_observation", "nature_presence", "observation", "user_id", "date_creation",
        "date_modification", "inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = EpisodeObservationDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"episode_observation_id", 101L},
            {"episode_id", 102L},
            {"patient_id", 103L},
            {"type_observation", "valeur_4"},
            {"nature_observation", "valeur_5"},
            {"nature_presence", "valeur_6"},
            {"observation", "valeur_7"},
            {"user_id", 108L},
            {"date_creation", New Date(2024, 10, 10)},
            {"date_modification", New Date(2024, 11, 11)},
            {"inactif", True}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.EpisodeId)
        Assert.AreEqual(103L, b.PatientId)
        Assert.AreEqual("valeur_4", b.TypeObservation)
        Assert.AreEqual("valeur_5", b.NatureObservation)
        Assert.AreEqual("valeur_6", b.NaturePresence)
        Assert.AreEqual("valeur_7", b.Observation)
        Assert.AreEqual(108L, b.UserCreation)
        Assert.AreEqual(New Date(2024, 10, 10), b.DateCreation)
        Assert.AreEqual(New Date(2024, 11, 11), b.DateModification)
        Assert.AreEqual(True, b.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = EpisodeObservationDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"episode_observation_id", 101L}}))

        Assert.AreEqual(0L, b.EpisodeId)
        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual("", b.TypeObservation)
        Assert.AreEqual("", b.NatureObservation)
        Assert.AreEqual("", b.NaturePresence)
        Assert.AreEqual("", b.Observation)
        Assert.AreEqual(0L, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
        Assert.AreEqual(Date.MinValue, b.DateModification)
        Assert.AreEqual(False, b.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' episode_observation_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"episode_observation_id", 101L}}
        valeurs.Remove("episode_observation_id")
        EpisodeObservationDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
