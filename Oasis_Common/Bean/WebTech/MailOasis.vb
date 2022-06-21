Imports Oasis_Common.ParametreMail

Public Class MailOasis
    Inherits Mail

    Property Patient As Patient = Nothing
    Property Internaute As Internaute = Nothing
    Property SousEpisode As SousEpisode = Nothing
    Property IsSousEpisode As Boolean = False
    Property Type As TypeMailParams

    Public Overloads Sub Send(loginRequestLog As LoginRequest)
        Dim parametreMailDao As New ParametreMailDao
        Dim parametreMail = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(Nothing, Me.Type)
        Me.IsHTML = parametreMail.IsBodyHtml
        Me.Body = parametreMail.Body
        Me.Subject = parametreMail.Objet

        'DATE
        Select Case Me.Type
            Case ParametreMail.TypeMailParams.CARNET_VACCINAL
                Body = Body.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
            Case ParametreMail.TypeMailParams.SYNTHESE
                Body = Body.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
        End Select

        If Patient IsNot Nothing Then
            'SIEGE
            Dim adrSiege As String = ""
            Dim siegeDao = New SiegeDao
            Dim siege = siegeDao.getSiegeById(Patient.PatientSiegeId)
            With siege
                If .SiegeDescription.Trim() <> String.Empty Then adrSiege = .SiegeDescription & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeAdresse1.Trim() <> String.Empty Then adrSiege += .SiegeAdresse1 & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeAdresse2.Trim() <> String.Empty Then adrSiege += .SiegeAdresse2 & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If (.SiegeCodePostal + .SiegeVille) <> String.Empty Then adrSiege += .SiegeCodePostal & " " & .SiegeVille & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeTelephone.Trim() <> String.Empty Then adrSiege += "Tél : " & .SiegeTelephone & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeFax.Trim() <> String.Empty Then adrSiege += "Fax : " & .SiegeFax & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeMail.Trim() <> String.Empty Then adrSiege += "Email : " & .SiegeMail & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
            End With
            Body = Body.Replace("@SiegeCoord", adrSiege)

            ' DEFAULT
            Body = Body.Replace("@PatientNom", Patient.PatientNom).Replace("@PatientPrenom", Patient.PatientPrenom) _
                .Replace("@PatientNir", Patient.PatientNir).Replace("@PatientEmail", Patient.PatientEmail) _
                .Replace("@PatientGenre", Patient.PatientGenre).Replace("@PatientDateNaissance", Patient.PatientDateNaissance)
        End If

        If SousEpisode IsNot Nothing Then
            Body = Body.Replace("@Reference", SousEpisode.Reference)
            Body = Body.Replace("@DateCreation", If(SousEpisode IsNot Nothing, SousEpisode.HorodateCreation.ToString("dd/MM/yyyy"), ""))
        End If

        If Internaute IsNot Nothing Then
            Body = Body.Replace("@InternauteRecovery", "https://ns3119889.ip-51-38-181.eu/Auth/Recover?key=" & Internaute.Recovery).Replace("@InternauteEmail", Internaute.Email).Replace("@InternauteUsername", Internaute.Username)
        End If

        MyBase.Send(loginRequestLog)
    End Sub
End Class
