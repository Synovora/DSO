Imports System.Data.SqlClient

Public Class SousEpisodeDetailSousTypeDao
    Inherits StandardDao

    Public Function getLstSousEpisodeDetailSousType(idSousEpisode As Long) As List(Of SousEpisodeDetailSousType)
        Dim lst As List(Of SousEpisodeDetailSousType) = New List(Of SousEpisodeDetailSousType)
        Dim data As DataTable = getTableSousEpisodeDetailSousType(idSousEpisode)
        For Each row In data.Rows
            lst.Add(BuildBean(row))
        Next
        Return lst
    End Function

    Public Function getTableSousEpisodeDetailSousType(idSousEpisode As Long) As DataTable
        Dim SQLString As String
        SQLString =
            "SELECT " & vbCrLf &
            "	  SED.id, " & vbCrLf &
            "     SED.id_sous_episode, " & vbCrLf &
            "     SED.id_sous_episode_sous_sous_type, " & vbCrLf &
            "	  SED.is_ald, " & vbCrLf &
            "	  SST.libelle " & vbCrLf

        SQLString += "FROM oasis.oa_sous_episode_detail SED " & vbCrLf
        SQLString += "JOIN oasis.oa_r_sous_episode_sous_sous_type SST ON SST.id = SED.id_sous_episode_sous_sous_type " & vbCrLf
        SQLString += "WHERE 1=1 " & vbCrLf
        SQLString += "AND SED.id_sous_episode= @idSousEpisode " & vbCrLf

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idSousEpisode", idSousEpisode)
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

    Public Function Create(con As SqlConnection, sousEpisodeDetail As SousEpisodeDetailSousType, transaction As SqlTransaction) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim isMyTransaction As Boolean = (transaction Is Nothing)
        If isMyTransaction Then con = GetConnection()

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_sous_episode_detail " &
                    "(id_sous_episode, id_sous_episode_sous_sous_type, is_ald )" &
            " VALUES (@id_sous_episode, @id_sous_episode_sous_sous_type, @is_ald); SELECT SCOPE_IDENTITY()"

            Dim cmd As SqlCommand
            If transaction Is Nothing Then
                cmd = New SqlCommand(SQLstring, con)
            Else
                cmd = New SqlCommand(SQLstring, con, transaction)
            End If

            With cmd.Parameters
                .AddWithValue("@id_sous_episode", sousEpisodeDetail.IdSousEpisode)
                .AddWithValue("@id_sous_episode_sous_sous_type", sousEpisodeDetail.IdSousEpisodeSousSousType)
                .AddWithValue("@is_ald", sousEpisodeDetail.IsALD)
            End With

            da.InsertCommand = cmd
            sousEpisodeDetail.Id = da.InsertCommand.ExecuteScalar()

        Catch ex As Exception
            If Not isMyTransaction Then Throw ex   ' on remonte l'excaption a l'appelant
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            If isMyTransaction Then con.Close()
        End Try

        Return codeRetour
    End Function

    Private Function BuildBean(row As DataRow) As SousEpisodeDetailSousType
        Dim seType As New SousEpisodeDetailSousType(row)
        Return seType
    End Function

End Class
