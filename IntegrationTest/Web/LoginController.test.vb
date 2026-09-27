Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Security.Principal
Imports System.Text
Imports System.Threading
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Web.Http.Filters
Imports Newtonsoft.Json
Imports Oasis_Common
Imports Oasis_Web
Imports Oasis_Web.Filters

''' <summary>
''' Appel d'une action d'API comme le fait le serveur : filtre d'authentification
''' global, puis AuthorizeAttribute global (voir WebApiConfig), puis l'action.
'''
''' On n'héberge pas le pipeline complet en mémoire : le filtre écrit dans
''' HttpContext.Current, qui ne suit pas les continuations asynchrones hors
''' d'IIS. Les étapes sont donc rejouées dans l'ordre, sur le thread du test.
''' Conséquence : la liaison du corps n'est pas exercée. Un corps JSON illisible
''' donne Nothing à l'action, c'est ce que les tests passent pour le simuler.
''' </summary>
Friend Module ApiDeTest

    Private principalAvantTest As IPrincipal

    ''' <summary>En-tête Authorization: Basic tel que l'envoie ApiOasis.</summary>
    Friend Function EnteteBasic(login As String, motDePasse As String) As AuthenticationHeaderValue
        Return New AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(login & ":" & motDePasse)))
    End Function

    ''' <summary>
    ''' Exécute nomAction sur un contrôleur neuf, derrière les deux filtres
    ''' globaux. Un refus d'un filtre renvoie sa réponse sans appeler l'action.
    ''' </summary>
    Friend Function AppelerApi(Of T As {ApiController, New})(entete As AuthenticationHeaderValue,
                                                            nomAction As String,
                                                            appel As Func(Of T, HttpResponseMessage),
                                                            Optional corps As HttpContent = Nothing) As HttpResponseMessage
        Dim configurationApi As New HttpConfiguration()
        Dim message As New HttpRequestMessage(HttpMethod.Post, "https://oasis.test/api/" & nomAction)
        message.Headers.Authorization = entete
        If corps IsNot Nothing Then message.Content = corps
        message.SetConfiguration(configurationApi)

        Dim controleur As New T()
        controleur.Configuration = configurationApi
        controleur.Request = message
        Dim descripteur As New HttpControllerDescriptor(configurationApi, GetType(T).Name, GetType(T))
        controleur.ControllerContext.ControllerDescriptor = descripteur
        controleur.ControllerContext.Controller = controleur
        Dim contexteAction As New HttpActionContext(
            controleur.ControllerContext,
            New ReflectedHttpActionDescriptor(descripteur, GetType(T).GetMethod(nomAction)))

        ' Authentification : AuthentificationApiAttribute, enregistré en premier.
        Dim contexteAuthentification As New HttpAuthenticationContext(contexteAction, Nothing)
        Dim filtre As New AuthentificationApiAttribute()
        filtre.AuthenticateAsync(contexteAuthentification, CancellationToken.None).Wait()
        If contexteAuthentification.ErrorResult IsNot Nothing Then
            Return contexteAuthentification.ErrorResult.ExecuteAsync(CancellationToken.None).Result
        End If
        controleur.RequestContext.Principal = contexteAuthentification.Principal

        ' Autorisation : l'AuthorizeAttribute de Web API, pas celui de MVC.
        Dim autorisation As New System.Web.Http.AuthorizeAttribute()
        autorisation.OnAuthorization(contexteAction)
        If contexteAction.Response IsNot Nothing Then Return contexteAction.Response

        Return appel(controleur)
    End Function

    ''' <summary>POST /api/login avec l'en-tête Basic, comme ApiOasis.login.</summary>
    Friend Function SeConnecter(login As String, motDePasse As String,
                                Optional corps As HttpContent = Nothing) As HttpResponseMessage
        Return AppelerApi(Of LoginController)(EnteteBasic(login, motDePasse), "PostValue",
                                              Function(c) c.PostValue(), corps)
    End Function

    Friend Function CorpsDe(reponse As HttpResponseMessage) As String
        If reponse.Content Is Nothing Then Return ""
        Return reponse.Content.ReadAsStringAsync().Result
    End Function

    ''' <summary>Corps désérialisé comme le client lourd le lit (Newtonsoft, réglages par défaut).</summary>
    Friend Function LireJson(Of T)(reponse As HttpResponseMessage) As T
        Return JsonConvert.DeserializeObject(Of T)(CorpsDe(reponse))
    End Function

    ''' <summary>
    ''' Le filtre affecte HttpContext.Current.User : sans contexte, il lève
    ''' NullReferenceException et répond 500. IIS en fournit un, le test aussi.
    ''' </summary>
    Friend Sub OuvrirContexteHttp()
        principalAvantTest = Thread.CurrentPrincipal
        System.Web.HttpContext.Current = New System.Web.HttpContext(
            New System.Web.HttpRequest("", "https://oasis.test/api/", ""),
            New System.Web.HttpResponse(New IO.StringWriter()))
    End Sub

    Friend Sub FermerContexteHttp()
        System.Web.HttpContext.Current = Nothing
        Thread.CurrentPrincipal = principalAvantTest
    End Sub

End Module

''' <summary>
''' /api/login : ouverture de session du client lourd, sous le compte du serveur.
''' </summary>
<TestClass()> Public Class TestControleurLogin
    Inherits TestIntegration

    Private Const MessageRefus As String = "Identifiant et/ou mot de passe erroné !"

    <TestInitialize>
    Public Sub PreparerServeur()
        UtiliserCompte(Compte.Web)
        OuvrirContexteHttp()
    End Sub

    <TestCleanup>
    Public Sub FermerServeur()
        FermerContexteHttp()
    End Sub

    Private Shared Function NombreEchecs(idUtilisateur As Long) As Integer
        Return CInt(Scalaire("SELECT COALESCE(oa_utilisateur_tentatives, 0) FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0",
                             idUtilisateur))
    End Function

    Private Shared Function EstVerrouille(idUtilisateur As Long) As Boolean
        Return CInt(Scalaire("SELECT CASE WHEN oa_utilisateur_verrou_jusqua > SYSDATETIME() THEN 1 ELSE 0 END" &
                             " FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idUtilisateur)) = 1
    End Function

    Private Shared Sub VerrouillerPour(idUtilisateur As Long, nbEchecs As Integer, minutes As Integer)
        Executer("UPDATE oasis.oa_utilisateur SET oa_utilisateur_tentatives = @p0," &
                 " oa_utilisateur_verrou_jusqua = DATEADD(minute, @p1, SYSDATETIME()) WHERE oa_utilisateur_id = @p2",
                 nbEchecs, minutes, idUtilisateur)
    End Sub

    <TestMethod()> Public Sub UnBonMotDePasseRendLaChaineDuCompteClient()
        Dim idUtilisateur = CreerUtilisateur("login.valide")

        Dim reponse = SeConnecter("login.valide", MotDePasseParDefaut)

        ' Comportement actuel : 202 Accepted et non 200. ApiOasis.login n'accepte que 202.
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Dim lu = LireJson(Of LoginResponse)(reponse)
        Dim chaineRecue = DecryptString(lu.ChaineConnexion)
        Assert.AreEqual(ChaineConnexion(Compte.Client), chaineRecue)
        Assert.AreNotEqual(ChaineConnexion(Compte.Web), chaineRecue, "jamais la chaîne du serveur")
        Assert.AreEqual(CInt(idUtilisateur), lu.Utilisateur.UtilisateurId)
        Assert.AreEqual("login.valide", lu.Utilisateur.UtilisateurLogin)
    End Sub

    <TestMethod()> Public Sub LaReponseNePorteNiEmpreinteNiClePrivee()
        Dim idUtilisateur = CreerUtilisateur("login.secrets", avecCle:=True)
        Dim empreinte = CStr(Scalaire("SELECT oa_password FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idUtilisateur)).Trim()
        Dim clePrivee = CStr(Scalaire("SELECT cle_privee FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idUtilisateur))
        Dim adresse = CStr(Scalaire("SELECT cle_publique FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idUtilisateur))
        Assert.IsFalse(String.IsNullOrEmpty(clePrivee), "le jeu de données doit fournir une clé")

        Dim reponse = SeConnecter("login.secrets", MotDePasseParDefaut)

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Dim json = CorpsDe(reponse)
        Dim lu = JsonConvert.DeserializeObject(Of LoginResponse)(json)
        Assert.IsNull(lu.Utilisateur.Password)
        Assert.IsTrue(String.IsNullOrEmpty(lu.Utilisateur.UtilisateurClePrivee))
        Assert.IsFalse(json.Contains(empreinte), "l'empreinte ne doit pas partir sur le réseau")
        Assert.IsFalse(json.Contains(clePrivee), "la clé privée ne doit pas partir sur le réseau")
        ' L'adresse publique, elle, est attendue par le client.
        Assert.AreEqual(adresse, lu.Utilisateur.UtilisateurAddress)
    End Sub

    <TestMethod()> Public Sub UnMauvaisMotDePasseEstRefuseEtCompte()
        Dim idUtilisateur = CreerUtilisateur("login.erreur")

        Dim reponse = SeConnecter("login.erreur", "Mauvais!2026")

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual(MessageRefus, CorpsDe(reponse))
        Assert.AreEqual(1, NombreEchecs(idUtilisateur))
        Assert.IsFalse(EstVerrouille(idUtilisateur))
    End Sub

    <TestMethod()> Public Sub UnLoginInconnuRecoitLaMemeReponse()
        Dim reponse = SeConnecter("login.inexistant", MotDePasseParDefaut)

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual(MessageRefus, CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UnUtilisateurInactifEstRefuse()
        Dim idUtilisateur = CreerUtilisateur("login.inactif")
        Executer("UPDATE oasis.oa_utilisateur SET oa_utilisateur_etat = 'I' WHERE oa_utilisateur_id = @p0", idUtilisateur)

        Dim reponse = SeConnecter("login.inactif", MotDePasseParDefaut)

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual(MessageRefus, CorpsDe(reponse))
        Assert.AreEqual(0, NombreEchecs(idUtilisateur), "un compte inactif n'est même pas lu")
    End Sub

    <TestMethod()> Public Sub UnCompteVerrouilleEstRefuseMemeAvecLeBonMotDePasse()
        Dim idUtilisateur = CreerUtilisateur("login.verrou")
        VerrouillerPour(idUtilisateur, 5, 10)

        Dim reponse = SeConnecter("login.verrou", MotDePasseParDefaut)

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual(MessageRefus, CorpsDe(reponse), "même réponse qu'un mauvais mot de passe")
        ' Comportement actuel : pendant le verrou, le mot de passe n'est pas vérifié
        ' et le compteur ne bouge pas.
        Assert.AreEqual(5, NombreEchecs(idUtilisateur))
        Assert.IsTrue(EstVerrouille(idUtilisateur))
    End Sub

    <TestMethod()> Public Sub CinqEchecsVerrouillentLeCompte()
        Dim idUtilisateur = CreerUtilisateur("login.cinq")
        For i = 1 To 4
            SeConnecter("login.cinq", "Mauvais!2026")
        Next
        Assert.AreEqual(4, NombreEchecs(idUtilisateur))
        Assert.IsFalse(EstVerrouille(idUtilisateur), "pas de verrou avant le seuil")

        SeConnecter("login.cinq", "Mauvais!2026")

        Assert.AreEqual(5, NombreEchecs(idUtilisateur))
        Assert.IsTrue(EstVerrouille(idUtilisateur))
        Assert.AreEqual(HttpStatusCode.Unauthorized, SeConnecter("login.cinq", MotDePasseParDefaut).StatusCode)
    End Sub

    <TestMethod()> Public Sub LeCompteurRepartAZeroApresUnSucces()
        Dim idUtilisateur = CreerUtilisateur("login.compteur")
        SeConnecter("login.compteur", "Mauvais!2026")
        SeConnecter("login.compteur", "Mauvais!2026")
        Assert.AreEqual(2, NombreEchecs(idUtilisateur))

        Assert.AreEqual(HttpStatusCode.Accepted, SeConnecter("login.compteur", MotDePasseParDefaut).StatusCode)

        Assert.AreEqual(0, NombreEchecs(idUtilisateur))
        Assert.IsTrue(IsDBNull(Scalaire("SELECT oa_utilisateur_verrou_jusqua FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0",
                                        idUtilisateur)))
    End Sub

    <TestMethod()> Public Sub UnVerrouEchuLaisseEntrerEtRemetLeCompteurAZero()
        Dim idUtilisateur = CreerUtilisateur("login.echu")
        VerrouillerPour(idUtilisateur, 5, -1)

        Assert.AreEqual(HttpStatusCode.Accepted, SeConnecter("login.echu", MotDePasseParDefaut).StatusCode)

        Assert.AreEqual(0, NombreEchecs(idUtilisateur))
        Assert.IsTrue(IsDBNull(Scalaire("SELECT oa_utilisateur_verrou_jusqua FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0",
                                        idUtilisateur)))
    End Sub

    <TestMethod()> Public Sub ApresUnVerrouEchuUnSeulEchecReverrouille()
        Dim idUtilisateur = CreerUtilisateur("login.rechute")
        VerrouillerPour(idUtilisateur, 5, -1)

        SeConnecter("login.rechute", "Mauvais!2026")

        ' Comportement actuel : l'échéance du verrou ne remet pas le compteur à zéro.
        ' Il passe à 6, au-dessus du seuil, et un seul échec reverrouille 15 minutes.
        Assert.AreEqual(6, NombreEchecs(idUtilisateur))
        Assert.IsTrue(EstVerrouille(idUtilisateur))
    End Sub

    <TestMethod()> Public Sub SansEnTeteLAppelEstRefuse()
        CreerUtilisateur("login.anonyme")

        Dim reponse = AppelerApi(Of LoginController)(Nothing, "PostValue", Function(c) c.PostValue())

        ' Refus de l'AuthorizeAttribute global, pas du filtre d'authentification.
        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub UnEnTeteIllisibleEstTraiteCommeAbsent()
        Dim reponse = AppelerApi(Of LoginController)(New AuthenticationHeaderValue("Basic", "pas-du-base64!"),
                                                     "PostValue", Function(c) c.PostValue())

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub LeCorpsDeLaRequeteEstIgnore()
        CreerUtilisateur("login.corps")
        Dim corpsIllisible As New StringContent("{pas du json", Encoding.UTF8, "application/json")

        Dim reponse = SeConnecter("login.corps", MotDePasseParDefaut, corpsIllisible)

        ' PostValue ne prend aucun paramètre : le corps n'est jamais lu.
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub LesIdentifiantsDansLeCorpsNeSuffisentPlus()
        CreerUtilisateur("login.ancien")
        Dim ancienProtocole = JsonConvert.SerializeObject(
            New LoginRequest With {.login = "login.ancien", .password = MotDePasseParDefaut})

        Dim reponse = AppelerApi(Of LoginController)(Nothing, "PostValue", Function(c) c.PostValue(),
                                                     New StringContent(ancienProtocole, Encoding.UTF8, "application/json"))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub UneAncienneEmpreinteEstMigreeALaConnexion()
        Dim idUtilisateur = CreerUtilisateur("login.sha1")
        Executer("UPDATE oasis.oa_utilisateur SET oa_password = @p0 WHERE oa_utilisateur_id = @p1",
                 Utilisateur.CryptePwd("login.sha1", MotDePasseParDefaut), idUtilisateur)

        Assert.AreEqual(HttpStatusCode.Accepted, SeConnecter("login.sha1", MotDePasseParDefaut).StatusCode)

        Dim stockee = CStr(Scalaire("SELECT oa_password FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idUtilisateur)).Trim()
        Assert.IsTrue(stockee.StartsWith("PBKDF2$"), stockee)
        Assert.IsTrue(MotDePasse.Verifier(MotDePasseParDefaut, stockee))
    End Sub

End Class
