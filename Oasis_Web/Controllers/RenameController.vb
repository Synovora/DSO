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

            ' Les deux noms viennent du client : ils doivent rester sous la zone de
            ' dépôt, sinon l'appel déplace n'importe quel fichier du serveur.
            Dim oldPath As String
            Dim newPath As String
            Try
                oldPath = ResoudreCheminDocument(renameRequest.OldName)
                newPath = ResoudreCheminDocument(renameRequest.NewName)
            Catch exNom As ArgumentException
                Return New HttpResponseMessage(HttpStatusCode.BadRequest) With {
                    .Content = New StringContent("Nom de fichier invalide"),
                    .ReasonPhrase = "Nom de fichier invalide"
                }
            End Try

            If Not File.Exists(oldPath) Then
                Dim resp = New HttpResponseMessage(HttpStatusCode.NotFound) With {
                    .Content = New StringContent("Fichier demandé inexistant"),
                    .ReasonPhrase = "Fichier demande inexistant"
                }
                Return resp
            End If

            Directory.CreateDirectory(Path.GetDirectoryName(newPath))
            File.Move(oldPath, newPath)

            Return Request.CreateResponse(HttpStatusCode.Accepted, "true")

        Catch e As ArgumentException
            Dim response = New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                .Content = New StringContent("Requête refusée"),
                .ReasonPhrase = "Requete refusee"
            }
            Return response

        Catch e As Exception
            ' Ne jamais renvoyer e.Message : il expose la configuration serveur.
            Dim response = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent("Erreur interne au serveur"),
                .ReasonPhrase = "Erreur interne au serveur"
            }

            Return response
        End Try

    End Function


End Class
