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
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn8 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadFListeActions))
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
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1025, 578)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 14
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
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "Date"
        GridViewTextBoxColumn1.Name = "Date"
        GridViewTextBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn1.Width = 80
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "Heure"
        GridViewTextBoxColumn2.Name = "Heure"
        GridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn2.Width = 70
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "Prénom patient"
        GridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn3.Name = "patientPrenom"
        GridViewTextBoxColumn3.Width = 150
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "Nom patient"
        GridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn4.Name = "patientNom"
        GridViewTextBoxColumn4.Width = 300
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.HeaderText = "Action"
        GridViewTextBoxColumn5.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn5.Name = "action"
        GridViewTextBoxColumn5.Width = 400
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.HeaderText = "fonction"
        GridViewTextBoxColumn6.IsVisible = False
        GridViewTextBoxColumn6.Name = "fonction"
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.HeaderText = "fonctionId"
        GridViewTextBoxColumn7.IsVisible = False
        GridViewTextBoxColumn7.Name = "fonctionId"
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.HeaderText = "patientId"
        GridViewTextBoxColumn8.IsVisible = False
        GridViewTextBoxColumn8.Name = "patientId"
        Me.RadGridViewAction.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8})
        Me.RadGridViewAction.MasterTemplate.ViewDefinition = TableViewDefinition1
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
        Me.RadBtnAvant.Text = "< Avant"
        '
        'RadBtnApres
        '
        Me.RadBtnApres.Location = New System.Drawing.Point(667, 7)
        Me.RadBtnApres.Name = "RadBtnApres"
        Me.RadBtnApres.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnApres.TabIndex = 19
        Me.RadBtnApres.Text = "Après >"
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
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
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

