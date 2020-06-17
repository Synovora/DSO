Public Class Ror
    Private _id As Long
    Private _specialiteId As Long
    Private _nom As String
    Private _oasis As Boolean
    Private _type As String
    Private _StructureId As Long
    Private _structureNom As String
    Private _adresse1 As String
    Private _adresse2 As String
    Private _codePostal As String
    Private _ville As String
    Private _code As String
    Private _telephone As String
    Private _email As String
    Private _commentaire As String
    Private _rpps As Long
    Private _finess As Long
    Private _adeli As Long
    Private _inactif As Boolean
    Private _userCreation As Long
    Private _dateCreation As Date
    Private _userModification As Long
    Private _dateModification As Date

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
        End Set
    End Property

    Public Property SpecialiteId As Long
        Get
            Return _specialiteId
        End Get
        Set(value As Long)
            _specialiteId = value
        End Set
    End Property

    Public Property Nom As String
        Get
            Return _nom
        End Get
        Set(value As String)
            _nom = value
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

    Public Property StructureId As Long
        Get
            Return _StructureId
        End Get
        Set(value As Long)
            _StructureId = value
        End Set
    End Property

    Public Property StructureNom As String
        Get
            Return _structureNom
        End Get
        Set(value As String)
            _structureNom = value
        End Set
    End Property

    Public Property Adresse1 As String
        Get
            Return _adresse1
        End Get
        Set(value As String)
            _adresse1 = value
        End Set
    End Property

    Public Property Adresse2 As String
        Get
            Return _adresse2
        End Get
        Set(value As String)
            _adresse2 = value
        End Set
    End Property

    Public Property CodePostal As String
        Get
            Return _codePostal
        End Get
        Set(value As String)
            _codePostal = value
        End Set
    End Property

    Public Property Ville As String
        Get
            Return _ville
        End Get
        Set(value As String)
            _ville = value
        End Set
    End Property

    Public Property Code As String
        Get
            Return _code
        End Get
        Set(value As String)
            _code = value
        End Set
    End Property

    Public Property Telephone As String
        Get
            Return _telephone
        End Get
        Set(value As String)
            _telephone = value
        End Set
    End Property

    Public Property Email As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
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

    Public Property Rpps As Long
        Get
            Return _rpps
        End Get
        Set(value As Long)
            _rpps = value
        End Set
    End Property

    Public Property Finess As Long
        Get
            Return _finess
        End Get
        Set(value As Long)
            _finess = value
        End Set
    End Property

    Public Property Adeli As Long
        Get
            Return _adeli
        End Get
        Set(value As Long)
            _adeli = value
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

    Public Property Oasis As Boolean
        Get
            Return _oasis
        End Get
        Set(value As Boolean)
            _oasis = value
        End Set
    End Property

    Public Function Clone() As Ror
        Dim newInstance As Ror = DirectCast(Me.MemberwiseClone(), Ror)
        Return newInstance
    End Function

End Class
