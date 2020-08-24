<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFAldCim10Selecteur
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadFAldCim10Selecteur))
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor1 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.PnlSelection = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadBtnSelection = New Telerik.WinControls.UI.RadButton()
        Me.LblCim10Code = New System.Windows.Forms.Label()
        Me.LblCim10Description = New System.Windows.Forms.Label()
        Me.RadAldDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        CType(Me.PnlSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSelection.SuspendLayout()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadAldDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadAldDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PnlSelection
        '
        Me.PnlSelection.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.PnlSelection.Controls.Add(Me.RadBtnSelection)
        Me.PnlSelection.Controls.Add(Me.LblCim10Code)
        Me.PnlSelection.Controls.Add(Me.LblCim10Description)
        Me.PnlSelection.HeaderText = "Code CIM10 sélectionné"
        Me.PnlSelection.Location = New System.Drawing.Point(720, 12)
        Me.PnlSelection.Name = "PnlSelection"
        Me.PnlSelection.Size = New System.Drawing.Size(431, 135)
        Me.PnlSelection.TabIndex = 0
        Me.PnlSelection.Text = "Code CIM10 sélectionné"
        '
        'RadBtnSelection
        '
        Me.RadBtnSelection.ForeColor = System.Drawing.Color.Black
        Me.RadBtnSelection.Image = CType(resources.GetObject("RadBtnSelection.Image"), System.Drawing.Image)
        Me.RadBtnSelection.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnSelection.Location = New System.Drawing.Point(19, 87)
        Me.RadBtnSelection.Name = "RadBtnSelection"
        Me.RadBtnSelection.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnSelection.TabIndex = 5
        '
        'LblCim10Code
        '
        Me.LblCim10Code.AutoSize = True
        Me.LblCim10Code.Location = New System.Drawing.Point(16, 41)
        Me.LblCim10Code.Name = "LblCim10Code"
        Me.LblCim10Code.Size = New System.Drawing.Size(19, 13)
        Me.LblCim10Code.TabIndex = 3
        Me.LblCim10Code.Text = "43"
        '
        'LblCim10Description
        '
        Me.LblCim10Description.AutoSize = True
        Me.LblCim10Description.Location = New System.Drawing.Point(89, 41)
        Me.LblCim10Description.Name = "LblCim10Description"
        Me.LblCim10Description.Size = New System.Drawing.Size(45, 13)
        Me.LblCim10Description.TabIndex = 4
        Me.LblCim10Description.Text = "Brûlure"
        '
        'RadAldDataGridView
        '
        Me.RadAldDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadAldDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadAldDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadAldDataGridView.ForeColor = System.Drawing.Color.Black
        Me.RadAldDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadAldDataGridView.Location = New System.Drawing.Point(12, 12)
        Me.RadAldDataGridView.Margin = New System.Windows.Forms.Padding(12)
        '
        '
        '
        Me.RadAldDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadAldDataGridView.MasterTemplate.AllowDeleteRow = False
        Me.RadAldDataGridView.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "AldCim10Id"
        GridViewTextBoxColumn1.HeaderText = "Identifiant"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.MinWidth = 20
        GridViewTextBoxColumn1.Name = "AldCim10Id"
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "AldCim10AldCode"
        GridViewTextBoxColumn2.HeaderText = "Code ALD"
        GridViewTextBoxColumn2.MinWidth = 20
        GridViewTextBoxColumn2.Name = "AldCim10AldCode"
        GridViewTextBoxColumn2.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending
        GridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn2.Width = 200
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "AldCim10Code"
        GridViewTextBoxColumn3.HeaderText = "Code CIM10"
        GridViewTextBoxColumn3.MinWidth = 20
        GridViewTextBoxColumn3.Name = "AldCim10Code"
        GridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn3.Width = 200
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "AldCim10Description"
        GridViewTextBoxColumn4.HeaderText = "Dénomination"
        GridViewTextBoxColumn4.MinWidth = 20
        GridViewTextBoxColumn4.Name = "AldCim10Description"
        GridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn4.Width = 500
        Me.RadAldDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4})
        Me.RadAldDataGridView.MasterTemplate.EnableGrouping = False
        SortDescriptor1.PropertyName = "AldCim10AldCode"
        Me.RadAldDataGridView.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor1})
        Me.RadAldDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadAldDataGridView.Name = "RadAldDataGridView"
        Me.RadAldDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadAldDataGridView.Size = New System.Drawing.Size(693, 340)
        Me.RadAldDataGridView.TabIndex = 1
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandonner.Image = CType(resources.GetObject("RadBtnAbandonner.Image"), System.Drawing.Image)
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(1109, 328)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandonner.TabIndex = 2
        '
        'RadFAldCim10Selecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandonner
        Me.ClientSize = New System.Drawing.Size(1161, 364)
        Me.Controls.Add(Me.RadBtnAbandonner)
        Me.Controls.Add(Me.RadAldDataGridView)
        Me.Controls.Add(Me.PnlSelection)
        Me.Name = "RadFAldCim10Selecteur"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sélecteur code CIM10 ALD"
        CType(Me.PnlSelection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSelection.ResumeLayout(False)
        Me.PnlSelection.PerformLayout()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadAldDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadAldDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PnlSelection As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadAldDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSelection As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblCim10Code As Label
    Friend WithEvents LblCim10Description As Label
End Class

