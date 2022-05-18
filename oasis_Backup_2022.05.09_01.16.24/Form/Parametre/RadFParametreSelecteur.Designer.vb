<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFParametreSelecteur
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
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewParm = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadGbxSelection = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadBtnSelection = New Telerik.WinControls.UI.RadButton()
        Me.TbxUnite = New System.Windows.Forms.TextBox()
        Me.TbxDescription = New System.Windows.Forms.TextBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGbxSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGbxSelection.SuspendLayout()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.RadGridViewParm.Location = New System.Drawing.Point(12, 12)
        '
        '
        '
        Me.RadGridViewParm.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewParm.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewParm.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "id"
        GridViewTextBoxColumn1.HeaderText = "id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "id"
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "description"
        GridViewTextBoxColumn2.HeaderText = "Description"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.Name = "description"
        GridViewTextBoxColumn2.Width = 200
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "unite"
        GridViewTextBoxColumn3.HeaderText = "Unité"
        GridViewTextBoxColumn3.Name = "unite"
        GridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn3.Width = 65
        Me.RadGridViewParm.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3})
        Me.RadGridViewParm.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewParm.Name = "RadGridViewParm"
        Me.RadGridViewParm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParm.ShowGroupPanel = False
        Me.RadGridViewParm.Size = New System.Drawing.Size(309, 506)
        Me.RadGridViewParm.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(297, 524)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 1
        '
        'RadGbxSelection
        '
        Me.RadGbxSelection.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGbxSelection.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.RadGbxSelection.Controls.Add(Me.RadBtnSelection)
        Me.RadGbxSelection.Controls.Add(Me.TbxUnite)
        Me.RadGbxSelection.Controls.Add(Me.TbxDescription)
        Me.RadGbxSelection.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGbxSelection.HeaderText = "Paramètre sélectionné"
        Me.RadGbxSelection.Location = New System.Drawing.Point(327, 185)
        Me.RadGbxSelection.Name = "RadGbxSelection"
        Me.RadGbxSelection.Size = New System.Drawing.Size(358, 118)
        Me.RadGbxSelection.TabIndex = 2
        Me.RadGbxSelection.Text = "Paramètre sélectionné"
        '
        'RadBtnSelection
        '
        Me.RadBtnSelection.Image = Global.Oasis_WF.My.Resources.Resources._select
        Me.RadBtnSelection.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnSelection.Location = New System.Drawing.Point(5, 84)
        Me.RadBtnSelection.Name = "RadBtnSelection"
        Me.RadBtnSelection.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnSelection.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.RadBtnSelection, "Valider la sélection")
        '
        'TbxUnite
        '
        Me.TbxUnite.Enabled = False
        Me.TbxUnite.Location = New System.Drawing.Point(5, 58)
        Me.TbxUnite.Name = "TbxUnite"
        Me.TbxUnite.ReadOnly = True
        Me.TbxUnite.Size = New System.Drawing.Size(141, 20)
        Me.TbxUnite.TabIndex = 1
        '
        'TbxDescription
        '
        Me.TbxDescription.Enabled = False
        Me.TbxDescription.Location = New System.Drawing.Point(5, 32)
        Me.TbxDescription.Name = "TbxDescription"
        Me.TbxDescription.ReadOnly = True
        Me.TbxDescription.Size = New System.Drawing.Size(348, 20)
        Me.TbxDescription.TabIndex = 0
        '
        'RadFParametreSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(697, 557)
        Me.Controls.Add(Me.RadGbxSelection)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGridViewParm)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFParametreSelecteur"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Sélection paramètre"
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGbxSelection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGbxSelection.ResumeLayout(False)
        Me.RadGbxSelection.PerformLayout()
        CType(Me.RadBtnSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGridViewParm As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGbxSelection As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadBtnSelection As Telerik.WinControls.UI.RadButton
    Friend WithEvents TbxUnite As TextBox
    Friend WithEvents TbxDescription As TextBox
    Friend WithEvents ToolTip As ToolTip
End Class

