<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFPPSListeParcours
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
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadChkParcoursTous = New Telerik.WinControls.UI.RadCheckBox()
        Me.RadChkParcoursNonCache = New Telerik.WinControls.UI.RadCheckBox()
        Me.RadBtnCreationParcours = New Telerik.WinControls.UI.RadButton()
        Me.RadParcoursDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAjout = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.RadChkParcoursTous, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadChkParcoursNonCache, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnCreationParcours, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadParcoursDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadParcoursDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAjout, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadChkParcoursTous
        '
        Me.RadChkParcoursTous.Location = New System.Drawing.Point(171, 13)
        Me.RadChkParcoursTous.Name = "RadChkParcoursTous"
        Me.RadChkParcoursTous.Size = New System.Drawing.Size(44, 18)
        Me.RadChkParcoursTous.TabIndex = 39
        Me.RadChkParcoursTous.Text = "Tous"
        '
        'RadChkParcoursNonCache
        '
        Me.RadChkParcoursNonCache.Location = New System.Drawing.Point(69, 13)
        Me.RadChkParcoursNonCache.Name = "RadChkParcoursNonCache"
        Me.RadChkParcoursNonCache.Size = New System.Drawing.Size(96, 18)
        Me.RadChkParcoursNonCache.TabIndex = 37
        Me.RadChkParcoursNonCache.Text = "Non masqué(s)"
        '
        'RadBtnCreationParcours
        '
        Me.RadBtnCreationParcours.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.RadBtnCreationParcours.Location = New System.Drawing.Point(0, 0)
        Me.RadBtnCreationParcours.Name = "RadBtnCreationParcours"
        Me.RadBtnCreationParcours.Size = New System.Drawing.Size(15, 15)
        Me.RadBtnCreationParcours.TabIndex = 41
        Me.RadBtnCreationParcours.Text = "+"
        '
        'RadParcoursDataGridView
        '
        Me.RadParcoursDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadParcoursDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadParcoursDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadParcoursDataGridView.ForeColor = System.Drawing.Color.Black
        Me.RadParcoursDataGridView.HideSelection = True
        Me.RadParcoursDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadParcoursDataGridView.Location = New System.Drawing.Point(8, 37)
        '
        '
        '
        Me.RadParcoursDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadParcoursDataGridView.MasterTemplate.AllowCellContextMenu = False
        Me.RadParcoursDataGridView.MasterTemplate.AllowColumnHeaderContextMenu = False
        Me.RadParcoursDataGridView.MasterTemplate.AllowDeleteRow = False
        Me.RadParcoursDataGridView.MasterTemplate.AllowDragToGroup = False
        Me.RadParcoursDataGridView.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "specialite"
        GridViewTextBoxColumn1.HeaderText = "Spécialité"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.Name = "specialite"
        GridViewTextBoxColumn1.ReadOnly = True
        GridViewTextBoxColumn1.Width = 120
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "nomIntervenant"
        GridViewTextBoxColumn2.HeaderText = "Nom"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.Name = "nomIntervenant"
        GridViewTextBoxColumn2.Width = 180
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "nomStructure"
        GridViewTextBoxColumn3.HeaderText = "Structure"
        GridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn3.Name = "nomStructure"
        GridViewTextBoxColumn3.Width = 180
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "consultationLast"
        GridViewTextBoxColumn4.HeaderText = "Dern. consult."
        GridViewTextBoxColumn4.IsVisible = False
        GridViewTextBoxColumn4.Name = "consultationLast"
        GridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn4.Width = 80
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.FieldName = "consultationNext"
        GridViewTextBoxColumn5.HeaderText = "Proch. consult."
        GridViewTextBoxColumn5.IsVisible = False
        GridViewTextBoxColumn5.Name = "consultationNext"
        GridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn5.Width = 80
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.FieldName = "commentaire"
        GridViewTextBoxColumn6.HeaderText = "Remarque"
        GridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn6.IsVisible = False
        GridViewTextBoxColumn6.Name = "commentaire"
        GridViewTextBoxColumn6.Width = 340
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.FieldName = "parcoursId"
        GridViewTextBoxColumn7.HeaderText = "Id"
        GridViewTextBoxColumn7.IsVisible = False
        GridViewTextBoxColumn7.Name = "parcoursId"
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.HeaderText = "HeureRDV"
        GridViewTextBoxColumn8.IsVisible = False
        GridViewTextBoxColumn8.Name = "ConsultationNextHeure"
        Me.RadParcoursDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8})
        Me.RadParcoursDataGridView.MasterTemplate.ShowFilteringRow = False
        Me.RadParcoursDataGridView.MasterTemplate.ShowRowHeaderColumn = False
        Me.RadParcoursDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadParcoursDataGridView.Name = "RadParcoursDataGridView"
        Me.RadParcoursDataGridView.ReadOnly = True
        Me.RadParcoursDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadParcoursDataGridView.ShowGroupPanel = False
        Me.RadParcoursDataGridView.Size = New System.Drawing.Size(503, 246)
        Me.RadParcoursDataGridView.TabIndex = 0
        '
        'RadBtnAjout
        '
        Me.RadBtnAjout.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.RadBtnAjout.Location = New System.Drawing.Point(12, 16)
        Me.RadBtnAjout.Name = "RadBtnAjout"
        Me.RadBtnAjout.Size = New System.Drawing.Size(15, 15)
        Me.RadBtnAjout.TabIndex = 42
        Me.RadBtnAjout.Text = "+"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(487, 289)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 43
        '
        'RadFPPSListeParcours
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(519, 320)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadBtnAjout)
        Me.Controls.Add(Me.RadParcoursDataGridView)
        Me.Controls.Add(Me.RadChkParcoursTous)
        Me.Controls.Add(Me.RadChkParcoursNonCache)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFPPSListeParcours"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Liste des intervenants de soin"
        CType(Me.RadChkParcoursTous, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadChkParcoursNonCache, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnCreationParcours, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadParcoursDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadParcoursDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAjout, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadChkParcoursTous As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents RadChkParcoursNonCache As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents RadBtnCreationParcours As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadParcoursDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAjout As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
End Class

