Public Class LigneDeVie
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
    Private _profilPatient As Boolean
    Private _parametreId1 As Long
    Private _parametreId2 As Long
    Private _parametreId3 As Long
    Private _parametreId4 As Long
    Private _parametreId5 As Long

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

    Public Property ActiviteSuiviGyncologique As Boolean
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

    Public Property TypeParametre As Boolean
        Get
            Return _typeParametre
        End Get
        Set(value As Boolean)
            _typeParametre = value
        End Set
    End Property

    Public Property ParametreId1 As Long
        Get
            Return _parametreId1
        End Get
        Set(value As Long)
            _parametreId1 = value
        End Set
    End Property

    Public Property ParametreId2 As Long
        Get
            Return _parametreId2
        End Get
        Set(value As Long)
            _parametreId2 = value
        End Set
    End Property

    Public Property ParametreId3 As Long
        Get
            Return _parametreId3
        End Get
        Set(value As Long)
            _parametreId3 = value
        End Set
    End Property

    Public Property ParametreId4 As Long
        Get
            Return _parametreId4
        End Get
        Set(value As Long)
            _parametreId4 = value
        End Set
    End Property

    Public Property ParametreId5 As Long
        Get
            Return _parametreId5
        End Get
        Set(value As Long)
            _parametreId5 = value
        End Set
    End Property

    Public Property ProfilPatient As Boolean
        Get
            Return _profilPatient
        End Get
        Set(value As Boolean)
            _profilPatient = value
        End Set
    End Property
End Class
