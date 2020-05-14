<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFPatientNoteDetailEdit
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
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.TxtNote = New System.Windows.Forms.TextBox()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAnnuler = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientUniteSanitaire = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientSite = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.LblUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurModification = New System.Windows.Forms.Label()
        Me.LblDateModification = New System.Windows.Forms.Label()
        Me.LblLabelDateModification = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblLabelUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblDateCreation = New System.Windows.Forms.Label()
        Me.LblLabelDateCreation = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAnnuler, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.Controls.Add(Me.TxtNote)
        Me.RadGroupBox2.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox2.HeaderText = "Note patient"
        Me.RadGroupBox2.Location = New System.Drawing.Point(9, 97)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(778, 230)
        Me.RadGroupBox2.TabIndex = 76
        Me.RadGroupBox2.Text = "Note patient"
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'TxtNote
        '
        Me.TxtNote.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TxtNote.Location = New System.Drawing.Point(2, 18)
        Me.TxtNote.MaxLength = 4000
        Me.TxtNote.Multiline = True
        Me.TxtNote.Name = "TxtNote"
        Me.TxtNote.Size = New System.Drawing.Size(774, 210)
        Me.TxtNote.TabIndex = 77
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandonner.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandonner.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(759, 369)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandonner.TabIndex = 66
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.ForeColor = System.Drawing.Color.Black
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.Location = New System.Drawing.Point(12, 369)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 64
        Me.RadBtnValidation.Text = "Valider"
        Me.ToolTip.SetToolTip(Me.RadBtnValidation, "Validation")
        '
        'RadBtnAnnuler
        '
        Me.RadBtnAnnuler.ForeColor = System.Drawing.Color.Black
        Me.RadBtnAnnuler.Image = Global.Oasis_WF.My.Resources.Resources.supprimer
        Me.RadBtnAnnuler.Location = New System.Drawing.Point(128, 369)
        Me.RadBtnAnnuler.Name = "RadBtnAnnuler"
        Me.RadBtnAnnuler.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAnnuler.TabIndex = 65
        Me.RadBtnAnnuler.Text = "Annuler"
        Me.ToolTip.SetToolTip(Me.RadBtnAnnuler, "Annuler la nore")
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
        Me.RadGroupBox1.Location = New System.Drawing.Point(9, 16)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(776, 69)
        Me.RadGroupBox1.TabIndex = 75
        Me.RadGroupBox1.Text = "Patient"
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
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
        'LblPatientUniteSanitaire
        '
        Me.LblPatientUniteSanitaire.AutoSize = True
        Me.LblPatientUniteSanitaire.Location = New System.Drawing.Point(296, 40)
        Me.LblPatientUniteSanitaire.Name = "LblPatientUniteSanitaire"
        Me.LblPatientUniteSanitaire.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientUniteSanitaire.TabIndex = 9
        Me.LblPatientUniteSanitaire.Text = "Auxerre"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(333, 18)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 13)
        Me.LblPatientAge.TabIndex = 2
        Me.LblPatientAge.Text = "35 ans"
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
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(437, 18)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientGenre.TabIndex = 3
        Me.LblPatientGenre.Text = "Masculin"
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
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(589, 18)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 5
        Me.LblPatientNIR.Text = "1601275125143"
        '
        'LblUtilisateurModification
        '
        Me.LblUtilisateurModification.AutoSize = True
        Me.LblUtilisateurModification.Location = New System.Drawing.Point(534, 339)
        Me.LblUtilisateurModification.Name = "LblUtilisateurModification"
        Me.LblUtilisateurModification.Size = New System.Drawing.Size(101, 13)
        Me.LblUtilisateurModification.TabIndex = 74
        Me.LblUtilisateurModification.Text = "Georges Moustaki"
        '
        'LblLabelUtilisateurModification
        '
        Me.LblLabelUtilisateurModification.AutoSize = True
        Me.LblLabelUtilisateurModification.Location = New System.Drawing.Point(506, 339)
        Me.LblLabelUtilisateurModification.Name = "LblLabelUtilisateurModification"
        Me.LblLabelUtilisateurModification.Size = New System.Drawing.Size(24, 13)
        Me.LblLabelUtilisateurModification.TabIndex = 73
        Me.LblLabelUtilisateurModification.Text = "par"
        '
        'LblDateModification
        '
        Me.LblDateModification.AutoSize = True
        Me.LblDateModification.Location = New System.Drawing.Point(424, 339)
        Me.LblDateModification.Name = "LblDateModification"
        Me.LblDateModification.Size = New System.Drawing.Size(63, 13)
        Me.LblDateModification.TabIndex = 72
        Me.LblDateModification.Text = "01/01/2000"
        '
        'LblLabelDateModification
        '
        Me.LblLabelDateModification.AutoSize = True
        Me.LblLabelDateModification.Location = New System.Drawing.Point(360, 339)
        Me.LblLabelDateModification.Name = "LblLabelDateModification"
        Me.LblLabelDateModification.Size = New System.Drawing.Size(65, 13)
        Me.LblLabelDateModification.TabIndex = 71
        Me.LblLabelDateModification.Text = "Modifiée le"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(169, 339)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(98, 13)
        Me.LblUtilisateurCreation.TabIndex = 70
        Me.LblUtilisateurCreation.Text = "Stéphane Durand"
        '
        'LblLabelUtilisateurCreation
        '
        Me.LblLabelUtilisateurCreation.AutoSize = True
        Me.LblLabelUtilisateurCreation.Location = New System.Drawing.Point(140, 339)
        Me.LblLabelUtilisateurCreation.Name = "LblLabelUtilisateurCreation"
        Me.LblLabelUtilisateurCreation.Size = New System.Drawing.Size(24, 13)
        Me.LblLabelUtilisateurCreation.TabIndex = 69
        Me.LblLabelUtilisateurCreation.Text = "par"
        '
        'LblDateCreation
        '
        Me.LblDateCreation.AutoSize = True
        Me.LblDateCreation.Location = New System.Drawing.Point(66, 339)
        Me.LblDateCreation.Name = "LblDateCreation"
        Me.LblDateCreation.Size = New System.Drawing.Size(63, 13)
        Me.LblDateCreation.TabIndex = 68
        Me.LblDateCreation.Text = "01/01/2000"
        '
        'LblLabelDateCreation
        '
        Me.LblLabelDateCreation.AutoSize = True
        Me.LblLabelDateCreation.Location = New System.Drawing.Point(14, 339)
        Me.LblLabelDateCreation.Name = "LblLabelDateCreation"
        Me.LblLabelDateCreation.Size = New System.Drawing.Size(48, 13)
        Me.LblLabelDateCreation.TabIndex = 67
        Me.LblLabelDateCreation.Text = "Créée le"
        '
        'RadFPatientNoteDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandonner
        Me.ClientSize = New System.Drawing.Size(795, 408)
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
        Me.Name = "RadFPatientNoteDetailEdit"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Note patient"
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.RadGroupBox2.PerformLayout()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAnnuler, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAnnuler As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientUniteSanitaire As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientSite As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents LblUtilisateurModification As Label
    Friend WithEvents LblLabelUtilisateurModification As Label
    Friend WithEvents LblDateModification As Label
    Friend WithEvents LblLabelDateModification As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblLabelUtilisateurCreation As Label
    Friend WithEvents LblDateCreation As Label
    Friend WithEvents LblLabelDateCreation As Label
    Friend WithEvents TxtNote As TextBox
    Friend WithEvents ToolTip As ToolTip
End Class

