Imports System.Data.SqlClient

Public Class LogDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As Log
        Dim log As New Log With {
            .Id = reader("id"),
            .Description = Coalesce(reader("description"), ""),
            .Origine = Coalesce(reader("origine"), ""),
            .TypeLog = Coalesce(reader("type_log"), ""),
            .UserLog = Coalesce(reader("user_creation"), 0),
            .DateLog = Coalesce(reader("date_creation"), Nothing)
        }
        Return log
    End Function

    Public Function GetLogById(LogId As Integer) As Log
        Dim log As Log
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_log WHERE id = @id"
            command.Parameters.AddWithValue("@id", LogId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    log = BuildBean(reader)
                Else
                    Throw New ArgumentException("Log inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return log
    End Function

    Public Sub CreateLog(log As Log, userLog As Utilisateur)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim dateCreation As Date = Date.Now.Date
        Dim SQLstring As String = "INSERT INTO oasis.oa_log" &
        " (description, type_log, origine, date_creation, user_creation)" &
        " VALUES (@description, @typeLog, @origine, @dateCreation, @userCreation)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@description", log.Description)
            .AddWithValue("@typelog", log.TypeLog)
            .AddWithValue("@origine", log.Origine)
            .AddWithValue("@dateCreation", Date.Now())
            .AddWithValue("@userCreation", userLog.UtilisateurId)
        End With
        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

End Class
