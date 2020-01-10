<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FDrcSelecteur
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
        Me.DrcDataGridView = New System.Windows.Forms.DataGridView()
        Me.drcId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.drcDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.categorieMajeure = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.drcOasis = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_drc_sexe = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.contexte = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_drc_age_min = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_drc_age_max = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TxtDrc = New System.Windows.Forms.TextBox()
        Me.BtnFiltrer = New System.Windows.Forms.Button()
        Me.BtnInitialiser = New System.Windows.Forms.Button()
        Me.BtnSelection = New System.Windows.Forms.Button()
        Me.LblDrcId = New System.Windows.Forms.Label()
        Me.LblDrcLibelle = New System.Windows.Forms.Label()
        Me.DrcDefinitionDataGridView = New System.Windows.Forms.DataGridView()
        Me.oa_drc_synonyme_libelle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LblLabelDrcAge = New System.Windows.Forms.Label()
        Me.LblAgeMinLbl = New System.Windows.Forms.Label()
        Me.LblDrcAgeMin = New System.Windows.Forms.Label()
        Me.LblAgeMaxLbl = New System.Windows.Forms.Label()
        Me.LblDrcAgeMax = New System.Windows.Forms.Label()
        Me.PnlSelection = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
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
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.LblPatientVille = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientCodePostal = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.LblPatientAdresse2 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.LblPatientAdresse1 = New System.Windows.Forms.Label()
        Me.CbxFiltreCategorieMajeure = New System.Windows.Forms.ComboBox()
        Me.ChkORC = New System.Windows.Forms.CheckBox()
        Me.LblCategorieOasis = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.DrcDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DrcDefinitionDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSelection.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DrcDataGridView
        '
        Me.DrcDataGridView.AllowUserToAddRows = False
        Me.DrcDataGridView.AllowUserToDeleteRows = False
        Me.DrcDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DrcDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.drcId, Me.drcDescription, Me.categorieMajeure, Me.drcOasis, Me.oa_drc_sexe, Me.contexte, Me.oa_drc_age_min, Me.oa_drc_age_max})
        Me.DrcDataGridView.Location = New System.Drawing.Point(12, 167)
        Me.DrcDataGridView.Name = "DrcDataGridView"
        Me.DrcDataGridView.ReadOnly = True
        Me.DrcDataGridView.RowHeadersVisible = False
        Me.DrcDataGridView.Size = New System.Drawing.Size(721, 298)
        Me.DrcDataGridView.TabIndex = 0
        '
        'drcId
        '
        Me.drcId.DataPropertyName = "oa_drc_id"
        Me.drcId.HeaderText = "DRC Id"
        Me.drcId.Name = "drcId"
        Me.drcId.ReadOnly = True
        Me.drcId.Width = 50
        '
        'drcDescription
        '
        Me.drcDescription.DataPropertyName = "oa_drc_libelle"
        Me.drcDescription.HeaderText = "Dénomination DRC"
        Me.drcDescription.Name = "drcDescription"
        Me.drcDescription.ReadOnly = True
        Me.drcDescription.Width = 300
        '
        'categorieMajeure
        '
        Me.categorieMajeure.HeaderText = "Catégorie majeure"
        Me.categorieMajeure.Name = "categorieMajeure"
        Me.categorieMajeure.ReadOnly = True
        Me.categorieMajeure.Width = 250
        '
        'drcOasis
        '
        Me.drcOasis.HeaderText = "ORC Oasis"
        Me.drcOasis.Name = "drcOasis"
        Me.drcOasis.ReadOnly = True
        '
        'oa_drc_sexe
        '
        Me.oa_drc_sexe.DataPropertyName = "oa_drc_sexe"
        Me.oa_drc_sexe.HeaderText = "Applicable"
        Me.oa_drc_sexe.Name = "oa_drc_sexe"
        Me.oa_drc_sexe.ReadOnly = True
        Me.oa_drc_sexe.Visible = False
        '
        'contexte
        '
        Me.contexte.HeaderText = "Contexte"
        Me.contexte.Name = "contexte"
        Me.contexte.ReadOnly = True
        Me.contexte.Visible = False
        '
        'oa_drc_age_min
        '
        Me.oa_drc_age_min.DataPropertyName = "oa_drc_age_min"
        Me.oa_drc_age_min.HeaderText = "Age min"
        Me.oa_drc_age_min.Name = "oa_drc_age_min"
        Me.oa_drc_age_min.ReadOnly = True
        Me.oa_drc_age_min.Visible = False
        Me.oa_drc_age_min.Width = 50
        '
        'oa_drc_age_max
        '
        Me.oa_drc_age_max.DataPropertyName = "oa_drc_age_max"
        Me.oa_drc_age_max.HeaderText = "Age max"
        Me.oa_drc_age_max.Name = "oa_drc_age_max"
        Me.oa_drc_age_max.ReadOnly = True
        Me.oa_drc_age_max.Visible = False
        Me.oa_drc_age_max.Width = 50
        '
        'TxtDrc
        '
        Me.TxtDrc.Location = New System.Drawing.Point(62, 125)
        Me.TxtDrc.Name = "TxtDrc"
        Me.TxtDrc.Size = New System.Drawing.Size(257, 20)
        Me.TxtDrc.TabIndex = 1
        '
        'BtnFiltrer
        '
        Me.BtnFiltrer.Location = New System.Drawing.Point(894, 123)
        Me.BtnFiltrer.Name = "BtnFiltrer"
        Me.BtnFiltrer.Size = New System.Drawing.Size(75, 23)
        Me.BtnFiltrer.TabIndex = 7
        Me.BtnFiltrer.Text = "Filtrer"
        Me.BtnFiltrer.UseVisualStyleBackColor = True
        '
        'BtnInitialiser
        '
        Me.BtnInitialiser.Location = New System.Drawing.Point(979, 123)
        Me.BtnInitialiser.Name = "BtnInitialiser"
        Me.BtnInitialiser.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitialiser.TabIndex = 8
        Me.BtnInitialiser.Text = "Initialiser"
        Me.BtnInitialiser.UseVisualStyleBackColor = True
        '
        'BtnSelection
        '
        Me.BtnSelection.Location = New System.Drawing.Point(22, 253)
        Me.BtnSelection.Name = "BtnSelection"
        Me.BtnSelection.Size = New System.Drawing.Size(75, 23)
        Me.BtnSelection.TabIndex = 0
        Me.BtnSelection.Text = "Sélectionner"
        Me.BtnSelection.UseVisualStyleBackColor = True
        '
        'LblDrcId
        '
        Me.LblDrcId.AutoSize = True
        Me.LblDrcId.Location = New System.Drawing.Point(19, 44)
        Me.LblDrcId.Name = "LblDrcId"
        Me.LblDrcId.Size = New System.Drawing.Size(19, 13)
        Me.LblDrcId.TabIndex = 1
        Me.LblDrcId.Text = "43"
        '
        'LblDrcLibelle
        '
        Me.LblDrcLibelle.AutoSize = True
        Me.LblDrcLibelle.Location = New System.Drawing.Point(92, 44)
        Me.LblDrcLibelle.Name = "LblDrcLibelle"
        Me.LblDrcLibelle.Size = New System.Drawing.Size(40, 13)
        Me.LblDrcLibelle.TabIndex = 2
        Me.LblDrcLibelle.Text = "Brûlure"
        '
        'DrcDefinitionDataGridView
        '
        Me.DrcDefinitionDataGridView.AllowUserToAddRows = False
        Me.DrcDefinitionDataGridView.AllowUserToDeleteRows = False
        Me.DrcDefinitionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DrcDefinitionDataGridView.ColumnHeadersVisible = False
        Me.DrcDefinitionDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.oa_drc_synonyme_libelle})
        Me.DrcDefinitionDataGridView.Location = New System.Drawing.Point(739, 168)
        Me.DrcDefinitionDataGridView.Name = "DrcDefinitionDataGridView"
        Me.DrcDefinitionDataGridView.ReadOnly = True
        Me.DrcDefinitionDataGridView.RowHeadersVisible = False
        Me.DrcDefinitionDataGridView.Size = New System.Drawing.Size(316, 298)
        Me.DrcDefinitionDataGridView.TabIndex = 10
        '
        'oa_drc_synonyme_libelle
        '
        Me.oa_drc_synonyme_libelle.DataPropertyName = "oa_drc_synonyme_libelle"
        Me.oa_drc_synonyme_libelle.HeaderText = "Critères"
        Me.oa_drc_synonyme_libelle.Name = "oa_drc_synonyme_libelle"
        Me.oa_drc_synonyme_libelle.ReadOnly = True
        Me.oa_drc_synonyme_libelle.Width = 400
        '
        'LblLabelDrcAge
        '
        Me.LblLabelDrcAge.AutoSize = True
        Me.LblLabelDrcAge.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelDrcAge.ForeColor = System.Drawing.Color.Red
        Me.LblLabelDrcAge.Location = New System.Drawing.Point(19, 146)
        Me.LblLabelDrcAge.Name = "LblLabelDrcAge"
        Me.LblLabelDrcAge.Size = New System.Drawing.Size(329, 13)
        Me.LblLabelDrcAge.TabIndex = 3
        Me.LblLabelDrcAge.Text = "L'âge du patient est hors des limites données par la DRC"
        '
        'LblAgeMinLbl
        '
        Me.LblAgeMinLbl.AutoSize = True
        Me.LblAgeMinLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAgeMinLbl.Location = New System.Drawing.Point(19, 78)
        Me.LblAgeMinLbl.Name = "LblAgeMinLbl"
        Me.LblAgeMinLbl.Size = New System.Drawing.Size(60, 13)
        Me.LblAgeMinLbl.TabIndex = 4
        Me.LblAgeMinLbl.Text = "Age min :"
        '
        'LblDrcAgeMin
        '
        Me.LblDrcAgeMin.AutoSize = True
        Me.LblDrcAgeMin.Location = New System.Drawing.Point(92, 78)
        Me.LblDrcAgeMin.Name = "LblDrcAgeMin"
        Me.LblDrcAgeMin.Size = New System.Drawing.Size(33, 13)
        Me.LblDrcAgeMin.TabIndex = 5
        Me.LblDrcAgeMin.Text = "5 ans"
        '
        'LblAgeMaxLbl
        '
        Me.LblAgeMaxLbl.AutoSize = True
        Me.LblAgeMaxLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAgeMaxLbl.Location = New System.Drawing.Point(19, 112)
        Me.LblAgeMaxLbl.Name = "LblAgeMaxLbl"
        Me.LblAgeMaxLbl.Size = New System.Drawing.Size(63, 13)
        Me.LblAgeMaxLbl.TabIndex = 6
        Me.LblAgeMaxLbl.Text = "Age max :"
        '
        'LblDrcAgeMax
        '
        Me.LblDrcAgeMax.AutoSize = True
        Me.LblDrcAgeMax.Location = New System.Drawing.Point(92, 112)
        Me.LblDrcAgeMax.Name = "LblDrcAgeMax"
        Me.LblDrcAgeMax.Size = New System.Drawing.Size(45, 13)
        Me.LblDrcAgeMax.TabIndex = 7
        Me.LblDrcAgeMax.Text = "Label10"
        '
        'PnlSelection
        '
        Me.PnlSelection.BackColor = System.Drawing.Color.Cornsilk
        Me.PnlSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlSelection.Controls.Add(Me.LblAgeMinLbl)
        Me.PnlSelection.Controls.Add(Me.LblDrcAgeMax)
        Me.PnlSelection.Controls.Add(Me.BtnSelection)
        Me.PnlSelection.Controls.Add(Me.LblDrcId)
        Me.PnlSelection.Controls.Add(Me.LblAgeMaxLbl)
        Me.PnlSelection.Controls.Add(Me.LblDrcLibelle)
        Me.PnlSelection.Controls.Add(Me.LblDrcAgeMin)
        Me.PnlSelection.Controls.Add(Me.LblLabelDrcAge)
        Me.PnlSelection.Location = New System.Drawing.Point(1061, 167)
        Me.PnlSelection.Name = "PnlSelection"
        Me.PnlSelection.Size = New System.Drawing.Size(363, 298)
        Me.PnlSelection.TabIndex = 11
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
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
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.LblPatientAge)
        Me.GroupBox1.Controls.Add(Me.LblPatientVille)
        Me.GroupBox1.Controls.Add(Me.LblPatientGenre)
        Me.GroupBox1.Controls.Add(Me.LblPatientCodePostal)
        Me.GroupBox1.Controls.Add(Me.LblPatientNIR)
        Me.GroupBox1.Controls.Add(Me.LblPatientAdresse2)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.LblPatientAdresse1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 10)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1412, 71)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Etat civil"
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
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(355, 33)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(37, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Tel. :"
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
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(6, 32)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Adresse :"
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
        'CbxFiltreCategorieMajeure
        '
        Me.CbxFiltreCategorieMajeure.FormattingEnabled = True
        Me.CbxFiltreCategorieMajeure.Location = New System.Drawing.Point(360, 125)
        Me.CbxFiltreCategorieMajeure.Name = "CbxFiltreCategorieMajeure"
        Me.CbxFiltreCategorieMajeure.Size = New System.Drawing.Size(242, 21)
        Me.CbxFiltreCategorieMajeure.TabIndex = 24
        '
        'ChkORC
        '
        Me.ChkORC.AutoSize = True
        Me.ChkORC.Location = New System.Drawing.Point(618, 127)
        Me.ChkORC.Name = "ChkORC"
        Me.ChkORC.Size = New System.Drawing.Size(84, 17)
        Me.ChkORC.TabIndex = 26
        Me.ChkORC.Text = "ORC (Oasis)"
        Me.ChkORC.UseVisualStyleBackColor = True
        '
        'LblCategorieOasis
        '
        Me.LblCategorieOasis.AutoSize = True
        Me.LblCategorieOasis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCategorieOasis.ForeColor = System.Drawing.Color.Red
        Me.LblCategorieOasis.Location = New System.Drawing.Point(440, 94)
        Me.LblCategorieOasis.Name = "LblCategorieOasis"
        Me.LblCategorieOasis.Size = New System.Drawing.Size(96, 13)
        Me.LblCategorieOasis.TabIndex = 27
        Me.LblCategorieOasis.Text = "Catégorie Oasis"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(270, 94)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(164, 13)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "Catégorie Oasis des DRC/ORC : "
        '
        'FDrcSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1436, 481)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblCategorieOasis)
        Me.Controls.Add(Me.ChkORC)
        Me.Controls.Add(Me.CbxFiltreCategorieMajeure)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PnlSelection)
        Me.Controls.Add(Me.DrcDefinitionDataGridView)
        Me.Controls.Add(Me.BtnInitialiser)
        Me.Controls.Add(Me.BtnFiltrer)
        Me.Controls.Add(Me.TxtDrc)
        Me.Controls.Add(Me.DrcDataGridView)
        Me.Name = "FDrcSelecteur"
        Me.Text = "Sélection d'un code DRC"
        CType(Me.DrcDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DrcDefinitionDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSelection.ResumeLayout(False)
        Me.PnlSelection.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DrcDataGridView As DataGridView
    Friend WithEvents TxtDrc As TextBox
    Friend WithEvents BtnFiltrer As Button
    Friend WithEvents BtnInitialiser As Button
    Friend WithEvents LblDrcLibelle As Label
    Friend WithEvents LblDrcId As Label
    Friend WithEvents BtnSelection As Button
    Friend WithEvents DrcDefinitionDataGridView As DataGridView
    Friend WithEvents LblDrcAgeMax As Label
    Friend WithEvents LblAgeMaxLbl As Label
    Friend WithEvents LblDrcAgeMin As Label
    Friend WithEvents LblAgeMinLbl As Label
    Friend WithEvents LblLabelDrcAge As Label
    Friend WithEvents PnlSelection As Panel
    Friend WithEvents GroupBox1 As GroupBox
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
    Friend WithEvents Label7 As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents LblPatientVille As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientCodePostal As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents LblPatientAdresse2 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents LblPatientAdresse1 As Label
    Friend WithEvents CbxFiltreCategorieMajeure As ComboBox
    Friend WithEvents ChkORC As CheckBox
    Friend WithEvents drcId As DataGridViewTextBoxColumn
    Friend WithEvents drcDescription As DataGridViewTextBoxColumn
    Friend WithEvents categorieMajeure As DataGridViewTextBoxColumn
    Friend WithEvents drcOasis As DataGridViewTextBoxColumn
    Friend WithEvents oa_drc_sexe As DataGridViewTextBoxColumn
    Friend WithEvents contexte As DataGridViewTextBoxColumn
    Friend WithEvents oa_drc_age_min As DataGridViewTextBoxColumn
    Friend WithEvents oa_drc_age_max As DataGridViewTextBoxColumn
    Friend WithEvents oa_drc_synonyme_libelle As DataGridViewTextBoxColumn
    Friend WithEvents LblCategorieOasis As Label
    Friend WithEvents Label1 As Label
End Class
