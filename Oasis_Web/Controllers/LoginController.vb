Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Oasis_Common

Public Class LoginController
    Inherits ApiController

    <AllowAnonymous>
    Public Function GetValues() As String
        Return "API Oasis - Login "
    End Function

    <AllowAnonymous>
    Public Function PostValue(<FromBody()> ByVal loginRequest As LoginRequest) As HttpResponseMessage
        Dim userDao As New UserDao

        Try
            verifPassword(loginRequest.login, loginRequest.password)
            Dim enc = EncryptString(ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString)
            Return Request.CreateResponse(HttpStatusCode.Accepted, enc)

        Catch e As ArgumentException
            Dim resp = New HttpResponseMessage(HttpStatusCode.Unauthorized) With {
                .Content = New StringContent("Identifiant et/ou mot de passe erroné !"),
                .ReasonPhrase = "Utilisateur introuvable"
            }
            Return resp

        Catch e As Exception
            ' Ne jamais renvoyer e.Message : sur cette route il peut contenir la
            ' chaîne de connexion ou le détail de l'erreur SQL.
            Dim resp = New HttpResponseMessage(HttpStatusCode.InternalServerError) With {
                .Content = New StringContent("Erreur interne au serveur"),
                .ReasonPhrase = "Erreur interne au serveur"
            }

            Return resp
        End Try
    End Function


End Class
