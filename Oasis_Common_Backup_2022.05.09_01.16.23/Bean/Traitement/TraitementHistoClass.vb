Public Class TraitementHisto

    Property HistorisationDate As DateTime
    Property HistorisationUtilisateurId As Integer
    Property HistorisationEtat As Integer
    Property HistorisationPatientId As Integer
    Property HistorisationTraitementId As Integer
    Property HistorisationMedicamentId As Integer
    Property HistorisationMedicamentDci As String
    Property HistorisationDateDebut As Date
    Property HistorisationDateFin As Date
    Property HistorisationCommentaire As String
    Property HistorisationOrdreAffichage As Integer
    Property HistorisationPosologieBase As String
    Property HistorisationPosologieRythme As Integer
    Property HistorisationPosologieMatin As Integer
    Property HistorisationPosologieMidi As Integer
    Property HistorisationPosologieApresMidi As Integer
    Property HistorisationPosologieSoir As Integer
    Property HistorisationFractionMatin As String
    Property HistorisationFractionMidi As String
    Property HistorisationFractionApresMidi As String
    Property HistorisationFractionSoir As String
    Property HistorisationPosologieCommentaire As String
    Property HistorisationFenetre As Boolean
    Property HistorisationFenetreDateDebut As Date
    Property HistorisationFenetreDateFin As Date
    Property HistorisationFenetreCommentaire As String
    Property HistorisationArret As String
    Property HistorisationArretCommentaire As String
    Property HistorisationDeclaratifHorsTraitement As Boolean
    Property HistorisationAllergie As Boolean
    Property HistorisationContreIndication As Boolean
    Property HistorisationAnnulation As String
    Property HistorisationAnnulationCommentaire As String


    Sub New()
        InitInstance()
    End Sub

    Sub InitInstance()
        Me.HistorisationDate = Nothing
        Me.HistorisationUtilisateurId = 0
        Me.HistorisationEtat = 0
        Me.HistorisationPatientId = 0
        Me.HistorisationTraitementId = 0
        Me.HistorisationMedicamentId = 0
        Me.HistorisationMedicamentDci = ""
        Me.HistorisationDateDebut = Nothing
        Me.HistorisationDateFin = Nothing
        Me.HistorisationCommentaire = ""
        Me.HistorisationOrdreAffichage = 0
        Me.HistorisationPosologieBase = ""
        Me.HistorisationPosologieRythme = 0
        Me.HistorisationPosologieMatin = 0
        Me.HistorisationPosologieMidi = 0
        Me.HistorisationPosologieApresMidi = 0
        Me.HistorisationPosologieSoir = 0
        Me.HistorisationFractionMatin = ""
        Me.HistorisationFractionMidi = ""
        Me.HistorisationFractionApresMidi = False
        Me.HistorisationFractionSoir = False
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

End Class


