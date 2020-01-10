<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FMedocDetail
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
        Me.LblMedicamentCIS = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.LblMedicamentDCI = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.LblMedicamentForme = New System.Windows.Forms.Label()
        Me.LblMedicamentAdministration = New System.Windows.Forms.Label()
        Me.LblMedicamentTitulaire = New System.Windows.Forms.Label()
        Me.medicamentPresentationDataGridView = New System.Windows.Forms.DataGridView()
        Me.CIP7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Presentation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.medicamentCompoDataGridView = New System.Windows.Forms.DataGridView()
        Me.Designation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Denomination = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Dosage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReferenceDosage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Nature = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        CType(Me.medicamentPresentationDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.medicamentCompoDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblMedicamentCIS
        '
        Me.LblMedicamentCIS.AutoSize = True
        Me.LblMedicamentCIS.Location = New System.Drawing.Point(178, 13)
        Me.LblMedicamentCIS.Name = "LblMedicamentCIS"
        Me.LblMedicamentCIS.Size = New System.Drawing.Size(55, 13)
        Me.LblMedicamentCIS.TabIndex = 36
        Me.LblMedicamentCIS.Text = "60002283"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(3, 13)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(35, 13)
        Me.Label10.TabIndex = 35
        Me.Label10.Text = "CIS :"
        '
        'LblMedicamentDCI
        '
        Me.LblMedicamentDCI.AutoSize = True
        Me.LblMedicamentDCI.Location = New System.Drawing.Point(178, 39)
        Me.LblMedicamentDCI.Name = "LblMedicamentDCI"
        Me.LblMedicamentDCI.Size = New System.Drawing.Size(253, 13)
        Me.LblMedicamentDCI.TabIndex = 34
        Me.LblMedicamentDCI.Text = "ANASTROZOLE ACCORD 1 mg, comprime pellicule"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(3, 39)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(166, 13)
        Me.Label6.TabIndex = 33
        Me.Label6.Text = "Dénomination commerciale :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 41
        Me.Label5.Text = "Forme :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(3, 126)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(94, 13)
        Me.Label7.TabIndex = 42
        Me.Label7.Text = "Administration :"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 64)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 13)
        Me.Label8.TabIndex = 43
        Me.Label8.Text = "Titulaire :"
        '
        'LblMedicamentForme
        '
        Me.LblMedicamentForme.AutoSize = True
        Me.LblMedicamentForme.Location = New System.Drawing.Point(178, 96)
        Me.LblMedicamentForme.Name = "LblMedicamentForme"
        Me.LblMedicamentForme.Size = New System.Drawing.Size(93, 13)
        Me.LblMedicamentForme.TabIndex = 49
        Me.LblMedicamentForme.Text = "comprime pellicule"
        '
        'LblMedicamentAdministration
        '
        Me.LblMedicamentAdministration.AutoSize = True
        Me.LblMedicamentAdministration.Location = New System.Drawing.Point(178, 126)
        Me.LblMedicamentAdministration.Name = "LblMedicamentAdministration"
        Me.LblMedicamentAdministration.Size = New System.Drawing.Size(30, 13)
        Me.LblMedicamentAdministration.TabIndex = 50
        Me.LblMedicamentAdministration.Text = "orale"
        '
        'LblMedicamentTitulaire
        '
        Me.LblMedicamentTitulaire.AutoSize = True
        Me.LblMedicamentTitulaire.Location = New System.Drawing.Point(178, 64)
        Me.LblMedicamentTitulaire.Name = "LblMedicamentTitulaire"
        Me.LblMedicamentTitulaire.Size = New System.Drawing.Size(173, 13)
        Me.LblMedicamentTitulaire.TabIndex = 51
        Me.LblMedicamentTitulaire.Text = "ACCORD HEALTHCARE FRANCE"
        '
        'medicamentPresentationDataGridView
        '
        Me.medicamentPresentationDataGridView.AllowUserToAddRows = False
        Me.medicamentPresentationDataGridView.AllowUserToDeleteRows = False
        Me.medicamentPresentationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.medicamentPresentationDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CIP7, Me.Presentation})
        Me.medicamentPresentationDataGridView.Location = New System.Drawing.Point(10, 309)
        Me.medicamentPresentationDataGridView.Name = "medicamentPresentationDataGridView"
        Me.medicamentPresentationDataGridView.ReadOnly = True
        Me.medicamentPresentationDataGridView.RowHeadersVisible = False
        Me.medicamentPresentationDataGridView.Size = New System.Drawing.Size(703, 93)
        Me.medicamentPresentationDataGridView.TabIndex = 52
        '
        'CIP7
        '
        Me.CIP7.HeaderText = "CIP7"
        Me.CIP7.Name = "CIP7"
        Me.CIP7.ReadOnly = True
        '
        'Presentation
        '
        Me.Presentation.HeaderText = "Présentation"
        Me.Presentation.Name = "Presentation"
        Me.Presentation.ReadOnly = True
        Me.Presentation.Width = 600
        '
        'medicamentCompoDataGridView
        '
        Me.medicamentCompoDataGridView.AllowUserToAddRows = False
        Me.medicamentCompoDataGridView.AllowUserToDeleteRows = False
        Me.medicamentCompoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.medicamentCompoDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Designation, Me.Denomination, Me.Dosage, Me.ReferenceDosage, Me.Nature})
        Me.medicamentCompoDataGridView.Location = New System.Drawing.Point(10, 168)
        Me.medicamentCompoDataGridView.Name = "medicamentCompoDataGridView"
        Me.medicamentCompoDataGridView.ReadOnly = True
        Me.medicamentCompoDataGridView.RowHeadersVisible = False
        Me.medicamentCompoDataGridView.Size = New System.Drawing.Size(907, 135)
        Me.medicamentCompoDataGridView.TabIndex = 53
        '
        'Designation
        '
        Me.Designation.HeaderText = "Designation"
        Me.Designation.Name = "Designation"
        Me.Designation.ReadOnly = True
        Me.Designation.Visible = False
        Me.Designation.Width = 150
        '
        'Denomination
        '
        Me.Denomination.HeaderText = "Dénomination"
        Me.Denomination.Name = "Denomination"
        Me.Denomination.ReadOnly = True
        Me.Denomination.Width = 300
        '
        'Dosage
        '
        Me.Dosage.HeaderText = "Dosage"
        Me.Dosage.Name = "Dosage"
        Me.Dosage.ReadOnly = True
        Me.Dosage.Width = 200
        '
        'ReferenceDosage
        '
        Me.ReferenceDosage.HeaderText = "Référence dosage"
        Me.ReferenceDosage.Name = "ReferenceDosage"
        Me.ReferenceDosage.ReadOnly = True
        Me.ReferenceDosage.Width = 200
        '
        'Nature
        '
        Me.Nature.HeaderText = "Nature"
        Me.Nature.Name = "Nature"
        Me.Nature.ReadOnly = True
        Me.Nature.Width = 200
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox1.Controls.Add(Me.LblMedicamentTitulaire)
        Me.GroupBox1.Controls.Add(Me.LblMedicamentAdministration)
        Me.GroupBox1.Controls.Add(Me.LblMedicamentForme)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.LblMedicamentCIS)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.LblMedicamentDCI)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(907, 150)
        Me.GroupBox1.TabIndex = 54
        Me.GroupBox1.TabStop = False
        '
        'FMedocDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(927, 409)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.medicamentCompoDataGridView)
        Me.Controls.Add(Me.medicamentPresentationDataGridView)
        Me.Name = "FMedocDetail"
        Me.Text = "Détail d'un médicament"
        CType(Me.medicamentPresentationDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.medicamentCompoDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LblMedicamentCIS As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents LblMedicamentDCI As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents LblMedicamentForme As Label
    Friend WithEvents LblMedicamentAdministration As Label
    Friend WithEvents LblMedicamentTitulaire As Label
    Friend WithEvents medicamentPresentationDataGridView As DataGridView
    Friend WithEvents CIP7 As DataGridViewTextBoxColumn
    Friend WithEvents Presentation As DataGridViewTextBoxColumn
    Friend WithEvents medicamentCompoDataGridView As DataGridView
    Friend WithEvents Designation As DataGridViewTextBoxColumn
    Friend WithEvents Denomination As DataGridViewTextBoxColumn
    Friend WithEvents Dosage As DataGridViewTextBoxColumn
    Friend WithEvents ReferenceDosage As DataGridViewTextBoxColumn
    Friend WithEvents Nature As DataGridViewTextBoxColumn
    Friend WithEvents GroupBox1 As GroupBox
End Class
