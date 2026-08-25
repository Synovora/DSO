Imports System.Configuration
Imports MimeKit
Imports Oasis_Common.ParametreMail

Public Class MailUtil

    Dim SMTPServerURL As String, SMTPport As Integer, SMTPUser As String, SMTPPassword As String, SMTPFrom As String

    ''' <param name="SMTPport">
    ''' Le paramètre s'appelait SMPTPort, avec les lettres interverties, alors que
    ''' le champ s'appelle SMTPport : l'affectation recopiait donc le champ sur
    ''' lui-même et le port configuré était perdu. MailKit interprète 0 comme
    ''' « port par défaut pour l'option de sécurité », d'où un envoi qui
    ''' fonctionnait tant que le fournisseur écoutait sur 465.
    ''' </param>
    Public Sub New(SMTPServerURL As String, SMTPport As Integer, SMTPUser As String, SMTPPassword As String, SMTPFrom As String)
        Me.SMTPServerURL = SMTPServerURL
        Me.SMTPport = SMTPport
        Me.SMTPUser = SMTPUser
        Me.SMTPPassword = SMTPPassword
        Me.SMTPFrom = SMTPFrom
    End Sub

    Public Sub SendMail(user As Utilisateur, mailOasis As MailOasis)

        ' Les paramètres SMTP sont fournis par l'appelant (le serveur). Cette
        ' méthode les relisait en base sans jamais s'en servir, ce qui obligeait
        ' tout poste appelant à pouvoir lire le mot de passe du compte d'envoi.
        Dim mimMessage = ComposerMessage(mailOasis)

        Using client = New MailKit.Net.Smtp.SmtpClient()
            client.Connect(Me.SMTPServerURL, Me.SMTPport, SecuriteSmtp())
            client.Authenticate(Me.SMTPUser, Me.SMTPPassword)
            client.Send(mimMessage)
            client.Disconnect(True)
        End Using


    End Sub

    ''' <summary>
    ''' Compose le message sans l'envoyer : expéditeur, destinataires validés un
    ''' par un, objet, corps texte ou HTML, pièce jointe. Séparé de l'envoi pour
    ''' être vérifiable sans serveur SMTP.
    ''' </summary>
    Public Function ComposerMessage(mailOasis As MailOasis) As MimeMessage
        Dim mimMessage = New MimeMessage()

        With mimMessage
            .From.Add(New MailboxAddress(mailOasis.AliasFrom, Me.SMTPFrom))
            ' Chaque destinataire est validé : la zone de saisie est libre et le
            ' message peut porter des documents médicaux.
            Dim tbl = If(mailOasis.AddressTo, "").Split(","c)
            Dim nbDestinataires = 0
            For Each adr As String In tbl
                Dim adresse = adr.Trim()
                If adresse = "" Then Continue For
                If adresse.IndexOfAny(New Char() {ChrW(13), ChrW(10), ChrW(0)}) >= 0 OrElse Not IsValidEmail(adresse) Then
                    Throw New ArgumentException("Adresse de destinataire invalide : " & adresse)
                End If
                .To.Add(MailboxAddress.Parse(adresse))
                nbDestinataires += 1
            Next
            If nbDestinataires = 0 Then
                Throw New ArgumentException("Aucun destinataire valide.")
            End If
            .Subject = mailOasis.Subject

            Dim builder = New BodyBuilder()
            If mailOasis.IsHTML = True Then
                builder.HtmlBody = mailOasis.Body
            Else
                builder.TextBody = mailOasis.Body
            End If

            If mailOasis.IsWithContenu Then
                builder.Attachments.Add(mailOasis.Filename, mailOasis.Contenu)
            End If
            .Body = builder.ToMessageBody
        End With

        Return mimMessage
    End Function

    ''' <summary>
    ''' Mode de sécurité de la liaison SMTP, réglé par MailSecuriteSmtp.
    '''
    ''' Par défaut SslOnConnect : TLS dès l'ouverture de la connexion, sans
    ''' négociation en clair. C'était le comportement obtenu jusqu'ici, et il faut
    ''' le conserver par défaut tant que MailKit n'est pas monté de version.
    '''
    ''' MailKit 2.13 est concerné par une injection de réponse pendant STARTTLS,
    ''' qui permet à un intercepteur de forcer le choix d'un mécanisme SASL plus
    ''' faible (GHSA, alerte Dependabot ouverte sur Oasis_Common). Le passage à
    ''' Auto, motivé par les fournisseurs qui n'écoutent qu'en 587, place la
    ''' liaison sur ce chemin de négociation : c'est un réglage à faire
    ''' délibérément, pas une valeur par défaut, et la vraie réponse est de monter
    ''' MailKit et MimeKit en 4.x sur les deux projets.
    '''
    ''' Valeurs acceptées : SslOnConnect, StartTls, StartTlsWhenAvailable, Auto,
    ''' None. Valeur absente ou non reconnue : SslOnConnect.
    ''' </summary>
    Public Shared Function SecuriteSmtp() As MailKit.Security.SecureSocketOptions
        Dim configure = ConfigurationManager.AppSettings("MailSecuriteSmtp")
        If String.IsNullOrWhiteSpace(configure) Then
            Return MailKit.Security.SecureSocketOptions.SslOnConnect
        End If

        Dim choisi As MailKit.Security.SecureSocketOptions
        If Not [Enum].TryParse(configure.Trim(), True, choisi) OrElse
           Not [Enum].IsDefined(GetType(MailKit.Security.SecureSocketOptions), choisi) Then
            Return MailKit.Security.SecureSocketOptions.SslOnConnect
        End If
        Return choisi
    End Function

End Class
