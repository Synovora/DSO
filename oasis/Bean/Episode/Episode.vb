Public Class Episode
    Private _id As Long
    Private _patientId As Long
    Private _type As String         'C: Consultation, V: Virteul
    Private _typeActivite As String 'Suivi pathologie chronique, pathologie aigue, prévention protection infantile, prévention suivi gynécologique, prévention suivi grossesse, prévention autre, social
    Private _descriptionActivite As String
    Private _typeProfil As String 'MEDICAL, PARAMEDICAL
    Private _commentaire As String
    Private _observationMedical As String
    Private _observationParamedical As String
    Private _decision As String
    Private _userCreation As Long
    Private _dateCreation As Date
    Private _userModification As Long
    Private _dateModification As Date
    Private _etat As String         'En cours, En attente, Cloturé
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

    Public Property Type As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
        End Set
    End Property

    Public Property TypeActivite As String
        Get
            Return _typeActivite
        End Get
        Set(value As String)
            _typeActivite = value
        End Set
    End Property

    Public Property DescriptionActivite As String
        Get
            Return _descriptionActivite
        End Get
        Set(value As String)
            _descriptionActivite = value
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

    Public Property Decision As String
        Get
            Return _decision
        End Get
        Set(value As String)
            _decision = value
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

    Public Property Etat As String
        Get
            Return _etat
        End Get
        Set(value As String)
            _etat = value
        End Set
    End Property

    Public Property UserModification As Long
        Get
            Return _userModification
        End Get
        Set(value As Long)
            _userModification = value
        End Set
    End Property

    Public Property ObservationMedical As String
        Get
            Return _observationMedical
        End Get
        Set(value As String)
            _observationMedical = value
        End Set
    End Property

    Public Property ObservationParamedical As String
        Get
            Return _observationParamedical
        End Get
        Set(value As String)
            _observationParamedical = value
        End Set
    End Property

    Public Property TypeProfil As String
        Get
            Return _typeProfil
        End Get
        Set(value As String)
            _typeProfil = value
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
