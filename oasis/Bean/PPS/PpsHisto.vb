Public Class PpsHisto
    Private _HistorisationDate As Date
    Private _HistorisationUtilisateurId As Integer
    Private _HistorisationEtat As Integer
    Private _PpsId As Integer
    Private _PatientId As Integer
    Private _categorie As Integer
    Private _sousCategorie As Integer
    Private _priorite As Integer
    Private _DrcId As Integer
    Private _affichageSynthese As Boolean
    Private _commentaire As String
    Private _dateDebut As Date
    Private _arret As Boolean
    Private _arretCommentaire As String
    Private _inactif As Boolean

    Public Property HistorisationDate As Date
        Get
            Return _HistorisationDate
        End Get
        Set(value As Date)
            _HistorisationDate = value
        End Set
    End Property

    Public Property HistorisationUtilisateurId As Integer
        Get
            Return _HistorisationUtilisateurId
        End Get
        Set(value As Integer)
            _HistorisationUtilisateurId = value
        End Set
    End Property

    Public Property HistorisationEtat As Integer
        Get
            Return _HistorisationEtat
        End Get
        Set(value As Integer)
            _HistorisationEtat = value
        End Set
    End Property

    Public Property PpsId As Integer
        Get
            Return _PpsId
        End Get
        Set(value As Integer)
            _PpsId = value
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

    Public Property Categorie As Integer
        Get
            Return _categorie
        End Get
        Set(value As Integer)
            _categorie = value
        End Set
    End Property

    Public Property SousCategorie As Integer
        Get
            Return _sousCategorie
        End Get
        Set(value As Integer)
            _sousCategorie = value
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
            Return _DrcId
        End Get
        Set(value As Integer)
            _DrcId = value
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

    Public Property Inactif As Boolean
        Get
            Return _inactif
        End Get
        Set(value As Boolean)
            _inactif = value
        End Set
    End Property

    Sub New()
        InitInstance()
    End Sub
    Private Sub InitInstance()
        HistorisationDate = Nothing
        HistorisationUtilisateurId = 0
        HistorisationEtat = 0
        PpsId = 0
        PatientId = 0
        Categorie = 0
        SousCategorie = 0
        Priorite = 0
        DrcId = 0
        AffichageSynthese = False
        Commentaire = ""
        DateDebut = Nothing
        Arret = False
        ArretCommentaire = ""
        Inactif = False
    End Sub

End Class


