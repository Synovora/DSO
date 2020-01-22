Public Class Ordonnance
    Private _id As Long
    Private _patientId As Long
    Private _episodeId As Long
    Private _utilisateurCreation As Long
    Private _dateCreation As Date
    Private _dateValidation As Date
    Private _userValidation As Long
    Private _dateEdition As Date
    Private _commentaire As String
    Private _renouvellement As Integer
    Private _inactif As Boolean

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
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

    Public Property EpisodeId As Long
        Get
            Return _episodeId
        End Get
        Set(value As Long)
            _episodeId = value
        End Set
    End Property

    Public Property UtilisateurCreation As Long
        Get
            Return _utilisateurCreation
        End Get
        Set(value As Long)
            _utilisateurCreation = value
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

    Public Property DateValidation As Date
        Get
            Return _dateValidation
        End Get
        Set(value As Date)
            _dateValidation = value
        End Set
    End Property

    Public Property DateEdition As Date
        Get
            Return _dateEdition
        End Get
        Set(value As Date)
            _dateEdition = value
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

    Public Property Renouvellement As Integer
        Get
            Return _renouvellement
        End Get
        Set(value As Integer)
            _renouvellement = value
        End Set
    End Property

    Public Property UserValidation As Long
        Get
            Return _userValidation
        End Get
        Set(value As Long)
            _userValidation = value
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
