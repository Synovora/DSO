<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFNotification
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
        Me.components = New System.ComponentModel.Container()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Texte = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 2000
        '
        'Timer2
        '
        Me.Timer2.Interval = 2000
        '
        'Texte
        '
        Me.Texte.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Texte.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Texte.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Texte.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Texte.Location = New System.Drawing.Point(0, 40)
        Me.Texte.Multiline = True
        Me.Texte.Name = "Texte"
        Me.Texte.Size = New System.Drawing.Size(552, 164)
        Me.Texte.TabIndex = 1
        Me.Texte.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(124, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(308, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Placez le curseur dans la fenêtre pour conserver l'affichage"
        '
        'RadFNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(552, 204)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Texte)
        Me.ForeColor = System.Drawing.Color.Maroon
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFNotification"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFNotification"
        Me.TopMost = True
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Timer2 As Timer
    Friend WithEvents Texte As TextBox
    Friend WithEvents Label1 As Label
End Class

