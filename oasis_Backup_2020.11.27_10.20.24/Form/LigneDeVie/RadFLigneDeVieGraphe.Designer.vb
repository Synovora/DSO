<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadFLigneDeVieGraphe
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
        Dim CartesianArea1 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
        Dim CategoricalAxis1 As Telerik.WinControls.UI.CategoricalAxis = New Telerik.WinControls.UI.CategoricalAxis()
        Dim LinearAxis1 As Telerik.WinControls.UI.LinearAxis = New Telerik.WinControls.UI.LinearAxis()
        Dim LineSeries1 As Telerik.WinControls.UI.LineSeries = New Telerik.WinControls.UI.LineSeries()
        Me.RadChartView1 = New Telerik.WinControls.UI.RadChartView()
        CType(Me.RadChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadChartView1
        '
        Me.RadChartView1.AreaDesign = CartesianArea1
        CategoricalAxis1.IsPrimary = True
        CategoricalAxis1.LabelRotationAngle = 300.0R
        CategoricalAxis1.Title = ""
        LinearAxis1.AxisType = Telerik.Charting.AxisType.Second
        LinearAxis1.IsPrimary = True
        LinearAxis1.LabelRotationAngle = 300.0R
        LinearAxis1.TickOrigin = Nothing
        LinearAxis1.Title = ""
        Me.RadChartView1.Axes.AddRange(New Telerik.WinControls.UI.Axis() {CategoricalAxis1, LinearAxis1})
        Me.RadChartView1.Location = New System.Drawing.Point(12, 70)
        Me.RadChartView1.Name = "RadChartView1"
        LineSeries1.HorizontalAxis = CategoricalAxis1
        LineSeries1.LabelAngle = 90.0R
        LineSeries1.LabelDistanceToPoint = 15.0R
        LineSeries1.Spline = True
        LineSeries1.VerticalAxis = LinearAxis1
        Me.RadChartView1.Series.AddRange(New Telerik.WinControls.UI.ChartSeries() {LineSeries1})
        Me.RadChartView1.ShowGrid = False
        Me.RadChartView1.Size = New System.Drawing.Size(926, 367)
        Me.RadChartView1.TabIndex = 0
        '
        'RadFLigneDeVieGraphe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(950, 473)
        Me.Controls.Add(Me.RadChartView1)
        Me.Name = "RadFLigneDeVieGraphe"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.Text = "RadFLigneDeVieGraphe"
        CType(Me.RadChartView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RadChartView1 As Telerik.WinControls.UI.RadChartView
End Class

