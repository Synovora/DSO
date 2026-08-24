Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Oasis_Common

''' <summary>
''' Changement de mot de passe des comptes soignants.
'''
''' Le poste calculait l'empreinte et l'écrivait lui-même dans oa_password. Comme
''' tout poste reçoit une connexion à la base, n'importe quel utilisateur pouvait
''' écraser l'empreinte d'un confrère par une valeur de son choix et se connecter
''' à sa place. La base refuse maintenant l'écriture de cette colonne au compte
''' des postes : le changement passe par ici, et le serveur ne l'accepte que pour
''' le compte qui vient de prouver son mot de passe actuel, ou pour un tiers si
''' l'appelant est administrateur.
''' </summary>
Public Class MotDePasseController
    Inherits ApiControllerOasis

    <HttpPost>
    <Route("api/motdepasse")>
    Public Function Changer(<FromBody()> ByVal requete As MotDePasseRequest) As HttpResponseMessage
        Try
            If requete Is Nothing Then
                Return Refus(HttpStatusCode.BadRequest, "Requete incomplete")
            End If

            ' Le mot de passe actuel a été présenté dans l'en-tête et vérifié par le
            ' filtre : c'est la preuve de possession du compte.
            Dim appelant = UtilisateurConnecte

            Dim cible = If(requete.UtilisateurId = 0, appelant.UtilisateurId, requete.UtilisateurId)
            Dim pourAutrui = (cible <> appelant.UtilisateurId)
            If pourAutrui AndAlso Not appelant.UtilisateurAdmin Then
                Return Refus(HttpStatusCode.Forbidden, "Droits insuffisants")
            End If

            If Not isValidePassword(If(requete.NouveauMotDePasse, "")) Then
                Return Refus(HttpStatusCode.BadRequest, "Mot de passe trop faible")
            End If

            ' Un mot de passe posé par un administrateur pour quelqu'un d'autre est
            ' à usage unique : son titulaire devra le changer à la connexion, car
            ' l'administrateur l'a forcément connu.
            Dim userDao As New UserDao
            userDao.UpdateMotDePasse(cible, MotDePasse.Hacher(requete.NouveauMotDePasse), pourAutrui)

            JournalAcces.Modification(appelant, 0,
                                      "Changement de mot de passe du compte " & cible)

            Return Request.CreateResponse(HttpStatusCode.Accepted, "true")

        Catch e As Exception
            Return Refus(HttpStatusCode.InternalServerError, "Erreur interne au serveur")
        End Try
    End Function

End Class
