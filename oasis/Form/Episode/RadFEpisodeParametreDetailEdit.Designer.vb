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
        Dim GridViewTextBoxColumn13 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn14 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn15 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn16 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn17 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewMaskBoxColumn2 As Telerik.WinControls.UI.GridViewMaskBoxColumn = New Telerik.WinControls.UI.GridViewMaskBoxColumn()
        Dim GridViewTextBoxColumn18 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn19 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn20 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn21 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition3 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn22 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn23 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn24 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition4 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.RadGridViewParm = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAjouter = New Telerik.WinControls.UI.RadButton()
        Me.RadDesktopAlert1 = New Telerik.WinControls.UI.RadDesktopAlert(Me.components)
        Me.RadBtnSupprimer = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSuppprimer = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSelect = New Telerik.WinControls.UI.RadButton()
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnCacher = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAjouter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSupprimer, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'RadGridViewParm
        '
        Me.RadGridViewParm.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewParm.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewParm.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewParm.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewParm.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewParm.Location = New System.Drawing.Point(12, 12)
        '
        '
        '
        Me.RadGridViewParm.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewParm.MasterTemplate.AllowDeleteRow = False
        GridViewTextBoxColumn13.EnableExpressionEditor = False
        GridViewTextBoxColumn13.FieldName = "episode_parametre_id"
        GridViewTextBoxColumn13.HeaderText = "episode_parametre_id"
        GridViewTextBoxColumn13.IsVisible = False
        GridViewTextBoxColumn13.Name = "episode_parametre_id"
        GridViewTextBoxColumn14.EnableExpressionEditor = False
        GridViewTextBoxColumn14.FieldName = "parametre_id"
        GridViewTextBoxColumn14.HeaderText = "parametre_id"
        GridViewTextBoxColumn14.IsVisible = False
        GridViewTextBoxColumn14.Name = "parametre_id"
        GridViewTextBoxColumn15.EnableExpressionEditor = False
        GridViewTextBoxColumn15.FieldName = "description"
        GridViewTextBoxColumn15.HeaderText = "description"
        GridViewTextBoxColumn15.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn15.Name = "description"
        GridViewTextBoxColumn15.ReadOnly = True
        GridViewTextBoxColumn15.Width = 250
        GridViewTextBoxColumn16.EnableExpressionEditor = False
        GridViewTextBoxColumn16.FieldName = "entier"
        GridViewTextBoxColumn16.HeaderText = "entier"
        GridViewTextBoxColumn16.IsVisible = False
        GridViewTextBoxColumn16.Name = "entier"
        GridViewTextBoxColumn16.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn17.EnableExpressionEditor = False
        GridViewTextBoxColumn17.FieldName = "decimal"
        GridViewTextBoxColumn17.HeaderText = "decimal"
        GridViewTextBoxColumn17.IsVisible = False
        GridViewTextBoxColumn17.Name = "decimal"
        GridViewTextBoxColumn17.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewMaskBoxColumn2.DataType = GetType(Decimal)
        GridViewMaskBoxColumn2.EnableExpressionEditor = False
        GridViewMaskBoxColumn2.FieldName = "valeurInput"
        GridViewMaskBoxColumn2.FormatInfo = New System.Globalization.CultureInfo("fr-FR")
        GridViewMaskBoxColumn2.HeaderText = "valeur"
        GridViewMaskBoxColumn2.Mask = "G"
        GridViewMaskBoxColumn2.MaskType = Telerik.WinControls.UI.MaskType.Numeric
        GridViewMaskBoxColumn2.Name = "valeurInput"
        GridViewMaskBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewMaskBoxColumn2.Width = 70
        GridViewTextBoxColumn18.EnableExpressionEditor = False
        GridViewTextBoxColumn18.FieldName = "unite"
        GridViewTextBoxColumn18.HeaderText = "Unité"
        GridViewTextBoxColumn18.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn18.Name = "unite"
        GridViewTextBoxColumn18.ReadOnly = True
        GridViewTextBoxColumn18.Width = 80
        GridViewTextBoxColumn19.EnableExpressionEditor = False
        GridViewTextBoxColumn19.FieldName = "valeur"
        GridViewTextBoxColumn19.HeaderText = "valeur"
        GridViewTextBoxColumn19.IsVisible = False
        GridViewTextBoxColumn19.Name = "valeur"
        GridViewTextBoxColumn20.EnableExpressionEditor = False
        GridViewTextBoxColumn20.FieldName = "parametre_ajoute"
        GridViewTextBoxColumn20.IsVisible = False
        GridViewTextBoxColumn20.Name = "parametre_ajoute"
        GridViewTextBoxColumn20.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn20.Width = 20
        GridViewTextBoxColumn21.EnableExpressionEditor = False
        GridViewTextBoxColumn21.FieldName = "ajoute"
        GridViewTextBoxColumn21.HeaderText = "+"
        GridViewTextBoxColumn21.Name = "ajoute"
        GridViewTextBoxColumn21.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn21.Width = 15
        Me.RadGridViewParm.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn13, GridViewTextBoxColumn14, GridViewTextBoxColumn15, GridViewTextBoxColumn16, GridViewTextBoxColumn17, GridViewMaskBoxColumn2, GridViewTextBoxColumn18, GridViewTextBoxColumn19, GridViewTextBoxColumn20, GridViewTextBoxColumn21})
        Me.RadGridViewParm.MasterTemplate.ShowFilteringRow = False
        Me.RadGridViewParm.MasterTemplate.ViewDefinition = TableViewDefinition3
        Me.RadGridViewParm.Name = "RadGridViewParm"
        Me.RadGridViewParm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParm.ShowGroupPanel = False
        Me.RadGridViewParm.Size = New System.Drawing.Size(459, 518)
        Me.RadGridViewParm.TabIndex = 5
        '
        'RadBtnAjouter
        '
        Me.RadBtnAjouter.ForeColor = System.Drawing.Color.Black
        Me.RadBtnAjouter.Location = New System.Drawing.Point(12, 536)
        Me.RadBtnAjouter.Name = "RadBtnAjouter"
        Me.RadBtnAjouter.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAjouter.TabIndex = 97
        Me.RadBtnAjouter.TabStop = False
        Me.RadBtnAjouter.Text = "+"
        '
        'RadDesktopAlert1
        '
        Me.RadDesktopAlert1.AutoCloseDelay = 5
        Me.RadDesktopAlert1.FadeAnimationSpeed = 5
        Me.RadDesktopAlert1.Opacity = 0.9!
        Me.RadDesktopAlert1.ScreenPosition = Telerik.WinControls.UI.AlertScreenPosition.TopCenter
        Me.RadDesktopAlert1.ThemeName = ""
        '
        'RadBtnSupprimer
        '
        Me.RadBtnSupprimer.Image = Global.Oasis_WF.My.Resources.Resources.supprimer1
        Me.RadBtnSupprimer.Location = New System.Drawing.Point(42, 536)
        Me.RadBtnSupprimer.Name = "RadBtnSupprimer"
        Me.RadBtnSupprimer.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnSupprimer.TabIndex = 97
        Me.RadBtnSupprimer.Text = "Supprimer"
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
        Me.RadGridView1.Location = New System.Drawing.Point(595, 12)
        '
        '
        '
        Me.RadGridView1.MasterTemplate.AllowAddNewRow = False
        Me.RadGridView1.MasterTemplate.AllowDeleteRow = False
        Me.RadGridView1.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn22.EnableExpressionEditor = False
        GridViewTextBoxColumn22.FieldName = "id"
        GridViewTextBoxColumn22.HeaderText = "id"
        GridViewTextBoxColumn22.IsVisible = False
        GridViewTextBoxColumn22.Name = "id"
        GridViewTextBoxColumn23.EnableExpressionEditor = False
        GridViewTextBoxColumn23.FieldName = "description"
        GridViewTextBoxColumn23.HeaderText = "Description"
        GridViewTextBoxColumn23.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn23.Name = "description"
        GridViewTextBoxColumn23.Width = 200
        GridViewTextBoxColumn24.EnableExpressionEditor = False
        GridViewTextBoxColumn24.FieldName = "unite"
        GridViewTextBoxColumn24.HeaderText = "Unité"
        GridViewTextBoxColumn24.Name = "unite"
        GridViewTextBoxColumn24.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn24.Width = 65
        Me.RadGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn22, GridViewTextBoxColumn23, GridViewTextBoxColumn24})
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition4
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridView1.ShowGroupPanel = False
        Me.RadGridView1.Size = New System.Drawing.Size(309, 518)
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
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFParametreDetailEdit"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAjouter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSupprimer, System.ComponentModel.ISupportInitialize).EndInit()
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

