Imports Oasis_Common

Public Class FrmLogin

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        End
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnValidate_Click(sender As Object, e As EventArgs) Handles BtnValidate.Click
        Dim userDao As UserDao = New UserDao

        Try
            userLog = userDao.getUserByLoginPassword(Me.TxtLogin.Text, Me.TxtPassword.Text)
        Catch ex As Exception
            Dim unused = MessageBox.Show("Authentification : " & ex.Message, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        Me.Cursor = Cursors.WaitCursor
        Using vFPatientListe As New RadFPatientListe
            vFPatientListe.UtilisateurConnecte = userLog
            Me.Hide()
            vFPatientListe.ShowDialog()
        End Using
        Me.Cursor = Cursors.Default

        TxtLogin.Text = ""
        TxtPassword.Text = ""
        Me.Show()
        TxtLogin.Focus()

    End Sub
End Class
