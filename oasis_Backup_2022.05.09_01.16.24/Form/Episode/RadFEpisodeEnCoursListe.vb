﻿Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class RadFEpisodeEnCoursListe

    Dim episodeDao As New EpisodeDao

    Dim patient As New Patient

    Dim InitForm As Boolean
    Dim Filtre As EnumFiltre

    Private Enum EnumFiltre
        TOUS
        WORKFLOW_EN_ATTENTE
        WORKFLOW_EN_ATTENTE_FONCTION
    End Enum

    Private Sub RadFEpisodeEnCoursListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Liste des épisodes patient en cours", userLog)

        InitForm = True
        RadioBtnTous.Checked = True
        Filtre = EnumFiltre.TOUS

        ChargementEpisode()
    End Sub

    Private Sub ChargementEpisode()
        Cursor.Current = Cursors.WaitCursor
        RadGridViewEpisode.Rows.Clear()
        Dim FonctionDestinataireType As String

        Dim episodeDataTable As DataTable
        episodeDataTable = episodeDao.GetAllEpisodeEnCours()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = episodeDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            Select Case Filtre
                Case enumFiltre.WORKFLOW_EN_ATTENTE
                    If Coalesce(episodeDataTable.Rows(i)("nature"), "") = "" Then
                        Continue For
                    End If
                Case enumFiltre.WORKFLOW_EN_ATTENTE_FONCTION
                    If Coalesce(episodeDataTable.Rows(i)("nature"), "") = "" Then
                        Continue For
                    End If
                    FonctionDestinataireType = Coalesce(episodeDataTable.Rows(i)("oa_r_fonction_type"), "")
                    Select Case FonctionDestinataireType
                        Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                            If userLog.TypeProfil <> ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
                                Continue For
                            End If
                        Case ProfilDao.EnumProfilType.MEDICAL.ToString
                            If userLog.TypeProfil <> ProfilDao.EnumProfilType.MEDICAL.ToString Then
                                Continue For
                            End If
                    End Select
            End Select

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
            'Activité pour un épisode virtuel
            If RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = "" Then
                RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = Coalesce(episodeDataTable.Rows(i)("type"), "")
            End If

            'Workflow --------------
            If Coalesce(episodeDataTable.Rows(i)("nature"), "") <> "" Then
                RadGridViewEpisode.Rows(iGrid).Cells("workflow").Value = True
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("workflow").Value = False
            End If

            Dim FonctionDestinataire As Long = Coalesce(episodeDataTable.Rows(i)("destinataire_fonction_id"), 0)
            RadGridViewEpisode.Rows(iGrid).Cells("workflowFonctionDestinataire").Value = Coalesce(episodeDataTable.Rows(i)("oa_r_fonction_designation"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = Coalesce(episodeDataTable.Rows(i)("etat"), "")
            If RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = Tache.EtatTache.EN_COURS.ToString() Then
                RadGridViewEpisode.Rows(iGrid).Cells("workflowAttribution").Value = Coalesce(episodeDataTable.Rows(i)("oa_utilisateur_prenom"), "") &
                    " " & Coalesce(episodeDataTable.Rows(i)("oa_utilisateur_nom"), "")
            Else
                If RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = Tache.EtatTache.EN_ATTENTE.ToString() Then
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowAttribution").Value = "Workflow non attribué"
                End If
            End If

            Dim TacheNature As String = Coalesce(episodeDataTable.Rows(i)("nature"), "")
            FonctionDestinataireType = Coalesce(episodeDataTable.Rows(i)("oa_r_fonction_type"), "")

            Select Case FonctionDestinataireType
                Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                    Select Case TacheNature
                        Case Tache.NatureTache.DEMANDE.ToString
                            RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = "Réponse à rendre"
                            'RadBtnWorkflowMed.Text = "Avis demandé"
                        Case Tache.NatureTache.REPONSE.ToString
                            RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = "Avis à valider"
                            'RadBtnWorkflowMed.Text = "Avis rendu"
                        Case Tache.NatureTache.COMPLEMENT.ToString
                            RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = "Précision à rendre"
                            'RadBtnWorkflowMed.Text = "Demande précision"
                    End Select
                Case ProfilDao.EnumProfilType.MEDICAL.ToString
                    Select Case TacheNature
                        Case Tache.NatureTache.DEMANDE.ToString
                            RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = "Réponse à rendre"
                            'RadBtnWorkflowIde.Text = "Avis demandé"
                        Case Tache.NatureTache.REPONSE.ToString
                            RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = "Avis à valider"
                            'RadBtnWorkflowIde.Text = "Avis rendu"
                        Case Tache.NatureTache.COMPLEMENT.ToString
                            RadGridViewEpisode.Rows(iGrid).Cells("workflowEtat").Value = "Précision à rendre"
                            'RadBtnWorkflowIde.Text = "Demande de précision"
                    End Select
            End Select

            RadGridViewEpisode.Rows(iGrid).Cells("workflowCommentaire").Value = Coalesce(episodeDataTable.Rows(i)("emetteur_commentaire"), "")
            Dim PrioriteValeur As Integer = Coalesce(episodeDataTable.Rows(i)("priorite"), 0)
            Select Case PrioriteValeur
                Case 100
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowPriorite").Value = "-- Urgent --"
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowPriorite").Style.ForeColor = Color.Red
                Case 200
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowPriorite").Value = "- Synchrone -"
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowPriorite").Style.ForeColor = Color.DarkBlue
                Case 300
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowPriorite").Value = "Asynchrone"
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowPriorite").Style.ForeColor = Color.Black
                Case Else
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowPriorite").Value = ""
                    RadGridViewEpisode.Rows(iGrid).Cells("workflowPriorite").Style.ForeColor = Color.Black
            End Select

            'Workflow --------------

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
                    patientAge = CalculAgeEnAnneeEtMoisString(patientDateNaissance)
                Else
                    patientAge = ""
                End If
                RadGridViewEpisode.Rows(iGrid).Cells("patient").Value = patientPrenom & " " & patientNom
                RadGridViewEpisode.Rows(iGrid).Cells("dateNaissance").Value = Coalesce(patientDateNaissance.ToString("dd.MM.yyyy"), Nothing)
                RadGridViewEpisode.Rows(iGrid).Cells("site").Value = Environnement.Table_site.GetSiteDescription(SiteId)
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("patient").Value = ""
                RadGridViewEpisode.Rows(iGrid).Cells("dateNaissance").Value = ""
                RadGridViewEpisode.Rows(iGrid).Cells("site").Value = ""
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewEpisode.Rows.Count > 0 Then
            Me.RadGridViewEpisode.CurrentRow = RadGridViewEpisode.Rows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridViewEpisode.CellDoubleClick
        Dim patientDao As New PatientDao
        If RadGridViewEpisode.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)
            If aRow >= 0 Then
                Dim episodeId As Integer = RadGridViewEpisode.Rows(aRow).Cells("episode_id").Value
                Dim patientId As Integer = RadGridViewEpisode.Rows(aRow).Cells("patient_id").Value
                'patientDao.SetPatient(patient, patientId)
                patient = patientDao.GetPatient(patientId)
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor

                Try
                    Using vRadFEpisodeDetail As New RadFEpisodeDetail
                        vRadFEpisodeDetail.SelectedEpisodeId = episodeId
                        vRadFEpisodeDetail.SelectedPatient = patient
                        vRadFEpisodeDetail.RendezVousId = 0
                        vRadFEpisodeDetail.UtilisateurConnecte = userLog
                        vRadFEpisodeDetail.ShowDialog()
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

                Me.Enabled = True
                ChargementEpisode()
            End If
        End If
    End Sub

    Private Sub MasterTemplate_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadGridViewEpisode.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            Try
                e.CellElement.ToolTipText = e.CellElement.Text
            Catch ex As Exception

            End Try
        End If
        ' --- on enleve le carre des checkbox
        Dim checkBoxCell As GridCheckBoxCellElement = TryCast(e.CellElement, GridCheckBoxCellElement)
        If checkBoxCell IsNot Nothing Then
            Dim editor As RadCheckBoxEditor = TryCast(checkBoxCell.Editor, RadCheckBoxEditor)
            Dim element As RadCheckBoxEditorElement = TryCast(editor.EditorElement, RadCheckBoxEditorElement)
            element.Checkmark.Border.Visibility = ElementVisibility.Collapsed
            element.Checkmark.Fill.Visibility = ElementVisibility.Collapsed
        End If
    End Sub

    Private Sub MasterTemplate_Click(sender As Object, e As EventArgs) Handles RadGridViewEpisode.Click

    End Sub

    Private Sub RadBtnRefresh_Click(sender As Object, e As EventArgs) Handles RadBtnRefresh.Click
        ChargementEpisode()
    End Sub

    Private Sub RadioBtnTous_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnTous.CheckedChanged
        If InitForm = True Then
            InitForm = False
        Else
            Filtre = enumFiltre.TOUS
            ChargementEpisode()
        End If
    End Sub

    Private Sub RadioBtnWorkflowEnAttente_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnWorkflowEnAttente.CheckedChanged
        Filtre = enumFiltre.WORKFLOW_EN_ATTENTE
        ChargementEpisode()
    End Sub

    Private Sub RadioBtnWorkflowPourSaFonction_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnWorkflowPourSaFonction.CheckedChanged
        Filtre = enumFiltre.WORKFLOW_EN_ATTENTE_FONCTION
        ChargementEpisode()
    End Sub
End Class