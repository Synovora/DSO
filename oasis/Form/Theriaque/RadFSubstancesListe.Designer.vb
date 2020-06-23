<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFSubstancesListe
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
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewSubstance = New Telerik.WinControls.UI.RadGridView()
        Me.TextBoxSpecialite = New System.Windows.Forms.TextBox()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGridViewSubstance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewSubstance.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGridViewSubstance
        '
        Me.RadGridViewSubstance.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewSubstance.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewSubstance.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewSubstance.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewSubstance.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewSubstance.Location = New System.Drawing.Point(12, 47)
        '
        '
        '
        Me.RadGridViewSubstance.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewSubstance.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewSubstance.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "column1"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.Name = "SAC_NOM"
        GridViewTextBoxColumn1.Width = 450
        Me.RadGridViewSubstance.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1})
        Me.RadGridViewSubstance.MasterTemplate.ShowColumnHeaders = False
        Me.RadGridViewSubstance.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewSubstance.Name = "RadGridViewSubstance"
        Me.RadGridViewSubstance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewSubstance.ShowGroupPanel = False
        Me.RadGridViewSubstance.Size = New System.Drawing.Size(494, 178)
        Me.RadGridViewSubstance.TabIndex = 0
        '
        'TextBoxSpecialite
        '
        Me.TextBoxSpecialite.Location = New System.Drawing.Point(12, 21)
        Me.TextBoxSpecialite.Name = "TextBoxSpecialite"
        Me.TextBoxSpecialite.ReadOnly = True
        Me.TextBoxSpecialite.Size = New System.Drawing.Size(494, 20)
        Me.TextBoxSpecialite.TabIndex = 1
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(482, 231)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 2
        '
        'RadFSubstancesListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(514, 267)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.TextBoxSpecialite)
        Me.Controls.Add(Me.RadGridViewSubstance)
        Me.Name = "RadFSubstancesListe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFSubstancesListe"
        CType(Me.RadGridViewSubstance.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewSubstance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadGridViewSubstance As Telerik.WinControls.UI.RadGridView
    Friend WithEvents TextBoxSpecialite As TextBox
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
End Class

