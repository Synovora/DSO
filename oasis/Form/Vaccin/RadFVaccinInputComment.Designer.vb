<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFVaccinInputComment
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
        Me.RadRichTextEditor1 = New Telerik.WinControls.UI.RadRichTextEditor()
        Me.RadBtnAddTraitement = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnDone = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnCancel = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.DTPExp = New System.Windows.Forms.DateTimePicker()
        Me.RadTextBox1 = New Telerik.WinControls.UI.RadTextBox()
        Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
        CType(Me.RadRichTextEditor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAddTraitement, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnDone, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadTextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadRichTextEditor1
        '
        Me.RadRichTextEditor1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.RadRichTextEditor1.Location = New System.Drawing.Point(13, 56)
        Me.RadRichTextEditor1.Name = "RadRichTextEditor1"
        Me.RadRichTextEditor1.SelectionFill = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadRichTextEditor1.Size = New System.Drawing.Size(467, 173)
        Me.RadRichTextEditor1.TabIndex = 0
        '
        'RadBtnAddTraitement
        '
        Me.RadBtnAddTraitement.Location = New System.Drawing.Point(13, 235)
        Me.RadBtnAddTraitement.Name = "RadBtnAddTraitement"
        Me.RadBtnAddTraitement.Size = New System.Drawing.Size(153, 24)
        Me.RadBtnAddTraitement.TabIndex = 1
        Me.RadBtnAddTraitement.Text = "Declarer une allergie"
        '
        'RadBtnDone
        '
        Me.RadBtnDone.Location = New System.Drawing.Point(369, 234)
        Me.RadBtnDone.Name = "RadBtnDone"
        Me.RadBtnDone.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnDone.TabIndex = 2
        Me.RadBtnDone.Text = "Valider"
        '
        'RadBtnCancel
        '
        Me.RadBtnCancel.Location = New System.Drawing.Point(253, 234)
        Me.RadBtnCancel.Name = "RadBtnCancel"
        Me.RadBtnCancel.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnCancel.TabIndex = 3
        Me.RadBtnCancel.Text = "Annuler"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(13, 13)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(24, 18)
        Me.RadLabel1.TabIndex = 4
        Me.RadLabel1.Text = "Lot:"
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(234, 11)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(26, 18)
        Me.RadLabel2.TabIndex = 5
        Me.RadLabel2.Text = "Exp:"
        '
        'DTPExp
        '
        Me.DTPExp.Location = New System.Drawing.Point(266, 10)
        Me.DTPExp.Name = "DTPExp"
        Me.DTPExp.Size = New System.Drawing.Size(212, 20)
        Me.DTPExp.TabIndex = 6
        '
        'RadTextBox1
        '
        Me.RadTextBox1.Location = New System.Drawing.Point(44, 10)
        Me.RadTextBox1.Name = "RadTextBox1"
        Me.RadTextBox1.Size = New System.Drawing.Size(149, 20)
        Me.RadTextBox1.TabIndex = 7
        '
        'RadLabel3
        '
        Me.RadLabel3.Location = New System.Drawing.Point(13, 37)
        Me.RadLabel3.Name = "RadLabel3"
        Me.RadLabel3.Size = New System.Drawing.Size(76, 18)
        Me.RadLabel3.TabIndex = 6
        Me.RadLabel3.Text = "Commentaire:"
        '
        'RadFVaccinInputComment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 270)
        Me.Controls.Add(Me.RadLabel3)
        Me.Controls.Add(Me.RadTextBox1)
        Me.Controls.Add(Me.DTPExp)
        Me.Controls.Add(Me.RadLabel2)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.RadBtnCancel)
        Me.Controls.Add(Me.RadBtnDone)
        Me.Controls.Add(Me.RadBtnAddTraitement)
        Me.Controls.Add(Me.RadRichTextEditor1)
        Me.Name = "RadFVaccinInputComment"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "Vaccin - Commentaire"
        CType(Me.RadRichTextEditor1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAddTraitement, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnDone, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadTextBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadRichTextEditor1 As Telerik.WinControls.UI.RadRichTextEditor
    Friend WithEvents RadBtnAddTraitement As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnDone As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnCancel As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents DTPExp As DateTimePicker
    Friend WithEvents RadTextBox1 As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
End Class

