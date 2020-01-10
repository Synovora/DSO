<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FPatientAldDetailEdit
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
        Me.TxtCommentaire = New System.Windows.Forms.TextBox()
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.BtnValidation = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LblPatientSite = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.LblPatientUniteSanitaire = New System.Windows.Forms.Label()
        Me.LblLabelDateCreation = New System.Windows.Forms.Label()
        Me.LblDateCreation = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblLabelDateModification = New System.Windows.Forms.Label()
        Me.LblDateModification = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.BtnAnnuler = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DteDateDebut = New System.Windows.Forms.DateTimePicker()
        Me.DteDateFin = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtAldId = New System.Windows.Forms.TextBox()
        Me.LblAldDescription = New System.Windows.Forms.Label()
        Me.TxtAldCim10Id = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Lblcim10Description = New System.Windows.Forms.Label()
        Me.LblLabelCodeALD = New System.Windows.Forms.Label()
        Me.LblCodeALD = New System.Windows.Forms.Label()
        Me.BtnSelectionAldCim10 = New System.Windows.Forms.Button()
        Me.BtnInitialiserAldCim10Id = New System.Windows.Forms.Button()
        Me.LblCodeCim10 = New System.Windows.Forms.Label()
        Me.LblLabelCim10Slash = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtCommentaire
        '
        Me.TxtCommentaire.Location = New System.Drawing.Point(163, 168)
        Me.TxtCommentaire.Multiline = True
        Me.TxtCommentaire.Name = "TxtCommentaire"
        Me.TxtCommentaire.Size = New System.Drawing.Size(708, 83)
        Me.TxtCommentaire.TabIndex = 1
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(796, 394)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 30
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(715, 394)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(75, 23)
        Me.BtnValidation.TabIndex = 20
        Me.BtnValidation.Text = "Validation"
        Me.BtnValidation.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox1.Controls.Add(Me.LblPatientPrenom)
        Me.GroupBox1.Controls.Add(Me.LblPatientNom)
        Me.GroupBox1.Controls.Add(Me.LblPatientAge)
        Me.GroupBox1.Controls.Add(Me.LblPatientGenre)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.LblPatientNIR)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.LblPatientSite)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.LblPatientUniteSanitaire)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(859, 64)
        Me.GroupBox1.TabIndex = 35
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Patient"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(6, 16)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(54, 13)
        Me.LblPatientPrenom.TabIndex = 0
        Me.LblPatientPrenom.Text = "Jean-Paul"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(120, 16)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(42, 13)
        Me.LblPatientNom.TabIndex = 1
        Me.LblPatientNom.Text = "Durand"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(229, 16)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(39, 13)
        Me.LblPatientAge.TabIndex = 2
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(333, 16)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(49, 13)
        Me.LblPatientGenre.TabIndex = 3
        Me.LblPatientGenre.Text = "Masculin"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(519, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "NIR :"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(590, 16)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 5
        Me.LblPatientNIR.Text = "1601275125143"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(6, 38)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(37, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Site :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(49, 38)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(33, 13)
        Me.LblPatientSite.TabIndex = 7
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(178, 38)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(113, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Site de référence :"
        '
        'LblPatientUniteSanitaire
        '
        Me.LblPatientUniteSanitaire.AutoSize = True
        Me.LblPatientUniteSanitaire.Location = New System.Drawing.Point(297, 38)
        Me.LblPatientUniteSanitaire.Name = "LblPatientUniteSanitaire"
        Me.LblPatientUniteSanitaire.Size = New System.Drawing.Size(43, 13)
        Me.LblPatientUniteSanitaire.TabIndex = 9
        Me.LblPatientUniteSanitaire.Text = "Auxerre"
        '
        'LblLabelDateCreation
        '
        Me.LblLabelDateCreation.AutoSize = True
        Me.LblLabelDateCreation.Location = New System.Drawing.Point(9, 97)
        Me.LblLabelDateCreation.Name = "LblLabelDateCreation"
        Me.LblLabelDateCreation.Size = New System.Drawing.Size(46, 13)
        Me.LblLabelDateCreation.TabIndex = 36
        Me.LblLabelDateCreation.Text = "Créée le"
        '
        'LblDateCreation
        '
        Me.LblDateCreation.AutoSize = True
        Me.LblDateCreation.Location = New System.Drawing.Point(61, 97)
        Me.LblDateCreation.Name = "LblDateCreation"
        Me.LblDateCreation.Size = New System.Drawing.Size(65, 13)
        Me.LblDateCreation.TabIndex = 37
        Me.LblDateCreation.Text = "01/01/2000"
        '
        'LblLabelUtilisateurCreation
        '
        Me.LblLabelUtilisateurCreation.AutoSize = True
        Me.LblLabelUtilisateurCreation.Location = New System.Drawing.Point(135, 97)
        Me.LblLabelUtilisateurCreation.Name = "LblLabelUtilisateurCreation"
        Me.LblLabelUtilisateurCreation.Size = New System.Drawing.Size(22, 13)
        Me.LblLabelUtilisateurCreation.TabIndex = 38
        Me.LblLabelUtilisateurCreation.Text = "par"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(164, 97)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(91, 13)
        Me.LblUtilisateurCreation.TabIndex = 39
        Me.LblUtilisateurCreation.Text = "Stéphane Durand"
        '
        'LblLabelDateModification
        '
        Me.LblLabelDateModification.AutoSize = True
        Me.LblLabelDateModification.Location = New System.Drawing.Point(355, 97)
        Me.LblLabelDateModification.Name = "LblLabelDateModification"
        Me.LblLabelDateModification.Size = New System.Drawing.Size(58, 13)
        Me.LblLabelDateModification.TabIndex = 40
        Me.LblLabelDateModification.Text = "Modifiée le"
        '
        'LblDateModification
        '
        Me.LblDateModification.AutoSize = True
        Me.LblDateModification.Location = New System.Drawing.Point(419, 97)
        Me.LblDateModification.Name = "LblDateModification"
        Me.LblDateModification.Size = New System.Drawing.Size(65, 13)
        Me.LblDateModification.TabIndex = 41
        Me.LblDateModification.Text = "01/01/2000"
        '
        'LblLabelUtilisateurModification
        '
        Me.LblLabelUtilisateurModification.AutoSize = True
        Me.LblLabelUtilisateurModification.Location = New System.Drawing.Point(501, 97)
        Me.LblLabelUtilisateurModification.Name = "LblLabelUtilisateurModification"
        Me.LblLabelUtilisateurModification.Size = New System.Drawing.Size(22, 13)
        Me.LblLabelUtilisateurModification.TabIndex = 42
        Me.LblLabelUtilisateurModification.Text = "par"
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(529, 97)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(93, 13)
        Me.LblUtilisateurModification.TabIndex = 43
        Me.LblUtilisateurModification.Text = "Georges Moustaki"
        '
        'BtnAnnuler
        '
        Me.BtnAnnuler.Location = New System.Drawing.Point(494, 394)
        Me.BtnAnnuler.Name = "BtnAnnuler"
        Me.BtnAnnuler.Size = New System.Drawing.Size(93, 23)
        Me.BtnAnnuler.TabIndex = 25
        Me.BtnAnnuler.Text = "Annulation ALD"
        Me.BtnAnnuler.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 168)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 45
        Me.Label1.Text = "Commentaire"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 144)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(28, 13)
        Me.Label2.TabIndex = 47
        Me.Label2.Text = "ALD"
        '
        'DteDateDebut
        '
        Me.DteDateDebut.Location = New System.Drawing.Point(163, 257)
        Me.DteDateDebut.Name = "DteDateDebut"
        Me.DteDateDebut.Size = New System.Drawing.Size(200, 20)
        Me.DteDateDebut.TabIndex = 5
        '
        'DteDateFin
        '
        Me.DteDateFin.Location = New System.Drawing.Point(163, 283)
        Me.DteDateFin.Name = "DteDateFin"
        Me.DteDateFin.Size = New System.Drawing.Size(200, 20)
        Me.DteDateFin.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 263)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 50
        Me.Label3.Text = "Date de début"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 289)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 13)
        Me.Label4.TabIndex = 51
        Me.Label4.Text = "Date de fin"
        '
        'TxtAldId
        '
        Me.TxtAldId.Location = New System.Drawing.Point(163, 142)
        Me.TxtAldId.Name = "TxtAldId"
        Me.TxtAldId.Size = New System.Drawing.Size(59, 20)
        Me.TxtAldId.TabIndex = 16
        '
        'LblAldDescription
        '
        Me.LblAldDescription.AutoSize = True
        Me.LblAldDescription.Location = New System.Drawing.Point(228, 145)
        Me.LblAldDescription.Name = "LblAldDescription"
        Me.LblAldDescription.Size = New System.Drawing.Size(103, 13)
        Me.LblAldDescription.TabIndex = 53
        Me.LblAldDescription.Text = "Description de l'ALD"
        '
        'TxtAldCim10Id
        '
        Me.TxtAldCim10Id.Location = New System.Drawing.Point(163, 309)
        Me.TxtAldCim10Id.Name = "TxtAldCim10Id"
        Me.TxtAldCim10Id.ReadOnly = True
        Me.TxtAldCim10Id.Size = New System.Drawing.Size(59, 20)
        Me.TxtAldCim10Id.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 312)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(136, 13)
        Me.Label6.TabIndex = 55
        Me.Label6.Text = "Codification supplémentaire"
        '
        'Lblcim10Description
        '
        Me.Lblcim10Description.AutoSize = True
        Me.Lblcim10Description.Location = New System.Drawing.Point(228, 312)
        Me.Lblcim10Description.Name = "Lblcim10Description"
        Me.Lblcim10Description.Size = New System.Drawing.Size(91, 13)
        Me.Lblcim10Description.TabIndex = 56
        Me.Lblcim10Description.Text = "Description cim10"
        '
        'LblLabelCodeALD
        '
        Me.LblLabelCodeALD.AutoSize = True
        Me.LblLabelCodeALD.Location = New System.Drawing.Point(9, 332)
        Me.LblLabelCodeALD.Name = "LblLabelCodeALD"
        Me.LblLabelCodeALD.Size = New System.Drawing.Size(98, 13)
        Me.LblLabelCodeALD.TabIndex = 57
        Me.LblLabelCodeALD.Text = "Code ALD / CIM10"
        '
        'LblCodeALD
        '
        Me.LblCodeALD.AutoSize = True
        Me.LblCodeALD.Location = New System.Drawing.Point(160, 332)
        Me.LblCodeALD.Name = "LblCodeALD"
        Me.LblCodeALD.Size = New System.Drawing.Size(55, 13)
        Me.LblCodeALD.TabIndex = 58
        Me.LblCodeALD.Text = "code ALD"
        '
        'BtnSelectionAldCim10
        '
        Me.BtnSelectionAldCim10.Location = New System.Drawing.Point(12, 360)
        Me.BtnSelectionAldCim10.Name = "BtnSelectionAldCim10"
        Me.BtnSelectionAldCim10.Size = New System.Drawing.Size(133, 23)
        Me.BtnSelectionAldCim10.TabIndex = 59
        Me.BtnSelectionAldCim10.Text = "Sélection ALD CIM10"
        Me.BtnSelectionAldCim10.UseVisualStyleBackColor = True
        '
        'BtnInitialiserAldCim10Id
        '
        Me.BtnInitialiserAldCim10Id.Location = New System.Drawing.Point(151, 360)
        Me.BtnInitialiserAldCim10Id.Name = "BtnInitialiserAldCim10Id"
        Me.BtnInitialiserAldCim10Id.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitialiserAldCim10Id.TabIndex = 60
        Me.BtnInitialiserAldCim10Id.Text = "Initialiser ALD CIM10"
        Me.BtnInitialiserAldCim10Id.UseVisualStyleBackColor = True
        '
        'LblCodeCim10
        '
        Me.LblCodeCim10.AutoSize = True
        Me.LblCodeCim10.Location = New System.Drawing.Point(234, 332)
        Me.LblCodeCim10.Name = "LblCodeCim10"
        Me.LblCodeCim10.Size = New System.Drawing.Size(66, 13)
        Me.LblCodeCim10.TabIndex = 61
        Me.LblCodeCim10.Text = "Code CIM10"
        '
        'LblLabelCim10Slash
        '
        Me.LblLabelCim10Slash.AutoSize = True
        Me.LblLabelCim10Slash.Location = New System.Drawing.Point(216, 332)
        Me.LblLabelCim10Slash.Name = "LblLabelCim10Slash"
        Me.LblLabelCim10Slash.Size = New System.Drawing.Size(12, 13)
        Me.LblLabelCim10Slash.TabIndex = 62
        Me.LblLabelCim10Slash.Text = "/"
        '
        'FPatientAldDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(883, 429)
        Me.Controls.Add(Me.LblLabelCim10Slash)
        Me.Controls.Add(Me.LblCodeCim10)
        Me.Controls.Add(Me.BtnInitialiserAldCim10Id)
        Me.Controls.Add(Me.BtnSelectionAldCim10)
        Me.Controls.Add(Me.LblCodeALD)
        Me.Controls.Add(Me.LblLabelCodeALD)
        Me.Controls.Add(Me.Lblcim10Description)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TxtAldCim10Id)
        Me.Controls.Add(Me.LblAldDescription)
        Me.Controls.Add(Me.TxtAldId)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.DteDateFin)
        Me.Controls.Add(Me.DteDateDebut)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnAnnuler)
        Me.Controls.Add(Me.LblUtilisateurModification)
        Me.Controls.Add(Me.LblLabelUtilisateurModification)
        Me.Controls.Add(Me.LblDateModification)
        Me.Controls.Add(Me.LblLabelDateModification)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.LblLabelUtilisateurCreation)
        Me.Controls.Add(Me.LblDateCreation)
        Me.Controls.Add(Me.LblLabelDateCreation)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Controls.Add(Me.TxtCommentaire)
        Me.Name = "FPatientAldDetailEdit"
        Me.Text = "Détail ALD patient"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtCommentaire As TextBox
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents BtnValidation As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents LblPatientSite As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents LblPatientUniteSanitaire As Label
    Friend WithEvents LblLabelDateCreation As Label
    Friend WithEvents LblDateCreation As Label
    Friend WithEvents LblLabelUtilisateurCreation As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblLabelDateModification As Label
    Friend WithEvents LblDateModification As Label
    Friend WithEvents LblLabelUtilisateurModification As Label
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents BtnAnnuler As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents DteDateDebut As DateTimePicker
    Friend WithEvents DteDateFin As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents TxtAldId As TextBox
    Friend WithEvents LblAldDescription As Label
    Friend WithEvents TxtAldCim10Id As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Lblcim10Description As Label
    Friend WithEvents LblLabelCodeALD As Label
    Friend WithEvents LblCodeALD As Label
    Friend WithEvents BtnSelectionAldCim10 As Button
    Friend WithEvents BtnInitialiserAldCim10Id As Button
    Friend WithEvents LblCodeCim10 As Label
    Friend WithEvents LblLabelCim10Slash As Label
End Class
