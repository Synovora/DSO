Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New CGVValence. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New CGVValence(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestCGVValenceLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "code", "description", "precaution", "valence", "ordre", "patient"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New CGVValence(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"code", "valeur_2"},
            {"description", "valeur_3"},
            {"precaution", "valeur_4"},
            {"valence", 105L},
            {"ordre", 106},
            {"patient", 107L}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual("valeur_2", b.Code)
        Assert.AreEqual("valeur_3", b.Description)
        Assert.AreEqual("valeur_4", b.Precaution)
        Assert.AreEqual(105L, b.Valence)
        Assert.AreEqual(106, b.Ordre)
        Assert.AreEqual(107L, b.Patient)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"code", "valeur_2"},
            {"description", "valeur_3"},
            {"precaution", "valeur_4"},
            {"valence", 105L},
            {"ordre", 106},
            {"patient", 107L}}
        valeurs.Remove("id")
        New CGVValence(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
