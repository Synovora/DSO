Imports System.Data.SqlClient

Public Class AldCim10Dao
    Inherits StandardDao
    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As AldCim10
        Dim aldCim10 As New AldCim10 With {
            .AldCim10Id = Convert.ToInt64(record("oa_ald_cim10_id")),
            .AldCim10AldId = Coalesce(record("oa_ald_cim10_ald_id"), 0),
            .AldCim10AldCode = Coalesce(record("oa_ald_cim10_ald_code"), ""),
            .AldCim10Code = Coalesce(record("oa_ald_cim10_code"), ""),
            .AldCim10Description = Coalesce(record("oa_ald_cim10_description"), "")
        }
        Return aldCim10
    End Function

    Public Function GetAldCim10ById(AldCim10Id As Integer) As AldCim10
        Dim aldCim10 As AldCim10
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_ald_cim10 WHERE oa_ald_cim10_id = @id"
            command.Parameters.AddWithValue("@id", AldCim10Id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    aldCim10 = BuildBean(reader)
                Else
                    Throw New ArgumentException("ALD Cim10 inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return aldCim10
    End Function

    Public Function GetAllAldCim10ByAldId(AldId As Long) As List(Of AldCim10)
        Dim con As SqlConnection = GetConnection()
        Dim aldCim10s As List(Of AldCim10) = New List(Of AldCim10)
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_ald_cim10 WHERE oa_ald_cim10_ald_id = @AldId;"
            command.Parameters.AddWithValue("@AldId", AldId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    aldCim10s.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return aldCim10s
    End Function
End Class
