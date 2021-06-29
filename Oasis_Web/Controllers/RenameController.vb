Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports Oasis_Common

Public Class RenameController
    Inherits ApiController

    ' GET api/<controller>
    Public Function GetValues() As String
        Return "API Oasis - Document file controleur "
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function PostValue(<FromBody()> ByVal renameRequest As RenameRequest) As HttpResponseMessage
        Try
            verifPassword(renameRequest.LoginRequest.login, renameRequest.LoginRequest.password)
            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Accepted)

            Dim oldPath = ConfigurationManager.AppSettings("FileUploadLocation") & "\" & renameRequest.OldName
            Dim newPath = ConfigurationManager.AppSettings("FileUploadLocation") & "\" & renameRequest.NewName

            If Not File.Exists(oldPath) Then
                response.StatusCode = HttpStatusCode.NotFound
                response.ReasonPhrase = String.Format("File not found: {0} .", renameRequest.OldName)
                Dim resp = New HttpResponseMessage(HttpStatusCode.NotFound) With {
                    .Content = New StringContent("Fichier demandé inexistant"),
                    .ReasonPhrase = String.Format("Fichier demandé inexistant: {0} .", renameRequest.OldName)
                }
                Return resp
            End If

            File.Move(oldPath, newPath)
            Return Request.CreateResponse(HttpStatusCode.Accepted, "true")

        Catch e As ArgumentException
            Dim response = New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                .Content = New StringContent(e.Message),
                .ReasonPhrase = "Utilisateur introuvable"
            }
            Return response

        Catch e As Exception
            Dim response = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent(e.Message),
                .ReasonPhrase = "Erreur interne au server: " & e.Message
            }

            Return response
        End Try

    End Function


End Class
