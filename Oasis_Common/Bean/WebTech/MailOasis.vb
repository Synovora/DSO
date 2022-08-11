Imports Oasis_Common.ParametreMail

Public Class MailOasis
    Inherits Mail

    Property Patient As Patient = Nothing
    Property Internaute As Internaute = Nothing
    Property SousEpisode As SousEpisode = Nothing
    Property IsSousEpisode As Boolean = False
    Property Type As TypeMailParams

    Public Sub New()
    End Sub

    Public Sub New(_Type As TypeMailParams, Optional _Patient As Patient = Nothing, Optional _Internaute As Internaute = Nothing, Optional _SousEpisode As SousEpisode = Nothing, Optional _IsSousEpisode As Boolean = False)
        Type = _Type
        Patient = _Patient
        Internaute = _Internaute
        SousEpisode = _SousEpisode
        IsSousEpisode = _IsSousEpisode

        Dim parametreMailDao As New ParametreMailDao
        Dim parametreMail = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(Nothing, Me.Type)

        Me.IsHTML = Me.IsHTML
        Me.Body = parametreMail.Body
        Me.Subject = parametreMail.Objet

        Process()
    End Sub

    Private Sub Process()
        'DATE
        Select Case Me.Type
            Case ParametreMail.TypeMailParams.CARNET_VACCINAL
                Body = Body.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
                Subject = Subject.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
            Case ParametreMail.TypeMailParams.SYNTHESE
                Body = Body.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
                Subject = Subject.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
        End Select

        If Patient IsNot Nothing Then
            'SIEGE
            Dim adrSiege As String = ""
            Dim siegeDao = New SiegeDao
            Dim siege = siegeDao.getSiegeById(Patient.PatientSiegeId)
            With siege
                If .SiegeDescription.Trim() <> String.Empty Then adrSiege = .SiegeDescription & If(Me.IsHTML, "<br />", vbCrLf)
                If .SiegeAdresse1.Trim() <> String.Empty Then adrSiege += .SiegeAdresse1 & If(Me.IsHTML, "<br />", vbCrLf)
                If .SiegeAdresse2.Trim() <> String.Empty Then adrSiege += .SiegeAdresse2 & If(Me.IsHTML, "<br />", vbCrLf)
                If (.SiegeCodePostal + .SiegeVille) <> String.Empty Then adrSiege += .SiegeCodePostal & " " & .SiegeVille & If(Me.IsHTML, "<br />", vbCrLf)
                If .SiegeTelephone.Trim() <> String.Empty Then adrSiege += "Tél : " & .SiegeTelephone & If(Me.IsHTML, "<br />", vbCrLf)
                If .SiegeFax.Trim() <> String.Empty Then adrSiege += "Fax : " & .SiegeFax & If(Me.IsHTML, "<br />", vbCrLf)
                If .SiegeMail.Trim() <> String.Empty Then adrSiege += "Email : " & .SiegeMail & If(Me.IsHTML, "<br />", vbCrLf)
            End With
            Body = Body.Replace("@SiegeCoord", adrSiege)
            Subject = Subject.Replace("@SiegeCoord", adrSiege)

            ' DEFAULT
            Body = Body.Replace("@PatientNom", Patient.PatientNom).Replace("@PatientPrenom", Patient.PatientPrenom) _
                .Replace("@PatientNir", Patient.PatientNir).Replace("@PatientEmail", Patient.PatientEmail) _
                .Replace("@PatientGenre", Patient.PatientGenre).Replace("@PatientDateNaissance", Patient.PatientDateNaissance)

            Subject = Subject.Replace("@PatientNom", Patient.PatientNom).Replace("@PatientPrenom", Patient.PatientPrenom) _
                 .Replace("@PatientNir", Patient.PatientNir).Replace("@PatientEmail", Patient.PatientEmail) _
                 .Replace("@PatientGenre", Patient.PatientGenre).Replace("@PatientDateNaissance", Patient.PatientDateNaissance)
        End If

        If SousEpisode IsNot Nothing Then
            Body = Body.Replace("@Reference", SousEpisode.Reference)
            Body = Body.Replace("@DateCreation", If(SousEpisode IsNot Nothing, SousEpisode.HorodateCreation.ToString("dd/MM/yyyy"), ""))
            Subject = Subject.Replace("@Reference", SousEpisode.Reference)
            Subject = Subject.Replace("@DateCreation", If(SousEpisode IsNot Nothing, SousEpisode.HorodateCreation.ToString("dd/MM/yyyy"), ""))
        End If

        If Internaute IsNot Nothing Then
            Body = Body.Replace("@InternauteRecovery", "https://ns3119889.ip-51-38-181.eu/Auth/Recover?key=" & Internaute.Recovery).Replace("@InternauteEmail", Internaute.Email).Replace("@InternauteUsername", Internaute.Username)
            Subject = Subject.Replace("@InternauteRecovery", "https://ns3119889.ip-51-38-181.eu/Auth/Recover?key=" & Internaute.Recovery).Replace("@InternauteEmail", Internaute.Email).Replace("@InternauteUsername", Internaute.Username)
        End If
    End Sub

End Class
