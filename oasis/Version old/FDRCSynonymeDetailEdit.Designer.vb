<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FDRCSynonymeDetailEdit
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
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.BtnValidation = New System.Windows.Forms.Button()
        Me.BtnAnnuler = New System.Windows.Forms.Button()
        Me.TxtSynonyme = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblDrcId = New System.Windows.Forms.Label()
        Me.LblDrcDescription = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(411, 94)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 2
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(330, 94)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(75, 23)
        Me.BtnValidation.TabIndex = 3
        Me.BtnValidation.Text = "Valider"
        Me.BtnValidation.UseVisualStyleBackColor = True
        '
        'BtnAnnuler
        '
        Me.BtnAnnuler.Location = New System.Drawing.Point(12, 94)
        Me.BtnAnnuler.Name = "BtnAnnuler"
        Me.BtnAnnuler.Size = New System.Drawing.Size(75, 23)
        Me.BtnAnnuler.TabIndex = 44
        Me.BtnAnnuler.Text = "Supprimer"
        Me.BtnAnnuler.UseVisualStyleBackColor = True
        '
        'TxtSynonyme
        '
        Me.TxtSynonyme.Location = New System.Drawing.Point(83, 53)
        Me.TxtSynonyme.Name = "TxtSynonyme"
        Me.TxtSynonyme.Size = New System.Drawing.Size(405, 20)
        Me.TxtSynonyme.TabIndex = 45
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 46
        Me.Label1.Text = "Synonyme"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 47
        Me.Label2.Text = "DRC :"
        '
        'LblDrcId
        '
        Me.LblDrcId.AutoSize = True
        Me.LblDrcId.Location = New System.Drawing.Point(80, 25)
        Me.LblDrcId.Name = "LblDrcId"
        Me.LblDrcId.Size = New System.Drawing.Size(32, 13)
        Me.LblDrcId.TabIndex = 48
        Me.LblDrcId.Text = "Code"
        '
        'LblDrcDescription
        '
        Me.LblDrcDescription.AutoSize = True
        Me.LblDrcDescription.Location = New System.Drawing.Point(147, 25)
        Me.LblDrcDescription.Name = "LblDrcDescription"
        Me.LblDrcDescription.Size = New System.Drawing.Size(60, 13)
        Me.LblDrcDescription.TabIndex = 49
        Me.LblDrcDescription.Text = "Description"
        '
        'FDRCSynonymeDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(501, 128)
        Me.Controls.Add(Me.LblDrcDescription)
        Me.Controls.Add(Me.LblDrcId)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtSynonyme)
        Me.Controls.Add(Me.BtnAnnuler)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Name = "FDRCSynonymeDetailEdit"
        Me.Text = "Synonyme"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents BtnValidation As Button
    Friend WithEvents BtnAnnuler As Button
    Friend WithEvents TxtSynonyme As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents LblDrcId As Label
    Friend WithEvents LblDrcDescription As Label
End Class
