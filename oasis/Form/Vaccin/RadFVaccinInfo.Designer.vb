<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFVaccinInfo
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
        Dim GridViewCheckBoxColumn1 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor1 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewCheckBoxColumn2 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor2 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewCheckBoxColumn3 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor3 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition3 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.GVValence = New Telerik.WinControls.UI.RadGridView()
        Me.GVVaccin = New Telerik.WinControls.UI.RadGridView()
        Me.BtnValidationProgram = New Telerik.WinControls.UI.RadButton()
        Me.BtnPrintOrdo = New Telerik.WinControls.UI.RadButton()
        Me.BtnAdminVaccin = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblOperator = New System.Windows.Forms.Label()
        Me.LblDate = New System.Windows.Forms.Label()
        Me.LabelStaticOperator = New System.Windows.Forms.Label()
        Me.LabelStaticDate = New System.Windows.Forms.Label()
        Me.RadGroupBoxEtatCivil = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblDateNaissance = New System.Windows.Forms.Label()
        Me.LblNonOasis = New System.Windows.Forms.Label()
        Me.LblALD = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LblPatientDateMaj = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblPatientSite = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.LblContreIndication = New System.Windows.Forms.Label()
        Me.LblAllergie = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.LblAgeVaccination = New Telerik.WinControls.UI.RadLabel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.TraitementContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUnTraitementToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoriqueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TraitementsObsoletesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.DéclarationAllergieToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.DéclarationAllergieOuContreindicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.GVValence, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVValence.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVVaccin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVVaccin.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnValidationProgram, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnPrintOrdo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnAdminVaccin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBoxEtatCivil.SuspendLayout()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LblAgeVaccination, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TraitementContextMenuStrip.SuspendLayout()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GVValence
        '
        Me.GVValence.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GVValence.Cursor = System.Windows.Forms.Cursors.Default
        Me.GVValence.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.GVValence.ForeColor = System.Drawing.Color.Black
        Me.GVValence.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.GVValence.Location = New System.Drawing.Point(742, 56)
        '
        '
        '
        GridViewCheckBoxColumn1.EnableExpressionEditor = False
        GridViewCheckBoxColumn1.MinWidth = 20
        GridViewCheckBoxColumn1.Name = "checked"
        GridViewCheckBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "Nom"
        GridViewTextBoxColumn1.Name = "nom"
        GridViewTextBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.Width = 180
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "id"
        GridViewTextBoxColumn2.IsVisible = False
        GridViewTextBoxColumn2.Name = "id"
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "valence"
        GridViewTextBoxColumn3.IsVisible = False
        GridViewTextBoxColumn3.Name = "valence"
        GridViewTextBoxColumn3.ReadOnly = True
        GridViewTextBoxColumn3.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending
        Me.GVValence.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewCheckBoxColumn1, GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3})
        Me.GVValence.MasterTemplate.ReadOnly = True
        Me.GVValence.MasterTemplate.ShowRowHeaderColumn = False
        SortDescriptor1.PropertyName = "valence"
        Me.GVValence.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor1})
        Me.GVValence.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.GVValence.Name = "GVValence"
        Me.GVValence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GVValence.ShowGroupPanel = False
        Me.GVValence.Size = New System.Drawing.Size(270, 596)
        Me.GVValence.TabIndex = 0
        '
        'GVVaccin
        '
        Me.GVVaccin.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GVVaccin.Cursor = System.Windows.Forms.Cursors.Default
        Me.GVVaccin.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.GVVaccin.ForeColor = System.Drawing.Color.Black
        Me.GVVaccin.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.GVVaccin.Location = New System.Drawing.Point(15, 160)
        '
        '
        '
        Me.GVVaccin.MasterTemplate.AllowAddNewRow = False
        Me.GVVaccin.MasterTemplate.AllowDeleteRow = False
        GridViewCheckBoxColumn2.EnableExpressionEditor = False
        GridViewCheckBoxColumn2.MinWidth = 20
        GridViewCheckBoxColumn2.Name = "checked"
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "Categorie"
        GridViewTextBoxColumn4.Name = "category"
        GridViewTextBoxColumn4.Width = 80
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.HeaderText = "Vaccins"
        GridViewTextBoxColumn5.Name = "dci"
        GridViewTextBoxColumn5.Width = 500
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.HeaderText = "id"
        GridViewTextBoxColumn6.IsVisible = False
        GridViewTextBoxColumn6.Name = "id"
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.HeaderText = "valence"
        GridViewTextBoxColumn7.IsVisible = False
        GridViewTextBoxColumn7.Name = "valence"
        Me.GVVaccin.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewCheckBoxColumn2, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7})
        Me.GVVaccin.MasterTemplate.EnableGrouping = False
        Me.GVVaccin.MasterTemplate.EnableSorting = False
        Me.GVVaccin.MasterTemplate.ShowRowHeaderColumn = False
        SortDescriptor2.PropertyName = "vaccin"
        Me.GVVaccin.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor2})
        Me.GVVaccin.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.GVVaccin.Name = "GVVaccin"
        Me.GVVaccin.ReadOnly = True
        Me.GVVaccin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GVVaccin.ShowGroupPanel = False
        Me.GVVaccin.Size = New System.Drawing.Size(701, 295)
        Me.GVVaccin.TabIndex = 1
        '
        'BtnValidationProgram
        '
        Me.BtnValidationProgram.Location = New System.Drawing.Point(15, 658)
        Me.BtnValidationProgram.Name = "BtnValidationProgram"
        Me.BtnValidationProgram.Size = New System.Drawing.Size(240, 24)
        Me.BtnValidationProgram.TabIndex = 2
        Me.BtnValidationProgram.Text = "Valider la programmation"
        '
        'BtnPrintOrdo
        '
        Me.BtnPrintOrdo.Location = New System.Drawing.Point(261, 658)
        Me.BtnPrintOrdo.Name = "BtnPrintOrdo"
        Me.BtnPrintOrdo.Size = New System.Drawing.Size(240, 24)
        Me.BtnPrintOrdo.TabIndex = 3
        Me.BtnPrintOrdo.Text = "Imprimer l'ordonnance"
        '
        'BtnAdminVaccin
        '
        Me.BtnAdminVaccin.Location = New System.Drawing.Point(742, 658)
        Me.BtnAdminVaccin.Name = "BtnAdminVaccin"
        Me.BtnAdminVaccin.Size = New System.Drawing.Size(271, 24)
        Me.BtnAdminVaccin.TabIndex = 4
        Me.BtnAdminVaccin.Text = "Administer les vaccins"
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.LblOperator)
        Me.RadGroupBox1.Controls.Add(Me.LblDate)
        Me.RadGroupBox1.Controls.Add(Me.LabelStaticOperator)
        Me.RadGroupBox1.Controls.Add(Me.LabelStaticDate)
        Me.RadGroupBox1.HeaderText = ""
        Me.RadGroupBox1.Location = New System.Drawing.Point(15, 79)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(701, 75)
        Me.RadGroupBox1.TabIndex = 5
        '
        'LblOperator
        '
        Me.LblOperator.AutoSize = True
        Me.LblOperator.Location = New System.Drawing.Point(76, 47)
        Me.LblOperator.Name = "LblOperator"
        Me.LblOperator.Size = New System.Drawing.Size(15, 13)
        Me.LblOperator.TabIndex = 3
        Me.LblOperator.Text = "--"
        '
        'LblDate
        '
        Me.LblDate.AutoSize = True
        Me.LblDate.Location = New System.Drawing.Point(76, 22)
        Me.LblDate.Name = "LblDate"
        Me.LblDate.Size = New System.Drawing.Size(15, 13)
        Me.LblDate.TabIndex = 2
        Me.LblDate.Text = "--"
        '
        'LabelStaticOperator
        '
        Me.LabelStaticOperator.AutoSize = True
        Me.LabelStaticOperator.Location = New System.Drawing.Point(6, 47)
        Me.LabelStaticOperator.Name = "LabelStaticOperator"
        Me.LabelStaticOperator.Size = New System.Drawing.Size(60, 13)
        Me.LabelStaticOperator.TabIndex = 1
        Me.LabelStaticOperator.Text = "Operateur"
        '
        'LabelStaticDate
        '
        Me.LabelStaticDate.AutoSize = True
        Me.LabelStaticDate.Location = New System.Drawing.Point(6, 22)
        Me.LabelStaticDate.Name = "LabelStaticDate"
        Me.LabelStaticDate.Size = New System.Drawing.Size(31, 13)
        Me.LabelStaticDate.TabIndex = 0
        Me.LabelStaticDate.Text = "Date"
        '
        'RadGroupBoxEtatCivil
        '
        Me.RadGroupBoxEtatCivil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBoxEtatCivil.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblDateNaissance)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblNonOasis)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblALD)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label13)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientDateMaj)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label5)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientSite)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label1)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientPrenom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientGenre)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNIR)
        Me.RadGroupBoxEtatCivil.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBoxEtatCivil.HeaderText = ""
        Me.RadGroupBoxEtatCivil.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBoxEtatCivil.Name = "RadGroupBoxEtatCivil"
        Me.RadGroupBoxEtatCivil.Size = New System.Drawing.Size(1000, 38)
        Me.RadGroupBoxEtatCivil.TabIndex = 6
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.GroupBoxContent).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        '
        'LblDateNaissance
        '
        Me.LblDateNaissance.AutoSize = True
        Me.LblDateNaissance.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblDateNaissance.ForeColor = System.Drawing.Color.Red
        Me.LblDateNaissance.Location = New System.Drawing.Point(339, 2)
        Me.LblDateNaissance.Name = "LblDateNaissance"
        Me.LblDateNaissance.Size = New System.Drawing.Size(65, 15)
        Me.LblDateNaissance.TabIndex = 45
        Me.LblDateNaissance.Text = "25-04-2018"
        '
        'LblNonOasis
        '
        Me.LblNonOasis.AutoSize = True
        Me.LblNonOasis.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.LblNonOasis.ForeColor = System.Drawing.Color.Red
        Me.LblNonOasis.Location = New System.Drawing.Point(231, 17)
        Me.LblNonOasis.Name = "LblNonOasis"
        Me.LblNonOasis.Size = New System.Drawing.Size(432, 19)
        Me.LblNonOasis.TabIndex = 44
        Me.LblNonOasis.Text = "++++++++++ !!! Patient hors système Oasis !!! ++++++++++"
        '
        'LblALD
        '
        Me.LblALD.AutoSize = True
        Me.LblALD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblALD.ForeColor = System.Drawing.Color.OrangeRed
        Me.LblALD.Location = New System.Drawing.Point(947, 2)
        Me.LblALD.Name = "LblALD"
        Me.LblALD.Size = New System.Drawing.Size(31, 13)
        Me.LblALD.TabIndex = 43
        Me.LblALD.Text = "ALD"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(588, 2)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 42
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(879, 2)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientDateMaj.TabIndex = 41
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(745, 2)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(710, 21)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(36, 13)
        Me.LblPatientSite.TabIndex = 37
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(667, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "Site :"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.LblPatientPrenom.ForeColor = System.Drawing.Color.Red
        Me.LblPatientPrenom.Location = New System.Drawing.Point(10, 2)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(73, 17)
        Me.LblPatientPrenom.TabIndex = 23
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.LblPatientNom.ForeColor = System.Drawing.Color.Red
        Me.LblPatientNom.Location = New System.Drawing.Point(136, 2)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(51, 17)
        Me.LblPatientNom.TabIndex = 24
        Me.LblPatientNom.Text = "Durand"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblPatientAge.ForeColor = System.Drawing.Color.DarkRed
        Me.LblPatientAge.Location = New System.Drawing.Point(412, 2)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 15)
        Me.LblPatientAge.TabIndex = 25
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblPatientGenre.ForeColor = System.Drawing.Color.DarkRed
        Me.LblPatientGenre.Location = New System.Drawing.Point(505, 2)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(55, 15)
        Me.LblPatientGenre.TabIndex = 26
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(639, 2)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 27
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'LblContreIndication
        '
        Me.LblContreIndication.AutoSize = True
        Me.LblContreIndication.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblContreIndication.ForeColor = System.Drawing.Color.OrangeRed
        Me.LblContreIndication.Location = New System.Drawing.Point(320, 58)
        Me.LblContreIndication.Name = "LblContreIndication"
        Me.LblContreIndication.Size = New System.Drawing.Size(117, 13)
        Me.LblContreIndication.TabIndex = 34
        Me.LblContreIndication.Text = "Contre-indication(s)"
        '
        'LblAllergie
        '
        Me.LblAllergie.AutoSize = True
        Me.LblAllergie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAllergie.ForeColor = System.Drawing.Color.OrangeRed
        Me.LblAllergie.Location = New System.Drawing.Point(453, 58)
        Me.LblAllergie.Name = "LblAllergie"
        Me.LblAllergie.Size = New System.Drawing.Size(67, 13)
        Me.LblAllergie.TabIndex = 33
        Me.LblAllergie.Text = "Allergie(s) "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.OrangeRed
        Me.Label2.Location = New System.Drawing.Point(526, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(190, 13)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "Allergie(s)  non medicamenteuse"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(13, 58)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(111, 18)
        Me.RadLabel1.TabIndex = 36
        Me.RadLabel1.Text = "Age a la vaccination :"
        '
        'LblAgeVaccination
        '
        Me.LblAgeVaccination.Location = New System.Drawing.Point(130, 58)
        Me.LblAgeVaccination.Name = "LblAgeVaccination"
        Me.LblAgeVaccination.Size = New System.Drawing.Size(15, 18)
        Me.LblAgeVaccination.TabIndex = 37
        Me.LblAgeVaccination.Text = "--"
        '
        'TraitementContextMenuStrip
        '
        Me.TraitementContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerUnTraitementToolStripMenuItem1, Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem, Me.HistoriqueToolStripMenuItem, Me.ToolStripSeparator1, Me.TraitementsObsoletesToolStripMenuItem, Me.ToolStripSeparator2, Me.DéclarationAllergieToolStripMenuItem, Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem, Me.ToolStripSeparator4, Me.DéclarationAllergieOuContreindicationToolStripMenuItem, Me.ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem})
        Me.TraitementContextMenuStrip.Name = "TraitementContextMenuStrip"
        Me.TraitementContextMenuStrip.Size = New System.Drawing.Size(346, 198)
        '
        'CréerUnTraitementToolStripMenuItem1
        '
        Me.CréerUnTraitementToolStripMenuItem1.Name = "CréerUnTraitementToolStripMenuItem1"
        Me.CréerUnTraitementToolStripMenuItem1.Size = New System.Drawing.Size(345, 22)
        Me.CréerUnTraitementToolStripMenuItem1.Text = "Créer un traitement"
        '
        'GérerUneFenêtreThérapeutiqueToolStripMenuItem
        '
        Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem.Name = "GérerUneFenêtreThérapeutiqueToolStripMenuItem"
        Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem.Size = New System.Drawing.Size(345, 22)
        Me.GérerUneFenêtreThérapeutiqueToolStripMenuItem.Text = "Gérer une fenêtre thérapeutique pour un traitement"
        '
        'HistoriqueToolStripMenuItem
        '
        Me.HistoriqueToolStripMenuItem.Name = "HistoriqueToolStripMenuItem"
        Me.HistoriqueToolStripMenuItem.Size = New System.Drawing.Size(345, 22)
        Me.HistoriqueToolStripMenuItem.Text = "Historique des modifications d'un traitement"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(342, 6)
        '
        'TraitementsObsoletesToolStripMenuItem
        '
        Me.TraitementsObsoletesToolStripMenuItem.Name = "TraitementsObsoletesToolStripMenuItem"
        Me.TraitementsObsoletesToolStripMenuItem.Size = New System.Drawing.Size(345, 22)
        Me.TraitementsObsoletesToolStripMenuItem.Text = "Afficher les traitements obsolètes"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(342, 6)
        '
        'DéclarationAllergieToolStripMenuItem
        '
        Me.DéclarationAllergieToolStripMenuItem.Name = "DéclarationAllergieToolStripMenuItem"
        Me.DéclarationAllergieToolStripMenuItem.Size = New System.Drawing.Size(345, 22)
        Me.DéclarationAllergieToolStripMenuItem.Text = "Déclaration allergie"
        '
        'ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem
        '
        Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Name = "ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem"
        Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Size = New System.Drawing.Size(345, 22)
        Me.ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Text = "Liste des substances allergiques"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(342, 6)
        '
        'DéclarationAllergieOuContreindicationToolStripMenuItem
        '
        Me.DéclarationAllergieOuContreindicationToolStripMenuItem.Name = "DéclarationAllergieOuContreindicationToolStripMenuItem"
        Me.DéclarationAllergieOuContreindicationToolStripMenuItem.Size = New System.Drawing.Size(345, 22)
        Me.DéclarationAllergieOuContreindicationToolStripMenuItem.Text = "Déclaration contre-indication"
        '
        'ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem
        '
        Me.ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Name = "ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem"
        Me.ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Size = New System.Drawing.Size(345, 22)
        Me.ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Text = "Liste des contre-indications"
        '
        'RadGridView1
        '
        Me.RadGridView1.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridView1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridView1.ForeColor = System.Drawing.Color.Black
        Me.RadGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridView1.Location = New System.Drawing.Point(15, 484)
        '
        '
        '
        Me.RadGridView1.MasterTemplate.AllowAddNewRow = False
        Me.RadGridView1.MasterTemplate.AllowDeleteRow = False
        GridViewCheckBoxColumn3.EnableExpressionEditor = False
        GridViewCheckBoxColumn3.MinWidth = 20
        GridViewCheckBoxColumn3.Name = "checked"
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.HeaderText = "Categorie"
        GridViewTextBoxColumn8.Name = "category"
        GridViewTextBoxColumn8.Width = 80
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.HeaderText = "Vaccins"
        GridViewTextBoxColumn9.Name = "dci"
        GridViewTextBoxColumn9.Width = 500
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.HeaderText = "id"
        GridViewTextBoxColumn10.IsVisible = False
        GridViewTextBoxColumn10.Name = "id"
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.HeaderText = "valence"
        GridViewTextBoxColumn11.IsVisible = False
        GridViewTextBoxColumn11.Name = "valence"
        Me.RadGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewCheckBoxColumn3, GridViewTextBoxColumn8, GridViewTextBoxColumn9, GridViewTextBoxColumn10, GridViewTextBoxColumn11})
        Me.RadGridView1.MasterTemplate.EnableGrouping = False
        Me.RadGridView1.MasterTemplate.EnableSorting = False
        Me.RadGridView1.MasterTemplate.ShowRowHeaderColumn = False
        SortDescriptor3.PropertyName = "vaccin"
        Me.RadGridView1.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor3})
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition3
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.ReadOnly = True
        Me.RadGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridView1.ShowGroupPanel = False
        Me.RadGridView1.Size = New System.Drawing.Size(701, 168)
        Me.RadGridView1.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 462)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(114, 13)
        Me.Label3.TabIndex = 38
        Me.Label3.Text = "Vaccins selectionnes:"
        '
        'RadFVaccinInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1025, 694)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.RadGridView1)
        Me.Controls.Add(Me.LblAgeVaccination)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LblContreIndication)
        Me.Controls.Add(Me.LblAllergie)
        Me.Controls.Add(Me.RadGroupBoxEtatCivil)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.BtnAdminVaccin)
        Me.Controls.Add(Me.BtnPrintOrdo)
        Me.Controls.Add(Me.BtnValidationProgram)
        Me.Controls.Add(Me.GVVaccin)
        Me.Controls.Add(Me.GVValence)
        Me.Name = "RadFVaccinInfo"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFVaccinInfo"
        CType(Me.GVValence.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVValence, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVVaccin.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVVaccin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnValidationProgram, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnPrintOrdo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnAdminVaccin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBoxEtatCivil.ResumeLayout(False)
        Me.RadGroupBoxEtatCivil.PerformLayout()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LblAgeVaccination, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TraitementContextMenuStrip.ResumeLayout(False)
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GVValence As Telerik.WinControls.UI.RadGridView
    Friend WithEvents GVVaccin As Telerik.WinControls.UI.RadGridView
    Friend WithEvents BtnValidationProgram As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnPrintOrdo As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnAdminVaccin As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblOperator As Label
    Friend WithEvents LblDate As Label
    Friend WithEvents LabelStaticOperator As Label
    Friend WithEvents LabelStaticDate As Label
    Friend WithEvents RadGroupBoxEtatCivil As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblDateNaissance As Label
    Friend WithEvents LblNonOasis As Label
    Friend WithEvents LblALD As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents LblPatientDateMaj As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LblPatientSite As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents LblContreIndication As Label
    Friend WithEvents LblAllergie As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents LblAgeVaccination As Telerik.WinControls.UI.RadLabel
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents TraitementContextMenuStrip As ContextMenuStrip
    Friend WithEvents CréerUnTraitementToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents GérerUneFenêtreThérapeutiqueToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HistoriqueToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents TraitementsObsoletesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents DéclarationAllergieToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents DéclarationAllergieOuContreindicationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RadGridView1 As Telerik.WinControls.UI.RadGridView
    Friend WithEvents Label3 As Label
End Class

