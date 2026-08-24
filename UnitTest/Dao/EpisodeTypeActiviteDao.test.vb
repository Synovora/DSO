Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par EpisodeTypeActiviteDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestEpisodeTypeActiviteDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_activite_type", "oa_activite_nature", "oa_activite_description", "oa_activite_inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = EpisodeTypeActiviteDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_activite_type", "valeur_1"},
            {"oa_activite_nature", "valeur_2"},
            {"oa_activite_description", "valeur_3"},
            {"oa_activite_inactif", True}}))

        Assert.AreEqual("valeur_1", b.[Type])
        Assert.AreEqual("valeur_2", b.Nature)
        Assert.AreEqual("valeur_3", b.Description)
        Assert.AreEqual(True, b.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = EpisodeTypeActiviteDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_activite_type", "valeur_1"}}))

        Assert.AreEqual("", b.Nature)
        Assert.AreEqual("", b.Description)
        Assert.AreEqual(False, b.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansTypeEstUneErreur()
        ' oa_activite_type n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_activite_type", "valeur_1"}}
        valeurs.Remove("oa_activite_type")
        EpisodeTypeActiviteDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
