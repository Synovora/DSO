Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class DashboardController
        Inherits Controller

        ReadOnly parametreDao As New ParametreDao
        ReadOnly ordonnanceDao As New OrdonnanceDao
        ReadOnly patientDao As New PatientDaoBase
        ReadOnly utilisateurDao As New UserDao
        ReadOnly ordonnanceDetailDao As New OrdonnanceDetailDao
        ReadOnly traitementDao As New TraitementDao
        ReadOnly autoSuiviDao As New AutoSuiviDao
        ReadOnly episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao

        <Authorize>
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

            Dim patient = patientDao.GetPatientById(Request.Cookies("patientId").Value)
            ViewBag.Patient = patient
            ViewBag.ParametresAutoSuivi = BuildAutoSuiviList(patient.PatientId)

            Return View()
        End Function


        Private Function BuildAutoSuiviList(patientId As Integer) As List(Of AutoSuiviItem)
            Dim parametres As List(Of AutoSuiviItem) = New List(Of AutoSuiviItem)
            Dim TypeActiviteAcode As String = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE
            Dim ListParametres As List(Of Long) = episodeProtocoleCollaboratifDao.GetListeParametreByPatientEtTypeEpisode(patientId, TypeActiviteAcode)
            For i = 0 To ListParametres.Count - 1
                Dim parametre = parametreDao.GetParametreById(ListParametres.Item(i))
                If parametre.ExclusionAutoSuivi = True Then
                    Continue For
                End If
                Dim autoSuivi = autoSuiviDao.GetAutoSuiviByPatientIdAndParametreId(patientId, ListParametres.Item(i))

                parametres.Add(New AutoSuiviItem With {
                    .PatientId = patientId,
                    .ParametreId = parametre.Id,
                    .Description = If(parametre.DescriptionPatient = "", parametre.Description, parametre.DescriptionPatient),
                    .IsActif = autoSuivi Is Nothing
                })
            Next
            Return parametres
        End Function

    End Class
End Namespace
