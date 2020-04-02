<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFEpisodeConclusionContextePatient
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
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn5 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn6 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn7 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGroupBoxEtatCivil = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblDateNaissance = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LblPatientDateMaj = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblPatientSite = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LblPatientPrenom = New System.Windows.Forms.Label()
        Me.LblPatientNom = New System.Windows.Forms.Label()
        Me.LblPatientAge = New System.Windows.Forms.Label()
        Me.LblPatientGenre = New System.Windows.Forms.Label()
        Me.LblPatientNIR = New System.Windows.Forms.Label()
        Me.RadContexteDataGridView = New Telerik.WinControls.UI.RadGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerUnContexteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RadChkContexteTous = New Telerik.WinControls.UI.RadCheckBox()
        Me.RadChkContextePublie = New Telerik.WinControls.UI.RadCheckBox()
        Me.RadBtnCreation = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadConclusionGridView = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnSuppprimer = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnSelect = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBoxEtatCivil.SuspendLayout()
        CType(Me.RadContexteDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadContexteDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.RadChkContexteTous, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadChkContextePublie, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnCreation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadConclusionGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadConclusionGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSuppprimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGroupBoxEtatCivil
        '
        Me.RadGroupBoxEtatCivil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBoxEtatCivil.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblDateNaissance)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label13)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientDateMaj)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label5)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientSite)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.Label4)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientPrenom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNom)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientAge)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientGenre)
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblPatientNIR)
        Me.RadGroupBoxEtatCivil.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupBoxEtatCivil.HeaderText = "Etat civil"
        Me.RadGroupBoxEtatCivil.Location = New System.Drawing.Point(3, 12)
        Me.RadGroupBoxEtatCivil.Name = "RadGroupBoxEtatCivil"
        Me.RadGroupBoxEtatCivil.Size = New System.Drawing.Size(1140, 59)
        Me.RadGroupBoxEtatCivil.TabIndex = 3
        Me.RadGroupBoxEtatCivil.Text = "Etat civil"
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'LblDateNaissance
        '
        Me.LblDateNaissance.AutoSize = True
        Me.LblDateNaissance.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.LblDateNaissance.ForeColor = System.Drawing.Color.Red
        Me.LblDateNaissance.Location = New System.Drawing.Point(472, 20)
        Me.LblDateNaissance.Name = "LblDateNaissance"
        Me.LblDateNaissance.Size = New System.Drawing.Size(74, 17)
        Me.LblDateNaissance.TabIndex = 47
        Me.LblDateNaissance.Text = "25-04-2018"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(771, 20)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 42
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(1062, 20)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientDateMaj.TabIndex = 41
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(928, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(971, 37)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(36, 13)
        Me.LblPatientSite.TabIndex = 37
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(928, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Site :"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.LblPatientPrenom.ForeColor = System.Drawing.Color.Red
        Me.LblPatientPrenom.Location = New System.Drawing.Point(9, 20)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(73, 17)
        Me.LblPatientPrenom.TabIndex = 23
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.LblPatientNom.ForeColor = System.Drawing.Color.Red
        Me.LblPatientNom.Location = New System.Drawing.Point(134, 20)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(51, 17)
        Me.LblPatientNom.TabIndex = 24
        Me.LblPatientNom.Text = "Durand"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.LblPatientAge.ForeColor = System.Drawing.Color.DarkRed
        Me.LblPatientAge.Location = New System.Drawing.Point(577, 20)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(46, 17)
        Me.LblPatientAge.TabIndex = 25
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.LblPatientGenre.ForeColor = System.Drawing.Color.DarkRed
        Me.LblPatientGenre.Location = New System.Drawing.Point(686, 20)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(59, 17)
        Me.LblPatientGenre.TabIndex = 26
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(822, 20)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 27
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'RadContexteDataGridView
        '
        Me.RadContexteDataGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadContexteDataGridView.ContextMenuStrip = Me.ContextMenuStrip1
        Me.RadContexteDataGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadContexteDataGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadContexteDataGridView.ForeColor = System.Drawing.Color.Black
        Me.RadContexteDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadContexteDataGridView.Location = New System.Drawing.Point(649, 113)
        '
        '
        '
        Me.RadContexteDataGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadContexteDataGridView.MasterTemplate.AllowCellContextMenu = False
        Me.RadContexteDataGridView.MasterTemplate.AllowDeleteRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "categorie contexte"
        GridViewTextBoxColumn1.IsVisible = False
        GridViewTextBoxColumn1.Name = "categorieContexte"
        GridViewTextBoxColumn2.AllowGroup = False
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "Contexte"
        GridViewTextBoxColumn2.Name = "contexte"
        GridViewTextBoxColumn2.ReadOnly = True
        GridViewTextBoxColumn2.Width = 450
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "contexteId"
        GridViewTextBoxColumn3.IsVisible = False
        GridViewTextBoxColumn3.Name = "contexteId"
        Me.RadContexteDataGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3})
        Me.RadContexteDataGridView.MasterTemplate.ShowFilteringRow = False
        Me.RadContexteDataGridView.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadContexteDataGridView.Name = "RadContexteDataGridView"
        Me.RadContexteDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadContexteDataGridView.ShowGroupPanel = False
        Me.RadContexteDataGridView.Size = New System.Drawing.Size(494, 347)
        Me.RadContexteDataGridView.TabIndex = 4
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerUnContexteToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(168, 26)
        '
        'CréerUnContexteToolStripMenuItem
        '
        Me.CréerUnContexteToolStripMenuItem.Name = "CréerUnContexteToolStripMenuItem"
        Me.CréerUnContexteToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.CréerUnContexteToolStripMenuItem.Text = "Créer un contexte"
        '
        'RadChkContexteTous
        '
        Me.RadChkContexteTous.Location = New System.Drawing.Point(955, 89)
        Me.RadChkContexteTous.Name = "RadChkContexteTous"
        Me.RadChkContexteTous.Size = New System.Drawing.Size(44, 18)
        Me.RadChkContexteTous.TabIndex = 34
        Me.RadChkContexteTous.Text = "Tous"
        '
        'RadChkContextePublie
        '
        Me.RadChkContextePublie.Location = New System.Drawing.Point(853, 89)
        Me.RadChkContextePublie.Name = "RadChkContextePublie"
        Me.RadChkContextePublie.Size = New System.Drawing.Size(96, 18)
        Me.RadChkContextePublie.TabIndex = 35
        Me.RadChkContextePublie.Text = "Non masqué(s)"
        '
        'RadBtnCreation
        '
        Me.RadBtnCreation.Location = New System.Drawing.Point(698, 83)
        Me.RadBtnCreation.Name = "RadBtnCreation"
        Me.RadBtnCreation.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnCreation.TabIndex = 37
        Me.RadBtnCreation.Text = "+ Créer un contexte"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1033, 466)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 38
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadConclusionGridView
        '
        Me.RadConclusionGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadConclusionGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadConclusionGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadConclusionGridView.ForeColor = System.Drawing.Color.Black
        Me.RadConclusionGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadConclusionGridView.Location = New System.Drawing.Point(3, 113)
        '
        '
        '
        Me.RadConclusionGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadConclusionGridView.MasterTemplate.AllowCellContextMenu = False
        Me.RadConclusionGridView.MasterTemplate.AllowDeleteRow = False
        GridViewTextBoxColumn4.AllowGroup = False
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "Conclusion"
        GridViewTextBoxColumn4.Name = "contexte"
        GridViewTextBoxColumn4.ReadOnly = True
        GridViewTextBoxColumn4.Width = 450
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.HeaderText = "contexte_id"
        GridViewTextBoxColumn5.IsVisible = False
        GridViewTextBoxColumn5.Name = "contexte_id"
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.HeaderText = "episode_contexte_id"
        GridViewTextBoxColumn6.IsVisible = False
        GridViewTextBoxColumn6.Name = "episode_contexte_id"
        GridViewTextBoxColumn7.AllowGroup = False
        GridViewTextBoxColumn7.AllowResize = False
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.HeaderText = "Ordre"
        GridViewTextBoxColumn7.Name = "ordre"
        GridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn7.Width = 60
        Me.RadConclusionGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7})
        Me.RadConclusionGridView.MasterTemplate.ShowFilteringRow = False
        Me.RadConclusionGridView.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadConclusionGridView.Name = "RadConclusionGridView"
        Me.RadConclusionGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadConclusionGridView.ShowGroupPanel = False
        Me.RadConclusionGridView.Size = New System.Drawing.Size(553, 347)
        Me.RadConclusionGridView.TabIndex = 39
        '
        'RadBtnSuppprimer
        '
        Me.RadBtnSuppprimer.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadBtnSuppprimer.Location = New System.Drawing.Point(562, 251)
        Me.RadBtnSuppprimer.Name = "RadBtnSuppprimer"
        Me.RadBtnSuppprimer.Size = New System.Drawing.Size(81, 24)
        Me.RadBtnSuppprimer.TabIndex = 41
        Me.RadBtnSuppprimer.Text = "Enlever >>>"
        '
        'RadBtnSelect
        '
        Me.RadBtnSelect.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadBtnSelect.Location = New System.Drawing.Point(562, 221)
        Me.RadBtnSelect.Name = "RadBtnSelect"
        Me.RadBtnSelect.Size = New System.Drawing.Size(81, 24)
        Me.RadBtnSelect.TabIndex = 40
        Me.RadBtnSelect.Text = "<<< Ajouter"
        '
        'RadFEpisodeConclusionContextePatient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1150, 495)
        Me.Controls.Add(Me.RadBtnSuppprimer)
        Me.Controls.Add(Me.RadBtnSelect)
        Me.Controls.Add(Me.RadConclusionGridView)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadBtnCreation)
        Me.Controls.Add(Me.RadChkContexteTous)
        Me.Controls.Add(Me.RadChkContextePublie)
        Me.Controls.Add(Me.RadContexteDataGridView)
        Me.Controls.Add(Me.RadGroupBoxEtatCivil)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFEpisodeConclusionContextePatient"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Conclusion médicale - Selection contexte du patient"
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBoxEtatCivil.ResumeLayout(False)
        Me.RadGroupBoxEtatCivil.PerformLayout()
        CType(Me.RadContexteDataGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadContexteDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.RadChkContexteTous, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadChkContextePublie, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnCreation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadConclusionGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadConclusionGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSuppprimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnSelect, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadGroupBoxEtatCivil As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents Label13 As Label
    Friend WithEvents LblPatientDateMaj As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LblPatientSite As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents LblPatientPrenom As Label
    Friend WithEvents LblPatientNom As Label
    Friend WithEvents LblPatientAge As Label
    Friend WithEvents LblPatientGenre As Label
    Friend WithEvents LblPatientNIR As Label
    Friend WithEvents RadContexteDataGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadChkContexteTous As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents RadChkContextePublie As Telerik.WinControls.UI.RadCheckBox
    Friend WithEvents RadBtnCreation As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents CréerUnContexteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RadConclusionGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents LblDateNaissance As Label
    Friend WithEvents RadBtnSuppprimer As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnSelect As Telerik.WinControls.UI.RadButton
End Class

