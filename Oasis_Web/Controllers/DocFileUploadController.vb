Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports Oasis_Common

Public Class DocFileUploadController
    Inherits ApiControllerOasis

    ''' <summary>
    ''' Taille maximale acceptée pour un document déposé. Un compte rendu pèse
    ''' quelques centaines de kilo-octets ; la borne évite qu'un appel authentifié
    ''' remplisse le disque du serveur.
    ''' </summary>
    Private Const TailleMaxiOctets As Long = 50L * 1024L * 1024L

    Public Async Function Upload() As Task(Of HttpResponseMessage)
        Dim provider As MultipartFormDataStreamProvider = Nothing
        Try
            If Not PeutAccederAuxDocuments(UtilisateurConnecte) Then
                Return Refus(HttpStatusCode.Forbidden, "Accès refusé")
            End If

            Dim fileuploadPath = ConfigurationManager.AppSettings("FileUploadLocation")
            ' Les pièces reçues atterrissent d'abord sous un nom généré par le
            ' framework dans un sous-dossier temporaire ; rien n'est écrit à
            ' l'emplacement définitif avant validation du nom et des droits.
            Dim dossierTemporaire = Path.Combine(fileuploadPath, "tmp")
            Directory.CreateDirectory(dossierTemporaire)
            provider = New MultipartFormDataStreamProvider(dossierTemporaire)
            Dim content = New StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(True))

            For Each header In Request.Content.Headers
                content.Headers.TryAddWithoutValidation(header.Key, header.Value)
            Next

            Await content.ReadAsMultipartAsync(provider)
            If provider.FileData Is Nothing OrElse provider.FileData.Count <> 1 Then
                Return Refus(HttpStatusCode.BadRequest, "Un seul fichier doit être posté")
            End If

            For Each fileData As MultipartFileData In provider.FileData
                If New FileInfo(fileData.LocalFileName).Length > TailleMaxiOctets Then
                    Return Refus(HttpStatusCode.RequestEntityTooLarge, "Document trop volumineux")
                End If

                Dim nomDemande = fileData.Headers.ContentDisposition.FileName.Replace(Chr(34), "")

                ' Le nom fourni par le client doit désigner un document réellement
                ' enregistré. Sans cette vérification, un appelant composait
                ' n'importe quelle combinaison d'identifiants et écrasait le
                ' document correspondant dans le dossier d'un autre patient.
                Dim document As HabilitationsDocuments.DocumentDemande
                Try
                    document = ResoudreDocument(nomDemande)
                Catch exAcces As UnauthorizedAccessException
                    Return Refus(HttpStatusCode.NotFound, "Document introuvable")
                End Try

                If Not document.EstModele AndAlso
                   Not PeutAccederAuPatient(UtilisateurConnecte, document.PatientId) Then
                    Return Refus(HttpStatusCode.Forbidden, "Accès refusé")
                End If

                Dim destination As String
                Try
                    destination = ResoudreCheminDocument(document.Nom)
                Catch exNom As ArgumentException
                    Return Refus(HttpStatusCode.BadRequest, "Nom de fichier invalide")
                End Try

                Directory.CreateDirectory(Path.GetDirectoryName(destination))
                If File.Exists(destination) Then
                    File.Delete(destination)
                End If
                File.Move(fileData.LocalFileName, destination)

                If Not document.EstModele Then
                    JournalAcces.Modification(UtilisateurConnecte, document.PatientId,
                                              "Dépôt document " & document.Nom)
                End If
            Next
            Return Request.CreateResponse(HttpStatusCode.Accepted, "true")

        Catch e As Exception
            ' Ne jamais renvoyer e.Message : il expose la configuration serveur.
            Return Refus(HttpStatusCode.InternalServerError, "Erreur interne au serveur")
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
