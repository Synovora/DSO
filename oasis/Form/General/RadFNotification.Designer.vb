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
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.Texte = New Telerik.WinControls.UI.RadPanel()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Texte, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'RadLabel1
        '
        Me.RadLabel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadLabel1.ForeColor = System.Drawing.Color.Red
        Me.RadLabel1.Location = New System.Drawing.Point(0, 172)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(298, 18)
        Me.RadLabel1.TabIndex = 3
        Me.RadLabel1.Text = "Placez le curseur dans la fenêtre pour conserver l'affichage"
        '
        'Texte
        '
        Me.Texte.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Texte.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Texte.Location = New System.Drawing.Point(0, 0)
        Me.Texte.Name = "Texte"
        Me.Texte.Size = New System.Drawing.Size(602, 172)
        Me.Texte.TabIndex = 4
        Me.Texte.Text = "Texte du message"
        Me.Texte.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        '
        'RadFNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(602, 190)
        Me.Controls.Add(Me.Texte)
        Me.Controls.Add(Me.RadLabel1)
        Me.ForeColor = System.Drawing.Color.Maroon
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFNotification"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFNotification"
        Me.TopMost = True
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Texte, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Timer2 As Timer
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Texte As Telerik.WinControls.UI.RadPanel
End Class

