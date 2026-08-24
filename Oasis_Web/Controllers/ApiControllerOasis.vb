Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Oasis_Common
Imports Oasis_Web.Filters

''' <summary>
''' Base des contrôleurs d'API. Donne accès à l'utilisateur authentifié par le
''' filtre global et fournit une réponse d'erreur uniforme.
'''
''' Les routes lisaient auparavant un login et un mot de passe dans le corps de la
''' requête et appelaient verifPassword chacune de leur côté. L'identité vient
''' maintenant du filtre, une fois, et aucune route ne peut l'oublier.
''' </summary>
Public MustInherit Class ApiControllerOasis
    Inherits ApiController

    ''' <summary>
    ''' Utilisateur authentifié. Jamais Nothing dans une action protégée : le
    ''' filtre d'autorisation aurait déjà refusé l'appel.
    ''' </summary>
    Protected ReadOnly Property UtilisateurConnecte As Utilisateur
        Get
            Dim identite = TryCast(User?.Identity, IdentiteUtilisateur)
            Return If(identite Is Nothing, Nothing, identite.Utilisateur)
        End Get
    End Property

    ''' <summary>
    ''' Réponse d'erreur. Le motif HTTP est réduit à l'ASCII : http.sys rejette une
    ''' ligne de statut portant un caractère accentué. Le message part en clair
    ''' dans le corps, où il n'a pas cette contrainte.
    ''' </summary>
    Protected Function Refus(code As HttpStatusCode, message As String) As HttpResponseMessage
        Dim motif As New System.Text.StringBuilder(message.Length)
        For Each c In message
            motif.Append(If(AscW(c) >= 32 AndAlso AscW(c) < 127, c, " "c))
        Next
        Return New HttpResponseMessage(code) With {
            .Content = New StringContent(message),
            .ReasonPhrase = motif.ToString().Trim()
        }
    End Function

End Class
