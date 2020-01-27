Imports System.Configuration
Imports System.IO
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

        ' --- recherche chaine de connextion / api rest
        If StandardDao.isConnectionStringFixed() = False Then
            Me.Cursor = Cursors.WaitCursor
            Dim loginRequest = New LoginRequest() With {
                .login = Me.TxtLogin.Text,
                .password = Me.TxtPassword.Text
            }
            Try
                Using apiOasisLogin As New ApiOasis()
                    StandardDao.fixConnectionString(apiOasisLogin.loginRest(loginRequest))
                End Using

                ' -- @@test : test upload
                'Using apiOasisUpload As New ApiOasis()
                ' apiOasisUpload.uploadFileRest(loginRequest.login, loginRequest.password, "1_1.PDF", File.ReadAllBytes("\db\brice\Directeur technique H_F - Offre d'emploi Directeur technique H_F - Apec, recrutement et offres d'emploi cadres.pdf"))
                'End Using
            Catch ex As Exception
                If MsgBox("" & ex.Message & vbCrLf & "Réessayer ?", MsgBoxStyle.YesNo Or MessageBoxIcon.Error, "Authentification Api") = MsgBoxResult.Yes Then
                    Return
                Else
                    Close()
                    End
                End If
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If

        Dim userDao As UserDao = New UserDao
        Try
            userLog = userDao.getUserByLoginPassword(Me.TxtLogin.Text, Me.TxtPassword.Text)
        Catch ex As Exception
            Dim unused = MessageBox.Show("" & ex.Message, "Authentification", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        Me.Cursor = Cursors.WaitCursor
        Using vFPatientListe As New FrmTacheMain
            'Using vFPatientListe As New RadFPatientListe
            'vFPatientListe.UtilisateurConnecte = userLog
            Me.Hide()
            vFPatientListe.ShowDialog()
        End Using
        Me.Cursor = Cursors.Default

        TxtLogin.Text = ""
        TxtPassword.Text = ""
        Me.Show()
        TxtLogin.Focus()

    End Sub

    Private Sub BtnValidateOld_Click(sender As Object, e As EventArgs) ' Handles BtnValidate.Click
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
