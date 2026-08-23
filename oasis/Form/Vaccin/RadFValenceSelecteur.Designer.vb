<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RadFValenceSelecteur
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
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor2 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition3 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn13 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn14 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition4 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RGVValenceVisible = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.RGVValenceNotVisible = New Telerik.WinControls.UI.RadGridView()
        Me.BtnValenceAdd = New System.Windows.Forms.Button()
        Me.BtnValenceRemove = New System.Windows.Forms.Button()
        Me.BtnValenceDown = New System.Windows.Forms.Button()
        Me.BtnValenceUp = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.RGVValenceVisible, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RGVValenceVisible.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RGVValenceNotVisible, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RGVValenceNotVisible.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RGVValenceVisible
        '
        Me.RGVValenceVisible.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RGVValenceVisible.Cursor = System.Windows.Forms.Cursors.Default
        Me.RGVValenceVisible.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RGVValenceVisible.ForeColor = System.Drawing.Color.Black
        Me.RGVValenceVisible.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RGVValenceVisible.Location = New System.Drawing.Point(12, 26)
        '
        '
        '
        Me.RGVValenceVisible.MasterTemplate.AllowAddNewRow = False
        Me.RGVValenceVisible.MasterTemplate.AllowDeleteRow = False
        Me.RGVValenceVisible.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.FieldName = "id"
        GridViewTextBoxColumn8.HeaderText = "id"
        GridViewTextBoxColumn8.IsVisible = False
        GridViewTextBoxColumn8.Name = "id"
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.FieldName = "unite"
        GridViewTextBoxColumn9.HeaderText = "Code"
        GridViewTextBoxColumn9.Name = "code"
        GridViewTextBoxColumn9.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn9.Width = 65
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.FieldName = "description"
        GridViewTextBoxColumn10.HeaderText = "Description"
        GridViewTextBoxColumn10.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn10.Name = "description"
        GridViewTextBoxColumn10.Width = 200
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.HeaderText = "column1"
        GridViewTextBoxColumn11.IsVisible = False
        GridViewTextBoxColumn11.Name = "order"
        Me.RGVValenceVisible.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn8, GridViewTextBoxColumn9, GridViewTextBoxColumn10, GridViewTextBoxColumn11})
        SortDescriptor2.PropertyName = "unite"
        Me.RGVValenceVisible.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor2})
        Me.RGVValenceVisible.MasterTemplate.ViewDefinition = TableViewDefinition3
        Me.RGVValenceVisible.Name = "RGVValenceVisible"
        Me.RGVValenceVisible.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RGVValenceVisible.ShowGroupPanel = False
        Me.RGVValenceVisible.Size = New System.Drawing.Size(309, 492)
        Me.RGVValenceVisible.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(664, 524)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 1
        '
        'RGVValenceNotVisible
        '
        Me.RGVValenceNotVisible.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RGVValenceNotVisible.Cursor = System.Windows.Forms.Cursors.Default
        Me.RGVValenceNotVisible.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RGVValenceNotVisible.ForeColor = System.Drawing.Color.Black
        Me.RGVValenceNotVisible.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RGVValenceNotVisible.Location = New System.Drawing.Point(379, 26)
        '
        '
        '
        Me.RGVValenceNotVisible.MasterTemplate.AllowAddNewRow = False
        Me.RGVValenceNotVisible.MasterTemplate.AllowDeleteRow = False
        Me.RGVValenceNotVisible.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn12.EnableExpressionEditor = False
        GridViewTextBoxColumn12.FieldName = "id"
        GridViewTextBoxColumn12.HeaderText = "id"
        GridViewTextBoxColumn12.IsVisible = False
        GridViewTextBoxColumn12.Name = "id"
        GridViewTextBoxColumn13.EnableExpressionEditor = False
        GridViewTextBoxColumn13.FieldName = "unite"
        GridViewTextBoxColumn13.HeaderText = "Code"
        GridViewTextBoxColumn13.Name = "code"
        GridViewTextBoxColumn13.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn13.Width = 65
        GridViewTextBoxColumn14.EnableExpressionEditor = False
        GridViewTextBoxColumn14.FieldName = "description"
        GridViewTextBoxColumn14.HeaderText = "Description"
        GridViewTextBoxColumn14.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn14.Name = "description"
        GridViewTextBoxColumn14.Width = 200
        Me.RGVValenceNotVisible.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn12, GridViewTextBoxColumn13, GridViewTextBoxColumn14})
        Me.RGVValenceNotVisible.MasterTemplate.ViewDefinition = TableViewDefinition4
        Me.RGVValenceNotVisible.Name = "RGVValenceNotVisible"
        Me.RGVValenceNotVisible.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RGVValenceNotVisible.ShowGroupPanel = False
        Me.RGVValenceNotVisible.Size = New System.Drawing.Size(309, 492)
        Me.RGVValenceNotVisible.TabIndex = 1
        '
        'BtnValenceAdd
        '
        Me.BtnValenceAdd.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnValenceAdd.Location = New System.Drawing.Point(327, 180)
        Me.BtnValenceAdd.Name = "BtnValenceAdd"
        Me.BtnValenceAdd.Size = New System.Drawing.Size(45, 45)
        Me.BtnValenceAdd.TabIndex = 2
        Me.BtnValenceAdd.Text = "←"
        Me.BtnValenceAdd.UseVisualStyleBackColor = True
        '
        'BtnValenceRemove
        '
        Me.BtnValenceRemove.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnValenceRemove.Location = New System.Drawing.Point(326, 231)
        Me.BtnValenceRemove.Name = "BtnValenceRemove"
        Me.BtnValenceRemove.Size = New System.Drawing.Size(45, 45)
        Me.BtnValenceRemove.TabIndex = 3
        Me.BtnValenceRemove.Text = "→"
        Me.BtnValenceRemove.UseVisualStyleBackColor = True
        '
        'BtnValenceDown
        '
        Me.BtnValenceDown.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnValenceDown.Location = New System.Drawing.Point(327, 100)
        Me.BtnValenceDown.Name = "BtnValenceDown"
        Me.BtnValenceDown.Size = New System.Drawing.Size(45, 45)
        Me.BtnValenceDown.TabIndex = 5
        Me.BtnValenceDown.Text = "↓"
        Me.BtnValenceDown.UseVisualStyleBackColor = True
        '
        'BtnValenceUp
        '
        Me.BtnValenceUp.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnValenceUp.Location = New System.Drawing.Point(327, 49)
        Me.BtnValenceUp.Name = "BtnValenceUp"
        Me.BtnValenceUp.Size = New System.Drawing.Size(45, 45)
        Me.BtnValenceUp.TabIndex = 4
        Me.BtnValenceUp.Text = "↑"
        Me.BtnValenceUp.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(245, 23)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Valence du calendrier general :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(376, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(347, 23)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Valence non associee au calendrier general :"
        '
        'RadFValenceSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(700, 557)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnValenceDown)
        Me.Controls.Add(Me.BtnValenceUp)
        Me.Controls.Add(Me.BtnValenceRemove)
        Me.Controls.Add(Me.BtnValenceAdd)
        Me.Controls.Add(Me.RGVValenceNotVisible)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RGVValenceVisible)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFValenceSelecteur"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Affichage valence"
        CType(Me.RGVValenceVisible.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RGVValenceVisible, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RGVValenceNotVisible.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RGVValenceNotVisible, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RGVValenceVisible As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents RGVValenceNotVisible As Telerik.WinControls.UI.RadGridView
    Friend WithEvents BtnValenceAdd As Button
    Friend WithEvents BtnValenceRemove As Button
    Friend WithEvents BtnValenceDown As Button
    Friend WithEvents BtnValenceUp As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class

