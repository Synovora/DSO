<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FPatientPPSListeGestion
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LblALD = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LblPatientDateMaj = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblPatientUniteSanitaire = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.LblPatientSite = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LblPatientTel2 = New System.Windows.Forms.Label()
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientTel1 = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.LblPatientVille = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientCodePostal = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.LblPatientAdresse2 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblPatientAdresse1 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtObjectifCommentaire = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.PreventionDataGridView = New System.Windows.Forms.DataGridView()
        Me.preventionId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.preventionPriorite = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.preventionDrcDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.preventionCommentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PreventionContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUneMesurePréventiveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoriqueDesModificationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.SuiviDataGridView = New System.Windows.Forms.DataGridView()
        Me.suiviId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviSpecialite = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviNomIntervenant = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviNomStructure = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviRythme = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviBase = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviDateLastConsultation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviDateNextConsultation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviCommentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.suiviAfficheSynthese = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SuiviContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUnSuiviToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoriqueDesModificationsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.StrategieDataGridView = New System.Windows.Forms.DataGridView()
        Me.strategieId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.strategieDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.strategieSousCategorie = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.strategieDrcDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.strategieCommentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StrategieContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUneStratégieContextuelleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoriqueDesModificationsToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.TxtObjectifCodeDrc = New System.Windows.Forms.TextBox()
        Me.TxtObjectifDrcLibelle = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.BtnValidationObjectif = New System.Windows.Forms.Button()
        Me.ToolTipPPS = New System.Windows.Forms.ToolTip(Me.components)
        Me.BtnSelectionDrcObjectif = New System.Windows.Forms.Button()
        Me.BtnInitDrcObjectif = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ObjectifContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.JHistoriqueDesModificationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PreventionDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PreventionContextMenuStrip.SuspendLayout()
        CType(Me.SuiviDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuiviContextMenuStrip.SuspendLayout()
        CType(Me.StrategieDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StrategieContextMenuStrip.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ObjectifContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
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
        Me.GroupBox1.Size = New System.Drawing.Size(1392, 71)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Etat civil"
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
        'LblPatientTel2
        '
        Me.LblPatientTel2.AutoSize = True
        Me.LblPatientTel2.Location = New System.Drawing.Point(398, 49)
        Me.LblPatientTel2.Name = "LblPatientTel2"
        Me.LblPatientTel2.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel2.TabIndex = 13
        Me.LblPatientTel2.Text = "0968542357"
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
        'LblPatientTel1
        '
        Me.LblPatientTel1.AutoSize = True
        Me.LblPatientTel1.Location = New System.Drawing.Point(398, 33)
        Me.LblPatientTel1.Name = "LblPatientTel1"
        Me.LblPatientTel1.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel1.TabIndex = 12
        Me.LblPatientTel1.Text = "0288425678"
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
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(245, 16)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(39, 13)
        Me.LblPatientAge.TabIndex = 3
        Me.LblPatientAge.Text = "35 ans"
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
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(355, 16)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(49, 13)
        Me.LblPatientGenre.TabIndex = 4
        Me.LblPatientGenre.Text = "Masculin"
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
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(556, 16)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 5
        Me.LblPatientNIR.Text = "1840675370367"
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
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(152, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "----- Objectif de santé -----"
        '
        'TxtObjectifCommentaire
        '
        Me.TxtObjectifCommentaire.Location = New System.Drawing.Point(91, 32)
        Me.TxtObjectifCommentaire.Multiline = True
        Me.TxtObjectifCommentaire.Name = "TxtObjectifCommentaire"
        Me.TxtObjectifCommentaire.Size = New System.Drawing.Size(1094, 44)
        Me.TxtObjectifCommentaire.TabIndex = 19
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(9, 188)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(188, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "----- Mesure(s) préventive(s) -----"
        '
        'PreventionDataGridView
        '
        Me.PreventionDataGridView.AllowUserToAddRows = False
        Me.PreventionDataGridView.AllowUserToDeleteRows = False
        Me.PreventionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PreventionDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.preventionId, Me.preventionPriorite, Me.preventionDrcDescription, Me.preventionCommentaire})
        Me.PreventionDataGridView.ContextMenuStrip = Me.PreventionContextMenuStrip
        Me.PreventionDataGridView.Location = New System.Drawing.Point(12, 204)
        Me.PreventionDataGridView.Name = "PreventionDataGridView"
        Me.PreventionDataGridView.ReadOnly = True
        Me.PreventionDataGridView.RowHeadersVisible = False
        Me.PreventionDataGridView.Size = New System.Drawing.Size(1392, 78)
        Me.PreventionDataGridView.TabIndex = 21
        '
        'preventionId
        '
        Me.preventionId.HeaderText = "Id PPS"
        Me.preventionId.Name = "preventionId"
        Me.preventionId.ReadOnly = True
        Me.preventionId.Visible = False
        '
        'preventionPriorite
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.preventionPriorite.DefaultCellStyle = DataGridViewCellStyle1
        Me.preventionPriorite.HeaderText = "Priorité"
        Me.preventionPriorite.Name = "preventionPriorite"
        Me.preventionPriorite.ReadOnly = True
        Me.preventionPriorite.Width = 70
        '
        'preventionDrcDescription
        '
        Me.preventionDrcDescription.HeaderText = "Objectif"
        Me.preventionDrcDescription.Name = "preventionDrcDescription"
        Me.preventionDrcDescription.ReadOnly = True
        Me.preventionDrcDescription.Width = 600
        '
        'preventionCommentaire
        '
        Me.preventionCommentaire.HeaderText = "Commentaire"
        Me.preventionCommentaire.Name = "preventionCommentaire"
        Me.preventionCommentaire.ReadOnly = True
        Me.preventionCommentaire.Width = 600
        '
        'PreventionContextMenuStrip
        '
        Me.PreventionContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerUneMesurePréventiveToolStripMenuItem, Me.HistoriqueDesModificationsToolStripMenuItem})
        Me.PreventionContextMenuStrip.Name = "PreventionContextMenuStrip"
        Me.PreventionContextMenuStrip.Size = New System.Drawing.Size(227, 48)
        '
        'CréerUneMesurePréventiveToolStripMenuItem
        '
        Me.CréerUneMesurePréventiveToolStripMenuItem.Name = "CréerUneMesurePréventiveToolStripMenuItem"
        Me.CréerUneMesurePréventiveToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.CréerUneMesurePréventiveToolStripMenuItem.Text = "Créer une mesure préventive"
        '
        'HistoriqueDesModificationsToolStripMenuItem
        '
        Me.HistoriqueDesModificationsToolStripMenuItem.Name = "HistoriqueDesModificationsToolStripMenuItem"
        Me.HistoriqueDesModificationsToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.HistoriqueDesModificationsToolStripMenuItem.Text = "Historique des modifications"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(9, 285)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 13)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "----- Suivi -----"
        '
        'SuiviDataGridView
        '
        Me.SuiviDataGridView.AllowUserToAddRows = False
        Me.SuiviDataGridView.AllowUserToDeleteRows = False
        Me.SuiviDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SuiviDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.suiviId, Me.suiviSpecialite, Me.suiviNomIntervenant, Me.suiviNomStructure, Me.suiviRythme, Me.suiviBase, Me.suiviDateLastConsultation, Me.suiviDateNextConsultation, Me.suiviCommentaire, Me.suiviAfficheSynthese})
        Me.SuiviDataGridView.ContextMenuStrip = Me.SuiviContextMenuStrip
        Me.SuiviDataGridView.Location = New System.Drawing.Point(12, 301)
        Me.SuiviDataGridView.Name = "SuiviDataGridView"
        Me.SuiviDataGridView.ReadOnly = True
        Me.SuiviDataGridView.RowHeadersVisible = False
        Me.SuiviDataGridView.Size = New System.Drawing.Size(1392, 150)
        Me.SuiviDataGridView.TabIndex = 23
        '
        'suiviId
        '
        Me.suiviId.HeaderText = "Id Suivi"
        Me.suiviId.Name = "suiviId"
        Me.suiviId.ReadOnly = True
        Me.suiviId.Visible = False
        '
        'suiviSpecialite
        '
        Me.suiviSpecialite.HeaderText = "Spécialite"
        Me.suiviSpecialite.Name = "suiviSpecialite"
        Me.suiviSpecialite.ReadOnly = True
        Me.suiviSpecialite.Width = 180
        '
        'suiviNomIntervenant
        '
        Me.suiviNomIntervenant.HeaderText = "Intervenant"
        Me.suiviNomIntervenant.Name = "suiviNomIntervenant"
        Me.suiviNomIntervenant.ReadOnly = True
        Me.suiviNomIntervenant.Width = 180
        '
        'suiviNomStructure
        '
        Me.suiviNomStructure.HeaderText = "Structure"
        Me.suiviNomStructure.Name = "suiviNomStructure"
        Me.suiviNomStructure.ReadOnly = True
        Me.suiviNomStructure.Width = 180
        '
        'suiviRythme
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.suiviRythme.DefaultCellStyle = DataGridViewCellStyle2
        Me.suiviRythme.HeaderText = "Rythme"
        Me.suiviRythme.Name = "suiviRythme"
        Me.suiviRythme.ReadOnly = True
        Me.suiviRythme.Width = 70
        '
        'suiviBase
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.suiviBase.DefaultCellStyle = DataGridViewCellStyle3
        Me.suiviBase.HeaderText = "Nombre"
        Me.suiviBase.Name = "suiviBase"
        Me.suiviBase.ReadOnly = True
        Me.suiviBase.Width = 50
        '
        'suiviDateLastConsultation
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.suiviDateLastConsultation.DefaultCellStyle = DataGridViewCellStyle4
        Me.suiviDateLastConsultation.HeaderText = "Dernière Consultation"
        Me.suiviDateLastConsultation.Name = "suiviDateLastConsultation"
        Me.suiviDateLastConsultation.ReadOnly = True
        '
        'suiviDateNextConsultation
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.suiviDateNextConsultation.DefaultCellStyle = DataGridViewCellStyle5
        Me.suiviDateNextConsultation.HeaderText = "Prochaine consultation"
        Me.suiviDateNextConsultation.Name = "suiviDateNextConsultation"
        Me.suiviDateNextConsultation.ReadOnly = True
        '
        'suiviCommentaire
        '
        Me.suiviCommentaire.HeaderText = "Commentaire"
        Me.suiviCommentaire.Name = "suiviCommentaire"
        Me.suiviCommentaire.ReadOnly = True
        Me.suiviCommentaire.Width = 450
        '
        'suiviAfficheSynthese
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.suiviAfficheSynthese.DefaultCellStyle = DataGridViewCellStyle6
        Me.suiviAfficheSynthese.HeaderText = "Affichage dans Synthèse"
        Me.suiviAfficheSynthese.Name = "suiviAfficheSynthese"
        Me.suiviAfficheSynthese.ReadOnly = True
        Me.suiviAfficheSynthese.Width = 70
        '
        'SuiviContextMenuStrip
        '
        Me.SuiviContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerUnSuiviToolStripMenuItem, Me.HistoriqueDesModificationsToolStripMenuItem1})
        Me.SuiviContextMenuStrip.Name = "SuiviContextMenuStrip"
        Me.SuiviContextMenuStrip.Size = New System.Drawing.Size(227, 70)
        '
        'CréerUnSuiviToolStripMenuItem
        '
        Me.CréerUnSuiviToolStripMenuItem.Name = "CréerUnSuiviToolStripMenuItem"
        Me.CréerUnSuiviToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.CréerUnSuiviToolStripMenuItem.Text = "Créer un suivi"
        '
        'HistoriqueDesModificationsToolStripMenuItem1
        '
        Me.HistoriqueDesModificationsToolStripMenuItem1.Name = "HistoriqueDesModificationsToolStripMenuItem1"
        Me.HistoriqueDesModificationsToolStripMenuItem1.Size = New System.Drawing.Size(226, 22)
        Me.HistoriqueDesModificationsToolStripMenuItem1.Text = "Historique des modifications"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(9, 454)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(207, 13)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "----- Stratégie(s) contextuelle(s) -----"
        '
        'StrategieDataGridView
        '
        Me.StrategieDataGridView.AllowUserToAddRows = False
        Me.StrategieDataGridView.AllowUserToDeleteRows = False
        Me.StrategieDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.StrategieDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.strategieId, Me.strategieDate, Me.strategieSousCategorie, Me.strategieDrcDescription, Me.strategieCommentaire})
        Me.StrategieDataGridView.ContextMenuStrip = Me.StrategieContextMenuStrip
        Me.StrategieDataGridView.Location = New System.Drawing.Point(12, 470)
        Me.StrategieDataGridView.Name = "StrategieDataGridView"
        Me.StrategieDataGridView.ReadOnly = True
        Me.StrategieDataGridView.RowHeadersVisible = False
        Me.StrategieDataGridView.Size = New System.Drawing.Size(1392, 111)
        Me.StrategieDataGridView.TabIndex = 25
        '
        'strategieId
        '
        Me.strategieId.HeaderText = "Identifiant stratégie"
        Me.strategieId.Name = "strategieId"
        Me.strategieId.ReadOnly = True
        Me.strategieId.Visible = False
        '
        'strategieDate
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.strategieDate.DefaultCellStyle = DataGridViewCellStyle7
        Me.strategieDate.HeaderText = "Date"
        Me.strategieDate.Name = "strategieDate"
        Me.strategieDate.ReadOnly = True
        '
        'strategieSousCategorie
        '
        Me.strategieSousCategorie.HeaderText = "Nature"
        Me.strategieSousCategorie.Name = "strategieSousCategorie"
        Me.strategieSousCategorie.ReadOnly = True
        Me.strategieSousCategorie.Width = 120
        '
        'strategieDrcDescription
        '
        Me.strategieDrcDescription.HeaderText = "Démarche stratégique"
        Me.strategieDrcDescription.Name = "strategieDrcDescription"
        Me.strategieDrcDescription.ReadOnly = True
        Me.strategieDrcDescription.Width = 575
        '
        'strategieCommentaire
        '
        Me.strategieCommentaire.HeaderText = "Commentaire"
        Me.strategieCommentaire.Name = "strategieCommentaire"
        Me.strategieCommentaire.ReadOnly = True
        Me.strategieCommentaire.Width = 575
        '
        'StrategieContextMenuStrip
        '
        Me.StrategieContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerUneStratégieContextuelleToolStripMenuItem, Me.HistoriqueDesModificationsToolStripMenuItem2})
        Me.StrategieContextMenuStrip.Name = "StrategieContextMenuStrip"
        Me.StrategieContextMenuStrip.Size = New System.Drawing.Size(241, 48)
        '
        'CréerUneStratégieContextuelleToolStripMenuItem
        '
        Me.CréerUneStratégieContextuelleToolStripMenuItem.Name = "CréerUneStratégieContextuelleToolStripMenuItem"
        Me.CréerUneStratégieContextuelleToolStripMenuItem.Size = New System.Drawing.Size(240, 22)
        Me.CréerUneStratégieContextuelleToolStripMenuItem.Text = "Créer une stratégie contextuelle"
        '
        'HistoriqueDesModificationsToolStripMenuItem2
        '
        Me.HistoriqueDesModificationsToolStripMenuItem2.Name = "HistoriqueDesModificationsToolStripMenuItem2"
        Me.HistoriqueDesModificationsToolStripMenuItem2.Size = New System.Drawing.Size(240, 22)
        Me.HistoriqueDesModificationsToolStripMenuItem2.Text = "Historique des modifications"
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(1329, 587)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 26
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'TxtObjectifCodeDrc
        '
        Me.TxtObjectifCodeDrc.Location = New System.Drawing.Point(91, 6)
        Me.TxtObjectifCodeDrc.Name = "TxtObjectifCodeDrc"
        Me.TxtObjectifCodeDrc.ReadOnly = True
        Me.TxtObjectifCodeDrc.Size = New System.Drawing.Size(44, 20)
        Me.TxtObjectifCodeDrc.TabIndex = 27
        '
        'TxtObjectifDrcLibelle
        '
        Me.TxtObjectifDrcLibelle.Location = New System.Drawing.Point(141, 6)
        Me.TxtObjectifDrcLibelle.Name = "TxtObjectifDrcLibelle"
        Me.TxtObjectifDrcLibelle.ReadOnly = True
        Me.TxtObjectifDrcLibelle.Size = New System.Drawing.Size(1044, 20)
        Me.TxtObjectifDrcLibelle.TabIndex = 28
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 13)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(86, 13)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "Code DRC/ORC"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(3, 35)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(43, 13)
        Me.Label11.TabIndex = 30
        Me.Label11.Text = "Objectif"
        '
        'BtnValidationObjectif
        '
        Me.BtnValidationObjectif.Location = New System.Drawing.Point(1246, 53)
        Me.BtnValidationObjectif.Name = "BtnValidationObjectif"
        Me.BtnValidationObjectif.Size = New System.Drawing.Size(111, 23)
        Me.BtnValidationObjectif.TabIndex = 31
        Me.BtnValidationObjectif.Text = "Validation objectif"
        Me.BtnValidationObjectif.UseVisualStyleBackColor = True
        '
        'BtnSelectionDrcObjectif
        '
        Me.BtnSelectionDrcObjectif.Location = New System.Drawing.Point(1201, 4)
        Me.BtnSelectionDrcObjectif.Name = "BtnSelectionDrcObjectif"
        Me.BtnSelectionDrcObjectif.Size = New System.Drawing.Size(75, 23)
        Me.BtnSelectionDrcObjectif.TabIndex = 32
        Me.BtnSelectionDrcObjectif.Text = "Sélection DRC/ORC"
        Me.BtnSelectionDrcObjectif.UseVisualStyleBackColor = True
        '
        'BtnInitDrcObjectif
        '
        Me.BtnInitDrcObjectif.Location = New System.Drawing.Point(1282, 4)
        Me.BtnInitDrcObjectif.Name = "BtnInitDrcObjectif"
        Me.BtnInitDrcObjectif.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitDrcObjectif.TabIndex = 33
        Me.BtnInitDrcObjectif.Text = "Initialiser"
        Me.BtnInitDrcObjectif.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.Panel1.ContextMenuStrip = Me.ObjectifContextMenuStrip
        Me.Panel1.Controls.Add(Me.BtnInitDrcObjectif)
        Me.Panel1.Controls.Add(Me.BtnSelectionDrcObjectif)
        Me.Panel1.Controls.Add(Me.BtnValidationObjectif)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.TxtObjectifDrcLibelle)
        Me.Panel1.Controls.Add(Me.TxtObjectifCodeDrc)
        Me.Panel1.Controls.Add(Me.TxtObjectifCommentaire)
        Me.Panel1.Location = New System.Drawing.Point(12, 102)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1392, 79)
        Me.Panel1.TabIndex = 34
        '
        'ObjectifContextMenuStrip
        '
        Me.ObjectifContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.JHistoriqueDesModificationsToolStripMenuItem})
        Me.ObjectifContextMenuStrip.Name = "ObjectifContextMenuStrip"
        Me.ObjectifContextMenuStrip.Size = New System.Drawing.Size(227, 26)
        '
        'JHistoriqueDesModificationsToolStripMenuItem
        '
        Me.JHistoriqueDesModificationsToolStripMenuItem.Name = "JHistoriqueDesModificationsToolStripMenuItem"
        Me.JHistoriqueDesModificationsToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.JHistoriqueDesModificationsToolStripMenuItem.Text = "Historique des modifications"
        '
        'FPatientPPSListeGestion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1416, 630)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Controls.Add(Me.StrategieDataGridView)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.SuiviDataGridView)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.PreventionDataGridView)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FPatientPPSListeGestion"
        Me.Text = "Gestion du plan personnalisé de soin du patient"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PreventionDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PreventionContextMenuStrip.ResumeLayout(False)
        CType(Me.SuiviDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SuiviContextMenuStrip.ResumeLayout(False)
        CType(Me.StrategieDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StrategieContextMenuStrip.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ObjectifContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LblALD As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents LblPatientDateMaj As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LblPatientUniteSanitaire As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents LblPatientSite As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents LblPatientTel2 As Label
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientTel1 As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents LblPatientVille As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientCodePostal As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents LblPatientAdresse2 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents LblPatientAdresse1 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtObjectifCommentaire As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents PreventionDataGridView As DataGridView
    Friend WithEvents Label8 As Label
    Friend WithEvents SuiviDataGridView As DataGridView
    Friend WithEvents Label9 As Label
    Friend WithEvents StrategieDataGridView As DataGridView
    Friend WithEvents preventionId As DataGridViewTextBoxColumn
    Friend WithEvents preventionPriorite As DataGridViewTextBoxColumn
    Friend WithEvents preventionDrcDescription As DataGridViewTextBoxColumn
    Friend WithEvents preventionCommentaire As DataGridViewTextBoxColumn
    Friend WithEvents PreventionContextMenuStrip As ContextMenuStrip
    Friend WithEvents SuiviContextMenuStrip As ContextMenuStrip
    Friend WithEvents StrategieContextMenuStrip As ContextMenuStrip
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents CréerUneMesurePréventiveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CréerUnSuiviToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CréerUneStratégieContextuelleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents suiviId As DataGridViewTextBoxColumn
    Friend WithEvents suiviSpecialite As DataGridViewTextBoxColumn
    Friend WithEvents suiviNomIntervenant As DataGridViewTextBoxColumn
    Friend WithEvents suiviNomStructure As DataGridViewTextBoxColumn
    Friend WithEvents suiviRythme As DataGridViewTextBoxColumn
    Friend WithEvents suiviBase As DataGridViewTextBoxColumn
    Friend WithEvents suiviDateLastConsultation As DataGridViewTextBoxColumn
    Friend WithEvents suiviDateNextConsultation As DataGridViewTextBoxColumn
    Friend WithEvents suiviCommentaire As DataGridViewTextBoxColumn
    Friend WithEvents suiviAfficheSynthese As DataGridViewTextBoxColumn
    Friend WithEvents TxtObjectifCodeDrc As TextBox
    Friend WithEvents TxtObjectifDrcLibelle As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents BtnValidationObjectif As Button
    Friend WithEvents ToolTipPPS As ToolTip
    Friend WithEvents BtnSelectionDrcObjectif As Button
    Friend WithEvents BtnInitDrcObjectif As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ObjectifContextMenuStrip As ContextMenuStrip
    Friend WithEvents JHistoriqueDesModificationsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HistoriqueDesModificationsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HistoriqueDesModificationsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents HistoriqueDesModificationsToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents strategieId As DataGridViewTextBoxColumn
    Friend WithEvents strategieDate As DataGridViewTextBoxColumn
    Friend WithEvents strategieSousCategorie As DataGridViewTextBoxColumn
    Friend WithEvents strategieDrcDescription As DataGridViewTextBoxColumn
    Friend WithEvents strategieCommentaire As DataGridViewTextBoxColumn
End Class
