''' <summary>
''' Encodage base64url (RFC 4648 §5) des signatures placées dans les URL et les
''' QR codes des ordonnances.
'''
''' Remplace Microsoft.IdentityModel.Tokens.Base64UrlEncoder. Ce paquet n'était
''' utilisé que pour ces deux appels, il traîne une version vulnérable
''' (CVE-2024-21319) et il entraînait derrière lui toute la pile
''' Microsoft.AspNetCore 2.2, hors support depuis 2019. La page publique
''' /Sign/Check décode une valeur fournie par n'importe qui : moins il y a de
''' code entre cette entrée et nous, mieux c'est.
'''
''' Le format produit est identique à celui de la bibliothèque remplacée, sans
''' quoi les ordonnances déjà imprimées ne se vérifieraient plus.
''' </summary>
Public Module Base64Url

    ''' <summary>Encode des octets en base64url, sans caractère de remplissage.</summary>
    Public Function Encoder(donnees As Byte()) As String
        If donnees Is Nothing Then Return ""
        Return Convert.ToBase64String(donnees).
            TrimEnd("="c).
            Replace("+"c, "-"c).
            Replace("/"c, "_"c)
    End Function

    ''' <summary>
    ''' Décode une valeur base64url. Lève FormatException si la valeur n'en est
    ''' pas une : l'appelant doit traiter ce cas, la valeur vient du réseau.
    ''' </summary>
    Public Function DecoderEnOctets(valeur As String) As Byte()
        If valeur Is Nothing Then Throw New FormatException("Valeur absente.")

        Dim standard = valeur.Replace("-"c, "+"c).Replace("_"c, "/"c)
        ' Le remplissage est retiré à l'encodage : on le rétablit pour FromBase64String.
        Select Case standard.Length Mod 4
            Case 0
                ' rien à ajouter
            Case 2
                standard &= "=="
            Case 3
                standard &= "="
            Case Else
                ' Une longueur congrue à 1 modulo 4 ne peut pas venir d'un encodage valide.
                Throw New FormatException("Longueur base64url invalide.")
        End Select

        Return Convert.FromBase64String(standard)
    End Function

End Module
