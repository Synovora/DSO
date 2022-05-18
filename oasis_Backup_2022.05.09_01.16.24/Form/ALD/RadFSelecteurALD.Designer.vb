<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFSelecteurALD
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
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition3 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.AldDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.PnlSelection = New System.Windows.Forms.Panel()
        Me.BtnSelection = New Telerik.WinControls.UI.RadButton()
        Me.LblAldCode = New System.Windows.Forms.Label()
        Me.LblAldDescription = New System.Windows.Forms.Label()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        CType(Me.AldDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AldDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSelection.SuspendLayout()
        CType(Me.BtnSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AldDataGridView
        '
        Me.AldDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.AldDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.AldDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.AldDataGridView.ForeColor = System.Drawing.Color.Black
        Me.AldDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.AldDataGridView.Location = New System.Drawing.Point(12, 26)
        '
        '
        '
        Me.AldDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.AldDataGridView.MasterTemplate.AllowDeleteRow = False
        Me.AldDataGridView.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.FieldName = "oa_ald_id"
        GridViewTextBoxColumn7.HeaderText = "Id."
        GridViewTextBoxColumn7.Name = "oa_ald_id"
        GridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn7.Width = 80
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.FieldName = "oa_ald_code"
        GridViewTextBoxColumn8.HeaderText = "Code"
        GridViewTextBoxColumn8.Name = "oa_ald_code"
        GridViewTextBoxColumn8.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn8.Width = 80
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.FieldName = "oa_ald_description"
        GridViewTextBoxColumn9.HeaderText = "Description"
        GridViewTextBoxColumn9.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn9.Name = "oa_ald_description"
        GridViewTextBoxColumn9.Width = 400
        Me.AldDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn7, GridViewTextBoxColumn8, GridViewTextBoxColumn9})
        Me.AldDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition3
        Me.AldDataGridView.Name = "AldDataGridView"
        Me.AldDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.AldDataGridView.ShowGroupPanel = False
        Me.AldDataGridView.Size = New System.Drawing.Size(605, 282)
        Me.AldDataGridView.TabIndex = 0
        '
        'PnlSelection
        '
        Me.PnlSelection.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.PnlSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PnlSelection.Controls.Add(Me.BtnSelection)
        Me.PnlSelection.Controls.Add(Me.LblAldCode)
        Me.PnlSelection.Controls.Add(Me.LblAldDescription)
        Me.PnlSelection.Location = New System.Drawing.Point(623, 99)
        Me.PnlSelection.Name = "PnlSelection"
        Me.PnlSelection.Size = New System.Drawing.Size(273, 131)
        Me.PnlSelection.TabIndex = 12
        '
        'BtnSelection
        '
        Me.BtnSelection.Image = Global.Oasis_WF.My.Resources.Resources._select
        Me.BtnSelection.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.BtnSelection.Location = New System.Drawing.Point(23, 78)
        Me.BtnSelection.Name = "BtnSelection"
        Me.BtnSelection.Size = New System.Drawing.Size(24, 24)
        Me.BtnSelection.TabIndex = 3
        Me.BtnSelection.Text = "RadButtonDelete"
        '
        'LblAldCode
        '
        Me.LblAldCode.AutoSize = True
        Me.LblAldCode.Location = New System.Drawing.Point(20, 46)
        Me.LblAldCode.Name = "LblAldCode"
        Me.LblAldCode.Size = New System.Drawing.Size(19, 13)
        Me.LblAldCode.TabIndex = 1
        Me.LblAldCode.Text = "43"
        '
        'LblAldDescription
        '
        Me.LblAldDescription.AutoSize = True
        Me.LblAldDescription.Location = New System.Drawing.Point(93, 46)
        Me.LblAldDescription.Name = "LblAldDescription"
        Me.LblAldDescription.Size = New System.Drawing.Size(45, 13)
        Me.LblAldDescription.TabIndex = 2
        Me.LblAldDescription.Text = "Brûlure"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(872, 310)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 13
        '
        'RadFSelecteurALD
        '
        Me.AcceptButton = Me.BtnSelection
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(910, 337)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.PnlSelection)
        Me.Controls.Add(Me.AldDataGridView)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFSelecteurALD"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "RadFSelecteurALD"
        CType(Me.AldDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AldDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSelection.ResumeLayout(False)
        Me.PnlSelection.PerformLayout()
        CType(Me.BtnSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AldDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents PnlSelection As Panel
    Friend WithEvents BtnSelection As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblAldCode As Label
    Friend WithEvents LblAldDescription As Label
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
End Class

