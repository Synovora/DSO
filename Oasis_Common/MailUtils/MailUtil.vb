Imports MimeKit
Imports Oasis_Common.ParametreMail

Public Class MailUtil

    Dim SMTPServerURL As String, SMTPport As Integer, SMTPUser As String, SMTPPassword As String, SMTPFrom As String

    Public Sub New(SMTPServerURL As String, SMPTPort As Integer, SMTPUser As String, SMTPPassword As String, SMTPFrom As String)
        Me.SMTPServerURL = SMTPServerURL
        Me.SMTPport = SMTPport
        Me.SMTPUser = SMTPUser
        Me.SMTPPassword = SMTPPassword
        Me.SMTPFrom = SMTPFrom
    End Sub

    Public Sub SendMail(user As Utilisateur, mailOasis As MailOasis)

        ' -- recuperation des parametres
        Dim paramMailDao As New ParametreMailDao
        Dim paramSMTP = paramMailDao.GetParametreMailBySiegeIdTypeMailParam(user.UtilisateurSiegeId, TypeMailParams.SMTP_PARAMETERS)


        Dim mimMessage = New MimeMessage()

        With mimMessage
            .From.Add(New MailboxAddress(mailOasis.AliasFrom, Me.SMTPFrom))
            Dim tbl = mailOasis.AdressTo.Split(",")
            For Each adr As String In tbl
                .To.Add(MailboxAddress.Parse(adr))
            Next
            .Subject = mailOasis.Subject

            Dim builder = New BodyBuilder()
            builder.TextBody = mailOasis.Body
            If mailOasis.IsWithContenu Then
                builder.Attachments.Add(mailOasis.Filename, mailOasis.Contenu)
            End If
            .Body = builder.ToMessageBody
        End With

        Using client = New MailKit.Net.Smtp.SmtpClient()
            client.Connect(Me.SMTPServerURL, Me.SMTPport, True)
            client.Authenticate(Me.SMTPUser, Me.SMTPPassword)
            client.Send(mimMessage)
            client.Disconnect(True)
        End Using


    End Sub

End Class
