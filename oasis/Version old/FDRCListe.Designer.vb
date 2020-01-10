<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FDRCListe
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DRCDataGridView = New System.Windows.Forms.DataGridView()
        Me.drcId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.drcDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.categorieMajeure = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.drcCategorie = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.drcOasis = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DRCMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUneDRCORCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnCreationORC = New System.Windows.Forms.Button()
        Me.DrcDSynonymeDataGridView = New System.Windows.Forms.DataGridView()
        Me.oa_drc_synonyme_id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_drc_synonyme_libelle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TxtFiltreDescription = New System.Windows.Forms.TextBox()
        Me.ChkORC = New System.Windows.Forms.CheckBox()
        Me.CbxFiltreCategorieMajeure = New System.Windows.Forms.ComboBox()
        Me.CbxFiltreCategorieOasis = New System.Windows.Forms.ComboBox()
        Me.BtnFiltrer = New System.Windows.Forms.Button()
        Me.BtnInitFiltre = New System.Windows.Forms.Button()
        Me.BtnCreerSynonyme = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.DRCDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DRCMenuStrip.SuspendLayout()
        CType(Me.DrcDSynonymeDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DRCDataGridView
        '
        Me.DRCDataGridView.AllowUserToAddRows = False
        Me.DRCDataGridView.AllowUserToDeleteRows = False
        Me.DRCDataGridView.AllowUserToResizeRows = False
        Me.DRCDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DRCDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.drcId, Me.drcDescription, Me.categorieMajeure, Me.drcCategorie, Me.drcOasis})
        Me.DRCDataGridView.ContextMenuStrip = Me.DRCMenuStrip
        Me.DRCDataGridView.Location = New System.Drawing.Point(12, 78)
        Me.DRCDataGridView.Name = "DRCDataGridView"
        Me.DRCDataGridView.ReadOnly = True
        Me.DRCDataGridView.RowHeadersVisible = False
        Me.DRCDataGridView.Size = New System.Drawing.Size(910, 439)
        Me.DRCDataGridView.TabIndex = 17
        '
        'drcId
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.drcId.DefaultCellStyle = DataGridViewCellStyle1
        Me.drcId.HeaderText = "DRC Id"
        Me.drcId.Name = "drcId"
        Me.drcId.ReadOnly = True
        Me.drcId.Width = 60
        '
        'drcDescription
        '
        Me.drcDescription.HeaderText = "Description"
        Me.drcDescription.Name = "drcDescription"
        Me.drcDescription.ReadOnly = True
        Me.drcDescription.Width = 300
        '
        'categorieMajeure
        '
        Me.categorieMajeure.HeaderText = "Catégorie majeure"
        Me.categorieMajeure.Name = "categorieMajeure"
        Me.categorieMajeure.ReadOnly = True
        Me.categorieMajeure.Width = 300
        '
        'drcCategorie
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Format = "d"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.drcCategorie.DefaultCellStyle = DataGridViewCellStyle2
        Me.drcCategorie.HeaderText = "Catégorie DRC/ORC"
        Me.drcCategorie.Name = "drcCategorie"
        Me.drcCategorie.ReadOnly = True
        Me.drcCategorie.Width = 140
        '
        'drcOasis
        '
        Me.drcOasis.HeaderText = "DRC Oasis"
        Me.drcOasis.Name = "drcOasis"
        Me.drcOasis.ReadOnly = True
        Me.drcOasis.Width = 80
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
        'BtnCreationORC
        '
        Me.BtnCreationORC.Location = New System.Drawing.Point(12, 532)
        Me.BtnCreationORC.Name = "BtnCreationORC"
        Me.BtnCreationORC.Size = New System.Drawing.Size(139, 23)
        Me.BtnCreationORC.TabIndex = 18
        Me.BtnCreationORC.Text = "Création ORC"
        Me.ToolTip1.SetToolTip(Me.BtnCreationORC, "DRC (Dictionnaire des Résultats de Consultation)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Une ORC est une DRC propre au s" &
        "ystème Oasis")
        Me.BtnCreationORC.UseVisualStyleBackColor = True
        '
        'DrcDSynonymeDataGridView
        '
        Me.DrcDSynonymeDataGridView.AllowUserToAddRows = False
        Me.DrcDSynonymeDataGridView.AllowUserToDeleteRows = False
        Me.DrcDSynonymeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DrcDSynonymeDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.oa_drc_synonyme_id, Me.oa_drc_synonyme_libelle})
        Me.DrcDSynonymeDataGridView.Location = New System.Drawing.Point(928, 78)
        Me.DrcDSynonymeDataGridView.Name = "DrcDSynonymeDataGridView"
        Me.DrcDSynonymeDataGridView.ReadOnly = True
        Me.DrcDSynonymeDataGridView.RowHeadersVisible = False
        Me.DrcDSynonymeDataGridView.Size = New System.Drawing.Size(411, 439)
        Me.DrcDSynonymeDataGridView.TabIndex = 19
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
        'TxtFiltreDescription
        '
        Me.TxtFiltreDescription.Location = New System.Drawing.Point(71, 45)
        Me.TxtFiltreDescription.Name = "TxtFiltreDescription"
        Me.TxtFiltreDescription.Size = New System.Drawing.Size(277, 20)
        Me.TxtFiltreDescription.TabIndex = 20
        '
        'ChkORC
        '
        Me.ChkORC.AutoSize = True
        Me.ChkORC.Location = New System.Drawing.Point(814, 48)
        Me.ChkORC.Name = "ChkORC"
        Me.ChkORC.Size = New System.Drawing.Size(84, 17)
        Me.ChkORC.TabIndex = 22
        Me.ChkORC.Text = "ORC (Oasis)"
        Me.ChkORC.UseVisualStyleBackColor = True
        '
        'CbxFiltreCategorieMajeure
        '
        Me.CbxFiltreCategorieMajeure.FormattingEnabled = True
        Me.CbxFiltreCategorieMajeure.Location = New System.Drawing.Point(376, 45)
        Me.CbxFiltreCategorieMajeure.Name = "CbxFiltreCategorieMajeure"
        Me.CbxFiltreCategorieMajeure.Size = New System.Drawing.Size(242, 21)
        Me.CbxFiltreCategorieMajeure.TabIndex = 23
        '
        'CbxFiltreCategorieOasis
        '
        Me.CbxFiltreCategorieOasis.FormattingEnabled = True
        Me.CbxFiltreCategorieOasis.Location = New System.Drawing.Point(672, 46)
        Me.CbxFiltreCategorieOasis.Name = "CbxFiltreCategorieOasis"
        Me.CbxFiltreCategorieOasis.Size = New System.Drawing.Size(121, 21)
        Me.CbxFiltreCategorieOasis.TabIndex = 24
        '
        'BtnFiltrer
        '
        Me.BtnFiltrer.Location = New System.Drawing.Point(766, 12)
        Me.BtnFiltrer.Name = "BtnFiltrer"
        Me.BtnFiltrer.Size = New System.Drawing.Size(75, 23)
        Me.BtnFiltrer.TabIndex = 25
        Me.BtnFiltrer.Text = "Filtrer"
        Me.BtnFiltrer.UseVisualStyleBackColor = True
        '
        'BtnInitFiltre
        '
        Me.BtnInitFiltre.Location = New System.Drawing.Point(847, 12)
        Me.BtnInitFiltre.Name = "BtnInitFiltre"
        Me.BtnInitFiltre.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitFiltre.TabIndex = 26
        Me.BtnInitFiltre.Text = "Réinitailiser"
        Me.BtnInitFiltre.UseVisualStyleBackColor = True
        '
        'BtnCreerSynonyme
        '
        Me.BtnCreerSynonyme.Location = New System.Drawing.Point(928, 532)
        Me.BtnCreerSynonyme.Name = "BtnCreerSynonyme"
        Me.BtnCreerSynonyme.Size = New System.Drawing.Size(114, 23)
        Me.BtnCreerSynonyme.TabIndex = 27
        Me.BtnCreerSynonyme.Text = "Créer un synonyme"
        Me.BtnCreerSynonyme.UseVisualStyleBackColor = True
        '
        'FDRCListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1353, 567)
        Me.Controls.Add(Me.BtnCreerSynonyme)
        Me.Controls.Add(Me.BtnInitFiltre)
        Me.Controls.Add(Me.BtnFiltrer)
        Me.Controls.Add(Me.CbxFiltreCategorieOasis)
        Me.Controls.Add(Me.CbxFiltreCategorieMajeure)
        Me.Controls.Add(Me.ChkORC)
        Me.Controls.Add(Me.TxtFiltreDescription)
        Me.Controls.Add(Me.DrcDSynonymeDataGridView)
        Me.Controls.Add(Me.BtnCreationORC)
        Me.Controls.Add(Me.DRCDataGridView)
        Me.Name = "FDRCListe"
        Me.Text = "Gestion des DRC/ORC"
        Me.ToolTip1.SetToolTip(Me, "DRC (Dictionnaire des Résultats de Consultation)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Une ORC est une DRC propre au s" &
        "ystème Oasis")
        CType(Me.DRCDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DRCMenuStrip.ResumeLayout(False)
        CType(Me.DrcDSynonymeDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DRCDataGridView As DataGridView
    Friend WithEvents BtnCreationORC As Button
    Friend WithEvents DrcDSynonymeDataGridView As DataGridView
    Friend WithEvents TxtFiltreDescription As TextBox
    Friend WithEvents ChkORC As CheckBox
    Friend WithEvents CbxFiltreCategorieMajeure As ComboBox
    Friend WithEvents CbxFiltreCategorieOasis As ComboBox
    Friend WithEvents BtnFiltrer As Button
    Friend WithEvents BtnInitFiltre As Button
    Friend WithEvents DRCMenuStrip As ContextMenuStrip
    Friend WithEvents CréerUneDRCORCToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnCreerSynonyme As Button
    Friend WithEvents oa_drc_synonyme_id As DataGridViewTextBoxColumn
    Friend WithEvents oa_drc_synonyme_libelle As DataGridViewTextBoxColumn
    Friend WithEvents drcId As DataGridViewTextBoxColumn
    Friend WithEvents drcDescription As DataGridViewTextBoxColumn
    Friend WithEvents categorieMajeure As DataGridViewTextBoxColumn
    Friend WithEvents drcCategorie As DataGridViewTextBoxColumn
    Friend WithEvents drcOasis As DataGridViewTextBoxColumn
    Friend WithEvents ToolTip1 As ToolTip
End Class
