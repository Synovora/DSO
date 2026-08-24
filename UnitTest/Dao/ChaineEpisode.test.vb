Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New ChaineEpisode. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New ChaineEpisode(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestChaineEpisodeLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "antecedent_id", "chaine_id"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New ChaineEpisode(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"antecedent_id", 102L},
            {"chaine_id", 103L}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.AntecedentId)
        Assert.AreEqual(103L, b.ChaineId)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"antecedent_id", 102L},
            {"chaine_id", 103L}}
        valeurs.Remove("id")
        Dim ignore = New ChaineEpisode(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
