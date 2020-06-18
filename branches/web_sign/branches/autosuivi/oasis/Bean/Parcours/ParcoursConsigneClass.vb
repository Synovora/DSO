Public Class ParcoursConsigne
    Private _id As Long
    Private _parcoursId As Long
    Private _patientId As Long
    Private _drcId As Long
    Private _typeEpisode As String
    Private _commentaire As String
    Private _ordre As Integer
    Private _age_min As Integer
    Private _age_max As Integer
    Private _age_unite As String
    Private _dateDebut As Date
    Private _dateFin As Date
    Private _inactif As Boolean
    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
        End Set
    End Property

    Public Property ParcoursId As Long
        Get
            Return _parcoursId
        End Get
        Set(value As Long)
            _parcoursId = value
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

    Public Property Commentaire As String
        Get
            Return _commentaire
        End Get
        Set(value As String)
            _commentaire = value
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

    Public Property DateDebut As Date
        Get
            Return _dateDebut
        End Get
        Set(value As Date)
            _dateDebut = value
        End Set
    End Property

    Public Property DateFin As Date
        Get
            Return _dateFin
        End Get
        Set(value As Date)
            _dateFin = value
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

    Public Property TypeEpisode As String
        Get
            Return _typeEpisode
        End Get
        Set(value As String)
            _typeEpisode = value
        End Set
    End Property

    Public Property AgeMin As Integer
        Get
            Return _age_min
        End Get
        Set(value As Integer)
            _age_min = value
        End Set
    End Property

    Public Property AgeMax As Integer
        Get
            Return _age_max
        End Get
        Set(value As Integer)
            _age_max = value
        End Set
    End Property

    Public Property AgeUnite As String
        Get
            Return _age_unite
        End Get
        Set(value As String)
            _age_unite = value
        End Set
    End Property
End Class
