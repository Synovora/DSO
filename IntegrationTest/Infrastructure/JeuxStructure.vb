Imports Oasis_Common

''' <summary>
''' Structures (sièges, unités sanitaires, sites) et annuaire professionnel de test.
'''
''' Aucun DAO n'écrit dans oa_siege, oa_unite_sanitaire ni oa_site : ces tables
''' sont tenues à la main en production. L'annuaire national
''' (ans_annuaire_professionnel_sante), ses boîtes aux lettres et le complément
''' sont importés par le paquet SSIS. Tout cela passe donc par du SQL brut, avec
''' les colonnes que lisent les BuildBean des DAO. Seule la table de référence
''' de l'annuaire a un INSERT applicatif (AnnuaireReferenceDao.CreationAnnuaireReference),
''' que les tests utilisent directement.
'''
''' Ces tables ne sont lues par aucun singleton d'EnvironnementBase : les tests
''' peuvent y créer leurs lignes.
''' </summary>
Public Module JeuxStructure

    ''' <summary>
    ''' Crée un siège et renvoie son id. statut à Nothing donne NULL (siège vu
    ''' comme actif par SiegeDao) ; "A" actif, "I" ou toute autre valeur inactif.
    ''' </summary>
    Function CreerSiege(description As String,
                        Optional statut As String = "A",
                        Optional ville As String = "Mamoudzou") As Long
        Return InsererLigneStructure("oasis.oa_siege", "oa_siege_id",
            "oa_siege_description, oa_siege_adresse1, oa_siege_adresse2, oa_siege_ville, oa_siege_code_postal," &
            " oa_siege_telephone, oa_siege_mail, oa_siege_fax, oa_siege_statut",
            description, "1 place du Siege", "BP 1", ville, "97600",
            "0269000001", "siege@exemple.fr", "0269000002", statut)
    End Function

    ''' <summary>
    ''' Crée une unité sanitaire rattachée à un siège (0 donne NULL) et renvoie son id.
    ''' inactif à Nothing donne NULL.
    ''' </summary>
    Function CreerUniteSanitaire(description As String,
                                 Optional siegeId As Long = 0,
                                 Optional inactif As Boolean? = False,
                                 Optional numeroStructure As Long = 4242) As Long
        Return InsererLigneStructure("oasis.oa_unite_sanitaire", "oa_unite_sanitaire_id",
            "oa_unite_sanitaire_description, oa_unite_sanitaire_siege_id, oa_unite_sanitaire_adresse1," &
            " oa_unite_sanitaire_adresse2, oa_unite_sanitaire_ville, oa_unite_sanitaire_code_postal," &
            " telephone, mail, fax, oa_unite_sanitaire_inactif, numero_structure",
            description, If(siegeId = 0, Nothing, CObj(siegeId)), "2 rue de l'Unite",
            "Batiment B", "Dzaoudzi", "97615",
            "0269000011", "unite@exemple.fr", "0269000012",
            If(inactif.HasValue, CObj(inactif.Value), Nothing), numeroStructure)
    End Function

    ''' <summary>
    ''' Crée un site rattaché à une unité sanitaire (0 donne NULL) et renvoie son id.
    ''' inactif à Nothing donne NULL ; territoireId à 0 donne NULL.
    ''' </summary>
    Function CreerSite(description As String,
                       Optional uniteSanitaireId As Long = 0,
                       Optional inactif As Boolean? = False,
                       Optional territoireId As Integer = 0) As Long
        Return InsererLigneStructure("oasis.oa_site", "oa_site_id",
            "oa_site_description, oa_site_territoire_id, oa_site_unite_sanitaire_id, oa_site_adresse1," &
            " oa_site_adresse2, oa_site_ville, oa_site_code_postal, telephone, mail, fax, oa_site_inactif",
            description, If(territoireId = 0, Nothing, CObj(territoireId)),
            If(uniteSanitaireId = 0, Nothing, CObj(uniteSanitaireId)),
            "3 chemin du Site", "Lieu-dit Test", "Sada", "97640",
            "0269000021", "site@exemple.fr", "0269000022",
            If(inactif.HasValue, CObj(inactif.Value), Nothing))
    End Function

    ''' <summary>
    ''' Colonnes de l'annuaire, dans l'ordre de l'INSERT de
    ''' AnnuaireReferenceDao.CreationAnnuaireReference. La table nationale et la table
    ''' de référence ont la même forme : les deux BuildBean lisent les mêmes colonnes.
    ''' </summary>
    Private ReadOnly ColonnesAnnuaire As String =
        "type_identifiant_pp, identifiant_pp, identifiant_national_pp, code_civilite_exercice," &
        " libelle_civilite_exercice, code_civilite, liblle_civilite, nom_exercice, prenom_exercice," &
        " code_profression, libelle_profession, code_categorie_professionnelle, libelle_categorie_professionnelle, code_type_savoir_faire," &
        " libelle_type_savoir_faire, code_savoir_faire," &
        " libellé_savoir_faire, code_mode_exercice, libelle_mode_exercice, numero_siret_site," &
        " numero_siren_site, numero_finess_site, numero_finess_etablissement_juridique, identifiant_technique_structure," &
        " raison_sociale_site, enseigne_commerciale_site, complement_destinataire_coord_structure, complement_point_geographique_coord_structure," &
        " numero_voie_coord_structure, indice_repetition_voie_coord_structure, code_type_voie_coord_structure, libelle_type_voie_coord_structure," &
        " libelle_voie_coord_structure, mention_distribution_coord_structure, bureau_cedex_coord_structure, code_postal_coord_structure," &
        " code_commune_coord_structure, libelle_commune_coord_structure, code_pays_coord_structure, libelle_pays_coord_structure," &
        " telephone_coord_structure, telephone2_coord_structure, telecopie_coord_structure, adresse_email_coord_structure," &
        " code_departement_structure, libelle_departement_structure, ancien_identifiant_structure, autorite_enregistrement," &
        " code_secteur_activite, libelle_secteur_activite, code_section_tableau_pharmaciens, libelle_section_tableau_pharmaciens"

    ''' <summary>
    ''' Fiche d'annuaire entièrement renseignée (aucune propriété à Nothing), comme
    ''' la renvoie GetAnnuaireProfessionnelById. Rien n'est écrit en base.
    ''' </summary>
    Function ProfessionnelDeTest(nom As String,
                                 Optional identifiantNational As String = "810001234567",
                                 Optional commune As String = "MAMOUDZOU",
                                 Optional codePostal As String = "97600",
                                 Optional codeProfession As Integer = 10,
                                 Optional codeSavoirFaire As String = "SM54",
                                 Optional codeStructure As String = "S0001",
                                 Optional raisonSociale As String = "CABINET DE TEST") As AnnuaireProfessionnel
        Return New AnnuaireProfessionnel With {
            .Typeidentifiant = 8,
            .Identifiant = identifiantNational.Substring(1),
            .IdentifiantNational = identifiantNational,
            .CodeCiviliteExercice = "DR",
            .LibelleCiviliteExercice = "Docteur",
            .CodeCivilite = "M",
            .LibelleCivilite = "Monsieur",
            .NomExercice = nom,
            .PrenomExercice = "Jean",
            .CodeProfession = codeProfession,
            .LibelleProfession = "Medecin",
            .CodeCategorieProfessionnelle = "C",
            .LibelleCategorieProfessionnelle = "Civil",
            .CodeTypeSavoirFaire = "S",
            .LibelleTypeSavoirFaire = "Specialite ordinale",
            .CodeSavoirFaire = codeSavoirFaire,
            .LibelleSavoirFaire = "Medecine generale",
            .CodeModeExercice = "L",
            .LibelleModeExercice = "Liberal",
            .NumeroSiretSite = "12345678900011",
            .NumeroSirenSite = "123456789",
            .NumeroFinessSite = "970000001",
            .NumeroFinessEtablissementJuridique = "970000002",
            .IdentifiantTechniqueStructure = codeStructure,
            .RaisonSocialeSite = raisonSociale,
            .EnseigneCommercialeSite = "Enseigne",
            .ComplementDestinataireCoordonneeStructure = "Destinataire",
            .ComplementPointGeographiqueCoordonneeStructure = "Residence Les Palmiers",
            .NumeroVoieCoordonneeStructure = "12",
            .IndiceRepetitionVoieCoordonneeStructure = "B",
            .CodeTypeVoieCoordonneeStructure = "R",
            .LibelleTypeVoieCoordonneeStructure = "Rue",
            .LibelleVoieCoordonneeStructure = "des Tests",
            .MentionDistributionCoordonneeStructure = "Mention",
            .BureauCedexCoordonneeStructure = codePostal & " " & commune,
            .CodePostalCoordonneeStructure = codePostal,
            .CodeCommuneCoordonneeStructure = "97611",
            .LibelleCommuneCoordonneeStructure = commune,
            .CodePaysCoordonneeStructure = "FR",
            .LibellePaysCoordonneeStructure = "France",
            .TelephoneCoordonneeStructure = "0269000031",
            .Telephone2CoordonneeStructure = "0269000032",
            .TelepcopieCoordonneeStructure = "0269000033",
            .emailCoordonneeStructure = "cabinet@exemple.fr",
            .CodeDepartementStructure = "976",
            .LibelleDepartementStructure = "Mayotte",
            .AncienIdentifiantStructure = "A0001",
            .AutoriteEnregistrement = "CNOM",
            .CodeSecteurActivite = "SA01",
            .LibelleSecteurActivite = "Cabinet individuel",
            .CodeSectionTableauPharmacien = "",
            .LibelleSectionTableauPharmacien = ""
        }
    End Function

    ''' <summary>
    ''' Écrit une fiche dans l'annuaire national importé (ans_annuaire_professionnel_sante),
    ''' comme le ferait l'import SSIS. Renvoie sa clé (Cle_entree).
    ''' </summary>
    Function CreerProfessionnelNational(fiche As AnnuaireProfessionnel) As Long
        Return InsererLigneStructure("oasis.ans_annuaire_professionnel_sante", "Cle_entree", ColonnesAnnuaire,
            fiche.Typeidentifiant, fiche.Identifiant, fiche.IdentifiantNational, fiche.CodeCiviliteExercice,
            fiche.LibelleCiviliteExercice, fiche.CodeCivilite, fiche.LibelleCivilite, fiche.NomExercice, fiche.PrenomExercice,
            fiche.CodeProfession, fiche.LibelleProfession, fiche.CodeCategorieProfessionnelle, fiche.LibelleCategorieProfessionnelle, fiche.CodeTypeSavoirFaire,
            fiche.LibelleTypeSavoirFaire, fiche.CodeSavoirFaire,
            fiche.LibelleSavoirFaire, fiche.CodeModeExercice, fiche.LibelleModeExercice, fiche.NumeroSiretSite,
            fiche.NumeroSirenSite, fiche.NumeroFinessSite, fiche.NumeroFinessEtablissementJuridique, fiche.IdentifiantTechniqueStructure,
            fiche.RaisonSocialeSite, fiche.EnseigneCommercialeSite, fiche.ComplementDestinataireCoordonneeStructure, fiche.ComplementPointGeographiqueCoordonneeStructure,
            fiche.NumeroVoieCoordonneeStructure, fiche.IndiceRepetitionVoieCoordonneeStructure, fiche.CodeTypeVoieCoordonneeStructure, fiche.LibelleTypeVoieCoordonneeStructure,
            fiche.LibelleVoieCoordonneeStructure, fiche.MentionDistributionCoordonneeStructure, fiche.BureauCedexCoordonneeStructure, fiche.CodePostalCoordonneeStructure,
            fiche.CodeCommuneCoordonneeStructure, fiche.LibelleCommuneCoordonneeStructure, fiche.CodePaysCoordonneeStructure, fiche.LibellePaysCoordonneeStructure,
            fiche.TelephoneCoordonneeStructure, fiche.Telephone2CoordonneeStructure, fiche.TelepcopieCoordonneeStructure, fiche.emailCoordonneeStructure,
            fiche.CodeDepartementStructure, fiche.LibelleDepartementStructure, fiche.AncienIdentifiantStructure, fiche.AutoriteEnregistrement,
            fiche.CodeSecteurActivite, fiche.LibelleSecteurActivite, fiche.CodeSectionTableauPharmacien, fiche.LibelleSectionTableauPharmacien)
    End Function

    ''' <summary>
    ''' Boîte aux lettres de messagerie sécurisée d'un professionnel
    ''' (ans_annuaire_professionnel_sante_bal), telle que l'import la dépose.
    ''' typeBal : AnnuaireProfessionnelBalDao.EnumTypeBal.PERSONNELLE ou ORGANISATION.
    ''' </summary>
    Sub CreerBalAnnuaire(identifiantNational As String, adresse As String,
                         Optional typeBal As String = "PER",
                         Optional raisonSociale As String = "CABINET DE TEST")
        Executer("INSERT INTO oasis.ans_annuaire_professionnel_sante_bal" &
                 " (identifiant_national_pp, type_bal, adresse_bal, raison_sociale_structure)" &
                 " VALUES (@p0, @p1, @p2, @p3)",
                 identifiantNational, typeBal, adresse, raisonSociale)
    End Sub

    ''' <summary>
    ''' Complément saisi pour une entrée de l'annuaire de référence
    ''' (ans_annuaire_professionnel_sante_reference_complement), sous la clé donnée.
    ''' Colonnes reprises d'AnnuaireProfessionnelSanteComplementDao.BuildBean.
    ''' </summary>
    Sub CreerComplementAnnuaire(cleEntree As Long, raisonSociale As String,
                                Optional adresse2 As String = "Complement d'adresse")
        Dim sql =
            "IF COLUMNPROPERTY(OBJECT_ID('oasis.ans_annuaire_professionnel_sante_reference_complement'), 'Cle_entree', 'IsIdentity') = 1" & vbCrLf &
            "    SET IDENTITY_INSERT oasis.ans_annuaire_professionnel_sante_reference_complement ON;" & vbCrLf &
            "INSERT INTO oasis.ans_annuaire_professionnel_sante_reference_complement" &
            " (Cle_entree, raison_sociale, adresse1, adresse2, telephone, telecopie, email_structure)" &
            " VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6);" & vbCrLf &
            "IF COLUMNPROPERTY(OBJECT_ID('oasis.ans_annuaire_professionnel_sante_reference_complement'), 'Cle_entree', 'IsIdentity') = 1" & vbCrLf &
            "    SET IDENTITY_INSERT oasis.ans_annuaire_professionnel_sante_reference_complement OFF;"
        Executer(sql, cleEntree, raisonSociale, "4 allee du Complement", adresse2,
                 "0269000041", "0269000042", "complement@exemple.fr")
    End Sub

    ''' <summary>
    ''' INSERT qui fonctionne que la clé soit une identité ou non, le schéma n'étant
    ''' pas encore connu. Les valeurs deviennent @p0, @p1... dans l'ordre des colonnes.
    ''' Renvoie l'id de la ligne créée.
    ''' </summary>
    Private Function InsererLigneStructure(table As String, colonneId As String, colonnes As String,
                                           ParamArray valeurs() As Object) As Long
        Dim marques = String.Join(", ", Enumerable.Range(0, valeurs.Length).Select(Function(i) "@p" & i))
        Dim sql =
            "DECLARE @id BIGINT;" & vbCrLf &
            "IF COLUMNPROPERTY(OBJECT_ID('" & table & "'), '" & colonneId & "', 'IsIdentity') = 1" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    INSERT INTO " & table & " (" & colonnes & ") VALUES (" & marques & ");" & vbCrLf &
            "    SET @id = SCOPE_IDENTITY();" & vbCrLf &
            "END" & vbCrLf &
            "ELSE" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    SELECT @id = COALESCE(MAX(" & colonneId & "), 0) + 1 FROM " & table & ";" & vbCrLf &
            "    INSERT INTO " & table & " (" & colonneId & ", " & colonnes & ") VALUES (@id, " & marques & ");" & vbCrLf &
            "END" & vbCrLf &
            "SELECT @id;"
        Return CLng(Scalaire(sql, valeurs))
    End Function

End Module
