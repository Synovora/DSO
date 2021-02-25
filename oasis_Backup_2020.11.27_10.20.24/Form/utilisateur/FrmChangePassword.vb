Imports Oasis_Common

Public Class FrmChangePassword

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        AfficheTitleForm(Me, Me.Text, userLog)

        LblMessagePassword.Text = messageFormatPassword
        Me.Tag = False
    End Sub

    Private Function ctrlFields() As String
        Dim message = ""
        If isValidePassword(TxtPassword1.Text.Trim()) = False Then
            message += ". Le mot de passe doit faire " & messageFormatPassword.ToLower & vbCrLf
        End If
        If TxtPassword1.Text.Trim() <> TxtPassword2.Text.Trim() Then
            message += ". Le mot de passe saisie est différent de la reSaisie " & vbCrLf
        End If
        If message = "" And TxtPassword1.Text.Trim = loginRequestLog.password Then
            message += ". Le mot de passe saisie doit être différent du précédent " & vbCrLf
        End If
        Return message
    End Function


    Private Sub BtnValider_Click(sender As Object, e As EventArgs) Handles BtnValider.Click
        Dim message = ctrlFields()
        If message <> "" Then
            MsgBox(message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Formulaire incorrectement renseigné")
            Exit Sub
        End If
        ' --- sauve nouveau mot de passe
        userLog.Password = Utilisateur.CryptePwd(userLog.UtilisateurLogin, TxtPassword1.Text.Trim)
        userLog.IsPasswordUniqueUsage = False
        Dim userDao As UserDao = New UserDao
        userDao.UpdateSansChangerEtatEtDates(userLog)
        ' -- maj pour Api
        loginRequestLog.password = TxtPassword1.Text.Trim
        ' -- indique chgt effectué
        Me.Tag = True
        Notification.show("Modification Mot de Passe", "Mot de Passe modifié avec succès !", 1)

        Close()
    End Sub

    Private Sub FrmChangePassword_Activated(sender As Object, e As EventArgs) Handles Me.Activated

    End Sub
End Class
