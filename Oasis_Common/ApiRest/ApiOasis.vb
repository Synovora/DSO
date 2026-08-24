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
    ''' <summary>
    ''' Identifiants du compte pour le compte duquel les appels sont émis. Ils
    ''' partaient auparavant dans le corps de chaque requête, sous une forme
    ''' différente par route. Ils voyagent maintenant dans l'en-tête Authorization,
    ''' que le filtre d'authentification du serveur est seul à lire.
    ''' </summary>
    Private identifiants As LoginRequest

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
    ''' Login via api rest oasis.
    '''
    ''' Renvoie la chaîne de connexion déchiffrée et l'utilisateur authentifié.
    ''' Le client se contentait auparavant de la chaîne, puis rejouait
    ''' l'authentification contre la base pour construire son Utilisateur : c'est
    ''' pour cela qu'il lui fallait lire l'empreinte du mot de passe et la clé de
    ''' signature. Le serveur renvoie maintenant l'objet, sans ces deux champs.
    ''' </summary>
    Public Function loginRest(loginRequest As LoginRequest) As LoginResponse
        identifiants = loginRequest
        Dim reponse = login().GetAwaiter.GetResult()
        If reponse Is Nothing OrElse String.IsNullOrWhiteSpace(reponse.ChaineConnexion) Then
            Throw New Exception("Réponse d'authentification inexploitable.")
        End If
        reponse.ChaineConnexion = DecryptString(reponse.ChaineConnexion)
        Return reponse
    End Function

    ''' <summary>
    ''' Demande au serveur de signer une charge avec la clé du compte fourni.
    ''' La clé privée ne descend pas sur le poste.
    ''' </summary>
    Public Function signerRest(loginRequest As LoginRequest, charge As Byte()) As SignatureResponse
        identifiants = loginRequest
        Return signer(charge).GetAwaiter.GetResult()
    End Function

    ''' <summary>
    ''' Demande au serveur de générer une clé de signature pour un utilisateur.
    ''' Seule l'adresse publique revient.
    ''' </summary>
    Public Function genererCleRest(loginRequest As LoginRequest, requete As CleSignatureRequest) As CleSignatureResponse
        identifiants = loginRequest
        Return genererCle(requete).GetAwaiter.GetResult()
    End Function

    ''' <summary>
    ''' Change le mot de passe d'un compte. Le calcul de l'empreinte et l'écriture
    ''' sont faits par le serveur : le poste n'a plus le droit d'écrire oa_password.
    ''' </summary>
    Public Sub changerMotDePasseRest(loginRequest As LoginRequest, requete As MotDePasseRequest)
        identifiants = loginRequest
        changerMotDePasse(requete).GetAwaiter.GetResult()
    End Sub

    ''' <summary>Dépôt d'un document sur le serveur.</summary>
    Public Sub uploadFileRest(loginRequest As LoginRequest, srcFileName As String, contenu As Byte())
        identifiants = loginRequest
        Dim str = uploadFile(srcFileName, contenu).GetAwaiter.GetResult()
    End Sub

    ''' <summary>Récupération d'un document depuis le serveur.</summary>
    Public Function downloadFileRest(loginRequest As LoginRequest, downloadRequest As DownloadRequest) As Byte()
        identifiants = loginRequest
        Return downloadFile(downloadRequest).GetAwaiter.GetResult()
    End Function

    ''' <param name="patientId">
    ''' Dossier concerné, 0 si l'envoi n'en vise aucun. Le serveur s'en sert pour
    ''' vérifier que le destinataire est bien connu de ce dossier, et pour tracer.
    ''' </param>
    Public Function sendMailRest(loginRequest As LoginRequest, mailOasis As MailOasis,
                                 Optional patientId As Long = 0) As String
        identifiants = loginRequest
        Return sendMail(mailOasis, patientId).GetAwaiter.GetResult()
    End Function

    ''' <summary>Renommage d'un document sur le serveur.</summary>
    Public Sub renameFileRest(loginRequest As LoginRequest, renameRequest As RenameRequest)
        identifiants = loginRequest
        renameFile(renameRequest).GetAwaiter.GetResult()
    End Sub

    Private Sub init(_serveurDomain As String)
        serveurDomain = _serveurDomain

        ' La validation du certificat serveur est active par défaut. La réponse de
        ' /api/login transporte la chaîne de connexion à la base : sans validation,
        ' un intercepteur sur le trajet réseau peut se faire passer pour le serveur
        ' et récupérer les identifiants.
        '
        ' AllowInvalidServerCertificate=true rétablit l'ancien comportement pour un
        ' environnement de test à certificat auto-signé. À ne jamais activer en
        ' production. La dérogation est limitée à ce client HTTP : elle ne désactive
        ' plus la validation pour l'ensemble du processus.
        Dim handler As New HttpClientHandler()
        If AutoriserCertificatInvalide() Then
            handler.ServerCertificateCustomValidationCallback =
                Function(request, certificate, chain, sslPolicyErrors) True
        End If

        client = New HttpClient(handler)
        ' Sans délai maximal, un serveur qui ne répond plus fige l'appelant pendant
        ' 100 secondes par requête : l'écran de connexion sur le poste, un thread
        ' IIS quand c'est le portail qui appelle.
        client.Timeout = TimeSpan.FromSeconds(30)
    End Sub

    ''' <summary>
    ''' Lit AllowInvalidServerCertificate dans la configuration. Absent ou illisible,
    ''' la valeur est False : on valide le certificat.
    ''' </summary>
    Private Shared Function AutoriserCertificatInvalide() As Boolean
        Dim valeur = ConfigurationManager.AppSettings("AllowInvalidServerCertificate")
        Dim autorise As Boolean
        If String.IsNullOrWhiteSpace(valeur) OrElse Not Boolean.TryParse(valeur, autorise) Then
            Return False
        End If
        Return autorise
    End Function

    Private Function login() As Task(Of LoginResponse)
        initHttp(serveurDomain)

        ' Corps vide : l'authentification est portée par l'en-tête.
        Dim response As HttpResponseMessage = client.PostAsync("/api/login", New StringContent("")).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
        End If
        Return response.Content.ReadAsAsync(Of LoginResponse)()
    End Function

    Private Function signer(charge As Byte()) As Task(Of SignatureResponse)
        initHttp(serveurDomain)

        Dim requete As New SignatureRequest With {
            .Charge = Convert.ToBase64String(charge)
        }
        Dim response As HttpResponseMessage = client.PostAsJsonAsync("/api/signature", requete).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
        End If
        Return response.Content.ReadAsAsync(Of SignatureResponse)()
    End Function

    Private Function changerMotDePasse(requete As MotDePasseRequest) As Task(Of String)
        initHttp(serveurDomain)

        Dim response As HttpResponseMessage = client.PostAsJsonAsync("/api/motdepasse", requete).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
        End If
        Return response.Content.ReadAsStringAsync()
    End Function

    Private Function genererCle(requete As CleSignatureRequest) As Task(Of CleSignatureResponse)
        initHttp(serveurDomain)

        Dim response As HttpResponseMessage = client.PostAsJsonAsync("/api/signature/cle", requete).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
        End If
        Return response.Content.ReadAsAsync(Of CleSignatureResponse)()
    End Function

    Private Function uploadFile(srcFileName As String, contenu As Byte()) As Task(Of String)
        initHttp(serveurDomain)

        ' Plus d'identifiants dans le formulaire : ils sont dans l'en-tête.
        Dim formContent = New MultipartFormDataContent From {
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

        Dim response As HttpResponseMessage = client.PostAsJsonAsync("/api/rename", renameRequest).Result
        If response.StatusCode <> HttpStatusCode.Accepted Then
            If response.StatusCode = HttpStatusCode.Unauthorized Then
                Throw New Exception("Identifiant et/ou mot de passe erroné !")
            End If
            Throw New Exception(response.ReasonPhrase)
        End If
        Return response.Content.ReadAsStringAsync()
    End Function


    Private Function sendMail(mailOasis As MailOasis, patientId As Long) As Task(Of String)
        initHttp(serveurDomain)

        Dim formContent = New MultipartFormDataContent From {
            {New StringContent(patientId.ToString()), "patientId"},
            {New StringContent(mailOasis.AliasFrom), "aliasFrom"},
            {New StringContent(mailOasis.AddressTo), "adressTo"},
            {New StringContent(mailOasis.Subject), "subject"},
            {New StringContent(mailOasis.Body), "body"},
            {New StringContent(mailOasis.IsSousEpisode), "isSousEpisode"},
            {New StringContent(mailOasis.IsHTML), "isHTML"}
        }

        If (mailOasis.IsWithContenu()) Then
            formContent.Add(New StreamContent(New MemoryStream(mailOasis.Contenu)), "filekey", mailOasis.Filename)
        End If

        Dim response As HttpResponseMessage = client.PostAsync("/api/sendMail", formContent).Result
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
        PoserAuthentification()
    End Sub

    ''' <summary>
    ''' Place les identifiants courants dans l'en-tête Authorization, en Basic.
    '''
    ''' Basic fait circuler le mot de passe à chaque appel : c'est une étape vers
    ''' un jeton de session, pas une destination. Le remplacement ne touchera que
    ''' cette méthode côté client, et LireIdentifiants côté serveur.
    ''' </summary>
    Private Sub PoserAuthentification()
        client.DefaultRequestHeaders.Authorization = Nothing
        If identifiants Is Nothing OrElse String.IsNullOrEmpty(identifiants.login) Then Exit Sub

        Dim brut = identifiants.login & ":" & If(identifiants.password, "")
        client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue(
            "Basic", Convert.ToBase64String(Text.Encoding.UTF8.GetBytes(brut)))
    End Sub

    ''' <summary>
    ''' Identifiants utilisés pour les appels suivants. À poser avant tout appel
    ''' autre que loginRest, qui les reçoit en paramètre.
    ''' </summary>
    Public Sub UtiliserIdentifiants(loginRequest As LoginRequest)
        identifiants = loginRequest
    End Sub


    Public Sub Dispose() Implements IDisposable.Dispose
        client.Dispose()
    End Sub
End Class
