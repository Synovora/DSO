Public Class SpecialiteTheriaque
    Private _id As Integer
    Private _codeAtc As String
    Private _dci As String
    Private _dciLongue As String

    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Public Property CodeAtc As String
        Get
            Return _codeAtc
        End Get
        Set(value As String)
            _codeAtc = value
        End Set
    End Property

    Public Property Dci As String
        Get
            Return _dci
        End Get
        Set(value As String)
            _dci = value
        End Set
    End Property

    Public Property DciLongue As String
        Get
            Return _dciLongue
        End Get
        Set(value As String)
            _dciLongue = value
        End Set
    End Property
End Class
