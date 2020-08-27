Imports System.Data.SqlClient
Imports Oasis_Common
Public Class EpisodeActeParamedicalDao
    Inherits StandardDao

    Public Function GetEpisodeActeParamedicalById(episodeActeParamedicalId As Integer) As EpisodeActeParamedical
        Dim episodeActeParamedical As EpisodeActeParamedical
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_episode_acte_paramedical WHERE oa_episode_acte_paramedical_id = @id"
            command.Parameters.AddWithValue("@id", episodeActeParamedicalId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episodeActeParamedical = BuildBean(reader)
                Else
                    Throw New ArgumentException("épisode acte médical inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episodeActeParamedical
    End Function

    Private Function BuildBean(reader As SqlDataReader) As EpisodeActeParamedical
        Dim episodeActeParamedical As New EpisodeActeParamedical

        episodeActeParamedical.Id = reader("oa_episode_acte_paramedical_id")
        episodeActeParamedical.PatientId = Coalesce(reader("patient_id"), 0)
        episodeActeParamedical.EpisodeId = Coalesce(reader("episode_id"), 0)
        episodeActeParamedical.DrcId = Coalesce(reader("drc_id"), 0)
        episodeActeParamedical.Observation = Coalesce(reader("observation"), "")
        episodeActeParamedical.TypeObservation = Coalesce(reader("type_observation"), "")
        episodeActeParamedical.UserId = Coalesce(reader("user_id"), 0)
        episodeActeParamedical.DateObservation = Coalesce(reader("date_saisie_observation"), Nothing)
        episodeActeParamedical.DateModification = Coalesce(reader("date_modification_observation"), Nothing)
        episodeActeParamedical.Inactif = Coalesce(reader("inactif"), False)
        Return episodeActeParamedical
    End Function


    Public Function getAllEpisodeActeParamedicalByEpisodeId(episodeId As Long, Optional typeObservation As String = "PARAMEDICAL") As DataTable
        Dim SQLString As String

        SQLString = "SELECT E.oa_episode_acte_paramedical_id," &
            " E.drc_id, E.observation, E.type_observation, E.user_id, E.date_saisie_observation," &
            " E.date_modification_observation, D.oa_drc_libelle, D.oa_drc_typ_epi, D.oa_drc_dur_prob_epis, D.oa_drc_oasis_categorie" &
            " FROM oasis.oa_episode_acte_paramedical E" &
            " LEFT JOIN oasis.oa_drc D ON E.drc_id = D.oa_drc_id" &
            " WHERE episode_id = " & episodeId.ToString &
            " AND type_observation = '" & typeObservation & "'" &
            " AND (E.inactif = '0' OR E.inactif is Null)"

        Using con As SqlConnection = GetConnection()
            Dim ContexteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ContexteDataAdapter
                ContexteDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        ContexteDataAdapter.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

    Public Function PutAllEpisodeActeParamedicalToMedicalByEpisodeId(episodeId As Long) As Boolean
        Dim SQLString As String

        SQLString = "SELECT * FROM oasis.oa_episode_acte_paramedical" &
            " WHERE episode_id = " & episodeId.ToString &
            " AND (inactif = '0' OR inactif is Null)"

        Using con As SqlConnection = GetConnection()
            Dim ContexteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ContexteDataAdapter
                ContexteDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        ContexteDataAdapter.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand
                        Dim rowCount As Integer = dt.Rows.Count - 1
                        Dim episodeActeParamedical As EpisodeActeParamedical
                        For i = 0 To rowCount Step 1
                            Dim episodeActeParamedicalId As Integer = dt.Rows(i)("oa_episode_acte_paramedical_id")
                            episodeActeParamedical = GetEpisodeActeParamedicalById(episodeActeParamedicalId)
                            episodeActeParamedical.TypeObservation = ProfilDao.EnumProfilType.MEDICAL.ToString
                            ModificationEpisodeActeParamedical(episodeActeParamedical)
                        Next
                    Catch ex As Exception
                        Throw ex
                    End Try

                    Return True
                End Using
            End Using
        End Using
    End Function


    Public Function CreateEpisodeActeParamedical(episodeActeParamedical As EpisodeActeParamedical) As Long
        Dim episodeActeParamedicalIdCree As Integer = 0
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF NOT EXISTS (SELECT 1 FROM oasis.oa_episode_acte_paramedical WHERE episode_id = @episodeId AND drc_id = @drcId AND (inactif = Null OR inactif = '0'))" &
        "INSERT INTO oasis.oa_episode_acte_paramedical" &
        " (drc_id, episode_id, patient_id, observation, type_observation, inactif)" &
        " VALUES (@drcId, @episodeId, @patientId, @observation, @typeObservation, @inactif); SELECT SCOPE_IDENTITY()"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@drcId", episodeActeParamedical.DrcId)
            .AddWithValue("@patientId", episodeActeParamedical.PatientId)
            .AddWithValue("@episodeId", episodeActeParamedical.EpisodeId)
            .AddWithValue("@Observation", episodeActeParamedical.Observation)
            .AddWithValue("@typeObservation", episodeActeParamedical.TypeObservation)
            .AddWithValue("@userId", episodeActeParamedical.UserId)
            .AddWithValue("@inactif", episodeActeParamedical.Inactif)
        End With

        Try
            da.InsertCommand = cmd
            episodeActeParamedicalIdCree = Coalesce(da.InsertCommand.ExecuteScalar(), 0)
            If episodeActeParamedicalIdCree <= 0 Then
                Throw New Exception("Collision: L'acte paramédical existe déjà pour cet épisode")
            End If
        Catch ex As Exception
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return episodeActeParamedicalIdCree
    End Function

    Public Function ModificationEpisodeActeParamedical(episodeActeParamedical As EpisodeActeParamedical) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_episode_acte_paramedical SET" &
        " drc_id = @drcId," &
        " observation = @observation," &
        " type_observation = @typeObservation," &
        " user_id = @userId," &
        " date_saisie_observation = @dateSaisieObservation," &
        " date_modification_observation = @dateModificationObservation," &
        " inactif = @inactif" &
        " WHERE oa_episode_acte_paramedical_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeActeParamedical.Id)
            .AddWithValue("@drcid", episodeActeParamedical.DrcId)
            .AddWithValue("@observation", episodeActeParamedical.Observation)
            .AddWithValue("@typeObservation", episodeActeParamedical.TypeObservation)
            .AddWithValue("@userId", episodeActeParamedical.UserId)
            .AddWithValue("@dateSaisieObservation", If(episodeActeParamedical.DateObservation = Nothing, DBNull.Value, episodeActeParamedical.DateObservation))
            .AddWithValue("@dateModificationObservation", If(episodeActeParamedical.DateModification = Nothing, DBNull.Value, episodeActeParamedical.DateModification))
            .AddWithValue("@inactif", episodeActeParamedical.Inactif)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ModificationEpisodeActeParamedicalObservation(episodeActeParamedicalId As Long, Observation As String, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim typeObservation As String = userLog.TypeProfil

        Dim SQLstring As String = "UPDATE oasis.oa_episode_acte_paramedical SET" &
        " observation = @observation," &
        " type_observation = @typeObservation," &
        " user_id = @userId," &
        " date_saisie_observation = @dateSaisieObservation," &
        " date_modification_observation = @dateModificationObservation" &
        " WHERE oa_episode_acte_paramedical_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeActeParamedicalId)
            .AddWithValue("@observation", Observation)
            .AddWithValue("@typeObservation", typeObservation)
            .AddWithValue("@userId", userLog.UtilisateurId)
            .AddWithValue("@dateSaisieObservation", Date.Now().ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateModificationObservation", Date.Now().ToString("yyyy-MM-dd HH:mm:ss"))
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function SuppressionEpisodeActeParamedicalByEpisodeId(episodeId As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "DELETE oasis.oa_episode_acte_paramedical" &
        " WHERE episode_id = @episodeId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@episodeId", episodeId)
        End With

        Try
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

End Class
