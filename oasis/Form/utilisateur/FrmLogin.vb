Imports System.Configuration
Imports System.IO
Imports Oasis_Common
Imports Oasis_WF.My.Resources
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization
Imports Telerik.WinForms.RichTextEditor

Public Class FrmLogin

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()
        AfficheTitleForm(Me, Me.Text, userLog)
        '  --- init internationnalisation du richTextBoxEditor ( 1 shot)
        RichTextBoxLocalizationProvider.CurrentProvider = RichTextBoxLocalizationProvider.FromStream(New MemoryStream(New System.Text.UTF8Encoding().GetBytes(FrenchRichTextBoxStrings.RichTextBoxStrings)))
        '  --- init internationnalisation du radgridview
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        Dim contactAdmin = ConfigurationManager.AppSettings("ContactAdministrateur")
        LblContactAdmin.Text = contactAdmin
        System.Threading.Thread.Sleep(2000)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function ChgtPassword() As Boolean
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using frm As New FrmChangePassword
                frm.ShowDialog()
                Return frm.Tag
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
        Return False

    End Function


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
        ' -- permet de voir si on vient du label "Changer mon mot de passe" ou du bouton "Valider"
        Dim isChgtVolontaire = Not TryCast(sender, RadLabel) Is Nothing

        If isChgtVolontaire AndAlso Me.TxtPassword.Text = "*" AndAlso Me.TxtLogin.Text = "" Then
            Dim frm As New FAuthentificattion
            frm.ShowDialog()
            Return
        End If

        If IsPermission() = False Then PasLeDroitLala()


        ' objet global pour APIs
        loginRequestLog = New LoginRequest() With {
                .login = Me.TxtLogin.Text,
                .password = Me.TxtPassword.Text
            }

        ' --- recherche chaine de connextion / api rest
        If StandardDao.IsConnectionStringFixed() = False Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Using apiOasis As New ApiOasis()
                    StandardDao.FixConnectionString(apiOasis.loginRest(loginRequestLog))
                End Using
            Catch ex As Exception
                If ex.Message = "Identifiant et/ou mot de passe erroné !" AndAlso IsPermission(True) = False Then PasLeDroitLala()
                If MsgBox("" & ex.Message & vbCrLf & "Réessayer ?", MsgBoxStyle.YesNo Or MessageBoxIcon.Error, "Authentification Api") = MsgBoxResult.Yes Then
                    Return
                Else
                    If isChgtVolontaire Then Return
                    Close()
                    End
                End If
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If

        Dim userDao As UserDao = New UserDao
        Me.Cursor = Cursors.WaitCursor
        Try
            userLog = userDao.getUserByLoginPassword(Me.TxtLogin.Text, Me.TxtPassword.Text)
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            If ex.Message = "Identifiant et/ou mot de passe erroné !" AndAlso IsPermission(True) = False Then PasLeDroitLala()
            Dim unused = MessageBox.Show("" & ex.Message, "Authentification", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Finally
            Me.Cursor = Cursors.Default
        End Try

        ResetPermission()

        ' --- test si changement de mot de passe imposé
        If userLog.IsPasswordUniqueUsage OrElse isChgtVolontaire Then
            If ChgtPassword() = False Then Return
        End If

        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()
        Try
            'Using form As New FrmTacheMain
            Using form As New RadFPatientListe
                form.UtilisateurConnecte = userLog
                Me.Hide()
                form.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Cursor = Cursors.Default
        End Try

        TxtLogin.Text = ""
        TxtPassword.Text = ""
        Me.Show()
        TxtLogin.Focus()

    End Sub

    Private Sub PasLeDroitLala()
        MsgBox("SECURITÉ - POSTE BLOQUÉ" & vbCrLf & "Contactez votre administrateur !", MsgBoxStyle.OkOnly Or MessageBoxIcon.Stop, "Contrôle de Sécurité")
        Close()
        End
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LblChangePassword_Click(sender As Object, e As EventArgs) Handles LblChangePassword.Click
        BtnValidate_Click(sender, e)
    End Sub

End Class
