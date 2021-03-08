<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RadFAutoSuivi
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
        Me.components = New System.ComponentModel.Container()
        Dim GridViewCheckBoxColumn1 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewAutoSuivi = New Telerik.WinControls.UI.RadGridView()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadGridViewAutoSuivi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewAutoSuivi.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGridViewAutoSuivi
        '
        Me.RadGridViewAutoSuivi.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewAutoSuivi.BeginEditMode = Telerik.WinControls.RadGridViewBeginEditMode.BeginEditProgrammatically
        Me.RadGridViewAutoSuivi.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewAutoSuivi.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadGridViewAutoSuivi.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewAutoSuivi.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewAutoSuivi.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewAutoSuivi.Location = New System.Drawing.Point(0, 0)
        Me.RadGridViewAutoSuivi.Margin = New System.Windows.Forms.Padding(3072)
        '
        '
        '
        Me.RadGridViewAutoSuivi.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewAutoSuivi.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewAutoSuivi.MasterTemplate.AllowEditRow = False
        GridViewCheckBoxColumn1.EnableExpressionEditor = False
        GridViewCheckBoxColumn1.HeaderText = "actif"
        GridViewCheckBoxColumn1.MinWidth = 20
        GridViewCheckBoxColumn1.Name = "actif"
        GridViewCheckBoxColumn1.Width = 30
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "description"
        GridViewTextBoxColumn1.MinWidth = 300
        GridViewTextBoxColumn1.Name = "description"
        GridViewTextBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.Width = 300
        Me.RadGridViewAutoSuivi.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewCheckBoxColumn1, GridViewTextBoxColumn1})
        Me.RadGridViewAutoSuivi.MasterTemplate.EnableGrouping = False
        Me.RadGridViewAutoSuivi.MasterTemplate.EnableSorting = False
        Me.RadGridViewAutoSuivi.MasterTemplate.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.None
        Me.RadGridViewAutoSuivi.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewAutoSuivi.Name = "RadGridViewAutoSuivi"
        Me.RadGridViewAutoSuivi.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewAutoSuivi.ShowGroupPanel = False
        Me.RadGridViewAutoSuivi.Size = New System.Drawing.Size(350, 420)
        Me.RadGridViewAutoSuivi.TabIndex = 4
        '
        'RadFAutoSuivi
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(350, 420)
        Me.Controls.Add(Me.RadGridViewAutoSuivi)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFAutoSuivi"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Parametre d'Auto-Suivi"
        CType(Me.RadGridViewAutoSuivi.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewAutoSuivi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents RadGridViewAutoSuivi As Telerik.WinControls.UI.RadGridView
End Class

