Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par SousEpisodeReponseDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestSousEpisodeReponseDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "episode_id", "id_sous_episode", "create_user_id", "horodate_creation",
        "nom_fichier", "commentaire", "validate_state", "validate_user_id", "validate_date",
        "sous_episode_libelle", "sous_episode_sous_libelle", "conclusion", "type_activite"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = SousEpisodeReponseDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"episode_id", 102L},
            {"id_sous_episode", 103L},
            {"create_user_id", 104L},
            {"horodate_creation", New Date(2024, 6, 6)},
            {"nom_fichier", "valeur_6"},
            {"commentaire", "valeur_7"},
            {"validate_state", "valeur_8"},
            {"validate_user_id", 109L},
            {"validate_date", New Date(2024, 11, 11)},
            {"sous_episode_libelle", "valeur_11"},
            {"sous_episode_sous_libelle", "valeur_12"},
            {"conclusion", "valeur_13"},
            {"type_activite", "valeur_14"}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.EpisodeId)
        Assert.AreEqual(103L, b.IdSousEpisode)
        Assert.AreEqual(104L, b.CreateUserId)
        Assert.AreEqual(New Date(2024, 6, 6), b.HorodateCreation)
        Assert.AreEqual("valeur_6", b.NomFichier)
        Assert.AreEqual("valeur_7", b.Commentaire)
        Assert.AreEqual("valeur_8", b.ValidateState)
        Assert.AreEqual(109L, b.ValidateUserId)
        Assert.AreEqual(New Date(2024, 11, 11), b.ValidateDate)
        Assert.AreEqual("valeur_11", b.SousEpisodeLibelle)
        Assert.AreEqual("valeur_12", b.SousEpisodeSousLibelle)
        Assert.AreEqual("valeur_13", b.Conclusion)
        Assert.AreEqual("valeur_14", b.TypeActivite)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = SousEpisodeReponseDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L}}))

        Assert.AreEqual(0L, b.EpisodeId)
        Assert.AreEqual(0L, b.IdSousEpisode)
        Assert.AreEqual(0L, b.CreateUserId)
        Assert.AreEqual(Date.MinValue, b.HorodateCreation)
        Assert.AreEqual("", b.NomFichier)
        Assert.AreEqual("", b.Commentaire)
        Assert.AreEqual("", b.ValidateState)
        Assert.AreEqual(0L, b.ValidateUserId)
        Assert.AreEqual(Date.MinValue, b.ValidateDate)
        Assert.IsNull(b.SousEpisodeLibelle)
        Assert.IsNull(b.SousEpisodeSousLibelle)
        Assert.IsNull(b.Conclusion)
        Assert.IsNull(b.TypeActivite)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L}}
        valeurs.Remove("id")
        SousEpisodeReponseDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
