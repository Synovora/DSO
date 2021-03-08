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
        Me.components = New System.ComponentModel.Container()
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn1 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewParm = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.LblDrcDenomination = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
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
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "id"
        GridViewTextBoxColumn1.HeaderText = "id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "id"
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "description"
        GridViewTextBoxColumn2.HeaderText = "description"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.Name = "description"
        GridViewTextBoxColumn2.Width = 150
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "unite"
        GridViewTextBoxColumn3.HeaderText = "unité"
        GridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn3.Name = "unite"
        GridViewTextBoxColumn3.Width = 80
        GridViewCheckBoxColumn1.EnableExpressionEditor = False
        GridViewCheckBoxColumn1.FieldName = "selection"
        GridViewCheckBoxColumn1.HeaderText = "Sélection"
        GridViewCheckBoxColumn1.MinWidth = 20
        GridViewCheckBoxColumn1.Name = "selection"
        GridViewCheckBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadGridViewParm.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewCheckBoxColumn1})
        Me.RadGridViewParm.MasterTemplate.ShowFilteringRow = False
        Me.RadGridViewParm.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewParm.Name = "RadGridViewParm"
        Me.RadGridViewParm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParm.ShowGroupPanel = False
        Me.RadGridViewParm.Size = New System.Drawing.Size(321, 532)
        Me.RadGridViewParm.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(307, 586)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 1
        '
        'RadBtnValidation
        '
        Me.RadBtnValidation.Image = Global.Oasis_WF.My.Resources.Resources.validation2
        Me.RadBtnValidation.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnValidation.Location = New System.Drawing.Point(12, 586)
        Me.RadBtnValidation.Name = "RadBtnValidation"
        Me.RadBtnValidation.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnValidation.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.RadBtnValidation, "Valider")
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
    Friend WithEvents ToolTip As ToolTip
End Class

