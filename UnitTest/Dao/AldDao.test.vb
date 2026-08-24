Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par AldDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestAldDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_ald_id", "oa_ald_code", "oa_ald_description"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = AldDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_ald_id", 101},
            {"oa_ald_code", "valeur_2"},
            {"oa_ald_description", "valeur_3"}}))

        Assert.AreEqual(101, b.AldId)
        Assert.AreEqual("valeur_2", b.AldCode)
        Assert.AreEqual("valeur_3", b.AldDescription)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = AldDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_ald_id", 101}}))

        Assert.AreEqual("", b.AldCode)
        Assert.AreEqual("", b.AldDescription)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansAldIdEstUneErreur()
        ' oa_ald_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_ald_id", 101}}
        valeurs.Remove("oa_ald_id")
        AldDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
