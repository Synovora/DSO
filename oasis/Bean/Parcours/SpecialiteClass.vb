Public Class Specialite
    Private _specialiteId As Long
    Private _Code As String
    Private _description As String
    Private _nature As String
    Private _parcours As Boolean
    Private _oasis As Boolean
    Private _genre As String
    Private _ageMin As Integer
    Private _ageMax As Integer
    Private _delaiPriseEnCharge As Integer

    Public Property SpecialiteId As Long
        Get
            Return _specialiteId
        End Get
        Set(value As Long)
            _specialiteId = value
        End Set
    End Property

    Public Property Code As String
        Get
            Return _Code
        End Get
        Set(value As String)
            _Code = value
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

    Public Property Nature As String
        Get
            Return _nature
        End Get
        Set(value As String)
            _nature = value
        End Set
    End Property

    Public Property Parcours As Boolean
        Get
            Return _parcours
        End Get
        Set(value As Boolean)
            _parcours = value
        End Set
    End Property

    Public Property Oasis As Boolean
        Get
            Return _oasis
        End Get
        Set(value As Boolean)
            _oasis = value
        End Set
    End Property

    Public Property Genre As String
        Get
            Return _genre
        End Get
        Set(value As String)
            _genre = value
        End Set
    End Property

    Public Property AgeMin As Integer
        Get
            Return _ageMin
        End Get
        Set(value As Integer)
            _ageMin = value
        End Set
    End Property

    Public Property AgeMax As Integer
        Get
            Return _ageMax
        End Get
        Set(value As Integer)
            _ageMax = value
        End Set
    End Property

    Public Property DelaiPriseEnCharge As Integer
        Get
            Return _delaiPriseEnCharge
        End Get
        Set(value As Integer)
            _delaiPriseEnCharge = value
        End Set
    End Property
End Class
