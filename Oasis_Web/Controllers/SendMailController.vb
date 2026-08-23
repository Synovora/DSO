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
        Dim provider As MultipartFormDataStreamProvider = Nothing
        Try
            Dim fileuploadPath = ConfigurationManager.AppSettings("FileUploadLocation")
            ' Les pièces jointes transitent par un sous-dossier temporaire, jamais
            ' directement dans la zone de dépôt des documents.
            Dim dossierTemporaire = Path.Combine(fileuploadPath, "tmp")
            Directory.CreateDirectory(dossierTemporaire)
            provider = New MultipartFormDataStreamProvider(dossierTemporaire)
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
                ' Path.GetFileName : le nom annoncé par le client ne doit pas pouvoir
                ' désigner un chemin.
                mailOasis.Filename = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Replace(Chr(34), ""))
                mailOasis.Contenu = File.ReadAllBytes(fileData.LocalFileName)
            Next

            ' ------------------------------------ params mail
            Dim smtpServer As String = Nothing
            Try

                Dim parametreMailDao As New ParametreMailDao
                Dim parametreMail = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(Nothing, TypeMailParams.SMTP_PARAMETERS)
                smtpServer = parametreMail.GetSMTPServerUrl()


                Dim mailUtil = New MailUtil(parametreMail.GetSMTPServerUrl(),
                                           parametreMail.GetSMTPPort(),
                                           parametreMail.GetSMTPUser(mailOasis.IsSousEpisode),
                                           parametreMail.GetSMTPPassword(mailOasis.IsSousEpisode),
                                           parametreMail.GetSMTPFrom(mailOasis.IsSousEpisode))
                mailUtil.SendMail(user, mailOasis)

                Return Request.CreateResponse(HttpStatusCode.Accepted, "true")
            Catch e As Exception
                ' Ni le message d'exception ni l'adresse du serveur SMTP ne doivent
                ' remonter au client.
                Dim resp = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent("Erreur interne au serveur lors de l'envoi du mail"),
                .ReasonPhrase = "Erreur interne au serveur lors de l'envoi du mail"
            }
                Return resp
            End Try

        Catch e As UnauthorizedAccessException
                Dim resp = New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                    .Content = New StringContent("Identifiant et/ou mot de passe erroné !"),
                    .ReasonPhrase = "Utilisateur introuvable"
                }
                Return resp

            Catch e As Exception
                Dim resp = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent("Erreur interne au serveur"),
                .ReasonPhrase = "Erreur interne au serveur"
            }

            Return resp
        Finally
            ' Les pièces jointes temporaires ne doivent pas rester sur le disque.
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
