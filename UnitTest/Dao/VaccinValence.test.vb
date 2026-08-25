Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New VaccinValence. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New VaccinValence(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestVaccinValenceLecture

    Private Shared ReadOnly Colonnes As String() = {
        "valence", "id", "code", "code_atc", "dci", "dci_longue", "date_import",
        "utilisateur_import"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New VaccinValence(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"valence", 101L},
            {"id", 102},
            {"code", 103L},
            {"code_atc", "valeur_4"},
            {"dci", "valeur_5"},
            {"dci_longue", "valeur_6"},
            {"date_import", New Date(2024, 8, 8)},
            {"utilisateur_import", 108L}}))

        Assert.AreEqual(101L, b.Valence)
        Assert.AreEqual(102, b.Id)
        Assert.AreEqual(103L, b.Code)
        Assert.AreEqual("valeur_4", b.CodeAtc)
        Assert.AreEqual("valeur_5", b.Dci)
        Assert.AreEqual("valeur_6", b.DciLongue)
        Assert.AreEqual(New Date(2024, 8, 8), b.DateImport)
        Assert.AreEqual(108L, b.UtilisateurImport)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansValenceEstUneErreur()
        ' valence n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"valence", 101L},
            {"id", 102},
            {"code", 103L},
            {"code_atc", "valeur_4"},
            {"dci", "valeur_5"},
            {"dci_longue", "valeur_6"},
            {"date_import", New Date(2024, 8, 8)},
            {"utilisateur_import", 108L}}
        valeurs.Remove("valence")
        Dim ignore = New VaccinValence(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
