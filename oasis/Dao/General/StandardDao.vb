Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Data.SqlClient


Public MustInherit Class StandardDao

    Protected Function GetConnection() As SqlConnection

        Dim strConnect As String = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString

        Dim conn As SqlConnection = New SqlConnection(strConnect)

        Do While True
            Try
                conn.Open()
                Exit Do
            Catch e As Exception
                MsgBox("Problème de connexion à la base de données (" & e.Message & ") - Validez pour réssayer")
            End Try
        Loop
        Return conn

    End Function


End Class
