<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFTacheModificationDemandeRendezVous
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
        Me.components = New System.ComponentModel.Container()
        Me.RadGroupBoxEtatCivil = New Telerik.WinControls.UI.RadGroupBox()
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
        Me.GbxIntervention = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadChkDRVAnneeSeulement = New Telerik.WinControls.UI.RadCheckBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.TxtRDVCommentaire = New System.Windows.Forms.TextBox()
        Me.NumAn = New System.Windows.Forms.NumericUpDown()
        Me.NumMois = New System.Windows.Forms.NumericUpDown()
        Me.LblPeriode = New System.Windows.Forms.Label()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.RadBtnPlanifierRdv = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBoxEtatCivil.SuspendLayout()
        CType(Me.GbxIntervention, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GbxIntervention.SuspendLayout()
        CType(Me.RadChkDRVAnneeSeulement, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumAn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumMois, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnPlanifierRdv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGroupBoxEtatCivil
        '
        Me.RadGroupBoxEtatCivil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBoxEtatCivil.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblALD)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label13)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientDateMaj)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label5)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientUniteSanitaire)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label6)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientSite)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label4)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientTel2)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientPrenom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientTel1)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label3)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientVille)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientGenre)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientCodePostal)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNIR)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientAdresse2)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label2)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientAdresse1)
        Me.RadGroupBoxEtatCivil.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBoxEtatCivil.HeaderText = "Etat civil"
        Me.RadGroupBoxEtatCivil.Location = New System.Drawing.Point(12, 2)
        Me.RadGroupBoxEtatCivil.Name = "RadGroupBoxEtatCivil"
        Me.RadGroupBoxEtatCivil.Size = New System.Drawing.Size(952, 71)
        Me.RadGroupBoxEtatCivil.TabIndex = 3
        Me.RadGroupBoxEtatCivil.Text = "Etat civil"
        '
        'LblALD
        '
        Me.LblALD.AutoSize = True
        Me.LblALD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblALD.ForeColor = System.Drawing.Color.OrangeRed
        Me.LblALD.Location = New System.Drawing.Point(902, 20)
        Me.LblALD.Name = "LblALD"
        Me.LblALD.Size = New System.Drawing.Size(31, 13)
        Me.LblALD.TabIndex = 43
        Me.LblALD.Text = "ALD"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(510, 20)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 42
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(801, 20)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientDateMaj.TabIndex = 41
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(667, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientUniteSanitaire
        '
        Me.LblPatientUniteSanitaire.AutoSize = True
        Me.LblPatientUniteSanitaire.Location = New System.Drawing.Point(848, 53)
        Me.LblPatientUniteSanitaire.Name = "LblPatientUniteSanitaire"
        Me.LblPatientUniteSanitaire.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientUniteSanitaire.TabIndex = 39
        Me.LblPatientUniteSanitaire.Text = "Auxerre"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(667, 53)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(175, 13)
        Me.Label6.TabIndex = 38
        Me.Label6.Text = "Centre médical de référence :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(710, 37)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(36, 13)
        Me.LblPatientSite.TabIndex = 37
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(667, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Site :"
        '
        'LblPatientTel2
        '
        Me.LblPatientTel2.AutoSize = True
        Me.LblPatientTel2.Location = New System.Drawing.Point(403, 53)
        Me.LblPatientTel2.Name = "LblPatientTel2"
        Me.LblPatientTel2.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel2.TabIndex = 35
        Me.LblPatientTel2.Text = "0968542357"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(11, 20)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientPrenom.TabIndex = 23
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientTel1
        '
        Me.LblPatientTel1.AutoSize = True
        Me.LblPatientTel1.Location = New System.Drawing.Point(403, 37)
        Me.LblPatientTel1.Name = "LblPatientTel1"
        Me.LblPatientTel1.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientTel1.TabIndex = 34
        Me.LblPatientTel1.Text = "0288425678"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(133, 20)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientNom.TabIndex = 24
        Me.LblPatientNom.Text = "Durand"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(360, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "Tel. :"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(314, 20)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 13)
        Me.LblPatientAge.TabIndex = 25
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientVille
        '
        Me.LblPatientVille.AutoSize = True
        Me.LblPatientVille.Location = New System.Drawing.Point(125, 53)
        Me.LblPatientVille.Name = "LblPatientVille"
        Me.LblPatientVille.Size = New System.Drawing.Size(57, 13)
        Me.LblPatientVille.TabIndex = 32
        Me.LblPatientVille.Text = "Lournand"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(424, 20)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientGenre.TabIndex = 26
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientCodePostal
        '
        Me.LblPatientCodePostal.AutoSize = True
        Me.LblPatientCodePostal.Location = New System.Drawing.Point(82, 53)
        Me.LblPatientCodePostal.Name = "LblPatientCodePostal"
        Me.LblPatientCodePostal.Size = New System.Drawing.Size(37, 13)
        Me.LblPatientCodePostal.TabIndex = 31
        Me.LblPatientCodePostal.Text = "71250"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(561, 20)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 27
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'LblPatientAdresse2
        '
        Me.LblPatientAdresse2.AutoSize = True
        Me.LblPatientAdresse2.Location = New System.Drawing.Point(236, 36)
        Me.LblPatientAdresse2.Name = "LblPatientAdresse2"
        Me.LblPatientAdresse2.Size = New System.Drawing.Size(55, 13)
        Me.LblPatientAdresse2.TabIndex = 30
        Me.LblPatientAdresse2.Text = "adresse 2"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(11, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "Adresse :"
        '
        'LblPatientAdresse1
        '
        Me.LblPatientAdresse1.AutoSize = True
        Me.LblPatientAdresse1.Location = New System.Drawing.Point(82, 36)
        Me.LblPatientAdresse1.Name = "LblPatientAdresse1"
        Me.LblPatientAdresse1.Size = New System.Drawing.Size(121, 13)
        Me.LblPatientAdresse1.TabIndex = 29
        Me.LblPatientAdresse1.Text = "3 rue de la république"
        '
        'GbxIntervention
        '
        Me.GbxIntervention.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.GbxIntervention.Controls.Add(Me.RadChkDRVAnneeSeulement)
        Me.GbxIntervention.Controls.Add(Me.Label16)
        Me.GbxIntervention.Controls.Add(Me.TxtRDVCommentaire)
        Me.GbxIntervention.Controls.Add(Me.NumAn)
        Me.GbxIntervention.Controls.Add(Me.NumMois)
        Me.GbxIntervention.Controls.Add(Me.LblPeriode)
        Me.GbxIntervention.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.GbxIntervention.HeaderText = "Demande de rendez-vous"
        Me.GbxIntervention.Location = New System.Drawing.Point(12, 79)
        Me.GbxIntervention.Name = "GbxIntervention"
        Me.GbxIntervention.Size = New System.Drawing.Size(952, 112)
        Me.GbxIntervention.TabIndex = 12
        Me.GbxIntervention.Text = "Demande de rendez-vous"
        CType(Me.GbxIntervention.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.GbxIntervention.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.GbxIntervention.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'RadChkDRVAnneeSeulement
        '
        Me.RadChkDRVAnneeSeulement.Location = New System.Drawing.Point(239, 26)
        Me.RadChkDRVAnneeSeulement.Name = "RadChkDRVAnneeSeulement"
        Me.RadChkDRVAnneeSeulement.Size = New System.Drawing.Size(107, 18)
        Me.RadChkDRVAnneeSeulement.TabIndex = 17
        Me.RadChkDRVAnneeSeulement.Text = "Année seulement"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(11, 56)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(75, 13)
        Me.Label16.TabIndex = 16
        Me.Label16.Text = "Commentaire"
        '
        'TxtRDVCommentaire
        '
        Me.TxtRDVCommentaire.Location = New System.Drawing.Point(128, 53)
        Me.TxtRDVCommentaire.MaxLength = 200
        Me.TxtRDVCommentaire.Multiline = True
        Me.TxtRDVCommentaire.Name = "TxtRDVCommentaire"
        Me.TxtRDVCommentaire.Size = New System.Drawing.Size(817, 44)
        Me.TxtRDVCommentaire.TabIndex = 15
        '
        'NumAn
        '
        Me.NumAn.Location = New System.Drawing.Point(170, 26)
        Me.NumAn.Maximum = New Decimal(New Integer() {2200, 0, 0, 0})
        Me.NumAn.Minimum = New Decimal(New Integer() {2018, 0, 0, 0})
        Me.NumAn.Name = "NumAn"
        Me.NumAn.Size = New System.Drawing.Size(55, 20)
        Me.NumAn.TabIndex = 10
        Me.NumAn.Value = New Decimal(New Integer() {2018, 0, 0, 0})
        '
        'NumMois
        '
        Me.NumMois.Location = New System.Drawing.Point(128, 27)
        Me.NumMois.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.NumMois.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumMois.Name = "NumMois"
        Me.NumMois.Size = New System.Drawing.Size(36, 20)
        Me.NumMois.TabIndex = 9
        Me.NumMois.Value = New Decimal(New Integer() {11, 0, 0, 0})
        '
        'LblPeriode
        '
        Me.LblPeriode.AutoSize = True
        Me.LblPeriode.Location = New System.Drawing.Point(11, 28)
        Me.LblPeriode.Name = "LblPeriode"
        Me.LblPeriode.Size = New System.Drawing.Size(46, 13)
        Me.LblPeriode.TabIndex = 8
        Me.LblPeriode.Text = "Période"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(940, 197)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 13
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.Location = New System.Drawing.Point(12, 197)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(106, 24)
        Me.RadBtnValidation.TabIndex = 11
        Me.RadBtnValidation.Text = "Validation"
        Me.ToolTip1.SetToolTip(Me.RadBtnValidation, "Valider la modification")
        '
        'RadBtnPlanifierRdv
        '
        Me.RadBtnPlanifierRdv.Location = New System.Drawing.Point(127, 197)
        Me.RadBtnPlanifierRdv.Name = "RadBtnPlanifierRdv"
        Me.RadBtnPlanifierRdv.Size = New System.Drawing.Size(134, 24)
        Me.RadBtnPlanifierRdv.TabIndex = 14
        Me.RadBtnPlanifierRdv.Text = "Planifier le rendez-vous"
        '
        'RadFTacheModificationDemandeRendezVous
        '
        Me.AcceptButton = Me.RadBtnValidation
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(975, 224)
        Me.Controls.Add(Me.RadBtnPlanifierRdv)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.GbxIntervention)
        Me.Controls.Add(Me.RadGroupBoxEtatCivil)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFTacheModificationDemandeRendezVous"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Modification demande de rendez-vous"
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBoxEtatCivil.ResumeLayout(False)
        Me.RadGroupBoxEtatCivil.PerformLayout()
        CType(Me.GbxIntervention, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GbxIntervention.ResumeLayout(False)
        Me.GbxIntervention.PerformLayout()
        CType(Me.RadChkDRVAnneeSeulement, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumAn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumMois, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnPlanifierRdv, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGroupBoxEtatCivil As Telerik.WinControls.UI.RadGroupBox
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
    Friend WithEvents GbxIntervention As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadChkDRVAnneeSeulement As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents Label16 As Label
    Friend WithEvents TxtRDVCommentaire As TextBox
    Friend WithEvents NumAn As NumericUpDown
    Friend WithEvents NumMois As NumericUpDown
    Friend WithEvents LblPeriode As Label
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents RadBtnPlanifierRdv As Telerik.WinControls.UI.RadButton
End Class

