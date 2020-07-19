Public Class DrcStandard
    Private _id As Long
    Private _typeActivite As String
    Private _drcId As Long
    Private _categorieOasis As Integer
    Private _ageMin As Integer
    Private _ageMax As Integer
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

    Public Property TypeActivite As String
        Get
            Return _typeActivite
        End Get
        Set(value As String)
            _typeActivite = value
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

    Public Property CategorieOasis As Integer
        Get
            Return _categorieOasis
        End Get
        Set(value As Integer)
            _categorieOasis = value
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
