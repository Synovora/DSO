<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSousEpisodeReponseAttribution
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
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        Me.RadGridView2 = New Telerik.WinControls.UI.RadGridView()
        Me.RadButton1 = New Telerik.WinControls.UI.RadButton()
        Me.RadButton2 = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView2.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButton2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.RadButton2)
        Me.RadPanel1.Controls.Add(Me.RadButton1)
        Me.RadPanel1.Location = New System.Drawing.Point(636, 12)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(121, 339)
        Me.RadPanel1.TabIndex = 1
        '
        'RadGridView1
        '
        Me.RadGridView1.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridView1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridView1.ForeColor = System.Drawing.Color.Black
        Me.RadGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridView1.Location = New System.Drawing.Point(13, 13)
        '
        '
        '
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "Objet"
        GridViewTextBoxColumn1.MinWidth = 100
        GridViewTextBoxColumn1.Name = "column1"
        GridViewTextBoxColumn1.Width = 300
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "De"
        GridViewTextBoxColumn2.MinWidth = 80
        GridViewTextBoxColumn2.Name = "column2"
        GridViewTextBoxColumn2.Width = 150
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "Date"
        GridViewTextBoxColumn3.MinWidth = 50
        GridViewTextBoxColumn3.Name = "column3"
        GridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn3.Width = 80
        Me.RadGridView1.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3})
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridView1.Size = New System.Drawing.Size(617, 338)
        Me.RadGridView1.TabIndex = 2
        '
        'RadGridView2
        '
        Me.RadGridView2.Location = New System.Drawing.Point(13, 358)
        '
        '
        '
        Me.RadGridView2.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridView2.Name = "RadGridView2"
        Me.RadGridView2.Size = New System.Drawing.Size(744, 338)
        Me.RadGridView2.TabIndex = 3
        '
        'RadButton1
        '
        Me.RadButton1.Location = New System.Drawing.Point(4, 33)
        Me.RadButton1.Name = "RadButton1"
        Me.RadButton1.Size = New System.Drawing.Size(114, 24)
        Me.RadButton1.TabIndex = 0
        Me.RadButton1.Text = "Supprimer"
        '
        'RadButton2
        '
        Me.RadButton2.Location = New System.Drawing.Point(4, 3)
        Me.RadButton2.Name = "RadButton2"
        Me.RadButton2.Size = New System.Drawing.Size(114, 24)
        Me.RadButton2.TabIndex = 1
        Me.RadButton2.Text = "Attribuer"
        '
        'FrmSousEpisodeReponseAttribution
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(770, 704)
        Me.Controls.Add(Me.RadGridView2)
        Me.Controls.Add(Me.RadGridView1)
        Me.Controls.Add(Me.RadPanel1)
        Me.Name = "FrmSousEpisodeReponseAttribution"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "s"
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView2.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButton2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadGridView1 As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadGridView2 As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadButton2 As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadButton1 As Telerik.WinControls.UI.RadButton
End Class

