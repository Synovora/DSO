Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Reflection
Imports System.Data.SqlClient


Public MustInherit Class StandardWebDao

    Protected Function GetConnection() As SqlConnection

        Dim strConnect As String = ConfigurationManager.ConnectionStrings("Oasis_Web.My.MySettings.oasisConnection").ConnectionString

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

    Public Shared Sub fixConnectionString()
        Dim DBCS = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection")
        If DBCS.ConnectionString = "" Then
            Dim writable = GetType(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)
            writable.SetValue(DBCS, False)
            DBCS.ConnectionString = "Data Source=ns3119889.ip-51-38-181.eu;Initial Catalog=oasis;persist security info=True;user id=sa;password=Oasis-689;MultipleActiveResultSets=True"
        End If
    End Sub


    Public Function Coalesce(ByVal ParamArray Parameters As Object()) As Object
        For Each Parameter As Object In Parameters
            If Not Parameter Is Nothing And Not IsDBNull(Parameter) Then
                Return Parameter
            End If
        Next
        Return Nothing
    End Function


End Class
