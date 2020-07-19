Public Class ParametreTypeEpisodeDrc
    Private _id As Long
    Private _typeEpisode As String
    Private _DrcId As Long
    Private _inactif As Boolean

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
        End Set
    End Property

    Public Property TypeEpisode As String
        Get
            Return _typeEpisode
        End Get
        Set(value As String)
            _typeEpisode = value
        End Set
    End Property

    Public Property DrcId As Long
        Get
            Return _DrcId
        End Get
        Set(value As Long)
            _DrcId = value
        End Set
    End Property

    Public Property Inactif As Boolean
        Get
            Return _inactif
        End Get
        Set(value As Boolean)
            _inactif = value
        End Set
    End Property
End Class
