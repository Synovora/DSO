Imports Nethereum.Signer
Imports Nethereum.Util

''' <summary>
''' Vérification cryptographique des signatures d'ordonnance.
'''
''' La page publique /Sign/Check se contentait jusqu'ici de retrouver une ligne
''' dont la colonne signature valait la valeur passée dans l'URL, puis d'afficher
''' le contenu vivant de cette ligne. Elle ne prouvait donc rien : toute ligne
''' portant cette valeur s'affichait comme authentique, quel que soit son contenu
''' et quel qu'en soit l'auteur.
'''
''' On récupère désormais l'adresse publique à partir de la signature et de la
''' charge signée, et on la compare à l'adresse enregistrée au moment de la
''' signature.
''' </summary>
Public Module VerificationSignature

    Public Enum ResultatVerification
        ''' <summary>Signature valide : la charge et le signataire correspondent.</summary>
        Valide
        ''' <summary>Signature invalide : la charge ou le signataire ne correspond pas.</summary>
        Invalide
        ''' <summary>
        ''' Ordonnance signée avant la conservation de la charge : rien à vérifier.
        ''' Elle ne doit pas être présentée comme authentifiée.
        ''' </summary>
        NonVerifiable
    End Enum

    ''' <summary>
    ''' Vérifie la signature d'une ordonnance à partir de la charge et de l'adresse
    ''' conservées lors de la validation.
    ''' </summary>
    Public Function Verifier(ordonnance As Ordonnance) As ResultatVerification
        If ordonnance Is Nothing Then Return ResultatVerification.Invalide

        If ordonnance.SignaturePayload Is Nothing OrElse ordonnance.SignaturePayload.Length = 0 OrElse
           String.IsNullOrWhiteSpace(ordonnance.SignatureAdresse) OrElse
           String.IsNullOrWhiteSpace(ordonnance.Signature) Then
            Return ResultatVerification.NonVerifiable
        End If

        Try
            Dim signer As New MessageSigner()
            Dim adresseRecuperee = signer.EcRecover(signer.Hash(ordonnance.SignaturePayload), ordonnance.Signature)
            If AddressUtil.Current.AreAddressesTheSame(adresseRecuperee, ordonnance.SignatureAdresse) Then
                Return ResultatVerification.Valide
            End If
            Return ResultatVerification.Invalide
        Catch ex As Exception
            ' Signature illisible : invalide, jamais « valide par défaut ».
            Return ResultatVerification.Invalide
        End Try
    End Function

    ''' <summary>
    ''' Reconstruit l'ordonnance telle qu'elle a été signée. C'est ce contenu qui
    ''' doit être affiché au vérificateur, et non la ligne vivante en base, qui a
    ''' pu être modifiée depuis.
    ''' </summary>
    Public Function OrdonnanceSignee(ordonnance As Ordonnance) As OrdonnanceFull
        If ordonnance Is Nothing OrElse ordonnance.SignaturePayload Is Nothing Then Return Nothing
        Try
            Return OrdonnanceFull.Deserialize(ordonnance.SignaturePayload)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Module
