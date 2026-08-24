Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports Oasis_Common
Imports Oasis_Common.ParametreMail

Public Class SendMailController
    Inherits ApiControllerOasis

    ''' <summary>Taille maximale d'une pièce jointe.</summary>
    Private Const TailleMaxiOctets As Long = 25L * 1024L * 1024L

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

            Dim mailOasis = New MailOasis
            With provider.FormData
                mailOasis.AliasFrom = .Item("aliasFrom")
                mailOasis.AddressTo = .Item("adressTo")
                mailOasis.Subject = .Item("subject")
                mailOasis.Body = .Item("body")
                mailOasis.IsSousEpisode = .Item("isSousEpisode")
                mailOasis.IsHTML = .Item("isHTML")
            End With

            ' Dossier concerné, quand l'envoi en vise un. Sert au contrôle des
            ' destinataires et à la trace.
            Dim patientId As Long = 0
            Long.TryParse(If(provider.FormData.Item("patientId"), ""), patientId)

            ' Tout compte authentifié pouvait écrire à n'importe quelle adresse,
            ' avec une pièce jointe, depuis le compte SMTP de la structure. Les
            ' destinataires sont désormais restreints aux adresses connues du
            ' dossier et de l'annuaire, plus les domaines déclarés en configuration.
            Dim destinataires = DestinatairesMail.Separer(mailOasis.AddressTo)
            If destinataires.Count = 0 Then
                Return Refus(HttpStatusCode.BadRequest, "Aucun destinataire")
            End If
            For Each destinataire In destinataires
                If Not DestinatairesMail.EstAutorise(destinataire, patientId) Then
                    JournalAcces.AccesRefuse(UtilisateurConnecte, patientId,
                                       "Envoi refusé vers " & destinataire)
                    Return Refus(HttpStatusCode.Forbidden,
                                 "Destinataire non autorisé : " & destinataire)
                End If
            Next

            For Each fileData As MultipartFileData In provider.FileData
                If New FileInfo(fileData.LocalFileName).Length > TailleMaxiOctets Then
                    Return Refus(HttpStatusCode.RequestEntityTooLarge, "Pièce jointe trop volumineuse")
                End If
                ' Path.GetFileName : le nom annoncé par le client ne doit pas pouvoir
                ' désigner un chemin.
                mailOasis.Filename = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Replace(Chr(34), ""))
                mailOasis.Contenu = File.ReadAllBytes(fileData.LocalFileName)
            Next

            ' ------------------------------------ params mail
            Try
                Dim parametreMailDao As New ParametreMailDao
                ' Seul le serveur lit les identifiants SMTP, et seulement ici.
                Dim parametreMail = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(
                    Nothing, TypeMailParams.SMTP_PARAMETERS, inclureSmtp:=True)

                Dim mailUtil = New MailUtil(parametreMail.GetSMTPServerUrl(),
                                           parametreMail.GetSMTPPort(),
                                           parametreMail.GetSMTPUser(mailOasis.IsSousEpisode),
                                           parametreMail.GetSMTPPassword(mailOasis.IsSousEpisode),
                                           parametreMail.GetSMTPFrom(mailOasis.IsSousEpisode))
                mailUtil.SendMail(UtilisateurConnecte, mailOasis)

                ' Un envoi sortant fait quitter la structure à des données de santé :
                ' il est tracé avec ses destinataires et le nom de la pièce jointe.
                JournalAcces.Sortie(UtilisateurConnecte, patientId,
                                    "Courriel vers " & String.Join(", ", destinataires) &
                                    If(mailOasis.IsWithContenu(), " avec piece jointe " & mailOasis.Filename, ""))

                Return Request.CreateResponse(HttpStatusCode.Accepted, "true")
            Catch e As Exception
                ' Ni le message d'exception ni l'adresse du serveur SMTP ne doivent
                ' remonter au client.
                Return Refus(HttpStatusCode.InternalServerError,
                             "Erreur interne au serveur lors de l'envoi du mail")
            End Try

        Catch e As Exception
            Return Refus(HttpStatusCode.InternalServerError, "Erreur interne au serveur")
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
