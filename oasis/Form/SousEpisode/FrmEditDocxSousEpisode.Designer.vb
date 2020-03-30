<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmEditDocxSousEpisode
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            RichTextEditorRibbonBar1.Dispose()
            RadRichTextEditorRuler1.Dispose()
            RadRichTextEditor1.Dispose()
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmEditDocxSousEpisode))
        Me.RadRichTextEditor1 = New Telerik.WinControls.UI.RadRichTextEditor()
        Me.RichTextEditorRibbonBar1 = New Telerik.WinControls.UI.RichTextEditorRibbonBar()
        Me.RadButtonElement1 = New Telerik.WinControls.UI.RadButtonElement()
        Me.RadRibbonFormBehavior1 = New Telerik.WinControls.UI.RadRibbonFormBehavior()
        Me.RadRichTextEditorRuler1 = New Telerik.WinControls.UI.RadRichTextEditorRuler()
        CType(Me.RadRichTextEditor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RichTextEditorRibbonBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadRichTextEditorRuler1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadRichTextEditorRuler1.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadRichTextEditor1
        '
        Me.RadRichTextEditor1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(189, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.RadRichTextEditor1.Location = New System.Drawing.Point(29, 29)
        Me.RadRichTextEditor1.Name = "RadRichTextEditor1"
        Me.RadRichTextEditor1.SelectionFill = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(158, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadRichTextEditor1.Size = New System.Drawing.Size(963, 553)
        Me.RadRichTextEditor1.TabIndex = 3
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
        Me.RichTextEditorRibbonBar1.MinimizeButton = False
        Me.RichTextEditorRibbonBar1.Name = "RichTextEditorRibbonBar1"
        '
        '
        '
        Me.RichTextEditorRibbonBar1.OptionsButton.Text = "Options"
        Me.RichTextEditorRibbonBar1.QuickAccessToolBarItems.AddRange(New Telerik.WinControls.RadItem() {Me.RadButtonElement1})
        Me.RichTextEditorRibbonBar1.ShowExpandButton = False
        Me.RichTextEditorRibbonBar1.ShowItemToolTips = False
        Me.RichTextEditorRibbonBar1.ShowLayoutModeButton = True
        Me.RichTextEditorRibbonBar1.Size = New System.Drawing.Size(993, 174)
        Me.RichTextEditorRibbonBar1.TabIndex = 4
        Me.RichTextEditorRibbonBar1.TabStop = False
        Me.RichTextEditorRibbonBar1.Text = "Document sous-épisode"
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0), Telerik.WinControls.UI.RadRibbonBarElement).Text = "Document sous-épisode"
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Image = CType(resources.GetObject("resource.Image"), System.Drawing.Image)
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).DisplayStyle = Telerik.WinControls.DisplayStyle.Image
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "Save"
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Padding = New System.Windows.Forms.Padding(2, 1, 2, 2)
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Name = "buttonSave"
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).StretchHorizontally = False
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).StretchVertically = False
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.RadQuickAccessOverflowButton).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(3).GetChildAt(1).GetChildAt(0), Telerik.WinControls.UI.RadImageButtonElement).Visibility = Telerik.WinControls.ElementVisibility.Visible
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(4).GetChildAt(0), Telerik.WinControls.UI.StripViewItemContainer).Padding = New System.Windows.Forms.Padding(44, 0, 84, 0)
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(4).GetChildAt(0).GetChildAt(0).GetChildAt(6), Telerik.WinControls.UI.RichTextEditorRibbonUI.RichTextEditorRibbonTab).Text = "View"
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(4).GetChildAt(0).GetChildAt(0).GetChildAt(6), Telerik.WinControls.UI.RichTextEditorRibbonUI.RichTextEditorRibbonTab).Visibility = Telerik.WinControls.ElementVisibility.Hidden
        CType(Me.RichTextEditorRibbonBar1.GetChildAt(0).GetChildAt(4).GetChildAt(0).GetChildAt(0).GetChildAt(6), Telerik.WinControls.UI.RichTextEditorRibbonUI.RichTextEditorRibbonTab).Name = "tabView"
        '
        'RadButtonElement1
        '
        Me.RadButtonElement1.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
        Me.RadButtonElement1.Image = CType(resources.GetObject("RadButtonElement1.Image"), System.Drawing.Image)
        Me.RadButtonElement1.Name = "RadButtonElement1"
        Me.RadButtonElement1.StretchHorizontally = False
        Me.RadButtonElement1.StretchVertically = False
        Me.RadButtonElement1.Text = "Enregistrer"
        Me.RadButtonElement1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'RadRibbonFormBehavior1
        '
        Me.RadRibbonFormBehavior1.Form = Me
        '
        'RadRichTextEditorRuler1
        '
        Me.RadRichTextEditorRuler1.AssociatedRichTextBox = Me.RadRichTextEditor1
        Me.RadRichTextEditorRuler1.Controls.Add(Me.RadRichTextEditor1)
        Me.RadRichTextEditorRuler1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadRichTextEditorRuler1.Location = New System.Drawing.Point(0, 174)
        Me.RadRichTextEditorRuler1.Name = "RadRichTextEditorRuler1"
        Me.RadRichTextEditorRuler1.Size = New System.Drawing.Size(993, 583)
        Me.RadRichTextEditorRuler1.TabIndex = 5
        '
        'FrmEditDocxSousEpisode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(993, 757)
        Me.Controls.Add(Me.RadRichTextEditorRuler1)
        Me.Controls.Add(Me.RichTextEditorRibbonBar1)
        Me.FormBehavior = Me.RadRibbonFormBehavior1
        Me.IconScaling = Telerik.WinControls.Enumerations.ImageScaling.None
        Me.Name = "FrmEditDocxSousEpisode"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Document sous-épisode"
        CType(Me.RadRichTextEditor1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RichTextEditorRibbonBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadRichTextEditorRuler1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadRichTextEditorRuler1.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadRichTextEditor1 As Telerik.WinControls.UI.RadRichTextEditor
    Friend WithEvents RichTextEditorRibbonBar1 As Telerik.WinControls.UI.RichTextEditorRibbonBar
    Friend WithEvents RadRibbonFormBehavior1 As Telerik.WinControls.UI.RadRibbonFormBehavior
    Friend WithEvents RadRichTextEditorRuler1 As Telerik.WinControls.UI.RadRichTextEditorRuler
    Friend WithEvents RadButtonElement1 As Telerik.WinControls.UI.RadButtonElement
End Class

