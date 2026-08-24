Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New SousEpisodeDetailSousType. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New SousEpisodeDetailSousType(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestSousEpisodeDetailSousTypeLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "id_sous_episode", "id_sous_episode_sous_sous_type", "is_ald"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New SousEpisodeDetailSousType(LigneDeTest.Rangee(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode", 102L},
            {"id_sous_episode_sous_sous_type", 103L},
            {"is_ald", True}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.IdSousEpisode)
        Assert.AreEqual(103L, b.IdSousEpisodeSousSousType)
        Assert.AreEqual(True, b.IsALD)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode", 102L},
            {"id_sous_episode_sous_sous_type", 103L},
            {"is_ald", True}}
        valeurs.Remove("id")
        Dim ignore = New SousEpisodeDetailSousType(LigneDeTest.Rangee(Colonnes, valeurs))
    End Sub

End Class
