<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FPatientListeDoublons
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PatientGridView = New System.Windows.Forms.DataGridView()
        Me.col_oa_patient_id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_nir = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_prenom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_nom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_date_naissance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_age = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patien_date_entree = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_oa_patient_date_sortie = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TxtIdSelected = New System.Windows.Forms.TextBox()
        Me.TxtNirSelected = New System.Windows.Forms.TextBox()
        Me.TxtPrenomSelected = New System.Windows.Forms.TextBox()
        Me.TxtNomSelected = New System.Windows.Forms.TextBox()
        Me.BtnSynthese = New System.Windows.Forms.Button()
        Me.PnlSelectedPatient = New System.Windows.Forms.Panel()
        Me.LblAgeSelected = New System.Windows.Forms.Label()
        Me.LblDateNaissanceSelected = New System.Windows.Forms.Label()
        Me.LblDateSortie = New System.Windows.Forms.Label()
        Me.LblLabelDateSortie = New System.Windows.Forms.Label()
        Me.LblPatientSorti = New System.Windows.Forms.Label()
        Me.BtnRetourListePatient = New System.Windows.Forms.Button()
        Me.BtnCreationPatient = New System.Windows.Forms.Button()
        CType(Me.PatientGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSelectedPatient.SuspendLayout()
        Me.SuspendLayout()
        '
        'PatientGridView
        '
        Me.PatientGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PatientGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.col_oa_patient_id, Me.col_oa_patient_nir, Me.col_oa_patient_prenom, Me.col_oa_patient_nom, Me.col_oa_patient_date_naissance, Me.col_oa_patient_age, Me.col_oa_patien_date_entree, Me.col_oa_patient_date_sortie})
        Me.PatientGridView.Location = New System.Drawing.Point(12, 12)
        Me.PatientGridView.Name = "PatientGridView"
        Me.PatientGridView.Size = New System.Drawing.Size(906, 291)
        Me.PatientGridView.TabIndex = 0
        '
        'col_oa_patient_id
        '
        Me.col_oa_patient_id.DataPropertyName = "oa_patient_id"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.col_oa_patient_id.DefaultCellStyle = DataGridViewCellStyle5
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
        DataGridViewCellStyle6.Format = "d"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.col_oa_patient_date_naissance.DefaultCellStyle = DataGridViewCellStyle6
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
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.col_oa_patien_date_entree.DefaultCellStyle = DataGridViewCellStyle7
        Me.col_oa_patien_date_entree.HeaderText = "Date entrée"
        Me.col_oa_patien_date_entree.Name = "col_oa_patien_date_entree"
        Me.col_oa_patien_date_entree.ReadOnly = True
        '
        'col_oa_patient_date_sortie
        '
        Me.col_oa_patient_date_sortie.DataPropertyName = "oa_patient_date_sortie_oasis"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.col_oa_patient_date_sortie.DefaultCellStyle = DataGridViewCellStyle8
        Me.col_oa_patient_date_sortie.HeaderText = "Date sortie"
        Me.col_oa_patient_date_sortie.Name = "col_oa_patient_date_sortie"
        Me.col_oa_patient_date_sortie.ReadOnly = True
        '
        'TxtIdSelected
        '
        Me.TxtIdSelected.BackColor = System.Drawing.Color.Cornsilk
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
        Me.TxtNirSelected.BackColor = System.Drawing.Color.Cornsilk
        Me.TxtNirSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtNirSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNirSelected.Location = New System.Drawing.Point(3, 38)
        Me.TxtNirSelected.Name = "TxtNirSelected"
        Me.TxtNirSelected.ReadOnly = True
        Me.TxtNirSelected.Size = New System.Drawing.Size(100, 13)
        Me.TxtNirSelected.TabIndex = 9
        Me.TxtNirSelected.Text = "Nir"
        '
        'TxtPrenomSelected
        '
        Me.TxtPrenomSelected.BackColor = System.Drawing.Color.Cornsilk
        Me.TxtPrenomSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtPrenomSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPrenomSelected.Location = New System.Drawing.Point(3, 65)
        Me.TxtPrenomSelected.Name = "TxtPrenomSelected"
        Me.TxtPrenomSelected.ReadOnly = True
        Me.TxtPrenomSelected.Size = New System.Drawing.Size(134, 13)
        Me.TxtPrenomSelected.TabIndex = 10
        Me.TxtPrenomSelected.Text = "Prénom"
        '
        'TxtNomSelected
        '
        Me.TxtNomSelected.BackColor = System.Drawing.Color.Cornsilk
        Me.TxtNomSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtNomSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNomSelected.Location = New System.Drawing.Point(3, 92)
        Me.TxtNomSelected.Name = "TxtNomSelected"
        Me.TxtNomSelected.ReadOnly = True
        Me.TxtNomSelected.Size = New System.Drawing.Size(228, 13)
        Me.TxtNomSelected.TabIndex = 11
        Me.TxtNomSelected.Text = "Nom"
        '
        'BtnSynthese
        '
        Me.BtnSynthese.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BtnSynthese.Location = New System.Drawing.Point(3, 253)
        Me.BtnSynthese.Name = "BtnSynthese"
        Me.BtnSynthese.Size = New System.Drawing.Size(117, 23)
        Me.BtnSynthese.TabIndex = 14
        Me.BtnSynthese.Text = "Outils de synthèse"
        Me.BtnSynthese.UseVisualStyleBackColor = True
        '
        'PnlSelectedPatient
        '
        Me.PnlSelectedPatient.BackColor = System.Drawing.Color.Cornsilk
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
        Me.PnlSelectedPatient.Location = New System.Drawing.Point(924, 12)
        Me.PnlSelectedPatient.Name = "PnlSelectedPatient"
        Me.PnlSelectedPatient.Size = New System.Drawing.Size(351, 291)
        Me.PnlSelectedPatient.TabIndex = 17
        '
        'LblAgeSelected
        '
        Me.LblAgeSelected.AutoSize = True
        Me.LblAgeSelected.Location = New System.Drawing.Point(94, 119)
        Me.LblAgeSelected.Name = "LblAgeSelected"
        Me.LblAgeSelected.Size = New System.Drawing.Size(39, 13)
        Me.LblAgeSelected.TabIndex = 19
        Me.LblAgeSelected.Text = "20 ans"
        '
        'LblDateNaissanceSelected
        '
        Me.LblDateNaissanceSelected.AutoSize = True
        Me.LblDateNaissanceSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDateNaissanceSelected.Location = New System.Drawing.Point(3, 119)
        Me.LblDateNaissanceSelected.Name = "LblDateNaissanceSelected"
        Me.LblDateNaissanceSelected.Size = New System.Drawing.Size(75, 13)
        Me.LblDateNaissanceSelected.TabIndex = 18
        Me.LblDateNaissanceSelected.Text = "01/01/2000"
        '
        'LblDateSortie
        '
        Me.LblDateSortie.AutoSize = True
        Me.LblDateSortie.Location = New System.Drawing.Point(68, 159)
        Me.LblDateSortie.Name = "LblDateSortie"
        Me.LblDateSortie.Size = New System.Drawing.Size(65, 13)
        Me.LblDateSortie.TabIndex = 17
        Me.LblDateSortie.Text = "01/01/2000"
        '
        'LblLabelDateSortie
        '
        Me.LblLabelDateSortie.AutoSize = True
        Me.LblLabelDateSortie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelDateSortie.Location = New System.Drawing.Point(0, 159)
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
        Me.LblPatientSorti.Location = New System.Drawing.Point(0, 183)
        Me.LblPatientSorti.Name = "LblPatientSorti"
        Me.LblPatientSorti.Size = New System.Drawing.Size(279, 13)
        Me.LblPatientSorti.TabIndex = 15
        Me.LblPatientSorti.Text = "Attention, ce patient est sorti du dispositif Oasis"
        '
        'BtnRetourListePatient
        '
        Me.BtnRetourListePatient.Location = New System.Drawing.Point(776, 310)
        Me.BtnRetourListePatient.Name = "BtnRetourListePatient"
        Me.BtnRetourListePatient.Size = New System.Drawing.Size(142, 23)
        Me.BtnRetourListePatient.TabIndex = 18
        Me.BtnRetourListePatient.Text = "Retour liste des patients"
        Me.BtnRetourListePatient.UseVisualStyleBackColor = True
        '
        'BtnCreationPatient
        '
        Me.BtnCreationPatient.Location = New System.Drawing.Point(631, 310)
        Me.BtnCreationPatient.Name = "BtnCreationPatient"
        Me.BtnCreationPatient.Size = New System.Drawing.Size(139, 23)
        Me.BtnCreationPatient.TabIndex = 19
        Me.BtnCreationPatient.Text = "Retour création patient"
        Me.BtnCreationPatient.UseVisualStyleBackColor = True
        '
        'FPatientListeDoublons
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1306, 345)
        Me.Controls.Add(Me.BtnCreationPatient)
        Me.Controls.Add(Me.BtnRetourListePatient)
        Me.Controls.Add(Me.PnlSelectedPatient)
        Me.Controls.Add(Me.PatientGridView)
        Me.Name = "FPatientListeDoublons"
        Me.Text = "Liste des patients Oasis"
        CType(Me.PatientGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSelectedPatient.ResumeLayout(False)
        Me.PnlSelectedPatient.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PatientGridView As DataGridView
    Friend WithEvents TxtIdSelected As TextBox
    Friend WithEvents TxtNirSelected As TextBox
    Friend WithEvents TxtPrenomSelected As TextBox
    Friend WithEvents TxtNomSelected As TextBox
    Friend WithEvents BtnSynthese As Button
    Friend WithEvents PnlSelectedPatient As Panel
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
    Friend WithEvents LblAgeSelected As Label
    Friend WithEvents LblDateNaissanceSelected As Label
    Friend WithEvents BtnRetourListePatient As Button
    Friend WithEvents BtnCreationPatient As Button
End Class
