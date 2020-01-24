Imports System.Configuration
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks

Public Class ApiOasis
    Implements IDisposable

    Private serveurDomain As String
    Private client As HttpClient

    Public Sub New(ByVal _serveurDomain As String)
        init(_serveurDomain)
    End Sub

    Public Sub New()
        serveurDomain = ConfigurationManager.AppSettings("ServeurOasis")
        If serveurDomain Is Nothing OrElse serveurDomain.Trim().Length < 3 Then
            Throw New Exception("Serveur non paramétré dans le fichier de configuration")
        End If
        init(serveurDomain)
    End Sub

    Private Sub init(_serveurDomain As String)
        ServicePointManager.ServerCertificateValidationCallback = AddressOf AcceptAllCertifications
        serveurDomain = _serveurDomain
        'isProxyManuel = _isProxyManuel
        'proxyUrl = _proxyUrl
        'proxyPort = _proxyPort
        'proxyLogin = _proxyLogin
        'proxyPassword = _proxyPassword
        client = New HttpClient()
    End Sub


    Public Function loginRest(loginRequest As LoginRequest) As String
        Dim str = login(loginRequest).GetAwaiter.GetResult()
        Return DecryptString(str)

    End Function

    Private Function login(loginRequest As LoginRequest) As Task(Of String)
        initHttp(serveurDomain)

        Dim response As HttpResponseMessage = client.PostAsJsonAsync("/api/login", loginRequest).Result
        If response.IsSuccessStatusCode = False Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
        End If
        Return response.Content.ReadAsAsync(Of String)()
    End Function

    Private Sub initHttp(serveurDomain As String)
        client.BaseAddress = New Uri("https://" + serveurDomain)
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

    End Sub


    Public Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        client.Dispose()
    End Sub
End Class
