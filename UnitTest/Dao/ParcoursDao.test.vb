Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par ParcoursDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestParcoursDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_parcours_id", "oa_parcours_patient_id", "oa_parcours_specialite",
        "oa_parcours_categorie_id", "oa_parcours_sous_categorie_id",
        "oa_parcours_intervenant_oasis", "oa_parcours_ror_id", "oa_parcours_commentaire",
        "oa_parcours_base", "oa_parcours_rythme", "oa_parcours_cacher", "oa_parcours_inactif",
        "oa_parcours_utilisateur_creation", "oa_parcours_date_creation",
        "oa_parcours_utilisateur_modification", "oa_parcours_date_modification"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = ParcoursDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_parcours_id", 101},
            {"oa_parcours_patient_id", 102},
            {"oa_parcours_specialite", 103},
            {"oa_parcours_categorie_id", 104},
            {"oa_parcours_sous_categorie_id", 105},
            {"oa_parcours_intervenant_oasis", True},
            {"oa_parcours_ror_id", 107},
            {"oa_parcours_commentaire", "valeur_8"},
            {"oa_parcours_base", "valeur_9"},
            {"oa_parcours_rythme", 110},
            {"oa_parcours_cacher", True},
            {"oa_parcours_inactif", True},
            {"oa_parcours_utilisateur_creation", 113},
            {"oa_parcours_date_creation", New Date(2024, 3, 15)},
            {"oa_parcours_utilisateur_modification", 115},
            {"oa_parcours_date_modification", New Date(2024, 5, 17)}}))

        Assert.AreEqual(101, b.Id)
        Assert.AreEqual(102, b.PatientId)
        Assert.AreEqual(103, b.SpecialiteId)
        Assert.AreEqual(104, b.CategorieId)
        Assert.AreEqual(105, b.SousCategorieId)
        Assert.AreEqual(True, b.IntervenantOasis)
        Assert.AreEqual(107, b.RorId)
        Assert.AreEqual("valeur_8", b.Commentaire)
        Assert.AreEqual("valeur_9", b.Base)
        Assert.AreEqual(110, b.Rythme)
        Assert.AreEqual(True, b.Cacher)
        Assert.AreEqual(True, b.Inactif)
        Assert.AreEqual(113, b.UserCreation)
        Assert.AreEqual(New Date(2024, 3, 15), b.DateCreation)
        Assert.AreEqual(115, b.UserModification)
        Assert.AreEqual(New Date(2024, 5, 17), b.DateModification)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = ParcoursDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_parcours_id", 101}}))

        Assert.AreEqual(0, b.PatientId)
        Assert.AreEqual(0, b.SpecialiteId)
        Assert.AreEqual(0, b.CategorieId)
        Assert.AreEqual(0, b.SousCategorieId)
        Assert.AreEqual(False, b.IntervenantOasis)
        Assert.AreEqual(0, b.RorId)
        Assert.AreEqual("", b.Commentaire)
        Assert.AreEqual("", b.Base)
        Assert.AreEqual(0, b.Rythme)
        Assert.AreEqual(False, b.Cacher)
        Assert.AreEqual(False, b.Inactif)
        Assert.AreEqual(0, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
        Assert.AreEqual(0, b.UserModification)
        Assert.AreEqual(Date.MinValue, b.DateModification)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' oa_parcours_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_parcours_id", 101}}
        valeurs.Remove("oa_parcours_id")
        ParcoursDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
