Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls.UI
Public Class RadFEpisodeParametresCreation
    Private _SelectedPatient As Patient

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Dim episodeDao As New EpisodeDao

    Private Sub RadFEpisodeParametresCreation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        NumDateRV.Value = Date.Now()
        NumheureRV.Value = Date.Now.Hour
        RadioBtn0.Checked = True
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip1.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If NumDateRV.Value.Date > Date.Now.Date Then
            MessageBox.Show("La date de saisie des paramètres doit être inférieure ou égale à la date du jour !")
        Else
            Dim episode As New Episode
            episode.Commentaire = TxtCommentaire.Text
            episode.DateCreation = Date.Now()
            episode.UserCreation = userLog.UtilisateurId
            episode.PatientId = SelectedPatient.patientId
            episode.Type = EpisodeDao.EnumTypeEpisode.PARAMETRE.ToString
            episode.TypeActivite = ""
            episode.TypeProfil = userLog.TypeProfil
            episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString

            Dim episodeId As Long
            episodeId = episodeDao.CreateEpisode(episode)
            If episodeId <> 0 Then
                'Création parametres aigus standards


                Using form As New RadFEpisodeParametresSaisie
                    form.SelectedPatient = SelectedPatient
                    form.SelectedEpisodeId = episodeId
                    form.ShowDialog()
                End Using
            End If
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
