Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New VaccinProgramRelation. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New VaccinProgramRelation(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestVaccinProgramRelationLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "patient", "date", "vaccin", "relation_vaccin_valence", "realisation_date",
        "realisation_operator", "realisation_operator_ror", "realisation_operator_text"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New VaccinProgramRelation(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"patient", 102L},
            {"date", 103L},
            {"vaccin", 104L},
            {"relation_vaccin_valence", 105L},
            {"realisation_date", New Date(2024, 7, 7)},
            {"realisation_operator", 107L},
            {"realisation_operator_ror", 108L},
            {"realisation_operator_text", "valeur_9"}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.Patient)
        Assert.AreEqual(103L, b.[Date])
        Assert.AreEqual(104L, b.Vaccin)
        Assert.AreEqual(105L, b.RelationVaccinValence)
        Assert.AreEqual(New Date(2024, 7, 7), b.RealisationDate)
        Assert.AreEqual(107L, b.RealisationOperator)
        Assert.AreEqual(108L, b.RealisationOperatorRor)
        Assert.AreEqual("valeur_9", b.RealisationOperatorText)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = New VaccinProgramRelation(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"patient", 102L},
            {"date", 103L},
            {"vaccin", 104L},
            {"relation_vaccin_valence", 105L}}))

        Assert.AreEqual(Date.MinValue, b.RealisationDate)
        Assert.AreEqual(0L, b.RealisationOperator)
        Assert.AreEqual(0L, b.RealisationOperatorRor)
        Assert.IsNull(b.RealisationOperatorText)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"patient", 102L},
            {"date", 103L},
            {"vaccin", 104L},
            {"relation_vaccin_valence", 105L}}
        valeurs.Remove("id")
        New VaccinProgramRelation(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
