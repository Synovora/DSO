<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFVaccinOperator
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
        Me.TextOperator = New Telerik.WinControls.UI.RadTextBox()
        Me.BtnDone = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel4 = New Telerik.WinControls.UI.RadLabel()
        Me.BtnUser = New Telerik.WinControls.UI.RadButton()
        CType(Me.TextOperator, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDone, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnUser, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextOperator
        '
        Me.TextOperator.Location = New System.Drawing.Point(13, 82)
        Me.TextOperator.Name = "TextOperator"
        Me.TextOperator.Size = New System.Drawing.Size(222, 20)
        Me.TextOperator.TabIndex = 0
        '
        'BtnDone
        '
        Me.BtnDone.Location = New System.Drawing.Point(125, 108)
        Me.BtnDone.Name = "BtnDone"
        Me.BtnDone.Size = New System.Drawing.Size(110, 24)
        Me.BtnDone.TabIndex = 1
        Me.BtnDone.Text = "Valider"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(112, 43)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(21, 18)
        Me.RadLabel1.TabIndex = 2
        Me.RadLabel1.Text = "Ou"
        '
        'RadLabel4
        '
        Me.RadLabel4.Location = New System.Drawing.Point(13, 58)
        Me.RadLabel4.Name = "RadLabel4"
        Me.RadLabel4.Size = New System.Drawing.Size(61, 18)
        Me.RadLabel4.TabIndex = 3
        Me.RadLabel4.Text = "Texte libre:"
        '
        'BtnUser
        '
        Me.BtnUser.Location = New System.Drawing.Point(13, 13)
        Me.BtnUser.Name = "BtnUser"
        Me.BtnUser.Size = New System.Drawing.Size(226, 24)
        Me.BtnUser.TabIndex = 4
        Me.BtnUser.Text = "Selectionner un operateur"
        '
        'RadFVaccinOperator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(251, 139)
        Me.Controls.Add(Me.RadLabel4)
        Me.Controls.Add(Me.BtnUser)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.BtnDone)
        Me.Controls.Add(Me.TextOperator)
        Me.Name = "RadFVaccinOperator"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFVaccinOperator"
        CType(Me.TextOperator, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDone, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnUser, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextOperator As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents BtnDone As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel4 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents BtnUser As Telerik.WinControls.UI.RadButton
End Class

