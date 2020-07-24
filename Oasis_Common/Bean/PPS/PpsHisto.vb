Public Class PpsHisto

    Property HistorisationDate As Date
    Property HistorisationUtilisateurId As Integer
    Property HistorisationEtat As Integer
    Property PpsId As Integer
    Property PatientId As Integer
    Property Categorie As Integer
    Property SousCategorie As Integer
    Property Priorite As Integer
    Property DrcId As Integer
    Property AffichageSynthese As Boolean
    Property Commentaire As String
    Property DateDebut As Date
    Property Arret As Boolean
    Property ArretCommentaire As String
    Property Inactif As Boolean

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


