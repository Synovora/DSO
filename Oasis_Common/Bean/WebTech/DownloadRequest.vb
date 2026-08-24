''' <summary>
''' Demande de téléchargement d'un document.
'''
''' Les identifiants ne figurent plus dans le corps : ils passent par l'en-tête
''' Authorization, lu par le filtre d'authentification de l'API.
''' </summary>
Public Class DownloadRequest
    Property FileName As String

End Class
