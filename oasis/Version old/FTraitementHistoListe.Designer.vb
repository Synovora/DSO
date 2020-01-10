<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FTraitementHistoListe
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
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TraitementDataGridView = New System.Windows.Forms.DataGridView()
        Me.histoDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.histoUtilisateur = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.histoNature = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ordreAffichage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dateDebut = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dateFin = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.posologieDuree = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.posologie = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.posologieCommentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.commentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fenetre = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fenetreDateDebut = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fenetreDateFin = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fenetreCommentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.arret = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.arretCommentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.allergie = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.contreIndication = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.annulation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.annulationCommentaire = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblMedicamentDenomination = New System.Windows.Forms.Label()
        CType(Me.TraitementDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TraitementDataGridView
        '
        Me.TraitementDataGridView.AllowUserToAddRows = False
        Me.TraitementDataGridView.AllowUserToDeleteRows = False
        Me.TraitementDataGridView.AllowUserToOrderColumns = True
        Me.TraitementDataGridView.AllowUserToResizeRows = False
        Me.TraitementDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TraitementDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.histoDate, Me.histoUtilisateur, Me.histoNature, Me.ordreAffichage, Me.dateDebut, Me.dateFin, Me.posologieDuree, Me.posologie, Me.posologieCommentaire, Me.commentaire, Me.fenetre, Me.fenetreDateDebut, Me.fenetreDateFin, Me.fenetreCommentaire, Me.arret, Me.arretCommentaire, Me.allergie, Me.contreIndication, Me.annulation, Me.annulationCommentaire})
        Me.TraitementDataGridView.Location = New System.Drawing.Point(12, 120)
        Me.TraitementDataGridView.Name = "TraitementDataGridView"
        Me.TraitementDataGridView.ReadOnly = True
        Me.TraitementDataGridView.RowHeadersVisible = False
        Me.TraitementDataGridView.Size = New System.Drawing.Size(1646, 205)
        Me.TraitementDataGridView.TabIndex = 0
        '
        'histoDate
        '
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.histoDate.DefaultCellStyle = DataGridViewCellStyle14
        Me.histoDate.HeaderText = "Date"
        Me.histoDate.Name = "histoDate"
        Me.histoDate.ReadOnly = True
        Me.histoDate.Width = 120
        '
        'histoUtilisateur
        '
        Me.histoUtilisateur.HeaderText = "Utilisateur"
        Me.histoUtilisateur.Name = "histoUtilisateur"
        Me.histoUtilisateur.ReadOnly = True
        '
        'histoNature
        '
        Me.histoNature.HeaderText = "Action"
        Me.histoNature.Name = "histoNature"
        Me.histoNature.ReadOnly = True
        Me.histoNature.Width = 130
        '
        'ordreAffichage
        '
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.ordreAffichage.DefaultCellStyle = DataGridViewCellStyle15
        Me.ordreAffichage.HeaderText = "Ordre affichage"
        Me.ordreAffichage.Name = "ordreAffichage"
        Me.ordreAffichage.ReadOnly = True
        Me.ordreAffichage.Width = 60
        '
        'dateDebut
        '
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.dateDebut.DefaultCellStyle = DataGridViewCellStyle16
        Me.dateDebut.HeaderText = "Début"
        Me.dateDebut.Name = "dateDebut"
        Me.dateDebut.ReadOnly = True
        Me.dateDebut.Width = 80
        '
        'dateFin
        '
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.dateFin.DefaultCellStyle = DataGridViewCellStyle17
        Me.dateFin.HeaderText = "Date fin"
        Me.dateFin.Name = "dateFin"
        Me.dateFin.ReadOnly = True
        Me.dateFin.Width = 80
        '
        'posologieDuree
        '
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.posologieDuree.DefaultCellStyle = DataGridViewCellStyle18
        Me.posologieDuree.HeaderText = "Durée posologie"
        Me.posologieDuree.Name = "posologieDuree"
        Me.posologieDuree.ReadOnly = True
        Me.posologieDuree.Width = 70
        '
        'posologie
        '
        DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.posologie.DefaultCellStyle = DataGridViewCellStyle19
        Me.posologie.HeaderText = "Posologie"
        Me.posologie.Name = "posologie"
        Me.posologie.ReadOnly = True
        Me.posologie.Width = 110
        '
        'posologieCommentaire
        '
        Me.posologieCommentaire.HeaderText = "Commentaire posologie"
        Me.posologieCommentaire.Name = "posologieCommentaire"
        Me.posologieCommentaire.ReadOnly = True
        '
        'commentaire
        '
        Me.commentaire.HeaderText = "Commentaire traitement"
        Me.commentaire.Name = "commentaire"
        Me.commentaire.ReadOnly = True
        '
        'fenetre
        '
        DataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.fenetre.DefaultCellStyle = DataGridViewCellStyle20
        Me.fenetre.HeaderText = "Fenetre Th."
        Me.fenetre.Name = "fenetre"
        Me.fenetre.ReadOnly = True
        Me.fenetre.Width = 50
        '
        'fenetreDateDebut
        '
        DataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.fenetreDateDebut.DefaultCellStyle = DataGridViewCellStyle21
        Me.fenetreDateDebut.HeaderText = "Date début fenêtre"
        Me.fenetreDateDebut.Name = "fenetreDateDebut"
        Me.fenetreDateDebut.ReadOnly = True
        Me.fenetreDateDebut.Width = 80
        '
        'fenetreDateFin
        '
        DataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.fenetreDateFin.DefaultCellStyle = DataGridViewCellStyle22
        Me.fenetreDateFin.HeaderText = "Date fin fenêtre"
        Me.fenetreDateFin.Name = "fenetreDateFin"
        Me.fenetreDateFin.ReadOnly = True
        Me.fenetreDateFin.Width = 80
        '
        'fenetreCommentaire
        '
        Me.fenetreCommentaire.HeaderText = "Commentaire Fenêtre"
        Me.fenetreCommentaire.Name = "fenetreCommentaire"
        Me.fenetreCommentaire.ReadOnly = True
        '
        'arret
        '
        DataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.arret.DefaultCellStyle = DataGridViewCellStyle23
        Me.arret.HeaderText = "Arrêt traitement"
        Me.arret.Name = "arret"
        Me.arret.ReadOnly = True
        Me.arret.Width = 50
        '
        'arretCommentaire
        '
        Me.arretCommentaire.HeaderText = "Commentaire arrêt"
        Me.arretCommentaire.Name = "arretCommentaire"
        Me.arretCommentaire.ReadOnly = True
        '
        'allergie
        '
        DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.allergie.DefaultCellStyle = DataGridViewCellStyle24
        Me.allergie.HeaderText = "Allergie"
        Me.allergie.Name = "allergie"
        Me.allergie.ReadOnly = True
        Me.allergie.Width = 50
        '
        'contreIndication
        '
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.contreIndication.DefaultCellStyle = DataGridViewCellStyle25
        Me.contreIndication.HeaderText = "Contre-indication"
        Me.contreIndication.Name = "contreIndication"
        Me.contreIndication.ReadOnly = True
        Me.contreIndication.Width = 50
        '
        'annulation
        '
        DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.annulation.DefaultCellStyle = DataGridViewCellStyle26
        Me.annulation.HeaderText = "Annulation traitement"
        Me.annulation.Name = "annulation"
        Me.annulation.ReadOnly = True
        Me.annulation.Width = 50
        '
        'annulationCommentaire
        '
        Me.annulationCommentaire.HeaderText = "Commentaire annulation"
        Me.annulationCommentaire.Name = "annulationCommentaire"
        Me.annulationCommentaire.ReadOnly = True
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
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 101)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(166, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Dénomination commerciale :"
        '
        'LblMedicamentDenomination
        '
        Me.LblMedicamentDenomination.AutoSize = True
        Me.LblMedicamentDenomination.Location = New System.Drawing.Point(175, 101)
        Me.LblMedicamentDenomination.Name = "LblMedicamentDenomination"
        Me.LblMedicamentDenomination.Size = New System.Drawing.Size(147, 13)
        Me.LblMedicamentDenomination.TabIndex = 17
        Me.LblMedicamentDenomination.Text = "Dénomination du médicament"
        '
        'FTraitementHistoListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1670, 336)
        Me.Controls.Add(Me.LblMedicamentDenomination)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TraitementDataGridView)
        Me.Name = "FTraitementHistoListe"
        Me.Text = "Historique du cycle de vie d'un traitement"
        CType(Me.TraitementDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TraitementDataGridView As DataGridView
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
    Friend WithEvents histoDate As DataGridViewTextBoxColumn
    Friend WithEvents histoUtilisateur As DataGridViewTextBoxColumn
    Friend WithEvents histoNature As DataGridViewTextBoxColumn
    Friend WithEvents ordreAffichage As DataGridViewTextBoxColumn
    Friend WithEvents dateDebut As DataGridViewTextBoxColumn
    Friend WithEvents dateFin As DataGridViewTextBoxColumn
    Friend WithEvents posologieDuree As DataGridViewTextBoxColumn
    Friend WithEvents posologie As DataGridViewTextBoxColumn
    Friend WithEvents posologieCommentaire As DataGridViewTextBoxColumn
    Friend WithEvents commentaire As DataGridViewTextBoxColumn
    Friend WithEvents fenetre As DataGridViewTextBoxColumn
    Friend WithEvents fenetreDateDebut As DataGridViewTextBoxColumn
    Friend WithEvents fenetreDateFin As DataGridViewTextBoxColumn
    Friend WithEvents fenetreCommentaire As DataGridViewTextBoxColumn
    Friend WithEvents arret As DataGridViewTextBoxColumn
    Friend WithEvents arretCommentaire As DataGridViewTextBoxColumn
    Friend WithEvents allergie As DataGridViewTextBoxColumn
    Friend WithEvents contreIndication As DataGridViewTextBoxColumn
    Friend WithEvents annulation As DataGridViewTextBoxColumn
    Friend WithEvents annulationCommentaire As DataGridViewTextBoxColumn
    Friend WithEvents Label1 As Label
    Friend WithEvents LblMedicamentDenomination As Label
End Class
