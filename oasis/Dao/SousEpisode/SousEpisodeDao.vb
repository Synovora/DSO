Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common

Public Class SousEpisodeDao
    Inherits StandardDao


    Public Function getLstSousEpisode(Optional idEpisode As Long = 0) As List(Of SousEpisode)
        Dim lst As List(Of SousEpisode) = New List(Of SousEpisode)
        Dim data As DataTable = getTableSousEpisode(idEpisode)
        For Each row In data.Rows
            lst.Add(buildBean(row))
        Next
        Return lst
    End Function

    Public Function getTableSousEpisode(Optional idEpisode As Long = 0, Optional isComplete As Boolean = False) As DataTable
        Dim SQLString As String
        'Console.WriteLine("----------> getTableSousEpisode")
        SQLString =
            "SELECT " & vbCrLf &
            "	  SE.id, " & vbCrLf &
            "     SE.episode_id, " & vbCrLf &
            "     SE.id_sous_episode_type, " & vbCrLf &
            "     SE.id_sous_episode_sous_type, " & vbCrLf &
            "     SE.create_user_id, " & vbCrLf &
            "     SE.horodate_creation, " & vbCrLf &
            "     SE.last_update_user_id, " & vbCrLf &
            "     SE.horodate_last_update, " & vbCrLf &
            "     SE.validate_user_id, " & vbCrLf &
            "     SE.horodate_validate, " & vbCrLf &
            "	  SE.nom_fichier, " & vbCrLf &
            "	  SE.commentaire, " & vbCrLf &
            "	  SE.is_ald, " & vbCrLf &
            "	  SE.is_reponse, " & vbCrLf &
            "	  SE.delai_since_validation " & vbCrLf
        If isComplete Then
            SQLString += "" &
            ",UC.oa_utilisateur_prenom + ' ' + UC.oa_utilisateur_nom as user_create,  " & vbCrLf &
            "UU.oa_utilisateur_prenom + ' ' + UU.oa_utilisateur_nom as user_update, " & vbCrLf &
            "UV.oa_utilisateur_prenom + ' ' + UV.oa_utilisateur_nom as user_validate, " & vbCrLf &
            "T.libelle as type_libelle, " & vbCrLf &
            "S.libelle as sous_type_libelle, " & vbCrLf &
            "S.redaction_profil_types, " & vbCrLf &
            "S.validation_profil_types " & vbCrLf
        End If

        SQLString += "FROM [oasis].[oa_sous_episode] SE " & vbCrLf
        If isComplete Then
            SQLString += "" &
            "Join oasis.oa_r_sous_episode_type T On T.id =SE.id_sous_episode_type " & vbCrLf &
            "Join oasis.oa_r_sous_episode_sous_type S ON S.id =SE.id_sous_episode_sous_type " & vbCrLf &
            "Join oasis.oa_utilisateur UC ON UC.oa_utilisateur_id =SE.create_user_id " & vbCrLf &
            "Left Join oasis.oa_utilisateur UU ON UC.oa_utilisateur_id =SE.last_update_user_id " & vbCrLf &
            "Left Join oasis.oa_utilisateur UV ON UV.oa_utilisateur_id =SE.validate_user_id " & vbCrLf
        End If

        If idEpisode <> 0 Then
            SQLString += "WHERE SE.episode_id= @idEpisode " & vbCrLf
        End If

        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                If idEpisode <> 0 Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idEpisode", idEpisode)
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

    Friend Function CountSousEpisode(selectedEpisodeId As Long) As Integer
        Dim SqlString As String = "SELECT COUNT(*) FROM oasis.oa_sous_episode WHERE episode_id=" & selectedEpisodeId

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = New SqlCommand(SqlString, con)
            Using command
                Return command.ExecuteScalar()
            End Using
        End Using
    End Function

    Friend Function ResumeSousEpisode(selectedEpisodeId As Long) As String
        Dim str As String = ""
        Dim dicCount As New Dictionary(Of Long, Integer)

        ' --- on compte les evenements par sous type
        Try
            Dim lst = getLstSousEpisode(selectedEpisodeId)
            For Each se In lst
                Dim i As Integer
                If dicCount.TryGetValue(se.IdSousEpisodeSousType, i) Then
                    dicCount(se.IdSousEpisodeSousType) = i + 1
                Else
                    dicCount.Add(se.IdSousEpisodeSousType, 1)
                End If
            Next
            If dicCount.Count > 0 Then
                Dim dictSTRef = New SousEpisodeSousTypeDao().getDictSousEpisodeSousType()
                For Each id In dicCount.Keys
                    str += If(str = "", "", vbCrLf) & dicCount(id).ToString & " " & dictSTRef(id).Libelle & If(dicCount(id) > 1, "s", "")
                Next
            End If
        Catch
        End Try
        Return str
    End Function

    Friend Function Create(sousEpisode As SousEpisode) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oa_sous_episode " &
                    "(episode_id , id_sous_episode_type , id_sous_episode_sous_type , create_user_id , horodate_creation , " &
                    " nom_fichier , commentaire , is_ald , is_reponse, delai_since_validation )" &
            " VALUES (@episode_id, @id_sous_episode_type, @id_sous_episode_sous_type, @create_user_id, @horodate_creation, " &
                     "@nom_fichier, @commentaire, @is_ald, @is_reponse, @delai_since_validation); SELECT SCOPE_IDENTITY()"

            sousEpisode.HorodateCreation = DateTime.Now
            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@episode_id", sousEpisode.EpisodeId)
                .AddWithValue("@id_sous_episode_type", sousEpisode.IdSousEpisodeType)
                .AddWithValue("@id_sous_episode_sous_type", sousEpisode.IdSousEpisodeSousType)
                .AddWithValue("@create_user_id", sousEpisode.CreateUserId)
                .AddWithValue("@horodate_creation", sousEpisode.HorodateCreation)

                .AddWithValue("@nom_fichier", sousEpisode.NomFichier)
                .AddWithValue("@commentaire", sousEpisode.Commentaire)
                .AddWithValue("@is_ald", sousEpisode.IsALD)
                .AddWithValue("@is_reponse", sousEpisode.IsReponse)
                .AddWithValue("@delai_since_validation", sousEpisode.DelaiSinceValidation)
            End With

            da.InsertCommand = cmd
            sousEpisode.Id = da.InsertCommand.ExecuteScalar()

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

    Friend Function updateValidation(idSousEpisode As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "UPDATE oasis.oa_sous_episode " &
                                      "SET validate_user_id = @ValidateUserId, horodate_validate = @HoroDateValidate " &
                                      "WHERE id=@Id"

            'SousEpisode.HorodateCreation = DateTime.Now
            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@ValidateUserId", userLog.UtilisateurId)
                .AddWithValue("@HoroDateValidate", DateTime.Now)
                .AddWithValue("@id", idSousEpisode)
            End With

            da.UpdateCommand = cmd
            Dim nb As Integer = da.UpdateCommand.ExecuteNonQuery()
            If (nb <> 1) Then
                Throw New Exception("Validation échouée (" & nb & ")")
            End If

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

    Private Function buildBean(row As DataRow) As SousEpisode
        Dim seType As New SousEpisode(row)
        Return seType
    End Function

End Class
