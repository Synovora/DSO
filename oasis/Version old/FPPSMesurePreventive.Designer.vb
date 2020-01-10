<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FPPSMesurePreventive
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
        Me.components = New System.ComponentModel.Container()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LblALD = New System.Windows.Forms.Label()
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
        Me.ToolTipPPS = New System.Windows.Forms.ToolTip(Me.components)
        Me.NumPriorite = New System.Windows.Forms.NumericUpDown()
        Me.TxtDrcId = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtDrcDescription = New System.Windows.Forms.TextBox()
        Me.TxtCommentaire = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.BtnValidation = New System.Windows.Forms.Button()
        Me.BtnAnnulation = New System.Windows.Forms.Button()
        Me.BtnDrcSelecteur = New System.Windows.Forms.Button()
        Me.BtnInitDrc = New System.Windows.Forms.Button()
        Me.LblLabelCommentaireArret = New System.Windows.Forms.Label()
        Me.TxtCommentaireArret = New System.Windows.Forms.TextBox()
        Me.BtnConfirmationAnnulation = New System.Windows.Forms.Button()
        Me.LblId = New System.Windows.Forms.Label()
        Me.LblLabelStrategieParModification = New System.Windows.Forms.Label()
        Me.LblLabelStrategieParCreation = New System.Windows.Forms.Label()
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblLabelStrategieDateCreation = New System.Windows.Forms.Label()
        Me.LblStrategieDateCreation = New System.Windows.Forms.Label()
        Me.LblLabelStrategieDateModification = New System.Windows.Forms.Label()
        Me.LblStrategieDateModification = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.NumPriorite, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox1.Controls.Add(Me.LblALD)
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
        Me.GroupBox1.Size = New System.Drawing.Size(999, 71)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Etat civil"
        '
        'LblALD
        '
        Me.LblALD.AutoSize = True
        Me.LblALD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblALD.ForeColor = System.Drawing.Color.Red
        Me.LblALD.Location = New System.Drawing.Point(903, 16)
        Me.LblALD.Name = "LblALD"
        Me.LblALD.Size = New System.Drawing.Size(31, 13)
        Me.LblALD.TabIndex = 22
        Me.LblALD.Text = "ALD"
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
        'NumPriorite
        '
        Me.NumPriorite.Location = New System.Drawing.Point(101, 129)
        Me.NumPriorite.Name = "NumPriorite"
        Me.NumPriorite.Size = New System.Drawing.Size(64, 20)
        Me.NumPriorite.TabIndex = 17
        '
        'TxtDrcId
        '
        Me.TxtDrcId.Location = New System.Drawing.Point(101, 155)
        Me.TxtDrcId.Name = "TxtDrcId"
        Me.TxtDrcId.ReadOnly = True
        Me.TxtDrcId.Size = New System.Drawing.Size(64, 20)
        Me.TxtDrcId.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 131)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Priorité"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 158)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "DRC/ORC"
        '
        'TxtDrcDescription
        '
        Me.TxtDrcDescription.Location = New System.Drawing.Point(171, 155)
        Me.TxtDrcDescription.Name = "TxtDrcDescription"
        Me.TxtDrcDescription.ReadOnly = True
        Me.TxtDrcDescription.Size = New System.Drawing.Size(840, 20)
        Me.TxtDrcDescription.TabIndex = 21
        '
        'TxtCommentaire
        '
        Me.TxtCommentaire.Location = New System.Drawing.Point(101, 210)
        Me.TxtCommentaire.Multiline = True
        Me.TxtCommentaire.Name = "TxtCommentaire"
        Me.TxtCommentaire.Size = New System.Drawing.Size(910, 98)
        Me.TxtCommentaire.TabIndex = 22
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 213)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 13)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Commentaire"
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(936, 445)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 24
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(855, 445)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(75, 23)
        Me.BtnValidation.TabIndex = 25
        Me.BtnValidation.Text = "Validation"
        Me.BtnValidation.UseVisualStyleBackColor = True
        '
        'BtnAnnulation
        '
        Me.BtnAnnulation.Location = New System.Drawing.Point(602, 445)
        Me.BtnAnnulation.Name = "BtnAnnulation"
        Me.BtnAnnulation.Size = New System.Drawing.Size(112, 23)
        Me.BtnAnnulation.TabIndex = 26
        Me.BtnAnnulation.Text = "Annuler mesure"
        Me.BtnAnnulation.UseVisualStyleBackColor = True
        '
        'BtnDrcSelecteur
        '
        Me.BtnDrcSelecteur.Location = New System.Drawing.Point(101, 181)
        Me.BtnDrcSelecteur.Name = "BtnDrcSelecteur"
        Me.BtnDrcSelecteur.Size = New System.Drawing.Size(119, 23)
        Me.BtnDrcSelecteur.TabIndex = 27
        Me.BtnDrcSelecteur.Text = "Sélection DRC/ORC"
        Me.BtnDrcSelecteur.UseVisualStyleBackColor = True
        '
        'BtnInitDrc
        '
        Me.BtnInitDrc.Location = New System.Drawing.Point(226, 181)
        Me.BtnInitDrc.Name = "BtnInitDrc"
        Me.BtnInitDrc.Size = New System.Drawing.Size(75, 23)
        Me.BtnInitDrc.TabIndex = 28
        Me.BtnInitDrc.Text = "Initialiser"
        Me.BtnInitDrc.UseVisualStyleBackColor = True
        '
        'LblLabelCommentaireArret
        '
        Me.LblLabelCommentaireArret.AutoSize = True
        Me.LblLabelCommentaireArret.Location = New System.Drawing.Point(102, 317)
        Me.LblLabelCommentaireArret.Name = "LblLabelCommentaireArret"
        Me.LblLabelCommentaireArret.Size = New System.Drawing.Size(120, 13)
        Me.LblLabelCommentaireArret.TabIndex = 35
        Me.LblLabelCommentaireArret.Text = "Commentaire annulation"
        '
        'TxtCommentaireArret
        '
        Me.TxtCommentaireArret.Location = New System.Drawing.Point(101, 333)
        Me.TxtCommentaireArret.Multiline = True
        Me.TxtCommentaireArret.Name = "TxtCommentaireArret"
        Me.TxtCommentaireArret.Size = New System.Drawing.Size(910, 60)
        Me.TxtCommentaireArret.TabIndex = 34
        '
        'BtnConfirmationAnnulation
        '
        Me.BtnConfirmationAnnulation.Location = New System.Drawing.Point(720, 445)
        Me.BtnConfirmationAnnulation.Name = "BtnConfirmationAnnulation"
        Me.BtnConfirmationAnnulation.Size = New System.Drawing.Size(129, 23)
        Me.BtnConfirmationAnnulation.TabIndex = 36
        Me.BtnConfirmationAnnulation.Text = "Confirmation annulation"
        Me.BtnConfirmationAnnulation.UseVisualStyleBackColor = True
        '
        'LblId
        '
        Me.LblId.AutoSize = True
        Me.LblId.Location = New System.Drawing.Point(998, 412)
        Me.LblId.Name = "LblId"
        Me.LblId.Size = New System.Drawing.Size(13, 13)
        Me.LblId.TabIndex = 78
        Me.LblId.Text = "?"
        '
        'LblLabelStrategieParModification
        '
        Me.LblLabelStrategieParModification.AutoSize = True
        Me.LblLabelStrategieParModification.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelStrategieParModification.Location = New System.Drawing.Point(613, 412)
        Me.LblLabelStrategieParModification.Name = "LblLabelStrategieParModification"
        Me.LblLabelStrategieParModification.Size = New System.Drawing.Size(25, 13)
        Me.LblLabelStrategieParModification.TabIndex = 77
        Me.LblLabelStrategieParModification.Text = "par"
        '
        'LblLabelStrategieParCreation
        '
        Me.LblLabelStrategieParCreation.AutoSize = True
        Me.LblLabelStrategieParCreation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelStrategieParCreation.Location = New System.Drawing.Point(226, 412)
        Me.LblLabelStrategieParCreation.Name = "LblLabelStrategieParCreation"
        Me.LblLabelStrategieParCreation.Size = New System.Drawing.Size(25, 13)
        Me.LblLabelStrategieParCreation.TabIndex = 76
        Me.LblLabelStrategieParCreation.Text = "par"
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(644, 412)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(127, 13)
        Me.LblUtilisateurModification.TabIndex = 75
        Me.LblUtilisateurModification.Text = "Utilisateur en modification"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(257, 412)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(109, 13)
        Me.LblUtilisateurCreation.TabIndex = 74
        Me.LblUtilisateurCreation.Text = "Utilisateur en création"
        '
        'LblLabelStrategieDateCreation
        '
        Me.LblLabelStrategieDateCreation.AutoSize = True
        Me.LblLabelStrategieDateCreation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelStrategieDateCreation.Location = New System.Drawing.Point(98, 412)
        Me.LblLabelStrategieDateCreation.Name = "LblLabelStrategieDateCreation"
        Me.LblLabelStrategieDateCreation.Size = New System.Drawing.Size(55, 13)
        Me.LblLabelStrategieDateCreation.TabIndex = 70
        Me.LblLabelStrategieDateCreation.Text = "Créé le :"
        '
        'LblStrategieDateCreation
        '
        Me.LblStrategieDateCreation.AutoSize = True
        Me.LblStrategieDateCreation.Location = New System.Drawing.Point(159, 412)
        Me.LblStrategieDateCreation.Name = "LblStrategieDateCreation"
        Me.LblStrategieDateCreation.Size = New System.Drawing.Size(61, 13)
        Me.LblStrategieDateCreation.TabIndex = 71
        Me.LblStrategieDateCreation.Text = "01.10.2019"
        '
        'LblLabelStrategieDateModification
        '
        Me.LblLabelStrategieDateModification.AutoSize = True
        Me.LblLabelStrategieDateModification.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelStrategieDateModification.Location = New System.Drawing.Point(470, 412)
        Me.LblLabelStrategieDateModification.Name = "LblLabelStrategieDateModification"
        Me.LblLabelStrategieDateModification.Size = New System.Drawing.Size(70, 13)
        Me.LblLabelStrategieDateModification.TabIndex = 72
        Me.LblLabelStrategieDateModification.Text = "Modifié le :"
        '
        'LblStrategieDateModification
        '
        Me.LblStrategieDateModification.AutoSize = True
        Me.LblStrategieDateModification.Location = New System.Drawing.Point(546, 412)
        Me.LblStrategieDateModification.Name = "LblStrategieDateModification"
        Me.LblStrategieDateModification.Size = New System.Drawing.Size(61, 13)
        Me.LblStrategieDateModification.TabIndex = 73
        Me.LblStrategieDateModification.Text = "02.10.2019"
        '
        'FPPSMesurePreventive
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1023, 476)
        Me.Controls.Add(Me.LblId)
        Me.Controls.Add(Me.LblLabelStrategieParModification)
        Me.Controls.Add(Me.LblLabelStrategieParCreation)
        Me.Controls.Add(Me.LblUtilisateurModification)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.LblLabelStrategieDateCreation)
        Me.Controls.Add(Me.LblStrategieDateCreation)
        Me.Controls.Add(Me.LblLabelStrategieDateModification)
        Me.Controls.Add(Me.LblStrategieDateModification)
        Me.Controls.Add(Me.BtnConfirmationAnnulation)
        Me.Controls.Add(Me.LblLabelCommentaireArret)
        Me.Controls.Add(Me.TxtCommentaireArret)
        Me.Controls.Add(Me.BtnInitDrc)
        Me.Controls.Add(Me.BtnDrcSelecteur)
        Me.Controls.Add(Me.BtnAnnulation)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TxtCommentaire)
        Me.Controls.Add(Me.TxtDrcDescription)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtDrcId)
        Me.Controls.Add(Me.NumPriorite)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FPPSMesurePreventive"
        Me.Text = "Plan Personnalisé de Soin : Mesure préventive"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.NumPriorite, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LblALD As Label
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
    Friend WithEvents ToolTipPPS As ToolTip
    Friend WithEvents NumPriorite As NumericUpDown
    Friend WithEvents TxtDrcId As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents TxtDrcDescription As TextBox
    Friend WithEvents TxtCommentaire As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents BtnValidation As Button
    Friend WithEvents BtnAnnulation As Button
    Friend WithEvents BtnDrcSelecteur As Button
    Friend WithEvents BtnInitDrc As Button
    Friend WithEvents LblLabelCommentaireArret As Label
    Friend WithEvents TxtCommentaireArret As TextBox
    Friend WithEvents BtnConfirmationAnnulation As Button
    Friend WithEvents LblId As Label
    Friend WithEvents LblLabelStrategieParModification As Label
    Friend WithEvents LblLabelStrategieParCreation As Label
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblLabelStrategieDateCreation As Label
    Friend WithEvents LblStrategieDateCreation As Label
    Friend WithEvents LblLabelStrategieDateModification As Label
    Friend WithEvents LblStrategieDateModification As Label
End Class
