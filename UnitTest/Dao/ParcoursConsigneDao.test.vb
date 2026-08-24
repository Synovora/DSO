Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par ParcoursConsigneDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestParcoursConsigneDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_parcours_consigne_id", "oa_parcours_consigne_patient_id", "oa_parcours_id",
        "oa_parcours_consigne_drc_id", "activite_type_episode", "oa_parcours_consigne_commentaire",
        "oa_parcours_consigne_ordre", "oa_parcours_age_min", "oa_parcours_age_max",
        "oa_parcours_age_unite", "oa_parcours_consigne_date_debut",
        "oa_parcours_consigne_date_fin", "oa_parcours_consigne_inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = ParcoursConsigneDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_parcours_consigne_id", 101L},
            {"oa_parcours_consigne_patient_id", 102L},
            {"oa_parcours_id", 103L},
            {"oa_parcours_consigne_drc_id", 104L},
            {"activite_type_episode", "valeur_5"},
            {"oa_parcours_consigne_commentaire", "valeur_6"},
            {"oa_parcours_consigne_ordre", 107},
            {"oa_parcours_age_min", 108},
            {"oa_parcours_age_max", 109},
            {"oa_parcours_age_unite", "valeur_10"},
            {"oa_parcours_consigne_date_debut", New Date(2024, 12, 12)},
            {"oa_parcours_consigne_date_fin", New Date(2024, 1, 13)},
            {"oa_parcours_consigne_inactif", True}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.PatientId)
        Assert.AreEqual(103L, b.ParcoursId)
        Assert.AreEqual(104L, b.DrcId)
        Assert.AreEqual("valeur_5", b.TypeEpisode)
        Assert.AreEqual("valeur_6", b.Commentaire)
        Assert.AreEqual(107, b.Ordre)
        Assert.AreEqual(108, b.AgeMin)
        Assert.AreEqual(109, b.AgeMax)
        Assert.AreEqual("valeur_10", b.AgeUnite)
        Assert.AreEqual(New Date(2024, 12, 12), b.DateDebut)
        Assert.AreEqual(New Date(2024, 1, 13), b.DateFin)
        Assert.AreEqual(True, b.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = ParcoursConsigneDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_parcours_consigne_id", 101L}}))

        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual(0L, b.ParcoursId)
        Assert.AreEqual(0L, b.DrcId)
        Assert.AreEqual("", b.TypeEpisode)
        Assert.AreEqual("", b.Commentaire)
        Assert.AreEqual(0, b.Ordre)
        Assert.AreEqual(0, b.AgeMin)
        Assert.AreEqual(0, b.AgeMax)
        Assert.AreEqual("", b.AgeUnite)
        Assert.AreEqual(Date.MinValue, b.DateDebut)
        Assert.AreEqual(Date.MinValue, b.DateFin)
        Assert.AreEqual(False, b.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' oa_parcours_consigne_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_parcours_consigne_id", 101L}}
        valeurs.Remove("oa_parcours_consigne_id")
        ParcoursConsigneDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
