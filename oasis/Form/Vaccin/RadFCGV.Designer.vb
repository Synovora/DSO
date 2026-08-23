<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RadFCGV
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadFCGV))
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.Grid = New Telerik.WinControls.UI.RadGridView()
        Me.RadButtonEditValence = New Telerik.WinControls.UI.RadButton()
        Me.BtnDateDelete = New Telerik.WinControls.UI.RadButton()
        Me.BtnDateAdd = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RadGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
        Me.RBDateInactif = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadLabel3 = New Telerik.WinControls.UI.RadLabel()
        Me.RBDateActif = New Telerik.WinControls.UI.RadRadioButton()
        Me.RBDateAll = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadLabel2 = New Telerik.WinControls.UI.RadLabel()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.TextYear = New Telerik.WinControls.UI.RadTextBox()
        Me.TextMonth = New Telerik.WinControls.UI.RadTextBox()
        Me.TextDay = New Telerik.WinControls.UI.RadTextBox()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadButtonEditValence, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDateDelete, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDateAdd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox2.SuspendLayout()
        CType(Me.RBDateInactif, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RBDateActif, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RBDateAll, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextYear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextMonth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextDay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1607, 650)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 2
        '
        'Grid
        '
        Me.Grid.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Grid.Cursor = System.Windows.Forms.Cursors.Default
        Me.Grid.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.Grid.ForeColor = System.Drawing.Color.Black
        Me.Grid.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Grid.Location = New System.Drawing.Point(12, 12)
        '
        '
        '
        Me.Grid.MasterTemplate.AllowAddNewRow = False
        Me.Grid.MasterTemplate.AllowCellContextMenu = False
        Me.Grid.MasterTemplate.AllowColumnChooser = False
        Me.Grid.MasterTemplate.AllowColumnHeaderContextMenu = False
        Me.Grid.MasterTemplate.AllowColumnReorder = False
        Me.Grid.MasterTemplate.AllowColumnResize = False
        Me.Grid.MasterTemplate.AllowDeleteRow = False
        Me.Grid.MasterTemplate.AllowDragToGroup = False
        Me.Grid.MasterTemplate.AllowEditRow = False
        Me.Grid.MasterTemplate.AllowRowHeaderContextMenu = False
        Me.Grid.MasterTemplate.AllowRowResize = False
        Me.Grid.MasterTemplate.AutoGenerateColumns = False
        Me.Grid.MasterTemplate.EnableGrouping = False
        Me.Grid.MasterTemplate.ShowFilteringRow = False
        Me.Grid.MasterTemplate.ShowRowHeaderColumn = False
        Me.Grid.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.Grid.Name = "Grid"
        Me.Grid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Grid.Size = New System.Drawing.Size(1452, 662)
        Me.Grid.TabIndex = 55
        '
        'RadButtonEditValence
        '
        Me.RadButtonEditValence.Location = New System.Drawing.Point(16, 21)
        Me.RadButtonEditValence.Name = "RadButtonEditValence"
        Me.RadButtonEditValence.Size = New System.Drawing.Size(132, 24)
        Me.RadButtonEditValence.TabIndex = 10
        Me.RadButtonEditValence.Text = "Gestion d'affichage"
        '
        'BtnDateDelete
        '
        Me.BtnDateDelete.Location = New System.Drawing.Point(16, 153)
        Me.BtnDateDelete.Name = "BtnDateDelete"
        Me.BtnDateDelete.Size = New System.Drawing.Size(132, 24)
        Me.BtnDateDelete.TabIndex = 14
        Me.BtnDateDelete.Text = "Supprimer"
        '
        'BtnDateAdd
        '
        Me.BtnDateAdd.Location = New System.Drawing.Point(16, 123)
        Me.BtnDateAdd.Name = "BtnDateAdd"
        Me.BtnDateAdd.Size = New System.Drawing.Size(132, 24)
        Me.BtnDateAdd.TabIndex = 12
        Me.BtnDateAdd.Text = "Ajouter"
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.RadButtonEditValence)
        Me.RadGroupBox1.HeaderText = "Valence"
        Me.RadGroupBox1.Location = New System.Drawing.Point(1470, 12)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(161, 56)
        Me.RadGroupBox1.TabIndex = 57
        Me.RadGroupBox1.Text = "Valence"
        '
        'RadGroupBox2
        '
        Me.RadGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox2.Controls.Add(Me.RBDateInactif)
        Me.RadGroupBox2.Controls.Add(Me.RadLabel3)
        Me.RadGroupBox2.Controls.Add(Me.RBDateActif)
        Me.RadGroupBox2.Controls.Add(Me.RBDateAll)
        Me.RadGroupBox2.Controls.Add(Me.RadLabel2)
        Me.RadGroupBox2.Controls.Add(Me.RadLabel1)
        Me.RadGroupBox2.Controls.Add(Me.TextYear)
        Me.RadGroupBox2.Controls.Add(Me.TextMonth)
        Me.RadGroupBox2.Controls.Add(Me.TextDay)
        Me.RadGroupBox2.Controls.Add(Me.BtnDateAdd)
        Me.RadGroupBox2.Controls.Add(Me.BtnDateDelete)
        Me.RadGroupBox2.HeaderText = "Date"
        Me.RadGroupBox2.Location = New System.Drawing.Point(1470, 74)
        Me.RadGroupBox2.Name = "RadGroupBox2"
        Me.RadGroupBox2.Size = New System.Drawing.Size(161, 188)
        Me.RadGroupBox2.TabIndex = 58
        Me.RadGroupBox2.Text = "Date"
        '
        'RBDateInactif
        '
        Me.RBDateInactif.Location = New System.Drawing.Point(105, 21)
        Me.RBDateInactif.Name = "RBDateInactif"
        Me.RBDateInactif.Size = New System.Drawing.Size(43, 18)
        Me.RBDateInactif.TabIndex = 16
        Me.RBDateInactif.Text = "Sans"
        '
        'RadLabel3
        '
        Me.RadLabel3.Location = New System.Drawing.Point(16, 98)
        Me.RadLabel3.Name = "RadLabel3"
        Me.RadLabel3.Size = New System.Drawing.Size(49, 18)
        Me.RadLabel3.TabIndex = 20
        Me.RadLabel3.Text = "Annees :"
        '
        'RBDateActif
        '
        Me.RBDateActif.Location = New System.Drawing.Point(58, 21)
        Me.RBDateActif.Name = "RBDateActif"
        Me.RBDateActif.Size = New System.Drawing.Size(44, 18)
        Me.RBDateActif.TabIndex = 15
        Me.RBDateActif.Text = "Avec"
        '
        'RBDateAll
        '
        Me.RBDateAll.Location = New System.Drawing.Point(8, 21)
        Me.RBDateAll.Name = "RBDateAll"
        Me.RBDateAll.Size = New System.Drawing.Size(44, 18)
        Me.RBDateAll.TabIndex = 14
        Me.RBDateAll.Text = "Tous"
        '
        'RadLabel2
        '
        Me.RadLabel2.Location = New System.Drawing.Point(16, 72)
        Me.RadLabel2.Name = "RadLabel2"
        Me.RadLabel2.Size = New System.Drawing.Size(36, 18)
        Me.RadLabel2.TabIndex = 19
        Me.RadLabel2.Text = "Mois :"
        '
        'RadLabel1
        '
        Me.RadLabel1.Location = New System.Drawing.Point(16, 46)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(38, 18)
        Me.RadLabel1.TabIndex = 18
        Me.RadLabel1.Text = "Jours :"
        '
        'TextYear
        '
        Me.TextYear.Location = New System.Drawing.Point(71, 97)
        Me.TextYear.Name = "TextYear"
        Me.TextYear.NullText = "0"
        Me.TextYear.Size = New System.Drawing.Size(77, 20)
        Me.TextYear.TabIndex = 17
        '
        'TextMonth
        '
        Me.TextMonth.Location = New System.Drawing.Point(71, 71)
        Me.TextMonth.Name = "TextMonth"
        Me.TextMonth.NullText = "0"
        Me.TextMonth.Size = New System.Drawing.Size(77, 20)
        Me.TextMonth.TabIndex = 16
        '
        'TextDay
        '
        Me.TextDay.Location = New System.Drawing.Point(71, 45)
        Me.TextDay.Name = "TextDay"
        Me.TextDay.NullText = "0"
        Me.TextDay.Size = New System.Drawing.Size(77, 20)
        Me.TextDay.TabIndex = 15
        '
        'RadFCGV
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1643, 686)
        Me.Controls.Add(Me.RadGroupBox2)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RadFCGV"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RadFATCListe"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadButtonEditValence, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDateDelete, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDateAdd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        CType(Me.RadGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox2.ResumeLayout(False)
        Me.RadGroupBox2.PerformLayout()
        CType(Me.RBDateInactif, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RBDateActif, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RBDateAll, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextYear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextMonth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextDay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents Grid As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadButtonEditValence As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnDateDelete As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnDateAdd As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadGroupBox2 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents RadLabel3 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel2 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents TextYear As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents TextMonth As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents TextDay As Telerik.WinControls.UI.RadTextBox
    Friend WithEvents RBDateInactif As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents RBDateActif As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents RBDateAll As Telerik.WinControls.UI.RadRadioButton
End Class

