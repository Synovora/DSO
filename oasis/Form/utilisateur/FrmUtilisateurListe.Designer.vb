<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUtilisateurListe
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn13 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn3 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewDateTimeColumn4 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn14 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn15 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn16 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.BtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.BtnActiverDesactiver = New System.Windows.Forms.Button()
        Me.BtnUpdate = New System.Windows.Forms.Button()
        Me.BtnNouveau = New System.Windows.Forms.Button()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.RadGroupBoxFiltre = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadioInactif = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadioActif = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadGroupBoxFiltre, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBoxFiltre.SuspendLayout()
        CType(Me.RadioInactif, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioActif, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel2
        '
        Me.RadPanel2.Controls.Add(Me.BtnAbandon)
        Me.RadPanel2.Controls.Add(Me.BtnActiverDesactiver)
        Me.RadPanel2.Controls.Add(Me.BtnUpdate)
        Me.RadPanel2.Controls.Add(Me.BtnNouveau)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel2.Location = New System.Drawing.Point(0, 403)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(1386, 57)
        Me.RadPanel2.TabIndex = 3
        '
        'BtnAbandon
        '
        Me.BtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnAbandon.ForeColor = System.Drawing.Color.Black
        Me.BtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.BtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.BtnAbandon.Location = New System.Drawing.Point(1327, 8)
        Me.BtnAbandon.Name = "BtnAbandon"
        Me.BtnAbandon.Size = New System.Drawing.Size(38, 38)
        Me.BtnAbandon.TabIndex = 35
        '
        'BtnActiverDesactiver
        '
        Me.BtnActiverDesactiver.ForeColor = System.Drawing.Color.Red
        Me.BtnActiverDesactiver.Location = New System.Drawing.Point(629, 8)
        Me.BtnActiverDesactiver.Name = "BtnActiverDesactiver"
        Me.BtnActiverDesactiver.Size = New System.Drawing.Size(216, 38)
        Me.BtnActiverDesactiver.TabIndex = 3
        Me.BtnActiverDesactiver.Text = "A/D"
        Me.BtnActiverDesactiver.UseVisualStyleBackColor = True
        '
        'BtnUpdate
        '
        Me.BtnUpdate.Location = New System.Drawing.Point(396, 8)
        Me.BtnUpdate.Name = "BtnUpdate"
        Me.BtnUpdate.Size = New System.Drawing.Size(216, 38)
        Me.BtnUpdate.TabIndex = 2
        Me.BtnUpdate.Text = "Modifier"
        Me.BtnUpdate.UseVisualStyleBackColor = True
        '
        'BtnNouveau
        '
        Me.BtnNouveau.Location = New System.Drawing.Point(157, 8)
        Me.BtnNouveau.Name = "BtnNouveau"
        Me.BtnNouveau.Size = New System.Drawing.Size(216, 38)
        Me.BtnNouveau.TabIndex = 1
        Me.BtnNouveau.Text = "Nouveau"
        Me.BtnNouveau.UseVisualStyleBackColor = True
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.RadGroupBoxFiltre)
        Me.RadPanel1.Controls.Add(Me.RadGridView1)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(1386, 403)
        Me.RadPanel1.TabIndex = 4
        '
        'RadGroupBoxFiltre
        '
        Me.RadGroupBoxFiltre.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBoxFiltre.Controls.Add(Me.RadioInactif)
        Me.RadGroupBoxFiltre.Controls.Add(Me.RadioActif)
        Me.RadGroupBoxFiltre.HeaderText = ""
        Me.RadGroupBoxFiltre.Location = New System.Drawing.Point(30, 12)
        Me.RadGroupBoxFiltre.Name = "RadGroupBoxFiltre"
        Me.RadGroupBoxFiltre.Size = New System.Drawing.Size(516, 30)
        Me.RadGroupBoxFiltre.TabIndex = 4
        '
        'RadioInactif
        '
        Me.RadioInactif.Location = New System.Drawing.Point(219, 5)
        Me.RadioInactif.Name = "RadioInactif"
        Me.RadioInactif.Size = New System.Drawing.Size(56, 18)
        Me.RadioInactif.TabIndex = 1
        Me.RadioInactif.TabStop = False
        Me.RadioInactif.Text = "Inactifs"
        '
        'RadioActif
        '
        Me.RadioActif.Location = New System.Drawing.Point(48, 5)
        Me.RadioActif.Name = "RadioActif"
        Me.RadioActif.Size = New System.Drawing.Size(48, 18)
        Me.RadioActif.TabIndex = 0
        Me.RadioActif.TabStop = False
        Me.RadioActif.Text = "Actifs"
        '
        'RadGridView1
        '
        Me.RadGridView1.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridView1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridView1.ForeColor = System.Drawing.Color.Black
        Me.RadGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridView1.Location = New System.Drawing.Point(103, 119)
        '
        '
        '
        Me.RadGridView1.MasterTemplate.AllowAddNewRow = False
        Me.RadGridView1.MasterTemplate.AllowDeleteRow = False
        Me.RadGridView1.MasterTemplate.AllowEditRow = False
        Me.RadGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.FieldName = "Id"
        GridViewTextBoxColumn9.HeaderText = "Id"
        GridViewTextBoxColumn9.IsVisible = False
        GridViewTextBoxColumn9.MaxWidth = 20
        GridViewTextBoxColumn9.MinWidth = 20
        GridViewTextBoxColumn9.Name = "Id"
        GridViewTextBoxColumn9.ReadOnly = True
        GridViewTextBoxColumn9.Width = 20
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.HeaderText = "Nom"
        GridViewTextBoxColumn10.MaxWidth = 200
        GridViewTextBoxColumn10.MinWidth = 200
        GridViewTextBoxColumn10.Name = "Nom"
        GridViewTextBoxColumn10.Width = 200
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.HeaderText = "Prénom"
        GridViewTextBoxColumn11.MaxWidth = 200
        GridViewTextBoxColumn11.MinWidth = 200
        GridViewTextBoxColumn11.Name = "Prenom"
        GridViewTextBoxColumn11.Width = 200
        GridViewTextBoxColumn12.EnableExpressionEditor = False
        GridViewTextBoxColumn12.HeaderText = "Identifiant"
        GridViewTextBoxColumn12.MaxWidth = 200
        GridViewTextBoxColumn12.MinWidth = 100
        GridViewTextBoxColumn12.Name = "Identifiant"
        GridViewTextBoxColumn12.Width = 100
        GridViewTextBoxColumn13.EnableExpressionEditor = False
        GridViewTextBoxColumn13.HeaderText = "Profil"
        GridViewTextBoxColumn13.MaxWidth = 200
        GridViewTextBoxColumn13.MinWidth = 100
        GridViewTextBoxColumn13.Name = "profil_designation"
        GridViewTextBoxColumn13.Width = 100
        GridViewDateTimeColumn3.EnableExpressionEditor = False
        GridViewDateTimeColumn3.FormatString = "{0:dd/MM/yyyy}"
        GridViewDateTimeColumn3.HeaderText = "Date Entrée"
        GridViewDateTimeColumn3.MaxWidth = 70
        GridViewDateTimeColumn3.MinWidth = 70
        GridViewDateTimeColumn3.Name = "date_entree"
        GridViewDateTimeColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDateTimeColumn3.Width = 70
        GridViewDateTimeColumn4.EnableExpressionEditor = False
        GridViewDateTimeColumn4.FormatString = "{0:dd/MM/yyyy}"
        GridViewDateTimeColumn4.HeaderText = "Date Sortie"
        GridViewDateTimeColumn4.MaxWidth = 70
        GridViewDateTimeColumn4.MinWidth = 70
        GridViewDateTimeColumn4.Name = "date_sortie"
        GridViewDateTimeColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewDateTimeColumn4.Width = 70
        GridViewTextBoxColumn14.EnableExpressionEditor = False
        GridViewTextBoxColumn14.HeaderText = "Siège"
        GridViewTextBoxColumn14.MaxWidth = 200
        GridViewTextBoxColumn14.MinWidth = 200
        GridViewTextBoxColumn14.Name = "siege"
        GridViewTextBoxColumn14.Width = 200
        GridViewTextBoxColumn15.EnableExpressionEditor = False
        GridViewTextBoxColumn15.HeaderText = "Unité Sanitaire"
        GridViewTextBoxColumn15.MaxWidth = 200
        GridViewTextBoxColumn15.MinWidth = 200
        GridViewTextBoxColumn15.Name = "unite_sanitaire"
        GridViewTextBoxColumn15.Width = 200
        GridViewTextBoxColumn16.EnableExpressionEditor = False
        GridViewTextBoxColumn16.HeaderText = "Site"
        GridViewTextBoxColumn16.MinWidth = 200
        GridViewTextBoxColumn16.Name = "site"
        GridViewTextBoxColumn16.Width = 200
        Me.RadGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn9, GridViewTextBoxColumn10, GridViewTextBoxColumn11, GridViewTextBoxColumn12, GridViewTextBoxColumn13, GridViewDateTimeColumn3, GridViewDateTimeColumn4, GridViewTextBoxColumn14, GridViewTextBoxColumn15, GridViewTextBoxColumn16})
        Me.RadGridView1.MasterTemplate.EnableFiltering = True
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridView1.Size = New System.Drawing.Size(1041, 213)
        Me.RadGridView1.TabIndex = 0
        '
        'FrmUtilisateurListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(1386, 460)
        Me.Controls.Add(Me.RadPanel1)
        Me.Controls.Add(Me.RadPanel2)
        Me.MinimizeBox = False
        Me.Name = "FrmUtilisateurListe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Liste des Utilisateurs"
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel2.ResumeLayout(False)
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        CType(Me.RadGroupBoxFiltre, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBoxFiltre.ResumeLayout(False)
        Me.RadGroupBoxFiltre.PerformLayout()
        CType(Me.RadioInactif, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioActif, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents BtnActiverDesactiver As Button
    Friend WithEvents BtnUpdate As Button
    Friend WithEvents BtnNouveau As Button
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadGridView1 As Telerik.WinControls.UI.RadGridView
    Friend WithEvents BtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBoxFiltre As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadioInactif As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents RadioActif As Telerik.WinControls.UI.RadRadioButton
End Class

