Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Nethereum.Signer
Imports Oasis_Common

''' <summary>
''' Signature des ordonnances et des sous-épisodes, et génération des clés.
'''
''' La clé privée du prescripteur vivait en clair dans oa_utilisateur.cle_privee
''' et descendait sur le poste à chaque connexion. Comme tout poste reçoit la
''' chaîne de connexion, n'importe quel utilisateur pouvait lire la clé de
''' n'importe quel prescripteur et signer en son nom, avec une signature que
''' /Sign/Check présente ensuite comme authentique.
'''
''' La clé ne quitte plus le serveur : le client envoie ici la charge à signer et
''' repart avec la signature. La base refuse au compte du client lourd la lecture
''' de la colonne, donc ce chemin est le seul.
''' </summary>
Public Class SignatureController
    Inherits ApiControllerOasis

    ''' <summary>
    ''' Taille maximale acceptée pour une charge à signer. Une ordonnance sérialisée
    ''' pèse quelques kilo-octets ; la borne évite qu'un appel authentifié fasse
    ''' hacher des dizaines de mégaoctets au serveur.
    ''' </summary>
    Private Const TailleChargeMaxi As Integer = 1024 * 1024

    <HttpPost>
    <Route("api/signature")>
    Public Function Signer(<FromBody()> ByVal requete As SignatureRequest) As HttpResponseMessage
        Try
            If requete Is Nothing Then
                Return Refus(HttpStatusCode.BadRequest, "Requête incomplète")
            End If

            ' L'utilisateur signe avec sa propre clé et rien d'autre : celle du
            ' compte authentifié par l'en-tête, jamais d'un identifiant passé dans
            ' la requête.
            Dim utilisateur = UtilisateurConnecte

            Dim charge As Byte()
            Try
                charge = Convert.FromBase64String(If(requete.Charge, ""))
            Catch exFormat As FormatException
                Return Refus(HttpStatusCode.BadRequest, "Charge illisible")
            End Try

            If charge.Length = 0 OrElse charge.Length > TailleChargeMaxi Then
                Return Refus(HttpStatusCode.BadRequest, "Charge de taille invalide")
            End If

            If String.IsNullOrWhiteSpace(utilisateur.UtilisateurClePrivee) Then
                Return Refus(HttpStatusCode.Conflict, "Aucune cle de signature pour ce compte")
            End If

            Dim reponse As New SignatureResponse With {
                .Signature = utilisateur.Sign(charge),
                .Adresse = utilisateur.UtilisateurAddress
            }
            Return Request.CreateResponse(HttpStatusCode.Accepted, reponse)

        Catch e As Exception
            ' Le message d'exception peut porter le détail de la clé ou de la base.
            Return Refus(HttpStatusCode.InternalServerError, "Erreur interne au serveur")
        End Try
    End Function

    ''' <summary>
    ''' Génère une paire de clés pour un utilisateur et n'en renvoie que l'adresse.
    ''' Réservé aux administrateurs, ou à l'utilisateur pour lui-même.
    ''' </summary>
    <HttpPost>
    <Route("api/signature/cle")>
    Public Function GenererCle(<FromBody()> ByVal requete As CleSignatureRequest) As HttpResponseMessage
        Try
            If requete Is Nothing Then
                Return Refus(HttpStatusCode.BadRequest, "Requête incomplète")
            End If

            Dim appelant = UtilisateurConnecte
            If Not appelant.UtilisateurAdmin AndAlso appelant.UtilisateurId <> requete.UtilisateurId Then
                Return Refus(HttpStatusCode.Forbidden, "Droits insuffisants")
            End If

            Dim userDao As New UserDao
            ' Remplacer une clé existante coupe le lien entre le prescripteur et les
            ' ordonnances déjà signées de son adresse précédente. La vérification
            ' rejoue l'adresse enregistrée sur l'ordonnance, donc les anciennes
            ' restent valides, mais l'acte reste délibéré.
            If userDao.ACleSignature(requete.UtilisateurId) AndAlso Not requete.Remplacer Then
                Return Refus(HttpStatusCode.Conflict, "Cet utilisateur a deja une cle")
            End If

            Dim ecKey As EthECKey = EthECKey.GenerateKey()
            Dim clePrivee = "0x" & BitConverter.ToString(ecKey.GetPrivateKeyAsBytes()).Replace("-", "")
            Dim adresse = ecKey.GetPublicAddress()
            userDao.EnregistrerCleSignature(requete.UtilisateurId, clePrivee, adresse)

            JournalAcces.Modification(appelant, 0,
                                      "Generation cle de signature pour l'utilisateur " & requete.UtilisateurId)

            Return Request.CreateResponse(HttpStatusCode.Accepted,
                                          New CleSignatureResponse With {.Adresse = adresse})

        Catch e As Exception
            Return Refus(HttpStatusCode.InternalServerError, "Erreur interne au serveur")
        End Try
    End Function

End Class
