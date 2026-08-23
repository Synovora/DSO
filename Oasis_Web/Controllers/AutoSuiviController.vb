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
    Public Class AutoSuiviController
        Inherits Controller

        ReadOnly parametreDao As New ParametreDao
        ReadOnly episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
        ReadOnly episodeDao As New EpisodeDao
        ReadOnly patientDao As New PatientDao
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
            ViewBag.ParametresAutoSuivi = BuildAutoSuiviList(patient.PatientId)
            For i As Integer = 0 To ViewBag.ParametresAutoSuivi.Count - 1
                System.Diagnostics.Debug.WriteLine(ViewBag.ParametresAutoSuivi(i).Description.ToString() & ViewBag.ParametresAutoSuivi(i).Id.ToString())
            Next i
            Return View()
        End Function

        'AutoSuiviValidate POST
        <System.Web.Mvc.HttpPost>
        <ValidateAntiForgeryToken>
        <System.Web.Mvc.Authorize>
        Public Function AutoSuiviValidate(data As String) As ActionResult
            Dim parametres = (WebUtility.UrlDecode(data)).Split("&")
            Dim patientId = Request.Cookies("patientId").Value
            Dim episode As New Episode With {
                .Commentaire = "AutoSuivi",
                .DateCreation = Date.Now,
                .UserCreation = 0,
                .PatientId = patientId,
                .Type = Episode.EnumTypeEpisode.PARAMETRE.ToString,
                .TypeActivite = Episode.EnumTypeEpisode.PARAMETRE.ToString,
                .DescriptionActivite = "",
                .TypeProfil = ProfilDao.EnumProfilType.PATIENT.ToString,
                .Etat = Episode.EnumEtatEpisode.CLOTURE.ToString
            }
            Debug.WriteLine(data, parametres)
            Dim episodeId As Long = episodeDao.CreateEpisode(episode, 0)
            If episodeId <> 0 Then
                For i = 0 To parametres.Count - 1
                    Dim key = parametres(i).Split("=")(0)
                    Debug.WriteLine(key)
                    Dim value = Coalesce(parametres(i).Split("=")(1), Nothing)
                    Debug.WriteLine(value)
                    If value = Nothing Then
                        Continue For
                    End If
                    Dim parametre = parametreDao.GetParametreById(key)
                    'Creation
                    Dim episodeParametre As EpisodeParametre = New EpisodeParametre With {
                        .EpisodeId = episodeId,
                        .ParametreId = parametre.Id,
                        .PatientId = episode.PatientId,
                        .Entier = parametre.Entier,
                        .Decimal = parametre.Decimal,
                        .Unite = parametre.Unite,
                        .Ordre = parametre.Ordre,
                        .Description = parametre.Description,
                        .Valeur = Decimal.Parse(value),
                        .Inactif = False
                    }
                    Debug.WriteLine(value, parametre.Entier)
                    episodeParametreDao.CreateEpisodeParametre(episodeParametre)
                Next
                Session("autosuivi") = True
            End If
        End Function


        Private Function BuildAutoSuiviList(patientId As Integer) As List(Of Parametre)
            Dim parametres As List(Of Parametre) = New List(Of Parametre)
            Dim TypeActiviteAcode As String = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE
            Dim ListParametres As List(Of Long) = episodeProtocoleCollaboratifDao.GetListeParametreByPatientEtTypeEpisode(patientId, TypeActiviteAcode)
            For i = 0 To ListParametres.Count - 1
                Dim parametre = parametreDao.GetParametreById(ListParametres.Item(i))
                If parametre.ExclusionAutoSuivi = True Then
                    Continue For
                End If
                'TODO: apply autosuivi mask
                'Dim autoSuivi = autoSuiviDao.GetAutoSuiviByPatientIdAndParametreId(patientId, ListParametres.Item(i))

                parametres.Add(parametre)
            Next
            Return parametres
        End Function

    End Class
End Namespace
