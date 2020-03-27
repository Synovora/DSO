<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmActionDoc
    Inherits Telerik.WinControls.UI.ShapedForm

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
        Me.RadFormConverter1 = New Telerik.WinControls.UI.RadFormConverter()
        Me.BtnEnregistrer = New Telerik.WinControls.UI.RadButton()
        Me.BtnAnnulation = New Telerik.WinControls.UI.RadButton()
        Me.RBtnQuitterSansEnreg = New Telerik.WinControls.UI.RadButton()
        Me.BtnSigner = New Telerik.WinControls.UI.RadButton()
        CType(Me.BtnEnregistrer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnAnnulation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RBtnQuitterSansEnreg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnSigner, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnEnregistrer
        '
        Me.BtnEnregistrer.Location = New System.Drawing.Point(8, 12)
        Me.BtnEnregistrer.Name = "BtnEnregistrer"
        Me.BtnEnregistrer.Size = New System.Drawing.Size(221, 26)
        Me.BtnEnregistrer.TabIndex = 1
        Me.BtnEnregistrer.Text = "Enregister"
        '
        'BtnAnnulation
        '
        Me.BtnAnnulation.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnAnnulation.Location = New System.Drawing.Point(8, 80)
        Me.BtnAnnulation.Name = "BtnAnnulation"
        Me.BtnAnnulation.Size = New System.Drawing.Size(221, 26)
        Me.BtnAnnulation.TabIndex = 2
        Me.BtnAnnulation.Text = "Annulation (retour au texte)"
        '
        'RBtnQuitterSansEnreg
        '
        Me.RBtnQuitterSansEnreg.Location = New System.Drawing.Point(6, 122)
        Me.RBtnQuitterSansEnreg.Name = "RBtnQuitterSansEnreg"
        Me.RBtnQuitterSansEnreg.Size = New System.Drawing.Size(223, 26)
        Me.RBtnQuitterSansEnreg.TabIndex = 3
        Me.RBtnQuitterSansEnreg.Text = "Quitter sans Enregistrer (modifs perdues)"
        '
        'BtnSigner
        '
        Me.BtnSigner.ForeColor = System.Drawing.Color.Red
        Me.BtnSigner.Location = New System.Drawing.Point(9, 46)
        Me.BtnSigner.Name = "BtnSigner"
        Me.BtnSigner.Size = New System.Drawing.Size(221, 26)
        Me.BtnSigner.TabIndex = 4
        Me.BtnSigner.Text = "Signer et Enregister"
        '
        'FrmActionDoc
        '
        Me.AllowResize = False
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderColor = System.Drawing.Color.FromArgb(CType(CType(59, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(130, Byte), Integer))
        Me.CancelButton = Me.BtnAnnulation
        Me.ClientSize = New System.Drawing.Size(239, 153)
        Me.Controls.Add(Me.BtnSigner)
        Me.Controls.Add(Me.RBtnQuitterSansEnreg)
        Me.Controls.Add(Me.BtnAnnulation)
        Me.Controls.Add(Me.BtnEnregistrer)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmActionDoc"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "FrmActionDoc"
        Me.ThemeName = ""
        CType(Me.BtnEnregistrer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnAnnulation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RBtnQuitterSansEnreg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnSigner, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadFormConverter1 As Telerik.WinControls.UI.RadFormConverter
    Friend WithEvents BtnEnregistrer As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnAnnulation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RBtnQuitterSansEnreg As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnSigner As Telerik.WinControls.UI.RadButton
End Class

