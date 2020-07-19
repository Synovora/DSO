Public Class EpisodeObservation
    Private _Id As Long
    Private _episodeId As Long
    Private _patientId As Long
    Private _typeObservation As String      'M: Medicale, P: Paramédicale
    Private _natureObservation As String    'S: Spécifique, L: Libre
    Private _naturePresence As String       'P: Présentiel, T: A distance
    Private _observation As String
    Private _userCreation As Long
    Private _dateCreation As DateTime
    Private _dateModification As DateTime
    Private _inactif As Boolean

    Public Property Id As Long
        Get
            Return _Id
        End Get
        Set(value As Long)
            _Id = value
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

    Public Property TypeObservation As String
        Get
            Return _typeObservation
        End Get
        Set(value As String)
            _typeObservation = value
        End Set
    End Property

    Public Property NatureObservation As String
        Get
            Return _natureObservation
        End Get
        Set(value As String)
            _natureObservation = value
        End Set
    End Property

    Public Property NaturePresence As String
        Get
            Return _naturePresence
        End Get
        Set(value As String)
            _naturePresence = value
        End Set
    End Property

    Public Property Observation As String
        Get
            Return _observation
        End Get
        Set(value As String)
            _observation = value
        End Set
    End Property

    Public Property UserCreation As Long
        Get
            Return _userCreation
        End Get
        Set(value As Long)
            _userCreation = value
        End Set
    End Property

    Public Property DateCreation As Date
        Get
            Return _dateCreation
        End Get
        Set(value As Date)
            _dateCreation = value
        End Set
    End Property

    Public Property DateModification As Date
        Get
            Return _dateModification
        End Get
        Set(value As Date)
            _dateModification = value
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


    Public Function Clone() As EpisodeObservation
        Dim newInstance As EpisodeObservation = DirectCast(Me.MemberwiseClone(), EpisodeObservation)
        Return newInstance
    End Function

End Class
