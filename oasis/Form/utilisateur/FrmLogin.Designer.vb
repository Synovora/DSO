<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLogin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmLogin))
        Me.BtnValidate = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.TxtPassword = New System.Windows.Forms.TextBox()
        Me.TxtLogin = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnValidate
        '
        Me.BtnValidate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BtnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnValidate.Location = New System.Drawing.Point(240, 133)
        Me.BtnValidate.Name = "BtnValidate"
        Me.BtnValidate.Size = New System.Drawing.Size(75, 26)
        Me.BtnValidate.TabIndex = 11
        Me.BtnValidate.Text = "Valider"
        Me.BtnValidate.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancel.Location = New System.Drawing.Point(392, 133)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(78, 26)
        Me.BtnCancel.TabIndex = 10
        Me.BtnCancel.Text = "&Annuler"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'TxtPassword
        '
        Me.TxtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtPassword.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.TxtPassword.Location = New System.Drawing.Point(240, 85)
        Me.TxtPassword.MaxLength = 45
        Me.TxtPassword.Name = "TxtPassword"
        Me.TxtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TxtPassword.Size = New System.Drawing.Size(230, 25)
        Me.TxtPassword.TabIndex = 9
        Me.TxtPassword.Text = "a"
        Me.TxtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TxtPassword.UseSystemPasswordChar = True
        '
        'TxtLogin
        '
        Me.TxtLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtLogin.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.TxtLogin.Location = New System.Drawing.Point(240, 37)
        Me.TxtLogin.MaxLength = 45
        Me.TxtLogin.Name = "TxtLogin"
        Me.TxtLogin.Size = New System.Drawing.Size(230, 25)
        Me.TxtLogin.TabIndex = 8
        Me.TxtLogin.Text = "Bertrand.Gambet"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(148, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 32)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Mot de passe"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(148, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 32)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Identifiant"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.BackgroundImage = Global.Oasis_WF.My.Resources.Resources.Keys_icon
        Me.Panel1.Location = New System.Drawing.Point(13, 24)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(124, 135)
        Me.Panel1.TabIndex = 12
        '
        'FrmLogin
        '
        Me.AcceptButton = Me.BtnValidate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(509, 198)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.BtnValidate)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.TxtPassword)
        Me.Controls.Add(Me.TxtLogin)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmLogin"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OASIS - Authentification"
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnValidate As Button
    Friend WithEvents BtnCancel As Button
    Friend WithEvents TxtPassword As TextBox
    Friend WithEvents TxtLogin As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel1 As Panel
End Class
