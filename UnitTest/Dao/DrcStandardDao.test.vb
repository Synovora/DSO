Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par DrcStandardDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestDrcStandardDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "type_activite_episode", "drc_id", "categorie_oasis", "age_min", "age_max",
        "date_modification", "inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = DrcStandardDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"type_activite_episode", "valeur_2"},
            {"drc_id", 103L},
            {"categorie_oasis", 104},
            {"age_min", 105},
            {"age_max", 106},
            {"date_modification", New Date(2024, 8, 8)},
            {"inactif", True}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual("valeur_2", b.TypeActivite)
        Assert.AreEqual(103L, b.DrcId)
        Assert.AreEqual(104, b.CategorieOasis)
        Assert.AreEqual(105, b.AgeMin)
        Assert.AreEqual(106, b.AgeMax)
        Assert.AreEqual(New Date(2024, 8, 8), b.DateModification)
        Assert.AreEqual(True, b.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = DrcStandardDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L}}))

        Assert.AreEqual("", b.TypeActivite)
        Assert.AreEqual(0L, b.DrcId)
        Assert.AreEqual(0, b.CategorieOasis)
        Assert.AreEqual(0, b.AgeMin)
        Assert.AreEqual(0, b.AgeMax)
        Assert.AreEqual(Date.MinValue, b.DateModification)
        Assert.AreEqual(False, b.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L}}
        valeurs.Remove("id")
        DrcStandardDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
