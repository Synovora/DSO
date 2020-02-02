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
        parametre = parametreDao.GetParametreById(ParametreId)




    End Sub
End Class
