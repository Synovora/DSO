Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New CGVDate. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New CGVDate(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestCGVDateLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "days", "patient", "operated_by", "operated_date", "signed_by", "signed_date"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New CGVDate(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"days", 102L},
            {"patient", 103L},
            {"operated_by", 104L},
            {"operated_date", New Date(2024, 6, 6)},
            {"signed_by", 106L},
            {"signed_date", New Date(2024, 8, 8)}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.Days)
        Assert.AreEqual(103L, b.Patient)
        Assert.AreEqual(104L, b.OperatedBy)
        Assert.AreEqual(New Date(2024, 6, 6), b.OperatedDate)
        Assert.AreEqual(106L, b.SignedBy)
        Assert.AreEqual(New Date(2024, 8, 8), b.SignedDate)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = New CGVDate(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"days", 102L},
            {"patient", 103L}}))

        Assert.AreEqual(0L, b.OperatedBy)
        Assert.AreEqual(Date.MinValue, b.OperatedDate)
        Assert.AreEqual(0L, b.SignedBy)
        Assert.AreEqual(Date.MinValue, b.SignedDate)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"days", 102L},
            {"patient", 103L}}
        valeurs.Remove("id")
        Dim ignore = New CGVDate(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
