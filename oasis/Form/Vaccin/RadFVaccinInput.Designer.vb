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
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewDateTimeColumn1 As Telerik.WinControls.UI.GridViewDateTimeColumn = New Telerik.WinControls.UI.GridViewDateTimeColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
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
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "id"
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "dci"
        GridViewTextBoxColumn2.MinWidth = 200
        GridViewTextBoxColumn2.Name = "dci"
        GridViewTextBoxColumn2.Width = 300
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "Lot"
        GridViewTextBoxColumn3.Name = "Lot"
        GridViewTextBoxColumn3.Width = 80
        GridViewDateTimeColumn1.EnableExpressionEditor = False
        GridViewDateTimeColumn1.HeaderText = "Exp"
        GridViewDateTimeColumn1.Name = "exp"
        GridViewDateTimeColumn1.Width = 100
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "Réal"
        GridViewTextBoxColumn4.Name = "Réal"
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.HeaderText = "Commentaire"
        GridViewTextBoxColumn5.Name = "commentaire"
        Me.GVVaccin.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewDateTimeColumn1, GridViewTextBoxColumn4, GridViewTextBoxColumn5})
        Me.GVVaccin.MasterTemplate.ShowRowHeaderColumn = False
        Me.GVVaccin.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.GVVaccin.Name = "GVVaccin"
        Me.GVVaccin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GVVaccin.ShowCellErrors = False
        Me.GVVaccin.ShowGroupPanel = False
        Me.GVVaccin.ShowRowErrors = False
        Me.GVVaccin.Size = New System.Drawing.Size(755, 349)
        Me.GVVaccin.TabIndex = 0
        '
        'BtnValidation
        '
        Me.BtnValidation.Location = New System.Drawing.Point(657, 367)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.BtnValidation.TabIndex = 1
        Me.BtnValidation.Text = "Valider"
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(541, 367)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(110, 24)
        Me.BtnCancel.TabIndex = 2
        Me.BtnCancel.Text = "Cancel"
        '
        'RadFVaccinInput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(779, 401)
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

