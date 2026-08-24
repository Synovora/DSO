Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par FileExtensionDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestFileExtensionDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "ext", "description"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = FileExtensionDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"ext", "valeur_2"},
            {"description", "valeur_3"}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual("valeur_2", b.Extension)
        Assert.AreEqual("valeur_3", b.Description)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = FileExtensionDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L}}))

        Assert.AreEqual("", b.Extension)
        Assert.AreEqual("", b.Description)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L}}
        valeurs.Remove("id")
        FileExtensionDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
