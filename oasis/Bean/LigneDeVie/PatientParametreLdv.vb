Public Class PatientParametreLdv
    Private _patientId As Long
    Private _typeConsultation As Boolean
    Private _typeVirtuel As Boolean
    Private _typeParametre As Boolean
    Private _activitePathologieAigue As Boolean
    Private _activiteSuiviChronique As Boolean
    Private _activitePreventionAutre As Boolean
    Private _activitePreventionEnfantPreScolaire As Boolean
    Private _activitePreventionEnfantScolaire As Boolean
    Private _activiteSuiviGrossesse As Boolean
    Private _activiteSuiviGyncologique As Boolean
    Private _activiteSocial As Boolean
    Private _profilMedical As Boolean
    Private _profilParamedical As Boolean
    Private _profilPatient
    Private _parametre1 As Long
    Private _parametre2 As Long
    Private _parametre3 As Long
    Private _parametre4 As Long
    Private _parametre5 As Long
    Private _userModification As Long
    Private _dateModification As Date

    Public Property PatientId As Long
        Get
            Return _patientId
        End Get
        Set(value As Long)
            _patientId = value
        End Set
    End Property

    Public Property TypeConsultation As Boolean
        Get
            Return _typeConsultation
        End Get
        Set(value As Boolean)
            _typeConsultation = value
        End Set
    End Property

    Public Property TypeVirtuel As Boolean
        Get
            Return _typeVirtuel
        End Get
        Set(value As Boolean)
            _typeVirtuel = value
        End Set
    End Property

    Public Property ActivitePathologieAigue As Boolean
        Get
            Return _activitePathologieAigue
        End Get
        Set(value As Boolean)
            _activitePathologieAigue = value
        End Set
    End Property

    Public Property ActiviteSuiviChronique As Boolean
        Get
            Return _activiteSuiviChronique
        End Get
        Set(value As Boolean)
            _activiteSuiviChronique = value
        End Set
    End Property

    Public Property ActivitePreventionAutre As Boolean
        Get
            Return _activitePreventionAutre
        End Get
        Set(value As Boolean)
            _activitePreventionAutre = value
        End Set
    End Property

    Public Property ActivitePreventionEnfantPreScolaire As Boolean
        Get
            Return _activitePreventionEnfantPreScolaire
        End Get
        Set(value As Boolean)
            _activitePreventionEnfantPreScolaire = value
        End Set
    End Property

    Public Property ActivitePreventionEnfantScolaire As Boolean
        Get
            Return _activitePreventionEnfantScolaire
        End Get
        Set(value As Boolean)
            _activitePreventionEnfantScolaire = value
        End Set
    End Property

    Public Property ActiviteSuiviGrossesse As Boolean
        Get
            Return _activiteSuiviGrossesse
        End Get
        Set(value As Boolean)
            _activiteSuiviGrossesse = value
        End Set
    End Property

    Public Property ActiviteSuiviGynecologique As Boolean
        Get
            Return _activiteSuiviGyncologique
        End Get
        Set(value As Boolean)
            _activiteSuiviGyncologique = value
        End Set
    End Property

    Public Property ActiviteSocial As Boolean
        Get
            Return _activiteSocial
        End Get
        Set(value As Boolean)
            _activiteSocial = value
        End Set
    End Property

    Public Property ProfilMedical As Boolean
        Get
            Return _profilMedical
        End Get
        Set(value As Boolean)
            _profilMedical = value
        End Set
    End Property

    Public Property ProfilParamedical As Boolean
        Get
            Return _profilParamedical
        End Get
        Set(value As Boolean)
            _profilParamedical = value
        End Set
    End Property

    Public Property Parametre1 As Long
        Get
            Return _parametre1
        End Get
        Set(value As Long)
            _parametre1 = value
        End Set
    End Property

    Public Property Parametre2 As Long
        Get
            Return _parametre2
        End Get
        Set(value As Long)
            _parametre2 = value
        End Set
    End Property

    Public Property Parametre3 As Long
        Get
            Return _parametre3
        End Get
        Set(value As Long)
            _parametre3 = value
        End Set
    End Property

    Public Property Parametre4 As Long
        Get
            Return _parametre4
        End Get
        Set(value As Long)
            _parametre4 = value
        End Set
    End Property

    Public Property Parametre5 As Long
        Get
            Return _parametre5
        End Get
        Set(value As Long)
            _parametre5 = value
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

    Public Property DateModification As Date
        Get
            Return _dateModification
        End Get
        Set(value As Date)
            _dateModification = value
        End Set
    End Property

    Public Property TypeParametre As Boolean
        Get
            Return _typeParametre
        End Get
        Set(value As Boolean)
            _typeParametre = value
        End Set
    End Property

    Public Property ProfilPatient As Object
        Get
            Return _profilPatient
        End Get
        Set(value As Object)
            _profilPatient = value
        End Set
    End Property
End Class
