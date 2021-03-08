<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAdminTemplateSousEpisode
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
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.RadButton2 = New Telerik.WinControls.UI.RadButton()
        Me.RadButton1 = New Telerik.WinControls.UI.RadButton()
        Me.DropDownSousType = New Telerik.WinControls.UI.RadDropDownList()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.DropDownType = New Telerik.WinControls.UI.RadDropDownList()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadButton2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownSousType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.RadButton2)
        Me.RadPanel1.Controls.Add(Me.RadButton1)
        Me.RadPanel1.Controls.Add(Me.DropDownSousType)
        Me.RadPanel1.Controls.Add(Me.Label6)
        Me.RadPanel1.Controls.Add(Me.DropDownType)
        Me.RadPanel1.Controls.Add(Me.Label5)
        Me.RadPanel1.Location = New System.Drawing.Point(48, 39)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(536, 174)
        Me.RadPanel1.TabIndex = 0
        '
        'RadButton2
        '
        Me.RadButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadButton2.Location = New System.Drawing.Point(350, 115)
        Me.RadButton2.Name = "RadButton2"
        Me.RadButton2.Size = New System.Drawing.Size(119, 35)
        Me.RadButton2.TabIndex = 52
        Me.RadButton2.Text = "Abandonner"
        '
        'RadButton1
        '
        Me.RadButton1.Location = New System.Drawing.Point(214, 114)
        Me.RadButton1.Name = "RadButton1"
        Me.RadButton1.Size = New System.Drawing.Size(83, 37)
        Me.RadButton1.TabIndex = 51
        Me.RadButton1.Text = "Editer"
        '
        'DropDownSousType
        '
        Me.DropDownSousType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.DropDownSousType.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.DropDownSousType.Location = New System.Drawing.Point(143, 78)
        Me.DropDownSousType.Name = "DropDownSousType"
        Me.DropDownSousType.Size = New System.Drawing.Size(328, 20)
        Me.DropDownSousType.TabIndex = 50
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(66, 82)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 49
        Me.Label6.Text = "Sous-Type"
        '
        'DropDownType
        '
        Me.DropDownType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.DropDownType.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
        Me.DropDownType.Location = New System.Drawing.Point(142, 49)
        Me.DropDownType.Name = "DropDownType"
        Me.DropDownType.Size = New System.Drawing.Size(328, 20)
        Me.DropDownType.TabIndex = 48
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(95, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 47
        Me.Label5.Text = "Type"
        '
        'FrmAdminTemplateSousEpisode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadButton2
        Me.ClientSize = New System.Drawing.Size(638, 262)
        Me.Controls.Add(Me.RadPanel1)
        Me.Name = "FrmAdminTemplateSousEpisode"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Editeur template Sous-Episode"
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.RadButton2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownSousType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadButton1 As Telerik.WinControls.UI.RadButton
    Friend WithEvents DropDownSousType As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents Label6 As Label
    Friend WithEvents DropDownType As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents Label5 As Label
    Friend WithEvents RadButton2 As Telerik.WinControls.UI.RadButton
End Class

