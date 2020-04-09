Public Class Traitement
    Private _traitementId As Integer
    Private _patientId As Integer
    Private _medicamentCis As Integer
    Private _medicamentMonographie As Integer
    Private _medicamentDci As String
    Private _denomination_longue As String
    Private _userCreation As Integer
    Private _dateCreation As Date
    Private _userModification As Integer
    Private _dateModification As Date
    Private _dateDebut As Date
    Private _dateFin As Date
    Private _ordreAffichage As Integer
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
    Private _fenetre As Boolean
    Private _fenetreDateDebut As Date
    Private _fenetreDateFin As Date
    Private _fenetreCommentaire As String
    Private _commentaire As String
    Private _arret As String
    Private _arretCommentaire As String
    Private _allergie As Boolean
    Private _declaratifHorsTraitement As Boolean
    Private _contreIndication As Boolean
    Private _annulation As String
    Private _annulationCommentaire As String

    Public Property TraitementId As Integer
        Get
            Return _traitementId
        End Get
        Set(value As Integer)
            _traitementId = value
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

    Public Property MedicamentId As Integer
        Get
            Return _medicamentCis
        End Get
        Set(value As Integer)
            _medicamentCis = value
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

    Public Property DateDebut As Date
        Get
            Return _dateDebut
        End Get
        Set(value As Date)
            _dateDebut = value
        End Set
    End Property

    Public Property OrdreAffichage As Integer
        Get
            Return _ordreAffichage
        End Get
        Set(value As Integer)
            _ordreAffichage = value
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

    Public Property PosologieCommentaire As String
        Get
            Return _posologieCommentaire
        End Get
        Set(value As String)
            _posologieCommentaire = value
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

    Public Property Commentaire As String
        Get
            Return _commentaire
        End Get
        Set(value As String)
            _commentaire = value
        End Set
    End Property

    Public Property Arret As String
        Get
            Return _arret
        End Get
        Set(value As String)
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

    Public Property Allergie As Boolean
        Get
            Return _allergie
        End Get
        Set(value As Boolean)
            _allergie = value
        End Set
    End Property

    Public Property ContreIndication As Boolean
        Get
            Return _contreIndication
        End Get
        Set(value As Boolean)
            _contreIndication = value
        End Set
    End Property

    Public Property Annulation As String
        Get
            Return _annulation
        End Get
        Set(value As String)
            _annulation = value
        End Set
    End Property

    Public Property AnnulationCommentaire As String
        Get
            Return _annulationCommentaire
        End Get
        Set(value As String)
            _annulationCommentaire = value
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

    Public Property DateFin As Date
        Get
            Return _dateFin
        End Get
        Set(value As Date)
            _dateFin = value
        End Set
    End Property

    Public Property DeclaratifHorsTraitement As Boolean
        Get
            Return _declaratifHorsTraitement
        End Get
        Set(value As Boolean)
            _declaratifHorsTraitement = value
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

    Public Property DenominationLongue As String
        Get
            Return _denomination_longue
        End Get
        Set(value As String)
            _denomination_longue = value
        End Set
    End Property

    Public Property MedicamentMonographie As Integer
        Get
            Return _medicamentMonographie
        End Get
        Set(value As Integer)
            _medicamentMonographie = value
        End Set
    End Property

    Public Function Clone() As Traitement
        Dim newInstance As Traitement = DirectCast(Me.MemberwiseClone(), Traitement)
        Return newInstance
    End Function

End Class


