<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFEpisodeParametresCreation
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
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBoxEtatCivil = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblALD = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LblPatientDateMaj = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblPatientSite = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.RadioBtn45 = New System.Windows.Forms.RadioButton()
        Me.RadioBtn30 = New System.Windows.Forms.RadioButton()
        Me.RadioBtn15 = New System.Windows.Forms.RadioButton()
        Me.RadioBtn0 = New System.Windows.Forms.RadioButton()
        Me.LblLabelHeureRV = New System.Windows.Forms.Label()
        Me.NumheureRV = New System.Windows.Forms.NumericUpDown()
        Me.NumDateRV = New System.Windows.Forms.DateTimePicker()
        Me.TxtCommentaire = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBoxEtatCivil.SuspendLayout()
        CType(Me.NumheureRV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(933, 180)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 0
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.Location = New System.Drawing.Point(12, 180)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 1
        Me.RadBtnValidation.Text = "Valider"
        Me.ToolTip1.SetToolTip(Me.RadBtnValidation, "Validation de la création de l'épisode de paramètres")
        '
        'RadGroupBoxEtatCivil
        '
        Me.RadGroupBoxEtatCivil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBoxEtatCivil.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblALD)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label13)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientDateMaj)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label5)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientSite)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label4)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientPrenom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientGenre)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNIR)
        Me.RadGroupBoxEtatCivil.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBoxEtatCivil.HeaderText = ""
        Me.RadGroupBoxEtatCivil.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBoxEtatCivil.Name = "RadGroupBoxEtatCivil"
        Me.RadGroupBoxEtatCivil.Size = New System.Drawing.Size(945, 38)
        Me.RadGroupBoxEtatCivil.TabIndex = 5
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.GroupBoxContent).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        '
        'LblALD
        '
        Me.LblALD.AutoSize = True
        Me.LblALD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblALD.ForeColor = System.Drawing.Color.OrangeRed
        Me.LblALD.Location = New System.Drawing.Point(900, 4)
        Me.LblALD.Name = "LblALD"
        Me.LblALD.Size = New System.Drawing.Size(31, 13)
        Me.LblALD.TabIndex = 43
        Me.LblALD.Text = "ALD"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(510, 4)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 42
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(801, 4)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientDateMaj.TabIndex = 41
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(667, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(710, 21)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(36, 13)
        Me.LblPatientSite.TabIndex = 37
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(667, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Site :"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(11, 4)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientPrenom.TabIndex = 23
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(133, 4)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientNom.TabIndex = 24
        Me.LblPatientNom.Text = "Durand"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(367, 4)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 13)
        Me.LblPatientAge.TabIndex = 25
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(427, 4)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientGenre.TabIndex = 26
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(561, 4)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 27
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'RadioBtn45
        '
        Me.RadioBtn45.AutoSize = True
        Me.RadioBtn45.Location = New System.Drawing.Point(775, 127)
        Me.RadioBtn45.Name = "RadioBtn45"
        Me.RadioBtn45.Size = New System.Drawing.Size(81, 17)
        Me.RadioBtn45.TabIndex = 28
        Me.RadioBtn45.TabStop = True
        Me.RadioBtn45.Text = "45 minutes"
        Me.RadioBtn45.UseVisualStyleBackColor = True
        '
        'RadioBtn30
        '
        Me.RadioBtn30.AutoSize = True
        Me.RadioBtn30.Location = New System.Drawing.Point(672, 127)
        Me.RadioBtn30.Name = "RadioBtn30"
        Me.RadioBtn30.Size = New System.Drawing.Size(81, 17)
        Me.RadioBtn30.TabIndex = 27
        Me.RadioBtn30.TabStop = True
        Me.RadioBtn30.Text = "30 minutes"
        Me.RadioBtn30.UseVisualStyleBackColor = True
        '
        'RadioBtn15
        '
        Me.RadioBtn15.AutoSize = True
        Me.RadioBtn15.Location = New System.Drawing.Point(569, 127)
        Me.RadioBtn15.Name = "RadioBtn15"
        Me.RadioBtn15.Size = New System.Drawing.Size(81, 17)
        Me.RadioBtn15.TabIndex = 26
        Me.RadioBtn15.TabStop = True
        Me.RadioBtn15.Text = "15 minutes"
        Me.RadioBtn15.UseVisualStyleBackColor = True
        '
        'RadioBtn0
        '
        Me.RadioBtn0.AutoSize = True
        Me.RadioBtn0.Location = New System.Drawing.Point(470, 127)
        Me.RadioBtn0.Name = "RadioBtn0"
        Me.RadioBtn0.Size = New System.Drawing.Size(73, 17)
        Me.RadioBtn0.TabIndex = 25
        Me.RadioBtn0.TabStop = True
        Me.RadioBtn0.Text = "O minute"
        Me.RadioBtn0.UseVisualStyleBackColor = True
        '
        'LblLabelHeureRV
        '
        Me.LblLabelHeureRV.AutoSize = True
        Me.LblLabelHeureRV.Location = New System.Drawing.Point(371, 126)
        Me.LblLabelHeureRV.Name = "LblLabelHeureRV"
        Me.LblLabelHeureRV.Size = New System.Drawing.Size(38, 13)
        Me.LblLabelHeureRV.TabIndex = 24
        Me.LblLabelHeureRV.Text = "Heure"
        '
        'NumheureRV
        '
        Me.NumheureRV.Location = New System.Drawing.Point(415, 124)
        Me.NumheureRV.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
        Me.NumheureRV.Name = "NumheureRV"
        Me.NumheureRV.Size = New System.Drawing.Size(48, 20)
        Me.NumheureRV.TabIndex = 23
        '
        'NumDateRV
        '
        Me.NumDateRV.Location = New System.Drawing.Point(148, 125)
        Me.NumDateRV.Name = "NumDateRV"
        Me.NumDateRV.Size = New System.Drawing.Size(200, 20)
        Me.NumDateRV.TabIndex = 22
        '
        'TxtCommentaire
        '
        Me.TxtCommentaire.Location = New System.Drawing.Point(148, 56)
        Me.TxtCommentaire.Multiline = True
        Me.TxtCommentaire.Name = "TxtCommentaire"
        Me.TxtCommentaire.Size = New System.Drawing.Size(809, 56)
        Me.TxtCommentaire.TabIndex = 29
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Commentaire"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 129)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Date et heure de saisie"
        '
        'RadFEpisodeParametresCreation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(967, 212)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtCommentaire)
        Me.Controls.Add(Me.RadioBtn45)
        Me.Controls.Add(Me.RadioBtn30)
        Me.Controls.Add(Me.RadioBtn15)
        Me.Controls.Add(Me.RadioBtn0)
        Me.Controls.Add(Me.LblLabelHeureRV)
        Me.Controls.Add(Me.NumheureRV)
        Me.Controls.Add(Me.NumDateRV)
        Me.Controls.Add(Me.RadGroupBoxEtatCivil)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFEpisodeParametresCreation"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Création épisode de saisie de paramètres"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBoxEtatCivil.ResumeLayout(False)
        Me.RadGroupBoxEtatCivil.PerformLayout()
        CType(Me.NumheureRV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBoxEtatCivil As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblALD As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents LblPatientDateMaj As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LblPatientSite As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents RadioBtn45 As RadioButton
    Friend WithEvents RadioBtn30 As RadioButton
    Friend WithEvents RadioBtn15 As RadioButton
    Friend WithEvents RadioBtn0 As RadioButton
    Friend WithEvents LblLabelHeureRV As Label
    Friend WithEvents NumheureRV As NumericUpDown
    Friend WithEvents NumDateRV As DateTimePicker
    Friend WithEvents TxtCommentaire As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class

