Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Http
Imports Oasis_Common

Namespace Controllers
    Public Class DocFileDownloadController
        Inherits ApiControllerOasis

        Public Function PostValue(<FromBody()> ByVal downloadRequest As DownloadRequest) As HttpResponseMessage
            Try
                If downloadRequest Is Nothing Then
                    Return Refus(HttpStatusCode.BadRequest, "Requête incomplète")
                End If

                ' L'identité vient du filtre d'authentification, plus du corps de la
                ' requête. Vérifier un mot de passe ne disait rien du droit d'accès
                ' au document demandé : les noms sont prévisibles, donc n'importe
                ' quel compte lisait l'ensemble du fonds documentaire.
                If Not PeutAccederAuxDocuments(UtilisateurConnecte) Then
                    Return Refus(HttpStatusCode.Forbidden, "Accès refusé")
                End If

                Dim document As HabilitationsDocuments.DocumentDemande
                Try
                    document = ResoudreDocument(downloadRequest.FileName)
                Catch exAcces As UnauthorizedAccessException
                    ' Nom qui ne correspond à aucun document enregistré : même
                    ' réponse qu'un fichier absent, pour ne pas confirmer quels
                    ' identifiants d'épisode existent.
                    Return Refus(HttpStatusCode.NotFound, "Document introuvable")
                End Try

                If Not document.EstModele AndAlso
                   Not PeutAccederAuPatient(UtilisateurConnecte, document.PatientId) Then
                    Return Refus(HttpStatusCode.Forbidden, "Accès refusé")
                End If

                Dim filePath As String
                Try
                    filePath = ResoudreCheminDocument(document.Nom)
                Catch exNom As ArgumentException
                    Return Refus(HttpStatusCode.BadRequest, "Nom de fichier invalide")
                End Try

                If Not File.Exists(filePath) Then
                    Return Refus(HttpStatusCode.NotFound, "Document introuvable")
                End If

                ' Consultation d'un document de dossier : tracée avec le patient
                ' concerné. Tant que la règle de périmètre patient n'est pas
                ' arrêtée, la trace est ce qui rend un accès indu constatable.
                If Not document.EstModele Then
                    JournalAcces.Consultation(UtilisateurConnecte, document.PatientId,
                                              "Téléchargement document " & document.Nom)
                End If

                Dim nomFichier = Path.GetFileName(filePath)
                Dim bytes As Byte() = File.ReadAllBytes(filePath)
                Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Accepted)
                response.Content = New ByteArrayContent(bytes)
                response.Content.Headers.ContentLength = bytes.LongLength
                response.Content.Headers.ContentDisposition = New ContentDispositionHeaderValue("attachment") With {
                    .FileName = nomFichier
                }
                response.Content.Headers.ContentType = New MediaTypeHeaderValue(MimeMapping.GetMimeMapping(nomFichier))
                Return response

            Catch e As Exception
                ' Ne jamais renvoyer e.Message : il expose la configuration serveur.
                Return Refus(HttpStatusCode.InternalServerError, "Erreur interne au serveur")
            End Try

        End Function

    End Class
End Namespace
