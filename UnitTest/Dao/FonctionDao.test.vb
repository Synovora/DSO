Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par FonctionDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestFonctionDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_r_fonction_id", "oa_r_fonction_designation", "oa_r_fonction_libelle",
        "oa_r_fonction_type", "oa_r_fonction_ror_id", "oa_r_fonction_inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = FonctionDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_r_fonction_id", 101L},
            {"oa_r_fonction_designation", "valeur_2"},
            {"oa_r_fonction_libelle", "valeur_3"},
            {"oa_r_fonction_type", "valeur_4"},
            {"oa_r_fonction_ror_id", 105L},
            {"oa_r_fonction_inactif", True}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual("valeur_2", b.Designation)
        Assert.AreEqual("valeur_3", b.Libelle)
        Assert.AreEqual("valeur_4", b.[Type])
        Assert.AreEqual(105L, b.RorId)
        Assert.AreEqual(True, b.IsInactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = FonctionDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_r_fonction_id", 101L}}))

        Assert.AreEqual("", b.Designation)
        Assert.AreEqual("", b.Libelle)
        Assert.AreEqual("", b.[Type])
        Assert.AreEqual(0L, b.RorId)
        Assert.AreEqual(False, b.IsInactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' oa_r_fonction_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_r_fonction_id", 101L}}
        valeurs.Remove("oa_r_fonction_id")
        FonctionDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
