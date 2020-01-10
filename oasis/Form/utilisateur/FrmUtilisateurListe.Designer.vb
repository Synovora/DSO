<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmUtilisateurListe
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
        Me.components = New System.ComponentModel.Container()
        Dim GridViewDecimalColumn1 As Telerik.WinControls.UI.GridViewDecimalColumn = New Telerik.WinControls.UI.GridViewDecimalColumn()
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn1 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewDateTimeColumn2 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn1 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn2 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn3 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnUpdate = New System.Windows.Forms.Button()
        Me.BtnCreate = New System.Windows.Forms.Button()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        Me.VuserfullBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetUtilisateurBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetUtilisateur = New Oasis_WF.dataSetUtilisateur()
        Me.V_user_fullTableAdapter = New Oasis_WF.dataSetUtilisateurTableAdapters.v_user_fullTableAdapter()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VuserfullBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetUtilisateurBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetUtilisateur, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel2
        '
        Me.RadPanel2.Controls.Add(Me.BtnDelete)
        Me.RadPanel2.Controls.Add(Me.BtnUpdate)
        Me.RadPanel2.Controls.Add(Me.BtnCreate)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel2.Location = New System.Drawing.Point(0, 213)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(977, 57)
        Me.RadPanel2.TabIndex = 3
        '
        'BtnDelete
        '
        Me.BtnDelete.ForeColor = System.Drawing.Color.Red
        Me.BtnDelete.Location = New System.Drawing.Point(629, 8)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(216, 38)
        Me.BtnDelete.TabIndex = 3
        Me.BtnDelete.Text = "Supprimer"
        Me.BtnDelete.UseVisualStyleBackColor = True
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
        'BtnCreate
        '
        Me.BtnCreate.Location = New System.Drawing.Point(157, 8)
        Me.BtnCreate.Name = "BtnCreate"
        Me.BtnCreate.Size = New System.Drawing.Size(216, 38)
        Me.BtnCreate.TabIndex = 1
        Me.BtnCreate.Text = "Créer"
        Me.BtnCreate.UseVisualStyleBackColor = True
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.RadGridView1)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(977, 213)
        Me.RadPanel1.TabIndex = 4
        '
        'RadGridView1
        '
        Me.RadGridView1.Location = New System.Drawing.Point(29, 47)
        '
        '
        '
        Me.RadGridView1.MasterTemplate.AllowAddNewRow = False
        Me.RadGridView1.MasterTemplate.AllowDeleteRow = False
        Me.RadGridView1.MasterTemplate.AllowEditRow = False
        Me.RadGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
        GridViewDecimalColumn1.DataType = GetType(Integer)
        GridViewDecimalColumn1.FieldName = "oa_utilisateur_id"
        GridViewDecimalColumn1.HeaderText = "Id"
        GridViewDecimalColumn1.IsAutoGenerated = True
        GridViewDecimalColumn1.IsVisible = False
        GridViewDecimalColumn1.Name = "oa_utilisateur_id"
        GridViewTextBoxColumn1.FieldName = "oa_utilisateur_prenom"
        GridViewTextBoxColumn1.HeaderText = "Prénom"
        GridViewTextBoxColumn1.IsAutoGenerated = True
        GridViewTextBoxColumn1.Name = "oa_utilisateur_prenom"
        GridViewTextBoxColumn1.Width = 55
        GridViewTextBoxColumn2.FieldName = "oa_utilisateur_nom"
        GridViewTextBoxColumn2.HeaderText = "Nom"
        GridViewTextBoxColumn2.IsAutoGenerated = True
        GridViewTextBoxColumn2.Name = "oa_utilisateur_nom"
        GridViewTextBoxColumn2.Width = 55
        GridViewTextBoxColumn3.FieldName = "oa_utilisateur_login"
        GridViewTextBoxColumn3.HeaderText = "Identifiant"
        GridViewTextBoxColumn3.IsAutoGenerated = True
        GridViewTextBoxColumn3.Name = "oa_utilisateur_login"
        GridViewTextBoxColumn3.Width = 55
        GridViewDateTimeColumn1.FieldName = "oa_utilisateur_date_entree"
        GridViewDateTimeColumn1.FormatString = "{0:dd/MM/yyyy}"
        GridViewDateTimeColumn1.HeaderText = "Date entrée"
        GridViewDateTimeColumn1.IsAutoGenerated = True
        GridViewDateTimeColumn1.Name = "oa_utilisateur_date_entree"
        GridViewDateTimeColumn1.Width = 55
        GridViewDateTimeColumn2.FieldName = "oa_utilisateur_date_sortie"
        GridViewDateTimeColumn2.FormatString = "{0:dd/MM/yyyy}"
        GridViewDateTimeColumn2.HeaderText = "Date sortie"
        GridViewDateTimeColumn2.IsAutoGenerated = True
        GridViewDateTimeColumn2.Name = "oa_utilisateur_date_sortie"
        GridViewDateTimeColumn2.Width = 55
        GridViewTextBoxColumn4.FieldName = "oa_utilisateur_etat"
        GridViewTextBoxColumn4.HeaderText = "Etat"
        GridViewTextBoxColumn4.IsAutoGenerated = True
        GridViewTextBoxColumn4.Name = "oa_utilisateur_etat"
        GridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn4.Width = 55
        GridViewCheckBoxColumn1.FieldName = "oa_utilisateur_admin"
        GridViewCheckBoxColumn1.HeaderText = "Admin"
        GridViewCheckBoxColumn1.IsAutoGenerated = True
        GridViewCheckBoxColumn1.Name = "oa_utilisateur_admin"
        GridViewCheckBoxColumn1.Width = 55
        GridViewTextBoxColumn5.FieldName = "oa_site_description"
        GridViewTextBoxColumn5.HeaderText = "Site"
        GridViewTextBoxColumn5.IsAutoGenerated = True
        GridViewTextBoxColumn5.Name = "oa_site_description"
        GridViewTextBoxColumn5.Width = 55
        GridViewTextBoxColumn6.FieldName = "oa_site_ville"
        GridViewTextBoxColumn6.HeaderText = "Ville"
        GridViewTextBoxColumn6.IsAutoGenerated = True
        GridViewTextBoxColumn6.Name = "oa_site_ville"
        GridViewTextBoxColumn6.Width = 55
        GridViewTextBoxColumn7.FieldName = "oa_site_code_postal"
        GridViewTextBoxColumn7.HeaderText = "CP"
        GridViewTextBoxColumn7.IsAutoGenerated = True
        GridViewTextBoxColumn7.Name = "oa_site_code_postal"
        GridViewTextBoxColumn7.Width = 55
        GridViewCheckBoxColumn2.FieldName = "oa_site_inactif"
        GridViewCheckBoxColumn2.HeaderText = "Inactif"
        GridViewCheckBoxColumn2.IsAutoGenerated = True
        GridViewCheckBoxColumn2.Name = "oa_site_inactif"
        GridViewCheckBoxColumn2.Width = 55
        GridViewTextBoxColumn8.FieldName = "oa_unite_sanitaire_description"
        GridViewTextBoxColumn8.HeaderText = "Unité Sanitaire"
        GridViewTextBoxColumn8.IsAutoGenerated = True
        GridViewTextBoxColumn8.Name = "oa_unite_sanitaire_description"
        GridViewTextBoxColumn8.Width = 55
        GridViewTextBoxColumn9.FieldName = "oa_unite_sanitaire_ville"
        GridViewTextBoxColumn9.HeaderText = "Ville"
        GridViewTextBoxColumn9.IsAutoGenerated = True
        GridViewTextBoxColumn9.Name = "oa_unite_sanitaire_ville"
        GridViewTextBoxColumn9.Width = 55
        GridViewTextBoxColumn10.FieldName = "oa_unite_sanitaire_code_postal"
        GridViewTextBoxColumn10.HeaderText = "CP"
        GridViewTextBoxColumn10.IsAutoGenerated = True
        GridViewTextBoxColumn10.Name = "oa_unite_sanitaire_code_postal"
        GridViewTextBoxColumn10.Width = 55
        GridViewCheckBoxColumn3.FieldName = "oa_unite_sanitaire_inactif"
        GridViewCheckBoxColumn3.HeaderText = "Inactive"
        GridViewCheckBoxColumn3.IsAutoGenerated = True
        GridViewCheckBoxColumn3.Name = "oa_unite_sanitaire_inactif"
        GridViewCheckBoxColumn3.Width = 58
        Me.RadGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewDecimalColumn1, GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewDateTimeColumn1, GridViewDateTimeColumn2, GridViewTextBoxColumn4, GridViewCheckBoxColumn1, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewCheckBoxColumn2, GridViewTextBoxColumn8, GridViewTextBoxColumn9, GridViewTextBoxColumn10, GridViewCheckBoxColumn3})
        Me.RadGridView1.MasterTemplate.DataSource = Me.VuserfullBindingSource
        Me.RadGridView1.MasterTemplate.EnableFiltering = True
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.Size = New System.Drawing.Size(835, 91)
        Me.RadGridView1.TabIndex = 0
        '
        'VuserfullBindingSource
        '
        Me.VuserfullBindingSource.DataMember = "v_user_full"
        Me.VuserfullBindingSource.DataSource = Me.DataSetUtilisateurBindingSource
        '
        'DataSetUtilisateurBindingSource
        '
        Me.DataSetUtilisateurBindingSource.DataSource = Me.DataSetUtilisateur
        Me.DataSetUtilisateurBindingSource.Position = 0
        '
        'DataSetUtilisateur
        '
        Me.DataSetUtilisateur.DataSetName = "dataSetUtilisateur"
        Me.DataSetUtilisateur.EnforceConstraints = False
        Me.DataSetUtilisateur.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'V_user_fullTableAdapter
        '
        Me.V_user_fullTableAdapter.ClearBeforeFill = True
        '
        'FrmUtilisateurListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(977, 270)
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
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VuserfullBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetUtilisateurBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetUtilisateur, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents BtnDelete As Button
    Friend WithEvents BtnUpdate As Button
    Friend WithEvents BtnCreate As Button
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadGridView1 As Telerik.WinControls.UI.RadGridView
    Friend WithEvents DataSetUtilisateurBindingSource As BindingSource
    Friend WithEvents DataSetUtilisateur As dataSetUtilisateur
    Friend WithEvents VuserfullBindingSource As BindingSource
    Friend WithEvents V_user_fullTableAdapter As dataSetUtilisateurTableAdapters.v_user_fullTableAdapter
End Class

