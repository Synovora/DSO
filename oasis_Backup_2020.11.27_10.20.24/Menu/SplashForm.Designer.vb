<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SplashForm
    Inherits Telerik.WinControls.UI.ShapedForm

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SplashForm))
        Me.RoundRectShapeForm = New Telerik.WinControls.RoundRectShape(Me.components)
        Me.RoundRectShapeTitle = New Telerik.WinControls.RoundRectShape(Me.components)
        Me.RadTitleBar1 = New Telerik.WinControls.UI.RadTitleBar()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.RadLabel1 = New Telerik.WinControls.UI.RadLabel()
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RoundRectShapeForm
        '
        Me.RoundRectShapeForm.IsRightToLeft = False
        '
        'RoundRectShapeTitle
        '
        Me.RoundRectShapeTitle.BottomLeftRounded = False
        Me.RoundRectShapeTitle.BottomRightRounded = False
        Me.RoundRectShapeTitle.IsRightToLeft = False
        '
        'RadTitleBar1
        '
        Me.RadTitleBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadTitleBar1.Location = New System.Drawing.Point(1, 1)
        Me.RadTitleBar1.Name = "RadTitleBar1"
        '
        '
        '
        Me.RadTitleBar1.RootElement.ApplyShapeToControl = True
        Me.RadTitleBar1.RootElement.Shape = Me.RoundRectShapeTitle
        Me.RadTitleBar1.Size = New System.Drawing.Size(588, 23)
        Me.RadTitleBar1.TabIndex = 0
        Me.RadTitleBar1.TabStop = False
        Me.RadTitleBar1.Text = "SplashForm"
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(590, 418)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'RadLabel1
        '
        Me.RadLabel1.AutoSize = False
        Me.RadLabel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.RadLabel1.ForeColor = System.Drawing.Color.White
        Me.RadLabel1.Location = New System.Drawing.Point(0, 396)
        Me.RadLabel1.Name = "RadLabel1"
        Me.RadLabel1.Size = New System.Drawing.Size(590, 22)
        Me.RadLabel1.TabIndex = 2
        Me.RadLabel1.Text = "(C) Synovora France 2019/2020"
        Me.RadLabel1.TextAlignment = System.Drawing.ContentAlignment.TopCenter
        '
        'SplashForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(590, 418)
        Me.Controls.Add(Me.RadLabel1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.RadTitleBar1)
        Me.Name = "SplashForm"
        Me.Shape = Me.RoundRectShapeForm
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SplashForm"
        CType(Me.RadTitleBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RoundRectShapeForm As Telerik.WinControls.RoundRectShape
    Friend WithEvents RoundRectShapeTitle As Telerik.WinControls.RoundRectShape
    Friend WithEvents RadTitleBar1 As Telerik.WinControls.UI.RadTitleBar
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents RadLabel1 As Telerik.WinControls.UI.RadLabel
End Class

