<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFActeParamedicalTest
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
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewActePara = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.CbxActiviteEpisode = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGridViewActePara, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewActePara.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGridViewActePara
        '
        Me.RadGridViewActePara.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewActePara.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewActePara.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewActePara.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewActePara.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewActePara.Location = New System.Drawing.Point(12, 71)
        '
        '
        '
        Me.RadGridViewActePara.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewActePara.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewActePara.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "DRC"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.Name = "drc"
        GridViewTextBoxColumn2.Width = 350
        Me.RadGridViewActePara.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn2})
        Me.RadGridViewActePara.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridViewActePara.Name = "RadGridViewActePara"
        Me.RadGridViewActePara.ReadOnly = True
        Me.RadGridViewActePara.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewActePara.ShowGroupPanel = False
        Me.RadGridViewActePara.Size = New System.Drawing.Size(397, 327)
        Me.RadGridViewActePara.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(299, 404)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 1
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'CbxActiviteEpisode
        '
        Me.CbxActiviteEpisode.FormattingEnabled = True
        Me.CbxActiviteEpisode.Location = New System.Drawing.Point(86, 29)
        Me.CbxActiviteEpisode.Name = "CbxActiviteEpisode"
        Me.CbxActiviteEpisode.Size = New System.Drawing.Size(293, 21)
        Me.CbxActiviteEpisode.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Type activité"
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Location = New System.Drawing.Point(183, 404)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 2
        Me.RadBtnValidation.Text = "Validation"
        '
        'RadFActeParamedicalTest
        '
        Me.AcceptButton = Me.RadBtnValidation
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(422, 434)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CbxActiviteEpisode)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGridViewActePara)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFActeParamedicalTest"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFActeParamedicalTest"
        CType(Me.RadGridViewActePara.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewActePara, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadGridViewActePara As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents CbxActiviteEpisode As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
End Class

