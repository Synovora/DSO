Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne de l'annuaire professionnel par
''' AnnuaireProfessionnelDao.BuildBean. Les noms de colonnes sont ceux de
''' l'import ANS, fautes d'orthographe comprises (liblle_civilite,
''' code_profression) : la liste ci-dessous est extraite du DAO et doit lui
''' rester identique.
''' </summary>
<TestClass()> Public Class TestAnnuaireProfessionnelDaoLecture

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

    <TestMethod()> Public Sub LesChampsDIdentiteEtDeContactSontLus()
        Dim a = AnnuaireProfessionnelDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"Cle_entree", 12345}, {"type_identifiant_pp", 8}, {"identifiant_pp", "10001234567"},
            {"identifiant_national_pp", "810001234567"}, {"code_civilite_exercice", "DR"},
            {"liblle_civilite", "Madame"}, {"nom_exercice", "DUPONT"}, {"prenom_exercice", "MARIE"},
            {"code_profression", 10}, {"libelle_profession", "Médecin"},
            {"libellé_savoir_faire", "Médecine générale"}, {"raison_sociale_site", "Cabinet Dupont"},
            {"code_postal_coord_structure", "75011"}, {"libelle_commune_coord_structure", "Paris"},
            {"telephone_coord_structure", "0102030405"}, {"adresse_email_coord_structure", "marie.dupont@exemple.fr"}}))

        Assert.AreEqual(12345, a.Cle_entree)
        Assert.AreEqual(8, a.Typeidentifiant)
        Assert.AreEqual("10001234567", a.Identifiant)
        Assert.AreEqual("810001234567", a.IdentifiantNational)
        Assert.AreEqual("DR", a.CodeCiviliteExercice)
        Assert.AreEqual("Madame", a.LibelleCivilite)
        Assert.AreEqual("DUPONT", a.NomExercice)
        Assert.AreEqual("MARIE", a.PrenomExercice)
        Assert.AreEqual(10, a.CodeProfession)
        Assert.AreEqual("Médecin", a.LibelleProfession)
        Assert.AreEqual("Médecine générale", a.LibelleSavoirFaire)
        Assert.AreEqual("Cabinet Dupont", a.RaisonSocialeSite)
        Assert.AreEqual("75011", a.CodePostalCoordonneeStructure)
        Assert.AreEqual("Paris", a.LibelleCommuneCoordonneeStructure)
        Assert.AreEqual("0102030405", a.TelephoneCoordonneeStructure)
        Assert.AreEqual("marie.dupont@exemple.fr", a.emailCoordonneeStructure)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim a = AnnuaireProfessionnelDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {{"Cle_entree", 1}}))

        Assert.AreEqual(1, a.Cle_entree)
        Assert.AreEqual(0, a.Typeidentifiant)
        Assert.AreEqual(0, a.CodeProfession)
        For Each texte In {a.Identifiant, a.IdentifiantNational, a.CodeCiviliteExercice, a.LibelleCivilite,
                           a.NomExercice, a.PrenomExercice, a.LibelleProfession, a.LibelleSavoirFaire,
                           a.RaisonSocialeSite, a.CodePostalCoordonneeStructure, a.LibelleCommuneCoordonneeStructure,
                           a.TelephoneCoordonneeStructure, a.emailCoordonneeStructure, a.NumeroFinessSite,
                           a.CodeSecteurActivite}
            Assert.AreEqual("", texte)
        Next
    End Sub

    <TestMethod()> Public Sub AucuneProprieteTexteNeResteNothing()
        ' Les écrans concatènent ces champs pour composer une adresse ou un
        ' libellé : Nothing y devient une NullReferenceException.
        Dim a = AnnuaireProfessionnelDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {{"Cle_entree", 1}}))
        For Each prop In GetType(AnnuaireProfessionnel).GetProperties()
            If prop.PropertyType Is GetType(String) Then
                Assert.IsNotNull(prop.GetValue(a), prop.Name)
            End If
        Next
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansCleEstUneErreur()
        AnnuaireProfessionnelDao.BuildBean(LigneDeTest.Ligne(Colonnes, Nothing))
    End Sub

End Class
