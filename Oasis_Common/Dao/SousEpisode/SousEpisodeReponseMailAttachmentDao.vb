Imports System.Data.SqlClient

Public Class SousEpisodeReponseMailAttachmentDao
    Inherits StandardDao

    'Public Function GetLstSousEpisodeReponseMail() As List(Of SousEpisodeReponseMail)
    '    Dim lst As List(Of SousEpisodeReponseMail) = New List(Of SousEpisodeReponseMail)
    '    Dim data As DataTable = GetTableSousEpisodeReponseMail()
    '    For Each row In data.Rows
    '        lst.Add(BuildBean(row))
    '    Next
    '    Return lst
    'End Function


    Public Function GetSousEpisodeReponseMailAttachmentByMailId(mailId) As List(Of SousEpisodeReponseMailAttachment)
        Dim con As SqlConnection = GetConnection()
        Dim attachments As List(Of SousEpisodeReponseMailAttachment) = New List(Of SousEpisodeReponseMailAttachment)
        Dim SQLString = "SELECT * FROM oasis.oa_sous_episode_reponse_mail_attachment WHERE mailId=@mailId"

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = SQLString
            command.Parameters.AddWithValue("@mailId", mailId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    attachments.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return attachments
    End Function

    Public Function GetSousEpisodeReponseMailAttachmentById(Id) As SousEpisodeReponseMailAttachment
        Dim patientNote As SousEpisodeReponseMailAttachment
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_sous_episode_reponse_mail_attachment WHERE id=@id"
            command.Parameters.AddWithValue("@id", Id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    patientNote = BuildBean(reader)
                Else
                    Throw New ArgumentException("SousEpisodeReponseMailAttachment inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return patientNote
    End Function

    Private Function BuildBean(reader As SqlDataReader) As SousEpisodeReponseMailAttachment
        Dim sousEpisode As New SousEpisodeReponseMailAttachment With {
            .Id = reader("id"),
            .MailId = reader("mailId"),
            .Filename = Coalesce(reader("filename"), Nothing),
            .Part = reader("part")
            }
        Return sousEpisode
    End Function

    Private Function BuildBean(row As DataRow) As SousEpisodeReponseMailAttachment
        Dim seType As New SousEpisodeReponseMailAttachment(row)
        Return seType
    End Function

End Class
