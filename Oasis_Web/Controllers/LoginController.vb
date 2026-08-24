Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Oasis_Common

Public Class LoginController
    Inherits ApiControllerOasis

    ''' <summary>
    ''' Authentifie et ouvre la session du client lourd.
    '''
    ''' La réponse porte désormais l'utilisateur authentifié en plus de la chaîne
    ''' de connexion. Le client rejouait sinon l'authentification contre la base,
    ''' ce qui l'obligeait à pouvoir lire l'empreinte du mot de passe et la clé de
    ''' signature de tout le monde. Cette forme de réponse est un contrat avec les
    ''' clients déployés : la changer impose une publication ClickOnce simultanée.
    ''' </summary>
    Public Function PostValue() As HttpResponseMessage
        Try
            ' Le filtre d'authentification a déjà vérifié les identifiants portés
            ' par l'en-tête Authorization : cette route ne fait plus que remettre
            ' la session. Les identifiants ne circulent plus dans le corps.
            Dim utilisateur = UtilisateurConnecte

            ' Ceinture et bretelles : ni l'empreinte ni la clé privée ne doivent
            ' partir sur le réseau, quoi qu'ait chargé la couche d'accès.
            utilisateur.Password = Nothing
            utilisateur.UtilisateurClePrivee = ""

            Dim reponse As New LoginResponse With {
                .ChaineConnexion = EncryptString(ChaineConnexionClient()),
                .Utilisateur = utilisateur
            }
            Return Request.CreateResponse(HttpStatusCode.Accepted, reponse)

        Catch e As Exception
            ' Ne jamais renvoyer e.Message : sur cette route il peut contenir la
            ' chaîne de connexion ou le détail de l'erreur SQL.
            Return Refus(HttpStatusCode.InternalServerError, "Erreur interne au serveur")
        End Try
    End Function

    ''' <summary>
    ''' Chaîne de connexion remise aux postes.
    '''
    ''' C'est celle du compte bridé (oasis_client), jamais celle du serveur : elle
    ''' part sur chaque machine cliente, et le compte du serveur peut lire les clés
    ''' de signature et les empreintes de mots de passe. L'absence de l'entrée est
    ''' une erreur de configuration, pas un cas à rattraper en silence par le
    ''' compte du serveur.
    ''' </summary>
    Private Shared Function ChaineConnexionClient() As String
        Dim entree = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnectionClient")
        If entree Is Nothing OrElse String.IsNullOrWhiteSpace(entree.ConnectionString) Then
            Throw New ConfigurationErrorsException(
                "La chaîne 'Oasis_WF.My.MySettings.oasisConnectionClient' est absente de Web.config. " &
                "Voir docs/migrations/2026-08-24-comptes-sql-separes.sql.")
        End If
        Return entree.ConnectionString
    End Function

End Class
