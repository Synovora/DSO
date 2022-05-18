Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class ValenceDao
    Inherits StandardDao

    Public Function GetById(valenceId As Integer) As Valence
        Dim valence As Valence
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_valence WHERE id = @id"
            command.Parameters.AddWithValue("@id", valenceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    valence = New Valence(reader)
                Else
                    Throw New ArgumentException("Valence inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return valence
    End Function

    Public Function GetList() As List(Of Valence)
        Dim con As SqlConnection = GetConnection()
        Dim valences As List(Of Valence) = New List(Of Valence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_valence ORDER BY ordre ASC"
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    valences.Add(New Valence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valences
    End Function

    Public Function GetLastOrder() As Valence
        Dim con As SqlConnection = GetConnection()
        Dim valence As Valence = Nothing

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT TOP 1 * FROM oasis.oa_valence ORDER BY ordre DESC"
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    valence = New Valence(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valence
    End Function

    Public Function GetByOrder(order As Integer) As Valence
        Dim con As SqlConnection = GetConnection()
        Dim valence As Valence = Nothing

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT TOP 1 * FROM oasis.oa_valence WHERE ordre = @ordre"
            With command.Parameters
                .AddWithValue("@ordre", order)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    valence = New Valence(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valence
    End Function

    Public Function GetListFromOrder(order As Integer) As List(Of Valence)
        Dim con As SqlConnection = GetConnection()
        Dim valences As List(Of Valence) = New List(Of Valence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_valence WHERE ordre >= @ordre"
            With command.Parameters
                .AddWithValue("@ordre", order)
            End With
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    valences.Add(New Valence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valences
    End Function

    Public Function Create(valence As Valence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim valenceId As Long

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "
            INSERT into oasis.oa_valence (code, description, precaution, date_creation, date_modification, utilisateur_creation, utilisateur_modification, actif, visible, ordre)
                VALUES (@code, @description, @precaution, @date_creation, @date_modification, @utilisateur_creation, @utilisateur_modification, @actif, @visible, @ordre);
            SELECT SCOPE_IDENTITY();
        "

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)
        Dim lastValence = GetLastOrder()

        With cmd.Parameters
            .AddWithValue("@code", valence.Code)
            .AddWithValue("@description", valence.Description)
            .AddWithValue("@precaution", valence.Precaution)
            .AddWithValue("@date_creation", If(valence.DateCreation <> Nothing, valence.DateCreation, DateTime.Now))
            .AddWithValue("@date_modification", If(valence.DateModification <> Nothing, valence.DateModification, DateTime.Now))
            .AddWithValue("@utilisateur_creation", valence.UtilisateurCreation)
            .AddWithValue("@utilisateur_modification", valence.UtilisateurModification)
            .AddWithValue("@actif", 1)
            .AddWithValue("@visible", 0)
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

    Public Function Delete(valence As Valence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim valenceId As Long

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "
            DELETE FROM oasis.oa_vaccin_cgv_relation_valence_date WHERE valence=@id;
            DELETE FROM oasis.oa_vaccin_cgv_valence WHERE valence=@id;
            DELETE FROM oasis.oa_relation_vaccin_valence WHERE valence=@id;
            DELETE FROM oasis.oa_valence WHERE id=@id;
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

    Public Function Update(valence As Valence) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()

        Dim SQLstring As String = "UPDATE oasis.oa_valence SET code = @code," &
        " description = @description, precaution = @precaution," &
        " date_modification = @date_modification, utilisateur_modification = @utilisateur_modification, visible = @visible, ordre = @ordre WHERE id = @id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@id", valence.Id)
            .AddWithValue("@code", valence.Code)
            .AddWithValue("@description", valence.Description)
            .AddWithValue("@precaution", valence.Precaution)
            .AddWithValue("@date_modification", DateTime.Now)
            .AddWithValue("@utilisateur_modification", valence.UtilisateurModification)
            .AddWithValue("@visible", valence.Visible)
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

    Public Function SetVisibility(valenceId As Long, visible As Boolean) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()

        Dim SQLstring As String = "UPDATE oasis.oa_valence SET visible = @visible, ordre = @ordre WHERE id = @id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)
        Dim lastValence = GetLastOrder()

        With cmd.Parameters
            .AddWithValue("@id", valenceId)
            .AddWithValue("@visible", visible)
            .AddWithValue("@ordre", If(visible AndAlso lastValence IsNot Nothing, lastValence.Ordre + 1, 0))
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

    Public Function SetOrder(valenceId As Long, ordre As Integer) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()

        Dim SQLstring As String = "UPDATE oasis.oa_valence SET ordre = @ordre WHERE id = @id;"

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

    Public Function CreateRelation(relationVaccinValence As RelationVaccinValence) As Long
        Dim da As New SqlDataAdapter()
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
        Dim da As New SqlDataAdapter()
        Dim relationId As Long

        Dim SQLstring As String = "DELETE FROM oasis.oa_relation_vaccin_valence WHERE valence=@valenceId AND vaccin=@vaccinId;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@valenceId", relationVaccinValence.Valence)
            .AddWithValue("@vaccinId", relationVaccinValence.Vaccin)
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

    Public Function GetRelationListByVaccin(vaccinId As Long) As List(Of RelationVaccinValence)
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

    Public Function GetRelationListByValence(valenceId As Long) As List(Of RelationVaccinValence)
        Dim con As SqlConnection = GetConnection()
        Dim relations As List(Of RelationVaccinValence) = New List(Of RelationVaccinValence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_relation_vaccin_valence WHERE valence=@valenceId"

            With command.Parameters
                .AddWithValue("@valenceId", valenceId)
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

    Public Function GetRelationList() As List(Of RelationVaccinValence)
        Dim con As SqlConnection = GetConnection()
        Dim valences As List(Of RelationVaccinValence) = New List(Of RelationVaccinValence)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_relation_vaccin_valence"
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    valences.Add(New RelationVaccinValence(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return valences
    End Function

End Class
