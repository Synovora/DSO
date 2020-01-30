Imports System.Data.SqlClient
Imports Oasis_Common
Public Class EpisodeParametreDao
    Inherits StandardDao

    Friend Function GetEpisodeParametreById(Id As Integer) As EpisodeParametre
        Dim episodeParametre As EpisodeParametre
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_episode_parametre WHERE episode_parametre_id = @id"
            command.Parameters.AddWithValue("@id", Id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episodeParametre = BuildBean(reader)
                Else
                    Throw New ArgumentException("épisode inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episodeParametre
    End Function

    Friend Function GetEpisodeParametreByParametreIdAndEpisodeId(parametreId As Integer, episodeId As Long) As EpisodeParametre
        Dim episodeParametre As EpisodeParametre
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_episode_parametre WHERE parametre_id = @parametreId AND episode_id = @episodeId"
            command.Parameters.AddWithValue("@parametreId", parametreId)
            command.Parameters.AddWithValue("@episodeId", episodeId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episodeParametre = BuildBean(reader)
                Else
                    episodeParametre = New EpisodeParametre
                    episodeParametre.Id = 0
                    episodeParametre.EpisodeId = 0
                    episodeParametre.ParametreId = 0
                    episodeParametre.PatientId = 0
                    episodeParametre.Valeur = 0
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episodeParametre
    End Function

    Private Function BuildBean(reader As SqlDataReader) As EpisodeParametre
        Dim episodeParametre As New EpisodeParametre

        episodeParametre.Id = reader("episode_parametre_id")
        episodeParametre.ParametreId = Coalesce(reader("parametre_id"), 0)
        episodeParametre.EpisodeId = Coalesce(reader("episode_id"), 0)
        episodeParametre.PatientId = Coalesce(reader("patient_id"), 0)
        episodeParametre.Valeur = Coalesce(reader("valeur"), 0)
        episodeParametre.Description = Coalesce(reader("description"), "")
        episodeParametre.Entier = Coalesce(reader("entier"), 0)
        episodeParametre.Decimal = Coalesce(reader("decimal"), 0)
        episodeParametre.Unite = Coalesce(reader("unite"), "")
        episodeParametre.ParametreAjoute = Coalesce(reader("parametre_ajoute"), False)
        episodeParametre.Ordre = Coalesce(reader("ordre"), 0)
        episodeParametre.Inactif = Coalesce(reader("inactif"), False)
        Return episodeParametre
    End Function

    Friend Function getAllParametreEpisodeByEpisodeId(episodeId As Long) As DataTable
        Dim SQLString As String

        SQLString = "SELECT * FROM oasis.oa_episode_parametre" &
        " WHERE episode_id = " & episodeId.ToString &
        " AND (inactif = '0' OR inactif is Null)" &
        " ORDER BY ordre"

        Using con As SqlConnection = GetConnection()
            Dim ContexteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ContexteDataAdapter
                ContexteDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        ContexteDataAdapter.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

    Friend Function CreateEpisodeParametre(episodeParametre As EpisodeParametre) As Boolean
        Dim nbcreate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF NOT EXISTS (SELECT 1 FROM oasis.oa_episode_parametre WHERE episode_id = @episodeId AND parametre_id = @parametreId AND (inactif = Null OR inactif = '0'))" &
        "INSERT INTO oasis.oa_episode_parametre" &
        " (parametre_id, episode_id, patient_id, valeur, description, entier," &
        " decimal, unite, parametre_ajoute, ordre, inactif)" &
        " VALUES (@parametreId, @episodeId, @patientId, @valeur, @description, @entier," &
        " @decimal, @unite, @parametreAjoute, @ordre, @inactif)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@parametreId", episodeParametre.ParametreId)
            .AddWithValue("@patientId", episodeParametre.PatientId)
            .AddWithValue("@episodeId", episodeParametre.EpisodeId)
            .AddWithValue("@valeur", episodeParametre.Valeur)
            .AddWithValue("@description", episodeParametre.Description)
            .AddWithValue("@entier", episodeParametre.Entier)
            .AddWithValue("@decimal", episodeParametre.Decimal)
            .AddWithValue("@unite", episodeParametre.Unite)
            .AddWithValue("@parametreAjoute", episodeParametre.ParametreAjoute)
            .AddWithValue("@ordre", episodeParametre.Ordre)
            .AddWithValue("@inactif", episodeParametre.Inactif)
        End With

        Try
            da.InsertCommand = cmd
            nbcreate = da.InsertCommand.ExecuteNonQuery()
            If nbcreate <= 0 Then
                Throw New Exception("Collision: Le paramètre existe déjà pour cet épisode")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ModificationEpisodeParametre(episodeParametre As EpisodeParametre) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_episode_parametre SET" &
        " parametre_id = @parametreId, episode_id = @episodeId, patient_id = @patientId," &
        " valeur = @valeur, description = @description," &
        " entier = @entier, decimal = @decimal," &
        " unite = @unite, ordre = @ordre, inactif = @inactif" &
        " where episode_parametre_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeParametre.Id)
            .AddWithValue("@patientId", episodeParametre.PatientId)
            .AddWithValue("@patientId", episodeParametre.PatientId)
            .AddWithValue("@episodeId", episodeParametre.EpisodeId)
            .AddWithValue("@valeur", episodeParametre.Valeur)
            .AddWithValue("@description", episodeParametre.Description)
            .AddWithValue("@entier", episodeParametre.Entier)
            .AddWithValue("@decimal", episodeParametre.Decimal)
            .AddWithValue("@unite", episodeParametre.Unite)
            .AddWithValue("@ordre", episodeParametre.Ordre)
            .AddWithValue("@inactif", episodeParametre.Inactif)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ModificationValeurEpisodeParametre(episodeParametreId As Long, Valeur As Decimal) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_episode_parametre SET" &
        " valeur = @valeur" &
        " WHERE episode_parametre_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeParametreId)
            .AddWithValue("@valeur", Valeur)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function AnnulationEpisodeParametre(episodeParametreId As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String =
        "UPDATE oasis.oa_episode_parametre" &
        " SET inactif = @inactif" &
        " WHERE episode_parametre_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeParametreId)
            .AddWithValue("@inactif", True)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function SuppressionEpisodeParametreByEpisodeId(episodeId As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "DELETE oasis.oa_episode_parametre" &
        " WHERE episode_id = @episodeId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@episodeId", episodeId)
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
