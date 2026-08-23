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

            ViewBag.ParcoursDeSoin = ChargementParcoursDeSoin(patient.PatientId)
            Return View()
        End Function

        Private Function ChargementParcoursDeSoin(patientId As Long)
            Dim ParcoursDataTable As DataTable
            Dim parcoursDao As New ParcoursDao
            Dim tacheDao As New TacheDao
            Dim IntervenantOasis As Boolean
            Dim Result As New List(Of List(Of String))

            ParcoursDataTable = parcoursDao.GetAllParcoursbyPatient(patientId)

            Dim rowCount As Integer = ParcoursDataTable.Rows.Count - 1
            Dim SpecialiteDescription As String
            Dim ParcoursCacher As Boolean

            For i = 0 To rowCount Step 1
                Dim tmp As New List(Of String)
                Dim rorId As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_ror_id"), 0)

                ParcoursCacher = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_cacher"), False)
                tmp.Add(ParcoursDataTable.Rows(i)("oa_parcours_id"))

                Dim SpecialiteId = ParcoursDataTable.Rows(i)("oa_parcours_specialite")
                SpecialiteDescription = Table_specialite.GetSpecialiteDescription(SpecialiteId)
                tmp.Add(SpecialiteDescription)

                If IntervenantOasis = True Then
                    tmp.Add("Oasis")
                    tmp.Add("Oasis")
                Else
                    tmp.Add(Coalesce(ParcoursDataTable.Rows(i)("oa_ror_nom"), ""))
                    tmp.Add(Coalesce(ParcoursDataTable.Rows(i)("oa_ror_structure_nom"), ""))
                End If

                tmp.Add(Coalesce(ParcoursDataTable.Rows(i)("NextRendezVous"), Nothing))
                tmp.Add(Coalesce(ParcoursDataTable.Rows(i)("DateDemandeRdv"), Nothing))
                Result.Add(tmp)
            Next
            Return Result
        End Function

    End Class
End Namespace
