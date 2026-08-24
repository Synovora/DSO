Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par AntecedentDeplacementDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestAntecedentDeplacementDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_antecedent_id", "oa_antecedent_niveau", "oa_antecedent_id_niveau1",
        "oa_antecedent_id_niveau2", "oa_antecedent_ordre_affichage1",
        "oa_antecedent_ordre_affichage2", "oa_antecedent_ordre_affichage3"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = AntecedentDeplacementDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_antecedent_id", 101},
            {"oa_antecedent_niveau", 102},
            {"oa_antecedent_id_niveau1", 103},
            {"oa_antecedent_id_niveau2", 104},
            {"oa_antecedent_ordre_affichage1", 105},
            {"oa_antecedent_ordre_affichage2", 106},
            {"oa_antecedent_ordre_affichage3", 107}}))

        Assert.AreEqual(101, b.Id)
        Assert.AreEqual(102, b.Niveau)
        Assert.AreEqual(103, b.Niveau1Id)
        Assert.AreEqual(104, b.Niveau2Id)
        Assert.AreEqual(105, b.Ordre1)
        Assert.AreEqual(106, b.Ordre2)
        Assert.AreEqual(107, b.Ordre3)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = AntecedentDeplacementDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_antecedent_id", 101}}))

        Assert.AreEqual(0, b.Niveau)
        Assert.AreEqual(0, b.Niveau1Id)
        Assert.AreEqual(0, b.Niveau2Id)
        Assert.AreEqual(0, b.Ordre1)
        Assert.AreEqual(0, b.Ordre2)
        Assert.AreEqual(0, b.Ordre3)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' oa_antecedent_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_antecedent_id", 101}}
        valeurs.Remove("oa_antecedent_id")
        AntecedentDeplacementDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
