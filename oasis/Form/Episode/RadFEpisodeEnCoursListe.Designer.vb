<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RadFEpisodeEnCoursListe
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
        Dim GridViewTextBoxColumn49 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn50 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn51 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn52 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn53 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn54 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn55 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn56 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn57 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn58 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn59 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn60 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn61 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn4 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn62 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn63 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn64 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition4 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadFEpisodeEnCoursListe))
        Me.RadGridViewEpisode = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnRefresh = New Telerik.WinControls.UI.RadButton()
        CType(Me.RadGridViewEpisode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridViewEpisode.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnRefresh, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.RadGridViewEpisode.Location = New System.Drawing.Point(6, 36)
        '
        '
        '
        Me.RadGridViewEpisode.MasterTemplate.AllowAddNewRow = False
        Me.RadGridViewEpisode.MasterTemplate.AllowDeleteRow = False
        Me.RadGridViewEpisode.MasterTemplate.AllowEditRow = False
        GridViewTextBoxColumn49.EnableExpressionEditor = False
        GridViewTextBoxColumn49.HeaderText = "Patient"
        GridViewTextBoxColumn49.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn49.Name = "patient"
        GridViewTextBoxColumn49.Width = 200
        GridViewTextBoxColumn50.EnableExpressionEditor = False
        GridViewTextBoxColumn50.HeaderText = "Date naissance"
        GridViewTextBoxColumn50.Name = "dateNaissance"
        GridViewTextBoxColumn50.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn50.Width = 100
        GridViewTextBoxColumn51.EnableExpressionEditor = False
        GridViewTextBoxColumn51.HeaderText = "Site"
        GridViewTextBoxColumn51.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn51.Name = "site"
        GridViewTextBoxColumn51.Width = 100
        GridViewTextBoxColumn52.EnableExpressionEditor = False
        GridViewTextBoxColumn52.HeaderText = "Type"
        GridViewTextBoxColumn52.IsVisible = False
        GridViewTextBoxColumn52.Name = "type"
        GridViewTextBoxColumn52.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn52.Width = 100
        GridViewTextBoxColumn53.EnableExpressionEditor = False
        GridViewTextBoxColumn53.HeaderText = "Activité"
        GridViewTextBoxColumn53.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn53.Name = "type_activite"
        GridViewTextBoxColumn53.Width = 180
        GridViewTextBoxColumn54.EnableExpressionEditor = False
        GridViewTextBoxColumn54.HeaderText = "Profil"
        GridViewTextBoxColumn54.IsVisible = False
        GridViewTextBoxColumn54.Name = "type_profil"
        GridViewTextBoxColumn54.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn54.Width = 100
        GridViewTextBoxColumn55.EnableExpressionEditor = False
        GridViewTextBoxColumn55.HeaderText = "commentaire"
        GridViewTextBoxColumn55.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn55.IsVisible = False
        GridViewTextBoxColumn55.Name = "commentaire"
        GridViewTextBoxColumn55.Width = 250
        GridViewTextBoxColumn56.EnableExpressionEditor = False
        GridViewTextBoxColumn56.HeaderText = "Création le"
        GridViewTextBoxColumn56.Name = "dateCreation"
        GridViewTextBoxColumn56.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn56.Width = 80
        GridViewTextBoxColumn57.EnableExpressionEditor = False
        GridViewTextBoxColumn57.HeaderText = "à"
        GridViewTextBoxColumn57.Name = "heureCreation"
        GridViewTextBoxColumn57.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn58.EnableExpressionEditor = False
        GridViewTextBoxColumn58.HeaderText = "par"
        GridViewTextBoxColumn58.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn58.IsVisible = False
        GridViewTextBoxColumn58.Name = "utilisateur"
        GridViewTextBoxColumn58.Width = 200
        GridViewTextBoxColumn59.EnableExpressionEditor = False
        GridViewTextBoxColumn59.HeaderText = "episode_id"
        GridViewTextBoxColumn59.IsVisible = False
        GridViewTextBoxColumn59.Name = "episode_id"
        GridViewTextBoxColumn60.EnableExpressionEditor = False
        GridViewTextBoxColumn60.HeaderText = "patient_id"
        GridViewTextBoxColumn60.IsVisible = False
        GridViewTextBoxColumn60.Name = "patient_id"
        GridViewTextBoxColumn61.EnableExpressionEditor = False
        GridViewTextBoxColumn61.HeaderText = "Durée (heures)"
        GridViewTextBoxColumn61.IsVisible = False
        GridViewTextBoxColumn61.Name = "duree"
        GridViewTextBoxColumn61.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn61.Width = 130
        GridViewCheckBoxColumn4.EnableExpressionEditor = False
        GridViewCheckBoxColumn4.HeaderText = "Workflow"
        GridViewCheckBoxColumn4.MinWidth = 20
        GridViewCheckBoxColumn4.Name = "workflow"
        GridViewCheckBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewCheckBoxColumn4.Width = 70
        GridViewTextBoxColumn62.EnableExpressionEditor = False
        GridViewTextBoxColumn62.HeaderText = "Destinataire"
        GridViewTextBoxColumn62.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn62.Name = "workflowFonctionDestinataire"
        GridViewTextBoxColumn62.Width = 200
        GridViewTextBoxColumn63.EnableExpressionEditor = False
        GridViewTextBoxColumn63.HeaderText = "Etat"
        GridViewTextBoxColumn63.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn63.Name = "workflowEtat"
        GridViewTextBoxColumn63.Width = 150
        GridViewTextBoxColumn64.EnableExpressionEditor = False
        GridViewTextBoxColumn64.HeaderText = "Attribution"
        GridViewTextBoxColumn64.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn64.Name = "workflowAttribution"
        GridViewTextBoxColumn64.Width = 220
        Me.RadGridViewEpisode.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn49, GridViewTextBoxColumn50, GridViewTextBoxColumn51, GridViewTextBoxColumn52, GridViewTextBoxColumn53, GridViewTextBoxColumn54, GridViewTextBoxColumn55, GridViewTextBoxColumn56, GridViewTextBoxColumn57, GridViewTextBoxColumn58, GridViewTextBoxColumn59, GridViewTextBoxColumn60, GridViewTextBoxColumn61, GridViewCheckBoxColumn4, GridViewTextBoxColumn62, GridViewTextBoxColumn63, GridViewTextBoxColumn64})
        Me.RadGridViewEpisode.MasterTemplate.ViewDefinition = TableViewDefinition4
        Me.RadGridViewEpisode.Name = "RadGridViewEpisode"
        Me.RadGridViewEpisode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewEpisode.ShowGroupPanel = False
        Me.RadGridViewEpisode.Size = New System.Drawing.Size(1382, 414)
        Me.RadGridViewEpisode.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1278, 456)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 1
        Me.RadBtnAbandon.Text = "Abandon"
        '
        'RadBtnRefresh
        '
        Me.RadBtnRefresh.Image = CType(resources.GetObject("RadBtnRefresh.Image"), System.Drawing.Image)
        Me.RadBtnRefresh.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnRefresh.Location = New System.Drawing.Point(1206, 456)
        Me.RadBtnRefresh.Name = "RadBtnRefresh"
        Me.RadBtnRefresh.Size = New System.Drawing.Size(27, 24)
        Me.RadBtnRefresh.TabIndex = 42
        '
        'RadFEpisodeEnCoursListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1395, 486)
        Me.Controls.Add(Me.RadBtnRefresh)
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
        CType(Me.RadBtnRefresh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGridViewEpisode As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnRefresh As Telerik.WinControls.UI.RadButton
End Class

