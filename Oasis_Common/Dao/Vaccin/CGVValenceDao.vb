Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class CGVValenceDao
    Inherits StandardDao

    Public Function GetByValencePatient(cgvValence As CGVValence) As CGVValence
        Dim valence As CGVValence
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_valence WHERE patient=@patient AND valence=@valence"
            command.Parameters.AddWithValue("@patient", cgvValence.Patient)
            command.Parameters.AddWithValue("@valence", cgvValence.Valence)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    valence = New CGVValence(reader)
                Else
                    Throw New ArgumentException("CGVValence inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return valence
    End Function

    Public Function GetById(valenceId As Integer) As CGVValence
        Dim valence As CGVValence
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_valence WHERE id = @id"
            command.Parameters.AddWithValue("@id", valenceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    valence = New CGVValence(reader)
                Else
                    Throw New ArgumentException("CGVValence inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return valence
    End Function

    Public Function GetListFromPatient(patientId As Long) As List(Of CGVValence)
        Dim con As SqlConnection = GetConnection()
        Dim valences As List(Of CGVValence) = New List(Of CGVValence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_valence WHERE patient=@patientId ORDER BY ordre ASC"
            With command.Parameters
                .AddWithValue("@patientId", patientId)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    valences.Add(New CGVValence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valences
    End Function

    Public Function GetList() As List(Of CGVValence)
        Dim con As SqlConnection = GetConnection()
        Dim valences As List(Of CGVValence) = New List(Of CGVValence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_valence ORDER BY ordre ASC"
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    valences.Add(New CGVValence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valences
    End Function

    Public Function GetLastOrder() As CGVValence
        Dim con As SqlConnection = GetConnection()
        Dim valence As CGVValence = Nothing

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT TOP 1 * FROM oasis.oa_vaccin_cgv_valence ORDER BY ordre DESC"
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    valence = New CGVValence(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valence
    End Function

    Public Function GetByOrder(order As Integer) As CGVValence
        Dim con As SqlConnection = GetConnection()
        Dim valence As CGVValence = Nothing

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT TOP 1 * FROM oasis.oa_vaccin_cgv_valence WHERE ordre = @ordre"
            With command.Parameters
                .AddWithValue("@ordre", order)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    valence = New CGVValence(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valence
    End Function

    Public Function GetListFromOrder(order As Integer) As List(Of CGVValence)
        Dim con As SqlConnection = GetConnection()
        Dim valences As List(Of CGVValence) = New List(Of CGVValence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_valence WHERE ordre >= @ordre"
            With command.Parameters
                .AddWithValue("@ordre", order)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    valences.Add(New CGVValence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valences
    End Function

    Public Function Create(valence As CGVValence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim valenceId As Long

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "
            INSERT into oasis.oa_vaccin_cgv_valence (code, description, precaution, valence, patient, ordre)
                VALUES (@code, @description, @precaution, @valence, @patient, @ordre);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)
        Dim lastValence = GetLastOrder()

        With cmd.Parameters
            .AddWithValue("@code", valence.Code)
            .AddWithValue("@description", valence.Description)
            .AddWithValue("@precaution", valence.Precaution)
            .AddWithValue("@valence", valence.Valence)
            .AddWithValue("@patient", valence.Patient)
            .AddWithValue("@ordre", If(lastValence IsNot Nothing, lastValence.Ordre + 1, 0))
        End With
        Try
            da.InsertCommand = cmd
            valenceId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return valenceId
    End Function

    Public Function Delete(valence As CGVValence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim valenceId As Long

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "
            DELETE FROM oasis.oa_vaccin_cgv_valence WHERE id=@id;
        "
        'DELETE FROM oasis.oa_relation_vaccin_valence WHERE valence=@id;

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", valence.Id)
        End With
        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
            valenceId = valence.Id
        Catch ex As Exception
            Throw New Exception(ex.Message)
            valenceId = 0
        Finally
            con.Close()
        End Try

        Return valenceId
    End Function

    Public Function Update(valence As CGVValence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()

        Dim SQLstring As String = "UPDATE oasis.oa_vaccin_cgv_valence SET code = @code," &
        " description = @description, precaution = @precaution," &
        " patient = @patient, ordre = @ordre WHERE id = @id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", valence.Id)
            .AddWithValue("@code", valence.Code)
            .AddWithValue("@description", valence.Description)
            .AddWithValue("@precaution", valence.Precaution)
            .AddWithValue("@valence", valence.Valence)
            .AddWithValue("@patient", valence.Patient)
            .AddWithValue("@ordre", valence.Ordre)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return valence.Id
    End Function

    Public Function SetOrder(valenceId As Long, ordre As Integer) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()

        Dim SQLstring As String = "UPDATE oasis.oa_vaccin_cgv_valence SET ordre = @ordre WHERE id = @id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", valenceId)
            .AddWithValue("@ordre", ordre)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return valenceId
    End Function

End Class
