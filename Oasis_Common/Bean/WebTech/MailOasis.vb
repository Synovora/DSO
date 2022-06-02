Imports Oasis_Common.ParametreMail
Imports System.IO

Public Class MailOasis
    Inherits Mail

    Property IdSiege As Long
    Property IsSousEpisode As Boolean = False
    Property Type As TypeMailParams

    Public Sub ReplaceZonesVariables(Optional patient As Patient = Nothing, Optional sousEpisode As SousEpisode = Nothing)
        Dim parametreMailDao As New ParametreMailDao
        Dim parametreMail = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(patient.PatientSiegeId, Me.Type)
        Me.IsHTML = parametreMail.IsBodyHtml
        Me.Body = parametreMail.Body
        Me.Subject = parametreMail.Objet

        Select Case Me.Type
            Case ParametreMail.TypeMailParams.CARNET_VACCINAL
                Body = Body.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
            Case ParametreMail.TypeMailParams.SYNTHESE
                Body = Body.Replace("@Reference", If(sousEpisode IsNot Nothing, sousEpisode.Reference, ""))
                Body = Body.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
            Case Else
                Body = Body.Replace("@DateCreation", If(sousEpisode IsNot Nothing, sousEpisode.HorodateCreation.ToString("dd/MM/yyyy"), ""))
        End Select

        If patient IsNot Nothing Then
            Dim adrSiege As String = ""
            Dim siegeDao = New SiegeDao
            Dim siege = siegeDao.getSiegeById(patient.PatientSiegeId)
            With siege
                If .SiegeDescription.Trim() <> String.Empty Then adrSiege = .SiegeDescription & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeAdresse1.Trim() <> String.Empty Then adrSiege += .SiegeAdresse1 & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeAdresse2.Trim() <> String.Empty Then adrSiege += .SiegeAdresse2 & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If (.SiegeCodePostal + .SiegeVille) <> String.Empty Then adrSiege += .SiegeCodePostal & " " & .SiegeVille & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeTelephone.Trim() <> String.Empty Then adrSiege += "Tél : " & .SiegeTelephone & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeFax.Trim() <> String.Empty Then adrSiege += "Fax : " & .SiegeFax & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeMail.Trim() <> String.Empty Then adrSiege += "Email : " & .SiegeMail & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
            End With

            Body = Body.Replace("@PatientNom", patient.PatientNom) _
                     .Replace("@PatientPrenom", patient.PatientPrenom) _
                     .Replace("@SiegeCoord", adrSiege)
        End If
    End Sub

End Class
