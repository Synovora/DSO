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
        Dim SQLString = "SELECT * FROM oasis.oa_sous_episode_reponse_mail"
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

    'Public Function GetById(idSousEpisodeReponseMail As Long) As SousEpisodeReponseMail
    '    Return GetLstSousEpisodeReponseMail(0, idSousEpisodeReponseMail, True)(0)
    'End Function

    Private Function BuildBean(row As DataRow) As SousEpisodeReponseMail
        Dim seType As New SousEpisodeReponseMail(row)
        Return seType
    End Function

End Class
