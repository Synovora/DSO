Public Class ParametreDrc
    Private _id As Long
    Private _drcId As Long
    Private _parametreId As Long

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
        End Set
    End Property

    Public Property DrcId As Long
        Get
            Return _drcId
        End Get
        Set(value As Long)
            _drcId = value
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
End Class
