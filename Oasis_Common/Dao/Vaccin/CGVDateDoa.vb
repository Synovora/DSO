Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class CGVDateDao
    Inherits StandardDao

    Public Function GetById(dateId As Long) As CGVDate
        Dim newCgvDate As CGVDate = Nothing
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_date WHERE id = @dateId"
            command.Parameters.AddWithValue("@dateId", dateId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    newCgvDate = New CGVDate(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return newCgvDate
    End Function

    Public Function GetByDaysPatient(cgvDate As CGVDate) As CGVDate
        Dim newCgvDate As CGVDate = Nothing
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_date WHERE days = @days AND patient = @patient"
            command.Parameters.AddWithValue("@days", cgvDate.Days)
            command.Parameters.AddWithValue("@patient", cgvDate.Patient)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    newCgvDate = New CGVDate(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return newCgvDate
    End Function

    Public Function GetRelationIfExist(relationValenceDate As RelationValenceDate) As RelationValenceDate
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_relation_valence_date WHERE date = @date AND valence = @valence"
            command.Parameters.AddWithValue("@date", relationValenceDate.Date)
            command.Parameters.AddWithValue("@valence", relationValenceDate.Valence)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    Return New RelationValenceDate(reader)
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return Nothing
    End Function

    Public Function GetListFromPatient(patientId As Long) As List(Of CGVDate)
        Dim con As SqlConnection = GetConnection()
        Dim cgvDates As List(Of CGVDate) = New List(Of CGVDate)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_date WHERE patient=@patientId"
            command.Parameters.AddWithValue("@patientId", patientId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    cgvDates.Add(New CGVDate(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return cgvDates
    End Function

    Public Function Create(cgvDate As CGVDate) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim cgvDateId As Long

        Dim SQLstring As String = "
            INSERT into oasis.oa_vaccin_cgv_date (days, patient)
                VALUES (@days, @patient);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@days", cgvDate.Days)
            .AddWithValue("@patient", cgvDate.Patient)
        End With
        Try
            da.InsertCommand = cmd
            cgvDateId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return cgvDateId
    End Function

    Public Function Delete(cgvDate As CGVDate) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim cgvDateId As Long

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "
            DELETE FROM oasis.oa_vaccin_cgv_relation_valence_date WHERE date=@id;
            DELETE FROM oasis.oa_vaccin_cgv_date WHERE id=@id;
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", cgvDate.Id)
        End With
        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
            cgvDateId = cgvDate.Id
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return cgvDateId
    End Function

    Public Function Update(cgvDate As CGVDate) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim cgvDateId As Long

        Dim SQLstring As String = "UPDATE oasis.oa_vaccin_cgv_date SET days=@days," &
        " patient=@patient, operated_by=@operated_by, ordonnance_id=@ordonnance_id," &
        " operated_date=@operated_date WHERE id=@id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@id", cgvDate.Id)
            .AddWithValue("@days", cgvDate.Days)
            .AddWithValue("@patient", cgvDate.Patient)
            .AddWithValue("@operated_by", If(cgvDate.OperatedBy = Nothing, DBNull.Value, cgvDate.OperatedBy))
            .AddWithValue("@operated_date", If(cgvDate.OperatedDate = Nothing, DBNull.Value, cgvDate.OperatedDate))
            .AddWithValue("@ordonnance_id", If(cgvDate.OrdonnanceId = Nothing, DBNull.Value, cgvDate.OrdonnanceId))
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return cgvDateId
    End Function


    Public Function UpdateRelation(relationValenceDate As RelationValenceDate) As Long
        Dim da As New SqlDataAdapter()

        Dim SQLstring As String = "UPDATE oasis.oa_vaccin_cgv_relation_valence_date SET valence=@valence," &
        " date=@date, patient=@patient," &
        " status=@status WHERE id=@id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@id", relationValenceDate.Id)
            .AddWithValue("@valence", relationValenceDate.Valence)
            .AddWithValue("@date", relationValenceDate.Date)
            .AddWithValue("@patient", relationValenceDate.Patient)
            .AddWithValue("@status", relationValenceDate.Status)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return relationValenceDate.Id
    End Function

    Public Function UpdateRelationStatus(relationValenceDate As RelationValenceDate) As Long
        Dim da As New SqlDataAdapter()

        Dim SQLstring As String = "UPDATE oasis.oa_vaccin_cgv_relation_valence_date SET status=@status WHERE date=@date AND valence=@valence AND patient=@patient;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@id", relationValenceDate.Id)
            .AddWithValue("@valence", relationValenceDate.Valence)
            .AddWithValue("@date", relationValenceDate.Date)
            .AddWithValue("@patient", relationValenceDate.Patient)
            .AddWithValue("@status", relationValenceDate.Status)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return relationValenceDate.Id
    End Function

    Public Function CreateRelation(relationValenceDate As RelationValenceDate) As Long
        Dim da As New SqlDataAdapter()
        Dim relationId As Long

        Dim SQLstring As String = "
            INSERT INTO oasis.oa_vaccin_cgv_relation_valence_date (valence, date, patient, status)
                VALUES (@valence, @date, @patient, @status);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@valence", relationValenceDate.Valence)
            .AddWithValue("@date", relationValenceDate.Date)
            .AddWithValue("@patient", relationValenceDate.Patient)
            .AddWithValue("@status", relationValenceDate.Status)
        End With
        Try
            da.InsertCommand = cmd
            relationId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            relationId = 0
        Finally
            con.Close()
        End Try

        Return relationId
    End Function

    Public Function DeleteRelation(relationVaccinValence As RelationValenceDate) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim relationId As Long

        Dim SQLstring As String = "DELETE FROM oasis.oa_vaccin_cgv_relation_valence_date WHERE date=@date AND valence=@valence AND patient=@patient;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@valence", relationVaccinValence.Valence)
            .AddWithValue("@date", relationVaccinValence.Date)
            .AddWithValue("@patient", relationVaccinValence.Patient)
        End With
        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
            relationId = relationVaccinValence.Id
        Catch ex As Exception
            Throw New Exception(ex.Message)
            relationId = 0
        Finally
            con.Close()
        End Try

        Return relationId
    End Function

    Public Function GetRelationListFromPatient(patientId As Long) As List(Of RelationValenceDate)
        Dim con As SqlConnection = GetConnection()
        Dim cgvDates As List(Of RelationValenceDate) = New List(Of RelationValenceDate)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_relation_valence_date WHERE patient=@patientId"
            With command.Parameters
                .AddWithValue("@patientId", patientId)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    cgvDates.Add(New RelationValenceDate(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return cgvDates
    End Function

    Public Function GetRelationListByValence(valenceId As Long) As List(Of RelationValenceDate)
        Dim con As SqlConnection = GetConnection()
        Dim cgvDates As List(Of RelationValenceDate) = New List(Of RelationValenceDate)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_cgv_relation_valence_date WHERE valence=@valenceId"
            With command.Parameters
                .AddWithValue("@valenceId", valenceId)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    cgvDates.Add(New RelationValenceDate(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return cgvDates
    End Function

    'Public Function GetListByVaccin(vaccinId As Long) As List(Of RelationVaccinValence)
    '    Dim con As SqlConnection = GetConnection()
    '    Dim relations As List(Of RelationVaccinValence) = New List(Of RelationVaccinValence)

    '    Try
    '        Dim command As SqlCommand = con.CreateCommand()
    '        command.CommandText = "SELECT * FROM oasis.oa_relation_vaccin_valence WHERE vaccin=@vaccinId"

    '        With command.Parameters
    '            .AddWithValue("@vaccinId", vaccinId)
    '        End With
    '        Using reader As SqlDataReader = command.ExecuteReader()
    '            While (reader.Read())
    '                relations.Add(New RelationVaccinValence(reader))
    '            End While
    '        End Using
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try

    '    Return relations
    'End Function

End Class
