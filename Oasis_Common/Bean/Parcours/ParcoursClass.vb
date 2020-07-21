Public Class Parcours
    Private _id As Integer
    Private _PatientId As Integer
    Private _specialiteId As Integer
    Private _categorieId As Integer
    Private _sousCategorieId As Integer
    Private _intervenantOasis As Boolean
    Private _rorId As Integer
    Private _commentaire As String
    Private _base As String
    Private _Rythme As Integer
    Private _Cacher As Boolean
    Private _inactif As Boolean
    Private _userCreation As Integer
    Private _dateCreation As Date
    Private _userModification As Integer
    Private _dateModification As Date

    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Public Property PatientId As Integer
        Get
            Return _PatientId
        End Get
        Set(value As Integer)
            _PatientId = value
        End Set
    End Property

    Public Property SpecialiteId As Integer
        Get
            Return _specialiteId
        End Get
        Set(value As Integer)
            _specialiteId = value
        End Set
    End Property

    Public Property SousCategorieId As Integer
        Get
            Return _sousCategorieId
        End Get
        Set(value As Integer)
            _sousCategorieId = value
        End Set
    End Property

    Public Property IntervenantOasis As Boolean
        Get
            Return _intervenantOasis
        End Get
        Set(value As Boolean)
            _intervenantOasis = value
        End Set
    End Property

    Public Property RorId As Integer
        Get
            Return _rorId
        End Get
        Set(value As Integer)
            _rorId = value
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

    Public Property Cacher As Boolean
        Get
            Return _Cacher
        End Get
        Set(value As Boolean)
            _Cacher = value
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

    Public Property UserCreation As Integer
        Get
            Return _userCreation
        End Get
        Set(value As Integer)
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

    Public Property UserModification As Integer
        Get
            Return _userModification
        End Get
        Set(value As Integer)
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

    Public Property CategorieId As Integer
        Get
            Return _categorieId
        End Get
        Set(value As Integer)
            _categorieId = value
        End Set
    End Property

    Public Property Base As String
        Get
            Return _base
        End Get
        Set(value As String)
            _base = value
        End Set
    End Property

    Public Property Rythme As Integer
        Get
            Return _Rythme
        End Get
        Set(value As Integer)
            _Rythme = value
        End Set
    End Property

    Public Function Clone() As Parcours
        Dim newInstance As Parcours = DirectCast(Me.MemberwiseClone(), Parcours)
        Return newInstance
    End Function

End Class
