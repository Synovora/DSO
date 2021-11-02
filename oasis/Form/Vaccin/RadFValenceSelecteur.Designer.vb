<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFValenceSelecteur
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
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor1 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RGVValenceVisible = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.RGVValenceNotVisible = New Telerik.WinControls.UI.RadGridView()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.BtnRemove = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.BtnUp = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnAddValence = New Telerik.WinControls.UI.RadButton()
        CType(Me.RGVValenceVisible, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RGVValenceVisible.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RGVValenceNotVisible, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RGVValenceNotVisible.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnAddValence, System.ComponentModel.ISupportInitialize).BeginInit()
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
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "id"
        GridViewTextBoxColumn1.HeaderText = "id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "id"
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "unite"
        GridViewTextBoxColumn2.HeaderText = "Code"
        GridViewTextBoxColumn2.Name = "code"
        GridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn2.Width = 65
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "description"
        GridViewTextBoxColumn3.HeaderText = "Description"
        GridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn3.Name = "description"
        GridViewTextBoxColumn3.Width = 200
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "column1"
        GridViewTextBoxColumn4.IsVisible = False
        GridViewTextBoxColumn4.Name = "order"
        Me.RGVValenceVisible.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4})
        SortDescriptor1.PropertyName = "unite"
        Me.RGVValenceVisible.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor1})
        Me.RGVValenceVisible.MasterTemplate.ViewDefinition = TableViewDefinition1
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
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.FieldName = "id"
        GridViewTextBoxColumn5.HeaderText = "id"
        GridViewTextBoxColumn5.IsVisible = False
        GridViewTextBoxColumn5.Name = "id"
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.FieldName = "unite"
        GridViewTextBoxColumn6.HeaderText = "Code"
        GridViewTextBoxColumn6.Name = "code"
        GridViewTextBoxColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn6.Width = 65
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.FieldName = "description"
        GridViewTextBoxColumn7.HeaderText = "Description"
        GridViewTextBoxColumn7.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn7.Name = "description"
        GridViewTextBoxColumn7.Width = 200
        Me.RGVValenceNotVisible.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7})
        Me.RGVValenceNotVisible.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RGVValenceNotVisible.Name = "RGVValenceNotVisible"
        Me.RGVValenceNotVisible.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RGVValenceNotVisible.ShowGroupPanel = False
        Me.RGVValenceNotVisible.Size = New System.Drawing.Size(309, 492)
        Me.RGVValenceNotVisible.TabIndex = 1
        '
        'BtnAdd
        '
        Me.BtnAdd.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAdd.Location = New System.Drawing.Point(327, 180)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(45, 45)
        Me.BtnAdd.TabIndex = 2
        Me.BtnAdd.Text = "←"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'BtnRemove
        '
        Me.BtnRemove.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRemove.Location = New System.Drawing.Point(326, 231)
        Me.BtnRemove.Name = "BtnRemove"
        Me.BtnRemove.Size = New System.Drawing.Size(45, 45)
        Me.BtnRemove.TabIndex = 3
        Me.BtnRemove.Text = "→"
        Me.BtnRemove.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(327, 100)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(45, 45)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "↓"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'BtnUp
        '
        Me.BtnUp.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUp.Location = New System.Drawing.Point(327, 49)
        Me.BtnUp.Name = "BtnUp"
        Me.BtnUp.Size = New System.Drawing.Size(45, 45)
        Me.BtnUp.TabIndex = 4
        Me.BtnUp.Text = "↑"
        Me.BtnUp.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(165, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Valence du calendrier general :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(376, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(234, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Valence non associee au calendrier general :"
        '
        'BtnAddValence
        '
        Me.BtnAddValence.Location = New System.Drawing.Point(548, 524)
        Me.BtnAddValence.Name = "BtnAddValence"
        Me.BtnAddValence.Size = New System.Drawing.Size(110, 24)
        Me.BtnAddValence.TabIndex = 8
        Me.BtnAddValence.Text = "Ajouter une valence"
        '
        'RadFValenceSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(700, 557)
        Me.Controls.Add(Me.BtnAddValence)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.BtnUp)
        Me.Controls.Add(Me.BtnRemove)
        Me.Controls.Add(Me.BtnAdd)
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
        CType(Me.BtnAddValence, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RGVValenceVisible As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents RGVValenceNotVisible As Telerik.WinControls.UI.RadGridView
    Friend WithEvents BtnAdd As Button
    Friend WithEvents BtnRemove As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents BtnUp As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnAddValence As Telerik.WinControls.UI.RadButton
End Class

