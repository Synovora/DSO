<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFListeActions
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
        Dim GridViewTextBoxColumn16 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn17 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn18 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn19 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn20 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition4 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.LblNomUtilisateur = New System.Windows.Forms.Label()
        Me.RadGridViewAction = New Telerik.WinControls.UI.RadGridView()
        Me.DteSelection = New System.Windows.Forms.DateTimePicker()
        Me.RadBtnAvant = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnApres = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewAction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewAction.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAvant, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnApres, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(939, 578)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 14
        Me.RadBtnAbandon.Text = "Abandonner"
        '
        'LblNomUtilisateur
        '
        Me.LblNomUtilisateur.AutoSize = True
        Me.LblNomUtilisateur.Location = New System.Drawing.Point(12, 9)
        Me.LblNomUtilisateur.Name = "LblNomUtilisateur"
        Me.LblNomUtilisateur.Size = New System.Drawing.Size(10, 13)
        Me.LblNomUtilisateur.TabIndex = 15
        Me.LblNomUtilisateur.Text = "."
        '
        'RadGridViewAction
        '
        Me.RadGridViewAction.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewAction.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewAction.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewAction.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewAction.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewAction.Location = New System.Drawing.Point(12, 43)
        '
        '
        '
        Me.RadGridViewAction.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewAction.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewAction.MasterTemplate.AllowDragToGroup = False
        Me.RadGridViewAction.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn16.EnableExpressionEditor = False
        GridViewTextBoxColumn16.HeaderText = "Date"
        GridViewTextBoxColumn16.Name = "Date"
        GridViewTextBoxColumn16.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn16.Width = 80
        GridViewTextBoxColumn17.EnableExpressionEditor = False
        GridViewTextBoxColumn17.HeaderText = "Heure"
        GridViewTextBoxColumn17.Name = "Heure"
        GridViewTextBoxColumn17.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn17.Width = 70
        GridViewTextBoxColumn18.EnableExpressionEditor = False
        GridViewTextBoxColumn18.HeaderText = "Prénom patient"
        GridViewTextBoxColumn18.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn18.Name = "patientPrenom"
        GridViewTextBoxColumn18.Width = 150
        GridViewTextBoxColumn19.EnableExpressionEditor = False
        GridViewTextBoxColumn19.HeaderText = "Nom patient"
        GridViewTextBoxColumn19.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn19.Name = "patientNom"
        GridViewTextBoxColumn19.Width = 300
        GridViewTextBoxColumn20.EnableExpressionEditor = False
        GridViewTextBoxColumn20.HeaderText = "Action"
        GridViewTextBoxColumn20.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn20.Name = "action"
        GridViewTextBoxColumn20.Width = 400
        Me.RadGridViewAction.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn16, GridViewTextBoxColumn17, GridViewTextBoxColumn18, GridViewTextBoxColumn19, GridViewTextBoxColumn20})
        Me.RadGridViewAction.MasterTemplate.ViewDefinition = TableViewDefinition4
        Me.RadGridViewAction.Name = "RadGridViewAction"
        Me.RadGridViewAction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewAction.Size = New System.Drawing.Size(1037, 529)
        Me.RadGridViewAction.TabIndex = 16
        '
        'DteSelection
        '
        Me.DteSelection.Location = New System.Drawing.Point(461, 9)
        Me.DteSelection.Name = "DteSelection"
        Me.DteSelection.Size = New System.Drawing.Size(200, 20)
        Me.DteSelection.TabIndex = 17
        '
        'RadBtnAvant
        '
        Me.RadBtnAvant.Location = New System.Drawing.Point(345, 7)
        Me.RadBtnAvant.Name = "RadBtnAvant"
        Me.RadBtnAvant.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAvant.TabIndex = 18
        Me.RadBtnAvant.Text = "Avant"
        '
        'RadBtnApres
        '
        Me.RadBtnApres.Location = New System.Drawing.Point(667, 7)
        Me.RadBtnApres.Name = "RadBtnApres"
        Me.RadBtnApres.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnApres.TabIndex = 19
        Me.RadBtnApres.Text = "Après"
        '
        'RadFListeActions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1061, 609)
        Me.Controls.Add(Me.RadBtnApres)
        Me.Controls.Add(Me.RadBtnAvant)
        Me.Controls.Add(Me.DteSelection)
        Me.Controls.Add(Me.RadGridViewAction)
        Me.Controls.Add(Me.LblNomUtilisateur)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFListeActions"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.Text = "RadFListeActions"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewAction.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewAction, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAvant, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnApres, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblNomUtilisateur As Label
    Friend WithEvents RadGridViewAction As Telerik.WinControls.UI.RadGridView
    Friend WithEvents DteSelection As DateTimePicker
    Friend WithEvents RadBtnAvant As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnApres As Telerik.WinControls.UI.RadButton
End Class

