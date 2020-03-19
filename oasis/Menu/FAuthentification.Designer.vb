<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FAuthentificattion
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
        Me.components = New System.ComponentModel.Container()
        Me.RbtAdminOui = New System.Windows.Forms.RadioButton()
        Me.RbtAdminNon = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CbxUtilisateur = New System.Windows.Forms.ComboBox()
        Me.BtnValidation = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BtnAbandon = New System.Windows.Forms.Button()
        Me.BtnListePatient = New System.Windows.Forms.Button()
        Me.BtnAdmin = New System.Windows.Forms.Button()
        Me.RadDesktopAlert1 = New Telerik.WinControls.UI.RadDesktopAlert(Me.components)
        Me.BtnTheriaque = New System.Windows.Forms.Button()
        Me.BtnTemplateSsEpisode = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RbtAdminOui
        '
        Me.RbtAdminOui.AutoSize = True
        Me.RbtAdminOui.Location = New System.Drawing.Point(26, 23)
        Me.RbtAdminOui.Name = "RbtAdminOui"
        Me.RbtAdminOui.Size = New System.Drawing.Size(41, 17)
        Me.RbtAdminOui.TabIndex = 0
        Me.RbtAdminOui.TabStop = True
        Me.RbtAdminOui.Text = "Oui"
        Me.RbtAdminOui.UseVisualStyleBackColor = True
        '
        'RbtAdminNon
        '
        Me.RbtAdminNon.AutoSize = True
        Me.RbtAdminNon.Location = New System.Drawing.Point(134, 23)
        Me.RbtAdminNon.Name = "RbtAdminNon"
        Me.RbtAdminNon.Size = New System.Drawing.Size(45, 17)
        Me.RbtAdminNon.TabIndex = 1
        Me.RbtAdminNon.TabStop = True
        Me.RbtAdminNon.Text = "Non"
        Me.RbtAdminNon.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Utilisateur"
        '
        'CbxUtilisateur
        '
        Me.CbxUtilisateur.FormattingEnabled = True
        Me.CbxUtilisateur.Items.AddRange(New Object() {"7 - Informaticien", "2 - Médecin", "5 - IDE", "8 - Secrétaire médicale", "6 - Secrétaire administrative"})
        Me.CbxUtilisateur.Location = New System.Drawing.Point(135, 73)
        Me.CbxUtilisateur.Name = "CbxUtilisateur"
        Me.CbxUtilisateur.Size = New System.Drawing.Size(210, 21)
        Me.CbxUtilisateur.TabIndex = 8
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(20, 116)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(117, 23)
        Me.BtnValidation.TabIndex = 9
        Me.BtnValidation.Text = "Liste des tâches"
        Me.BtnValidation.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RbtAdminNon)
        Me.GroupBox1.Controls.Add(Me.RbtAdminOui)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(223, 49)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Admin"
        '
        'BtnAbandon
        '
        Me.BtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnAbandon.Location = New System.Drawing.Point(261, 267)
        Me.BtnAbandon.Name = "BtnAbandon"
        Me.BtnAbandon.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandon.TabIndex = 13
        Me.BtnAbandon.Text = "Abandon"
        Me.BtnAbandon.UseVisualStyleBackColor = True
        '
        'BtnListePatient
        '
        Me.BtnListePatient.Location = New System.Drawing.Point(20, 145)
        Me.BtnListePatient.Name = "BtnListePatient"
        Me.BtnListePatient.Size = New System.Drawing.Size(117, 23)
        Me.BtnListePatient.TabIndex = 14
        Me.BtnListePatient.Text = "Liste patients"
        Me.BtnListePatient.UseVisualStyleBackColor = True
        '
        'BtnAdmin
        '
        Me.BtnAdmin.Location = New System.Drawing.Point(20, 174)
        Me.BtnAdmin.Name = "BtnAdmin"
        Me.BtnAdmin.Size = New System.Drawing.Size(117, 23)
        Me.BtnAdmin.TabIndex = 15
        Me.BtnAdmin.Text = "Admin"
        Me.BtnAdmin.UseVisualStyleBackColor = True
        '
        'RadDesktopAlert1
        '
        Me.RadDesktopAlert1.AutoCloseDelay = 5
        '
        'BtnTheriaque
        '
        Me.BtnTheriaque.Location = New System.Drawing.Point(20, 238)
        Me.BtnTheriaque.Name = "BtnTheriaque"
        Me.BtnTheriaque.Size = New System.Drawing.Size(117, 23)
        Me.BtnTheriaque.TabIndex = 16
        Me.BtnTheriaque.Text = "Thériaque"
        Me.BtnTheriaque.UseVisualStyleBackColor = True
        '
        'BtnTemplateSsEpisode
        '
        Me.BtnTemplateSsEpisode.Location = New System.Drawing.Point(20, 275)
        Me.BtnTemplateSsEpisode.Name = "BtnTemplateSsEpisode"
        Me.BtnTemplateSsEpisode.Size = New System.Drawing.Size(117, 23)
        Me.BtnTemplateSsEpisode.TabIndex = 17
        Me.BtnTemplateSsEpisode.Text = "TemplateSousEpisode"
        Me.BtnTemplateSsEpisode.UseVisualStyleBackColor = True
        '
        'FAuthentificattion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(372, 310)
        Me.Controls.Add(Me.BtnTemplateSsEpisode)
        Me.Controls.Add(Me.BtnTheriaque)
        Me.Controls.Add(Me.BtnAdmin)
        Me.Controls.Add(Me.BtnListePatient)
        Me.Controls.Add(Me.BtnAbandon)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.CbxUtilisateur)
        Me.Controls.Add(Me.Label3)
        Me.Name = "FAuthentificattion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Authentification"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RbtAdminOui As RadioButton
    Friend WithEvents RbtAdminNon As RadioButton
    Friend WithEvents Label3 As Label
    Friend WithEvents CbxUtilisateur As ComboBox
    Friend WithEvents BtnValidation As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents BtnAbandon As Button
    Friend WithEvents BtnListePatient As Button
    Friend WithEvents BtnAdmin As Button
    Friend WithEvents RadDesktopAlert1 As Telerik.WinControls.UI.RadDesktopAlert
    Friend WithEvents BtnTheriaque As Button
    Friend WithEvents BtnTemplateSsEpisode As Button
End Class
