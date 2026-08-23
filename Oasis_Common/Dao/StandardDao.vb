Imports System.Configuration
Imports System.Reflection
Imports System.Data.SqlClient

Public MustInherit Class StandardDao

    ''' <summary>
    ''' Proposé par le client lourd uniquement (voir Oasis_WF), pour reproposer une
    ''' tentative de connexion à l'utilisateur. Renvoie True pour réessayer.
    '''
    ''' Cette classe est partagée avec Oasis_Web : appeler MsgBox directement ici
    ''' bloquait le thread IIS et transformait une coupure de base de données en
    ''' boucle infinie consommant un worker par requête. Sans crochet défini,
    ''' l'exception remonte immédiatement.
    ''' </summary>
    Public Shared Property DemanderNouvelEssaiConnexion As Func(Of String, Boolean) = Nothing

    Protected Function GetConnection() As SqlConnection
        Do
            Dim conn As SqlConnection = New SqlConnection(GetConnectionStringOasis())
            Try
                conn.Open()
                Return conn
            Catch e As Exception
                conn.Dispose()
                Dim crochet = DemanderNouvelEssaiConnexion
                If crochet Is Nothing OrElse Not crochet(e.Message) Then
                    Throw
                End If
            End Try
        Loop
    End Function

    Public Shared Sub FixConnectionString(newConnectionStringIfEmpty As String)
        Dim DBCS = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection")
        If DBCS.ConnectionString = "" Then
            Dim writable = GetType(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)
            writable.SetValue(DBCS, False)
            DBCS.ConnectionString = newConnectionStringIfEmpty
        End If
    End Sub

    Public Shared Function IsConnectionStringFixed() As Boolean
        Dim DBCS = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection")
        Return DBCS.ConnectionString.Length <> 0
    End Function

End Class
