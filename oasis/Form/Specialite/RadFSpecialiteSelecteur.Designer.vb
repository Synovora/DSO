<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFSpecialiteSelecteur
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
        Dim GridViewDecimalColumn1 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDecimalColumn2 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn3 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewCheckBoxColumn1 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewCheckBoxColumn2 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewCheckBoxColumn3 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadSpecialiteDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.OarspecialiteBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DS_Specialite = New Oasis_WF.DS_Specialite()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSelection = New Telerik.WinControls.UI.RadButton()
        Me.RadGbxSelect = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblNature = New System.Windows.Forms.Label()
        Me.LblSpecialite = New System.Windows.Forms.Label()
        Me.Oa_r_specialiteTableAdapter = New Oasis_WF.DS_SpecialiteTableAdapters.oa_r_specialiteTableAdapter()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadSpecialiteDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadSpecialiteDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OarspecialiteBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DS_Specialite, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGbxSelect, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGbxSelect.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadSpecialiteDataGridView
        '
        Me.RadSpecialiteDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadSpecialiteDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadSpecialiteDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadSpecialiteDataGridView.ForeColor = System.Drawing.Color.Black
        Me.RadSpecialiteDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadSpecialiteDataGridView.Location = New System.Drawing.Point(12, 43)
        '
        '
        '
        Me.RadSpecialiteDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadSpecialiteDataGridView.MasterTemplate.AllowCellContextMenu = False
        Me.RadSpecialiteDataGridView.MasterTemplate.AllowDeleteRow = False
        Me.RadSpecialiteDataGridView.MasterTemplate.AllowDragToGroup = False
        Me.RadSpecialiteDataGridView.MasterTemplate.AllowEditRow = False
        GridViewDecimalColumn1.DataType = GetType(Integer)
        GridViewDecimalColumn1.EnableExpressionEditor = False
        GridViewDecimalColumn1.FieldName = "oa_r_specialite_id"
        GridViewDecimalColumn1.HeaderText = "oa_r_specialite_id"
        GridViewDecimalColumn1.IsAutoGenerated = True
        GridViewDecimalColumn1.IsVisible = False
        GridViewDecimalColumn1.Name = "oa_r_specialite_id"
        GridViewDecimalColumn1.ReadOnly = True
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "oa_specialite_code"
        GridViewTextBoxColumn1.HeaderText = "oa_specialite_code"
        GridViewTextBoxColumn1.IsAutoGenerated = True
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "oa_specialite_code"
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "oa_r_specialite_description"
        GridViewTextBoxColumn2.HeaderText = "Spécialité"
        GridViewTextBoxColumn2.IsAutoGenerated = True
        GridViewTextBoxColumn2.Name = "oa_r_specialite_description"
        GridViewTextBoxColumn2.Width = 350
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "oa_r_specialite_nature"
        GridViewTextBoxColumn3.HeaderText = "Nature"
        GridViewTextBoxColumn3.IsAutoGenerated = True
        GridViewTextBoxColumn3.Name = "oa_r_specialite_nature"
        GridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn3.Width = 120
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "oa_r_specialite_genre"
        GridViewTextBoxColumn4.HeaderText = "oa_r_specialite_genre"
        GridViewTextBoxColumn4.IsAutoGenerated = True
        GridViewTextBoxColumn4.IsVisible = False
        GridViewTextBoxColumn4.Name = "oa_r_specialite_genre"
        GridViewDecimalColumn2.DataType = GetType(Integer)
        GridViewDecimalColumn2.EnableExpressionEditor = False
        GridViewDecimalColumn2.FieldName = "oa_r_specialite_age_min"
        GridViewDecimalColumn2.HeaderText = "oa_r_specialite_age_min"
        GridViewDecimalColumn2.IsAutoGenerated = True
        GridViewDecimalColumn2.IsVisible = False
        GridViewDecimalColumn2.Name = "oa_r_specialite_age_min"
        GridViewDecimalColumn3.DataType = GetType(Integer)
        GridViewDecimalColumn3.EnableExpressionEditor = False
        GridViewDecimalColumn3.FieldName = "oa_r_specialite_age_max"
        GridViewDecimalColumn3.HeaderText = "oa_r_specialite_age_max"
        GridViewDecimalColumn3.IsAutoGenerated = True
        GridViewDecimalColumn3.IsVisible = False
        GridViewDecimalColumn3.Name = "oa_r_specialite_age_max"
        GridViewCheckBoxColumn1.EnableExpressionEditor = False
        GridViewCheckBoxColumn1.FieldName = "oa_r_specialite_inactif"
        GridViewCheckBoxColumn1.HeaderText = "oa_r_specialite_inactif"
        GridViewCheckBoxColumn1.IsAutoGenerated = True
        GridViewCheckBoxColumn1.IsVisible = False
        GridViewCheckBoxColumn1.MinWidth = 20
        GridViewCheckBoxColumn1.Name = "oa_r_specialite_inactif"
        GridViewCheckBoxColumn2.EnableExpressionEditor = False
        GridViewCheckBoxColumn2.FieldName = "oa_r_parcours"
        GridViewCheckBoxColumn2.HeaderText = "oa_r_parcours"
        GridViewCheckBoxColumn2.IsAutoGenerated = True
        GridViewCheckBoxColumn2.IsVisible = False
        GridViewCheckBoxColumn2.MinWidth = 20
        GridViewCheckBoxColumn2.Name = "oa_r_parcours"
        GridViewCheckBoxColumn3.AllowGroup = False
        GridViewCheckBoxColumn3.AllowSearching = False
        GridViewCheckBoxColumn3.AllowSort = False
        GridViewCheckBoxColumn3.EnableExpressionEditor = False
        GridViewCheckBoxColumn3.FieldName = "oa_r_oasis"
        GridViewCheckBoxColumn3.HeaderText = "Oasis"
        GridViewCheckBoxColumn3.IsAutoGenerated = True
        GridViewCheckBoxColumn3.MinWidth = 20
        GridViewCheckBoxColumn3.Name = "oa_r_oasis"
        GridViewCheckBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewCheckBoxColumn3.Width = 70
        Me.RadSpecialiteDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn1, GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewDecimalColumn2, GridViewDecimalColumn3, GridViewCheckBoxColumn1, GridViewCheckBoxColumn2, GridViewCheckBoxColumn3})
        Me.RadSpecialiteDataGridView.MasterTemplate.DataSource = Me.OarspecialiteBindingSource
        Me.RadSpecialiteDataGridView.MasterTemplate.EnableFiltering = True
        Me.RadSpecialiteDataGridView.MasterTemplate.EnableGrouping = False
        Me.RadSpecialiteDataGridView.MasterTemplate.ShowRowHeaderColumn = False
        Me.RadSpecialiteDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadSpecialiteDataGridView.Name = "RadSpecialiteDataGridView"
        Me.RadSpecialiteDataGridView.ReadOnly = True
        Me.RadSpecialiteDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadSpecialiteDataGridView.ShowGroupPanel = False
        Me.RadSpecialiteDataGridView.Size = New System.Drawing.Size(568, 324)
        Me.RadSpecialiteDataGridView.TabIndex = 0
        '
        'OarspecialiteBindingSource
        '
        Me.OarspecialiteBindingSource.DataMember = "oa_r_specialite"
        Me.OarspecialiteBindingSource.DataSource = Me.DS_Specialite
        '
        'DS_Specialite
        '
        Me.DS_Specialite.DataSetName = "DS_Specialite"
        Me.DS_Specialite.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(786, 342)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 1
        Me.RadBtnAbandon.Text = "Abandonner"
        '
        'RadBtnSelection
        '
        Me.RadBtnSelection.Image = Global.Oasis_WF.My.Resources.Resources._select
        Me.RadBtnSelection.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnSelection.Location = New System.Drawing.Point(21, 123)
        Me.RadBtnSelection.Name = "RadBtnSelection"
        Me.RadBtnSelection.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnSelection.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.RadBtnSelection, "Valider la sélection")
        '
        'RadGbxSelect
        '
        Me.RadGbxSelect.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGbxSelect.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.RadGbxSelect.Controls.Add(Me.LblNature)
        Me.RadGbxSelect.Controls.Add(Me.LblSpecialite)
        Me.RadGbxSelect.Controls.Add(Me.RadBtnSelection)
        Me.RadGbxSelect.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGbxSelect.HeaderText = "Spécialité sélectionnée"
        Me.RadGbxSelect.Location = New System.Drawing.Point(590, 43)
        Me.RadGbxSelect.Name = "RadGbxSelect"
        Me.RadGbxSelect.Size = New System.Drawing.Size(306, 174)
        Me.RadGbxSelect.TabIndex = 3
        Me.RadGbxSelect.Text = "Spécialité sélectionnée"
        '
        'LblNature
        '
        Me.LblNature.AutoSize = True
        Me.LblNature.Location = New System.Drawing.Point(18, 82)
        Me.LblNature.Name = "LblNature"
        Me.LblNature.Size = New System.Drawing.Size(42, 13)
        Me.LblNature.TabIndex = 4
        Me.LblNature.Text = "Nature"
        '
        'LblSpecialite
        '
        Me.LblSpecialite.AutoSize = True
        Me.LblSpecialite.Location = New System.Drawing.Point(18, 45)
        Me.LblSpecialite.Name = "LblSpecialite"
        Me.LblSpecialite.Size = New System.Drawing.Size(56, 13)
        Me.LblSpecialite.TabIndex = 3
        Me.LblSpecialite.Text = "Spécialité"
        '
        'Oa_r_specialiteTableAdapter
        '
        Me.Oa_r_specialiteTableAdapter.ClearBeforeFill = True
        '
        'RadFSpecialiteSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(906, 378)
        Me.Controls.Add(Me.RadGbxSelect)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadSpecialiteDataGridView)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFSpecialiteSelecteur"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sélection d'une spécialité"
        CType(Me.RadSpecialiteDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadSpecialiteDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OarspecialiteBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DS_Specialite, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGbxSelect, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGbxSelect.ResumeLayout(False)
        Me.RadGbxSelect.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadSpecialiteDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSelection As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGbxSelect As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents DS_Specialite As DS_Specialite
    Friend WithEvents OarspecialiteBindingSource As BindingSource
    Friend WithEvents Oa_r_specialiteTableAdapter As DS_SpecialiteTableAdapters.oa_r_specialiteTableAdapter
    Friend WithEvents LblNature As Label
    Friend WithEvents LblSpecialite As Label
    Friend WithEvents ToolTip As ToolTip
End Class

