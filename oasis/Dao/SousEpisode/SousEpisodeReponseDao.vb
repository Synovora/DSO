Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common

Public Class SousEpisodeReponseDao
    Inherits StandardDao


    Public Function getLstSousEpisodeReponse(Optional idSousEpisodeType As Long = 0) As List(Of SousEpisodeReponse)
        Dim lst As List(Of SousEpisodeReponse) = New List(Of SousEpisodeReponse)
        Dim data As DataTable = getTableSousEpisodeReponse(idSousEpisodeType)
        For Each row In data.Rows
            lst.Add(buildBean(row))
        Next
        Return lst
    End Function

    Public Function getTableSousEpisodeReponse(idSousEpisode As Long) As DataTable
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
            "WHERE id_sous_episode = @idSousEpisode " & vbCrLf

        'Console.WriteLine(SQLString)

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


    Friend Function Create(seType As SousEpisodeReponse) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oa_r_sous_episode_reponse " &
                    "(id_sous_episode, create_user, horodate_creation, nom_fichier, commentaire)" &
            " VALUES (@id_sous_episode, @CreateUser, @dateCreation, @NomFichier , @Commentaire)"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@id_sous_episode", seType.IdSousEpisode)
                .AddWithValue("@CreateUser", seType.CreateUserId)
                .AddWithValue("@dateCreation", seType.HorodateCreation)
                .AddWithValue("@NomFichier", seType.NomFichier)
                .AddWithValue("@Commentaire", seType.Commentaire)
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

    Private Function buildBean(row As DataRow) As SousEpisodeReponse
        Dim seType As New SousEpisodeReponse(row)
        Return seType
    End Function

End Class
