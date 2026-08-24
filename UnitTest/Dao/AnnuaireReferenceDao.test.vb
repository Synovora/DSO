Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par AnnuaireReferenceDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestAnnuaireReferenceDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "Cle_entree", "type_identifiant_pp", "identifiant_pp", "identifiant_national_pp",
        "code_civilite_exercice", "libelle_civilite_exercice", "code_civilite", "liblle_civilite",
        "nom_exercice", "prenom_exercice", "code_profression", "libelle_profession",
        "code_categorie_professionnelle", "libelle_categorie_professionnelle",
        "code_type_savoir_faire", "libelle_type_savoir_faire", "code_savoir_faire",
        "libellé_savoir_faire", "code_mode_exercice", "libelle_mode_exercice", "numero_siret_site",
        "numero_siren_site", "numero_finess_site", "numero_finess_etablissement_juridique",
        "identifiant_technique_structure", "raison_sociale_site", "enseigne_commerciale_site",
        "complement_destinataire_coord_structure", "complement_point_geographique_coord_structure",
        "numero_voie_coord_structure", "indice_repetition_voie_coord_structure",
        "code_type_voie_coord_structure", "libelle_type_voie_coord_structure",
        "libelle_voie_coord_structure", "mention_distribution_coord_structure",
        "bureau_cedex_coord_structure", "code_postal_coord_structure",
        "code_commune_coord_structure", "libelle_commune_coord_structure",
        "code_pays_coord_structure", "libelle_pays_coord_structure", "telephone_coord_structure",
        "telephone2_coord_structure", "telecopie_coord_structure", "adresse_email_coord_structure",
        "code_departement_structure", "libelle_departement_structure",
        "ancien_identifiant_structure", "autorite_enregistrement", "code_secteur_activite",
        "libelle_secteur_activite", "code_section_tableau_pharmaciens",
        "libelle_section_tableau_pharmaciens"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = AnnuaireReferenceDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"Cle_entree", 101},
            {"type_identifiant_pp", 102},
            {"identifiant_pp", "valeur_3"},
            {"identifiant_national_pp", "valeur_4"},
            {"code_civilite_exercice", "valeur_5"},
            {"libelle_civilite_exercice", "valeur_6"},
            {"code_civilite", "valeur_7"},
            {"liblle_civilite", "valeur_8"},
            {"nom_exercice", "valeur_9"},
            {"prenom_exercice", "valeur_10"},
            {"code_profression", 111},
            {"libelle_profession", "valeur_12"},
            {"code_categorie_professionnelle", "valeur_13"},
            {"libelle_categorie_professionnelle", "valeur_14"},
            {"code_type_savoir_faire", "valeur_15"},
            {"libelle_type_savoir_faire", "valeur_16"},
            {"code_savoir_faire", "valeur_17"},
            {"libellé_savoir_faire", "valeur_18"},
            {"code_mode_exercice", "valeur_19"},
            {"libelle_mode_exercice", "valeur_20"},
            {"numero_siret_site", "valeur_21"},
            {"numero_siren_site", "valeur_22"},
            {"numero_finess_site", "valeur_23"},
            {"numero_finess_etablissement_juridique", "valeur_24"},
            {"identifiant_technique_structure", "valeur_25"},
            {"raison_sociale_site", "valeur_26"},
            {"enseigne_commerciale_site", "valeur_27"},
            {"complement_destinataire_coord_structure", "valeur_28"},
            {"complement_point_geographique_coord_structure", "valeur_29"},
            {"numero_voie_coord_structure", "valeur_30"},
            {"indice_repetition_voie_coord_structure", "valeur_31"},
            {"code_type_voie_coord_structure", "valeur_32"},
            {"libelle_type_voie_coord_structure", "valeur_33"},
            {"libelle_voie_coord_structure", "valeur_34"},
            {"mention_distribution_coord_structure", "valeur_35"},
            {"bureau_cedex_coord_structure", "valeur_36"},
            {"code_postal_coord_structure", "valeur_37"},
            {"code_commune_coord_structure", "valeur_38"},
            {"libelle_commune_coord_structure", "valeur_39"},
            {"code_pays_coord_structure", "valeur_40"},
            {"libelle_pays_coord_structure", "valeur_41"},
            {"telephone_coord_structure", "valeur_42"},
            {"telephone2_coord_structure", "valeur_43"},
            {"telecopie_coord_structure", "valeur_44"},
            {"adresse_email_coord_structure", "valeur_45"},
            {"code_departement_structure", "valeur_46"},
            {"libelle_departement_structure", "valeur_47"},
            {"ancien_identifiant_structure", "valeur_48"},
            {"autorite_enregistrement", "valeur_49"},
            {"code_secteur_activite", "valeur_50"},
            {"libelle_secteur_activite", "valeur_51"},
            {"code_section_tableau_pharmaciens", "valeur_52"},
            {"libelle_section_tableau_pharmaciens", "valeur_53"}}))

        Assert.AreEqual(101, b.Cle_entree)
        Assert.AreEqual(102, b.Typeidentifiant)
        Assert.AreEqual("valeur_3", b.Identifiant)
        Assert.AreEqual("valeur_4", b.IdentifiantNational)
        Assert.AreEqual("valeur_5", b.CodeCiviliteExercice)
        Assert.AreEqual("valeur_6", b.LibelleCiviliteExercice)
        Assert.AreEqual("valeur_7", b.CodeCivilite)
        Assert.AreEqual("valeur_8", b.LibelleCivilite)
        Assert.AreEqual("valeur_9", b.NomExercice)
        Assert.AreEqual("valeur_10", b.PrenomExercice)
        Assert.AreEqual(111, b.CodeProfession)
        Assert.AreEqual("valeur_12", b.LibelleProfession)
        Assert.AreEqual("valeur_13", b.CodeCategorieProfessionnelle)
        Assert.AreEqual("valeur_14", b.LibelleCategorieProfessionnelle)
        Assert.AreEqual("valeur_15", b.CodeTypeSavoirFaire)
        Assert.AreEqual("valeur_16", b.LibelleTypeSavoirFaire)
        Assert.AreEqual("valeur_17", b.CodeSavoirFaire)
        Assert.AreEqual("valeur_18", b.LibelleSavoirFaire)
        Assert.AreEqual("valeur_19", b.CodeModeExercice)
        Assert.AreEqual("valeur_20", b.LibelleModeExercice)
        Assert.AreEqual("valeur_21", b.NumeroSiretSite)
        Assert.AreEqual("valeur_22", b.NumeroSirenSite)
        Assert.AreEqual("valeur_23", b.NumeroFinessSite)
        Assert.AreEqual("valeur_24", b.NumeroFinessEtablissementJuridique)
        Assert.AreEqual("valeur_25", b.IdentifiantTechniqueStructure)
        Assert.AreEqual("valeur_26", b.RaisonSocialeSite)
        Assert.AreEqual("valeur_27", b.EnseigneCommercialeSite)
        Assert.AreEqual("valeur_28", b.ComplementDestinataireCoordonneeStructure)
        Assert.AreEqual("valeur_29", b.ComplementPointGeographiqueCoordonneeStructure)
        Assert.AreEqual("valeur_30", b.NumeroVoieCoordonneeStructure)
        Assert.AreEqual("valeur_31", b.IndiceRepetitionVoieCoordonneeStructure)
        Assert.AreEqual("valeur_32", b.CodeTypeVoieCoordonneeStructure)
        Assert.AreEqual("valeur_33", b.LibelleTypeVoieCoordonneeStructure)
        Assert.AreEqual("valeur_34", b.LibelleVoieCoordonneeStructure)
        Assert.AreEqual("valeur_35", b.MentionDistributionCoordonneeStructure)
        Assert.AreEqual("valeur_36", b.BureauCedexCoordonneeStructure)
        Assert.AreEqual("valeur_37", b.CodePostalCoordonneeStructure)
        Assert.AreEqual("valeur_38", b.CodeCommuneCoordonneeStructure)
        Assert.AreEqual("valeur_39", b.LibelleCommuneCoordonneeStructure)
        Assert.AreEqual("valeur_40", b.CodePaysCoordonneeStructure)
        Assert.AreEqual("valeur_41", b.LibellePaysCoordonneeStructure)
        Assert.AreEqual("valeur_42", b.TelephoneCoordonneeStructure)
        Assert.AreEqual("valeur_43", b.Telephone2CoordonneeStructure)
        Assert.AreEqual("valeur_44", b.TelepcopieCoordonneeStructure)
        Assert.AreEqual("valeur_45", b.emailCoordonneeStructure)
        Assert.AreEqual("valeur_46", b.CodeDepartementStructure)
        Assert.AreEqual("valeur_47", b.LibelleDepartementStructure)
        Assert.AreEqual("valeur_48", b.AncienIdentifiantStructure)
        Assert.AreEqual("valeur_49", b.AutoriteEnregistrement)
        Assert.AreEqual("valeur_50", b.CodeSecteurActivite)
        Assert.AreEqual("valeur_51", b.LibelleSecteurActivite)
        Assert.AreEqual("valeur_52", b.CodeSectionTableauPharmacien)
        Assert.AreEqual("valeur_53", b.LibelleSectionTableauPharmacien)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = AnnuaireReferenceDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"Cle_entree", 101}}))

        Assert.AreEqual(0, b.Typeidentifiant)
        Assert.AreEqual("", b.Identifiant)
        Assert.AreEqual("", b.IdentifiantNational)
        Assert.AreEqual("", b.CodeCiviliteExercice)
        Assert.AreEqual("", b.LibelleCiviliteExercice)
        Assert.AreEqual("", b.CodeCivilite)
        Assert.AreEqual("", b.LibelleCivilite)
        Assert.AreEqual("", b.NomExercice)
        Assert.AreEqual("", b.PrenomExercice)
        Assert.AreEqual(0, b.CodeProfession)
        Assert.AreEqual("", b.LibelleProfession)
        Assert.AreEqual("", b.CodeCategorieProfessionnelle)
        Assert.AreEqual("", b.LibelleCategorieProfessionnelle)
        Assert.AreEqual("", b.CodeTypeSavoirFaire)
        Assert.AreEqual("", b.LibelleTypeSavoirFaire)
        Assert.AreEqual("", b.CodeSavoirFaire)
        Assert.AreEqual("", b.LibelleSavoirFaire)
        Assert.AreEqual("", b.CodeModeExercice)
        Assert.AreEqual("", b.LibelleModeExercice)
        Assert.AreEqual("", b.NumeroSiretSite)
        Assert.AreEqual("", b.NumeroSirenSite)
        Assert.AreEqual("", b.NumeroFinessSite)
        Assert.AreEqual("", b.NumeroFinessEtablissementJuridique)
        Assert.AreEqual("", b.IdentifiantTechniqueStructure)
        Assert.AreEqual("", b.RaisonSocialeSite)
        Assert.AreEqual("", b.EnseigneCommercialeSite)
        Assert.AreEqual("", b.ComplementDestinataireCoordonneeStructure)
        Assert.AreEqual("", b.ComplementPointGeographiqueCoordonneeStructure)
        Assert.AreEqual("", b.NumeroVoieCoordonneeStructure)
        Assert.AreEqual("", b.IndiceRepetitionVoieCoordonneeStructure)
        Assert.AreEqual("", b.CodeTypeVoieCoordonneeStructure)
        Assert.AreEqual("", b.LibelleTypeVoieCoordonneeStructure)
        Assert.AreEqual("", b.LibelleVoieCoordonneeStructure)
        Assert.AreEqual("", b.MentionDistributionCoordonneeStructure)
        Assert.AreEqual("", b.BureauCedexCoordonneeStructure)
        Assert.AreEqual("", b.CodePostalCoordonneeStructure)
        Assert.AreEqual("", b.CodeCommuneCoordonneeStructure)
        Assert.AreEqual("", b.LibelleCommuneCoordonneeStructure)
        Assert.AreEqual("", b.CodePaysCoordonneeStructure)
        Assert.AreEqual("", b.LibellePaysCoordonneeStructure)
        Assert.AreEqual("", b.TelephoneCoordonneeStructure)
        Assert.AreEqual("", b.Telephone2CoordonneeStructure)
        Assert.AreEqual("", b.TelepcopieCoordonneeStructure)
        Assert.AreEqual("", b.emailCoordonneeStructure)
        Assert.AreEqual("", b.CodeDepartementStructure)
        Assert.AreEqual("", b.LibelleDepartementStructure)
        Assert.AreEqual("", b.AncienIdentifiantStructure)
        Assert.AreEqual("", b.AutoriteEnregistrement)
        Assert.AreEqual("", b.CodeSecteurActivite)
        Assert.AreEqual("", b.LibelleSecteurActivite)
        Assert.AreEqual("", b.CodeSectionTableauPharmacien)
        Assert.AreEqual("", b.LibelleSectionTableauPharmacien)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansCle_entreeEstUneErreur()
        ' Cle_entree n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"Cle_entree", 101}}
        valeurs.Remove("Cle_entree")
        AnnuaireReferenceDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
