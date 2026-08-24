Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par EpisodeParametreDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestEpisodeParametreDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "episode_parametre_id", "parametre_id", "episode_id", "patient_id", "valeur",
        "description", "entier", "decimal", "unite", "parametre_ajoute", "ordre", "inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = EpisodeParametreDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"episode_parametre_id", 101L},
            {"parametre_id", 102L},
            {"episode_id", 103L},
            {"patient_id", 104L},
            {"description", "valeur_6"},
            {"entier", 107},
            {"decimal", 108},
            {"unite", "valeur_9"},
            {"parametre_ajoute", True},
            {"ordre", 111},
            {"inactif", True}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.ParametreId)
        Assert.AreEqual(103L, b.EpisodeId)
        Assert.AreEqual(104L, b.PatientId)
        Assert.AreEqual("valeur_6", b.Description)
        Assert.AreEqual(107, b.Entier)
        Assert.AreEqual(108, b.[Decimal])
        Assert.AreEqual("valeur_9", b.Unite)
        Assert.AreEqual(True, b.ParametreAjoute)
        Assert.AreEqual(111, b.Ordre)
        Assert.AreEqual(True, b.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = EpisodeParametreDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"episode_parametre_id", 101L}}))

        Assert.AreEqual(0L, b.ParametreId)
        Assert.AreEqual(0L, b.EpisodeId)
        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual("", b.Description)
        Assert.AreEqual(0, b.Entier)
        Assert.AreEqual(0, b.[Decimal])
        Assert.AreEqual("", b.Unite)
        Assert.AreEqual(False, b.ParametreAjoute)
        Assert.AreEqual(0, b.Ordre)
        Assert.AreEqual(False, b.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' episode_parametre_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"episode_parametre_id", 101L}}
        valeurs.Remove("episode_parametre_id")
        EpisodeParametreDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
