<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFPatientRequestSelector
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
        Me.RadGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
        Me.BtnDRC = New System.Windows.Forms.Button()
        Me.BtnEtatJournalier = New System.Windows.Forms.Button()
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadGroupBox1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadGroupBox1
        '
        Me.RadGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.RadGroupBox1.Controls.Add(Me.BtnEtatJournalier)
        Me.RadGroupBox1.Controls.Add(Me.BtnDRC)
        Me.RadGroupBox1.HeaderText = "Fonction"
        Me.RadGroupBox1.Location = New System.Drawing.Point(13, 13)
        Me.RadGroupBox1.Name = "RadGroupBox1"
        Me.RadGroupBox1.Size = New System.Drawing.Size(200, 245)
        Me.RadGroupBox1.TabIndex = 0
        Me.RadGroupBox1.Text = "Fonction"
        '
        'BtnDRC
        '
        Me.BtnDRC.Location = New System.Drawing.Point(6, 22)
        Me.BtnDRC.Name = "BtnDRC"
        Me.BtnDRC.Size = New System.Drawing.Size(189, 23)
        Me.BtnDRC.TabIndex = 0
        Me.BtnDRC.Text = "Liste des patients via DRC"
        Me.BtnDRC.UseVisualStyleBackColor = True
        '
        'BtnEtatJournalier
        '
        Me.BtnEtatJournalier.Location = New System.Drawing.Point(5, 51)
        Me.BtnEtatJournalier.Name = "BtnEtatJournalier"
        Me.BtnEtatJournalier.Size = New System.Drawing.Size(189, 23)
        Me.BtnEtatJournalier.TabIndex = 1
        Me.BtnEtatJournalier.Text = "Etat Journalier"
        Me.BtnEtatJournalier.UseVisualStyleBackColor = True
        '
        'RadFPatientRequestSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(234, 270)
        Me.Controls.Add(Me.RadGroupBox1)
        Me.Name = "RadFPatientRequestSelector"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFPatientRequestSelector"
        CType(Me.RadGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadGroupBox1.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadGroupBox1 As Telerik.WinControls.UI.RadGroupBox
    Friend WithEvents BtnDRC As Button
    Friend WithEvents BtnEtatJournalier As Button
End Class

