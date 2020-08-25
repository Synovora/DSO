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

        '<AllowAnonymous>
        'Public Function Index() As ActionResult
        '    Return View("login")
        'End Function

        <AllowAnonymous>
        <ActionName("login")>
        Public Function authlogin() As ActionResult
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("auth-register")>
        Public Function authregister() As ActionResult
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("auth-recoverpw")>
        Public Function authrecoverpw() As ActionResult
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("auth-lock-screen")>
        Public Function authlockscreen() As ActionResult
            Return View()
        End Function

        'Login POST
        <HttpPost>
        <ValidateAntiForgeryToken>
        <AllowAnonymous>
        Public Function Login(user As UserLogin, ReturnUrl As String) As ActionResult
            Dim message As String
            Dim internauteDao As InternauteDao = New InternauteDao
            Dim internautePermissionDao As InternautePermissionDao = New InternautePermissionDao
            Dim internaute As Internaute
            Try
                internaute = internauteDao.GetInternauteByLoginPassword(user.Username, user.Password)
                Dim timeout As Integer = If(user.RememberMe, 525600, 20)
                Dim ticket = New FormsAuthenticationTicket(user.Username, user.RememberMe, timeout)
                Dim encrypted As String = FormsAuthentication.Encrypt(ticket)
                Dim cookie = New HttpCookie(FormsAuthentication.FormsCookieName, encrypted) With {
                    .Expires = DateTime.Now.AddMinutes(timeout),
                    .HttpOnly = True
                }
                Response.Cookies.Add(cookie)
                Response.Cookies("internauteId").Value = internaute.Id
                Response.Cookies("internauteId").Expires = DateTime.Now.AddDays(90)
                Session("internauteId") = internaute.Id
                Dim internautePermission = internautePermissionDao.GetPermissionsByInternaute(internaute.Id)
                'Session("patientId") = internautePermission(0).PatientId
                Response.Cookies("patientId").Value = internautePermission(0).PatientId
                Response.Cookies("patientId").Expires = DateTime.Now.AddDays(90)
                If (Url.IsLocalUrl(ReturnUrl)) Then
                    Return Redirect(ReturnUrl)
                Else
                    Return RedirectToAction("Index", "Dashboard")
                End If

            Catch ex As Exception
                message = ex.Message
            End Try

            ViewBag.Message = message
            Return View()
        End Function

        <HttpPost>
        <Authorize>
        Public Function Logout() As ActionResult
            FormsAuthentication.SignOut()
            Session.Abandon()
            Return RedirectToAction("Login", "Auth")
        End Function
    End Class
End Namespace
