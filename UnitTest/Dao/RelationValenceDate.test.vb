Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New RelationValenceDate. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New RelationValenceDate(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestRelationValenceDateLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "valence", "date", "patient", "status"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New RelationValenceDate(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"valence", 102L},
            {"date", 103L},
            {"patient", 104L},
            {"status", 105S}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.Valence)
        Assert.AreEqual(103L, b.[Date])
        Assert.AreEqual(104L, b.Patient)
        Assert.AreEqual(105S, b.Status)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"valence", 102L},
            {"date", 103L},
            {"patient", 104L},
            {"status", 105S}}
        valeurs.Remove("id")
        New RelationValenceDate(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
