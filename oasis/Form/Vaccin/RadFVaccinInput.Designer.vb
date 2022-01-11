<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFVaccinInput
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
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn2 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor2 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.GVVaccin = New Telerik.WinControls.UI.RadGridView()
        Me.BtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.BtnCancel = New Telerik.WinControls.UI.RadButton()
        CType(Me.GVVaccin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVVaccin.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GVVaccin
        '
        Me.GVVaccin.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GVVaccin.Cursor = System.Windows.Forms.Cursors.Default
        Me.GVVaccin.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.GVVaccin.ForeColor = System.Drawing.Color.Black
        Me.GVVaccin.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.GVVaccin.Location = New System.Drawing.Point(12, 12)
        '
        '
        '
        Me.GVVaccin.MasterTemplate.AllowAddNewRow = False
        Me.GVVaccin.MasterTemplate.AllowDragToGroup = False
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.HeaderText = "id"
        GridViewTextBoxColumn6.IsVisible = False
        GridViewTextBoxColumn6.Name = "id"
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.HeaderText = "Nom"
        GridViewTextBoxColumn7.MinWidth = 200
        GridViewTextBoxColumn7.Name = "dci"
        GridViewTextBoxColumn7.Width = 300
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.HeaderText = "Lot"
        GridViewTextBoxColumn8.Name = "Lot"
        GridViewTextBoxColumn8.Width = 80
        GridViewDateTimeColumn2.CustomFormat = "MM/yyyy"
        GridViewDateTimeColumn2.DateTimeKind = System.DateTimeKind.Local
        GridViewDateTimeColumn2.EnableExpressionEditor = False
        GridViewDateTimeColumn2.FilteringTimePrecision = Telerik.WinControls.UI.GridViewTimePrecisionMode.None
        GridViewDateTimeColumn2.HeaderText = "Exp"
        GridViewDateTimeColumn2.Name = "exp"
        GridViewDateTimeColumn2.Width = 100
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.HeaderText = "Réalisation"
        GridViewTextBoxColumn9.Name = "realisation"
        GridViewTextBoxColumn9.Width = 160
        GridViewTextBoxColumn10.AllowGroup = False
        GridViewTextBoxColumn10.AllowResize = False
        GridViewTextBoxColumn10.AllowSort = False
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.HeaderText = "Commentaire"
        GridViewTextBoxColumn10.Name = "commentaire"
        GridViewTextBoxColumn10.ReadOnly = True
        GridViewTextBoxColumn10.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending
        GridViewTextBoxColumn10.VisibleInColumnChooser = False
        GridViewTextBoxColumn10.Width = 300
        Me.GVVaccin.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8, GridViewDateTimeColumn2, GridViewTextBoxColumn9, GridViewTextBoxColumn10})
        Me.GVVaccin.MasterTemplate.ShowRowHeaderColumn = False
        SortDescriptor2.PropertyName = "commentaire"
        Me.GVVaccin.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor2})
        Me.GVVaccin.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.GVVaccin.Name = "GVVaccin"
        Me.GVVaccin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GVVaccin.ShowCellErrors = False
        Me.GVVaccin.ShowGroupPanel = False
        Me.GVVaccin.ShowRowErrors = False
        Me.GVVaccin.Size = New System.Drawing.Size(948, 349)
        Me.GVVaccin.TabIndex = 0
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(851, 367)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.BtnValidation.TabIndex = 1
        Me.BtnValidation.Text = "Valider"
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(735, 367)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(110, 24)
        Me.BtnCancel.TabIndex = 2
        Me.BtnCancel.Text = "Cancel"
        '
        'RadFVaccinInput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(972, 400)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.GVVaccin)
        Me.Name = "RadFVaccinInput"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFVaccinInput"
        CType(Me.GVVaccin.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVVaccin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GVVaccin As Telerik.WinControls.UI.RadGridView
    Friend WithEvents BtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnCancel As Telerik.WinControls.UI.RadButton
End Class

