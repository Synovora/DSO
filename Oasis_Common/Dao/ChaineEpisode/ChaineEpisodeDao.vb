Imports System.Data.SqlClient

Public Class ChaineEpisodeDao
    Inherits StandardDao

    Public Function GetList(Optional antecedentId As Long = Nothing, Optional chaineId As Long = Nothing) As List(Of ChaineEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim chaineEpisodes As List(Of ChaineEpisode) = New List(Of ChaineEpisode)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "
            SELECT * From oasis.oa_chaine_episode, oasis.oa_antecedent" & vbCrLf &
            "WHERE oasis.oa_chaine_episode.antecedent_id = oasis.oa_antecedent.oa_antecedent_id " & vbCrLf
            If chaineId Then
                command.CommandText += "AND oasis.oa_chaine_episode.chaine_id = " & If(chaineId = 0, "NULL", CStr(chaineId)) & vbCrLf
            End If
            If antecedentId Then
                command.CommandText += "AND oasis.oa_chaine_episode.antecedent_id = " & If(antecedentId = 0, "NULL", CStr(antecedentId)) & vbCrLf
            End If

            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    chaineEpisodes.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return chaineEpisodes
    End Function

    Public Function GetListByPatient(patientId As Integer, Optional chaineId As Long = Nothing, Optional antecedentId As Long = Nothing) As List(Of ChaineEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim chaineEpisodes As List(Of ChaineEpisode) = New List(Of ChaineEpisode)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "
            SELECT * From oasis.oa_chaine_episode, oasis.oa_antecedent
            WHERE oasis.oa_chaine_episode.id = oasis.oa_antecedent.oa_antecedent_id
            AND oasis.oa_antecedent.oa_antecedent_patient_id = " + patientId.ToString
            If chaineId Then
                command.CommandText += "AND oasis.oa_chaine_episode.chaine_id = " + If(chaineId = 0, "NULL", CStr(chaineId))
            End If
            If antecedentId Then
                command.CommandText += "AND oasis.oa_chaine_episode.antecedent_id = " + If(antecedentId = 0, "NULL", CStr(antecedentId))
            End If

            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    chaineEpisodes.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return chaineEpisodes
    End Function

    Public Function GetById(id As Long) As ChaineEpisode
        Dim patientNote As ChaineEpisode
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()
            With command
                .CommandText = "SELECT * FROM oasis.oa_chaine_episode WHERE id=@id"
                .Parameters.AddWithValue("@id", id)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    patientNote = BuildBean(reader)
                Else
                    Throw New ArgumentException("ChaineEpisode inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return patientNote
    End Function

    Public Function Create(chaineEpisode As ChaineEpisode) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlTransaction = con.BeginTransaction
        Dim codeRetour As Boolean = True

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_chaine_episode " &
                    "(antecedent_id, chaine_id, actif)" &
            " VALUES (@antecedent_id, @chaine_id, @actif)"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@antecedent_id", If(chaineEpisode.AntecedentId <> Nothing, chaineEpisode.AntecedentId, DBNull.Value))
                .AddWithValue("@chaine_id", If(chaineEpisode.ChaineId <> Nothing, chaineEpisode.ChaineId, DBNull.Value))
                .AddWithValue("@actif", chaineEpisode.Actif)
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

    Public Sub Delete(chaineEpisode As ChaineEpisode)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "DELETE FROM oasis.oa_chaine_episode WHERE chaine_id=@chaine_id AND antecedent_id=@antecedent_id"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@chaine_id", chaineEpisode.ChaineId)
            .AddWithValue("@antecedent_id", chaineEpisode.AntecedentId)
        End With
        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Function AddRelation(relationChaineEpisode As RelationChaineEpisode) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlTransaction = con.BeginTransaction
        Dim codeRetour As Boolean = True

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_relation_chaine_episode " &
            "(chaine_id, episode_id)" &
            " VALUES (@chaine_id, @episode_id)"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@chaine_id", relationChaineEpisode.ChaineId)
                .AddWithValue("@episode_id", relationChaineEpisode.EpisodeId)
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

    Public Sub DeleteRelation(relationChaineEpisode As RelationChaineEpisode)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "DELETE FROM oasis.oa_relation_chaine_episode WHERE chaine_id=@chaine_id AND episode_id=@episode_id"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@chaine_id", relationChaineEpisode.ChaineId)
            .AddWithValue("@episode_id", relationChaineEpisode.EpisodeId)
        End With
        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Function GetRelationListByPatient(patientId As Integer) As List(Of RelationChaineEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim relationChaineEpisodes As List(Of RelationChaineEpisode) = New List(Of RelationChaineEpisode)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "
            SELECT oasis.oa_relation_chaine_episode.* From oasis.oa_chaine_episode, oasis.oa_antecedent, oasis.oa_relation_chaine_episode
            WHERE oasis.oa_chaine_episode.id = oasis.oa_antecedent.oa_antecedent_id
            AND oasis.oa_relation_chaine_episode.chaine_id = oasis.oa_chaine_episode.id
            AND oasis.oa_antecedent.oa_antecedent_patient_id = " + patientId.ToString
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    relationChaineEpisodes.Add(BuildBeanR(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return relationChaineEpisodes
    End Function

    'Public Function GetLstSousEpisodeReponseMail() As List(Of SousEpisodeReponseMail)
    '    Dim con As SqlConnection = GetConnection()
    '    Dim sousEpisodeReponseMails As List(Of SousEpisodeReponseMail) = New List(Of SousEpisodeReponseMail)

    '    Try
    '        Dim command As SqlCommand = con.CreateCommand()
    '        command.CommandText = "SELECT id, auteur, objet, status, horodate_creation, patient_id FROM oasis.oa_sous_episode_reponse_mail WHERE status='unprocessed'"
    '        Using reader As SqlDataReader = command.ExecuteReader()
    '            While (reader.Read())
    '                sousEpisodeReponseMails.Add(BuildBean(reader))
    '            End While
    '        End Using
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try
    '    Return sousEpisodeReponseMails
    'End Function

    'Public Function GetSousEpisodeReponseMailById(id As Long) As SousEpisodeReponseMail
    '    Dim patientNote As SousEpisodeReponseMail
    '    Dim con As SqlConnection = GetConnection()

    '    Try
    '        Dim command As SqlCommand = con.CreateCommand()
    '        command.CommandText = "SELECT * FROM oasis.oa_sous_episode_reponse_mail WHERE id=@id"
    '        command.Parameters.AddWithValue("@id", id)
    '        Using reader As SqlDataReader = command.ExecuteReader()
    '            If reader.Read() Then
    '                patientNote = BuildBean(reader)
    '            Else
    '                Throw New ArgumentException("SousEpisodeReponseMail inexistant !")
    '            End If
    '        End Using

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try

    '    Return patientNote
    'End Function

    'Public Sub DeleteSousEpisodeReponseMailById(id As Long)
    '    Dim da As SqlDataAdapter = New SqlDataAdapter()
    '    Dim con As SqlConnection = GetConnection()
    '    Dim SQLstring As String = "UPDATE oasis.oa_sous_episode_reponse_mail SET" &
    '    " status=@status" &
    '    " WHERE id=@id"

    '    Dim cmd As New SqlCommand(SQLstring, con)
    '    With cmd.Parameters
    '        .AddWithValue("@id", id)
    '        .AddWithValue("@status", "deleted")
    '    End With
    '    Try
    '        da.UpdateCommand = cmd
    '        da.UpdateCommand.ExecuteNonQuery()
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try
    'End Sub

    'Public Function ProcessSousEpisodeReponseMailById(id As Long)
    '    Dim da As SqlDataAdapter = New SqlDataAdapter()
    '    Dim con As SqlConnection = GetConnection()
    '    Dim SQLstring As String = "UPDATE oasis.oa_sous_episode_reponse_mail SET" &
    '    " status=@status" &
    '    " WHERE id=@id"

    '    Dim cmd As New SqlCommand(SQLstring, con)
    '    With cmd.Parameters
    '        .AddWithValue("@id", id)
    '        .AddWithValue("@status", "processed")
    '    End With
    '    Try
    '        da.UpdateCommand = cmd
    '        da.UpdateCommand.ExecuteNonQuery()
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try
    'End Function

    Public Function BuildBean(reader As SqlDataReader) As ChaineEpisode
        Return New ChaineEpisode(reader)
    End Function

    Public Function BuildBeanR(reader As SqlDataReader) As RelationChaineEpisode
        Return New RelationChaineEpisode(reader)
    End Function

End Class
