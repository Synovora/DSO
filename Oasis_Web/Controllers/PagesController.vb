Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc

Namespace Oasis_Web.Controllers
    Public Class PagesController
        Inherits Controller

        <ActionName("pages-login")>
        Public Function pageslogin() As ActionResult
            Return View()
        End Function

        <HttpPost>
        Public Function ValidateLogin(ByVal email As String, ByVal password As String) As ActionResult
            Dim dbEmail As String = "Test"
            Dim dbPassword As String = "123"
            Dim IsValidUser As Boolean = False
            If email = dbEmail AndAlso password = dbPassword Then IsValidUser = True
            Return Json(New With {Key .IsValidUser = IsValidUser
            })
        End Function

        <ActionName("pages-login-2")>
        Public Function pageslogin2() As ActionResult
            Return View()
        End Function

        <ActionName("pages-register")>
        Public Function pagesregister() As ActionResult
            Return View()
        End Function

        <ActionName("pages-register-2")>
        Public Function pagesregister2() As ActionResult
            Return View()
        End Function

        <ActionName("pages-recoverpw")>
        Public Function pagesrecoverpw() As ActionResult
            Return View()
        End Function

        <ActionName("pages-recoverpw-2")>
        Public Function pagesrecoverpw2() As ActionResult
            Return View()
        End Function

        <ActionName("pages-lock-screen")>
        Public Function pageslockscreen() As ActionResult
            Return View()
        End Function

        <ActionName("pages-lock-screen-2")>
        Public Function pageslockscreen2() As ActionResult
            Return View()
        End Function

        <ActionName("pages-starter")>
        Public Function pagesstarter() As ActionResult
            Return View()
        End Function

        <ActionName("pages-maintenance")>
        Public Function pagesmaintenance() As ActionResult
            Return View()
        End Function

        <ActionName("pages-comingsoon")>
        Public Function pagescomingsoon() As ActionResult
            Return View()
        End Function

        <ActionName("pages-timeline")>
        Public Function pagestimeline() As ActionResult
            Return View()
        End Function

        <ActionName("pages-faqs")>
        Public Function pagesfaqs() As ActionResult
            Return View()
        End Function

        <ActionName("pages-pricing")>
        Public Function pagespricing() As ActionResult
            Return View()
        End Function

        <ActionName("pages-404")>
        Public Function pages404() As ActionResult
            Return View()
        End Function

        <ActionName("pages-500")>
        Public Function pages500() As ActionResult
            Return View()
        End Function
    End Class
End Namespace
