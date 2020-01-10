<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFEpisodeEnCoursListe
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
        Dim GridViewTextBoxColumn9 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn10 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn11 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn12 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition1 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewEpisode = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGridViewEpisode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewEpisode.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGridViewEpisode
        '
        Me.RadGridViewEpisode.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadGridViewEpisode.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadGridViewEpisode.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.RadGridViewEpisode.ForeColor = System.Drawing.Color.Black
        Me.RadGridViewEpisode.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadGridViewEpisode.Location = New System.Drawing.Point(12, 36)
        '
        '
        '
        Me.RadGridViewEpisode.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewEpisode.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewEpisode.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn1.EnableExpressionEditor = False
        GridViewTextBoxColumn1.HeaderText = "Patient"
        GridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn1.Name = "patient"
        GridViewTextBoxColumn1.Width = 200
        GridViewTextBoxColumn2.EnableExpressionEditor = False
        GridViewTextBoxColumn2.HeaderText = "Site"
        GridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn2.Name = "site"
        GridViewTextBoxColumn2.Width = 100
        GridViewTextBoxColumn3.EnableExpressionEditor = False
        GridViewTextBoxColumn3.HeaderText = "Type"
        GridViewTextBoxColumn3.Name = "type"
        GridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn3.Width = 100
        GridViewTextBoxColumn4.EnableExpressionEditor = False
        GridViewTextBoxColumn4.HeaderText = "Activité"
        GridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn4.Name = "type_activite"
        GridViewTextBoxColumn4.Width = 180
        GridViewTextBoxColumn5.EnableExpressionEditor = False
        GridViewTextBoxColumn5.HeaderText = "Profil"
        GridViewTextBoxColumn5.Name = "type_profil"
        GridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn5.Width = 100
        GridViewTextBoxColumn6.EnableExpressionEditor = False
        GridViewTextBoxColumn6.HeaderText = "commentaire"
        GridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn6.Name = "commentaire"
        GridViewTextBoxColumn6.Width = 250
        GridViewTextBoxColumn7.EnableExpressionEditor = False
        GridViewTextBoxColumn7.HeaderText = "Création le"
        GridViewTextBoxColumn7.Name = "dateCreation"
        GridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn7.Width = 80
        GridViewTextBoxColumn8.EnableExpressionEditor = False
        GridViewTextBoxColumn8.HeaderText = "à"
        GridViewTextBoxColumn8.Name = "heureCreation"
        GridViewTextBoxColumn8.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn9.EnableExpressionEditor = False
        GridViewTextBoxColumn9.HeaderText = "par"
        GridViewTextBoxColumn9.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn9.Name = "utilisateur"
        GridViewTextBoxColumn9.Width = 200
        GridViewTextBoxColumn10.EnableExpressionEditor = False
        GridViewTextBoxColumn10.HeaderText = "episode_id"
        GridViewTextBoxColumn10.IsVisible = False
        GridViewTextBoxColumn10.Name = "episode_id"
        GridViewTextBoxColumn11.EnableExpressionEditor = False
        GridViewTextBoxColumn11.HeaderText = "patient_id"
        GridViewTextBoxColumn11.IsVisible = False
        GridViewTextBoxColumn11.Name = "patient_id"
        GridViewTextBoxColumn12.EnableExpressionEditor = False
        GridViewTextBoxColumn12.HeaderText = "Durée (heures)"
        GridViewTextBoxColumn12.Name = "duree"
        GridViewTextBoxColumn12.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn12.Width = 130
        Me.RadGridViewEpisode.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn1, GridViewTextBoxColumn2, GridViewTextBoxColumn3, GridViewTextBoxColumn4, GridViewTextBoxColumn5, GridViewTextBoxColumn6, GridViewTextBoxColumn7, GridViewTextBoxColumn8, GridViewTextBoxColumn9, GridViewTextBoxColumn10, GridViewTextBoxColumn11, GridViewTextBoxColumn12})
        Me.RadGridViewEpisode.MasterTemplate.ViewDefinition = TableViewDefinition1
        Me.RadGridViewEpisode.Name = "RadGridViewEpisode"
        Me.RadGridViewEpisode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewEpisode.ShowGroupPanel = False
        Me.RadGridViewEpisode.Size = New System.Drawing.Size(1423, 414)
        Me.RadGridViewEpisode.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1325, 456)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 1
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadFEpisodeEnCoursListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1447, 486)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Controls.Add(Me.RadGridViewEpisode)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RadFEpisodeEnCoursListe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Liste des épisode en cours"
        CType(Me.RadGridViewEpisode.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewEpisode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGridViewEpisode As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
End Class

