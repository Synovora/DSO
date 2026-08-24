Imports System.Data.SqlClient

Public Class AnnuaireProfessionnelDao

    Inherits StandardDao

    ''' <summary>
    ''' Lit une ligne de l'annuaire professionnel telle quelle, sans toucher à la base.
    ''' IDataRecord plutôt que SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As AnnuaireProfessionnel
        Dim AnnuaireProfessionnel As New AnnuaireProfessionnel With {
            .Cle_entree = record("Cle_entree"),
            .Typeidentifiant = Coalesce(record("type_identifiant_pp"), 0),
            .Identifiant = Coalesce(record("identifiant_pp"), ""),
            .IdentifiantNational = Coalesce(record("identifiant_national_pp"), ""),
            .CodeCiviliteExercice = Coalesce(record("code_civilite_exercice"), ""),
            .LibelleCiviliteExercice = Coalesce(record("libelle_civilite_exercice"), ""),
            .CodeCivilite = Coalesce(record("code_civilite"), ""),
            .LibelleCivilite = Coalesce(record("liblle_civilite"), ""),
            .NomExercice = Coalesce(record("nom_exercice"), ""),
            .PrenomExercice = Coalesce(record("prenom_exercice"), ""),
            .CodeProfession = Coalesce(record("code_profression"), 0),
            .LibelleProfession = Coalesce(record("libelle_profession"), ""),
            .CodeCategorieProfessionnelle = Coalesce(record("code_categorie_professionnelle"), ""),
            .LibelleCategorieProfessionnelle = Coalesce(record("libelle_categorie_professionnelle"), ""),
            .CodeTypeSavoirFaire = Coalesce(record("code_type_savoir_faire"), ""),
            .LibelleTypeSavoirFaire = Coalesce(record("libelle_type_savoir_faire"), ""),
            .CodeSavoirFaire = Coalesce(record("code_savoir_faire"), ""),
            .LibelleSavoirFaire = Coalesce(record("libellé_savoir_faire"), ""),
            .CodeModeExercice = Coalesce(record("code_mode_exercice"), ""),
            .LibelleModeExercice = Coalesce(record("libelle_mode_exercice"), ""),
            .NumeroSiretSite = Coalesce(record("numero_siret_site"), ""),
            .NumeroSirenSite = Coalesce(record("numero_siren_site"), ""),
            .NumeroFinessSite = Coalesce(record("numero_finess_site"), ""),
            .NumeroFinessEtablissementJuridique = Coalesce(record("numero_finess_etablissement_juridique"), ""),
            .IdentifiantTechniqueStructure = Coalesce(record("identifiant_technique_structure"), ""),
            .RaisonSocialeSite = Coalesce(record("raison_sociale_site"), ""),
            .EnseigneCommercialeSite = Coalesce(record("enseigne_commerciale_site"), ""),
            .ComplementDestinataireCoordonneeStructure = Coalesce(record("complement_destinataire_coord_structure"), ""),
            .ComplementPointGeographiqueCoordonneeStructure = Coalesce(record("complement_point_geographique_coord_structure"), ""),
            .NumeroVoieCoordonneeStructure = Coalesce(record("numero_voie_coord_structure"), ""),
            .IndiceRepetitionVoieCoordonneeStructure = Coalesce(record("indice_repetition_voie_coord_structure"), ""),
            .CodeTypeVoieCoordonneeStructure = Coalesce(record("code_type_voie_coord_structure"), ""),
            .LibelleTypeVoieCoordonneeStructure = Coalesce(record("libelle_type_voie_coord_structure"), ""),
            .LibelleVoieCoordonneeStructure = Coalesce(record("libelle_voie_coord_structure"), ""),
            .MentionDistributionCoordonneeStructure = Coalesce(record("mention_distribution_coord_structure"), ""),
            .BureauCedexCoordonneeStructure = Coalesce(record("bureau_cedex_coord_structure"), ""),
            .CodePostalCoordonneeStructure = Coalesce(record("code_postal_coord_structure"), ""),
            .CodeCommuneCoordonneeStructure = Coalesce(record("code_commune_coord_structure"), ""),
            .LibelleCommuneCoordonneeStructure = Coalesce(record("libelle_commune_coord_structure"), ""),
            .CodePaysCoordonneeStructure = Coalesce(record("code_pays_coord_structure"), ""),
            .LibellePaysCoordonneeStructure = Coalesce(record("libelle_pays_coord_structure"), ""),
            .TelephoneCoordonneeStructure = Coalesce(record("telephone_coord_structure"), ""),
            .Telephone2CoordonneeStructure = Coalesce(record("telephone2_coord_structure"), ""),
            .TelepcopieCoordonneeStructure = Coalesce(record("telecopie_coord_structure"), ""),
            .emailCoordonneeStructure = Coalesce(record("adresse_email_coord_structure"), ""),
            .CodeDepartementStructure = Coalesce(record("code_departement_structure"), ""),
            .LibelleDepartementStructure = Coalesce(record("libelle_departement_structure"), ""),
            .AncienIdentifiantStructure = Coalesce(record("ancien_identifiant_structure"), ""),
            .AutoriteEnregistrement = Coalesce(record("autorite_enregistrement"), ""),
            .CodeSecteurActivite = Coalesce(record("code_secteur_activite"), ""),
            .LibelleSecteurActivite = Coalesce(record("libelle_secteur_activite"), ""),
            .CodeSectionTableauPharmacien = Coalesce(record("code_section_tableau_pharmaciens"), ""),
            .LibelleSectionTableauPharmacien = Coalesce(record("libelle_section_tableau_pharmaciens"), "")
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

    Public Function GetProfessionnelSanteByNomAndCommune(CodeProfessionId As Integer, CodeSavoirFaireId As String, nomExercice As String, communeExercice As String, departementExercice As String) As DataTable
        Dim SQLString As String = "SELECT Cle_entree, code_civilite_exercice, prenom_exercice, nom_exercice," &
        " raison_sociale_site, libelle_commune_coord_structure, complement_point_geographique_coord_structure, numero_voie_coord_structure," &
        " indice_repetition_voie_coord_structure, libelle_type_voie_coord_structure, libelle_voie_coord_structure, bureau_cedex_coord_structure" &
        " FROM oasis.ans_annuaire_professionnel_sante"

        Dim ClauseWhere As String = " WHERE "
        ' Toutes les valeurs viennent de zones de saisie ou de données importées :
        ' elles passent par des paramètres, jamais par le texte de la commande.
        If CodeProfessionId AndAlso CodeSavoirFaireId IsNot Nothing Then
            ClauseWhere += "code_profression = @codeProfession AND code_savoir_faire = @codeSavoirFaire"
        Else
            ClauseWhere += "1=1"
        End If

        If nomExercice.Trim() <> "" Then
            ClauseWhere += " AND nom_exercice LIKE @nom"
        End If

        If communeExercice.Trim() <> "" Then
            ClauseWhere += " AND libelle_commune_coord_structure LIKE @commune"
        End If

        If departementExercice.Trim() <> "" Then
            ClauseWhere += " AND code_postal_coord_structure LIKE @departement"
        End If

        Dim ClauseOrderBy As String = " ORDER BY nom_exercice ASC;"

        SQLString += ClauseWhere
        SQLString += ClauseOrderBy

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                With TraitementDataAdapter.SelectCommand.Parameters
                    If CodeProfessionId AndAlso CodeSavoirFaireId IsNot Nothing Then
                        .AddWithValue("@codeProfession", CodeProfessionId)
                        .AddWithValue("@codeSavoirFaire", CodeSavoirFaireId)
                    End If
                    If nomExercice.Trim() <> "" Then .AddWithValue("@nom", "%" & EchapperLike(nomExercice) & "%")
                    If communeExercice.Trim() <> "" Then .AddWithValue("@commune", "%" & EchapperLike(communeExercice) & "%")
                    If departementExercice.Trim() <> "" Then .AddWithValue("@departement", EchapperLike(departementExercice) & "%")
                End With
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetStruturesByProfessionnel(IdentifiantNational As String) As DataTable
        Dim SQLString As String = "SELECT 
            Cle_entree
            ,identifiant_national_pp
            ,raison_sociale_site
	        ,identifiant_technique_structure
	        ,indice_repetition_voie_coord_structure
	        ,libelle_type_voie_coord_structure
	        ,libelle_voie_coord_structure
	        ,code_postal_coord_structure
            ,bureau_cedex_coord_structure
	        ,libelle_commune_coord_structure
	        ,CNTE.cnt" &
        " FROM oasis.ans_annuaire_professionnel_sante A" &
        " OUTER APPLY (
	        SELECT COUNT(*) as cnt FROM oasis.ans_annuaire_professionnel_sante_reference REF
	        WHERE REF.identifiant_national_pp = A.identifiant_national_pp
	        AND REF.identifiant_technique_structure = A.identifiant_technique_structure) AS CNTE"

        Dim ClauseWhere As String = " WHERE A.identifiant_national_pp = @identifiantNational"

        Dim ClauseOrderBy As String = " ORDER BY raison_sociale_site ASC;"

        SQLString += ClauseWhere
        SQLString += ClauseOrderBy

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                TraitementDataAdapter.SelectCommand.Parameters.AddWithValue(
                    "@identifiantNational", If(IdentifiantNational, "").Trim())
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function

End Class
