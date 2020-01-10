<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FAldCim10Selecteur
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.AldDataGridView = New System.Windows.Forms.DataGridView()
        Me.BtnSelection = New System.Windows.Forms.Button()
        Me.LblCim10Code = New System.Windows.Forms.Label()
        Me.LblCim10Description = New System.Windows.Forms.Label()
        Me.PnlSelection = New System.Windows.Forms.Panel()
        Me.oa_ald_cim10_id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_ald_cim10_ald_code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_ald_cim10_code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_ald_cim10_description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.AldDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSelection.SuspendLayout()
        Me.SuspendLayout()
        '
        'AldDataGridView
        '
        Me.AldDataGridView.AllowUserToAddRows = False
        Me.AldDataGridView.AllowUserToDeleteRows = False
        Me.AldDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AldDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.oa_ald_cim10_id, Me.oa_ald_cim10_ald_code, Me.oa_ald_cim10_code, Me.oa_ald_cim10_description})
        Me.AldDataGridView.Location = New System.Drawing.Point(12, 51)
        Me.AldDataGridView.Name = "AldDataGridView"
        Me.AldDataGridView.ReadOnly = True
        Me.AldDataGridView.RowHeadersVisible = False
        Me.AldDataGridView.Size = New System.Drawing.Size(667, 298)
        Me.AldDataGridView.TabIndex = 0
        '
        'BtnSelection
        '
        Me.BtnSelection.Location = New System.Drawing.Point(19, 82)
        Me.BtnSelection.Name = "BtnSelection"
        Me.BtnSelection.Size = New System.Drawing.Size(75, 23)
        Me.BtnSelection.TabIndex = 0
        Me.BtnSelection.Text = "Sélectionner"
        Me.BtnSelection.UseVisualStyleBackColor = True
        '
        'LblCim10Code
        '
        Me.LblCim10Code.AutoSize = True
        Me.LblCim10Code.Location = New System.Drawing.Point(20, 46)
        Me.LblCim10Code.Name = "LblCim10Code"
        Me.LblCim10Code.Size = New System.Drawing.Size(19, 13)
        Me.LblCim10Code.TabIndex = 1
        Me.LblCim10Code.Text = "43"
        '
        'LblCim10Description
        '
        Me.LblCim10Description.AutoSize = True
        Me.LblCim10Description.Location = New System.Drawing.Point(93, 46)
        Me.LblCim10Description.Name = "LblCim10Description"
        Me.LblCim10Description.Size = New System.Drawing.Size(40, 13)
        Me.LblCim10Description.TabIndex = 2
        Me.LblCim10Description.Text = "Brûlure"
        '
        'PnlSelection
        '
        Me.PnlSelection.BackColor = System.Drawing.Color.Cornsilk
        Me.PnlSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlSelection.Controls.Add(Me.BtnSelection)
        Me.PnlSelection.Controls.Add(Me.LblCim10Code)
        Me.PnlSelection.Controls.Add(Me.LblCim10Description)
        Me.PnlSelection.Location = New System.Drawing.Point(685, 119)
        Me.PnlSelection.Name = "PnlSelection"
        Me.PnlSelection.Size = New System.Drawing.Size(273, 131)
        Me.PnlSelection.TabIndex = 11
        '
        'oa_ald_cim10_id
        '
        Me.oa_ald_cim10_id.DataPropertyName = "oa_ald_cim10_id"
        Me.oa_ald_cim10_id.HeaderText = "Identifiant"
        Me.oa_ald_cim10_id.Name = "oa_ald_cim10_id"
        Me.oa_ald_cim10_id.ReadOnly = True
        Me.oa_ald_cim10_id.Visible = False
        '
        'oa_ald_cim10_ald_code
        '
        Me.oa_ald_cim10_ald_code.DataPropertyName = "oa_ald_cim10_ald_code"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.oa_ald_cim10_ald_code.DefaultCellStyle = DataGridViewCellStyle1
        Me.oa_ald_cim10_ald_code.HeaderText = "Code ALD"
        Me.oa_ald_cim10_ald_code.Name = "oa_ald_cim10_ald_code"
        Me.oa_ald_cim10_ald_code.ReadOnly = True
        '
        'oa_ald_cim10_code
        '
        Me.oa_ald_cim10_code.DataPropertyName = "oa_ald_cim10_code"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.oa_ald_cim10_code.DefaultCellStyle = DataGridViewCellStyle2
        Me.oa_ald_cim10_code.HeaderText = "Code CIM10"
        Me.oa_ald_cim10_code.Name = "oa_ald_cim10_code"
        Me.oa_ald_cim10_code.ReadOnly = True
        '
        'oa_ald_cim10_description
        '
        Me.oa_ald_cim10_description.DataPropertyName = "oa_ald_cim10_description"
        Me.oa_ald_cim10_description.HeaderText = "Description CIM10"
        Me.oa_ald_cim10_description.Name = "oa_ald_cim10_description"
        Me.oa_ald_cim10_description.ReadOnly = True
        Me.oa_ald_cim10_description.Width = 465
        '
        'FAldCim10Selecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(970, 365)
        Me.Controls.Add(Me.PnlSelection)
        Me.Controls.Add(Me.AldDataGridView)
        Me.Name = "FAldCim10Selecteur"
        Me.Text = "Sélection d'un code ALD"
        CType(Me.AldDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSelection.ResumeLayout(False)
        Me.PnlSelection.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AldDataGridView As DataGridView
    Friend WithEvents LblCim10Description As Label
    Friend WithEvents LblCim10Code As Label
    Friend WithEvents BtnSelection As Button
    Friend WithEvents PnlSelection As Panel
    Friend WithEvents oa_ald_cim10_id As DataGridViewTextBoxColumn
    Friend WithEvents oa_ald_cim10_ald_code As DataGridViewTextBoxColumn
    Friend WithEvents oa_ald_cim10_code As DataGridViewTextBoxColumn
    Friend WithEvents oa_ald_cim10_description As DataGridViewTextBoxColumn
End Class
