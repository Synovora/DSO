Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par ParametreDrcDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestParametreDrcDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "drc_id", "parametre_id"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = ParametreDrcDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"drc_id", 102L},
            {"parametre_id", 103L}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.DrcId)
        Assert.AreEqual(103L, b.ParametreId)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = ParametreDrcDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L}}))

        Assert.AreEqual(0L, b.DrcId)
        Assert.AreEqual(0L, b.ParametreId)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L}}
        valeurs.Remove("id")
        ParametreDrcDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
