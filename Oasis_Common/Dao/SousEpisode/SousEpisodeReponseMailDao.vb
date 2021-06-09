Imports System.Data.SqlClient

Public Class SousEpisodeReponseMailDao
    Inherits StandardDao

    Public Function GetLstSousEpisodeReponseMail() As List(Of SousEpisodeReponseMail)
        Dim con As SqlConnection = GetConnection()
        Dim sousEpisodeReponseMails As List(Of SousEpisodeReponseMail) = New List(Of SousEpisodeReponseMail)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT id, auteur, objet, status, horodate_creation, patient_id FROM oasis.oa_sous_episode_reponse_mail WHERE status='unprocessed'"
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    sousEpisodeReponseMails.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return sousEpisodeReponseMails
    End Function

    Public Function GetSousEpisodeReponseMailById(id As Long) As SousEpisodeReponseMail
        Dim patientNote As SousEpisodeReponseMail
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_sous_episode_reponse_mail WHERE id=@id"
            command.Parameters.AddWithValue("@id", id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    patientNote = BuildBean(reader)
                Else
                    Throw New ArgumentException("SousEpisodeReponseMail inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return patientNote
    End Function

    Public Function DeleteSousEpisodeReponseMailById(id As Long)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "UPDATE oasis.oa_sous_episode_reponse_mail SET" &
        " status=@status" &
        " WHERE id=@id"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@id", id)
            .AddWithValue("@status", "deleted")
        End With
        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Function

    Public Function BuildBean(reader As SqlDataReader) As SousEpisodeReponseMail
        Dim seType As New SousEpisodeReponseMail(reader)
        Return seType
    End Function

End Class
