<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMailOrdonnance
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMailOrdonnance))
        Me.TxtObjet = New System.Windows.Forms.TextBox()
        Me.LblObjet = New System.Windows.Forms.Label()
        Me.TxtTo = New System.Windows.Forms.TextBox()
        Me.LblIdentifiant = New System.Windows.Forms.Label()
        Me.RadGroupIdentite = New Telerik.WinControls.UI.RadGroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rbPatient = New Telerik.WinControls.UI.RadRadioButton()
        Me.rbPharmacie = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.BtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.BtnValider = New System.Windows.Forms.Button()
        Me.PnlBoutons = New Telerik.WinControls.UI.RadPanel()
        Me.TxtBody = New System.Windows.Forms.TextBox()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        CType(Me.RadGroupIdentite, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupIdentite.SuspendLayout()
        CType(Me.rbPatient, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbPharmacie, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlBoutons.SuspendLayout()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtObjet
        '
        Me.TxtObjet.BackColor = System.Drawing.SystemColors.Window
        Me.TxtObjet.Location = New System.Drawing.Point(118, 92)
        Me.TxtObjet.Name = "TxtObjet"
        Me.TxtObjet.Size = New System.Drawing.Size(674, 23)
        Me.TxtObjet.TabIndex = 3
        '
        'LblObjet
        '
        Me.LblObjet.AutoSize = True
        Me.LblObjet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblObjet.Location = New System.Drawing.Point(10, 96)
        Me.LblObjet.Name = "LblObjet"
        Me.LblObjet.Size = New System.Drawing.Size(37, 13)
        Me.LblObjet.TabIndex = 2
        Me.LblObjet.Text = "Objet"
        '
        'TxtTo
        '
        Me.TxtTo.Location = New System.Drawing.Point(118, 61)
        Me.TxtTo.Name = "TxtTo"
        Me.TxtTo.Size = New System.Drawing.Size(300, 23)
        Me.TxtTo.TabIndex = 1
        '
        'LblIdentifiant
        '
        Me.LblIdentifiant.AutoSize = True
        Me.LblIdentifiant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblIdentifiant.Location = New System.Drawing.Point(10, 65)
        Me.LblIdentifiant.Name = "LblIdentifiant"
        Me.LblIdentifiant.Size = New System.Drawing.Size(75, 13)
        Me.LblIdentifiant.TabIndex = 0
        Me.LblIdentifiant.Text = "Destinataire"
        '
        'RadGroupIdentite
        '
        Me.RadGroupIdentite.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupIdentite.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupIdentite.Controls.Add(Me.Label1)
        Me.RadGroupIdentite.Controls.Add(Me.rbPatient)
        Me.RadGroupIdentite.Controls.Add(Me.rbPharmacie)
        Me.RadGroupIdentite.Controls.Add(Me.TxtObjet)
        Me.RadGroupIdentite.Controls.Add(Me.LblObjet)
        Me.RadGroupIdentite.Controls.Add(Me.TxtTo)
        Me.RadGroupIdentite.Controls.Add(Me.LblIdentifiant)
        Me.RadGroupIdentite.Controls.Add(Me.RadLabel1)
        Me.RadGroupIdentite.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupIdentite.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold)
        Me.RadGroupIdentite.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupIdentite.HeaderText = ""
        Me.RadGroupIdentite.Location = New System.Drawing.Point(0, 0)
        Me.RadGroupIdentite.Name = "RadGroupIdentite"
        Me.RadGroupIdentite.Size = New System.Drawing.Size(905, 137)
        Me.RadGroupIdentite.TabIndex = 4
        CType(Me.RadGroupIdentite.GetChildAt(0), Telerik.WinControls.UI.RadGroupBoxElement).Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Pré-choix"
        '
        'rbPatient
        '
        Me.rbPatient.Location = New System.Drawing.Point(118, 34)
        Me.rbPatient.Name = "rbPatient"
        Me.rbPatient.Size = New System.Drawing.Size(55, 18)
        Me.rbPatient.TabIndex = 7
        Me.rbPatient.Text = "Patient"
        '
        'rbPharmacie
        '
        Me.rbPharmacie.Location = New System.Drawing.Point(118, 10)
        Me.rbPharmacie.Name = "rbPharmacie"
        Me.rbPharmacie.Size = New System.Drawing.Size(72, 18)
        Me.rbPharmacie.TabIndex = 6
        Me.rbPharmacie.Text = "Pharmacie"
        '
        'RadLabel1
        '
        Me.RadLabel1.AutoSize = False
        Me.RadLabel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadLabel1.Location = New System.Drawing.Point(2, 117)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(901, 18)
        Me.RadLabel1.TabIndex = 9
        Me.RadLabel1.Text = "Corps du message"
        Me.RadLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnAbandon
        '
        Me.BtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnAbandon.ForeColor = System.Drawing.Color.Black
        Me.BtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.BtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.BtnAbandon.Location = New System.Drawing.Point(715, 8)
        Me.BtnAbandon.Name = "BtnAbandon"
        Me.BtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.BtnAbandon.TabIndex = 1
        '
        'BtnValider
        '
        Me.BtnValider.ForeColor = System.Drawing.Color.Red
        Me.BtnValider.Location = New System.Drawing.Point(83, 8)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(216, 24)
        Me.BtnValider.TabIndex = 0
        Me.BtnValider.Text = "Valider"
        Me.BtnValider.UseVisualStyleBackColor = True
        '
        'PnlBoutons
        '
        Me.PnlBoutons.Controls.Add(Me.BtnAbandon)
        Me.PnlBoutons.Controls.Add(Me.BtnValider)
        Me.PnlBoutons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PnlBoutons.Location = New System.Drawing.Point(0, 519)
        Me.PnlBoutons.Name = "PnlBoutons"
        Me.PnlBoutons.Size = New System.Drawing.Size(905, 40)
        Me.PnlBoutons.TabIndex = 6
        '
        'TxtBody
        '
        Me.TxtBody.BackColor = System.Drawing.SystemColors.Window
        Me.TxtBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TxtBody.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.TxtBody.Location = New System.Drawing.Point(0, 0)
        Me.TxtBody.Multiline = True
        Me.TxtBody.Name = "TxtBody"
        Me.TxtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TxtBody.Size = New System.Drawing.Size(905, 382)
        Me.TxtBody.TabIndex = 8
        '
        'RadPanel1
        '
        Me.RadPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadPanel1.Controls.Add(Me.TxtBody)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadPanel1.Location = New System.Drawing.Point(0, 137)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(905, 382)
        Me.RadPanel1.TabIndex = 9
        Me.RadPanel1.TextAlignment = System.Drawing.ContentAlignment.TopCenter
        '
        'FrmMailOrdonnance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(905, 559)
        Me.Controls.Add(Me.RadPanel1)
        Me.Controls.Add(Me.RadGroupIdentite)
        Me.Controls.Add(Me.PnlBoutons)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "FrmMailOrdonnance"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Envoi Ordonnance en Email"
        CType(Me.RadGroupIdentite, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupIdentite.ResumeLayout(False)
        Me.RadGroupIdentite.PerformLayout()
        CType(Me.rbPatient, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbPharmacie, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlBoutons.ResumeLayout(False)
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TxtObjet As TextBox
    Friend WithEvents LblObjet As Label
    Friend WithEvents TxtTo As TextBox
    Friend WithEvents LblIdentifiant As Label
    Friend WithEvents RadGroupIdentite As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents BtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnValider As Button
    Friend WithEvents PnlBoutons As Telerik.WinControls.UI.RadPanel
    Friend WithEvents Label1 As Label
    Friend WithEvents rbPatient As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents rbPharmacie As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents TxtBody As TextBox
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
End Class

