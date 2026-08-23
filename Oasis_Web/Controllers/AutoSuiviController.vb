Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http.Formatting
Imports System.Web
Imports System.Web.Http
Imports System.Web.Mvc
Imports System.Globalization
Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class AutoSuiviController
        Inherits PortailController

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

            Dim patient = GetPatientConnecte()
            If patient Is Nothing Then Return AccesRefuse()
            ViewBag.Patient = patient
            ViewBag.ParametresAutoSuivi = BuildAutoSuiviList(patient.PatientId)
            Return View()
        End Function

        'AutoSuiviValidate POST
        <System.Web.Mvc.HttpPost>
        <ValidateAntiForgeryToken>
        <System.Web.Mvc.Authorize>
        Public Function AutoSuiviValidate(data As String) As ActionResult
            Dim patient = GetPatientConnecte()
            If patient Is Nothing Then Return AccesRefuse()

            ' On n'accepte que les paramètres réellement proposés à ce patient.
            Dim parametresAutorises = BuildAutoSuiviList(patient.PatientId).ToDictionary(Function(p) CLng(p.Id))

            ' Parsage et validation AVANT toute écriture, pour ne pas laisser
            ' d'épisode orphelin en cas d'entrée invalide.
            Dim mesures As New List(Of KeyValuePair(Of Parametre, Decimal))
            For Each couple In WebUtility.UrlDecode(If(data, "")).Split("&"c)
                Dim morceaux = couple.Split("="c)
                If morceaux.Length <> 2 Then Continue For
                Dim key As Long
                Dim valeur As Decimal
                If Not Long.TryParse(morceaux(0), key) Then Continue For
                If String.IsNullOrWhiteSpace(morceaux(1)) Then Continue For
                If Not Decimal.TryParse(morceaux(1), NumberStyles.Number, CultureInfo.InvariantCulture, valeur) Then
                    Return New HttpStatusCodeResult(400, "Valeur invalide")
                End If
                Dim parametre As Parametre = Nothing
                If Not parametresAutorises.TryGetValue(key, parametre) Then
                    Return New HttpStatusCodeResult(400, "Paramètre non autorisé")
                End If
                mesures.Add(New KeyValuePair(Of Parametre, Decimal)(parametre, valeur))
            Next

            If mesures.Count = 0 Then
                Return New HttpStatusCodeResult(400, "Aucune mesure valide")
            End If

            Dim episode As New Episode With {
                .Commentaire = "AutoSuivi",
                .DateCreation = Date.Now,
                .UserCreation = 0,
                .PatientId = patient.PatientId,
                .Type = Episode.EnumTypeEpisode.PARAMETRE.ToString,
                .TypeActivite = Episode.EnumTypeEpisode.PARAMETRE.ToString,
                .DescriptionActivite = "",
                .TypeProfil = ProfilDao.EnumProfilType.PATIENT.ToString,
                .Etat = Episode.EnumEtatEpisode.CLOTURE.ToString
            }
            Dim episodeId As Long = episodeDao.CreateEpisode(episode, 0)
            If episodeId = 0 Then
                Return New HttpStatusCodeResult(500, "Création de l'épisode impossible")
            End If

            For Each mesure In mesures
                Dim parametre = mesure.Key
                episodeParametreDao.CreateEpisodeParametre(New EpisodeParametre With {
                    .EpisodeId = episodeId,
                    .ParametreId = parametre.Id,
                    .PatientId = episode.PatientId,
                    .Entier = parametre.Entier,
                    .Decimal = parametre.Decimal,
                    .Unite = parametre.Unite,
                    .Ordre = parametre.Ordre,
                    .Description = parametre.Description,
                    .Valeur = mesure.Value,
                    .Inactif = False
                })
            Next
            Session("autosuivi") = True
            Return New HttpStatusCodeResult(200)
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
