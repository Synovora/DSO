Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports Oasis_Common

Public Class DocFileController
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
        Try
            Dim isOneFile As Boolean = False
            Dim fileuploadPath = ConfigurationManager.AppSettings("FileUploadLocation")
            Dim provider = New MultipartFormDataStreamProvider(fileuploadPath)
            Dim content = New StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(True))

            For Each header In Request.Content.Headers
                content.Headers.TryAddWithoutValidation(header.Key, header.Value)
            Next

            Await content.ReadAsMultipartAsync(provider)
            If provider.FileData Is Nothing OrElse provider.FileData.Count <> 1 Then
                Throw New Exception("Un seul fichier doit être posté !")
            End If

            ' -- on verifie que le login / pwassword est ok 
            Dim login As String = provider.FormData.Item("login")
            Dim password As String = provider.FormData.Item("password")
            verifPassword(login, password)

            For Each fileData As MultipartFileData In provider.FileData
                Dim originalFileName = fileuploadPath + "\" + fileData.Headers.ContentDisposition.FileName.Replace(Chr(34), "")
                If File.Exists(originalFileName) Then
                    File.Delete(originalFileName)
                End If
                File.Move(fileData.LocalFileName, originalFileName)
            Next
            Return Request.CreateResponse(HttpStatusCode.Accepted, "true")

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

    Private Sub verifPassword(login As String, password As String)
        Dim userDao As UserDao = New UserDao
        Dim userLog = Nothing
        userLog = userDao.getUserByLoginPassword(login, password)
        Return

    End Sub

End Class
