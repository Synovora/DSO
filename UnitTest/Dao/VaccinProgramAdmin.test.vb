Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New VaccinProgramAdmin. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New VaccinProgramAdmin(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestVaccinProgramAdminLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "vaccin_program_relation", "lot", "expiration", "comment"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New VaccinProgramAdmin(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"vaccin_program_relation", 102L},
            {"lot", "valeur_3"},
            {"expiration", New Date(2024, 5, 5)},
            {"comment", "valeur_5"}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.VaccinProgramRelation)
        Assert.AreEqual("valeur_3", b.Lot)
        Assert.AreEqual(New Date(2024, 5, 5), b.Expiration)
        Assert.AreEqual("valeur_5", b.Comment)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"vaccin_program_relation", 102L},
            {"lot", "valeur_3"},
            {"expiration", New Date(2024, 5, 5)},
            {"comment", "valeur_5"}}
        valeurs.Remove("id")
        New VaccinProgramAdmin(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
