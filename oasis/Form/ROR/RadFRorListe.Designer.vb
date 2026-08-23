<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RadFRorListe
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim GridViewTextBoxColumn41 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn42 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn43 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn44 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn45 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn46 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn47 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn48 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn49 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn50 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition5 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadFRorListe))
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.LblType = New System.Windows.Forms.Label()
        Me.lblLabelType = New System.Windows.Forms.Label()
        Me.LblSpecialiteFiltre = New System.Windows.Forms.Label()
        Me.LblLabelSpecialite = New System.Windows.Forms.Label()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.GbxSelection = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblVille = New System.Windows.Forms.Label()
        Me.LblAdresse = New System.Windows.Forms.Label()
        Me.LblStructure = New System.Windows.Forms.Label()
        Me.LblSpecialite = New System.Windows.Forms.Label()
        Me.LblNom = New System.Windows.Forms.Label()
        Me.RadBtnSelection = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAnnuaireProf = New Telerik.WinControls.UI.RadButton()
        Me.RadGridViewRor = New Telerik.WinControls.UI.RadGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréationNouvelIntervenantToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModificationIntervenantToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RadBtnCreation = New Telerik.WinControls.UI.RadButton()
        Me.VrorBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.RORDS = New Oasis_WF.RORDS()
        Me.RadPanel3 = New Telerik.WinControls.UI.RadPanel()
        Me.RadBtnModification = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.V_rorTableAdapter = New Oasis_WF.RORDSTableAdapters.v_rorTableAdapter()
        Me.RadBtnDetail = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me.GbxSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GbxSelection.SuspendLayout()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAnnuaireProf, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewRor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewRor.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.RadBtnCreation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VrorBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RORDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel3.SuspendLayout()
        CType(Me.RadBtnModification, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnDetail, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.RadPanel1.Size = New System.Drawing.Size(1806, 48)
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
        Me.RadPanel2.Controls.Add(Me.GbxSelection)
        Me.RadPanel2.Controls.Add(Me.RadBtnAnnuaireProf)
        Me.RadPanel2.Controls.Add(Me.RadGridViewRor)
        Me.RadPanel2.Controls.Add(Me.RadBtnCreation)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel2.Location = New System.Drawing.Point(0, 48)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(1806, 485)
        Me.RadPanel2.TabIndex = 1
        '
        'GbxSelection
        '
        Me.GbxSelection.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.GbxSelection.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.GbxSelection.Controls.Add(Me.RadBtnDetail)
        Me.GbxSelection.Controls.Add(Me.LblVille)
        Me.GbxSelection.Controls.Add(Me.LblAdresse)
        Me.GbxSelection.Controls.Add(Me.LblStructure)
        Me.GbxSelection.Controls.Add(Me.LblSpecialite)
        Me.GbxSelection.Controls.Add(Me.LblNom)
        Me.GbxSelection.Controls.Add(Me.RadBtnSelection)
        Me.GbxSelection.Dock = System.Windows.Forms.DockStyle.Right
        Me.GbxSelection.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.GbxSelection.HeaderText = "Professionnel de santé sélectionné"
        Me.GbxSelection.Location = New System.Drawing.Point(1350, 0)
        Me.GbxSelection.Name = "GbxSelection"
        Me.GbxSelection.Size = New System.Drawing.Size(456, 485)
        Me.GbxSelection.TabIndex = 2
        Me.GbxSelection.Text = "Professionnel de santé sélectionné"
        '
        'LblVille
        '
        Me.LblVille.AutoSize = True
        Me.LblVille.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblVille.Location = New System.Drawing.Point(10, 131)
        Me.LblVille.Name = "LblVille"
        Me.LblVille.Size = New System.Drawing.Size(106, 13)
        Me.LblVille.TabIndex = 5
        Me.LblVille.Text = "Code postal et ville"
        '
        'LblAdresse
        '
        Me.LblAdresse.AutoSize = True
        Me.LblAdresse.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblAdresse.Location = New System.Drawing.Point(10, 106)
        Me.LblAdresse.Name = "LblAdresse"
        Me.LblAdresse.Size = New System.Drawing.Size(48, 13)
        Me.LblAdresse.TabIndex = 4
        Me.LblAdresse.Text = "Adresse"
        '
        'LblStructure
        '
        Me.LblStructure.AutoSize = True
        Me.LblStructure.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblStructure.Location = New System.Drawing.Point(9, 81)
        Me.LblStructure.Name = "LblStructure"
        Me.LblStructure.Size = New System.Drawing.Size(54, 13)
        Me.LblStructure.TabIndex = 3
        Me.LblStructure.Text = "Structure"
        '
        'LblSpecialite
        '
        Me.LblSpecialite.AutoSize = True
        Me.LblSpecialite.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblSpecialite.Location = New System.Drawing.Point(9, 56)
        Me.LblSpecialite.MaximumSize = New System.Drawing.Size(450, 13)
        Me.LblSpecialite.Name = "LblSpecialite"
        Me.LblSpecialite.Size = New System.Drawing.Size(56, 13)
        Me.LblSpecialite.TabIndex = 2
        Me.LblSpecialite.Text = "Spécialité"
        '
        'LblNom
        '
        Me.LblNom.AutoSize = True
        Me.LblNom.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblNom.Location = New System.Drawing.Point(9, 31)
        Me.LblNom.MaximumSize = New System.Drawing.Size(450, 13)
        Me.LblNom.Name = "LblNom"
        Me.LblNom.Size = New System.Drawing.Size(33, 13)
        Me.LblNom.TabIndex = 1
        Me.LblNom.Text = "Nom"
        '
        'RadBtnSelection
        '
        Me.RadBtnSelection.Image = Global.Oasis_WF.My.Resources.Resources._select
        Me.RadBtnSelection.Location = New System.Drawing.Point(12, 285)
        Me.RadBtnSelection.Name = "RadBtnSelection"
        Me.RadBtnSelection.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnSelection.TabIndex = 0
        Me.RadBtnSelection.Text = "Sélection"
        '
        'RadBtnAnnuaireProf
        '
        Me.RadBtnAnnuaireProf.Location = New System.Drawing.Point(3, 17)
        Me.RadBtnAnnuaireProf.Name = "RadBtnAnnuaireProf"
        Me.RadBtnAnnuaireProf.Size = New System.Drawing.Size(251, 24)
        Me.RadBtnAnnuaireProf.TabIndex = 2
        Me.RadBtnAnnuaireProf.Text = "Répertoire national des professionnels de santé" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'RadGridViewRor
        '
        Me.RadGridViewRor.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewRor.ContextMenuStrip = Me.ContextMenuStrip1
        Me.RadGridViewRor.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewRor.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewRor.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewRor.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewRor.Location = New System.Drawing.Point(0, 47)
        '
        '
        '
        Me.RadGridViewRor.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewRor.MasterTemplate.AllowCellContextMenu = False
        Me.RadGridViewRor.MasterTemplate.AllowColumnReorder = False
        Me.RadGridViewRor.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewRor.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn41.EnableExpressionEditor = False
        GridViewTextBoxColumn41.HeaderText = "Nom"
        GridViewTextBoxColumn41.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn41.Name = "nom"
        GridViewTextBoxColumn41.Width = 200
        GridViewTextBoxColumn42.EnableExpressionEditor = False
        GridViewTextBoxColumn42.HeaderText = "Spécialité"
        GridViewTextBoxColumn42.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn42.Name = "specialiteDescription"
        GridViewTextBoxColumn42.Width = 200
        GridViewTextBoxColumn43.EnableExpressionEditor = False
        GridViewTextBoxColumn43.HeaderText = "Nature"
        GridViewTextBoxColumn43.Name = "type"
        GridViewTextBoxColumn43.Width = 75
        GridViewTextBoxColumn44.EnableExpressionEditor = False
        GridViewTextBoxColumn44.HeaderText = "Structure"
        GridViewTextBoxColumn44.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn44.Name = "structure"
        GridViewTextBoxColumn44.Width = 300
        GridViewTextBoxColumn45.EnableExpressionEditor = False
        GridViewTextBoxColumn45.HeaderText = "Adresse"
        GridViewTextBoxColumn45.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn45.Name = "adresse"
        GridViewTextBoxColumn45.Width = 300
        GridViewTextBoxColumn46.EnableExpressionEditor = False
        GridViewTextBoxColumn46.HeaderText = "Code postal"
        GridViewTextBoxColumn46.Name = "codePostal"
        GridViewTextBoxColumn46.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn46.Width = 70
        GridViewTextBoxColumn47.EnableExpressionEditor = False
        GridViewTextBoxColumn47.HeaderText = "Ville"
        GridViewTextBoxColumn47.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn47.Name = "ville"
        GridViewTextBoxColumn47.Width = 180
        GridViewTextBoxColumn48.EnableExpressionEditor = False
        GridViewTextBoxColumn48.HeaderText = "cleAnnuaire"
        GridViewTextBoxColumn48.IsVisible = False
        GridViewTextBoxColumn48.Name = "cleAnnuaire"
        GridViewTextBoxColumn49.EnableExpressionEditor = False
        GridViewTextBoxColumn49.HeaderText = "ror id"
        GridViewTextBoxColumn49.IsVisible = False
        GridViewTextBoxColumn49.Name = "rorId"
        GridViewTextBoxColumn50.EnableExpressionEditor = False
        GridViewTextBoxColumn50.HeaderText = "spécialite id"
        GridViewTextBoxColumn50.IsVisible = False
        GridViewTextBoxColumn50.Name = "specialiteId"
        Me.RadGridViewRor.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn41, GridViewTextBoxColumn42, GridViewTextBoxColumn43, GridViewTextBoxColumn44, GridViewTextBoxColumn45, GridViewTextBoxColumn46, GridViewTextBoxColumn47, GridViewTextBoxColumn48, GridViewTextBoxColumn49, GridViewTextBoxColumn50})
        Me.RadGridViewRor.MasterTemplate.EnableFiltering = True
        Me.RadGridViewRor.MasterTemplate.EnableGrouping = False
        Me.RadGridViewRor.MasterTemplate.ShowRowHeaderColumn = False
        Me.RadGridViewRor.MasterTemplate.ViewDefinition = TableViewDefinition5
        Me.RadGridViewRor.Name = "RadGridViewRor"
        Me.RadGridViewRor.ReadOnly = True
        Me.RadGridViewRor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewRor.ShowGroupPanel = False
        Me.RadGridViewRor.Size = New System.Drawing.Size(1338, 456)
        Me.RadGridViewRor.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréationNouvelIntervenantToolStripMenuItem, Me.ModificationIntervenantToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(222, 48)
        '
        'CréationNouvelIntervenantToolStripMenuItem
        '
        Me.CréationNouvelIntervenantToolStripMenuItem.Name = "CréationNouvelIntervenantToolStripMenuItem"
        Me.CréationNouvelIntervenantToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.CréationNouvelIntervenantToolStripMenuItem.Text = "Création nouvel intervenant"
        '
        'ModificationIntervenantToolStripMenuItem
        '
        Me.ModificationIntervenantToolStripMenuItem.Name = "ModificationIntervenantToolStripMenuItem"
        Me.ModificationIntervenantToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.ModificationIntervenantToolStripMenuItem.Text = "Modification intervenant"
        '
        'RadBtnCreation
        '
        Me.RadBtnCreation.Location = New System.Drawing.Point(260, 17)
        Me.RadBtnCreation.Name = "RadBtnCreation"
        Me.RadBtnCreation.Size = New System.Drawing.Size(169, 24)
        Me.RadBtnCreation.TabIndex = 1
        Me.RadBtnCreation.Text = "+ Création nouvel intervenant"
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
        Me.RadPanel3.Controls.Add(Me.RadBtnModification)
        Me.RadPanel3.Controls.Add(Me.RadBtnAbandon)
        Me.RadPanel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel3.Location = New System.Drawing.Point(0, 485)
        Me.RadPanel3.Name = "RadPanel3"
        Me.RadPanel3.Size = New System.Drawing.Size(1806, 48)
        Me.RadPanel3.TabIndex = 2
        '
        'RadBtnModification
        '
        Me.RadBtnModification.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RadBtnModification.Image = Global.Oasis_WF.My.Resources.Resources.modifier
        Me.RadBtnModification.Location = New System.Drawing.Point(3, 14)
        Me.RadBtnModification.Name = "RadBtnModification"
        Me.RadBtnModification.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnModification.TabIndex = 3
        Me.RadBtnModification.Text = "Modifier"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1776, 14)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 0
        '
        'V_rorTableAdapter
        '
        Me.V_rorTableAdapter.ClearBeforeFill = True
        '
        'RadBtnDetail
        '
        Me.RadBtnDetail.Location = New System.Drawing.Point(128, 285)
        Me.RadBtnDetail.Name = "RadBtnDetail"
        Me.RadBtnDetail.Size = New System.Drawing.Size(168, 24)
        Me.RadBtnDetail.TabIndex = 24
        Me.RadBtnDetail.Text = "Détail professionnel de santé"
        '
        'RadFRorListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1806, 533)
        Me.Controls.Add(Me.RadPanel3)
        Me.Controls.Add(Me.RadPanel2)
        Me.Controls.Add(Me.RadPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RadFRorListe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Référentiel interne des professionnel de santé"
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel2.ResumeLayout(False)
        CType(Me.GbxSelection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GbxSelection.ResumeLayout(False)
        Me.GbxSelection.PerformLayout()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAnnuaireProf, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewRor.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewRor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.RadBtnCreation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VrorBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RORDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel3.ResumeLayout(False)
        CType(Me.RadBtnModification, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel3 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGridViewRor As Telerik.WinControls.UI.RadGridView
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
    Friend WithEvents RadBtnModification As Telerik.WinControls.UI.RadButton
    Friend WithEvents ModificationIntervenantToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RadBtnAnnuaireProf As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblVille As Label
    Friend WithEvents LblAdresse As Label
    Friend WithEvents LblStructure As Label
    Friend WithEvents RadBtnDetail As Telerik.WinControls.UI.RadButton
End Class

