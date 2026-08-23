Imports System.Data.SqlClient

Public Class AnnuaireReferenceDao
    Inherits StandardDao

    Public Enum EnumSourceAnnuaire
        ANNUAIRE_REFERENCE = 0
        ANNUAIRE_NATIONAL = 1
    End Enum

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

    Public Function GetAnnuaireReferenceById(annuaireProfessionneld As Integer) As AnnuaireProfessionnel
        Dim annuaireProfessionnel As AnnuaireProfessionnel
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.ans_annuaire_professionnel_sante_reference WHERE Cle_entree = @id"
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

    Public Function ChargementEtatCivil(annuaireProfession As AnnuaireProfessionnel) As AnnuaireEtatCivil
        Dim annuaireEtatCivil As New AnnuaireEtatCivil
        annuaireEtatCivil.Nom = annuaireProfession.LibelleCiviliteExercice.Trim() & " " & annuaireProfession.PrenomExercice.Trim() & " " & annuaireProfession.NomExercice
        annuaireEtatCivil.Profession = annuaireProfession.LibelleProfession.Trim() & " " & annuaireProfession.LibelleSavoirFaire
        annuaireEtatCivil.RaisonSociale = annuaireProfession.RaisonSocialeSite.Trim()
        Dim adresse1 As String = annuaireProfession.ComplementPointGeographiqueCoordonneeStructure.Trim() & " " &
            annuaireProfession.NumeroVoieCoordonneeStructure.Trim() & " " &
            annuaireProfession.IndiceRepetitionVoieCoordonneeStructure.Trim() & " " &
            annuaireProfession.LibelleTypeVoieCoordonneeStructure.Trim() & " " &
            annuaireProfession.LibelleVoieCoordonneeStructure
        annuaireEtatCivil.Adresse1 = adresse1.Trim()
        annuaireEtatCivil.Adresse2 = annuaireProfession.BureauCedexCoordonneeStructure.Trim()

        Return annuaireEtatCivil
    End Function

    Public Function CreationAnnuaireReference(annuaireReference As AnnuaireProfessionnel, userLog As Utilisateur) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim cleReferenceId As Long

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.ans_annuaire_professionnel_sante_reference" &
        " (type_identifiant_pp, identifiant_pp, identifiant_national_pp, code_civilite_exercice," &
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
        " code_secteur_activite, libelle_secteur_activite, code_section_tableau_pharmaciens, libelle_section_tableau_pharmaciens)" &
        " VALUES (@type_identifiant_pp, @identifiant_pp, @identifiant_national_pp, @code_civilite_exercice," &
        " @libelle_civilite_exercice, @code_civilite, @liblle_civilite, @nom_exercice, @prenom_exercice," &
        " @code_profression, @libelle_profession, @code_categorie_professionnelle, @libelle_categorie_professionnelle, @code_type_savoir_faire," &
        " @libelle_type_savoir_faire, @code_savoir_faire," &
        " @libellé_savoir_faire, @code_mode_exercice, @libelle_mode_exercice, @numero_siret_site," &
        " @numero_siren_site, @numero_finess_site, @numero_finess_etablissement_juridique, @identifiant_technique_structure," &
        " @raison_sociale_site, @enseigne_commerciale_site, @complement_destinataire_coord_structure, @complement_point_geographique_coord_structure," &
        " @numero_voie_coord_structure, @indice_repetition_voie_coord_structure, @code_type_voie_coord_structure, @libelle_type_voie_coord_structure," &
        " @libelle_voie_coord_structure, @mention_distribution_coord_structure, @bureau_cedex_coord_structure, @code_postal_coord_structure," &
        " @code_commune_coord_structure, @libelle_commune_coord_structure, @code_pays_coord_structure, @libelle_pays_coord_structure," &
        " @telephone_coord_structure, @telephone2_coord_structure, @telecopie_coord_structure, @adresse_email_coord_structure," &
        " @code_departement_structure, @libelle_departement_structure, @ancien_identifiant_structure, @autorite_enregistrement," &
        " @code_secteur_activite, @libelle_secteur_activite, @code_section_tableau_pharmaciens, @libelle_section_tableau_pharmaciens); SELECT SCOPE_IDENTITY()"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@type_identifiant_pp", annuaireReference.Typeidentifiant)
            .AddWithValue("@identifiant_pp", annuaireReference.Identifiant)
            .AddWithValue("@identifiant_national_pp", annuaireReference.IdentifiantNational)
            .AddWithValue("@code_civilite_exercice", annuaireReference.CodeCiviliteExercice)
            .AddWithValue("@libelle_civilite_exercice", annuaireReference.LibelleCiviliteExercice)
            .AddWithValue("@code_civilite", annuaireReference.CodeCivilite)
            .AddWithValue("@liblle_civilite", annuaireReference.LibelleCivilite)
            .AddWithValue("@nom_exercice", annuaireReference.NomExercice)
            .AddWithValue("@prenom_exercice", annuaireReference.PrenomExercice)
            .AddWithValue("@code_profression", annuaireReference.CodeProfession)
            .AddWithValue("@libelle_profession", annuaireReference.LibelleProfession)
            .AddWithValue("@code_categorie_professionnelle", annuaireReference.CodeCategorieProfessionnelle)
            .AddWithValue("@libelle_categorie_professionnelle", annuaireReference.LibelleCategorieProfessionnelle)
            .AddWithValue("@code_type_savoir_faire", annuaireReference.CodeTypeSavoirFaire)
            .AddWithValue("@libelle_type_savoir_faire", annuaireReference.LibelleTypeSavoirFaire)
            .AddWithValue("@code_savoir_faire", annuaireReference.CodeSavoirFaire)
            .AddWithValue("@libellé_savoir_faire", annuaireReference.LibelleSavoirFaire)
            .AddWithValue("@code_mode_exercice", annuaireReference.CodeModeExercice)
            .AddWithValue("@libelle_mode_exercice", annuaireReference.LibelleModeExercice)
            .AddWithValue("@numero_siret_site", annuaireReference.NumeroSiretSite)
            .AddWithValue("@numero_siren_site", annuaireReference.NumeroSirenSite)
            .AddWithValue("@numero_finess_site", annuaireReference.NumeroFinessSite)
            .AddWithValue("@numero_finess_etablissement_juridique", annuaireReference.NumeroFinessEtablissementJuridique)
            .AddWithValue("@identifiant_technique_structure", annuaireReference.IdentifiantTechniqueStructure)
            .AddWithValue("@raison_sociale_site", annuaireReference.RaisonSocialeSite)
            .AddWithValue("@enseigne_commerciale_site", annuaireReference.EnseigneCommercialeSite)
            .AddWithValue("@complement_destinataire_coord_structure", annuaireReference.ComplementDestinataireCoordonneeStructure)
            .AddWithValue("@complement_point_geographique_coord_structure", annuaireReference.ComplementPointGeographiqueCoordonneeStructure)
            .AddWithValue("@numero_voie_coord_structure", annuaireReference.NumeroVoieCoordonneeStructure)
            .AddWithValue("@indice_repetition_voie_coord_structure", annuaireReference.IndiceRepetitionVoieCoordonneeStructure)
            .AddWithValue("@code_type_voie_coord_structure", annuaireReference.CodeTypeVoieCoordonneeStructure)
            .AddWithValue("@libelle_type_voie_coord_structure", annuaireReference.LibelleTypeVoieCoordonneeStructure)
            .AddWithValue("@libelle_voie_coord_structure", annuaireReference.LibelleVoieCoordonneeStructure)
            .AddWithValue("@mention_distribution_coord_structure", annuaireReference.MentionDistributionCoordonneeStructure)
            .AddWithValue("@bureau_cedex_coord_structure", annuaireReference.BureauCedexCoordonneeStructure)
            .AddWithValue("@code_postal_coord_structure", annuaireReference.CodePostalCoordonneeStructure)
            .AddWithValue("@code_commune_coord_structure", annuaireReference.CodeCommuneCoordonneeStructure)
            .AddWithValue("@libelle_commune_coord_structure", annuaireReference.LibelleCommuneCoordonneeStructure)
            .AddWithValue("@code_pays_coord_structure", annuaireReference.CodePaysCoordonneeStructure)
            .AddWithValue("@libelle_pays_coord_structure", annuaireReference.LibellePaysCoordonneeStructure)
            .AddWithValue("@telephone_coord_structure", annuaireReference.TelephoneCoordonneeStructure)
            .AddWithValue("@telephone2_coord_structure", annuaireReference.Telephone2CoordonneeStructure)
            .AddWithValue("@telecopie_coord_structure", annuaireReference.TelepcopieCoordonneeStructure)
            .AddWithValue("@adresse_email_coord_structure", annuaireReference.emailCoordonneeStructure)
            .AddWithValue("@code_departement_structure", annuaireReference.CodeDepartementStructure)
            .AddWithValue("@libelle_departement_structure", annuaireReference.LibelleDepartementStructure)
            .AddWithValue("@ancien_identifiant_structure", annuaireReference.AncienIdentifiantStructure)
            .AddWithValue("@autorite_enregistrement", annuaireReference.AutoriteEnregistrement)
            .AddWithValue("@code_secteur_activite", annuaireReference.CodeSecteurActivite)
            .AddWithValue("@libelle_secteur_activite", annuaireReference.LibelleSecteurActivite)
            .AddWithValue("@code_section_tableau_pharmaciens", annuaireReference.CodeSectionTableauPharmacien)
            .AddWithValue("@libelle_section_tableau_pharmaciens", annuaireReference.LibelleSectionTableauPharmacien)
        End With

        Try
            da.InsertCommand = cmd
            cleReferenceId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If codeRetour = True Then
            Return cleReferenceId
        Else
            cleReferenceId = 0
        End If

        Return codeRetour
    End Function
End Class
