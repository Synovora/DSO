Imports Oasis_Common
Imports Telerik.Charting
Imports Telerik.WinControls.UI

Public Class RadFLigneDeVieGraphe
    Private _ListeValeur As List(Of Decimal)
    Private _ListeDate As List(Of Date)
    Private _parametreId As Long

    Public Property ListeValeur As List(Of Decimal)
        Get
            Return _ListeValeur
        End Get
        Set(value As List(Of Decimal))
            _ListeValeur = value
        End Set
    End Property

    Public Property ListeDate As List(Of Date)
        Get
            Return _ListeDate
        End Get
        Set(value As List(Of Date))
            _ListeDate = value
        End Set
    End Property

    Public Property ParametreId As Long
        Get
            Return _parametreId
        End Get
        Set(value As Long)
            _parametreId = value
        End Set
    End Property

    Dim parametreDao As New ParametreDao
    Dim parametre As Parametre

    Private Sub RadFLigneDeVieGraphe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ParametreId <> 0 Then
            parametre = parametreDao.GetParametreById(ParametreId)
        End If

        Dim series As New LineSeries()
        series.DataPoints.Add(New CategoricalDataPoint(37, DateTime.Now))
        series.DataPoints.Add(New CategoricalDataPoint(38.2, DateTime.Now.AddMonths(1).ToString("dd.MM.yyyy")))
        series.DataPoints.Add(New CategoricalDataPoint(38.8, DateTime.Now.AddDays(38).ToString("dd.MM.yyyy")))
        series.DataPoints.Add(New CategoricalDataPoint(37.5, DateTime.Now.AddDays(72).ToString("dd.MM.yyyy")))

        Dim verticalAxis As LinearAxis = RadChartView1.Axes.[Get](Of LinearAxis)(1)
        verticalAxis.Maximum = 40
        verticalAxis.Minimum = 36

        Dim categoricalAxis As New DateTimeCategoricalAxis()
        categoricalAxis.DateTimeComponent = DateTimeComponent.Week
        categoricalAxis.PlotMode = AxisPlotMode.BetweenTicks
        'categoricalAxis.LabelFormat = "{0:m}"

        'First assign the axis to the VerticalAxis property and then add the series to the chart
        series.HorizontalAxis = categoricalAxis
        'series.VerticalAxis = verticalAxis

        RadChartView1.Series.Add(series)

    End Sub
End Class
