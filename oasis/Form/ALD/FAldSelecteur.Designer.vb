﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FAldSelecteur
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
        Me.LblAldCode = New System.Windows.Forms.Label()
        Me.LblAldDescription = New System.Windows.Forms.Label()
        Me.PnlSelection = New System.Windows.Forms.Panel()
        Me.oa_ald_id = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_ald_code = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.oa_ald_description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.AldDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSelection.SuspendLayout()
        Me.SuspendLayout()
        '
        'AldDataGridView
        '
        Me.AldDataGridView.AllowUserToAddRows = False
        Me.AldDataGridView.AllowUserToDeleteRows = False
        Me.AldDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AldDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.oa_ald_id, Me.oa_ald_code, Me.oa_ald_description})
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
        'LblAldCode
        '
        Me.LblAldCode.AutoSize = True
        Me.LblAldCode.Location = New System.Drawing.Point(20, 46)
        Me.LblAldCode.Name = "LblAldCode"
        Me.LblAldCode.Size = New System.Drawing.Size(19, 13)
        Me.LblAldCode.TabIndex = 1
        Me.LblAldCode.Text = "43"
        '
        'LblAldDescription
        '
        Me.LblAldDescription.AutoSize = True
        Me.LblAldDescription.Location = New System.Drawing.Point(93, 46)
        Me.LblAldDescription.Name = "LblAldDescription"
        Me.LblAldDescription.Size = New System.Drawing.Size(40, 13)
        Me.LblAldDescription.TabIndex = 2
        Me.LblAldDescription.Text = "Brûlure"
        '
        'PnlSelection
        '
        Me.PnlSelection.BackColor = System.Drawing.Color.Cornsilk
        Me.PnlSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlSelection.Controls.Add(Me.BtnSelection)
        Me.PnlSelection.Controls.Add(Me.LblAldCode)
        Me.PnlSelection.Controls.Add(Me.LblAldDescription)
        Me.PnlSelection.Location = New System.Drawing.Point(685, 119)
        Me.PnlSelection.Name = "PnlSelection"
        Me.PnlSelection.Size = New System.Drawing.Size(273, 131)
        Me.PnlSelection.TabIndex = 11
        '
        'oa_ald_id
        '
        Me.oa_ald_id.DataPropertyName = "oa_ald_id"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.oa_ald_id.DefaultCellStyle = DataGridViewCellStyle1
        Me.oa_ald_id.HeaderText = "Identifiant Ald"
        Me.oa_ald_id.Name = "oa_ald_id"
        Me.oa_ald_id.ReadOnly = True
        '
        'oa_ald_code
        '
        Me.oa_ald_code.DataPropertyName = "oa_ald_code"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.oa_ald_code.DefaultCellStyle = DataGridViewCellStyle2
        Me.oa_ald_code.HeaderText = "Code ALD"
        Me.oa_ald_code.Name = "oa_ald_code"
        Me.oa_ald_code.ReadOnly = True
        '
        'oa_ald_description
        '
        Me.oa_ald_description.DataPropertyName = "oa_ald_description"
        Me.oa_ald_description.HeaderText = "Description ALD"
        Me.oa_ald_description.Name = "oa_ald_description"
        Me.oa_ald_description.ReadOnly = True
        Me.oa_ald_description.Width = 465
        '
        'FAldSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(970, 365)
        Me.Controls.Add(Me.PnlSelection)
        Me.Controls.Add(Me.AldDataGridView)
        Me.Name = "FAldSelecteur"
        Me.Text = "Sélection d'un code ALD"
        CType(Me.AldDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSelection.ResumeLayout(False)
        Me.PnlSelection.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AldDataGridView As DataGridView
    Friend WithEvents LblAldDescription As Label
    Friend WithEvents LblAldCode As Label
    Friend WithEvents BtnSelection As Button
    Friend WithEvents PnlSelection As Panel
    Friend WithEvents oa_ald_id As DataGridViewTextBoxColumn
    Friend WithEvents oa_ald_code As DataGridViewTextBoxColumn
    Friend WithEvents oa_ald_description As DataGridViewTextBoxColumn
End Class
