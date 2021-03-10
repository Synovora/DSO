<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFEpisodeProtocoleAiguDetail
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
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.TxtObservation = New System.Windows.Forms.TextBox()
        Me.TxtGuide = New System.Windows.Forms.TextBox()
        Me.TxtReponseCommentee = New System.Windows.Forms.TextBox()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadGroupBox3 = New Telerik.WinControls.UI.RadGroupBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        CType(Me.RadGroupBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox3.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnValidation.Location = New System.Drawing.Point(14, 318)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnValidation.TabIndex = 5
        Me.ToolTip.SetToolTip(Me.RadBtnValidation, "Valider")
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1111, 318)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 4
        Me.ToolTip.SetToolTip(Me.RadBtnAbandon, "Abandon")
        '
        'TxtObservation
        '
        Me.TxtObservation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TxtObservation.Location = New System.Drawing.Point(2, 18)
        Me.TxtObservation.Multiline = True
        Me.TxtObservation.Name = "TxtObservation"
        Me.TxtObservation.Size = New System.Drawing.Size(367, 280)
        Me.TxtObservation.TabIndex = 3
        '
        'TxtGuide
        '
        Me.TxtGuide.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TxtGuide.Location = New System.Drawing.Point(2, 18)
        Me.TxtGuide.Multiline = True
        Me.TxtGuide.Name = "TxtGuide"
        Me.TxtGuide.ReadOnly = True
        Me.TxtGuide.Size = New System.Drawing.Size(367, 280)
        Me.TxtGuide.TabIndex = 6
        '
        'TxtReponseCommentee
        '
        Me.TxtReponseCommentee.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TxtReponseCommentee.Location = New System.Drawing.Point(2, 18)
        Me.TxtReponseCommentee.Multiline = True
        Me.TxtReponseCommentee.Name = "TxtReponseCommentee"
        Me.TxtReponseCommentee.ReadOnly = True
        Me.TxtReponseCommentee.Size = New System.Drawing.Size(367, 280)
        Me.TxtReponseCommentee.TabIndex = 7
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.TxtGuide)
        Me.RadGroupBox1.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.RadGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox1.HeaderText = "Guide d'observation"
        Me.RadGroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(371, 300)
        Me.RadGroupBox1.TabIndex = 8
        Me.RadGroupBox1.Text = "Guide d'observation"
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.Controls.Add(Me.TxtObservation)
        Me.RadGroupBox2.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.RadGroupBox2.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox2.HeaderText = "Observation"
        Me.RadGroupBox2.Location = New System.Drawing.Point(389, 12)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(371, 300)
        Me.RadGroupBox2.TabIndex = 9
        Me.RadGroupBox2.Text = "Observation"
        '
        'RadGroupBox3
        '
        Me.RadGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox3.Controls.Add(Me.TxtReponseCommentee)
        Me.RadGroupBox3.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.RadGroupBox3.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox3.HeaderText = "Réponse commentée"
        Me.RadGroupBox3.Location = New System.Drawing.Point(766, 12)
        Me.RadGroupBox3.Name = "RadGroupBox3"
        Me.RadGroupBox3.Size = New System.Drawing.Size(371, 300)
        Me.RadGroupBox3.TabIndex = 10
        Me.RadGroupBox3.Text = "Réponse commentée"
        '
        'RadFEpisodeProtocoleAiguDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1151, 347)
        Me.Controls.Add(Me.RadGroupBox3)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFEpisodeProtocoleAiguDetail"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.Text = "Episode : Saisie protocole pathologie aigu"
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.RadGroupBox2.PerformLayout()
        CType(Me.RadGroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox3.ResumeLayout(False)
        Me.RadGroupBox3.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents TxtObservation As TextBox
    Friend WithEvents TxtGuide As TextBox
    Friend WithEvents TxtReponseCommentee As TextBox
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadGroupBox3 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents ToolTip As ToolTip
End Class

