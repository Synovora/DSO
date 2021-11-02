Imports System.Data.SqlClient

Public Class ChaineEpisodeDao
    Inherits StandardDao

    Public Function GetList(Optional antecedentId As List(Of Long) = Nothing, Optional chaineId As List(Of Long) = Nothing) As List(Of ChaineEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim chaineEpisodes As List(Of ChaineEpisode) = New List(Of ChaineEpisode)
        Dim command As SqlCommand = con.CreateCommand()

        Try
            command.CommandText = "
            SELECT * From oasis.oa_chaine_episode, oasis.oa_antecedent" & vbCrLf &
            "WHERE oasis.oa_chaine_episode.antecedent_id = oasis.oa_antecedent.oa_antecedent_id " & vbCrLf
            If Not IsNothing(chaineId) AndAlso chaineId.Count > 0 Then
                Dim CEString As String = ""
                For i As Integer = 0 To chaineId.Count - 1 Step 1
                    CEString += String.Format("{0}{1}", chaineId(i), If(i = chaineId.Count - 1, "", ","))
                Next
                command.CommandText += "AND oasis.oa_chaine_episode.chaine_id IN (" & CEString & ")" & vbCrLf
            End If
            If Not IsNothing(antecedentId) AndAlso antecedentId.Count > 0 Then
                Dim CEString As String = ""
                For i As Integer = 0 To antecedentId.Count - 1 Step 1
                    CEString += String.Format("{0}{1}", antecedentId(i), If(i = antecedentId.Count - 1, "", ","))
                Next
                command.CommandText += "AND oasis.oa_chaine_episode.antecedent_id IN (" & CEString & ")" & vbCrLf
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

    Public Function GetList(Optional antecedentId As Long = Nothing, Optional chaineId As Long = Nothing) As List(Of ChaineEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim chaineEpisodes As List(Of ChaineEpisode) = New List(Of ChaineEpisode)
        Dim command As SqlCommand = con.CreateCommand()

        Try
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

    Public Function GetListByPatient(patientId As Integer, Optional chaineId As Long = Nothing, Optional antecedentId As Long = Nothing, Optional other As String = Nothing) As List(Of ChaineEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim chaineEpisodes As List(Of ChaineEpisode) = New List(Of ChaineEpisode)
        Dim command As SqlCommand = con.CreateCommand()

        Try
            command.CommandText = "
            SELECT * From oasis.oa_chaine_episode, oasis.oa_antecedent
            WHERE oasis.oa_chaine_episode.antecedent_id = oasis.oa_antecedent.oa_antecedent_id
            AND oasis.oa_antecedent.oa_antecedent_patient_id = " + patientId.ToString
            If chaineId <> Nothing Then
                command.CommandText += "AND oasis.oa_chaine_episode.chaine_id = " + If(chaineId = 0, "NULL", CStr(chaineId))
            End If
            If antecedentId <> Nothing Then
                command.CommandText += "AND oasis.oa_chaine_episode.antecedent_id = " + If(antecedentId = 0, "NULL", CStr(antecedentId))
            End If

            If other <> Nothing Then
                command.CommandText += other
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
        Dim command As SqlCommand = con.CreateCommand()

        Try
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

    Public Function Create(chaineEpisode As ChaineEpisode) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlTransaction = con.BeginTransaction
        Dim id As Long

        Try
            Dim cmd As New SqlCommand("INSERT INTO oasis.oa_chaine_episode " &
                    "(antecedent_id, chaine_id)" &
            " VALUES (@antecedent_id, @chaine_id); SELECT SCOPE_IDENTITY()", con, transaction)
            With cmd.Parameters
                .AddWithValue("@antecedent_id", N2N(Of Long)(chaineEpisode.AntecedentId))
                .AddWithValue("@chaine_id", N2N(Of Long)(chaineEpisode.ChaineId))
            End With

            da.InsertCommand = cmd
            id = da.InsertCommand.ExecuteNonQuery()
            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return id
    End Function

    Public Function Delete(chaineEpisode As ChaineEpisode) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim id As Long

        Dim cmd As New SqlCommand("DELETE FROM oasis.oa_chaine_episode" & vbCrLf &
                                  " WHERE chaine_id=@chaine_id AND antecedent_id=@antecedent_id;" & vbCrLf &
                                  " SELECT SCOPE_IDENTITY()", con)
        With cmd.Parameters
            .AddWithValue("@chaine_id", chaineEpisode.ChaineId)
            .AddWithValue("@antecedent_id", chaineEpisode.AntecedentId)
        End With
        Try
            da.UpdateCommand = cmd
            id = da.UpdateCommand.ExecuteScalar()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return id
    End Function

    Public Function AddRelation(relationChaineEpisode As RelationChaineEpisode) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlTransaction = con.BeginTransaction
        Dim id As Long

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_relation_chaine_episode (chaine_id, episode_id)" & vbCrLf &
            " VALUES (@chaine_id, @episode_id);" & vbCrLf &
            " SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@chaine_id", relationChaineEpisode.ChaineId)
                .AddWithValue("@episode_id", relationChaineEpisode.EpisodeId)
            End With

            da.InsertCommand = cmd
            id = da.InsertCommand.ExecuteNonQuery()
            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return id
    End Function

    Public Function DeleteRelation(relationChaineEpisode As RelationChaineEpisode) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim id As Long

        Dim cmd As New SqlCommand("DELETE FROM oasis.oa_relation_chaine_episode" & vbCrLf &
                                  " WHERE chaine_id=@chaine_id And episode_id=@episode_id;" & vbCrLf &
                                  " SELECT SCOPE_IDENTITY()", con)
        With cmd.Parameters
            .AddWithValue("@chaine_id", relationChaineEpisode.ChaineId)
            .AddWithValue("@episode_id", relationChaineEpisode.EpisodeId)
        End With
        Try
            da.UpdateCommand = cmd
            id = da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return id
    End Function

    'TODO: FIX error
    Public Function GetRelationListByPatient(patient As Patient) As List(Of RelationChaineEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim relationChaineEpisodes As List(Of RelationChaineEpisode) = New List(Of RelationChaineEpisode)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "
            SELECT oasis.oa_relation_chaine_episode.* From oasis.oa_chaine_episode, oasis.oa_antecedent, oasis.oa_relation_chaine_episode
            WHERE oasis.oa_chaine_episode.id = oasis.oa_antecedent.oa_antecedent_id
            AND oasis.oa_relation_chaine_episode.chaine_id = oasis.oa_chaine_episode.id
            AND oasis.oa_antecedent.oa_antecedent_patient_id = " + patient.PatientId.ToString
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

    Public Function GetRelationListByEpisode(episode As Episode) As List(Of RelationChaineEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim relationChaineEpisodes As List(Of RelationChaineEpisode) = New List(Of RelationChaineEpisode)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "
            SELECT oasis.oa_relation_chaine_episode.* From oasis.oa_relation_chaine_episode
            WHERE  oasis.oa_relation_chaine_episode.episode_id = " + episode.Id.ToString
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

    Public Function BuildBean(reader As SqlDataReader) As ChaineEpisode
        Return New ChaineEpisode(reader)
    End Function

    Public Function BuildBeanR(reader As SqlDataReader) As RelationChaineEpisode
        Return New RelationChaineEpisode(reader)
    End Function

End Class
