Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par New Antecedent. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' Le DAO se contente de New Antecedent(reader) : c'est le constructeur qui lit.
''' </summary>
<TestClass()> Public Class TestAntecedentLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_antecedent_id", "oa_antecedent_patient_id", "oa_antecedent_type",
        "oa_antecedent_drc_id", "oa_antecedent_description", "oa_antecedent_date_creation",
        "oa_antecedent_utilisateur_creation", "oa_antecedent_date_modification",
        "oa_antecedent_utilisateur_modification", "oa_antecedent_diagnostic",
        "oa_antecedent_date_debut", "oa_antecedent_date_fin", "oa_antecedent_ald_id",
        "oa_antecedent_ald_cim_10_id", "oa_antecedent_ald_valide", "oa_antecedent_ald_date_debut",
        "oa_antecedent_ald_date_fin", "oa_antecedent_ald_demande_en_cours",
        "oa_antecedent_ald_demande_date", "oa_antecedent_arret", "oa_antecedent_arret_commentaire",
        "oa_antecedent_nature", "oa_antecedent_priorite", "oa_antecedent_niveau",
        "oa_antecedent_id_niveau1", "oa_antecedent_id_niveau2", "oa_antecedent_ordre_affichage1",
        "oa_antecedent_ordre_affichage2", "oa_antecedent_ordre_affichage3",
        "oa_antecedent_statut_affichage", "oa_antecedent_statut_affichage_transformation",
        "oa_antecedent_categorie_contexte", "oa_episode_id", "oa_antecedent_inactif",
        "oa_chaine_episode_date_fin"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = New Antecedent(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_antecedent_id", 101},
            {"oa_antecedent_patient_id", 102},
            {"oa_antecedent_type", "valeur_3"},
            {"oa_antecedent_drc_id", 104},
            {"oa_antecedent_description", "valeur_5"},
            {"oa_antecedent_date_creation", New Date(2024, 7, 7)},
            {"oa_antecedent_utilisateur_creation", 107},
            {"oa_antecedent_date_modification", New Date(2024, 9, 9)},
            {"oa_antecedent_utilisateur_modification", 109},
            {"oa_antecedent_diagnostic", 110},
            {"oa_antecedent_date_debut", New Date(2024, 12, 12)},
            {"oa_antecedent_date_fin", New Date(2024, 1, 13)},
            {"oa_antecedent_ald_id", 113},
            {"oa_antecedent_ald_cim_10_id", 114},
            {"oa_antecedent_ald_valide", True},
            {"oa_antecedent_ald_date_debut", New Date(2024, 5, 17)},
            {"oa_antecedent_ald_date_fin", New Date(2024, 6, 18)},
            {"oa_antecedent_ald_demande_en_cours", True},
            {"oa_antecedent_ald_demande_date", New Date(2024, 8, 20)},
            {"oa_antecedent_arret", True},
            {"oa_antecedent_arret_commentaire", "valeur_21"},
            {"oa_antecedent_nature", "valeur_22"},
            {"oa_antecedent_priorite", "valeur_23"},
            {"oa_antecedent_niveau", 124},
            {"oa_antecedent_id_niveau1", 125},
            {"oa_antecedent_id_niveau2", 126},
            {"oa_antecedent_ordre_affichage1", 127},
            {"oa_antecedent_ordre_affichage2", 128},
            {"oa_antecedent_ordre_affichage3", 129},
            {"oa_antecedent_statut_affichage", "valeur_30"},
            {"oa_antecedent_statut_affichage_transformation", "valeur_31"},
            {"oa_antecedent_categorie_contexte", "valeur_32"},
            {"oa_episode_id", 133L},
            {"oa_antecedent_inactif", True},
            {"oa_chaine_episode_date_fin", New Date(2024, 12, 8)}}))

        Assert.AreEqual(101, b.Id)
        Assert.AreEqual(102, b.PatientId)
        Assert.AreEqual("valeur_3", b.[Type])
        Assert.AreEqual(104, b.DrcId)
        Assert.AreEqual("valeur_5", b.Description)
        Assert.AreEqual(New Date(2024, 7, 7), b.DateCreation)
        Assert.AreEqual(107, b.UserCreation)
        Assert.AreEqual(New Date(2024, 9, 9), b.DateModification)
        Assert.AreEqual(109, b.UserModification)
        Assert.AreEqual(110, b.Diagnostic)
        Assert.AreEqual(New Date(2024, 12, 12), b.DateDebut)
        Assert.AreEqual(New Date(2024, 1, 13), b.DateFin)
        Assert.AreEqual(113, b.AldId)
        Assert.AreEqual(114, b.AldCim10Id)
        Assert.AreEqual(True, b.AldValide)
        Assert.AreEqual(New Date(2024, 5, 17), b.AldDateDebut)
        Assert.AreEqual(New Date(2024, 6, 18), b.AldDateFin)
        Assert.AreEqual(True, b.AldDemandeEnCours)
        Assert.AreEqual(New Date(2024, 8, 20), b.AldDateDemande)
        Assert.AreEqual(True, b.Arret)
        Assert.AreEqual("valeur_21", b.ArretCommentaire)
        Assert.AreEqual("valeur_22", b.Nature)
        Assert.AreEqual("valeur_23", b.Priorite)
        Assert.AreEqual(124, b.Niveau)
        Assert.AreEqual(125, b.Niveau1Id)
        Assert.AreEqual(126, b.Niveau2Id)
        Assert.AreEqual(127, b.Ordre1)
        Assert.AreEqual(128, b.Ordre2)
        Assert.AreEqual(129, b.Ordre3)
        Assert.AreEqual("valeur_30", b.StatutAffichage)
        Assert.AreEqual("valeur_31", b.StatutAffichageTransformation)
        Assert.AreEqual("valeur_32", b.CategorieContexte)
        Assert.AreEqual(133L, b.EpisodeId)
        Assert.AreEqual(True, b.Inactif)
        Assert.AreEqual(New Date(2024, 12, 8), b.ChaineEpisodeDateFin)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = New Antecedent(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_antecedent_id", 101},
            {"oa_antecedent_patient_id", 102},
            {"oa_antecedent_type", "valeur_3"},
            {"oa_antecedent_drc_id", 104}}))

        Assert.IsNull(b.Description)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
        Assert.AreEqual(0, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateModification)
        Assert.AreEqual(0, b.UserModification)
        Assert.AreEqual(0, b.Diagnostic)
        Assert.AreEqual(Date.MinValue, b.DateDebut)
        Assert.AreEqual(Date.MinValue, b.DateFin)
        Assert.AreEqual(0, b.AldId)
        Assert.AreEqual(0, b.AldCim10Id)
        Assert.AreEqual(False, b.AldValide)
        Assert.AreEqual(Date.MinValue, b.AldDateDebut)
        Assert.AreEqual(Date.MinValue, b.AldDateFin)
        Assert.AreEqual(False, b.AldDemandeEnCours)
        Assert.AreEqual(Date.MinValue, b.AldDateDemande)
        Assert.AreEqual(False, b.Arret)
        Assert.IsNull(b.ArretCommentaire)
        Assert.IsNull(b.Nature)
        Assert.IsNull(b.Priorite)
        Assert.AreEqual(0, b.Niveau)
        Assert.AreEqual(0, b.Niveau1Id)
        Assert.AreEqual(0, b.Niveau2Id)
        Assert.AreEqual(0, b.Ordre1)
        Assert.AreEqual(0, b.Ordre2)
        Assert.AreEqual(0, b.Ordre3)
        Assert.IsNull(b.StatutAffichage)
        Assert.IsNull(b.StatutAffichageTransformation)
        Assert.IsNull(b.CategorieContexte)
        Assert.AreEqual(0L, b.EpisodeId)
        Assert.AreEqual(False, b.Inactif)
        Assert.AreEqual(Date.MinValue, b.ChaineEpisodeDateFin)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' oa_antecedent_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_antecedent_id", 101},
            {"oa_antecedent_patient_id", 102},
            {"oa_antecedent_type", "valeur_3"},
            {"oa_antecedent_drc_id", 104}}
        valeurs.Remove("oa_antecedent_id")
        Dim ignore = New Antecedent(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
