Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par DrcDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestDrcDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_drc_id", "oa_drc_libelle", "oa_drc_sexe", "oa_drc_typ_epi", "oa_drc_age_min",
        "oa_drc_age_max", "oa_drc_categorie_majeure_id", "oa_drc_oasis_categorie",
        "oa_drc_code_cim_defaut", "oa_drc_code_cisp_defaut", "oa_drc_ald_id", "oa_drc_ald_code",
        "oa_drc_dur_prob_epis", "oa_drc_url", "oa_drc_date_creation",
        "oa_drc_utilisateur_creation", "oa_drc_date_modification",
        "oa_drc_utilisateur_modification"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = DrcDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_drc_id", 101},
            {"oa_drc_libelle", "valeur_2"},
            {"oa_drc_sexe", 103},
            {"oa_drc_typ_epi", "valeur_4"},
            {"oa_drc_age_min", 105},
            {"oa_drc_age_max", 106},
            {"oa_drc_categorie_majeure_id", 107},
            {"oa_drc_oasis_categorie", 108},
            {"oa_drc_code_cim_defaut", "valeur_9"},
            {"oa_drc_code_cisp_defaut", "valeur_10"},
            {"oa_drc_ald_id", 111},
            {"oa_drc_ald_code", "valeur_12"},
            {"oa_drc_dur_prob_epis", "valeur_13"},
            {"oa_drc_url", "valeur_14"},
            {"oa_drc_date_creation", New Date(2024, 5, 17)},
            {"oa_drc_utilisateur_creation", 117L},
            {"oa_drc_date_modification", New Date(2024, 7, 19)},
            {"oa_drc_utilisateur_modification", 119L}}))

        Assert.AreEqual(101, b.DrcId)
        Assert.AreEqual("valeur_2", b.DrcLibelle)
        Assert.AreEqual(103, b.DrcSexe)
        Assert.AreEqual("valeur_4", b.DrcTypeEpisode)
        Assert.AreEqual(105, b.DrcAgeMin)
        Assert.AreEqual(106, b.DrcAgeMax)
        Assert.AreEqual(107, b.CategorieMajeure)
        Assert.AreEqual(108, b.CategorieOasisId)
        Assert.AreEqual("valeur_9", b.CodeCim)
        Assert.AreEqual("valeur_10", b.CodeCisp)
        Assert.AreEqual(111, b.AldId)
        Assert.AreEqual("valeur_12", b.AldCode)
        Assert.AreEqual("valeur_13", b.Commentaire)
        Assert.AreEqual("valeur_14", b.Wiki)
        Assert.AreEqual("valeur_4", b.ReponseCommentee)
        Assert.AreEqual(New Date(2024, 5, 17), b.DateCreation)
        Assert.AreEqual(117L, b.UserCreation)
        Assert.AreEqual(New Date(2024, 7, 19), b.DateModification)
        Assert.AreEqual(119L, b.UserModification)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = DrcDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_drc_id", 101}}))

        Assert.AreEqual("", b.DrcLibelle)
        Assert.AreEqual(0, b.DrcSexe)
        Assert.AreEqual("", b.DrcTypeEpisode)
        Assert.AreEqual(0, b.DrcAgeMin)
        Assert.AreEqual(0, b.DrcAgeMax)
        Assert.AreEqual(0, b.CategorieMajeure)
        Assert.AreEqual(0, b.CategorieOasisId)
        Assert.AreEqual("", b.CodeCim)
        Assert.AreEqual("", b.CodeCisp)
        Assert.AreEqual(0, b.AldId)
        Assert.AreEqual("", b.AldCode)
        Assert.AreEqual("", b.Commentaire)
        Assert.AreEqual("", b.Wiki)
        Assert.AreEqual("", b.ReponseCommentee)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
        Assert.AreEqual(0L, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateModification)
        Assert.AreEqual(0L, b.UserModification)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansDrcIdEstUneErreur()
        ' oa_drc_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_drc_id", 101}}
        valeurs.Remove("oa_drc_id")
        DrcDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
