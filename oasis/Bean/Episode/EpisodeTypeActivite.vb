Public Class EpisodeTypeActivite
    Private _type As String
    Private _nature As String
    Private _description As String
    Private _inactif As Boolean

    Public Property Type As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
        End Set
    End Property

    Public Property Nature As String
        Get
            Return _nature
        End Get
        Set(value As String)
            _nature = value
        End Set
    End Property

    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
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
