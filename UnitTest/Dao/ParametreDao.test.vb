Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par ParametreDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestParametreDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "description", "description_patient", "entier", "decimal", "unite", "valeur_min",
        "valeur_max", "ordre", "inactif", "exclusion_auto_suivi", "aide_associee", "wiki"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = ParametreDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"description", "valeur_2"},
            {"description_patient", "valeur_3"},
            {"entier", 104},
            {"decimal", 105},
            {"unite", "valeur_6"},
            {"valeur_min", 7.5D},
            {"valeur_max", 8.5D},
            {"ordre", 109},
            {"inactif", True},
            {"exclusion_auto_suivi", "valeur_11"},
            {"aide_associee", "valeur_12"},
            {"wiki", "valeur_13"}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual("valeur_2", b.Description)
        Assert.AreEqual("valeur_3", b.DescriptionPatient)
        Assert.AreEqual(104, b.Entier)
        Assert.AreEqual(105, b.[Decimal])
        Assert.AreEqual("valeur_6", b.Unite)
        Assert.AreEqual(7.5D, b.ValeurMin)
        Assert.AreEqual(8.5D, b.ValeurMax)
        Assert.AreEqual(109, b.Ordre)
        Assert.AreEqual(True, b.Inactif)
        Assert.AreEqual("valeur_11", b.ExclusionAutoSuivi)
        Assert.AreEqual("valeur_12", b.AideAssociee)
        Assert.AreEqual("valeur_13", b.Wiki)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = ParametreDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L}}))

        Assert.AreEqual("", b.Description)
        Assert.AreEqual("", b.DescriptionPatient)
        Assert.AreEqual(0, b.Entier)
        Assert.AreEqual(0, b.[Decimal])
        Assert.AreEqual("", b.Unite)
        Assert.AreEqual(0D, b.ValeurMin)
        Assert.AreEqual(0D, b.ValeurMax)
        Assert.AreEqual(0, b.Ordre)
        Assert.AreEqual(False, b.Inactif)
        ' Propriété String qui tient lieu de booléen : les appelants la comparent à True,
        ' ce qui fonctionne avec "False" et lèverait avec "". On fige donc "False".
        Assert.AreEqual("False", b.ExclusionAutoSuivi)
        Assert.AreEqual("", b.AideAssociee)
        Assert.AreEqual("", b.Wiki)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L}}
        valeurs.Remove("id")
        ParametreDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
