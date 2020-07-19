Public Class Pps
    Private _id As Integer
    Private _patientId As Integer
    Private _categorieId As Integer
    Private _sousCategorieId As Integer
    Private _specialiteId As Integer
    Private _priorite As Integer
    Private _drcId As Integer
    Private _affichageSynthese As Boolean
    Private _commentaire As String
    Private _dateDebut As Date
    Private _arret As Boolean
    Private _arretCommentaire As String
    Private _dateCreation As Date
    Private _userCreation As Integer
    Private _dateModification As Date
    Private _userModification As Integer
    Private _inactif As Boolean

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
            Return _patientId
        End Get
        Set(value As Integer)
            _patientId = value
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

    Public Property SousCategorieId As Integer
        Get
            Return _sousCategorieId
        End Get
        Set(value As Integer)
            _sousCategorieId = value
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

    Public Property Priorite As Integer
        Get
            Return _priorite
        End Get
        Set(value As Integer)
            _priorite = value
        End Set
    End Property

    Public Property DrcId As Integer
        Get
            Return _drcId
        End Get
        Set(value As Integer)
            _drcId = value
        End Set
    End Property

    Public Property AffichageSynthese As Boolean
        Get
            Return _affichageSynthese
        End Get
        Set(value As Boolean)
            _affichageSynthese = value
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

    Public Property DateDebut As Date
        Get
            Return _dateDebut
        End Get
        Set(value As Date)
            _dateDebut = value
        End Set
    End Property

    Public Property Arret As Boolean
        Get
            Return _arret
        End Get
        Set(value As Boolean)
            _arret = value
        End Set
    End Property

    Public Property ArretCommentaire As String
        Get
            Return _arretCommentaire
        End Get
        Set(value As String)
            _arretCommentaire = value
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

    Public Property UserCreation As Integer
        Get
            Return _userCreation
        End Get
        Set(value As Integer)
            _userCreation = value
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

    Public Property UserModification As Integer
        Get
            Return _userModification
        End Get
        Set(value As Integer)
            _userModification = value
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

    Public Function Clone() As Pps
        Dim newInstance As Pps = DirectCast(Me.MemberwiseClone(), Pps)
        Return newInstance
    End Function

End Class
