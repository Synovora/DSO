''' <summary>
''' Demande de changement de mot de passe adressée à /api/motdepasse.
'''
''' L'empreinte était calculée sur le poste et écrite directement en base. Un
''' poste pouvait donc remplacer l'empreinte de n'importe quel compte par une
''' valeur choisie, puis se connecter à sa place. Le calcul et l'écriture sont
''' désormais du ressort du serveur, et la base refuse au poste l'écriture de la
''' colonne.
''' </summary>
Public Class MotDePasseRequest

    ''' <summary>
    ''' Compte dont le mot de passe change. 0 pour l'appelant lui-même. Un autre
    ''' compte n'est accepté que d'un administrateur, et le nouveau mot de passe
    ''' est alors à usage unique.
    ''' </summary>
    Property UtilisateurId As Integer

    ''' <summary>Nouveau mot de passe, en clair. Il ne transite que par HTTPS.</summary>
    Property NouveauMotDePasse As String

End Class
