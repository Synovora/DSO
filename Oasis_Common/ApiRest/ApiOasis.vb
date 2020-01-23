Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks

Public Class ApiOasis

    Property serveurDomain As String

    Dim request As HttpWebRequest
    Dim dictLoginPwd As Dictionary(Of String, String)
    Dim loginRequest As LoginRequest
    Shared client As HttpClient = New HttpClient()

    Public Sub New(ByVal _serveurDomain As String)
        serveurDomain = _serveurDomain
        'isProxyManuel = _isProxyManuel
        'proxyUrl = _proxyUrl
        'proxyPort = _proxyPort
        'proxyLogin = _proxyLogin
        'proxyPassword = _proxyPassword
    End Sub

    Public Function loginRest(loginRquest As LoginRequest) As String
        Dim str = login(serveurDomain, loginRequest).GetAwaiter().GetResult()
        Return str
    End Function

    Private Shared Async Function login(serveurDomain As String, ByVal loginRequest As LoginRequest) As Task(Of String)
        initHttp(serveurDomain)
        Dim response As HttpResponseMessage = Await client.PostAsJsonAsync("/login", loginRequest)
        response.EnsureSuccessStatusCode()
        Dim str = Await response.Content.ReadAsAsync(Of String)()
        Return str
    End Function

    Private Shared Sub initHttp(serveurDomain As String)
        client.BaseAddress = New Uri("https://" + serveurDomain + "/api")
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

    End Sub





End Class
