Imports Oasis_Common

Public Class RadFEpisodeObservationDetailEdit
    Property SelectedEpisodeId As Long
    Property SelectedPatient As Patient
    Property SelectedObservationId As Long
    Property CodeRetour As Boolean

    Dim EditMode As Integer

    ReadOnly episodeObservationDao As New EpisodeObservationDao
    ReadOnly userDao As New UserDao

    Dim episodeObservationRead As EpisodeObservation
    Dim episodeObservationUpdate As EpisodeObservation
    Dim user As New Utilisateur

    Private Sub RadFEpisodeObservationDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Observation libre", userLog)
        Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Me.CodeRetour = False

        CbxPresence.Items.Clear()
        CbxPresence.Items.Add(EpisodeObservation.EnumNaturePresence.DISTANT.ToString)
        CbxPresence.Items.Add(EpisodeObservation.EnumNaturePresence.PRESENTIEL.ToString)

        If SelectedObservationId <> 0 Then
            EditMode = EpisodeTypeActivite.EnumEditMode.Modification
            episodeObservationRead = episodeObservationDao.GetEpisodeObservationById(SelectedObservationId)
            episodeObservationUpdate = episodeObservationRead.Clone()
            ChargementObservation()
            If userLog.UtilisateurId <> episodeObservationRead.UserCreation AndAlso userLog.UtilisateurAdmin = False Then
                InhibeModificationObservation()
                ToolTip.SetToolTip(Me.TxtObservation, "Observation non modifiable, seul l'auteur ou un administrateur peut la modifier")
            Else
                Dim temps As Integer
                temps = DateDiff(DateInterval.Hour, episodeObservationUpdate.DateCreation, Date.Now)
                If temps > 24 AndAlso userLog.UtilisateurAdmin = False Then
                    InhibeModificationObservation()
                    ToolTip.SetToolTip(Me.TxtObservation, "Observation non modifiable car elle a été créée depuis plus de 24 heures, seul un administrateur peut la modifier")
                Else
                    Select Case userLog.TypeProfil
                        Case ProfilDao.EnumProfilType.MEDICAL.ToString
                            If episodeObservationRead.TypeObservation <> ProfilDao.EnumProfilType.MEDICAL.ToString Then
                                InhibeModificationObservation()
                            End If
                        Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                            If episodeObservationRead.TypeObservation <> ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
                                InhibeModificationObservation()
                            End If
                        Case Else
                            InhibeModificationObservation()
                    End Select
                End If
            End If
        Else
            EditMode = EpisodeTypeActivite.EnumEditMode.Creation
            InitialisationCreation()
        End If
    End Sub

    Private Sub InhibeModificationObservation()
        RadBtnValidation.Hide()
        TxtObservation.ReadOnly = True
        CbxPresence.Enabled = False
    End Sub

    Private Sub TxtObservation_TextChanged(sender As Object, e As EventArgs) Handles TxtObservation.TextChanged
        episodeObservationUpdate.Observation = TxtObservation.Text
        GestionAffichageBoutonValidation()
    End Sub


    Private Sub CbxPresence_TextChanged(sender As Object, e As EventArgs) Handles CbxPresence.TextChanged
        episodeObservationUpdate.NaturePresence = CbxPresence.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub GestionAffichageBoutonValidation()
        If EditMode = EpisodeTypeActivite.EnumEditMode.Modification Then
            If episodeObservationDao.Compare(episodeObservationUpdate, episodeObservationRead) = False Then
                RadBtnValidation.Enabled = True
            Else
                RadBtnValidation.Enabled = False
            End If
        End If
    End Sub

    Private Sub ChargementObservation()
        If episodeObservationRead.DateCreation <> Nothing Then
            LblCreationObs1.Show()
            LblCreationObs3.Show()
            LblObsDateCreation.Text = episodeObservationRead.DateCreation.ToString("dd.MM.yyyy")
            LblObsHeureCreation.Text = episodeObservationRead.DateCreation.ToString("HH:mm")
        Else
            LblCreationObs1.Hide()
            LblObsDateCreation.Hide()
        End If

        If episodeObservationRead.UserCreation <> 0 Then
            user = userDao.getUserById(episodeObservationRead.UserCreation)
            LblCreationObs2.Show()
            LblUtilisateurCreation.Text = user.UtilisateurPrenom & " " & user.UtilisateurNom
        Else
            LblCreationObs2.Hide()
            LblUtilisateurCreation.Hide()
        End If

        If episodeObservationRead.DateModification <> Nothing Then
            LblModificationObs.Show()
            LblObsDateModification.Text = episodeObservationRead.DateModification.ToString("dd.MM.yyyy")
        Else
            LblModificationObs.Hide()
            LblObsDateModification.Hide()
        End If

        Select Case episodeObservationRead.NaturePresence
            Case ProfilDao.EnumProfilType.MEDICAL.ToString
                CbxPresence.Text = EpisodeObservation.EnumNaturePresence.DISTANT.ToString
            Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                CbxPresence.Text = EpisodeObservation.EnumNaturePresence.PRESENTIEL.ToString
            Case Else
                Select Case userLog.TypeProfil
                    Case ProfilDao.EnumProfilType.MEDICAL.ToString
                        episodeObservationUpdate.NaturePresence = EpisodeObservation.EnumNaturePresence.DISTANT.ToString
                        CbxPresence.Text = EpisodeObservation.EnumNaturePresence.DISTANT.ToString
                    Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                        episodeObservationUpdate.NaturePresence = EpisodeObservation.EnumNaturePresence.PRESENTIEL.ToString
                        CbxPresence.Text = EpisodeObservation.EnumNaturePresence.PRESENTIEL.ToString
                    Case Else
                        MessageBox.Show("Erreur, ce profil ne peut pas créer d'observation libre (" & userLog.TypeProfil & ")")
                        Close()
                End Select
        End Select

        TxtObservation.Text = episodeObservationRead.Observation

        GestionAffichageBoutonValidation()
    End Sub

    Private Sub InitialisationCreation()
        LblCreationObs1.Hide()
        LblObsDateCreation.Hide()
        LblObsHeureCreation.Hide()
        LblCreationObs2.Hide()
        LblCreationObs3.Hide()
        LblUtilisateurCreation.Hide()
        LblModificationObs.Hide()
        LblObsDateModification.Hide()

        episodeObservationUpdate = New EpisodeObservation
        episodeObservationUpdate.Id = 0
        episodeObservationUpdate.EpisodeId = SelectedEpisodeId
        episodeObservationUpdate.PatientId = SelectedPatient.PatientId
        episodeObservationUpdate.UserCreation = userLog.UtilisateurId
        episodeObservationUpdate.TypeObservation = userLog.TypeProfil
        episodeObservationUpdate.NatureObservation = EpisodeObservation.EnumNatureEpisodeObservation.LIBRE.ToString
        Select Case userLog.TypeProfil
            Case ProfilDao.EnumProfilType.MEDICAL.ToString
                episodeObservationUpdate.NaturePresence = EpisodeObservation.EnumNaturePresence.DISTANT.ToString
                CbxPresence.Text = EpisodeObservation.EnumNaturePresence.DISTANT.ToString
            Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                episodeObservationUpdate.NaturePresence = EpisodeObservation.EnumNaturePresence.PRESENTIEL.ToString
                CbxPresence.Text = EpisodeObservation.EnumNaturePresence.PRESENTIEL.ToString
            Case Else
                MessageBox.Show("Erreur, ce profil ne peut pas créer d'observation libre (" & userLog.TypeProfil & ")")
                Close()
        End Select
        episodeObservationUpdate.Observation = ""
        TxtObservation.Text = ""

        RadBtnValidation.Enabled = True
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If TxtObservation.Text = "" Then
            MessageBox.Show("La saisie de l'observation est obligatoire!")
        Else
            Select Case EditMode
                Case EpisodeTypeActivite.EnumEditMode.Creation
                    If episodeObservationDao.CreateEpisodeObservation(episodeObservationUpdate) = True Then
                        MessageBox.Show("Observation créée")
                        Me.CodeRetour = True
                        Close()
                    End If
                Case EpisodeTypeActivite.EnumEditMode.Modification
                    If episodeObservationDao.ModificationEpisodeObservation(episodeObservationUpdate) = True Then
                        MessageBox.Show("Observation modifiée")
                        Me.CodeRetour = True
                        Close()
                    End If
            End Select
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
