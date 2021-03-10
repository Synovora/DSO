<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFDrcSynonymeDetailEdit
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
        Me.LblDrcDescription = New System.Windows.Forms.Label()
        Me.LblDrcId = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtSynonyme = New System.Windows.Forms.TextBox()
        Me.RadBtnAnnuler = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadBtnAnnuler, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LblDrcDescription
        '
        Me.LblDrcDescription.AutoSize = True
        Me.LblDrcDescription.Location = New System.Drawing.Point(147, 9)
        Me.LblDrcDescription.Name = "LblDrcDescription"
        Me.LblDrcDescription.Size = New System.Drawing.Size(66, 13)
        Me.LblDrcDescription.TabIndex = 57
        Me.LblDrcDescription.Text = "Description"
        '
        'LblDrcId
        '
        Me.LblDrcId.AutoSize = True
        Me.LblDrcId.Location = New System.Drawing.Point(80, 9)
        Me.LblDrcId.Name = "LblDrcId"
        Me.LblDrcId.Size = New System.Drawing.Size(34, 13)
        Me.LblDrcId.TabIndex = 56
        Me.LblDrcId.Text = "Code"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 55
        Me.Label2.Text = "DRC :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 54
        Me.Label1.Text = "Synonyme"
        '
        'TxtSynonyme
        '
        Me.TxtSynonyme.Location = New System.Drawing.Point(83, 37)
        Me.TxtSynonyme.MaxLength = 100
        Me.TxtSynonyme.Name = "TxtSynonyme"
        Me.TxtSynonyme.Size = New System.Drawing.Size(405, 20)
        Me.TxtSynonyme.TabIndex = 53
        '
        'RadBtnAnnuler
        '
        Me.RadBtnAnnuler.ForeColor = System.Drawing.Color.Black
        Me.RadBtnAnnuler.Image = Global.Oasis_WF.My.Resources.Resources.supprimer1
        Me.RadBtnAnnuler.Location = New System.Drawing.Point(124, 89)
        Me.RadBtnAnnuler.Name = "RadBtnAnnuler"
        Me.RadBtnAnnuler.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAnnuler.TabIndex = 58
        Me.RadBtnAnnuler.Text = "Supprimer"
        Me.ToolTip.SetToolTip(Me.RadBtnAnnuler, "Supprimer")
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.ForeColor = System.Drawing.Color.Black
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.Location = New System.Drawing.Point(8, 89)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 59
        Me.RadBtnValidation.Text = "Valider"
        Me.ToolTip.SetToolTip(Me.RadBtnValidation, "Valider")
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandonner.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandonner.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(486, 89)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandonner.TabIndex = 60
        '
        'RadFDrcSynonymeDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandonner
        Me.ClientSize = New System.Drawing.Size(522, 124)
        Me.Controls.Add(Me.RadBtnAbandonner)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnAnnuler)
        Me.Controls.Add(Me.LblDrcDescription)
        Me.Controls.Add(Me.LblDrcId)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtSynonyme)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFDrcSynonymeDetailEdit"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Détail synonyme DRC"
        CType(Me.RadBtnAnnuler, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LblDrcDescription As Label
    Friend WithEvents LblDrcId As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtSynonyme As TextBox
    Friend WithEvents RadBtnAnnuler As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
End Class

