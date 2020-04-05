<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFWkfCommentaire
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
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewWkfCommentaire = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGridViewWkfCommentaire, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewWkfCommentaire.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGridViewWkfCommentaire
        '
        Me.RadGridViewWkfCommentaire.AutoSizeRows = True
        Me.RadGridViewWkfCommentaire.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewWkfCommentaire.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewWkfCommentaire.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewWkfCommentaire.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewWkfCommentaire.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewWkfCommentaire.Location = New System.Drawing.Point(12, 12)
        '
        '
        '
        Me.RadGridViewWkfCommentaire.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewWkfCommentaire.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewWkfCommentaire.MasterTemplate.AllowDragToGroup = False
        Me.RadGridViewWkfCommentaire.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "column1"
        GridViewTextBoxColumn1.Name = "emetteurNom"
        GridViewTextBoxColumn1.Width = 300
        GridViewTextBoxColumn2.AllowGroup = False
        GridViewTextBoxColumn2.AllowSort = False
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "column1"
        GridViewTextBoxColumn2.Name = "emetteurCommentaire"
        GridViewTextBoxColumn2.VisibleInColumnChooser = False
        GridViewTextBoxColumn2.Width = 500
        Me.RadGridViewWkfCommentaire.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2})
        Me.RadGridViewWkfCommentaire.MasterTemplate.EnableGrouping = False
        Me.RadGridViewWkfCommentaire.MasterTemplate.ShowColumnHeaders = False
        Me.RadGridViewWkfCommentaire.MasterTemplate.ShowFilteringRow = False
        Me.RadGridViewWkfCommentaire.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewWkfCommentaire.Name = "RadGridViewWkfCommentaire"
        Me.RadGridViewWkfCommentaire.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewWkfCommentaire.ShowGroupPanel = False
        Me.RadGridViewWkfCommentaire.Size = New System.Drawing.Size(840, 150)
        Me.RadGridViewWkfCommentaire.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(742, 168)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 1
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadFWkfCommentaire
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(858, 197)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGridViewWkfCommentaire)
        Me.Name = "RadFWkfCommentaire"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFWkfCommentaire"
        CType(Me.RadGridViewWkfCommentaire.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewWkfCommentaire, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGridViewWkfCommentaire As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
End Class

