Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par EpisodeContexteDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestEpisodeContexteDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "episode_contexte_id", "episode_id", "patient_id", "contexte_id", "user_creation",
        "date_creation"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = EpisodeContexteDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"episode_contexte_id", 101L},
            {"episode_id", 102L},
            {"patient_id", 103L},
            {"contexte_id", 104L},
            {"user_creation", 105L},
            {"date_creation", New Date(2024, 7, 7)}}))

        Assert.AreEqual(101L, b.EpisodeContexteId)
        Assert.AreEqual(102L, b.EpisodeId)
        Assert.AreEqual(103L, b.PatientId)
        Assert.AreEqual(104L, b.ContexteId)
        Assert.AreEqual(105L, b.UserCreation)
        Assert.AreEqual(New Date(2024, 7, 7), b.DateCreation)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = EpisodeContexteDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"episode_contexte_id", 101L}}))

        Assert.AreEqual(0L, b.EpisodeId)
        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual(0L, b.ContexteId)
        Assert.AreEqual(0L, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansEpisodeContexteIdEstUneErreur()
        ' episode_contexte_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"episode_contexte_id", 101L}}
        valeurs.Remove("episode_contexte_id")
        EpisodeContexteDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
