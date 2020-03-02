<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSousEpisode
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
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.BtnValidate = New Telerik.WinControls.UI.RadButton()
        Me.BtnDetail = New Telerik.WinControls.UI.RadButton()
        Me.BtnCreate = New Telerik.WinControls.UI.RadButton()
        Me.BtnCancel = New Telerik.WinControls.UI.RadButton()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.TxtRDVCommentaire = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblDateCreation = New Telerik.WinControls.UI.RadLabel()
        Me.LblDateModif = New Telerik.WinControls.UI.RadLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblDateValidation = New Telerik.WinControls.UI.RadLabel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.RadDropDownList1 = New Telerik.WinControls.UI.RadDropDownList()
        Me.DropDownType = New Telerik.WinControls.UI.RadDropDownList()
        Me.DropDownSousType = New Telerik.WinControls.UI.RadDropDownList()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LblFichier = New Telerik.WinControls.UI.RadLabel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.ChkBReponseAttendue = New Telerik.WinControls.UI.RadCheckBox()
        Me.RadMaskedEditBox1 = New Telerik.WinControls.UI.RadMaskedEditBox()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.BtnValidate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnCreate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me.lblDateCreation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LblDateModif, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LblDateValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadDropDownList1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DropDownSousType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LblFichier, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChkBReponseAttendue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadMaskedEditBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.BtnValidate)
        Me.RadPanel1.Controls.Add(Me.BtnDetail)
        Me.RadPanel1.Controls.Add(Me.BtnCreate)
        Me.RadPanel1.Controls.Add(Me.BtnCancel)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel1.Location = New System.Drawing.Point(0, 609)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(488, 37)
        Me.RadPanel1.TabIndex = 1
        '
        'BtnValidate
        '
        Me.BtnValidate.ForeColor = System.Drawing.Color.Red
        Me.BtnValidate.Location = New System.Drawing.Point(282, 6)
        Me.BtnValidate.Name = "BtnValidate"
        Me.BtnValidate.Size = New System.Drawing.Size(110, 24)
        Me.BtnValidate.TabIndex = 10
        Me.BtnValidate.TabStop = False
        Me.BtnValidate.Text = "Valider"
        Me.BtnValidate.Visible = False
        '
        'BtnDetail
        '
        Me.BtnDetail.Location = New System.Drawing.Point(156, 6)
        Me.BtnDetail.Name = "BtnDetail"
        Me.BtnDetail.Size = New System.Drawing.Size(110, 24)
        Me.BtnDetail.TabIndex = 9
        Me.BtnDetail.Text = "Détail"
        Me.BtnDetail.Visible = False
        '
        'BtnCreate
        '
        Me.BtnCreate.Location = New System.Drawing.Point(31, 6)
        Me.BtnCreate.Name = "BtnCreate"
        Me.BtnCreate.Size = New System.Drawing.Size(110, 24)
        Me.BtnCreate.TabIndex = 8
        Me.BtnCreate.Text = "Nouveau"
        '
        'BtnCancel
        '
        Me.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancel.Location = New System.Drawing.Point(401, 6)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(81, 24)
        Me.BtnCancel.TabIndex = 7
        Me.BtnCancel.Text = "Abandonner"
        '
        'RadPanel2
        '
        Me.RadPanel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadPanel2.Controls.Add(Me.RadMaskedEditBox1)
        Me.RadPanel2.Controls.Add(Me.ChkBReponseAttendue)
        Me.RadPanel2.Controls.Add(Me.Label8)
        Me.RadPanel2.Controls.Add(Me.Label9)
        Me.RadPanel2.Controls.Add(Me.Label10)
        Me.RadPanel2.Controls.Add(Me.LblFichier)
        Me.RadPanel2.Controls.Add(Me.Label7)
        Me.RadPanel2.Controls.Add(Me.DropDownSousType)
        Me.RadPanel2.Controls.Add(Me.Label6)
        Me.RadPanel2.Controls.Add(Me.DropDownType)
        Me.RadPanel2.Controls.Add(Me.RadDropDownList1)
        Me.RadPanel2.Controls.Add(Me.Label5)
        Me.RadPanel2.Controls.Add(Me.Label4)
        Me.RadPanel2.Controls.Add(Me.LblDateValidation)
        Me.RadPanel2.Controls.Add(Me.Label3)
        Me.RadPanel2.Controls.Add(Me.LblDateModif)
        Me.RadPanel2.Controls.Add(Me.Label1)
        Me.RadPanel2.Controls.Add(Me.lblDateCreation)
        Me.RadPanel2.Controls.Add(Me.Label2)
        Me.RadPanel2.Controls.Add(Me.Label16)
        Me.RadPanel2.Controls.Add(Me.TxtRDVCommentaire)
        Me.RadPanel2.Controls.Add(Me.Label12)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadPanel2.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(488, 449)
        Me.RadPanel2.TabIndex = 16
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Label16.Location = New System.Drawing.Point(6, 106)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(474, 13)
        Me.Label16.TabIndex = 27
        Me.Label16.Text = "Typologie"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TxtRDVCommentaire
        '
        Me.TxtRDVCommentaire.Location = New System.Drawing.Point(6, 363)
        Me.TxtRDVCommentaire.MaxLength = 200
        Me.TxtRDVCommentaire.Multiline = True
        Me.TxtRDVCommentaire.Name = "TxtRDVCommentaire"
        Me.TxtRDVCommentaire.Size = New System.Drawing.Size(474, 68)
        Me.TxtRDVCommentaire.TabIndex = 26
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(14, 32)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(54, 13)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Création"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Label2.Location = New System.Drawing.Point(5, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(474, 13)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "Horodatages"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDateCreation
        '
        Me.lblDateCreation.Location = New System.Drawing.Point(154, 29)
        Me.lblDateCreation.Name = "lblDateCreation"
        Me.lblDateCreation.Size = New System.Drawing.Size(87, 18)
        Me.lblDateCreation.TabIndex = 29
        Me.lblDateCreation.Text = "LblDateCreation"
        '
        'LblDateModif
        '
        Me.LblDateModif.Location = New System.Drawing.Point(154, 52)
        Me.LblDateModif.Name = "LblDateModif"
        Me.LblDateModif.Size = New System.Drawing.Size(74, 18)
        Me.LblDateModif.TabIndex = 31
        Me.LblDateModif.Text = "LblDateModif"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(14, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Dernière Modification"
        '
        'LblDateValidation
        '
        Me.LblDateValidation.Location = New System.Drawing.Point(154, 76)
        Me.LblDateValidation.Name = "LblDateValidation"
        Me.LblDateValidation.Size = New System.Drawing.Size(95, 18)
        Me.LblDateValidation.TabIndex = 33
        Me.LblDateValidation.Text = "LblDateValidation"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 32
        Me.Label3.Text = "Validation"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Label4.Location = New System.Drawing.Point(6, 347)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(474, 13)
        Me.Label4.TabIndex = 34
        Me.Label4.Text = "Commentaire"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 133)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 35
        Me.Label5.Text = "Type"
        '
        'RadDropDownList1
        '
        Me.RadDropDownList1.Location = New System.Drawing.Point(545, 197)
        Me.RadDropDownList1.Name = "RadDropDownList1"
        Me.RadDropDownList1.Size = New System.Drawing.Size(8, 8)
        Me.RadDropDownList1.TabIndex = 36
        Me.RadDropDownList1.Text = "RadDropDownList1"
        '
        'DropDownType
        '
        Me.DropDownType.Location = New System.Drawing.Point(152, 126)
        Me.DropDownType.Name = "DropDownType"
        Me.DropDownType.Size = New System.Drawing.Size(328, 20)
        Me.DropDownType.TabIndex = 37
        '
        'DropDownSousType
        '
        Me.DropDownSousType.Location = New System.Drawing.Point(152, 152)
        Me.DropDownSousType.Name = "DropDownSousType"
        Me.DropDownSousType.Size = New System.Drawing.Size(328, 20)
        Me.DropDownSousType.TabIndex = 39
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 159)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 38
        Me.Label6.Text = "Sous-Type"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 185)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(45, 13)
        Me.Label7.TabIndex = 40
        Me.Label7.Text = "Fichier"
        '
        'LblFichier
        '
        Me.LblFichier.Location = New System.Drawing.Point(152, 178)
        Me.LblFichier.Name = "LblFichier"
        Me.LblFichier.Size = New System.Drawing.Size(53, 18)
        Me.LblFichier.TabIndex = 41
        Me.LblFichier.Text = "LblFichier"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(294, 239)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(122, 13)
        Me.Label8.TabIndex = 45
        Me.Label8.Text = "Délai maxi (en jours)"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(12, 239)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(111, 13)
        Me.Label9.TabIndex = 43
        Me.Label9.Text = "Réponse attendue"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Label10.Location = New System.Drawing.Point(6, 214)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(474, 13)
        Me.Label10.TabIndex = 42
        Me.Label10.Text = "Réponses / Documents reçus"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ChkBReponseAttendue
        '
        Me.ChkBReponseAttendue.Location = New System.Drawing.Point(152, 238)
        Me.ChkBReponseAttendue.Name = "ChkBReponseAttendue"
        Me.ChkBReponseAttendue.Size = New System.Drawing.Size(15, 15)
        Me.ChkBReponseAttendue.TabIndex = 47
        '
        'RadMaskedEditBox1
        '
        Me.RadMaskedEditBox1.Location = New System.Drawing.Point(434, 235)
        Me.RadMaskedEditBox1.Mask = "###"
        Me.RadMaskedEditBox1.MaskType = Telerik.WinControls.UI.MaskType.Numeric
        Me.RadMaskedEditBox1.Name = "RadMaskedEditBox1"
        Me.RadMaskedEditBox1.Size = New System.Drawing.Size(42, 20)
        Me.RadMaskedEditBox1.TabIndex = 48
        Me.RadMaskedEditBox1.TabStop = False
        '
        'FrmSousEpisode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(488, 646)
        Me.Controls.Add(Me.RadPanel2)
        Me.Controls.Add(Me.RadPanel1)
        Me.MinimizeBox = False
        Me.Name = "FrmSousEpisode"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Sous-Episode"
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        CType(Me.BtnValidate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnCreate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel2.ResumeLayout(False)
        Me.RadPanel2.PerformLayout()
        CType(Me.lblDateCreation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LblDateModif, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LblDateValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadDropDownList1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DropDownSousType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LblFichier, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChkBReponseAttendue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadMaskedEditBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents BtnValidate As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnDetail As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnCreate As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnCancel As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents lblDateCreation As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents TxtRDVCommentaire As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents LblDateModif As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Label1 As Label
    Friend WithEvents LblDateValidation As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Label3 As Label
    Friend WithEvents LblFichier As Telerik.WinControls.UI.RadLabel
    Friend WithEvents Label7 As Label
    Friend WithEvents DropDownSousType As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents Label6 As Label
    Friend WithEvents DropDownType As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents RadDropDownList1 As Telerik.WinControls.UI.RadDropDownList
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ChkBReponseAttendue As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents RadMaskedEditBox1 As Telerik.WinControls.UI.RadMaskedEditBox
End Class

