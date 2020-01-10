<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDRCListe
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
        Dim GridViewDecimalColumn31 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn16 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDecimalColumn32 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn33 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn34 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn17 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDecimalColumn35 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn36 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn37 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDecimalColumn38 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn18 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn19 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn20 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn7 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewDecimalColumn39 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewDateTimeColumn8 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewDecimalColumn40 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim SortDescriptor4 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition4 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadDRCDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.OadrcBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DatSetDRC = New Oasis_WF.DatSetDRC()
        Me.Oa_drcTableAdapter = New Oasis_WF.DatSetDRCTableAdapters.oa_drcTableAdapter()
        CType(Me.RadDRCDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadDRCDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OadrcBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatSetDRC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadDRCDataGridView
        '
        Me.RadDRCDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadDRCDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadDRCDataGridView.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadDRCDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadDRCDataGridView.ForeColor = System.Drawing.Color.Black
        Me.RadDRCDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadDRCDataGridView.Location = New System.Drawing.Point(0, 61)
        '
        '
        '
        Me.RadDRCDataGridView.MasterTemplate.AllowAddNewRow = False
        GridViewDecimalColumn31.AllowFiltering = False
        GridViewDecimalColumn31.DataType = GetType(Integer)
        GridViewDecimalColumn31.EnableExpressionEditor = False
        GridViewDecimalColumn31.FieldName = "oa_drc_id"
        GridViewDecimalColumn31.HeaderText = "Id."
        GridViewDecimalColumn31.IsAutoGenerated = True
        GridViewDecimalColumn31.Name = "oa_drc_id"
        GridViewDecimalColumn31.ReadOnly = True
        GridViewDecimalColumn31.Width = 46
        GridViewTextBoxColumn16.EnableExpressionEditor = False
        GridViewTextBoxColumn16.FieldName = "oa_drc_libelle"
        GridViewTextBoxColumn16.HeaderText = "Libelle"
        GridViewTextBoxColumn16.IsAutoGenerated = True
        GridViewTextBoxColumn16.Name = "oa_drc_libelle"
        GridViewTextBoxColumn16.Width = 302
        GridViewDecimalColumn32.DataType = GetType(Integer)
        GridViewDecimalColumn32.EnableExpressionEditor = False
        GridViewDecimalColumn32.FieldName = "oa_drc_categorie_majeure_id"
        GridViewDecimalColumn32.HeaderText = "CMD"
        GridViewDecimalColumn32.IsAutoGenerated = True
        GridViewDecimalColumn32.Name = "oa_drc_categorie_majeure_id"
        GridViewDecimalColumn32.ReadOnly = True
        GridViewDecimalColumn32.Width = 84
        GridViewDecimalColumn33.DataType = GetType(Short)
        GridViewDecimalColumn33.EnableExpressionEditor = False
        GridViewDecimalColumn33.FieldName = "oa_drc_oasis"
        GridViewDecimalColumn33.HeaderText = "ORC"
        GridViewDecimalColumn33.IsAutoGenerated = True
        GridViewDecimalColumn33.Name = "oa_drc_oasis"
        GridViewDecimalColumn33.ReadOnly = True
        GridViewDecimalColumn33.Width = 84
        GridViewDecimalColumn34.DataType = GetType(Integer)
        GridViewDecimalColumn34.EnableExpressionEditor = False
        GridViewDecimalColumn34.FieldName = "oa_drc_oasis_categorie"
        GridViewDecimalColumn34.HeaderText = "categorie"
        GridViewDecimalColumn34.IsAutoGenerated = True
        GridViewDecimalColumn34.Name = "oa_drc_oasis_categorie"
        GridViewDecimalColumn34.ReadOnly = True
        GridViewDecimalColumn34.Width = 84
        GridViewTextBoxColumn17.EnableExpressionEditor = False
        GridViewTextBoxColumn17.HeaderText = "Catégorie Oasis"
        GridViewTextBoxColumn17.Name = "categorieOasisLibelle"
        GridViewTextBoxColumn17.SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending
        GridViewTextBoxColumn17.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn17.Width = 109
        GridViewDecimalColumn35.DataType = GetType(Short)
        GridViewDecimalColumn35.EnableExpressionEditor = False
        GridViewDecimalColumn35.FieldName = "oa_drc_oasis_invalide"
        GridViewDecimalColumn35.HeaderText = "invalide"
        GridViewDecimalColumn35.IsAutoGenerated = True
        GridViewDecimalColumn35.Name = "oa_drc_oasis_invalide"
        GridViewDecimalColumn35.ReadOnly = True
        GridViewDecimalColumn35.Width = 84
        GridViewDecimalColumn36.DataType = GetType(Integer)
        GridViewDecimalColumn36.EnableExpressionEditor = False
        GridViewDecimalColumn36.FieldName = "oa_drc_sexe"
        GridViewDecimalColumn36.HeaderText = "sexe"
        GridViewDecimalColumn36.IsAutoGenerated = True
        GridViewDecimalColumn36.Name = "oa_drc_sexe"
        GridViewDecimalColumn36.Width = 84
        GridViewDecimalColumn37.AllowFiltering = False
        GridViewDecimalColumn37.DataType = GetType(Integer)
        GridViewDecimalColumn37.EnableExpressionEditor = False
        GridViewDecimalColumn37.FieldName = "oa_drc_age_min"
        GridViewDecimalColumn37.HeaderText = "age_min"
        GridViewDecimalColumn37.IsAutoGenerated = True
        GridViewDecimalColumn37.Name = "oa_drc_age_min"
        GridViewDecimalColumn37.Width = 84
        GridViewDecimalColumn38.AllowFiltering = False
        GridViewDecimalColumn38.DataType = GetType(Integer)
        GridViewDecimalColumn38.EnableExpressionEditor = False
        GridViewDecimalColumn38.FieldName = "oa_drc_age_max"
        GridViewDecimalColumn38.HeaderText = "age_max"
        GridViewDecimalColumn38.IsAutoGenerated = True
        GridViewDecimalColumn38.Name = "oa_drc_age_max"
        GridViewDecimalColumn38.Width = 84
        GridViewTextBoxColumn18.AllowFiltering = False
        GridViewTextBoxColumn18.EnableExpressionEditor = False
        GridViewTextBoxColumn18.FieldName = "oa_drc_ald_code"
        GridViewTextBoxColumn18.HeaderText = "ald_code"
        GridViewTextBoxColumn18.IsAutoGenerated = True
        GridViewTextBoxColumn18.Name = "oa_drc_ald_code"
        GridViewTextBoxColumn18.ReadOnly = True
        GridViewTextBoxColumn18.Width = 84
        GridViewTextBoxColumn19.AllowFiltering = False
        GridViewTextBoxColumn19.EnableExpressionEditor = False
        GridViewTextBoxColumn19.FieldName = "oa_drc_code_cim_defaut"
        GridViewTextBoxColumn19.HeaderText = "Cim10"
        GridViewTextBoxColumn19.IsAutoGenerated = True
        GridViewTextBoxColumn19.Name = "oa_drc_code_cim_defaut"
        GridViewTextBoxColumn19.Width = 84
        GridViewTextBoxColumn20.AllowFiltering = False
        GridViewTextBoxColumn20.EnableExpressionEditor = False
        GridViewTextBoxColumn20.FieldName = "oa_drc_code_cisp_defaut"
        GridViewTextBoxColumn20.HeaderText = "CISP"
        GridViewTextBoxColumn20.IsAutoGenerated = True
        GridViewTextBoxColumn20.Name = "oa_drc_code_cisp_defaut"
        GridViewTextBoxColumn20.Width = 84
        GridViewDateTimeColumn7.AllowFiltering = False
        GridViewDateTimeColumn7.EnableExpressionEditor = False
        GridViewDateTimeColumn7.FieldName = "oa_drc_date_creation"
        GridViewDateTimeColumn7.HeaderText = "date_creation"
        GridViewDateTimeColumn7.IsAutoGenerated = True
        GridViewDateTimeColumn7.IsVisible = False
        GridViewDateTimeColumn7.Name = "oa_drc_date_creation"
        GridViewDateTimeColumn7.ReadOnly = True
        GridViewDateTimeColumn7.Width = 84
        GridViewDecimalColumn39.AllowFiltering = False
        GridViewDecimalColumn39.DataType = GetType(Integer)
        GridViewDecimalColumn39.EnableExpressionEditor = False
        GridViewDecimalColumn39.FieldName = "oa_drc_utilisateur_creation"
        GridViewDecimalColumn39.HeaderText = "utilisateur_creation"
        GridViewDecimalColumn39.IsAutoGenerated = True
        GridViewDecimalColumn39.IsVisible = False
        GridViewDecimalColumn39.Name = "oa_drc_utilisateur_creation"
        GridViewDecimalColumn39.ReadOnly = True
        GridViewDecimalColumn39.Width = 84
        GridViewDateTimeColumn8.AllowFiltering = False
        GridViewDateTimeColumn8.EnableExpressionEditor = False
        GridViewDateTimeColumn8.FieldName = "oa_drc_date_modification"
        GridViewDateTimeColumn8.HeaderText = "date_modification"
        GridViewDateTimeColumn8.IsAutoGenerated = True
        GridViewDateTimeColumn8.IsVisible = False
        GridViewDateTimeColumn8.Name = "oa_drc_date_modification"
        GridViewDateTimeColumn8.ReadOnly = True
        GridViewDateTimeColumn8.Width = 84
        GridViewDecimalColumn40.AllowFiltering = False
        GridViewDecimalColumn40.DataType = GetType(Integer)
        GridViewDecimalColumn40.EnableExpressionEditor = False
        GridViewDecimalColumn40.FieldName = "oa_drc_utilisateur_modification"
        GridViewDecimalColumn40.HeaderText = "utilisateur_modification"
        GridViewDecimalColumn40.IsAutoGenerated = True
        GridViewDecimalColumn40.IsVisible = False
        GridViewDecimalColumn40.Name = "oa_drc_utilisateur_modification"
        GridViewDecimalColumn40.ReadOnly = True
        GridViewDecimalColumn40.Width = 89
        Me.RadDRCDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn31, GridViewTextBoxColumn16, GridViewDecimalColumn32, GridViewDecimalColumn33, GridViewDecimalColumn34, GridViewTextBoxColumn17, GridViewDecimalColumn35, GridViewDecimalColumn36, GridViewDecimalColumn37, GridViewDecimalColumn38, GridViewTextBoxColumn18, GridViewTextBoxColumn19, GridViewTextBoxColumn20, GridViewDateTimeColumn7, GridViewDecimalColumn39, GridViewDateTimeColumn8, GridViewDecimalColumn40})
        Me.RadDRCDataGridView.MasterTemplate.DataSource = Me.OadrcBindingSource
        Me.RadDRCDataGridView.MasterTemplate.EnableFiltering = True
        SortDescriptor4.Direction = System.ComponentModel.ListSortDirection.Descending
        SortDescriptor4.PropertyName = "categorieOasisLibelle"
        Me.RadDRCDataGridView.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor4})
        Me.RadDRCDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition4
        Me.RadDRCDataGridView.Name = "RadDRCDataGridView"
        Me.RadDRCDataGridView.ReadOnly = True
        Me.RadDRCDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadDRCDataGridView.Size = New System.Drawing.Size(1229, 496)
        Me.RadDRCDataGridView.TabIndex = 0
        '
        'OadrcBindingSource
        '
        Me.OadrcBindingSource.DataMember = "oa_drc"
        Me.OadrcBindingSource.DataSource = Me.DatSetDRC
        '
        'DatSetDRC
        '
        Me.DatSetDRC.DataSetName = "DatSetDRC"
        Me.DatSetDRC.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Oa_drcTableAdapter
        '
        Me.Oa_drcTableAdapter.ClearBeforeFill = True
        '
        'FrmDRCListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1229, 557)
        Me.Controls.Add(Me.RadDRCDataGridView)
        Me.Name = "FrmDRCListe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadForm1"
        CType(Me.RadDRCDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadDRCDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OadrcBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatSetDRC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadDRCDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents DatSetDRC As DatSetDRC
    Friend WithEvents OadrcBindingSource As BindingSource
    Friend WithEvents Oa_drcTableAdapter As DatSetDRCTableAdapters.oa_drcTableAdapter
End Class

