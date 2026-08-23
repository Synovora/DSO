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


End Class
