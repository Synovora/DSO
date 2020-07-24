Public Class Pps

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
