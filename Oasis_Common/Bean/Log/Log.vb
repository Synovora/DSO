Public Class Log

    Public Enum EnumTypeLog
        ERREUR
        INFO
    End Enum

    Property Id As Long
    Property Description As String
    Property Origine As String
    Property TypeLog As String
    Property DateLog As Date
    Property UserLog As Utilisateur

End Class
