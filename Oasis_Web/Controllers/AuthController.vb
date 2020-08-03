Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Models

Namespace Oasis_Web.Controllers

    Public Class AuthController
        Inherits Controller

        Public Function Index() As ActionResult
            Return View("login")
        End Function

        <ActionName("login")>
        Public Function authlogin() As ActionResult
            Return View()
        End Function

        <ActionName("auth-register")>
        Public Function authregister() As ActionResult
            Return View()
        End Function

        <ActionName("auth-recoverpw")>
        Public Function authrecoverpw() As ActionResult
            Return View()
        End Function

        <ActionName("auth-lock-screen")>
        Public Function authlockscreen() As ActionResult
            Return View()
        End Function

        'Login POST
        <HttpPost>
        <ValidateAntiForgeryToken>
        Public Function Login(user As UserLogin, ReturnUrl As String) As ActionResult
            Dim message As String
            Dim internauteDao As InternauteDao = New InternauteDao

            If internauteDao.getUserByLoginPassword(user.Username, user.Password) IsNot Nothing Then
                Dim timeout As Integer = If(user.RememberMe, 525600, 20)
                Dim ticket = New FormsAuthenticationTicket(user.Username, user.RememberMe, timeout)
                Dim encrypted As String = FormsAuthentication.Encrypt(ticket)
                Dim cookie = New HttpCookie(FormsAuthentication.FormsCookieName, encrypted) With {
                    .Expires = DateTime.Now.AddMinutes(timeout),
                    .HttpOnly = True
                }
                Response.Cookies.Add(cookie)

                If (Url.IsLocalUrl(ReturnUrl)) Then
                    Return Redirect(ReturnUrl)
                Else
                    Return RedirectToAction("Index", "Dashboard")
                End If

            Else

                message = "Invalied credential provided"
            End If

            ViewBag.Message = message
            Return View()
        End Function

        '//Logout
        '[HttpPost]
        '[Authorize]
        'Public ActionResult Logout()
        '{
        '    FormsAuthentication.SignOut();
        '    Return RedirectToAction("Login", "User");
        '}
    End Class
End Namespace
