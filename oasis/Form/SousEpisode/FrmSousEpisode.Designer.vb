<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmSousEpisode
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
        Dim GridViewCheckBoxColumn1 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewDateTimeColumn1 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn2 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn3 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn2 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewDateTimeColumn4 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.BtnValidate = New Telerik.WinControls.UI.RadButton()
        Me.BtnDetail = New Telerik.WinControls.UI.RadButton()
        Me.BtnCreate = New Telerik.WinControls.UI.RadButton()
        Me.BtnCancel = New Telerik.WinControls.UI.RadButton()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.RadSousEpisodeGrid = New Telerik.WinControls.UI.RadGridView()
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
        GridViewTextBoxColumn1.DataType = GetType(Long)
        GridViewTextBoxColumn1.FieldName = "id"
        GridViewTextBoxColumn1.HeaderText = "Id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "id"
        GridViewTextBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleRight
        GridViewTextBoxColumn1.Width = 43
        GridViewCheckBoxColumn1.HeaderText = "ALD"
        GridViewCheckBoxColumn1.Name = "IsAld"
        GridViewCheckBoxColumn1.ReadOnly = True
        GridViewCheckBoxColumn1.Width = 26
        GridViewDateTimeColumn1.HeaderText = "Création"
        GridViewDateTimeColumn1.Name = "HorodateCreation"
        GridViewDateTimeColumn1.ReadOnly = True
        GridViewDateTimeColumn1.Width = 59
        GridViewTextBoxColumn2.HeaderText = "Par"
        GridViewTextBoxColumn2.Name = "CreateUser"
        GridViewTextBoxColumn2.ReadOnly = True
        GridViewTextBoxColumn2.Width = 119
        GridViewDateTimeColumn2.HeaderText = "Mise à jour"
        GridViewDateTimeColumn2.Name = "HorodateLastUpdate"
        GridViewDateTimeColumn2.ReadOnly = True
        GridViewDateTimeColumn2.Width = 61
        GridViewTextBoxColumn3.HeaderText = "Par"
        GridViewTextBoxColumn3.Name = "LastUpdateUser"
        GridViewTextBoxColumn3.ReadOnly = True
        GridViewTextBoxColumn3.Width = 118
        GridViewDateTimeColumn3.HeaderText = "Validation"
        GridViewDateTimeColumn3.Name = "HorodateValidate"
        GridViewDateTimeColumn3.Width = 60
        GridViewTextBoxColumn4.HeaderText = "Par"
        GridViewTextBoxColumn4.Name = "ValidateUser"
        GridViewTextBoxColumn4.Width = 112
        GridViewTextBoxColumn5.HeaderText = "Type"
        GridViewTextBoxColumn5.Name = "Type"
        GridViewTextBoxColumn5.ReadOnly = True
        GridViewTextBoxColumn5.Width = 190
        GridViewTextBoxColumn6.HeaderText = "Nom Fichier"
        GridViewTextBoxColumn6.Name = "NomFichier"
        GridViewTextBoxColumn6.ReadOnly = True
        GridViewTextBoxColumn6.Width = 217
        GridViewTextBoxColumn7.HeaderText = "Commentaire"
        GridViewTextBoxColumn7.Name = "Commentaire"
        GridViewTextBoxColumn7.ReadOnly = True
        GridViewTextBoxColumn7.Width = 215
        GridViewTextBoxColumn8.HeaderText = "ValidationProfilTypes"
        GridViewTextBoxColumn8.IsVisible = False
        GridViewTextBoxColumn8.Name = "ValidationProfilTypes"
        GridViewTextBoxColumn8.VisibleInColumnChooser = False
        GridViewTextBoxColumn8.Width = 46
        GridViewCheckBoxColumn2.HeaderText = "Réponse reçue"
        GridViewCheckBoxColumn2.Name = "IsReponseRecue"
        GridViewCheckBoxColumn2.ReadOnly = True
        GridViewCheckBoxColumn2.Width = 61
        GridViewDateTimeColumn4.HeaderText = "Date réception"
        GridViewDateTimeColumn4.Name = "HorodateLastRecu"
        GridViewDateTimeColumn4.ReadOnly = True
        GridViewDateTimeColumn4.Width = 47
        Me.RadSousEpisodeGrid.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewCheckBoxColumn1, GridViewDateTimeColumn1, GridViewTextBoxColumn2, GridViewDateTimeColumn2, GridViewTextBoxColumn3, GridViewDateTimeColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8, GridViewCheckBoxColumn2, GridViewDateTimeColumn4})
        Me.RadSousEpisodeGrid.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadSousEpisodeGrid.Name = "RadSousEpisodeGrid"
        Me.RadSousEpisodeGrid.ReadOnly = True
        Me.RadSousEpisodeGrid.Size = New System.Drawing.Size(1295, 332)
        Me.RadSousEpisodeGrid.TabIndex = 3
        '
        'FrmSousEpisode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(1298, 535)
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
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sous-Episodes"
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
End Class

