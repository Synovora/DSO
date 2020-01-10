<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FAntecedentHistoListe
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
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
        Me.histoDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.histoUtilisateur = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.histoAction = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.categorieContexte = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.drc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.diagnostic = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dateDebut = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dateFin = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.publication = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.arret = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.arretCommentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.inactif = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.niveau = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentId1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentId2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ordre1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ordre2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ordre3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.antecedentId = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.GroupBox1.TabIndex = 16
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
        Me.AntecedentDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.histoDate, Me.histoUtilisateur, Me.histoAction, Me.type, Me.categorieContexte, Me.drc, Me.description, Me.diagnostic, Me.dateDebut, Me.dateFin, Me.publication, Me.arret, Me.arretCommentaire, Me.inactif, Me.niveau, Me.antecedentId1, Me.antecedentId2, Me.ordre1, Me.ordre2, Me.ordre3, Me.antecedentId})
        Me.AntecedentDataGridView.Location = New System.Drawing.Point(12, 114)
        Me.AntecedentDataGridView.Name = "AntecedentDataGridView"
        Me.AntecedentDataGridView.ReadOnly = True
        Me.AntecedentDataGridView.RowHeadersVisible = False
        Me.AntecedentDataGridView.Size = New System.Drawing.Size(1607, 219)
        Me.AntecedentDataGridView.TabIndex = 17
        '
        'histoDate
        '
        Me.histoDate.HeaderText = "Date"
        Me.histoDate.Name = "histoDate"
        Me.histoDate.ReadOnly = True
        '
        'histoUtilisateur
        '
        Me.histoUtilisateur.HeaderText = "Utilisateur"
        Me.histoUtilisateur.Name = "histoUtilisateur"
        Me.histoUtilisateur.ReadOnly = True
        '
        'histoAction
        '
        Me.histoAction.HeaderText = "Action"
        Me.histoAction.Name = "histoAction"
        Me.histoAction.ReadOnly = True
        Me.histoAction.Width = 130
        '
        'type
        '
        Me.type.HeaderText = "Type"
        Me.type.Name = "type"
        Me.type.ReadOnly = True
        Me.type.Width = 80
        '
        'categorieContexte
        '
        Me.categorieContexte.HeaderText = "Catégorie"
        Me.categorieContexte.Name = "categorieContexte"
        Me.categorieContexte.ReadOnly = True
        '
        'drc
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.drc.DefaultCellStyle = DataGridViewCellStyle1
        Me.drc.HeaderText = "Id. DRC"
        Me.drc.Name = "drc"
        Me.drc.ReadOnly = True
        Me.drc.Width = 50
        '
        'description
        '
        Me.description.HeaderText = "Description"
        Me.description.Name = "description"
        Me.description.ReadOnly = True
        Me.description.Width = 200
        '
        'diagnostic
        '
        Me.diagnostic.HeaderText = "Diagnostic"
        Me.diagnostic.Name = "diagnostic"
        Me.diagnostic.ReadOnly = True
        '
        'dateDebut
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.dateDebut.DefaultCellStyle = DataGridViewCellStyle2
        Me.dateDebut.HeaderText = "Date début"
        Me.dateDebut.Name = "dateDebut"
        Me.dateDebut.ReadOnly = True
        '
        'dateFin
        '
        Me.dateFin.HeaderText = "Date fin validité"
        Me.dateFin.Name = "dateFin"
        Me.dateFin.ReadOnly = True
        '
        'publication
        '
        Me.publication.HeaderText = "Publication"
        Me.publication.Name = "publication"
        Me.publication.ReadOnly = True
        '
        'arret
        '
        Me.arret.HeaderText = "Arrêt"
        Me.arret.Name = "arret"
        Me.arret.ReadOnly = True
        Me.arret.Width = 60
        '
        'arretCommentaire
        '
        Me.arretCommentaire.HeaderText = "Commentaire arrêt"
        Me.arretCommentaire.Name = "arretCommentaire"
        Me.arretCommentaire.ReadOnly = True
        Me.arretCommentaire.Width = 120
        '
        'inactif
        '
        Me.inactif.HeaderText = "Activité"
        Me.inactif.Name = "inactif"
        Me.inactif.ReadOnly = True
        Me.inactif.Visible = False
        '
        'niveau
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.niveau.DefaultCellStyle = DataGridViewCellStyle3
        Me.niveau.HeaderText = "Niveau"
        Me.niveau.Name = "niveau"
        Me.niveau.ReadOnly = True
        Me.niveau.Width = 50
        '
        'antecedentId1
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.antecedentId1.DefaultCellStyle = DataGridViewCellStyle4
        Me.antecedentId1.HeaderText = "Antécédent niveau1"
        Me.antecedentId1.Name = "antecedentId1"
        Me.antecedentId1.ReadOnly = True
        Me.antecedentId1.Width = 80
        '
        'antecedentId2
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.antecedentId2.DefaultCellStyle = DataGridViewCellStyle5
        Me.antecedentId2.HeaderText = "Antécédent niveau2"
        Me.antecedentId2.Name = "antecedentId2"
        Me.antecedentId2.ReadOnly = True
        Me.antecedentId2.Width = 80
        '
        'ordre1
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.ordre1.DefaultCellStyle = DataGridViewCellStyle6
        Me.ordre1.HeaderText = "Ordre niveau 1"
        Me.ordre1.Name = "ordre1"
        Me.ordre1.ReadOnly = True
        Me.ordre1.Width = 50
        '
        'ordre2
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.ordre2.DefaultCellStyle = DataGridViewCellStyle7
        Me.ordre2.HeaderText = "Ordre niveau 2"
        Me.ordre2.Name = "ordre2"
        Me.ordre2.ReadOnly = True
        Me.ordre2.Width = 50
        '
        'ordre3
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.ordre3.DefaultCellStyle = DataGridViewCellStyle8
        Me.ordre3.HeaderText = "Ordre niveau 3"
        Me.ordre3.Name = "ordre3"
        Me.ordre3.ReadOnly = True
        Me.ordre3.Width = 50
        '
        'antecedentId
        '
        Me.antecedentId.HeaderText = "Id antécédent"
        Me.antecedentId.Name = "antecedentId"
        Me.antecedentId.ReadOnly = True
        Me.antecedentId.Visible = False
        '
        'FAntecedentHistoListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1631, 340)
        Me.Controls.Add(Me.AntecedentDataGridView)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FAntecedentHistoListe"
        Me.Text = "Historique du cycle de vie d'un antécédent"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.AntecedentDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
    Friend WithEvents histoDate As DataGridViewTextBoxColumn
    Friend WithEvents histoUtilisateur As DataGridViewTextBoxColumn
    Friend WithEvents histoAction As DataGridViewTextBoxColumn
    Friend WithEvents type As DataGridViewTextBoxColumn
    Friend WithEvents categorieContexte As DataGridViewTextBoxColumn
    Friend WithEvents drc As DataGridViewTextBoxColumn
    Friend WithEvents description As DataGridViewTextBoxColumn
    Friend WithEvents diagnostic As DataGridViewTextBoxColumn
    Friend WithEvents dateDebut As DataGridViewTextBoxColumn
    Friend WithEvents dateFin As DataGridViewTextBoxColumn
    Friend WithEvents publication As DataGridViewTextBoxColumn
    Friend WithEvents arret As DataGridViewTextBoxColumn
    Friend WithEvents arretCommentaire As DataGridViewTextBoxColumn
    Friend WithEvents inactif As DataGridViewTextBoxColumn
    Friend WithEvents niveau As DataGridViewTextBoxColumn
    Friend WithEvents antecedentId1 As DataGridViewTextBoxColumn
    Friend WithEvents antecedentId2 As DataGridViewTextBoxColumn
    Friend WithEvents ordre1 As DataGridViewTextBoxColumn
    Friend WithEvents ordre2 As DataGridViewTextBoxColumn
    Friend WithEvents ordre3 As DataGridViewTextBoxColumn
    Friend WithEvents antecedentId As DataGridViewTextBoxColumn
End Class
