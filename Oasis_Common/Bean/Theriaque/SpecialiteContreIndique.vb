Public Class SpecialiteContreIndique
    Private _contreIndication As Boolean
    Private _messageContreIndication As String

    Public Property ContreIndication As Boolean
        Get
            Return _contreIndication
        End Get
        Set(value As Boolean)
            _contreIndication = value
        End Set
    End Property

    Public Property MessageContreIndication As String
        Get
            Return _messageContreIndication
        End Get
        Set(value As String)
            _messageContreIndication = value
        End Set
    End Property
End Class
