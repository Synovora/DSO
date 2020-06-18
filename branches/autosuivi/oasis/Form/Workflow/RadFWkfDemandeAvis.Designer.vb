<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFWkfDemandeAvis
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
        Me.LblPatientDateNaissance = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.RadBtnMessagePrecedent = New Telerik.WinControls.UI.RadButton()
        Me.LblLabelTypeTache = New System.Windows.Forms.Label()
        Me.TxtCommentaireDemande = New System.Windows.Forms.TextBox()
        Me.LblPriorite = New System.Windows.Forms.Label()
        Me.RadioBtnAsynchrone = New System.Windows.Forms.RadioButton()
        Me.RadioBtnSynchrone = New System.Windows.Forms.RadioButton()
        Me.RadioBtnAvisUrgent = New System.Windows.Forms.RadioButton()
        Me.CbxDestinataireFonction = New System.Windows.Forms.ComboBox()
        Me.RadPanelDestinataire = New Telerik.WinControls.UI.RadPanel()
        Me.LblDestinataireFonction = New System.Windows.Forms.Label()
        Me.LblDestinataireLocalisation = New System.Windows.Forms.Label()
        Me.LblVersDestinataire = New System.Windows.Forms.Label()
        Me.RadBtnEpisode = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSousEpisode = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSynthèse = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnLigneDeVie = New Telerik.WinControls.UI.RadButton()
        Me.LblWorkflowDescription = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBoxEtatCivil.SuspendLayout()
        CType(Me.RadBtnMessagePrecedent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanelDestinataire, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanelDestinataire.SuspendLayout()
        CType(Me.RadBtnEpisode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSousEpisode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSynthèse, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnLigneDeVie, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGroupBoxEtatCivil
        '
        Me.RadGroupBoxEtatCivil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBoxEtatCivil.BackColor = System.Drawing.Color.Bisque
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientDateNaissance)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBoxEtatCivil.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBoxEtatCivil.HeaderText = ""
        Me.RadGroupBoxEtatCivil.Location = New System.Drawing.Point(3, 96)
        Me.RadGroupBoxEtatCivil.Name = "RadGroupBoxEtatCivil"
        Me.RadGroupBoxEtatCivil.Size = New System.Drawing.Size(641, 37)
        Me.RadGroupBoxEtatCivil.TabIndex = 3
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'LblPatientDateNaissance
        '
        Me.LblPatientDateNaissance.AutoSize = True
        Me.LblPatientDateNaissance.Location = New System.Drawing.Point(267, 9)
        Me.LblPatientDateNaissance.Name = "LblPatientDateNaissance"
        Me.LblPatientDateNaissance.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientDateNaissance.TabIndex = 26
        Me.LblPatientDateNaissance.Text = "jj/mm/aaaa"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(11, 9)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(82, 13)
        Me.LblPatientNom.TabIndex = 23
        Me.LblPatientNom.Text = "LblPatientNom"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(372, 9)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 13)
        Me.LblPatientAge.TabIndex = 25
        Me.LblPatientAge.Text = "35 ans"
        '
        'RadBtnMessagePrecedent
        '
        Me.RadBtnMessagePrecedent.Location = New System.Drawing.Point(619, 189)
        Me.RadBtnMessagePrecedent.Name = "RadBtnMessagePrecedent"
        Me.RadBtnMessagePrecedent.Size = New System.Drawing.Size(25, 24)
        Me.RadBtnMessagePrecedent.TabIndex = 27
        Me.RadBtnMessagePrecedent.Text = ">"
        Me.ToolTip.SetToolTip(Me.RadBtnMessagePrecedent, "Consulter les messages précédents")
        '
        'LblLabelTypeTache
        '
        Me.LblLabelTypeTache.AutoSize = True
        Me.LblLabelTypeTache.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.LblLabelTypeTache.ForeColor = System.Drawing.Color.DarkRed
        Me.LblLabelTypeTache.Location = New System.Drawing.Point(0, 7)
        Me.LblLabelTypeTache.Name = "LblLabelTypeTache"
        Me.LblLabelTypeTache.Size = New System.Drawing.Size(144, 15)
        Me.LblLabelTypeTache.TabIndex = 7
        Me.LblLabelTypeTache.Text = "Création demande d'avis"
        '
        'TxtCommentaireDemande
        '
        Me.TxtCommentaireDemande.Location = New System.Drawing.Point(3, 133)
        Me.TxtCommentaireDemande.MaxLength = 512
        Me.TxtCommentaireDemande.Multiline = True
        Me.TxtCommentaireDemande.Name = "TxtCommentaireDemande"
        Me.TxtCommentaireDemande.Size = New System.Drawing.Size(641, 59)
        Me.TxtCommentaireDemande.TabIndex = 15
        '
        'LblPriorite
        '
        Me.LblPriorite.AutoSize = True
        Me.LblPriorite.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblPriorite.Location = New System.Drawing.Point(253, 200)
        Me.LblPriorite.Name = "LblPriorite"
        Me.LblPriorite.Size = New System.Drawing.Size(51, 13)
        Me.LblPriorite.TabIndex = 20
        Me.LblPriorite.Text = "Priorité :"
        '
        'RadioBtnAsynchrone
        '
        Me.RadioBtnAsynchrone.AutoSize = True
        Me.RadioBtnAsynchrone.Location = New System.Drawing.Point(524, 198)
        Me.RadioBtnAsynchrone.Name = "RadioBtnAsynchrone"
        Me.RadioBtnAsynchrone.Size = New System.Drawing.Size(85, 17)
        Me.RadioBtnAsynchrone.TabIndex = 19
        Me.RadioBtnAsynchrone.TabStop = True
        Me.RadioBtnAsynchrone.Text = "Asynchrone"
        Me.RadioBtnAsynchrone.UseVisualStyleBackColor = True
        '
        'RadioBtnSynchrone
        '
        Me.RadioBtnSynchrone.AutoSize = True
        Me.RadioBtnSynchrone.Location = New System.Drawing.Point(421, 198)
        Me.RadioBtnSynchrone.Name = "RadioBtnSynchrone"
        Me.RadioBtnSynchrone.Size = New System.Drawing.Size(79, 17)
        Me.RadioBtnSynchrone.TabIndex = 18
        Me.RadioBtnSynchrone.TabStop = True
        Me.RadioBtnSynchrone.Text = "Synchrone"
        Me.RadioBtnSynchrone.UseVisualStyleBackColor = True
        '
        'RadioBtnAvisUrgent
        '
        Me.RadioBtnAvisUrgent.AutoSize = True
        Me.RadioBtnAvisUrgent.Location = New System.Drawing.Point(318, 198)
        Me.RadioBtnAvisUrgent.Name = "RadioBtnAvisUrgent"
        Me.RadioBtnAvisUrgent.Size = New System.Drawing.Size(83, 17)
        Me.RadioBtnAvisUrgent.TabIndex = 17
        Me.RadioBtnAvisUrgent.TabStop = True
        Me.RadioBtnAvisUrgent.Text = "Avis urgent"
        Me.RadioBtnAvisUrgent.UseVisualStyleBackColor = True
        '
        'CbxDestinataireFonction
        '
        Me.CbxDestinataireFonction.FormattingEnabled = True
        Me.CbxDestinataireFonction.Location = New System.Drawing.Point(12, 10)
        Me.CbxDestinataireFonction.Name = "CbxDestinataireFonction"
        Me.CbxDestinataireFonction.Size = New System.Drawing.Size(149, 21)
        Me.CbxDestinataireFonction.TabIndex = 16
        '
        'RadPanelDestinataire
        '
        Me.RadPanelDestinataire.BackColor = System.Drawing.Color.Thistle
        Me.RadPanelDestinataire.Controls.Add(Me.LblDestinataireFonction)
        Me.RadPanelDestinataire.Controls.Add(Me.LblDestinataireLocalisation)
        Me.RadPanelDestinataire.Controls.Add(Me.CbxDestinataireFonction)
        Me.RadPanelDestinataire.Location = New System.Drawing.Point(3, 51)
        Me.RadPanelDestinataire.Name = "RadPanelDestinataire"
        Me.RadPanelDestinataire.Size = New System.Drawing.Size(641, 40)
        Me.RadPanelDestinataire.TabIndex = 18
        '
        'LblDestinataireFonction
        '
        Me.LblDestinataireFonction.AutoSize = True
        Me.LblDestinataireFonction.Location = New System.Drawing.Point(422, 13)
        Me.LblDestinataireFonction.Name = "LblDestinataireFonction"
        Me.LblDestinataireFonction.Size = New System.Drawing.Size(130, 13)
        Me.LblDestinataireFonction.TabIndex = 19
        Me.LblDestinataireFonction.Text = "LblDestinataireFonction"
        '
        'LblDestinataireLocalisation
        '
        Me.LblDestinataireLocalisation.AutoSize = True
        Me.LblDestinataireLocalisation.Location = New System.Drawing.Point(267, 13)
        Me.LblDestinataireLocalisation.Name = "LblDestinataireLocalisation"
        Me.LblDestinataireLocalisation.Size = New System.Drawing.Size(145, 13)
        Me.LblDestinataireLocalisation.TabIndex = 18
        Me.LblDestinataireLocalisation.Text = "LblDestinataireLocalisation"
        '
        'LblVersDestinataire
        '
        Me.LblVersDestinataire.AutoSize = True
        Me.LblVersDestinataire.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.LblVersDestinataire.Location = New System.Drawing.Point(0, 25)
        Me.LblVersDestinataire.Name = "LblVersDestinataire"
        Me.LblVersDestinataire.Size = New System.Drawing.Size(94, 20)
        Me.LblVersDestinataire.TabIndex = 21
        Me.LblVersDestinataire.Text = "Destinataire"
        '
        'RadBtnEpisode
        '
        Me.RadBtnEpisode.Location = New System.Drawing.Point(33, 248)
        Me.RadBtnEpisode.Name = "RadBtnEpisode"
        Me.RadBtnEpisode.Size = New System.Drawing.Size(97, 24)
        Me.RadBtnEpisode.TabIndex = 22
        Me.RadBtnEpisode.Text = "Episode"
        '
        'RadBtnSousEpisode
        '
        Me.RadBtnSousEpisode.Location = New System.Drawing.Point(136, 248)
        Me.RadBtnSousEpisode.Name = "RadBtnSousEpisode"
        Me.RadBtnSousEpisode.Size = New System.Drawing.Size(97, 24)
        Me.RadBtnSousEpisode.TabIndex = 23
        Me.RadBtnSousEpisode.Text = "Sous-épisode"
        '
        'RadBtnSynthèse
        '
        Me.RadBtnSynthèse.Location = New System.Drawing.Point(239, 248)
        Me.RadBtnSynthèse.Name = "RadBtnSynthèse"
        Me.RadBtnSynthèse.Size = New System.Drawing.Size(97, 24)
        Me.RadBtnSynthèse.TabIndex = 24
        Me.RadBtnSynthèse.Text = "Synthèse"
        '
        'RadBtnLigneDeVie
        '
        Me.RadBtnLigneDeVie.Location = New System.Drawing.Point(342, 248)
        Me.RadBtnLigneDeVie.Name = "RadBtnLigneDeVie"
        Me.RadBtnLigneDeVie.Size = New System.Drawing.Size(97, 24)
        Me.RadBtnLigneDeVie.TabIndex = 25
        Me.RadBtnLigneDeVie.Text = "Ligne de vie"
        '
        'LblWorkflowDescription
        '
        Me.LblWorkflowDescription.AutoSize = True
        Me.LblWorkflowDescription.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.LblWorkflowDescription.Location = New System.Drawing.Point(198, 7)
        Me.LblWorkflowDescription.Name = "LblWorkflowDescription"
        Me.LblWorkflowDescription.Size = New System.Drawing.Size(143, 15)
        Me.LblWorkflowDescription.TabIndex = 26
        Me.LblWorkflowDescription.Text = "LblWorkflowDescription"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.CheckBox1.Location = New System.Drawing.Point(3, 198)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(83, 17)
        Me.CheckBox1.TabIndex = 28
        Me.CheckBox1.Text = "CheckBox1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.CheckBox2.Location = New System.Drawing.Point(3, 221)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(83, 17)
        Me.CheckBox2.TabIndex = 29
        Me.CheckBox2.Text = "CheckBox2"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(620, 248)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 4
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.ForeColor = System.Drawing.Color.Black
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnValidation.Location = New System.Drawing.Point(3, 248)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnValidation.TabIndex = 5
        Me.ToolTip.SetToolTip(Me.RadBtnValidation, "Valider")
        '
        'RadFWkfDemandeAvis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(647, 277)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.RadBtnMessagePrecedent)
        Me.Controls.Add(Me.LblWorkflowDescription)
        Me.Controls.Add(Me.RadBtnLigneDeVie)
        Me.Controls.Add(Me.RadBtnSynthèse)
        Me.Controls.Add(Me.RadBtnSousEpisode)
        Me.Controls.Add(Me.RadBtnEpisode)
        Me.Controls.Add(Me.LblVersDestinataire)
        Me.Controls.Add(Me.LblLabelTypeTache)
        Me.Controls.Add(Me.LblPriorite)
        Me.Controls.Add(Me.RadioBtnAsynchrone)
        Me.Controls.Add(Me.RadPanelDestinataire)
        Me.Controls.Add(Me.RadioBtnSynchrone)
        Me.Controls.Add(Me.RadioBtnAvisUrgent)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.TxtCommentaireDemande)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGroupBoxEtatCivil)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFWkfDemandeAvis"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "RadFWkfDemandeAvis"
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBoxEtatCivil.ResumeLayout(False)
        Me.RadGroupBoxEtatCivil.PerformLayout()
        CType(Me.RadBtnMessagePrecedent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanelDestinataire, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanelDestinataire.ResumeLayout(False)
        Me.RadPanelDestinataire.PerformLayout()
        CType(Me.RadBtnEpisode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSousEpisode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSynthèse, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnLigneDeVie, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadGroupBoxEtatCivil As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblLabelTypeTache As Label
    Friend WithEvents TxtCommentaireDemande As TextBox
    Friend WithEvents CbxDestinataireFonction As ComboBox
    Friend WithEvents RadioBtnAsynchrone As RadioButton
    Friend WithEvents RadioBtnSynchrone As RadioButton
    Friend WithEvents RadioBtnAvisUrgent As RadioButton
    Friend WithEvents LblPriorite As Label
    Friend WithEvents RadPanelDestinataire As Telerik.WinControls.UI.RadPanel
    Friend WithEvents LblPatientDateNaissance As Label
    Friend WithEvents LblVersDestinataire As Label
    Friend WithEvents LblDestinataireLocalisation As Label
    Friend WithEvents RadBtnEpisode As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSousEpisode As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSynthèse As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnLigneDeVie As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblWorkflowDescription As Label
    Friend WithEvents RadBtnMessagePrecedent As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblDestinataireFonction As Label
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents CheckBox2 As CheckBox
End Class

