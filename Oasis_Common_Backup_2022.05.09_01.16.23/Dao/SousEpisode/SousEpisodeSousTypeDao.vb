﻿Imports System.Data.SqlClient

Public Class SousEpisodeSousTypeDao
    Inherits StandardDao

    Public Function getLstSousEpisodeSousType(Optional idSousEpisodeType As Long = 0) As List(Of SousEpisodeSousType)
        Dim lst As List(Of SousEpisodeSousType) = New List(Of SousEpisodeSousType)
        Dim data As DataTable = getTableSousEpisodeSousType(idSousEpisodeType)
        For Each row In data.Rows
            lst.Add(BuildBean(row))
        Next
        Return lst
    End Function
    Public Function getDictSousEpisodeSousType(Optional idSousEpisodeType As Long = 0) As Dictionary(Of Long, SousEpisodeSousType)
        Dim dic As Dictionary(Of Long, SousEpisodeSousType) = New Dictionary(Of Long, SousEpisodeSousType)
        Dim data As DataTable = getTableSousEpisodeSousType(idSousEpisodeType)
        For Each row In data.Rows
            dic.Add(row("id"), BuildBean(row))
        Next
        Return dic
    End Function

    Public Function getTableSousEpisodeSousType(Optional idSousEpisodeType As Long = 0) As DataTable
        Dim SQLString As String =
            "SELECT " & vbCrLf &
            "	  id, " & vbCrLf &
            "     id_sous_episode_type, " & vbCrLf &
            "     horodate_creation, " & vbCrLf &
            "	  libelle, " & vbCrLf &
            "	  redaction_profil_types, " & vbCrLf &
            "	  validation_profil_types, " & vbCrLf &
            "	  is_ald_possible, " & vbCrLf &
            "	  is_reponse_requise, " & vbCrLf &
            "	  delai_reponse, " & vbCrLf &
            "	  commentaire " & vbCrLf &
            "FROM [oasis].[oa_r_sous_episode_sous_type] " & vbCrLf

        If idSousEpisodeType <> 0 Then
            SQLString += "WHERE id_sous_episode_type= @idSousEpisodeType " & vbCrLf
        End If

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                If idSousEpisodeType <> 0 Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idSousEpisodeType", idSousEpisodeType)
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


    Public Function Create(seType As SousEpisodeSousType) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oa_r_sous_episode_sous_type " &
                    "(id_sous_episode_type, horodate_creation, libelle, redaction_profil_types, validation_profil_types, is_ald_possible, is_reponse_requise, delai_reponse, commentaire)" &
            " VALUES (@id_sous_episode_type, @dateCreation, @libelle , @redaction_profil_types, @validation_profil_types, @is_ald_possible, @is_reponse_requise, @delai_reponse, @commentaire)"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id_sous_episode_type", seType.IdSousEpisodeType)
                .AddWithValue("@dateCreation", seType.HorodateCreation)
                .AddWithValue("@libelle", seType.Libelle)
                .AddWithValue("@redaction_profil_types", seType.RedactionProfilTypes)
                .AddWithValue("@validation_profil_types", seType.ValidationProfilTypes)
                .AddWithValue("@is_ald_possible", seType.IsALDPossible)
                .AddWithValue("@is_reponse_requise", seType.IsReponseRequise)
                .AddWithValue("@delai_reponse", seType.DelaiReponse)
                .AddWithValue("@commentaire", seType.Commentaire)
            End With

            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return codeRetour
    End Function

    Private Function BuildBean(row As DataRow) As SousEpisodeSousType
        Dim seType As New SousEpisodeSousType(row)
        Return seType
    End Function

End Class