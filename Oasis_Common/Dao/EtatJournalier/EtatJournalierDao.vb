Imports System.Data.SqlClient

Public Class EtatJournalierDao
    Inherits StandardDao

    Public Function GetDrcById(DrcId As Long) As Drc
        Dim drc As Drc
        Using con As SqlConnection = GetConnection()

            Try
                Dim command As SqlCommand = con.CreateCommand()

                command.CommandText =
                    "SELECT * FROM oasis.oa_drc WHERE oa_drc_id = @id"
                command.Parameters.AddWithValue("@id", DrcId)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        drc = BuildBean(reader)
                    Else
                        Throw New ArgumentException("DRC inexistante !")
                    End If
                End Using

            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return drc
    End Function

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As Drc
        Dim drc As New Drc With {
            .DrcId = Convert.ToInt64(record("oa_drc_id")),
            .DrcLibelle = Coalesce(record("oa_drc_libelle"), ""),
            .DrcSexe = Coalesce(record("oa_drc_sexe"), 0),
            .DrcTypeEpisode = Coalesce(record("oa_drc_typ_epi"), ""),
            .DrcAgeMin = Coalesce(record("oa_drc_age_min"), 0),
            .DrcAgeMax = Coalesce(record("oa_drc_age_max"), 0),
            .CategorieMajeure = Coalesce(record("oa_drc_categorie_majeure_id"), 0),
            .CategorieOasisId = Coalesce(record("oa_drc_oasis_categorie"), 0),
            .CodeCim = Coalesce(record("oa_drc_code_cim_defaut"), ""),
            .CodeCisp = Coalesce(record("oa_drc_code_cisp_defaut"), ""),
            .AldId = Coalesce(record("oa_drc_ald_id"), 0),
            .AldCode = Coalesce(record("oa_drc_ald_code"), ""),
            .Commentaire = Coalesce(record("oa_drc_dur_prob_epis"), ""),
            .ReponseCommentee = Coalesce(record("oa_drc_typ_epi"), ""),
            .DateCreation = Coalesce(record("oa_drc_date_creation"), Nothing),
            .UserCreation = Coalesce(record("oa_drc_utilisateur_creation"), 0),
            .DateModification = Coalesce(record("oa_drc_date_modification"), Nothing),
            .UserModification = Coalesce(record("oa_drc_utilisateur_modification"), 0)
        }
        Return drc
    End Function


End Class
