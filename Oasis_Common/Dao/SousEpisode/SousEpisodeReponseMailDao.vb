Imports System.Data.SqlClient

Public Class SousEpisodeReponseMailDao
    Inherits StandardDao

    Public Function GetLstSousEpisodeReponseMail() As List(Of SousEpisodeReponseMail)
        Dim lst As List(Of SousEpisodeReponseMail) = New List(Of SousEpisodeReponseMail)
        Dim data As DataTable = GetTableSousEpisodeReponseMail()
        For Each row In data.Rows
            lst.Add(BuildBean(row))
        Next
        Return lst
    End Function


    Public Function GetTableSousEpisodeReponseMail() As DataTable
        Dim SQLString = "SELECT id, auteur, objet, status, horodate_creation, patient_id FROM oasis.oa_sous_episode_reponse_mail WHERE status='unprocessed'"
        Using con As SqlConnection = GetConnection()
            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetSousEpisodeReponseMailById(Id) As SousEpisodeReponseMail
        Dim patientNote As SousEpisodeReponseMail
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_sous_episode_reponse_mail WHERE id=@id"
            command.Parameters.AddWithValue("@id", Id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    patientNote = BuildBean(reader)
                Else
                    Throw New ArgumentException("SousEpisodeReponseMail inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return patientNote
    End Function

    Public Function BuildBean(row As DataRow) As SousEpisodeReponseMail
        Dim seType As New SousEpisodeReponseMail(row)
        Return seType
    End Function

    Public Function BuildBean(reader As SqlDataReader) As SousEpisodeReponseMail
        Dim sousEpisode As New SousEpisodeReponseMail With {
            .Id = reader("id"),
            .HorodateCreation = reader("horodate_creation"),
            .PatientId = Coalesce(reader("patient_id"), Nothing),
            .Status = reader("status"),
            .Auteur = reader("auteur"),
            .Objet = Coalesce(reader("objet"), ""),
            .Corps = Coalesce(reader("corps"), "")
            }
        Return sousEpisode
    End Function

End Class
