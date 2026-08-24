''' <summary>
''' Réponse de /api/login.
'''
''' Elle ne contenait que la chaîne de connexion chiffrée. Le client lourd
''' s'authentifiait alors une seconde fois, directement contre la base, pour
''' construire son Utilisateur : c'est la seule raison pour laquelle il avait
''' besoin de lire oa_password et cle_privee. Le serveur a déjà l'objet en main
''' quand il vérifie le mot de passe, il le renvoie donc, épuré de ses secrets.
'''
''' Changer cette forme casse les clients déjà déployés : toute modification
''' doit partir avec une publication ClickOnce.
''' </summary>
Public Class LoginResponse

    ''' <summary>Chaîne de connexion SQL, chiffrée par EncryptString.</summary>
    Property ChaineConnexion As String

    ''' <summary>
    ''' Utilisateur authentifié. Ni l'empreinte du mot de passe ni la clé privée
    ''' de signature n'y figurent : elles ne quittent jamais le serveur.
    ''' </summary>
    Property Utilisateur As Utilisateur

End Class
