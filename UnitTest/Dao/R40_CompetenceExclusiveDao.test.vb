Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par R40_CompetenceExclusiveDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestR40_CompetenceExclusiveDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oid", "code", "libelle"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = NosCompetenceExclusiveDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oid", "valeur_1"},
            {"code", "valeur_2"},
            {"libelle", "valeur_3"}}))

        Assert.AreEqual("valeur_1", b.Oid)
        Assert.AreEqual("valeur_2", b.Code)
        Assert.AreEqual("valeur_3", b.Libelle)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = NosCompetenceExclusiveDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oid", "valeur_1"}}))

        Assert.AreEqual("", b.Code)
        Assert.AreEqual("", b.Libelle)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansOidEstUneErreur()
        ' oid n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oid", "valeur_1"}}
        valeurs.Remove("oid")
        NosCompetenceExclusiveDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
