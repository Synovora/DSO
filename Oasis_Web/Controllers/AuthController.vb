Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc

Namespace Oasis_Web.Controllers
    Public Class AuthController
        Inherits Controller

        Public Function Index() As ActionResult
            Return View("auth-login")
        End Function

        <ActionName("auth-login")>
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

        <HttpPost>
        Public Function ValidateLogin(ByVal email As String, ByVal password As String) As ActionResult
            Dim dbEmail As String = "Test"
            Dim dbPassword As String = "123"
            Dim IsValidUser As Boolean = False
            If email = dbEmail AndAlso password = dbPassword Then IsValidUser = True
            Return Json(New With {
    Key .IsValidUser = IsValidUser
            })
        End Function
    End Class
End Namespace
