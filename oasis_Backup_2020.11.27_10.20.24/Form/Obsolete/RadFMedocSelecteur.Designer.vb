<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFMedocSelecteur
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
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadMedicamentGridView = New Telerik.WinControls.UI.RadGridView()
        Me.ContextMenuStripMedicament = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DétailMédicamentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VmedocBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.OasisDataSet2 = New Oasis_WF.oasisDataSet2()
        Me.RadPnlSelectedMedicament = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblMedicamentCis = New System.Windows.Forms.Label()
        Me.LblMedicamentDci = New System.Windows.Forms.Label()
        Me.LblMedicamentForme = New System.Windows.Forms.Label()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.lblContreIndication = New System.Windows.Forms.Label()
        Me.LblALD = New System.Windows.Forms.Label()
        Me.LblAllergie = New System.Windows.Forms.Label()
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
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LblPatientAdresse1 = New System.Windows.Forms.Label()
        Me.RadBtnSelect = New Telerik.WinControls.UI.RadButton()
        Me.V_medocTableAdapter = New Oasis_WF.oasisDataSet2TableAdapters.v_medocTableAdapter()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnDetailMedoc = New Telerik.WinControls.UI.RadButton()
        Me.TxtFiltreDenomination = New Telerik.WinControls.UI.RadTextBox()
        Me.LblLabelFiltreAffichage = New System.Windows.Forms.Label()
        Me.LblAffichage = New System.Windows.Forms.Label()
        Me.RadBtnFiltre = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadMedicamentGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadMedicamentGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripMedicament.SuspendLayout()
        CType(Me.VmedocBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OasisDataSet2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPnlSelectedMedicament, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPnlSelectedMedicament.SuspendLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnDetailMedoc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxtFiltreDenomination, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnFiltre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadMedicamentGridView
        '
        Me.RadMedicamentGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadMedicamentGridView.ContextMenuStrip = Me.ContextMenuStripMedicament
        Me.RadMedicamentGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadMedicamentGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadMedicamentGridView.ForeColor = System.Drawing.Color.Black
        Me.RadMedicamentGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadMedicamentGridView.Location = New System.Drawing.Point(12, 148)
        '
        '
        '
        Me.RadMedicamentGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadMedicamentGridView.MasterTemplate.AllowCellContextMenu = False
        Me.RadMedicamentGridView.MasterTemplate.AllowDeleteRow = False
        Me.RadMedicamentGridView.MasterTemplate.AllowEditRow = False
        GridViewDecimalColumn1.DataType = GetType(Integer)
        GridViewDecimalColumn1.EnableExpressionEditor = False
        GridViewDecimalColumn1.FieldName = "oa_medicament_cis"
        GridViewDecimalColumn1.HeaderText = "CIS"
        GridViewDecimalColumn1.IsAutoGenerated = True
        GridViewDecimalColumn1.IsVisible = False
        GridViewDecimalColumn1.Name = "oa_medicament_cis"
        GridViewDecimalColumn1.ReadOnly = True
        GridViewDecimalColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDecimalColumn1.Width = 70
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "oa_medicament_dci"
        GridViewTextBoxColumn1.HeaderText = "Dénomination"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.IsAutoGenerated = True
        GridViewTextBoxColumn1.Name = "oa_medicament_dci"
        GridViewTextBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.Width = 500
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "oa_medicament_forme"
        GridViewTextBoxColumn2.HeaderText = "Forme"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.IsAutoGenerated = True
        GridViewTextBoxColumn2.Name = "oa_medicament_forme"
        GridViewTextBoxColumn2.ReadOnly = True
        GridViewTextBoxColumn2.Width = 300
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "oa_medicament_voie_administration"
        GridViewTextBoxColumn3.HeaderText = "Voie administration"
        GridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn3.IsAutoGenerated = True
        GridViewTextBoxColumn3.Name = "oa_medicament_voie_administration"
        GridViewTextBoxColumn3.ReadOnly = True
        GridViewTextBoxColumn3.Width = 200
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "oa_medicament_titulaire"
        GridViewTextBoxColumn4.HeaderText = "Titulaire"
        GridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn4.IsAutoGenerated = True
        GridViewTextBoxColumn4.Name = "oa_medicament_titulaire"
        GridViewTextBoxColumn4.ReadOnly = True
        GridViewTextBoxColumn4.Width = 200
        Me.RadMedicamentGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn1, GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4})
        Me.RadMedicamentGridView.MasterTemplate.DataSource = Me.VmedocBindingSource
        Me.RadMedicamentGridView.MasterTemplate.EnableFiltering = True
        Me.RadMedicamentGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadMedicamentGridView.Name = "RadMedicamentGridView"
        Me.RadMedicamentGridView.ReadOnly = True
        Me.RadMedicamentGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadMedicamentGridView.Size = New System.Drawing.Size(1247, 329)
        Me.RadMedicamentGridView.TabIndex = 0
        '
        'ContextMenuStripMedicament
        '
        Me.ContextMenuStripMedicament.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DétailMédicamentToolStripMenuItem})
        Me.ContextMenuStripMedicament.Name = "ContextMenuStripMedicament"
        Me.ContextMenuStripMedicament.Size = New System.Drawing.Size(175, 26)
        '
        'DétailMédicamentToolStripMenuItem
        '
        Me.DétailMédicamentToolStripMenuItem.Name = "DétailMédicamentToolStripMenuItem"
        Me.DétailMédicamentToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.DétailMédicamentToolStripMenuItem.Text = "Détail médicament"
        '
        'VmedocBindingSource
        '
        Me.VmedocBindingSource.DataMember = "v_medoc"
        Me.VmedocBindingSource.DataSource = Me.OasisDataSet2
        '
        'OasisDataSet2
        '
        Me.OasisDataSet2.DataSetName = "oasisDataSet2"
        Me.OasisDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'RadPnlSelectedMedicament
        '
        Me.RadPnlSelectedMedicament.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadPnlSelectedMedicament.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadPnlSelectedMedicament.Controls.Add(Me.LblMedicamentCis)
        Me.RadPnlSelectedMedicament.Controls.Add(Me.LblMedicamentDci)
        Me.RadPnlSelectedMedicament.Controls.Add(Me.LblMedicamentForme)
        Me.RadPnlSelectedMedicament.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadPnlSelectedMedicament.HeaderText = "Médicament sélectionné"
        Me.RadPnlSelectedMedicament.Location = New System.Drawing.Point(12, 483)
        Me.RadPnlSelectedMedicament.Name = "RadPnlSelectedMedicament"
        Me.RadPnlSelectedMedicament.Size = New System.Drawing.Size(1247, 106)
        Me.RadPnlSelectedMedicament.TabIndex = 1
        Me.RadPnlSelectedMedicament.Text = "Médicament sélectionné"
        '
        'LblMedicamentCis
        '
        Me.LblMedicamentCis.AutoSize = True
        Me.LblMedicamentCis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMedicamentCis.Location = New System.Drawing.Point(5, 29)
        Me.LblMedicamentCis.Name = "LblMedicamentCis"
        Me.LblMedicamentCis.Size = New System.Drawing.Size(63, 13)
        Me.LblMedicamentCis.TabIndex = 12
        Me.LblMedicamentCis.Text = "60005856"
        '
        'LblMedicamentDci
        '
        Me.LblMedicamentDci.AutoSize = True
        Me.LblMedicamentDci.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMedicamentDci.Location = New System.Drawing.Point(5, 51)
        Me.LblMedicamentDci.Name = "LblMedicamentDci"
        Me.LblMedicamentDci.Size = New System.Drawing.Size(657, 13)
        Me.LblMedicamentDci.TabIndex = 13
        Me.LblMedicamentDci.Text = "RHODODENDRON FERRUGINEUM WELEDA, degre de dilution compris entre 2CH et 30CH ou e" &
    "ntre 4DH et 60DH"
        '
        'LblMedicamentForme
        '
        Me.LblMedicamentForme.AutoSize = True
        Me.LblMedicamentForme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMedicamentForme.Location = New System.Drawing.Point(5, 75)
        Me.LblMedicamentForme.Name = "LblMedicamentForme"
        Me.LblMedicamentForme.Size = New System.Drawing.Size(250, 13)
        Me.LblMedicamentForme.TabIndex = 14
        Me.LblMedicamentForme.Text = "granules et  solution en gouttes en gouttes"
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBox2.Controls.Add(Me.lblContreIndication)
        Me.RadGroupBox2.Controls.Add(Me.LblALD)
        Me.RadGroupBox2.Controls.Add(Me.LblAllergie)
        Me.RadGroupBox2.Controls.Add(Me.Label13)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientDateMaj)
        Me.RadGroupBox2.Controls.Add(Me.Label5)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientUniteSanitaire)
        Me.RadGroupBox2.Controls.Add(Me.Label6)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientSite)
        Me.RadGroupBox2.Controls.Add(Me.Label4)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientTel2)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientPrenom)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientTel1)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBox2.Controls.Add(Me.Label3)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientVille)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientGenre)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientCodePostal)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientNIR)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientAdresse2)
        Me.RadGroupBox2.Controls.Add(Me.Label7)
        Me.RadGroupBox2.Controls.Add(Me.LblPatientAdresse1)
        Me.RadGroupBox2.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox2.HeaderText = "Etat civil"
        Me.RadGroupBox2.Location = New System.Drawing.Point(12, 7)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(1247, 77)
        Me.RadGroupBox2.TabIndex = 2
        Me.RadGroupBox2.Text = "Etat civil"
        '
        'lblContreIndication
        '
        Me.lblContreIndication.AutoSize = True
        Me.lblContreIndication.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContreIndication.ForeColor = System.Drawing.Color.Crimson
        Me.lblContreIndication.Location = New System.Drawing.Point(1021, 51)
        Me.lblContreIndication.Name = "lblContreIndication"
        Me.lblContreIndication.Size = New System.Drawing.Size(117, 13)
        Me.lblContreIndication.TabIndex = 18
        Me.lblContreIndication.Text = "Contre-indication(s)"
        '
        'LblALD
        '
        Me.LblALD.AutoSize = True
        Me.LblALD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblALD.ForeColor = System.Drawing.Color.Crimson
        Me.LblALD.Location = New System.Drawing.Point(941, 18)
        Me.LblALD.Name = "LblALD"
        Me.LblALD.Size = New System.Drawing.Size(31, 13)
        Me.LblALD.TabIndex = 64
        Me.LblALD.Text = "ALD"
        '
        'LblAllergie
        '
        Me.LblAllergie.AutoSize = True
        Me.LblAllergie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAllergie.ForeColor = System.Drawing.Color.Crimson
        Me.LblAllergie.Location = New System.Drawing.Point(941, 51)
        Me.LblAllergie.Name = "LblAllergie"
        Me.LblAllergie.Size = New System.Drawing.Size(63, 13)
        Me.LblAllergie.TabIndex = 17
        Me.LblAllergie.Text = "Allergie(s)"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(504, 18)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 63
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(795, 18)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientDateMaj.TabIndex = 62
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(661, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientUniteSanitaire
        '
        Me.LblPatientUniteSanitaire.AutoSize = True
        Me.LblPatientUniteSanitaire.Location = New System.Drawing.Point(842, 51)
        Me.LblPatientUniteSanitaire.Name = "LblPatientUniteSanitaire"
        Me.LblPatientUniteSanitaire.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientUniteSanitaire.TabIndex = 60
        Me.LblPatientUniteSanitaire.Text = "Auxerre"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(661, 51)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(175, 13)
        Me.Label6.TabIndex = 59
        Me.Label6.Text = "Centre médical de référence :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(704, 35)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(36, 13)
        Me.LblPatientSite.TabIndex = 58
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(661, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 57
        Me.Label4.Text = "Site :"
        '
        'LblPatientTel2
        '
        Me.LblPatientTel2.AutoSize = True
        Me.LblPatientTel2.Location = New System.Drawing.Point(397, 51)
        Me.LblPatientTel2.Name = "LblPatientTel2"
        Me.LblPatientTel2.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel2.TabIndex = 56
        Me.LblPatientTel2.Text = "0968542357"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(5, 18)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientPrenom.TabIndex = 44
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientTel1
        '
        Me.LblPatientTel1.AutoSize = True
        Me.LblPatientTel1.Location = New System.Drawing.Point(397, 35)
        Me.LblPatientTel1.Name = "LblPatientTel1"
        Me.LblPatientTel1.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel1.TabIndex = 55
        Me.LblPatientTel1.Text = "0288425678"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(127, 18)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientNom.TabIndex = 45
        Me.LblPatientNom.Text = "Durand"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(354, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 54
        Me.Label3.Text = "Tel. :"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(310, 18)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 13)
        Me.LblPatientAge.TabIndex = 46
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientVille
        '
        Me.LblPatientVille.AutoSize = True
        Me.LblPatientVille.Location = New System.Drawing.Point(119, 51)
        Me.LblPatientVille.Name = "LblPatientVille"
        Me.LblPatientVille.Size = New System.Drawing.Size(57, 13)
        Me.LblPatientVille.TabIndex = 53
        Me.LblPatientVille.Text = "Lournand"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(420, 18)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientGenre.TabIndex = 47
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientCodePostal
        '
        Me.LblPatientCodePostal.AutoSize = True
        Me.LblPatientCodePostal.Location = New System.Drawing.Point(76, 51)
        Me.LblPatientCodePostal.Name = "LblPatientCodePostal"
        Me.LblPatientCodePostal.Size = New System.Drawing.Size(37, 13)
        Me.LblPatientCodePostal.TabIndex = 52
        Me.LblPatientCodePostal.Text = "71250"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(555, 18)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 48
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'LblPatientAdresse2
        '
        Me.LblPatientAdresse2.AutoSize = True
        Me.LblPatientAdresse2.Location = New System.Drawing.Point(230, 34)
        Me.LblPatientAdresse2.Name = "LblPatientAdresse2"
        Me.LblPatientAdresse2.Size = New System.Drawing.Size(55, 13)
        Me.LblPatientAdresse2.TabIndex = 51
        Me.LblPatientAdresse2.Text = "adresse 2"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(5, 34)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(60, 13)
        Me.Label7.TabIndex = 49
        Me.Label7.Text = "Adresse :"
        '
        'LblPatientAdresse1
        '
        Me.LblPatientAdresse1.AutoSize = True
        Me.LblPatientAdresse1.Location = New System.Drawing.Point(76, 34)
        Me.LblPatientAdresse1.Name = "LblPatientAdresse1"
        Me.LblPatientAdresse1.Size = New System.Drawing.Size(121, 13)
        Me.LblPatientAdresse1.TabIndex = 50
        Me.LblPatientAdresse1.Text = "3 rue de la république"
        '
        'RadBtnSelect
        '
        Me.RadBtnSelect.Location = New System.Drawing.Point(12, 597)
        Me.RadBtnSelect.Name = "RadBtnSelect"
        Me.RadBtnSelect.Size = New System.Drawing.Size(138, 24)
        Me.RadBtnSelect.TabIndex = 15
        Me.RadBtnSelect.Text = "Confirmer la sélection"
        '
        'V_medocTableAdapter
        '
        Me.V_medocTableAdapter.ClearBeforeFill = True
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1149, 597)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 16
        Me.RadBtnAbandon.Text = "Abandonner"
        '
        'RadBtnDetailMedoc
        '
        Me.RadBtnDetailMedoc.Location = New System.Drawing.Point(1033, 597)
        Me.RadBtnDetailMedoc.Name = "RadBtnDetailMedoc"
        Me.RadBtnDetailMedoc.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnDetailMedoc.TabIndex = 17
        Me.RadBtnDetailMedoc.Text = "Détail médicament"
        '
        'TxtFiltreDenomination
        '
        Me.TxtFiltreDenomination.Location = New System.Drawing.Point(129, 103)
        Me.TxtFiltreDenomination.Name = "TxtFiltreDenomination"
        Me.TxtFiltreDenomination.ShowClearButton = True
        Me.TxtFiltreDenomination.Size = New System.Drawing.Size(326, 20)
        Me.TxtFiltreDenomination.TabIndex = 18
        '
        'LblLabelFiltreAffichage
        '
        Me.LblLabelFiltreAffichage.AutoSize = True
        Me.LblLabelFiltreAffichage.Location = New System.Drawing.Point(12, 105)
        Me.LblLabelFiltreAffichage.Name = "LblLabelFiltreAffichage"
        Me.LblLabelFiltreAffichage.Size = New System.Drawing.Size(109, 13)
        Me.LblLabelFiltreAffichage.TabIndex = 19
        Me.LblLabelFiltreAffichage.Text = "Filtre dénomination"
        '
        'LblAffichage
        '
        Me.LblAffichage.AutoSize = True
        Me.LblAffichage.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblAffichage.ForeColor = System.Drawing.Color.Red
        Me.LblAffichage.Location = New System.Drawing.Point(623, 105)
        Me.LblAffichage.Name = "LblAffichage"
        Me.LblAffichage.Size = New System.Drawing.Size(58, 13)
        Me.LblAffichage.TabIndex = 20
        Me.LblAffichage.Text = "Veuillez ..."
        '
        'RadBtnFiltre
        '
        Me.RadBtnFiltre.Location = New System.Drawing.Point(479, 101)
        Me.RadBtnFiltre.Name = "RadBtnFiltre"
        Me.RadBtnFiltre.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnFiltre.TabIndex = 18
        Me.RadBtnFiltre.Text = "Filtrer"
        '
        'RadFMedocSelecteur
        '
        Me.AcceptButton = Me.RadBtnFiltre
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1271, 636)
        Me.Controls.Add(Me.RadBtnFiltre)
        Me.Controls.Add(Me.LblAffichage)
        Me.Controls.Add(Me.LblLabelFiltreAffichage)
        Me.Controls.Add(Me.TxtFiltreDenomination)
        Me.Controls.Add(Me.RadBtnDetailMedoc)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadBtnSelect)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Controls.Add(Me.RadPnlSelectedMedicament)
        Me.Controls.Add(Me.RadMedicamentGridView)
        Me.Name = "RadFMedocSelecteur"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sélecteur médicament"
        CType(Me.RadMedicamentGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadMedicamentGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripMedicament.ResumeLayout(False)
        CType(Me.VmedocBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OasisDataSet2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPnlSelectedMedicament, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPnlSelectedMedicament.ResumeLayout(False)
        Me.RadPnlSelectedMedicament.PerformLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.RadGroupBox2.PerformLayout()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnDetailMedoc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxtFiltreDenomination, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnFiltre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadMedicamentGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadPnlSelectedMedicament As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblMedicamentCis As Label
    Friend WithEvents LblMedicamentDci As Label
    Friend WithEvents LblMedicamentForme As Label
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadBtnSelect As Telerik.WinControls.UI.RadButton
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
    Friend WithEvents Label7 As Label
    Friend WithEvents LblPatientAdresse1 As Label
    Friend WithEvents ContextMenuStripMedicament As ContextMenuStrip
    Friend WithEvents DétailMédicamentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OasisDataSet2 As oasisDataSet2
    Friend WithEvents VmedocBindingSource As BindingSource
    Friend WithEvents V_medocTableAdapter As oasisDataSet2TableAdapters.v_medocTableAdapter
    Friend WithEvents lblContreIndication As Label
    Friend WithEvents LblAllergie As Label
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnDetailMedoc As Telerik.WinControls.UI.RadButton
    Friend WithEvents TxtFiltreDenomination As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents LblLabelFiltreAffichage As Label
    Friend WithEvents LblAffichage As Label
    Friend WithEvents RadBtnFiltre As Telerik.WinControls.UI.RadButton
End Class

