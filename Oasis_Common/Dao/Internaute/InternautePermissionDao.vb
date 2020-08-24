Imports System.Data.SqlClient

Public Class InternautePermissionDao
    Inherits StandardDao

    Public Function BuildBean(reader As SqlDataReader) As InternautePermission
        Dim internautePermission As New InternautePermission With {
            .PermissionId = reader("permission_id"),
            .PatientId = Coalesce(reader("patient_id"), 0),
            .InternauteId = Coalesce(reader("internaute_id"), 0),
            .PermissionLevel = Coalesce(reader("permission_level"), 0)
        }
        Return internautePermission
    End Function

    Public Function GetPermissionsByInternaute(internauteId As Integer) As List(Of InternautePermission)
        Dim internautePermissions As List(Of InternautePermission) = New List(Of InternautePermission)

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute_permission WHERE internaute_id = @internauteId;"
                command.Parameters.AddWithValue("@internauteId", internauteId)
                Using reader As SqlDataReader = command.ExecuteReader()
                    While (reader.Read())
                        internautePermissions.Add(BuildBean(reader))
                    End While
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return internautePermissions
    End Function
End Class
