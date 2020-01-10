<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FMedocSelecteur
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
        Me.MedicamentGridView = New System.Windows.Forms.DataGridView()
        Me.col_medicament_cis = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_dci = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_forme = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_voie_administration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_etat_commercialisation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_titulaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ContextMenuStripMedicament = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DétailMédicamentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TxtDCI = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnFiltrer = New System.Windows.Forms.Button()
        Me.BtnInitialiser = New System.Windows.Forms.Button()
        Me.TxtCIS = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtLabo = New System.Windows.Forms.TextBox()
        Me.LblMedicamentCis = New System.Windows.Forms.Label()
        Me.LblMedicamentDci = New System.Windows.Forms.Label()
        Me.LblMedicamentForme = New System.Windows.Forms.Label()
        Me.BtnSelect = New System.Windows.Forms.Button()
        Me.PnlSelectedMedicament = New System.Windows.Forms.Panel()
        Me.LblMedicamentAlerte = New System.Windows.Forms.Label()
        Me.LblAllergie = New System.Windows.Forms.Label()
        Me.lblContreIndication = New System.Windows.Forms.Label()
        CType(Me.MedicamentGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripMedicament.SuspendLayout()
        Me.PnlSelectedMedicament.SuspendLayout()
        Me.SuspendLayout()
        '
        'MedicamentGridView
        '
        Me.MedicamentGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.MedicamentGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.col_medicament_cis, Me.col_medicament_dci, Me.col_medicament_forme, Me.col_medicament_voie_administration, Me.col_medicament_etat_commercialisation, Me.col_medicament_titulaire})
        Me.MedicamentGridView.ContextMenuStrip = Me.ContextMenuStripMedicament
        Me.MedicamentGridView.Location = New System.Drawing.Point(26, 106)
        Me.MedicamentGridView.Name = "MedicamentGridView"
        Me.MedicamentGridView.Size = New System.Drawing.Size(1403, 275)
        Me.MedicamentGridView.TabIndex = 0
        '
        'col_medicament_cis
        '
        Me.col_medicament_cis.DataPropertyName = "oa_medicament_cis"
        Me.col_medicament_cis.FillWeight = 80.0!
        Me.col_medicament_cis.HeaderText = "CIS"
        Me.col_medicament_cis.Name = "col_medicament_cis"
        Me.col_medicament_cis.ReadOnly = True
        Me.col_medicament_cis.Width = 70
        '
        'col_medicament_dci
        '
        Me.col_medicament_dci.DataPropertyName = "oa_medicament_dci"
        Me.col_medicament_dci.FillWeight = 250.0!
        Me.col_medicament_dci.HeaderText = "Dénomination"
        Me.col_medicament_dci.Name = "col_medicament_dci"
        Me.col_medicament_dci.ReadOnly = True
        Me.col_medicament_dci.Width = 450
        '
        'col_medicament_forme
        '
        Me.col_medicament_forme.DataPropertyName = "oa_medicament_forme"
        Me.col_medicament_forme.HeaderText = "Forme"
        Me.col_medicament_forme.Name = "col_medicament_forme"
        Me.col_medicament_forme.ReadOnly = True
        Me.col_medicament_forme.Width = 300
        '
        'col_medicament_voie_administration
        '
        Me.col_medicament_voie_administration.DataPropertyName = "oa_medicament_voie_administration"
        Me.col_medicament_voie_administration.HeaderText = "Administration"
        Me.col_medicament_voie_administration.Name = "col_medicament_voie_administration"
        Me.col_medicament_voie_administration.ReadOnly = True
        Me.col_medicament_voie_administration.Width = 200
        '
        'col_medicament_etat_commercialisation
        '
        Me.col_medicament_etat_commercialisation.DataPropertyName = "oa_medicament_etat_commercialisation"
        Me.col_medicament_etat_commercialisation.HeaderText = "Commercialisation"
        Me.col_medicament_etat_commercialisation.Name = "col_medicament_etat_commercialisation"
        Me.col_medicament_etat_commercialisation.ReadOnly = True
        '
        'col_medicament_titulaire
        '
        Me.col_medicament_titulaire.DataPropertyName = "oa_medicament_titulaire"
        Me.col_medicament_titulaire.HeaderText = "Labo"
        Me.col_medicament_titulaire.Name = "col_medicament_titulaire"
        Me.col_medicament_titulaire.ReadOnly = True
        Me.col_medicament_titulaire.Width = 200
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
        'TxtDCI
        '
        Me.TxtDCI.Location = New System.Drawing.Point(109, 62)
        Me.TxtDCI.Name = "TxtDCI"
        Me.TxtDCI.Size = New System.Drawing.Size(100, 20)
        Me.TxtDCI.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(31, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Dénomination"
        '
        'BtnFiltrer
        '
        Me.BtnFiltrer.Location = New System.Drawing.Point(671, 60)
        Me.BtnFiltrer.Name = "BtnFiltrer"
        Me.BtnFiltrer.Size = New System.Drawing.Size(75, 23)
        Me.BtnFiltrer.TabIndex = 1
        Me.BtnFiltrer.Text = "Filtrer"
        Me.BtnFiltrer.UseVisualStyleBackColor = True
        '
        'BtnInitialiser
        '
        Me.BtnInitialiser.Location = New System.Drawing.Point(778, 60)
        Me.BtnInitialiser.Name = "BtnInitialiser"
        Me.BtnInitialiser.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitialiser.TabIndex = 4
        Me.BtnInitialiser.Text = "Initialiser"
        Me.BtnInitialiser.UseVisualStyleBackColor = True
        '
        'TxtCIS
        '
        Me.TxtCIS.Location = New System.Drawing.Point(423, 62)
        Me.TxtCIS.Name = "TxtCIS"
        Me.TxtCIS.Size = New System.Drawing.Size(100, 20)
        Me.TxtCIS.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(393, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(24, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "CIS"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(231, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Labo"
        '
        'TxtLabo
        '
        Me.TxtLabo.Location = New System.Drawing.Point(268, 62)
        Me.TxtLabo.Name = "TxtLabo"
        Me.TxtLabo.Size = New System.Drawing.Size(100, 20)
        Me.TxtLabo.TabIndex = 8
        '
        'LblMedicamentCis
        '
        Me.LblMedicamentCis.AutoSize = True
        Me.LblMedicamentCis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMedicamentCis.Location = New System.Drawing.Point(3, 13)
        Me.LblMedicamentCis.Name = "LblMedicamentCis"
        Me.LblMedicamentCis.Size = New System.Drawing.Size(63, 13)
        Me.LblMedicamentCis.TabIndex = 9
        Me.LblMedicamentCis.Text = "60005856"
        '
        'LblMedicamentDci
        '
        Me.LblMedicamentDci.AutoSize = True
        Me.LblMedicamentDci.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMedicamentDci.Location = New System.Drawing.Point(3, 35)
        Me.LblMedicamentDci.Name = "LblMedicamentDci"
        Me.LblMedicamentDci.Size = New System.Drawing.Size(657, 13)
        Me.LblMedicamentDci.TabIndex = 10
        Me.LblMedicamentDci.Text = "RHODODENDRON FERRUGINEUM WELEDA, degre de dilution compris entre 2CH et 30CH ou e" &
    "ntre 4DH et 60DH"
        '
        'LblMedicamentForme
        '
        Me.LblMedicamentForme.AutoSize = True
        Me.LblMedicamentForme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMedicamentForme.Location = New System.Drawing.Point(3, 59)
        Me.LblMedicamentForme.Name = "LblMedicamentForme"
        Me.LblMedicamentForme.Size = New System.Drawing.Size(250, 13)
        Me.LblMedicamentForme.TabIndex = 11
        Me.LblMedicamentForme.Text = "granules et  solution en gouttes en gouttes"
        '
        'BtnSelect
        '
        Me.BtnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BtnSelect.Location = New System.Drawing.Point(6, 84)
        Me.BtnSelect.Name = "BtnSelect"
        Me.BtnSelect.Size = New System.Drawing.Size(142, 23)
        Me.BtnSelect.TabIndex = 12
        Me.BtnSelect.Text = "Confirmer la sélection"
        Me.BtnSelect.UseVisualStyleBackColor = True
        '
        'PnlSelectedMedicament
        '
        Me.PnlSelectedMedicament.BackColor = System.Drawing.Color.Cornsilk
        Me.PnlSelectedMedicament.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlSelectedMedicament.Controls.Add(Me.LblMedicamentCis)
        Me.PnlSelectedMedicament.Controls.Add(Me.BtnSelect)
        Me.PnlSelectedMedicament.Controls.Add(Me.LblMedicamentDci)
        Me.PnlSelectedMedicament.Controls.Add(Me.LblMedicamentForme)
        Me.PnlSelectedMedicament.Location = New System.Drawing.Point(26, 399)
        Me.PnlSelectedMedicament.Name = "PnlSelectedMedicament"
        Me.PnlSelectedMedicament.Size = New System.Drawing.Size(689, 117)
        Me.PnlSelectedMedicament.TabIndex = 13
        '
        'LblMedicamentAlerte
        '
        Me.LblMedicamentAlerte.AutoSize = True
        Me.LblMedicamentAlerte.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblMedicamentAlerte.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMedicamentAlerte.ForeColor = System.Drawing.Color.Red
        Me.LblMedicamentAlerte.Location = New System.Drawing.Point(729, 434)
        Me.LblMedicamentAlerte.Name = "LblMedicamentAlerte"
        Me.LblMedicamentAlerte.Size = New System.Drawing.Size(71, 22)
        Me.LblMedicamentAlerte.TabIndex = 14
        Me.LblMedicamentAlerte.Text = "Allergie"
        '
        'LblAllergie
        '
        Me.LblAllergie.AutoSize = True
        Me.LblAllergie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAllergie.ForeColor = System.Drawing.Color.DarkBlue
        Me.LblAllergie.Location = New System.Drawing.Point(972, 65)
        Me.LblAllergie.Name = "LblAllergie"
        Me.LblAllergie.Size = New System.Drawing.Size(63, 13)
        Me.LblAllergie.TabIndex = 15
        Me.LblAllergie.Text = "Allergie(s)"
        '
        'lblContreIndication
        '
        Me.lblContreIndication.AutoSize = True
        Me.lblContreIndication.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContreIndication.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblContreIndication.Location = New System.Drawing.Point(1137, 65)
        Me.lblContreIndication.Name = "lblContreIndication"
        Me.lblContreIndication.Size = New System.Drawing.Size(117, 13)
        Me.lblContreIndication.TabIndex = 16
        Me.lblContreIndication.Text = "Contre-indication(s)"
        '
        'FMedocSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1434, 542)
        Me.Controls.Add(Me.lblContreIndication)
        Me.Controls.Add(Me.LblAllergie)
        Me.Controls.Add(Me.LblMedicamentAlerte)
        Me.Controls.Add(Me.PnlSelectedMedicament)
        Me.Controls.Add(Me.TxtLabo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtCIS)
        Me.Controls.Add(Me.BtnInitialiser)
        Me.Controls.Add(Me.BtnFiltrer)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtDCI)
        Me.Controls.Add(Me.MedicamentGridView)
        Me.Name = "FMedocSelecteur"
        Me.Text = "Sélectionner un médicament"
        CType(Me.MedicamentGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripMedicament.ResumeLayout(False)
        Me.PnlSelectedMedicament.ResumeLayout(False)
        Me.PnlSelectedMedicament.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MedicamentGridView As DataGridView
    Friend WithEvents TxtDCI As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnFiltrer As Button
    Friend WithEvents BtnInitialiser As Button
    Friend WithEvents TxtCIS As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents TxtLabo As TextBox
    Friend WithEvents LblMedicamentCis As Label
    Friend WithEvents LblMedicamentDci As Label
    Friend WithEvents LblMedicamentForme As Label
    Friend WithEvents BtnSelect As Button
    Friend WithEvents PnlSelectedMedicament As Panel
    Friend WithEvents LblMedicamentAlerte As Label
    Friend WithEvents ContextMenuStripMedicament As ContextMenuStrip
    Friend WithEvents DétailMédicamentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents col_medicament_cis As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_dci As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_forme As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_voie_administration As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_etat_commercialisation As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_titulaire As DataGridViewTextBoxColumn
    Friend WithEvents LblAllergie As Label
    Friend WithEvents lblContreIndication As Label
End Class
