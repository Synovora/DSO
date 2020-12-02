<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFDrcAideEnLigne
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
        Me.TxtCommentaireDrc = New System.Windows.Forms.TextBox()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        Me.TxtDescriptionDrc = New System.Windows.Forms.TextBox()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtCommentaireDrc
        '
        Me.TxtCommentaireDrc.Location = New System.Drawing.Point(12, 35)
        Me.TxtCommentaireDrc.Multiline = True
        Me.TxtCommentaireDrc.Name = "TxtCommentaireDrc"
        Me.TxtCommentaireDrc.ReadOnly = True
        Me.TxtCommentaireDrc.Size = New System.Drawing.Size(831, 498)
        Me.TxtCommentaireDrc.TabIndex = 0
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandonner.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(819, 539)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandonner.TabIndex = 168
        '
        'TxtDescriptionDrc
        '
        Me.TxtDescriptionDrc.Location = New System.Drawing.Point(12, 9)
        Me.TxtDescriptionDrc.Name = "TxtDescriptionDrc"
        Me.TxtDescriptionDrc.ReadOnly = True
        Me.TxtDescriptionDrc.Size = New System.Drawing.Size(831, 20)
        Me.TxtDescriptionDrc.TabIndex = 169
        '
        'RadFDrcAideEnLigne
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandonner
        Me.ClientSize = New System.Drawing.Size(853, 569)
        Me.Controls.Add(Me.TxtDescriptionDrc)
        Me.Controls.Add(Me.RadBtnAbandonner)
        Me.Controls.Add(Me.TxtCommentaireDrc)
        Me.Name = "RadFDrcAideEnLigne"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Aide en ligne"
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtCommentaireDrc As TextBox
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
    Friend WithEvents TxtDescriptionDrc As TextBox
End Class

