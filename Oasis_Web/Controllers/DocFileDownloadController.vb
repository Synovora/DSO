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
                'Dim filePath As String = HttpContext.Current.Server.MapPath("~/Files/") & fileName
                Dim filePath = ConfigurationManager.AppSettings("FileUploadLocation") & "\" & downloadRequest.FileName

                If Not File.Exists(filePath) Then
                    response.StatusCode = HttpStatusCode.NotFound
                    response.ReasonPhrase = String.Format("File not found: {0} .", downloadRequest.FileName)
                    Dim resp = New HttpResponseMessage(HttpStatusCode.NotFound) With {
                        .Content = New StringContent("Fichier demandé inexistant"),
                        .ReasonPhrase = String.Format("Fichier demandé inexistant: {0} .", downloadRequest.FileName)
                    }
                    Return resp
                End If

                Dim bytes As Byte() = File.ReadAllBytes(filePath)
                response.Content = New ByteArrayContent(bytes)
                response.Content.Headers.ContentLength = bytes.LongLength
                response.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("attachment") With {
                    .FileName = downloadRequest.FileName
                }
                response.Content.Headers.ContentType = New MediaTypeHeaderValue(MimeMapping.GetMimeMapping(downloadRequest.FileName))
                Return response

            Catch e As ArgumentException
                Dim resp = New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                    .Content = New StringContent(e.Message),
                    .ReasonPhrase = "Utilisateur introuvable"
                }
                Return resp

            Catch e As Exception
                Dim resp = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent(e.Message),
                .ReasonPhrase = "Erreur interne au server"
                }
                Return resp

            End Try

        End Function


    End Class
End Namespace