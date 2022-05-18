Public Class ContexteCourrier

    Public Structure EnumParcoursBaseItem
        Const Medical = "Médical"
        Const BioEnvironnemental = "Bio-environnemental"
    End Structure

    Public Structure EnumParcoursBaseCode
        Const Medical = "M"
        Const BioEnvironnemental = "B"
    End Structure

    Public Enum EnumDiagnostic
        SUSPICION_DE = 2
        NOTION_DE = 3
    End Enum

    Property Id As Long
    Property PatientId As Long
    Property Description As String

End Class
