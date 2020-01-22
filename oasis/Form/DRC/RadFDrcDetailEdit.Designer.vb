<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFDrcDetailEdit
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
        Me.LblALDDescription = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TxtAld = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtCISP = New System.Windows.Forms.TextBox()
        Me.TxtCIM10 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.NumAgeMax = New System.Windows.Forms.NumericUpDown()
        Me.CbxSexe = New System.Windows.Forms.ComboBox()
        Me.CbxCategorieOasis = New System.Windows.Forms.ComboBox()
        Me.NumAgeMin = New System.Windows.Forms.NumericUpDown()
        Me.LblCategorieMajeure = New System.Windows.Forms.Label()
        Me.CbxCategorieMajeure = New System.Windows.Forms.ComboBox()
        Me.TxtId = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtLibelle = New System.Windows.Forms.TextBox()
        Me.LblDRCId = New System.Windows.Forms.Label()
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblDateModification = New System.Windows.Forms.Label()
        Me.LblLabelDateModification = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblDateCreation = New System.Windows.Forms.Label()
        Me.LblLabelDateCreation = New System.Windows.Forms.Label()
        Me.RadBtnAnnuler = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnTransformer = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnParametre = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadBtnProtocole = New Telerik.WinControls.UI.RadButton()
        Me.TxtCommentaire = New System.Windows.Forms.TextBox()
        Me.TxtReponseCommentee = New System.Windows.Forms.TextBox()
        Me.RadGbxReponse = New Telerik.WinControls.UI.RadGroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.NumAgeMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumAgeMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAnnuler, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnTransformer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnParametre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        CType(Me.RadBtnProtocole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGbxReponse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGbxReponse.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LblALDDescription
        '
        Me.LblALDDescription.AutoSize = True
        Me.LblALDDescription.Location = New System.Drawing.Point(247, 180)
        Me.LblALDDescription.Name = "LblALDDescription"
        Me.LblALDDescription.Size = New System.Drawing.Size(89, 13)
        Me.LblALDDescription.TabIndex = 159
        Me.LblALDDescription.Text = "Description ALD"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(9, 180)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(57, 13)
        Me.Label10.TabIndex = 158
        Me.Label10.Text = "Code ALD"
        '
        'TxtAld
        '
        Me.TxtAld.Location = New System.Drawing.Point(144, 177)
        Me.TxtAld.MaxLength = 11
        Me.TxtAld.Name = "TxtAld"
        Me.TxtAld.Size = New System.Drawing.Size(86, 20)
        Me.TxtAld.TabIndex = 151
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(9, 155)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(59, 13)
        Me.Label9.TabIndex = 157
        Me.Label9.Text = "Code CISP"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(9, 130)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(69, 13)
        Me.Label8.TabIndex = 156
        Me.Label8.Text = "Code CIM10"
        '
        'TxtCISP
        '
        Me.TxtCISP.Location = New System.Drawing.Point(144, 152)
        Me.TxtCISP.MaxLength = 10
        Me.TxtCISP.Name = "TxtCISP"
        Me.TxtCISP.Size = New System.Drawing.Size(87, 20)
        Me.TxtCISP.TabIndex = 148
        '
        'TxtCIM10
        '
        Me.TxtCIM10.Location = New System.Drawing.Point(144, 127)
        Me.TxtCIM10.MaxLength = 10
        Me.TxtCIM10.Name = "TxtCIM10"
        Me.TxtCIM10.Size = New System.Drawing.Size(86, 20)
        Me.TxtCIM10.TabIndex = 143
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 104)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 155
        Me.Label7.Text = "Age max"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 79)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 154
        Me.Label6.Text = "Age min"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 13)
        Me.Label5.TabIndex = 153
        Me.Label5.Text = "Applicable pour"
        '
        'NumAgeMax
        '
        Me.NumAgeMax.Location = New System.Drawing.Point(144, 102)
        Me.NumAgeMax.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
        Me.NumAgeMax.Name = "NumAgeMax"
        Me.NumAgeMax.Size = New System.Drawing.Size(67, 20)
        Me.NumAgeMax.TabIndex = 137
        '
        'CbxSexe
        '
        Me.CbxSexe.FormattingEnabled = True
        Me.CbxSexe.Location = New System.Drawing.Point(144, 51)
        Me.CbxSexe.Name = "CbxSexe"
        Me.CbxSexe.Size = New System.Drawing.Size(121, 21)
        Me.CbxSexe.TabIndex = 135
        '
        'CbxCategorieOasis
        '
        Me.CbxCategorieOasis.FormattingEnabled = True
        Me.CbxCategorieOasis.Location = New System.Drawing.Point(144, 75)
        Me.CbxCategorieOasis.Name = "CbxCategorieOasis"
        Me.CbxCategorieOasis.Size = New System.Drawing.Size(204, 21)
        Me.CbxCategorieOasis.TabIndex = 133
        '
        'NumAgeMin
        '
        Me.NumAgeMin.Location = New System.Drawing.Point(144, 77)
        Me.NumAgeMin.Name = "NumAgeMin"
        Me.NumAgeMin.Size = New System.Drawing.Size(67, 20)
        Me.NumAgeMin.TabIndex = 136
        '
        'LblCategorieMajeure
        '
        Me.LblCategorieMajeure.AutoSize = True
        Me.LblCategorieMajeure.Location = New System.Drawing.Point(9, 28)
        Me.LblCategorieMajeure.Name = "LblCategorieMajeure"
        Me.LblCategorieMajeure.Size = New System.Drawing.Size(101, 13)
        Me.LblCategorieMajeure.TabIndex = 152
        Me.LblCategorieMajeure.Text = "Catégorie majeure"
        '
        'CbxCategorieMajeure
        '
        Me.CbxCategorieMajeure.FormattingEnabled = True
        Me.CbxCategorieMajeure.Location = New System.Drawing.Point(144, 25)
        Me.CbxCategorieMajeure.Name = "CbxCategorieMajeure"
        Me.CbxCategorieMajeure.Size = New System.Drawing.Size(573, 21)
        Me.CbxCategorieMajeure.TabIndex = 134
        '
        'TxtId
        '
        Me.TxtId.Location = New System.Drawing.Point(144, 25)
        Me.TxtId.Name = "TxtId"
        Me.TxtId.ReadOnly = True
        Me.TxtId.Size = New System.Drawing.Size(67, 20)
        Me.TxtId.TabIndex = 131
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 13)
        Me.Label3.TabIndex = 150
        Me.Label3.Text = "Catégorie Oasis"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 149
        Me.Label2.Text = "Description"
        '
        'TxtLibelle
        '
        Me.TxtLibelle.Location = New System.Drawing.Point(144, 50)
        Me.TxtLibelle.MaxLength = 150
        Me.TxtLibelle.Name = "TxtLibelle"
        Me.TxtLibelle.Size = New System.Drawing.Size(675, 20)
        Me.TxtLibelle.TabIndex = 132
        '
        'LblDRCId
        '
        Me.LblDRCId.AutoSize = True
        Me.LblDRCId.Location = New System.Drawing.Point(9, 28)
        Me.LblDRCId.Name = "LblDRCId"
        Me.LblDRCId.Size = New System.Drawing.Size(113, 13)
        Me.LblDRCId.TabIndex = 147
        Me.LblDRCId.Text = "Identifiant DRC/ORC"
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(605, 451)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(101, 13)
        Me.LblUtilisateurModification.TabIndex = 146
        Me.LblUtilisateurModification.Text = "Georges Moustaki"
        '
        'LblLabelUtilisateurModification
        '
        Me.LblLabelUtilisateurModification.AutoSize = True
        Me.LblLabelUtilisateurModification.Location = New System.Drawing.Point(577, 451)
        Me.LblLabelUtilisateurModification.Name = "LblLabelUtilisateurModification"
        Me.LblLabelUtilisateurModification.Size = New System.Drawing.Size(24, 13)
        Me.LblLabelUtilisateurModification.TabIndex = 145
        Me.LblLabelUtilisateurModification.Text = "par"
        '
        'LblDateModification
        '
        Me.LblDateModification.AutoSize = True
        Me.LblDateModification.Location = New System.Drawing.Point(495, 451)
        Me.LblDateModification.Name = "LblDateModification"
        Me.LblDateModification.Size = New System.Drawing.Size(63, 13)
        Me.LblDateModification.TabIndex = 144
        Me.LblDateModification.Text = "01/01/2000"
        '
        'LblLabelDateModification
        '
        Me.LblLabelDateModification.AutoSize = True
        Me.LblLabelDateModification.Location = New System.Drawing.Point(431, 451)
        Me.LblLabelDateModification.Name = "LblLabelDateModification"
        Me.LblLabelDateModification.Size = New System.Drawing.Size(65, 13)
        Me.LblLabelDateModification.TabIndex = 142
        Me.LblLabelDateModification.Text = "Modifiée le"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(167, 451)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(98, 13)
        Me.LblUtilisateurCreation.TabIndex = 141
        Me.LblUtilisateurCreation.Text = "Stéphane Durand"
        '
        'LblLabelUtilisateurCreation
        '
        Me.LblLabelUtilisateurCreation.AutoSize = True
        Me.LblLabelUtilisateurCreation.Location = New System.Drawing.Point(138, 451)
        Me.LblLabelUtilisateurCreation.Name = "LblLabelUtilisateurCreation"
        Me.LblLabelUtilisateurCreation.Size = New System.Drawing.Size(24, 13)
        Me.LblLabelUtilisateurCreation.TabIndex = 140
        Me.LblLabelUtilisateurCreation.Text = "par"
        '
        'LblDateCreation
        '
        Me.LblDateCreation.AutoSize = True
        Me.LblDateCreation.Location = New System.Drawing.Point(64, 451)
        Me.LblDateCreation.Name = "LblDateCreation"
        Me.LblDateCreation.Size = New System.Drawing.Size(63, 13)
        Me.LblDateCreation.TabIndex = 139
        Me.LblDateCreation.Text = "01/01/2000"
        '
        'LblLabelDateCreation
        '
        Me.LblLabelDateCreation.AutoSize = True
        Me.LblLabelDateCreation.Location = New System.Drawing.Point(12, 451)
        Me.LblLabelDateCreation.Name = "LblLabelDateCreation"
        Me.LblLabelDateCreation.Size = New System.Drawing.Size(48, 13)
        Me.LblLabelDateCreation.TabIndex = 138
        Me.LblLabelDateCreation.Text = "Créée le"
        '
        'RadBtnAnnuler
        '
        Me.RadBtnAnnuler.Location = New System.Drawing.Point(10, 484)
        Me.RadBtnAnnuler.Name = "RadBtnAnnuler"
        Me.RadBtnAnnuler.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAnnuler.TabIndex = 164
        Me.RadBtnAnnuler.Text = "Annuler la DRC"
        '
        'RadBtnTransformer
        '
        Me.RadBtnTransformer.Location = New System.Drawing.Point(126, 484)
        Me.RadBtnTransformer.Name = "RadBtnTransformer"
        Me.RadBtnTransformer.Size = New System.Drawing.Size(149, 24)
        Me.RadBtnTransformer.TabIndex = 165
        Me.RadBtnTransformer.Text = "Transformer DRC en DORC"
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Location = New System.Drawing.Point(608, 484)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 166
        Me.RadBtnValidation.Text = "Validation"
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(724, 484)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandonner.TabIndex = 167
        Me.RadBtnAbandonner.Text = "Abandonner"
        '
        'RadBtnParametre
        '
        Me.RadBtnParametre.Location = New System.Drawing.Point(281, 484)
        Me.RadBtnParametre.Name = "RadBtnParametre"
        Me.RadBtnParametre.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnParametre.TabIndex = 168
        Me.RadBtnParametre.Text = "Parametres"
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBox1.Controls.Add(Me.Label1)
        Me.RadGroupBox1.Controls.Add(Me.TxtCommentaire)
        Me.RadGroupBox1.Controls.Add(Me.LblDRCId)
        Me.RadGroupBox1.Controls.Add(Me.TxtLibelle)
        Me.RadGroupBox1.Controls.Add(Me.Label2)
        Me.RadGroupBox1.Controls.Add(Me.Label3)
        Me.RadGroupBox1.Controls.Add(Me.TxtId)
        Me.RadGroupBox1.Controls.Add(Me.CbxCategorieOasis)
        Me.RadGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox1.HeaderText = "DORC"
        Me.RadGroupBox1.Location = New System.Drawing.Point(10, 12)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(824, 213)
        Me.RadGroupBox1.TabIndex = 169
        Me.RadGroupBox1.Text = "DORC"
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBox2.Controls.Add(Me.LblCategorieMajeure)
        Me.RadGroupBox2.Controls.Add(Me.CbxCategorieMajeure)
        Me.RadGroupBox2.Controls.Add(Me.NumAgeMin)
        Me.RadGroupBox2.Controls.Add(Me.CbxSexe)
        Me.RadGroupBox2.Controls.Add(Me.NumAgeMax)
        Me.RadGroupBox2.Controls.Add(Me.Label5)
        Me.RadGroupBox2.Controls.Add(Me.Label6)
        Me.RadGroupBox2.Controls.Add(Me.LblALDDescription)
        Me.RadGroupBox2.Controls.Add(Me.Label7)
        Me.RadGroupBox2.Controls.Add(Me.Label10)
        Me.RadGroupBox2.Controls.Add(Me.TxtCIM10)
        Me.RadGroupBox2.Controls.Add(Me.TxtAld)
        Me.RadGroupBox2.Controls.Add(Me.TxtCISP)
        Me.RadGroupBox2.Controls.Add(Me.Label9)
        Me.RadGroupBox2.Controls.Add(Me.Label8)
        Me.RadGroupBox2.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox2.HeaderText = "Caractéristiques"
        Me.RadGroupBox2.Location = New System.Drawing.Point(10, 231)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(824, 209)
        Me.RadGroupBox2.TabIndex = 170
        Me.RadGroupBox2.Text = "Caractéristiques"
        '
        'RadBtnProtocole
        '
        Me.RadBtnProtocole.Location = New System.Drawing.Point(397, 484)
        Me.RadBtnProtocole.Name = "RadBtnProtocole"
        Me.RadBtnProtocole.Size = New System.Drawing.Size(146, 24)
        Me.RadBtnProtocole.TabIndex = 171
        Me.RadBtnProtocole.Text = "Associer Acte Paramedical"
        '
        'TxtCommentaire
        '
        Me.TxtCommentaire.Location = New System.Drawing.Point(144, 102)
        Me.TxtCommentaire.MaxLength = 5000
        Me.TxtCommentaire.Multiline = True
        Me.TxtCommentaire.Name = "TxtCommentaire"
        Me.TxtCommentaire.Size = New System.Drawing.Size(675, 106)
        Me.TxtCommentaire.TabIndex = 151
        '
        'TxtReponseCommentee
        '
        Me.TxtReponseCommentee.Location = New System.Drawing.Point(5, 21)
        Me.TxtReponseCommentee.Multiline = True
        Me.TxtReponseCommentee.Name = "TxtReponseCommentee"
        Me.TxtReponseCommentee.Size = New System.Drawing.Size(512, 402)
        Me.TxtReponseCommentee.TabIndex = 172
        '
        'RadGbxReponse
        '
        Me.RadGbxReponse.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGbxReponse.Controls.Add(Me.TxtReponseCommentee)
        Me.RadGbxReponse.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGbxReponse.HeaderText = "Réponse commentée"
        Me.RadGbxReponse.Location = New System.Drawing.Point(880, 12)
        Me.RadGbxReponse.Name = "RadGbxReponse"
        Me.RadGbxReponse.Size = New System.Drawing.Size(522, 428)
        Me.RadGbxReponse.TabIndex = 173
        Me.RadGbxReponse.Text = "Réponse commentée"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 114)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 152
        Me.Label1.Text = "Commentaire associé"
        '
        'RadFDrcDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandonner
        Me.ClientSize = New System.Drawing.Size(1409, 516)
        Me.Controls.Add(Me.RadGbxReponse)
        Me.Controls.Add(Me.RadBtnProtocole)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.RadBtnParametre)
        Me.Controls.Add(Me.RadBtnAbandonner)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnTransformer)
        Me.Controls.Add(Me.RadBtnAnnuler)
        Me.Controls.Add(Me.LblUtilisateurModification)
        Me.Controls.Add(Me.LblLabelUtilisateurModification)
        Me.Controls.Add(Me.LblDateModification)
        Me.Controls.Add(Me.LblLabelDateModification)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.LblLabelUtilisateurCreation)
        Me.Controls.Add(Me.LblDateCreation)
        Me.Controls.Add(Me.LblLabelDateCreation)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "RadFDrcDetailEdit"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Détail DRC"
        CType(Me.NumAgeMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumAgeMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAnnuler, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnTransformer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnParametre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.RadGroupBox2.PerformLayout()
        CType(Me.RadBtnProtocole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGbxReponse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGbxReponse.ResumeLayout(False)
        Me.RadGbxReponse.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LblALDDescription As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents TxtAld As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents TxtCISP As TextBox
    Friend WithEvents TxtCIM10 As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents NumAgeMax As NumericUpDown
    Friend WithEvents CbxSexe As ComboBox
    Friend WithEvents CbxCategorieOasis As ComboBox
    Friend WithEvents NumAgeMin As NumericUpDown
    Friend WithEvents LblCategorieMajeure As Label
    Friend WithEvents CbxCategorieMajeure As ComboBox
    Friend WithEvents TxtId As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TxtLibelle As TextBox
    Friend WithEvents LblDRCId As Label
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents LblLabelUtilisateurModification As Label
    Friend WithEvents LblDateModification As Label
    Friend WithEvents LblLabelDateModification As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblLabelUtilisateurCreation As Label
    Friend WithEvents LblDateCreation As Label
    Friend WithEvents LblLabelDateCreation As Label
    Friend WithEvents RadBtnAnnuler As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnTransformer As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnParametre As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadBtnProtocole As Telerik.WinControls.UI.RadButton
    Friend WithEvents TxtCommentaire As TextBox
    Friend WithEvents TxtReponseCommentee As TextBox
    Friend WithEvents RadGbxReponse As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents Label1 As Label
End Class

