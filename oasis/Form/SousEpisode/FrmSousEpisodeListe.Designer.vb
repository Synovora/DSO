<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmSousEpisodeListe
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
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn1 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCommandColumn1 As Telerik.WinControls.UI.GridViewCommandColumn = New Telerik.WinControls.UI.GridViewCommandColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn1 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewDateTimeColumn2 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn3 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn4 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn2 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn3 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewDateTimeColumn5 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.SubGridDocs = New Telerik.WinControls.UI.GridViewTemplate()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.BtnValidate = New Telerik.WinControls.UI.RadButton()
        Me.BtnDetail = New Telerik.WinControls.UI.RadButton()
        Me.BtnCreate = New Telerik.WinControls.UI.RadButton()
        Me.BtnCancel = New Telerik.WinControls.UI.RadButton()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.RadSousEpisodeGrid = New Telerik.WinControls.UI.RadGridView()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        CType(Me.SubGridDocs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.BtnValidate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnCreate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me.RadSousEpisodeGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadSousEpisodeGrid.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SubGridDocs
        '
        Me.SubGridDocs.AllowAddNewRow = False
        Me.SubGridDocs.AllowColumnChooser = False
        Me.SubGridDocs.AllowDeleteRow = False
        Me.SubGridDocs.AllowDragToGroup = False
        Me.SubGridDocs.AllowEditRow = False
        Me.SubGridDocs.AllowMultiColumnSorting = False
        Me.SubGridDocs.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
        Me.SubGridDocs.AutoUpdateObjectRelationalSource = False
        Me.SubGridDocs.Caption = "Documents reçus attachés (réponses)"
        GridViewTextBoxColumn1.HeaderText = "Id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "Id"
        GridViewTextBoxColumn2.HeaderText = "IdSousEpisode"
        GridViewTextBoxColumn2.IsVisible = False
        GridViewTextBoxColumn2.Name = "IdSousEpisode"
        GridViewDateTimeColumn1.FormatString = "{0:dd/MM/yyyy HH:mm}"
        GridViewDateTimeColumn1.HeaderText = "Création"
        GridViewDateTimeColumn1.Name = "HorodateCreation"
        GridViewTextBoxColumn3.HeaderText = "Par"
        GridViewTextBoxColumn3.Name = "CreateUser"
        GridViewTextBoxColumn4.HeaderText = "Nom du Fichier"
        GridViewTextBoxColumn4.Name = "NomFichier"
        GridViewCommandColumn1.DefaultText = "Ouvrir"
        GridViewCommandColumn1.HeaderText = "Ouvrir"
        GridViewCommandColumn1.MaxWidth = 100
        GridViewCommandColumn1.Name = "SgBtnDownload"
        GridViewCommandColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewCommandColumn1.UseDefaultText = True
        GridViewTextBoxColumn5.HeaderText = "Commentaire"
        GridViewTextBoxColumn5.Name = "Commentaire"
        Me.SubGridDocs.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewDateTimeColumn1, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewCommandColumn1, GridViewTextBoxColumn5})
        Me.SubGridDocs.EnableGrouping = False
        Me.SubGridDocs.EnableSorting = False
        Me.SubGridDocs.ReadOnly = True
        Me.SubGridDocs.ShowChildViewCaptions = True
        Me.SubGridDocs.ShowFilterCellOperatorText = False
        Me.SubGridDocs.ShowFilteringRow = False
        Me.SubGridDocs.ViewDefinition = TableViewDefinition1
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.BtnValidate)
        Me.RadPanel1.Controls.Add(Me.BtnDetail)
        Me.RadPanel1.Controls.Add(Me.BtnCreate)
        Me.RadPanel1.Controls.Add(Me.BtnCancel)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel1.Location = New System.Drawing.Point(0, 498)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(1298, 37)
        Me.RadPanel1.TabIndex = 0
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
        Me.BtnCancel.Location = New System.Drawing.Point(1162, 6)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(110, 24)
        Me.BtnCancel.TabIndex = 7
        Me.BtnCancel.Text = "Abandonner"
        '
        'RadPanel2
        '
        Me.RadPanel2.Controls.Add(Me.RadSousEpisodeGrid)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel2.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(1298, 498)
        Me.RadPanel2.TabIndex = 1
        '
        'RadSousEpisodeGrid
        '
        Me.RadSousEpisodeGrid.EnableCustomDrawing = True
        Me.RadSousEpisodeGrid.Location = New System.Drawing.Point(0, 0)
        '
        '
        '
        Me.RadSousEpisodeGrid.MasterTemplate.AllowAddNewRow = False
        Me.RadSousEpisodeGrid.MasterTemplate.AllowColumnChooser = False
        Me.RadSousEpisodeGrid.MasterTemplate.AllowEditRow = False
        Me.RadSousEpisodeGrid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
        GridViewTextBoxColumn6.DataType = GetType(Long)
        GridViewTextBoxColumn6.FieldName = "IdSousEpisode"
        GridViewTextBoxColumn6.HeaderText = "IdSousEpisode"
        GridViewTextBoxColumn6.IsVisible = False
        GridViewTextBoxColumn6.Name = "IdSousEpisode"
        GridViewTextBoxColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleRight
        GridViewTextBoxColumn6.Width = 43
        GridViewCheckBoxColumn1.HeaderText = "ALD"
        GridViewCheckBoxColumn1.MinWidth = 30
        GridViewCheckBoxColumn1.Name = "IsAld"
        GridViewCheckBoxColumn1.ReadOnly = True
        GridViewCheckBoxColumn1.Width = 36
        GridViewDateTimeColumn2.FormatString = "{0:dd/MM/yyyy HH:mm}"
        GridViewDateTimeColumn2.HeaderText = "Création"
        GridViewDateTimeColumn2.Name = "HorodateCreation"
        GridViewDateTimeColumn2.ReadOnly = True
        GridViewDateTimeColumn2.Width = 67
        GridViewTextBoxColumn7.HeaderText = "Par"
        GridViewTextBoxColumn7.Name = "CreateUser"
        GridViewTextBoxColumn7.ReadOnly = True
        GridViewTextBoxColumn7.Width = 135
        GridViewDateTimeColumn3.FormatString = "{0:dd/MM/yyyy HH:mm}"
        GridViewDateTimeColumn3.HeaderText = "Mise à jour"
        GridViewDateTimeColumn3.Name = "HorodateLastUpdate"
        GridViewDateTimeColumn3.ReadOnly = True
        GridViewDateTimeColumn3.Width = 69
        GridViewTextBoxColumn8.HeaderText = "Par"
        GridViewTextBoxColumn8.Name = "LastUpdateUser"
        GridViewTextBoxColumn8.ReadOnly = True
        GridViewTextBoxColumn8.Width = 134
        GridViewDateTimeColumn4.FormatString = "{0:dd/MM/yyyy HH:mm}"
        GridViewDateTimeColumn4.HeaderText = "Validation"
        GridViewDateTimeColumn4.Name = "HorodateValidate"
        GridViewDateTimeColumn4.Width = 68
        GridViewTextBoxColumn9.HeaderText = "Par"
        GridViewTextBoxColumn9.Name = "ValidateUser"
        GridViewTextBoxColumn9.Width = 126
        GridViewTextBoxColumn10.HeaderText = "Type"
        GridViewTextBoxColumn10.Name = "Type"
        GridViewTextBoxColumn10.ReadOnly = True
        GridViewTextBoxColumn10.Width = 215
        GridViewCheckBoxColumn2.HeaderText = "Réponse Requise"
        GridViewCheckBoxColumn2.Name = "IsReponse"
        GridViewCheckBoxColumn2.Width = 56
        GridViewTextBoxColumn11.HeaderText = "Commentaire"
        GridViewTextBoxColumn11.Name = "Commentaire"
        GridViewTextBoxColumn11.ReadOnly = True
        GridViewTextBoxColumn11.Width = 242
        GridViewTextBoxColumn12.HeaderText = "ValidationProfilTypes"
        GridViewTextBoxColumn12.IsVisible = False
        GridViewTextBoxColumn12.Name = "ValidationProfilTypes"
        GridViewTextBoxColumn12.VisibleInColumnChooser = False
        GridViewTextBoxColumn12.Width = 46
        GridViewCheckBoxColumn3.HeaderText = "Réponse reçue"
        GridViewCheckBoxColumn3.Name = "IsReponseRecue"
        GridViewCheckBoxColumn3.ReadOnly = True
        GridViewCheckBoxColumn3.Width = 69
        GridViewDateTimeColumn5.FormatString = "{0:dd/MM/yyyy HH:mm}"
        GridViewDateTimeColumn5.HeaderText = "Date réception"
        GridViewDateTimeColumn5.Name = "HorodateLastRecu"
        GridViewDateTimeColumn5.ReadOnly = True
        GridViewDateTimeColumn5.Width = 47
        Me.RadSousEpisodeGrid.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn6, GridViewCheckBoxColumn1, GridViewDateTimeColumn2, GridViewTextBoxColumn7, GridViewDateTimeColumn3, GridViewTextBoxColumn8, GridViewDateTimeColumn4, GridViewTextBoxColumn9, GridViewTextBoxColumn10, GridViewCheckBoxColumn2, GridViewTextBoxColumn11, GridViewTextBoxColumn12, GridViewCheckBoxColumn3, GridViewDateTimeColumn5})
        Me.RadSousEpisodeGrid.MasterTemplate.Templates.AddRange(New Telerik.WinControls.UI.GridViewTemplate() {Me.SubGridDocs})
        Me.RadSousEpisodeGrid.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadSousEpisodeGrid.Name = "RadSousEpisodeGrid"
        Me.RadSousEpisodeGrid.ReadOnly = True
        Me.RadSousEpisodeGrid.Size = New System.Drawing.Size(1295, 332)
        Me.RadSousEpisodeGrid.TabIndex = 3
        '
        'FrmSousEpisodeListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(1298, 535)
        Me.Controls.Add(Me.RadPanel2)
        Me.Controls.Add(Me.RadPanel1)
        Me.MinimizeBox = False
        Me.Name = "FrmSousEpisodeListe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sous-Episodes"
        CType(Me.SubGridDocs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        CType(Me.BtnValidate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnCreate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel2.ResumeLayout(False)
        CType(Me.RadSousEpisodeGrid.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadSousEpisodeGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadSousEpisodeGrid As Telerik.WinControls.UI.RadGridView
    Friend WithEvents BtnCancel As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnValidate As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnDetail As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnCreate As Telerik.WinControls.UI.RadButton
    Friend WithEvents SubGridDocs As Telerik.WinControls.UI.GridViewTemplate
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
End Class

