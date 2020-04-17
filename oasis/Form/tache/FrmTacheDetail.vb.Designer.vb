<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTacheDetail_vb
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GbxSortieOasis = New Telerik.WinControls.UI.RadGroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtUtilisateur = New System.Windows.Forms.TextBox()
        Me.TxtCommentaire = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.TxtFonction = New System.Windows.Forms.TextBox()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.TxtUniteSan = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtSite = New System.Windows.Forms.TextBox()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.BtnEpidode = New Telerik.WinControls.UI.RadButton()
        Me.lblTypeTache = New Telerik.WinControls.UI.RadLabel()
        Me.TxtTypeRDV = New System.Windows.Forms.TextBox()
        Me.LblTypeRDV = New System.Windows.Forms.Label()
        Me.BtnSynthese = New Telerik.WinControls.UI.RadButton()
        Me.BtnPatient = New Telerik.WinControls.UI.RadButton()
        Me.TxtPatientNom = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtPatientNir = New System.Windows.Forms.TextBox()
        Me.RadGroupParcours = New Telerik.WinControls.UI.RadGroupBox()
        Me.BtnDetailIntervenant = New Telerik.WinControls.UI.RadButton()
        Me.TxtSpecialite = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtIntervenant = New System.Windows.Forms.TextBox()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.RadIdTache = New Telerik.WinControls.UI.RadLabel()
        Me.RadButtonAbandon = New Telerik.WinControls.UI.RadButton()
        Me.BtnFixeRDV = New Telerik.WinControls.UI.RadButton()
        Me.BtnAnnulation = New Telerik.WinControls.UI.RadButton()
        Me.BtnAttribution = New Telerik.WinControls.UI.RadButton()
        Me.RadPanorama1 = New Telerik.WinControls.UI.RadPanorama()
        Me.RadGroupCible = New Telerik.WinControls.UI.RadGroupBox()
        Me.TxtFonctionCible = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtNomCible = New System.Windows.Forms.TextBox()
        CType(Me.GbxSortieOasis, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GbxSortieOasis.SuspendLayout()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        CType(Me.BtnEpidode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblTypeTache, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnSynthese, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnPatient, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupParcours, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupParcours.SuspendLayout()
        CType(Me.BtnDetailIntervenant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadIdTache, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButtonAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnFixeRDV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnAnnulation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnAttribution, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanorama1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupCible, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupCible.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GbxSortieOasis
        '
        Me.GbxSortieOasis.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.GbxSortieOasis.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.GbxSortieOasis.Controls.Add(Me.Label1)
        Me.GbxSortieOasis.Controls.Add(Me.TxtUtilisateur)
        Me.GbxSortieOasis.Controls.Add(Me.TxtCommentaire)
        Me.GbxSortieOasis.Controls.Add(Me.Label12)
        Me.GbxSortieOasis.Controls.Add(Me.TxtFonction)
        Me.GbxSortieOasis.Dock = System.Windows.Forms.DockStyle.Top
        Me.GbxSortieOasis.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.GbxSortieOasis.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.GbxSortieOasis.HeaderText = "Emetteur Tâche"
        Me.GbxSortieOasis.Location = New System.Drawing.Point(0, 135)
        Me.GbxSortieOasis.Name = "GbxSortieOasis"
        Me.GbxSortieOasis.Size = New System.Drawing.Size(726, 117)
        Me.GbxSortieOasis.TabIndex = 2
        Me.GbxSortieOasis.Text = "Emetteur Tâche"
        Me.GbxSortieOasis.ThemeName = "ControlDefault"
        CType(Me.GbxSortieOasis.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.GbxSortieOasis.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.GbxSortieOasis.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Commentaire"
        '
        'TxtUtilisateur
        '
        Me.TxtUtilisateur.Enabled = False
        Me.TxtUtilisateur.Location = New System.Drawing.Point(118, 25)
        Me.TxtUtilisateur.Name = "TxtUtilisateur"
        Me.TxtUtilisateur.ReadOnly = True
        Me.TxtUtilisateur.Size = New System.Drawing.Size(307, 25)
        Me.TxtUtilisateur.TabIndex = 1
        '
        'TxtCommentaire
        '
        Me.TxtCommentaire.Location = New System.Drawing.Point(118, 52)
        Me.TxtCommentaire.Multiline = True
        Me.TxtCommentaire.Name = "TxtCommentaire"
        Me.TxtCommentaire.ReadOnly = True
        Me.TxtCommentaire.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TxtCommentaire.Size = New System.Drawing.Size(600, 57)
        Me.TxtCommentaire.TabIndex = 3
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(8, 29)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(105, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Utilisat./Fonction"
        '
        'TxtFonction
        '
        Me.TxtFonction.Enabled = False
        Me.TxtFonction.Location = New System.Drawing.Point(433, 25)
        Me.TxtFonction.Name = "TxtFonction"
        Me.TxtFonction.ReadOnly = True
        Me.TxtFonction.Size = New System.Drawing.Size(285, 25)
        Me.TxtFonction.TabIndex = 2
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBox1.Controls.Add(Me.TxtUniteSan)
        Me.RadGroupBox1.Controls.Add(Me.Label4)
        Me.RadGroupBox1.Controls.Add(Me.TxtSite)
        Me.RadGroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupBox1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox1.HeaderText = "Localisation"
        Me.RadGroupBox1.Location = New System.Drawing.Point(0, 252)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(726, 55)
        Me.RadGroupBox1.TabIndex = 2
        Me.RadGroupBox1.Text = "Localisation"
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'TxtUniteSan
        '
        Me.TxtUniteSan.Enabled = False
        Me.TxtUniteSan.Location = New System.Drawing.Point(118, 25)
        Me.TxtUniteSan.Name = "TxtUniteSan"
        Me.TxtUniteSan.ReadOnly = True
        Me.TxtUniteSan.Size = New System.Drawing.Size(292, 25)
        Me.TxtUniteSan.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Unité / Site"
        '
        'TxtSite
        '
        Me.TxtSite.Enabled = False
        Me.TxtSite.Location = New System.Drawing.Point(416, 25)
        Me.TxtSite.Name = "TxtSite"
        Me.TxtSite.ReadOnly = True
        Me.TxtSite.Size = New System.Drawing.Size(300, 25)
        Me.TxtSite.TabIndex = 2
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBox2.Controls.Add(Me.BtnEpidode)
        Me.RadGroupBox2.Controls.Add(Me.lblTypeTache)
        Me.RadGroupBox2.Controls.Add(Me.TxtTypeRDV)
        Me.RadGroupBox2.Controls.Add(Me.LblTypeRDV)
        Me.RadGroupBox2.Controls.Add(Me.BtnSynthese)
        Me.RadGroupBox2.Controls.Add(Me.BtnPatient)
        Me.RadGroupBox2.Controls.Add(Me.TxtPatientNom)
        Me.RadGroupBox2.Controls.Add(Me.Label2)
        Me.RadGroupBox2.Controls.Add(Me.TxtPatientNir)
        Me.RadGroupBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupBox2.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupBox2.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox2.HeaderText = "Patient"
        Me.RadGroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(726, 81)
        Me.RadGroupBox2.TabIndex = 3
        Me.RadGroupBox2.Text = "Patient"
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'BtnEpidode
        '
        Me.BtnEpidode.Location = New System.Drawing.Point(648, 51)
        Me.BtnEpidode.Name = "BtnEpidode"
        Me.BtnEpidode.Size = New System.Drawing.Size(71, 20)
        Me.BtnEpidode.TabIndex = 8
        Me.BtnEpidode.Text = "Episode"
        Me.BtnEpidode.Visible = False
        '
        'lblTypeTache
        '
        Me.lblTypeTache.Location = New System.Drawing.Point(420, 51)
        Me.lblTypeTache.Name = "lblTypeTache"
        Me.lblTypeTache.Size = New System.Drawing.Size(74, 18)
        Me.lblTypeTache.TabIndex = 7
        Me.lblTypeTache.Text = "LblTypeTache"
        '
        'TxtTypeRDV
        '
        Me.TxtTypeRDV.BackColor = System.Drawing.SystemColors.Control
        Me.TxtTypeRDV.Enabled = False
        Me.TxtTypeRDV.Location = New System.Drawing.Point(118, 51)
        Me.TxtTypeRDV.Name = "TxtTypeRDV"
        Me.TxtTypeRDV.ReadOnly = True
        Me.TxtTypeRDV.Size = New System.Drawing.Size(292, 25)
        Me.TxtTypeRDV.TabIndex = 6
        '
        'LblTypeRDV
        '
        Me.LblTypeRDV.AutoSize = True
        Me.LblTypeRDV.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTypeRDV.Location = New System.Drawing.Point(8, 53)
        Me.LblTypeRDV.Name = "LblTypeRDV"
        Me.LblTypeRDV.Size = New System.Drawing.Size(65, 13)
        Me.LblTypeRDV.TabIndex = 5
        Me.LblTypeRDV.Text = "Type RDV"
        '
        'BtnSynthese
        '
        Me.BtnSynthese.Location = New System.Drawing.Point(648, 25)
        Me.BtnSynthese.Name = "BtnSynthese"
        Me.BtnSynthese.Size = New System.Drawing.Size(71, 20)
        Me.BtnSynthese.TabIndex = 4
        Me.BtnSynthese.Text = "Synthèse"
        '
        'BtnPatient
        '
        Me.BtnPatient.Location = New System.Drawing.Point(571, 25)
        Me.BtnPatient.Name = "BtnPatient"
        Me.BtnPatient.Size = New System.Drawing.Size(71, 20)
        Me.BtnPatient.TabIndex = 3
        Me.BtnPatient.Text = "Détail"
        '
        'TxtPatientNom
        '
        Me.TxtPatientNom.Enabled = False
        Me.TxtPatientNom.Location = New System.Drawing.Point(118, 25)
        Me.TxtPatientNom.Name = "TxtPatientNom"
        Me.TxtPatientNom.ReadOnly = True
        Me.TxtPatientNom.Size = New System.Drawing.Size(292, 25)
        Me.TxtPatientNom.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Nom / NIR"
        '
        'TxtPatientNir
        '
        Me.TxtPatientNir.Enabled = False
        Me.TxtPatientNir.Location = New System.Drawing.Point(418, 25)
        Me.TxtPatientNir.Name = "TxtPatientNir"
        Me.TxtPatientNir.ReadOnly = True
        Me.TxtPatientNir.Size = New System.Drawing.Size(142, 25)
        Me.TxtPatientNir.TabIndex = 2
        '
        'RadGroupParcours
        '
        Me.RadGroupParcours.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupParcours.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupParcours.Controls.Add(Me.BtnDetailIntervenant)
        Me.RadGroupParcours.Controls.Add(Me.TxtSpecialite)
        Me.RadGroupParcours.Controls.Add(Me.Label3)
        Me.RadGroupParcours.Controls.Add(Me.TxtIntervenant)
        Me.RadGroupParcours.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupParcours.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupParcours.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupParcours.HeaderText = "Parcours"
        Me.RadGroupParcours.Location = New System.Drawing.Point(0, 81)
        Me.RadGroupParcours.Name = "RadGroupParcours"
        Me.RadGroupParcours.Size = New System.Drawing.Size(726, 54)
        Me.RadGroupParcours.TabIndex = 4
        Me.RadGroupParcours.Text = "Parcours"
        CType(Me.RadGroupParcours.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupParcours.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupParcours.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'BtnDetailIntervenant
        '
        Me.BtnDetailIntervenant.Location = New System.Drawing.Point(648, 25)
        Me.BtnDetailIntervenant.Name = "BtnDetailIntervenant"
        Me.BtnDetailIntervenant.Size = New System.Drawing.Size(71, 20)
        Me.BtnDetailIntervenant.TabIndex = 3
        Me.BtnDetailIntervenant.Text = "Détail"
        '
        'TxtSpecialite
        '
        Me.TxtSpecialite.Enabled = False
        Me.TxtSpecialite.Location = New System.Drawing.Point(118, 25)
        Me.TxtSpecialite.Name = "TxtSpecialite"
        Me.TxtSpecialite.ReadOnly = True
        Me.TxtSpecialite.Size = New System.Drawing.Size(148, 25)
        Me.TxtSpecialite.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Spclt/Intervenant"
        '
        'TxtIntervenant
        '
        Me.TxtIntervenant.Enabled = False
        Me.TxtIntervenant.Location = New System.Drawing.Point(274, 25)
        Me.TxtIntervenant.Name = "TxtIntervenant"
        Me.TxtIntervenant.ReadOnly = True
        Me.TxtIntervenant.Size = New System.Drawing.Size(368, 25)
        Me.TxtIntervenant.TabIndex = 2
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.RadIdTache)
        Me.RadPanel1.Controls.Add(Me.RadButtonAbandon)
        Me.RadPanel1.Controls.Add(Me.BtnFixeRDV)
        Me.RadPanel1.Controls.Add(Me.BtnAnnulation)
        Me.RadPanel1.Controls.Add(Me.BtnAttribution)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel1.Location = New System.Drawing.Point(0, 365)
        Me.RadPanel1.Name = "RadPanel1"
        '
        '
        '
        Me.RadPanel1.RootElement.BorderHighlightThickness = 1
        Me.RadPanel1.Size = New System.Drawing.Size(726, 45)
        Me.RadPanel1.TabIndex = 5
        CType(Me.RadPanel1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
        '
        'RadIdTache
        '
        Me.RadIdTache.Location = New System.Drawing.Point(715, 15)
        Me.RadIdTache.Name = "RadIdTache"
        Me.RadIdTache.Size = New System.Drawing.Size(11, 18)
        Me.RadIdTache.TabIndex = 3
        Me.RadIdTache.Text = "?"
        '
        'RadButtonAbandon
        '
        Me.RadButtonAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadButtonAbandon.Location = New System.Drawing.Point(602, 6)
        Me.RadButtonAbandon.Name = "RadButtonAbandon"
        Me.RadButtonAbandon.Size = New System.Drawing.Size(110, 34)
        Me.RadButtonAbandon.TabIndex = 0
        Me.RadButtonAbandon.Text = "Abandonner"
        '
        'BtnFixeRDV
        '
        Me.BtnFixeRDV.Location = New System.Drawing.Point(240, 6)
        Me.BtnFixeRDV.Name = "BtnFixeRDV"
        Me.BtnFixeRDV.Size = New System.Drawing.Size(110, 34)
        Me.BtnFixeRDV.TabIndex = 1
        Me.BtnFixeRDV.Text = "Fixer un rendez-vous"
        Me.BtnFixeRDV.Visible = False
        '
        'BtnAnnulation
        '
        Me.BtnAnnulation.ForeColor = System.Drawing.Color.DarkRed
        Me.BtnAnnulation.Location = New System.Drawing.Point(123, 6)
        Me.BtnAnnulation.Name = "BtnAnnulation"
        Me.BtnAnnulation.Size = New System.Drawing.Size(110, 34)
        Me.BtnAnnulation.TabIndex = 2
        Me.BtnAnnulation.Text = "Annuler Tâche"
        Me.BtnAnnulation.Visible = False
        '
        'BtnAttribution
        '
        Me.BtnAttribution.Location = New System.Drawing.Point(6, 6)
        Me.BtnAttribution.Name = "BtnAttribution"
        Me.BtnAttribution.Size = New System.Drawing.Size(110, 34)
        Me.BtnAttribution.TabIndex = 0
        Me.BtnAttribution.Text = "Attribuer/desattribue"
        Me.BtnAttribution.Visible = False
        '
        'RadPanorama1
        '
        Me.RadPanorama1.Location = New System.Drawing.Point(540, 421)
        Me.RadPanorama1.Name = "RadPanorama1"
        Me.RadPanorama1.Size = New System.Drawing.Size(240, 150)
        Me.RadPanorama1.TabIndex = 5
        '
        'RadGroupCible
        '
        Me.RadGroupCible.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupCible.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupCible.Controls.Add(Me.TxtFonctionCible)
        Me.RadGroupCible.Controls.Add(Me.Label5)
        Me.RadGroupCible.Controls.Add(Me.TxtNomCible)
        Me.RadGroupCible.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupCible.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.RadGroupCible.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupCible.HeaderText = "Traitée par"
        Me.RadGroupCible.Location = New System.Drawing.Point(0, 307)
        Me.RadGroupCible.Name = "RadGroupCible"
        Me.RadGroupCible.Size = New System.Drawing.Size(726, 55)
        Me.RadGroupCible.TabIndex = 3
        Me.RadGroupCible.Text = "Traitée par"
        CType(Me.RadGroupCible.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupCible.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupCible.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'TxtFonctionCible
        '
        Me.TxtFonctionCible.Enabled = False
        Me.TxtFonctionCible.Location = New System.Drawing.Point(118, 25)
        Me.TxtFonctionCible.Name = "TxtFonctionCible"
        Me.TxtFonctionCible.ReadOnly = True
        Me.TxtFonctionCible.Size = New System.Drawing.Size(242, 25)
        Me.TxtFonctionCible.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(95, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Fonction / Nom"
        '
        'TxtNomCible
        '
        Me.TxtNomCible.Enabled = False
        Me.TxtNomCible.Location = New System.Drawing.Point(366, 25)
        Me.TxtNomCible.Name = "TxtNomCible"
        Me.TxtNomCible.ReadOnly = True
        Me.TxtNomCible.Size = New System.Drawing.Size(350, 25)
        Me.TxtNomCible.TabIndex = 2
        '
        'FrmTacheDetail_vb
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadButtonAbandon
        Me.ClientSize = New System.Drawing.Size(726, 410)
        Me.Controls.Add(Me.RadGroupCible)
        Me.Controls.Add(Me.RadPanorama1)
        Me.Controls.Add(Me.RadPanel1)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.GbxSortieOasis)
        Me.Controls.Add(Me.RadGroupParcours)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.ForeColor = System.Drawing.Color.DarkRed
        Me.MinimizeBox = False
        Me.Name = "FrmTacheDetail_vb"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Détail Tâche"
        Me.ThemeName = "ControlDefault"
        CType(Me.GbxSortieOasis, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GbxSortieOasis.ResumeLayout(False)
        Me.GbxSortieOasis.PerformLayout()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.RadGroupBox2.PerformLayout()
        CType(Me.BtnEpidode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblTypeTache, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnSynthese, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnPatient, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupParcours, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupParcours.ResumeLayout(False)
        Me.RadGroupParcours.PerformLayout()
        CType(Me.BtnDetailIntervenant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.RadIdTache, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButtonAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnFixeRDV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnAnnulation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnAttribution, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanorama1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupCible, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupCible.ResumeLayout(False)
        Me.RadGroupCible.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GbxSortieOasis As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtUtilisateur As TextBox
    Friend WithEvents TxtCommentaire As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents TxtFonction As TextBox
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents TxtUniteSan As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents TxtSite As TextBox
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents TxtPatientNom As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TxtPatientNir As TextBox
    Friend WithEvents BtnPatient As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnSynthese As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupParcours As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents TxtSpecialite As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TxtIntervenant As TextBox
    Friend WithEvents BtnDetailIntervenant As Telerik.WinControls.UI.RadButton
    Friend WithEvents TxtTypeRDV As TextBox
    Friend WithEvents LblTypeRDV As Label
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents BtnAttribution As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadPanorama1 As Telerik.WinControls.UI.RadPanorama
    Friend WithEvents lblTypeTache As Telerik.WinControls.UI.RadLabel
    Friend WithEvents BtnAnnulation As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnFixeRDV As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnEpidode As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadButtonAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupCible As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents TxtFonctionCible As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TxtNomCible As TextBox
    Friend WithEvents RadIdTache As Telerik.WinControls.UI.RadLabel
End Class

