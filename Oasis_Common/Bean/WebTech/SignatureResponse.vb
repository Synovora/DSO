''' <summary>
''' Résultat d'une signature effectuée par le serveur.
''' </summary>
Public Class SignatureResponse

    ''' <summary>Signature au format Nethereum (0x...).</summary>
    Property Signature As String

    ''' <summary>
    ''' Adresse publique du signataire au moment de la signature. C'est cette
    ''' valeur qui est enregistrée à côté de la signature et rejouée lors de la
    ''' vérification, de sorte qu'une rotation de clé n'invalide pas les
    ''' ordonnances déjà signées.
    ''' </summary>
    Property Adresse As String

End Class
