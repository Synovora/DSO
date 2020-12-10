<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFDrcStandardTypeActiviteDetail
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
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.TxtActiviteEpisode = New System.Windows.Forms.TextBox()
        Me.LblAgeUnite = New System.Windows.Forms.Label()
        Me.LblAgeMax = New System.Windows.Forms.Label()
        Me.NumAgeMax = New System.Windows.Forms.NumericUpDown()
        Me.NumAgeMin = New System.Windows.Forms.NumericUpDown()
        Me.LblAgeMin = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TxtCategorieOasis = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtDrcDescription = New System.Windows.Forms.TextBox()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAnnulation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnDrcDetail = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.NumAgeMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumAgeMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAnnulation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnDrcDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.TxtActiviteEpisode)
        Me.RadGroupBox1.Controls.Add(Me.LblAgeUnite)
        Me.RadGroupBox1.Controls.Add(Me.LblAgeMax)
        Me.RadGroupBox1.Controls.Add(Me.NumAgeMax)
        Me.RadGroupBox1.Controls.Add(Me.NumAgeMin)
        Me.RadGroupBox1.Controls.Add(Me.LblAgeMin)
        Me.RadGroupBox1.Controls.Add(Me.Label12)
        Me.RadGroupBox1.Controls.Add(Me.Label11)
        Me.RadGroupBox1.Controls.Add(Me.TxtCategorieOasis)
        Me.RadGroupBox1.Controls.Add(Me.Label1)
        Me.RadGroupBox1.Controls.Add(Me.TxtDrcDescription)
        Me.RadGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox1.HeaderText = ""
        Me.RadGroupBox1.Location = New System.Drawing.Point(4, 5)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(779, 143)
        Me.RadGroupBox1.TabIndex = 7
        '
        'TxtActiviteEpisode
        '
        Me.TxtActiviteEpisode.Enabled = False
        Me.TxtActiviteEpisode.Location = New System.Drawing.Point(110, 73)
        Me.TxtActiviteEpisode.Name = "TxtActiviteEpisode"
        Me.TxtActiviteEpisode.ReadOnly = True
        Me.TxtActiviteEpisode.Size = New System.Drawing.Size(272, 20)
        Me.TxtActiviteEpisode.TabIndex = 24
        '
        'LblAgeUnite
        '
        Me.LblAgeUnite.AutoSize = True
        Me.LblAgeUnite.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblAgeUnite.Location = New System.Drawing.Point(387, 102)
        Me.LblAgeUnite.Name = "LblAgeUnite"
        Me.LblAgeUnite.Size = New System.Drawing.Size(125, 13)
        Me.LblAgeUnite.TabIndex = 23
        Me.LblAgeUnite.Text = "(Age exprimé en mois)"
        '
        'LblAgeMax
        '
        Me.LblAgeMax.AutoSize = True
        Me.LblAgeMax.Location = New System.Drawing.Point(207, 101)
        Me.LblAgeMax.Name = "LblAgeMax"
        Me.LblAgeMax.Size = New System.Drawing.Size(85, 13)
        Me.LblAgeMax.TabIndex = 22
        Me.LblAgeMax.Text = "Age max (exclu)"
        '
        'NumAgeMax
        '
        Me.NumAgeMax.Location = New System.Drawing.Point(303, 99)
        Me.NumAgeMax.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
        Me.NumAgeMax.Name = "NumAgeMax"
        Me.NumAgeMax.Size = New System.Drawing.Size(70, 20)
        Me.NumAgeMax.TabIndex = 21
        '
        'NumAgeMin
        '
        Me.NumAgeMin.Location = New System.Drawing.Point(114, 99)
        Me.NumAgeMin.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
        Me.NumAgeMin.Name = "NumAgeMin"
        Me.NumAgeMin.Size = New System.Drawing.Size(70, 20)
        Me.NumAgeMin.TabIndex = 20
        '
        'LblAgeMin
        '
        Me.LblAgeMin.AutoSize = True
        Me.LblAgeMin.Location = New System.Drawing.Point(11, 102)
        Me.LblAgeMin.Name = "LblAgeMin"
        Me.LblAgeMin.Size = New System.Drawing.Size(83, 13)
        Me.LblAgeMin.TabIndex = 19
        Me.LblAgeMin.Text = "Age min (inclu)"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(11, 76)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(69, 13)
        Me.Label12.TabIndex = 18
        Me.Label12.Text = "Type activité"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(11, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(88, 13)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "Catégorie Oasis"
        '
        'TxtCategorieOasis
        '
        Me.TxtCategorieOasis.Enabled = False
        Me.TxtCategorieOasis.Location = New System.Drawing.Point(110, 47)
        Me.TxtCategorieOasis.Name = "TxtCategorieOasis"
        Me.TxtCategorieOasis.ReadOnly = True
        Me.TxtCategorieOasis.Size = New System.Drawing.Size(272, 20)
        Me.TxtCategorieOasis.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "DRC"
        '
        'TxtDrcDescription
        '
        Me.TxtDrcDescription.Enabled = False
        Me.TxtDrcDescription.Location = New System.Drawing.Point(110, 21)
        Me.TxtDrcDescription.Name = "TxtDrcDescription"
        Me.TxtDrcDescription.ReadOnly = True
        Me.TxtDrcDescription.Size = New System.Drawing.Size(657, 20)
        Me.TxtDrcDescription.TabIndex = 5
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(759, 159)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 8
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.ForeColor = System.Drawing.Color.Black
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.Location = New System.Drawing.Point(4, 159)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 9
        Me.RadBtnValidation.Text = "Valider"
        Me.ToolTip.SetToolTip(Me.RadBtnValidation, "Valider les données saisies")
        '
        'RadBtnAnnulation
        '
        Me.RadBtnAnnulation.ForeColor = System.Drawing.Color.Black
        Me.RadBtnAnnulation.Image = Global.Oasis_WF.My.Resources.Resources.supprimer
        Me.RadBtnAnnulation.Location = New System.Drawing.Point(120, 159)
        Me.RadBtnAnnulation.Name = "RadBtnAnnulation"
        Me.RadBtnAnnulation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAnnulation.TabIndex = 10
        Me.RadBtnAnnulation.Text = "Supprimer"
        Me.ToolTip.SetToolTip(Me.RadBtnAnnulation, "Supprimer la DRC standard")
        '
        'RadBtnDrcDetail
        '
        Me.RadBtnDrcDetail.Location = New System.Drawing.Point(236, 159)
        Me.RadBtnDrcDetail.Name = "RadBtnDrcDetail"
        Me.RadBtnDrcDetail.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnDrcDetail.TabIndex = 11
        Me.RadBtnDrcDetail.Text = "Détail"
        '
        'RadFDrcStandardTypeActiviteDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(790, 192)
        Me.Controls.Add(Me.RadBtnDrcDetail)
        Me.Controls.Add(Me.RadBtnAnnulation)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.MinimizeBox = False
        Me.Name = "RadFDrcStandardTypeActiviteDetail"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DRC Standard par type activité"
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.NumAgeMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumAgeMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAnnulation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnDrcDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblAgeUnite As Label
    Friend WithEvents LblAgeMax As Label
    Friend WithEvents NumAgeMax As NumericUpDown
    Friend WithEvents NumAgeMin As NumericUpDown
    Friend WithEvents LblAgeMin As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents TxtCategorieOasis As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtDrcDescription As TextBox
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAnnulation As Telerik.WinControls.UI.RadButton
    Friend WithEvents TxtActiviteEpisode As TextBox
    Friend WithEvents RadBtnDrcDetail As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
End Class

