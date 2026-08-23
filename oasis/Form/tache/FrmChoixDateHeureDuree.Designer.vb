<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmChoixDateHeureDuree
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
        Me.RadButtonAbandon = New Telerik.WinControls.UI.RadButton()
        Me.BtnValider = New Telerik.WinControls.UI.RadButton()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.TxtRDVCommentaire = New System.Windows.Forms.TextBox()
        Me.RadioBtn45 = New System.Windows.Forms.RadioButton()
        Me.RadioBtn30 = New System.Windows.Forms.RadioButton()
        Me.RadioBtn15 = New System.Windows.Forms.RadioButton()
        Me.RadioBtn0 = New System.Windows.Forms.RadioButton()
        Me.NumheureRV = New System.Windows.Forms.NumericUpDown()
        Me.NumDateRV = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadButtonAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnValider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me.NumheureRV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.RadButtonAbandon)
        Me.RadPanel1.Controls.Add(Me.BtnValider)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel1.Location = New System.Drawing.Point(0, 244)
        Me.RadPanel1.Name = "RadPanel1"
        '
        '
        '
        Me.RadPanel1.RootElement.BorderHighlightThickness = 1
        Me.RadPanel1.Size = New System.Drawing.Size(488, 45)
        Me.RadPanel1.TabIndex = 14
        CType(Me.RadPanel1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
        '
        'RadButtonAbandon
        '
        Me.RadButtonAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadButtonAbandon.Location = New System.Drawing.Point(370, 6)
        Me.RadButtonAbandon.Name = "RadButtonAbandon"
        Me.RadButtonAbandon.Size = New System.Drawing.Size(110, 34)
        Me.RadButtonAbandon.TabIndex = 0
        Me.RadButtonAbandon.Text = "Abandonner"
        '
        'BtnValider
        '
        Me.BtnValider.Location = New System.Drawing.Point(6, 6)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(110, 34)
        Me.BtnValider.TabIndex = 0
        Me.BtnValider.Text = "Valider"
        '
        'RadPanel2
        '
        Me.RadPanel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadPanel2.Controls.Add(Me.Label16)
        Me.RadPanel2.Controls.Add(Me.TxtRDVCommentaire)
        Me.RadPanel2.Controls.Add(Me.RadioBtn45)
        Me.RadPanel2.Controls.Add(Me.RadioBtn30)
        Me.RadPanel2.Controls.Add(Me.RadioBtn15)
        Me.RadPanel2.Controls.Add(Me.RadioBtn0)
        Me.RadPanel2.Controls.Add(Me.NumheureRV)
        Me.RadPanel2.Controls.Add(Me.NumDateRV)
        Me.RadPanel2.Controls.Add(Me.Label1)
        Me.RadPanel2.Controls.Add(Me.Label12)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadPanel2.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(488, 238)
        Me.RadPanel2.TabIndex = 15
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Label16.Location = New System.Drawing.Point(6, 151)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(474, 13)
        Me.Label16.TabIndex = 27
        Me.Label16.Text = "Commentaire"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TxtRDVCommentaire
        '
        Me.TxtRDVCommentaire.Location = New System.Drawing.Point(6, 167)
        Me.TxtRDVCommentaire.MaxLength = 200
        Me.TxtRDVCommentaire.Multiline = True
        Me.TxtRDVCommentaire.Name = "TxtRDVCommentaire"
        Me.TxtRDVCommentaire.Size = New System.Drawing.Size(474, 68)
        Me.TxtRDVCommentaire.TabIndex = 26
        '
        'RadioBtn45
        '
        Me.RadioBtn45.AutoSize = True
        Me.RadioBtn45.Location = New System.Drawing.Point(230, 119)
        Me.RadioBtn45.Name = "RadioBtn45"
        Me.RadioBtn45.Size = New System.Drawing.Size(81, 17)
        Me.RadioBtn45.TabIndex = 25
        Me.RadioBtn45.Text = "45 minutes"
        Me.RadioBtn45.UseVisualStyleBackColor = True
        '
        'RadioBtn30
        '
        Me.RadioBtn30.AutoSize = True
        Me.RadioBtn30.Location = New System.Drawing.Point(230, 93)
        Me.RadioBtn30.Name = "RadioBtn30"
        Me.RadioBtn30.Size = New System.Drawing.Size(81, 17)
        Me.RadioBtn30.TabIndex = 24
        Me.RadioBtn30.Text = "30 minutes"
        Me.RadioBtn30.UseVisualStyleBackColor = True
        '
        'RadioBtn15
        '
        Me.RadioBtn15.AutoSize = True
        Me.RadioBtn15.Location = New System.Drawing.Point(230, 70)
        Me.RadioBtn15.Name = "RadioBtn15"
        Me.RadioBtn15.Size = New System.Drawing.Size(81, 17)
        Me.RadioBtn15.TabIndex = 23
        Me.RadioBtn15.Text = "15 minutes"
        Me.RadioBtn15.UseVisualStyleBackColor = True
        '
        'RadioBtn0
        '
        Me.RadioBtn0.AutoSize = True
        Me.RadioBtn0.Checked = True
        Me.RadioBtn0.Location = New System.Drawing.Point(230, 47)
        Me.RadioBtn0.Name = "RadioBtn0"
        Me.RadioBtn0.Size = New System.Drawing.Size(70, 17)
        Me.RadioBtn0.TabIndex = 22
        Me.RadioBtn0.TabStop = True
        Me.RadioBtn0.Text = "0 minute"
        Me.RadioBtn0.UseVisualStyleBackColor = True
        '
        'NumheureRV
        '
        Me.NumheureRV.Location = New System.Drawing.Point(159, 82)
        Me.NumheureRV.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.NumheureRV.Name = "NumheureRV"
        Me.NumheureRV.Size = New System.Drawing.Size(48, 20)
        Me.NumheureRV.TabIndex = 19
        Me.NumheureRV.Value = New Decimal(New Integer() {9, 0, 0, 0})
        '
        'NumDateRV
        '
        Me.NumDateRV.Location = New System.Drawing.Point(158, 19)
        Me.NumDateRV.Name = "NumDateRV"
        Me.NumDateRV.Size = New System.Drawing.Size(200, 20)
        Me.NumDateRV.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(85, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Heure"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(85, 23)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(34, 13)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Date"
        '
        'FrmChoixDateHeureDuree
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadButtonAbandon
        Me.ClientSize = New System.Drawing.Size(488, 289)
        Me.Controls.Add(Me.RadPanel2)
        Me.Controls.Add(Me.RadPanel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmChoixDateHeureDuree"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Prise de Rendez-vous"
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        CType(Me.RadButtonAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnValider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel2.ResumeLayout(False)
        Me.RadPanel2.PerformLayout()
        CType(Me.NumheureRV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadButtonAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnValider As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents NumheureRV As NumericUpDown
    Friend WithEvents NumDateRV As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents RadioBtn45 As RadioButton
    Friend WithEvents RadioBtn30 As RadioButton
    Friend WithEvents RadioBtn15 As RadioButton
    Friend WithEvents RadioBtn0 As RadioButton
    Friend WithEvents Label16 As Label
    Friend WithEvents TxtRDVCommentaire As TextBox
End Class

