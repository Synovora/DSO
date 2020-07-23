Public Class Parametre

    Public Enum EnumParametreId
        POIDS = 1
        TAILLE = 2
        IMC = 3
        PAM = 8
        PAS = 6
        PAD = 7
    End Enum

    Property Id As Long
    Property Description As String
    Property Entier As Integer
    Property [Decimal] As Integer
    Property Unite As String
    Property ValeurMin As Decimal
    Property ValeurMax As Decimal
    Property Ordre As Integer
    Property Inactif As Boolean
    Property DescriptionPatient As String
    Property ExclusionAutoSuivi As String

End Class
