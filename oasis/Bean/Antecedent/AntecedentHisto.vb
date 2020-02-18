Public Class AntecedentHisto
    Private privateHistorisationDate As Date
    Private PrivateHistorisationUtilisateurId As Integer
    Private PrivateHistorisationEtat As Integer
    Private privateHistorisationAntecedentId As Integer
    Private privateHistorisationPatientId As Integer
    Private privateHistorisationDiagnostic As Integer
    Private privateHistorisationType As String
    Private privateHistorisationDrcId As String
    Private privateHistorisationDescription As String
    Private privateHistorisationDateCreation As Date
    Private privateHistorisationUtilisateurCreation As Integer
    Private privateHistorisationDateModification As Date
    Private privateHistorisationUtilisateurModification As Integer
    Private privateHistorisationDateDebut As Date
    Private privateHistorisationDateFin As Date
    Private _aldId As Integer
    Private _aldCim10Id As Integer
    Private _aldValide As Boolean
    Private _aldDateDebut As Date
    Private _aldDateFin As Date
    Private _aldDemandeEnCours As Boolean
    Private _aldDateDemande As Date
    Private privateHistorisationArret As Boolean
    Private privateHistorisationArretCommentaire As String
    Private privateHistorisationNature As String
    Private privateHistorisationNiveau As Integer
    Private privateHistorisationAntecedentId1 As Integer
    Private privateHistorisationAntecedentId2 As Integer
    Private privateHistorisationOrdreAffichage1 As Integer
    Private privateHistorisationOrdreAffichage2 As Integer
    Private privateHistorisationOrdreAffichage3 As Integer
    Private privateHistorisationStatutAffichage As String
    Private privateHistorisationCategorie As String
    Private privateHistorisationInactif As Boolean

    Sub New()
        InitInstance()
    End Sub

    Sub InitInstance()
        Me.HistorisationDate = Nothing
        Me.UtilisateurId = 0
        Me.Etat = 0
        Me.AntecedentId = 0
        Me.PatientId = 0
        Me.Diagnostic = 0
        Me.Type = ""
        Me.DrcId = ""
        Me.Description = ""
        Me.DateCreation = Nothing
        Me.UtilisateurCreation = 0
        Me.DateModification = Nothing
        Me.UtilisateurModification = 0
        Me.DateDebut = Nothing
        Me.DateFin = Nothing
        Me.Arret = False
        Me.ArretCommentaire = ""
        Me.Nature = ""
        Me.Niveau = 0
        Me.Niveau1Id = 0
        Me.Niveau2Id = 0
        Me.Ordre1 = 0
        Me.Ordre2 = 0
        Me.Ordre3 = 0
        Me.StatutAffichage = ""
        Me.Categorie = ""
        Me.Inactif = False
        Me.AldId = 0
        Me.AldDateDebut = Nothing
        Me.AldDateFin = Nothing
        Me.AldValide = False
        Me.AldDateDemande = Nothing
        Me.AldDemandeEnCours = False
        Me.AldCim10Id = 0
    End Sub

    Public Property HistorisationDate As Date
        Get
            Return privateHistorisationDate
        End Get
        Set(value As Date)
            privateHistorisationDate = value
        End Set
    End Property

    Public Property UtilisateurId As Integer
        Get
            Return PrivateHistorisationUtilisateurId
        End Get
        Set(value As Integer)
            PrivateHistorisationUtilisateurId = value
        End Set
    End Property

    Public Property Etat As Integer
        Get
            Return PrivateHistorisationEtat
        End Get
        Set(value As Integer)
            PrivateHistorisationEtat = value
        End Set
    End Property

    Public Property AntecedentId As Integer
        Get
            Return privateHistorisationAntecedentId
        End Get
        Set(value As Integer)
            privateHistorisationAntecedentId = value
        End Set
    End Property

    Public Property PatientId As Integer
        Get
            Return privateHistorisationPatientId
        End Get
        Set(value As Integer)
            privateHistorisationPatientId = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return privateHistorisationType
        End Get
        Set(value As String)
            privateHistorisationType = value
        End Set
    End Property

    Public Property DrcId As String
        Get
            Return privateHistorisationDrcId
        End Get
        Set(value As String)
            privateHistorisationDrcId = value
        End Set
    End Property

    Public Property Description As String
        Get
            Return privateHistorisationDescription
        End Get
        Set(value As String)
            privateHistorisationDescription = value
        End Set
    End Property

    Public Property DateCreation As Date
        Get
            Return privateHistorisationDateCreation
        End Get
        Set(value As Date)
            privateHistorisationDateCreation = value
        End Set
    End Property

    Public Property UtilisateurCreation As Integer
        Get
            Return privateHistorisationUtilisateurCreation
        End Get
        Set(value As Integer)
            privateHistorisationUtilisateurCreation = value
        End Set
    End Property

    Public Property DateModification As Date
        Get
            Return privateHistorisationDateModification
        End Get
        Set(value As Date)
            privateHistorisationDateModification = value
        End Set
    End Property

    Public Property UtilisateurModification As Integer
        Get
            Return privateHistorisationUtilisateurModification
        End Get
        Set(value As Integer)
            privateHistorisationUtilisateurModification = value
        End Set
    End Property

    Public Property DateDebut As Date
        Get
            Return privateHistorisationDateDebut
        End Get
        Set(value As Date)
            privateHistorisationDateDebut = value
        End Set
    End Property

    Public Property DateFin As Date
        Get
            Return privateHistorisationDateFin
        End Get
        Set(value As Date)
            privateHistorisationDateFin = value
        End Set
    End Property

    Public Property Arret As Boolean
        Get
            Return privateHistorisationArret
        End Get
        Set(value As Boolean)
            privateHistorisationArret = value
        End Set
    End Property

    Public Property ArretCommentaire As String
        Get
            Return privateHistorisationArretCommentaire
        End Get
        Set(value As String)
            privateHistorisationArretCommentaire = value
        End Set
    End Property

    Public Property Nature As String
        Get
            Return privateHistorisationNature
        End Get
        Set(value As String)
            privateHistorisationNature = value
        End Set
    End Property

    Public Property Niveau As Integer
        Get
            Return privateHistorisationNiveau
        End Get
        Set(value As Integer)
            privateHistorisationNiveau = value
        End Set
    End Property

    Public Property Niveau1Id As Integer
        Get
            Return privateHistorisationAntecedentId1
        End Get
        Set(value As Integer)
            privateHistorisationAntecedentId1 = value
        End Set
    End Property

    Public Property Niveau2Id As Integer
        Get
            Return privateHistorisationAntecedentId2
        End Get
        Set(value As Integer)
            privateHistorisationAntecedentId2 = value
        End Set
    End Property

    Public Property Ordre1 As Integer
        Get
            Return privateHistorisationOrdreAffichage1
        End Get
        Set(value As Integer)
            privateHistorisationOrdreAffichage1 = value
        End Set
    End Property

    Public Property Ordre2 As Integer
        Get
            Return privateHistorisationOrdreAffichage2
        End Get
        Set(value As Integer)
            privateHistorisationOrdreAffichage2 = value
        End Set
    End Property

    Public Property Ordre3 As Integer
        Get
            Return privateHistorisationOrdreAffichage3
        End Get
        Set(value As Integer)
            privateHistorisationOrdreAffichage3 = value
        End Set
    End Property

    Public Property StatutAffichage As String
        Get
            Return privateHistorisationStatutAffichage
        End Get
        Set(value As String)
            privateHistorisationStatutAffichage = value
        End Set
    End Property

    Public Property Categorie As String
        Get
            Return privateHistorisationCategorie
        End Get
        Set(value As String)
            privateHistorisationCategorie = value
        End Set
    End Property

    Public Property Inactif As Boolean
        Get
            Return privateHistorisationInactif
        End Get
        Set(value As Boolean)
            privateHistorisationInactif = value
        End Set
    End Property

    Public Property Diagnostic As Integer
        Get
            Return privateHistorisationDiagnostic
        End Get
        Set(value As Integer)
            privateHistorisationDiagnostic = value
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
End Class
