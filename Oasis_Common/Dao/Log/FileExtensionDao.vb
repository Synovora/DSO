Imports System.Data.SqlClient

Public Class FileExtensionDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As FileExtension
        Dim extension As New FileExtension With {
            .Id = reader("id"),
            .Extension = Coalesce(reader("ext"), ""),
            .Description = Coalesce(reader("description"), "")
        }
        Return extension
    End Function

    Public Function GetAllFileExtension() As List(Of FileExtension)
        Dim con As SqlConnection = GetConnection()
        Dim extensions As List(Of FileExtension) = New List(Of FileExtension)
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_r_file_extension"
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    extensions.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return extensions
    End Function

    Public Function GetFileExtensionById(LogId As Integer) As FileExtension
        Dim extension As FileExtension
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_r_file_extension WHERE id = @id"
            command.Parameters.AddWithValue("@id", LogId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    extension = BuildBean(reader)
                Else
                    Throw New ArgumentException("Log inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return extension
    End Function

End Class
