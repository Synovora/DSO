Imports System.Data.SqlClient
Imports Oasis_Common
Public Class EpisodeObservationDao
    Inherits StandardDao
    Public Enum EnumTypeEpisodeObservation
        MEDICAL
        PARAMEDICAL
    End Enum

    Public Enum EnumNatureEpisodeObservation
        SPECIFIQUE
        LIBRE
    End Enum

    Public Enum EnumNaturePresence
        PRESENTIEL
        DISTANT
    End Enum

    Friend Function GetEpisodeObservationById(Id As Integer) As EpisodeObservation
        Dim episodeObservation As EpisodeObservation
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_episode_observation WHERE episode_observation_id = @id"
            command.Parameters.AddWithValue("@id", Id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episodeObservation = BuildBean(reader)
                Else
                    Throw New ArgumentException("épisode inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episodeObservation
    End Function

    Private Function BuildBean(reader As SqlDataReader) As EpisodeObservation
        Dim episodeObservation As New EpisodeObservation With {
            .Id = reader("episode_observation_id"),
            .EpisodeId = Coalesce(reader("episode_id"), 0),
            .PatientId = Coalesce(reader("patient_id"), 0),
            .TypeObservation = Coalesce(reader("type_observation"), ""),
            .NatureObservation = Coalesce(reader("nature_observation"), ""),
            .NaturePresence = Coalesce(reader("nature_presence"), ""),
            .Observation = Coalesce(reader("observation"), ""),
            .UserCreation = Coalesce(reader("user_id"), 0),
            .DateCreation = Coalesce(reader("date_creation"), Nothing),
            .DateModification = Coalesce(reader("date_modification"), Nothing),
            .Inactif = Coalesce(reader("inactif"), False)
        }
        Return episodeObservation
    End Function

    Public Function GetEpisodeObservationLibreByEpisode(episodeId As Integer) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_episode_observation" &
                        " WHERE (inactif = 'False' or inactif is Null)" &
                        " AND nature_observation = 'LIBRE'" &
                        " AND episode_id = " & episodeId.ToString &
                        " ORDER BY episode_observation_id DESC"

        Dim ObservationDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ParcoursDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursDataAdapter
                ParcoursDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                'Using ParcoursDataTable
                Try
                    ParcoursDataAdapter.Fill(ObservationDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
                'End Using
            End Using
        End Using

        Return ObservationDataTable
    End Function

    Friend Function Compare(source1 As EpisodeObservation, source2 As EpisodeObservation) As Boolean
        If source1.Id <> source2.Id Then
            Return False
        End If
        If source1.EpisodeId <> source2.EpisodeId Then
            Return False
        End If
        If source1.PatientId <> source2.PatientId Then
            Return False
        End If
        If source1.UserCreation <> source2.UserCreation Then
            Return False
        End If
        If source1.TypeObservation <> source2.TypeObservation Then
            Return False
        End If
        If source1.NatureObservation <> source2.NatureObservation Then
            Return False
        End If
        If source1.NaturePresence <> source2.NaturePresence Then
            Return False
        End If
        If source1.Observation <> source2.Observation Then
            Return False
        End If
        If source1.DateCreation.Date <> source2.DateCreation.Date Then
            Return False
        End If
        If source1.DateModification.Date <> source2.DateModification.Date Then
            Return False
        End If

        Return True
    End Function

    Friend Function CreateEpisodeObservation(episodeObservation As EpisodeObservation) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "INSERT INTO oasis.oa_episode_observation" &
        " (episode_id, patient_id, user_id, type_observation, nature_observation," &
        " nature_presence, observation, date_creation, date_modification, inactif)" &
        " VALUES (@episodeId, @patientId, @userId, @typeObservation, @natureObservation," &
        " @naturePresence, @observation, @dateCreation, @dateModification, @inactif)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", episodeObservation.PatientId)
            .AddWithValue("@episodeId", episodeObservation.EpisodeId)
            .AddWithValue("@userId", episodeObservation.UserCreation)
            .AddWithValue("@typeObservation", episodeObservation.TypeObservation)
            .AddWithValue("@natureObservation", episodeObservation.NatureObservation)
            .AddWithValue("@naturePresence", episodeObservation.NaturePresence)
            .AddWithValue("@observation", episodeObservation.Observation)
            .AddWithValue("@dateCreation", Date.Now())
            .AddWithValue("@dateModification", DBNull.Value)
            .AddWithValue("@inactif", episodeObservation.Inactif)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ModificationEpisodeObservation(episodeObservation As EpisodeObservation) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "UPDATE oasis.oa_episode_observation SET" &
        " episode_id = @episodeId, patient_id = @patientId, user_id = @userId, type_observation = @typeObservation," &
        " nature_observation = @natureObservation, nature_presence = @naturePresence," &
        " observation = @observation, date_creation = @dateCreation, date_modification = @dateModification, inactif = @inactif" &
        " where episode_observation_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeObservation.Id)
            .AddWithValue("@patientId", episodeObservation.PatientId)
            .AddWithValue("@episodeId", episodeObservation.EpisodeId)
            .AddWithValue("@userId", episodeObservation.UserCreation)
            .AddWithValue("@typeObservation", episodeObservation.TypeObservation)
            .AddWithValue("@natureObservation", episodeObservation.NatureObservation)
            .AddWithValue("@naturePresence", episodeObservation.NaturePresence)
            .AddWithValue("@observation", episodeObservation.Observation)
            .AddWithValue("@dateCreation", episodeObservation.DateCreation)
            .AddWithValue("@dateModification", Date.Now())
            .AddWithValue("@inactif", episodeObservation.Inactif)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

End Class
