<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FmDRCListe
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.BtnCreerSynonyme = New System.Windows.Forms.Button()
        Me.BtnInitFiltre = New System.Windows.Forms.Button()
        Me.BtnFiltrer = New System.Windows.Forms.Button()
        Me.CbxFiltreCategorieOasis = New System.Windows.Forms.ComboBox()
        Me.CbxFiltreCategorieMajeure = New System.Windows.Forms.ComboBox()
        Me.ChkORC = New System.Windows.Forms.CheckBox()
        Me.TxtFiltreDescription = New System.Windows.Forms.TextBox()
        Me.DrcDSynonymeDataGridView = New System.Windows.Forms.DataGridView()
        Me.oa_drc_synonyme_id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_drc_synonyme_libelle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnCreationORC = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DRCMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUneDRCORCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RadDRCDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.RadGridView2 = New Telerik.WinControls.UI.RadGridView()
        CType(Me.DrcDSynonymeDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DRCMenuStrip.SuspendLayout()
        CType(Me.RadDRCDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadDRCDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView2.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnCreerSynonyme
        '
        Me.BtnCreerSynonyme.Location = New System.Drawing.Point(926, 530)
        Me.BtnCreerSynonyme.Name = "BtnCreerSynonyme"
        Me.BtnCreerSynonyme.Size = New System.Drawing.Size(114, 23)
        Me.BtnCreerSynonyme.TabIndex = 37
        Me.BtnCreerSynonyme.Text = "Créer un synonyme"
        Me.BtnCreerSynonyme.UseVisualStyleBackColor = True
        '
        'BtnInitFiltre
        '
        Me.BtnInitFiltre.Location = New System.Drawing.Point(845, 10)
        Me.BtnInitFiltre.Name = "BtnInitFiltre"
        Me.BtnInitFiltre.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitFiltre.TabIndex = 36
        Me.BtnInitFiltre.Text = "Réinitailiser"
        Me.BtnInitFiltre.UseVisualStyleBackColor = True
        '
        'BtnFiltrer
        '
        Me.BtnFiltrer.Location = New System.Drawing.Point(764, 10)
        Me.BtnFiltrer.Name = "BtnFiltrer"
        Me.BtnFiltrer.Size = New System.Drawing.Size(75, 23)
        Me.BtnFiltrer.TabIndex = 35
        Me.BtnFiltrer.Text = "Filtrer"
        Me.BtnFiltrer.UseVisualStyleBackColor = True
        '
        'CbxFiltreCategorieOasis
        '
        Me.CbxFiltreCategorieOasis.FormattingEnabled = True
        Me.CbxFiltreCategorieOasis.Location = New System.Drawing.Point(670, 44)
        Me.CbxFiltreCategorieOasis.Name = "CbxFiltreCategorieOasis"
        Me.CbxFiltreCategorieOasis.Size = New System.Drawing.Size(121, 21)
        Me.CbxFiltreCategorieOasis.TabIndex = 34
        '
        'CbxFiltreCategorieMajeure
        '
        Me.CbxFiltreCategorieMajeure.FormattingEnabled = True
        Me.CbxFiltreCategorieMajeure.Location = New System.Drawing.Point(374, 43)
        Me.CbxFiltreCategorieMajeure.Name = "CbxFiltreCategorieMajeure"
        Me.CbxFiltreCategorieMajeure.Size = New System.Drawing.Size(242, 21)
        Me.CbxFiltreCategorieMajeure.TabIndex = 33
        '
        'ChkORC
        '
        Me.ChkORC.AutoSize = True
        Me.ChkORC.Location = New System.Drawing.Point(812, 46)
        Me.ChkORC.Name = "ChkORC"
        Me.ChkORC.Size = New System.Drawing.Size(84, 17)
        Me.ChkORC.TabIndex = 32
        Me.ChkORC.Text = "ORC (Oasis)"
        Me.ChkORC.UseVisualStyleBackColor = True
        '
        'TxtFiltreDescription
        '
        Me.TxtFiltreDescription.Location = New System.Drawing.Point(69, 43)
        Me.TxtFiltreDescription.Name = "TxtFiltreDescription"
        Me.TxtFiltreDescription.Size = New System.Drawing.Size(277, 20)
        Me.TxtFiltreDescription.TabIndex = 31
        '
        'DrcDSynonymeDataGridView
        '
        Me.DrcDSynonymeDataGridView.AllowUserToAddRows = False
        Me.DrcDSynonymeDataGridView.AllowUserToDeleteRows = False
        Me.DrcDSynonymeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DrcDSynonymeDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.oa_drc_synonyme_id, Me.oa_drc_synonyme_libelle})
        Me.DrcDSynonymeDataGridView.Location = New System.Drawing.Point(952, 76)
        Me.DrcDSynonymeDataGridView.Name = "DrcDSynonymeDataGridView"
        Me.DrcDSynonymeDataGridView.ReadOnly = True
        Me.DrcDSynonymeDataGridView.RowHeadersVisible = False
        Me.DrcDSynonymeDataGridView.Size = New System.Drawing.Size(411, 146)
        Me.DrcDSynonymeDataGridView.TabIndex = 30
        '
        'oa_drc_synonyme_id
        '
        Me.oa_drc_synonyme_id.DataPropertyName = "oa_drc_synonyme_id"
        Me.oa_drc_synonyme_id.HeaderText = "Synonyme Id"
        Me.oa_drc_synonyme_id.Name = "oa_drc_synonyme_id"
        Me.oa_drc_synonyme_id.ReadOnly = True
        Me.oa_drc_synonyme_id.Visible = False
        '
        'oa_drc_synonyme_libelle
        '
        Me.oa_drc_synonyme_libelle.DataPropertyName = "oa_drc_synonyme_libelle"
        Me.oa_drc_synonyme_libelle.HeaderText = "     Synonyme(s)"
        Me.oa_drc_synonyme_libelle.Name = "oa_drc_synonyme_libelle"
        Me.oa_drc_synonyme_libelle.ReadOnly = True
        Me.oa_drc_synonyme_libelle.Width = 400
        '
        'BtnCreationORC
        '
        Me.BtnCreationORC.Location = New System.Drawing.Point(10, 530)
        Me.BtnCreationORC.Name = "BtnCreationORC"
        Me.BtnCreationORC.Size = New System.Drawing.Size(139, 23)
        Me.BtnCreationORC.TabIndex = 29
        Me.BtnCreationORC.Text = "Création ORC"
        Me.BtnCreationORC.UseVisualStyleBackColor = True
        '
        'DRCMenuStrip
        '
        Me.DRCMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerUneDRCORCToolStripMenuItem})
        Me.DRCMenuStrip.Name = "ContextMenuStrip1"
        Me.DRCMenuStrip.Size = New System.Drawing.Size(198, 26)
        '
        'CréerUneDRCORCToolStripMenuItem
        '
        Me.CréerUneDRCORCToolStripMenuItem.Name = "CréerUneDRCORCToolStripMenuItem"
        Me.CréerUneDRCORCToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
        Me.CréerUneDRCORCToolStripMenuItem.Text = "Modification DRC/ORC"
        '
        'RadDRCDataGridView
        '
        Me.RadDRCDataGridView.BackColor = System.Drawing.SystemColors.Control
        Me.RadDRCDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadDRCDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadDRCDataGridView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadDRCDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadDRCDataGridView.Location = New System.Drawing.Point(12, 76)
        '
        '
        '
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "Id."
        GridViewTextBoxColumn1.Name = "drcId"
        GridViewTextBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.Width = 60
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "Description"
        GridViewTextBoxColumn2.Name = "drcDescription"
        GridViewTextBoxColumn2.ReadOnly = True
        GridViewTextBoxColumn2.Width = 300
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "Catégorie majeure"
        GridViewTextBoxColumn3.Name = "categorieMajeure"
        GridViewTextBoxColumn3.ReadOnly = True
        GridViewTextBoxColumn3.Width = 300
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "Catégorie DRC/ORC"
        GridViewTextBoxColumn4.Name = "drcCategorie"
        GridViewTextBoxColumn4.ReadOnly = True
        GridViewTextBoxColumn4.Width = 140
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.HeaderText = "DRC Oasis"
        GridViewTextBoxColumn5.Name = "drcOasis"
        GridViewTextBoxColumn5.ReadOnly = True
        GridViewTextBoxColumn5.Width = 80
        Me.RadDRCDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5})
        Me.RadDRCDataGridView.MasterTemplate.EnableGrouping = False
        Me.RadDRCDataGridView.MasterTemplate.EnablePaging = True
        Me.RadDRCDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadDRCDataGridView.Name = "RadDRCDataGridView"
        Me.RadDRCDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        '
        '
        '
        Me.RadDRCDataGridView.RootElement.AutoSize = False
        Me.RadDRCDataGridView.Size = New System.Drawing.Size(922, 433)
        Me.RadDRCDataGridView.TabIndex = 38
        '
        'RadGridView2
        '
        Me.RadGridView2.Location = New System.Drawing.Point(952, 241)
        '
        '
        '
        Me.RadGridView2.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridView2.Name = "RadGridView2"
        Me.RadGridView2.Size = New System.Drawing.Size(411, 268)
        Me.RadGridView2.TabIndex = 39
        '
        'FmDRCListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1375, 561)
        Me.Controls.Add(Me.RadGridView2)
        Me.Controls.Add(Me.RadDRCDataGridView)
        Me.Controls.Add(Me.BtnCreerSynonyme)
        Me.Controls.Add(Me.BtnInitFiltre)
        Me.Controls.Add(Me.BtnFiltrer)
        Me.Controls.Add(Me.CbxFiltreCategorieOasis)
        Me.Controls.Add(Me.CbxFiltreCategorieMajeure)
        Me.Controls.Add(Me.ChkORC)
        Me.Controls.Add(Me.TxtFiltreDescription)
        Me.Controls.Add(Me.DrcDSynonymeDataGridView)
        Me.Controls.Add(Me.BtnCreationORC)
        Me.Name = "FmDRCListe"
        Me.Text = "FmDRCListe"
        CType(Me.DrcDSynonymeDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DRCMenuStrip.ResumeLayout(False)
        CType(Me.RadDRCDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadDRCDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView2.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnCreerSynonyme As Button
    Friend WithEvents BtnInitFiltre As Button
    Friend WithEvents BtnFiltrer As Button
    Friend WithEvents CbxFiltreCategorieOasis As ComboBox
    Friend WithEvents CbxFiltreCategorieMajeure As ComboBox
    Friend WithEvents ChkORC As CheckBox
    Friend WithEvents TxtFiltreDescription As TextBox
    Friend WithEvents DrcDSynonymeDataGridView As DataGridView
    Friend WithEvents oa_drc_synonyme_id As DataGridViewTextBoxColumn
    Friend WithEvents oa_drc_synonyme_libelle As DataGridViewTextBoxColumn
    Friend WithEvents BtnCreationORC As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents DRCMenuStrip As ContextMenuStrip
    Friend WithEvents CréerUneDRCORCToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RadDRCDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadGridView2 As Telerik.WinControls.UI.RadGridView
End Class
