Imports System.Data.SqlClient

Module ModuleConnection
    Dim oaConnection As New SqlConnection
    Private Function getConnectionString() As String
        Dim csb As SqlConnectionStringBuilder = New SqlConnectionStringBuilder()
        csb.DataSource = "BEL64A0078"   'In production this would more likely come from App.config
        csb.InitialCatalog = "oasis"
        csb.IntegratedSecurity = True
        Return csb.ConnectionString

    End Function

    Public Function getConnection() As Boolean
        Dim bConnection As Boolean = False

        oaConnection = New SqlConnection(getConnectionString())

        Try
            oaConnection.Open()
            bConnection = True
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            getConnection = bConnection
        End Try

    End Function


    ' Functions to add, edit, delete data into database
    Public Sub runSql(ByVal SqlString As String)

        'Creates the ConnectionString. 

        Dim cmd As New SqlCommand

        If (getConnection() = True) Then
            Try
                cmd.CommandType = CommandType.Text
                cmd.CommandText = SqlString
                cmd.ExecuteNonQuery()
                cmd.Dispose()
                oaConnection.Close()
                MsgBox("Les données ont été sauvegardées !")
            Catch ex As Exception
                MsgBox("Erreur lors de la sauvegarde des données")
            End Try
        End If

    End Sub

End Module
