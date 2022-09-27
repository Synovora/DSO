Imports System.IO
Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class ResultatsController
        Inherits Controller

        ReadOnly fileExtensionDao As New FileExtensionDao
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
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", fileName))
            Response.TransmitFile(filePath)
            Response.End()

            Return Nothing
        End Function

        <Authorize>
        Public Function Index(ByVal MySousEpisodeLibelles As String, ByVal MySousEpisodeSousLibelle As String) As ActionResult
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

            Dim Extensions = fileExtensionDao.GetAllFileExtension()
            Dim Resultats = ChargementResultats(patient.PatientId)

            Dim sousEpisodeLibelles = Resultats.Select(Function(item) item.SousEpisodeLibelle).Distinct().ToList.Select(Function(obj) New SelectListItem() With {.Value = obj, .Text = obj}).Reverse.Append(New SelectListItem() With {.Value = "Tous", .Text = "Tous"}).Reverse.ToList
            ViewData("sousEpisodeLibelles") = sousEpisodeLibelles

            Dim SousEpisodeSousLibelle = New List(Of SelectListItem)
            If Not (MySousEpisodeLibelles Is Nothing OrElse MySousEpisodeLibelles = "Tous") Then
                SousEpisodeSousLibelle = Resultats.Where(Function(x) x.SousEpisodeLibelle = MySousEpisodeLibelles).ToList().Select(Function(item) item.SousEpisodeSousLibelle).Distinct().ToList.Select(Function(obj) New SelectListItem() With {.Value = obj, .Text = obj}).Reverse.Append(New SelectListItem() With {.Value = "Tous", .Text = "Tous"}).Reverse.ToList
            End If
            ViewData("SousEpisodeSousLibelle") = SousEpisodeSousLibelle

            For x = 0 To Resultats.Count - 1
                Resultats(x).NomFichier = Resultats(x).GetFilenameServer(Resultats(x).EpisodeId)
                Resultats(x).Commentaire = Coalesce(Extensions.Find(Function(y) y.Extension = Path.GetExtension(Resultats(x).NomFichier).Replace(".", ""))?.Description, "fichier inconnu")
            Next

            If MySousEpisodeLibelles Is Nothing OrElse MySousEpisodeLibelles = "Tous" Then
                ViewBag.Resultats = Resultats.GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element}).Take(12)
            Else
                If MySousEpisodeSousLibelle Is Nothing OrElse SousEpisodeSousLibelle.Find(Function(x) x.Value = MySousEpisodeSousLibelle) Is Nothing OrElse MySousEpisodeSousLibelle = "Tous" Then
                    ViewBag.Resultats = Resultats.Where(Function(x) x.SousEpisodeLibelle = MySousEpisodeLibelles).ToList().GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element})
                Else
                    ViewBag.Resultats = Resultats.Where(Function(x) x.SousEpisodeLibelle = MySousEpisodeLibelles AndAlso x.SousEpisodeSousLibelle = MySousEpisodeSousLibelle).ToList().GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element})
                End If
            End If

            Return View()
        End Function

        Private Function ChargementResultats(patientId As Long) As List(Of SousEpisodeReponse)
            Dim sousEpisodeReponseDao As New SousEpisodeReponseDao
            Dim tacheDao As New TacheDao

            Return sousEpisodeReponseDao.GetReponseCompleteByUser(patientId)
        End Function

    End Class
End Namespace
