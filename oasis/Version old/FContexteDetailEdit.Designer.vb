<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FContexteDetailEdit
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
        Me.TxtDrcId = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblDrcDenomination = New System.Windows.Forms.Label()
        Me.TxtContexteDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DteDateDebut = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnRecupereDrc = New System.Windows.Forms.Button()
        Me.DtnAbandon = New System.Windows.Forms.Button()
        Me.BtnValider = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtArretCommentaire = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.LblModificationContexte2 = New System.Windows.Forms.Label()
        Me.LblCreationContexte2 = New System.Windows.Forms.Label()
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblContexteDateCreation = New System.Windows.Forms.Label()
        Me.LblContexteDateModification = New System.Windows.Forms.Label()
        Me.BtnSupprimer = New System.Windows.Forms.Button()
        Me.ChkPublie = New System.Windows.Forms.CheckBox()
        Me.ChkCache = New System.Windows.Forms.CheckBox()
        Me.BtnTransformer = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GbxArret = New System.Windows.Forms.GroupBox()
        Me.BtnValidationArret = New System.Windows.Forms.Button()
        Me.GbxStatutAffichage = New System.Windows.Forms.GroupBox()
        Me.LblPublication = New System.Windows.Forms.Label()
        Me.BtnValidationPublication = New System.Windows.Forms.Button()
        Me.GbxPrincipal = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.ChkDiagnosticNotion = New System.Windows.Forms.CheckBox()
        Me.ChkDiagnosticSuspecte = New System.Windows.Forms.CheckBox()
        Me.ChkDiagnosticConfirme = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.CbxCategorieContexte = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.NumOrdreAffichage = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.DteDateFin = New System.Windows.Forms.DateTimePicker()
        Me.BtnModifier = New System.Windows.Forms.Button()
        Me.BtnPublication = New System.Windows.Forms.Button()
        Me.BtnArret = New System.Windows.Forms.Button()
        Me.LblCreationContexte1 = New System.Windows.Forms.Label()
        Me.LblModificationContexte1 = New System.Windows.Forms.Label()
        Me.BtnValidationCreationContexte = New System.Windows.Forms.Button()
        Me.LblId = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GbxArret.SuspendLayout()
        Me.GbxStatutAffichage.SuspendLayout()
        Me.GbxPrincipal.SuspendLayout()
        CType(Me.NumOrdreAffichage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.GroupBox1.Size = New System.Drawing.Size(747, 64)
        Me.GroupBox1.TabIndex = 34
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
        'TxtDrcId
        '
        Me.TxtDrcId.Location = New System.Drawing.Point(128, 45)
        Me.TxtDrcId.Name = "TxtDrcId"
        Me.TxtDrcId.Size = New System.Drawing.Size(53, 20)
        Me.TxtDrcId.TabIndex = 35
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "Code DRC/ORC"
        '
        'LblDrcDenomination
        '
        Me.LblDrcDenomination.AutoSize = True
        Me.LblDrcDenomination.Location = New System.Drawing.Point(192, 48)
        Me.LblDrcDenomination.Name = "LblDrcDenomination"
        Me.LblDrcDenomination.Size = New System.Drawing.Size(126, 13)
        Me.LblDrcDenomination.TabIndex = 37
        Me.LblDrcDenomination.Text = "Dénomination ORC/DRC"
        '
        'TxtContexteDescription
        '
        Me.TxtContexteDescription.Location = New System.Drawing.Point(128, 72)
        Me.TxtContexteDescription.Multiline = True
        Me.TxtContexteDescription.Name = "TxtContexteDescription"
        Me.TxtContexteDescription.Size = New System.Drawing.Size(615, 45)
        Me.TxtContexteDescription.TabIndex = 38
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 39
        Me.Label3.Text = "Description"
        '
        'DteDateDebut
        '
        Me.DteDateDebut.Location = New System.Drawing.Point(128, 123)
        Me.DteDateDebut.Name = "DteDateDebut"
        Me.DteDateDebut.Size = New System.Drawing.Size(200, 20)
        Me.DteDateDebut.TabIndex = 40
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(2, 129)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Date début"
        '
        'BtnRecupereDrc
        '
        Me.BtnRecupereDrc.Location = New System.Drawing.Point(397, 125)
        Me.BtnRecupereDrc.Name = "BtnRecupereDrc"
        Me.BtnRecupereDrc.Size = New System.Drawing.Size(143, 23)
        Me.BtnRecupereDrc.TabIndex = 44
        Me.BtnRecupereDrc.Text = "Copier dénomination DRC"
        Me.ToolTip1.SetToolTip(Me.BtnRecupereDrc, "Alimente la description avec la dénomination de la DRC." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Si la description est dé" &
        "jà saisie, celle-ci sera remplacée par la dénomination.")
        Me.BtnRecupereDrc.UseVisualStyleBackColor = True
        '
        'DtnAbandon
        '
        Me.DtnAbandon.Location = New System.Drawing.Point(684, 498)
        Me.DtnAbandon.Name = "DtnAbandon"
        Me.DtnAbandon.Size = New System.Drawing.Size(75, 23)
        Me.DtnAbandon.TabIndex = 45
        Me.DtnAbandon.Text = "Abandonner"
        Me.DtnAbandon.UseVisualStyleBackColor = True
        '
        'BtnValider
        '
        Me.BtnValider.BackColor = System.Drawing.Color.Wheat
        Me.BtnValider.Location = New System.Drawing.Point(618, 149)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(94, 23)
        Me.BtnValider.TabIndex = 46
        Me.BtnValider.Text = "Validation"
        Me.BtnValider.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(2, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 13)
        Me.Label2.TabIndex = 47
        Me.Label2.Text = "Statut affichage"
        '
        'TxtArretCommentaire
        '
        Me.TxtArretCommentaire.Location = New System.Drawing.Point(126, 16)
        Me.TxtArretCommentaire.Multiline = True
        Me.TxtArretCommentaire.Name = "TxtArretCommentaire"
        Me.TxtArretCommentaire.Size = New System.Drawing.Size(464, 60)
        Me.TxtArretCommentaire.TabIndex = 49
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(0, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(109, 13)
        Me.Label8.TabIndex = 50
        Me.Label8.Text = "Commentaire arrêt"
        '
        'LblModificationContexte2
        '
        Me.LblModificationContexte2.AutoSize = True
        Me.LblModificationContexte2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblModificationContexte2.Location = New System.Drawing.Point(458, 467)
        Me.LblModificationContexte2.Name = "LblModificationContexte2"
        Me.LblModificationContexte2.Size = New System.Drawing.Size(25, 13)
        Me.LblModificationContexte2.TabIndex = 63
        Me.LblModificationContexte2.Text = "par"
        '
        'LblCreationContexte2
        '
        Me.LblCreationContexte2.AutoSize = True
        Me.LblCreationContexte2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCreationContexte2.Location = New System.Drawing.Point(138, 467)
        Me.LblCreationContexte2.Name = "LblCreationContexte2"
        Me.LblCreationContexte2.Size = New System.Drawing.Size(25, 13)
        Me.LblCreationContexte2.TabIndex = 62
        Me.LblCreationContexte2.Text = "par"
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(489, 467)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(127, 13)
        Me.LblUtilisateurModification.TabIndex = 61
        Me.LblUtilisateurModification.Text = "Utilisateur en modification"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(169, 467)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(109, 13)
        Me.LblUtilisateurCreation.TabIndex = 60
        Me.LblUtilisateurCreation.Text = "Utilisateur en création"
        '
        'LblContexteDateCreation
        '
        Me.LblContexteDateCreation.AutoSize = True
        Me.LblContexteDateCreation.Location = New System.Drawing.Point(71, 467)
        Me.LblContexteDateCreation.Name = "LblContexteDateCreation"
        Me.LblContexteDateCreation.Size = New System.Drawing.Size(61, 13)
        Me.LblContexteDateCreation.TabIndex = 57
        Me.LblContexteDateCreation.Text = "01.10.2019"
        '
        'LblContexteDateModification
        '
        Me.LblContexteDateModification.AutoSize = True
        Me.LblContexteDateModification.Location = New System.Drawing.Point(391, 467)
        Me.LblContexteDateModification.Name = "LblContexteDateModification"
        Me.LblContexteDateModification.Size = New System.Drawing.Size(61, 13)
        Me.LblContexteDateModification.TabIndex = 59
        Me.LblContexteDateModification.Text = "02.10.2019"
        '
        'BtnSupprimer
        '
        Me.BtnSupprimer.Location = New System.Drawing.Point(298, 498)
        Me.BtnSupprimer.Name = "BtnSupprimer"
        Me.BtnSupprimer.Size = New System.Drawing.Size(75, 23)
        Me.BtnSupprimer.TabIndex = 64
        Me.BtnSupprimer.Text = "Supprimer"
        Me.BtnSupprimer.UseVisualStyleBackColor = True
        '
        'ChkPublie
        '
        Me.ChkPublie.AutoSize = True
        Me.ChkPublie.Location = New System.Drawing.Point(128, 16)
        Me.ChkPublie.Name = "ChkPublie"
        Me.ChkPublie.Size = New System.Drawing.Size(55, 17)
        Me.ChkPublie.TabIndex = 65
        Me.ChkPublie.Text = "Publié"
        Me.ToolTip1.SetToolTip(Me.ChkPublie, "Un antécédent publié est visible par défaut dans l'outil de synthèse.")
        Me.ChkPublie.UseVisualStyleBackColor = True
        '
        'ChkCache
        '
        Me.ChkCache.AutoSize = True
        Me.ChkCache.Location = New System.Drawing.Point(247, 16)
        Me.ChkCache.Name = "ChkCache"
        Me.ChkCache.Size = New System.Drawing.Size(57, 17)
        Me.ChkCache.TabIndex = 66
        Me.ChkCache.Text = "Caché"
        Me.ToolTip1.SetToolTip(Me.ChkCache, "Un antécédent caché est visible dans l'outil de synthèse sur la demande de l'util" &
        "isateur.")
        Me.ChkCache.UseVisualStyleBackColor = True
        '
        'BtnTransformer
        '
        Me.BtnTransformer.Location = New System.Drawing.Point(379, 498)
        Me.BtnTransformer.Name = "BtnTransformer"
        Me.BtnTransformer.Size = New System.Drawing.Size(75, 23)
        Me.BtnTransformer.TabIndex = 68
        Me.BtnTransformer.Text = "Transformer"
        Me.ToolTip1.SetToolTip(Me.BtnTransformer, "La réactivation d'un antécédent, réinitialise cet antécédent dans l'état de conte" &
        "xte médical duquel il est issu.")
        Me.BtnTransformer.UseVisualStyleBackColor = True
        '
        'GbxArret
        '
        Me.GbxArret.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.GbxArret.Controls.Add(Me.BtnValidationArret)
        Me.GbxArret.Controls.Add(Me.Label8)
        Me.GbxArret.Controls.Add(Me.TxtArretCommentaire)
        Me.GbxArret.Location = New System.Drawing.Point(12, 364)
        Me.GbxArret.Name = "GbxArret"
        Me.GbxArret.Size = New System.Drawing.Size(747, 88)
        Me.GbxArret.TabIndex = 69
        Me.GbxArret.TabStop = False
        Me.GbxArret.Text = "Arrêt"
        '
        'BtnValidationArret
        '
        Me.BtnValidationArret.BackColor = System.Drawing.Color.Wheat
        Me.BtnValidationArret.Location = New System.Drawing.Point(618, 34)
        Me.BtnValidationArret.Name = "BtnValidationArret"
        Me.BtnValidationArret.Size = New System.Drawing.Size(99, 23)
        Me.BtnValidationArret.TabIndex = 51
        Me.BtnValidationArret.Text = "Validation arrêt"
        Me.BtnValidationArret.UseVisualStyleBackColor = False
        '
        'GbxStatutAffichage
        '
        Me.GbxStatutAffichage.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.GbxStatutAffichage.Controls.Add(Me.LblPublication)
        Me.GbxStatutAffichage.Controls.Add(Me.BtnValidationPublication)
        Me.GbxStatutAffichage.Controls.Add(Me.ChkCache)
        Me.GbxStatutAffichage.Controls.Add(Me.ChkPublie)
        Me.GbxStatutAffichage.Controls.Add(Me.Label2)
        Me.GbxStatutAffichage.Location = New System.Drawing.Point(12, 309)
        Me.GbxStatutAffichage.Name = "GbxStatutAffichage"
        Me.GbxStatutAffichage.Size = New System.Drawing.Size(747, 44)
        Me.GbxStatutAffichage.TabIndex = 70
        Me.GbxStatutAffichage.TabStop = False
        Me.GbxStatutAffichage.Text = "Publication"
        '
        'LblPublication
        '
        Me.LblPublication.AutoSize = True
        Me.LblPublication.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPublication.ForeColor = System.Drawing.Color.DarkBlue
        Me.LblPublication.Location = New System.Drawing.Point(477, 17)
        Me.LblPublication.Name = "LblPublication"
        Me.LblPublication.Size = New System.Drawing.Size(95, 13)
        Me.LblPublication.TabIndex = 69
        Me.LblPublication.Text = "Contexte publié"
        '
        'BtnValidationPublication
        '
        Me.BtnValidationPublication.BackColor = System.Drawing.Color.Wheat
        Me.BtnValidationPublication.Location = New System.Drawing.Point(618, 12)
        Me.BtnValidationPublication.Name = "BtnValidationPublication"
        Me.BtnValidationPublication.Size = New System.Drawing.Size(118, 23)
        Me.BtnValidationPublication.TabIndex = 68
        Me.BtnValidationPublication.Text = "Validation publication"
        Me.BtnValidationPublication.UseVisualStyleBackColor = False
        '
        'GbxPrincipal
        '
        Me.GbxPrincipal.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.GbxPrincipal.Controls.Add(Me.Label12)
        Me.GbxPrincipal.Controls.Add(Me.ChkDiagnosticNotion)
        Me.GbxPrincipal.Controls.Add(Me.ChkDiagnosticSuspecte)
        Me.GbxPrincipal.Controls.Add(Me.ChkDiagnosticConfirme)
        Me.GbxPrincipal.Controls.Add(Me.Label11)
        Me.GbxPrincipal.Controls.Add(Me.CbxCategorieContexte)
        Me.GbxPrincipal.Controls.Add(Me.Label10)
        Me.GbxPrincipal.Controls.Add(Me.NumOrdreAffichage)
        Me.GbxPrincipal.Controls.Add(Me.Label6)
        Me.GbxPrincipal.Controls.Add(Me.DteDateFin)
        Me.GbxPrincipal.Controls.Add(Me.BtnRecupereDrc)
        Me.GbxPrincipal.Controls.Add(Me.Label4)
        Me.GbxPrincipal.Controls.Add(Me.DteDateDebut)
        Me.GbxPrincipal.Controls.Add(Me.Label3)
        Me.GbxPrincipal.Controls.Add(Me.TxtContexteDescription)
        Me.GbxPrincipal.Controls.Add(Me.LblDrcDenomination)
        Me.GbxPrincipal.Controls.Add(Me.Label1)
        Me.GbxPrincipal.Controls.Add(Me.TxtDrcId)
        Me.GbxPrincipal.Controls.Add(Me.BtnValider)
        Me.GbxPrincipal.Location = New System.Drawing.Point(12, 84)
        Me.GbxPrincipal.Name = "GbxPrincipal"
        Me.GbxPrincipal.Size = New System.Drawing.Size(747, 216)
        Me.GbxPrincipal.TabIndex = 71
        Me.GbxPrincipal.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(2, 184)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(67, 13)
        Me.Label12.TabIndex = 57
        Me.Label12.Text = "Diagnostic"
        '
        'ChkDiagnosticNotion
        '
        Me.ChkDiagnosticNotion.AutoSize = True
        Me.ChkDiagnosticNotion.Location = New System.Drawing.Point(366, 183)
        Me.ChkDiagnosticNotion.Name = "ChkDiagnosticNotion"
        Me.ChkDiagnosticNotion.Size = New System.Drawing.Size(72, 17)
        Me.ChkDiagnosticNotion.TabIndex = 56
        Me.ChkDiagnosticNotion.Text = "Notion de"
        Me.ChkDiagnosticNotion.UseVisualStyleBackColor = True
        '
        'ChkDiagnosticSuspecte
        '
        Me.ChkDiagnosticSuspecte.AutoSize = True
        Me.ChkDiagnosticSuspecte.Location = New System.Drawing.Point(247, 183)
        Me.ChkDiagnosticSuspecte.Name = "ChkDiagnosticSuspecte"
        Me.ChkDiagnosticSuspecte.Size = New System.Drawing.Size(71, 17)
        Me.ChkDiagnosticSuspecte.TabIndex = 54
        Me.ChkDiagnosticSuspecte.Text = "Suspecté"
        Me.ChkDiagnosticSuspecte.UseVisualStyleBackColor = True
        '
        'ChkDiagnosticConfirme
        '
        Me.ChkDiagnosticConfirme.AutoSize = True
        Me.ChkDiagnosticConfirme.Location = New System.Drawing.Point(128, 183)
        Me.ChkDiagnosticConfirme.Name = "ChkDiagnosticConfirme"
        Me.ChkDiagnosticConfirme.Size = New System.Drawing.Size(67, 17)
        Me.ChkDiagnosticConfirme.TabIndex = 53
        Me.ChkDiagnosticConfirme.Text = "Confirmé"
        Me.ChkDiagnosticConfirme.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(2, 21)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(61, 13)
        Me.Label11.TabIndex = 52
        Me.Label11.Text = "Catégorie"
        '
        'CbxCategorieContexte
        '
        Me.CbxCategorieContexte.AutoCompleteCustomSource.AddRange(New String() {"Médical", "Bio-environnental"})
        Me.CbxCategorieContexte.FormattingEnabled = True
        Me.CbxCategorieContexte.Items.AddRange(New Object() {"Médical", "Bio-environnemental"})
        Me.CbxCategorieContexte.Location = New System.Drawing.Point(128, 18)
        Me.CbxCategorieContexte.Name = "CbxCategorieContexte"
        Me.CbxCategorieContexte.Size = New System.Drawing.Size(121, 21)
        Me.CbxCategorieContexte.TabIndex = 51
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(519, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(105, 13)
        Me.Label10.TabIndex = 50
        Me.Label10.Text = "Ordre d'affichage"
        Me.Label10.Visible = False
        '
        'NumOrdreAffichage
        '
        Me.NumOrdreAffichage.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.NumOrdreAffichage.Location = New System.Drawing.Point(645, 11)
        Me.NumOrdreAffichage.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NumOrdreAffichage.Name = "NumOrdreAffichage"
        Me.NumOrdreAffichage.Size = New System.Drawing.Size(75, 20)
        Me.NumOrdreAffichage.TabIndex = 49
        Me.NumOrdreAffichage.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(3, 156)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 13)
        Me.Label6.TabIndex = 48
        Me.Label6.Text = "Date de fin"
        '
        'DteDateFin
        '
        Me.DteDateFin.Location = New System.Drawing.Point(128, 149)
        Me.DteDateFin.Name = "DteDateFin"
        Me.DteDateFin.Size = New System.Drawing.Size(200, 20)
        Me.DteDateFin.TabIndex = 47
        '
        'BtnModifier
        '
        Me.BtnModifier.Location = New System.Drawing.Point(12, 498)
        Me.BtnModifier.Name = "BtnModifier"
        Me.BtnModifier.Size = New System.Drawing.Size(75, 23)
        Me.BtnModifier.TabIndex = 72
        Me.BtnModifier.Text = "Modifier"
        Me.BtnModifier.UseVisualStyleBackColor = True
        '
        'BtnPublication
        '
        Me.BtnPublication.Location = New System.Drawing.Point(93, 498)
        Me.BtnPublication.Name = "BtnPublication"
        Me.BtnPublication.Size = New System.Drawing.Size(75, 23)
        Me.BtnPublication.TabIndex = 73
        Me.BtnPublication.Text = "Publication"
        Me.BtnPublication.UseVisualStyleBackColor = True
        '
        'BtnArret
        '
        Me.BtnArret.Location = New System.Drawing.Point(174, 498)
        Me.BtnArret.Name = "BtnArret"
        Me.BtnArret.Size = New System.Drawing.Size(75, 23)
        Me.BtnArret.TabIndex = 74
        Me.BtnArret.Text = "Arrêter"
        Me.BtnArret.UseVisualStyleBackColor = True
        '
        'LblCreationContexte1
        '
        Me.LblCreationContexte1.AutoSize = True
        Me.LblCreationContexte1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCreationContexte1.Location = New System.Drawing.Point(9, 467)
        Me.LblCreationContexte1.Name = "LblCreationContexte1"
        Me.LblCreationContexte1.Size = New System.Drawing.Size(55, 13)
        Me.LblCreationContexte1.TabIndex = 56
        Me.LblCreationContexte1.Text = "Créé le :"
        '
        'LblModificationContexte1
        '
        Me.LblModificationContexte1.AutoSize = True
        Me.LblModificationContexte1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblModificationContexte1.Location = New System.Drawing.Point(322, 467)
        Me.LblModificationContexte1.Name = "LblModificationContexte1"
        Me.LblModificationContexte1.Size = New System.Drawing.Size(70, 13)
        Me.LblModificationContexte1.TabIndex = 75
        Me.LblModificationContexte1.Text = "Modifié le :"
        '
        'BtnValidationCreationContexte
        '
        Me.BtnValidationCreationContexte.Location = New System.Drawing.Point(603, 498)
        Me.BtnValidationCreationContexte.Name = "BtnValidationCreationContexte"
        Me.BtnValidationCreationContexte.Size = New System.Drawing.Size(75, 23)
        Me.BtnValidationCreationContexte.TabIndex = 76
        Me.BtnValidationCreationContexte.Text = "Validation"
        Me.BtnValidationCreationContexte.UseVisualStyleBackColor = True
        '
        'LblId
        '
        Me.LblId.AutoSize = True
        Me.LblId.Location = New System.Drawing.Point(746, 467)
        Me.LblId.Name = "LblId"
        Me.LblId.Size = New System.Drawing.Size(13, 13)
        Me.LblId.TabIndex = 77
        Me.LblId.Text = "?"
        '
        'FContexteDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(772, 529)
        Me.Controls.Add(Me.LblId)
        Me.Controls.Add(Me.BtnValidationCreationContexte)
        Me.Controls.Add(Me.LblModificationContexte1)
        Me.Controls.Add(Me.BtnArret)
        Me.Controls.Add(Me.BtnPublication)
        Me.Controls.Add(Me.BtnModifier)
        Me.Controls.Add(Me.GbxPrincipal)
        Me.Controls.Add(Me.GbxStatutAffichage)
        Me.Controls.Add(Me.GbxArret)
        Me.Controls.Add(Me.BtnTransformer)
        Me.Controls.Add(Me.BtnSupprimer)
        Me.Controls.Add(Me.LblModificationContexte2)
        Me.Controls.Add(Me.LblCreationContexte2)
        Me.Controls.Add(Me.LblUtilisateurModification)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.LblCreationContexte1)
        Me.Controls.Add(Me.LblContexteDateCreation)
        Me.Controls.Add(Me.LblContexteDateModification)
        Me.Controls.Add(Me.DtnAbandon)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FContexteDetailEdit"
        Me.Text = "Contexte"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GbxArret.ResumeLayout(False)
        Me.GbxArret.PerformLayout()
        Me.GbxStatutAffichage.ResumeLayout(False)
        Me.GbxStatutAffichage.PerformLayout()
        Me.GbxPrincipal.ResumeLayout(False)
        Me.GbxPrincipal.PerformLayout()
        CType(Me.NumOrdreAffichage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

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
    Friend WithEvents TxtDrcId As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents LblDrcDenomination As Label
    Friend WithEvents TxtContexteDescription As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents DteDateDebut As DateTimePicker
    Friend WithEvents Label4 As Label
    Friend WithEvents BtnRecupereDrc As Button
    Friend WithEvents DtnAbandon As Button
    Friend WithEvents BtnValider As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents TxtArretCommentaire As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents LblModificationContexte2 As Label
    Friend WithEvents LblCreationContexte2 As Label
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblContexteDateCreation As Label
    Friend WithEvents LblContexteDateModification As Label
    Friend WithEvents BtnSupprimer As Button
    Friend WithEvents ChkPublie As CheckBox
    Friend WithEvents ChkCache As CheckBox
    Friend WithEvents BtnTransformer As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents GbxArret As GroupBox
    Friend WithEvents BtnValidationArret As Button
    Friend WithEvents GbxStatutAffichage As GroupBox
    Friend WithEvents GbxPrincipal As GroupBox
    Friend WithEvents BtnValidationPublication As Button
    Friend WithEvents BtnModifier As Button
    Friend WithEvents BtnPublication As Button
    Friend WithEvents BtnArret As Button
    Friend WithEvents LblCreationContexte1 As Label
    Friend WithEvents LblModificationContexte1 As Label
    Friend WithEvents BtnValidationCreationContexte As Button
    Friend WithEvents LblPublication As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents NumOrdreAffichage As NumericUpDown
    Friend WithEvents Label6 As Label
    Friend WithEvents DteDateFin As DateTimePicker
    Friend WithEvents Label11 As Label
    Friend WithEvents CbxCategorieContexte As ComboBox
    Friend WithEvents LblId As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents ChkDiagnosticNotion As CheckBox
    Friend WithEvents ChkDiagnosticSuspecte As CheckBox
    Friend WithEvents ChkDiagnosticConfirme As CheckBox
End Class
