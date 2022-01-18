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
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn13 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn14 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn15 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn16 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.GVVaccin = New Telerik.WinControls.UI.RadGridView()
        Me.BtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.BtnCancel = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.LVOperator = New Telerik.WinControls.UI.RadListView()
        Me.DTPRealisation = New System.Windows.Forms.DateTimePicker()
        CType(Me.GVVaccin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVVaccin.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LVOperator, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.GVVaccin.Location = New System.Drawing.Point(12, 38)
        '
        '
        '
        Me.GVVaccin.MasterTemplate.AllowAddNewRow = False
        Me.GVVaccin.MasterTemplate.AllowDragToGroup = False
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.HeaderText = "id"
        GridViewTextBoxColumn9.IsVisible = False
        GridViewTextBoxColumn9.Name = "id"
        GridViewTextBoxColumn10.AllowGroup = False
        GridViewTextBoxColumn10.AllowResize = False
        GridViewTextBoxColumn10.AllowSort = False
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.HeaderText = "Nom"
        GridViewTextBoxColumn10.MinWidth = 200
        GridViewTextBoxColumn10.Name = "dci"
        GridViewTextBoxColumn10.ReadOnly = True
        GridViewTextBoxColumn10.VisibleInColumnChooser = False
        GridViewTextBoxColumn10.Width = 300
        GridViewTextBoxColumn11.AllowGroup = False
        GridViewTextBoxColumn11.AllowResize = False
        GridViewTextBoxColumn11.AllowSort = False
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.HeaderText = "Lot"
        GridViewTextBoxColumn11.Name = "lot"
        GridViewTextBoxColumn11.ReadOnly = True
        GridViewTextBoxColumn11.VisibleInColumnChooser = False
        GridViewTextBoxColumn11.Width = 80
        GridViewTextBoxColumn12.AllowGroup = False
        GridViewTextBoxColumn12.AllowResize = False
        GridViewTextBoxColumn12.AllowSort = False
        GridViewTextBoxColumn12.EnableExpressionEditor = False
        GridViewTextBoxColumn12.HeaderText = "Exp"
        GridViewTextBoxColumn12.MinWidth = 80
        GridViewTextBoxColumn12.Name = "expiration"
        GridViewTextBoxColumn12.ReadOnly = True
        GridViewTextBoxColumn12.VisibleInColumnChooser = False
        GridViewTextBoxColumn12.Width = 80
        GridViewTextBoxColumn13.AllowGroup = False
        GridViewTextBoxColumn13.AllowResize = False
        GridViewTextBoxColumn13.AllowSort = False
        GridViewTextBoxColumn13.EnableExpressionEditor = False
        GridViewTextBoxColumn13.HeaderText = "Réalisation"
        GridViewTextBoxColumn13.Name = "realisation"
        GridViewTextBoxColumn13.ReadOnly = True
        GridViewTextBoxColumn13.VisibleInColumnChooser = False
        GridViewTextBoxColumn13.Width = 160
        GridViewTextBoxColumn14.AllowGroup = False
        GridViewTextBoxColumn14.AllowResize = False
        GridViewTextBoxColumn14.AllowSort = False
        GridViewTextBoxColumn14.EnableExpressionEditor = False
        GridViewTextBoxColumn14.HeaderText = "Commentaire"
        GridViewTextBoxColumn14.Name = "comment"
        GridViewTextBoxColumn14.ReadOnly = True
        GridViewTextBoxColumn14.VisibleInColumnChooser = False
        GridViewTextBoxColumn14.Width = 300
        GridViewTextBoxColumn15.AllowGroup = False
        GridViewTextBoxColumn15.AllowResize = False
        GridViewTextBoxColumn15.AllowSort = False
        GridViewTextBoxColumn15.EnableExpressionEditor = False
        GridViewTextBoxColumn15.HeaderText = "column1"
        GridViewTextBoxColumn15.IsVisible = False
        GridViewTextBoxColumn15.Name = "relation_id"
        GridViewTextBoxColumn15.ReadOnly = True
        GridViewTextBoxColumn15.VisibleInColumnChooser = False
        GridViewTextBoxColumn16.AllowGroup = False
        GridViewTextBoxColumn16.AllowResize = False
        GridViewTextBoxColumn16.AllowSort = False
        GridViewTextBoxColumn16.EnableExpressionEditor = False
        GridViewTextBoxColumn16.HeaderText = "column1"
        GridViewTextBoxColumn16.IsVisible = False
        GridViewTextBoxColumn16.Name = "program_id"
        GridViewTextBoxColumn16.ReadOnly = True
        GridViewTextBoxColumn16.VisibleInColumnChooser = False
        Me.GVVaccin.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn9, GridViewTextBoxColumn10, GridViewTextBoxColumn11, GridViewTextBoxColumn12, GridViewTextBoxColumn13, GridViewTextBoxColumn14, GridViewTextBoxColumn15, GridViewTextBoxColumn16})
        Me.GVVaccin.MasterTemplate.ShowRowHeaderColumn = False
        Me.GVVaccin.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.GVVaccin.Name = "GVVaccin"
        Me.GVVaccin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GVVaccin.ShowCellErrors = False
        Me.GVVaccin.ShowGroupPanel = False
        Me.GVVaccin.ShowRowErrors = False
        Me.GVVaccin.Size = New System.Drawing.Size(948, 325)
        Me.GVVaccin.TabIndex = 0
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(851, 369)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.BtnValidation.TabIndex = 1
        Me.BtnValidation.Text = "Valider"
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(735, 369)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(110, 24)
        Me.BtnCancel.TabIndex = 2
        Me.BtnCancel.Text = "Cancel"
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(380, 12)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(60, 18)
        Me.RadLabel2.TabIndex = 9
        Me.RadLabel2.Text = "Operateur:"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(12, 12)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(114, 18)
        Me.RadLabel1.TabIndex = 8
        Me.RadLabel1.Text = "Date de la realisation:"
        '
        'LVOperator
        '
        Me.LVOperator.Location = New System.Drawing.Point(446, 12)
        Me.LVOperator.Name = "LVOperator"
        Me.LVOperator.Size = New System.Drawing.Size(515, 20)
        Me.LVOperator.TabIndex = 7
        '
        'DTPRealisation
        '
        Me.DTPRealisation.Location = New System.Drawing.Point(132, 12)
        Me.DTPRealisation.Name = "DTPRealisation"
        Me.DTPRealisation.Size = New System.Drawing.Size(227, 20)
        Me.DTPRealisation.TabIndex = 6
        '
        'RadFVaccinInput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(972, 401)
        Me.Controls.Add(Me.RadLabel2)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.LVOperator)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.DTPRealisation)
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
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LVOperator, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GVVaccin As Telerik.WinControls.UI.RadGridView
    Friend WithEvents BtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnCancel As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents LVOperator As Telerik.WinControls.UI.RadListView
    Friend WithEvents DTPRealisation As DateTimePicker
End Class

