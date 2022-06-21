Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports Oasis_Common
Imports Oasis_Common.ParametreMail

Public Class SendMailController
    Inherits ApiController

    ' GET api/<controller>
    Public Function GetValues() As String
        Return "API Oasis - Send Mail controleur "
    End Function

    Public Async Function SendMail() As Task(Of HttpResponseMessage)
        Try
            Dim isOneFile As Boolean = False
            Dim fileuploadPath = ConfigurationManager.AppSettings("FileUploadLocation")
            Dim provider = New MultipartFormDataStreamProvider(fileuploadPath)
            Dim content = New StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(True))

            For Each header In Request.Content.Headers
                content.Headers.TryAddWithoutValidation(header.Key, header.Value)
            Next

            Await content.ReadAsMultipartAsync(provider)

            Dim login As String = provider.FormData.Item("login")
            Dim password As String = provider.FormData.Item("password")

            Dim user As Utilisateur = Nothing
            Try
                user = ModuleUtilsBase.verifPassword(login, password)
            Catch ex As Exception
                Throw New UnauthorizedAccessException
            End Try

            Dim mailOasis = New MailOasis
            With provider.FormData
                mailOasis.AliasFrom = .Item("aliasFrom")
                mailOasis.AddressTo = .Item("adressTo")
                mailOasis.Subject = .Item("subject")
                mailOasis.Body = .Item("body")
                mailOasis.IsSousEpisode = .Item("isSousEpisode")
                mailOasis.IsHTML = .Item("isHTML")
            End With

            For Each fileData As MultipartFileData In provider.FileData
                mailOasis.Filename = fileData.Headers.ContentDisposition.FileName.Replace(Chr(34), "")
                mailOasis.Contenu = File.ReadAllBytes(fileData.LocalFileName)
            Next

            ' ------------------------------------ params mail
            Dim smtpServer As String = Nothing
            Dim parametreMailDao As New ParametreMailDao
            Dim parametreMail = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(mailOasis.Patient.PatientSiegeId, TypeMailParams.SMTP_PARAMETERS)
            smtpServer = parametreMail.GetSMTPServerUrl()


            Dim mailUtil = New MailUtil(parametreMail.GetSMTPServerUrl(),
                                       parametreMail.GetSMTPPort(),
                                       parametreMail.GetSMTPUser(mailOasis.IsSousEpisode),
                                       parametreMail.GetSMTPPassword(mailOasis.IsSousEpisode),
                                       parametreMail.GetSMTPFrom(mailOasis.IsSousEpisode))
            mailUtil.SendMail(user, mailOasis)

            Return Request.CreateResponse(HttpStatusCode.Accepted, "true")

        Catch e As UnauthorizedAccessException
            Dim resp = New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                .Content = New StringContent(e.Message),
                .ReasonPhrase = "Utilisateur introuvable"
            }
            Return resp

        Catch e As Exception
            Dim resp = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent(e.Message),
                .ReasonPhrase = "Erreur interne au server : " + e.Message
            }

            Return resp
        End Try

    End Function



End Class
