Imports System.Data.SqlClient

Public Class ParametreDrcDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As ParametreDrc
        Dim parametreDrc As New ParametreDrc With {
            .Id = reader("id"),
            .DrcId = Coalesce(reader("drc_id"), 0),
            .ParametreId = Coalesce(reader("parametre_id"), 0)
        }
        Return parametreDrc
    End Function

    Public Function GetParametreDrcById(parametreId As Integer) As ParametreDrc
        Dim parametreDrc As ParametreDrc
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_drc_parametre WHERE id = @id"
            command.Parameters.AddWithValue("@id", parametreId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    parametreDrc = BuildBean(reader)
                Else
                    Throw New ArgumentException("Paramètre inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return parametreDrc
    End Function

    Public Function GetParametreByDrcAndId(DrcId As Long, parametreId As Long) As ParametreDrc
        Dim parametreDrc As ParametreDrc
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_drc_parametre " &
                                    " WHERE drc_id = @drcId AND parametre_id = @parametreId"
            command.Parameters.AddWithValue("@drcId", DrcId)
            command.Parameters.AddWithValue("@parametreId", parametreId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    parametreDrc = BuildBean(reader)
                Else
                    parametreDrc = New ParametreDrc With {
                        .Id = 0,
                        .ParametreId = 0,
                        .DrcId = 0
                    }
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return parametreDrc
    End Function

    Public Sub CreationParametreDrc(parametreDrc As ParametreDrc, userLog As Utilisateur)
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim NbInsert As Integer
        Dim con As SqlConnection = GetConnection()
        Dim dateCreation As Date = Date.Now.Date
        Dim SQLstring As String = "INSERT INTO oasis.oa_drc_parametre" &
        " (drc_id, parametre_id)" &
        " VALUES (@drcId, @parametreId)"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@drcId", parametreDrc.DrcId)
            .AddWithValue("@parametreId", parametreDrc.ParametreId)
        End With
        Try
            da.InsertCommand = cmd
            NbInsert = da.InsertCommand.ExecuteNonQuery()
            If NbInsert = 0 Then
                Dim anomalie As String = "La création du paramètre n'a pas abouti - DRC N° : " & parametreDrc.DrcId.ToString & " Id. paramètre : " & parametreDrc.ParametreId.ToString
                Throw New Exception(anomalie)
                CreateLog(anomalie, "ParametreDrcDao", Log.EnumTypeLog.ERREUR.ToString, userLog)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub ModificationParametreDrc(parametreDrc As ParametreDrc, userLog As Utilisateur)
        Dim NbUpdate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "UPDATE oasis.oa_drc_parametre SET" &
        " drc_id = @drcId, parametre_id = @parametreId" &
        " WHERE id = @Id"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@Id", parametreDrc.Id)
            .AddWithValue("@drcId", parametreDrc.DrcId)
            .AddWithValue("@parametreId", parametreDrc.ParametreId)
        End With
        Try
            da.UpdateCommand = cmd
            NbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If NbUpdate = 0 Then
                Dim anomalie As String = "La modification du paramètre n'a pas abouti - Id : " & parametreDrc.Id.ToString & " DRC N° : " & parametreDrc.DrcId.ToString & " Id. paramètre : " & parametreDrc.ParametreId.ToString
                Throw New Exception(anomalie)
                CreateLog(anomalie, "ParametreDrcDao", Log.EnumTypeLog.ERREUR.ToString, userLog)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub SuppressionParametreDrcByDrcId(DrcId As Long)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "DELETE oasis.oa_drc_parametre" &
        " WHERE Drc_id = @DrcId"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@DrcId", DrcId)
        End With
        Try
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    'TODO: Change it
    Public Function GetParametresByDrcId(DrcId As Long) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_drc_parametre" &
        " WHERE drc_id = " & DrcId.ToString
        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        da.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

End Class
