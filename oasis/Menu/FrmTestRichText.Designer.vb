<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTestRichText
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
        Me.RadRichTextEditor1 = New Telerik.WinControls.UI.RadRichTextEditor()
        Me.RichTextEditorRibbonBar1 = New Telerik.WinControls.UI.RichTextEditorRibbonBar()
        Me.RadRibbonFormBehavior1 = New Telerik.WinControls.UI.RadRibbonFormBehavior()
        CType(Me.RadRichTextEditor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RichTextEditorRibbonBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadRichTextEditor1
        '
        Me.RadRichTextEditor1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.RadRichTextEditor1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadRichTextEditor1.Location = New System.Drawing.Point(0, 174)
        Me.RadRichTextEditor1.Name = "RadRichTextEditor1"
        Me.RadRichTextEditor1.SelectionFill = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadRichTextEditor1.Size = New System.Drawing.Size(819, 273)
        Me.RadRichTextEditor1.TabIndex = 0
        '
        'RichTextEditorRibbonBar1
        '
        Me.RichTextEditorRibbonBar1.ApplicationMenuStyle = Telerik.WinControls.UI.ApplicationMenuStyle.BackstageView
        Me.RichTextEditorRibbonBar1.AssociatedRichTextEditor = Me.RadRichTextEditor1
        Me.RichTextEditorRibbonBar1.BuiltInStylesVersion = Telerik.WinForms.Documents.Model.Styles.BuiltInStylesVersion.Office2013
        Me.RichTextEditorRibbonBar1.EnableKeyMap = False
        '
        '
        '
        Me.RichTextEditorRibbonBar1.ExitButton.Text = "Exit"
        Me.RichTextEditorRibbonBar1.LocalizationSettings.LayoutModeText = "Simplified Layout"
        Me.RichTextEditorRibbonBar1.Location = New System.Drawing.Point(0, 0)
        Me.RichTextEditorRibbonBar1.Name = "RichTextEditorRibbonBar1"
        '
        '
        '
        Me.RichTextEditorRibbonBar1.OptionsButton.Text = "Options"
        Me.RichTextEditorRibbonBar1.ShowLayoutModeButton = True
        Me.RichTextEditorRibbonBar1.Size = New System.Drawing.Size(819, 174)
        Me.RichTextEditorRibbonBar1.TabIndex = 1
        Me.RichTextEditorRibbonBar1.TabStop = False
        Me.RichTextEditorRibbonBar1.Text = "RichTextEditorRibbonBar1"
        '
        'RadRibbonFormBehavior1
        '
        Me.RadRibbonFormBehavior1.Form = Me
        '
        'FrmTestRichText
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(819, 447)
        Me.Controls.Add(Me.RadRichTextEditor1)
        Me.Controls.Add(Me.RichTextEditorRibbonBar1)
        Me.FormBehavior = Me.RadRibbonFormBehavior1
        Me.IconScaling = Telerik.WinControls.Enumerations.ImageScaling.None
        Me.Name = "FrmTestRichText"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RichTextEditorRibbonBar1"
        CType(Me.RadRichTextEditor1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RichTextEditorRibbonBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadRichTextEditor1 As Telerik.WinControls.UI.RadRichTextEditor
    Friend WithEvents RichTextEditorRibbonBar1 As Telerik.WinControls.UI.RichTextEditorRibbonBar
    Friend WithEvents RadRibbonFormBehavior1 As Telerik.WinControls.UI.RadRibbonFormBehavior
End Class

