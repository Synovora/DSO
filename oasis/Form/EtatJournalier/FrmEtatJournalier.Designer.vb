<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmEtatJournalier
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
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor1 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridView = New Telerik.WinControls.UI.RadGridView()
        Me.RadDateTimePicker = New Telerik.WinControls.UI.RadDateTimePicker()
        Me.RadButton = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadDateTimePicker, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGridView
        '
        Me.RadGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridView.ForeColor = System.Drawing.Color.Black
        Me.RadGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridView.Location = New System.Drawing.Point(24, 58)
        Me.RadGridView.Margin = New System.Windows.Forms.Padding(6144)
        '
        '
        '
        Me.RadGridView.MasterTemplate.AllowAddNewRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "Nom"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.BottomCenter
        GridViewTextBoxColumn1.MinWidth = 400
        GridViewTextBoxColumn1.Name = "nom"
        GridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending
        GridViewTextBoxColumn1.Width = 400
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "Prenom"
        GridViewTextBoxColumn2.MinWidth = 400
        GridViewTextBoxColumn2.Name = "prenom"
        GridViewTextBoxColumn2.Width = 400
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "DN"
        GridViewTextBoxColumn3.MinWidth = 400
        GridViewTextBoxColumn3.Name = "dn"
        GridViewTextBoxColumn3.Width = 400
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "NIR"
        GridViewTextBoxColumn4.MinWidth = 400
        GridViewTextBoxColumn4.Name = "nir"
        GridViewTextBoxColumn4.Width = 400
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.HeaderText = "Type"
        GridViewTextBoxColumn5.MinWidth = 400
        GridViewTextBoxColumn5.Name = "type"
        GridViewTextBoxColumn5.Width = 400
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.HeaderText = "site"
        GridViewTextBoxColumn6.MinWidth = 400
        GridViewTextBoxColumn6.Name = "site"
        GridViewTextBoxColumn6.Width = 400
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.HeaderText = "Episode Id"
        GridViewTextBoxColumn7.MinWidth = 400
        GridViewTextBoxColumn7.Name = "episodeId"
        GridViewTextBoxColumn7.Width = 400
        Me.RadGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7})
        SortDescriptor1.PropertyName = "nom"
        Me.RadGridView.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor1})
        Me.RadGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridView.Name = "RadGridView"
        Me.RadGridView.ReadOnly = True
        Me.RadGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridView.Size = New System.Drawing.Size(734, 766)
        Me.RadGridView.TabIndex = 0
        Me.RadGridView.ThemeName = "ControlDefault"
        '
        'RadDateTimePicker
        '
        Me.RadDateTimePicker.Location = New System.Drawing.Point(24, 13)
        Me.RadDateTimePicker.Name = "RadDateTimePicker"
        Me.RadDateTimePicker.Size = New System.Drawing.Size(328, 35)
        Me.RadDateTimePicker.TabIndex = 1
        Me.RadDateTimePicker.TabStop = False
        Me.RadDateTimePicker.Text = "lundi 12 octobre 2020"
        Me.RadDateTimePicker.Value = New Date(2020, 10, 12, 23, 30, 11, 710)
        '
        'RadButton
        '
        Me.RadButton.Location = New System.Drawing.Point(616, 16)
        Me.RadButton.Name = "RadButton"
        Me.RadButton.Size = New System.Drawing.Size(141, 31)
        Me.RadButton.TabIndex = 2
        Me.RadButton.Text = "Envoi CPS"
        '
        'FrmEtatJournalier
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(783, 851)
        Me.Controls.Add(Me.RadButton)
        Me.Controls.Add(Me.RadDateTimePicker)
        Me.Controls.Add(Me.RadGridView)
        Me.Name = "FrmEtatJournalier"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "Episode"
        CType(Me.RadGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadDateTimePicker, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadDateTimePicker As Telerik.WinControls.UI.RadDateTimePicker
    Friend WithEvents RadButton As Telerik.WinControls.UI.RadButton
End Class

