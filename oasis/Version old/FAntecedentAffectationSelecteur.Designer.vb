﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FAntecedentAffectationSelecteur
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
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
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
        Me.AntecedentDataGridView = New System.Windows.Forms.DataGridView()
        Me.TxtAntecedentAAffecter = New System.Windows.Forms.TextBox()
        Me.LblAntecedentLbl = New System.Windows.Forms.Label()
        Me.BtnConfirmer = New System.Windows.Forms.Button()
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.ChkAntecedentMajeur = New System.Windows.Forms.CheckBox()
        Me.antecedent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentIdNiveau1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.niveau = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ordreAffichage1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ordreAffichage2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LblAffectationOu = New System.Windows.Forms.Label()
        Me.ChkAntecedentaAffecter = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.AntecedentDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.LblPatientDateMaj)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.LblPatientUniteSanitaire)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.LblPatientSite)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.LblPatientTel2)
        Me.GroupBox1.Controls.Add(Me.LblPatientPrenom)
        Me.GroupBox1.Controls.Add(Me.LblPatientTel1)
        Me.GroupBox1.Controls.Add(Me.LblPatientNom)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.LblPatientAge)
        Me.GroupBox1.Controls.Add(Me.LblPatientVille)
        Me.GroupBox1.Controls.Add(Me.LblPatientGenre)
        Me.GroupBox1.Controls.Add(Me.LblPatientCodePostal)
        Me.GroupBox1.Controls.Add(Me.LblPatientNIR)
        Me.GroupBox1.Controls.Add(Me.LblPatientAdresse2)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.LblPatientAdresse1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(948, 71)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Etat civil"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(505, 16)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 21
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(796, 16)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(61, 13)
        Me.LblPatientDateMaj.TabIndex = 19
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(662, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientUniteSanitaire
        '
        Me.LblPatientUniteSanitaire.AutoSize = True
        Me.LblPatientUniteSanitaire.Location = New System.Drawing.Point(843, 49)
        Me.LblPatientUniteSanitaire.Name = "LblPatientUniteSanitaire"
        Me.LblPatientUniteSanitaire.Size = New System.Drawing.Size(43, 13)
        Me.LblPatientUniteSanitaire.TabIndex = 17
        Me.LblPatientUniteSanitaire.Text = "Auxerre"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(662, 49)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(175, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Centre médical de référence :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(705, 33)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(33, 13)
        Me.LblPatientSite.TabIndex = 15
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(662, 33)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Site :"
        '
        'LblPatientTel2
        '
        Me.LblPatientTel2.AutoSize = True
        Me.LblPatientTel2.Location = New System.Drawing.Point(398, 49)
        Me.LblPatientTel2.Name = "LblPatientTel2"
        Me.LblPatientTel2.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel2.TabIndex = 13
        Me.LblPatientTel2.Text = "0968542357"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(6, 16)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(60, 13)
        Me.LblPatientPrenom.TabIndex = 1
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientTel1
        '
        Me.LblPatientTel1.AutoSize = True
        Me.LblPatientTel1.Location = New System.Drawing.Point(398, 33)
        Me.LblPatientTel1.Name = "LblPatientTel1"
        Me.LblPatientTel1.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel1.TabIndex = 12
        Me.LblPatientTel1.Text = "0288425678"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(128, 16)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(42, 13)
        Me.LblPatientNom.TabIndex = 2
        Me.LblPatientNom.Text = "Durand"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(355, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Tel. :"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(245, 16)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(39, 13)
        Me.LblPatientAge.TabIndex = 3
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientVille
        '
        Me.LblPatientVille.AutoSize = True
        Me.LblPatientVille.Location = New System.Drawing.Point(120, 49)
        Me.LblPatientVille.Name = "LblPatientVille"
        Me.LblPatientVille.Size = New System.Drawing.Size(52, 13)
        Me.LblPatientVille.TabIndex = 10
        Me.LblPatientVille.Text = "Lournand"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(355, 16)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(49, 13)
        Me.LblPatientGenre.TabIndex = 4
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientCodePostal
        '
        Me.LblPatientCodePostal.AutoSize = True
        Me.LblPatientCodePostal.Location = New System.Drawing.Point(77, 49)
        Me.LblPatientCodePostal.Name = "LblPatientCodePostal"
        Me.LblPatientCodePostal.Size = New System.Drawing.Size(37, 13)
        Me.LblPatientCodePostal.TabIndex = 9
        Me.LblPatientCodePostal.Text = "71250"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(556, 16)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 5
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'LblPatientAdresse2
        '
        Me.LblPatientAdresse2.AutoSize = True
        Me.LblPatientAdresse2.Location = New System.Drawing.Point(231, 32)
        Me.LblPatientAdresse2.Name = "LblPatientAdresse2"
        Me.LblPatientAdresse2.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientAdresse2.TabIndex = 8
        Me.LblPatientAdresse2.Text = "adresse 2"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Adresse :"
        '
        'LblPatientAdresse1
        '
        Me.LblPatientAdresse1.AutoSize = True
        Me.LblPatientAdresse1.Location = New System.Drawing.Point(77, 32)
        Me.LblPatientAdresse1.Name = "LblPatientAdresse1"
        Me.LblPatientAdresse1.Size = New System.Drawing.Size(109, 13)
        Me.LblPatientAdresse1.TabIndex = 7
        Me.LblPatientAdresse1.Text = "3 rue de la république"
        '
        'AntecedentDataGridView
        '
        Me.AntecedentDataGridView.AllowUserToAddRows = False
        Me.AntecedentDataGridView.AllowUserToDeleteRows = False
        Me.AntecedentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AntecedentDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.antecedent, Me.antecedentIdNiveau1, Me.niveau, Me.ordreAffichage1, Me.ordreAffichage2, Me.antecedentId})
        Me.AntecedentDataGridView.Location = New System.Drawing.Point(12, 205)
        Me.AntecedentDataGridView.Name = "AntecedentDataGridView"
        Me.AntecedentDataGridView.ReadOnly = True
        Me.AntecedentDataGridView.RowHeadersVisible = False
        Me.AntecedentDataGridView.Size = New System.Drawing.Size(654, 275)
        Me.AntecedentDataGridView.TabIndex = 16
        '
        'TxtAntecedentAAffecter
        '
        Me.TxtAntecedentAAffecter.BackColor = System.Drawing.SystemColors.Info
        Me.TxtAntecedentAAffecter.Location = New System.Drawing.Point(167, 98)
        Me.TxtAntecedentAAffecter.Name = "TxtAntecedentAAffecter"
        Me.TxtAntecedentAAffecter.ReadOnly = True
        Me.TxtAntecedentAAffecter.Size = New System.Drawing.Size(477, 20)
        Me.TxtAntecedentAAffecter.TabIndex = 17
        '
        'LblAntecedentLbl
        '
        Me.LblAntecedentLbl.AutoSize = True
        Me.LblAntecedentLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAntecedentLbl.Location = New System.Drawing.Point(9, 105)
        Me.LblAntecedentLbl.Name = "LblAntecedentLbl"
        Me.LblAntecedentLbl.Size = New System.Drawing.Size(139, 13)
        Me.LblAntecedentLbl.TabIndex = 18
        Me.LblAntecedentLbl.Text = "Antécédent à affecter :"
        '
        'BtnConfirmer
        '
        Me.BtnConfirmer.Location = New System.Drawing.Point(766, 278)
        Me.BtnConfirmer.Name = "BtnConfirmer"
        Me.BtnConfirmer.Size = New System.Drawing.Size(75, 23)
        Me.BtnConfirmer.TabIndex = 19
        Me.BtnConfirmer.Text = "Confirmer"
        Me.BtnConfirmer.UseVisualStyleBackColor = True
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(766, 326)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 20
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'ChkAntecedentMajeur
        '
        Me.ChkAntecedentMajeur.AutoSize = True
        Me.ChkAntecedentMajeur.Location = New System.Drawing.Point(12, 136)
        Me.ChkAntecedentMajeur.Name = "ChkAntecedentMajeur"
        Me.ChkAntecedentMajeur.Size = New System.Drawing.Size(240, 17)
        Me.ChkAntecedentMajeur.TabIndex = 22
        Me.ChkAntecedentMajeur.Text = "Transformer l'antécédent ci-dessus en Majeur"
        Me.ChkAntecedentMajeur.UseVisualStyleBackColor = True
        '
        'antecedent
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.antecedent.DefaultCellStyle = DataGridViewCellStyle10
        Me.antecedent.HeaderText = "Antécédent"
        Me.antecedent.Name = "antecedent"
        Me.antecedent.ReadOnly = True
        Me.antecedent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.antecedent.Width = 600
        '
        'antecedentIdNiveau1
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.antecedentIdNiveau1.DefaultCellStyle = DataGridViewCellStyle11
        Me.antecedentIdNiveau1.HeaderText = "Antcédent niveau 1"
        Me.antecedentIdNiveau1.Name = "antecedentIdNiveau1"
        Me.antecedentIdNiveau1.ReadOnly = True
        Me.antecedentIdNiveau1.Visible = False
        '
        'niveau
        '
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.niveau.DefaultCellStyle = DataGridViewCellStyle12
        Me.niveau.HeaderText = "Niveau"
        Me.niveau.Name = "niveau"
        Me.niveau.ReadOnly = True
        Me.niveau.Width = 50
        '
        'ordreAffichage1
        '
        Me.ordreAffichage1.HeaderText = "Ordre affichage 1"
        Me.ordreAffichage1.Name = "ordreAffichage1"
        Me.ordreAffichage1.ReadOnly = True
        Me.ordreAffichage1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ordreAffichage1.Visible = False
        '
        'ordreAffichage2
        '
        Me.ordreAffichage2.HeaderText = "Ordre affichage 2"
        Me.ordreAffichage2.Name = "ordreAffichage2"
        Me.ordreAffichage2.ReadOnly = True
        Me.ordreAffichage2.Visible = False
        '
        'antecedentId
        '
        Me.antecedentId.HeaderText = "Identifiant antécédent"
        Me.antecedentId.Name = "antecedentId"
        Me.antecedentId.ReadOnly = True
        Me.antecedentId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.antecedentId.Visible = False
        '
        'LblAffectationOu
        '
        Me.LblAffectationOu.AutoSize = True
        Me.LblAffectationOu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAffectationOu.Location = New System.Drawing.Point(9, 156)
        Me.LblAffectationOu.Name = "LblAffectationOu"
        Me.LblAffectationOu.Size = New System.Drawing.Size(23, 13)
        Me.LblAffectationOu.TabIndex = 23
        Me.LblAffectationOu.Text = "Ou"
        '
        'ChkAntecedentaAffecter
        '
        Me.ChkAntecedentaAffecter.AutoSize = True
        Me.ChkAntecedentaAffecter.Location = New System.Drawing.Point(12, 172)
        Me.ChkAntecedentaAffecter.Name = "ChkAntecedentaAffecter"
        Me.ChkAntecedentaAffecter.Size = New System.Drawing.Size(473, 17)
        Me.ChkAntecedentaAffecter.TabIndex = 24
        Me.ChkAntecedentaAffecter.Text = "Sélectionner un antécédent dans la liste ci-dessous, auquel l'antécédent ci-dessu" &
    "s sera affecté"
        Me.ChkAntecedentaAffecter.UseVisualStyleBackColor = True
        '
        'FAntecedentAffectationSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(967, 489)
        Me.Controls.Add(Me.ChkAntecedentaAffecter)
        Me.Controls.Add(Me.LblAffectationOu)
        Me.Controls.Add(Me.ChkAntecedentMajeur)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Controls.Add(Me.BtnConfirmer)
        Me.Controls.Add(Me.LblAntecedentLbl)
        Me.Controls.Add(Me.TxtAntecedentAAffecter)
        Me.Controls.Add(Me.AntecedentDataGridView)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FAntecedentAffectationSelecteur"
        Me.Text = "FAntecedentOrdreSelecteur"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.AntecedentDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
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
    Friend WithEvents AntecedentDataGridView As DataGridView
    Friend WithEvents TxtAntecedentAAffecter As TextBox
    Friend WithEvents LblAntecedentLbl As Label
    Friend WithEvents BtnConfirmer As Button
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents ChkAntecedentMajeur As CheckBox
    Friend WithEvents antecedent As DataGridViewTextBoxColumn
    Friend WithEvents antecedentIdNiveau1 As DataGridViewTextBoxColumn
    Friend WithEvents niveau As DataGridViewTextBoxColumn
    Friend WithEvents ordreAffichage1 As DataGridViewTextBoxColumn
    Friend WithEvents ordreAffichage2 As DataGridViewTextBoxColumn
    Friend WithEvents antecedentId As DataGridViewTextBoxColumn
    Friend WithEvents LblAffectationOu As Label
    Friend WithEvents ChkAntecedentaAffecter As CheckBox
End Class
