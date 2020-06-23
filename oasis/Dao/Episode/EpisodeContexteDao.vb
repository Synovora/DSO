
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class EpisodeContexteDao
    Inherits StandardDao

    Friend Function GetAllEpisodeContexteByEpisodeId(episodeId As Long) As DataTable
        Dim SQLString As String
        SQLString = "SELECT episode_contexte_id, episode_id, patient_id, contexte_id, user_creation, date_creation, " &
                    " A.oa_antecedent_description, A.oa_antecedent_diagnostic," &
                    " A.oa_antecedent_date_modification, A.oa_antecedent_date_creation, A.oa_antecedent_categorie_contexte" &
                    " FROM oasis.oa_episode_contexte" &
                    " LEFT JOIN oasis.oa_antecedent A ON A.oa_antecedent_id = contexte_id" &
                    " WHERE episode_id = " & episodeId.ToString() &
                    " ORDER BY date_creation"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    da.Fill(dt)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return dt
    End Function

    Friend Function GetEpisodeById(episodeContexteId As Integer) As EpisodeContexte
        Dim episodeContexte As EpisodeContexte
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_episode_contexte WHERE episode_contexte_id = @id"
            command.Parameters.AddWithValue("@id", episodeContexteId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episodeContexte = BuildBean(reader)
                Else
                    Throw New ArgumentException("Contexte de l'épisode inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episodeContexte
    End Function

    Private Function BuildBean(reader As SqlDataReader) As EpisodeContexte
        Dim episodeContexte As New EpisodeContexte

        episodeContexte.EpisodeContexteId = reader("episode_contexte_id")
        episodeContexte.EpisodeId = Coalesce(reader("episode_id"), 0)
        episodeContexte.PatientId = Coalesce(reader("patient_id"), 0)
        episodeContexte.ContexteId = Coalesce(reader("contexte_id"), 0)
        episodeContexte.UserCreation = Coalesce(reader("user_creation"), 0)
        episodeContexte.DateCreation = Coalesce(reader("date_creation"), Nothing)
        Return episodeContexte
    End Function

    Friend Function CreateEpisodeContexte(episodeContexte As EpisodeContexte) As Boolean
        Dim nbcreate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim episodeIdCree As Integer = 0
        Dim CodeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String =
        "IF Not EXISTS (SELECT 1 FROM oasis.oa_episode_contexte WHERE episode_id = @episodeid And contexte_id = @contexteid)" &
        " INSERT INTO oasis.oa_episode_contexte" &
        " (patient_id, episode_id, contexte_id, user_creation, date_creation)" &
        " VALUES (@patientId, @episodeid, @contexteId, @userCreation, @dateCreation)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", episodeContexte.PatientId)
            .AddWithValue("@episodeId", episodeContexte.EpisodeId)
            .AddWithValue("@contexteId", episodeContexte.ContexteId)
            .AddWithValue("@userCreation", userLog.UtilisateurId)
            .AddWithValue("@dateCreation", Date.Now)
        End With

        Try
            da.InsertCommand = cmd
            nbcreate = da.InsertCommand.ExecuteNonQuery()
            If nbcreate <= 0 Then
                Throw New Exception("Collision: Le contexte existe déjà pour cet épisode patient")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            con.Close()
        End Try

        Return CodeRetour
    End Function

    Friend Function SuppressionEpisodeContexteById(episodeContexteId As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "DELETE oasis.oa_episode_contexte" &
        " WHERE episode_contexte_id = @episodeContexteId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@episodeContexteId", episodeContexteId)
        End With

        Try
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

End Class
