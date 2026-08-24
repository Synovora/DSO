Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par AldCim10Dao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestAldCim10DaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_ald_cim10_id", "oa_ald_cim10_ald_id", "oa_ald_cim10_ald_code", "oa_ald_cim10_code",
        "oa_ald_cim10_description"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = AldCim10Dao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_ald_cim10_id", 101},
            {"oa_ald_cim10_ald_id", 102},
            {"oa_ald_cim10_ald_code", "valeur_3"},
            {"oa_ald_cim10_code", "valeur_4"},
            {"oa_ald_cim10_description", "valeur_5"}}))

        Assert.AreEqual(101, b.AldCim10Id)
        Assert.AreEqual(102, b.AldCim10AldId)
        Assert.AreEqual("valeur_3", b.AldCim10AldCode)
        Assert.AreEqual("valeur_4", b.AldCim10Code)
        Assert.AreEqual("valeur_5", b.AldCim10Description)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = AldCim10Dao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_ald_cim10_id", 101}}))

        Assert.AreEqual(0, b.AldCim10AldId)
        Assert.AreEqual("", b.AldCim10AldCode)
        Assert.AreEqual("", b.AldCim10Code)
        Assert.AreEqual("", b.AldCim10Description)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansAldCim10IdEstUneErreur()
        ' oa_ald_cim10_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_ald_cim10_id", 101}}
        valeurs.Remove("oa_ald_cim10_id")
        AldCim10Dao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
