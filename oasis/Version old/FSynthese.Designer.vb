<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FSynthese
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FSynthese))
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblPatientAdresse1 = New System.Windows.Forms.Label()
        Me.LblPatientAdresse2 = New System.Windows.Forms.Label()
        Me.LblPatientCodePostal = New System.Windows.Forms.Label()
        Me.LblPatientVille = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LblPatientTel1 = New System.Windows.Forms.Label()
        Me.LblPatientTel2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.EtatCivilContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemMaps = New System.Windows.Forms.ToolStripMenuItem()
        Me.DétailPatientToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListeDesALDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LblALD = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LblPatientDateMaj = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblPatientUniteSanitaire = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.LblPatientSite = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.AntecedentDataGridView = New System.Windows.Forms.DataGridView()
        Me.antecedent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentDrcId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentNiveau = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentPereId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AntecedentContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerAntecedentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoriqueDesModificationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModifierLordreDunAntécédentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangerLaffectationDunAntecedentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TraitementDataGridView = New System.Windows.Forms.DataGridView()
        Me.medicamentDci = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.posologie = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dateDebut = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DateModification = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TraitementId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnFenetreTherapeutique = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.fenetreTherapeutique = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MedicamentCis = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TraitementContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUnTraitementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TraitementsObsoletesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DéclarerUneAllergieToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoriqueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrdonnanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ParcoursDeSoinDataGridView = New System.Windows.Forms.DataGridView()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.ContexteDataGridView = New System.Windows.Forms.DataGridView()
        Me.categorieContexte = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Contexte = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ContexteId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ContexteMedicalContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoriqueDesModificationsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.PPSDataGridView = New System.Windows.Forms.DataGridView()
        Me.pps = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ppsId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PPSContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.GestionDuPlanPersonnaliséDeSantéToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LblAllergie = New System.Windows.Forms.Label()
        Me.LblContreIndication = New System.Windows.Forms.Label()
        Me.TxtAllergies = New System.Windows.Forms.TextBox()
        Me.ChkPublie = New System.Windows.Forms.CheckBox()
        Me.ChkTous = New System.Windows.Forms.CheckBox()
        Me.ChkParPriorite = New System.Windows.Forms.CheckBox()
        Me.ChkParChronologie = New System.Windows.Forms.CheckBox()
        Me.ChkContextePublie = New System.Windows.Forms.CheckBox()
        Me.ChkContexteTous = New System.Windows.Forms.CheckBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.BtnDirectives = New System.Windows.Forms.Button()
        Me.BtnSocial = New System.Windows.Forms.Button()
        Me.BtnLigneDeVie = New System.Windows.Forms.Button()
        Me.BtnEpisode = New System.Windows.Forms.Button()
        Me.BtnNotesMedicales = New System.Windows.Forms.Button()
        Me.BtnVaccins = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.EtatCivilContextMenuStrip.SuspendLayout()
        CType(Me.AntecedentDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AntecedentContextMenuStrip.SuspendLayout()
        CType(Me.TraitementDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TraitementContextMenuStrip.SuspendLayout()
        CType(Me.ParcoursDeSoinDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ContexteDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContexteMedicalContextMenuStrip.SuspendLayout()
        CType(Me.PPSDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PPSContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(6, 16)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(60, 13)
        Me.LblPatientPrenom.TabIndex = 1
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(128, 16)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(42, 13)
        Me.LblPatientNom.TabIndex = 2
        Me.LblPatientNom.Text = "Durand"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(245, 16)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(39, 13)
        Me.LblPatientAge.TabIndex = 3
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(355, 16)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(49, 13)
        Me.LblPatientGenre.TabIndex = 4
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(556, 16)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 5
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Adresse :"
        '
        'LblPatientAdresse1
        '
        Me.LblPatientAdresse1.AutoSize = True
        Me.LblPatientAdresse1.Location = New System.Drawing.Point(77, 32)
        Me.LblPatientAdresse1.Name = "LblPatientAdresse1"
        Me.LblPatientAdresse1.Size = New System.Drawing.Size(109, 13)
        Me.LblPatientAdresse1.TabIndex = 7
        Me.LblPatientAdresse1.Text = "3 rue de la république"
        '
        'LblPatientAdresse2
        '
        Me.LblPatientAdresse2.AutoSize = True
        Me.LblPatientAdresse2.Location = New System.Drawing.Point(231, 32)
        Me.LblPatientAdresse2.Name = "LblPatientAdresse2"
        Me.LblPatientAdresse2.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientAdresse2.TabIndex = 8
        Me.LblPatientAdresse2.Text = "adresse 2"
        '
        'LblPatientCodePostal
        '
        Me.LblPatientCodePostal.AutoSize = True
        Me.LblPatientCodePostal.Location = New System.Drawing.Point(77, 49)
        Me.LblPatientCodePostal.Name = "LblPatientCodePostal"
        Me.LblPatientCodePostal.Size = New System.Drawing.Size(37, 13)
        Me.LblPatientCodePostal.TabIndex = 9
        Me.LblPatientCodePostal.Text = "71250"
        '
        'LblPatientVille
        '
        Me.LblPatientVille.AutoSize = True
        Me.LblPatientVille.Location = New System.Drawing.Point(120, 49)
        Me.LblPatientVille.Name = "LblPatientVille"
        Me.LblPatientVille.Size = New System.Drawing.Size(52, 13)
        Me.LblPatientVille.TabIndex = 10
        Me.LblPatientVille.Text = "Lournand"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(355, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Tel. :"
        '
        'LblPatientTel1
        '
        Me.LblPatientTel1.AutoSize = True
        Me.LblPatientTel1.Location = New System.Drawing.Point(398, 33)
        Me.LblPatientTel1.Name = "LblPatientTel1"
        Me.LblPatientTel1.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel1.TabIndex = 12
        Me.LblPatientTel1.Text = "0288425678"
        '
        'LblPatientTel2
        '
        Me.LblPatientTel2.AutoSize = True
        Me.LblPatientTel2.Location = New System.Drawing.Point(398, 49)
        Me.LblPatientTel2.Name = "LblPatientTel2"
        Me.LblPatientTel2.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel2.TabIndex = 13
        Me.LblPatientTel2.Text = "0968542357"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox1.ContextMenuStrip = Me.EtatCivilContextMenuStrip
        Me.GroupBox1.Controls.Add(Me.LblALD)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.LblPatientDateMaj)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.LblPatientUniteSanitaire)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.LblPatientSite)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.LblPatientTel2)
        Me.GroupBox1.Controls.Add(Me.LblPatientPrenom)
        Me.GroupBox1.Controls.Add(Me.LblPatientTel1)
        Me.GroupBox1.Controls.Add(Me.LblPatientNom)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.LblPatientAge)
        Me.GroupBox1.Controls.Add(Me.LblPatientVille)
        Me.GroupBox1.Controls.Add(Me.LblPatientGenre)
        Me.GroupBox1.Controls.Add(Me.LblPatientCodePostal)
        Me.GroupBox1.Controls.Add(Me.LblPatientNIR)
        Me.GroupBox1.Controls.Add(Me.LblPatientAdresse2)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.LblPatientAdresse1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(948, 71)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Etat civil"
        '
        'EtatCivilContextMenuStrip
        '
        Me.EtatCivilContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemMaps, Me.DétailPatientToolStripMenuItem, Me.ListeDesALDToolStripMenuItem})
        Me.EtatCivilContextMenuStrip.Name = "EtatCivilContextMenuStrip"
        Me.EtatCivilContextMenuStrip.Size = New System.Drawing.Size(266, 70)
        '
        'ToolStripMenuItemMaps
        '
        Me.ToolStripMenuItemMaps.Name = "ToolStripMenuItemMaps"
        Me.ToolStripMenuItemMaps.Size = New System.Drawing.Size(265, 22)
        Me.ToolStripMenuItemMaps.Text = "Afficher l'adresse dans Google Maps"
        '
        'DétailPatientToolStripMenuItem
        '
        Me.DétailPatientToolStripMenuItem.Name = "DétailPatientToolStripMenuItem"
        Me.DétailPatientToolStripMenuItem.Size = New System.Drawing.Size(265, 22)
        Me.DétailPatientToolStripMenuItem.Text = "Détail patient"
        '
        'ListeDesALDToolStripMenuItem
        '
        Me.ListeDesALDToolStripMenuItem.Name = "ListeDesALDToolStripMenuItem"
        Me.ListeDesALDToolStripMenuItem.Size = New System.Drawing.Size(265, 22)
        Me.ListeDesALDToolStripMenuItem.Text = "Liste des ALD"
        '
        'LblALD
        '
        Me.LblALD.AutoSize = True
        Me.LblALD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblALD.ForeColor = System.Drawing.Color.Red
        Me.LblALD.Location = New System.Drawing.Point(903, 16)
        Me.LblALD.Name = "LblALD"
        Me.LblALD.Size = New System.Drawing.Size(31, 13)
        Me.LblALD.TabIndex = 22
        Me.LblALD.Text = "ALD"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(505, 16)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 21
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(796, 16)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(61, 13)
        Me.LblPatientDateMaj.TabIndex = 19
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(662, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientUniteSanitaire
        '
        Me.LblPatientUniteSanitaire.AutoSize = True
        Me.LblPatientUniteSanitaire.Location = New System.Drawing.Point(843, 49)
        Me.LblPatientUniteSanitaire.Name = "LblPatientUniteSanitaire"
        Me.LblPatientUniteSanitaire.Size = New System.Drawing.Size(43, 13)
        Me.LblPatientUniteSanitaire.TabIndex = 17
        Me.LblPatientUniteSanitaire.Text = "Auxerre"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(662, 49)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(175, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Centre médical de référence :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(705, 33)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(33, 13)
        Me.LblPatientSite.TabIndex = 15
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(662, 33)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Site :"
        '
        'AntecedentDataGridView
        '
        Me.AntecedentDataGridView.AllowDrop = True
        Me.AntecedentDataGridView.AllowUserToAddRows = False
        Me.AntecedentDataGridView.AllowUserToDeleteRows = False
        Me.AntecedentDataGridView.AllowUserToResizeRows = False
        Me.AntecedentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AntecedentDataGridView.ColumnHeadersVisible = False
        Me.AntecedentDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.antecedent, Me.antecedentDescription, Me.antecedentId, Me.antecedentDrcId, Me.antecedentNiveau, Me.antecedentPereId})
        Me.AntecedentDataGridView.ContextMenuStrip = Me.AntecedentContextMenuStrip
        Me.AntecedentDataGridView.Location = New System.Drawing.Point(12, 101)
        Me.AntecedentDataGridView.Name = "AntecedentDataGridView"
        Me.AntecedentDataGridView.ReadOnly = True
        Me.AntecedentDataGridView.RowHeadersVisible = False
        Me.AntecedentDataGridView.Size = New System.Drawing.Size(948, 132)
        Me.AntecedentDataGridView.TabIndex = 15
        '
        'antecedent
        '
        Me.antecedent.HeaderText = "Antécédents"
        Me.antecedent.Name = "antecedent"
        Me.antecedent.ReadOnly = True
        Me.antecedent.Width = 925
        '
        'antecedentDescription
        '
        Me.antecedentDescription.HeaderText = "Description antécédent"
        Me.antecedentDescription.Name = "antecedentDescription"
        Me.antecedentDescription.ReadOnly = True
        Me.antecedentDescription.Visible = False
        '
        'antecedentId
        '
        Me.antecedentId.HeaderText = "antecedentId"
        Me.antecedentId.Name = "antecedentId"
        Me.antecedentId.ReadOnly = True
        Me.antecedentId.Visible = False
        '
        'antecedentDrcId
        '
        Me.antecedentDrcId.HeaderText = "DRC Id"
        Me.antecedentDrcId.Name = "antecedentDrcId"
        Me.antecedentDrcId.ReadOnly = True
        Me.antecedentDrcId.Visible = False
        '
        'antecedentNiveau
        '
        Me.antecedentNiveau.HeaderText = "Niveau"
        Me.antecedentNiveau.Name = "antecedentNiveau"
        Me.antecedentNiveau.ReadOnly = True
        Me.antecedentNiveau.Visible = False
        '
        'antecedentPereId
        '
        Me.antecedentPereId.HeaderText = "Antécédent Pere Id"
        Me.antecedentPereId.Name = "antecedentPereId"
        Me.antecedentPereId.ReadOnly = True
        Me.antecedentPereId.Visible = False
        '
        'AntecedentContextMenuStrip
        '
        Me.AntecedentContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerAntecedentToolStripMenuItem, Me.HistoriqueDesModificationsToolStripMenuItem, Me.ModifierLordreDunAntécédentToolStripMenuItem, Me.ChangerLaffectationDunAntecedentToolStripMenuItem})
        Me.AntecedentContextMenuStrip.Name = "AntecedentContextMenuStrip"
        Me.AntecedentContextMenuStrip.Size = New System.Drawing.Size(275, 92)
        '
        'CréerAntecedentToolStripMenuItem
        '
        Me.CréerAntecedentToolStripMenuItem.Name = "CréerAntecedentToolStripMenuItem"
        Me.CréerAntecedentToolStripMenuItem.Size = New System.Drawing.Size(274, 22)
        Me.CréerAntecedentToolStripMenuItem.Text = "Créer un antécédent"
        '
        'HistoriqueDesModificationsToolStripMenuItem
        '
        Me.HistoriqueDesModificationsToolStripMenuItem.Name = "HistoriqueDesModificationsToolStripMenuItem"
        Me.HistoriqueDesModificationsToolStripMenuItem.Size = New System.Drawing.Size(274, 22)
        Me.HistoriqueDesModificationsToolStripMenuItem.Text = "Historique des modifications"
        '
        'ModifierLordreDunAntécédentToolStripMenuItem
        '
        Me.ModifierLordreDunAntécédentToolStripMenuItem.Name = "ModifierLordreDunAntécédentToolStripMenuItem"
        Me.ModifierLordreDunAntécédentToolStripMenuItem.Size = New System.Drawing.Size(274, 22)
        Me.ModifierLordreDunAntécédentToolStripMenuItem.Text = "Modifier l'ordre d'un antécédent"
        '
        'ChangerLaffectationDunAntecedentToolStripMenuItem
        '
        Me.ChangerLaffectationDunAntecedentToolStripMenuItem.Name = "ChangerLaffectationDunAntecedentToolStripMenuItem"
        Me.ChangerLaffectationDunAntecedentToolStripMenuItem.Size = New System.Drawing.Size(274, 22)
        Me.ChangerLaffectationDunAntecedentToolStripMenuItem.Text = "Changer l'affectation d'un antécédent"
        '
        'TraitementDataGridView
        '
        Me.TraitementDataGridView.AllowUserToAddRows = False
        Me.TraitementDataGridView.AllowUserToDeleteRows = False
        Me.TraitementDataGridView.AllowUserToResizeRows = False
        Me.TraitementDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TraitementDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.medicamentDci, Me.posologie, Me.dateDebut, Me.DateModification, Me.TraitementId, Me.BtnFenetreTherapeutique, Me.fenetreTherapeutique, Me.MedicamentCis})
        Me.TraitementDataGridView.ContextMenuStrip = Me.TraitementContextMenuStrip
        Me.TraitementDataGridView.Location = New System.Drawing.Point(12, 252)
        Me.TraitementDataGridView.Name = "TraitementDataGridView"
        Me.TraitementDataGridView.ReadOnly = True
        Me.TraitementDataGridView.RowHeadersVisible = False
        Me.TraitementDataGridView.Size = New System.Drawing.Size(948, 150)
        Me.TraitementDataGridView.TabIndex = 16
        '
        'medicamentDci
        '
        Me.medicamentDci.HeaderText = "Dénomination commerciale"
        Me.medicamentDci.Name = "medicamentDci"
        Me.medicamentDci.ReadOnly = True
        Me.medicamentDci.Width = 350
        '
        'posologie
        '
        Me.posologie.HeaderText = "Posologie"
        Me.posologie.Name = "posologie"
        Me.posologie.ReadOnly = True
        '
        'dateDebut
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.Format = "d"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.dateDebut.DefaultCellStyle = DataGridViewCellStyle1
        Me.dateDebut.HeaderText = "Début"
        Me.dateDebut.Name = "dateDebut"
        Me.dateDebut.ReadOnly = True
        '
        'DateModification
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DateModification.DefaultCellStyle = DataGridViewCellStyle2
        Me.DateModification.HeaderText = "Modification"
        Me.DateModification.Name = "DateModification"
        Me.DateModification.ReadOnly = True
        '
        'TraitementId
        '
        Me.TraitementId.HeaderText = "TraitementId"
        Me.TraitementId.Name = "TraitementId"
        Me.TraitementId.ReadOnly = True
        Me.TraitementId.Visible = False
        '
        'BtnFenetreTherapeutique
        '
        Me.BtnFenetreTherapeutique.HeaderText = "Fenêtre Th."
        Me.BtnFenetreTherapeutique.Name = "BtnFenetreTherapeutique"
        Me.BtnFenetreTherapeutique.ReadOnly = True
        Me.BtnFenetreTherapeutique.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.BtnFenetreTherapeutique.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.BtnFenetreTherapeutique.Text = "Gérer"
        Me.BtnFenetreTherapeutique.UseColumnTextForButtonValue = True
        Me.BtnFenetreTherapeutique.Visible = False
        Me.BtnFenetreTherapeutique.Width = 85
        '
        'fenetreTherapeutique
        '
        Me.fenetreTherapeutique.HeaderText = "Fenetre Thérapeutique"
        Me.fenetreTherapeutique.Name = "fenetreTherapeutique"
        Me.fenetreTherapeutique.ReadOnly = True
        Me.fenetreTherapeutique.Visible = False
        '
        'MedicamentCis
        '
        Me.MedicamentCis.HeaderText = "CIS"
        Me.MedicamentCis.Name = "MedicamentCis"
        Me.MedicamentCis.ReadOnly = True
        Me.MedicamentCis.Visible = False
        '
        'TraitementContextMenuStrip
        '
        Me.TraitementContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerUnTraitementToolStripMenuItem, Me.TraitementsObsoletesToolStripMenuItem, Me.DéclarerUneAllergieToolStripMenuItem, Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem, Me.HistoriqueToolStripMenuItem, Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem, Me.OrdonnanceToolStripMenuItem})
        Me.TraitementContextMenuStrip.Name = "TraitementContextMenuStrip"
        Me.TraitementContextMenuStrip.Size = New System.Drawing.Size(317, 158)
        '
        'CréerUnTraitementToolStripMenuItem
        '
        Me.CréerUnTraitementToolStripMenuItem.Name = "CréerUnTraitementToolStripMenuItem"
        Me.CréerUnTraitementToolStripMenuItem.Size = New System.Drawing.Size(316, 22)
        Me.CréerUnTraitementToolStripMenuItem.Text = "Créer un traitement"
        '
        'TraitementsObsoletesToolStripMenuItem
        '
        Me.TraitementsObsoletesToolStripMenuItem.Name = "TraitementsObsoletesToolStripMenuItem"
        Me.TraitementsObsoletesToolStripMenuItem.Size = New System.Drawing.Size(316, 22)
        Me.TraitementsObsoletesToolStripMenuItem.Text = "Afficher les traitements obsolètes"
        '
        'DéclarerUneAllergieToolStripMenuItem
        '
        Me.DéclarerUneAllergieToolStripMenuItem.Name = "DéclarerUneAllergieToolStripMenuItem"
        Me.DéclarerUneAllergieToolStripMenuItem.Size = New System.Drawing.Size(316, 22)
        Me.DéclarerUneAllergieToolStripMenuItem.Text = "Déclarer une allergie ou une contre-indication"
        Me.DéclarerUneAllergieToolStripMenuItem.Visible = False
        '
        'ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem
        '
        Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Name = "ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem"
        Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Size = New System.Drawing.Size(316, 22)
        Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Text = "Liste des médicaments déclarés allergiques"
        '
        'HistoriqueToolStripMenuItem
        '
        Me.HistoriqueToolStripMenuItem.Name = "HistoriqueToolStripMenuItem"
        Me.HistoriqueToolStripMenuItem.Size = New System.Drawing.Size(316, 22)
        Me.HistoriqueToolStripMenuItem.Text = "Historique des modifications"
        '
        'GérerUneFenêtreThérapeutiqueToolStripMenuItem
        '
        Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem.Name = "GérerUneFenêtreThérapeutiqueToolStripMenuItem"
        Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem.Size = New System.Drawing.Size(316, 22)
        Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem.Text = "Fenêtre thérapeutique"
        '
        'OrdonnanceToolStripMenuItem
        '
        Me.OrdonnanceToolStripMenuItem.Name = "OrdonnanceToolStripMenuItem"
        Me.OrdonnanceToolStripMenuItem.Size = New System.Drawing.Size(316, 22)
        Me.OrdonnanceToolStripMenuItem.Text = "Ordonnance"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 85)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "----- Antécédents -----"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 236)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(121, 13)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "----- Traitements -----"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(12, 405)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(150, 13)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "----- Parcours de soin -----"
        '
        'ParcoursDeSoinDataGridView
        '
        Me.ParcoursDeSoinDataGridView.AllowUserToAddRows = False
        Me.ParcoursDeSoinDataGridView.AllowUserToDeleteRows = False
        Me.ParcoursDeSoinDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ParcoursDeSoinDataGridView.ColumnHeadersVisible = False
        Me.ParcoursDeSoinDataGridView.Location = New System.Drawing.Point(12, 421)
        Me.ParcoursDeSoinDataGridView.Name = "ParcoursDeSoinDataGridView"
        Me.ParcoursDeSoinDataGridView.ReadOnly = True
        Me.ParcoursDeSoinDataGridView.RowHeadersVisible = False
        Me.ParcoursDeSoinDataGridView.Size = New System.Drawing.Size(948, 150)
        Me.ParcoursDeSoinDataGridView.TabIndex = 20
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(9, 574)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(111, 13)
        Me.Label10.TabIndex = 21
        Me.Label10.Text = "----- Contextes -----"
        '
        'ContexteDataGridView
        '
        Me.ContexteDataGridView.AllowUserToAddRows = False
        Me.ContexteDataGridView.AllowUserToDeleteRows = False
        Me.ContexteDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ContexteDataGridView.ColumnHeadersVisible = False
        Me.ContexteDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.categorieContexte, Me.Contexte, Me.ContexteId})
        Me.ContexteDataGridView.ContextMenuStrip = Me.ContexteMedicalContextMenuStrip
        Me.ContexteDataGridView.Location = New System.Drawing.Point(12, 590)
        Me.ContexteDataGridView.Name = "ContexteDataGridView"
        Me.ContexteDataGridView.ReadOnly = True
        Me.ContexteDataGridView.RowHeadersVisible = False
        Me.ContexteDataGridView.Size = New System.Drawing.Size(948, 154)
        Me.ContexteDataGridView.TabIndex = 22
        '
        'categorieContexte
        '
        Me.categorieContexte.HeaderText = "Catégorie Contexte"
        Me.categorieContexte.Name = "categorieContexte"
        Me.categorieContexte.ReadOnly = True
        Me.categorieContexte.Visible = False
        Me.categorieContexte.Width = 110
        '
        'Contexte
        '
        Me.Contexte.HeaderText = "Contexte"
        Me.Contexte.Name = "Contexte"
        Me.Contexte.ReadOnly = True
        Me.Contexte.Width = 925
        '
        'ContexteId
        '
        Me.ContexteId.HeaderText = "ContexteMedicalId"
        Me.ContexteId.Name = "ContexteId"
        Me.ContexteId.ReadOnly = True
        Me.ContexteId.Visible = False
        '
        'ContexteMedicalContextMenuStrip
        '
        Me.ContexteMedicalContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.HistoriqueDesModificationsToolStripMenuItem1})
        Me.ContexteMedicalContextMenuStrip.Name = "ContexteContextMenuStrip"
        Me.ContexteMedicalContextMenuStrip.Size = New System.Drawing.Size(227, 48)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(226, 22)
        Me.ToolStripMenuItem1.Text = "Créer un contexte"
        '
        'HistoriqueDesModificationsToolStripMenuItem1
        '
        Me.HistoriqueDesModificationsToolStripMenuItem1.Name = "HistoriqueDesModificationsToolStripMenuItem1"
        Me.HistoriqueDesModificationsToolStripMenuItem1.Size = New System.Drawing.Size(226, 22)
        Me.HistoriqueDesModificationsToolStripMenuItem1.Text = "Historique des modifications"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(12, 747)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(219, 13)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "----- Projet Personnalisé de Santé -----"
        '
        'PPSDataGridView
        '
        Me.PPSDataGridView.AllowUserToAddRows = False
        Me.PPSDataGridView.AllowUserToDeleteRows = False
        Me.PPSDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PPSDataGridView.ColumnHeadersVisible = False
        Me.PPSDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.pps, Me.ppsId})
        Me.PPSDataGridView.ContextMenuStrip = Me.PPSContextMenuStrip
        Me.PPSDataGridView.Location = New System.Drawing.Point(12, 763)
        Me.PPSDataGridView.Name = "PPSDataGridView"
        Me.PPSDataGridView.ReadOnly = True
        Me.PPSDataGridView.RowHeadersVisible = False
        Me.PPSDataGridView.Size = New System.Drawing.Size(948, 150)
        Me.PPSDataGridView.TabIndex = 24
        '
        'pps
        '
        Me.pps.HeaderText = "PPS"
        Me.pps.Name = "pps"
        Me.pps.ReadOnly = True
        Me.pps.Width = 925
        '
        'ppsId
        '
        Me.ppsId.HeaderText = "PPS Id"
        Me.ppsId.Name = "ppsId"
        Me.ppsId.ReadOnly = True
        Me.ppsId.Visible = False
        '
        'PPSContextMenuStrip
        '
        Me.PPSContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GestionDuPlanPersonnaliséDeSantéToolStripMenuItem})
        Me.PPSContextMenuStrip.Name = "PPSContextMenuStrip1"
        Me.PPSContextMenuStrip.Size = New System.Drawing.Size(274, 26)
        '
        'GestionDuPlanPersonnaliséDeSantéToolStripMenuItem
        '
        Me.GestionDuPlanPersonnaliséDeSantéToolStripMenuItem.Name = "GestionDuPlanPersonnaliséDeSantéToolStripMenuItem"
        Me.GestionDuPlanPersonnaliséDeSantéToolStripMenuItem.Size = New System.Drawing.Size(273, 22)
        Me.GestionDuPlanPersonnaliséDeSantéToolStripMenuItem.Text = "Gestion du plan personnalisé de santé"
        '
        'LblAllergie
        '
        Me.LblAllergie.AutoSize = True
        Me.LblAllergie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAllergie.ForeColor = System.Drawing.Color.Blue
        Me.LblAllergie.Location = New System.Drawing.Point(289, 236)
        Me.LblAllergie.Name = "LblAllergie"
        Me.LblAllergie.Size = New System.Drawing.Size(67, 13)
        Me.LblAllergie.TabIndex = 26
        Me.LblAllergie.Text = "Allergie(s) "
        Me.ToolTip.SetToolTip(Me.LblAllergie, resources.GetString("LblAllergie.ToolTip"))
        '
        'LblContreIndication
        '
        Me.LblContreIndication.AutoSize = True
        Me.LblContreIndication.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblContreIndication.ForeColor = System.Drawing.Color.Blue
        Me.LblContreIndication.Location = New System.Drawing.Point(156, 236)
        Me.LblContreIndication.Name = "LblContreIndication"
        Me.LblContreIndication.Size = New System.Drawing.Size(117, 13)
        Me.LblContreIndication.TabIndex = 30
        Me.LblContreIndication.Text = "Contre-indication(s)"
        Me.ToolTip.SetToolTip(Me.LblContreIndication, resources.GetString("LblContreIndication.ToolTip"))
        '
        'TxtAllergies
        '
        Me.TxtAllergies.BackColor = System.Drawing.SystemColors.Control
        Me.TxtAllergies.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAllergies.ForeColor = System.Drawing.Color.Crimson
        Me.TxtAllergies.Location = New System.Drawing.Point(358, 233)
        Me.TxtAllergies.Name = "TxtAllergies"
        Me.TxtAllergies.ReadOnly = True
        Me.TxtAllergies.Size = New System.Drawing.Size(602, 19)
        Me.TxtAllergies.TabIndex = 31
        '
        'ChkPublie
        '
        Me.ChkPublie.AutoSize = True
        Me.ChkPublie.Location = New System.Drawing.Point(255, 84)
        Me.ChkPublie.Name = "ChkPublie"
        Me.ChkPublie.Size = New System.Drawing.Size(66, 17)
        Me.ChkPublie.TabIndex = 34
        Me.ChkPublie.Text = "Publié(s)"
        Me.ToolTip.SetToolTip(Me.ChkPublie, "Option permettant d'afficher uniquement les antécédents ""Publiés"". Option par déf" &
        "aut")
        Me.ChkPublie.UseVisualStyleBackColor = True
        '
        'ChkTous
        '
        Me.ChkTous.AutoSize = True
        Me.ChkTous.Location = New System.Drawing.Point(327, 84)
        Me.ChkTous.Name = "ChkTous"
        Me.ChkTous.Size = New System.Drawing.Size(50, 17)
        Me.ChkTous.TabIndex = 35
        Me.ChkTous.Text = "Tous"
        Me.ToolTip.SetToolTip(Me.ChkTous, "Option permettant d'afficher les antécédents ""Publiés"" et ""Cachés"". " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Les antécéd" &
        "ents ""Cachés"" sont affichés en rouge.")
        Me.ChkTous.UseVisualStyleBackColor = True
        '
        'ChkParPriorite
        '
        Me.ChkParPriorite.AutoSize = True
        Me.ChkParPriorite.Location = New System.Drawing.Point(471, 84)
        Me.ChkParPriorite.Name = "ChkParPriorite"
        Me.ChkParPriorite.Size = New System.Drawing.Size(76, 17)
        Me.ChkParPriorite.TabIndex = 36
        Me.ChkParPriorite.Text = "Par priorité"
        Me.ToolTip.SetToolTip(Me.ChkParPriorite, "Option permettant d'afficher les antécédents par importance et selon leur organis" &
        "ation hierarchique." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Cette hierarchisation est organisée sur 3 niveaux.")
        Me.ChkParPriorite.UseVisualStyleBackColor = True
        '
        'ChkParChronologie
        '
        Me.ChkParChronologie.AutoSize = True
        Me.ChkParChronologie.Location = New System.Drawing.Point(553, 84)
        Me.ChkParChronologie.Name = "ChkParChronologie"
        Me.ChkParChronologie.Size = New System.Drawing.Size(100, 17)
        Me.ChkParChronologie.TabIndex = 37
        Me.ChkParChronologie.Text = "Par chronologie"
        Me.ToolTip.SetToolTip(Me.ChkParChronologie, "Option permettant d'afficher les antécédents chronologiquement (date de création)" &
        "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "du plus récent au plus ancien.")
        Me.ChkParChronologie.UseVisualStyleBackColor = True
        '
        'ChkContextePublie
        '
        Me.ChkContextePublie.AutoSize = True
        Me.ChkContextePublie.Location = New System.Drawing.Point(255, 573)
        Me.ChkContextePublie.Name = "ChkContextePublie"
        Me.ChkContextePublie.Size = New System.Drawing.Size(66, 17)
        Me.ChkContextePublie.TabIndex = 38
        Me.ChkContextePublie.Text = "Publié(s)"
        Me.ToolTip.SetToolTip(Me.ChkContextePublie, "Option permettant d'afficher uniquement les contextes ""Publiés"". Option par défau" &
        "t")
        Me.ChkContextePublie.UseVisualStyleBackColor = True
        '
        'ChkContexteTous
        '
        Me.ChkContexteTous.AutoSize = True
        Me.ChkContexteTous.Location = New System.Drawing.Point(327, 573)
        Me.ChkContexteTous.Name = "ChkContexteTous"
        Me.ChkContexteTous.Size = New System.Drawing.Size(50, 17)
        Me.ChkContexteTous.TabIndex = 39
        Me.ChkContexteTous.Text = "Tous"
        Me.ToolTip.SetToolTip(Me.ChkContexteTous, "Option permettant d'afficher les contextes ""Publiés"" et ""Cachés"". " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Les contextes" &
        " ""Cachés"" sont affichés en rouge.")
        Me.ChkContexteTous.UseVisualStyleBackColor = True
        '
        'BtnDirectives
        '
        Me.BtnDirectives.Location = New System.Drawing.Point(336, 919)
        Me.BtnDirectives.Name = "BtnDirectives"
        Me.BtnDirectives.Size = New System.Drawing.Size(114, 23)
        Me.BtnDirectives.TabIndex = 40
        Me.BtnDirectives.Text = "Directives anticipées"
        Me.BtnDirectives.UseVisualStyleBackColor = True
        '
        'BtnSocial
        '
        Me.BtnSocial.Location = New System.Drawing.Point(12, 919)
        Me.BtnSocial.Name = "BtnSocial"
        Me.BtnSocial.Size = New System.Drawing.Size(75, 23)
        Me.BtnSocial.TabIndex = 41
        Me.BtnSocial.Text = "Social"
        Me.BtnSocial.UseVisualStyleBackColor = True
        '
        'BtnLigneDeVie
        '
        Me.BtnLigneDeVie.Location = New System.Drawing.Point(255, 919)
        Me.BtnLigneDeVie.Name = "BtnLigneDeVie"
        Me.BtnLigneDeVie.Size = New System.Drawing.Size(75, 23)
        Me.BtnLigneDeVie.TabIndex = 42
        Me.BtnLigneDeVie.Text = "Ligne de vie"
        Me.BtnLigneDeVie.UseVisualStyleBackColor = True
        '
        'BtnEpisode
        '
        Me.BtnEpisode.Location = New System.Drawing.Point(885, 919)
        Me.BtnEpisode.Name = "BtnEpisode"
        Me.BtnEpisode.Size = New System.Drawing.Size(75, 23)
        Me.BtnEpisode.TabIndex = 43
        Me.BtnEpisode.Text = "Episode"
        Me.BtnEpisode.UseVisualStyleBackColor = True
        '
        'BtnNotesMedicales
        '
        Me.BtnNotesMedicales.Location = New System.Drawing.Point(174, 919)
        Me.BtnNotesMedicales.Name = "BtnNotesMedicales"
        Me.BtnNotesMedicales.Size = New System.Drawing.Size(75, 23)
        Me.BtnNotesMedicales.TabIndex = 44
        Me.BtnNotesMedicales.Text = "Notes"
        Me.BtnNotesMedicales.UseVisualStyleBackColor = True
        '
        'BtnVaccins
        '
        Me.BtnVaccins.Location = New System.Drawing.Point(93, 919)
        Me.BtnVaccins.Name = "BtnVaccins"
        Me.BtnVaccins.Size = New System.Drawing.Size(75, 23)
        Me.BtnVaccins.TabIndex = 45
        Me.BtnVaccins.Text = "Vaccins"
        Me.BtnVaccins.UseVisualStyleBackColor = True
        '
        'FSynthese
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(979, 948)
        Me.Controls.Add(Me.BtnVaccins)
        Me.Controls.Add(Me.BtnNotesMedicales)
        Me.Controls.Add(Me.BtnEpisode)
        Me.Controls.Add(Me.BtnLigneDeVie)
        Me.Controls.Add(Me.BtnSocial)
        Me.Controls.Add(Me.BtnDirectives)
        Me.Controls.Add(Me.ChkContexteTous)
        Me.Controls.Add(Me.ChkContextePublie)
        Me.Controls.Add(Me.ChkParChronologie)
        Me.Controls.Add(Me.ChkParPriorite)
        Me.Controls.Add(Me.ChkTous)
        Me.Controls.Add(Me.ChkPublie)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TxtAllergies)
        Me.Controls.Add(Me.LblContreIndication)
        Me.Controls.Add(Me.LblAllergie)
        Me.Controls.Add(Me.PPSDataGridView)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.ContexteDataGridView)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.ParcoursDeSoinDataGridView)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.AntecedentDataGridView)
        Me.Controls.Add(Me.TraitementDataGridView)
        Me.Name = "FSynthese"
        Me.Text = "Outil de synthèse"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.EtatCivilContextMenuStrip.ResumeLayout(False)
        CType(Me.AntecedentDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AntecedentContextMenuStrip.ResumeLayout(False)
        CType(Me.TraitementDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TraitementContextMenuStrip.ResumeLayout(False)
        CType(Me.ParcoursDeSoinDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ContexteDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContexteMedicalContextMenuStrip.ResumeLayout(False)
        CType(Me.PPSDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PPSContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents LblPatientAdresse1 As Label
    Friend WithEvents LblPatientAdresse2 As Label
    Friend WithEvents LblPatientCodePostal As Label
    Friend WithEvents LblPatientVille As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents LblPatientTel1 As Label
    Friend WithEvents LblPatientTel2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LblPatientUniteSanitaire As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents LblPatientSite As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents LblPatientDateMaj As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents AntecedentDataGridView As DataGridView
    Friend WithEvents TraitementDataGridView As DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents ParcoursDeSoinDataGridView As DataGridView
    Friend WithEvents Label10 As Label
    Friend WithEvents ContexteDataGridView As DataGridView
    Friend WithEvents Label11 As Label
    Friend WithEvents PPSDataGridView As DataGridView
    Friend WithEvents TraitementContextMenuStrip As ContextMenuStrip
    Friend WithEvents TraitementsObsoletesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AntecedentContextMenuStrip As ContextMenuStrip
    Friend WithEvents EtatCivilContextMenuStrip As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemMaps As ToolStripMenuItem
    Friend WithEvents LblAllergie As Label
    Friend WithEvents ContexteMedicalContextMenuStrip As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents Label13 As Label
    Friend WithEvents LblContreIndication As Label
    Friend WithEvents CréerUnTraitementToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DéclarerUneAllergieToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HistoriqueToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TxtAllergies As TextBox
    Friend WithEvents GérerUneFenêtreThérapeutiqueToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents medicamentDci As DataGridViewTextBoxColumn
    Friend WithEvents posologie As DataGridViewTextBoxColumn
    Friend WithEvents dateDebut As DataGridViewTextBoxColumn
    Friend WithEvents DateModification As DataGridViewTextBoxColumn
    Friend WithEvents TraitementId As DataGridViewTextBoxColumn
    Friend WithEvents BtnFenetreTherapeutique As DataGridViewButtonColumn
    Friend WithEvents fenetreTherapeutique As DataGridViewTextBoxColumn
    Friend WithEvents MedicamentCis As DataGridViewTextBoxColumn
    Friend WithEvents CréerAntecedentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HistoriqueDesModificationsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ModifierLordreDunAntécédentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChangerLaffectationDunAntecedentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChkPublie As CheckBox
    Friend WithEvents ChkTous As CheckBox
    Friend WithEvents ChkParPriorite As CheckBox
    Friend WithEvents ChkParChronologie As CheckBox
    Friend WithEvents HistoriqueDesModificationsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ChkContextePublie As CheckBox
    Friend WithEvents ChkContexteTous As CheckBox
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents BtnDirectives As Button
    Friend WithEvents BtnSocial As Button
    Friend WithEvents BtnLigneDeVie As Button
    Friend WithEvents BtnEpisode As Button
    Friend WithEvents BtnNotesMedicales As Button
    Friend WithEvents OrdonnanceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PPSContextMenuStrip As ContextMenuStrip
    Friend WithEvents GestionDuPlanPersonnaliséDeSantéToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DétailPatientToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnVaccins As Button
    Friend WithEvents categorieContexte As DataGridViewTextBoxColumn
    Friend WithEvents Contexte As DataGridViewTextBoxColumn
    Friend WithEvents ContexteId As DataGridViewTextBoxColumn
    Friend WithEvents pps As DataGridViewTextBoxColumn
    Friend WithEvents ppsId As DataGridViewTextBoxColumn
    Friend WithEvents ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents antecedent As DataGridViewTextBoxColumn
    Friend WithEvents antecedentDescription As DataGridViewTextBoxColumn
    Friend WithEvents antecedentId As DataGridViewTextBoxColumn
    Friend WithEvents antecedentDrcId As DataGridViewTextBoxColumn
    Friend WithEvents antecedentNiveau As DataGridViewTextBoxColumn
    Friend WithEvents antecedentPereId As DataGridViewTextBoxColumn
    Friend WithEvents ListeDesALDToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LblALD As Label
End Class
