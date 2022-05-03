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

        <Authorize>
        Public Function Index() As ActionResult
            Dim internauteConnectionDao As New InternauteConnectionDao
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

            Dim internauteConnections = internauteConnectionDao.GetConnectionByInternaute(Request.Cookies("internauteId").Value)
            ViewBag.Connections = internauteConnections

            Return View()
        End Function

    End Class
End Namespace
