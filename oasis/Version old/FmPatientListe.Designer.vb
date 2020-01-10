<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FmPatientListe
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
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn1 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn2 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewDateTimeColumn3 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DteFiltreDateNaissance = New System.Windows.Forms.DateTimePicker()
        Me.ChkPatientNonOasis = New System.Windows.Forms.CheckBox()
        Me.ChkPatientTous = New System.Windows.Forms.CheckBox()
        Me.ChkPatientOasis = New System.Windows.Forms.CheckBox()
        Me.RadBtnDetailPatient = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSynthese = New Telerik.WinControls.UI.RadButton()
        Me.LblAgeSelected = New System.Windows.Forms.Label()
        Me.LblDateNaissanceSelected = New System.Windows.Forms.Label()
        Me.LblDateSortie = New System.Windows.Forms.Label()
        Me.LblLabelDateSortie = New System.Windows.Forms.Label()
        Me.LblPatientSorti = New System.Windows.Forms.Label()
        Me.TxtNomSelected = New System.Windows.Forms.TextBox()
        Me.TxtIdSelected = New System.Windows.Forms.TextBox()
        Me.TxtNirSelected = New System.Windows.Forms.TextBox()
        Me.TxtPrenomSelected = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadPatientGridView = New Telerik.WinControls.UI.RadGridView()
        Me.PnlSelectedPatient = New Telerik.WinControls.UI.RadPanel()
        Me.RadTxtPrenom = New Telerik.WinControls.UI.RadTextBox()
        Me.RadTxtNom = New Telerik.WinControls.UI.RadTextBox()
        Me.RadBtnDisplay = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnInitialize = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnRefresh = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnCreatePatient = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAdmin = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnMedoc = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadBtnDetailPatient, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSynthese, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPatientGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPatientGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PnlSelectedPatient, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSelectedPatient.SuspendLayout()
        CType(Me.RadTxtPrenom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadTxtNom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnInitialize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnRefresh, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnCreatePatient, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAdmin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnMedoc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(328, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 13)
        Me.Label3.TabIndex = 40
        Me.Label3.Text = "Date naissance"
        '
        'DteFiltreDateNaissance
        '
        Me.DteFiltreDateNaissance.Location = New System.Drawing.Point(415, 14)
        Me.DteFiltreDateNaissance.Name = "DteFiltreDateNaissance"
        Me.DteFiltreDateNaissance.Size = New System.Drawing.Size(200, 20)
        Me.DteFiltreDateNaissance.TabIndex = 15
        '
        'ChkPatientNonOasis
        '
        Me.ChkPatientNonOasis.AutoSize = True
        Me.ChkPatientNonOasis.Location = New System.Drawing.Point(111, 49)
        Me.ChkPatientNonOasis.Name = "ChkPatientNonOasis"
        Me.ChkPatientNonOasis.Size = New System.Drawing.Size(109, 17)
        Me.ChkPatientNonOasis.TabIndex = 17
        Me.ChkPatientNonOasis.Text = "Patient non Oasis"
        Me.ChkPatientNonOasis.UseVisualStyleBackColor = True
        '
        'ChkPatientTous
        '
        Me.ChkPatientTous.AutoSize = True
        Me.ChkPatientTous.Location = New System.Drawing.Point(226, 49)
        Me.ChkPatientTous.Name = "ChkPatientTous"
        Me.ChkPatientTous.Size = New System.Drawing.Size(50, 17)
        Me.ChkPatientTous.TabIndex = 18
        Me.ChkPatientTous.Text = "Tous"
        Me.ChkPatientTous.UseVisualStyleBackColor = True
        '
        'ChkPatientOasis
        '
        Me.ChkPatientOasis.AutoSize = True
        Me.ChkPatientOasis.Location = New System.Drawing.Point(12, 49)
        Me.ChkPatientOasis.Name = "ChkPatientOasis"
        Me.ChkPatientOasis.Size = New System.Drawing.Size(93, 17)
        Me.ChkPatientOasis.TabIndex = 16
        Me.ChkPatientOasis.Text = "Patients Oasis"
        Me.ChkPatientOasis.UseVisualStyleBackColor = True
        '
        'RadBtnDetailPatient
        '
        Me.RadBtnDetailPatient.Location = New System.Drawing.Point(17, 262)
        Me.RadBtnDetailPatient.Name = "RadBtnDetailPatient"
        Me.RadBtnDetailPatient.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnDetailPatient.TabIndex = 55
        Me.RadBtnDetailPatient.Text = "Détail patient"
        '
        'RadBtnSynthese
        '
        Me.RadBtnSynthese.Location = New System.Drawing.Point(17, 232)
        Me.RadBtnSynthese.Name = "RadBtnSynthese"
        Me.RadBtnSynthese.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnSynthese.TabIndex = 50
        Me.RadBtnSynthese.Text = "Outil de synthèse"
        '
        'LblAgeSelected
        '
        Me.LblAgeSelected.AutoSize = True
        Me.LblAgeSelected.Location = New System.Drawing.Point(105, 128)
        Me.LblAgeSelected.Name = "LblAgeSelected"
        Me.LblAgeSelected.Size = New System.Drawing.Size(40, 13)
        Me.LblAgeSelected.TabIndex = 19
        Me.LblAgeSelected.Text = "20 ans"
        '
        'LblDateNaissanceSelected
        '
        Me.LblDateNaissanceSelected.AutoSize = True
        Me.LblDateNaissanceSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDateNaissanceSelected.Location = New System.Drawing.Point(14, 128)
        Me.LblDateNaissanceSelected.Name = "LblDateNaissanceSelected"
        Me.LblDateNaissanceSelected.Size = New System.Drawing.Size(75, 13)
        Me.LblDateNaissanceSelected.TabIndex = 18
        Me.LblDateNaissanceSelected.Text = "01/01/2000"
        '
        'LblDateSortie
        '
        Me.LblDateSortie.AutoSize = True
        Me.LblDateSortie.Location = New System.Drawing.Point(79, 168)
        Me.LblDateSortie.Name = "LblDateSortie"
        Me.LblDateSortie.Size = New System.Drawing.Size(63, 13)
        Me.LblDateSortie.TabIndex = 17
        Me.LblDateSortie.Text = "01/01/2000"
        '
        'LblLabelDateSortie
        '
        Me.LblLabelDateSortie.AutoSize = True
        Me.LblLabelDateSortie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelDateSortie.Location = New System.Drawing.Point(11, 168)
        Me.LblLabelDateSortie.Name = "LblLabelDateSortie"
        Me.LblLabelDateSortie.Size = New System.Drawing.Size(77, 13)
        Me.LblLabelDateSortie.TabIndex = 16
        Me.LblLabelDateSortie.Text = "Date sortie :"
        '
        'LblPatientSorti
        '
        Me.LblPatientSorti.AutoSize = True
        Me.LblPatientSorti.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPatientSorti.ForeColor = System.Drawing.Color.Red
        Me.LblPatientSorti.Location = New System.Drawing.Point(11, 192)
        Me.LblPatientSorti.Name = "LblPatientSorti"
        Me.LblPatientSorti.Size = New System.Drawing.Size(279, 13)
        Me.LblPatientSorti.TabIndex = 15
        Me.LblPatientSorti.Text = "Attention, ce patient est sorti du dispositif Oasis"
        '
        'TxtNomSelected
        '
        Me.TxtNomSelected.BackColor = System.Drawing.Color.White
        Me.TxtNomSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtNomSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNomSelected.Location = New System.Drawing.Point(14, 101)
        Me.TxtNomSelected.Name = "TxtNomSelected"
        Me.TxtNomSelected.ReadOnly = True
        Me.TxtNomSelected.Size = New System.Drawing.Size(228, 13)
        Me.TxtNomSelected.TabIndex = 11
        Me.TxtNomSelected.Text = "Nom"
        '
        'TxtIdSelected
        '
        Me.TxtIdSelected.BackColor = System.Drawing.Color.White
        Me.TxtIdSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtIdSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtIdSelected.Location = New System.Drawing.Point(14, 20)
        Me.TxtIdSelected.Name = "TxtIdSelected"
        Me.TxtIdSelected.ReadOnly = True
        Me.TxtIdSelected.Size = New System.Drawing.Size(53, 13)
        Me.TxtIdSelected.TabIndex = 8
        Me.TxtIdSelected.Text = "Id"
        '
        'TxtNirSelected
        '
        Me.TxtNirSelected.BackColor = System.Drawing.Color.White
        Me.TxtNirSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtNirSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNirSelected.Location = New System.Drawing.Point(14, 47)
        Me.TxtNirSelected.Name = "TxtNirSelected"
        Me.TxtNirSelected.ReadOnly = True
        Me.TxtNirSelected.Size = New System.Drawing.Size(100, 13)
        Me.TxtNirSelected.TabIndex = 9
        Me.TxtNirSelected.Text = "Nir"
        '
        'TxtPrenomSelected
        '
        Me.TxtPrenomSelected.BackColor = System.Drawing.Color.White
        Me.TxtPrenomSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtPrenomSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPrenomSelected.Location = New System.Drawing.Point(14, 74)
        Me.TxtPrenomSelected.Name = "TxtPrenomSelected"
        Me.TxtPrenomSelected.ReadOnly = True
        Me.TxtPrenomSelected.Size = New System.Drawing.Size(134, 13)
        Me.TxtPrenomSelected.TabIndex = 10
        Me.TxtPrenomSelected.Text = "Prénom"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(177, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "Nom"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "Prenom"
        '
        'RadPatientGridView
        '
        Me.RadPatientGridView.BackColor = System.Drawing.SystemColors.Control
        Me.RadPatientGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadPatientGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadPatientGridView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadPatientGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadPatientGridView.Location = New System.Drawing.Point(12, 72)
        '
        '
        '
        Me.RadPatientGridView.MasterTemplate.AllowAddNewRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "oa_patient_id"
        GridViewTextBoxColumn1.HeaderText = "Id.Oasis"
        GridViewTextBoxColumn1.Name = "col_oa_patient_id"
        GridViewTextBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn1.Width = 80
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "oa_patient_nir"
        GridViewTextBoxColumn2.HeaderText = "NIR"
        GridViewTextBoxColumn2.Name = "col_oa_patient_nir"
        GridViewTextBoxColumn2.ReadOnly = True
        GridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn2.Width = 100
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "oa_patient_prenom"
        GridViewTextBoxColumn3.HeaderText = "Prénom"
        GridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn3.Name = "col_oa_patient_prenom"
        GridViewTextBoxColumn3.ReadOnly = True
        GridViewTextBoxColumn3.Width = 120
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "oa_patient_nom"
        GridViewTextBoxColumn4.HeaderText = "Nom"
        GridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn4.Name = "col_oa_patient_nom"
        GridViewTextBoxColumn4.ReadOnly = True
        GridViewTextBoxColumn4.Width = 200
        GridViewDateTimeColumn1.EnableExpressionEditor = False
        GridViewDateTimeColumn1.FieldName = "oa_patient_date_naissance"
        GridViewDateTimeColumn1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        GridViewDateTimeColumn1.FormatString = "{0:dd/MM/yyyy}"
        GridViewDateTimeColumn1.HeaderText = "Date naissance"
        GridViewDateTimeColumn1.Name = "col_oa_patient_date_naissance"
        GridViewDateTimeColumn1.ReadOnly = True
        GridViewDateTimeColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDateTimeColumn1.Width = 100
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.FieldName = "oa_patient_age"
        GridViewTextBoxColumn5.HeaderText = "Age"
        GridViewTextBoxColumn5.Name = "col_oa_patient_age"
        GridViewTextBoxColumn5.ReadOnly = True
        GridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn5.Width = 60
        GridViewDateTimeColumn2.EnableExpressionEditor = False
        GridViewDateTimeColumn2.FieldName = "oa_patient_date_entree_oasis"
        GridViewDateTimeColumn2.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        GridViewDateTimeColumn2.FormatString = "{0:dd/MM/yyyy}"
        GridViewDateTimeColumn2.HeaderText = "Date entrée"
        GridViewDateTimeColumn2.Name = "col_oa_patient_date_entree_oasis"
        GridViewDateTimeColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDateTimeColumn2.Width = 100
        GridViewDateTimeColumn3.EnableExpressionEditor = False
        GridViewDateTimeColumn3.FieldName = "oa_patient_date_sortie_oasis"
        GridViewDateTimeColumn3.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        GridViewDateTimeColumn3.FormatString = "{0:dd/MM/yyyy}"
        GridViewDateTimeColumn3.HeaderText = "Date sortie"
        GridViewDateTimeColumn3.Name = "col_oa_patient_date_sortie_oasis"
        GridViewDateTimeColumn3.ReadOnly = True
        GridViewDateTimeColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDateTimeColumn3.Width = 100
        Me.RadPatientGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewDateTimeColumn1, GridViewTextBoxColumn5, GridViewDateTimeColumn2, GridViewDateTimeColumn3})
        Me.RadPatientGridView.MasterTemplate.EnablePaging = True
        Me.RadPatientGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadPatientGridView.Name = "RadPatientGridView"
        Me.RadPatientGridView.ReadOnly = True
        Me.RadPatientGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadPatientGridView.Size = New System.Drawing.Size(906, 544)
        Me.RadPatientGridView.TabIndex = 31
        '
        'PnlSelectedPatient
        '
        Me.PnlSelectedPatient.BackColor = System.Drawing.Color.White
        Me.PnlSelectedPatient.Controls.Add(Me.RadBtnDetailPatient)
        Me.PnlSelectedPatient.Controls.Add(Me.TxtIdSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.RadBtnSynthese)
        Me.PnlSelectedPatient.Controls.Add(Me.TxtPrenomSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.LblAgeSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.TxtNirSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.LblDateNaissanceSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.TxtNomSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.LblDateSortie)
        Me.PnlSelectedPatient.Controls.Add(Me.LblPatientSorti)
        Me.PnlSelectedPatient.Controls.Add(Me.LblLabelDateSortie)
        Me.PnlSelectedPatient.Location = New System.Drawing.Point(924, 72)
        Me.PnlSelectedPatient.Name = "PnlSelectedPatient"
        '
        '
        '
        Me.PnlSelectedPatient.RootElement.ApplyShapeToControl = False
        Me.PnlSelectedPatient.RootElement.EnableBorderHighlight = True
        Me.PnlSelectedPatient.Size = New System.Drawing.Size(351, 299)
        Me.PnlSelectedPatient.TabIndex = 42
        Me.PnlSelectedPatient.ThemeName = "ControlDefault"
        CType(Me.PnlSelectedPatient.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).ClipDrawing = False
        '
        'RadTxtPrenom
        '
        Me.RadTxtPrenom.Location = New System.Drawing.Point(61, 15)
        Me.RadTxtPrenom.Name = "RadTxtPrenom"
        Me.RadTxtPrenom.ShowClearButton = True
        Me.RadTxtPrenom.Size = New System.Drawing.Size(100, 20)
        Me.RadTxtPrenom.TabIndex = 5
        '
        'RadTxtNom
        '
        Me.RadTxtNom.Location = New System.Drawing.Point(212, 15)
        Me.RadTxtNom.Name = "RadTxtNom"
        Me.RadTxtNom.ShowClearButton = True
        Me.RadTxtNom.Size = New System.Drawing.Size(100, 20)
        Me.RadTxtNom.TabIndex = 10
        '
        'RadBtnDisplay
        '
        Me.RadBtnDisplay.Location = New System.Drawing.Point(630, 13)
        Me.RadBtnDisplay.Name = "RadBtnDisplay"
        Me.RadBtnDisplay.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnDisplay.TabIndex = 20
        Me.RadBtnDisplay.Text = "Filtrer"
        '
        'RadBtnInitialize
        '
        Me.RadBtnInitialize.Location = New System.Drawing.Point(746, 13)
        Me.RadBtnInitialize.Name = "RadBtnInitialize"
        Me.RadBtnInitialize.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnInitialize.TabIndex = 25
        Me.RadBtnInitialize.Text = "Initialiser filtres"
        '
        'RadBtnRefresh
        '
        Me.RadBtnRefresh.Location = New System.Drawing.Point(862, 13)
        Me.RadBtnRefresh.Name = "RadBtnRefresh"
        Me.RadBtnRefresh.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnRefresh.TabIndex = 30
        Me.RadBtnRefresh.Text = "Recharger écran"
        '
        'RadBtnCreatePatient
        '
        Me.RadBtnCreatePatient.Location = New System.Drawing.Point(12, 626)
        Me.RadBtnCreatePatient.Name = "RadBtnCreatePatient"
        Me.RadBtnCreatePatient.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnCreatePatient.TabIndex = 35
        Me.RadBtnCreatePatient.Text = "Créer patient Oasis"
        '
        'RadBtnAdmin
        '
        Me.RadBtnAdmin.Location = New System.Drawing.Point(128, 626)
        Me.RadBtnAdmin.Name = "RadBtnAdmin"
        Me.RadBtnAdmin.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAdmin.TabIndex = 40
        Me.RadBtnAdmin.Text = "Administration DPI"
        '
        'RadBtnMedoc
        '
        Me.RadBtnMedoc.Location = New System.Drawing.Point(244, 626)
        Me.RadBtnMedoc.Name = "RadBtnMedoc"
        Me.RadBtnMedoc.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnMedoc.TabIndex = 45
        Me.RadBtnMedoc.Text = "Médicaments"
        '
        'FmPatientListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1284, 661)
        Me.Controls.Add(Me.RadBtnMedoc)
        Me.Controls.Add(Me.RadBtnAdmin)
        Me.Controls.Add(Me.RadBtnCreatePatient)
        Me.Controls.Add(Me.RadBtnRefresh)
        Me.Controls.Add(Me.RadBtnInitialize)
        Me.Controls.Add(Me.RadBtnDisplay)
        Me.Controls.Add(Me.RadTxtNom)
        Me.Controls.Add(Me.RadTxtPrenom)
        Me.Controls.Add(Me.PnlSelectedPatient)
        Me.Controls.Add(Me.RadPatientGridView)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.DteFiltreDateNaissance)
        Me.Controls.Add(Me.ChkPatientNonOasis)
        Me.Controls.Add(Me.ChkPatientTous)
        Me.Controls.Add(Me.ChkPatientOasis)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "FmPatientListe"
        Me.Text = "Liste des patients"
        CType(Me.RadBtnDetailPatient, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSynthese, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPatientGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPatientGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PnlSelectedPatient, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSelectedPatient.ResumeLayout(False)
        Me.PnlSelectedPatient.PerformLayout()
        CType(Me.RadTxtPrenom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadTxtNom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnDisplay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnInitialize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnRefresh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnCreatePatient, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAdmin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnMedoc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label3 As Label
    Friend WithEvents DteFiltreDateNaissance As DateTimePicker
    Friend WithEvents ChkPatientNonOasis As CheckBox
    Friend WithEvents ChkPatientTous As CheckBox
    Friend WithEvents ChkPatientOasis As CheckBox
    Friend WithEvents LblAgeSelected As Label
    Friend WithEvents LblDateNaissanceSelected As Label
    Friend WithEvents LblDateSortie As Label
    Friend WithEvents LblLabelDateSortie As Label
    Friend WithEvents LblPatientSorti As Label
    Friend WithEvents TxtNomSelected As TextBox
    Friend WithEvents TxtIdSelected As TextBox
    Friend WithEvents TxtNirSelected As TextBox
    Friend WithEvents BtnSynthese As Button
    Friend WithEvents TxtPrenomSelected As TextBox
    Friend WithEvents BtnPatientDetail As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents RadPatientGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnDetailPatient As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSynthese As Telerik.WinControls.UI.RadButton
    Friend WithEvents PnlSelectedPatient As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadTxtPrenom As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadTxtNom As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RadBtnDisplay As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnInitialize As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnRefresh As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnCreatePatient As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAdmin As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnMedoc As Telerik.WinControls.UI.RadButton
End Class
