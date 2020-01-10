<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFDrcParametresEdit
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
        Dim GridViewTextBoxColumn13 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn14 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn15 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn5 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim TableViewDefinition5 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewParm = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.LblDrcDenomination = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGridViewParm
        '
        Me.RadGridViewParm.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewParm.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewParm.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewParm.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewParm.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewParm.Location = New System.Drawing.Point(12, 39)
        '
        '
        '
        Me.RadGridViewParm.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewParm.MasterTemplate.AllowCellContextMenu = False
        Me.RadGridViewParm.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewParm.MasterTemplate.AllowDragToGroup = False
        GridViewTextBoxColumn13.EnableExpressionEditor = False
        GridViewTextBoxColumn13.FieldName = "id"
        GridViewTextBoxColumn13.HeaderText = "id"
        GridViewTextBoxColumn13.IsVisible = False
        GridViewTextBoxColumn13.Name = "id"
        GridViewTextBoxColumn14.EnableExpressionEditor = False
        GridViewTextBoxColumn14.FieldName = "description"
        GridViewTextBoxColumn14.HeaderText = "description"
        GridViewTextBoxColumn14.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn14.Name = "description"
        GridViewTextBoxColumn14.Width = 150
        GridViewTextBoxColumn15.EnableExpressionEditor = False
        GridViewTextBoxColumn15.FieldName = "unite"
        GridViewTextBoxColumn15.HeaderText = "unité"
        GridViewTextBoxColumn15.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn15.Name = "unite"
        GridViewTextBoxColumn15.Width = 80
        GridViewCheckBoxColumn5.EnableExpressionEditor = False
        GridViewCheckBoxColumn5.FieldName = "selection"
        GridViewCheckBoxColumn5.HeaderText = "Sélection"
        GridViewCheckBoxColumn5.MinWidth = 20
        GridViewCheckBoxColumn5.Name = "selection"
        GridViewCheckBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadGridViewParm.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn13, GridViewTextBoxColumn14, GridViewTextBoxColumn15, GridViewCheckBoxColumn5})
        Me.RadGridViewParm.MasterTemplate.ShowFilteringRow = False
        Me.RadGridViewParm.MasterTemplate.ViewDefinition = TableViewDefinition5
        Me.RadGridViewParm.Name = "RadGridViewParm"
        Me.RadGridViewParm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParm.ShowGroupPanel = False
        Me.RadGridViewParm.Size = New System.Drawing.Size(321, 532)
        Me.RadGridViewParm.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(223, 586)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 1
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Location = New System.Drawing.Point(107, 586)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnValidation.TabIndex = 2
        Me.RadBtnValidation.Text = "Validation"
        '
        'LblDrcDenomination
        '
        Me.LblDrcDenomination.AutoSize = True
        Me.LblDrcDenomination.Location = New System.Drawing.Point(143, 9)
        Me.LblDrcDenomination.Name = "LblDrcDenomination"
        Me.LblDrcDenomination.Size = New System.Drawing.Size(100, 13)
        Me.LblDrcDenomination.TabIndex = 3
        Me.LblDrcDenomination.Text = "Drc dénomination"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Groupe de paramètres : "
        '
        'RadFDrcParametresEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(343, 622)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblDrcDenomination)
        Me.Controls.Add(Me.RadBtnValidation)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGridViewParm)
        Me.MinimizeBox = False
        Me.Name = "RadFDrcParametresEdit"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Saisie paramètres de la DRC"
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadGridViewParm As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblDrcDenomination As Label
    Friend WithEvents Label1 As Label
End Class

