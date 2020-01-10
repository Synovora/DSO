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
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn13 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn14 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewMaskBoxColumn2 As Telerik.WinControls.UI.GridViewMaskBoxColumn = New Telerik.WinControls.UI.GridViewMaskBoxColumn()
        Dim GridViewTextBoxColumn15 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn16 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn17 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn18 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.RadGridViewParm = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAjouter = New Telerik.WinControls.UI.RadButton()
        Me.RadDesktopAlert1 = New Telerik.WinControls.UI.RadDesktopAlert(Me.components)
        Me.RadBtnSupprimer = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewParm.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAjouter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSupprimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(347, 536)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 0
        Me.RadBtnAbandon.Text = "Abandonner"
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
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.FieldName = "episode_parametre_id"
        GridViewTextBoxColumn10.HeaderText = "episode_parametre_id"
        GridViewTextBoxColumn10.IsVisible = False
        GridViewTextBoxColumn10.Name = "episode_parametre_id"
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.FieldName = "parametre_id"
        GridViewTextBoxColumn11.HeaderText = "parametre_id"
        GridViewTextBoxColumn11.IsVisible = False
        GridViewTextBoxColumn11.Name = "parametre_id"
        GridViewTextBoxColumn12.EnableExpressionEditor = False
        GridViewTextBoxColumn12.FieldName = "description"
        GridViewTextBoxColumn12.HeaderText = "description"
        GridViewTextBoxColumn12.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn12.Name = "description"
        GridViewTextBoxColumn12.ReadOnly = True
        GridViewTextBoxColumn12.Width = 250
        GridViewTextBoxColumn13.EnableExpressionEditor = False
        GridViewTextBoxColumn13.FieldName = "entier"
        GridViewTextBoxColumn13.HeaderText = "entier"
        GridViewTextBoxColumn13.IsVisible = False
        GridViewTextBoxColumn13.Name = "entier"
        GridViewTextBoxColumn13.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn14.EnableExpressionEditor = False
        GridViewTextBoxColumn14.FieldName = "decimal"
        GridViewTextBoxColumn14.HeaderText = "decimal"
        GridViewTextBoxColumn14.IsVisible = False
        GridViewTextBoxColumn14.Name = "decimal"
        GridViewTextBoxColumn14.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
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
        GridViewTextBoxColumn15.EnableExpressionEditor = False
        GridViewTextBoxColumn15.FieldName = "unite"
        GridViewTextBoxColumn15.HeaderText = "Unité"
        GridViewTextBoxColumn15.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn15.Name = "unite"
        GridViewTextBoxColumn15.ReadOnly = True
        GridViewTextBoxColumn15.Width = 80
        GridViewTextBoxColumn16.EnableExpressionEditor = False
        GridViewTextBoxColumn16.FieldName = "valeur"
        GridViewTextBoxColumn16.HeaderText = "valeur"
        GridViewTextBoxColumn16.IsVisible = False
        GridViewTextBoxColumn16.Name = "valeur"
        GridViewTextBoxColumn17.EnableExpressionEditor = False
        GridViewTextBoxColumn17.FieldName = "parametre_ajoute"
        GridViewTextBoxColumn17.IsVisible = False
        GridViewTextBoxColumn17.Name = "parametre_ajoute"
        GridViewTextBoxColumn17.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn17.Width = 20
        GridViewTextBoxColumn18.EnableExpressionEditor = False
        GridViewTextBoxColumn18.FieldName = "ajoute"
        GridViewTextBoxColumn18.HeaderText = "+"
        GridViewTextBoxColumn18.Name = "ajoute"
        GridViewTextBoxColumn18.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn18.Width = 15
        Me.RadGridViewParm.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn10, GridViewTextBoxColumn11, GridViewTextBoxColumn12, GridViewTextBoxColumn13, GridViewTextBoxColumn14, GridViewMaskBoxColumn2, GridViewTextBoxColumn15, GridViewTextBoxColumn16, GridViewTextBoxColumn17, GridViewTextBoxColumn18})
        Me.RadGridViewParm.MasterTemplate.ShowFilteringRow = False
        Me.RadGridViewParm.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridViewParm.Name = "RadGridViewParm"
        Me.RadGridViewParm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewParm.ShowGroupPanel = False
        Me.RadGridViewParm.Size = New System.Drawing.Size(459, 518)
        Me.RadGridViewParm.TabIndex = 5
        '
        'RadBtnAjouter
        '
        Me.RadBtnAjouter.Location = New System.Drawing.Point(214, 536)
        Me.RadBtnAjouter.Name = "RadBtnAjouter"
        Me.RadBtnAjouter.Size = New System.Drawing.Size(127, 24)
        Me.RadBtnAjouter.TabIndex = 97
        Me.RadBtnAjouter.Text = "Ajouter un paramètre"
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
        Me.RadBtnSupprimer.Location = New System.Drawing.Point(98, 536)
        Me.RadBtnSupprimer.Name = "RadBtnSupprimer"
        Me.RadBtnSupprimer.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnSupprimer.TabIndex = 97
        Me.RadBtnSupprimer.Text = "Supprimer"
        '
        'RadFEpisodeParametreDetailEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(483, 567)
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
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents RadGridViewParm As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAjouter As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadDesktopAlert1 As Telerik.WinControls.UI.RadDesktopAlert
    Friend WithEvents RadBtnSupprimer As Telerik.WinControls.UI.RadButton
End Class

