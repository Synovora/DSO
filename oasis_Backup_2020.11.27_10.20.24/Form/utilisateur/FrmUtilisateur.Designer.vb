<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmUtilisateur
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PnlBoutons = New Telerik.WinControls.UI.RadPanel()
        Me.BtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.BtnValider = New System.Windows.Forms.Button()
        Me.RadGroupIdentite = New Telerik.WinControls.UI.RadGroupBox()
        Me.TxtRPPS = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ChkAdmin = New Telerik.WinControls.UI.RadCheckBox()
        Me.TxtMail = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtTelephone = New System.Windows.Forms.TextBox()
        Me.LblTelephone = New System.Windows.Forms.Label()
        Me.DropDownProfil = New Telerik.WinControls.UI.RadDropDownList()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtPrenom = New System.Windows.Forms.TextBox()
        Me.LblPrenom = New System.Windows.Forms.Label()
        Me.TxtNom = New System.Windows.Forms.TextBox()
        Me.LblNom = New System.Windows.Forms.Label()
        Me.TxtIdentifiant = New System.Windows.Forms.TextBox()
        Me.LblIdentifiant = New System.Windows.Forms.Label()
        Me.RadGroupLocalisation = New Telerik.WinControls.UI.RadGroupBox()
        Me.DropDownSite = New Telerik.WinControls.UI.RadDropDownList()
        Me.DropDownUS = New Telerik.WinControls.UI.RadDropDownList()
        Me.DropDownSiege = New Telerik.WinControls.UI.RadDropDownList()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RadGroupPassword = New Telerik.WinControls.UI.RadGroupBox()
        Me.TxtPassword2 = New System.Windows.Forms.TextBox()
        Me.TxtPassword1 = New System.Windows.Forms.TextBox()
        Me.LblPassword2 = New System.Windows.Forms.Label()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.LblMessagePassword = New System.Windows.Forms.Label()
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlBoutons.SuspendLayout()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupIdentite, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupIdentite.SuspendLayout()
        CType(Me.ChkAdmin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownProfil, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupLocalisation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupLocalisation.SuspendLayout()
        CType(Me.DropDownSite, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownUS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownSiege, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupPassword.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PnlBoutons
        '
        Me.PnlBoutons.Controls.Add(Me.BtnAbandon)
        Me.PnlBoutons.Controls.Add(Me.BtnValider)
        Me.PnlBoutons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PnlBoutons.Location = New System.Drawing.Point(0, 404)
        Me.PnlBoutons.Name = "PnlBoutons"
        Me.PnlBoutons.Size = New System.Drawing.Size(644, 57)
        Me.PnlBoutons.TabIndex = 2
        '
        'BtnAbandon
        '
        Me.BtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnAbandon.ForeColor = System.Drawing.Color.Black
        Me.BtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.BtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.BtnAbandon.Location = New System.Drawing.Point(519, 8)
        Me.BtnAbandon.Name = "BtnAbandon"
        Me.BtnAbandon.Size = New System.Drawing.Size(38, 38)
        Me.BtnAbandon.TabIndex = 1
        '
        'BtnValider
        '
        Me.BtnValider.ForeColor = System.Drawing.Color.Red
        Me.BtnValider.Location = New System.Drawing.Point(83, 8)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(216, 38)
        Me.BtnValider.TabIndex = 0
        Me.BtnValider.Text = "Valider"
        Me.BtnValider.UseVisualStyleBackColor = True
        '
        'RadGroupIdentite
        '
        Me.RadGroupIdentite.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupIdentite.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupIdentite.Controls.Add(Me.TxtRPPS)
        Me.RadGroupIdentite.Controls.Add(Me.Label1)
        Me.RadGroupIdentite.Controls.Add(Me.ChkAdmin)
        Me.RadGroupIdentite.Controls.Add(Me.TxtMail)
        Me.RadGroupIdentite.Controls.Add(Me.Label7)
        Me.RadGroupIdentite.Controls.Add(Me.TxtTelephone)
        Me.RadGroupIdentite.Controls.Add(Me.LblTelephone)
        Me.RadGroupIdentite.Controls.Add(Me.DropDownProfil)
        Me.RadGroupIdentite.Controls.Add(Me.Label5)
        Me.RadGroupIdentite.Controls.Add(Me.TxtPrenom)
        Me.RadGroupIdentite.Controls.Add(Me.LblPrenom)
        Me.RadGroupIdentite.Controls.Add(Me.TxtNom)
        Me.RadGroupIdentite.Controls.Add(Me.LblNom)
        Me.RadGroupIdentite.Controls.Add(Me.TxtIdentifiant)
        Me.RadGroupIdentite.Controls.Add(Me.LblIdentifiant)
        Me.RadGroupIdentite.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupIdentite.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupIdentite.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupIdentite.HeaderText = "Identité"
        Me.RadGroupIdentite.Location = New System.Drawing.Point(0, 0)
        Me.RadGroupIdentite.Name = "RadGroupIdentite"
        Me.RadGroupIdentite.Size = New System.Drawing.Size(644, 200)
        Me.RadGroupIdentite.TabIndex = 0
        Me.RadGroupIdentite.Text = "Identité"
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'TxtRPPS
        '
        Me.TxtRPPS.BackColor = System.Drawing.SystemColors.Window
        Me.TxtRPPS.Location = New System.Drawing.Point(118, 168)
        Me.TxtRPPS.MaxLength = 11
        Me.TxtRPPS.Name = "TxtRPPS"
        Me.TxtRPPS.Size = New System.Drawing.Size(93, 25)
        Me.TxtRPPS.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 172)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "RPPS"
        '
        'ChkAdmin
        '
        Me.ChkAdmin.Location = New System.Drawing.Point(536, 147)
        Me.ChkAdmin.Name = "ChkAdmin"
        Me.ChkAdmin.Size = New System.Drawing.Size(94, 18)
        Me.ChkAdmin.TabIndex = 12
        Me.ChkAdmin.Text = "Administrateur"
        '
        'TxtMail
        '
        Me.TxtMail.BackColor = System.Drawing.SystemColors.Window
        Me.TxtMail.Location = New System.Drawing.Point(118, 121)
        Me.TxtMail.Name = "TxtMail"
        Me.TxtMail.Size = New System.Drawing.Size(514, 25)
        Me.TxtMail.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(10, 125)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(30, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Mail"
        '
        'TxtTelephone
        '
        Me.TxtTelephone.Location = New System.Drawing.Point(118, 97)
        Me.TxtTelephone.Name = "TxtTelephone"
        Me.TxtTelephone.Size = New System.Drawing.Size(292, 25)
        Me.TxtTelephone.TabIndex = 7
        '
        'LblTelephone
        '
        Me.LblTelephone.AutoSize = True
        Me.LblTelephone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTelephone.Location = New System.Drawing.Point(10, 101)
        Me.LblTelephone.Name = "LblTelephone"
        Me.LblTelephone.Size = New System.Drawing.Size(67, 13)
        Me.LblTelephone.TabIndex = 6
        Me.LblTelephone.Text = "Téléphone"
        '
        'DropDownProfil
        '
        Me.DropDownProfil.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.DropDownProfil.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.DropDownProfil.Location = New System.Drawing.Point(118, 147)
        Me.DropDownProfil.Name = "DropDownProfil"
        Me.DropDownProfil.Size = New System.Drawing.Size(387, 20)
        Me.DropDownProfil.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(10, 151)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Profil"
        '
        'TxtPrenom
        '
        Me.TxtPrenom.BackColor = System.Drawing.SystemColors.Window
        Me.TxtPrenom.Location = New System.Drawing.Point(118, 73)
        Me.TxtPrenom.Name = "TxtPrenom"
        Me.TxtPrenom.Size = New System.Drawing.Size(514, 25)
        Me.TxtPrenom.TabIndex = 5
        '
        'LblPrenom
        '
        Me.LblPrenom.AutoSize = True
        Me.LblPrenom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPrenom.Location = New System.Drawing.Point(10, 78)
        Me.LblPrenom.Name = "LblPrenom"
        Me.LblPrenom.Size = New System.Drawing.Size(49, 13)
        Me.LblPrenom.TabIndex = 4
        Me.LblPrenom.Text = "Prénom"
        '
        'TxtNom
        '
        Me.TxtNom.BackColor = System.Drawing.SystemColors.Window
        Me.TxtNom.Location = New System.Drawing.Point(118, 49)
        Me.TxtNom.Name = "TxtNom"
        Me.TxtNom.Size = New System.Drawing.Size(514, 25)
        Me.TxtNom.TabIndex = 3
        '
        'LblNom
        '
        Me.LblNom.AutoSize = True
        Me.LblNom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNom.Location = New System.Drawing.Point(10, 53)
        Me.LblNom.Name = "LblNom"
        Me.LblNom.Size = New System.Drawing.Size(32, 13)
        Me.LblNom.TabIndex = 2
        Me.LblNom.Text = "Nom"
        '
        'TxtIdentifiant
        '
        Me.TxtIdentifiant.Location = New System.Drawing.Point(118, 25)
        Me.TxtIdentifiant.Name = "TxtIdentifiant"
        Me.TxtIdentifiant.Size = New System.Drawing.Size(292, 25)
        Me.TxtIdentifiant.TabIndex = 1
        '
        'LblIdentifiant
        '
        Me.LblIdentifiant.AutoSize = True
        Me.LblIdentifiant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblIdentifiant.Location = New System.Drawing.Point(10, 29)
        Me.LblIdentifiant.Name = "LblIdentifiant"
        Me.LblIdentifiant.Size = New System.Drawing.Size(64, 13)
        Me.LblIdentifiant.TabIndex = 0
        Me.LblIdentifiant.Text = "Identifiant"
        '
        'RadGroupLocalisation
        '
        Me.RadGroupLocalisation.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupLocalisation.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupLocalisation.Controls.Add(Me.DropDownSite)
        Me.RadGroupLocalisation.Controls.Add(Me.DropDownUS)
        Me.RadGroupLocalisation.Controls.Add(Me.DropDownSiege)
        Me.RadGroupLocalisation.Controls.Add(Me.Label2)
        Me.RadGroupLocalisation.Controls.Add(Me.Label3)
        Me.RadGroupLocalisation.Controls.Add(Me.Label4)
        Me.RadGroupLocalisation.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupLocalisation.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupLocalisation.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupLocalisation.HeaderText = "Localisation"
        Me.RadGroupLocalisation.Location = New System.Drawing.Point(0, 200)
        Me.RadGroupLocalisation.Name = "RadGroupLocalisation"
        Me.RadGroupLocalisation.Size = New System.Drawing.Size(644, 98)
        Me.RadGroupLocalisation.TabIndex = 1
        Me.RadGroupLocalisation.Text = "Localisation"
        CType(Me.RadGroupLocalisation.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupLocalisation.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupLocalisation.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'DropDownSite
        '
        Me.DropDownSite.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.DropDownSite.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.DropDownSite.Location = New System.Drawing.Point(119, 67)
        Me.DropDownSite.Name = "DropDownSite"
        Me.DropDownSite.Size = New System.Drawing.Size(387, 20)
        Me.DropDownSite.TabIndex = 5
        '
        'DropDownUS
        '
        Me.DropDownUS.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.DropDownUS.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.DropDownUS.Location = New System.Drawing.Point(119, 48)
        Me.DropDownUS.Name = "DropDownUS"
        Me.DropDownUS.Size = New System.Drawing.Size(387, 20)
        Me.DropDownUS.TabIndex = 3
        '
        'DropDownSiege
        '
        Me.DropDownSiege.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.DropDownSiege.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.DropDownSiege.Location = New System.Drawing.Point(119, 29)
        Me.DropDownSiege.Name = "DropDownSiege"
        Me.DropDownSiege.Size = New System.Drawing.Size(387, 20)
        Me.DropDownSiege.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(10, 73)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Site"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(10, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Unité Sanitaire"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 33)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Siège"
        '
        'RadGroupPassword
        '
        Me.RadGroupPassword.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupPassword.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupPassword.Controls.Add(Me.LblMessagePassword)
        Me.RadGroupPassword.Controls.Add(Me.TxtPassword2)
        Me.RadGroupPassword.Controls.Add(Me.TxtPassword1)
        Me.RadGroupPassword.Controls.Add(Me.LblPassword2)
        Me.RadGroupPassword.Controls.Add(Me.lblPassword)
        Me.RadGroupPassword.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupPassword.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupPassword.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupPassword.HeaderText = "Mot de Passe"
        Me.RadGroupPassword.Location = New System.Drawing.Point(0, 298)
        Me.RadGroupPassword.Name = "RadGroupPassword"
        Me.RadGroupPassword.Size = New System.Drawing.Size(644, 100)
        Me.RadGroupPassword.TabIndex = 3
        Me.RadGroupPassword.Text = "Mot de Passe"
        CType(Me.RadGroupPassword.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupPassword.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupPassword.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'TxtPassword2
        '
        Me.TxtPassword2.BackColor = System.Drawing.SystemColors.Window
        Me.TxtPassword2.Location = New System.Drawing.Point(121, 53)
        Me.TxtPassword2.Name = "TxtPassword2"
        Me.TxtPassword2.Size = New System.Drawing.Size(514, 25)
        Me.TxtPassword2.TabIndex = 5
        Me.TxtPassword2.UseSystemPasswordChar = True
        '
        'TxtPassword1
        '
        Me.TxtPassword1.BackColor = System.Drawing.SystemColors.Window
        Me.TxtPassword1.Location = New System.Drawing.Point(121, 29)
        Me.TxtPassword1.Name = "TxtPassword1"
        Me.TxtPassword1.Size = New System.Drawing.Size(514, 25)
        Me.TxtPassword1.TabIndex = 4
        Me.TxtPassword1.UseSystemPasswordChar = True
        '
        'LblPassword2
        '
        Me.LblPassword2.AutoSize = True
        Me.LblPassword2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPassword2.Location = New System.Drawing.Point(10, 58)
        Me.LblPassword2.Name = "LblPassword2"
        Me.LblPassword2.Size = New System.Drawing.Size(92, 13)
        Me.LblPassword2.TabIndex = 2
        Me.LblPassword2.Text = "Resaisie (vérif)"
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.Location = New System.Drawing.Point(10, 36)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(41, 13)
        Me.lblPassword.TabIndex = 0
        Me.lblPassword.Text = "Saisie"
        '
        'LblMessagePassword
        '
        Me.LblMessagePassword.AutoSize = True
        Me.LblMessagePassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMessagePassword.Location = New System.Drawing.Point(123, 82)
        Me.LblMessagePassword.Name = "LblMessagePassword"
        Me.LblMessagePassword.Size = New System.Drawing.Size(167, 13)
        Me.LblMessagePassword.TabIndex = 6
        Me.LblMessagePassword.Text = "variable messageFormatPassword"
        '
        'FrmUtilisateur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(644, 461)
        Me.Controls.Add(Me.RadGroupPassword)
        Me.Controls.Add(Me.RadGroupLocalisation)
        Me.Controls.Add(Me.RadGroupIdentite)
        Me.Controls.Add(Me.PnlBoutons)
        Me.MinimizeBox = False
        Me.Name = "FrmUtilisateur"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fiche Utilisateur"
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlBoutons.ResumeLayout(False)
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupIdentite, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupIdentite.ResumeLayout(False)
        Me.RadGroupIdentite.PerformLayout()
        CType(Me.ChkAdmin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownProfil, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupLocalisation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupLocalisation.ResumeLayout(False)
        Me.RadGroupLocalisation.PerformLayout()
        CType(Me.DropDownSite, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownUS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownSiege, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupPassword, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupPassword.ResumeLayout(False)
        Me.RadGroupPassword.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PnlBoutons As Telerik.WinControls.UI.RadPanel
    Friend WithEvents BtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnValider As Button
    Friend WithEvents RadGroupIdentite As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents TxtNom As TextBox
    Friend WithEvents LblNom As Label
    Friend WithEvents TxtIdentifiant As TextBox
    Friend WithEvents LblIdentifiant As Label
    Friend WithEvents TxtPrenom As TextBox
    Friend WithEvents LblPrenom As Label
    Friend WithEvents RadGroupLocalisation As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents DropDownProfil As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents DropDownSite As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents DropDownUS As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents DropDownSiege As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents TxtMail As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents TxtTelephone As TextBox
    Friend WithEvents LblTelephone As Label
    Friend WithEvents RadGroupPassword As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents TxtPassword2 As TextBox
    Friend WithEvents TxtPassword1 As TextBox
    Friend WithEvents LblPassword2 As Label
    Friend WithEvents lblPassword As Label
    Friend WithEvents ChkAdmin As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents TxtRPPS As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents LblMessagePassword As Label
End Class

