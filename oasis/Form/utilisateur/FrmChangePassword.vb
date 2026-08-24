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
        '
        ' L'empreinte était calculée ici et écrite directement en base. Tout poste
        ' pouvait donc réécrire celle d'un autre compte. Le serveur s'en charge, et
        ' n'accepte le changement que sur présentation du mot de passe actuel.
        Dim nouveau = TxtPassword1.Text.Trim
        Try
            Using apiOasis As New ApiOasis()
                apiOasis.changerMotDePasseRest(loginRequestLog, New MotDePasseRequest With {
                    .UtilisateurId = 0,
                    .NouveauMotDePasse = nouveau
                })
            End Using
        Catch ex As Exception
            MsgBox("Le mot de passe n'a pas pu être changé :" & vbCrLf & ex.Message,
                   MsgBoxStyle.Exclamation, "Modification Mot de Passe")
            Exit Sub
        End Try

        userLog.IsPasswordUniqueUsage = False
        ' -- maj pour Api
        loginRequestLog.password = nouveau
        ' -- indique chgt effectué
        Me.Tag = True
        Notification.show("Modification Mot de Passe", "Mot de Passe modifié avec succès !", 1)

        Close()
    End Sub

    Private Sub FrmChangePassword_Activated(sender As Object, e As EventArgs) Handles Me.Activated

    End Sub
End Class
