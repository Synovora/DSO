Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New RelationChaineEpisode. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New RelationChaineEpisode(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestRelationChaineEpisodeLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "episode_id", "chaine_id"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New RelationChaineEpisode(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"episode_id", 102L},
            {"chaine_id", 103L}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.EpisodeId)
        Assert.AreEqual(103L, b.ChaineId)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"episode_id", 102L},
            {"chaine_id", 103L}}
        valeurs.Remove("id")
        New RelationChaineEpisode(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
