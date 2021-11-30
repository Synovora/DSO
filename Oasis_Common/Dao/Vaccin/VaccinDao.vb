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

End Class
