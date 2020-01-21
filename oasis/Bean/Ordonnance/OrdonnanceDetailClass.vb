Public Class OrdonnanceDetail
    Private _ligneId As Integer
    Private _OrdonnanceId As Integer
    Private _traitement As Boolean
    Private _TraitementId As Integer
    Private _ald As Boolean
    Private _aDelivrer As Boolean
    Private _OrdreAffichage As Integer
    Private _medicamentCis As Integer
    Private _medicamentDci As String
    Private _dateDebut As Date
    Private _dateFin As Date
    Private _duree As Integer
    Private _posologieBase As String
    Private _posologieRythme As Integer
    Private _posologieMatin As Integer
    Private _posologieMidi As Integer
    Private _posologieApresMidi As Integer
    Private _posologieSoir As Integer
    Private _fractionMatin As String
    Private _fractionMidi As String
    Private _fractionApresMidi As String
    Private _fractionSoir As String
    Private _posologieCommentaire As String
    Private _Commentaire As String
    Private _fenetre As Boolean
    Private _fenetreDateDebut As Date
    Private _fenetreDateFin As Date
    Private _fenetreCommentaire As String
    Private _inactif As Boolean

    Public Property LigneId As Integer
        Get
            Return _ligneId
        End Get
        Set(value As Integer)
            _ligneId = value
        End Set
    End Property

    Public Property OrdonnanceId As Integer
        Get
            Return _OrdonnanceId
        End Get
        Set(value As Integer)
            _OrdonnanceId = value
        End Set
    End Property

    Public Property Traitement As Boolean
        Get
            Return _traitement
        End Get
        Set(value As Boolean)
            _traitement = value
        End Set
    End Property

    Public Property TraitementId As Integer
        Get
            Return _TraitementId
        End Get
        Set(value As Integer)
            _TraitementId = value
        End Set
    End Property

    Public Property OrdreAffichage As Integer
        Get
            Return _OrdreAffichage
        End Get
        Set(value As Integer)
            _OrdreAffichage = value
        End Set
    End Property

    Public Property MedicamentCis As Integer
        Get
            Return _medicamentCis
        End Get
        Set(value As Integer)
            _medicamentCis = value
        End Set
    End Property

    Public Property MedicamentDci As String
        Get
            Return _medicamentDci
        End Get
        Set(value As String)
            _medicamentDci = value
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

    Public Property DateFin As Date
        Get
            Return _dateFin
        End Get
        Set(value As Date)
            _dateFin = value
        End Set
    End Property

    Public Property PosologieBase As String
        Get
            Return _posologieBase
        End Get
        Set(value As String)
            _posologieBase = value
        End Set
    End Property

    Public Property PosologieRythme As Integer
        Get
            Return _posologieRythme
        End Get
        Set(value As Integer)
            _posologieRythme = value
        End Set
    End Property

    Public Property PosologieMatin As Integer
        Get
            Return _posologieMatin
        End Get
        Set(value As Integer)
            _posologieMatin = value
        End Set
    End Property

    Public Property PosologieMidi As Integer
        Get
            Return _posologieMidi
        End Get
        Set(value As Integer)
            _posologieMidi = value
        End Set
    End Property

    Public Property PosologieApresMidi As Integer
        Get
            Return _posologieApresMidi
        End Get
        Set(value As Integer)
            _posologieApresMidi = value
        End Set
    End Property

    Public Property PosologieSoir As Integer
        Get
            Return _posologieSoir
        End Get
        Set(value As Integer)
            _posologieSoir = value
        End Set
    End Property

    Public Property Commentaire As String
        Get
            Return _Commentaire
        End Get
        Set(value As String)
            _Commentaire = value
        End Set
    End Property

    Public Property Fenetre As Boolean
        Get
            Return _fenetre
        End Get
        Set(value As Boolean)
            _fenetre = value
        End Set
    End Property

    Public Property FenetreDateDebut As Date
        Get
            Return _fenetreDateDebut
        End Get
        Set(value As Date)
            _fenetreDateDebut = value
        End Set
    End Property

    Public Property FenetreDateFin As Date
        Get
            Return _fenetreDateFin
        End Get
        Set(value As Date)
            _fenetreDateFin = value
        End Set
    End Property

    Public Property FenetreCommentaire As String
        Get
            Return _fenetreCommentaire
        End Get
        Set(value As String)
            _fenetreCommentaire = value
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

    Public Property PosologieCommentaire As String
        Get
            Return _posologieCommentaire
        End Get
        Set(value As String)
            _posologieCommentaire = value
        End Set
    End Property

    Public Property Ald As Boolean
        Get
            Return _ald
        End Get
        Set(value As Boolean)
            _ald = value
        End Set
    End Property

    Public Property FractionMatin As String
        Get
            Return _fractionMatin
        End Get
        Set(value As String)
            _fractionMatin = value
        End Set
    End Property

    Public Property FractionMidi As String
        Get
            Return _fractionMidi
        End Get
        Set(value As String)
            _fractionMidi = value
        End Set
    End Property

    Public Property FractionApresMidi As String
        Get
            Return _fractionApresMidi
        End Get
        Set(value As String)
            _fractionApresMidi = value
        End Set
    End Property

    Public Property FractionSoir As String
        Get
            Return _fractionSoir
        End Get
        Set(value As String)
            _fractionSoir = value
        End Set
    End Property

    Public Property ADelivrer As Boolean
        Get
            Return _aDelivrer
        End Get
        Set(value As Boolean)
            _aDelivrer = value
        End Set
    End Property

    Public Property Duree As Integer
        Get
            Return _duree
        End Get
        Set(value As Integer)
            _duree = value
        End Set
    End Property

    Public Function Clone() As OrdonnanceDetail
        Dim newInstance As OrdonnanceDetail = DirectCast(Me.MemberwiseClone(), OrdonnanceDetail)
        Return newInstance
    End Function

End Class
