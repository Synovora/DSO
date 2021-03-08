<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFLigneDeVieParametreSelecteur
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
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewParmDispo = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnSuppprimer = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSelect = New Telerik.WinControls.UI.RadButton()
        Me.RadGridViewParm = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGridViewParmDispo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParmDispo.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSuppprimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGridViewParmDispo
        '
        Me.RadGridViewParmDispo.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewParmDispo.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewParmDispo.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewParmDispo.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewParmDispo.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewParmDispo.Location = New System.Drawing.Point(433, 13)
        '
        '
        '
        Me.RadGridViewParmDispo.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewParmDispo.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewParmDispo.MasterTemplate.AllowEditRow = False
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
        Me.RadGridViewParmDispo.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3})
        Me.RadGridViewParmDispo.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewParmDispo.Name = "RadGridViewParmDispo"
        Me.RadGridViewParmDispo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParmDispo.ShowGroupPanel = False
        Me.RadGridViewParmDispo.Size = New System.Drawing.Size(309, 518)
        Me.RadGridViewParmDispo.TabIndex = 108
        '
        'RadBtnSuppprimer
        '
        Me.RadBtnSuppprimer.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadBtnSuppprimer.Location = New System.Drawing.Point(333, 256)
        Me.RadBtnSuppprimer.Name = "RadBtnSuppprimer"
        Me.RadBtnSuppprimer.Size = New System.Drawing.Size(81, 24)
        Me.RadBtnSuppprimer.TabIndex = 107
        Me.RadBtnSuppprimer.Text = "Enlever >>>"
        '
        'RadBtnSelect
        '
        Me.RadBtnSelect.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadBtnSelect.Location = New System.Drawing.Point(333, 226)
        Me.RadBtnSelect.Name = "RadBtnSelect"
        Me.RadBtnSelect.Size = New System.Drawing.Size(81, 24)
        Me.RadBtnSelect.TabIndex = 106
        Me.RadBtnSelect.Text = "<<< Ajouter"
        '
        'RadGridViewParm
        '
        Me.RadGridViewParm.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewParm.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewParm.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewParm.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewParm.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewParm.Location = New System.Drawing.Point(5, 13)
        '
        '
        '
        Me.RadGridViewParm.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewParm.MasterTemplate.AllowDeleteRow = False
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "parametre_id"
        GridViewTextBoxColumn4.HeaderText = "parametre_id"
        GridViewTextBoxColumn4.IsVisible = False
        GridViewTextBoxColumn4.Name = "id"
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.FieldName = "description"
        GridViewTextBoxColumn5.HeaderText = "description"
        GridViewTextBoxColumn5.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn5.Name = "description"
        GridViewTextBoxColumn5.ReadOnly = True
        GridViewTextBoxColumn5.Width = 200
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.FieldName = "unite"
        GridViewTextBoxColumn6.HeaderText = "Unité"
        GridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn6.Name = "unite"
        GridViewTextBoxColumn6.ReadOnly = True
        GridViewTextBoxColumn6.Width = 65
        Me.RadGridViewParm.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6})
        Me.RadGridViewParm.MasterTemplate.ShowFilteringRow = False
        Me.RadGridViewParm.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridViewParm.Name = "RadGridViewParm"
        Me.RadGridViewParm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParm.ShowGroupPanel = False
        Me.RadGridViewParm.Size = New System.Drawing.Size(307, 518)
        Me.RadGridViewParm.TabIndex = 103
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(718, 537)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 102
        '
        'RadFLigneDeVieParametreSelecteur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(748, 567)
        Me.Controls.Add(Me.RadGridViewParmDispo)
        Me.Controls.Add(Me.RadBtnSuppprimer)
        Me.Controls.Add(Me.RadBtnSelect)
        Me.Controls.Add(Me.RadGridViewParm)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFLigneDeVieParametreSelecteur"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Sélecteur paramètres"
        CType(Me.RadGridViewParmDispo.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParmDispo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSuppprimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGridViewParmDispo As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnSuppprimer As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSelect As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGridViewParm As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
End Class

