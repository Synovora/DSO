Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common
Imports System.IO

Public Class SousEpisodeReponseDao
    Inherits StandardDao

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idSousEpisodeReponse"></param>
    ''' <returns></returns>
    Public Function GetLstSousEpisodeReponse(idSousEpisode As Long, Optional idSousEpisodeReponse As Long = 0) As List(Of SousEpisodeReponse)
        Dim lst As List(Of SousEpisodeReponse) = New List(Of SousEpisodeReponse)
        Dim data As DataTable = GetTableSousEpisodeReponse(idSousEpisode, idSousEpisodeReponse)
        For Each row In data.Rows
            lst.Add(BuildBean(row))
        Next
        Return lst
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idSousEpisode"></param>
    ''' <returns></returns>
    Public Function GetTableSousEpisodeReponse(Optional idSousEpisode As Long = 0, Optional idSousEpisodeReponse As Long = 0) As DataTable
        Dim SQLString As String
        'Console.WriteLine("----------> getAllTacheEnCours")
        SQLString =
            "SELECT " & vbCrLf &
            "	  SER.id, " & vbCrLf &
            "     SER.id_sous_episode, " & vbCrLf &
            "     SER.create_user_id, " & vbCrLf &
            "     SER.horodate_creation, " & vbCrLf &
            "	  SER.nom_fichier, " & vbCrLf &
            "	  SER.commentaire, " & vbCrLf &
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
        'Console.WriteLine(SQLString)

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

    Friend Sub Delete(sousEpisode As SousEpisode, idReponseRecue As Long, isDernier As Boolean)
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
            MessageBox.Show(ex.Message)
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
    Friend Function Create(sousEpisode As SousEpisode, sousEpisodeReponse As SousEpisodeReponse, filenameSrc As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_sous_episode_reponse " &
                    "(id_sous_episode, create_user_id, horodate_creation, nom_fichier, commentaire)" &
            " VALUES (@id_sous_episode, @CreateUser, @dateCreation, @NomFichier , @Commentaire); SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id_sous_episode", sousEpisodeReponse.IdSousEpisode)
                .AddWithValue("@CreateUser", sousEpisodeReponse.CreateUserId)
                .AddWithValue("@dateCreation", sousEpisodeReponse.HorodateCreation)
                .AddWithValue("@NomFichier", sousEpisodeReponse.NomFichier)
                .AddWithValue("@Commentaire", sousEpisodeReponse.Commentaire)
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
                                        sousEpisodeReponse.getFilenameServer(sousEpisode.EpisodeId, idSEReponse),
                                        File.ReadAllBytes(filenameSrc))
            End Using
            ' -- upload ok => on fixe l'id de la reponse
            sousEpisodeReponse.Id = idSEReponse

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

    Public Function GetContenu(idEpisode As Long, sousEpisodeReponse As SousEpisodeReponse) As Byte()
        Dim filename = sousEpisodeReponse.getFilenameServer(idEpisode)
        ' -- download
        Using apiOasis As New ApiOasis()
            Dim downloadRequest As New DownloadRequest With {
               .LoginRequest = loginRequestLog,
               .FileName = filename
               }
            Return apiOasis.downloadFileRest(downloadRequest)
        End Using

    End Function

    Friend Function GetById(idSousEpisodeReponse As Long) As SousEpisodeReponse
        Return GetLstSousEpisodeReponse(0, idSousEpisodeReponse)(0)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="row"></param>
    ''' <returns></returns>
    Private Function BuildBean(row As DataRow) As SousEpisodeReponse
        Dim seType As New SousEpisodeReponse(row)
        Return seType
    End Function

End Class
