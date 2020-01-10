Imports System.Data.SqlClient


Public Class LogDao
    Inherits StandardDao

    Public Enum EnumTypeLog
        ERREUR
        INFO
    End Enum

    Friend Function GetLogById(LogId As Integer) As Log
        Dim log As Log
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_log WHERE id = @id"
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

    Private Function BuildBean(reader As SqlDataReader) As Log
        Dim log As New Log

        log.Id = reader("id")
        log.Description = Coalesce(reader("description"), "")
        log.Origine = Coalesce(reader("origine"), "")
        log.TypeLog = Coalesce(reader("type_log"), "")
        log.UserLog = Coalesce(reader("user_creation"), 0)
        log.DateLog = Coalesce(reader("date_creation"), Nothing)
        Return log
    End Function

    Friend Function CreateLog(log As Log) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim CodeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "INSERT INTO oasis.oa_log" &
        " (description, type_log, origine, date_creation, user_creation)" &
        " VALUES (@description, @typeLog, @origine, @dateCreation, @userCreation)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@description", log.Description)
            .AddWithValue("@typelog", log.TypeLog)
            .AddWithValue("@origine", log.Origine)
            .AddWithValue("@dateCreation", Date.Now().ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@userCreation", userLog.UtilisateurId)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            con.Close()
        End Try

        Return CodeRetour
    End Function

End Class
