Imports System.Configuration
Imports System.IO
Imports Oasis_Common
Imports Oasis_WF.My.Resources
Imports Telerik.WinControls.UI.Localization
Imports Telerik.WinForms.RichTextEditor

Public Class FAuthentificattion
    Enum EnumUtilisateur
        Medecin_Test
        Informaticien
        Medecin_Francis
        Medecin_Fabrice
        IDE_Jeanne
        IDE_Evelyne
        Sage_femme
        Secrétaire_médicale
        Secrétaire_administrative
    End Enum

    Dim userdao As New UserDao

    Dim UtilisateurConnecte As Utilisateur
    Dim UtilisateurId As Integer
    Dim Admin As Boolean
    'Dim NiveauAcces As Integer

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTry()
    End Sub

    Private Sub FAuthentificattion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.UtilisateurConnecte = New Utilisateur()

        CbxUtilisateur.DataSource = [Enum].GetValues(GetType(EnumUtilisateur))
        RbtAdminNon.Checked = True
    End Sub

    Private Sub RbtAdminOui_CheckedChanged(sender As Object, e As EventArgs) Handles RbtAdminOui.CheckedChanged
        If RbtAdminOui.Checked = True Then
            RbtAdminNon.Checked = False
        Else
            RbtAdminNon.Checked = True
        End If
    End Sub

    Private Sub RbtAdminNon_CheckedChanged(sender As Object, e As EventArgs) Handles RbtAdminNon.CheckedChanged
        If RbtAdminNon.Checked = True Then
            RbtAdminOui.Checked = False
        Else
            RbtAdminOui.Checked = True
        End If
    End Sub

    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click

        InitAppelForm()
        Application.DoEvents()
        Cursor.Current = Cursors.WaitCursor
        Try
            Me.Enabled = False

            Using form As New FrmTacheMain 'FrmAgendaMedecin
                form.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub BtnListePatient_Click(sender As Object, e As EventArgs) Handles BtnListePatient.Click
        InitAppelForm()
        Application.DoEvents()
        Cursor.Current = Cursors.WaitCursor
        Using form As New RadFPatientListe 'FrmAgendaMedecin
            form.UtilisateurConnecte = userLog
            form.ShowDialog()
        End Using
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub BtnAdmin_Click(sender As Object, e As EventArgs) Handles BtnAdmin.Click
        InitAppelForm()
        Application.DoEvents()
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Using vFMenuAdmin As New FrmMain
            vFMenuAdmin.ShowDialog() 'Modal
        End Using
        Me.Cursor = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub InitAppelForm()
        Select Case CbxUtilisateur.Text
            Case "Informaticien"
                UtilisateurId = 7
                Me.UtilisateurConnecte.UtilisateurProfilId = "INFORMATICIEN"
            Case "Medecin_Francis"
                UtilisateurId = 2
                Me.UtilisateurConnecte.UtilisateurProfilId = "MEDECIN"
            Case "Medecin_Fabrice"
                UtilisateurId = 3
                Me.UtilisateurConnecte.UtilisateurProfilId = "MEDECIN"
            Case "Medecin_Test"
                UtilisateurId = 11
                Me.UtilisateurConnecte.UtilisateurProfilId = "MEDECIN"
            Case "IDE_Jeanne"
                UtilisateurId = 5
                Me.UtilisateurConnecte.UtilisateurProfilId = "IDE"
            Case "IDE_Evelyne"
                UtilisateurId = 9
                Me.UtilisateurConnecte.UtilisateurProfilId = "IDE"
            Case "Sage_femme"
                UtilisateurId = 10
                Me.UtilisateurConnecte.UtilisateurProfilId = "SAGE_FEMME"
            Case "Secrétaire_médicale"
                UtilisateurId = 8
                Me.UtilisateurConnecte.UtilisateurProfilId = "SECRETAIRE_MEDICALE"
            Case "Secrétaire_administrative"
                UtilisateurId = 6
                Me.UtilisateurConnecte.UtilisateurProfilId = "ADMINISTRATIF"
            Case Else
                UtilisateurId = 1
        End Select

        If RbtAdminOui.Checked = True Then
            Admin = True
        Else
            Admin = False
        End If

        ' -- pour test api rest
        loginRequestLog = New LoginRequest() With {
                .login = "Bertrand.Gambet",
                .password = "a"
        }

        If StandardDao.isConnectionStringFixed() = False Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Using apiOasis As New ApiOasis()
                    StandardDao.fixConnectionString(apiOasis.loginRest(loginRequestLog))
                End Using

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

        Me.Cursor = Cursors.WaitCursor
        Dim UtilisateurConnecte As New Utilisateur()
        Try
            UtilisateurConnecte = userdao.getUserById(UtilisateurId)
            UtilisateurConnecte.UtilisateurAdmin = Admin
            userLog = UtilisateurConnecte
            '  --- init internationnalisation du richTextBoxEditor
            RichTextBoxLocalizationProvider.CurrentProvider = RichTextBoxLocalizationProvider.FromStream(New MemoryStream(New System.Text.UTF8Encoding().GetBytes(FrenchRichTextBoxStrings.RichTextBoxStrings)))
            '  --- init internationnalisation du radgridview
            RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()
        Finally
            Me.Cursor = Cursors.Default
        End Try



    End Sub

    Private Sub BtnAbandon_Click(sender As Object, e As EventArgs) Handles BtnAbandon.Click
        Close()
    End Sub

    Private Sub BtnTest_Click(sender As Object, e As EventArgs)
        'Me.RadDesktopAlert1.ContentImage = envelopeImage
        Me.RadDesktopAlert1.CaptionText = "New E-mail Notification"
        Me.RadDesktopAlert1.ContentText = "Hello Jack, I am writing to inform you "
        Me.RadDesktopAlert1.Show()
    End Sub

    Private Sub BtnTest2_Click(sender As Object, e As EventArgs)
        Dim form As New RadFNotification()
        form.Message = "Test message"
        form.Show()

    End Sub

    Private Sub BtnTheriaque_Click(sender As Object, e As EventArgs) Handles BtnTheriaque.Click
        'Using form As New FrmTestDynamiqueDocument
        'Form.ShowDialog()
        'End Using
        'Return
        InitAppelForm()
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim print As New PrtSynthese
            Dim selectedPatient As Patient = PatientDao.GetPatientById(1)
            print.SelectedPatient = selectedPatient
            print.printDocument()
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Cursor.Current = Cursors.Default

    End Sub

    Private Sub BtnTemplateSsEpisode_Click(sender As Object, e As EventArgs) Handles BtnTemplateSsEpisode.Click
        Try
            InitAppelForm()
            Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using formT As New FrmAdminTemplateSousEpisode()
                formT.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try
    End Sub

    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Using frmLogin As New FrmLogin
                frmLogin.ShowDialog()
            End Using
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub BtnDebloque_Click(sender As Object, e As EventArgs) Handles BtnDebloque.Click
        ResetPermission()
        afficheTry()
        MsgBox("Poste débloqué")

    End Sub

    Private Sub AfficheTry()
        Dim nb = ReadPermTry()
        LblNbTry.Text = "Nbre essai(s) en cours : " & nb & "/" & MAX_TRY
        LblNbTry.ForeColor = If(nb >= MAX_TRY, Color.Red, Color.Black)
    End Sub
End Class