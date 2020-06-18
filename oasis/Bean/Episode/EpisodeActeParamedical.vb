Public Class EpisodeActeParamedical
    Private _id As Long
    Private _episodeId As Long
    Private _patientId As Long
    Private _drcId As Long
    Private _observation As String
    Private _typeObservation As String
    Private _userId As Long
    Private _dateObservation As Date
    Private _dateModification As Date
    Private _inactif As Boolean

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
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

    Public Property DrcId As Long
        Get
            Return _drcId
        End Get
        Set(value As Long)
            _drcId = value
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

    Public Property TypeObservation As String
        Get
            Return _typeObservation
        End Get
        Set(value As String)
            _typeObservation = value
        End Set
    End Property

    Public Property UserId As Long
        Get
            Return _userId
        End Get
        Set(value As Long)
            _userId = value
        End Set
    End Property

    Public Property DateObservation As Date
        Get
            Return _dateObservation
        End Get
        Set(value As Date)
            _dateObservation = value
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
End Class
