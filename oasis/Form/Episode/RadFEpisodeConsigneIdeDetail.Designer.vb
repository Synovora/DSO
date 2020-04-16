<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFEpisodeConsigneIdeDetail
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
        Me.TxtDénominationDrc = New System.Windows.Forms.TextBox()
        Me.TxtDenominationConsigneIde = New System.Windows.Forms.TextBox()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RadBtnCopierDenomination = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnCopierDenomination, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtDénominationDrc
        '
        Me.TxtDénominationDrc.Location = New System.Drawing.Point(108, 22)
        Me.TxtDénominationDrc.Name = "TxtDénominationDrc"
        Me.TxtDénominationDrc.ReadOnly = True
        Me.TxtDénominationDrc.Size = New System.Drawing.Size(769, 20)
        Me.TxtDénominationDrc.TabIndex = 0
        '
        'TxtDenominationConsigneIde
        '
        Me.TxtDenominationConsigneIde.Location = New System.Drawing.Point(108, 48)
        Me.TxtDenominationConsigneIde.Multiline = True
        Me.TxtDenominationConsigneIde.Name = "TxtDenominationConsigneIde"
        Me.TxtDenominationConsigneIde.Size = New System.Drawing.Size(769, 92)
        Me.TxtDenominationConsigneIde.TabIndex = 1
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(853, 146)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.RadBtnAbandon, "Abandon")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "DORC"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Consigne IDE"
        '
        'RadBtnCopierDenomination
        '
        Me.RadBtnCopierDenomination.Location = New System.Drawing.Point(138, 146)
        Me.RadBtnCopierDenomination.Name = "RadBtnCopierDenomination"
        Me.RadBtnCopierDenomination.Size = New System.Drawing.Size(140, 24)
        Me.RadBtnCopierDenomination.TabIndex = 5
        Me.RadBtnCopierDenomination.Text = "Copier dénomination"
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnValidation.Location = New System.Drawing.Point(108, 146)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnValidation.TabIndex = 6
        Me.ToolTip.SetToolTip(Me.RadBtnValidation, "Valider")
        '
        'RadFEpisodeConsigneIdeDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(889, 178)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnCopierDenomination)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.TxtDenominationConsigneIde)
        Me.Controls.Add(Me.TxtDénominationDrc)
        Me.Name = "RadFEpisodeConsigneIdeDetail"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFEpisodeConsigneIdeDetail"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnCopierDenomination, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtDénominationDrc As TextBox
    Friend WithEvents TxtDenominationConsigneIde As TextBox
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents RadBtnCopierDenomination As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
End Class

