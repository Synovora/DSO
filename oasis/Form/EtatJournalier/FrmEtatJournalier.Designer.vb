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
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn13 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn14 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor2 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
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
        Me.RadGridView.Location = New System.Drawing.Point(24, 57)
        Me.RadGridView.Margin = New System.Windows.Forms.Padding(6)
        '
        '
        '
        Me.RadGridView.MasterTemplate.AllowAddNewRow = False
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.HeaderText = "Nom"
        GridViewTextBoxColumn8.MinWidth = 10
        GridViewTextBoxColumn8.Name = "nom"
        GridViewTextBoxColumn8.Width = 100
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.HeaderText = "Prenom"
        GridViewTextBoxColumn9.MinWidth = 10
        GridViewTextBoxColumn9.Name = "prenom"
        GridViewTextBoxColumn9.Width = 100
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.HeaderText = "DN"
        GridViewTextBoxColumn10.MinWidth = 10
        GridViewTextBoxColumn10.Name = "dn"
        GridViewTextBoxColumn10.Width = 100
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.HeaderText = "INS"
        GridViewTextBoxColumn11.MinWidth = 10
        GridViewTextBoxColumn11.Name = "ins"
        GridViewTextBoxColumn11.Width = 100
        GridViewTextBoxColumn12.EnableExpressionEditor = False
        GridViewTextBoxColumn12.HeaderText = "Type"
        GridViewTextBoxColumn12.MinWidth = 10
        GridViewTextBoxColumn12.Name = "type"
        GridViewTextBoxColumn12.Width = 100
        GridViewTextBoxColumn13.EnableExpressionEditor = False
        GridViewTextBoxColumn13.HeaderText = "site"
        GridViewTextBoxColumn13.MinWidth = 10
        GridViewTextBoxColumn13.Name = "site"
        GridViewTextBoxColumn13.Width = 100
        GridViewTextBoxColumn14.EnableExpressionEditor = False
        GridViewTextBoxColumn14.HeaderText = "Episode Id"
        GridViewTextBoxColumn14.MinWidth = 10
        GridViewTextBoxColumn14.Name = "episodeId"
        GridViewTextBoxColumn14.Width = 120
        Me.RadGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn8, GridViewTextBoxColumn9, GridViewTextBoxColumn10, GridViewTextBoxColumn11, GridViewTextBoxColumn12, GridViewTextBoxColumn13, GridViewTextBoxColumn14})
        SortDescriptor2.Direction = System.ComponentModel.ListSortDirection.Descending
        SortDescriptor2.PropertyName = "column1"
        Me.RadGridView.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor2})
        Me.RadGridView.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridView.Name = "RadGridView"
        Me.RadGridView.ReadOnly = True
        Me.RadGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridView.Size = New System.Drawing.Size(734, 767)
        Me.RadGridView.TabIndex = 0
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

