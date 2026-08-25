Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New Valence. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New Valence(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestValenceLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "code", "description", "precaution", "date_creation", "date_modification",
        "utilisateur_creation", "utilisateur_modification", "actif", "visible", "ordre"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New Valence(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"code", "valeur_2"},
            {"description", "valeur_3"},
            {"precaution", "valeur_4"},
            {"date_creation", New Date(2024, 6, 6)},
            {"date_modification", New Date(2024, 7, 7)},
            {"utilisateur_creation", 107L},
            {"utilisateur_modification", 108L},
            {"actif", True},
            {"visible", True},
            {"ordre", 111}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual("valeur_2", b.Code)
        Assert.AreEqual("valeur_3", b.Description)
        Assert.AreEqual("valeur_4", b.Precaution)
        Assert.AreEqual(New Date(2024, 6, 6), b.DateCreation)
        Assert.AreEqual(New Date(2024, 7, 7), b.DateModification)
        Assert.AreEqual(107L, b.UtilisateurCreation)
        Assert.AreEqual(108L, b.UtilisateurModification)
        Assert.AreEqual(True, b.Actif)
        Assert.AreEqual(True, b.Visible)
        Assert.AreEqual(111, b.Ordre)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"code", "valeur_2"},
            {"description", "valeur_3"},
            {"precaution", "valeur_4"},
            {"date_creation", New Date(2024, 6, 6)},
            {"date_modification", New Date(2024, 7, 7)},
            {"utilisateur_creation", 107L},
            {"utilisateur_modification", 108L},
            {"actif", True},
            {"visible", True},
            {"ordre", 111}}
        valeurs.Remove("id")
        Dim ignore = New Valence(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
