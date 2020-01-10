<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFMedocDetail
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
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblMedicamentTitulaire = New System.Windows.Forms.Label()
        Me.LblMedicamentAdministration = New System.Windows.Forms.Label()
        Me.LblMedicamentForme = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblMedicamentCIS = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.LblMedicamentDCI = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.RadmedicamentCompoDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.RadmedicamentPresentationDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadmedicamentCompoDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadmedicamentCompoDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadmedicamentPresentationDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadmedicamentPresentationDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBox1.Controls.Add(Me.LblMedicamentTitulaire)
        Me.RadGroupBox1.Controls.Add(Me.LblMedicamentAdministration)
        Me.RadGroupBox1.Controls.Add(Me.LblMedicamentForme)
        Me.RadGroupBox1.Controls.Add(Me.Label8)
        Me.RadGroupBox1.Controls.Add(Me.Label7)
        Me.RadGroupBox1.Controls.Add(Me.Label5)
        Me.RadGroupBox1.Controls.Add(Me.LblMedicamentCIS)
        Me.RadGroupBox1.Controls.Add(Me.Label10)
        Me.RadGroupBox1.Controls.Add(Me.LblMedicamentDCI)
        Me.RadGroupBox1.Controls.Add(Me.Label6)
        Me.RadGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBox1.HeaderText = "Médicament"
        Me.RadGroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(920, 161)
        Me.RadGroupBox1.TabIndex = 0
        Me.RadGroupBox1.Text = "Médicament"
        '
        'LblMedicamentTitulaire
        '
        Me.LblMedicamentTitulaire.AutoSize = True
        Me.LblMedicamentTitulaire.Location = New System.Drawing.Point(180, 69)
        Me.LblMedicamentTitulaire.Name = "LblMedicamentTitulaire"
        Me.LblMedicamentTitulaire.Size = New System.Drawing.Size(164, 13)
        Me.LblMedicamentTitulaire.TabIndex = 61
        Me.LblMedicamentTitulaire.Text = "ACCORD HEALTHCARE FRANCE"
        '
        'LblMedicamentAdministration
        '
        Me.LblMedicamentAdministration.AutoSize = True
        Me.LblMedicamentAdministration.Location = New System.Drawing.Point(180, 131)
        Me.LblMedicamentAdministration.Name = "LblMedicamentAdministration"
        Me.LblMedicamentAdministration.Size = New System.Drawing.Size(33, 13)
        Me.LblMedicamentAdministration.TabIndex = 60
        Me.LblMedicamentAdministration.Text = "orale"
        '
        'LblMedicamentForme
        '
        Me.LblMedicamentForme.AutoSize = True
        Me.LblMedicamentForme.Location = New System.Drawing.Point(180, 101)
        Me.LblMedicamentForme.Name = "LblMedicamentForme"
        Me.LblMedicamentForme.Size = New System.Drawing.Size(103, 13)
        Me.LblMedicamentForme.TabIndex = 59
        Me.LblMedicamentForme.Text = "comprime pellicule"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(5, 69)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 13)
        Me.Label8.TabIndex = 58
        Me.Label8.Text = "Titulaire :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(5, 131)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(94, 13)
        Me.Label7.TabIndex = 57
        Me.Label7.Text = "Administration :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(5, 101)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 56
        Me.Label5.Text = "Forme :"
        '
        'LblMedicamentCIS
        '
        Me.LblMedicamentCIS.AutoSize = True
        Me.LblMedicamentCIS.Location = New System.Drawing.Point(180, 18)
        Me.LblMedicamentCIS.Name = "LblMedicamentCIS"
        Me.LblMedicamentCIS.Size = New System.Drawing.Size(55, 13)
        Me.LblMedicamentCIS.TabIndex = 55
        Me.LblMedicamentCIS.Text = "60002283"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(5, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(35, 13)
        Me.Label10.TabIndex = 54
        Me.Label10.Text = "CIS :"
        '
        'LblMedicamentDCI
        '
        Me.LblMedicamentDCI.AutoSize = True
        Me.LblMedicamentDCI.Location = New System.Drawing.Point(180, 44)
        Me.LblMedicamentDCI.Name = "LblMedicamentDCI"
        Me.LblMedicamentDCI.Size = New System.Drawing.Size(260, 13)
        Me.LblMedicamentDCI.TabIndex = 53
        Me.LblMedicamentDCI.Text = "ANASTROZOLE ACCORD 1 mg, comprime pellicule"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(5, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(166, 13)
        Me.Label6.TabIndex = 52
        Me.Label6.Text = "Dénomination commerciale :"
        '
        'RadmedicamentCompoDataGridView
        '
        Me.RadmedicamentCompoDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadmedicamentCompoDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadmedicamentCompoDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadmedicamentCompoDataGridView.ForeColor = System.Drawing.Color.Black
        Me.RadmedicamentCompoDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadmedicamentCompoDataGridView.Location = New System.Drawing.Point(12, 179)
        '
        '
        '
        Me.RadmedicamentCompoDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadmedicamentCompoDataGridView.MasterTemplate.AllowCellContextMenu = False
        Me.RadmedicamentCompoDataGridView.MasterTemplate.AllowColumnReorder = False
        Me.RadmedicamentCompoDataGridView.MasterTemplate.AllowDeleteRow = False
        Me.RadmedicamentCompoDataGridView.MasterTemplate.AllowDragToGroup = False
        Me.RadmedicamentCompoDataGridView.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "denomination"
        GridViewTextBoxColumn1.HeaderText = "Substance"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.Name = "denomination"
        GridViewTextBoxColumn1.Width = 300
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "dosage"
        GridViewTextBoxColumn2.HeaderText = "Dosage"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.Name = "dosage"
        GridViewTextBoxColumn2.ReadOnly = True
        GridViewTextBoxColumn2.Width = 200
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "referenceDosage"
        GridViewTextBoxColumn3.HeaderText = "Référence dosage"
        GridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn3.Name = "referenceDosage"
        GridViewTextBoxColumn3.ReadOnly = True
        GridViewTextBoxColumn3.Width = 200
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "nature"
        GridViewTextBoxColumn4.HeaderText = "Nature"
        GridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn4.Name = "nature"
        GridViewTextBoxColumn4.Width = 200
        Me.RadmedicamentCompoDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4})
        Me.RadmedicamentCompoDataGridView.MasterTemplate.ShowRowHeaderColumn = False
        Me.RadmedicamentCompoDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadmedicamentCompoDataGridView.Name = "RadmedicamentCompoDataGridView"
        Me.RadmedicamentCompoDataGridView.ReadOnly = True
        Me.RadmedicamentCompoDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadmedicamentCompoDataGridView.ShowGroupPanel = False
        Me.RadmedicamentCompoDataGridView.Size = New System.Drawing.Size(920, 153)
        Me.RadmedicamentCompoDataGridView.TabIndex = 1
        '
        'RadmedicamentPresentationDataGridView
        '
        Me.RadmedicamentPresentationDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadmedicamentPresentationDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadmedicamentPresentationDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadmedicamentPresentationDataGridView.ForeColor = System.Drawing.Color.Black
        Me.RadmedicamentPresentationDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadmedicamentPresentationDataGridView.Location = New System.Drawing.Point(12, 338)
        '
        '
        '
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.AllowCellContextMenu = False
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.AllowColumnReorder = False
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.AllowDeleteRow = False
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.AllowDragToGroup = False
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.FieldName = "CIP7"
        GridViewTextBoxColumn5.HeaderText = "CIP7"
        GridViewTextBoxColumn5.Name = "CIP7"
        GridViewTextBoxColumn5.ReadOnly = True
        GridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn5.Width = 100
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.FieldName = "presentation"
        GridViewTextBoxColumn6.HeaderText = "Présentation"
        GridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn6.Name = "presentation"
        GridViewTextBoxColumn6.ReadOnly = True
        GridViewTextBoxColumn6.Width = 600
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn5, GridViewTextBoxColumn6})
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.ShowRowHeaderColumn = False
        Me.RadmedicamentPresentationDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadmedicamentPresentationDataGridView.Name = "RadmedicamentPresentationDataGridView"
        Me.RadmedicamentPresentationDataGridView.ReadOnly = True
        Me.RadmedicamentPresentationDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadmedicamentPresentationDataGridView.ShowGroupPanel = False
        Me.RadmedicamentPresentationDataGridView.Size = New System.Drawing.Size(920, 103)
        Me.RadmedicamentPresentationDataGridView.TabIndex = 2
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(822, 447)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandonner.TabIndex = 3
        Me.RadBtnAbandonner.Text = "Abandonner"
        '
        'RadFMedocDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandonner
        Me.ClientSize = New System.Drawing.Size(944, 477)
        Me.Controls.Add(Me.RadBtnAbandonner)
        Me.Controls.Add(Me.RadmedicamentPresentationDataGridView)
        Me.Controls.Add(Me.RadmedicamentCompoDataGridView)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFMedocDetail"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RadFMedocDetail"
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        Me.RadGroupBox1.PerformLayout()
        CType(Me.RadmedicamentCompoDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadmedicamentCompoDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadmedicamentPresentationDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadmedicamentPresentationDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblMedicamentTitulaire As Label
    Friend WithEvents LblMedicamentAdministration As Label
    Friend WithEvents LblMedicamentForme As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LblMedicamentCIS As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents LblMedicamentDCI As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents RadmedicamentCompoDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadmedicamentPresentationDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
End Class

