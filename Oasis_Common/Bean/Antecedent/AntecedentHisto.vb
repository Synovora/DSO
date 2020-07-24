Public Class AntecedentHisto
    Property HistorisationDate As Date
    Property UtilisateurId As Integer
    Property Etat As Integer
    Property AntecedentId As Integer
    Property PatientId As Integer
    Property Diagnostic As Integer
    Property Type As String
    Property DrcId As String
    Property Description As String
    Property DateCreation As Date
    Property UtilisateurCreation As Integer
    Property DateModification As Date
    Property UtilisateurModification As Integer
    Property DateDebut As Date
    Property DateFin As Date
    Property AldId As Integer
    Property AldCim10Id As Integer
    Property AldValide As Boolean
    Property AldDateDebut As Date
    Property AldDateFin As Date
    Property AldDemandeEnCours As Boolean
    Property AldDateDemande As Date
    Property Arret As Boolean
    Property ArretCommentaire As String
    Property Nature As String
    Property Niveau As Integer
    Property Niveau1Id As Integer
    Property Niveau2Id As Integer
    Property Ordre1 As Integer
    Property Ordre2 As Integer
    Property Ordre3 As Integer
    Property StatutAffichage As String
    Property Categorie As String
    Property Inactif As Boolean

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

End Class
