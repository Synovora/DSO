<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RadFCPV
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
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadFCPV))
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.Grid = New Telerik.WinControls.UI.RadGridView()
        Me.RadButtonEditValence = New Telerik.WinControls.UI.RadButton()
        Me.BtnDateDelete = New Telerik.WinControls.UI.RadButton()
        Me.BtnDateAdd = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RBDateInactif = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
        Me.RBDateActif = New Telerik.WinControls.UI.RadRadioButton()
        Me.RBDateAll = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.TextYear = New Telerik.WinControls.UI.RadTextBox()
        Me.TextMonth = New Telerik.WinControls.UI.RadTextBox()
        Me.TextDay = New Telerik.WinControls.UI.RadTextBox()
        Me.BtnImport = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBoxEtatCivil = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblDateNaissance = New System.Windows.Forms.Label()
        Me.LblALD = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LblPatientDateMaj = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblPatientSite = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.RadGroupBox3 = New Telerik.WinControls.UI.RadGroupBox()
        Me.BtnSendMail = New Telerik.WinControls.UI.RadButton()
        Me.DTPEnd = New System.Windows.Forms.DateTimePicker()
        Me.DTPStart = New System.Windows.Forms.DateTimePicker()
        Me.RadLabel5 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel6 = New Telerik.WinControls.UI.RadLabel()
        Me.BtnCarnet = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButtonEditValence, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDateDelete, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDateAdd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        CType(Me.RBDateInactif, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RBDateActif, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RBDateAll, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextYear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextMonth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextDay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnImport, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBoxEtatCivil.SuspendLayout()
        CType(Me.RadGroupBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox3.SuspendLayout()
        CType(Me.BtnSendMail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnCarnet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1607, 694)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 2
        '
        'Grid
        '
        Me.Grid.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Grid.Cursor = System.Windows.Forms.Cursors.Default
        Me.Grid.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.Grid.ForeColor = System.Drawing.Color.Black
        Me.Grid.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Grid.Location = New System.Drawing.Point(12, 56)
        '
        '
        '
        Me.Grid.MasterTemplate.AllowAddNewRow = False
        Me.Grid.MasterTemplate.AllowCellContextMenu = False
        Me.Grid.MasterTemplate.AllowColumnChooser = False
        Me.Grid.MasterTemplate.AllowColumnHeaderContextMenu = False
        Me.Grid.MasterTemplate.AllowColumnReorder = False
        Me.Grid.MasterTemplate.AllowColumnResize = False
        Me.Grid.MasterTemplate.AllowDeleteRow = False
        Me.Grid.MasterTemplate.AllowDragToGroup = False
        Me.Grid.MasterTemplate.AllowEditRow = False
        Me.Grid.MasterTemplate.AllowRowHeaderContextMenu = False
        Me.Grid.MasterTemplate.AllowRowResize = False
        Me.Grid.MasterTemplate.AutoGenerateColumns = False
        Me.Grid.MasterTemplate.EnableGrouping = False
        Me.Grid.MasterTemplate.ShowFilteringRow = False
        Me.Grid.MasterTemplate.ShowRowHeaderColumn = False
        Me.Grid.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.Grid.Name = "Grid"
        Me.Grid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Grid.Size = New System.Drawing.Size(1452, 662)
        Me.Grid.TabIndex = 55
        '
        'RadButtonEditValence
        '
        Me.RadButtonEditValence.Location = New System.Drawing.Point(16, 21)
        Me.RadButtonEditValence.Name = "RadButtonEditValence"
        Me.RadButtonEditValence.Size = New System.Drawing.Size(132, 24)
        Me.RadButtonEditValence.TabIndex = 10
        Me.RadButtonEditValence.Text = "Gestion d'affichage"
        '
        'BtnDateDelete
        '
        Me.BtnDateDelete.Location = New System.Drawing.Point(16, 153)
        Me.BtnDateDelete.Name = "BtnDateDelete"
        Me.BtnDateDelete.Size = New System.Drawing.Size(132, 24)
        Me.BtnDateDelete.TabIndex = 14
        Me.BtnDateDelete.Text = "Supprimer"
        '
        'BtnDateAdd
        '
        Me.BtnDateAdd.Location = New System.Drawing.Point(16, 123)
        Me.BtnDateAdd.Name = "BtnDateAdd"
        Me.BtnDateAdd.Size = New System.Drawing.Size(132, 24)
        Me.BtnDateAdd.TabIndex = 12
        Me.BtnDateAdd.Text = "Ajouter"
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.RadButtonEditValence)
        Me.RadGroupBox1.HeaderText = "Valence"
        Me.RadGroupBox1.Location = New System.Drawing.Point(1470, 12)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(161, 56)
        Me.RadGroupBox1.TabIndex = 57
        Me.RadGroupBox1.Text = "Valence"
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.Controls.Add(Me.RBDateInactif)
        Me.RadGroupBox2.Controls.Add(Me.RadLabel3)
        Me.RadGroupBox2.Controls.Add(Me.RBDateActif)
        Me.RadGroupBox2.Controls.Add(Me.RBDateAll)
        Me.RadGroupBox2.Controls.Add(Me.RadLabel2)
        Me.RadGroupBox2.Controls.Add(Me.RadLabel1)
        Me.RadGroupBox2.Controls.Add(Me.TextYear)
        Me.RadGroupBox2.Controls.Add(Me.TextMonth)
        Me.RadGroupBox2.Controls.Add(Me.TextDay)
        Me.RadGroupBox2.Controls.Add(Me.BtnDateAdd)
        Me.RadGroupBox2.Controls.Add(Me.BtnDateDelete)
        Me.RadGroupBox2.HeaderText = "Date"
        Me.RadGroupBox2.Location = New System.Drawing.Point(1470, 74)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(161, 188)
        Me.RadGroupBox2.TabIndex = 58
        Me.RadGroupBox2.Text = "Date"
        '
        'RBDateInactif
        '
        Me.RBDateInactif.Location = New System.Drawing.Point(105, 21)
        Me.RBDateInactif.Name = "RBDateInactif"
        Me.RBDateInactif.Size = New System.Drawing.Size(43, 18)
        Me.RBDateInactif.TabIndex = 16
        Me.RBDateInactif.Text = "Sans"
        '
        'RadLabel3
        '
        Me.RadLabel3.Location = New System.Drawing.Point(16, 98)
        Me.RadLabel3.Name = "RadLabel3"
        Me.RadLabel3.Size = New System.Drawing.Size(49, 18)
        Me.RadLabel3.TabIndex = 20
        Me.RadLabel3.Text = "Annees :"
        '
        'RBDateActif
        '
        Me.RBDateActif.Location = New System.Drawing.Point(58, 21)
        Me.RBDateActif.Name = "RBDateActif"
        Me.RBDateActif.Size = New System.Drawing.Size(44, 18)
        Me.RBDateActif.TabIndex = 15
        Me.RBDateActif.Text = "Avec"
        '
        'RBDateAll
        '
        Me.RBDateAll.Location = New System.Drawing.Point(8, 21)
        Me.RBDateAll.Name = "RBDateAll"
        Me.RBDateAll.Size = New System.Drawing.Size(44, 18)
        Me.RBDateAll.TabIndex = 14
        Me.RBDateAll.Text = "Tous"
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(16, 72)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(36, 18)
        Me.RadLabel2.TabIndex = 19
        Me.RadLabel2.Text = "Mois :"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(16, 46)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(38, 18)
        Me.RadLabel1.TabIndex = 18
        Me.RadLabel1.Text = "Jours :"
        '
        'TextYear
        '
        Me.TextYear.Location = New System.Drawing.Point(71, 97)
        Me.TextYear.Name = "TextYear"
        Me.TextYear.NullText = "0"
        Me.TextYear.Size = New System.Drawing.Size(77, 27)
        Me.TextYear.TabIndex = 17
        '
        'TextMonth
        '
        Me.TextMonth.Location = New System.Drawing.Point(71, 71)
        Me.TextMonth.Name = "TextMonth"
        Me.TextMonth.NullText = "0"
        Me.TextMonth.Size = New System.Drawing.Size(77, 27)
        Me.TextMonth.TabIndex = 16
        '
        'TextDay
        '
        Me.TextDay.Location = New System.Drawing.Point(71, 45)
        Me.TextDay.Name = "TextDay"
        Me.TextDay.NullText = "0"
        Me.TextDay.Size = New System.Drawing.Size(77, 27)
        Me.TextDay.TabIndex = 15
        '
        'BtnImport
        '
        Me.BtnImport.Location = New System.Drawing.Point(1470, 423)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(161, 47)
        Me.BtnImport.TabIndex = 15
        Me.BtnImport.Text = "Importer depuis le calendrier general"
        Me.BtnImport.TextWrap = True
        Me.BtnImport.Visible = False
        '
        'RadGroupBoxEtatCivil
        '
        Me.RadGroupBoxEtatCivil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBoxEtatCivil.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblDateNaissance)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblALD)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label13)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientDateMaj)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label5)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientSite)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label4)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientPrenom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientGenre)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNIR)
        Me.RadGroupBoxEtatCivil.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBoxEtatCivil.HeaderText = ""
        Me.RadGroupBoxEtatCivil.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBoxEtatCivil.Name = "RadGroupBoxEtatCivil"
        Me.RadGroupBoxEtatCivil.Size = New System.Drawing.Size(1452, 38)
        Me.RadGroupBoxEtatCivil.TabIndex = 59
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.GroupBoxContent).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        '
        'LblDateNaissance
        '
        Me.LblDateNaissance.AutoSize = True
        Me.LblDateNaissance.Location = New System.Drawing.Point(447, 4)
        Me.LblDateNaissance.Name = "LblDateNaissance"
        Me.LblDateNaissance.Size = New System.Drawing.Size(96, 23)
        Me.LblDateNaissance.TabIndex = 46
        Me.LblDateNaissance.Text = "25-04-2018"
        '
        'LblALD
        '
        Me.LblALD.AutoSize = True
        Me.LblALD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblALD.ForeColor = System.Drawing.Color.OrangeRed
        Me.LblALD.Location = New System.Drawing.Point(1119, 4)
        Me.LblALD.Name = "LblALD"
        Me.LblALD.Size = New System.Drawing.Size(46, 20)
        Me.LblALD.TabIndex = 43
        Me.LblALD.Text = "ALD"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(682, 4)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(52, 20)
        Me.Label13.TabIndex = 42
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(973, 4)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(96, 23)
        Me.LblPatientDateMaj.TabIndex = 41
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(839, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(194, 20)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(882, 21)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(53, 23)
        Me.LblPatientSite.TabIndex = 37
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(839, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 20)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Site :"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(11, 4)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(95, 23)
        Me.LblPatientPrenom.TabIndex = 23
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(133, 4)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(67, 23)
        Me.LblPatientNom.TabIndex = 24
        Me.LblPatientNom.Text = "Durand"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(516, 4)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(59, 23)
        Me.LblPatientAge.TabIndex = 25
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(599, 4)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(77, 23)
        Me.LblPatientGenre.TabIndex = 26
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(733, 4)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(127, 23)
        Me.LblPatientNIR.TabIndex = 27
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'RadGroupBox3
        '
        Me.RadGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox3.Controls.Add(Me.BtnSendMail)
        Me.RadGroupBox3.Controls.Add(Me.DTPEnd)
        Me.RadGroupBox3.Controls.Add(Me.DTPStart)
        Me.RadGroupBox3.Controls.Add(Me.RadLabel5)
        Me.RadGroupBox3.Controls.Add(Me.RadLabel6)
        Me.RadGroupBox3.Controls.Add(Me.BtnCarnet)
        Me.RadGroupBox3.HeaderText = "Carnet vaccinal"
        Me.RadGroupBox3.Location = New System.Drawing.Point(1470, 268)
        Me.RadGroupBox3.Name = "RadGroupBox3"
        Me.RadGroupBox3.Size = New System.Drawing.Size(161, 149)
        Me.RadGroupBox3.TabIndex = 59
        Me.RadGroupBox3.Text = "Carnet vaccinal"
        '
        'BtnSendMail
        '
        Me.BtnSendMail.Location = New System.Drawing.Point(16, 115)
        Me.BtnSendMail.Name = "BtnSendMail"
        Me.BtnSendMail.Size = New System.Drawing.Size(132, 24)
        Me.BtnSendMail.TabIndex = 13
        Me.BtnSendMail.Text = "Envoyer par email"
        '
        'DTPEnd
        '
        Me.DTPEnd.Location = New System.Drawing.Point(53, 59)
        Me.DTPEnd.Name = "DTPEnd"
        Me.DTPEnd.Size = New System.Drawing.Size(95, 26)
        Me.DTPEnd.TabIndex = 21
        '
        'DTPStart
        '
        Me.DTPStart.Location = New System.Drawing.Point(53, 27)
        Me.DTPStart.Name = "DTPStart"
        Me.DTPStart.Size = New System.Drawing.Size(95, 26)
        Me.DTPStart.TabIndex = 20
        '
        'RadLabel5
        '
        Me.RadLabel5.Location = New System.Drawing.Point(8, 61)
        Me.RadLabel5.Name = "RadLabel5"
        Me.RadLabel5.Size = New System.Drawing.Size(26, 18)
        Me.RadLabel5.TabIndex = 19
        Me.RadLabel5.Text = "Fin :"
        '
        'RadLabel6
        '
        Me.RadLabel6.Location = New System.Drawing.Point(8, 29)
        Me.RadLabel6.Name = "RadLabel6"
        Me.RadLabel6.Size = New System.Drawing.Size(43, 18)
        Me.RadLabel6.TabIndex = 18
        Me.RadLabel6.Text = "Debut :"
        '
        'BtnCarnet
        '
        Me.BtnCarnet.Location = New System.Drawing.Point(16, 85)
        Me.BtnCarnet.Name = "BtnCarnet"
        Me.BtnCarnet.Size = New System.Drawing.Size(132, 24)
        Me.BtnCarnet.TabIndex = 12
        Me.BtnCarnet.Text = "Imprimer"
        '
        'RadFCPV
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1643, 728)
        Me.Controls.Add(Me.RadGroupBox3)
        Me.Controls.Add(Me.RadGroupBoxEtatCivil)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RadFCPV"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFATCListe"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButtonEditValence, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDateDelete, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDateAdd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.RadGroupBox2.PerformLayout()
        CType(Me.RBDateInactif, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RBDateActif, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RBDateAll, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextYear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextMonth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextDay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnImport, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBoxEtatCivil.ResumeLayout(False)
        Me.RadGroupBoxEtatCivil.PerformLayout()
        CType(Me.RadGroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox3.ResumeLayout(False)
        Me.RadGroupBox3.PerformLayout()
        CType(Me.BtnSendMail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnCarnet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents Grid As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadButtonEditValence As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnDateDelete As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnDateAdd As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents TextYear As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents TextMonth As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents TextDay As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RBDateInactif As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents RBDateActif As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents RBDateAll As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents BtnImport As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBoxEtatCivil As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblDateNaissance As Label
    Friend WithEvents LblALD As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents LblPatientDateMaj As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LblPatientSite As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents RadGroupBox3 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents DTPEnd As DateTimePicker
    Friend WithEvents DTPStart As DateTimePicker
    Friend WithEvents RadLabel5 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel6 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents BtnCarnet As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnSendMail As Telerik.WinControls.UI.RadButton
End Class

