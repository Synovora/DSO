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
        Dim GridViewTextBoxColumn55 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn56 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn57 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn58 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn59 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn60 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn61 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn62 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn63 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn64 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn65 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn66 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn67 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewCheckBoxColumn4 As Telerik.WinControls.UI.GridViewCheckBoxColumn = New Telerik.WinControls.UI.GridViewCheckBoxColumn()
        Dim GridViewTextBoxColumn68 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn69 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn70 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn71 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim GridViewTextBoxColumn72 As Telerik.WinControls.UI.GridViewTextBoxColumn = New Telerik.WinControls.UI.GridViewTextBoxColumn()
        Dim TableViewDefinition4 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadGridViewEpisode = New Telerik.WinControls.UI.RadGridView()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnRefresh = New Telerik.WinControls.UI.RadButton()
        Me.RadioBtnTous = New System.Windows.Forms.RadioButton()
        Me.RadioBtnWorkflowEnAttente = New System.Windows.Forms.RadioButton()
        Me.RadioBtnWorkflowPourSaFonction = New System.Windows.Forms.RadioButton()
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
        GridViewTextBoxColumn55.EnableExpressionEditor = False
        GridViewTextBoxColumn55.HeaderText = "Patient"
        GridViewTextBoxColumn55.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn55.Name = "patient"
        GridViewTextBoxColumn55.Width = 200
        GridViewTextBoxColumn56.EnableExpressionEditor = False
        GridViewTextBoxColumn56.HeaderText = "Date naissance"
        GridViewTextBoxColumn56.Name = "dateNaissance"
        GridViewTextBoxColumn56.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn56.Width = 100
        GridViewTextBoxColumn57.EnableExpressionEditor = False
        GridViewTextBoxColumn57.HeaderText = "Site"
        GridViewTextBoxColumn57.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn57.Name = "site"
        GridViewTextBoxColumn57.Width = 100
        GridViewTextBoxColumn58.EnableExpressionEditor = False
        GridViewTextBoxColumn58.HeaderText = "Type"
        GridViewTextBoxColumn58.IsVisible = False
        GridViewTextBoxColumn58.Name = "type"
        GridViewTextBoxColumn58.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn58.Width = 100
        GridViewTextBoxColumn59.EnableExpressionEditor = False
        GridViewTextBoxColumn59.HeaderText = "Activité"
        GridViewTextBoxColumn59.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn59.Name = "type_activite"
        GridViewTextBoxColumn59.Width = 180
        GridViewTextBoxColumn60.EnableExpressionEditor = False
        GridViewTextBoxColumn60.HeaderText = "Profil"
        GridViewTextBoxColumn60.IsVisible = False
        GridViewTextBoxColumn60.Name = "type_profil"
        GridViewTextBoxColumn60.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn60.Width = 100
        GridViewTextBoxColumn61.EnableExpressionEditor = False
        GridViewTextBoxColumn61.HeaderText = "commentaire"
        GridViewTextBoxColumn61.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn61.IsVisible = False
        GridViewTextBoxColumn61.Name = "commentaire"
        GridViewTextBoxColumn61.Width = 250
        GridViewTextBoxColumn62.EnableExpressionEditor = False
        GridViewTextBoxColumn62.HeaderText = "Création le"
        GridViewTextBoxColumn62.Name = "dateCreation"
        GridViewTextBoxColumn62.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn62.Width = 80
        GridViewTextBoxColumn63.EnableExpressionEditor = False
        GridViewTextBoxColumn63.HeaderText = "à"
        GridViewTextBoxColumn63.Name = "heureCreation"
        GridViewTextBoxColumn63.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn64.EnableExpressionEditor = False
        GridViewTextBoxColumn64.HeaderText = "par"
        GridViewTextBoxColumn64.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn64.IsVisible = False
        GridViewTextBoxColumn64.Name = "utilisateur"
        GridViewTextBoxColumn64.Width = 200
        GridViewTextBoxColumn65.EnableExpressionEditor = False
        GridViewTextBoxColumn65.HeaderText = "episode_id"
        GridViewTextBoxColumn65.IsVisible = False
        GridViewTextBoxColumn65.Name = "episode_id"
        GridViewTextBoxColumn66.EnableExpressionEditor = False
        GridViewTextBoxColumn66.HeaderText = "patient_id"
        GridViewTextBoxColumn66.IsVisible = False
        GridViewTextBoxColumn66.Name = "patient_id"
        GridViewTextBoxColumn67.EnableExpressionEditor = False
        GridViewTextBoxColumn67.HeaderText = "Durée (heures)"
        GridViewTextBoxColumn67.IsVisible = False
        GridViewTextBoxColumn67.Name = "duree"
        GridViewTextBoxColumn67.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn67.Width = 130
        GridViewCheckBoxColumn4.EnableExpressionEditor = False
        GridViewCheckBoxColumn4.HeaderText = "Workflow"
        GridViewCheckBoxColumn4.MinWidth = 20
        GridViewCheckBoxColumn4.Name = "workflow"
        GridViewCheckBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewCheckBoxColumn4.Width = 70
        GridViewTextBoxColumn68.EnableExpressionEditor = False
        GridViewTextBoxColumn68.HeaderText = "Destinataire"
        GridViewTextBoxColumn68.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn68.Name = "workflowFonctionDestinataire"
        GridViewTextBoxColumn68.Width = 200
        GridViewTextBoxColumn69.EnableExpressionEditor = False
        GridViewTextBoxColumn69.HeaderText = "Etat"
        GridViewTextBoxColumn69.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn69.Name = "workflowEtat"
        GridViewTextBoxColumn69.Width = 150
        GridViewTextBoxColumn70.EnableExpressionEditor = False
        GridViewTextBoxColumn70.HeaderText = "Attribution"
        GridViewTextBoxColumn70.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn70.IsVisible = False
        GridViewTextBoxColumn70.Name = "workflowAttribution"
        GridViewTextBoxColumn70.Width = 220
        GridViewTextBoxColumn71.EnableExpressionEditor = False
        GridViewTextBoxColumn71.HeaderText = "Commentaire"
        GridViewTextBoxColumn71.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        GridViewTextBoxColumn71.Name = "workflowCommentaire"
        GridViewTextBoxColumn71.Width = 250
        GridViewTextBoxColumn72.EnableExpressionEditor = False
        GridViewTextBoxColumn72.HeaderText = "Priorité"
        GridViewTextBoxColumn72.Name = "workflowPriorite"
        GridViewTextBoxColumn72.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        GridViewTextBoxColumn72.Width = 70
        Me.RadGridViewEpisode.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() {GridViewTextBoxColumn55, GridViewTextBoxColumn56, GridViewTextBoxColumn57, GridViewTextBoxColumn58, GridViewTextBoxColumn59, GridViewTextBoxColumn60, GridViewTextBoxColumn61, GridViewTextBoxColumn62, GridViewTextBoxColumn63, GridViewTextBoxColumn64, GridViewTextBoxColumn65, GridViewTextBoxColumn66, GridViewTextBoxColumn67, GridViewCheckBoxColumn4, GridViewTextBoxColumn68, GridViewTextBoxColumn69, GridViewTextBoxColumn70, GridViewTextBoxColumn71, GridViewTextBoxColumn72})
        Me.RadGridViewEpisode.MasterTemplate.ViewDefinition = TableViewDefinition4
        Me.RadGridViewEpisode.Name = "RadGridViewEpisode"
        Me.RadGridViewEpisode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadGridViewEpisode.ShowGroupPanel = False
        Me.RadGridViewEpisode.Size = New System.Drawing.Size(1482, 414)
        Me.RadGridViewEpisode.TabIndex = 0
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandon.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1464, 456)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandon.TabIndex = 1
        '
        'RadBtnRefresh
        '
        Me.RadBtnRefresh.Image = Global.Oasis_WF.My.Resources.Resources.reload
        Me.RadBtnRefresh.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.RadBtnRefresh.Location = New System.Drawing.Point(1434, 456)
        Me.RadBtnRefresh.Name = "RadBtnRefresh"
        Me.RadBtnRefresh.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnRefresh.TabIndex = 42
        '
        'RadioBtnTous
        '
        Me.RadioBtnTous.AutoSize = True
        Me.RadioBtnTous.Location = New System.Drawing.Point(6, 12)
        Me.RadioBtnTous.Name = "RadioBtnTous"
        Me.RadioBtnTous.Size = New System.Drawing.Size(48, 17)
        Me.RadioBtnTous.TabIndex = 43
        Me.RadioBtnTous.TabStop = True
        Me.RadioBtnTous.Text = "Tous"
        Me.RadioBtnTous.UseVisualStyleBackColor = True
        '
        'RadioBtnWorkflowEnAttente
        '
        Me.RadioBtnWorkflowEnAttente.AutoSize = True
        Me.RadioBtnWorkflowEnAttente.Location = New System.Drawing.Point(60, 12)
        Me.RadioBtnWorkflowEnAttente.Name = "RadioBtnWorkflowEnAttente"
        Me.RadioBtnWorkflowEnAttente.Size = New System.Drawing.Size(132, 17)
        Me.RadioBtnWorkflowEnAttente.TabIndex = 44
        Me.RadioBtnWorkflowEnAttente.TabStop = True
        Me.RadioBtnWorkflowEnAttente.Text = "Workflow en attente"
        Me.RadioBtnWorkflowEnAttente.UseVisualStyleBackColor = True
        '
        'RadioBtnWorkflowPourSaFonction
        '
        Me.RadioBtnWorkflowPourSaFonction.AutoSize = True
        Me.RadioBtnWorkflowPourSaFonction.Location = New System.Drawing.Point(198, 12)
        Me.RadioBtnWorkflowPourSaFonction.Name = "RadioBtnWorkflowPourSaFonction"
        Me.RadioBtnWorkflowPourSaFonction.Size = New System.Drawing.Size(221, 17)
        Me.RadioBtnWorkflowPourSaFonction.TabIndex = 45
        Me.RadioBtnWorkflowPourSaFonction.TabStop = True
        Me.RadioBtnWorkflowPourSaFonction.Text = "Workflow en attente pour sa fonction"
        Me.RadioBtnWorkflowPourSaFonction.UseVisualStyleBackColor = True
        '
        'RadFEpisodeEnCoursListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1492, 486)
        Me.Controls.Add(Me.RadioBtnWorkflowPourSaFonction)
        Me.Controls.Add(Me.RadioBtnWorkflowEnAttente)
        Me.Controls.Add(Me.RadioBtnTous)
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
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Liste des épisode en cours"
        CType(Me.RadGridViewEpisode.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridViewEpisode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnRefresh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadGridViewEpisode As Telerik.WinControls.UI.RadGridView
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnRefresh As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadioBtnTous As RadioButton
    Friend WithEvents RadioBtnWorkflowEnAttente As RadioButton
    Friend WithEvents RadioBtnWorkflowPourSaFonction As RadioButton
End Class

