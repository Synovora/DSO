Imports System.IO
Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class ResultatsController
        Inherits Controller

        ReadOnly sousEpisodeReponseDao As New SousEpisodeReponseDao
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
        Public Function Index(ByVal MySousEpisodeLibelles As String, ByVal MySousEpisodeSousLibelle As String, Optional Page As Integer = 0) As ActionResult
            Dim internauteConnectionDao As New InternauteConnectionDao
            Dim strName As String = Constants.LAYOUT_VERTICAL
            Dim strWelcomeText As String = "Resultats"
            Dim nbrOfItems = 10

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
            Dim Resultats = sousEpisodeReponseDao.GetReponseCompleteByUser(patient.PatientId)
            Dim Filter = sousEpisodeReponseDao.GetAllFilterByUser(patient.PatientId)
            ViewData("Page") = Coalesce(Page, 1)
            ViewData("PageCount") = nbrOfItems

            Dim sousEpisodeLibelles = Filter.Select(Function(obj) obj(0)).Distinct.Select(Function(obj) New SelectListItem() With {.Value = obj, .Text = obj}).Reverse.Append(New SelectListItem() With {.Value = "Tous", .Text = "Tous"}).Reverse.ToList
            ViewData("sousEpisodeLibelles") = sousEpisodeLibelles

            Dim SousEpisodeSousLibelle = New List(Of SelectListItem)
            If Not (MySousEpisodeLibelles Is Nothing OrElse MySousEpisodeLibelles = "Tous") Then
                SousEpisodeSousLibelle = Filter.Where(Function(x) x(0) = MySousEpisodeLibelles).ToList().Select(Function(item) item(1)).ToList.Select(Function(obj) New SelectListItem() With {.Value = obj, .Text = obj}).Reverse.Append(New SelectListItem() With {.Value = "Tous", .Text = "Tous"}).Reverse.ToList
            End If
            ViewData("SousEpisodeSousLibelle") = SousEpisodeSousLibelle

            For Each resultat In Resultats
                resultat.NomFichier = resultat.GetFilenameServer(resultat.EpisodeId)
                resultat.Commentaire = Coalesce(Extensions.Find(Function(y) y.Extension = Path.GetExtension(resultat.NomFichier).Replace(".", ""))?.Description, "fichier inconnu")
            Next

            Dim result = Nothing

            If MySousEpisodeLibelles Is Nothing OrElse MySousEpisodeLibelles = "Tous" Then
                result = Resultats.GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element}).Skip(Page * nbrOfItems).Take(nbrOfItems).ToList()
                ViewData("PageTotal") = Resultats.GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element}).ToList().Count()
            Else
                If MySousEpisodeSousLibelle Is Nothing OrElse SousEpisodeSousLibelle.Find(Function(x) x.Value = MySousEpisodeSousLibelle) Is Nothing OrElse MySousEpisodeSousLibelle = "Tous" Then
                    result = Resultats.Where(Function(x) x.SousEpisodeLibelle = MySousEpisodeLibelles).ToList().GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element}).Skip(Page * nbrOfItems).Take(nbrOfItems).ToList()
                    ViewData("PageTotal") = Resultats.Where(Function(x) x.SousEpisodeLibelle = MySousEpisodeLibelles).ToList().GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element}).Count()
                Else
                    result = Resultats.Where(Function(x) x.SousEpisodeLibelle = MySousEpisodeLibelles AndAlso x.SousEpisodeSousLibelle = MySousEpisodeSousLibelle).ToList().GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element}).Skip(Page * nbrOfItems).Take(nbrOfItems).ToList()
                    ViewData("PageTotal") = Resultats.Where(Function(x) x.SousEpisodeLibelle = MySousEpisodeLibelles AndAlso x.SousEpisodeSousLibelle = MySousEpisodeSousLibelle).ToList().GroupBy(Function(x) x.IdSousEpisode, Function(key, element) New With {Key .Value = key, Key .Element = element}).Count()
                End If
            End If
            ViewBag.Resultats = result

            Return View()
        End Function

        Public Function Pagination(ByVal Page As Integer) As ActionResult

            ViewBag("Page") = Page
        End Function

    End Class
End Namespace
