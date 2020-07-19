Public Class EpisodeContexte
    Private _episodeContexteId As Long
    Private _episodeId As Long
    Private _patientId As Long
    Private _contexteId As Long
    Private _dateCreation As Date
    Private _userCreation As Long

    Public Property EpisodeContexteId As Long
        Get
            Return _episodeContexteId
        End Get
        Set(value As Long)
            _episodeContexteId = value
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

    Public Property ContexteId As Long
        Get
            Return _contexteId
        End Get
        Set(value As Long)
            _contexteId = value
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

    Public Property UserCreation As Long
        Get
            Return _userCreation
        End Get
        Set(value As Long)
            _userCreation = value
        End Set
    End Property
End Class
