Imports System.Data.SqlClient
Imports System.IO

Public Class SousEpisodeReponseDao
    Inherits StandardDao

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idSousEpisodeReponse"></param>
    ''' <returns></returns>
    Public Function getLstSousEpisodeReponse(idSousEpisode As Long, Optional idSousEpisodeReponse As Long = 0) As List(Of SousEpisodeReponse)
        Dim lst As List(Of SousEpisodeReponse) = New List(Of SousEpisodeReponse)
        Dim data As DataTable = getTableSousEpisodeReponse(idSousEpisode, idSousEpisodeReponse)
        For Each row In data.Rows
            lst.Add(buildBean(row))
        Next
        Return lst
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idSousEpisode"></param>
    ''' <returns></returns>
    Public Function getTableSousEpisodeReponse(Optional idSousEpisode As Long = 0, Optional idSousEpisodeReponse As Long = 0) As DataTable
        Dim SQLString =
            "SELECT " & vbCrLf &
            "	  SER.id, " & vbCrLf &
            "     SER.id_sous_episode, " & vbCrLf &
            "     SER.create_user_id, " & vbCrLf &
            "     SER.horodate_creation, " & vbCrLf &
            "	  SER.nom_fichier, " & vbCrLf &
            "	  SER.commentaire, " & vbCrLf &
            "	  COALESCE(SER.validate_state,'') as validate_state, " & vbCrLf &
            "	  SER.validate_user_id, " & vbCrLf &
            "	  SER.validate_date, " & vbCrLf &
            "	  SER.episode_id, " & vbCrLf &
            "     UC.oa_utilisateur_prenom + ' ' + UC.oa_utilisateur_nom as user_create " & vbCrLf &
            "FROM oasis.oa_sous_episode_reponse SER " & vbCrLf &
            "JOIN oasis.oa_utilisateur UC ON UC.oa_utilisateur_id = SER.create_user_id " & vbCrLf &
            "WHERE 1=1 " & vbCrLf
        If idSousEpisode <> 0 Then
            SQLString += "AND id_sous_episode = @idSousEpisode " & vbCrLf
        End If
        If idSousEpisodeReponse <> 0 Then
            SQLString += "AND id = @id"
        End If

        Debug.WriteLine(SQLString)
        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                If idSousEpisode <> 0 Then
                    tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idSousEpisode", idSousEpisode)
                End If
                If idSousEpisodeReponse <> 0 Then
                    tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@id", idSousEpisodeReponse)
                End If
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

    Public Sub valider(sousEpisodeReponseId As Long, userLog As Utilisateur)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring = "UPDATE oasis.oa_sous_episode_reponse SET validate_state = 'v', validate_user_id = @validateUserId, validate_date = @validateDate WHERE id = @id"
            Dim cmd = New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id", sousEpisodeReponseId)
                .AddWithValue("@validateUserId", userLog.UtilisateurId)
                .AddWithValue("@validateDate", DateTime.Now)
            End With
            da.UpdateCommand = cmd
            Dim nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <> 1 Then
                Throw New Exception("Problème : Enregistrements mouvementés : " & nbUpdate & " au lieu de 1 !")
            End If

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try
    End Sub

    Public Sub askValider(sousEpisodeReponseId As Long, userLog As Utilisateur)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring = "UPDATE oasis.oa_sous_episode_reponse SET validate_state = 'm', validate_user_id = @validateUserId, validate_date = @validateDate WHERE id = @id"
            Dim cmd = New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id", sousEpisodeReponseId)
                .AddWithValue("@validateUserId", userLog.UtilisateurId)
                .AddWithValue("@validateDate", DateTime.Now)
            End With
            da.UpdateCommand = cmd
            Dim nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <> 1 Then
                Throw New Exception("Problème : Enregistrements mouvementés : " & nbUpdate & " au lieu de 1 !")
            End If

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try
    End Sub

    'TODO check SousEpisode -> SousEpisodeResponse
    Public Sub delete(sousEpisode As SousEpisode, idReponseRecue As Long, isDernier As Boolean)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "delete oasis.oa_sous_episode_reponse " &
                    "WHERE id = @id"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id", idReponseRecue)
            End With

            da.DeleteCommand = cmd
            Dim nb = da.DeleteCommand.ExecuteNonQuery()
            If nb <> 1 Then Throw New Exception("Pb suppression : " & nb & " au lieu de 1 enrigistrement supprimé")

            If isDernier Then
                ' -- update pere pour dire "pas de reponse recue"
                Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
                sousEpisodeDao.resetReponseRecue(con, sousEpisode, transaction)
            End If

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sousEpisode"></param>
    ''' <param name="sousEpisodeReponse"></param>
    ''' <param name="filenameSrc"></param>
    ''' <returns></returns>
    Public Function CreateByMoving(sousEpisode As SousEpisode, sousEpisodeReponse As SousEpisodeReponse, filenameSrc As String, loginRequestLog As Object) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_sous_episode_reponse " &
                    "(id_sous_episode, create_user_id, horodate_creation, nom_fichier, commentaire, validate_state, episode_id)" &
            " VALUES (@id_sous_episode, @CreateUser, @dateCreation, @NomFichier , @Commentaire, @ValidateState, (SELECT episode_id FROM oasis.oa_sous_episode WHERE id = @id_sous_episode)); SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id_sous_episode", sousEpisodeReponse.IdSousEpisode)
                .AddWithValue("@CreateUser", sousEpisodeReponse.CreateUserId)
                .AddWithValue("@dateCreation", sousEpisodeReponse.HorodateCreation)
                .AddWithValue("@NomFichier", sousEpisodeReponse.NomFichier)
                .AddWithValue("@Commentaire", sousEpisodeReponse.Commentaire)
                .AddWithValue("@ValidateState", "!")
            End With

            da.InsertCommand = cmd
            Dim idSEReponse = da.InsertCommand.ExecuteScalar()

            ' --- Update du record pere
            SQLstring = " UPDATE oasis.oa_sous_episode SET is_reponse_recue = 'true', horodate_last_recu = @dateCreation WHERE id = @id_sous_episode"
            cmd = New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id_sous_episode", sousEpisodeReponse.IdSousEpisode)
                .AddWithValue("@dateCreation", sousEpisodeReponse.HorodateCreation)
            End With
            da.UpdateCommand = cmd
            Dim nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <> 1 Then
                Throw New Exception("Problème : Enregistrements mouvementés : " & nbUpdate & " au lieu de 1 !")
            End If

            ' --- tentative de rename

            Using apiOasis As New ApiOasis()
                Dim renameRequest As New RenameRequest With {
                   .LoginRequest = loginRequestLog,
                   .OldName = filenameSrc,
                   .NewName = sousEpisodeReponse.GetFilenameServer(sousEpisode.EpisodeId, idSEReponse)}
                apiOasis.renameFileRest(renameRequest)
            End Using
            ' -- renane ok => on fixe l'id de la reponse
            sousEpisodeReponse.Id = idSEReponse

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

    Public Function Create(sousEpisode As SousEpisode, sousEpisodeReponse As SousEpisodeReponse, filenameSrc As String, loginRequestLog As Object) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_sous_episode_reponse " &
                    "(id_sous_episode, create_user_id, horodate_creation, nom_fichier, commentaire, validate_state, episode_id)" &
            " VALUES (@id_sous_episode, @CreateUser, @dateCreation, @NomFichier , @Commentaire, @ValidateState, (SELECT episode_id FROM oasis.oa_sous_episode WHERE id = @id_sous_episode)); SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id_sous_episode", sousEpisodeReponse.IdSousEpisode)
                .AddWithValue("@CreateUser", sousEpisodeReponse.CreateUserId)
                .AddWithValue("@dateCreation", sousEpisodeReponse.HorodateCreation)
                .AddWithValue("@NomFichier", sousEpisodeReponse.NomFichier)
                .AddWithValue("@Commentaire", sousEpisodeReponse.Commentaire)
                .AddWithValue("@ValidateState", "!")
            End With

            da.InsertCommand = cmd
            Dim idSEReponse = da.InsertCommand.ExecuteScalar()

            ' --- Update du record pere
            SQLstring = " UPDATE oasis.oa_sous_episode SET is_reponse_recue = 'true', horodate_last_recu = @dateCreation WHERE id = @id_sous_episode"
            cmd = New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id_sous_episode", sousEpisodeReponse.IdSousEpisode)
                .AddWithValue("@dateCreation", sousEpisodeReponse.HorodateCreation)
            End With
            da.UpdateCommand = cmd
            Dim nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <> 1 Then
                Throw New Exception("Problème : Enregistrements mouvementés : " & nbUpdate & " au lieu de 1 !")
            End If

            ' --- tentative d'upload
            Using apiOasis As New ApiOasis()
                apiOasis.uploadFileRest(loginRequestLog.login,
                                        loginRequestLog.password,
                                        sousEpisodeReponse.GetFilenameServer(sousEpisode.EpisodeId, idSEReponse),
                                        File.ReadAllBytes(filenameSrc))
            End Using
            ' -- upload ok => on fixe l'id de la reponse
            sousEpisodeReponse.Id = idSEReponse

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

    Public Function getContenu(filename As String, loginRequestLog As LoginRequest) As Byte()

        ' -- download
        Using apiOasis As New ApiOasis()
            Dim downloadRequest As New DownloadRequest With {
               .LoginRequest = loginRequestLog,
               .FileName = filename
               }
            Return apiOasis.downloadFileRest(downloadRequest)
        End Using

    End Function

    Public Function getContenu(idEpisode As Long, sousEpisodeReponse As SousEpisodeReponse, loginRequestLog As LoginRequest) As Byte()
        Dim filename = sousEpisodeReponse.GetFilenameServer(idEpisode)
        ' -- download
        Using apiOasis As New ApiOasis()
            Dim downloadRequest As New DownloadRequest With {
               .LoginRequest = loginRequestLog,
               .FileName = filename
               }
            Return apiOasis.downloadFileRest(downloadRequest)
        End Using

    End Function

    Public Function getById(idSousEpisodeReponse As Long) As SousEpisodeReponse
        Return getLstSousEpisodeReponse(0, idSousEpisodeReponse)(0)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="row"></param>
    ''' <returns></returns>
    Private Function buildBean(row As DataRow) As SousEpisodeReponse
        Dim seType As New SousEpisodeReponse(row)
        Return seType
    End Function

End Class
