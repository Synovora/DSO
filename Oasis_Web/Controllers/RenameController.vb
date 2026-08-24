Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Oasis_Common

Public Class RenameController
    Inherits ApiControllerOasis

    Public Function PostValue(<FromBody()> ByVal renameRequest As RenameRequest) As HttpResponseMessage
        Try
            If renameRequest Is Nothing Then
                Return Refus(HttpStatusCode.BadRequest, "Requête incomplète")
            End If

            If Not PeutAccederAuxDocuments(UtilisateurConnecte) Then
                Return Refus(HttpStatusCode.Forbidden, "Accès refusé")
            End If

            ' Les deux noms viennent du client. Chacun doit désigner un document
            ' réellement enregistré, et les deux doivent concerner le même patient :
            ' sans cela, un renommage déplace le document d'un dossier vers un autre.
            Dim source As HabilitationsDocuments.DocumentDemande
            Dim cible As HabilitationsDocuments.DocumentDemande
            Try
                source = ResoudreDocument(renameRequest.OldName)
                cible = ResoudreDocument(renameRequest.NewName)
            Catch exAcces As UnauthorizedAccessException
                Return Refus(HttpStatusCode.NotFound, "Document introuvable")
            End Try

            If source.EstModele <> cible.EstModele OrElse source.PatientId <> cible.PatientId Then
                JournalAcces.AccesRefuse(UtilisateurConnecte, source.PatientId,
                                   "Renommage vers un autre dossier refusé : " &
                                   source.Nom & " vers " & cible.Nom)
                Return Refus(HttpStatusCode.Forbidden, "Accès refusé")
            End If

            If Not source.EstModele AndAlso
               Not PeutAccederAuPatient(UtilisateurConnecte, source.PatientId) Then
                Return Refus(HttpStatusCode.Forbidden, "Accès refusé")
            End If

            Dim oldPath As String
            Dim newPath As String
            Try
                oldPath = ResoudreCheminDocument(source.Nom)
                newPath = ResoudreCheminDocument(cible.Nom)
            Catch exNom As ArgumentException
                Return Refus(HttpStatusCode.BadRequest, "Nom de fichier invalide")
            End Try

            If Not File.Exists(oldPath) Then
                Return Refus(HttpStatusCode.NotFound, "Document introuvable")
            End If

            Directory.CreateDirectory(Path.GetDirectoryName(newPath))
            File.Move(oldPath, newPath)

            If Not source.EstModele Then
                JournalAcces.Modification(UtilisateurConnecte, source.PatientId,
                                          "Renommage document " & source.Nom & " vers " & cible.Nom)
            End If

            Return Request.CreateResponse(HttpStatusCode.Accepted, "true")

        Catch e As Exception
            ' Ne jamais renvoyer e.Message : il expose la configuration serveur.
            Return Refus(HttpStatusCode.InternalServerError, "Erreur interne au serveur")
        End Try

    End Function

End Class
