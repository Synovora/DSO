Imports System.Collections.Specialized
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Security.Principal
Imports System.Threading
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing
Imports System.Web.Security
Imports Oasis_Common

''' <summary>
''' Appel direct des contrôleurs MVC du portail patient, sans IIS ni rendu de vue.
'''
''' Le portail authentifie l'internaute par un ticket Forms dont le nom est l'id
''' de l'internaute (AuthController.Login). En production, le module Forms lit le
''' cookie et pose un FormsIdentity sur HttpContext.User ; ici, le faux contexte
''' reçoit directement ce FormsIdentity. Le patient affiché est ensuite déduit de
''' la première permission de l'internaute (PortailController.GetPatientConnecte).
'''
''' Les attributs de filtre ne s'exécutent pas quand on appelle l'action à la
''' main : AutorisationPortail rejoue l'AuthorizeAttribute global de
''' FilterConfig sur le descripteur de l'action, AllowAnonymous compris.
'''
''' Ordre à respecter dans un test : créer les données d'abord (compte Client par
''' défaut, celui du client lourd qui crée patients et comptes portail), puis
''' construire le contrôleur, qui bascule sur le compte Web du serveur.
''' </summary>
Friend Module PortailDeTest

    ''' <summary>Adresse du faux navigateur quand aucun proxy ne transmet X-Forwarded-For.</summary>
    Friend Const AdresseAppelantPortail As String = "198.51.100.20"

    ' --- Contrôleurs ------------------------------------------------------------------

    ''' <summary>Contrôleur neuf pour un internaute authentifié par son ticket Forms.</summary>
    Friend Function ControleurPortail(Of T As {Controller, New})(internauteId As Long,
                                                                 Optional methode As String = "GET") As T
        Return ControleurPortailPour(Of T)(PrincipalInternautePortail(internauteId), methode)
    End Function

    ''' <summary>Contrôleur neuf pour un visiteur non authentifié.</summary>
    Friend Function ControleurPortailAnonyme(Of T As {Controller, New})(Optional methode As String = "GET") As T
        Return ControleurPortailPour(Of T)(PrincipalAnonymePortail(), methode)
    End Function

    ''' <summary>
    ''' Contrôleur neuf dont HttpContext.User vaut principal (Nothing permis), sous le
    ''' compte Web. Url est renseigné : Login s'en sert pour valider ReturnUrl.
    ''' </summary>
    Friend Function ControleurPortailPour(Of T As {Controller, New})(principal As IPrincipal,
                                                                    Optional methode As String = "GET") As T
        UtiliserCompte(Compte.Web)
        Dim controleur As New T()
        Dim contexte As New ContexteHttpPortail(principal, methode)
        Dim contexteRequete As New RequestContext(contexte, New RouteData())
        controleur.ControllerContext = New ControllerContext(contexteRequete, controleur)
        controleur.Url = New UrlHelper(contexteRequete)
        Return controleur
    End Function

    ''' <summary>Faux contexte HTTP d'un contrôleur construit par ControleurPortailPour.</summary>
    Friend Function ContextePortail(controleur As Controller) As ContexteHttpPortail
        Return DirectCast(controleur.HttpContext, ContexteHttpPortail)
    End Function

    ' --- Identités ---------------------------------------------------------------------

    ''' <summary>Identité posée par le module Forms pour le ticket de cet internaute.</summary>
    Friend Function PrincipalInternautePortail(internauteId As Long) As IPrincipal
        Return PrincipalTicketPortail(internauteId.ToString(CultureInfo.InvariantCulture))
    End Function

    ''' <summary>Identité Forms authentifiée portant un nom arbitraire (ticket forgé ou ancien).</summary>
    Friend Function PrincipalTicketPortail(nomTicket As String) As IPrincipal
        Return New GenericPrincipal(New FormsIdentity(New FormsAuthenticationTicket(nomTicket, False, 30)), New String() {})
    End Function

    ''' <summary>Visiteur anonyme, tel que le voit ASP.NET sans cookie d'authentification.</summary>
    Friend Function PrincipalAnonymePortail() As IPrincipal
        Return New GenericPrincipal(New GenericIdentity(""), New String() {})
    End Function

    ' --- Filtre d'autorisation ---------------------------------------------------------

    ''' <summary>
    ''' Rejoue sur l'action l'AuthorizeAttribute que FilterConfig enregistre pour
    ''' tout le site (les Authorize posés sur les actions n'y ajoutent ni rôle ni
    ''' utilisateur). Renvoie Nothing quand la requête passe, HttpUnauthorizedResult
    ''' sinon. La méthode est choisie par son nom et ses types de paramètres.
    ''' </summary>
    Friend Function AutorisationPortail(controleur As Controller, nomMethode As String,
                                        ParamArray typesParametres() As Type) As ActionResult
        Dim typeControleur = controleur.GetType()
        Dim methode = typeControleur.GetMethod(nomMethode, If(typesParametres, Type.EmptyTypes))
        If methode Is Nothing Then
            Throw New ArgumentException($"Action {nomMethode} introuvable sur {typeControleur.Name}.")
        End If
        Dim nomAction = methode.Name
        Dim attributNom = methode.GetCustomAttribute(Of ActionNameAttribute)()
        If attributNom IsNot Nothing Then nomAction = attributNom.Name

        Dim descripteur As New ReflectedActionDescriptor(methode, nomAction, New ReflectedControllerDescriptor(typeControleur))
        Dim contexte As New AuthorizationContext(controleur.ControllerContext, descripteur)
        Dim filtreGlobal As New AuthorizeAttribute()
        filtreGlobal.OnAuthorization(contexte)
        Return contexte.Result
    End Function

    ' --- Résultats ---------------------------------------------------------------------

    Friend Function VuePortail(resultat As ActionResult) As ViewResult
        Assert.IsInstanceOfType(resultat, GetType(ViewResult))
        Return DirectCast(resultat, ViewResult)
    End Function

    Friend Function RedirectionPortail(resultat As ActionResult) As RedirectToRouteResult
        Assert.IsInstanceOfType(resultat, GetType(RedirectToRouteResult))
        Return DirectCast(resultat, RedirectToRouteResult)
    End Function

    ''' <summary>Vérifie une redirection vers controleurAttendu/actionAttendue.</summary>
    Friend Sub VerifierRedirectionPortail(resultat As ActionResult, controleurAttendu As String, actionAttendue As String)
        Dim redirection = RedirectionPortail(resultat)
        Assert.AreEqual(actionAttendue, CStr(redirection.RouteValues("action")))
        Assert.AreEqual(controleurAttendu, CStr(redirection.RouteValues("controller")))
    End Sub

    ''' <summary>Vérifie un HttpStatusCodeResult (et, si donnée, sa description).</summary>
    Friend Sub VerifierStatutPortail(resultat As ActionResult, codeAttendu As Integer,
                                     Optional descriptionAttendue As String = Nothing)
        Assert.IsInstanceOfType(resultat, GetType(HttpStatusCodeResult))
        Dim statut = DirectCast(resultat, HttpStatusCodeResult)
        Assert.AreEqual(codeAttendu, statut.StatusCode)
        If descriptionAttendue IsNot Nothing Then Assert.AreEqual(descriptionAttendue, statut.StatusDescription)
    End Sub

    ''' <summary>Refus standard de PortailController.AccesRefuse : 403, « Accès refusé ».</summary>
    Friend Sub VerifierAccesRefusePortail(resultat As ActionResult)
        VerifierStatutPortail(resultat, 403, "Accès refusé")
    End Sub

    ''' <summary>Refus du filtre d'autorisation : 401, avant toute exécution de l'action.</summary>
    Friend Sub VerifierNonAuthentifiePortail(resultat As ActionResult)
        Assert.IsInstanceOfType(resultat, GetType(HttpUnauthorizedResult))
    End Sub

    ''' <summary>Propriété d'un objet de type anonyme (les groupes de Resultats.Index).</summary>
    Friend Function ProprietePortail(objet As Object, nomPropriete As String) As Object
        Return objet.GetType().GetProperty(nomPropriete).GetValue(objet)
    End Function

    ''' <summary>
    ''' Exécute appel sous la culture fr-FR, celle des postes et du serveur :
    ''' plusieurs actions formatent ou relisent des dates selon la culture courante.
    ''' </summary>
    Friend Function EnFrancaisPortail(Of TR)(appel As Func(Of TR)) As TR
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Return appel()
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Function

    ' --- HttpContext.Current, pour FormsAuthentication -----------------------------

    ''' <summary>
    ''' FormsAuthentication.SetAuthCookie et SignOut passent par HttpContext.Current,
    ''' et non par le contexte du contrôleur. Le navigateur déclaré accepte les
    ''' cookies : sans cela, le mode par défaut (UseDeviceProfile) irait chercher le
    ''' profil du navigateur dans la configuration d'IIS.
    ''' </summary>
    Friend Sub OuvrirHttpContextPortail()
        Dim requete As New HttpRequest("", "https://portail.test/Auth/login", "")
        requete.Browser = New HttpBrowserCapabilities() With {
            .Capabilities = New Hashtable(StringComparer.OrdinalIgnoreCase) From {{"cookies", "true"}}
        }
        HttpContext.Current = New HttpContext(requete, New HttpResponse(New StringWriter()))
    End Sub

    Friend Sub FermerHttpContextPortail()
        HttpContext.Current = Nothing
    End Sub

    ''' <summary>
    ''' Le ticket Forms est chiffré avec la machineKey. Sans IIS, la clé
    ''' « AutoGenerate,IsolateApps » par défaut peut ne pas se calculer (pas de
    ''' chemin virtuel d'application) : le test devient alors Inconclusive au lieu
    ''' d'échouer sur une limite de l'hébergement. Une machineKey explicite dans
    ''' app.config lève la limite.
    ''' </summary>
    Friend Sub ExigerChiffrementFormsPortail()
        Try
            FormsAuthentication.Encrypt(New FormsAuthenticationTicket("sonde", False, 1))
        Catch ex As Exception
            Assert.Inconclusive("FormsAuthentication.Encrypt indisponible hors IIS (" & ex.GetType().Name & " : " & ex.Message &
                                ") : déclarer une <machineKey> explicite dans IntegrationTest/app.config.")
        End Try
    End Sub

    ''' <summary>Cookie Forms posé sur HttpContext.Current.Response, ou Nothing.</summary>
    Friend Function CookieFormsPortail() As HttpCookie
        ' Pas d'accès par nom : sur une réponse, l'indexeur crée le cookie absent.
        Dim cookies = HttpContext.Current.Response.Cookies
        For i = 0 To cookies.Count - 1
            If cookies(i).Name = FormsAuthentication.FormsCookieName Then Return cookies(i)
        Next
        Return Nothing
    End Function

    ' --- Données -----------------------------------------------------------------------

    ''' <summary>
    ''' Compte portail rattaché à un patient, comme le bouton « Créer l'internaute »
    ''' de la fiche patient (RadFPatientDetailEdit) : InternauteDao.Create puis
    ''' InternautePermissionDao.Create, permission 1, sous le compte courant. Mot de
    ''' passe : MotDePasseParDefaut. recovery est écrit sans date d'expiration, comme
    ''' le fait le client lourd. Renvoie l'id de l'internaute.
    ''' </summary>
    Friend Function CreerComptePortail(patientId As Long,
                                       Optional email As String = Nothing,
                                       Optional recovery As String = "cle-recuperation-test") As Long
        Dim idInternaute = CreerInternaute(email, recovery)
        AutoriserPatientPortail(idInternaute, patientId)
        Return idInternaute
    End Function

    ''' <summary>Donne à l'internaute l'accès au dossier du patient (InternautePermissionDao.Create).</summary>
    Friend Sub AutoriserPatientPortail(internauteId As Long, patientId As Long)
        Dim daoPermission As New InternautePermissionDao
        daoPermission.Create(New InternautePermission With {
            .Internaute = CInt(internauteId),
            .Patient = CInt(patientId),
            .Permission = 1
        })
    End Sub

    ''' <summary>
    ''' Intervenant du parcours de soins par ParcoursDao.CreateIntervenantParcours,
    ''' comme le parcours par défaut que crée le client lourd (médecin référent
    ''' Oasis, ROR 1). L'historique qu'écrit le DAO nomme la base en dur : le test
    ''' appelant doit commencer par ExigerBaseOasis. Renvoie l'id.
    ''' </summary>
    Friend Function CreerParcoursPortail(patientId As Long, utilisateurId As Long,
                                         Optional base As String = ParcoursDao.EnumParcoursBaseCode.ParAn,
                                         Optional rythme As Integer = 1) As Long
        ' Spécialité 1 et sous-catégorie 4 : médecin référent ; catégorie 3 : suivi
        ' (EnvironnementBase.EnumSpecialiteOasis, EnumSousCategoriePPS, EnumCategoriePPS).
        Dim daoParcours As New ParcoursDao
        Return daoParcours.CreateIntervenantParcours(New Parcours With {
            .PatientId = CInt(patientId),
            .SpecialiteId = 1,
            .CategorieId = 3,
            .SousCategorieId = 4,
            .IntervenantOasis = True,
            .RorId = 1,
            .Commentaire = "",
            .Base = base,
            .Rythme = rythme,
            .Cacher = False,
            .Inactif = False,
            .UserCreation = CInt(utilisateurId),
            .DateCreation = Date.Today
        }, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
    End Function

End Module

' --- Faux objets HTTP --------------------------------------------------------------------
' Aucune bibliothèque d'imitation dans les paquets : sous-classes écrites à la main,
' limitées à ce que les contrôleurs du portail et AuthorizeAttribute lisent.

Friend NotInheritable Class ContexteHttpPortail
    Inherits HttpContextBase

    Private ReadOnly _requete As RequetePortail
    Private ReadOnly _reponse As New ReponsePortail()
    Private ReadOnly _session As New SessionPortail()
    Private ReadOnly _elements As New Hashtable()
    Private _principal As IPrincipal

    Friend Sub New(principal As IPrincipal, methode As String)
        _principal = principal
        _requete = New RequetePortail(methode)
    End Sub

    Friend ReadOnly Property FausseRequete As RequetePortail
        Get
            Return _requete
        End Get
    End Property

    Friend ReadOnly Property FausseReponse As ReponsePortail
        Get
            Return _reponse
        End Get
    End Property

    Friend ReadOnly Property FausseSession As SessionPortail
        Get
            Return _session
        End Get
    End Property

    Public Overrides ReadOnly Property Request As HttpRequestBase
        Get
            Return _requete
        End Get
    End Property

    Public Overrides ReadOnly Property Response As HttpResponseBase
        Get
            Return _reponse
        End Get
    End Property

    Public Overrides ReadOnly Property Session As HttpSessionStateBase
        Get
            Return _session
        End Get
    End Property

    Public Overrides ReadOnly Property Items As IDictionary
        Get
            Return _elements
        End Get
    End Property

    Public Overrides Property User As IPrincipal
        Get
            Return _principal
        End Get
        Set(value As IPrincipal)
            _principal = value
        End Set
    End Property

End Class

Friend NotInheritable Class RequetePortail
    Inherits HttpRequestBase

    Private ReadOnly _methode As String
    Private ReadOnly _variablesServeur As New NameValueCollection()
    Private ReadOnly _entetes As New NameValueCollection()
    Private ReadOnly _formulaire As New NameValueCollection()
    Private ReadOnly _chaineRequete As New NameValueCollection()
    Private ReadOnly _cookies As New HttpCookieCollection()

    ''' <summary>Adresse de l'appelant vue par IIS (REMOTE_ADDR).</summary>
    Friend Property AdresseClient As String = AdresseAppelantPortail

    Friend Sub New(methode As String)
        _methode = methode
    End Sub

    Public Overrides ReadOnly Property HttpMethod As String
        Get
            Return _methode
        End Get
    End Property

    Public Overrides ReadOnly Property ServerVariables As NameValueCollection
        Get
            Return _variablesServeur
        End Get
    End Property

    Public Overrides ReadOnly Property Headers As NameValueCollection
        Get
            Return _entetes
        End Get
    End Property

    Public Overrides ReadOnly Property Form As NameValueCollection
        Get
            Return _formulaire
        End Get
    End Property

    Public Overrides ReadOnly Property QueryString As NameValueCollection
        Get
            Return _chaineRequete
        End Get
    End Property

    Public Overrides ReadOnly Property Cookies As HttpCookieCollection
        Get
            Return _cookies
        End Get
    End Property

    Public Overrides ReadOnly Property UserHostAddress As String
        Get
            Return AdresseClient
        End Get
    End Property

    Public Overrides ReadOnly Property Url As Uri
        Get
            Return New Uri("https://portail.test/")
        End Get
    End Property

    Public Overrides ReadOnly Property ApplicationPath As String
        Get
            Return "/"
        End Get
    End Property

    Public Overrides ReadOnly Property AppRelativeCurrentExecutionFilePath As String
        Get
            Return "~/"
        End Get
    End Property

    Public Overrides ReadOnly Property PathInfo As String
        Get
            Return ""
        End Get
    End Property

End Class

Friend NotInheritable Class ReponsePortail
    Inherits HttpResponseBase

    Private ReadOnly _cookies As New HttpCookieCollection()
    Private ReadOnly _cache As New CachePortail()
    Private _statut As Integer = 200
    Private _descriptionStatut As String = "OK"

    Public Overrides ReadOnly Property Cookies As HttpCookieCollection
        Get
            Return _cookies
        End Get
    End Property

    Public Overrides ReadOnly Property Cache As HttpCachePolicyBase
        Get
            Return _cache
        End Get
    End Property

    Public Overrides Property StatusCode As Integer
        Get
            Return _statut
        End Get
        Set(value As Integer)
            _statut = value
        End Set
    End Property

    Public Overrides Property StatusDescription As String
        Get
            Return _descriptionStatut
        End Get
        Set(value As String)
            _descriptionStatut = value
        End Set
    End Property

End Class

''' <summary>AuthorizeAttribute règle le cache de la réponse quand il laisse passer.</summary>
Friend NotInheritable Class CachePortail
    Inherits HttpCachePolicyBase

    Public Overrides Sub SetProxyMaxAge(delta As TimeSpan)
    End Sub

    Public Overrides Sub AddValidationCallback(handler As HttpCacheValidateHandler, data As Object)
    End Sub

End Class

Friend NotInheritable Class SessionPortail
    Inherits HttpSessionStateBase

    Private ReadOnly _valeurs As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)

    ''' <summary>Vrai après Session.Abandon.</summary>
    Friend Property Abandonnee As Boolean

    Default Public Overrides Property Item(name As String) As Object
        Get
            Dim valeur As Object = Nothing
            _valeurs.TryGetValue(name, valeur)
            Return valeur
        End Get
        Set(value As Object)
            _valeurs(name) = value
        End Set
    End Property

    Public Overrides ReadOnly Property Count As Integer
        Get
            Return _valeurs.Count
        End Get
    End Property

    Public Overrides Sub Abandon()
        Abandonnee = True
    End Sub

End Class
