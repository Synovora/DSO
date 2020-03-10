Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common

Public Class SousEpisodeSousSousTypeDao
    Inherits StandardDao

    Public Function getLstSousEpisodeSousSousType(Optional idSousEpisodeSousType As Long = 0) As List(Of SousEpisodeSousSousType)
        Dim lst As List(Of SousEpisodeSousSousType) = New List(Of SousEpisodeSousSousType)
        Dim data As DataTable = getTableSousEpisodeSousSousType(idSousEpisodeSousType)
        For Each row In data.Rows
            lst.Add(buildBean(row))
        Next
        Return lst
    End Function
    Public Function getDictSousEpisodeSousType(Optional idSousEpisodeSousType As Long = 0) As Dictionary(Of Long, SousEpisodeSousSousType)
        Dim dic As Dictionary(Of Long, SousEpisodeSousSousType) = New Dictionary(Of Long, SousEpisodeSousSousType)
        Dim data As DataTable = getTableSousEpisodeSousSousType(idSousEpisodeSousType)
        For Each row In data.Rows
            dic.Add(row("id"), buildBean(row))
        Next
        Return dic
    End Function

    Public Function getTableSousEpisodeSousSousType(Optional idSousEpisodeSousType As Long = 0) As DataTable
        Dim SQLString As String
        'Console.WriteLine("----------> getAllTacheEnCours")
        SQLString =
            "SELECT " & vbCrLf &
            "	  id, " & vbCrLf &
            "     id_sous_episode_sous_type, " & vbCrLf &
            "     horodate_creation, " & vbCrLf &
            "	  libelle " & vbCrLf &
            "FROM [oasis].[oa_r_sous_episode_sous_sous_type] " & vbCrLf

        If idSousEpisodeSousType <> 0 Then
            SQLString += "WHERE id_sous_episode_sous_type= @idSousEpisodeSousType " & vbCrLf
        End If

        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                If idSousEpisodeSousType <> 0 Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idSousEpisodeSousType", idSousEpisodeSousType)
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


    Friend Function Create(seType As SousEpisodeSousSousType) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oa_r_sous_episode_sous_sous_type " &
                    "(id_sous_episode_sous_type, horodate_creation, libelle)" &
            " VALUES (@id_sous_episode_sous_type, @dateCreation, @libelle)"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id_sous_episode_sous_type", seType.IdSousEpisodeSousType)
                .AddWithValue("@dateCreation", seType.HorodateCreation)
                .AddWithValue("@libelle", seType.Libelle)
            End With

            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return codeRetour
    End Function

    Private Function buildBean(row As DataRow) As SousEpisodeSousSousType
        Dim seType As New SousEpisodeSousSousType(row)
        Return seType
    End Function
End Class
