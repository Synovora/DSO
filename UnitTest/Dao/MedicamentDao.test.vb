Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par MedicamentDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestMedicamentDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_medicament_cis", "oa_medicament_dci", "oa_medicament_forme", "oa_medicament_titulaire",
        "oa_medicament_voie_administration"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = MedicamentDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_medicament_cis", 101},
            {"oa_medicament_dci", "valeur_2"},
            {"oa_medicament_forme", "valeur_3"},
            {"oa_medicament_titulaire", "valeur_4"},
            {"oa_medicament_voie_administration", "valeur_5"}}))

        Assert.AreEqual(101, b.MedicamentCis)
        Assert.AreEqual("valeur_2", b.MedicamentDci)
        Assert.AreEqual("valeur_3", b.Forme)
        Assert.AreEqual("valeur_4", b.Titulaire)
        Assert.AreEqual("valeur_5", b.VoieAdministration)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = MedicamentDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_medicament_cis", 101}}))

        Assert.AreEqual("", b.MedicamentDci)
        Assert.AreEqual("", b.Forme)
        Assert.AreEqual("", b.Titulaire)
        Assert.AreEqual("", b.VoieAdministration)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansMedicamentCisEstUneErreur()
        ' oa_medicament_cis n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_medicament_cis", 101}}
        valeurs.Remove("oa_medicament_cis")
        MedicamentDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
