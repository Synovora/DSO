Imports System.Configuration
Imports System.Reflection
Imports System.Data.SqlClient


Public MustInherit Class StandardDao

    Protected Function GetConnection() As SqlConnection

        Dim strConnect As String = GetConnectionStringOasis() ' ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString

        Dim conn As SqlConnection = New SqlConnection(strConnect)

        Do While True
            Try
                conn.Open()
                Exit Do
            Catch e As Exception
                MsgBox("Problème de connexion à la base de données (" & e.Message & ") - Validez pour rééssayer")
            End Try
        Loop
        Return conn

    End Function

    Public Shared Sub fixConnectionString(newConnectionStringIfEmpty As String)
        Dim DBCS = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection")
        If DBCS.ConnectionString = "" Then
            Dim writable = GetType(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)
            writable.SetValue(DBCS, False)
            DBCS.ConnectionString = newConnectionStringIfEmpty
        End If
    End Sub

    Public Shared Function isConnectionStringFixed() As Boolean
        Dim DBCS = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection")
        Return DBCS.ConnectionString.Length <> 0
    End Function

End Class
