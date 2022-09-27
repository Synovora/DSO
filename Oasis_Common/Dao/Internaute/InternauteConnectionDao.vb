Imports System.Data.SqlClient

Public Class InternauteConnectionDao
    Inherits StandardDao

    Public Function Create(internauteConnection As InternauteConnection) As Long
        Dim da As New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction
        Dim Id As Long

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_internaute_connection (" & vbCrLf &
                                     " internaute, datetime, ip)" & vbCrLf &
                                     " VALUES (" & vbCrLf &
                                     " @internaute, @datetime, @ip);" & vbCrLf &
                                     " SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@internaute", internauteConnection.Internaute)
                .AddWithValue("@datetime", internauteConnection.Datetime)
                .AddWithValue("@ip", internauteConnection.Ip)
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

    Public Function GetConnectionByInternaute(internaute As Integer) As List(Of InternauteConnection)
        Dim internautePermissions As New List(Of InternauteConnection)

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT TOP 6 * FROM oasis.oa_internaute_connection WHERE internaute = @internaute ORDER BY id DESC;"
                command.Parameters.AddWithValue("@internaute", internaute)
                Using reader As SqlDataReader = command.ExecuteReader()
                    While (reader.Read())
                        internautePermissions.Add(New InternauteConnection(reader))
                    End While
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return internautePermissions
    End Function
End Class
