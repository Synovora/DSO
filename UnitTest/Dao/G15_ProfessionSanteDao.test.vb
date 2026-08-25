Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par G15_ProfessionSanteDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestG15_ProfessionSanteDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oid", "code", "libelle"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = NosProfessionSanteDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oid", "valeur_1"},
            {"code", 102},
            {"libelle", "valeur_3"}}))

        Assert.AreEqual("valeur_1", b.Oid)
        Assert.AreEqual(102, b.Code)
        Assert.AreEqual("valeur_3", b.Libelle)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = NosProfessionSanteDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oid", "valeur_1"}}))

        Assert.AreEqual(0, b.Code)
        Assert.AreEqual("", b.Libelle)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansOidEstUneErreur()
        ' oid n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oid", "valeur_1"}}
        valeurs.Remove("oid")
        NosProfessionSanteDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
