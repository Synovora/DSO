Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New Vaccin. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New Vaccin(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestVaccinLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "code", "code_atc", "dci", "dci_longue", "date_import", "utilisateur_import"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New Vaccin(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101},
            {"code", 102L},
            {"code_atc", "valeur_3"},
            {"dci", "valeur_4"},
            {"dci_longue", "valeur_5"},
            {"date_import", New Date(2024, 7, 7)},
            {"utilisateur_import", 107L}}))

        Assert.AreEqual(101, b.Id)
        Assert.AreEqual(102L, b.Code)
        Assert.AreEqual("valeur_3", b.CodeAtc)
        Assert.AreEqual("valeur_4", b.Dci)
        Assert.AreEqual("valeur_5", b.DciLongue)
        Assert.AreEqual(New Date(2024, 7, 7), b.DateImport)
        Assert.AreEqual(107L, b.UtilisateurImport)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101},
            {"code", 102L},
            {"code_atc", "valeur_3"},
            {"dci", "valeur_4"},
            {"dci_longue", "valeur_5"},
            {"date_import", New Date(2024, 7, 7)},
            {"utilisateur_import", 107L}}
        valeurs.Remove("id")
        New Vaccin(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
