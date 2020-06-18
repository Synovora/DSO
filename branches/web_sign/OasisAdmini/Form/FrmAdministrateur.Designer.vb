<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAdministrateur
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmAdministrateur))
        Me.BtnDebloque = New System.Windows.Forms.Button()
        Me.LblNbTry = New System.Windows.Forms.Label()
        Me.TxtPassword = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'BtnDebloque
        '
        Me.BtnDebloque.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BtnDebloque.Location = New System.Drawing.Point(49, 89)
        Me.BtnDebloque.Name = "BtnDebloque"
        Me.BtnDebloque.Size = New System.Drawing.Size(192, 61)
        Me.BtnDebloque.TabIndex = 2
        Me.BtnDebloque.Text = "Déblocage"
        Me.BtnDebloque.UseVisualStyleBackColor = False
        '
        'LblNbTry
        '
        Me.LblNbTry.AutoSize = True
        Me.LblNbTry.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNbTry.ForeColor = System.Drawing.Color.White
        Me.LblNbTry.Location = New System.Drawing.Point(48, 21)
        Me.LblNbTry.Name = "LblNbTry"
        Me.LblNbTry.Size = New System.Drawing.Size(57, 20)
        Me.LblNbTry.TabIndex = 0
        Me.LblNbTry.Text = "Label1"
        '
        'TxtPassword
        '
        Me.TxtPassword.Location = New System.Drawing.Point(122, 58)
        Me.TxtPassword.Name = "TxtPassword"
        Me.TxtPassword.Size = New System.Drawing.Size(119, 20)
        Me.TxtPassword.TabIndex = 1
        Me.TxtPassword.UseSystemPasswordChar = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label1.Location = New System.Drawing.Point(50, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Mot de Passe"
        '
        'FrmAdministrateur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DimGray
        Me.ClientSize = New System.Drawing.Size(289, 186)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtPassword)
        Me.Controls.Add(Me.LblNbTry)
        Me.Controls.Add(Me.BtnDebloque)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmAdministrateur"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " Administration - Déblocage Poste"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnDebloque As Button
    Friend WithEvents LblNbTry As Label
    Friend WithEvents TxtPassword As TextBox
    Friend WithEvents Label1 As Label
End Class
