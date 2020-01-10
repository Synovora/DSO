<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FAntecedentOccultesListe
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
        Me.AntecedentDataGridView = New System.Windows.Forms.DataGridView()
        Me.BtnConfirmer = New System.Windows.Forms.Button()
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.antecedent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.patientId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ordreAffichage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.AntecedentDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AntecedentDataGridView
        '
        Me.AntecedentDataGridView.AllowUserToAddRows = False
        Me.AntecedentDataGridView.AllowUserToDeleteRows = False
        Me.AntecedentDataGridView.AllowUserToOrderColumns = True
        Me.AntecedentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AntecedentDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.antecedent, Me.patientId, Me.ordreAffichage, Me.antecedentId})
        Me.AntecedentDataGridView.Location = New System.Drawing.Point(12, 89)
        Me.AntecedentDataGridView.Name = "AntecedentDataGridView"
        Me.AntecedentDataGridView.ReadOnly = True
        Me.AntecedentDataGridView.RowHeadersVisible = False
        Me.AntecedentDataGridView.Size = New System.Drawing.Size(723, 348)
        Me.AntecedentDataGridView.TabIndex = 16
        '
        'BtnConfirmer
        '
        Me.BtnConfirmer.Location = New System.Drawing.Point(579, 463)
        Me.BtnConfirmer.Name = "BtnConfirmer"
        Me.BtnConfirmer.Size = New System.Drawing.Size(75, 23)
        Me.BtnConfirmer.TabIndex = 19
        Me.BtnConfirmer.Text = "Confirmer"
        Me.BtnConfirmer.UseVisualStyleBackColor = True
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(660, 463)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 20
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'antecedent
        '
        Me.antecedent.HeaderText = "Antécédent"
        Me.antecedent.Name = "antecedent"
        Me.antecedent.ReadOnly = True
        Me.antecedent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.antecedent.Width = 600
        '
        'patientId
        '
        Me.patientId.HeaderText = "Identifiant Patient"
        Me.patientId.Name = "patientId"
        Me.patientId.ReadOnly = True
        '
        'ordreAffichage
        '
        Me.ordreAffichage.HeaderText = "Ordre affichage"
        Me.ordreAffichage.Name = "ordreAffichage"
        Me.ordreAffichage.ReadOnly = True
        Me.ordreAffichage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ordreAffichage.Visible = False
        '
        'antecedentId
        '
        Me.antecedentId.HeaderText = "Identifiant antécédent"
        Me.antecedentId.Name = "antecedentId"
        Me.antecedentId.ReadOnly = True
        Me.antecedentId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.antecedentId.Visible = False
        '
        'FAntecedentOccultesListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(743, 498)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Controls.Add(Me.BtnConfirmer)
        Me.Controls.Add(Me.AntecedentDataGridView)
        Me.Name = "FAntecedentOccultesListe"
        Me.Text = "Liste des antécédents occultés"
        CType(Me.AntecedentDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AntecedentDataGridView As DataGridView
    Friend WithEvents BtnConfirmer As Button
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents antecedent As DataGridViewTextBoxColumn
    Friend WithEvents patientId As DataGridViewTextBoxColumn
    Friend WithEvents ordreAffichage As DataGridViewTextBoxColumn
    Friend WithEvents antecedentId As DataGridViewTextBoxColumn
End Class
