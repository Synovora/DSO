Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New SousEpisodeType. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New SousEpisodeType(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestSousEpisodeTypeLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "categorie", "horodate_creation", "libelle", "is_with_destinataire"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New SousEpisodeType(LigneDeTest.Rangee(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"categorie", "valeur_2"},
            {"horodate_creation", New Date(2024, 4, 4)},
            {"libelle", "valeur_4"},
            {"is_with_destinataire", True}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual("valeur_2", b.Category)
        Assert.AreEqual(New Date(2024, 4, 4), b.HorodateCreation)
        Assert.AreEqual("valeur_4", b.Libelle)
        Assert.AreEqual(True, b.IsWithDestinataire)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"categorie", "valeur_2"},
            {"horodate_creation", New Date(2024, 4, 4)},
            {"libelle", "valeur_4"},
            {"is_with_destinataire", True}}
        valeurs.Remove("id")
        Dim ignore = New SousEpisodeType(LigneDeTest.Rangee(Colonnes, valeurs))
    End Sub

End Class
