<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFRorListe
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
        Dim GridViewDecimalColumn2 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDecimalColumn3 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.LblType = New System.Windows.Forms.Label()
        Me.lblLabelType = New System.Windows.Forms.Label()
        Me.LblSpecialiteFiltre = New System.Windows.Forms.Label()
        Me.LblLabelSpecialite = New System.Windows.Forms.Label()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        Me.VrorBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.RORDS = New Oasis_WF.RORDS()
        Me.RadPanel3 = New Telerik.WinControls.UI.RadPanel()
        Me.GbxSelection = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblSpecialite = New System.Windows.Forms.Label()
        Me.LblNom = New System.Windows.Forms.Label()
        Me.RadBtnSelection = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnCreation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.V_rorTableAdapter = New Oasis_WF.RORDSTableAdapters.v_rorTableAdapter()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréationNouvelIntervenantToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VrorBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RORDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel3.SuspendLayout()
        CType(Me.GbxSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GbxSelection.SuspendLayout()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnCreation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.LblType)
        Me.RadPanel1.Controls.Add(Me.lblLabelType)
        Me.RadPanel1.Controls.Add(Me.LblSpecialiteFiltre)
        Me.RadPanel1.Controls.Add(Me.LblLabelSpecialite)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(1097, 48)
        Me.RadPanel1.TabIndex = 0
        '
        'LblType
        '
        Me.LblType.AutoSize = True
        Me.LblType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblType.Location = New System.Drawing.Point(56, 19)
        Me.LblType.Name = "LblType"
        Me.LblType.Size = New System.Drawing.Size(67, 13)
        Me.LblType.TabIndex = 3
        Me.LblType.Text = "Intervenant"
        '
        'lblLabelType
        '
        Me.lblLabelType.AutoSize = True
        Me.lblLabelType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblLabelType.Location = New System.Drawing.Point(12, 19)
        Me.lblLabelType.Name = "lblLabelType"
        Me.lblLabelType.Size = New System.Drawing.Size(38, 13)
        Me.lblLabelType.TabIndex = 2
        Me.lblLabelType.Text = "Type :"
        '
        'LblSpecialiteFiltre
        '
        Me.LblSpecialiteFiltre.AutoSize = True
        Me.LblSpecialiteFiltre.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblSpecialiteFiltre.Location = New System.Drawing.Point(218, 19)
        Me.LblSpecialiteFiltre.Name = "LblSpecialiteFiltre"
        Me.LblSpecialiteFiltre.Size = New System.Drawing.Size(52, 13)
        Me.LblSpecialiteFiltre.TabIndex = 1
        Me.LblSpecialiteFiltre.Text = "Médecin"
        '
        'LblLabelSpecialite
        '
        Me.LblLabelSpecialite.AutoSize = True
        Me.LblLabelSpecialite.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblLabelSpecialite.Location = New System.Drawing.Point(150, 19)
        Me.LblLabelSpecialite.Name = "LblLabelSpecialite"
        Me.LblLabelSpecialite.Size = New System.Drawing.Size(62, 13)
        Me.LblLabelSpecialite.TabIndex = 0
        Me.LblLabelSpecialite.Text = "Spécialité :"
        '
        'RadPanel2
        '
        Me.RadPanel2.Controls.Add(Me.RadGridView1)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel2.Location = New System.Drawing.Point(0, 48)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(1097, 503)
        Me.RadPanel2.TabIndex = 1
        '
        'RadGridView1
        '
        Me.RadGridView1.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.RadGridView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadGridView1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridView1.ForeColor = System.Drawing.Color.Black
        Me.RadGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridView1.Location = New System.Drawing.Point(0, 0)
        '
        '
        '
        Me.RadGridView1.MasterTemplate.AllowAddNewRow = False
        Me.RadGridView1.MasterTemplate.AllowCellContextMenu = False
        Me.RadGridView1.MasterTemplate.AllowColumnReorder = False
        Me.RadGridView1.MasterTemplate.AllowDeleteRow = False
        Me.RadGridView1.MasterTemplate.AllowEditRow = False
        GridViewDecimalColumn1.DataType = GetType(Long)
        GridViewDecimalColumn1.EnableExpressionEditor = False
        GridViewDecimalColumn1.FieldName = "oa_ror_id"
        GridViewDecimalColumn1.HeaderText = "oa_ror_id"
        GridViewDecimalColumn1.IsAutoGenerated = True
        GridViewDecimalColumn1.IsVisible = False
        GridViewDecimalColumn1.Name = "oa_ror_id"
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "oa_ror_nom"
        GridViewTextBoxColumn1.HeaderText = "Nom"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.IsAutoGenerated = True
        GridViewTextBoxColumn1.Name = "oa_ror_nom"
        GridViewTextBoxColumn1.Width = 250
        GridViewDecimalColumn2.DataType = GetType(Long)
        GridViewDecimalColumn2.EnableExpressionEditor = False
        GridViewDecimalColumn2.FieldName = "oa_ror_specialite_id"
        GridViewDecimalColumn2.HeaderText = "Spécialité"
        GridViewDecimalColumn2.IsAutoGenerated = True
        GridViewDecimalColumn2.IsVisible = False
        GridViewDecimalColumn2.Name = "oa_ror_specialite_id"
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "oa_r_specialite_description"
        GridViewTextBoxColumn2.HeaderText = "Spécialité"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.IsAutoGenerated = True
        GridViewTextBoxColumn2.Name = "oa_r_specialite_description"
        GridViewTextBoxColumn2.Width = 200
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "oa_ror_Type"
        GridViewTextBoxColumn3.HeaderText = "Nature"
        GridViewTextBoxColumn3.IsAutoGenerated = True
        GridViewTextBoxColumn3.Name = "oa_ror_Type"
        GridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn3.Width = 100
        GridViewDecimalColumn3.AllowFiltering = False
        GridViewDecimalColumn3.DataType = GetType(Long)
        GridViewDecimalColumn3.EnableExpressionEditor = False
        GridViewDecimalColumn3.FieldName = "oa_ror_Structure_id"
        GridViewDecimalColumn3.HeaderText = "Id. Structure"
        GridViewDecimalColumn3.IsAutoGenerated = True
        GridViewDecimalColumn3.IsVisible = False
        GridViewDecimalColumn3.Name = "oa_ror_Structure_id"
        GridViewDecimalColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn3.Width = 70
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "oa_ror_adresse1"
        GridViewTextBoxColumn4.HeaderText = "Adresse"
        GridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn4.IsAutoGenerated = True
        GridViewTextBoxColumn4.Name = "oa_ror_adresse1"
        GridViewTextBoxColumn4.Width = 250
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.FieldName = "oa_ror_code_postal"
        GridViewTextBoxColumn5.HeaderText = "Code postal"
        GridViewTextBoxColumn5.IsAutoGenerated = True
        GridViewTextBoxColumn5.Name = "oa_ror_code_postal"
        GridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn5.Width = 75
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.FieldName = "oa_ror_ville"
        GridViewTextBoxColumn6.HeaderText = "Ville"
        GridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn6.IsAutoGenerated = True
        GridViewTextBoxColumn6.Name = "oa_ror_ville"
        GridViewTextBoxColumn6.Width = 200
        Me.RadGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn1, GridViewTextBoxColumn1, GridViewDecimalColumn2, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewDecimalColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6})
        Me.RadGridView1.MasterTemplate.DataSource = Me.VrorBindingSource
        Me.RadGridView1.MasterTemplate.EnableFiltering = True
        Me.RadGridView1.MasterTemplate.EnableGrouping = False
        Me.RadGridView1.MasterTemplate.ShowRowHeaderColumn = False
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.ReadOnly = True
        Me.RadGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridView1.ShowGroupPanel = False
        Me.RadGridView1.Size = New System.Drawing.Size(1097, 503)
        Me.RadGridView1.TabIndex = 0
        '
        'VrorBindingSource
        '
        Me.VrorBindingSource.DataMember = "v_ror"
        Me.VrorBindingSource.DataSource = Me.RORDS
        '
        'RORDS
        '
        Me.RORDS.DataSetName = "RORDS"
        Me.RORDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'RadPanel3
        '
        Me.RadPanel3.Controls.Add(Me.GbxSelection)
        Me.RadPanel3.Controls.Add(Me.RadBtnCreation)
        Me.RadPanel3.Controls.Add(Me.RadBtnAbandon)
        Me.RadPanel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel3.Location = New System.Drawing.Point(0, 464)
        Me.RadPanel3.Name = "RadPanel3"
        Me.RadPanel3.Size = New System.Drawing.Size(1097, 87)
        Me.RadPanel3.TabIndex = 2
        '
        'GbxSelection
        '
        Me.GbxSelection.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.GbxSelection.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.GbxSelection.Controls.Add(Me.LblSpecialite)
        Me.GbxSelection.Controls.Add(Me.LblNom)
        Me.GbxSelection.Controls.Add(Me.RadBtnSelection)
        Me.GbxSelection.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.GbxSelection.HeaderText = "Professionnel de santé sélectionné"
        Me.GbxSelection.Location = New System.Drawing.Point(3, 3)
        Me.GbxSelection.Name = "GbxSelection"
        Me.GbxSelection.Size = New System.Drawing.Size(632, 81)
        Me.GbxSelection.TabIndex = 2
        Me.GbxSelection.Text = "Professionnel de santé sélectionné"
        '
        'LblSpecialite
        '
        Me.LblSpecialite.AutoSize = True
        Me.LblSpecialite.Location = New System.Drawing.Point(9, 52)
        Me.LblSpecialite.MaximumSize = New System.Drawing.Size(450, 13)
        Me.LblSpecialite.Name = "LblSpecialite"
        Me.LblSpecialite.Size = New System.Drawing.Size(40, 13)
        Me.LblSpecialite.TabIndex = 2
        Me.LblSpecialite.Text = "Label2"
        '
        'LblNom
        '
        Me.LblNom.AutoSize = True
        Me.LblNom.Location = New System.Drawing.Point(9, 31)
        Me.LblNom.MaximumSize = New System.Drawing.Size(450, 13)
        Me.LblNom.Name = "LblNom"
        Me.LblNom.Size = New System.Drawing.Size(427, 13)
        Me.LblNom.TabIndex = 1
        Me.LblNom.Text = "1234567890123456789012345678901234567890123456789012345678901234567890"
        '
        'RadBtnSelection
        '
        Me.RadBtnSelection.Location = New System.Drawing.Point(517, 48)
        Me.RadBtnSelection.Name = "RadBtnSelection"
        Me.RadBtnSelection.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnSelection.TabIndex = 0
        Me.RadBtnSelection.Text = "Sélection"
        '
        'RadBtnCreation
        '
        Me.RadBtnCreation.Location = New System.Drawing.Point(816, 51)
        Me.RadBtnCreation.Name = "RadBtnCreation"
        Me.RadBtnCreation.Size = New System.Drawing.Size(151, 24)
        Me.RadBtnCreation.TabIndex = 1
        Me.RadBtnCreation.Text = "Création nouvel intervenant"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(973, 51)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 0
        Me.RadBtnAbandon.Text = "Abandonner"
        '
        'V_rorTableAdapter
        '
        Me.V_rorTableAdapter.ClearBeforeFill = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréationNouvelIntervenantToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(222, 26)
        '
        'CréationNouvelIntervenantToolStripMenuItem
        '
        Me.CréationNouvelIntervenantToolStripMenuItem.Name = "CréationNouvelIntervenantToolStripMenuItem"
        Me.CréationNouvelIntervenantToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.CréationNouvelIntervenantToolStripMenuItem.Text = "Création nouvel intervenant"
        '
        'RadFRorListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1097, 551)
        Me.Controls.Add(Me.RadPanel3)
        Me.Controls.Add(Me.RadPanel2)
        Me.Controls.Add(Me.RadPanel1)
        Me.Name = "RadFRorListe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.Text = "ROR Oasis"
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel2.ResumeLayout(False)
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VrorBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RORDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel3.ResumeLayout(False)
        CType(Me.GbxSelection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GbxSelection.ResumeLayout(False)
        Me.GbxSelection.PerformLayout()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnCreation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel3 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGridView1 As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnCreation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RORDS As RORDS
    Friend WithEvents VrorBindingSource As BindingSource
    Friend WithEvents V_rorTableAdapter As RORDSTableAdapters.v_rorTableAdapter
    Friend WithEvents GbxSelection As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadBtnSelection As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblSpecialite As Label
    Friend WithEvents LblNom As Label
    Friend WithEvents LblSpecialiteFiltre As Label
    Friend WithEvents LblLabelSpecialite As Label
    Friend WithEvents LblType As Label
    Friend WithEvents lblLabelType As Label
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents CréationNouvelIntervenantToolStripMenuItem As ToolStripMenuItem
End Class

