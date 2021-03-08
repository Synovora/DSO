Imports Oasis_Common

Module EpisodeUtils
    Public Function CallEpisode(selectedPatient As Patient, rendezVousId As Long, userLog As Utilisateur, Optional EcransPrecedent As EnumAccesEcranPrecedent = EnumAccesEcranPrecedent.SANS) As Boolean
        Dim IsRendezVousCloture As Boolean = False
        'Tester si l'utilisateur a une fonction de type MEDICAL ou PARAMEDICALE
        If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Or userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString) Then
            Dim Message1 As String = "Votre profil de type (" & userLog.TypeProfil & ") ne vous permet pas de gérer un épisode patient, processus annulé"
            Dim Message2 As String = "Les types de profil autorisés sont : " & ProfilDao.EnumProfilType.MEDICAL.ToString() & " et " & ProfilDao.EnumProfilType.PARAMEDICAL.ToString()
            Throw New Exception(Message1 & vbCrLf & Message2)
            Return False
            Exit Function
        End If

        Dim episodeDao As New EpisodeDao
        Dim episode As Episode
        episode = episodeDao.GetEpisodeEnCoursByPatientId(selectedPatient.PatientId)
        If episode.Id = 0 Then
            Using vRadFEpisodeDetailCreation As New RadFEpisodeDetailCreation
                vRadFEpisodeDetailCreation.SelectedPatient = selectedPatient
                vRadFEpisodeDetailCreation.ShowDialog()
                If vRadFEpisodeDetailCreation.CodeRetour = True Then
                    Using vRadFEpisodeDetail As New RadFEpisodeDetail
                        vRadFEpisodeDetail.SelectedEpisodeId = vRadFEpisodeDetailCreation.EpisodeId
                        vRadFEpisodeDetail.SelectedPatient = selectedPatient
                        vRadFEpisodeDetail.RendezVousId = rendezVousId
                        vRadFEpisodeDetail.UtilisateurConnecte = userLog
                        vRadFEpisodeDetail.EcranPrecedent = EcransPrecedent
                        vRadFEpisodeDetail.ShowDialog()
                        IsRendezVousCloture = vRadFEpisodeDetail.IsRendezVousCloture
                    End Using
                End If
            End Using
        Else
            Cursor.Current = Cursors.WaitCursor
            Using vRadFEpisodeDetail As New RadFEpisodeDetail
                vRadFEpisodeDetail.SelectedEpisodeId = episode.Id
                vRadFEpisodeDetail.SelectedPatient = selectedPatient
                vRadFEpisodeDetail.RendezVousId = rendezVousId
                vRadFEpisodeDetail.UtilisateurConnecte = userLog
                vRadFEpisodeDetail.ShowDialog()
                IsRendezVousCloture = vRadFEpisodeDetail.IsRendezVousCloture
            End Using
        End If

        Return IsRendezVousCloture
    End Function
End Module
