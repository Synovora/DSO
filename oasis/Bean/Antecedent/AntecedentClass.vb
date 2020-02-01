Public Class Antecedent
    Private _id As Integer
    Private _patientId As Integer
    Private _type As String
    Private _drcId As Integer
    Private _description As String
    Private _dateCreation As Date
    Private _userCreation As Integer
    Private _dateModification As Date
    Private _userModification As Integer
    Private _diagnostic As Integer
    Private _dateDebut As DateTime
    Private _dateFin As Date
    Private _aldId As Integer
    Private _aldCim10Id As Integer
    Private _aldValide As Boolean
    Private _aldDateDebut As Date
    Private _aldDateFin As Date
    Private _aldDemandeEnCours As Boolean
    Private _aldDateDemande As Date
    Private _arret As Boolean
    Private _arretCommentaire As String
    Private _nature As String
    Private _priorite As String
    Private _niveau As Integer
    Private _niveau1Id As Integer
    Private _niveau2Id As Integer
    Private _ordre1 As Integer
    Private _ordre2 As Integer
    Private _ordre3 As Integer
    Private _statutAffichage As String
    Private _categorieContexte As String
    Private _episodeId As Long
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

    Public Property Type As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
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

    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
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

    Public Property Diagnostic As Integer
        Get
            Return _diagnostic
        End Get
        Set(value As Integer)
            _diagnostic = value
        End Set
    End Property

    Public Property DateDebut As DateTime
        Get
            Return _dateDebut
        End Get
        Set(value As DateTime)
            _dateDebut = value
        End Set
    End Property

    Public Property DateFin As Date
        Get
            Return _dateFin
        End Get
        Set(value As Date)
            _dateFin = value
        End Set
    End Property

    Public Property AldId As Integer
        Get
            Return _aldId
        End Get
        Set(value As Integer)
            _aldId = value
        End Set
    End Property

    Public Property AldCim10Id As Integer
        Get
            Return _aldCim10Id
        End Get
        Set(value As Integer)
            _aldCim10Id = value
        End Set
    End Property

    Public Property AldValide As Boolean
        Get
            Return _aldValide
        End Get
        Set(value As Boolean)
            _aldValide = value
        End Set
    End Property

    Public Property AldDateDebut As Date
        Get
            Return _aldDateDebut
        End Get
        Set(value As Date)
            _aldDateDebut = value
        End Set
    End Property

    Public Property AldDateFin As Date
        Get
            Return _aldDateFin
        End Get
        Set(value As Date)
            _aldDateFin = value
        End Set
    End Property

    Public Property AldDemandeEnCours As Boolean
        Get
            Return _aldDemandeEnCours
        End Get
        Set(value As Boolean)
            _aldDemandeEnCours = value
        End Set
    End Property

    Public Property AldDateDemande As Date
        Get
            Return _aldDateDemande
        End Get
        Set(value As Date)
            _aldDateDemande = value
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

    Public Property Nature As String
        Get
            Return _nature
        End Get
        Set(value As String)
            _nature = value
        End Set
    End Property

    Public Property Priorite As String
        Get
            Return _priorite
        End Get
        Set(value As String)
            _priorite = value
        End Set
    End Property

    Public Property Niveau As Integer
        Get
            Return _niveau
        End Get
        Set(value As Integer)
            _niveau = value
        End Set
    End Property

    Public Property Niveau1Id As Integer
        Get
            Return _niveau1Id
        End Get
        Set(value As Integer)
            _niveau1Id = value
        End Set
    End Property

    Public Property Niveau2Id As Integer
        Get
            Return _niveau2Id
        End Get
        Set(value As Integer)
            _niveau2Id = value
        End Set
    End Property

    Public Property Ordre1 As Integer
        Get
            Return _ordre1
        End Get
        Set(value As Integer)
            _ordre1 = value
        End Set
    End Property

    Public Property Ordre2 As Integer
        Get
            Return _ordre2
        End Get
        Set(value As Integer)
            _ordre2 = value
        End Set
    End Property

    Public Property Ordre3 As Integer
        Get
            Return _ordre3
        End Get
        Set(value As Integer)
            _ordre3 = value
        End Set
    End Property

    Public Property StatutAffichage As String
        Get
            Return _statutAffichage
        End Get
        Set(value As String)
            _statutAffichage = value
        End Set
    End Property

    Public Property CategorieContexte As String
        Get
            Return _categorieContexte
        End Get
        Set(value As String)
            _categorieContexte = value
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

    Public Property EpisodeId As Long
        Get
            Return _episodeId
        End Get
        Set(value As Long)
            _episodeId = value
        End Set
    End Property

    Public Function Clone() As Antecedent
        Dim newInstance As Antecedent = DirectCast(Me.MemberwiseClone(), Antecedent)
        Return newInstance
    End Function

End Class
