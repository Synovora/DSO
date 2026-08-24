Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par SpecialiteDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestSpecialiteDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_r_specialite_id", "oa_specialite_code", "oa_r_specialite_description",
        "oa_r_specialite_nature", "oa_r_parcours", "oa_r_oasis", "oa_r_specialite_genre",
        "oa_r_specialite_age_min", "oa_r_specialite_age_max", "oa_r_delaiPriseEnCharge",
        "oa_r_code_nos_g15_profession", "oa_r_code_nos_r04_type_savoir_faire",
        "oa_r_code_savoir_faire"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = SpecialiteDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_r_specialite_id", 101L},
            {"oa_specialite_code", "valeur_2"},
            {"oa_r_specialite_description", "valeur_3"},
            {"oa_r_specialite_nature", "valeur_4"},
            {"oa_r_parcours", True},
            {"oa_r_oasis", True},
            {"oa_r_specialite_genre", "valeur_7"},
            {"oa_r_specialite_age_min", 108},
            {"oa_r_specialite_age_max", 109},
            {"oa_r_delaiPriseEnCharge", 110},
            {"oa_r_code_nos_g15_profession", 111},
            {"oa_r_code_nos_r04_type_savoir_faire", "valeur_12"},
            {"oa_r_code_savoir_faire", "valeur_13"}}))

        Assert.AreEqual(101L, b.SpecialiteId)
        Assert.AreEqual("valeur_2", b.Code)
        Assert.AreEqual("valeur_3", b.Description)
        Assert.AreEqual("valeur_4", b.Nature)
        Assert.AreEqual(True, b.Parcours)
        Assert.AreEqual(True, b.Oasis)
        Assert.AreEqual("valeur_7", b.Genre)
        Assert.AreEqual(108, b.AgeMin)
        Assert.AreEqual(109, b.AgeMax)
        Assert.AreEqual(110, b.DelaiPriseEnCharge)
        Assert.AreEqual(111, b.NosG15CodeProfession)
        Assert.AreEqual("valeur_12", b.NosR40TypeSavoirFaire)
        Assert.AreEqual("valeur_13", b.NosCodeSavoirFaire)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = SpecialiteDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_r_specialite_id", 101L}}))

        Assert.AreEqual("", b.Code)
        Assert.AreEqual("", b.Description)
        Assert.AreEqual("", b.Nature)
        Assert.AreEqual(False, b.Parcours)
        Assert.AreEqual(False, b.Oasis)
        Assert.AreEqual("", b.Genre)
        Assert.AreEqual(0, b.AgeMin)
        Assert.AreEqual(0, b.AgeMax)
        Assert.AreEqual(0, b.DelaiPriseEnCharge)
        Assert.AreEqual(0, b.NosG15CodeProfession)
        Assert.AreEqual("", b.NosR40TypeSavoirFaire)
        Assert.AreEqual("", b.NosCodeSavoirFaire)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansSpecialiteIdEstUneErreur()
        ' oa_r_specialite_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_r_specialite_id", 101L}}
        valeurs.Remove("oa_r_specialite_id")
        SpecialiteDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
