<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFDrcActePMAssocieEdit
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
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblDrcDenomination = New System.Windows.Forms.Label()
        Me.RadGridViewDrcAsso = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSelect = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSuppprimer = New Telerik.WinControls.UI.RadButton()
        Me.DrcDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.RadGridViewDrcAsso, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewDrcAsso.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSuppprimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DrcDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DrcDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(169, 19)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Protocole collaboratif : "
        '
        'LblDrcDenomination
        '
        Me.LblDrcDenomination.AutoSize = True
        Me.LblDrcDenomination.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.LblDrcDenomination.Location = New System.Drawing.Point(187, 9)
        Me.LblDrcDenomination.Name = "LblDrcDenomination"
        Me.LblDrcDenomination.Size = New System.Drawing.Size(119, 19)
        Me.LblDrcDenomination.TabIndex = 5
        Me.LblDrcDenomination.Text = "Drc dénomination"
        '
        'RadGridViewDrcAsso
        '
        Me.RadGridViewDrcAsso.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewDrcAsso.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewDrcAsso.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewDrcAsso.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewDrcAsso.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewDrcAsso.Location = New System.Drawing.Point(12, 69)
        '
        '
        '
        Me.RadGridViewDrcAsso.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewDrcAsso.MasterTemplate.AllowCellContextMenu = False
        Me.RadGridViewDrcAsso.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewDrcAsso.MasterTemplate.AllowDragToGroup = False
        Me.RadGridViewDrcAsso.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "id"
        GridViewTextBoxColumn1.HeaderText = "id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "id"
        GridViewTextBoxColumn2.AllowGroup = False
        GridViewTextBoxColumn2.AllowSort = False
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "denomination"
        GridViewTextBoxColumn2.HeaderText = "Denomination DRC Acte Paramédical"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.Name = "denomination"
        GridViewTextBoxColumn2.Width = 400
        Me.RadGridViewDrcAsso.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2})
        Me.RadGridViewDrcAsso.MasterTemplate.ShowFilteringRow = False
        Me.RadGridViewDrcAsso.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewDrcAsso.Name = "RadGridViewDrcAsso"
        Me.RadGridViewDrcAsso.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewDrcAsso.ShowGroupPanel = False
        Me.RadGridViewDrcAsso.Size = New System.Drawing.Size(448, 502)
        Me.RadGridViewDrcAsso.TabIndex = 7
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1050, 577)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 8
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadBtnSelect
        '
        Me.RadBtnSelect.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadBtnSelect.Location = New System.Drawing.Point(476, 216)
        Me.RadBtnSelect.Name = "RadBtnSelect"
        Me.RadBtnSelect.Size = New System.Drawing.Size(81, 24)
        Me.RadBtnSelect.TabIndex = 9
        Me.RadBtnSelect.Text = "<<< Ajouter"
        '
        'RadBtnSuppprimer
        '
        Me.RadBtnSuppprimer.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadBtnSuppprimer.Location = New System.Drawing.Point(476, 246)
        Me.RadBtnSuppprimer.Name = "RadBtnSuppprimer"
        Me.RadBtnSuppprimer.Size = New System.Drawing.Size(81, 24)
        Me.RadBtnSuppprimer.TabIndex = 10
        Me.RadBtnSuppprimer.Text = "Enlever >>>"
        '
        'DrcDataGridView
        '
        Me.DrcDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DrcDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.DrcDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.DrcDataGridView.ForeColor = System.Drawing.Color.Black
        Me.DrcDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.DrcDataGridView.Location = New System.Drawing.Point(576, 69)
        '
        '
        '
        Me.DrcDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.DrcDataGridView.MasterTemplate.AllowDeleteRow = False
        Me.DrcDataGridView.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "drcId"
        GridViewTextBoxColumn3.HeaderText = "drcId"
        GridViewTextBoxColumn3.IsVisible = False
        GridViewTextBoxColumn3.Name = "drcId"
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "drcDescription"
        GridViewTextBoxColumn4.HeaderText = "Denomination DRC Acte Paramédical"
        GridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn4.Name = "drcDescription"
        GridViewTextBoxColumn4.Width = 350
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.FieldName = "drcOasis"
        GridViewTextBoxColumn5.HeaderText = "DRC Oasis"
        GridViewTextBoxColumn5.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn5.Name = "drcOasis"
        GridViewTextBoxColumn5.Width = 70
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.FieldName = "contexte"
        GridViewTextBoxColumn6.HeaderText = "Genre"
        GridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn6.Name = "contexte"
        GridViewTextBoxColumn6.Width = 120
        Me.DrcDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6})
        Me.DrcDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.DrcDataGridView.Name = "DrcDataGridView"
        Me.DrcDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DrcDataGridView.ShowGroupPanel = False
        Me.DrcDataGridView.Size = New System.Drawing.Size(584, 502)
        Me.DrcDataGridView.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.Label2.Location = New System.Drawing.Point(572, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(223, 20)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Actes paramédicaux disponibles"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.Label3.Location = New System.Drawing.Point(13, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(371, 20)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Actes paramédicaux associés au protocole collaboratif"
        '
        'RadFDrcActePMAssocieEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1171, 608)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DrcDataGridView)
        Me.Controls.Add(Me.RadBtnSuppprimer)
        Me.Controls.Add(Me.RadBtnSelect)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGridViewDrcAsso)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblDrcDenomination)
        Me.MinimizeBox = False
        Me.Name = "RadFDrcActePMAssocieEdit"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Association actes paramédicaux"
        CType(Me.RadGridViewDrcAsso.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewDrcAsso, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSuppprimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DrcDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DrcDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents LblDrcDenomination As Label
    Friend WithEvents RadGridViewDrcAsso As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSelect As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSuppprimer As Telerik.WinControls.UI.RadButton
    Friend WithEvents DrcDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
End Class

