<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FPatientDetailEdit
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtPrenom = New System.Windows.Forms.TextBox()
        Me.TxtNom = New System.Windows.Forms.TextBox()
        Me.TxtAdresse1 = New System.Windows.Forms.TextBox()
        Me.TxtAdresse2 = New System.Windows.Forms.TextBox()
        Me.TxtCodePostal = New System.Windows.Forms.TextBox()
        Me.DteDateNaissance = New System.Windows.Forms.DateTimePicker()
        Me.LblPrenom = New System.Windows.Forms.Label()
        Me.LblNom = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblDateNaissance = New System.Windows.Forms.Label()
        Me.CbxGenre = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtVille = New System.Windows.Forms.TextBox()
        Me.TxtTelFixe = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtTelMobile = New System.Windows.Forms.TextBox()
        Me.BtnGoogleMaps = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.BtnModifier = New System.Windows.Forms.Button()
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.BtnValider = New System.Windows.Forms.Button()
        Me.NotePatientDataGridView = New System.Windows.Forms.DataGridView()
        Me.auteur = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.note = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.noteId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NoteContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUneNoteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TxtNIR = New System.Windows.Forms.TextBox()
        Me.TxtEmail = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.DteDateEntree = New System.Windows.Forms.DateTimePicker()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.DteDateSortie = New System.Windows.Forms.DateTimePicker()
        Me.DteDateDeces = New System.Windows.Forms.DateTimePicker()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.CbxSite = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.CbxUniteSanitaire = New System.Windows.Forms.ComboBox()
        Me.LblLabelIdOasis = New System.Windows.Forms.Label()
        Me.LblIdentifiantOasis = New System.Windows.Forms.Label()
        Me.LblAge = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.ChkCouvertureInternet = New System.Windows.Forms.CheckBox()
        Me.TxtCommentaireSortie = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.GbxSortieOasis = New System.Windows.Forms.GroupBox()
        Me.BtnValidationSortie = New System.Windows.Forms.Button()
        Me.BtnSortieOasis = New System.Windows.Forms.Button()
        Me.TxtNomMarital = New System.Windows.Forms.TextBox()
        Me.LblNomMarital = New System.Windows.Forms.Label()
        Me.LblModulo = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.BtnValidationDateNaissance = New System.Windows.Forms.Button()
        Me.BtnCreerNote = New System.Windows.Forms.Button()
        CType(Me.NotePatientDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.NoteContextMenuStrip.SuspendLayout()
        Me.GbxSortieOasis.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtPrenom
        '
        Me.TxtPrenom.Location = New System.Drawing.Point(145, 81)
        Me.TxtPrenom.Name = "TxtPrenom"
        Me.TxtPrenom.Size = New System.Drawing.Size(136, 20)
        Me.TxtPrenom.TabIndex = 4
        '
        'TxtNom
        '
        Me.TxtNom.Location = New System.Drawing.Point(145, 107)
        Me.TxtNom.Name = "TxtNom"
        Me.TxtNom.Size = New System.Drawing.Size(200, 20)
        Me.TxtNom.TabIndex = 5
        '
        'TxtAdresse1
        '
        Me.TxtAdresse1.Location = New System.Drawing.Point(145, 212)
        Me.TxtAdresse1.Name = "TxtAdresse1"
        Me.TxtAdresse1.Size = New System.Drawing.Size(200, 20)
        Me.TxtAdresse1.TabIndex = 25
        '
        'TxtAdresse2
        '
        Me.TxtAdresse2.Location = New System.Drawing.Point(145, 238)
        Me.TxtAdresse2.Name = "TxtAdresse2"
        Me.TxtAdresse2.Size = New System.Drawing.Size(200, 20)
        Me.TxtAdresse2.TabIndex = 26
        '
        'TxtCodePostal
        '
        Me.TxtCodePostal.Location = New System.Drawing.Point(145, 264)
        Me.TxtCodePostal.Name = "TxtCodePostal"
        Me.TxtCodePostal.Size = New System.Drawing.Size(61, 20)
        Me.TxtCodePostal.TabIndex = 30
        '
        'DteDateNaissance
        '
        Me.DteDateNaissance.Location = New System.Drawing.Point(145, 55)
        Me.DteDateNaissance.Name = "DteDateNaissance"
        Me.DteDateNaissance.Size = New System.Drawing.Size(200, 20)
        Me.DteDateNaissance.TabIndex = 1
        '
        'LblPrenom
        '
        Me.LblPrenom.AutoSize = True
        Me.LblPrenom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPrenom.Location = New System.Drawing.Point(24, 84)
        Me.LblPrenom.Name = "LblPrenom"
        Me.LblPrenom.Size = New System.Drawing.Size(49, 13)
        Me.LblPrenom.TabIndex = 6
        Me.LblPrenom.Text = "Prénom"
        '
        'LblNom
        '
        Me.LblNom.AutoSize = True
        Me.LblNom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNom.Location = New System.Drawing.Point(24, 110)
        Me.LblNom.Name = "LblNom"
        Me.LblNom.Size = New System.Drawing.Size(32, 13)
        Me.LblNom.TabIndex = 7
        Me.LblNom.Text = "Nom"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(24, 136)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Genre"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(24, 240)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Adresse 2"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(24, 266)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Code Postal"
        '
        'LblDateNaissance
        '
        Me.LblDateNaissance.AutoSize = True
        Me.LblDateNaissance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDateNaissance.Location = New System.Drawing.Point(24, 57)
        Me.LblDateNaissance.Name = "LblDateNaissance"
        Me.LblDateNaissance.Size = New System.Drawing.Size(113, 13)
        Me.LblDateNaissance.TabIndex = 11
        Me.LblDateNaissance.Text = "Date de naissance"
        '
        'CbxGenre
        '
        Me.CbxGenre.FormattingEnabled = True
        Me.CbxGenre.Location = New System.Drawing.Point(145, 133)
        Me.CbxGenre.Name = "CbxGenre"
        Me.CbxGenre.Size = New System.Drawing.Size(121, 21)
        Me.CbxGenre.TabIndex = 10
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(24, 214)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(63, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Adresse 1"
        '
        'TxtVille
        '
        Me.TxtVille.Location = New System.Drawing.Point(145, 290)
        Me.TxtVille.Name = "TxtVille"
        Me.TxtVille.Size = New System.Drawing.Size(200, 20)
        Me.TxtVille.TabIndex = 35
        '
        'TxtTelFixe
        '
        Me.TxtTelFixe.Location = New System.Drawing.Point(145, 316)
        Me.TxtTelFixe.Name = "TxtTelFixe"
        Me.TxtTelFixe.Size = New System.Drawing.Size(100, 20)
        Me.TxtTelFixe.TabIndex = 40
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 292)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Ville"
        '
        'TxtTelMobile
        '
        Me.TxtTelMobile.Location = New System.Drawing.Point(145, 342)
        Me.TxtTelMobile.Name = "TxtTelMobile"
        Me.TxtTelMobile.Size = New System.Drawing.Size(100, 20)
        Me.TxtTelMobile.TabIndex = 45
        '
        'BtnGoogleMaps
        '
        Me.BtnGoogleMaps.Location = New System.Drawing.Point(411, 236)
        Me.BtnGoogleMaps.Name = "BtnGoogleMaps"
        Me.BtnGoogleMaps.Size = New System.Drawing.Size(95, 23)
        Me.BtnGoogleMaps.TabIndex = 200
        Me.BtnGoogleMaps.Text = "Google Maps"
        Me.BtnGoogleMaps.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(24, 318)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Tel. fixe"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(24, 344)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Tel. mobile"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(24, 449)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(29, 13)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Site"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(24, 188)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(29, 13)
        Me.Label9.TabIndex = 25
        Me.Label9.Text = "NIR"
        '
        'BtnModifier
        '
        Me.BtnModifier.Location = New System.Drawing.Point(18, 704)
        Me.BtnModifier.Name = "BtnModifier"
        Me.BtnModifier.Size = New System.Drawing.Size(75, 23)
        Me.BtnModifier.TabIndex = 210
        Me.BtnModifier.Text = "Modifier"
        Me.BtnModifier.UseVisualStyleBackColor = True
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(423, 704)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 240
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'BtnValider
        '
        Me.BtnValider.Location = New System.Drawing.Point(342, 704)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(75, 23)
        Me.BtnValider.TabIndex = 230
        Me.BtnValider.Text = "Valider"
        Me.BtnValider.UseVisualStyleBackColor = True
        '
        'NotePatientDataGridView
        '
        Me.NotePatientDataGridView.AllowUserToAddRows = False
        Me.NotePatientDataGridView.AllowUserToDeleteRows = False
        Me.NotePatientDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.NotePatientDataGridView.ColumnHeadersVisible = False
        Me.NotePatientDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.auteur, Me.note, Me.noteId})
        Me.NotePatientDataGridView.ContextMenuStrip = Me.NoteContextMenuStrip
        Me.NotePatientDataGridView.Location = New System.Drawing.Point(592, 12)
        Me.NotePatientDataGridView.Name = "NotePatientDataGridView"
        Me.NotePatientDataGridView.ReadOnly = True
        Me.NotePatientDataGridView.RowHeadersVisible = False
        Me.NotePatientDataGridView.Size = New System.Drawing.Size(658, 668)
        Me.NotePatientDataGridView.TabIndex = 300
        '
        'auteur
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.auteur.DefaultCellStyle = DataGridViewCellStyle3
        Me.auteur.HeaderText = "Auteur"
        Me.auteur.Name = "auteur"
        Me.auteur.ReadOnly = True
        Me.auteur.Width = 120
        '
        'note
        '
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.note.DefaultCellStyle = DataGridViewCellStyle4
        Me.note.HeaderText = "Note"
        Me.note.Name = "note"
        Me.note.ReadOnly = True
        Me.note.Width = 548
        '
        'noteId
        '
        Me.noteId.HeaderText = "Note Id"
        Me.noteId.Name = "noteId"
        Me.noteId.ReadOnly = True
        Me.noteId.Visible = False
        '
        'NoteContextMenuStrip
        '
        Me.NoteContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerUneNoteToolStripMenuItem})
        Me.NoteContextMenuStrip.Name = "NoteContextMenuStrip"
        Me.NoteContextMenuStrip.Size = New System.Drawing.Size(153, 26)
        '
        'CréerUneNoteToolStripMenuItem
        '
        Me.CréerUneNoteToolStripMenuItem.Name = "CréerUneNoteToolStripMenuItem"
        Me.CréerUneNoteToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CréerUneNoteToolStripMenuItem.Text = "Créer une note"
        '
        'TxtNIR
        '
        Me.TxtNIR.Location = New System.Drawing.Point(145, 186)
        Me.TxtNIR.Name = "TxtNIR"
        Me.TxtNIR.Size = New System.Drawing.Size(100, 20)
        Me.TxtNIR.TabIndex = 20
        '
        'TxtEmail
        '
        Me.TxtEmail.Location = New System.Drawing.Point(145, 368)
        Me.TxtEmail.Name = "TxtEmail"
        Me.TxtEmail.Size = New System.Drawing.Size(200, 20)
        Me.TxtEmail.TabIndex = 50
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(24, 370)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(37, 13)
        Me.Label10.TabIndex = 33
        Me.Label10.Text = "eMail"
        '
        'DteDateEntree
        '
        Me.DteDateEntree.Location = New System.Drawing.Point(145, 394)
        Me.DteDateEntree.Name = "DteDateEntree"
        Me.DteDateEntree.Size = New System.Drawing.Size(200, 20)
        Me.DteDateEntree.TabIndex = 55
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(24, 396)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(74, 13)
        Me.Label11.TabIndex = 35
        Me.Label11.Text = "Date entrée"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(15, 19)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(69, 13)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "Date sortie"
        '
        'DteDateSortie
        '
        Me.DteDateSortie.Location = New System.Drawing.Point(125, 13)
        Me.DteDateSortie.Name = "DteDateSortie"
        Me.DteDateSortie.Size = New System.Drawing.Size(200, 20)
        Me.DteDateSortie.TabIndex = 90
        '
        'DteDateDeces
        '
        Me.DteDateDeces.Location = New System.Drawing.Point(145, 494)
        Me.DteDateDeces.Name = "DteDateDeces"
        Me.DteDateDeces.Size = New System.Drawing.Size(200, 20)
        Me.DteDateDeces.TabIndex = 80
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(24, 500)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(72, 13)
        Me.Label13.TabIndex = 39
        Me.Label13.Text = "Date décès"
        '
        'CbxSite
        '
        Me.CbxSite.FormattingEnabled = True
        Me.CbxSite.Location = New System.Drawing.Point(145, 447)
        Me.CbxSite.Name = "CbxSite"
        Me.CbxSite.Size = New System.Drawing.Size(121, 21)
        Me.CbxSite.TabIndex = 60
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(23, 421)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(89, 13)
        Me.Label14.TabIndex = 41
        Me.Label14.Text = "Unité sanitaire"
        '
        'CbxUniteSanitaire
        '
        Me.CbxUniteSanitaire.FormattingEnabled = True
        Me.CbxUniteSanitaire.Location = New System.Drawing.Point(145, 420)
        Me.CbxUniteSanitaire.Name = "CbxUniteSanitaire"
        Me.CbxUniteSanitaire.Size = New System.Drawing.Size(121, 21)
        Me.CbxUniteSanitaire.TabIndex = 65
        '
        'LblLabelIdOasis
        '
        Me.LblLabelIdOasis.AutoSize = True
        Me.LblLabelIdOasis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelIdOasis.Location = New System.Drawing.Point(24, 20)
        Me.LblLabelIdOasis.Name = "LblLabelIdOasis"
        Me.LblLabelIdOasis.Size = New System.Drawing.Size(99, 13)
        Me.LblLabelIdOasis.TabIndex = 43
        Me.LblLabelIdOasis.Text = "Identifiant Oasis"
        '
        'LblIdentifiantOasis
        '
        Me.LblIdentifiantOasis.AutoSize = True
        Me.LblIdentifiantOasis.Location = New System.Drawing.Point(142, 20)
        Me.LblIdentifiantOasis.Name = "LblIdentifiantOasis"
        Me.LblIdentifiantOasis.Size = New System.Drawing.Size(43, 13)
        Me.LblIdentifiantOasis.TabIndex = 44
        Me.LblIdentifiantOasis.Text = "123456"
        '
        'LblAge
        '
        Me.LblAge.AutoSize = True
        Me.LblAge.Location = New System.Drawing.Point(352, 61)
        Me.LblAge.Name = "LblAge"
        Me.LblAge.Size = New System.Drawing.Size(39, 13)
        Me.LblAge.TabIndex = 45
        Me.LblAge.Text = "99 ans"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(24, 474)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(117, 13)
        Me.Label16.TabIndex = 46
        Me.Label16.Text = "Couverture Internet"
        '
        'ChkCouvertureInternet
        '
        Me.ChkCouvertureInternet.AutoSize = True
        Me.ChkCouvertureInternet.Location = New System.Drawing.Point(145, 474)
        Me.ChkCouvertureInternet.Name = "ChkCouvertureInternet"
        Me.ChkCouvertureInternet.Size = New System.Drawing.Size(15, 14)
        Me.ChkCouvertureInternet.TabIndex = 70
        Me.ChkCouvertureInternet.UseVisualStyleBackColor = True
        '
        'TxtCommentaireSortie
        '
        Me.TxtCommentaireSortie.Location = New System.Drawing.Point(125, 39)
        Me.TxtCommentaireSortie.Multiline = True
        Me.TxtCommentaireSortie.Name = "TxtCommentaireSortie"
        Me.TxtCommentaireSortie.Size = New System.Drawing.Size(333, 57)
        Me.TxtCommentaireSortie.TabIndex = 100
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(15, 42)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(79, 13)
        Me.Label17.TabIndex = 49
        Me.Label17.Text = "Commentaire"
        '
        'GbxSortieOasis
        '
        Me.GbxSortieOasis.BackColor = System.Drawing.Color.White
        Me.GbxSortieOasis.Controls.Add(Me.BtnValidationSortie)
        Me.GbxSortieOasis.Controls.Add(Me.Label17)
        Me.GbxSortieOasis.Controls.Add(Me.TxtCommentaireSortie)
        Me.GbxSortieOasis.Controls.Add(Me.DteDateSortie)
        Me.GbxSortieOasis.Controls.Add(Me.Label12)
        Me.GbxSortieOasis.Location = New System.Drawing.Point(18, 538)
        Me.GbxSortieOasis.Name = "GbxSortieOasis"
        Me.GbxSortieOasis.Size = New System.Drawing.Size(480, 142)
        Me.GbxSortieOasis.TabIndex = 50
        Me.GbxSortieOasis.TabStop = False
        Me.GbxSortieOasis.Text = "Sortie Oasis"
        '
        'BtnValidationSortie
        '
        Me.BtnValidationSortie.Location = New System.Drawing.Point(125, 102)
        Me.BtnValidationSortie.Name = "BtnValidationSortie"
        Me.BtnValidationSortie.Size = New System.Drawing.Size(112, 23)
        Me.BtnValidationSortie.TabIndex = 101
        Me.BtnValidationSortie.Text = "Validation sortie"
        Me.BtnValidationSortie.UseVisualStyleBackColor = True
        '
        'BtnSortieOasis
        '
        Me.BtnSortieOasis.Location = New System.Drawing.Point(99, 704)
        Me.BtnSortieOasis.Name = "BtnSortieOasis"
        Me.BtnSortieOasis.Size = New System.Drawing.Size(126, 23)
        Me.BtnSortieOasis.TabIndex = 220
        Me.BtnSortieOasis.Text = "Déclaration de sortie"
        Me.BtnSortieOasis.UseVisualStyleBackColor = True
        '
        'TxtNomMarital
        '
        Me.TxtNomMarital.Location = New System.Drawing.Point(145, 160)
        Me.TxtNomMarital.Name = "TxtNomMarital"
        Me.TxtNomMarital.Size = New System.Drawing.Size(200, 20)
        Me.TxtNomMarital.TabIndex = 15
        '
        'LblNomMarital
        '
        Me.LblNomMarital.AutoSize = True
        Me.LblNomMarital.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNomMarital.Location = New System.Drawing.Point(24, 162)
        Me.LblNomMarital.Name = "LblNomMarital"
        Me.LblNomMarital.Size = New System.Drawing.Size(73, 13)
        Me.LblNomMarital.TabIndex = 53
        Me.LblNomMarital.Text = "Nom marital"
        '
        'LblModulo
        '
        Me.LblModulo.AutoSize = True
        Me.LblModulo.Location = New System.Drawing.Point(316, 189)
        Me.LblModulo.Name = "LblModulo"
        Me.LblModulo.Size = New System.Drawing.Size(19, 13)
        Me.LblModulo.TabIndex = 301
        Me.LblModulo.Text = "99"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(251, 188)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(59, 13)
        Me.Label15.TabIndex = 302
        Me.Label15.Text = "Clé NIR :"
        '
        'BtnValidationDateNaissance
        '
        Me.BtnValidationDateNaissance.Location = New System.Drawing.Point(411, 52)
        Me.BtnValidationDateNaissance.Name = "BtnValidationDateNaissance"
        Me.BtnValidationDateNaissance.Size = New System.Drawing.Size(150, 23)
        Me.BtnValidationDateNaissance.TabIndex = 2
        Me.BtnValidationDateNaissance.Text = "Validation date naissance"
        Me.BtnValidationDateNaissance.UseVisualStyleBackColor = True
        '
        'BtnCreerNote
        '
        Me.BtnCreerNote.Location = New System.Drawing.Point(1150, 704)
        Me.BtnCreerNote.Name = "BtnCreerNote"
        Me.BtnCreerNote.Size = New System.Drawing.Size(100, 23)
        Me.BtnCreerNote.TabIndex = 303
        Me.BtnCreerNote.Text = "Créer une note"
        Me.BtnCreerNote.UseVisualStyleBackColor = True
        '
        'FPatientDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1262, 744)
        Me.Controls.Add(Me.BtnCreerNote)
        Me.Controls.Add(Me.BtnValidationDateNaissance)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.LblModulo)
        Me.Controls.Add(Me.LblNomMarital)
        Me.Controls.Add(Me.TxtNomMarital)
        Me.Controls.Add(Me.BtnSortieOasis)
        Me.Controls.Add(Me.GbxSortieOasis)
        Me.Controls.Add(Me.ChkCouvertureInternet)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.LblAge)
        Me.Controls.Add(Me.LblIdentifiantOasis)
        Me.Controls.Add(Me.LblLabelIdOasis)
        Me.Controls.Add(Me.CbxUniteSanitaire)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.CbxSite)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.DteDateDeces)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.DteDateEntree)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.TxtEmail)
        Me.Controls.Add(Me.TxtNIR)
        Me.Controls.Add(Me.NotePatientDataGridView)
        Me.Controls.Add(Me.BtnValider)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Controls.Add(Me.BtnModifier)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnGoogleMaps)
        Me.Controls.Add(Me.TxtTelMobile)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtTelFixe)
        Me.Controls.Add(Me.TxtVille)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.CbxGenre)
        Me.Controls.Add(Me.LblDateNaissance)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LblNom)
        Me.Controls.Add(Me.LblPrenom)
        Me.Controls.Add(Me.DteDateNaissance)
        Me.Controls.Add(Me.TxtCodePostal)
        Me.Controls.Add(Me.TxtAdresse2)
        Me.Controls.Add(Me.TxtAdresse1)
        Me.Controls.Add(Me.TxtNom)
        Me.Controls.Add(Me.TxtPrenom)
        Me.Name = "FPatientDetailEdit"
        Me.Text = "Patient"
        CType(Me.NotePatientDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.NoteContextMenuStrip.ResumeLayout(False)
        Me.GbxSortieOasis.ResumeLayout(False)
        Me.GbxSortieOasis.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtPrenom As TextBox
    Friend WithEvents TxtNom As TextBox
    Friend WithEvents TxtAdresse1 As TextBox
    Friend WithEvents TxtAdresse2 As TextBox
    Friend WithEvents TxtCodePostal As TextBox
    Friend WithEvents DteDateNaissance As DateTimePicker
    Friend WithEvents LblPrenom As Label
    Friend WithEvents LblNom As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LblDateNaissance As Label
    Friend WithEvents CbxGenre As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents TxtVille As TextBox
    Friend WithEvents TxtTelFixe As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtTelMobile As TextBox
    Friend WithEvents BtnGoogleMaps As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents BtnModifier As Button
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents BtnValider As Button
    Friend WithEvents NotePatientDataGridView As DataGridView
    Friend WithEvents NoteContextMenuStrip As ContextMenuStrip
    Friend WithEvents CréerUneNoteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TxtNIR As TextBox
    Friend WithEvents TxtEmail As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents DteDateEntree As DateTimePicker
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents DteDateSortie As DateTimePicker
    Friend WithEvents DteDateDeces As DateTimePicker
    Friend WithEvents Label13 As Label
    Friend WithEvents CbxSite As ComboBox
    Friend WithEvents Label14 As Label
    Friend WithEvents CbxUniteSanitaire As ComboBox
    Friend WithEvents LblLabelIdOasis As Label
    Friend WithEvents LblIdentifiantOasis As Label
    Friend WithEvents LblAge As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents ChkCouvertureInternet As CheckBox
    Friend WithEvents TxtCommentaireSortie As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents GbxSortieOasis As GroupBox
    Friend WithEvents BtnSortieOasis As Button
    Friend WithEvents TxtNomMarital As TextBox
    Friend WithEvents LblNomMarital As Label
    Friend WithEvents BtnValidationSortie As Button
    Friend WithEvents LblModulo As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents auteur As DataGridViewTextBoxColumn
    Friend WithEvents note As DataGridViewTextBoxColumn
    Friend WithEvents noteId As DataGridViewTextBoxColumn
    Friend WithEvents AnnulerUneNoteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnValidationDateNaissance As Button
    Friend WithEvents BtnCreerNote As Button
End Class
