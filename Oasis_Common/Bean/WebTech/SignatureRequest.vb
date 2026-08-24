''' <summary>
''' Demande de signature adressée à /api/signature.
'''
''' La clé privée du prescripteur reste sur le serveur : le client envoie la
''' charge à signer et reçoit la signature. Il ne peut donc plus signer au nom
''' d'un autre utilisateur, ni emporter la clé sur son poste.
''' </summary>
Public Class SignatureRequest

    ''' <summary>Charge à signer, encodée en base64.</summary>
    Property Charge As String

End Class
