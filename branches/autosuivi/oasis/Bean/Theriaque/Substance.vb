Public Class Substance
    Private _substanceId As Integer
    Private _substanceDenomination As String
    Private _substancePereId As Integer

    Public Property SubstanceId As Integer
        Get
            Return _substanceId
        End Get
        Set(value As Integer)
            _substanceId = value
        End Set
    End Property

    Public Property SubstanceDenomination As String
        Get
            Return _substanceDenomination
        End Get
        Set(value As String)
            _substanceDenomination = value
        End Set
    End Property

    Public Property SubstancePereId As Integer
        Get
            Return _substancePereId
        End Get
        Set(value As Integer)
            _substancePereId = value
        End Set
    End Property
End Class
