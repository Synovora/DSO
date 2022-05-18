Public Class Pps

    Public Enum EnumCategoriePPS
        OBJECTIF_SANTE = 1
        MESURE_PREVENTIVE = 2
        SUIVI_INTERVENANT = 3
        STRATEGIE = 4
    End Enum

    Public Enum EnumSousCategoriePPS
        Prophylactique = 7
        Sociale = 8
        Symptomatique = 9
        Curative = 10
        Diagnostique = 11
        Palliative = 12
    End Enum
    Property Id As Integer
    Property PatientId As Integer
    Property CategorieId As Integer
    Property SousCategorieId As Integer
    Property SpecialiteId As Integer
    Property Priorite As Integer
    Property DrcId As Integer
    Property AffichageSynthese As Boolean
    Property Commentaire As String
    Property DateDebut As Date
    Property DateFin As Date
    Property Arret As Boolean
    Property ArretCommentaire As String
    Property DateCreation As Date
    Property UserCreation As Integer
    Property DateModification As Date
    Property UserModification As Integer
    Property Inactif As Boolean

    Public Function Clone() As Pps
        Dim newInstance As Pps = DirectCast(Me.MemberwiseClone(), Pps)
        Return newInstance
    End Function

End Class
