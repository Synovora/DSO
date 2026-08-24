Imports System.Net
Imports System.Net.Http
Imports System.Security.Principal
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web.Http.Filters
Imports System.Web.Http.Results
Imports Oasis_Common

Namespace Filters

    ''' <summary>
    ''' Identité authentifiée pour un appel d'API. Porte l'Utilisateur résolu, de
    ''' sorte que les contrôleurs n'aient plus à le recharger ni à faire confiance
    ''' à un identifiant présent dans le corps de la requête.
    ''' </summary>
    Public Class IdentiteUtilisateur
        Implements IIdentity

        Public ReadOnly Property Utilisateur As Utilisateur

        Public Sub New(utilisateur As Utilisateur)
            Me.Utilisateur = utilisateur
        End Sub

        Public ReadOnly Property AuthenticationType As String Implements IIdentity.AuthenticationType
            Get
                Return "Basic"
            End Get
        End Property

        Public ReadOnly Property IsAuthenticated As Boolean Implements IIdentity.IsAuthenticated
            Get
                Return Utilisateur IsNot Nothing
            End Get
        End Property

        Public ReadOnly Property Name As String Implements IIdentity.Name
            Get
                Return If(Utilisateur Is Nothing, "", Utilisateur.UtilisateurLogin)
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Authentification de l'API, enregistrée globalement.
    '''
    ''' Rien n'imposait d'authentification au niveau du cadre applicatif : le
    ''' filtre AuthorizeAttribute posé dans FilterConfig ne couvre que les
    ''' contrôleurs MVC, pas les ApiController. Chaque route se défendait donc
    ''' seule, en appelant verifPassword sur des identifiants lus dans le corps de
    ''' la requête, et une route qui oubliait de le faire était ouverte sans que
    ''' rien ne le signale.
    '''
    ''' Les identifiants passent désormais par l'en-tête Authorization, en Basic.
    ''' Un seul endroit les lit, un seul endroit résout l'utilisateur, et le refus
    ''' est le comportement par défaut : une route qui veut être anonyme doit le
    ''' déclarer par AllowAnonymous.
    '''
    ''' Basic est une étape, pas une destination : il fait circuler le mot de passe
    ''' à chaque appel. Le remplacer par un jeton de session ne touchera que
    ''' LireIdentifiants et la construction de l'identité ci-dessous.
    ''' </summary>
    Public Class AuthentificationApiAttribute
        Inherits Attribute
        Implements IAuthenticationFilter

        Public ReadOnly Property AllowMultiple As Boolean Implements IFilter.AllowMultiple
            Get
                Return False
            End Get
        End Property

        Public Function AuthenticateAsync(context As HttpAuthenticationContext,
                                          cancellationToken As CancellationToken) As Task _
                                          Implements IAuthenticationFilter.AuthenticateAsync
            Dim identifiants = LireIdentifiants(context.Request)
            If identifiants Is Nothing Then
                ' Pas d'en-tête exploitable : on laisse le principal anonyme. Les
                ' routes AllowAnonymous passent, les autres seront refusées par le
                ' filtre d'autorisation.
                Return Task.FromResult(0)
            End If

            Try
                Dim utilisateur = verifPassword(identifiants.Item1, identifiants.Item2)
                Dim principal As New GenericPrincipal(New IdentiteUtilisateur(utilisateur), New String() {})
                context.Principal = principal
                Thread.CurrentPrincipal = principal
                HttpContext.Current.User = principal
            Catch ex As ArgumentException
                ' Compte inconnu, mot de passe erroné ou compte verrouillé : même
                ' réponse dans les trois cas.
                context.ErrorResult = New ResponseMessageResult(
                    New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                        .Content = New StringContent("Identifiant et/ou mot de passe erroné !"),
                        .ReasonPhrase = "Authentification refusee"
                    })
            Catch ex As Exception
                context.ErrorResult = New ResponseMessageResult(
                    New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                        .Content = New StringContent("Erreur interne au serveur"),
                        .ReasonPhrase = "Erreur interne au serveur"
                    })
            End Try

            Return Task.FromResult(0)
        End Function

        Public Function ChallengeAsync(context As HttpAuthenticationChallengeContext,
                                       cancellationToken As CancellationToken) As Task _
                                       Implements IAuthenticationFilter.ChallengeAsync
            ' Pas d'en-tête WWW-Authenticate : le client est une application, pas un
            ' navigateur, et une invite d'authentification native n'aurait pas de sens.
            Return Task.FromResult(0)
        End Function

        ''' <summary>
        ''' Login et mot de passe de l'en-tête Authorization: Basic, ou Nothing si
        ''' l'en-tête est absent ou illisible.
        ''' </summary>
        Private Shared Function LireIdentifiants(requete As HttpRequestMessage) As Tuple(Of String, String)
            Dim entete = requete.Headers.Authorization
            If entete Is Nothing OrElse
               Not String.Equals(entete.Scheme, "Basic", StringComparison.OrdinalIgnoreCase) OrElse
               String.IsNullOrWhiteSpace(entete.Parameter) Then
                Return Nothing
            End If

            Dim decode As String
            Try
                decode = Encoding.UTF8.GetString(Convert.FromBase64String(entete.Parameter))
            Catch ex As FormatException
                Return Nothing
            End Try

            ' Le mot de passe peut contenir des deux-points, pas le login.
            Dim separateur = decode.IndexOf(":"c)
            If separateur <= 0 Then Return Nothing

            Return Tuple.Create(decode.Substring(0, separateur), decode.Substring(separateur + 1))
        End Function

    End Class

End Namespace
