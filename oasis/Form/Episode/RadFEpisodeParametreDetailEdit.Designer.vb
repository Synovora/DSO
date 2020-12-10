<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFEpisodeParametreDetailEdit
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
        Dim GridViewMaskBoxColumn1 As Telerik.WinControls.UI.GridViewMaskBoxColumn = New Telerik.WinControls.UI.GridViewMaskBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.RadBtnAjouter = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSupprimer = New Telerik.WinControls.UI.RadButton()
        Me.RadGridViewParm = New Telerik.WinControls.UI.RadGridView()
        Me.RadDesktopAlert1 = New Telerik.WinControls.UI.RadDesktopAlert(Me.components)
        Me.RadBtnSuppprimer = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSelect = New Telerik.WinControls.UI.RadButton()
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnCacher = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAjouter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSupprimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSuppprimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnCacher, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(447, 536)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 0
        '
        'RadBtnAjouter
        '
        Me.RadBtnAjouter.ForeColor = System.Drawing.Color.Black
        Me.RadBtnAjouter.Location = New System.Drawing.Point(12, 4)
        Me.RadBtnAjouter.Name = "RadBtnAjouter"
        Me.RadBtnAjouter.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAjouter.TabIndex = 97
        Me.RadBtnAjouter.TabStop = False
        Me.RadBtnAjouter.Text = "+"
        Me.ToolTip.SetToolTip(Me.RadBtnAjouter, "Ajouter des paramètres")
        '
        'RadBtnSupprimer
        '
        Me.RadBtnSupprimer.Image = Global.Oasis_WF.My.Resources.Resources.supprimer1
        Me.RadBtnSupprimer.Location = New System.Drawing.Point(42, 4)
        Me.RadBtnSupprimer.Name = "RadBtnSupprimer"
        Me.RadBtnSupprimer.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnSupprimer.TabIndex = 97
        Me.RadBtnSupprimer.Text = "Supprimer"
        Me.ToolTip.SetToolTip(Me.RadBtnSupprimer, "Supprimer")
        '
        'RadGridViewParm
        '
        Me.RadGridViewParm.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewParm.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewParm.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewParm.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewParm.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewParm.Location = New System.Drawing.Point(12, 34)
        '
        '
        '
        Me.RadGridViewParm.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewParm.MasterTemplate.AllowDeleteRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "episode_parametre_id"
        GridViewTextBoxColumn1.HeaderText = "episode_parametre_id"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "episode_parametre_id"
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "parametre_id"
        GridViewTextBoxColumn2.HeaderText = "parametre_id"
        GridViewTextBoxColumn2.IsVisible = False
        GridViewTextBoxColumn2.Name = "parametre_id"
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "description"
        GridViewTextBoxColumn3.HeaderText = "description"
        GridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn3.Name = "description"
        GridViewTextBoxColumn3.ReadOnly = True
        GridViewTextBoxColumn3.Width = 250
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "entier"
        GridViewTextBoxColumn4.HeaderText = "entier"
        GridViewTextBoxColumn4.IsVisible = False
        GridViewTextBoxColumn4.Name = "entier"
        GridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.FieldName = "decimal"
        GridViewTextBoxColumn5.HeaderText = "decimal"
        GridViewTextBoxColumn5.IsVisible = False
        GridViewTextBoxColumn5.Name = "decimal"
        GridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewMaskBoxColumn1.DataType = GetType(Decimal)
        GridViewMaskBoxColumn1.EnableExpressionEditor = False
        GridViewMaskBoxColumn1.FieldName = "valeurInput"
        GridViewMaskBoxColumn1.FormatInfo = New System.Globalization.CultureInfo("fr-FR")
        GridViewMaskBoxColumn1.HeaderText = "valeur"
        GridViewMaskBoxColumn1.Mask = "G"
        GridViewMaskBoxColumn1.MaskType = Telerik.WinControls.UI.MaskType.Numeric
        GridViewMaskBoxColumn1.Name = "valeurInput"
        GridViewMaskBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewMaskBoxColumn1.Width = 70
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.FieldName = "unite"
        GridViewTextBoxColumn6.HeaderText = "Unité"
        GridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn6.Name = "unite"
        GridViewTextBoxColumn6.ReadOnly = True
        GridViewTextBoxColumn6.Width = 80
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.FieldName = "valeur"
        GridViewTextBoxColumn7.HeaderText = "valeur"
        GridViewTextBoxColumn7.IsVisible = False
        GridViewTextBoxColumn7.Name = "valeur"
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.FieldName = "parametre_ajoute"
        GridViewTextBoxColumn8.IsVisible = False
        GridViewTextBoxColumn8.Name = "parametre_ajoute"
        GridViewTextBoxColumn8.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn8.Width = 20
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.FieldName = "ajoute"
        GridViewTextBoxColumn9.HeaderText = "+"
        GridViewTextBoxColumn9.Name = "ajoute"
        GridViewTextBoxColumn9.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn9.Width = 15
        Me.RadGridViewParm.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewMaskBoxColumn1, GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8, GridViewTextBoxColumn9})
        Me.RadGridViewParm.MasterTemplate.ShowFilteringRow = False
        Me.RadGridViewParm.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewParm.Name = "RadGridViewParm"
        Me.RadGridViewParm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParm.ShowGroupPanel = False
        Me.RadGridViewParm.Size = New System.Drawing.Size(459, 496)
        Me.RadGridViewParm.TabIndex = 5
        '
        'RadDesktopAlert1
        '
        Me.RadDesktopAlert1.AutoCloseDelay = 5
        Me.RadDesktopAlert1.FadeAnimationSpeed = 5
        Me.RadDesktopAlert1.Opacity = 0.9!
        Me.RadDesktopAlert1.ScreenPosition = Telerik.WinControls.UI.AlertScreenPosition.TopCenter
        Me.RadDesktopAlert1.ThemeName = ""
        '
        'RadBtnSuppprimer
        '
        Me.RadBtnSuppprimer.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadBtnSuppprimer.Location = New System.Drawing.Point(495, 255)
        Me.RadBtnSuppprimer.Name = "RadBtnSuppprimer"
        Me.RadBtnSuppprimer.Size = New System.Drawing.Size(81, 24)
        Me.RadBtnSuppprimer.TabIndex = 99
        Me.RadBtnSuppprimer.Text = "Enlever >>>"
        '
        'RadBtnSelect
        '
        Me.RadBtnSelect.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadBtnSelect.Location = New System.Drawing.Point(495, 225)
        Me.RadBtnSelect.Name = "RadBtnSelect"
        Me.RadBtnSelect.Size = New System.Drawing.Size(81, 24)
        Me.RadBtnSelect.TabIndex = 98
        Me.RadBtnSelect.Text = "<<< Ajouter"
        '
        'RadGridView1
        '
        Me.RadGridView1.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridView1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridView1.ForeColor = System.Drawing.Color.Black
        Me.RadGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridView1.Location = New System.Drawing.Point(595, 34)
        '
        '
        '
        Me.RadGridView1.MasterTemplate.AllowAddNewRow = False
        Me.RadGridView1.MasterTemplate.AllowDeleteRow = False
        Me.RadGridView1.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.FieldName = "id"
        GridViewTextBoxColumn10.HeaderText = "id"
        GridViewTextBoxColumn10.IsVisible = False
        GridViewTextBoxColumn10.Name = "id"
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.FieldName = "description"
        GridViewTextBoxColumn11.HeaderText = "Description"
        GridViewTextBoxColumn11.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn11.Name = "description"
        GridViewTextBoxColumn11.Width = 200
        GridViewTextBoxColumn12.EnableExpressionEditor = False
        GridViewTextBoxColumn12.FieldName = "unite"
        GridViewTextBoxColumn12.HeaderText = "Unité"
        GridViewTextBoxColumn12.Name = "unite"
        GridViewTextBoxColumn12.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn12.Width = 65
        Me.RadGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn10, GridViewTextBoxColumn11, GridViewTextBoxColumn12})
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridView1.ShowGroupPanel = False
        Me.RadGridView1.Size = New System.Drawing.Size(309, 496)
        Me.RadGridView1.TabIndex = 100
        '
        'RadBtnCacher
        '
        Me.RadBtnCacher.Location = New System.Drawing.Point(701, 536)
        Me.RadBtnCacher.Name = "RadBtnCacher"
        Me.RadBtnCacher.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnCacher.TabIndex = 101
        Me.RadBtnCacher.Text = "Cacher"
        '
        'RadFEpisodeParametreDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(915, 567)
        Me.Controls.Add(Me.RadBtnCacher)
        Me.Controls.Add(Me.RadGridView1)
        Me.Controls.Add(Me.RadBtnSuppprimer)
        Me.Controls.Add(Me.RadBtnSelect)
        Me.Controls.Add(Me.RadBtnSupprimer)
        Me.Controls.Add(Me.RadBtnAjouter)
        Me.Controls.Add(Me.RadGridViewParm)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.MinimizeBox = False
        Me.Name = "RadFEpisodeParametreDetailEdit"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFParametreDetailEdit"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAjouter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSupprimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSuppprimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnCacher, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents RadGridViewParm As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAjouter As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadDesktopAlert1 As Telerik.WinControls.UI.RadDesktopAlert
    Friend WithEvents RadBtnSupprimer As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSuppprimer As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSelect As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGridView1 As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnCacher As Telerik.WinControls.UI.RadButton
End Class

