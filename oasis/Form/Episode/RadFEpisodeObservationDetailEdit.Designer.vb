<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFEpisodeObservationDetailEdit
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
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.LblModificationObs = New System.Windows.Forms.Label()
        Me.LblCreationObs2 = New System.Windows.Forms.Label()
        Me.LblUtilisateurCreation = New System.Windows.Forms.Label()
        Me.LblCreationObs1 = New System.Windows.Forms.Label()
        Me.LblObsDateCreation = New System.Windows.Forms.Label()
        Me.LblObsDateModification = New System.Windows.Forms.Label()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.CbxPresence = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtObservation = New System.Windows.Forms.TextBox()
        Me.LblCreationObs3 = New System.Windows.Forms.Label()
        Me.LblObsHeureCreation = New System.Windows.Forms.Label()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(846, 182)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 44
        Me.RadBtnAbandon.Text = "Abandonner"
        '
        'LblModificationObs
        '
        Me.LblModificationObs.AutoSize = True
        Me.LblModificationObs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblModificationObs.Location = New System.Drawing.Point(385, 185)
        Me.LblModificationObs.Name = "LblModificationObs"
        Me.LblModificationObs.Size = New System.Drawing.Size(70, 13)
        Me.LblModificationObs.TabIndex = 93
        Me.LblModificationObs.Text = "Modifié le :"
        '
        'LblCreationObs2
        '
        Me.LblCreationObs2.AutoSize = True
        Me.LblCreationObs2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCreationObs2.Location = New System.Drawing.Point(201, 185)
        Me.LblCreationObs2.Name = "LblCreationObs2"
        Me.LblCreationObs2.Size = New System.Drawing.Size(25, 13)
        Me.LblCreationObs2.TabIndex = 91
        Me.LblCreationObs2.Text = "par"
        '
        'LblUtilisateurCreation
        '
        Me.LblUtilisateurCreation.AutoSize = True
        Me.LblUtilisateurCreation.Location = New System.Drawing.Point(232, 185)
        Me.LblUtilisateurCreation.Name = "LblUtilisateurCreation"
        Me.LblUtilisateurCreation.Size = New System.Drawing.Size(121, 13)
        Me.LblUtilisateurCreation.TabIndex = 89
        Me.LblUtilisateurCreation.Text = "Utilisateur en création"
        '
        'LblCreationObs1
        '
        Me.LblCreationObs1.AutoSize = True
        Me.LblCreationObs1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCreationObs1.Location = New System.Drawing.Point(7, 185)
        Me.LblCreationObs1.Name = "LblCreationObs1"
        Me.LblCreationObs1.Size = New System.Drawing.Size(55, 13)
        Me.LblCreationObs1.TabIndex = 86
        Me.LblCreationObs1.Text = "Créé le :"
        '
        'LblObsDateCreation
        '
        Me.LblObsDateCreation.AutoSize = True
        Me.LblObsDateCreation.Location = New System.Drawing.Point(69, 185)
        Me.LblObsDateCreation.Name = "LblObsDateCreation"
        Me.LblObsDateCreation.Size = New System.Drawing.Size(61, 13)
        Me.LblObsDateCreation.TabIndex = 87
        Me.LblObsDateCreation.Text = "01.10.2019"
        '
        'LblObsDateModification
        '
        Me.LblObsDateModification.AutoSize = True
        Me.LblObsDateModification.Location = New System.Drawing.Point(454, 185)
        Me.LblObsDateModification.Name = "LblObsDateModification"
        Me.LblObsDateModification.Size = New System.Drawing.Size(61, 13)
        Me.LblObsDateModification.TabIndex = 88
        Me.LblObsDateModification.Text = "02.10.2019"
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnValidation.Location = New System.Drawing.Point(730, 182)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 45
        Me.RadBtnValidation.Text = "Validation"
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBox2.Controls.Add(Me.CbxPresence)
        Me.RadGroupBox2.Controls.Add(Me.Label8)
        Me.RadGroupBox2.Controls.Add(Me.Label7)
        Me.RadGroupBox2.Controls.Add(Me.TxtObservation)
        Me.RadGroupBox2.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox2.HeaderText = "Observation libre"
        Me.RadGroupBox2.Location = New System.Drawing.Point(4, 5)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(952, 171)
        Me.RadGroupBox2.TabIndex = 46
        Me.RadGroupBox2.Text = "Observation libre"
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupBox2.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'CbxPresence
        '
        Me.CbxPresence.FormattingEnabled = True
        Me.CbxPresence.Location = New System.Drawing.Point(159, 37)
        Me.CbxPresence.Name = "CbxPresence"
        Me.CbxPresence.Size = New System.Drawing.Size(146, 21)
        Me.CbxPresence.TabIndex = 20
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label8.Location = New System.Drawing.Point(11, 40)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(98, 13)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Nature présence :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label7.Location = New System.Drawing.Point(11, 67)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 13)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "Commentaire :"
        '
        'TxtObservation
        '
        Me.TxtObservation.Location = New System.Drawing.Point(160, 64)
        Me.TxtObservation.Multiline = True
        Me.TxtObservation.Name = "TxtObservation"
        Me.TxtObservation.Size = New System.Drawing.Size(787, 97)
        Me.TxtObservation.TabIndex = 17
        '
        'LblCreationObs3
        '
        Me.LblCreationObs3.AutoSize = True
        Me.LblCreationObs3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCreationObs3.Location = New System.Drawing.Point(136, 185)
        Me.LblCreationObs3.Name = "LblCreationObs3"
        Me.LblCreationObs3.Size = New System.Drawing.Size(14, 13)
        Me.LblCreationObs3.TabIndex = 94
        Me.LblCreationObs3.Text = "à"
        '
        'LblObsHeureCreation
        '
        Me.LblObsHeureCreation.AutoSize = True
        Me.LblObsHeureCreation.Location = New System.Drawing.Point(156, 185)
        Me.LblObsHeureCreation.Name = "LblObsHeureCreation"
        Me.LblObsHeureCreation.Size = New System.Drawing.Size(34, 13)
        Me.LblObsHeureCreation.TabIndex = 95
        Me.LblObsHeureCreation.Text = "00:00"
        '
        'RadFEpisodeObservationDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(959, 210)
        Me.Controls.Add(Me.LblObsHeureCreation)
        Me.Controls.Add(Me.LblCreationObs3)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.LblModificationObs)
        Me.Controls.Add(Me.LblCreationObs2)
        Me.Controls.Add(Me.LblUtilisateurCreation)
        Me.Controls.Add(Me.LblCreationObs1)
        Me.Controls.Add(Me.LblObsDateCreation)
        Me.Controls.Add(Me.LblObsDateModification)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFEpisodeObservationDetailEdit"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RadFEpisodeObservationDetailEdit"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.RadGroupBox2.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents LblModificationObs As Label
    Friend WithEvents LblCreationObs2 As Label
    Friend WithEvents LblUtilisateurCreation As Label
    Friend WithEvents LblCreationObs1 As Label
    Friend WithEvents LblObsDateCreation As Label
    Friend WithEvents LblObsDateModification As Label
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents CbxPresence As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents TxtObservation As TextBox
    Friend WithEvents LblCreationObs3 As Label
    Friend WithEvents LblObsHeureCreation As Label
End Class

