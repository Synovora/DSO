﻿Imports System.Configuration
Imports System.IO
Imports Oasis_Common
Imports Oasis_WF.My.Resources
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
    Dim NiveauAcces As Integer

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        StandardDao.fixConnectionString("Data Source=ns3119889.ip-51-38-181.eu;Initial Catalog=oasis;persist security info=True;user id=sa;password=Oasis-689;encrypt=true;trustServerCertificate=true;MultipleActiveResultSets=True")
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
        Cursor.Current = Cursors.WaitCursor
        InitAppelForm()
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
        Cursor.Current = Cursors.WaitCursor
        InitAppelForm()
        Using form As New RadFPatientListe 'FrmAgendaMedecin
            form.UtilisateurConnecte = userLog
            form.ShowDialog()
        End Using
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub BtnAdmin_Click(sender As Object, e As EventArgs) Handles BtnAdmin.Click
        InitAppelForm()
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

        Dim UtilisateurConnecte As New Utilisateur()
        UtilisateurConnecte = userdao.getUserById(UtilisateurId)
        UtilisateurConnecte.UtilisateurAdmin = Admin
        userLog = UtilisateurConnecte

        ' -- pour test api rest
        loginRequestLog = New LoginRequest() With {
                .login = "Bertrand.Gambet",
                .password = "a"
        }

        '  --- init internationnalisation du richTextBoxEditor
        RichTextBoxLocalizationProvider.CurrentProvider = RichTextBoxLocalizationProvider.FromStream(New MemoryStream(New System.Text.UTF8Encoding().GetBytes(FrenchRichTextBoxStrings.RichTextBoxStrings)))

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
        Dim form As New RadFATCListe()
        form.ShowDialog()
    End Sub

    Private Sub BtnTemplateSsEpisode_Click(sender As Object, e As EventArgs) Handles BtnTemplateSsEpisode.Click
        InitAppelForm()
        Dim form As New FrmAdminTemplateSousEpisode
        form.ShowDialog()

    End Sub
End Class