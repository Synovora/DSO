Imports System.Data.SqlClient

Public Class AnnuaireProfessionnelDao

    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As AnnuaireProfessionnel
        Dim AnnuaireProfessionnel As New AnnuaireProfessionnel With {
            .Cle_entree = reader("Cle_entree"),
            .Typeidentifiant = Coalesce(reader("type_identifiant_pp"), 0),
            .Identifiant = Coalesce(reader("identifiant_pp"), ""),
            .IdentifiantNational = Coalesce(reader("identifiant_national_pp"), ""),
            .CodeCiviliteExercice = Coalesce(reader("code_civilite_exercice"), ""),
            .LibelleCiviliteExercice = Coalesce(reader("libelle_civilite_exercice"), ""),
            .CodeCivilite = Coalesce(reader("code_civilite"), ""),
            .LibelleCivilite = Coalesce(reader("liblle_civilite"), ""),
            .NomExercice = Coalesce(reader("nom_exercice"), ""),
            .PrenomExercice = Coalesce(reader("prenom_exercice"), ""),
            .CodeProfession = Coalesce(reader("code_profression"), 0),
            .LibelleProfession = Coalesce(reader("libelle_profession"), ""),
            .CodeCategorieProfessionnelle = Coalesce(reader("code_categorie_professionnelle"), ""),
            .LibelleCategorieProfessionnelle = Coalesce(reader("libelle_categorie_professionnelle"), ""),
            .CodeTypeSavoirFaire = Coalesce(reader("code_type_savoir_faire"), ""),
            .LibelleTypeSavoirFaire = Coalesce(reader("libelle_type_savoir_faire"), ""),
            .CodeSavoirFaire = Coalesce(reader("code_savoir_faire"), ""),
            .LibelleSavoirFaire = Coalesce(reader("libellé_savoir_faire"), ""),
            .CodeModeExercice = Coalesce(reader("code_mode_exercice"), ""),
            .LibelleModeExercice = Coalesce(reader("libelle_mode_exercice"), ""),
            .NumeroSiretSite = Coalesce(reader("numero_siret_site"), ""),
            .NumeroSirenSite = Coalesce(reader("numero_siren_site"), ""),
            .NumeroFinessSite = Coalesce(reader("numero_finess_site"), ""),
            .NumeroFinessEtablissementJuridique = Coalesce(reader("numero_finess_etablissement_juridique"), ""),
            .IdentifiantTechniqueStructure = Coalesce(reader("identifiant_technique_structure"), ""),
            .RaisonSocialeSite = Coalesce(reader("raison_sociale_site"), ""),
            .EnseigneCommercialeSite = Coalesce(reader("enseigne_commerciale_site"), ""),
            .ComplementDestinataireCoordonneeStructure = Coalesce(reader("complement_destinataire_coord_structure"), ""),
            .ComplementPointGeographiqueCoordonneeStructure = Coalesce(reader("complement_point_geographique_coord_structure"), ""),
            .NumeroVoieCoordonneeStructure = Coalesce(reader("numero_voie_coord_structure"), ""),
            .IndiceRepetitionVoieCoordonneeStructure = Coalesce(reader("indice_repetition_voie_coord_structure"), ""),
            .CodeTypeVoieCoordonneeStructure = Coalesce(reader("code_type_voie_coord_structure"), ""),
            .LibelleTypeVoieCoordonneeStructure = Coalesce(reader("libelle_type_voie_coord_structure"), ""),
            .LibelleVoieCoordonneeStructure = Coalesce(reader("libelle_voie_coord_structure"), ""),
            .MentionDistributionCoordonneeStructure = Coalesce(reader("mention_distribution_coord_structure"), ""),
            .BureauCedexCoordonneeStructure = Coalesce(reader("bureau_cedex_coord_structure"), ""),
            .CodePostalCoordonneeStructure = Coalesce(reader("code_postal_coord_structure"), ""),
            .CodeCommuneCoordonneeStructure = Coalesce(reader("code_commune_coord_structure"), ""),
            .LibelleCommuneCoordonneeStructure = Coalesce(reader("libelle_commune_coord_structure"), ""),
            .CodePaysCoordonneeStructure = Coalesce(reader("code_pays_coord_structure"), ""),
            .LibellePaysCoordonneeStructure = Coalesce(reader("libelle_pays_coord_structure"), ""),
            .TelephoneCoordonneeStructure = Coalesce(reader("telephone_coord_structure"), ""),
            .Telephone2CoordonneeStructure = Coalesce(reader("telephone2_coord_structure"), ""),
            .TelepcopieCoordonneeStructure = Coalesce(reader("telecopie_coord_structure"), ""),
            .emailCoordonneeStructure = Coalesce(reader("adresse_email_coord_structure"), ""),
            .CodeDepartementStructure = Coalesce(reader("code_departement_structure"), ""),
            .LibelleDepartementStructure = Coalesce(reader("libelle_departement_structure"), ""),
            .AncienIdentifiantStructure = Coalesce(reader("ancien_identifiant_structure"), ""),
            .AutoriteEnregistrement = Coalesce(reader("autorite_enregistrement"), ""),
            .CodeSecteurActivite = Coalesce(reader("code_secteur_activite"), ""),
            .LibelleSecteurActivite = Coalesce(reader("libelle_secteur_activite"), ""),
            .CodeSectionTableauPharmacien = Coalesce(reader("code_section_tableau_pharmaciens"), ""),
            .LibelleSectionTableauPharmacien = Coalesce(reader("libelle_section_tableau_pharmaciens"), "")
        }
        Return AnnuaireProfessionnel
    End Function

    Public Function GetAnnuaireProfessionnelById(annuaireProfessionneld As Integer) As AnnuaireProfessionnel
        Dim annuaireProfessionnel As AnnuaireProfessionnel
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.ans_annuaire_professionnel_sante WHERE Cle_entree = @id"
            command.Parameters.AddWithValue("@id", annuaireProfessionneld)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    annuaireProfessionnel = BuildBean(reader)
                Else
                    Throw New ArgumentException("Professionnel de santé inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return annuaireProfessionnel
    End Function

End Class
