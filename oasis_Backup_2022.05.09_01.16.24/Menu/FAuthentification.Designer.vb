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
        Me.BtnTest = New System.Windows.Forms.Button()
        Me.BtnTemplateSsEpisode = New System.Windows.Forms.Button()
        Me.BtnLogin = New System.Windows.Forms.Button()
        Me.BtnDebloque = New System.Windows.Forms.Button()
        Me.LblNbTry = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RbtAdminOui
        '
        Me.RbtAdminOui.AutoSize = True
        Me.RbtAdminOui.Location = New System.Drawing.Point(8, 35)
        Me.RbtAdminOui.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.RbtAdminOui.Name = "RbtAdminOui"
        Me.RbtAdminOui.Size = New System.Drawing.Size(58, 24)
        Me.RbtAdminOui.TabIndex = 0
        Me.RbtAdminOui.TabStop = True
        Me.RbtAdminOui.Text = "Oui"
        Me.RbtAdminOui.UseVisualStyleBackColor = True
        '
        'RbtAdminNon
        '
        Me.RbtAdminNon.AutoSize = True
        Me.RbtAdminNon.Location = New System.Drawing.Point(88, 35)
        Me.RbtAdminNon.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.RbtAdminNon.Name = "RbtAdminNon"
        Me.RbtAdminNon.Size = New System.Drawing.Size(63, 24)
        Me.RbtAdminNon.TabIndex = 1
        Me.RbtAdminNon.TabStop = True
        Me.RbtAdminNon.Text = "Non"
        Me.RbtAdminNon.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 115)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 20)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Utilisateur:"
        '
        'CbxUtilisateur
        '
        Me.CbxUtilisateur.FormattingEnabled = True
        Me.CbxUtilisateur.Items.AddRange(New Object() {"7 - Informaticien", "2 - Médecin", "5 - IDE", "8 - Secrétaire médicale", "6 - Secrétaire administrative"})
        Me.CbxUtilisateur.Location = New System.Drawing.Point(101, 112)
        Me.CbxUtilisateur.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.CbxUtilisateur.Name = "CbxUtilisateur"
        Me.CbxUtilisateur.Size = New System.Drawing.Size(338, 28)
        Me.CbxUtilisateur.TabIndex = 8
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(13, 171)
        Me.BtnValidation.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(176, 35)
        Me.BtnValidation.TabIndex = 9
        Me.BtnValidation.Text = "Liste des tâches"
        Me.BtnValidation.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RbtAdminNon)
        Me.GroupBox1.Controls.Add(Me.RbtAdminOui)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 18)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Size = New System.Drawing.Size(427, 75)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Admin"
        '
        'BtnAbandon
        '
        Me.BtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnAbandon.Location = New System.Drawing.Point(240, 425)
        Me.BtnAbandon.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnAbandon.Name = "BtnAbandon"
        Me.BtnAbandon.Size = New System.Drawing.Size(201, 35)
        Me.BtnAbandon.TabIndex = 13
        Me.BtnAbandon.Text = "Abandon"
        Me.BtnAbandon.UseVisualStyleBackColor = True
        '
        'BtnListePatient
        '
        Me.BtnListePatient.Location = New System.Drawing.Point(13, 216)
        Me.BtnListePatient.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnListePatient.Name = "BtnListePatient"
        Me.BtnListePatient.Size = New System.Drawing.Size(176, 35)
        Me.BtnListePatient.TabIndex = 14
        Me.BtnListePatient.Text = "Liste patients"
        Me.BtnListePatient.UseVisualStyleBackColor = True
        '
        'BtnAdmin
        '
        Me.BtnAdmin.Location = New System.Drawing.Point(13, 261)
        Me.BtnAdmin.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnAdmin.Name = "BtnAdmin"
        Me.BtnAdmin.Size = New System.Drawing.Size(176, 35)
        Me.BtnAdmin.TabIndex = 15
        Me.BtnAdmin.Text = "Admin"
        Me.BtnAdmin.UseVisualStyleBackColor = True
        '
        'RadDesktopAlert1
        '
        Me.RadDesktopAlert1.AutoCloseDelay = 5
        '
        'BtnTest
        '
        Me.BtnTest.Location = New System.Drawing.Point(13, 351)
        Me.BtnTest.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnTest.Name = "BtnTest"
        Me.BtnTest.Size = New System.Drawing.Size(176, 35)
        Me.BtnTest.TabIndex = 16
        Me.BtnTest.Text = "Test"
        Me.BtnTest.UseVisualStyleBackColor = True
        '
        'BtnTemplateSsEpisode
        '
        Me.BtnTemplateSsEpisode.Location = New System.Drawing.Point(13, 425)
        Me.BtnTemplateSsEpisode.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnTemplateSsEpisode.Name = "BtnTemplateSsEpisode"
        Me.BtnTemplateSsEpisode.Size = New System.Drawing.Size(176, 35)
        Me.BtnTemplateSsEpisode.TabIndex = 17
        Me.BtnTemplateSsEpisode.Text = "TemplateSousEpisode"
        Me.BtnTemplateSsEpisode.UseVisualStyleBackColor = True
        '
        'BtnLogin
        '
        Me.BtnLogin.Location = New System.Drawing.Point(13, 306)
        Me.BtnLogin.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnLogin.Name = "BtnLogin"
        Me.BtnLogin.Size = New System.Drawing.Size(176, 35)
        Me.BtnLogin.TabIndex = 18
        Me.BtnLogin.Text = "Login"
        Me.BtnLogin.UseVisualStyleBackColor = True
        '
        'BtnDebloque
        '
        Me.BtnDebloque.Location = New System.Drawing.Point(240, 223)
        Me.BtnDebloque.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnDebloque.Name = "BtnDebloque"
        Me.BtnDebloque.Size = New System.Drawing.Size(201, 35)
        Me.BtnDebloque.TabIndex = 19
        Me.BtnDebloque.Text = "Débloque Poste"
        Me.BtnDebloque.UseVisualStyleBackColor = True
        '
        'LblNbTry
        '
        Me.LblNbTry.AutoSize = True
        Me.LblNbTry.Location = New System.Drawing.Point(236, 178)
        Me.LblNbTry.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblNbTry.Name = "LblNbTry"
        Me.LblNbTry.Size = New System.Drawing.Size(58, 20)
        Me.LblNbTry.TabIndex = 20
        Me.LblNbTry.Text = "Action:"
        '
        'FAuthentificattion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(456, 478)
        Me.Controls.Add(Me.LblNbTry)
        Me.Controls.Add(Me.BtnDebloque)
        Me.Controls.Add(Me.BtnLogin)
        Me.Controls.Add(Me.BtnTemplateSsEpisode)
        Me.Controls.Add(Me.BtnTest)
        Me.Controls.Add(Me.BtnAdmin)
        Me.Controls.Add(Me.BtnListePatient)
        Me.Controls.Add(Me.BtnAbandon)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.CbxUtilisateur)
        Me.Controls.Add(Me.Label3)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimizeBox = False
        Me.Name = "FAuthentificattion"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
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
    Friend WithEvents BtnTest As Button
    Friend WithEvents BtnTemplateSsEpisode As Button
    Friend WithEvents BtnLogin As Button
    Friend WithEvents BtnDebloque As Button
    Friend WithEvents LblNbTry As Label
End Class
