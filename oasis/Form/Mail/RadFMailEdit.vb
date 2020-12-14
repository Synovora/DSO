Imports Oasis_Common
Public Class RadFMailEdit
    Property sendMailTo As String
    Property sendMailFrom As String
    Property sendMailsender As String
    Property sendMailSubject As String

    Private Sub RadFMailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If sendMailTo <> "" Then
            TxtSendMailTo.Text = sendMailTo
        End If

        If sendMailSubject <> "" Then
            TxtSendMailSubject.Text = sendMailSubject
        End If

    End Sub

    Private Sub RadBtnValider_Click(sender As Object, e As EventArgs) Handles RadBtnValider.Click
        Dim validation As Boolean = True
        'Contrôle mail sendMailTo
        If TxtSendMailTo.Text <> "" Then
            If ModuleUtilsBase.IsValidEmail(TxtSendMailTo.Text) = False Then
                MessageBox.Show("Erreur : Le mail du destinataire n'est pas valide")
                validation = False
            End If
        Else
            MessageBox.Show("Erreur : Le mail du destinataire n'est pas renseigné")
            validation = False
        End If

        'Contrôle mail sendMailCc
        If TxtSendMailCc.Text <> "" Then
            If ModuleUtilsBase.IsValidEmail(TxtSendMailCc.Text) = False Then
                MessageBox.Show("Erreur : Le mail en Cc n'est pas valide")
                validation = False
            End If
        End If

        'Contrôle mail sendMailBc

        'Contrôle mail sendMailFrom

        'Contrôle sujet est renseignée
        If TxtSendMailSubject.Text = "" Then
            MessageBox.Show("Erreur : Le sujet est obligatoire")
            validation = False
        End If

        'Contrôle message est renseignée
        If TxtSendMailMessage.Text = "" Then
            MessageBox.Show("Erreur : Le sujet est obligatoire")
            validation = False
        End If

        'Validation
        If validation = True Then
            SendMessage()
        End If

    End Sub

    Private Sub SendMessage()

        'Ecriture dans la table "send_mail_trigger"
        Dim mail As New Mail
        mail.sendMailTo = TxtSendMailTo.Text
        mail.sendMailCc = TxtSendMailCc.Text
        mail.sendMailBcc = TxtSendMailBc.Text
        mail.sendMailFrom = sendMailFrom
        mail.sendMailSender = sendMailsender
        mail.sendMailSubject = TxtSendMailSubject.Text
        mail.sendMailMessage = TxtSendMailMessage.Text

        Dim mailDao As New MailDao
        If mailDao.CreateMail(mail, userLog) = True Then
            Dim form As New RadFNotification()
            form.Message = "Mail en cours d'envoi"
            form.Show()
            Close()
        Else
            MessageBox.Show("Erreur mail !")
        End If

    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
