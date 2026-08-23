Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports Oasis_Common

Public Class DocFileUploadController
    Inherits ApiController

    ' GET api/<controller>
    Public Function GetValues() As String
        Return "API Oasis - Document file controleur "
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Upload() As Task(Of HttpResponseMessage)
        Dim provider As MultipartFormDataStreamProvider = Nothing
        Try
            Dim fileuploadPath = ConfigurationManager.AppSettings("FileUploadLocation")
            ' Les pièces reçues atterrissent d'abord sous un nom généré par le
            ' framework dans un sous-dossier temporaire ; rien n'est écrit à
            ' l'emplacement définitif avant authentification et validation du nom.
            Dim dossierTemporaire = Path.Combine(fileuploadPath, "tmp")
            Directory.CreateDirectory(dossierTemporaire)
            provider = New MultipartFormDataStreamProvider(dossierTemporaire)
            Dim content = New StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(True))

            For Each header In Request.Content.Headers
                content.Headers.TryAddWithoutValidation(header.Key, header.Value)
            Next

            Await content.ReadAsMultipartAsync(provider)
            If provider.FileData Is Nothing OrElse provider.FileData.Count <> 1 Then
                Throw New ArgumentException("Un seul fichier doit être posté !")
            End If

            ' -- on verifie que le login / mot de passe est ok
            Dim login As String = provider.FormData.Item("login")
            Dim password As String = provider.FormData.Item("password")
            verifPassword(login, password)

            For Each fileData As MultipartFileData In provider.FileData
                Dim nomDemande = fileData.Headers.ContentDisposition.FileName.Replace(Chr(34), "")
                ' Le nom fourni par le client ne peut désigner que la zone de dépôt.
                Dim destination = ResoudreCheminDocument(nomDemande)
                Directory.CreateDirectory(Path.GetDirectoryName(destination))
                If File.Exists(destination) Then
                    File.Delete(destination)
                End If
                File.Move(fileData.LocalFileName, destination)
            Next
            Return Request.CreateResponse(HttpStatusCode.Accepted, "true")

        Catch e As ArgumentException
            Dim resp = New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                .Content = New StringContent("Requête refusée"),
                .ReasonPhrase = "Requete refusee"
            }
            Return resp

        Catch e As Exception
            ' Ne jamais renvoyer e.Message : il expose la configuration serveur.
            Dim resp = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent("Erreur interne au serveur"),
                .ReasonPhrase = "Erreur interne au serveur"
            }

            Return resp
        Finally
            ' Les fichiers temporaires non déplacés ne doivent pas s'accumuler.
            If provider IsNot Nothing AndAlso provider.FileData IsNot Nothing Then
                For Each fileData As MultipartFileData In provider.FileData
                    Try
                        If File.Exists(fileData.LocalFileName) Then File.Delete(fileData.LocalFileName)
                    Catch
                    End Try
                Next
            End If
        End Try

    End Function


End Class
