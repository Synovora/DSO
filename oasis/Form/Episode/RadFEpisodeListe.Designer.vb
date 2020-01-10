<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFEpisodeListe
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
        Dim TableViewDefinition2 As Telerik.WinControls.UI.TableViewDefinition = New Telerik.WinControls.UI.TableViewDefinition()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.RadBtnEpisode = New Telerik.WinControls.UI.RadButton()
        Me.RadGroupBoxEtatCivil = New Telerik.WinControls.UI.RadGroupBox()
        Me.LblALD = New System.Windows.Forms.Label()
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
        Me.RadGridView1 = New Telerik.WinControls.UI.RadGridView()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadBtnEpisode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBoxEtatCivil.SuspendLayout()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(907, 524)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 0
        Me.RadBtnAbandon.Text = "Abandonner"
        '
        'RadBtnEpisode
        '
        Me.RadBtnEpisode.Location = New System.Drawing.Point(791, 524)
        Me.RadBtnEpisode.Name = "RadBtnEpisode"
        Me.RadBtnEpisode.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnEpisode.TabIndex = 1
        Me.RadBtnEpisode.Text = "Détail"
        '
        'RadGroupBoxEtatCivil
        '
        Me.RadGroupBoxEtatCivil.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBoxEtatCivil.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.RadGroupBoxEtatCivil.Controls.Add(Me.LblALD)
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
        Me.RadGroupBoxEtatCivil.HeaderText = ""
        Me.RadGroupBoxEtatCivil.Location = New System.Drawing.Point(12, 12)
        Me.RadGroupBoxEtatCivil.Name = "RadGroupBoxEtatCivil"
        Me.RadGroupBoxEtatCivil.Size = New System.Drawing.Size(1000, 38)
        Me.RadGroupBoxEtatCivil.TabIndex = 2
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.GroupBoxContent).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office
        CType(Me.RadGroupBoxEtatCivil.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.GroupBoxHeader).Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        '
        'LblALD
        '
        Me.LblALD.AutoSize = True
        Me.LblALD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblALD.ForeColor = System.Drawing.Color.OrangeRed
        Me.LblALD.Location = New System.Drawing.Point(947, 4)
        Me.LblALD.Name = "LblALD"
        Me.LblALD.Size = New System.Drawing.Size(31, 13)
        Me.LblALD.TabIndex = 43
        Me.LblALD.Text = "ALD"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(510, 4)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 42
        Me.Label13.Text = "NIR :"
        '
        'LblPatientDateMaj
        '
        Me.LblPatientDateMaj.AutoSize = True
        Me.LblPatientDateMaj.Location = New System.Drawing.Point(801, 4)
        Me.LblPatientDateMaj.Name = "LblPatientDateMaj"
        Me.LblPatientDateMaj.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientDateMaj.TabIndex = 41
        Me.LblPatientDateMaj.Text = "23-05-2019"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(667, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Dernière mise à jour :"
        '
        'LblPatientSite
        '
        Me.LblPatientSite.AutoSize = True
        Me.LblPatientSite.Location = New System.Drawing.Point(710, 21)
        Me.LblPatientSite.Name = "LblPatientSite"
        Me.LblPatientSite.Size = New System.Drawing.Size(36, 13)
        Me.LblPatientSite.TabIndex = 37
        Me.LblPatientSite.Text = "Cluny"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(667, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Site :"
        '
        'LblPatientPrenom
        '
        Me.LblPatientPrenom.AutoSize = True
        Me.LblPatientPrenom.Location = New System.Drawing.Point(11, 4)
        Me.LblPatientPrenom.Name = "LblPatientPrenom"
        Me.LblPatientPrenom.Size = New System.Drawing.Size(63, 13)
        Me.LblPatientPrenom.TabIndex = 23
        Me.LblPatientPrenom.Text = "Jean-Pierre"
        '
        'LblPatientNom
        '
        Me.LblPatientNom.AutoSize = True
        Me.LblPatientNom.Location = New System.Drawing.Point(133, 4)
        Me.LblPatientNom.Name = "LblPatientNom"
        Me.LblPatientNom.Size = New System.Drawing.Size(46, 13)
        Me.LblPatientNom.TabIndex = 24
        Me.LblPatientNom.Text = "Durand"
        '
        'LblPatientAge
        '
        Me.LblPatientAge.AutoSize = True
        Me.LblPatientAge.Location = New System.Drawing.Point(367, 4)
        Me.LblPatientAge.Name = "LblPatientAge"
        Me.LblPatientAge.Size = New System.Drawing.Size(40, 13)
        Me.LblPatientAge.TabIndex = 25
        Me.LblPatientAge.Text = "35 ans"
        '
        'LblPatientGenre
        '
        Me.LblPatientGenre.AutoSize = True
        Me.LblPatientGenre.Location = New System.Drawing.Point(427, 4)
        Me.LblPatientGenre.Name = "LblPatientGenre"
        Me.LblPatientGenre.Size = New System.Drawing.Size(53, 13)
        Me.LblPatientGenre.TabIndex = 26
        Me.LblPatientGenre.Text = "Masculin"
        '
        'LblPatientNIR
        '
        Me.LblPatientNIR.AutoSize = True
        Me.LblPatientNIR.Location = New System.Drawing.Point(561, 4)
        Me.LblPatientNIR.Name = "LblPatientNIR"
        Me.LblPatientNIR.Size = New System.Drawing.Size(85, 13)
        Me.LblPatientNIR.TabIndex = 27
        Me.LblPatientNIR.Text = "1840675370367"
        '
        'RadGridView1
        '
        Me.RadGridView1.Location = New System.Drawing.Point(12, 79)
        '
        '
        '
        Me.RadGridView1.MasterTemplate.ViewDefinition = TableViewDefinition2
        Me.RadGridView1.Name = "RadGridView1"
        Me.RadGridView1.Size = New System.Drawing.Size(1000, 439)
        Me.RadGridView1.TabIndex = 3
        '
        'RadFEpisodeListe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1020, 560)
        Me.Controls.Add(Me.RadGridView1)
        Me.Controls.Add(Me.RadGroupBoxEtatCivil)
        Me.Controls.Add(Me.RadBtnEpisode)
        Me.Controls.Add(Me.RadBtnAbandon)
        Me.Name = "RadFEpisodeListe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Liste des épisodes du patient"
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadBtnEpisode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGroupBoxEtatCivil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBoxEtatCivil.ResumeLayout(False)
        Me.RadGroupBoxEtatCivil.PerformLayout()
        CType(Me.RadGridView1.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadBtnEpisode As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadGroupBoxEtatCivil As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents LblALD As Label
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
    Friend WithEvents RadGridView1 As Telerik.WinControls.UI.RadGridView
End Class

