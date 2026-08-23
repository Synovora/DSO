<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmChangePassword
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
        Me.RadGroupPassword = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblMessagePassword = New System.Windows.Forms.Label()
        Me.TxtPassword2 = New System.Windows.Forms.TextBox()
        Me.TxtPassword1 = New System.Windows.Forms.TextBox()
        Me.LblPassword2 = New System.Windows.Forms.Label()
        Me.lblPassword = New System.Windows.Forms.Label()
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlBoutons.SuspendLayout()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.PnlBoutons.Location = New System.Drawing.Point(0, 105)
        Me.PnlBoutons.Name = "PnlBoutons"
        Me.PnlBoutons.Size = New System.Drawing.Size(654, 40)
        Me.PnlBoutons.TabIndex = 1
        '
        'BtnAbandon
        '
        Me.BtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnAbandon.ForeColor = System.Drawing.Color.Black
        Me.BtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.BtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.BtnAbandon.Location = New System.Drawing.Point(600, 7)
        Me.BtnAbandon.Name = "BtnAbandon"
        Me.BtnAbandon.Size = New System.Drawing.Size(35, 26)
        Me.BtnAbandon.TabIndex = 1
        '
        'BtnValider
        '
        Me.BtnValider.ForeColor = System.Drawing.Color.Red
        Me.BtnValider.Location = New System.Drawing.Point(24, 7)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(111, 26)
        Me.BtnValider.TabIndex = 0
        Me.BtnValider.Text = "Valider"
        Me.BtnValider.UseVisualStyleBackColor = True
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
        Me.RadGroupPassword.HeaderText = "Nouveau Mot de Passe"
        Me.RadGroupPassword.Location = New System.Drawing.Point(0, 0)
        Me.RadGroupPassword.Name = "RadGroupPassword"
        Me.RadGroupPassword.Size = New System.Drawing.Size(654, 100)
        Me.RadGroupPassword.TabIndex = 0
        Me.RadGroupPassword.Text = "Nouveau Mot de Passe"
        CType(Me.RadGroupPassword.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupPassword.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupPassword.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'LblMessagePassword
        '
        Me.LblMessagePassword.AutoSize = True
        Me.LblMessagePassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMessagePassword.Location = New System.Drawing.Point(123, 82)
        Me.LblMessagePassword.Name = "LblMessagePassword"
        Me.LblMessagePassword.Size = New System.Drawing.Size(167, 13)
        Me.LblMessagePassword.TabIndex = 4
        Me.LblMessagePassword.Text = "variable messageFormatPassword"
        '
        'TxtPassword2
        '
        Me.TxtPassword2.BackColor = System.Drawing.SystemColors.Window
        Me.TxtPassword2.Location = New System.Drawing.Point(121, 53)
        Me.TxtPassword2.Name = "TxtPassword2"
        Me.TxtPassword2.Size = New System.Drawing.Size(514, 25)
        Me.TxtPassword2.TabIndex = 3
        Me.TxtPassword2.UseSystemPasswordChar = True
        '
        'TxtPassword1
        '
        Me.TxtPassword1.BackColor = System.Drawing.SystemColors.Window
        Me.TxtPassword1.Location = New System.Drawing.Point(121, 29)
        Me.TxtPassword1.Name = "TxtPassword1"
        Me.TxtPassword1.Size = New System.Drawing.Size(514, 25)
        Me.TxtPassword1.TabIndex = 1
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
        'FrmChangePassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(654, 145)
        Me.Controls.Add(Me.RadGroupPassword)
        Me.Controls.Add(Me.PnlBoutons)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmChangePassword"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Changement de Mot de Passe"
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlBoutons.ResumeLayout(False)
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupPassword, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupPassword.ResumeLayout(False)
        Me.RadGroupPassword.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PnlBoutons As Telerik.WinControls.UI.RadPanel
    Friend WithEvents BtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnValider As Button
    Friend WithEvents RadGroupPassword As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblMessagePassword As Label
    Friend WithEvents TxtPassword2 As TextBox
    Friend WithEvents TxtPassword1 As TextBox
    Friend WithEvents LblPassword2 As Label
    Friend WithEvents lblPassword As Label
End Class

