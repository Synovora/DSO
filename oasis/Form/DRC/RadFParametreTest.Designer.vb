<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFParametreTest
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
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CbxActiviteEpisode = New System.Windows.Forms.ComboBox()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadGridViewParametre = New Telerik.WinControls.UI.RadGridView()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParametre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParametre.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Location = New System.Drawing.Point(183, 387)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 6
        Me.RadBtnValidation.Text = "Validation"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Type activité"
        '
        'CbxActiviteEpisode
        '
        Me.CbxActiviteEpisode.FormattingEnabled = True
        Me.CbxActiviteEpisode.Location = New System.Drawing.Point(86, 12)
        Me.CbxActiviteEpisode.Name = "CbxActiviteEpisode"
        Me.CbxActiviteEpisode.Size = New System.Drawing.Size(293, 21)
        Me.CbxActiviteEpisode.TabIndex = 7
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(299, 387)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 5
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadGridViewParametre
        '
        Me.RadGridViewParametre.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewParametre.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewParametre.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewParametre.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewParametre.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewParametre.Location = New System.Drawing.Point(12, 54)
        '
        '
        '
        Me.RadGridViewParametre.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewParametre.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewParametre.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "Parametre"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.Name = "parametre"
        GridViewTextBoxColumn1.Width = 350
        Me.RadGridViewParametre.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1})
        Me.RadGridViewParametre.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewParametre.Name = "RadGridViewParametre"
        Me.RadGridViewParametre.ReadOnly = True
        Me.RadGridViewParametre.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParametre.ShowGroupPanel = False
        Me.RadGridViewParametre.Size = New System.Drawing.Size(397, 327)
        Me.RadGridViewParametre.TabIndex = 4
        '
        'RadFParametreTest
        '
        Me.AcceptButton = Me.RadBtnValidation
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(419, 424)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CbxActiviteEpisode)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGridViewParametre)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFParametreTest"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFParametreTest"
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParametre.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParametre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents Label1 As Label
    Friend WithEvents CbxActiviteEpisode As ComboBox
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGridViewParametre As Telerik.WinControls.UI.RadGridView
End Class

