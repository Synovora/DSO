<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FPatientDetail
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
        Me.LblPatientId = New System.Windows.Forms.Label()
        Me.LblPatientNir = New System.Windows.Forms.Label()
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientDateNaissance = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientVille = New System.Windows.Forms.Label()
        Me.LblPatientSiteId = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LblPatientId
        '
        Me.LblPatientId.AutoSize = True
        Me.LblPatientId.Location = New System.Drawing.Point(177, 68)
        Me.LblPatientId.Name = "LblPatientId"
        Me.LblPatientId.Size = New System.Drawing.Size(49, 13)
        Me.LblPatientId.TabIndex = 8
        Me.LblPatientId.Text = "PatientId"
        '
        'LblPatientNir
        '
        Me.LblPatientNir.AutoSize = True
        Me.LblPatientNir.Location = New System.Drawing.Point(177, 95)
        Me.LblPatientNir.Name = "LblPatientNir"
        Me.LblPatientNir.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientNir.TabIndex = 9
        Me.LblPatientNir.Text = "PatientNir"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(177, 122)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(76, 13)
        Me.LblPatientPrenom.TabIndex = 10
        Me.LblPatientPrenom.Text = "PatientPrenom"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(177, 149)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(62, 13)
        Me.LblPatientNom.TabIndex = 11
        Me.LblPatientNom.Text = "PatientNom"
        '
        'LblPatientDateNaissance
        '
        Me.LblPatientDateNaissance.AutoSize = True
        Me.LblPatientDateNaissance.Location = New System.Drawing.Point(177, 174)
        Me.LblPatientDateNaissance.Name = "LblPatientDateNaissance"
        Me.LblPatientDateNaissance.Size = New System.Drawing.Size(113, 13)
        Me.LblPatientDateNaissance.TabIndex = 12
        Me.LblPatientDateNaissance.Text = "PatientDateNaissance"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(177, 198)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(69, 13)
        Me.LblPatientGenre.TabIndex = 13
        Me.LblPatientGenre.Text = "PatientGenre"
        '
        'LblPatientVille
        '
        Me.LblPatientVille.AutoSize = True
        Me.LblPatientVille.Location = New System.Drawing.Point(177, 222)
        Me.LblPatientVille.Name = "LblPatientVille"
        Me.LblPatientVille.Size = New System.Drawing.Size(59, 13)
        Me.LblPatientVille.TabIndex = 14
        Me.LblPatientVille.Text = "PatientVille"
        '
        'LblPatientSiteId
        '
        Me.LblPatientSiteId.AutoSize = True
        Me.LblPatientSiteId.Location = New System.Drawing.Point(177, 247)
        Me.LblPatientSiteId.Name = "LblPatientSiteId"
        Me.LblPatientSiteId.Size = New System.Drawing.Size(67, 13)
        Me.LblPatientSiteId.TabIndex = 15
        Me.LblPatientSiteId.Text = "PatientSiteId"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(64, 247)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Site :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(64, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Identifiant patient :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(64, 95)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "NIR :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(64, 122)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 13)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "Prénom :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(64, 149)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Nom :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(64, 174)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(102, 13)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "Date de naissance :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(64, 198)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(42, 13)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Genre :"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(64, 222)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(32, 13)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Ville :"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(283, 174)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(59, 13)
        Me.LblPatientAge.TabIndex = 24
        Me.LblPatientAge.Text = "PatientAge"
        '
        'Form5
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.LblPatientAge)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblPatientSiteId)
        Me.Controls.Add(Me.LblPatientVille)
        Me.Controls.Add(Me.LblPatientGenre)
        Me.Controls.Add(Me.LblPatientDateNaissance)
        Me.Controls.Add(Me.LblPatientNom)
        Me.Controls.Add(Me.LblPatientPrenom)
        Me.Controls.Add(Me.LblPatientNir)
        Me.Controls.Add(Me.LblPatientId)
        Me.Name = "Form5"
        Me.Text = "Détail patient"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblPatientId As Label
    Friend WithEvents LblPatientNir As Label
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientDateNaissance As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientVille As Label
    Friend WithEvents LblPatientSiteId As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents LblPatientAge As Label
End Class
