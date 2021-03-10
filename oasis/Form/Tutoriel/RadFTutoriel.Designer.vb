<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFTutoriel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadFTutoriel))
        Me.TxtCommentaireDrc = New System.Windows.Forms.TextBox()
        Me.RadBtnAbandonner = New Telerik.WinControls.UI.RadButton()
        Me.TxtDescriptionDrc = New System.Windows.Forms.TextBox()
        Me.WebBrowser = New System.Windows.Forms.WebBrowser()
        Me.RadSplitContainer1 = New Telerik.WinControls.UI.RadSplitContainer()
        Me.SplitPanel1 = New Telerik.WinControls.UI.SplitPanel()
        Me.SplitPanel2 = New Telerik.WinControls.UI.SplitPanel()
        Me.RadSplitContainer2 = New Telerik.WinControls.UI.RadSplitContainer()
        Me.SplitPanel4 = New Telerik.WinControls.UI.SplitPanel()
        Me.SplitPanel5 = New Telerik.WinControls.UI.SplitPanel()
        Me.SplitPanel3 = New Telerik.WinControls.UI.SplitPanel()
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadSplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadSplitContainer1.SuspendLayout()
        CType(Me.SplitPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitPanel1.SuspendLayout()
        CType(Me.SplitPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitPanel2.SuspendLayout()
        CType(Me.RadSplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadSplitContainer2.SuspendLayout()
        CType(Me.SplitPanel4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitPanel4.SuspendLayout()
        CType(Me.SplitPanel5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitPanel5.SuspendLayout()
        CType(Me.SplitPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitPanel3.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtCommentaireDrc
        '
        Me.TxtCommentaireDrc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TxtCommentaireDrc.Location = New System.Drawing.Point(0, 0)
        Me.TxtCommentaireDrc.Multiline = True
        Me.TxtCommentaireDrc.Name = "TxtCommentaireDrc"
        Me.TxtCommentaireDrc.ReadOnly = True
        Me.TxtCommentaireDrc.Size = New System.Drawing.Size(468, 673)
        Me.TxtCommentaireDrc.TabIndex = 0
        '
        'RadBtnAbandonner
        '
        Me.RadBtnAbandonner.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadBtnAbandonner.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandonner.Image = Global.Oasis_WF.My.Resources.Resources._exit
        Me.RadBtnAbandonner.Location = New System.Drawing.Point(1457, 3)
        Me.RadBtnAbandonner.Name = "RadBtnAbandonner"
        Me.RadBtnAbandonner.Size = New System.Drawing.Size(24, 24)
        Me.RadBtnAbandonner.TabIndex = 168
        '
        'TxtDescriptionDrc
        '
        Me.TxtDescriptionDrc.Location = New System.Drawing.Point(3, 3)
        Me.TxtDescriptionDrc.Name = "TxtDescriptionDrc"
        Me.TxtDescriptionDrc.ReadOnly = True
        Me.TxtDescriptionDrc.Size = New System.Drawing.Size(465, 20)
        Me.TxtDescriptionDrc.TabIndex = 169
        '
        'WebBrowser
        '
        Me.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(1012, 673)
        Me.WebBrowser.TabIndex = 170
        '
        'RadSplitContainer1
        '
        Me.RadSplitContainer1.Controls.Add(Me.SplitPanel1)
        Me.RadSplitContainer1.Controls.Add(Me.SplitPanel2)
        Me.RadSplitContainer1.Controls.Add(Me.SplitPanel3)
        Me.RadSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadSplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.RadSplitContainer1.Name = "RadSplitContainer1"
        Me.RadSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        '
        '
        Me.RadSplitContainer1.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.RadSplitContainer1.Size = New System.Drawing.Size(1484, 765)
        Me.RadSplitContainer1.TabIndex = 171
        Me.RadSplitContainer1.TabStop = False
        '
        'SplitPanel1
        '
        Me.SplitPanel1.Controls.Add(Me.TxtDescriptionDrc)
        Me.SplitPanel1.Location = New System.Drawing.Point(0, 0)
        Me.SplitPanel1.Name = "SplitPanel1"
        '
        '
        '
        Me.SplitPanel1.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.SplitPanel1.Size = New System.Drawing.Size(1484, 47)
        Me.SplitPanel1.SizeInfo.AutoSizeScale = New System.Drawing.SizeF(0!, -0.2712462!)
        Me.SplitPanel1.SizeInfo.SplitterCorrection = New System.Drawing.Size(0, -112)
        Me.SplitPanel1.TabIndex = 0
        Me.SplitPanel1.TabStop = False
        Me.SplitPanel1.Text = "SplitPanel1"
        '
        'SplitPanel2
        '
        Me.SplitPanel2.Controls.Add(Me.RadSplitContainer2)
        Me.SplitPanel2.Location = New System.Drawing.Point(0, 51)
        Me.SplitPanel2.Name = "SplitPanel2"
        '
        '
        '
        Me.SplitPanel2.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.SplitPanel2.Size = New System.Drawing.Size(1484, 673)
        Me.SplitPanel2.SizeInfo.AutoSizeScale = New System.Drawing.SizeF(0!, 0.5557023!)
        Me.SplitPanel2.SizeInfo.SplitterCorrection = New System.Drawing.Size(0, 239)
        Me.SplitPanel2.TabIndex = 1
        Me.SplitPanel2.TabStop = False
        Me.SplitPanel2.Text = "SplitPanel2"
        '
        'RadSplitContainer2
        '
        Me.RadSplitContainer2.Controls.Add(Me.SplitPanel4)
        Me.RadSplitContainer2.Controls.Add(Me.SplitPanel5)
        Me.RadSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadSplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.RadSplitContainer2.Name = "RadSplitContainer2"
        '
        '
        '
        Me.RadSplitContainer2.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.RadSplitContainer2.Size = New System.Drawing.Size(1484, 673)
        Me.RadSplitContainer2.TabIndex = 0
        Me.RadSplitContainer2.TabStop = False
        '
        'SplitPanel4
        '
        Me.SplitPanel4.Controls.Add(Me.TxtCommentaireDrc)
        Me.SplitPanel4.Location = New System.Drawing.Point(0, 0)
        Me.SplitPanel4.Name = "SplitPanel4"
        '
        '
        '
        Me.SplitPanel4.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.SplitPanel4.Size = New System.Drawing.Size(468, 673)
        Me.SplitPanel4.SizeInfo.AutoSizeScale = New System.Drawing.SizeF(-0.1837838!, 0!)
        Me.SplitPanel4.SizeInfo.SplitterCorrection = New System.Drawing.Size(-274, 0)
        Me.SplitPanel4.TabIndex = 0
        Me.SplitPanel4.TabStop = False
        Me.SplitPanel4.Text = "SplitPanel4"
        '
        'SplitPanel5
        '
        Me.SplitPanel5.Controls.Add(Me.WebBrowser)
        Me.SplitPanel5.Location = New System.Drawing.Point(472, 0)
        Me.SplitPanel5.Name = "SplitPanel5"
        '
        '
        '
        Me.SplitPanel5.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.SplitPanel5.Size = New System.Drawing.Size(1012, 673)
        Me.SplitPanel5.SizeInfo.AutoSizeScale = New System.Drawing.SizeF(0.1837838!, 0!)
        Me.SplitPanel5.SizeInfo.SplitterCorrection = New System.Drawing.Size(274, 0)
        Me.SplitPanel5.TabIndex = 1
        Me.SplitPanel5.TabStop = False
        Me.SplitPanel5.Text = "SplitPanel5"
        '
        'SplitPanel3
        '
        Me.SplitPanel3.Controls.Add(Me.RadBtnAbandonner)
        Me.SplitPanel3.Location = New System.Drawing.Point(0, 728)
        Me.SplitPanel3.Name = "SplitPanel3"
        '
        '
        '
        Me.SplitPanel3.RootElement.MinSize = New System.Drawing.Size(25, 25)
        Me.SplitPanel3.Size = New System.Drawing.Size(1484, 37)
        Me.SplitPanel3.SizeInfo.AutoSizeScale = New System.Drawing.SizeF(0!, -0.2844562!)
        Me.SplitPanel3.SizeInfo.SplitterCorrection = New System.Drawing.Size(0, -127)
        Me.SplitPanel3.TabIndex = 2
        Me.SplitPanel3.TabStop = False
        Me.SplitPanel3.Text = "SplitPanel3"
        '
        'RadFTutoriel
        '
        Me.AcceptButton = Me.RadBtnAbandonner
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandonner
        Me.ClientSize = New System.Drawing.Size(1484, 765)
        Me.Controls.Add(Me.RadSplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RadFTutoriel"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Tutoriel"
        CType(Me.RadBtnAbandonner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadSplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadSplitContainer1.ResumeLayout(False)
        CType(Me.SplitPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitPanel1.ResumeLayout(False)
        Me.SplitPanel1.PerformLayout()
        CType(Me.SplitPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitPanel2.ResumeLayout(False)
        CType(Me.RadSplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadSplitContainer2.ResumeLayout(False)
        CType(Me.SplitPanel4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitPanel4.ResumeLayout(False)
        Me.SplitPanel4.PerformLayout()
        CType(Me.SplitPanel5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitPanel5.ResumeLayout(False)
        CType(Me.SplitPanel3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitPanel3.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TxtCommentaireDrc As TextBox
    Friend WithEvents RadBtnAbandonner As Telerik.WinControls.UI.RadButton
    Friend WithEvents TxtDescriptionDrc As TextBox
    Friend WithEvents WebBrowser As WebBrowser
    Friend WithEvents RadSplitContainer1 As Telerik.WinControls.UI.RadSplitContainer
    Friend WithEvents SplitPanel1 As Telerik.WinControls.UI.SplitPanel
    Friend WithEvents SplitPanel2 As Telerik.WinControls.UI.SplitPanel
    Friend WithEvents RadSplitContainer2 As Telerik.WinControls.UI.RadSplitContainer
    Friend WithEvents SplitPanel4 As Telerik.WinControls.UI.SplitPanel
    Friend WithEvents SplitPanel5 As Telerik.WinControls.UI.SplitPanel
    Friend WithEvents SplitPanel3 As Telerik.WinControls.UI.SplitPanel
End Class

