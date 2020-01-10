<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FmPatientNoteDetailEdit
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
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblDateModification = New System.Windows.Forms.Label()
        Me.LblLabelDateModification = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblDateCreation = New System.Windows.Forms.Label()
        Me.LblLabelDateCreation = New System.Windows.Forms.Label()
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
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadBtnAnnuler = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        Me.RadTxtNote = New Telerik.WinControls.UI.RadRichTextEditor()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadBtnAnnuler, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadTxtNote, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(537, 335)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(93, 13)
        Me.LblUtilisateurModification.TabIndex = 56
        Me.LblUtilisateurModification.Text = "Georges Moustaki"
        '
        'LblLabelUtilisateurModification
        '
        Me.LblLabelUtilisateurModification.AutoSize = True
        Me.LblLabelUtilisateurModification.Location = New System.Drawing.Point(509, 335)
        Me.LblLabelUtilisateurModification.Name = "LblLabelUtilisateurModification"
        Me.LblLabelUtilisateurModification.Size = New System.Drawing.Size(22, 13)
        Me.LblLabelUtilisateurModification.TabIndex = 55
        Me.LblLabelUtilisateurModification.Text = "par"
        '
        'LblDateModification
        '
        Me.LblDateModification.AutoSize = True
        Me.LblDateModification.Location = New System.Drawing.Point(427, 335)
        Me.LblDateModification.Name = "LblDateModification"
        Me.LblDateModification.Size = New System.Drawing.Size(65, 13)
        Me.LblDateModification.TabIndex = 54
        Me.LblDateModification.Text = "01/01/2000"
        '
        'LblLabelDateModification
        '
        Me.LblLabelDateModification.AutoSize = True
        Me.LblLabelDateModification.Location = New System.Drawing.Point(363, 335)
        Me.LblLabelDateModification.Name = "LblLabelDateModification"
        Me.LblLabelDateModification.Size = New System.Drawing.Size(58, 13)
        Me.LblLabelDateModification.TabIndex = 53
        Me.LblLabelDateModification.Text = "Modifiée le"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(172, 335)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(91, 13)
        Me.LblUtilisateurCreation.TabIndex = 52
        Me.LblUtilisateurCreation.Text = "Stéphane Durand"
        '
        'LblLabelUtilisateurCreation
        '
        Me.LblLabelUtilisateurCreation.AutoSize = True
        Me.LblLabelUtilisateurCreation.Location = New System.Drawing.Point(143, 335)
        Me.LblLabelUtilisateurCreation.Name = "LblLabelUtilisateurCreation"
        Me.LblLabelUtilisateurCreation.Size = New System.Drawing.Size(22, 13)
        Me.LblLabelUtilisateurCreation.TabIndex = 51
        Me.LblLabelUtilisateurCreation.Text = "par"
        '
        'LblDateCreation
        '
        Me.LblDateCreation.AutoSize = True
        Me.LblDateCreation.Location = New System.Drawing.Point(69, 335)
        Me.LblDateCreation.Name = "LblDateCreation"
        Me.LblDateCreation.Size = New System.Drawing.Size(65, 13)
        Me.LblDateCreation.TabIndex = 50
        Me.LblDateCreation.Text = "01/01/2000"
        '
        'LblLabelDateCreation
        '
        Me.LblLabelDateCreation.AutoSize = True
        Me.LblLabelDateCreation.Location = New System.Drawing.Point(17, 335)
        Me.LblLabelDateCreation.Name = "LblLabelDateCreation"
        Me.LblLabelDateCreation.Size = New System.Drawing.Size(46, 13)
        Me.LblLabelDateCreation.TabIndex = 49
        Me.LblLabelDateCreation.Text = "Créée le"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(5, 18)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(56, 13)
        Me.LblPatientPrenom.TabIndex = 0
        Me.LblPatientPrenom.Text = "Jean-Paul"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(119, 18)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientNom.TabIndex = 1
        Me.LblPatientNom.Text = "Durand"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(228, 18)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 13)
        Me.LblPatientAge.TabIndex = 2
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(332, 18)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientGenre.TabIndex = 3
        Me.LblPatientGenre.Text = "Masculin"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(518, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "NIR :"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(589, 18)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 5
        Me.LblPatientNIR.Text = "1601275125143"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(5, 40)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(37, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Site :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(48, 40)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(36, 13)
        Me.LblPatientSite.TabIndex = 7
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(177, 40)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(113, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Site de référence :"
        '
        'LblPatientUniteSanitaire
        '
        Me.LblPatientUniteSanitaire.AutoSize = True
        Me.LblPatientUniteSanitaire.Location = New System.Drawing.Point(296, 40)
        Me.LblPatientUniteSanitaire.Name = "LblPatientUniteSanitaire"
        Me.LblPatientUniteSanitaire.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientUniteSanitaire.TabIndex = 9
        Me.LblPatientUniteSanitaire.Text = "Auxerre"
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.LblPatientPrenom)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientUniteSanitaire)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBox1.Controls.Add(Me.Label9)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientGenre)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientSite)
        Me.RadGroupBox1.Controls.Add(Me.Label5)
        Me.RadGroupBox1.Controls.Add(Me.Label7)
        Me.RadGroupBox1.Controls.Add(Me.LblPatientNIR)
        Me.RadGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox1.HeaderText = "Patient"
        Me.RadGroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(747, 69)
        Me.RadGroupBox1.TabIndex = 58
        Me.RadGroupBox1.Text = "Patient"
        '
        'RadBtnAnnuler
        '
        Me.RadBtnAnnuler.Location = New System.Drawing.Point(12, 365)
        Me.RadBtnAnnuler.Name = "RadBtnAnnuler"
        Me.RadBtnAnnuler.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAnnuler.TabIndex = 10
        Me.RadBtnAnnuler.Text = "Annuler la note"
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Location = New System.Drawing.Point(533, 365)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 5
        Me.RadBtnValidation.Text = "Validation"
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(649, 365)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandonner.TabIndex = 15
        Me.RadBtnAbandonner.Text = "Abandonner"
        '
        'RadTxtNote
        '
        Me.RadTxtNote.BorderColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.RadTxtNote.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadTxtNote.Location = New System.Drawing.Point(2, 18)
        Me.RadTxtNote.Name = "RadTxtNote"
        '
        '
        '
        Me.RadTxtNote.RootElement.CustomFontSize = 8.0!
        Me.RadTxtNote.SelectionFill = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadTxtNote.Size = New System.Drawing.Size(743, 210)
        Me.RadTxtNote.TabIndex = 1
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.Controls.Add(Me.RadTxtNote)
        Me.RadGroupBox2.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox2.HeaderText = "Note patient"
        Me.RadGroupBox2.Location = New System.Drawing.Point(12, 93)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(747, 230)
        Me.RadGroupBox2.TabIndex = 63
        Me.RadGroupBox2.Text = "Note patient"
        '
        'FmPatientNoteDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(773, 397)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Controls.Add(Me.RadBtnAbandonner)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnAnnuler)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.LblUtilisateurModification)
        Me.Controls.Add(Me.LblLabelUtilisateurModification)
        Me.Controls.Add(Me.LblDateModification)
        Me.Controls.Add(Me.LblLabelDateModification)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.LblLabelUtilisateurCreation)
        Me.Controls.Add(Me.LblDateCreation)
        Me.Controls.Add(Me.LblLabelDateCreation)
        Me.Name = "FmPatientNoteDetailEdit"
        Me.Text = "Note patient"
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.RadBtnAnnuler, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadTxtNote, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents LblLabelUtilisateurModification As Label
    Friend WithEvents LblDateModification As Label
    Friend WithEvents LblLabelDateModification As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblLabelUtilisateurCreation As Label
    Friend WithEvents LblDateCreation As Label
    Friend WithEvents LblLabelDateCreation As Label
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
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadBtnAnnuler As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadTxtNote As Telerik.WinControls.UI.RadRichTextEditor
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
End Class
