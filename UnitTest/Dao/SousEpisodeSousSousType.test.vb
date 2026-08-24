Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New SousEpisodeSousSousType. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New SousEpisodeSousSousType(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestSousEpisodeSousSousTypeLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "id_sous_episode_sous_type", "horodate_creation", "libelle", "commentaire"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New SousEpisodeSousSousType(LigneDeTest.Rangee(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode_sous_type", 102L},
            {"horodate_creation", New Date(2024, 4, 4)},
            {"libelle", "valeur_4"},
            {"commentaire", "valeur_5"}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.IdSousEpisodeSousType)
        Assert.AreEqual(New Date(2024, 4, 4), b.HorodateCreation)
        Assert.AreEqual("valeur_4", b.Libelle)
        Assert.AreEqual("valeur_5", b.Commentaire)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = New SousEpisodeSousSousType(LigneDeTest.Rangee(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode_sous_type", 102L},
            {"horodate_creation", New Date(2024, 4, 4)},
            {"libelle", "valeur_4"}}))

        Assert.AreEqual("", b.Commentaire)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode_sous_type", 102L},
            {"horodate_creation", New Date(2024, 4, 4)},
            {"libelle", "valeur_4"}}
        valeurs.Remove("id")
        Dim ignore = New SousEpisodeSousSousType(LigneDeTest.Rangee(Colonnes, valeurs))
    End Sub

End Class
