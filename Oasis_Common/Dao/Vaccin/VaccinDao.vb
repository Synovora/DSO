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

    'Public Function Delete(valence As Valence) As Long
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
    '        .AddWithValue("@id", valence.Id)
    '    End With
    '    Try
    '        da.InsertCommand = cmd
    '        da.InsertCommand.ExecuteScalar()
    '        valenceId = valence.Id
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '        valenceId = 0
    '    Finally
    '        con.Close()
    '    End Try

    '    Return valenceId
    'End Function

    'Public Function Update(valence As Valence) As Long
    '    Dim da As SqlDataAdapter = New SqlDataAdapter()
    '    Dim valenceId As Long

    '    Dim dateModification As Date = Date.Now.Date

    '    Dim SQLstring As String = "UPDATE oasis.oa_valence SET code = @code," &
    '    " description = @description, precaution = @precaution," &
    '    " date_modification = @date_modification, utilisateur_modification = @utilisateur_modification WHERE id = @id;" &
    '     "SELECT SCOPE_IDENTITY();"

    '    Dim con As SqlConnection = GetConnection()
    '    Dim cmd As New SqlCommand(SQLstring, con)

    '    With cmd.Parameters
    '        .AddWithValue("@id", valence.Id)
    '        .AddWithValue("@code", valence.Code)
    '        .AddWithValue("@description", valence.Description)
    '        .AddWithValue("@precaution", valence.Precaution)
    '        .AddWithValue("@date_modification", If(valence.DateModification <> Nothing, valence.DateModification, DateTime.Now))
    '        .AddWithValue("@utilisateur_modification", valence.UtilisateurCreation)
    '    End With

    '    Try
    '        da.UpdateCommand = cmd
    '        valenceId = da.InsertCommand.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    Finally
    '        con.Close()
    '    End Try

    '    Return valenceId
    'End Function

    Public Function CreateRelation(relationVaccinValence As RelationVaccinValence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim relationId As Long

        Dim SQLstring As String = "
            INSERT INTO oasis.oa_relation_vaccin_valence (vaccin, valence)
                VALUES (@vaccin, @valence);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@vaccin", relationVaccinValence.Vaccin)
            .AddWithValue("@valence", relationVaccinValence.Valence)
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

    Public Function DeleteRelation(relationVaccinValence As RelationVaccinValence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim relationId As Long

        Dim SQLstring As String = "DELETE FROM oasis.oa_relation_vaccin_valence WHERE id=@id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", relationVaccinValence.Id)
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

    Public Function GetListByVaccin(vaccinId As Long) As List(Of RelationVaccinValence)
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

    Public Function CreateVaccinProgram(vaccinProgram As VaccinProgramRelation) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim vaccinProgramId As Long

        Dim SQLstring As String = "
            INSERT INTO oasis.oa_vaccin_program_relation (date, patient)
                VALUES (@date, @patient);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@vaccin", vaccinProgram.Vaccin)
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

    Public Function DeleteVaccinProgram(vaccinProgram As VaccinProgramRelation) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim vaccinProgramId As Long

        Dim SQLstring As String = "DELETE FROM oasis.oa_vaccin_program_relation WHERE patient=@patientId AND date=@dateId;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", vaccinProgram.Patient)
            .AddWithValue("@dateId", vaccinProgram.Date)
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

    Public Function getAll() As List(Of VaccinValence)
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
    Public Function CreateVaccinProgramRelation(vaccinProgram As VaccinProgramRelation) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim vaccinProgramId As Long

        Dim SQLstring As String = "
            INSERT INTO oasis.oa_vaccin_program_relation (vaccin, date, patient)
                VALUES (@vaccin, @date, @patient);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@vaccin", vaccinProgram.Vaccin)
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
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim vaccinProgramId As Long

        Dim SQLstring As String = "DELETE FROM oasis.oa_vaccin_program_relation WHERE patient=@patientId AND date=@dateId;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", vaccinProgram.Patient)
            .AddWithValue("@dateId", vaccinProgram.Date)
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
        Dim vaccinPrograms As List(Of VaccinProgramRelation) = New List(Of VaccinProgramRelation)

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
