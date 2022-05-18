Public Class Specialite

    Public Structure EnumTypeSavoirFaire
        Const COMPETENCE_EXCLUSIVE = "CEX"
        Const SPECIALITE_ORDINALE = "S"
    End Structure

    Property SpecialiteId As Long
    Property Code As String
    Property Description As String
    Property Nature As String
    Property Parcours As Boolean
    Property Oasis As Boolean
    Property Genre As String
    Property AgeMin As Integer
    Property AgeMax As Integer
    Property DelaiPriseEnCharge As Integer
    Property NosG15CodeProfession As Integer
    Property NosR40TypeSavoirFaire As String
    Property NosCodeSavoirFaire As String

End Class
