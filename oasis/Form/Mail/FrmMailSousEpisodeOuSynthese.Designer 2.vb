<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMailSousEpisodeOuSynthese
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
        Dim GridViewTextBoxColumn1 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn2 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn3 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn4 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim SortDescriptor1 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim SortDescriptor2 As Telerik.WinControls.Data.SortDescriptor = New Telerik.WinControls.Data.SortDescriptor()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMailSousEpisodeOuSynthese))
        Me.TxtObjet = New System.Windows.Forms.TextBox()
        Me.LblObjet = New System.Windows.Forms.Label()
        Me.TxtTo = New System.Windows.Forms.TextBox()
        Me.LblIdentifiant = New System.Windows.Forms.Label()
        Me.RadGroupIdentite = New Telerik.WinControls.UI.RadGroupBox()
        Me.RbParcours = New Telerik.WinControls.UI.RadRadioButton()
        Me.rbPatient = New Telerik.WinControls.UI.RadRadioButton()
        Me.RadParcoursGrid = New Telerik.WinControls.UI.RadGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        Me.BtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.BtnValider = New System.Windows.Forms.Button()
        Me.PnlBoutons = New Telerik.WinControls.UI.RadPanel()
        Me.TxtBody = New System.Windows.Forms.TextBox()
        Me.RadPanel1 = New Telerik.WinControls.UI.RadPanel()
        CType(Me.RadGroupIdentite, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupIdentite.SuspendLayout()
        CType(Me.RbParcours, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbPatient, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadParcoursGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadParcoursGrid.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlBoutons.SuspendLayout()
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanel1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtObjet
        '
        Me.TxtObjet.BackColor = System.Drawing.SystemColors.Window
        Me.TxtObjet.Location = New System.Drawing.Point(118, 314)
        Me.TxtObjet.Name = "TxtObjet"
        Me.TxtObjet.Size = New System.Drawing.Size(820, 23)
        Me.TxtObjet.TabIndex = 3
        '
        'LblObjet
        '
        Me.LblObjet.AutoSize = True
        Me.LblObjet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblObjet.Location = New System.Drawing.Point(10, 318)
        Me.LblObjet.Name = "LblObjet"
        Me.LblObjet.Size = New System.Drawing.Size(37, 13)
        Me.LblObjet.TabIndex = 2
        Me.LblObjet.Text = "Objet"
        '
        'TxtTo
        '
        Me.TxtTo.Location = New System.Drawing.Point(118, 283)
        Me.TxtTo.Name = "TxtTo"
        Me.TxtTo.Size = New System.Drawing.Size(444, 23)
        Me.TxtTo.TabIndex = 1
        '
        'LblIdentifiant
        '
        Me.LblIdentifiant.AutoSize = True
        Me.LblIdentifiant.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblIdentifiant.Location = New System.Drawing.Point(10, 287)
        Me.LblIdentifiant.Name = "LblIdentifiant"
        Me.LblIdentifiant.Size = New System.Drawing.Size(75, 13)
        Me.LblIdentifiant.TabIndex = 0
        Me.LblIdentifiant.Text = "Destinataire"
        '
        'RadGroupIdentite
        '
        Me.RadGroupIdentite.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupIdentite.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupIdentite.Controls.Add(Me.RbParcours)
        Me.RadGroupIdentite.Controls.Add(Me.rbPatient)
        Me.RadGroupIdentite.Controls.Add(Me.RadParcoursGrid)
        Me.RadGroupIdentite.Controls.Add(Me.Label1)
        Me.RadGroupIdentite.Controls.Add(Me.TxtObjet)
        Me.RadGroupIdentite.Controls.Add(Me.LblObjet)
        Me.RadGroupIdentite.Controls.Add(Me.TxtTo)
        Me.RadGroupIdentite.Controls.Add(Me.LblIdentifiant)
        Me.RadGroupIdentite.Controls.Add(Me.RadLabel1)
        Me.RadGroupIdentite.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadGroupIdentite.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold)
        Me.RadGroupIdentite.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        Me.RadGroupIdentite.HeaderText = ""
        Me.RadGroupIdentite.Location = New System.Drawing.Point(0, 0)
        Me.RadGroupIdentite.Name = "RadGroupIdentite"
        Me.RadGroupIdentite.Size = New System.Drawing.Size(939, 371)
        Me.RadGroupIdentite.TabIndex = 10
        CType(Me.RadGroupIdentite.GetChildAt(0), Telerik.WinControls.UI.RadGroupBoxElement).Padding = New System.Windows.Forms.Padding(2, 18, 2, 2)
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.SystemColors.ActiveCaption
        CType(Me.RadGroupIdentite.GetChildAt(0).GetChildAt(1).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        '
        'RbParcours
        '
        Me.RbParcours.Location = New System.Drawing.Point(118, 109)
        Me.RbParcours.Name = "RbParcours"
        Me.RbParcours.Size = New System.Drawing.Size(103, 18)
        Me.RbParcours.TabIndex = 12
        Me.RbParcours.TabStop = False
        Me.RbParcours.Text = "Parcours de soin"
        '
        'rbPatient
        '
        Me.rbPatient.Location = New System.Drawing.Point(118, 244)
        Me.rbPatient.Name = "rbPatient"
        Me.rbPatient.Size = New System.Drawing.Size(55, 18)
        Me.rbPatient.TabIndex = 11
        Me.rbPatient.TabStop = False
        Me.rbPatient.Text = "Patient"
        '
        'RadParcoursGrid
        '
        Me.RadParcoursGrid.AutoSizeRows = True
        Me.RadParcoursGrid.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RadParcoursGrid.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadParcoursGrid.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadParcoursGrid.ForeColor = System.Drawing.Color.Black
        Me.RadParcoursGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadParcoursGrid.Location = New System.Drawing.Point(233, 21)
        '
        '
        '
        Me.RadParcoursGrid.MasterTemplate.AllowAddNewRow = False
        Me.RadParcoursGrid.MasterTemplate.AllowCellContextMenu = False
        Me.RadParcoursGrid.MasterTemplate.AllowDeleteRow = False
        Me.RadParcoursGrid.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.FieldName = "oa_r_specialite_description"
        GridViewTextBoxColumn1.HeaderText = "Spécialité"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.Name = "specialite"
        GridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending
        GridViewTextBoxColumn1.Width = 200
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.FieldName = "oa_ror_nom"
        GridViewTextBoxColumn2.HeaderText = "Nom"
        GridViewTextBoxColumn2.Name = "nomIntervenant"
        GridViewTextBoxColumn2.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending
        GridViewTextBoxColumn2.Width = 200
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.FieldName = "email"
        GridViewTextBoxColumn3.HeaderText = "Emails"
        GridViewTextBoxColumn3.Name = "email"
        GridViewTextBoxColumn3.Width = 305
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.FieldName = "oa_ror_id"
        GridViewTextBoxColumn4.HeaderText = "id_ror"
        GridViewTextBoxColumn4.IsVisible = False
        GridViewTextBoxColumn4.Name = "idRor"
        Me.RadParcoursGrid.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4})
        Me.RadParcoursGrid.MasterTemplate.ShowRowHeaderColumn = False
        SortDescriptor1.PropertyName = "specialite"
        SortDescriptor2.PropertyName = "nomIntervenant"
        Me.RadParcoursGrid.MasterTemplate.SortDescriptors.AddRange(New Telerik.WinControls.Data.SortDescriptor() {SortDescriptor1, SortDescriptor2})
        Me.RadParcoursGrid.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadParcoursGrid.Name = "RadParcoursGrid"
        Me.RadParcoursGrid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadParcoursGrid.ShowGroupPanel = False
        Me.RadParcoursGrid.Size = New System.Drawing.Size(704, 213)
        Me.RadParcoursGrid.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 112)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Pré-choix"
        '
        'RadLabel1
        '
        Me.RadLabel1.AutoSize = False
        Me.RadLabel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadLabel1.Location = New System.Drawing.Point(2, 351)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(935, 18)
        Me.RadLabel1.TabIndex = 9
        Me.RadLabel1.Text = "Corps du message"
        Me.RadLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnAbandon
        '
        Me.BtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnAbandon.ForeColor = System.Drawing.Color.Black
        Me.BtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.BtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.BtnAbandon.Location = New System.Drawing.Point(715, 8)
        Me.BtnAbandon.Name = "BtnAbandon"
        Me.BtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.BtnAbandon.TabIndex = 1
        '
        'BtnValider
        '
        Me.BtnValider.ForeColor = System.Drawing.Color.Red
        Me.BtnValider.Location = New System.Drawing.Point(83, 8)
        Me.BtnValider.Name = "BtnValider"
        Me.BtnValider.Size = New System.Drawing.Size(216, 24)
        Me.BtnValider.TabIndex = 0
        Me.BtnValider.Text = "Valider"
        Me.BtnValider.UseVisualStyleBackColor = True
        '
        'PnlBoutons
        '
        Me.PnlBoutons.Controls.Add(Me.BtnAbandon)
        Me.PnlBoutons.Controls.Add(Me.BtnValider)
        Me.PnlBoutons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PnlBoutons.Location = New System.Drawing.Point(0, 638)
        Me.PnlBoutons.Name = "PnlBoutons"
        Me.PnlBoutons.Size = New System.Drawing.Size(939, 40)
        Me.PnlBoutons.TabIndex = 11
        '
        'TxtBody
        '
        Me.TxtBody.BackColor = System.Drawing.SystemColors.Window
        Me.TxtBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TxtBody.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.TxtBody.Location = New System.Drawing.Point(0, 0)
        Me.TxtBody.Multiline = True
        Me.TxtBody.Name = "TxtBody"
        Me.TxtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TxtBody.Size = New System.Drawing.Size(939, 267)
        Me.TxtBody.TabIndex = 8
        '
        'RadPanel1
        '
        Me.RadPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadPanel1.Controls.Add(Me.TxtBody)
        Me.RadPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadPanel1.Location = New System.Drawing.Point(0, 371)
        Me.RadPanel1.Name = "RadPanel1"
        Me.RadPanel1.Size = New System.Drawing.Size(939, 267)
        Me.RadPanel1.TabIndex = 12
        Me.RadPanel1.TextAlignment = System.Drawing.ContentAlignment.TopCenter
        '
        'FrmMailSousEpisodeOuSynthese
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnAbandon
        Me.ClientSize = New System.Drawing.Size(939, 678)
        Me.Controls.Add(Me.RadPanel1)
        Me.Controls.Add(Me.RadGroupIdentite)
        Me.Controls.Add(Me.PnlBoutons)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "FrmMailSousEpisodeOuSynthese"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Envoi de mail "
        CType(Me.RadGroupIdentite, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupIdentite.ResumeLayout(False)
        Me.RadGroupIdentite.PerformLayout()
        CType(Me.RbParcours, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbPatient, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadParcoursGrid.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadParcoursGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PnlBoutons, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlBoutons.ResumeLayout(False)
        CType(Me.RadPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanel1.ResumeLayout(False)
        Me.RadPanel1.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TxtObjet As TextBox
    Friend WithEvents LblObjet As Label
    Friend WithEvents TxtTo As TextBox
    Friend WithEvents LblIdentifiant As Label
    Friend WithEvents RadGroupIdentite As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
    Friend WithEvents BtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents BtnValider As Button
    Friend WithEvents PnlBoutons As Telerik.WinControls.UI.RadPanel
    Friend WithEvents TxtBody As TextBox
    Friend WithEvents RadPanel1 As Telerik.WinControls.UI.RadPanel
    Friend WithEvents RadParcoursGrid As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RbParcours As Telerik.WinControls.UI.RadRadioButton
    Friend WithEvents rbPatient As Telerik.WinControls.UI.RadRadioButton
End Class

