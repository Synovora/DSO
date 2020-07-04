Imports System.Configuration
Imports Oasis_Common

Public Class RadFEpisodeDetailCreation
    Private _SelectedPatient As PatientBase
    Private _episodeId As Long
    Private _CodeRetour As Boolean

    Public Property SelectedPatient As PatientBase
        Get
            Return _SelectedPatient
        End Get
        Set(value As PatientBase)
            _SelectedPatient = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _CodeRetour
        End Get
        Set(value As Boolean)
            _CodeRetour = value
        End Set
    End Property

    Public Property EpisodeId As Long
        Get
            Return _episodeId
        End Get
        Set(value As Long)
            _episodeId = value
        End Set
    End Property

    Dim episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
    Dim episodeActiviteDao As New EpisodeTypeActiviteDao
    Dim episodeActiviteDT As New DataTable
    Dim episodeDao As New EpisodeDao
    Dim contexteDao As New ContexteDao

    Private Sub RadFEpisodeDetailCreation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim TypeActiviteEpisode As String
        Dim LimiteAgeEnfant As Integer = 0
        Dim AgeMinPreventionFemme As Integer = 0
        Dim LimiteAgeEnfantParm As Integer = ConfigurationManager.AppSettings("limiteAgeEnfant")
        If IsNumeric(LimiteAgeEnfantParm) Then
            LimiteAgeEnfant = CInt(LimiteAgeEnfantParm)
        End If
        Dim AgeMinPreventionFemmeParm As Integer = ConfigurationManager.AppSettings("AgeMinPreventionFemme")
        If IsNumeric(AgeMinPreventionFemmeParm) Then
            AgeMinPreventionFemme = CInt(AgeMinPreventionFemmeParm)
        End If

        Dim genre, enfant As String
        Dim agePatient As Integer = CalculAgeEnAnnee(SelectedPatient.PatientDateNaissance)

        Dim listActivite As New List(Of String)
        episodeActiviteDT = episodeActiviteDao.GetAllEpisodeActivite
        Dim i As Integer
        Dim rowCount As Integer = episodeActiviteDT.Rows.Count - 1
        For i = 0 To rowCount Step 1
            genre = Coalesce(episodeActiviteDT.Rows(i)("oa_activite_genre"), "")
            If genre = "F" Then
                If SelectedPatient.PatientGenreId.Trim = "M" Then
                    Continue For
                End If
                If agePatient < AgeMinPreventionFemme Then
                    Continue For
                End If
            End If
            enfant = Coalesce(episodeActiviteDT.Rows(i)("oa_activite_enfant"), False)
            If enfant = True Then
                If LimiteAgeEnfant <> 0 Then
                    If agePatient > LimiteAgeEnfant Then
                        Continue For
                    End If
                End If
            End If

            TypeActiviteEpisode = episodeDao.GetItemTypeActiviteByCode(episodeActiviteDT.Rows(i)("oa_activite_type"))
            If TypeActiviteEpisode <> "" Then
                listActivite.Add(TypeActiviteEpisode)
            End If
        Next

        CbxEpisodeActivite.DataSource = listActivite

        RadioBtnConsultation.Checked = True

        Me.CodeRetour = False
        ChargementEtatCivil()

    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj)
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        'Création épisode
        Dim IsCreationOk As Boolean = False
        Dim episodeDao As New EpisodeDao
        Dim episode As New Episode
        episode.Commentaire = TxtCommentaire.Text
        episode.DescriptionActivite = TxtDescriptionActivite.Text
        If RadioBtnConsultation.Checked = True Then
            episode.Type = EpisodeDao.EnumTypeEpisode.CONSULTATION.ToString
        Else
            episode.Type = EpisodeDao.EnumTypeEpisode.VIRTUEL.ToString
        End If
        episode.TypeActivite = CbxEpisodeActivite.Text
        episode.TypeActivite = episodeDao.GetCodeTypeActiviteByItem(CbxEpisodeActivite.Text)
        If episode.TypeActivite <> "" Or episode.Type = EpisodeDao.EnumTypeEpisode.VIRTUEL.ToString Then
            If episode.TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE Then
                Dim DrcIdGrossesse As Integer = ConfigurationManager.AppSettings("DrcIdGrossesse")
                If DrcIdGrossesse = 0 Then
                    outils.CreateLog("DRC Grossesse non trouvée", Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
                    DrcIdGrossesse = 833
                End If
                If contexteDao.ExistContexteValideWithDrcId(SelectedPatient.patientId, DrcIdGrossesse) Then
                    IsCreationOk = True
                Else
                    IsCreationOk = False
                    MessageBox.Show("Pour créer un épisode de type 'Suivi grossesse', un contexte de type 'Grossesse' (code : " & DrcIdGrossesse.ToString & ") doit exister au préalable pour la patiente ")
                End If
            Else
                IsCreationOk = True
            End If
            If IsCreationOk Then
                episode.PatientId = SelectedPatient.patientId
                episode.TypeProfil = userLog.TypeProfil
                EpisodeId = episodeDao.CreateEpisode(episode)
                episode.Id = EpisodeId
                If EpisodeId <> 0 Then
                    Cursor.Current = Cursors.WaitCursor

                    'Génération paramètres et actes paramédicaux
                    episodeProtocoleCollaboratifDao.GenerateParametreEtProtocoleCollaboratifByEpisode(episode)

                    'Création automatique des intervenants Osis s'ils n'existent pas
                    Dim parcoursDao As New ParcoursDao
                    Try
                        Dim parcours As Parcours
                        parcours = parcoursDao.getParcoursIDEbyPatient(SelectedPatient.patientId)
                    Catch ex As Exception
                        If ex.Message.StartsWith("parcours inexistant") Then
                            parcoursDao.CreateIntervenantOasisByPatient(SelectedPatient.patientId, False)
                        End If
                    End Try

                    Cursor.Current = Cursors.Default
                    CodeRetour = True
                    Close()
                Else
                    MessageBox.Show("Erreur : la création de l'épisode n'a pas aboutie!")
                    CodeRetour = False
                    Close()
                End If
            End If
        Else
            MessageBox.Show("la saisie du type d'activité de l'épisode est obligatoire")
        End If

    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        CodeRetour = False
        Close()
    End Sub

    Private Sub RadioBtnConsultation_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnConsultation.CheckedChanged
        CbxEpisodeActivite.Text = CbxEpisodeActivite.Items(0)
        CbxEpisodeActivite.Show()
    End Sub

    Private Sub RadioBtnVirtuel_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnVirtuel.CheckedChanged
        CbxEpisodeActivite.Text = ""
        CbxEpisodeActivite.Hide()
    End Sub
End Class
