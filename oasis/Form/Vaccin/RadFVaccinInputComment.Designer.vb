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
        CType(Me.RadRichTextEditor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAddTraitement, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnDone, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadRichTextEditor1
        '
        Me.RadRichTextEditor1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.RadRichTextEditor1.Location = New System.Drawing.Point(13, 13)
        Me.RadRichTextEditor1.Name = "RadRichTextEditor1"
        Me.RadRichTextEditor1.SelectionFill = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadRichTextEditor1.Size = New System.Drawing.Size(467, 216)
        Me.RadRichTextEditor1.TabIndex = 0
        '
        'RadBtnAddTraitement
        '
        Me.RadBtnAddTraitement.Location = New System.Drawing.Point(13, 235)
        Me.RadBtnAddTraitement.Name = "RadBtnAddTraitement"
        Me.RadBtnAddTraitement.Size = New System.Drawing.Size(153, 24)
        Me.RadBtnAddTraitement.TabIndex = 1
        Me.RadBtnAddTraitement.Text = "Declarer un traitement"
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
        'RadFVaccinInputComment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 270)
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
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadRichTextEditor1 As Telerik.WinControls.UI.RadRichTextEditor
    Friend WithEvents RadBtnAddTraitement As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnDone As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnCancel As Telerik.WinControls.UI.RadButton
End Class

