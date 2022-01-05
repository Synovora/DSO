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
            command.CommandText = "SELECT cgv_valence.*, valence.code, valence.description, valence.precaution FROM oasis.oa_vaccin_cgv_valence cgv_valence LEFT JOIN oasis.oa_valence valence ON valence.id = cgv_valence.valence WHERE cgv_valence.patient=@patient AND cgv_valence.valence=@valence"
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
            command.CommandText = "SELECT cgv_valence.*, valence.code, valence.description, valence.precaution FROM oasis.oa_vaccin_cgv_valence cgv_valence LEFT JOIN oasis.oa_valence valence ON valence.id = cgv_valence.valence WHERE cgv_valence.id = @id"
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
            command.CommandText = "SELECT cgv_valence.*, valence.code, valence.description, valence.precaution FROM oasis.oa_vaccin_cgv_valence cgv_valence LEFT JOIN oasis.oa_valence valence ON valence.id = cgv_valence.valence WHERE patient=@patientId ORDER BY cgv_valence.ordre ASC"
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
            command.CommandText = "SELECT cgv_valence.*, valence.code, valence.description, valence.precaution FROM oasis.oa_vaccin_cgv_valence cgv_valence LEFT JOIN oasis.oa_valence valence ON valence.id = cgv_valence.valence ORDER BY ordre ASC"
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
            command.CommandText = "SELECT TOP 1 cgv_valence.*, valence.code, valence.description, valence.precaution FROM oasis.oa_vaccin_cgv_valence cgv_valence LEFT JOIN oasis.oa_valence valence ON valence.id = cgv_valence.valence ORDER BY ordre DESC"
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
            command.CommandText = "SELECT TOP 1 cgv_valence.*, valence.code, valence.description, valence.precaution FROM oasis.oa_vaccin_cgv_valence cgv_valence LEFT JOIN oasis.oa_valence valence ON valence.id = cgv_valence.valence WHERE cgv_valence.ordre = @ordre"
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
            command.CommandText = "SELECT cgv_valence.*, valence.code, valence.description, valence.precaution FROM oasis.oa_vaccin_cgv_valence cgv_valence LEFT JOIN oasis.oa_valence valence ON valence.id = cgv_valence.valence WHERE cgv_valence.ordre >= @ordre"
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
            INSERT into oasis.oa_vaccin_cgv_valence (valence, patient, ordre)
                VALUES (@valence, @patient, @ordre);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)
        Dim lastValence = GetLastOrder()

        With cmd.Parameters
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

    'Public Function DeleteByValence(valenceId As Long) As Long
    '    Dim da As SqlDataAdapter = New SqlDataAdapter()
    '    Dim valenceId As Long

    '    Dim dateCreation As Date = Date.Now.Date

    '    Dim SQLstring As String = "
    '        DELETE FROM oasis.oa_relation_vaccin_valence WHERE valence=@id;
    '        DELETE FROM oasis.oa_valence WHERE id=@id;
    '    "

    '    Dim con As SqlConnection = GetConnection()
    '    Dim cmd As New SqlCommand(SQLstring, con)

    '    With cmd.Parameters
    '        .AddWithValue("@id", Valence.Id)
    '    End With
    '    Try
    '        da.InsertCommand = cmd
    '        da.InsertCommand.ExecuteScalar()
    '        valenceId = Valence.Id
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '        valenceId = 0
    '    Finally
    '        con.Close()
    '    End Try

    '    Return valenceId
    'End Function

    Public Function Update(valence As CGVValence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()

        Dim SQLstring As String = "UPDATE oasis.oa_vaccin_cgv_valence SET" &
        " patient = @patient, ordre = @ordre WHERE id = @id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", valence.Id)
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
