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
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.LblNomUtilisateur = New System.Windows.Forms.Label()
        Me.RadGridViewAction = New Telerik.WinControls.UI.RadGridView()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewAction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewAction.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(914, 578)
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
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.HeaderText = "Date"
        GridViewTextBoxColumn6.Name = "Date"
        GridViewTextBoxColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn6.Width = 60
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.HeaderText = "Heure"
        GridViewTextBoxColumn7.Name = "Heure"
        GridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn7.Width = 60
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.HeaderText = "Prénom patient"
        GridViewTextBoxColumn8.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn8.Name = "patientPrenom"
        GridViewTextBoxColumn8.Width = 150
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.HeaderText = "Nom patient"
        GridViewTextBoxColumn9.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn9.Name = "patientNom"
        GridViewTextBoxColumn9.Width = 300
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.HeaderText = "Action"
        GridViewTextBoxColumn10.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn10.Name = "action"
        GridViewTextBoxColumn10.Width = 400
        Me.RadGridViewAction.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8, GridViewTextBoxColumn9, GridViewTextBoxColumn10})
        Me.RadGridViewAction.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridViewAction.Name = "RadGridViewAction"
        Me.RadGridViewAction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewAction.Size = New System.Drawing.Size(1012, 529)
        Me.RadGridViewAction.TabIndex = 16
        '
        'RadFListeActions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1037, 609)
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
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents LblNomUtilisateur As Label
    Friend WithEvents RadGridViewAction As Telerik.WinControls.UI.RadGridView
End Class

