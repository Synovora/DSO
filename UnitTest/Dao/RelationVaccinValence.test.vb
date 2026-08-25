Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New RelationVaccinValence. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New RelationVaccinValence(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestRelationVaccinValenceLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "vaccin", "valence"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New RelationVaccinValence(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"vaccin", 102L},
            {"valence", 103L}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.Vaccin)
        Assert.AreEqual(103L, b.Valence)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"vaccin", 102L},
            {"valence", 103L}}
        valeurs.Remove("id")
        New RelationVaccinValence(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
