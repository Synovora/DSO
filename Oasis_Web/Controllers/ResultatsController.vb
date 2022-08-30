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

        <ActionName("download")>
        Public Function Download(fileName As String) As ActionResult

            Dim filePath = ConfigurationManager.AppSettings("FileUploadLocation") & "\" & fileName

            Response.Clear()
            Response.ClearContent()
            Response.ClearHeaders()
            Response.ContentType = "application/pdf"
            Response.Buffer = True
            Response.AddHeader("Content-Disposition", "attachment;filename=toto.pdf")
            Response.TransmitFile("Z:\aneopsy\Download\Facture_2557668.pdf")
            Response.End()

            Return Nothing
        End Function

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

            Dim Resultats = ChargementResultats(patient.PatientId)

            Dim sousEpisodeLibelles = Resultats.Select(Function(item) item.SousEpisodeLibelle).Distinct().ToList.Select(Function(obj) New SelectListItem() With {.Value = obj, .Text = obj}).Reverse.Append(New SelectListItem() With {.Value = "Tous", .Text = "Tous"}).Reverse.ToList
            ViewData("sousEpisodeLibelles") = sousEpisodeLibelles

            Dim SousEpisodeSousLibelle = Resultats.Select(Function(item) item.SousEpisodeSousLibelle).Distinct().ToList.Select(Function(obj) New SelectListItem() With {.Value = obj, .Text = obj}).Reverse.Append(New SelectListItem() With {.Value = "Tous", .Text = "Tous"}).Reverse.ToList
            ViewData("SousEpisodeSousLibelle") = SousEpisodeSousLibelle

            For x = 0 To Resultats.Count - 1
                Resultats(x).NomFichier = Resultats(x).GetFilenameServer(Resultats(x).EpisodeId)
            Next

            ViewBag.Resultats = Resultats.GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element}).Take(10)


            Return View()
        End Function

        Private Function ChargementResultats(patientId As Long) As List(Of SousEpisodeReponse)
            Dim sousEpisodeReponseDao As New SousEpisodeReponseDao
            Dim tacheDao As New TacheDao

            Return sousEpisodeReponseDao.GetReponseCompleteByUser(patientId)
        End Function

    End Class
End Namespace
