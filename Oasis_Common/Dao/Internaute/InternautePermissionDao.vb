Imports System.Data.SqlClient

Public Class InternautePermissionDao
    Inherits StandardDao

    Public Function Create(internautePermission As InternautePermission) As Long
        Dim da As New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction
        Dim Id As Long

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_internaute_permission (" & vbCrLf &
                                     " internaute, patient, permission)" & vbCrLf &
                                     " VALUES (" & vbCrLf &
                                     " @internaute, @patient, @permission);" & vbCrLf &
                                     " SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@internaute", internautePermission.Internaute)
                .AddWithValue("@patient", internautePermission.Patient)
                .AddWithValue("@permission", internautePermission.Permission)
            End With

            da.InsertCommand = cmd
            Id = da.InsertCommand.ExecuteScalar()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return Id
    End Function

    Public Function GetPermissionsByInternaute(internaute As Integer) As List(Of InternautePermission)
        Dim internautePermissions As New List(Of InternautePermission)

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute_permission WHERE internaute = @internaute;"
                command.Parameters.AddWithValue("@internaute", internaute)
                Using reader As SqlDataReader = command.ExecuteReader()
                    While (reader.Read())
                        internautePermissions.Add(New InternautePermission(reader))
                    End While
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return internautePermissions
    End Function

    Public Function GetPermissionsByPatient(patient As Long) As List(Of InternautePermission)
        Dim internautePermissions As New List(Of InternautePermission)

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute_permission WHERE patient=@patient;"
                command.Parameters.AddWithValue("@patient", patient)
                Using reader As SqlDataReader = command.ExecuteReader()
                    While (reader.Read())
                        internautePermissions.Add(New InternautePermission(reader))
                    End While
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return internautePermissions
    End Function
End Class
