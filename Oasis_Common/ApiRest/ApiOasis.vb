Imports System.Configuration
Imports System.IO
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

    ''' <summary>
    ''' Login via api rest oasis
    ''' </summary>
    ''' <param name="loginRequest"></param>
    ''' <returns></returns>
    Public Function loginRest(loginRequest As LoginRequest) As String
        Dim str = login(loginRequest).GetAwaiter.GetResult()
        Return DecryptString(str)

    End Function

    ''' <summary>
    ''' upload de fichier via api rest oasis
    ''' </summary>
    ''' <param name="login"></param>
    ''' <param name="password"></param>
    ''' <param name="srcFileName"></param>
    ''' <param name="contenu"></param>
    Public Sub uploadFileRest(login As String, password As String, srcFileName As String, contenu As Byte())
        Dim str = uploadFile(login, password, srcFileName, contenu).GetAwaiter.GetResult()
    End Sub

    ''' <summary>
    ''' download de fichier via api rest oasis
    ''' </summary>
    ''' <param name="downloadRequest"></param>
    ''' <returns></returns>
    Public Function downloadFileRest(downloadRequest As DownloadRequest) As Byte()
        Dim tblByte As Byte() = downloadFile(downloadRequest).GetAwaiter.GetResult()
        Return tblByte
    End Function

    Public Function renameFileRest(renameRequest As RenameRequest) As String
        Console.WriteLine(renameRequest.OldName & " " & renameRequest.NewName)
        Dim str = renameFile(renameRequest).GetAwaiter.GetResult()
        Return DecryptString(str)

    End Function


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

    Private Function login(loginRequest As LoginRequest) As Task(Of String)
        initHttp(serveurDomain)

        Dim response As HttpResponseMessage = client.PostAsJsonAsync("/api/login", loginRequest).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
        End If
        Return response.Content.ReadAsAsync(Of String)()
    End Function

    Private Function uploadFile(login As String, password As String, srcFileName As String, contenu As Byte()) As Task(Of String)
        initHttp(serveurDomain)

        Dim formContent = New MultipartFormDataContent From {
            {New StringContent(login), "login"},
            {New StringContent(password), "password"},
            {New StreamContent(New MemoryStream(contenu)), "filekey", srcFileName}
        }

        Dim response As HttpResponseMessage = client.PostAsync("/api/docfileupload", formContent).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
        End If
        Return response.Content.ReadAsStringAsync()
    End Function


    Private Function downloadFile(downloadRequest As DownloadRequest) As Task(Of Byte())
        initHttp(serveurDomain)

        Dim response As HttpResponseMessage = client.PostAsJsonAsync("/api/docfiledownload", downloadRequest).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
        End If
        Return response.Content.ReadAsByteArrayAsync()
    End Function

    Private Function renameFile(renameRequest As RenameRequest) As Task(Of String)
        initHttp(serveurDomain)
        Console.WriteLine("initHttp")
        Dim response As HttpResponseMessage = client.PostAsJsonAsync("/api/rename", renameRequest).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
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
