<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FAntecedentDetailEdit
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FAntecedentDetailEdit))
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
        Me.TxtAntecedentDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DteDateDebut = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnRecupereDrc = New System.Windows.Forms.Button()
        Me.DtnAbandon = New System.Windows.Forms.Button()
        Me.BtnValider = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblModificationAntecedent2 = New System.Windows.Forms.Label()
        Me.LblCreationAntecedent2 = New System.Windows.Forms.Label()
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblAntecedentDateCreation = New System.Windows.Forms.Label()
        Me.LblAntecedentDateModification = New System.Windows.Forms.Label()
        Me.BtnSupprimer = New System.Windows.Forms.Button()
        Me.ChkPublie = New System.Windows.Forms.CheckBox()
        Me.ChkCache = New System.Windows.Forms.CheckBox()
        Me.ChkOcculte = New System.Windows.Forms.CheckBox()
        Me.BtnReactiver = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BtnPublication = New System.Windows.Forms.Button()
        Me.GbxStatutAffichage = New System.Windows.Forms.GroupBox()
        Me.LblPublication = New System.Windows.Forms.Label()
        Me.BtnValidationPublication = New System.Windows.Forms.Button()
        Me.GbxPrincipal = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ChkDiagnosticNotion = New System.Windows.Forms.CheckBox()
        Me.ChkDiagnosticSuspecte = New System.Windows.Forms.CheckBox()
        Me.ChkDiagnosticConfirme = New System.Windows.Forms.CheckBox()
        Me.BtnModifier = New System.Windows.Forms.Button()
        Me.LblCreationAntecedent1 = New System.Windows.Forms.Label()
        Me.LblModificationAntecedent1 = New System.Windows.Forms.Label()
        Me.BtnValidationCreationAntecedent = New System.Windows.Forms.Button()
        Me.LblId = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GbxStatutAffichage.SuspendLayout()
        Me.GbxPrincipal.SuspendLayout()
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
        Me.TxtDrcId.Location = New System.Drawing.Point(128, 11)
        Me.TxtDrcId.Name = "TxtDrcId"
        Me.TxtDrcId.Size = New System.Drawing.Size(53, 20)
        Me.TxtDrcId.TabIndex = 35
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "Code DRC/ORC"
        '
        'LblDrcDenomination
        '
        Me.LblDrcDenomination.AutoSize = True
        Me.LblDrcDenomination.Location = New System.Drawing.Point(192, 14)
        Me.LblDrcDenomination.Name = "LblDrcDenomination"
        Me.LblDrcDenomination.Size = New System.Drawing.Size(126, 13)
        Me.LblDrcDenomination.TabIndex = 37
        Me.LblDrcDenomination.Text = "Dénomination ORC/DRC"
        '
        'TxtAntecedentDescription
        '
        Me.TxtAntecedentDescription.Location = New System.Drawing.Point(128, 38)
        Me.TxtAntecedentDescription.Multiline = True
        Me.TxtAntecedentDescription.Name = "TxtAntecedentDescription"
        Me.TxtAntecedentDescription.Size = New System.Drawing.Size(615, 45)
        Me.TxtAntecedentDescription.TabIndex = 38
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 39
        Me.Label3.Text = "Description"
        '
        'DteDateDebut
        '
        Me.DteDateDebut.Location = New System.Drawing.Point(128, 92)
        Me.DteDateDebut.Name = "DteDateDebut"
        Me.DteDateDebut.Size = New System.Drawing.Size(200, 20)
        Me.DteDateDebut.TabIndex = 40
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(2, 98)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Date début"
        '
        'BtnRecupereDrc
        '
        Me.BtnRecupereDrc.Location = New System.Drawing.Point(397, 91)
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
        Me.DtnAbandon.Location = New System.Drawing.Point(684, 344)
        Me.DtnAbandon.Name = "DtnAbandon"
        Me.DtnAbandon.Size = New System.Drawing.Size(75, 23)
        Me.DtnAbandon.TabIndex = 45
        Me.DtnAbandon.Text = "Abandonner"
        Me.DtnAbandon.UseVisualStyleBackColor = True
        '
        'BtnValider
        '
        Me.BtnValider.BackColor = System.Drawing.Color.Wheat
        Me.BtnValider.Location = New System.Drawing.Point(618, 86)
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
        'LblModificationAntecedent2
        '
        Me.LblModificationAntecedent2.AutoSize = True
        Me.LblModificationAntecedent2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblModificationAntecedent2.Location = New System.Drawing.Point(458, 313)
        Me.LblModificationAntecedent2.Name = "LblModificationAntecedent2"
        Me.LblModificationAntecedent2.Size = New System.Drawing.Size(25, 13)
        Me.LblModificationAntecedent2.TabIndex = 63
        Me.LblModificationAntecedent2.Text = "par"
        '
        'LblCreationAntecedent2
        '
        Me.LblCreationAntecedent2.AutoSize = True
        Me.LblCreationAntecedent2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCreationAntecedent2.Location = New System.Drawing.Point(138, 313)
        Me.LblCreationAntecedent2.Name = "LblCreationAntecedent2"
        Me.LblCreationAntecedent2.Size = New System.Drawing.Size(25, 13)
        Me.LblCreationAntecedent2.TabIndex = 62
        Me.LblCreationAntecedent2.Text = "par"
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(489, 313)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(127, 13)
        Me.LblUtilisateurModification.TabIndex = 61
        Me.LblUtilisateurModification.Text = "Utilisateur en modification"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(169, 313)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(109, 13)
        Me.LblUtilisateurCreation.TabIndex = 60
        Me.LblUtilisateurCreation.Text = "Utilisateur en création"
        '
        'LblAntecedentDateCreation
        '
        Me.LblAntecedentDateCreation.AutoSize = True
        Me.LblAntecedentDateCreation.Location = New System.Drawing.Point(71, 313)
        Me.LblAntecedentDateCreation.Name = "LblAntecedentDateCreation"
        Me.LblAntecedentDateCreation.Size = New System.Drawing.Size(61, 13)
        Me.LblAntecedentDateCreation.TabIndex = 57
        Me.LblAntecedentDateCreation.Text = "01.10.2019"
        '
        'LblAntecedentDateModification
        '
        Me.LblAntecedentDateModification.AutoSize = True
        Me.LblAntecedentDateModification.Location = New System.Drawing.Point(391, 313)
        Me.LblAntecedentDateModification.Name = "LblAntecedentDateModification"
        Me.LblAntecedentDateModification.Size = New System.Drawing.Size(61, 13)
        Me.LblAntecedentDateModification.TabIndex = 59
        Me.LblAntecedentDateModification.Text = "02.10.2019"
        '
        'BtnSupprimer
        '
        Me.BtnSupprimer.Location = New System.Drawing.Point(255, 344)
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
        'ChkOcculte
        '
        Me.ChkOcculte.AutoSize = True
        Me.ChkOcculte.Location = New System.Drawing.Point(366, 16)
        Me.ChkOcculte.Name = "ChkOcculte"
        Me.ChkOcculte.Size = New System.Drawing.Size(63, 17)
        Me.ChkOcculte.TabIndex = 67
        Me.ChkOcculte.Text = "Occulté"
        Me.ToolTip1.SetToolTip(Me.ChkOcculte, "Un antécédent occulté n'est plus visible." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "les épisodes de soin qui lui sont asso" &
        "ciés sont visibles dans la ligne de vie du patient.")
        Me.ChkOcculte.UseVisualStyleBackColor = True
        '
        'BtnReactiver
        '
        Me.BtnReactiver.Location = New System.Drawing.Point(174, 344)
        Me.BtnReactiver.Name = "BtnReactiver"
        Me.BtnReactiver.Size = New System.Drawing.Size(75, 23)
        Me.BtnReactiver.TabIndex = 68
        Me.BtnReactiver.Text = "Réactiver"
        Me.ToolTip1.SetToolTip(Me.BtnReactiver, "La réactivation d'un antécédent, réinitialise cet antécédent dans l'état du conte" &
        "xte dont il est issu.")
        Me.BtnReactiver.UseVisualStyleBackColor = True
        '
        'BtnPublication
        '
        Me.BtnPublication.Location = New System.Drawing.Point(93, 344)
        Me.BtnPublication.Name = "BtnPublication"
        Me.BtnPublication.Size = New System.Drawing.Size(75, 23)
        Me.BtnPublication.TabIndex = 73
        Me.BtnPublication.Text = "Publication"
        Me.ToolTip1.SetToolTip(Me.BtnPublication, resources.GetString("BtnPublication.ToolTip"))
        Me.BtnPublication.UseVisualStyleBackColor = True
        '
        'GbxStatutAffichage
        '
        Me.GbxStatutAffichage.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.GbxStatutAffichage.Controls.Add(Me.LblPublication)
        Me.GbxStatutAffichage.Controls.Add(Me.BtnValidationPublication)
        Me.GbxStatutAffichage.Controls.Add(Me.ChkOcculte)
        Me.GbxStatutAffichage.Controls.Add(Me.ChkCache)
        Me.GbxStatutAffichage.Controls.Add(Me.ChkPublie)
        Me.GbxStatutAffichage.Controls.Add(Me.Label2)
        Me.GbxStatutAffichage.Location = New System.Drawing.Point(12, 252)
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
        Me.LblPublication.Size = New System.Drawing.Size(110, 13)
        Me.LblPublication.TabIndex = 69
        Me.LblPublication.Text = "Antécédent publié"
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
        Me.GbxPrincipal.Controls.Add(Me.Label6)
        Me.GbxPrincipal.Controls.Add(Me.ChkDiagnosticNotion)
        Me.GbxPrincipal.Controls.Add(Me.ChkDiagnosticSuspecte)
        Me.GbxPrincipal.Controls.Add(Me.ChkDiagnosticConfirme)
        Me.GbxPrincipal.Controls.Add(Me.BtnRecupereDrc)
        Me.GbxPrincipal.Controls.Add(Me.Label4)
        Me.GbxPrincipal.Controls.Add(Me.DteDateDebut)
        Me.GbxPrincipal.Controls.Add(Me.Label3)
        Me.GbxPrincipal.Controls.Add(Me.TxtAntecedentDescription)
        Me.GbxPrincipal.Controls.Add(Me.LblDrcDenomination)
        Me.GbxPrincipal.Controls.Add(Me.Label1)
        Me.GbxPrincipal.Controls.Add(Me.TxtDrcId)
        Me.GbxPrincipal.Controls.Add(Me.BtnValider)
        Me.GbxPrincipal.Location = New System.Drawing.Point(12, 84)
        Me.GbxPrincipal.Name = "GbxPrincipal"
        Me.GbxPrincipal.Size = New System.Drawing.Size(747, 162)
        Me.GbxPrincipal.TabIndex = 71
        Me.GbxPrincipal.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(2, 130)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 51
        Me.Label6.Text = "Diagnostic"
        '
        'ChkDiagnosticNotion
        '
        Me.ChkDiagnosticNotion.AutoSize = True
        Me.ChkDiagnosticNotion.Location = New System.Drawing.Point(366, 129)
        Me.ChkDiagnosticNotion.Name = "ChkDiagnosticNotion"
        Me.ChkDiagnosticNotion.Size = New System.Drawing.Size(72, 17)
        Me.ChkDiagnosticNotion.TabIndex = 50
        Me.ChkDiagnosticNotion.Text = "Notion de"
        Me.ChkDiagnosticNotion.UseVisualStyleBackColor = True
        '
        'ChkDiagnosticSuspecte
        '
        Me.ChkDiagnosticSuspecte.AutoSize = True
        Me.ChkDiagnosticSuspecte.Location = New System.Drawing.Point(247, 129)
        Me.ChkDiagnosticSuspecte.Name = "ChkDiagnosticSuspecte"
        Me.ChkDiagnosticSuspecte.Size = New System.Drawing.Size(71, 17)
        Me.ChkDiagnosticSuspecte.TabIndex = 48
        Me.ChkDiagnosticSuspecte.Text = "Suspecté"
        Me.ChkDiagnosticSuspecte.UseVisualStyleBackColor = True
        '
        'ChkDiagnosticConfirme
        '
        Me.ChkDiagnosticConfirme.AutoSize = True
        Me.ChkDiagnosticConfirme.Location = New System.Drawing.Point(128, 129)
        Me.ChkDiagnosticConfirme.Name = "ChkDiagnosticConfirme"
        Me.ChkDiagnosticConfirme.Size = New System.Drawing.Size(67, 17)
        Me.ChkDiagnosticConfirme.TabIndex = 47
        Me.ChkDiagnosticConfirme.Text = "Confirmé"
        Me.ChkDiagnosticConfirme.UseVisualStyleBackColor = True
        '
        'BtnModifier
        '
        Me.BtnModifier.Location = New System.Drawing.Point(12, 344)
        Me.BtnModifier.Name = "BtnModifier"
        Me.BtnModifier.Size = New System.Drawing.Size(75, 23)
        Me.BtnModifier.TabIndex = 72
        Me.BtnModifier.Text = "Modifier"
        Me.BtnModifier.UseVisualStyleBackColor = True
        '
        'LblCreationAntecedent1
        '
        Me.LblCreationAntecedent1.AutoSize = True
        Me.LblCreationAntecedent1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCreationAntecedent1.Location = New System.Drawing.Point(9, 313)
        Me.LblCreationAntecedent1.Name = "LblCreationAntecedent1"
        Me.LblCreationAntecedent1.Size = New System.Drawing.Size(55, 13)
        Me.LblCreationAntecedent1.TabIndex = 56
        Me.LblCreationAntecedent1.Text = "Créé le :"
        '
        'LblModificationAntecedent1
        '
        Me.LblModificationAntecedent1.AutoSize = True
        Me.LblModificationAntecedent1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblModificationAntecedent1.Location = New System.Drawing.Point(322, 313)
        Me.LblModificationAntecedent1.Name = "LblModificationAntecedent1"
        Me.LblModificationAntecedent1.Size = New System.Drawing.Size(70, 13)
        Me.LblModificationAntecedent1.TabIndex = 75
        Me.LblModificationAntecedent1.Text = "Modifié le :"
        '
        'BtnValidationCreationAntecedent
        '
        Me.BtnValidationCreationAntecedent.Location = New System.Drawing.Point(603, 344)
        Me.BtnValidationCreationAntecedent.Name = "BtnValidationCreationAntecedent"
        Me.BtnValidationCreationAntecedent.Size = New System.Drawing.Size(75, 23)
        Me.BtnValidationCreationAntecedent.TabIndex = 76
        Me.BtnValidationCreationAntecedent.Text = "Validation"
        Me.BtnValidationCreationAntecedent.UseVisualStyleBackColor = True
        '
        'LblId
        '
        Me.LblId.AutoSize = True
        Me.LblId.Location = New System.Drawing.Point(742, 313)
        Me.LblId.Name = "LblId"
        Me.LblId.Size = New System.Drawing.Size(13, 13)
        Me.LblId.TabIndex = 77
        Me.LblId.Text = "?"
        '
        'FAntecedentDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(772, 380)
        Me.Controls.Add(Me.LblId)
        Me.Controls.Add(Me.BtnValidationCreationAntecedent)
        Me.Controls.Add(Me.LblModificationAntecedent1)
        Me.Controls.Add(Me.BtnPublication)
        Me.Controls.Add(Me.BtnModifier)
        Me.Controls.Add(Me.GbxPrincipal)
        Me.Controls.Add(Me.GbxStatutAffichage)
        Me.Controls.Add(Me.BtnReactiver)
        Me.Controls.Add(Me.BtnSupprimer)
        Me.Controls.Add(Me.LblModificationAntecedent2)
        Me.Controls.Add(Me.LblCreationAntecedent2)
        Me.Controls.Add(Me.LblUtilisateurModification)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.LblCreationAntecedent1)
        Me.Controls.Add(Me.LblAntecedentDateCreation)
        Me.Controls.Add(Me.LblAntecedentDateModification)
        Me.Controls.Add(Me.DtnAbandon)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FAntecedentDetailEdit"
        Me.Text = "Antécédent"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GbxStatutAffichage.ResumeLayout(False)
        Me.GbxStatutAffichage.PerformLayout()
        Me.GbxPrincipal.ResumeLayout(False)
        Me.GbxPrincipal.PerformLayout()
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
    Friend WithEvents TxtAntecedentDescription As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents DteDateDebut As DateTimePicker
    Friend WithEvents Label4 As Label
    Friend WithEvents BtnRecupereDrc As Button
    Friend WithEvents DtnAbandon As Button
    Friend WithEvents BtnValider As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents LblModificationAntecedent2 As Label
    Friend WithEvents LblCreationAntecedent2 As Label
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblAntecedentDateCreation As Label
    Friend WithEvents LblAntecedentDateModification As Label
    Friend WithEvents BtnSupprimer As Button
    Friend WithEvents ChkPublie As CheckBox
    Friend WithEvents ChkCache As CheckBox
    Friend WithEvents ChkOcculte As CheckBox
    Friend WithEvents BtnReactiver As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents GbxStatutAffichage As GroupBox
    Friend WithEvents GbxPrincipal As GroupBox
    Friend WithEvents BtnValidationPublication As Button
    Friend WithEvents BtnModifier As Button
    Friend WithEvents BtnPublication As Button
    Friend WithEvents LblCreationAntecedent1 As Label
    Friend WithEvents LblModificationAntecedent1 As Label
    Friend WithEvents BtnValidationCreationAntecedent As Button
    Friend WithEvents LblPublication As Label
    Friend WithEvents LblId As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents ChkDiagnosticNotion As CheckBox
    Friend WithEvents ChkDiagnosticSuspecte As CheckBox
    Friend WithEvents ChkDiagnosticConfirme As CheckBox
End Class
