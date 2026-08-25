Imports System.Net
Imports System.Net.Http
Imports System.Threading
Imports System.Threading.Tasks

''' <summary>
''' Transport HTTP de test : enregistre la dernière requête reçue et rend la
''' réponse préparée. Se branche sur ApiOasis par son second constructeur.
''' </summary>
Friend Class FauxServeur
    Inherits HttpMessageHandler

    ''' <summary>Statut de la réponse. 202 est ce que l'API renvoie en cas de succès.</summary>
    Public Property Statut As HttpStatusCode = HttpStatusCode.Accepted
    Public Property Motif As String = Nothing
    Public Property CorpsReponse As HttpContent = Nothing

    Public Property Appels As Integer
    Public Property Requete As HttpRequestMessage
    ''' <summary>Corps de la requête, lu avant que le client ne le libère.</summary>
    Public Property CorpsRequete As String

    Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
        Appels += 1
        Requete = request
        CorpsRequete = If(request.Content Is Nothing, Nothing, request.Content.ReadAsStringAsync().Result)

        Dim reponse As New HttpResponseMessage(Statut) With {.RequestMessage = request}
        If Motif IsNot Nothing Then reponse.ReasonPhrase = Motif
        reponse.Content = If(CorpsReponse, New StringContent(""))
        Return Task.FromResult(reponse)
    End Function

    Public Shared Function Json(objet As Object) As HttpContent
        Return New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(objet), Text.Encoding.UTF8, "application/json")
    End Function

End Class
