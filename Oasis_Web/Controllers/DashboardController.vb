Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http.Formatting
Imports System.Web
Imports System.Web.Http
Imports System.Web.Mvc
Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class DashboardController
        Inherits Controller

        ReadOnly parametreDao As New ParametreDao
        ReadOnly ordonnanceDao As New OrdonnanceDao
        ReadOnly patientDao As New PatientDao
        ReadOnly utilisateurDao As New UserDao
        ReadOnly ordonnanceDetailDao As New OrdonnanceDetailDao
        ReadOnly traitementDao As New TraitementDao
        ReadOnly autoSuiviDao As New AutoSuiviDao
        ReadOnly episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
        ReadOnly episodeDao As New EpisodeDao
        Dim episodeParametreDao As New EpisodeParametreDao

        <System.Web.Mvc.Authorize>
        Public Function Index() As ActionResult

            Dim strName As String = Constants.LAYOUT_VERTICAL
            Dim strWelcomeText As String = "Dashboard"
            If TempData("ModeName") IsNot Nothing Then strName = TempData("ModeName").ToString()
            If TempData("WelcomeText") IsNot Nothing Then strWelcomeText = TempData("WelcomeText").ToString()
            ViewBag.ModeName = strName
            ViewBag.WelcomeText = strWelcomeText

            If Request.Cookies("patientId") Is Nothing Then
                Return View("~/Views/Pages/pages-500.cshtml")
            End If

            Dim patient = patientDao.GetPatient(Request.Cookies("patientId").Value)
            ViewBag.Patient = patient

            Return View()
        End Function

    End Class
End Namespace
