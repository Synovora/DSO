Imports System.Configuration
Imports System.IO
Imports Oasis_Common
Imports Oasis_WF.My.Resources
Imports Telerik.WinForms.RichTextEditor

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
                Using apiOasis As New ApiOasis()
                    StandardDao.fixConnectionString(apiOasis.loginRest(loginRequest))
                End Using
                loginRequestLog = loginRequest '   pour acces api upload et download ultérieure

                ' -- @@test : test upload
                'Using apiOasis As New ApiOasis()
                'ApiOasis.uploadFileRest(loginRequest.login, loginRequest.password, "webdette.csv", File.ReadAllBytes("C:\db\lore\conciliation\\webdette.csv"))
                'End Using

                ' -- @@test : test download
                'Using apiOasis As New ApiOasis()
                '    Dim downloadRequest As New DownloadRequest With {
                '       .LoginRequest = loginRequest,
                '       .FileName = "webdette.csv"
                '       }
                'File.WriteAllBytes("c:\db\oasis\download\" & downloadRequest.FileName, apiOasis.downloadFileRest(downloadRequest))
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

    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles Me.Load
        '  --- init internationnalisation du richTextBoxEditor ( 1 shot)
        RichTextBoxLocalizationProvider.CurrentProvider = RichTextBoxLocalizationProvider.FromStream(New MemoryStream(New System.Text.UTF8Encoding().GetBytes(FrenchRichTextBoxStrings.RichTextBoxStrings)))
    End Sub
End Class
