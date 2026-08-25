Imports System.Data.SqlClient

Public Class LogDao
    Inherits StandardDao

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As Log
        Dim log As New Log With {
            .Id = record("id"),
            .Description = Coalesce(record("description"), ""),
            .Origine = Coalesce(record("origine"), ""),
            .TypeLog = Coalesce(record("type_log"), ""),
            .UserLog = New Utilisateur With {.UtilisateurId = Coalesce(record("user_creation"), 0)},
            .DateLog = Coalesce(record("date_creation"), Nothing)
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

    Public Sub CreateLog(log As Log)
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
            .AddWithValue("@userCreation", log.UserLog.UtilisateurId)
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
