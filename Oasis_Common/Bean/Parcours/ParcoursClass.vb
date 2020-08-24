Public Class Parcours

    Property Id As Integer
    Property PatientId As Integer
    Property SpecialiteId As Integer
    Property CategorieId As Integer
    Property SousCategorieId As Integer
    Property IntervenantOasis As Boolean
    Property RorId As Integer
    Property Commentaire As String
    Property Base As String
    Property Rythme As Integer
    Property Cacher As Boolean
    Property Inactif As Boolean
    Property UserCreation As Integer
    Property DateCreation As Date
    Property UserModification As Integer
    Property DateModification As Date

    Public Function Clone() As Parcours
        Dim newInstance As Parcours = DirectCast(Me.MemberwiseClone(), Parcours)
        Return newInstance
    End Function

End Class
