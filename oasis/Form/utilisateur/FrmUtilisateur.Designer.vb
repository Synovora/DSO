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
        Me.TxtNom = New System.Windows.Forms.TextBox()
        Me.LblNom = New System.Windows.Forms.Label()
        Me.TxtIdentifiant = New System.Windows.Forms.TextBox()
        Me.LblIdentifiant = New System.Windows.Forms.Label()
        Me.TxtPrenom = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadGroupProfil = New Telerik.WinControls.UI.RadGroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.DropDownProfil = New Telerik.WinControls.UI.RadDropDownList()
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlBoutons.SuspendLayout()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupIdentite, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupIdentite.SuspendLayout()
        CType(Me.RadGroupProfil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupProfil.SuspendLayout()
        CType(Me.DropDownProfil, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PnlBoutons
        '
        Me.PnlBoutons.Controls.Add(Me.BtnAbandon)
        Me.PnlBoutons.Controls.Add(Me.BtnValider)
        Me.PnlBoutons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PnlBoutons.Location = New System.Drawing.Point(0, 443)
        Me.PnlBoutons.Name = "PnlBoutons"
        Me.PnlBoutons.Size = New System.Drawing.Size(644, 57)
        Me.PnlBoutons.TabIndex = 4
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
        Me.BtnAbandon.TabIndex = 35
        '
        'BtnValider
        '
        Me.BtnValider.ForeColor = System.Drawing.Color.Red
        Me.BtnValider.Location = New System.Drawing.Point(83, 8)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(216, 38)
        Me.BtnValider.TabIndex = 3
        Me.BtnValider.Text = "Valider"
        Me.BtnValider.UseVisualStyleBackColor = True
        '
        'RadGroupIdentite
        '
        Me.RadGroupIdentite.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupIdentite.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupIdentite.Controls.Add(Me.DropDownProfil)
        Me.RadGroupIdentite.Controls.Add(Me.Label5)
        Me.RadGroupIdentite.Controls.Add(Me.TxtPrenom)
        Me.RadGroupIdentite.Controls.Add(Me.Label1)
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
        Me.RadGroupIdentite.Size = New System.Drawing.Size(644, 134)
        Me.RadGroupIdentite.TabIndex = 5
        Me.RadGroupIdentite.Text = "Identité"
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'TxtNom
        '
        Me.TxtNom.BackColor = System.Drawing.SystemColors.Window
        Me.TxtNom.Location = New System.Drawing.Point(118, 49)
        Me.TxtNom.Name = "TxtNom"
        Me.TxtNom.Size = New System.Drawing.Size(514, 25)
        Me.TxtNom.TabIndex = 6
        '
        'LblNom
        '
        Me.LblNom.AutoSize = True
        Me.LblNom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNom.Location = New System.Drawing.Point(8, 53)
        Me.LblNom.Name = "LblNom"
        Me.LblNom.Size = New System.Drawing.Size(32, 13)
        Me.LblNom.TabIndex = 5
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
        Me.LblIdentifiant.Location = New System.Drawing.Point(8, 29)
        Me.LblIdentifiant.Name = "LblIdentifiant"
        Me.LblIdentifiant.Size = New System.Drawing.Size(64, 13)
        Me.LblIdentifiant.TabIndex = 0
        Me.LblIdentifiant.Text = "Identifiant"
        '
        'TxtPrenom
        '
        Me.TxtPrenom.BackColor = System.Drawing.SystemColors.Window
        Me.TxtPrenom.Location = New System.Drawing.Point(118, 73)
        Me.TxtPrenom.Name = "TxtPrenom"
        Me.TxtPrenom.Size = New System.Drawing.Size(514, 25)
        Me.TxtPrenom.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 78)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Prénom"
        '
        'RadGroupProfil
        '
        Me.RadGroupProfil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupProfil.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupProfil.Controls.Add(Me.TextBox1)
        Me.RadGroupProfil.Controls.Add(Me.Label2)
        Me.RadGroupProfil.Controls.Add(Me.TextBox2)
        Me.RadGroupProfil.Controls.Add(Me.Label3)
        Me.RadGroupProfil.Controls.Add(Me.TextBox3)
        Me.RadGroupProfil.Controls.Add(Me.Label4)
        Me.RadGroupProfil.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupProfil.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupProfil.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupProfil.HeaderText = "Profil"
        Me.RadGroupProfil.Location = New System.Drawing.Point(0, 134)
        Me.RadGroupProfil.Name = "RadGroupProfil"
        Me.RadGroupProfil.Size = New System.Drawing.Size(644, 108)
        Me.RadGroupProfil.TabIndex = 6
        Me.RadGroupProfil.Text = "Profil"
        CType(Me.RadGroupProfil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupProfil.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupProfil.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox1.Location = New System.Drawing.Point(118, 73)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(514, 25)
        Me.TextBox1.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(10, 78)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Prénom"
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox2.Location = New System.Drawing.Point(118, 49)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(514, 25)
        Me.TextBox2.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Nom"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(118, 25)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(292, 25)
        Me.TextBox3.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Identifiant"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 105)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Profil"
        '
        'DropDownProfil
        '
        Me.DropDownProfil.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.DropDownProfil.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.DropDownProfil.Location = New System.Drawing.Point(118, 103)
        Me.DropDownProfil.Name = "DropDownProfil"
        Me.DropDownProfil.Size = New System.Drawing.Size(387, 20)
        Me.DropDownProfil.TabIndex = 45
        '
        'FrmUtilisateur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(644, 500)
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
        CType(Me.RadGroupProfil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupProfil.ResumeLayout(False)
        Me.RadGroupProfil.PerformLayout()
        CType(Me.DropDownProfil, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents Label1 As Label
    Friend WithEvents RadGroupProfil As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents DropDownProfil As Telerik.WinControls.UI.RadDropDownList
End Class

