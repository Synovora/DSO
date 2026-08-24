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

        Using client = New MailKit.Net.Smtp.SmtpClient()
            ' Auto : SSL implicite sur 465, STARTTLS sur 587. Le forçage à True
            ' rendait tout fournisseur en STARTTLS impossible à configurer.
            client.Connect(Me.SMTPServerURL, Me.SMTPport,
                           MailKit.Security.SecureSocketOptions.Auto)
            client.Authenticate(Me.SMTPUser, Me.SMTPPassword)
            client.Send(mimMessage)
            client.Disconnect(True)
        End Using


    End Sub

End Class
