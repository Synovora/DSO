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
        Me.RadGroupProfil = New Telerik.WinControls.UI.RadGroupBox()
        Me.DropDownSite = New Telerik.WinControls.UI.RadDropDownList()
        Me.DropDownUS = New Telerik.WinControls.UI.RadDropDownList()
        Me.DropDownSiege = New Telerik.WinControls.UI.RadDropDownList()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.TxtPassword2 = New System.Windows.Forms.TextBox()
        Me.TxtPassword1 = New System.Windows.Forms.TextBox()
        Me.LblPassword2 = New System.Windows.Forms.Label()
        Me.lblPassword = New System.Windows.Forms.Label()
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlBoutons.SuspendLayout()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupIdentite, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupIdentite.SuspendLayout()
        CType(Me.DropDownProfil, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupProfil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupProfil.SuspendLayout()
        CType(Me.DropDownSite, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownUS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownSiege, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PnlBoutons
        '
        Me.PnlBoutons.Controls.Add(Me.BtnAbandon)
        Me.PnlBoutons.Controls.Add(Me.BtnValider)
        Me.PnlBoutons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PnlBoutons.Location = New System.Drawing.Point(0, 368)
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
        Me.RadGroupIdentite.Size = New System.Drawing.Size(644, 183)
        Me.RadGroupIdentite.TabIndex = 0
        Me.RadGroupIdentite.Text = "Identité"
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
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
        Me.DropDownProfil.Location = New System.Drawing.Point(118, 155)
        Me.DropDownProfil.Name = "DropDownProfil"
        Me.DropDownProfil.Size = New System.Drawing.Size(387, 20)
        Me.DropDownProfil.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(10, 159)
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
        'RadGroupProfil
        '
        Me.RadGroupProfil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupProfil.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupProfil.Controls.Add(Me.DropDownSite)
        Me.RadGroupProfil.Controls.Add(Me.DropDownUS)
        Me.RadGroupProfil.Controls.Add(Me.DropDownSiege)
        Me.RadGroupProfil.Controls.Add(Me.Label2)
        Me.RadGroupProfil.Controls.Add(Me.Label3)
        Me.RadGroupProfil.Controls.Add(Me.Label4)
        Me.RadGroupProfil.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupProfil.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupProfil.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupProfil.HeaderText = "Localisation"
        Me.RadGroupProfil.Location = New System.Drawing.Point(0, 183)
        Me.RadGroupProfil.Name = "RadGroupProfil"
        Me.RadGroupProfil.Size = New System.Drawing.Size(644, 98)
        Me.RadGroupProfil.TabIndex = 1
        Me.RadGroupProfil.Text = "Localisation"
        CType(Me.RadGroupProfil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupProfil.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupProfil.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
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
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBox1.Controls.Add(Me.TxtPassword2)
        Me.RadGroupBox1.Controls.Add(Me.TxtPassword1)
        Me.RadGroupBox1.Controls.Add(Me.LblPassword2)
        Me.RadGroupBox1.Controls.Add(Me.lblPassword)
        Me.RadGroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupBox1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox1.HeaderText = "Mot de Passe"
        Me.RadGroupBox1.Location = New System.Drawing.Point(0, 281)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(644, 85)
        Me.RadGroupBox1.TabIndex = 3
        Me.RadGroupBox1.Text = "Mot de Passe"
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
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
        'FrmUtilisateur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(644, 425)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.RadGroupProfil)
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
        CType(Me.DropDownProfil, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupProfil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupProfil.ResumeLayout(False)
        Me.RadGroupProfil.PerformLayout()
        CType(Me.DropDownSite, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownUS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownSiege, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
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
    Friend WithEvents RadGroupProfil As Telerik.WinControls.UI.RadGroupBox
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
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents TxtPassword2 As TextBox
    Friend WithEvents TxtPassword1 As TextBox
    Friend WithEvents LblPassword2 As Label
    Friend WithEvents lblPassword As Label
End Class

