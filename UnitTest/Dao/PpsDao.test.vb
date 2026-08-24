Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par PpsDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestPpsDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_pps_id", "oa_pps_patient_id", "oa_pps_categorie", "oa_pps_sous_categorie",
        "oa_pps_priorite", "oa_pps_drc_id", "oa_pps_affichage_synthese", "oa_pps_commentaire",
        "oa_pps_date_debut", "oa_pps_date_fin", "oa_pps_arret", "oa_pps_commentaire_arret",
        "oa_pps_utilisateur_creation", "oa_pps_date_creation", "oa_pps_utilisateur_modification",
        "oa_pps_date_modification"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = PpsDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_pps_id", 101},
            {"oa_pps_patient_id", 102},
            {"oa_pps_categorie", 103},
            {"oa_pps_sous_categorie", 104},
            {"oa_pps_priorite", 105},
            {"oa_pps_drc_id", 106},
            {"oa_pps_affichage_synthese", True},
            {"oa_pps_commentaire", "valeur_8"},
            {"oa_pps_date_debut", New Date(2024, 10, 10)},
            {"oa_pps_arret", True},
            {"oa_pps_commentaire_arret", "valeur_12"},
            {"oa_pps_utilisateur_creation", 113},
            {"oa_pps_date_creation", New Date(2024, 3, 15)},
            {"oa_pps_utilisateur_modification", 115},
            {"oa_pps_date_modification", New Date(2024, 5, 17)}}))

        Assert.AreEqual(101, b.Id)
        Assert.AreEqual(102, b.PatientId)
        Assert.AreEqual(103, b.CategorieId)
        Assert.AreEqual(104, b.SousCategorieId)
        Assert.AreEqual(105, b.Priorite)
        Assert.AreEqual(106, b.DrcId)
        Assert.AreEqual(True, b.AffichageSynthese)
        Assert.AreEqual("valeur_8", b.Commentaire)
        Assert.AreEqual(New Date(2024, 10, 10), b.DateDebut)
        Assert.AreEqual(True, b.Arret)
        Assert.AreEqual("valeur_12", b.ArretCommentaire)
        Assert.AreEqual(113, b.UserCreation)
        Assert.AreEqual(New Date(2024, 3, 15), b.DateCreation)
        Assert.AreEqual(115, b.UserModification)
        Assert.AreEqual(New Date(2024, 5, 17), b.DateModification)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = PpsDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_pps_id", 101}}))

        Assert.AreEqual(0, b.PatientId)
        Assert.AreEqual(0, b.CategorieId)
        Assert.AreEqual(0, b.SousCategorieId)
        Assert.AreEqual(0, b.Priorite)
        Assert.AreEqual(0, b.DrcId)
        Assert.AreEqual(False, b.AffichageSynthese)
        Assert.AreEqual("", b.Commentaire)
        Assert.AreEqual(Date.MinValue, b.DateDebut)
        Assert.AreEqual(False, b.Arret)
        Assert.AreEqual("", b.ArretCommentaire)
        Assert.AreEqual(0, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
        Assert.AreEqual(0, b.UserModification)
        Assert.AreEqual(Date.MinValue, b.DateModification)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' oa_pps_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_pps_id", 101}}
        valeurs.Remove("oa_pps_id")
        PpsDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
