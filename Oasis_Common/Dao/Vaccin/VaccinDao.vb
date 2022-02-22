Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class VaccinDao
    Inherits StandardDao

    Public Function GetById(vaccinId As Integer) As Vaccin
        Dim vaccin As Vaccin
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin WHERE id = @id"
            command.Parameters.AddWithValue("@id", vaccinId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    vaccin = New Vaccin(reader)
                Else
                    Throw New ArgumentException("Vaccin inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return vaccin
    End Function

    Public Function GetByCode(vaccinCode As String) As Vaccin
        Dim vaccin As Vaccin = Nothing
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin WHERE code = @vaccinCode"
            command.Parameters.AddWithValue("@vaccinCode", vaccinCode)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    vaccin = New Vaccin(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return vaccin
    End Function

    Public Function GetList() As List(Of Vaccin)
        Dim con As SqlConnection = GetConnection()
        Dim vaccins As List(Of Vaccin) = New List(Of Vaccin)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin"
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    vaccins.Add(New Vaccin(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return vaccins
    End Function

    Public Function Create(vaccin As Vaccin) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim vaccinId As Long

        Dim SQLstring As String = "
            INSERT into oasis.oa_vaccin (code, code_atc, dci, dci_longue, date_import, utilisateur_import)
                VALUES (@code, @code_atc, @dci, @dci_longue, @date_import, @utilisateur_import);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@code", vaccin.Code)
            .AddWithValue("@code_atc", vaccin.CodeAtc)
            .AddWithValue("@dci", vaccin.Dci)
            .AddWithValue("@dci_longue", vaccin.DciLongue)
            .AddWithValue("@date_import", If(vaccin.DateImport <> Nothing, vaccin.DateImport, DateTime.Now))
            .AddWithValue("@utilisateur_import", vaccin.UtilisateurImport)
        End With
        Try
            da.InsertCommand = cmd
            vaccinId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            vaccinId = 0
        Finally
            con.Close()
        End Try

        Return vaccinId
    End Function

    Public Function GetListRelationByVaccin(vaccinId As Long) As List(Of RelationVaccinValence)
        Dim con As SqlConnection = GetConnection()
        Dim relations As List(Of RelationVaccinValence) = New List(Of RelationVaccinValence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_relation_vaccin_valence WHERE vaccin=@vaccinId"

            With command.Parameters
                .AddWithValue("@vaccinId", vaccinId)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    relations.Add(New RelationVaccinValence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return relations
    End Function

    Public Function getFromValences(valenceIds As List(Of Long)) As List(Of VaccinValence)
        Dim con As SqlConnection = GetConnection()
        Dim vaccins As List(Of VaccinValence) = New List(Of VaccinValence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = String.Format("SELECT * FROM [oasis].[oasis].[oa_vaccin] Vaccin " & vbCrLf &
            "LEFT JOIN [oasis].[oasis].[oa_relation_vaccin_valence] RVV " & vbCrLf &
            "ON RVV.vaccin=Vaccin.code WHERE valence IN ({0})", String.Join(",", valenceIds.ToArray()))

            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    vaccins.Add(New VaccinValence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return vaccins
    End Function

    Public Function GetListVaccinValence() As List(Of VaccinValence)
        Dim con As SqlConnection = GetConnection()
        Dim vaccins As List(Of VaccinValence) = New List(Of VaccinValence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM [oasis].[oasis].[oa_vaccin] Vaccin " & vbCrLf &
            "LEFT JOIN [oasis].[oasis].[oa_relation_vaccin_valence] RVV " & vbCrLf &
            "ON RVV.vaccin=Vaccin.code WHERE RVV.id IS NOT NULL"

            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    vaccins.Add(New VaccinValence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return vaccins
    End Function

    '
    ' PROGRAMMATION RELATION
    '
    Public Function UpdateVaccinProgramRelation(vaccinProgramRelation As VaccinProgramRelation) As Long
        Dim da As New SqlDataAdapter()
        Dim vaccinId As Long

        Dim SQLstring As String = "UPDATE oasis.oa_vaccin_program_relation SET vaccin=@vaccin, relation_vaccin_valence=@relationVaccinValence, date=@date, patient=@patient, realisation_date=@realisation_date, " & vbCrLf &
        "realisation_operator=@realisation_operator, realisation_operator_text=@realisation_operator_text, realisation_operator_ror=@realisation_operator_ror WHERE id=@id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", vaccinProgramRelation.Id)
            .AddWithValue("@vaccin", vaccinProgramRelation.Vaccin)
            .AddWithValue("@relationVaccinValence", vaccinProgramRelation.RelationVaccinValence)
            .AddWithValue("@date", vaccinProgramRelation.Date)
            .AddWithValue("@patient", vaccinProgramRelation.Patient)
            .AddWithValue("@realisation_date", vaccinProgramRelation.RealisationDate)
            .AddWithValue("@realisation_operator", vaccinProgramRelation.RealisationOperator)
            .AddWithValue("@realisation_operator_ror", vaccinProgramRelation.RealisationOperatorRor)
            .AddWithValue("@realisation_operator_text", vaccinProgramRelation.RealisationOperatorText)
        End With
        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
            vaccinId = vaccinProgramRelation.Id
        Catch ex As Exception
            Throw New Exception(ex.Message)
            vaccinId = 0
        Finally
            con.Close()
        End Try

        Return vaccinId
    End Function

    Public Function CreateVaccinProgramRelation(vaccinProgram As VaccinProgramRelation) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim vaccinProgramId As Long

        Dim SQLstring As String = "
            INSERT INTO oasis.oa_vaccin_program_relation (vaccin, date, patient, relation_vaccin_valence)
                VALUES (@vaccin, @date, @patient, @relationVaccinValence);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@vaccin", vaccinProgram.Vaccin)
            .AddWithValue("@relationVaccinValence", vaccinProgram.RelationVaccinValence)
            .AddWithValue("@date", vaccinProgram.Date)
            .AddWithValue("@Patient", vaccinProgram.Patient)
        End With
        Try
            da.InsertCommand = cmd
            vaccinProgramId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            vaccinProgramId = 0
        Finally
            con.Close()
        End Try

        Return vaccinProgramId
    End Function

    Public Function DeleteVaccinProgramRelation(vaccinProgram As VaccinProgramRelation) As Long
        Dim da As New SqlDataAdapter()
        Dim vaccinProgramId As Long

        Dim SQLstring As String = "DELETE FROM oasis.oa_vaccin_program_relation WHERE patient=@patientId AND date=@dateId AND vaccin=@vaccin;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", vaccinProgram.Patient)
            .AddWithValue("@dateId", vaccinProgram.Date)
            .AddWithValue("@vaccin", vaccinProgram.Vaccin)
        End With
        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
            vaccinProgramId = vaccinProgram.Id
        Catch ex As Exception
            Throw New Exception(ex.Message)
            vaccinProgramId = 0
        Finally
            con.Close()
        End Try

        Return vaccinProgramId
    End Function

    Public Function GetVaccinProgramRelationListDatePatient(dateId As Long, patientId As Long) As List(Of VaccinProgramRelation)
        Dim con As SqlConnection = GetConnection()
        Dim vaccinPrograms As New List(Of VaccinProgramRelation)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_vaccin_program_relation WHERE date=@dateId AND patient=@patientId;"

            With command.Parameters
                .AddWithValue("@dateId", dateId)
                .AddWithValue("@patientId", patientId)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    vaccinPrograms.Add(New VaccinProgramRelation(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return vaccinPrograms
    End Function

    Public Function GetFirstVaccinProgramRelationListDatePatient(dateId As Long, patientId As Long) As VaccinProgramRelation
        Dim con As SqlConnection = GetConnection()
        Dim vaccinProgram As VaccinProgramRelation = Nothing

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT TOP 1  * FROM oasis.oa_vaccin_program_relation WHERE date=@dateId AND patient=@patientId;"

            With command.Parameters
                .AddWithValue("@dateId", dateId)
                .AddWithValue("@patientId", patientId)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    vaccinProgram = New VaccinProgramRelation(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return vaccinProgram
    End Function

    '
    ' VACCIN PROGRAM ADMINISTRATION
    '

    Public Function CreateVaccinProgramAdministration(vaccinProgramAdmin As VaccinProgramAdmin) As Long
        Dim da As New SqlDataAdapter()
        Dim vaccinId As Long

        Dim SQLstring As String = "
            INSERT into oasis.oa_vaccin_program_admin (vaccin_program_relation, lot, expiration, comment)
                VALUES (@vaccin_program_relation, @lot, @expiration, @comment);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@vaccin_program_relation", vaccinProgramAdmin.VaccinProgramRelation)
            .AddWithValue("@lot", vaccinProgramAdmin.Lot)
            .AddWithValue("@expiration", vaccinProgramAdmin.Expiration)
            .AddWithValue("@comment", vaccinProgramAdmin.Comment)
        End With
        Try
            da.InsertCommand = cmd
            vaccinId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            vaccinId = 0
        Finally
            con.Close()
        End Try

        Return vaccinId
    End Function

    Public Function UpdateVaccinProgramAdministration(vaccinProgramAdmin As VaccinProgramAdmin) As Long
        Dim da As New SqlDataAdapter()
        Dim vaccinId As Long

        Dim SQLstring As String = "
            UPDATE oasis.oa_vaccin_program_admin SET vaccin_program_relation=@vaccin_program_relation, lot=@lot, expiration=@expiration, comment=@comment WHERE id=@id;
        "
        'SELECT SCOPE_IDENTITY();

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", vaccinProgramAdmin.Id)
            .AddWithValue("@vaccin_program_relation", vaccinProgramAdmin.VaccinProgramRelation)
            .AddWithValue("@lot", vaccinProgramAdmin.Lot)
            .AddWithValue("@expiration", vaccinProgramAdmin.Expiration)
            .AddWithValue("@comment", vaccinProgramAdmin.Comment)
        End With
        Try
            da.InsertCommand = cmd
            Debug.WriteLine(GetSqlCommandTextForLogs(cmd))
            'vaccinId = 
            da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            vaccinId = 0
        Finally
            con.Close()
        End Try

        Return vaccinId
    End Function

    Public Function GetVaccinProgramAdministrationByRelation(vaccinProgramRelationId As Integer) As VaccinProgramAdmin
        Dim vaccinProgramAdmin As VaccinProgramAdmin
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.[oa_vaccin_program_admin] WHERE vaccin_program_relation=@vaccinProgramRelationId"
            command.Parameters.AddWithValue("@vaccinProgramRelationId", vaccinProgramRelationId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    vaccinProgramAdmin = New VaccinProgramAdmin(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return vaccinProgramAdmin
    End Function

    ''
    '' VACCIN PROGAM
    ''
    'Public Function CreateVaccinProgram(vaccinProgram As VaccinProgram) As Long
    '    Dim da As SqlDataAdapter = New SqlDataAdapter()
    '    Dim vaccinProgramId As Long

    '    Dim SQLstring As String = "
    '        INSERT INTO oasis.oa_vaccin_program (date, patient, program_date, program_user)
    '            VALUES (@date, @patient, @program_date, @program_user);
    '        SELECT SCOPE_IDENTITY();
    '    "

    '    Dim con As SqlConnection = GetConnection()
    '    Dim cmd As New SqlCommand(SQLstring, con)

    '    With cmd.Parameters
    '        .AddWithValue("@date", vaccinProgram.Date)
    '        .AddWithValue("@Patient", vaccinProgram.Patient)
    '        .AddWithValue("@program_date", vaccinProgram.ProgramDate)
    '        .AddWithValue("@program_user", vaccinProgram.ProgramUser)
    '    End With
    '    Try
    '        da.InsertCommand = cmd
    '        vaccinProgramId = da.InsertCommand.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '        vaccinProgramId = 0
    '    Finally
    '        con.Close()
    '    End Try

    '    Return vaccinProgramId
    'End Function

    'Public Function UpdateVaccinProgram(vaccinProgram As VaccinProgram) As Long
    '    Dim da As SqlDataAdapter = New SqlDataAdapter()
    '    Dim vaccinProgramId As Long

    '    Dim SQLstring As String = "
    '        UPDATE oasis.oa_vaccin_program SET date=@date, patient=@patient, program_date=@program_date, program_user=@program_user)
    '            WHERE id=@id
    '        SELECT SCOPE_IDENTITY();
    '    "

    '    Dim con As SqlConnection = GetConnection()
    '    Dim cmd As New SqlCommand(SQLstring, con)

    '    With cmd.Parameters
    '        .AddWithValue("@date", vaccinProgram.Date)
    '        .AddWithValue("@Patient", vaccinProgram.Patient)
    '        .AddWithValue("@program_date", vaccinProgram.ProgramDate)
    '        .AddWithValue("@program_user", vaccinProgram.ProgramUser)
    '        .AddWithValue("@id", vaccinProgram.Id)
    '    End With
    '    Try
    '        da.InsertCommand = cmd
    '        vaccinProgramId = da.InsertCommand.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '        vaccinProgramId = 0
    '    Finally
    '        con.Close()
    '    End Try

    '    Return vaccinProgramId
    'End Function

    'Public Function DeleteVaccinProgram(vaccinProgram As VaccinProgram) As Long
    '    Dim da As SqlDataAdapter = New SqlDataAdapter()
    '    Dim vaccinProgramId As Long

    '    Dim SQLstring As String = "DELETE FROM oasis.oa_vaccin_program WHERE patient=@patientId AND date=@dateId;"

    '    Dim con As SqlConnection = GetConnection()
    '    Dim cmd As New SqlCommand(SQLstring, con)

    '    With cmd.Parameters
    '        .AddWithValue("@patientId", vaccinProgram.Patient)
    '        .AddWithValue("@dateId", vaccinProgram.Date)
    '    End With
    '    Try
    '        da.InsertCommand = cmd
    '        da.InsertCommand.ExecuteScalar()
    '        vaccinProgramId = vaccinProgram.Id
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '        vaccinProgramId = 0
    '    Finally
    '        con.Close()
    '    End Try

    '    Return vaccinProgramId
    'End Function

    'Public Function GetVaccinProgramByPatientDate(vaccinProgram As VaccinProgram) As VaccinProgram
    '    Dim da As SqlDataAdapter = New SqlDataAdapter()

    '    Dim SQLstring As String = "SELECT * FROM oasis.oa_vaccin_program WHERE patient=@patientId AND date=@dateId;"

    '    Dim con As SqlConnection = GetConnection()
    '    Dim cmd As New SqlCommand(SQLstring, con)

    '    With cmd.Parameters
    '        .AddWithValue("@patientId", vaccinProgram.Patient)
    '        .AddWithValue("@dateId", vaccinProgram.Date)
    '    End With
    '    Try
    '        Using reader As SqlDataReader = cmd.ExecuteReader()
    '            If reader.Read() Then
    '                vaccinProgram = New VaccinProgram(reader)
    '            End If
    '        End Using
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try

    '    Return vaccinProgram
    'End Function

    'Public Function GetVaccinProgramListByPatient(patientId As Long) As List(Of VaccinProgram)
    '    Dim con As SqlConnection = GetConnection()
    '    Dim vaccinPrograms As List(Of VaccinProgram) = New List(Of VaccinProgram)

    '    Try
    '        Dim command As SqlCommand = con.CreateCommand()
    '        command.CommandText = "SELECT * FROM oasis.oa_vaccin_program WHERE patient=@patientId;"
    '        With cmd.Parameters
    '            .AddWithValue("@patientId", patientId)
    '        End With
    '        Using reader As SqlDataReader = command.ExecuteReader()
    '            While (reader.Read())
    '                vaccinPrograms.Add(New VaccinProgram(reader))
    '            End While
    '        End Using
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try

    '    Return vaccinPrograms
    'End Function

End Class
