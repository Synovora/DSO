<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFVaccinInput
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
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.GVVaccin = New Telerik.WinControls.UI.RadGridView()
        Me.BtnValidation = New Telerik.WinControls.UI.RadButton()
        Me.BtnCancel = New Telerik.WinControls.UI.RadButton()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.DTPRealisation = New System.Windows.Forms.DateTimePicker()
        Me.BtnSelectOperator = New Telerik.WinControls.UI.RadButton()
        Me.TextOperator = New Telerik.WinControls.UI.RadTextBox()
        CType(Me.GVVaccin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVVaccin.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnSelectOperator, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextOperator, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GVVaccin
        '
        Me.GVVaccin.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GVVaccin.Cursor = System.Windows.Forms.Cursors.Default
        Me.GVVaccin.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.GVVaccin.ForeColor = System.Drawing.Color.Black
        Me.GVVaccin.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.GVVaccin.Location = New System.Drawing.Point(12, 38)
        '
        '
        '
        Me.GVVaccin.MasterTemplate.AllowAddNewRow = False
        Me.GVVaccin.MasterTemplate.AllowDragToGroup = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "id"
        GridViewTextBoxColumn2.AllowGroup = False
        GridViewTextBoxColumn2.AllowResize = False
        GridViewTextBoxColumn2.AllowSort = False
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "Nom"
        GridViewTextBoxColumn2.MinWidth = 200
        GridViewTextBoxColumn2.Name = "dci"
        GridViewTextBoxColumn2.ReadOnly = True
        GridViewTextBoxColumn2.VisibleInColumnChooser = False
        GridViewTextBoxColumn2.Width = 300
        GridViewTextBoxColumn3.AllowGroup = False
        GridViewTextBoxColumn3.AllowResize = False
        GridViewTextBoxColumn3.AllowSort = False
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "Lot"
        GridViewTextBoxColumn3.Name = "lot"
        GridViewTextBoxColumn3.ReadOnly = True
        GridViewTextBoxColumn3.VisibleInColumnChooser = False
        GridViewTextBoxColumn3.Width = 80
        GridViewTextBoxColumn4.AllowGroup = False
        GridViewTextBoxColumn4.AllowResize = False
        GridViewTextBoxColumn4.AllowSort = False
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "Exp"
        GridViewTextBoxColumn4.MinWidth = 80
        GridViewTextBoxColumn4.Name = "expiration"
        GridViewTextBoxColumn4.ReadOnly = True
        GridViewTextBoxColumn4.VisibleInColumnChooser = False
        GridViewTextBoxColumn4.Width = 80
        GridViewTextBoxColumn5.AllowGroup = False
        GridViewTextBoxColumn5.AllowResize = False
        GridViewTextBoxColumn5.AllowSort = False
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.HeaderText = "Réalisation"
        GridViewTextBoxColumn5.Name = "realisation"
        GridViewTextBoxColumn5.ReadOnly = True
        GridViewTextBoxColumn5.VisibleInColumnChooser = False
        GridViewTextBoxColumn5.Width = 160
        GridViewTextBoxColumn6.AllowGroup = False
        GridViewTextBoxColumn6.AllowResize = False
        GridViewTextBoxColumn6.AllowSort = False
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.HeaderText = "Commentaire"
        GridViewTextBoxColumn6.Name = "comment"
        GridViewTextBoxColumn6.ReadOnly = True
        GridViewTextBoxColumn6.VisibleInColumnChooser = False
        GridViewTextBoxColumn6.Width = 300
        GridViewTextBoxColumn7.AllowGroup = False
        GridViewTextBoxColumn7.AllowResize = False
        GridViewTextBoxColumn7.AllowSort = False
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.HeaderText = "column1"
        GridViewTextBoxColumn7.IsVisible = False
        GridViewTextBoxColumn7.Name = "relation_id"
        GridViewTextBoxColumn7.ReadOnly = True
        GridViewTextBoxColumn7.VisibleInColumnChooser = False
        GridViewTextBoxColumn8.AllowGroup = False
        GridViewTextBoxColumn8.AllowResize = False
        GridViewTextBoxColumn8.AllowSort = False
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.HeaderText = "column1"
        GridViewTextBoxColumn8.IsVisible = False
        GridViewTextBoxColumn8.Name = "program_id"
        GridViewTextBoxColumn8.ReadOnly = True
        GridViewTextBoxColumn8.VisibleInColumnChooser = False
        Me.GVVaccin.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8})
        Me.GVVaccin.MasterTemplate.ShowRowHeaderColumn = False
        Me.GVVaccin.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.GVVaccin.Name = "GVVaccin"
        Me.GVVaccin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GVVaccin.ShowCellErrors = False
        Me.GVVaccin.ShowGroupPanel = False
        Me.GVVaccin.ShowRowErrors = False
        Me.GVVaccin.Size = New System.Drawing.Size(948, 325)
        Me.GVVaccin.TabIndex = 0
        '
        'BtnValidation
        '
        Me.BtnValidation.Enabled = False
        Me.BtnValidation.Location = New System.Drawing.Point(851, 369)
        Me.BtnValidation.Name = "BtnValidation"
        Me.BtnValidation.Size = New System.Drawing.Size(110, 24)
        Me.BtnValidation.TabIndex = 1
        Me.BtnValidation.Text = "Valider"
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(735, 369)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(110, 24)
        Me.BtnCancel.TabIndex = 2
        Me.BtnCancel.Text = "Annuler"
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(380, 12)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(60, 18)
        Me.RadLabel2.TabIndex = 9
        Me.RadLabel2.Text = "Operateur:"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(12, 12)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(114, 18)
        Me.RadLabel1.TabIndex = 8
        Me.RadLabel1.Text = "Date de la realisation:"
        '
        'DTPRealisation
        '
        Me.DTPRealisation.Location = New System.Drawing.Point(132, 12)
        Me.DTPRealisation.Name = "DTPRealisation"
        Me.DTPRealisation.Size = New System.Drawing.Size(227, 26)
        Me.DTPRealisation.TabIndex = 6
        '
        'BtnSelectOperator
        '
        Me.BtnSelectOperator.Location = New System.Drawing.Point(801, 9)
        Me.BtnSelectOperator.Name = "BtnSelectOperator"
        Me.BtnSelectOperator.Size = New System.Drawing.Size(159, 24)
        Me.BtnSelectOperator.TabIndex = 10
        Me.BtnSelectOperator.Text = "Selectionner un operateur"
        '
        'TextOperator
        '
        Me.TextOperator.Location = New System.Drawing.Point(447, 12)
        Me.TextOperator.Name = "TextOperator"
        Me.TextOperator.ReadOnly = True
        Me.TextOperator.Size = New System.Drawing.Size(348, 27)
        Me.TextOperator.TabIndex = 11
        '
        'RadFVaccinInput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(972, 401)
        Me.Controls.Add(Me.TextOperator)
        Me.Controls.Add(Me.BtnSelectOperator)
        Me.Controls.Add(Me.RadLabel2)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.BtnValidation)
        Me.Controls.Add(Me.DTPRealisation)
        Me.Controls.Add(Me.GVVaccin)
        Me.Name = "RadFVaccinInput"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFVaccinInput"
        CType(Me.GVVaccin.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVVaccin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnSelectOperator, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextOperator, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GVVaccin As Telerik.WinControls.UI.RadGridView
    Friend WithEvents BtnValidation As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnCancel As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents DTPRealisation As DateTimePicker
    Friend WithEvents BtnSelectOperator As Telerik.WinControls.UI.RadButton
    Friend WithEvents TextOperator As Telerik.WinControls.UI.RadTextBox
End Class

