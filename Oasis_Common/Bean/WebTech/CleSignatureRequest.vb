''' <summary>
''' Demande de génération d'une clé de signature pour un utilisateur.
'''
''' La paire était produite sur le poste qui éditait la fiche, puis écrite en
''' base : la clé privée existait donc en clair sur un poste client avant même
''' d'arriver au serveur. Elle est désormais générée par le serveur, qui ne
''' renvoie que l'adresse publique.
''' </summary>
Public Class CleSignatureRequest

    ''' <summary>Utilisateur pour lequel la clé est générée.</summary>
    Property UtilisateurId As Integer

    ''' <summary>
    ''' Vrai pour remplacer une clé existante (rotation). Faux, la demande est
    ''' refusée si l'utilisateur a déjà une clé, afin de ne pas rompre par
    ''' inadvertance le rattachement des ordonnances déjà signées.
    ''' </summary>
    Property Remplacer As Boolean

End Class
