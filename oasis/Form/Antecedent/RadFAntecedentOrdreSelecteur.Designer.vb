<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFAntecedentOrdreSelecteur
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
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.LblAntecedentLbl = New System.Windows.Forms.Label()
        Me.TxtAntecedentAOrdonner = New System.Windows.Forms.TextBox()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
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
        Me.RadPanel3 = New Telerik.WinControls.UI.RadPanel()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        Me.RadPanel4 = New Telerik.WinControls.UI.RadPanel()
        Me.RadAntecedentDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnConfirmerAvant = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnConfirmerApres = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel3.SuspendLayout()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel4.SuspendLayout()
        CType(Me.RadAntecedentDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadAntecedentDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnConfirmerAvant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnConfirmerApres, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.LblAntecedentLbl)
        Me.RadPanel1.Controls.Add(Me.TxtAntecedentAOrdonner)
        Me.RadPanel1.Controls.Add(Me.RadGroupBox1)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(943, 142)
        Me.RadPanel1.TabIndex = 0
        '
        'LblAntecedentLbl
        '
        Me.LblAntecedentLbl.AutoSize = True
        Me.LblAntecedentLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAntecedentLbl.Location = New System.Drawing.Point(5, 108)
        Me.LblAntecedentLbl.Name = "LblAntecedentLbl"
        Me.LblAntecedentLbl.Size = New System.Drawing.Size(156, 13)
        Me.LblAntecedentLbl.TabIndex = 20
        Me.LblAntecedentLbl.Text = "Antécédent à réordonner :"
        '
        'TxtAntecedentAOrdonner
        '
        Me.TxtAntecedentAOrdonner.BackColor = System.Drawing.SystemColors.Info
        Me.TxtAntecedentAOrdonner.Location = New System.Drawing.Point(163, 106)
        Me.TxtAntecedentAOrdonner.Name = "TxtAntecedentAOrdonner"
        Me.TxtAntecedentAOrdonner.ReadOnly = True
        Me.TxtAntecedentAOrdonner.Size = New System.Drawing.Size(477, 20)
        Me.TxtAntecedentAOrdonner.TabIndex = 19
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.Label13)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientDateMaj)
        Me.RadGroupBox1.Controls.Add(Me.Label5)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientUniteSanitaire)
        Me.RadGroupBox1.Controls.Add(Me.Label6)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientSite)
        Me.RadGroupBox1.Controls.Add(Me.Label4)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientTel2)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientPrenom)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientTel1)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBox1.Controls.Add(Me.Label3)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientVille)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientGenre)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientCodePostal)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientNIR)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientAdresse2)
        Me.RadGroupBox1.Controls.Add(Me.Label2)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientAdresse1)
        Me.RadGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox1.HeaderText = "Etat civil"
        Me.RadGroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(935, 84)
        Me.RadGroupBox1.TabIndex = 2
        Me.RadGroupBox1.Text = "Etat civil"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(508, 27)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 62
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(799, 27)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientDateMaj.TabIndex = 61
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(665, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 60
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientUniteSanitaire
        '
        Me.LblPatientUniteSanitaire.AutoSize = True
        Me.LblPatientUniteSanitaire.Location = New System.Drawing.Point(846, 60)
        Me.LblPatientUniteSanitaire.Name = "LblPatientUniteSanitaire"
        Me.LblPatientUniteSanitaire.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientUniteSanitaire.TabIndex = 59
        Me.LblPatientUniteSanitaire.Text = "Auxerre"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(665, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(175, 13)
        Me.Label6.TabIndex = 58
        Me.Label6.Text = "Centre médical de référence :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(708, 44)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(36, 13)
        Me.LblPatientSite.TabIndex = 57
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(665, 44)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 56
        Me.Label4.Text = "Site :"
        '
        'LblPatientTel2
        '
        Me.LblPatientTel2.AutoSize = True
        Me.LblPatientTel2.Location = New System.Drawing.Point(401, 60)
        Me.LblPatientTel2.Name = "LblPatientTel2"
        Me.LblPatientTel2.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel2.TabIndex = 55
        Me.LblPatientTel2.Text = "0968542357"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(9, 27)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientPrenom.TabIndex = 43
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientTel1
        '
        Me.LblPatientTel1.AutoSize = True
        Me.LblPatientTel1.Location = New System.Drawing.Point(401, 44)
        Me.LblPatientTel1.Name = "LblPatientTel1"
        Me.LblPatientTel1.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel1.TabIndex = 54
        Me.LblPatientTel1.Text = "0288425678"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(135, 27)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientNom.TabIndex = 44
        Me.LblPatientNom.Text = "Durand"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(358, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 53
        Me.Label3.Text = "Tel. :"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(363, 27)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 13)
        Me.LblPatientAge.TabIndex = 45
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientVille
        '
        Me.LblPatientVille.AutoSize = True
        Me.LblPatientVille.Location = New System.Drawing.Point(123, 60)
        Me.LblPatientVille.Name = "LblPatientVille"
        Me.LblPatientVille.Size = New System.Drawing.Size(57, 13)
        Me.LblPatientVille.TabIndex = 52
        Me.LblPatientVille.Text = "Lournand"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(422, 27)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientGenre.TabIndex = 46
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientCodePostal
        '
        Me.LblPatientCodePostal.AutoSize = True
        Me.LblPatientCodePostal.Location = New System.Drawing.Point(80, 60)
        Me.LblPatientCodePostal.Name = "LblPatientCodePostal"
        Me.LblPatientCodePostal.Size = New System.Drawing.Size(37, 13)
        Me.LblPatientCodePostal.TabIndex = 51
        Me.LblPatientCodePostal.Text = "71250"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(559, 27)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 47
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'LblPatientAdresse2
        '
        Me.LblPatientAdresse2.AutoSize = True
        Me.LblPatientAdresse2.Location = New System.Drawing.Point(234, 43)
        Me.LblPatientAdresse2.Name = "LblPatientAdresse2"
        Me.LblPatientAdresse2.Size = New System.Drawing.Size(55, 13)
        Me.LblPatientAdresse2.TabIndex = 50
        Me.LblPatientAdresse2.Text = "adresse 2"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 48
        Me.Label2.Text = "Adresse :"
        '
        'LblPatientAdresse1
        '
        Me.LblPatientAdresse1.AutoSize = True
        Me.LblPatientAdresse1.Location = New System.Drawing.Point(80, 43)
        Me.LblPatientAdresse1.Name = "LblPatientAdresse1"
        Me.LblPatientAdresse1.Size = New System.Drawing.Size(121, 13)
        Me.LblPatientAdresse1.TabIndex = 49
        Me.LblPatientAdresse1.Text = "3 rue de la république"
        '
        'RadPanel3
        '
        Me.RadPanel3.Controls.Add(Me.RadBtnAbandonner)
        Me.RadPanel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel3.Location = New System.Drawing.Point(0, 338)
        Me.RadPanel3.Name = "RadPanel3"
        Me.RadPanel3.Size = New System.Drawing.Size(943, 34)
        Me.RadPanel3.TabIndex = 2
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(821, 4)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandonner.TabIndex = 0
        Me.RadBtnAbandonner.Text = "Abandonner"
        '
        'RadPanel4
        '
        Me.RadPanel4.Controls.Add(Me.RadAntecedentDataGridView)
        Me.RadPanel4.Dock = System.Windows.Forms.DockStyle.Left
        Me.RadPanel4.Location = New System.Drawing.Point(0, 142)
        Me.RadPanel4.Name = "RadPanel4"
        Me.RadPanel4.Size = New System.Drawing.Size(652, 196)
        Me.RadPanel4.TabIndex = 3
        '
        'RadAntecedentDataGridView
        '
        Me.RadAntecedentDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadAntecedentDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadAntecedentDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadAntecedentDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadAntecedentDataGridView.ForeColor = System.Drawing.Color.Black
        Me.RadAntecedentDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadAntecedentDataGridView.Location = New System.Drawing.Point(0, 0)
        '
        '
        '
        Me.RadAntecedentDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadAntecedentDataGridView.MasterTemplate.AllowColumnReorder = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "antecedent"
        GridViewTextBoxColumn1.HeaderText = "Antécédent"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.Name = "antecedent"
        GridViewTextBoxColumn1.Width = 600
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "ordreAffichage"
        GridViewTextBoxColumn2.HeaderText = "Ordre affichage"
        GridViewTextBoxColumn2.IsVisible = False
        GridViewTextBoxColumn2.Name = "ordreAffichage"
        GridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn2.Width = 100
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "antecedentId"
        GridViewTextBoxColumn3.HeaderText = "Id."
        GridViewTextBoxColumn3.IsVisible = False
        GridViewTextBoxColumn3.Name = "antecedentId"
        Me.RadAntecedentDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3})
        Me.RadAntecedentDataGridView.MasterTemplate.EnableGrouping = False
        Me.RadAntecedentDataGridView.MasterTemplate.EnableSorting = False
        Me.RadAntecedentDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadAntecedentDataGridView.Name = "RadAntecedentDataGridView"
        Me.RadAntecedentDataGridView.ReadOnly = True
        Me.RadAntecedentDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadAntecedentDataGridView.Size = New System.Drawing.Size(652, 196)
        Me.RadAntecedentDataGridView.TabIndex = 0
        '
        'RadBtnConfirmerAvant
        '
        Me.RadBtnConfirmerAvant.Location = New System.Drawing.Point(752, 181)
        Me.RadBtnConfirmerAvant.Name = "RadBtnConfirmerAvant"
        Me.RadBtnConfirmerAvant.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnConfirmerAvant.TabIndex = 24
        Me.RadBtnConfirmerAvant.Text = "Ordonner avant"
        '
        'RadBtnConfirmerApres
        '
        Me.RadBtnConfirmerApres.Location = New System.Drawing.Point(752, 239)
        Me.RadBtnConfirmerApres.Name = "RadBtnConfirmerApres"
        Me.RadBtnConfirmerApres.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnConfirmerApres.TabIndex = 25
        Me.RadBtnConfirmerApres.Text = "Ordonner après"
        '
        'RadFAntecedentOrdreSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandonner
        Me.ClientSize = New System.Drawing.Size(943, 372)
        Me.Controls.Add(Me.RadBtnConfirmerApres)
        Me.Controls.Add(Me.RadBtnConfirmerAvant)
        Me.Controls.Add(Me.RadPanel4)
        Me.Controls.Add(Me.RadPanel3)
        Me.Controls.Add(Me.RadPanel1)
        Me.Location = New System.Drawing.Point(630, 920)
        Me.Name = "RadFAntecedentOrdreSelecteur"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Changement d'ordre d'un antécédent"
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.RadPanel3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel3.ResumeLayout(False)
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel4.ResumeLayout(False)
        CType(Me.RadAntecedentDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadAntecedentDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnConfirmerAvant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnConfirmerApres, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel3 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel4 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
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
    Friend WithEvents RadAntecedentDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnConfirmerAvant As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnConfirmerApres As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblAntecedentLbl As Label
    Friend WithEvents TxtAntecedentAOrdonner As TextBox
End Class

