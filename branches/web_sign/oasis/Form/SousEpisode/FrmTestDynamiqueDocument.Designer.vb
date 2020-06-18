<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmTestDynamiqueDocument
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmTestDynamiqueDocument))
        Me.RadButton1 = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadButton1
        '
        Me.RadButton1.Location = New System.Drawing.Point(52, 100)
        Me.RadButton1.Name = "RadButton1"
        Me.RadButton1.Size = New System.Drawing.Size(188, 33)
        Me.RadButton1.TabIndex = 0
        Me.RadButton1.Text = "Création document"
        '
        'FrmTestDynamiqueDocument
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 270)
        Me.Controls.Add(Me.RadButton1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmTestDynamiqueDocument"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "   FrmTestDynamiqueDocument"
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadButton1 As Telerik.WinControls.UI.RadButton
End Class

