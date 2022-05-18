<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFTestMethodes
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
        Me.RadBtnTest = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadBtnTest, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnTest
        '
        Me.RadBtnTest.Location = New System.Drawing.Point(46, 60)
        Me.RadBtnTest.Name = "RadBtnTest"
        Me.RadBtnTest.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnTest.TabIndex = 0
        Me.RadBtnTest.Text = "Send message"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.Location = New System.Drawing.Point(279, 234)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 4
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadFTestMethodes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(401, 270)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadBtnTest)
        Me.Name = "RadFTestMethodes"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFTestMethodes"
        CType(Me.RadBtnTest, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadBtnTest As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
End Class

