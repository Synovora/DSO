Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New SousEpisodeSousType. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New SousEpisodeSousType(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestSousEpisodeSousTypeLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "id_sous_episode_type", "horodate_creation", "libelle", "validation_profil_types",
        "redaction_profil_types", "is_ald_possible", "is_reponse_requise", "delai_reponse",
        "commentaire"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New SousEpisodeSousType(LigneDeTest.Rangee(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode_type", 102L},
            {"horodate_creation", New Date(2024, 4, 4)},
            {"libelle", "valeur_4"},
            {"validation_profil_types", "valeur_5"},
            {"redaction_profil_types", "valeur_6"},
            {"is_ald_possible", True},
            {"is_reponse_requise", True},
            {"delai_reponse", 109},
            {"commentaire", "valeur_10"}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.IdSousEpisodeType)
        Assert.AreEqual(New Date(2024, 4, 4), b.HorodateCreation)
        Assert.AreEqual("valeur_4", b.Libelle)
        Assert.AreEqual("valeur_5", b.ValidationProfilTypes)
        Assert.AreEqual("valeur_6", b.RedactionProfilTypes)
        Assert.AreEqual(True, b.IsALDPossible)
        Assert.AreEqual(True, b.IsReponseRequise)
        Assert.AreEqual(109, b.DelaiReponse)
        Assert.AreEqual("valeur_10", b.Commentaire)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = New SousEpisodeSousType(LigneDeTest.Rangee(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode_type", 102L},
            {"horodate_creation", New Date(2024, 4, 4)},
            {"libelle", "valeur_4"},
            {"validation_profil_types", "valeur_5"},
            {"redaction_profil_types", "valeur_6"},
            {"is_ald_possible", True}}))

        Assert.AreEqual(False, b.IsReponseRequise)
        ' Propriété String qui tient lieu de booléen : les appelants la comparent à True,
        ' ce qui fonctionne avec "False" et lèverait avec "". On fige donc "False".
        Assert.AreEqual("False", b.Commentaire)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"id_sous_episode_type", 102L},
            {"horodate_creation", New Date(2024, 4, 4)},
            {"libelle", "valeur_4"},
            {"validation_profil_types", "valeur_5"},
            {"redaction_profil_types", "valeur_6"},
            {"is_ald_possible", True}}
        valeurs.Remove("id")
        Dim ignore = New SousEpisodeSousType(LigneDeTest.Rangee(Colonnes, valeurs))
    End Sub

End Class
