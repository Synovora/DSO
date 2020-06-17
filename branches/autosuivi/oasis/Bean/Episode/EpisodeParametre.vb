Public Class EpisodeParametre
    Private _id As Long
    Private _parametreId As Long
    Private _episodeId As Long
    Private _patientId As Long
    Private _valeur As Decimal
    Private _description As String
    Private _entier As Integer
    Private _decimal As Integer
    Private _unite As String
    Private _parametreAjoute As Boolean
    Private _ordre As Integer
    Private _inactif As Boolean

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
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

    Public Property EpisodeId As Long
        Get
            Return _episodeId
        End Get
        Set(value As Long)
            _episodeId = value
        End Set
    End Property

    Public Property PatientId As Long
        Get
            Return _patientId
        End Get
        Set(value As Long)
            _patientId = value
        End Set
    End Property

    Public Property Valeur As Decimal
        Get
            Return _valeur
        End Get
        Set(value As Decimal)
            _valeur = value
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

    Public Property Entier As Integer
        Get
            Return _entier
        End Get
        Set(value As Integer)
            _entier = value
        End Set
    End Property

    Public Property [Decimal] As Integer
        Get
            Return _decimal
        End Get
        Set(value As Integer)
            _decimal = value
        End Set
    End Property

    Public Property Unite As String
        Get
            Return _unite
        End Get
        Set(value As String)
            _unite = value
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

    Public Property ParametreAjoute As Boolean
        Get
            Return _parametreAjoute
        End Get
        Set(value As Boolean)
            _parametreAjoute = value
        End Set
    End Property

    Public Property Ordre As Integer
        Get
            Return _ordre
        End Get
        Set(value As Integer)
            _ordre = value
        End Set
    End Property
End Class
