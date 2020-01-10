<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FDRCDetailEdit
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
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.BtnValidation = New System.Windows.Forms.Button()
        Me.LblLabelDateCreation = New System.Windows.Forms.Label()
        Me.LblDateCreation = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblLabelDateModification = New System.Windows.Forms.Label()
        Me.LblDateModification = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtLibelle = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtId = New System.Windows.Forms.TextBox()
        Me.CbxCategorieMajeure = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.NumAgeMin = New System.Windows.Forms.NumericUpDown()
        Me.CbxCategorieOasis = New System.Windows.Forms.ComboBox()
        Me.CbxSexe = New System.Windows.Forms.ComboBox()
        Me.NumAgeMax = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.BtnAnnuler = New System.Windows.Forms.Button()
        Me.BtnTransformer = New System.Windows.Forms.Button()
        Me.TxtCIM10 = New System.Windows.Forms.TextBox()
        Me.TxtCISP = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TxtAld = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.LblALDDescription = New System.Windows.Forms.Label()
        CType(Me.NumAgeMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumAgeMax, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(757, 333)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 130
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(676, 333)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(75, 23)
        Me.BtnValidation.TabIndex = 120
        Me.BtnValidation.Text = "Validation"
        Me.BtnValidation.UseVisualStyleBackColor = True
        '
        'LblLabelDateCreation
        '
        Me.LblLabelDateCreation.AutoSize = True
        Me.LblLabelDateCreation.Location = New System.Drawing.Point(9, 301)
        Me.LblLabelDateCreation.Name = "LblLabelDateCreation"
        Me.LblLabelDateCreation.Size = New System.Drawing.Size(46, 13)
        Me.LblLabelDateCreation.TabIndex = 36
        Me.LblLabelDateCreation.Text = "Créée le"
        '
        'LblDateCreation
        '
        Me.LblDateCreation.AutoSize = True
        Me.LblDateCreation.Location = New System.Drawing.Point(61, 301)
        Me.LblDateCreation.Name = "LblDateCreation"
        Me.LblDateCreation.Size = New System.Drawing.Size(65, 13)
        Me.LblDateCreation.TabIndex = 37
        Me.LblDateCreation.Text = "01/01/2000"
        '
        'LblLabelUtilisateurCreation
        '
        Me.LblLabelUtilisateurCreation.AutoSize = True
        Me.LblLabelUtilisateurCreation.Location = New System.Drawing.Point(135, 301)
        Me.LblLabelUtilisateurCreation.Name = "LblLabelUtilisateurCreation"
        Me.LblLabelUtilisateurCreation.Size = New System.Drawing.Size(22, 13)
        Me.LblLabelUtilisateurCreation.TabIndex = 38
        Me.LblLabelUtilisateurCreation.Text = "par"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(164, 301)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(91, 13)
        Me.LblUtilisateurCreation.TabIndex = 39
        Me.LblUtilisateurCreation.Text = "Stéphane Durand"
        '
        'LblLabelDateModification
        '
        Me.LblLabelDateModification.AutoSize = True
        Me.LblLabelDateModification.Location = New System.Drawing.Point(428, 301)
        Me.LblLabelDateModification.Name = "LblLabelDateModification"
        Me.LblLabelDateModification.Size = New System.Drawing.Size(58, 13)
        Me.LblLabelDateModification.TabIndex = 40
        Me.LblLabelDateModification.Text = "Modifiée le"
        '
        'LblDateModification
        '
        Me.LblDateModification.AutoSize = True
        Me.LblDateModification.Location = New System.Drawing.Point(492, 301)
        Me.LblDateModification.Name = "LblDateModification"
        Me.LblDateModification.Size = New System.Drawing.Size(65, 13)
        Me.LblDateModification.TabIndex = 41
        Me.LblDateModification.Text = "01/01/2000"
        '
        'LblLabelUtilisateurModification
        '
        Me.LblLabelUtilisateurModification.AutoSize = True
        Me.LblLabelUtilisateurModification.Location = New System.Drawing.Point(574, 301)
        Me.LblLabelUtilisateurModification.Name = "LblLabelUtilisateurModification"
        Me.LblLabelUtilisateurModification.Size = New System.Drawing.Size(22, 13)
        Me.LblLabelUtilisateurModification.TabIndex = 42
        Me.LblLabelUtilisateurModification.Text = "par"
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(602, 301)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(93, 13)
        Me.LblUtilisateurModification.TabIndex = 43
        Me.LblUtilisateurModification.Text = "Georges Moustaki"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 13)
        Me.Label1.TabIndex = 44
        Me.Label1.Text = "Identifiant DRC/ORC"
        '
        'TxtLibelle
        '
        Me.TxtLibelle.Location = New System.Drawing.Point(151, 37)
        Me.TxtLibelle.MaxLength = 150
        Me.TxtLibelle.Name = "TxtLibelle"
        Me.TxtLibelle.Size = New System.Drawing.Size(573, 20)
        Me.TxtLibelle.TabIndex = 10
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 46
        Me.Label2.Text = "Description"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 13)
        Me.Label3.TabIndex = 48
        Me.Label3.Text = "Catégorie Oasis"
        '
        'TxtId
        '
        Me.TxtId.Location = New System.Drawing.Point(151, 12)
        Me.TxtId.Name = "TxtId"
        Me.TxtId.ReadOnly = True
        Me.TxtId.Size = New System.Drawing.Size(67, 20)
        Me.TxtId.TabIndex = 1
        '
        'CbxCategorieMajeure
        '
        Me.CbxCategorieMajeure.FormattingEnabled = True
        Me.CbxCategorieMajeure.Location = New System.Drawing.Point(151, 88)
        Me.CbxCategorieMajeure.Name = "CbxCategorieMajeure"
        Me.CbxCategorieMajeure.Size = New System.Drawing.Size(573, 21)
        Me.CbxCategorieMajeure.TabIndex = 20
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 91)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 13)
        Me.Label4.TabIndex = 51
        Me.Label4.Text = "Catégorie majeure"
        '
        'NumAgeMin
        '
        Me.NumAgeMin.Location = New System.Drawing.Point(151, 140)
        Me.NumAgeMin.Name = "NumAgeMin"
        Me.NumAgeMin.Size = New System.Drawing.Size(67, 20)
        Me.NumAgeMin.TabIndex = 30
        '
        'CbxCategorieOasis
        '
        Me.CbxCategorieOasis.FormattingEnabled = True
        Me.CbxCategorieOasis.Location = New System.Drawing.Point(151, 62)
        Me.CbxCategorieOasis.Name = "CbxCategorieOasis"
        Me.CbxCategorieOasis.Size = New System.Drawing.Size(204, 21)
        Me.CbxCategorieOasis.TabIndex = 15
        '
        'CbxSexe
        '
        Me.CbxSexe.FormattingEnabled = True
        Me.CbxSexe.Location = New System.Drawing.Point(151, 114)
        Me.CbxSexe.Name = "CbxSexe"
        Me.CbxSexe.Size = New System.Drawing.Size(121, 21)
        Me.CbxSexe.TabIndex = 25
        '
        'NumAgeMax
        '
        Me.NumAgeMax.Location = New System.Drawing.Point(151, 165)
        Me.NumAgeMax.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
        Me.NumAgeMax.Name = "NumAgeMax"
        Me.NumAgeMax.Size = New System.Drawing.Size(67, 20)
        Me.NumAgeMax.TabIndex = 35
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 117)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(80, 13)
        Me.Label5.TabIndex = 56
        Me.Label5.Text = "Applicable pour"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(16, 142)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 57
        Me.Label6.Text = "Age min"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(16, 167)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(48, 13)
        Me.Label7.TabIndex = 58
        Me.Label7.Text = "Age max"
        '
        'BtnAnnuler
        '
        Me.BtnAnnuler.Location = New System.Drawing.Point(12, 333)
        Me.BtnAnnuler.Name = "BtnAnnuler"
        Me.BtnAnnuler.Size = New System.Drawing.Size(75, 23)
        Me.BtnAnnuler.TabIndex = 100
        Me.BtnAnnuler.Text = "Supprimer"
        Me.BtnAnnuler.UseVisualStyleBackColor = True
        '
        'BtnTransformer
        '
        Me.BtnTransformer.Location = New System.Drawing.Point(93, 333)
        Me.BtnTransformer.Name = "BtnTransformer"
        Me.BtnTransformer.Size = New System.Drawing.Size(144, 23)
        Me.BtnTransformer.TabIndex = 110
        Me.BtnTransformer.Text = "Transformer DRC en ORC"
        Me.BtnTransformer.UseVisualStyleBackColor = True
        '
        'TxtCIM10
        '
        Me.TxtCIM10.Location = New System.Drawing.Point(151, 190)
        Me.TxtCIM10.MaxLength = 10
        Me.TxtCIM10.Name = "TxtCIM10"
        Me.TxtCIM10.Size = New System.Drawing.Size(86, 20)
        Me.TxtCIM10.TabIndex = 40
        '
        'TxtCISP
        '
        Me.TxtCISP.Location = New System.Drawing.Point(151, 215)
        Me.TxtCISP.MaxLength = 10
        Me.TxtCISP.Name = "TxtCISP"
        Me.TxtCISP.Size = New System.Drawing.Size(87, 20)
        Me.TxtCISP.TabIndex = 45
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(16, 193)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 13)
        Me.Label8.TabIndex = 63
        Me.Label8.Text = "Code CIM10"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(16, 218)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(59, 13)
        Me.Label9.TabIndex = 64
        Me.Label9.Text = "Code CISP"
        '
        'TxtAld
        '
        Me.TxtAld.Location = New System.Drawing.Point(151, 240)
        Me.TxtAld.MaxLength = 11
        Me.TxtAld.Name = "TxtAld"
        Me.TxtAld.Size = New System.Drawing.Size(86, 20)
        Me.TxtAld.TabIndex = 50
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(16, 243)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(56, 13)
        Me.Label10.TabIndex = 66
        Me.Label10.Text = "Code ALD"
        '
        'LblALDDescription
        '
        Me.LblALDDescription.AutoSize = True
        Me.LblALDDescription.Location = New System.Drawing.Point(254, 243)
        Me.LblALDDescription.Name = "LblALDDescription"
        Me.LblALDDescription.Size = New System.Drawing.Size(84, 13)
        Me.LblALDDescription.TabIndex = 67
        Me.LblALDDescription.Text = "Description ALD"
        '
        'FDRCDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(844, 368)
        Me.Controls.Add(Me.LblALDDescription)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.TxtAld)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TxtCISP)
        Me.Controls.Add(Me.TxtCIM10)
        Me.Controls.Add(Me.BtnTransformer)
        Me.Controls.Add(Me.BtnAnnuler)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.NumAgeMax)
        Me.Controls.Add(Me.CbxSexe)
        Me.Controls.Add(Me.CbxCategorieOasis)
        Me.Controls.Add(Me.NumAgeMin)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.CbxCategorieMajeure)
        Me.Controls.Add(Me.TxtId)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtLibelle)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblUtilisateurModification)
        Me.Controls.Add(Me.LblLabelUtilisateurModification)
        Me.Controls.Add(Me.LblDateModification)
        Me.Controls.Add(Me.LblLabelDateModification)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.LblLabelUtilisateurCreation)
        Me.Controls.Add(Me.LblDateCreation)
        Me.Controls.Add(Me.LblLabelDateCreation)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Name = "FDRCDetailEdit"
        Me.Text = "Gestion DRC/ORC"
        CType(Me.NumAgeMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumAgeMax, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents BtnValidation As Button
    Friend WithEvents LblLabelDateCreation As Label
    Friend WithEvents LblDateCreation As Label
    Friend WithEvents LblLabelUtilisateurCreation As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblLabelDateModification As Label
    Friend WithEvents LblDateModification As Label
    Friend WithEvents LblLabelUtilisateurModification As Label
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtLibelle As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents TxtId As TextBox
    Friend WithEvents CbxCategorieMajeure As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents NumAgeMin As NumericUpDown
    Friend WithEvents CbxCategorieOasis As ComboBox
    Friend WithEvents CbxSexe As ComboBox
    Friend WithEvents NumAgeMax As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents BtnAnnuler As Button
    Friend WithEvents BtnTransformer As Button
    Friend WithEvents TxtCIM10 As TextBox
    Friend WithEvents TxtCISP As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents TxtAld As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents LblALDDescription As Label
End Class
