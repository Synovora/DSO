Imports Oasis_Common

Public Class RadFEpisodeEnCoursListe

    Dim episodeDao As New EpisodeDao

    Dim patient As New Patient

    Private Sub RadFEpisodeEnCoursListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEpisode()
    End Sub

    Private Sub ChargementEpisode()
        RadGridViewEpisode.Rows.Clear()

        Dim episodeDataTable As DataTable
        episodeDataTable = episodeDao.GetAllEpisodeEnCours()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = episodeDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewEpisode.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadGridViewEpisode.Rows(iGrid).Cells("episode_id").Value = episodeDataTable.Rows(i)("episode_id")
            RadGridViewEpisode.Rows(iGrid).Cells("patient_id").Value = episodeDataTable.Rows(i)("patient_id")
            RadGridViewEpisode.Rows(iGrid).Cells("type").Value = Coalesce(episodeDataTable.Rows(i)("type"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = Coalesce(episodeDataTable.Rows(i)("type_activite"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("type_profil").Value = Coalesce(episodeDataTable.Rows(i)("type_profil"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("commentaire").Value = Coalesce(episodeDataTable.Rows(i)("commentaire"), "")

            Dim utilisateurId As Long = Coalesce(episodeDataTable.Rows(i)("user_creation"), 0)
            If utilisateurId <> 0 Then
                RadGridViewEpisode.Rows(iGrid).Cells("utilisateur").Value = Coalesce(episodeDataTable.Rows(i)("oa_utilisateur_prenom"), "") & " " & Coalesce(episodeDataTable.Rows(i)("oa_utilisateur_nom"), "")
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("utilisateur").Value = ""
            End If

            Dim dateCreation As Date = Coalesce(episodeDataTable.Rows(i)("date_creation"), Nothing)
            If dateCreation <> Nothing Then
                RadGridViewEpisode.Rows(iGrid).Cells("dateCreation").Value = dateCreation.ToString("dd/MM/yyyy")
                RadGridViewEpisode.Rows(iGrid).Cells("heureCreation").Value = dateCreation.ToString("HH:mm")
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("dateCreation").Value = ""
                RadGridViewEpisode.Rows(iGrid).Cells("heureCreation").Value = ""
            End If

            Dim DureeString As String = outils.CalculDureeEnJourEtHeureString(dateCreation, Date.Now)
            RadGridViewEpisode.Rows(iGrid).Cells("duree").Value = DureeString
            Dim Duree As Integer = DateDiff(DateInterval.Hour, dateCreation, Date.Now)
            If Duree > 24 Then
                RadGridViewEpisode.Rows(iGrid).Cells("duree").Style.ForeColor = Color.Red
            End If

            Dim PatientId As Long = Coalesce(episodeDataTable.Rows(i)("patient_id"), 0)
            If PatientId <> 0 Then
                Dim SiteId As Integer = Coalesce(episodeDataTable.Rows(i)("oa_patient_site_id"), 0)
                Dim patientPrenom, patientNom, patientAge As String
                Dim patientDateNaissance As Date = Coalesce(episodeDataTable.Rows(i)("oa_patient_date_naissance"), Nothing)
                patientNom = Coalesce(episodeDataTable.Rows(i)("oa_patient_nom"), "")
                patientPrenom = Coalesce(episodeDataTable.Rows(i)("oa_patient_prenom"), "")
                If patientDateNaissance <> Nothing Then
                    patientAge = outils.CalculAgeEnAnneeEtMoisString(patientDateNaissance)
                Else
                    patientAge = ""
                End If
                RadGridViewEpisode.Rows(iGrid).Cells("patient").Value = patientPrenom & " " & patientNom & " " & patientAge
                RadGridViewEpisode.Rows(iGrid).Cells("site").Value = Environnement.Table_site.GetSiteDescription(SiteId)
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("patient").Value = ""
                RadGridViewEpisode.Rows(iGrid).Cells("site").Value = ""
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewEpisode.Rows.Count > 0 Then
            Me.RadGridViewEpisode.CurrentRow = RadGridViewEpisode.Rows(0)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridViewEpisode.CellDoubleClick
        If RadGridViewEpisode.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)
            If aRow >= 0 Then
                Dim episodeId As Integer = RadGridViewEpisode.Rows(aRow).Cells("episode_id").Value
                Dim patientId As Integer = RadGridViewEpisode.Rows(aRow).Cells("patient_id").Value
                PatientDao.SetPatient(patient, patientId)
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vRadFEpisodeDetail As New RadFEpisodeDetail
                    vRadFEpisodeDetail.SelectedEpisodeId = episodeId
                    vRadFEpisodeDetail.SelectedPatient = patient
                    vRadFEpisodeDetail.RendezVousId = 0
                    vRadFEpisodeDetail.UtilisateurConnecte = userLog
                    vRadFEpisodeDetail.ShowDialog()
                End Using
                Me.Enabled = True
                ChargementEpisode()
            End If
        End If
    End Sub
End Class
