Public Class DrcActeParamedicalAsso
    Private _id As Long
    Private _protocleCollabaratifDrcId As Long
    Private _acteParamedicalDrcId As Long

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
        End Set
    End Property

    Public Property ProtocleCollabaratifDrcId As Long
        Get
            Return _protocleCollabaratifDrcId
        End Get
        Set(value As Long)
            _protocleCollabaratifDrcId = value
        End Set
    End Property

    Public Property ActeParamedicalDrcId As Long
        Get
            Return _acteParamedicalDrcId
        End Get
        Set(value As Long)
            _acteParamedicalDrcId = value
        End Set
    End Property
End Class
