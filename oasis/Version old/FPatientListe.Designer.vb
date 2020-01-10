<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FPatientListe
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
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PatientGridView = New System.Windows.Forms.DataGridView()
        Me.col_oa_patient_id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_nir = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_prenom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_nom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_date_naissance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_age = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patien_date_entree = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_date_sortie = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnRefresh = New System.Windows.Forms.Button()
        Me.TxtPrenom = New System.Windows.Forms.TextBox()
        Me.TxtNom = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnDisplay = New System.Windows.Forms.Button()
        Me.BtnInitialize = New System.Windows.Forms.Button()
        Me.TxtIdSelected = New System.Windows.Forms.TextBox()
        Me.TxtNirSelected = New System.Windows.Forms.TextBox()
        Me.TxtPrenomSelected = New System.Windows.Forms.TextBox()
        Me.TxtNomSelected = New System.Windows.Forms.TextBox()
        Me.BtnPatientDetail = New System.Windows.Forms.Button()
        Me.BtnSynthese = New System.Windows.Forms.Button()
        Me.BtnCreatePatient = New System.Windows.Forms.Button()
        Me.BtnMedoc = New System.Windows.Forms.Button()
        Me.PnlSelectedPatient = New System.Windows.Forms.Panel()
        Me.LblAgeSelected = New System.Windows.Forms.Label()
        Me.LblDateNaissanceSelected = New System.Windows.Forms.Label()
        Me.LblDateSortie = New System.Windows.Forms.Label()
        Me.LblLabelDateSortie = New System.Windows.Forms.Label()
        Me.LblPatientSorti = New System.Windows.Forms.Label()
        Me.ChkPatientOasis = New System.Windows.Forms.CheckBox()
        Me.ChkPatientTous = New System.Windows.Forms.CheckBox()
        Me.BtnAdmin = New System.Windows.Forms.Button()
        Me.ChkPatientNonOasis = New System.Windows.Forms.CheckBox()
        Me.DteFiltreDateNaissance = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        CType(Me.PatientGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSelectedPatient.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PatientGridView
        '
        Me.PatientGridView.AllowUserToAddRows = False
        Me.PatientGridView.AllowUserToDeleteRows = False
        Me.PatientGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PatientGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.col_oa_patient_id, Me.col_oa_patient_nir, Me.col_oa_patient_prenom, Me.col_oa_patient_nom, Me.col_oa_patient_date_naissance, Me.col_oa_patient_age, Me.col_oa_patien_date_entree, Me.col_oa_patient_date_sortie})
        Me.PatientGridView.Location = New System.Drawing.Point(7, 73)
        Me.PatientGridView.Name = "PatientGridView"
        Me.PatientGridView.ReadOnly = True
        Me.PatientGridView.RowHeadersVisible = False
        Me.PatientGridView.Size = New System.Drawing.Size(885, 389)
        Me.PatientGridView.TabIndex = 0
        '
        'col_oa_patient_id
        '
        Me.col_oa_patient_id.DataPropertyName = "oa_patient_id"
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.col_oa_patient_id.DefaultCellStyle = DataGridViewCellStyle25
        Me.col_oa_patient_id.HeaderText = "Id Oasis"
        Me.col_oa_patient_id.Name = "col_oa_patient_id"
        Me.col_oa_patient_id.ReadOnly = True
        Me.col_oa_patient_id.Width = 80
        '
        'col_oa_patient_nir
        '
        Me.col_oa_patient_nir.DataPropertyName = "oa_patient_nir"
        Me.col_oa_patient_nir.HeaderText = "NIR"
        Me.col_oa_patient_nir.Name = "col_oa_patient_nir"
        Me.col_oa_patient_nir.ReadOnly = True
        '
        'col_oa_patient_prenom
        '
        Me.col_oa_patient_prenom.DataPropertyName = "oa_patient_prenom"
        Me.col_oa_patient_prenom.HeaderText = "Prénom"
        Me.col_oa_patient_prenom.Name = "col_oa_patient_prenom"
        Me.col_oa_patient_prenom.ReadOnly = True
        Me.col_oa_patient_prenom.Width = 120
        '
        'col_oa_patient_nom
        '
        Me.col_oa_patient_nom.DataPropertyName = "oa_patient_nom"
        Me.col_oa_patient_nom.HeaderText = "Nom"
        Me.col_oa_patient_nom.Name = "col_oa_patient_nom"
        Me.col_oa_patient_nom.ReadOnly = True
        Me.col_oa_patient_nom.Width = 200
        '
        'col_oa_patient_date_naissance
        '
        Me.col_oa_patient_date_naissance.DataPropertyName = "oa_patient_date_naissance"
        DataGridViewCellStyle26.Format = "d"
        DataGridViewCellStyle26.NullValue = Nothing
        Me.col_oa_patient_date_naissance.DefaultCellStyle = DataGridViewCellStyle26
        Me.col_oa_patient_date_naissance.HeaderText = "Date naissance"
        Me.col_oa_patient_date_naissance.Name = "col_oa_patient_date_naissance"
        Me.col_oa_patient_date_naissance.ReadOnly = True
        '
        'col_oa_patient_age
        '
        Me.col_oa_patient_age.DataPropertyName = "oa_patient_age"
        Me.col_oa_patient_age.HeaderText = "Age"
        Me.col_oa_patient_age.Name = "col_oa_patient_age"
        Me.col_oa_patient_age.ReadOnly = True
        Me.col_oa_patient_age.Width = 60
        '
        'col_oa_patien_date_entree
        '
        Me.col_oa_patien_date_entree.DataPropertyName = "oa_patient_date_entree_oasis"
        DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.col_oa_patien_date_entree.DefaultCellStyle = DataGridViewCellStyle27
        Me.col_oa_patien_date_entree.HeaderText = "Date entrée"
        Me.col_oa_patien_date_entree.Name = "col_oa_patien_date_entree"
        Me.col_oa_patien_date_entree.ReadOnly = True
        '
        'col_oa_patient_date_sortie
        '
        Me.col_oa_patient_date_sortie.DataPropertyName = "oa_patient_date_sortie_oasis"
        DataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.col_oa_patient_date_sortie.DefaultCellStyle = DataGridViewCellStyle28
        Me.col_oa_patient_date_sortie.HeaderText = "Date sortie"
        Me.col_oa_patient_date_sortie.Name = "col_oa_patient_date_sortie"
        Me.col_oa_patient_date_sortie.ReadOnly = True
        '
        'BtnRefresh
        '
        Me.BtnRefresh.Location = New System.Drawing.Point(1096, 36)
        Me.BtnRefresh.Name = "BtnRefresh"
        Me.BtnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.BtnRefresh.TabIndex = 1
        Me.BtnRefresh.Text = "Refresh"
        Me.BtnRefresh.UseVisualStyleBackColor = True
        '
        'TxtPrenom
        '
        Me.TxtPrenom.Location = New System.Drawing.Point(192, 38)
        Me.TxtPrenom.Name = "TxtPrenom"
        Me.TxtPrenom.Size = New System.Drawing.Size(100, 20)
        Me.TxtPrenom.TabIndex = 2
        '
        'TxtNom
        '
        Me.TxtNom.Location = New System.Drawing.Point(333, 37)
        Me.TxtNom.Name = "TxtNom"
        Me.TxtNom.Size = New System.Drawing.Size(100, 20)
        Me.TxtNom.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(208, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Prenom"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(360, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Nom"
        '
        'BtnDisplay
        '
        Me.BtnDisplay.Location = New System.Drawing.Point(934, 36)
        Me.BtnDisplay.Name = "BtnDisplay"
        Me.BtnDisplay.Size = New System.Drawing.Size(75, 23)
        Me.BtnDisplay.TabIndex = 6
        Me.BtnDisplay.Text = "Filtrer"
        Me.BtnDisplay.UseVisualStyleBackColor = True
        '
        'BtnInitialize
        '
        Me.BtnInitialize.Location = New System.Drawing.Point(1015, 36)
        Me.BtnInitialize.Name = "BtnInitialize"
        Me.BtnInitialize.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitialize.TabIndex = 7
        Me.BtnInitialize.Text = "Initialiser"
        Me.BtnInitialize.UseVisualStyleBackColor = True
        '
        'TxtIdSelected
        '
        Me.TxtIdSelected.BackColor = System.Drawing.Color.White
        Me.TxtIdSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtIdSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtIdSelected.Location = New System.Drawing.Point(3, 11)
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
        Me.TxtNirSelected.Location = New System.Drawing.Point(71, 11)
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
        Me.TxtPrenomSelected.Location = New System.Drawing.Point(3, 36)
        Me.TxtPrenomSelected.Name = "TxtPrenomSelected"
        Me.TxtPrenomSelected.ReadOnly = True
        Me.TxtPrenomSelected.Size = New System.Drawing.Size(134, 13)
        Me.TxtPrenomSelected.TabIndex = 10
        Me.TxtPrenomSelected.Text = "Prénom"
        '
        'TxtNomSelected
        '
        Me.TxtNomSelected.BackColor = System.Drawing.Color.White
        Me.TxtNomSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtNomSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNomSelected.Location = New System.Drawing.Point(143, 30)
        Me.TxtNomSelected.Name = "TxtNomSelected"
        Me.TxtNomSelected.ReadOnly = True
        Me.TxtNomSelected.Size = New System.Drawing.Size(228, 13)
        Me.TxtNomSelected.TabIndex = 11
        Me.TxtNomSelected.Text = "Nom"
        '
        'BtnPatientDetail
        '
        Me.BtnPatientDetail.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BtnPatientDetail.Location = New System.Drawing.Point(3, 167)
        Me.BtnPatientDetail.Name = "BtnPatientDetail"
        Me.BtnPatientDetail.Size = New System.Drawing.Size(117, 23)
        Me.BtnPatientDetail.TabIndex = 13
        Me.BtnPatientDetail.Text = "Détail patient"
        Me.BtnPatientDetail.UseVisualStyleBackColor = True
        '
        'BtnSynthese
        '
        Me.BtnSynthese.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BtnSynthese.Location = New System.Drawing.Point(3, 196)
        Me.BtnSynthese.Name = "BtnSynthese"
        Me.BtnSynthese.Size = New System.Drawing.Size(117, 23)
        Me.BtnSynthese.TabIndex = 14
        Me.BtnSynthese.Text = "Outils de synthèse"
        Me.BtnSynthese.UseVisualStyleBackColor = True
        '
        'BtnCreatePatient
        '
        Me.BtnCreatePatient.Location = New System.Drawing.Point(7, 468)
        Me.BtnCreatePatient.Name = "BtnCreatePatient"
        Me.BtnCreatePatient.Size = New System.Drawing.Size(146, 23)
        Me.BtnCreatePatient.TabIndex = 15
        Me.BtnCreatePatient.Text = "Création patient Oasis"
        Me.BtnCreatePatient.UseVisualStyleBackColor = True
        '
        'BtnMedoc
        '
        Me.BtnMedoc.Location = New System.Drawing.Point(285, 468)
        Me.BtnMedoc.Name = "BtnMedoc"
        Me.BtnMedoc.Size = New System.Drawing.Size(75, 23)
        Me.BtnMedoc.TabIndex = 16
        Me.BtnMedoc.Text = "Médicaments"
        Me.BtnMedoc.UseVisualStyleBackColor = True
        Me.BtnMedoc.Visible = False
        '
        'PnlSelectedPatient
        '
        Me.PnlSelectedPatient.BackColor = System.Drawing.Color.White
        Me.PnlSelectedPatient.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlSelectedPatient.Controls.Add(Me.LblAgeSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.LblDateNaissanceSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.LblDateSortie)
        Me.PnlSelectedPatient.Controls.Add(Me.LblLabelDateSortie)
        Me.PnlSelectedPatient.Controls.Add(Me.LblPatientSorti)
        Me.PnlSelectedPatient.Controls.Add(Me.TxtNomSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.TxtIdSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.TxtNirSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.BtnSynthese)
        Me.PnlSelectedPatient.Controls.Add(Me.TxtPrenomSelected)
        Me.PnlSelectedPatient.Controls.Add(Me.BtnPatientDetail)
        Me.PnlSelectedPatient.Location = New System.Drawing.Point(898, 73)
        Me.PnlSelectedPatient.Name = "PnlSelectedPatient"
        Me.PnlSelectedPatient.Size = New System.Drawing.Size(380, 231)
        Me.PnlSelectedPatient.TabIndex = 17
        '
        'LblAgeSelected
        '
        Me.LblAgeSelected.AutoSize = True
        Me.LblAgeSelected.Location = New System.Drawing.Point(98, 61)
        Me.LblAgeSelected.Name = "LblAgeSelected"
        Me.LblAgeSelected.Size = New System.Drawing.Size(39, 13)
        Me.LblAgeSelected.TabIndex = 19
        Me.LblAgeSelected.Text = "20 ans"
        '
        'LblDateNaissanceSelected
        '
        Me.LblDateNaissanceSelected.AutoSize = True
        Me.LblDateNaissanceSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDateNaissanceSelected.Location = New System.Drawing.Point(3, 61)
        Me.LblDateNaissanceSelected.Name = "LblDateNaissanceSelected"
        Me.LblDateNaissanceSelected.Size = New System.Drawing.Size(75, 13)
        Me.LblDateNaissanceSelected.TabIndex = 18
        Me.LblDateNaissanceSelected.Text = "01/01/2000"
        '
        'LblDateSortie
        '
        Me.LblDateSortie.AutoSize = True
        Me.LblDateSortie.Location = New System.Drawing.Point(98, 86)
        Me.LblDateSortie.Name = "LblDateSortie"
        Me.LblDateSortie.Size = New System.Drawing.Size(65, 13)
        Me.LblDateSortie.TabIndex = 17
        Me.LblDateSortie.Text = "01/01/2000"
        '
        'LblLabelDateSortie
        '
        Me.LblLabelDateSortie.AutoSize = True
        Me.LblLabelDateSortie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelDateSortie.Location = New System.Drawing.Point(3, 86)
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
        Me.LblPatientSorti.Location = New System.Drawing.Point(0, 126)
        Me.LblPatientSorti.Name = "LblPatientSorti"
        Me.LblPatientSorti.Size = New System.Drawing.Size(279, 13)
        Me.LblPatientSorti.TabIndex = 15
        Me.LblPatientSorti.Text = "Attention, ce patient est sorti du dispositif Oasis"
        '
        'ChkPatientOasis
        '
        Me.ChkPatientOasis.AutoSize = True
        Me.ChkPatientOasis.Location = New System.Drawing.Point(6, 19)
        Me.ChkPatientOasis.Name = "ChkPatientOasis"
        Me.ChkPatientOasis.Size = New System.Drawing.Size(93, 17)
        Me.ChkPatientOasis.TabIndex = 18
        Me.ChkPatientOasis.Text = "Patients Oasis"
        Me.ChkPatientOasis.UseVisualStyleBackColor = True
        '
        'ChkPatientTous
        '
        Me.ChkPatientTous.AutoSize = True
        Me.ChkPatientTous.Location = New System.Drawing.Point(220, 19)
        Me.ChkPatientTous.Name = "ChkPatientTous"
        Me.ChkPatientTous.Size = New System.Drawing.Size(50, 17)
        Me.ChkPatientTous.TabIndex = 19
        Me.ChkPatientTous.Text = "Tous"
        Me.ChkPatientTous.UseVisualStyleBackColor = True
        '
        'BtnAdmin
        '
        Me.BtnAdmin.Location = New System.Drawing.Point(159, 468)
        Me.BtnAdmin.Name = "BtnAdmin"
        Me.BtnAdmin.Size = New System.Drawing.Size(120, 23)
        Me.BtnAdmin.TabIndex = 20
        Me.BtnAdmin.Text = "Administration DPI"
        Me.BtnAdmin.UseVisualStyleBackColor = True
        '
        'ChkPatientNonOasis
        '
        Me.ChkPatientNonOasis.AutoSize = True
        Me.ChkPatientNonOasis.Location = New System.Drawing.Point(105, 19)
        Me.ChkPatientNonOasis.Name = "ChkPatientNonOasis"
        Me.ChkPatientNonOasis.Size = New System.Drawing.Size(109, 17)
        Me.ChkPatientNonOasis.TabIndex = 21
        Me.ChkPatientNonOasis.Text = "Patient non Oasis"
        Me.ChkPatientNonOasis.UseVisualStyleBackColor = True
        '
        'DteFiltreDateNaissance
        '
        Me.DteFiltreDateNaissance.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DteFiltreDateNaissance.Location = New System.Drawing.Point(506, 35)
        Me.DteFiltreDateNaissance.Name = "DteFiltreDateNaissance"
        Me.DteFiltreDateNaissance.Size = New System.Drawing.Size(102, 20)
        Me.DteFiltreDateNaissance.TabIndex = 22
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(519, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 13)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Date naissance"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(90, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Filtre :"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.GroupBox1.Controls.Add(Me.ChkPatientOasis)
        Me.GroupBox1.Controls.Add(Me.ChkPatientTous)
        Me.GroupBox1.Controls.Add(Me.ChkPatientNonOasis)
        Me.GroupBox1.Location = New System.Drawing.Point(616, 18)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(275, 49)
        Me.GroupBox1.TabIndex = 25
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Sélection patient"
        '
        'FPatientListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1296, 505)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.DteFiltreDateNaissance)
        Me.Controls.Add(Me.BtnAdmin)
        Me.Controls.Add(Me.PnlSelectedPatient)
        Me.Controls.Add(Me.BtnMedoc)
        Me.Controls.Add(Me.BtnCreatePatient)
        Me.Controls.Add(Me.BtnInitialize)
        Me.Controls.Add(Me.BtnDisplay)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtNom)
        Me.Controls.Add(Me.TxtPrenom)
        Me.Controls.Add(Me.BtnRefresh)
        Me.Controls.Add(Me.PatientGridView)
        Me.Name = "FPatientListe"
        Me.Text = "Liste des patients Oasis"
        CType(Me.PatientGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSelectedPatient.ResumeLayout(False)
        Me.PnlSelectedPatient.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PatientGridView As DataGridView
    Friend WithEvents BtnRefresh As Button
    Friend WithEvents TxtPrenom As TextBox
    Friend WithEvents TxtNom As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnDisplay As Button
    Friend WithEvents BtnInitialize As Button
    Friend WithEvents TxtIdSelected As TextBox
    Friend WithEvents TxtNirSelected As TextBox
    Friend WithEvents TxtPrenomSelected As TextBox
    Friend WithEvents TxtNomSelected As TextBox
    Friend WithEvents BtnPatientDetail As Button
    Friend WithEvents BtnSynthese As Button
    Friend WithEvents BtnCreatePatient As Button
    Friend WithEvents BtnMedoc As Button
    Friend WithEvents PnlSelectedPatient As Panel
    Friend WithEvents ChkPatientOasis As CheckBox
    Friend WithEvents ChkPatientTous As CheckBox
    Friend WithEvents LblPatientSorti As Label
    Friend WithEvents LblDateSortie As Label
    Friend WithEvents LblLabelDateSortie As Label
    Friend WithEvents col_oa_patient_id As DataGridViewTextBoxColumn
    Friend WithEvents col_oa_patient_nir As DataGridViewTextBoxColumn
    Friend WithEvents col_oa_patient_prenom As DataGridViewTextBoxColumn
    Friend WithEvents col_oa_patient_nom As DataGridViewTextBoxColumn
    Friend WithEvents col_oa_patient_date_naissance As DataGridViewTextBoxColumn
    Friend WithEvents col_oa_patient_age As DataGridViewTextBoxColumn
    Friend WithEvents col_oa_patien_date_entree As DataGridViewTextBoxColumn
    Friend WithEvents col_oa_patient_date_sortie As DataGridViewTextBoxColumn
    Friend WithEvents BtnAdmin As Button
    Friend WithEvents LblAgeSelected As Label
    Friend WithEvents LblDateNaissanceSelected As Label
    Friend WithEvents ChkPatientNonOasis As CheckBox
    Friend WithEvents DteFiltreDateNaissance As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents GroupBox1 As GroupBox
End Class
