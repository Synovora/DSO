<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFTestMethodes
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
        Me.RadBtnIntervenant = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnTraitement = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAntecedent = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnContexte = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadBtnIntervenant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnTraitement, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAntecedent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnContexte, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnIntervenant
        '
        Me.RadBtnIntervenant.Location = New System.Drawing.Point(46, 60)
        Me.RadBtnIntervenant.Name = "RadBtnIntervenant"
        Me.RadBtnIntervenant.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnIntervenant.TabIndex = 0
        Me.RadBtnIntervenant.Text = "Intervenant Patient"
        '
        'RadBtnTraitement
        '
        Me.RadBtnTraitement.Location = New System.Drawing.Point(46, 90)
        Me.RadBtnTraitement.Name = "RadBtnTraitement"
        Me.RadBtnTraitement.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnTraitement.TabIndex = 1
        Me.RadBtnTraitement.Text = "Traitement"
        '
        'RadBtnAntecedent
        '
        Me.RadBtnAntecedent.Location = New System.Drawing.Point(46, 120)
        Me.RadBtnAntecedent.Name = "RadBtnAntecedent"
        Me.RadBtnAntecedent.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAntecedent.TabIndex = 2
        Me.RadBtnAntecedent.Text = "Antécédent"
        '
        'RadBtnContexte
        '
        Me.RadBtnContexte.Location = New System.Drawing.Point(46, 150)
        Me.RadBtnContexte.Name = "RadBtnContexte"
        Me.RadBtnContexte.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnContexte.TabIndex = 3
        Me.RadBtnContexte.Text = "Contexte"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.Location = New System.Drawing.Point(279, 234)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 4
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadFTestMethodes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(401, 270)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadBtnContexte)
        Me.Controls.Add(Me.RadBtnAntecedent)
        Me.Controls.Add(Me.RadBtnTraitement)
        Me.Controls.Add(Me.RadBtnIntervenant)
        Me.Name = "RadFTestMethodes"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFTestMethodes"
        CType(Me.RadBtnIntervenant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnTraitement, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAntecedent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnContexte, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadBtnIntervenant As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnTraitement As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAntecedent As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnContexte As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
End Class

