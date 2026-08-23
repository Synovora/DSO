<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FMedocListe
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
        Me.MedicamentGridView = New System.Windows.Forms.DataGridView()
        Me.TxtDCI = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnFiltrer = New System.Windows.Forms.Button()
        Me.BtnInitialiser = New System.Windows.Forms.Button()
        Me.TxtCIS = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtLabo = New System.Windows.Forms.TextBox()
        Me.col_medicament_cis = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_dci = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_forme = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_voie_administration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_etat_commercialisation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_medicament_titulaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.MedicamentGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MedicamentGridView
        '
        Me.MedicamentGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.MedicamentGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.col_medicament_cis, Me.col_medicament_dci, Me.col_medicament_forme, Me.col_medicament_voie_administration, Me.col_medicament_etat_commercialisation, Me.col_medicament_titulaire})
        Me.MedicamentGridView.Location = New System.Drawing.Point(26, 106)
        Me.MedicamentGridView.Name = "MedicamentGridView"
        Me.MedicamentGridView.Size = New System.Drawing.Size(1416, 275)
        Me.MedicamentGridView.TabIndex = 0
        '
        'TxtDCI
        '
        Me.TxtDCI.Location = New System.Drawing.Point(228, 62)
        Me.TxtDCI.Name = "TxtDCI"
        Me.TxtDCI.Size = New System.Drawing.Size(100, 20)
        Me.TxtDCI.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(197, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(25, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "DCI"
        '
        'BtnFiltrer
        '
        Me.BtnFiltrer.Location = New System.Drawing.Point(561, 56)
        Me.BtnFiltrer.Name = "BtnFiltrer"
        Me.BtnFiltrer.Size = New System.Drawing.Size(75, 23)
        Me.BtnFiltrer.TabIndex = 3
        Me.BtnFiltrer.Text = "Filtrer"
        Me.BtnFiltrer.UseVisualStyleBackColor = True
        '
        'BtnInitialiser
        '
        Me.BtnInitialiser.Location = New System.Drawing.Point(671, 55)
        Me.BtnInitialiser.Name = "BtnInitialiser"
        Me.BtnInitialiser.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitialiser.TabIndex = 4
        Me.BtnInitialiser.Text = "Initialiser"
        Me.BtnInitialiser.UseVisualStyleBackColor = True
        '
        'TxtCIS
        '
        Me.TxtCIS.Location = New System.Drawing.Point(68, 62)
        Me.TxtCIS.Name = "TxtCIS"
        Me.TxtCIS.Size = New System.Drawing.Size(100, 20)
        Me.TxtCIS.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(38, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(24, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "CIS"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(350, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Labo"
        '
        'TxtLabo
        '
        Me.TxtLabo.Location = New System.Drawing.Point(416, 62)
        Me.TxtLabo.Name = "TxtLabo"
        Me.TxtLabo.Size = New System.Drawing.Size(100, 20)
        Me.TxtLabo.TabIndex = 8
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
        Me.col_medicament_dci.HeaderText = "DCI"
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
        'Form6
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1449, 450)
        Me.Controls.Add(Me.TxtLabo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtCIS)
        Me.Controls.Add(Me.BtnInitialiser)
        Me.Controls.Add(Me.BtnFiltrer)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtDCI)
        Me.Controls.Add(Me.MedicamentGridView)
        Me.Name = "Form6"
        Me.Text = "Liste des médicaments"
        CType(Me.MedicamentGridView, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents col_medicament_cis As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_dci As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_forme As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_voie_administration As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_etat_commercialisation As DataGridViewTextBoxColumn
    Friend WithEvents col_medicament_titulaire As DataGridViewTextBoxColumn
End Class
