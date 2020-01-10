<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FPPSInitCreation
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
        Me.CbxCategorie = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CbxSousCategorie = New System.Windows.Forms.ComboBox()
        Me.BtnAbandonner = New System.Windows.Forms.Button()
        Me.BtnValider = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'CbxCategorie
        '
        Me.CbxCategorie.FormattingEnabled = True
        Me.CbxCategorie.Location = New System.Drawing.Point(153, 60)
        Me.CbxCategorie.Name = "CbxCategorie"
        Me.CbxCategorie.Size = New System.Drawing.Size(121, 21)
        Me.CbxCategorie.TabIndex = 35
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 63)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "Catégorie PPS"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 119)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 13)
        Me.Label2.TabIndex = 37
        Me.Label2.Text = "Sous-catégorie PPS"
        '
        'CbxSousCategorie
        '
        Me.CbxSousCategorie.FormattingEnabled = True
        Me.CbxSousCategorie.Location = New System.Drawing.Point(153, 111)
        Me.CbxSousCategorie.Name = "CbxSousCategorie"
        Me.CbxSousCategorie.Size = New System.Drawing.Size(121, 21)
        Me.CbxSousCategorie.TabIndex = 38
        '
        'BtnAbandonner
        '
        Me.BtnAbandonner.Location = New System.Drawing.Point(353, 247)
        Me.BtnAbandonner.Name = "BtnAbandonner"
        Me.BtnAbandonner.Size = New System.Drawing.Size(75, 23)
        Me.BtnAbandonner.TabIndex = 39
        Me.BtnAbandonner.Text = "Abandonner"
        Me.BtnAbandonner.UseVisualStyleBackColor = True
        '
        'BtnValider
        '
        Me.BtnValider.Location = New System.Drawing.Point(272, 247)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(75, 23)
        Me.BtnValider.TabIndex = 40
        Me.BtnValider.Text = "Valider"
        Me.BtnValider.UseVisualStyleBackColor = True
        '
        'FPPSInitCreation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(440, 282)
        Me.Controls.Add(Me.BtnValider)
        Me.Controls.Add(Me.BtnAbandonner)
        Me.Controls.Add(Me.CbxSousCategorie)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CbxCategorie)
        Me.Name = "FPPSInitCreation"
        Me.Text = "FPPSInitCreation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CbxCategorie As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents CbxSousCategorie As ComboBox
    Friend WithEvents BtnAbandonner As Button
    Friend WithEvents BtnValider As Button
End Class
