<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RadFValenceCreation
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
        Me.components = New System.ComponentModel.Container()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.precaution = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.valider = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.code = New System.Windows.Forms.TextBox()
        Me.description = New System.Windows.Forms.TextBox()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.valider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Code"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(66, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Description"
        '
        'precaution
        '
        Me.precaution.Location = New System.Drawing.Point(102, 83)
        Me.precaution.MaxLength = 2048
        Me.precaution.Multiline = True
        Me.precaution.Name = "precaution"
        Me.precaution.Size = New System.Drawing.Size(732, 90)
        Me.precaution.TabIndex = 7
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 86)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Precaution"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(810, 179)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 9
        Me.ToolTip.SetToolTip(Me.RadBtnAbandon, "Abandon")
        '
        'valider
        '
        Me.valider.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.valider.Location = New System.Drawing.Point(15, 179)
        Me.valider.Name = "valider"
        Me.valider.Size = New System.Drawing.Size(110, 24)
        Me.valider.TabIndex = 10
        Me.valider.Text = "Valider"
        Me.ToolTip.SetToolTip(Me.valider, "Valider")
        '
        'code
        '
        Me.code.Location = New System.Drawing.Point(102, 6)
        Me.code.MaxLength = 80
        Me.code.Name = "code"
        Me.code.Size = New System.Drawing.Size(169, 20)
        Me.code.TabIndex = 13
        '
        'description
        '
        Me.description.Location = New System.Drawing.Point(102, 32)
        Me.description.MaxLength = 256
        Me.description.Multiline = True
        Me.description.Name = "description"
        Me.description.Size = New System.Drawing.Size(732, 45)
        Me.description.TabIndex = 14
        '
        'RadFValenceCreation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(840, 210)
        Me.Controls.Add(Me.description)
        Me.Controls.Add(Me.code)
        Me.Controls.Add(Me.valider)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.precaution)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFValenceCreation"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Creation de valence"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.valider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents precaution As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents valider As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents code As TextBox
    Friend WithEvents description As TextBox
End Class

