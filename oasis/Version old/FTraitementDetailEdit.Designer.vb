<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FTraitementDetailEdit
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblTraitementDateFin = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.LblFenetreTherapeutique = New System.Windows.Forms.Label()
        Me.LblTraitementDateDebut = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.LblLabelTraitementDateCreation = New System.Windows.Forms.Label()
        Me.LblTraitementDateCreation = New System.Windows.Forms.Label()
        Me.LblLabelTraitementDateModification = New System.Windows.Forms.Label()
        Me.LblTraitementDateModification = New System.Windows.Forms.Label()
        Me.LblFenetreDateDebut = New System.Windows.Forms.Label()
        Me.LblFenetreTherapeutiqueAu = New System.Windows.Forms.Label()
        Me.LblFenetreDateFin = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BtnMedoc = New System.Windows.Forms.Button()
        Me.LblTraitementPosologie = New System.Windows.Forms.Label()
        Me.LblMedicamentDCI = New System.Windows.Forms.Label()
        Me.TxtTraitementPosologieCommentaire = New System.Windows.Forms.TextBox()
        Me.LblTraitementMedicamentCIS = New System.Windows.Forms.Label()
        Me.CbxTraitementBase = New System.Windows.Forms.ComboBox()
        Me.LblTraitementBase = New System.Windows.Forms.Label()
        Me.GbxPosologie = New System.Windows.Forms.GroupBox()
        Me.LblRythmeSoir = New System.Windows.Forms.Label()
        Me.LblRythmeApresMidi = New System.Windows.Forms.Label()
        Me.LblRythmeMidi = New System.Windows.Forms.Label()
        Me.LblRythmeMatin = New System.Windows.Forms.Label()
        Me.NumRythmeSoir = New System.Windows.Forms.NumericUpDown()
        Me.NumRythmeApresMidi = New System.Windows.Forms.NumericUpDown()
        Me.NumRythmeMidi = New System.Windows.Forms.NumericUpDown()
        Me.PgbMiseAJour = New System.Windows.Forms.ProgressBar()
        Me.NumRythmeMatin = New System.Windows.Forms.NumericUpDown()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.LblTraitementRythme = New System.Windows.Forms.Label()
        Me.MedicamentGroupBox = New System.Windows.Forms.GroupBox()
        Me.LblMedicamentTitulaire = New System.Windows.Forms.Label()
        Me.LblMedicamentAdministration = New System.Windows.Forms.Label()
        Me.LblMedicamentForme = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.BtnValidation = New System.Windows.Forms.Button()
        Me.BtnRetour = New System.Windows.Forms.Button()
        Me.TxtTraitementCommentaire = New System.Windows.Forms.TextBox()
        Me.GbxTraitement = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.NumNumeroOrdre = New System.Windows.Forms.NumericUpDown()
        Me.LblTraitementDuree = New System.Windows.Forms.Label()
        Me.LblLabelTraitementDuree = New System.Windows.Forms.Label()
        Me.DteTraitementDateFin = New System.Windows.Forms.DateTimePicker()
        Me.DteTraitementDateDebut = New System.Windows.Forms.DateTimePicker()
        Me.BtnSupprimerTraitement = New System.Windows.Forms.Button()
        Me.BtnArretTraitement = New System.Windows.Forms.Button()
        Me.BtnAnnulerTraitement = New System.Windows.Forms.Button()
        Me.GbxArretTraitement = New System.Windows.Forms.GroupBox()
        Me.BtnValidationArret = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtCommentaireArret = New System.Windows.Forms.TextBox()
        Me.ChkContreIndication = New System.Windows.Forms.CheckBox()
        Me.ChkAllergie = New System.Windows.Forms.CheckBox()
        Me.GbxAnnulationTraitement = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtCommentaireAnnulation = New System.Windows.Forms.TextBox()
        Me.BtnValidationAnnulation = New System.Windows.Forms.Button()
        Me.GbxFenetreTherapeutique = New System.Windows.Forms.GroupBox()
        Me.LblFenetreActive = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblLabelTraitementParCreation = New System.Windows.Forms.Label()
        Me.LblLabelTraitementParModification = New System.Windows.Forms.Label()
        Me.LblstatutTraitement = New System.Windows.Forms.Label()
        Me.PnlStatutTraitement = New System.Windows.Forms.Panel()
        Me.LblId = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GbxPosologie.SuspendLayout()
        CType(Me.NumRythmeSoir, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumRythmeApresMidi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumRythmeMidi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumRythmeMatin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MedicamentGroupBox.SuspendLayout()
        Me.GbxTraitement.SuspendLayout()
        CType(Me.NumNumeroOrdre, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GbxArretTraitement.SuspendLayout()
        Me.GbxAnnulationTraitement.SuspendLayout()
        Me.GbxFenetreTherapeutique.SuspendLayout()
        Me.PnlStatutTraitement.SuspendLayout()
        Me.SuspendLayout()
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
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Posologie :"
        '
        'LblTraitementDateFin
        '
        Me.LblTraitementDateFin.AutoSize = True
        Me.LblTraitementDateFin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTraitementDateFin.Location = New System.Drawing.Point(6, 46)
        Me.LblTraitementDateFin.Name = "LblTraitementDateFin"
        Me.LblTraitementDateFin.Size = New System.Drawing.Size(84, 13)
        Me.LblTraitementDateFin.TabIndex = 14
        Me.LblTraitementDateFin.Text = "Fin traitement"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 39)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(36, 13)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "DCI :"
        '
        'LblFenetreTherapeutique
        '
        Me.LblFenetreTherapeutique.AutoSize = True
        Me.LblFenetreTherapeutique.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFenetreTherapeutique.ForeColor = System.Drawing.Color.DarkBlue
        Me.LblFenetreTherapeutique.Location = New System.Drawing.Point(85, 16)
        Me.LblFenetreTherapeutique.Name = "LblFenetreTherapeutique"
        Me.LblFenetreTherapeutique.Size = New System.Drawing.Size(173, 15)
        Me.LblFenetreTherapeutique.TabIndex = 17
        Me.LblFenetreTherapeutique.Text = "Fenêtre Thérapeutique du"
        '
        'LblTraitementDateDebut
        '
        Me.LblTraitementDateDebut.AutoSize = True
        Me.LblTraitementDateDebut.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTraitementDateDebut.Location = New System.Drawing.Point(6, 20)
        Me.LblTraitementDateDebut.Name = "LblTraitementDateDebut"
        Me.LblTraitementDateDebut.Size = New System.Drawing.Size(88, 13)
        Me.LblTraitementDateDebut.TabIndex = 19
        Me.LblTraitementDateDebut.Text = "Date de début"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(6, 72)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(79, 13)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "Commentaire"
        '
        'LblLabelTraitementDateCreation
        '
        Me.LblLabelTraitementDateCreation.AutoSize = True
        Me.LblLabelTraitementDateCreation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelTraitementDateCreation.Location = New System.Drawing.Point(12, 667)
        Me.LblLabelTraitementDateCreation.Name = "LblLabelTraitementDateCreation"
        Me.LblLabelTraitementDateCreation.Size = New System.Drawing.Size(55, 13)
        Me.LblLabelTraitementDateCreation.TabIndex = 24
        Me.LblLabelTraitementDateCreation.Text = "Créé le :"
        '
        'LblTraitementDateCreation
        '
        Me.LblTraitementDateCreation.AutoSize = True
        Me.LblTraitementDateCreation.Location = New System.Drawing.Point(73, 667)
        Me.LblTraitementDateCreation.Name = "LblTraitementDateCreation"
        Me.LblTraitementDateCreation.Size = New System.Drawing.Size(61, 13)
        Me.LblTraitementDateCreation.TabIndex = 25
        Me.LblTraitementDateCreation.Text = "01.10.2019"
        '
        'LblLabelTraitementDateModification
        '
        Me.LblLabelTraitementDateModification.AutoSize = True
        Me.LblLabelTraitementDateModification.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelTraitementDateModification.Location = New System.Drawing.Point(384, 667)
        Me.LblLabelTraitementDateModification.Name = "LblLabelTraitementDateModification"
        Me.LblLabelTraitementDateModification.Size = New System.Drawing.Size(70, 13)
        Me.LblLabelTraitementDateModification.TabIndex = 26
        Me.LblLabelTraitementDateModification.Text = "Modifié le :"
        '
        'LblTraitementDateModification
        '
        Me.LblTraitementDateModification.AutoSize = True
        Me.LblTraitementDateModification.Location = New System.Drawing.Point(460, 667)
        Me.LblTraitementDateModification.Name = "LblTraitementDateModification"
        Me.LblTraitementDateModification.Size = New System.Drawing.Size(61, 13)
        Me.LblTraitementDateModification.TabIndex = 27
        Me.LblTraitementDateModification.Text = "02.10.2019"
        '
        'LblFenetreDateDebut
        '
        Me.LblFenetreDateDebut.AutoSize = True
        Me.LblFenetreDateDebut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFenetreDateDebut.ForeColor = System.Drawing.Color.DarkBlue
        Me.LblFenetreDateDebut.Location = New System.Drawing.Point(263, 16)
        Me.LblFenetreDateDebut.Name = "LblFenetreDateDebut"
        Me.LblFenetreDateDebut.Size = New System.Drawing.Size(69, 15)
        Me.LblFenetreDateDebut.TabIndex = 28
        Me.LblFenetreDateDebut.Text = "01.01.2019"
        '
        'LblFenetreTherapeutiqueAu
        '
        Me.LblFenetreTherapeutiqueAu.AutoSize = True
        Me.LblFenetreTherapeutiqueAu.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFenetreTherapeutiqueAu.ForeColor = System.Drawing.Color.DarkBlue
        Me.LblFenetreTherapeutiqueAu.Location = New System.Drawing.Point(343, 16)
        Me.LblFenetreTherapeutiqueAu.Name = "LblFenetreTherapeutiqueAu"
        Me.LblFenetreTherapeutiqueAu.Size = New System.Drawing.Size(23, 15)
        Me.LblFenetreTherapeutiqueAu.TabIndex = 29
        Me.LblFenetreTherapeutiqueAu.Text = "au"
        '
        'LblFenetreDateFin
        '
        Me.LblFenetreDateFin.AutoSize = True
        Me.LblFenetreDateFin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFenetreDateFin.ForeColor = System.Drawing.Color.DarkBlue
        Me.LblFenetreDateFin.Location = New System.Drawing.Point(382, 16)
        Me.LblFenetreDateFin.Name = "LblFenetreDateFin"
        Me.LblFenetreDateFin.Size = New System.Drawing.Size(69, 15)
        Me.LblFenetreDateFin.TabIndex = 30
        Me.LblFenetreDateFin.Text = "31.01.2019"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(12, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(35, 13)
        Me.Label10.TabIndex = 31
        Me.Label10.Text = "CIS :"
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
        Me.GroupBox1.Location = New System.Drawing.Point(15, 7)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(747, 64)
        Me.GroupBox1.TabIndex = 33
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Patient"
        '
        'BtnMedoc
        '
        Me.BtnMedoc.Location = New System.Drawing.Point(16, 758)
        Me.BtnMedoc.Name = "BtnMedoc"
        Me.BtnMedoc.Size = New System.Drawing.Size(129, 23)
        Me.BtnMedoc.TabIndex = 34
        Me.BtnMedoc.Text = "Détail médicament"
        Me.BtnMedoc.UseVisualStyleBackColor = True
        '
        'LblTraitementPosologie
        '
        Me.LblTraitementPosologie.AutoSize = True
        Me.LblTraitementPosologie.ForeColor = System.Drawing.Color.DarkBlue
        Me.LblTraitementPosologie.Location = New System.Drawing.Point(117, 73)
        Me.LblTraitementPosologie.Name = "LblTraitementPosologie"
        Me.LblTraitementPosologie.Size = New System.Drawing.Size(31, 13)
        Me.LblTraitementPosologie.TabIndex = 13
        Me.LblTraitementPosologie.Text = "1.0.1"
        '
        'LblMedicamentDCI
        '
        Me.LblMedicamentDCI.AutoSize = True
        Me.LblMedicamentDCI.Location = New System.Drawing.Point(108, 39)
        Me.LblMedicamentDCI.Name = "LblMedicamentDCI"
        Me.LblMedicamentDCI.Size = New System.Drawing.Size(300, 13)
        Me.LblMedicamentDCI.TabIndex = 16
        Me.LblMedicamentDCI.Text = "TRAMADOL EG L.P. 200 mg, comprime a liberation prolongee"
        '
        'TxtTraitementPosologieCommentaire
        '
        Me.TxtTraitementPosologieCommentaire.AcceptsReturn = True
        Me.TxtTraitementPosologieCommentaire.Location = New System.Drawing.Point(117, 93)
        Me.TxtTraitementPosologieCommentaire.MaxLength = 256
        Me.TxtTraitementPosologieCommentaire.Multiline = True
        Me.TxtTraitementPosologieCommentaire.Name = "TxtTraitementPosologieCommentaire"
        Me.TxtTraitementPosologieCommentaire.Size = New System.Drawing.Size(457, 59)
        Me.TxtTraitementPosologieCommentaire.TabIndex = 35
        '
        'LblTraitementMedicamentCIS
        '
        Me.LblTraitementMedicamentCIS.AutoSize = True
        Me.LblTraitementMedicamentCIS.Location = New System.Drawing.Point(108, 18)
        Me.LblTraitementMedicamentCIS.Name = "LblTraitementMedicamentCIS"
        Me.LblTraitementMedicamentCIS.Size = New System.Drawing.Size(55, 13)
        Me.LblTraitementMedicamentCIS.TabIndex = 32
        Me.LblTraitementMedicamentCIS.Text = "12345678"
        '
        'CbxTraitementBase
        '
        Me.CbxTraitementBase.FormattingEnabled = True
        Me.CbxTraitementBase.Items.AddRange(New Object() {"Journalier", "Hebdomadaire", "Mensuel", "Annuel"})
        Me.CbxTraitementBase.Location = New System.Drawing.Point(117, 18)
        Me.CbxTraitementBase.Name = "CbxTraitementBase"
        Me.CbxTraitementBase.Size = New System.Drawing.Size(121, 21)
        Me.CbxTraitementBase.TabIndex = 36
        '
        'LblTraitementBase
        '
        Me.LblTraitementBase.AutoSize = True
        Me.LblTraitementBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTraitementBase.Location = New System.Drawing.Point(12, 21)
        Me.LblTraitementBase.Name = "LblTraitementBase"
        Me.LblTraitementBase.Size = New System.Drawing.Size(35, 13)
        Me.LblTraitementBase.TabIndex = 37
        Me.LblTraitementBase.Text = "Base"
        '
        'GbxPosologie
        '
        Me.GbxPosologie.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.GbxPosologie.Controls.Add(Me.LblRythmeSoir)
        Me.GbxPosologie.Controls.Add(Me.LblRythmeApresMidi)
        Me.GbxPosologie.Controls.Add(Me.LblRythmeMidi)
        Me.GbxPosologie.Controls.Add(Me.LblRythmeMatin)
        Me.GbxPosologie.Controls.Add(Me.NumRythmeSoir)
        Me.GbxPosologie.Controls.Add(Me.NumRythmeApresMidi)
        Me.GbxPosologie.Controls.Add(Me.NumRythmeMidi)
        Me.GbxPosologie.Controls.Add(Me.PgbMiseAJour)
        Me.GbxPosologie.Controls.Add(Me.NumRythmeMatin)
        Me.GbxPosologie.Controls.Add(Me.Label14)
        Me.GbxPosologie.Controls.Add(Me.TxtTraitementPosologieCommentaire)
        Me.GbxPosologie.Controls.Add(Me.LblTraitementRythme)
        Me.GbxPosologie.Controls.Add(Me.LblTraitementBase)
        Me.GbxPosologie.Controls.Add(Me.CbxTraitementBase)
        Me.GbxPosologie.Controls.Add(Me.Label2)
        Me.GbxPosologie.Controls.Add(Me.LblTraitementPosologie)
        Me.GbxPosologie.Location = New System.Drawing.Point(15, 186)
        Me.GbxPosologie.Name = "GbxPosologie"
        Me.GbxPosologie.Size = New System.Drawing.Size(747, 158)
        Me.GbxPosologie.TabIndex = 38
        Me.GbxPosologie.TabStop = False
        Me.GbxPosologie.Text = "Posologie"
        '
        'LblRythmeSoir
        '
        Me.LblRythmeSoir.AutoSize = True
        Me.LblRythmeSoir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRythmeSoir.Location = New System.Drawing.Point(574, 46)
        Me.LblRythmeSoir.Name = "LblRythmeSoir"
        Me.LblRythmeSoir.Size = New System.Drawing.Size(27, 13)
        Me.LblRythmeSoir.TabIndex = 54
        Me.LblRythmeSoir.Text = "soir"
        '
        'LblRythmeApresMidi
        '
        Me.LblRythmeApresMidi.AutoSize = True
        Me.LblRythmeApresMidi.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRythmeApresMidi.Location = New System.Drawing.Point(439, 46)
        Me.LblRythmeApresMidi.Name = "LblRythmeApresMidi"
        Me.LblRythmeApresMidi.Size = New System.Drawing.Size(64, 13)
        Me.LblRythmeApresMidi.TabIndex = 53
        Me.LblRythmeApresMidi.Text = "après-midi"
        '
        'LblRythmeMidi
        '
        Me.LblRythmeMidi.AutoSize = True
        Me.LblRythmeMidi.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRythmeMidi.Location = New System.Drawing.Point(302, 46)
        Me.LblRythmeMidi.Name = "LblRythmeMidi"
        Me.LblRythmeMidi.Size = New System.Drawing.Size(29, 13)
        Me.LblRythmeMidi.TabIndex = 52
        Me.LblRythmeMidi.Text = "midi"
        '
        'LblRythmeMatin
        '
        Me.LblRythmeMatin.AutoSize = True
        Me.LblRythmeMatin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRythmeMatin.Location = New System.Drawing.Point(169, 46)
        Me.LblRythmeMatin.Name = "LblRythmeMatin"
        Me.LblRythmeMatin.Size = New System.Drawing.Size(37, 13)
        Me.LblRythmeMatin.TabIndex = 51
        Me.LblRythmeMatin.Text = "matin"
        '
        'NumRythmeSoir
        '
        Me.NumRythmeSoir.Location = New System.Drawing.Point(522, 44)
        Me.NumRythmeSoir.Name = "NumRythmeSoir"
        Me.NumRythmeSoir.Size = New System.Drawing.Size(46, 20)
        Me.NumRythmeSoir.TabIndex = 50
        '
        'NumRythmeApresMidi
        '
        Me.NumRythmeApresMidi.Location = New System.Drawing.Point(387, 44)
        Me.NumRythmeApresMidi.Name = "NumRythmeApresMidi"
        Me.NumRythmeApresMidi.Size = New System.Drawing.Size(46, 20)
        Me.NumRythmeApresMidi.TabIndex = 49
        '
        'NumRythmeMidi
        '
        Me.NumRythmeMidi.Location = New System.Drawing.Point(252, 44)
        Me.NumRythmeMidi.Name = "NumRythmeMidi"
        Me.NumRythmeMidi.Size = New System.Drawing.Size(46, 20)
        Me.NumRythmeMidi.TabIndex = 48
        '
        'PgbMiseAJour
        '
        Me.PgbMiseAJour.Location = New System.Drawing.Point(300, 15)
        Me.PgbMiseAJour.MarqueeAnimationSpeed = 1
        Me.PgbMiseAJour.Name = "PgbMiseAJour"
        Me.PgbMiseAJour.Size = New System.Drawing.Size(100, 23)
        Me.PgbMiseAJour.Step = 1
        Me.PgbMiseAJour.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.PgbMiseAJour.TabIndex = 47
        '
        'NumRythmeMatin
        '
        Me.NumRythmeMatin.Location = New System.Drawing.Point(117, 44)
        Me.NumRythmeMatin.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.NumRythmeMatin.Name = "NumRythmeMatin"
        Me.NumRythmeMatin.Size = New System.Drawing.Size(46, 20)
        Me.NumRythmeMatin.TabIndex = 2
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(12, 96)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(79, 13)
        Me.Label14.TabIndex = 44
        Me.Label14.Text = "Commentaire"
        '
        'LblTraitementRythme
        '
        Me.LblTraitementRythme.AutoSize = True
        Me.LblTraitementRythme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTraitementRythme.Location = New System.Drawing.Point(12, 46)
        Me.LblTraitementRythme.Name = "LblTraitementRythme"
        Me.LblTraitementRythme.Size = New System.Drawing.Size(49, 13)
        Me.LblTraitementRythme.TabIndex = 38
        Me.LblTraitementRythme.Text = "Rythme"
        '
        'MedicamentGroupBox
        '
        Me.MedicamentGroupBox.BackColor = System.Drawing.SystemColors.Info
        Me.MedicamentGroupBox.Controls.Add(Me.LblMedicamentTitulaire)
        Me.MedicamentGroupBox.Controls.Add(Me.LblMedicamentAdministration)
        Me.MedicamentGroupBox.Controls.Add(Me.LblMedicamentForme)
        Me.MedicamentGroupBox.Controls.Add(Me.Label15)
        Me.MedicamentGroupBox.Controls.Add(Me.Label16)
        Me.MedicamentGroupBox.Controls.Add(Me.Label17)
        Me.MedicamentGroupBox.Controls.Add(Me.Label10)
        Me.MedicamentGroupBox.Controls.Add(Me.Label6)
        Me.MedicamentGroupBox.Controls.Add(Me.LblMedicamentDCI)
        Me.MedicamentGroupBox.Controls.Add(Me.LblTraitementMedicamentCIS)
        Me.MedicamentGroupBox.Location = New System.Drawing.Point(15, 76)
        Me.MedicamentGroupBox.Name = "MedicamentGroupBox"
        Me.MedicamentGroupBox.Size = New System.Drawing.Size(747, 105)
        Me.MedicamentGroupBox.TabIndex = 39
        Me.MedicamentGroupBox.TabStop = False
        Me.MedicamentGroupBox.Text = "Médicament"
        '
        'LblMedicamentTitulaire
        '
        Me.LblMedicamentTitulaire.AutoSize = True
        Me.LblMedicamentTitulaire.Location = New System.Drawing.Point(108, 81)
        Me.LblMedicamentTitulaire.Name = "LblMedicamentTitulaire"
        Me.LblMedicamentTitulaire.Size = New System.Drawing.Size(173, 13)
        Me.LblMedicamentTitulaire.TabIndex = 57
        Me.LblMedicamentTitulaire.Text = "ACCORD HEALTHCARE FRANCE"
        '
        'LblMedicamentAdministration
        '
        Me.LblMedicamentAdministration.AutoSize = True
        Me.LblMedicamentAdministration.Location = New System.Drawing.Point(433, 60)
        Me.LblMedicamentAdministration.Name = "LblMedicamentAdministration"
        Me.LblMedicamentAdministration.Size = New System.Drawing.Size(30, 13)
        Me.LblMedicamentAdministration.TabIndex = 56
        Me.LblMedicamentAdministration.Text = "orale"
        '
        'LblMedicamentForme
        '
        Me.LblMedicamentForme.AutoSize = True
        Me.LblMedicamentForme.Location = New System.Drawing.Point(108, 60)
        Me.LblMedicamentForme.Name = "LblMedicamentForme"
        Me.LblMedicamentForme.Size = New System.Drawing.Size(93, 13)
        Me.LblMedicamentForme.TabIndex = 55
        Me.LblMedicamentForme.Text = "comprime pellicule"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(12, 81)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(61, 13)
        Me.Label15.TabIndex = 54
        Me.Label15.Text = "Titulaire :"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(333, 61)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(94, 13)
        Me.Label16.TabIndex = 53
        Me.Label16.Text = "Administration :"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(12, 60)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(49, 13)
        Me.Label17.TabIndex = 52
        Me.Label17.Text = "Forme :"
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(606, 758)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(75, 23)
        Me.BtnValidation.TabIndex = 40
        Me.BtnValidation.Text = "Valider"
        Me.BtnValidation.UseVisualStyleBackColor = True
        '
        'BtnRetour
        '
        Me.BtnRetour.Location = New System.Drawing.Point(687, 758)
        Me.BtnRetour.Name = "BtnRetour"
        Me.BtnRetour.Size = New System.Drawing.Size(75, 23)
        Me.BtnRetour.TabIndex = 41
        Me.BtnRetour.Text = "Abandonner"
        Me.BtnRetour.UseVisualStyleBackColor = True
        '
        'TxtTraitementCommentaire
        '
        Me.TxtTraitementCommentaire.Location = New System.Drawing.Point(117, 70)
        Me.TxtTraitementCommentaire.MaxLength = 256
        Me.TxtTraitementCommentaire.Multiline = True
        Me.TxtTraitementCommentaire.Name = "TxtTraitementCommentaire"
        Me.TxtTraitementCommentaire.Size = New System.Drawing.Size(457, 59)
        Me.TxtTraitementCommentaire.TabIndex = 42
        '
        'GbxTraitement
        '
        Me.GbxTraitement.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.GbxTraitement.Controls.Add(Me.Label4)
        Me.GbxTraitement.Controls.Add(Me.NumNumeroOrdre)
        Me.GbxTraitement.Controls.Add(Me.LblTraitementDuree)
        Me.GbxTraitement.Controls.Add(Me.LblLabelTraitementDuree)
        Me.GbxTraitement.Controls.Add(Me.DteTraitementDateFin)
        Me.GbxTraitement.Controls.Add(Me.DteTraitementDateDebut)
        Me.GbxTraitement.Controls.Add(Me.TxtTraitementCommentaire)
        Me.GbxTraitement.Controls.Add(Me.LblTraitementDateFin)
        Me.GbxTraitement.Controls.Add(Me.LblTraitementDateDebut)
        Me.GbxTraitement.Controls.Add(Me.Label11)
        Me.GbxTraitement.Location = New System.Drawing.Point(15, 351)
        Me.GbxTraitement.Name = "GbxTraitement"
        Me.GbxTraitement.Size = New System.Drawing.Size(747, 138)
        Me.GbxTraitement.TabIndex = 45
        Me.GbxTraitement.TabStop = False
        Me.GbxTraitement.Text = "Traitement"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(544, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(105, 13)
        Me.Label4.TabIndex = 63
        Me.Label4.Text = "Ordre d'affichage"
        '
        'NumNumeroOrdre
        '
        Me.NumNumeroOrdre.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.NumNumeroOrdre.Location = New System.Drawing.Point(654, 18)
        Me.NumNumeroOrdre.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NumNumeroOrdre.Name = "NumNumeroOrdre"
        Me.NumNumeroOrdre.Size = New System.Drawing.Size(59, 20)
        Me.NumNumeroOrdre.TabIndex = 62
        '
        'LblTraitementDuree
        '
        Me.LblTraitementDuree.AutoSize = True
        Me.LblTraitementDuree.Location = New System.Drawing.Point(450, 33)
        Me.LblTraitementDuree.Name = "LblTraitementDuree"
        Me.LblTraitementDuree.Size = New System.Drawing.Size(44, 13)
        Me.LblTraitementDuree.TabIndex = 61
        Me.LblTraitementDuree.Text = "3 jour(s)"
        '
        'LblLabelTraitementDuree
        '
        Me.LblLabelTraitementDuree.AutoSize = True
        Me.LblLabelTraitementDuree.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelTraitementDuree.Location = New System.Drawing.Point(343, 33)
        Me.LblLabelTraitementDuree.Name = "LblLabelTraitementDuree"
        Me.LblLabelTraitementDuree.Size = New System.Drawing.Size(101, 13)
        Me.LblLabelTraitementDuree.TabIndex = 60
        Me.LblLabelTraitementDuree.Text = "Durée traitement"
        '
        'DteTraitementDateFin
        '
        Me.DteTraitementDateFin.Location = New System.Drawing.Point(117, 42)
        Me.DteTraitementDateFin.Name = "DteTraitementDateFin"
        Me.DteTraitementDateFin.Size = New System.Drawing.Size(200, 20)
        Me.DteTraitementDateFin.TabIndex = 59
        Me.DteTraitementDateFin.Value = New Date(2999, 12, 31, 0, 0, 0, 0)
        '
        'DteTraitementDateDebut
        '
        Me.DteTraitementDateDebut.Location = New System.Drawing.Point(117, 14)
        Me.DteTraitementDateDebut.MinDate = New Date(2000, 1, 1, 0, 0, 0, 0)
        Me.DteTraitementDateDebut.Name = "DteTraitementDateDebut"
        Me.DteTraitementDateDebut.Size = New System.Drawing.Size(200, 20)
        Me.DteTraitementDateDebut.TabIndex = 58
        Me.DteTraitementDateDebut.Value = New Date(2019, 9, 5, 16, 45, 22, 0)
        '
        'BtnSupprimerTraitement
        '
        Me.BtnSupprimerTraitement.Location = New System.Drawing.Point(374, 758)
        Me.BtnSupprimerTraitement.Name = "BtnSupprimerTraitement"
        Me.BtnSupprimerTraitement.Size = New System.Drawing.Size(121, 23)
        Me.BtnSupprimerTraitement.TabIndex = 46
        Me.BtnSupprimerTraitement.Text = "Supprimer traitement"
        Me.BtnSupprimerTraitement.UseVisualStyleBackColor = True
        '
        'BtnArretTraitement
        '
        Me.BtnArretTraitement.Location = New System.Drawing.Point(151, 758)
        Me.BtnArretTraitement.Name = "BtnArretTraitement"
        Me.BtnArretTraitement.Size = New System.Drawing.Size(111, 23)
        Me.BtnArretTraitement.TabIndex = 47
        Me.BtnArretTraitement.Text = "Arret traitement"
        Me.BtnArretTraitement.UseVisualStyleBackColor = True
        '
        'BtnAnnulerTraitement
        '
        Me.BtnAnnulerTraitement.Location = New System.Drawing.Point(268, 758)
        Me.BtnAnnulerTraitement.Name = "BtnAnnulerTraitement"
        Me.BtnAnnulerTraitement.Size = New System.Drawing.Size(100, 23)
        Me.BtnAnnulerTraitement.TabIndex = 48
        Me.BtnAnnulerTraitement.Text = "Annuler traitement"
        Me.BtnAnnulerTraitement.UseVisualStyleBackColor = True
        '
        'GbxArretTraitement
        '
        Me.GbxArretTraitement.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.GbxArretTraitement.Controls.Add(Me.BtnValidationArret)
        Me.GbxArretTraitement.Controls.Add(Me.Label1)
        Me.GbxArretTraitement.Controls.Add(Me.TxtCommentaireArret)
        Me.GbxArretTraitement.Controls.Add(Me.ChkContreIndication)
        Me.GbxArretTraitement.Controls.Add(Me.ChkAllergie)
        Me.GbxArretTraitement.Location = New System.Drawing.Point(15, 496)
        Me.GbxArretTraitement.Name = "GbxArretTraitement"
        Me.GbxArretTraitement.Size = New System.Drawing.Size(747, 94)
        Me.GbxArretTraitement.TabIndex = 49
        Me.GbxArretTraitement.TabStop = False
        Me.GbxArretTraitement.Text = "Arrêt traitement"
        '
        'BtnValidationArret
        '
        Me.BtnValidationArret.BackColor = System.Drawing.Color.Wheat
        Me.BtnValidationArret.Location = New System.Drawing.Point(593, 32)
        Me.BtnValidationArret.Name = "BtnValidationArret"
        Me.BtnValidationArret.Size = New System.Drawing.Size(120, 23)
        Me.BtnValidationArret.TabIndex = 50
        Me.BtnValidationArret.Text = "Validation arrêt"
        Me.BtnValidationArret.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Commentaire"
        '
        'TxtCommentaireArret
        '
        Me.TxtCommentaireArret.Location = New System.Drawing.Point(117, 19)
        Me.TxtCommentaireArret.Multiline = True
        Me.TxtCommentaireArret.Name = "TxtCommentaireArret"
        Me.TxtCommentaireArret.Size = New System.Drawing.Size(457, 47)
        Me.TxtCommentaireArret.TabIndex = 2
        '
        'ChkContreIndication
        '
        Me.ChkContreIndication.AutoSize = True
        Me.ChkContreIndication.Location = New System.Drawing.Point(275, 72)
        Me.ChkContreIndication.Name = "ChkContreIndication"
        Me.ChkContreIndication.Size = New System.Drawing.Size(105, 17)
        Me.ChkContreIndication.TabIndex = 1
        Me.ChkContreIndication.Text = "Contre-indication"
        Me.ChkContreIndication.UseVisualStyleBackColor = True
        '
        'ChkAllergie
        '
        Me.ChkAllergie.AutoSize = True
        Me.ChkAllergie.Location = New System.Drawing.Point(117, 72)
        Me.ChkAllergie.Name = "ChkAllergie"
        Me.ChkAllergie.Size = New System.Drawing.Size(60, 17)
        Me.ChkAllergie.TabIndex = 0
        Me.ChkAllergie.Text = "Allergie"
        Me.ChkAllergie.UseVisualStyleBackColor = True
        '
        'GbxAnnulationTraitement
        '
        Me.GbxAnnulationTraitement.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.GbxAnnulationTraitement.Controls.Add(Me.Label3)
        Me.GbxAnnulationTraitement.Controls.Add(Me.TxtCommentaireAnnulation)
        Me.GbxAnnulationTraitement.Controls.Add(Me.BtnValidationAnnulation)
        Me.GbxAnnulationTraitement.Location = New System.Drawing.Point(16, 596)
        Me.GbxAnnulationTraitement.Name = "GbxAnnulationTraitement"
        Me.GbxAnnulationTraitement.Size = New System.Drawing.Size(746, 60)
        Me.GbxAnnulationTraitement.TabIndex = 50
        Me.GbxAnnulationTraitement.TabStop = False
        Me.GbxAnnulationTraitement.Text = "Annulation"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(5, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Commentaire"
        '
        'TxtCommentaireAnnulation
        '
        Me.TxtCommentaireAnnulation.Location = New System.Drawing.Point(116, 19)
        Me.TxtCommentaireAnnulation.Multiline = True
        Me.TxtCommentaireAnnulation.Name = "TxtCommentaireAnnulation"
        Me.TxtCommentaireAnnulation.Size = New System.Drawing.Size(457, 35)
        Me.TxtCommentaireAnnulation.TabIndex = 1
        '
        'BtnValidationAnnulation
        '
        Me.BtnValidationAnnulation.BackColor = System.Drawing.Color.Wheat
        Me.BtnValidationAnnulation.Location = New System.Drawing.Point(592, 19)
        Me.BtnValidationAnnulation.Name = "BtnValidationAnnulation"
        Me.BtnValidationAnnulation.Size = New System.Drawing.Size(120, 23)
        Me.BtnValidationAnnulation.TabIndex = 0
        Me.BtnValidationAnnulation.Text = "Validation annulation"
        Me.BtnValidationAnnulation.UseVisualStyleBackColor = False
        '
        'GbxFenetreTherapeutique
        '
        Me.GbxFenetreTherapeutique.BackColor = System.Drawing.Color.Wheat
        Me.GbxFenetreTherapeutique.Controls.Add(Me.LblFenetreActive)
        Me.GbxFenetreTherapeutique.Controls.Add(Me.LblFenetreTherapeutique)
        Me.GbxFenetreTherapeutique.Controls.Add(Me.LblFenetreDateDebut)
        Me.GbxFenetreTherapeutique.Controls.Add(Me.LblFenetreTherapeutiqueAu)
        Me.GbxFenetreTherapeutique.Controls.Add(Me.LblFenetreDateFin)
        Me.GbxFenetreTherapeutique.Location = New System.Drawing.Point(15, 698)
        Me.GbxFenetreTherapeutique.Name = "GbxFenetreTherapeutique"
        Me.GbxFenetreTherapeutique.Size = New System.Drawing.Size(747, 43)
        Me.GbxFenetreTherapeutique.TabIndex = 51
        Me.GbxFenetreTherapeutique.TabStop = False
        '
        'LblFenetreActive
        '
        Me.LblFenetreActive.AutoSize = True
        Me.LblFenetreActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFenetreActive.ForeColor = System.Drawing.Color.Red
        Me.LblFenetreActive.Location = New System.Drawing.Point(497, 16)
        Me.LblFenetreActive.Name = "LblFenetreActive"
        Me.LblFenetreActive.Size = New System.Drawing.Size(54, 15)
        Me.LblFenetreActive.TabIndex = 31
        Me.LblFenetreActive.Text = "(Active)"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(171, 667)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(109, 13)
        Me.LblUtilisateurCreation.TabIndex = 52
        Me.LblUtilisateurCreation.Text = "Utilisateur en création"
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(558, 667)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(127, 13)
        Me.LblUtilisateurModification.TabIndex = 53
        Me.LblUtilisateurModification.Text = "Utilisateur en modification"
        '
        'LblLabelTraitementParCreation
        '
        Me.LblLabelTraitementParCreation.AutoSize = True
        Me.LblLabelTraitementParCreation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelTraitementParCreation.Location = New System.Drawing.Point(140, 667)
        Me.LblLabelTraitementParCreation.Name = "LblLabelTraitementParCreation"
        Me.LblLabelTraitementParCreation.Size = New System.Drawing.Size(25, 13)
        Me.LblLabelTraitementParCreation.TabIndex = 54
        Me.LblLabelTraitementParCreation.Text = "par"
        '
        'LblLabelTraitementParModification
        '
        Me.LblLabelTraitementParModification.AutoSize = True
        Me.LblLabelTraitementParModification.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLabelTraitementParModification.Location = New System.Drawing.Point(527, 667)
        Me.LblLabelTraitementParModification.Name = "LblLabelTraitementParModification"
        Me.LblLabelTraitementParModification.Size = New System.Drawing.Size(25, 13)
        Me.LblLabelTraitementParModification.TabIndex = 55
        Me.LblLabelTraitementParModification.Text = "par"
        '
        'LblstatutTraitement
        '
        Me.LblstatutTraitement.AutoSize = True
        Me.LblstatutTraitement.BackColor = System.Drawing.SystemColors.Info
        Me.LblstatutTraitement.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblstatutTraitement.ForeColor = System.Drawing.Color.DarkBlue
        Me.LblstatutTraitement.Location = New System.Drawing.Point(2, 3)
        Me.LblstatutTraitement.Name = "LblstatutTraitement"
        Me.LblstatutTraitement.Size = New System.Drawing.Size(61, 15)
        Me.LblstatutTraitement.TabIndex = 58
        Me.LblstatutTraitement.Text = "Création"
        '
        'PnlStatutTraitement
        '
        Me.PnlStatutTraitement.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlStatutTraitement.Controls.Add(Me.LblstatutTraitement)
        Me.PnlStatutTraitement.Location = New System.Drawing.Point(149, 746)
        Me.PnlStatutTraitement.Name = "PnlStatutTraitement"
        Me.PnlStatutTraitement.Size = New System.Drawing.Size(534, 25)
        Me.PnlStatutTraitement.TabIndex = 59
        '
        'LblId
        '
        Me.LblId.AutoSize = True
        Me.LblId.Location = New System.Drawing.Point(749, 667)
        Me.LblId.Name = "LblId"
        Me.LblId.Size = New System.Drawing.Size(13, 13)
        Me.LblId.TabIndex = 60
        Me.LblId.Text = "?"
        '
        'FTraitementDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(774, 786)
        Me.Controls.Add(Me.LblId)
        Me.Controls.Add(Me.PnlStatutTraitement)
        Me.Controls.Add(Me.LblLabelTraitementParModification)
        Me.Controls.Add(Me.LblLabelTraitementParCreation)
        Me.Controls.Add(Me.LblUtilisateurModification)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.GbxFenetreTherapeutique)
        Me.Controls.Add(Me.GbxAnnulationTraitement)
        Me.Controls.Add(Me.GbxArretTraitement)
        Me.Controls.Add(Me.BtnAnnulerTraitement)
        Me.Controls.Add(Me.BtnArretTraitement)
        Me.Controls.Add(Me.BtnSupprimerTraitement)
        Me.Controls.Add(Me.GbxTraitement)
        Me.Controls.Add(Me.BtnRetour)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.MedicamentGroupBox)
        Me.Controls.Add(Me.GbxPosologie)
        Me.Controls.Add(Me.BtnMedoc)
        Me.Controls.Add(Me.LblLabelTraitementDateCreation)
        Me.Controls.Add(Me.LblTraitementDateCreation)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LblLabelTraitementDateModification)
        Me.Controls.Add(Me.LblTraitementDateModification)
        Me.Name = "FTraitementDetailEdit"
        Me.Text = "Détail traitement"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GbxPosologie.ResumeLayout(False)
        Me.GbxPosologie.PerformLayout()
        CType(Me.NumRythmeSoir, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumRythmeApresMidi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumRythmeMidi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumRythmeMatin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MedicamentGroupBox.ResumeLayout(False)
        Me.MedicamentGroupBox.PerformLayout()
        Me.GbxTraitement.ResumeLayout(False)
        Me.GbxTraitement.PerformLayout()
        CType(Me.NumNumeroOrdre, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GbxArretTraitement.ResumeLayout(False)
        Me.GbxArretTraitement.PerformLayout()
        Me.GbxAnnulationTraitement.ResumeLayout(False)
        Me.GbxAnnulationTraitement.PerformLayout()
        Me.GbxFenetreTherapeutique.ResumeLayout(False)
        Me.GbxFenetreTherapeutique.PerformLayout()
        Me.PnlStatutTraitement.ResumeLayout(False)
        Me.PnlStatutTraitement.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

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
    Friend WithEvents Label2 As Label
    Friend WithEvents LblTraitementDateFin As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents LblFenetreTherapeutique As Label
    Friend WithEvents LblTraitementDateDebut As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents LblLabelTraitementDateCreation As Label
    Friend WithEvents LblTraitementDateCreation As Label
    Friend WithEvents LblLabelTraitementDateModification As Label
    Friend WithEvents LblTraitementDateModification As Label
    Friend WithEvents LblFenetreDateDebut As Label
    Friend WithEvents LblFenetreTherapeutiqueAu As Label
    Friend WithEvents LblFenetreDateFin As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents BtnMedoc As Button
    Friend WithEvents LblTraitementPosologie As Label
    Friend WithEvents LblMedicamentDCI As Label
    Friend WithEvents TxtTraitementPosologieCommentaire As TextBox
    Friend WithEvents LblTraitementMedicamentCIS As Label
    Friend WithEvents CbxTraitementBase As ComboBox
    Friend WithEvents LblTraitementBase As Label
    Friend WithEvents GbxPosologie As GroupBox
    Friend WithEvents Label14 As Label
    Friend WithEvents LblTraitementRythme As Label
    Friend WithEvents MedicamentGroupBox As GroupBox
    Friend WithEvents BtnValidation As Button
    Friend WithEvents BtnRetour As Button
    Friend WithEvents TxtTraitementCommentaire As TextBox
    Friend WithEvents LblMedicamentTitulaire As Label
    Friend WithEvents LblMedicamentAdministration As Label
    Friend WithEvents LblMedicamentForme As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents GbxTraitement As GroupBox
    Friend WithEvents BtnSupprimerTraitement As Button
    Friend WithEvents NumRythmeMatin As NumericUpDown
    Friend WithEvents DteTraitementDateFin As DateTimePicker
    Friend WithEvents DteTraitementDateDebut As DateTimePicker
    Friend WithEvents LblTraitementDuree As Label
    Friend WithEvents LblLabelTraitementDuree As Label
    Friend WithEvents PgbMiseAJour As ProgressBar
    Friend WithEvents BtnArretTraitement As Button
    Friend WithEvents BtnAnnulerTraitement As Button
    Friend WithEvents GbxArretTraitement As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtCommentaireArret As TextBox
    Friend WithEvents ChkContreIndication As CheckBox
    Friend WithEvents ChkAllergie As CheckBox
    Friend WithEvents BtnValidationArret As Button
    Friend WithEvents GbxAnnulationTraitement As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TxtCommentaireAnnulation As TextBox
    Friend WithEvents BtnValidationAnnulation As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents NumNumeroOrdre As NumericUpDown
    Friend WithEvents GbxFenetreTherapeutique As GroupBox
    Friend WithEvents LblFenetreActive As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents LblLabelTraitementParCreation As Label
    Friend WithEvents LblLabelTraitementParModification As Label
    Friend WithEvents PnlStatutTraitement As Panel
    Friend WithEvents LblstatutTraitement As Label
    Friend WithEvents LblId As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents LblRythmeSoir As Label
    Friend WithEvents LblRythmeApresMidi As Label
    Friend WithEvents LblRythmeMidi As Label
    Friend WithEvents LblRythmeMatin As Label
    Friend WithEvents NumRythmeSoir As NumericUpDown
    Friend WithEvents NumRythmeApresMidi As NumericUpDown
    Friend WithEvents NumRythmeMidi As NumericUpDown
End Class
