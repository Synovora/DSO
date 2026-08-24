''' <summary>
''' Renommage d'un document. Les identifiants passent par l'en-tête Authorization,
''' plus par le corps de la requête.
''' </summary>
Public Class RenameRequest
    Property OldName As String
    Property NewName As String

End Class
