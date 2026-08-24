Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New SousEpisodeReponseMail. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New SousEpisodeReponseMail(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestSousEpisodeReponseMailLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "horodate_creation", "patient_id", "status", "auteur", "objet"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New SousEpisodeReponseMail(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"horodate_creation", New Date(2024, 3, 3)},
            {"patient_id", 103L},
            {"status", "valeur_4"},
            {"auteur", "valeur_5"},
            {"objet", "valeur_6"}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(New Date(2024, 3, 3), b.HorodateCreation)
        Assert.AreEqual(103L, b.PatientId)
        Assert.AreEqual("valeur_4", b.Status)
        Assert.AreEqual("valeur_5", b.Auteur)
        Assert.AreEqual("valeur_6", b.Objet)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = New SousEpisodeReponseMail(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"horodate_creation", New Date(2024, 3, 3)},
            {"status", "valeur_3"},
            {"auteur", "valeur_4"}}))

        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual("", b.Objet)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"horodate_creation", New Date(2024, 3, 3)},
            {"status", "valeur_3"},
            {"auteur", "valeur_4"}}
        valeurs.Remove("id")
        New SousEpisodeReponseMail(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
