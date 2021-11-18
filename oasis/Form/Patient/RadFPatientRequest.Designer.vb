<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RadFPatientRequest
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
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn13 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn14 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn15 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn16 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn17 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn18 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn19 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn20 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadFPatientRequest))
        Me.RadPatientGridView = New Telerik.WinControls.UI.RadGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CréerLesIntervenantsOasisParDéfautToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        Me.LblOccurrenceLue = New System.Windows.Forms.Label()
        Me.RadChkPatientTous = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadChkPatientNonOasis = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadChkPatientOasis = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadPanel2 = New Telerik.WinControls.UI.RadPanel()
        Me.RadButtonAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadPanel4 = New Telerik.WinControls.UI.RadPanel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.RadDesktopAlert1 = New Telerik.WinControls.UI.RadDesktopAlert(Me.components)
        Me.BtnDRC = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadPatientGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPatientGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me.RadChkPatientTous, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadChkPatientNonOasis, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadChkPatientOasis, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel2.SuspendLayout()
        CType(Me.RadButtonAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadPanel4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel4.SuspendLayout()
        CType(Me.BtnDRC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadPatientGridView
        '
        Me.RadPatientGridView.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadPatientGridView.ContextMenuStrip = Me.ContextMenuStrip1
        Me.RadPatientGridView.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadPatientGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPatientGridView.EnableHotTracking = False
        Me.RadPatientGridView.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadPatientGridView.ForeColor = System.Drawing.Color.Black
        Me.RadPatientGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadPatientGridView.Location = New System.Drawing.Point(0, 0)
        '
        '
        '
        Me.RadPatientGridView.MasterTemplate.AllowAddNewRow = False
        Me.RadPatientGridView.MasterTemplate.AllowCellContextMenu = False
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.HeaderText = "Id"
        GridViewTextBoxColumn11.Name = "oa_patient_id"
        GridViewTextBoxColumn11.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn12.EnableExpressionEditor = False
        GridViewTextBoxColumn12.HeaderText = "Prénom"
        GridViewTextBoxColumn12.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn12.Name = "oa_patient_prenom"
        GridViewTextBoxColumn12.Width = 120
        GridViewTextBoxColumn13.EnableExpressionEditor = False
        GridViewTextBoxColumn13.HeaderText = "Nom"
        GridViewTextBoxColumn13.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn13.Name = "oa_patient_nom"
        GridViewTextBoxColumn13.Width = 180
        GridViewTextBoxColumn14.EnableExpressionEditor = False
        GridViewTextBoxColumn14.HeaderText = "NIR"
        GridViewTextBoxColumn14.Name = "oa_patient_nir"
        GridViewTextBoxColumn14.TextAlignment = System.Drawing.ContentAlignment.TopCenter
        GridViewTextBoxColumn14.Width = 110
        GridViewTextBoxColumn15.EnableExpressionEditor = False
        GridViewTextBoxColumn15.HeaderText = "Site"
        GridViewTextBoxColumn15.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn15.Name = "site"
        GridViewTextBoxColumn15.Width = 100
        GridViewTextBoxColumn16.EnableExpressionEditor = False
        GridViewTextBoxColumn16.HeaderText = "Naissance"
        GridViewTextBoxColumn16.Name = "oa_patient_date_naissance"
        GridViewTextBoxColumn16.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn16.Width = 80
        GridViewTextBoxColumn17.EnableExpressionEditor = False
        GridViewTextBoxColumn17.HeaderText = "Age"
        GridViewTextBoxColumn17.Name = "age"
        GridViewTextBoxColumn17.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn17.Width = 85
        GridViewTextBoxColumn18.EnableExpressionEditor = False
        GridViewTextBoxColumn18.HeaderText = "Lieu naissance"
        GridViewTextBoxColumn18.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn18.Name = "oa_patient_lieu_naissance"
        GridViewTextBoxColumn18.Width = 150
        GridViewTextBoxColumn19.EnableExpressionEditor = False
        GridViewTextBoxColumn19.HeaderText = "Date entrée"
        GridViewTextBoxColumn19.Name = "oa_patient_date_entree_oasis"
        GridViewTextBoxColumn19.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn19.Width = 80
        GridViewTextBoxColumn20.EnableExpressionEditor = False
        GridViewTextBoxColumn20.HeaderText = "Sortie"
        GridViewTextBoxColumn20.Name = "oa_patient_date_sortie_oasis"
        GridViewTextBoxColumn20.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn20.Width = 80
        Me.RadPatientGridView.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn11, GridViewTextBoxColumn12, GridViewTextBoxColumn13, GridViewTextBoxColumn14, GridViewTextBoxColumn15, GridViewTextBoxColumn16, GridViewTextBoxColumn17, GridViewTextBoxColumn18, GridViewTextBoxColumn19, GridViewTextBoxColumn20})
        Me.RadPatientGridView.MasterTemplate.EnableFiltering = True
        Me.RadPatientGridView.MasterTemplate.ShowRowHeaderColumn = False
        Me.RadPatientGridView.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadPatientGridView.Name = "RadPatientGridView"
        Me.RadPatientGridView.ReadOnly = True
        Me.RadPatientGridView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadPatientGridView.ShowGroupPanel = False
        Me.RadPatientGridView.Size = New System.Drawing.Size(1456, 478)
        Me.RadPatientGridView.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CréerLesIntervenantsOasisParDéfautToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(276, 26)
        '
        'CréerLesIntervenantsOasisParDéfautToolStripMenuItem
        '
        Me.CréerLesIntervenantsOasisParDéfautToolStripMenuItem.Name = "CréerLesIntervenantsOasisParDéfautToolStripMenuItem"
        Me.CréerLesIntervenantsOasisParDéfautToolStripMenuItem.Size = New System.Drawing.Size(275, 22)
        Me.CréerLesIntervenantsOasisParDéfautToolStripMenuItem.Text = "Créer les intervenants Oasis par défaut"
        '
        'RadPanel1
        '
        Me.RadPanel1.Controls.Add(Me.BtnDRC)
        Me.RadPanel1.Controls.Add(Me.LblOccurrenceLue)
        Me.RadPanel1.Controls.Add(Me.RadChkPatientTous)
        Me.RadPanel1.Controls.Add(Me.RadChkPatientNonOasis)
        Me.RadPanel1.Controls.Add(Me.RadChkPatientOasis)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadPanel1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(1456, 41)
        Me.RadPanel1.TabIndex = 43
        '
        'LblOccurrenceLue
        '
        Me.LblOccurrenceLue.AutoSize = True
        Me.LblOccurrenceLue.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LblOccurrenceLue.ForeColor = System.Drawing.Color.DarkRed
        Me.LblOccurrenceLue.Location = New System.Drawing.Point(360, 13)
        Me.LblOccurrenceLue.Name = "LblOccurrenceLue"
        Me.LblOccurrenceLue.Size = New System.Drawing.Size(102, 13)
        Me.LblOccurrenceLue.TabIndex = 3
        Me.LblOccurrenceLue.Text = "n occurrences lues"
        '
        'RadChkPatientTous
        '
        Me.RadChkPatientTous.Location = New System.Drawing.Point(226, 12)
        Me.RadChkPatientTous.Name = "RadChkPatientTous"
        Me.RadChkPatientTous.Size = New System.Drawing.Size(44, 18)
        Me.RadChkPatientTous.TabIndex = 2
        Me.RadChkPatientTous.Text = "Tous"
        '
        'RadChkPatientNonOasis
        '
        Me.RadChkPatientNonOasis.Location = New System.Drawing.Point(108, 12)
        Me.RadChkPatientNonOasis.Name = "RadChkPatientNonOasis"
        Me.RadChkPatientNonOasis.Size = New System.Drawing.Size(112, 18)
        Me.RadChkPatientNonOasis.TabIndex = 1
        Me.RadChkPatientNonOasis.Text = "Patients non Oasis"
        '
        'RadChkPatientOasis
        '
        Me.RadChkPatientOasis.Location = New System.Drawing.Point(12, 12)
        Me.RadChkPatientOasis.Name = "RadChkPatientOasis"
        Me.RadChkPatientOasis.Size = New System.Drawing.Size(90, 18)
        Me.RadChkPatientOasis.TabIndex = 0
        Me.RadChkPatientOasis.Text = "Patients Oasis"
        '
        'RadPanel2
        '
        Me.RadPanel2.Controls.Add(Me.RadButtonAbandon)
        Me.RadPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadPanel2.Location = New System.Drawing.Point(0, 519)
        Me.RadPanel2.Name = "RadPanel2"
        Me.RadPanel2.Size = New System.Drawing.Size(1456, 44)
        Me.RadPanel2.TabIndex = 44
        '
        'RadButtonAbandon
        '
        Me.RadButtonAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadButtonAbandon.ForeColor = System.Drawing.Color.Black
        Me.RadButtonAbandon.Image = CType(resources.GetObject("RadButtonAbandon.Image"), System.Drawing.Image)
        Me.RadButtonAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadButtonAbandon.Location = New System.Drawing.Point(1413, 8)
        Me.RadButtonAbandon.Name = "RadButtonAbandon"
        Me.RadButtonAbandon.Size = New System.Drawing.Size(31, 24)
        Me.RadButtonAbandon.TabIndex = 43
        '
        'RadPanel4
        '
        Me.RadPanel4.Controls.Add(Me.RadPatientGridView)
        Me.RadPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel4.Location = New System.Drawing.Point(0, 41)
        Me.RadPanel4.Name = "RadPanel4"
        Me.RadPanel4.Size = New System.Drawing.Size(1456, 478)
        Me.RadPanel4.TabIndex = 46
        '
        'RadDesktopAlert1
        '
        Me.RadDesktopAlert1.AutoCloseDelay = 5
        Me.RadDesktopAlert1.FadeAnimationSpeed = 5
        Me.RadDesktopAlert1.Opacity = 0.9!
        Me.RadDesktopAlert1.ScreenPosition = Telerik.WinControls.UI.AlertScreenPosition.TopCenter
        Me.RadDesktopAlert1.ThemeName = ""
        '
        'BtnDRC
        '
        Me.BtnDRC.Location = New System.Drawing.Point(722, 9)
        Me.BtnDRC.Name = "BtnDRC"
        Me.BtnDRC.Size = New System.Drawing.Size(110, 24)
        Me.BtnDRC.TabIndex = 69
        Me.BtnDRC.Text = "Selectionner DRC"
        '
        'RadFPatientRequest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadButtonAbandon
        Me.ClientSize = New System.Drawing.Size(1456, 563)
        Me.Controls.Add(Me.RadPanel4)
        Me.Controls.Add(Me.RadPanel2)
        Me.Controls.Add(Me.RadPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "RadFPatientRequest"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Liste des patients"
        CType(Me.RadPatientGridView.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPatientGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me.RadChkPatientTous, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadChkPatientNonOasis, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadChkPatientOasis, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel2.ResumeLayout(False)
        CType(Me.RadButtonAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadPanel4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel4.ResumeLayout(False)
        CType(Me.BtnDRC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadPatientGridView As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel2 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadPanel4 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadChkPatientTous As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents RadChkPatientNonOasis As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents RadChkPatientOasis As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents RadButtonAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents CréerLesIntervenantsOasisParDéfautToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents RadDesktopAlert1 As Telerik.WinControls.UI.RadDesktopAlert
    Friend WithEvents LblOccurrenceLue As Label
    Friend WithEvents BtnDRC As Telerik.WinControls.UI.RadButton
End Class

