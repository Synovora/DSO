Public Class TraitementHisto
    Private privateHistorisationDate As DateTime
    Private PrivateHistorisationUtilisateurId As Integer
    Private PrivateHistorisationEtat As Integer
    Private privateHistorisationPatientId As Integer
    Private PrivateHistorisationTraitementId As Integer
    Private PrivateHistorisationDateDebut As Date
    Private PrivateHistorisationDateFin As Date
    Private PrivateHistorisationCommentaire As String
    Private PrivateHistorisationOrdreAffichage As Integer
    Private PrivateHistorisationPosologieBase As String
    Private PrivateHistorisationPosologieRythme As Integer
    Private PrivateHistorisationPosologieMatin As Boolean
    Private PrivateHistorisationPosologieMidi As Boolean
    Private PrivateHistorisationPosologieApresMidi As Boolean
    Private PrivateHistorisationPosologieSoir As Boolean
    Private PrivateHistorisationPosologieCommentaire As String
    Private PrivateHistorisationFenetre As Boolean
    Private PrivateHistorisationFenetreDateDebut As Date
    Private PrivateHistorisationFenetreDateFin As Date
    Private PrivateHistorisationFenetreCommentaire As String
    Private PrivateHistorisationArret As String
    Private PrivateHistorisationArretCommentaire As String
    Private PrivateHistorisationDeclaratifHorsTraitement As Boolean
    Private PrivateHistorisationAllergie As Boolean
    Private PrivateHistorisationContreIndication As Boolean
    Private PrivateHistorisationAnnulation As String
    Private PrivateHistorisationAnnulationCommentaire As String


    Sub New()
        InitInstance()
    End Sub

    Sub InitInstance()
        Me.HistorisationDate = Nothing
        Me.HistorisationUtilisateurId = 0
        Me.HistorisationEtat = 0
        Me.HistorisationPatientId = 0
        Me.HistorisationTraitementId = 0
        Me.HistorisationDateDebut = Nothing
        Me.HistorisationDateFin = Nothing
        Me.HistorisationCommentaire = ""
        Me.HistorisationOrdreAffichage = 0
        Me.HistorisationPosologieBase = ""
        Me.HistorisationPosologieRythme = 0
        Me.HistorisationPosologieMatin = False
        Me.HistorisationPosologieMidi = False
        Me.HistorisationPosologieApresMidi = False
        Me.HistorisationPosologieSoir = False
        Me.HistorisationPosologieCommentaire = ""
        Me.HistorisationFenetre = False
        Me.HistorisationFenetreDateDebut = Nothing
        Me.HistorisationFenetreDateFin = Nothing
        Me.HistorisationFenetreCommentaire = ""
        Me.HistorisationArret = " "
        Me.HistorisationArretCommentaire = ""
        Me.HistorisationDeclaratifHorsTraitement = False
        Me.HistorisationAllergie = False
        Me.HistorisationContreIndication = False
        Me.HistorisationAnnulation = " "
        Me.HistorisationAnnulationCommentaire = ""
    End Sub

    Public Property HistorisationDate As DateTime
        Get
            Return privateHistorisationDate
        End Get
        Set(value As DateTime)
            privateHistorisationDate = value
        End Set
    End Property

    Public Property HistorisationUtilisateurId As Integer
        Get
            Return PrivateHistorisationUtilisateurId
        End Get
        Set(value As Integer)
            PrivateHistorisationUtilisateurId = value
        End Set
    End Property

    Public Property HistorisationEtat As Integer
        Get
            Return PrivateHistorisationEtat
        End Get
        Set(value As Integer)
            PrivateHistorisationEtat = value
        End Set
    End Property

    Public Property HistorisationTraitementId As Integer
        Get
            Return PrivateHistorisationTraitementId
        End Get
        Set(value As Integer)
            PrivateHistorisationTraitementId = value
        End Set
    End Property

    Public Property HistorisationDateDebut As Date
        Get
            Return PrivateHistorisationDateDebut
        End Get
        Set(value As Date)
            PrivateHistorisationDateDebut = value
        End Set
    End Property

    Public Property HistorisationDateFin As Date
        Get
            Return PrivateHistorisationDateFin
        End Get
        Set(value As Date)
            PrivateHistorisationDateFin = value
        End Set
    End Property

    Public Property HistorisationCommentaire As String
        Get
            Return PrivateHistorisationCommentaire
        End Get
        Set(value As String)
            PrivateHistorisationCommentaire = value
        End Set
    End Property

    Public Property HistorisationOrdreAffichage As Integer
        Get
            Return PrivateHistorisationOrdreAffichage
        End Get
        Set(value As Integer)
            PrivateHistorisationOrdreAffichage = value
        End Set
    End Property

    Public Property HistorisationPosologieBase As String
        Get
            Return PrivateHistorisationPosologieBase
        End Get
        Set(value As String)
            PrivateHistorisationPosologieBase = value
        End Set
    End Property

    Public Property HistorisationPosologieRythme As Integer
        Get
            Return PrivateHistorisationPosologieRythme
        End Get
        Set(value As Integer)
            PrivateHistorisationPosologieRythme = value
        End Set
    End Property

    Public Property HistorisationPosologieMatin As Boolean
        Get
            Return PrivateHistorisationPosologieMatin
        End Get
        Set(value As Boolean)
            PrivateHistorisationPosologieMatin = value
        End Set
    End Property

    Public Property HistorisationPosologieMidi As Boolean
        Get
            Return PrivateHistorisationPosologieMidi
        End Get
        Set(value As Boolean)
            PrivateHistorisationPosologieMidi = value
        End Set
    End Property

    Public Property HistorisationPosologieApresMidi As Boolean
        Get
            Return PrivateHistorisationPosologieApresMidi
        End Get
        Set(value As Boolean)
            PrivateHistorisationPosologieApresMidi = value
        End Set
    End Property

    Public Property HistorisationPosologieSoir As Boolean
        Get
            Return PrivateHistorisationPosologieSoir
        End Get
        Set(value As Boolean)
            PrivateHistorisationPosologieSoir = value
        End Set
    End Property

    Public Property HistorisationPosologieCommentaire As String
        Get
            Return PrivateHistorisationPosologieCommentaire
        End Get
        Set(value As String)
            PrivateHistorisationPosologieCommentaire = value
        End Set
    End Property

    Public Property HistorisationFenetre As Boolean
        Get
            Return PrivateHistorisationFenetre
        End Get
        Set(value As Boolean)
            PrivateHistorisationFenetre = value
        End Set
    End Property

    Public Property HistorisationFenetreDateDebut As Date
        Get
            Return PrivateHistorisationFenetreDateDebut
        End Get
        Set(value As Date)
            PrivateHistorisationFenetreDateDebut = value
        End Set
    End Property

    Public Property HistorisationFenetreDateFin As Date
        Get
            Return PrivateHistorisationFenetreDateFin
        End Get
        Set(value As Date)
            PrivateHistorisationFenetreDateFin = value
        End Set
    End Property

    Public Property HistorisationFenetreCommentaire As String
        Get
            Return PrivateHistorisationFenetreCommentaire
        End Get
        Set(value As String)
            PrivateHistorisationFenetreCommentaire = value
        End Set
    End Property

    Public Property HistorisationArret As String
        Get
            Return PrivateHistorisationArret
        End Get
        Set(value As String)
            PrivateHistorisationArret = value
        End Set
    End Property

    Public Property HistorisationArretCommentaire As String
        Get
            Return PrivateHistorisationArretCommentaire
        End Get
        Set(value As String)
            PrivateHistorisationArretCommentaire = value
        End Set
    End Property

    Public Property HistorisationDeclaratifHorsTraitement As Boolean
        Get
            Return PrivateHistorisationDeclaratifHorsTraitement
        End Get
        Set(value As Boolean)
            PrivateHistorisationDeclaratifHorsTraitement = value
        End Set
    End Property

    Public Property HistorisationAllergie As Boolean
        Get
            Return PrivateHistorisationAllergie
        End Get
        Set(value As Boolean)
            PrivateHistorisationAllergie = value
        End Set
    End Property

    Public Property HistorisationContreIndication As Boolean
        Get
            Return PrivateHistorisationContreIndication
        End Get
        Set(value As Boolean)
            PrivateHistorisationContreIndication = value
        End Set
    End Property

    Public Property HistorisationAnnulation As String
        Get
            Return PrivateHistorisationAnnulation
        End Get
        Set(value As String)
            PrivateHistorisationAnnulation = value
        End Set
    End Property

    Public Property HistorisationAnnulationCommentaire As String
        Get
            Return PrivateHistorisationAnnulationCommentaire
        End Get
        Set(value As String)
            PrivateHistorisationAnnulationCommentaire = value
        End Set
    End Property

    Public Property HistorisationPatientId As Integer
        Get
            Return privateHistorisationPatientId
        End Get
        Set(value As Integer)
            privateHistorisationPatientId = value
        End Set
    End Property
End Class


