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

    Private Function BuildBean(reader As SqlDataReader) As Drc
        Dim drc As New Drc With {
            .DrcId = Convert.ToInt64(reader("oa_drc_id")),
            .DrcLibelle = Coalesce(reader("oa_drc_libelle"), ""),
            .DrcSexe = Coalesce(reader("oa_drc_sexe"), 0),
            .DrcTypeEpisode = Coalesce(reader("oa_drc_typ_epi"), ""),
            .DrcAgeMin = Coalesce(reader("oa_drc_age_min"), 0),
            .DrcAgeMax = Coalesce(reader("oa_drc_age_max"), 0),
            .CategorieMajeure = Coalesce(reader("oa_drc_categorie_majeure_id"), 0),
            .CategorieOasisId = Coalesce(reader("oa_drc_oasis_categorie"), 0),
            .CodeCim = Coalesce(reader("oa_drc_code_cim_defaut"), ""),
            .CodeCisp = Coalesce(reader("oa_drc_code_cisp_defaut"), ""),
            .AldId = Coalesce(reader("oa_drc_ald_id"), 0),
            .AldCode = Coalesce(reader("oa_drc_ald_code"), ""),
            .Commentaire = Coalesce(reader("oa_drc_dur_prob_epis"), ""),
            .ReponseCommentee = Coalesce(reader("oa_drc_typ_epi"), ""),
            .DateCreation = Coalesce(reader("oa_drc_date_creation"), Nothing),
            .UserCreation = Coalesce(reader("oa_drc_utilisateur_creation"), 0),
            .DateModification = Coalesce(reader("oa_drc_date_modification"), Nothing),
            .UserModification = Coalesce(reader("oa_drc_utilisateur_modification"), 0)
        }
        Return drc
    End Function


End Class
