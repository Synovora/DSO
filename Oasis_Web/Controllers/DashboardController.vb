Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class DashboardController
        Inherits Controller

        Public Function Index() As ActionResult
            Dim ordonnanceDao As New OrdonnanceDao
            Dim patientDao As New PatientDaoBase
            Dim utilisateurDao As New UserDao
            Dim ordonnanceDetailDao As New OrdonnanceDetailDao
            Dim traitementDao As New TraitementDao

            Dim strName As String = Constants.LAYOUT_VERTICAL
            Dim strWelcomeText As String = "Dashboard"
            If TempData("ModeName") IsNot Nothing Then strName = TempData("ModeName").ToString()
            If TempData("WelcomeText") IsNot Nothing Then strWelcomeText = TempData("WelcomeText").ToString()
            ViewBag.ModeName = strName
            ViewBag.WelcomeText = strWelcomeText
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
    End Class
End Namespace
