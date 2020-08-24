﻿Imports System.Data.SqlClient

Public Class SousEpisodeDao
    Inherits StandardDao

    Public Function getLstSousEpisode(idEpisode As Long, Optional idSousEpisode As Long = 0, Optional isWithInactif As Boolean = False) As List(Of SousEpisode)
        Dim lst As List(Of SousEpisode) = New List(Of SousEpisode)
        Dim data As DataTable = getTableSousEpisode(idEpisode, idSousEpisode, isWithInactif)
        For Each row In data.Rows
            lst.Add(buildBean(row))
        Next
        Return lst
    End Function

    Public Function getTableSousEpisode(idEpisode As Long, Optional idSousEpisode As Long = 0, Optional isComplete As Boolean = False, Optional isWithInactif As Boolean = False) As DataTable
        Dim SQLString As String
        'Console.WriteLine("----------> getTableSousEpisode")
        SQLString =
            "SELECT " & vbCrLf &
            "	  SE.id, " & vbCrLf &
            "     SE.episode_id, " & vbCrLf &
            "     SE.id_intervenant, " & vbCrLf &
            "     SE.id_sous_episode_type, " & vbCrLf &
            "     SE.id_sous_episode_sous_type, " & vbCrLf &
            "     SE.create_user_id, " & vbCrLf &
            "     SE.horodate_creation, " & vbCrLf &
            "     SE.last_update_user_id, " & vbCrLf &
            "     SE.horodate_last_update, " & vbCrLf &
            "     SE.validate_user_id, " & vbCrLf &
            "     SE.horodate_validate, " & vbCrLf &
            "	  SE.commentaire, " & vbCrLf &
            "	  SE.is_ald, " & vbCrLf &
            "	  SE.is_reponse, " & vbCrLf &
            "	  SE.delai_since_validation, " & vbCrLf &
            "	  SE.is_reponse_recue, " & vbCrLf &
            "	  SE.horodate_last_recu, " & vbCrLf &
            "	  SE.is_inactif " & vbCrLf

        If isComplete Then
            SQLString += "" &
            ",UC.oa_utilisateur_prenom + ' ' + UC.oa_utilisateur_nom as user_create,  " & vbCrLf &
            "UU.oa_utilisateur_prenom + ' ' + UU.oa_utilisateur_nom as user_update, " & vbCrLf &
            "UV.oa_utilisateur_prenom + ' ' + UV.oa_utilisateur_nom as user_validate, " & vbCrLf &
            "T.libelle as type_libelle, " & vbCrLf &
            "S.libelle as sous_type_libelle, " & vbCrLf &
            "S.redaction_profil_types, " & vbCrLf &
            "S.validation_profil_types, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id ) as nb_reponse " & vbCrLf
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

        SQLString += "WHERE 1=1 " & vbCrLf

        If idEpisode <> 0 Then
            SQLString += "AND SE.episode_id= @idEpisode " & vbCrLf
        End If

        If idSousEpisode <> 0 Then
            SQLString += "AND SE.id= @idSousEpisode " & vbCrLf
        End If

        If isWithInactif = False Then
            SQLString += "AND SE.is_inactif= @is_inactif " & vbCrLf
        End If

        SQLString += "ORDER by SE.id DESC"

        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                If idSousEpisode <> 0 Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idSousEpisode", idSousEpisode)
                If idEpisode <> 0 Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idEpisode", idEpisode)
                If isWithInactif = False Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@is_inactif", False)
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

    Public Function CountSousEpisode(selectedEpisodeId As Long, Optional isWithInactif As Boolean = False) As Integer
        Dim SqlString As String = "SELECT COUNT(*) FROM oasis.oa_sous_episode WHERE episode_id=" & selectedEpisodeId &
            If(isWithInactif, "", " AND is_inactif = 0 ")

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = New SqlCommand(SqlString, con)
            Using command
                Return command.ExecuteScalar()
            End Using
        End Using
    End Function

    Public Function ResumeSousEpisode(selectedEpisodeId As Long, Optional isWithInactif As Boolean = False) As String
        Dim str As String = ""
        Dim dicCount As New Dictionary(Of Long, Integer)

        ' --- on compte les evenements par sous type
        Try
            Dim lst = getLstSousEpisode(selectedEpisodeId,, isWithInactif)
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

    Public Sub resetReponseRecue(con As SqlConnection, sousEpisode As SousEpisode, transaction As SqlTransaction)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim SQLstring = " UPDATE oasis.oa_sous_episode SET is_reponse_recue = 'false', horodate_last_recu = @dateLastRecu WHERE id = @id"
        Dim cmd = New SqlCommand(SQLstring, con, transaction)
        With cmd.Parameters
            .AddWithValue("@id", sousEpisode.Id)
            .AddWithValue("@dateLastRecu", DBNull.Value)
        End With
        da.UpdateCommand = cmd
        Dim nbUpdate = da.UpdateCommand.ExecuteNonQuery()
        If nbUpdate <> 1 Then
            Throw New Exception("Problème : Enregistrements mouvementés : " & nbUpdate & " au lieu de 1 !")
        End If


    End Sub

    Public Function Create(sousEpisode As SousEpisode) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_sous_episode " &
                    "(episode_id , id_intervenant, id_sous_episode_type , id_sous_episode_sous_type , create_user_id , horodate_creation , " &
                    " commentaire , is_ald , is_reponse, delai_since_validation )" &
            " VALUES (@episode_id, @id_intervenant, @id_sous_episode_type, @id_sous_episode_sous_type, @create_user_id, @horodate_creation, " &
                     " @commentaire, @is_ald, @is_reponse, @delai_since_validation); SELECT SCOPE_IDENTITY()"

            sousEpisode.HorodateCreation = DateTime.Now
            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@episode_id", sousEpisode.EpisodeId)
                .AddWithValue("@id_intervenant", If(sousEpisode.IdIntervenant = 0, DBNull.Value, sousEpisode.IdIntervenant))
                .AddWithValue("@id_sous_episode_type", sousEpisode.IdSousEpisodeType)
                .AddWithValue("@id_sous_episode_sous_type", sousEpisode.IdSousEpisodeSousType)
                .AddWithValue("@create_user_id", sousEpisode.CreateUserId)
                .AddWithValue("@horodate_creation", sousEpisode.HorodateCreation)

                .AddWithValue("@commentaire", sousEpisode.Commentaire)
                .AddWithValue("@is_ald", sousEpisode.IsALD)
                .AddWithValue("@is_reponse", sousEpisode.IsReponse)
                .AddWithValue("@delai_since_validation", sousEpisode.DelaiSinceValidation)
            End With

            da.InsertCommand = cmd
            sousEpisode.Id = da.InsertCommand.ExecuteScalar()

            ' -- on fixe les idSousEpisode de la table fille (details)
            If sousEpisode.lstDetail.Count > 0 Then
                Dim sousEpisodeDetailSousTypeDao As SousEpisodeDetailSousTypeDao = New SousEpisodeDetailSousTypeDao
                For Each detail In sousEpisode.lstDetail
                    detail.IdSousEpisode = sousEpisode.Id
                    sousEpisodeDetailSousTypeDao.Create(con, detail, transaction)
                Next
            End If

            ' --- process de signature 
            'If isWithSign Then
            ' updateValidation(con, sousEpisode.Id, transaction)
            ' sousEpisode.HorodateValidate = Date.Now
            ' sousEpisode.ValidateUserId = userLog.UtilisateurId
            'End If

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


    Public Sub writeDocAndEventualySign(sousEpisode As SousEpisode, tbl As Byte(), signature As String, dateSignature As Date, userLog As Utilisateur, loginRequestLog As Object)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlTransaction = con.BeginTransaction

        Try
            ' -- 1 : enregistrement en base si signataure
            If String.IsNullOrEmpty(signature) = False Then
                dateSignature = DateTime.Now
                Dim SQLstring As String = "UPDATE oasis.oa_sous_episode " &
                                      "SET validate_user_id = @ValidateUserId, horodate_validate = @HoroDateValidate, signature = @Signature " &
                                      "WHERE id=@Id"

                Dim cmd As New SqlCommand(SQLstring, con, transaction)
                With cmd.Parameters
                    .AddWithValue("@ValidateUserId", userLog.UtilisateurId)
                    .AddWithValue("@HoroDateValidate", dateSignature)
                    .AddWithValue("@id", sousEpisode.Id)
                    .AddWithValue("@Signature", signature)
                End With

                Console.WriteLine("@Signature: " & signature)

                da.UpdateCommand = cmd
                Dim nb As Integer = da.UpdateCommand.ExecuteNonQuery()
                If (nb <> 1) Then
                    Throw New Exception("Validation échouée (" & nb & ")")
                End If
            End If

            ' -- 2 : serialisation du fichier
            sousEpisode.WriteContenuModel(tbl, loginRequestLog)

            ' -- 3 : commit et maj bean si signature
            If String.IsNullOrEmpty(signature) = False Then
                transaction.Commit()
                sousEpisode.ValidateUserId = userLog.UtilisateurId
                sousEpisode.HorodateValidate = dateSignature
            End If


        Catch ex As Exception
            If String.IsNullOrEmpty(signature) = False Then transaction.Rollback()
            Throw ex
        Finally
            transaction.Dispose()
            con.Close()
        End Try

    End Sub

    Public Function updateValidation(con As SqlConnection, idSousEpisode As Long, transaction As SqlTransaction, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim isMyTransaction As Boolean = (transaction Is Nothing)
        If isMyTransaction Then
            con = GetConnection()
            transaction = con.BeginTransaction
        End If


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

            If isMyTransaction Then transaction.Commit()

        Catch ex As Exception
            If isMyTransaction Then
                transaction.Rollback()
            Else
                Throw ex    ' on remonte l'excaption a l'appelant
            End If
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            If isMyTransaction Then transaction.Dispose()
            If isMyTransaction Then con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function inactiverSousEpisode(con As SqlConnection, idSousEpisode As Long, transaction As SqlTransaction) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim isMyTransaction As Boolean = (transaction Is Nothing)
        If isMyTransaction Then
            con = GetConnection()
            transaction = con.BeginTransaction
        End If

        Try
            Dim SQLstring As String = "UPDATE oasis.oa_sous_episode " &
                                      "SET is_inactif = @is_inactif " &
                                      "WHERE id=@Id"

            'SousEpisode.HorodateCreation = DateTime.Now
            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@is_inactif", True)
                .AddWithValue("@id", idSousEpisode)
            End With

            da.UpdateCommand = cmd
            Dim nb As Integer = da.UpdateCommand.ExecuteNonQuery()
            If (nb <> 1) Then
                Throw New Exception("Inactivation échouée (" & nb & ")")
            End If

            If isMyTransaction Then transaction.Commit()

        Catch ex As Exception
            If isMyTransaction Then
                transaction.Rollback()
            Else
                Throw ex    ' on remonte l'excaption a l'appelant
            End If
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            If isMyTransaction Then transaction.Dispose()
            If isMyTransaction Then con.Close()
        End Try

        Return codeRetour
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idSousEpisode"></param>
    ''' <returns></returns>
    Public Function getById(idSousEpisode As Long) As SousEpisode
        Return getLstSousEpisode(0, idSousEpisode, True)(0)
    End Function

    Private Function buildBean(row As DataRow) As SousEpisode
        Dim seType As New SousEpisode(row)
        Return seType
    End Function

End Class