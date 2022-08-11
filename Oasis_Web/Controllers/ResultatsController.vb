Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class ResultatsController
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

        'Function Index(model As ResultatsModel) As ActionResult
        '    ' Here you can use the model.SelectedItem which will
        '    ' return you the id of the selected item from the DropDown and 
        '    ' model.SelectedItems which will return you the list of ids of
        '    ' the selected items in the ListBox.

        'End Function

        <Authorize>
        Public Function Index() As ActionResult
            Dim internauteConnectionDao As New InternauteConnectionDao
            Dim strName As String = Constants.LAYOUT_VERTICAL
            Dim strWelcomeText As String = "Resultats"

            If TempData("ModeName") IsNot Nothing Then strName = TempData("ModeName").ToString()
            If TempData("WelcomeText") IsNot Nothing Then strWelcomeText = TempData("WelcomeText").ToString()
            ViewBag.ModeName = strName
            ViewBag.WelcomeText = strWelcomeText

            If Request.Cookies("patientId") Is Nothing Then
                Return View("~/Views/Pages/pages-500.cshtml")
            End If

            Dim patient = patientDao.GetPatient(Request.Cookies("patientId").Value)
            ViewBag.Patient = patient

            ViewBag.Resultats = ChargementResultats(patient.PatientId)
            Dim model = New ResultatsModel With {
            .Items = {
                New SelectListItem() With {.Value = "1", .Text = "item 1"},
                New SelectListItem() With {.Value = "2", .Text = "item 2"},
                New SelectListItem() With {.Value = "3", .Text = "item 3"}
                }
            }
            ViewBag.Items = model.Items
            Return View(model)
        End Function

        Private Function ChargementResultats(patientId As Long) As List(Of SousEpisodeReponse)

            Dim sousEpisodeReponseDao As New SousEpisodeReponseDao
            Dim tacheDao As New TacheDao

            Return sousEpisodeReponseDao.GetReponseByUser(patientId)

        End Function

    End Class
End Namespace
