Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New SousEpisodeReponse. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New SousEpisodeReponse(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestSousEpisodeReponseLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "id_sous_episode", "create_user_id", "horodate_creation", "nom_fichier",
        "commentaire", "validate_state", "validate_user_id", "validate_date", "episode_id"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New SousEpisodeReponse(LigneDeTest.Rangee(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode", 102L},
            {"create_user_id", 103L},
            {"horodate_creation", New Date(2024, 5, 5)},
            {"nom_fichier", "valeur_5"},
            {"commentaire", "valeur_6"},
            {"validate_state", "valeur_7"},
            {"validate_user_id", 108L},
            {"validate_date", New Date(2024, 10, 10)},
            {"episode_id", 110L}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.IdSousEpisode)
        Assert.AreEqual(103L, b.CreateUserId)
        Assert.AreEqual(New Date(2024, 5, 5), b.HorodateCreation)
        Assert.AreEqual("valeur_5", b.NomFichier)
        Assert.AreEqual("valeur_6", b.Commentaire)
        Assert.AreEqual("valeur_7", b.ValidateState)
        Assert.AreEqual(108L, b.ValidateUserId)
        Assert.AreEqual(New Date(2024, 10, 10), b.ValidateDate)
        Assert.AreEqual(110L, b.EpisodeId)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = New SousEpisodeReponse(LigneDeTest.Rangee(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode", 102L},
            {"create_user_id", 103L},
            {"horodate_creation", New Date(2024, 5, 5)}}))

        Assert.AreEqual("", b.NomFichier)
        Assert.AreEqual("", b.Commentaire)
        Assert.AreEqual("!", b.ValidateState)
        Assert.AreEqual(0L, b.ValidateUserId)
        Assert.AreEqual(Date.MinValue, b.ValidateDate)
        Assert.AreEqual(0L, b.EpisodeId)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode", 102L},
            {"create_user_id", 103L},
            {"horodate_creation", New Date(2024, 5, 5)}}
        valeurs.Remove("id")
        New SousEpisodeReponse(LigneDeTest.Rangee(Colonnes, valeurs))
    End Sub

End Class
