Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Oasis_Common

Public Class LoginController
    Inherits ApiController

    ' GET api/<controller>
    Public Function GetValues() As String
        Return "API Oasis - Login "
    End Function


    ' POST api/<controller>
    Public Function PostValue(<FromBody()> ByVal loginRequest As LoginRequest) As HttpResponseMessage
        Dim userDao As UserDao = New UserDao
        Dim userLog = Nothing
        Try
            userLog = userDao.getUserByLoginPassword(loginRequest.login,
                                                     loginRequest.password)

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

    'Public Function GetValue(<FromUri()> ByVal login As String, <FromUri()> ByVal password As String) As LoginRequest
    '    Dim userDao As UserDao = New UserDao

    '    Dim userLog = userDao.getUserByLoginPassword(login,
    '                                                 password)

    '    'Return userLog 'ConfigurationManager.ConnectionStrings("Oasis_Web.My.MySettings.oasisConnection").ConnectionString
    '    Dim loginRequest As New LoginRequest
    '    loginRequest.login = userLog.UtilisateurLogin
    '    loginRequest.password = userLog.Password
    '    Return loginRequest
    'End Function

    ' PUT api/<controller>/5
    'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)
    '
    'End Sub

    ' DELETE api/<controller>/5
    'Public Sub DeleteValue(ByVal id As Integer)
    '
    '    End Sub
End Class
