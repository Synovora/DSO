Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Http
Imports Oasis_Common

Namespace Controllers
    Public Class DocFileDownloadController
        Inherits ApiController

        Public Function PostValue(<FromBody()> ByVal downloadRequest As DownloadRequest) As HttpResponseMessage
            Try
                ' --- verification droits
                verifPassword(downloadRequest.LoginRequest.login, downloadRequest.LoginRequest.password)

                Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Accepted)
                ' Le nom vient du client : il doit correspondre au motif attendu et
                ' rester sous la zone de dépôt (sinon ..\..\Web.config est lisible).
                Dim filePath As String
                Try
                    filePath = ResoudreCheminDocument(downloadRequest.FileName)
                Catch exNom As ArgumentException
                    Return New HttpResponseMessage(HttpStatusCode.BadRequest) With {
                        .Content = New StringContent("Nom de fichier invalide"),
                        .ReasonPhrase = "Nom de fichier invalide"
                    }
                End Try

                If Not File.Exists(filePath) Then
                    Dim resp = New HttpResponseMessage(HttpStatusCode.NotFound) With {
                        .Content = New StringContent("Fichier demandé inexistant"),
                        .ReasonPhrase = "Fichier demande inexistant"
                    }
                    Return resp
                End If

                Dim nomFichier = Path.GetFileName(filePath)
                Dim bytes As Byte() = File.ReadAllBytes(filePath)
                response.Content = New ByteArrayContent(bytes)
                response.Content.Headers.ContentLength = bytes.LongLength
                response.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("attachment") With {
                    .FileName = nomFichier
                }
                response.Content.Headers.ContentType = New MediaTypeHeaderValue(MimeMapping.GetMimeMapping(nomFichier))
                Return response

            Catch e As ArgumentException
                Dim resp = New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                    .Content = New StringContent("Identifiant et/ou mot de passe erroné !"),
                    .ReasonPhrase = "Utilisateur introuvable"
                }
                Return resp

            Catch e As Exception
                ' Ne jamais renvoyer e.Message : il expose la configuration serveur.
                Dim resp = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent("Erreur interne au serveur"),
                .ReasonPhrase = "Erreur interne au serveur"
                }
                Return resp

            End Try

        End Function


    End Class
End Namespace