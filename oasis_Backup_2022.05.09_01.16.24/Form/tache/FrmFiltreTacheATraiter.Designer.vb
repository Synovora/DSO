<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmFiltreTacheATraiter
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
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.RadTreeView1 = New Telerik.WinControls.UI.RadTreeView()
        Me.RadTextBox1 = New Telerik.WinControls.UI.RadTextBox()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.BtnValidate = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadTreeView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadTextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.RadTreeView1)
        Me.RadPanel1.Controls.Add(Me.RadTextBox1)
        Me.RadPanel1.Controls.Add(Me.RadLabel1)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(437, 449)
        Me.RadPanel1.TabIndex = 0
        '
        'RadTreeView1
        '
        Me.RadTreeView1.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadTreeView1.CheckBoxes = True
        Me.RadTreeView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadTreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadTreeView1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadTreeView1.ForeColor = System.Drawing.Color.Black
        Me.RadTreeView1.Location = New System.Drawing.Point(0, 28)
        Me.RadTreeView1.Name = "RadTreeView1"
        Me.RadTreeView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadTreeView1.Size = New System.Drawing.Size(437, 297)
        Me.RadTreeView1.SpacingBetweenNodes = -1
        Me.RadTreeView1.TabIndex = 2
        '
        'RadTextBox1
        '
        Me.RadTextBox1.AutoSize = False
        Me.RadTextBox1.BackColor = System.Drawing.Color.AntiqueWhite
        Me.RadTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadTextBox1.Location = New System.Drawing.Point(0, 325)
        Me.RadTextBox1.Multiline = True
        Me.RadTextBox1.Name = "RadTextBox1"
        Me.RadTextBox1.ReadOnly = True
        Me.RadTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.RadTextBox1.Size = New System.Drawing.Size(437, 124)
        Me.RadTextBox1.TabIndex = 3
        '
        'RadLabel1
        '
        Me.RadLabel1.AutoSize = False
        Me.RadLabel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadLabel1.Location = New System.Drawing.Point(0, 0)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(437, 28)
        Me.RadLabel1.TabIndex = 1
        Me.RadLabel1.Text = "Unités Sanitaires / SItes"
        '
        'RadPanel2
        '
        Me.RadPanel2.Controls.Add(Me.BtnValidate)
        Me.RadPanel2.Controls.Add(Me.BtnCancel)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel2.Location = New System.Drawing.Point(0, 449)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(437, 45)
        Me.RadPanel2.TabIndex = 1
        '
        'BtnValidate
        '
        Me.BtnValidate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BtnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnValidate.Location = New System.Drawing.Point(103, 9)
        Me.BtnValidate.Name = "BtnValidate"
        Me.BtnValidate.Size = New System.Drawing.Size(75, 26)
        Me.BtnValidate.TabIndex = 13
        Me.BtnValidate.Text = "Valider"
        Me.BtnValidate.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancel.Location = New System.Drawing.Point(255, 9)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(78, 26)
        Me.BtnCancel.TabIndex = 12
        Me.BtnCancel.Text = "&Annuler"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'FrmFiltreTacheATraiter
        '
        Me.AcceptButton = Me.BtnValidate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(437, 494)
        Me.Controls.Add(Me.RadPanel1)
        Me.Controls.Add(Me.RadPanel2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmFiltreTacheATraiter"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Selection de site"
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        CType(Me.RadTreeView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadTextBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel2.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadTreeView1 As Telerik.WinControls.UI.RadTreeView
    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents BtnValidate As Button
    Friend WithEvents BtnCancel As Button
    Friend WithEvents RadTextBox1 As Telerik.WinControls.UI.RadTextBox
End Class

