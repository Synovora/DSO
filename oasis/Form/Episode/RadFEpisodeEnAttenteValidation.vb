Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Public Class RadFEpisodeEnAttenteValidation
    Dim episodeDao As New EpisodeDao

    Private Sub RadFEpisodeEnAttenteValidation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Episodes avec document(s) en attente de validation médicale", userLog)
        ChargementEpisode()
    End Sub

    Private Sub ChargementEpisode()
        Cursor.Current = Cursors.WaitCursor
        RadGridViewEpisode.Rows.Clear()

        Dim episodeDataTable As DataTable
        episodeDataTable = episodeDao.GetAllEpisodeEnAttenteValidation()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = episodeDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewEpisode.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadGridViewEpisode.Rows(iGrid).Cells("episode_id").Value = episodeDataTable.Rows(i)("episode_id")
            RadGridViewEpisode.Rows(iGrid).Cells("patient_id").Value = episodeDataTable.Rows(i)("patient_id")
            RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = Coalesce(episodeDataTable.Rows(i)("type_activite"), "")
            If RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = "" Then
                RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = Coalesce(episodeDataTable.Rows(i)("type"), "")
            End If

            Dim dateCreation As Date = Coalesce(episodeDataTable.Rows(i)("date_creation"), Nothing)
            If dateCreation <> Nothing Then
                RadGridViewEpisode.Rows(iGrid).Cells("dateCreation").Value = dateCreation.ToString("dd/MM/yyyy")
                RadGridViewEpisode.Rows(iGrid).Cells("heureCreation").Value = dateCreation.ToString("HH:mm")
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("dateCreation").Value = ""
                RadGridViewEpisode.Rows(iGrid).Cells("heureCreation").Value = ""
            End If

            RadGridViewEpisode.Rows(iGrid).Cells("site").Value = Coalesce(episodeDataTable.Rows(i)("oa_site_description"), "")
            Dim patientDateNaissance As Date = Coalesce(episodeDataTable.Rows(i)("oa_patient_date_naissance"), Nothing)
            RadGridViewEpisode.Rows(iGrid).Cells("dateNaissance").Value = Coalesce(patientDateNaissance.ToString("dd.MM.yyyy"), Nothing)
            Dim patientNom As String = Coalesce(episodeDataTable.Rows(i)("oa_patient_nom"), "")
            Dim patientPrenom As String = Coalesce(episodeDataTable.Rows(i)("oa_patient_prenom"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("patient").Value = patientPrenom & " " & patientNom

            Dim ordonnanceId As Long = Coalesce(episodeDataTable.Rows(i)("oa_ordonnance_id"), 0)
            If ordonnanceId <> 0 Then
                RadGridViewEpisode.Rows(iGrid).Cells("ordonnance").Value = True
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("ordonnance").Value = False
            End If
            RadGridViewEpisode.Rows(iGrid).Cells("nombreSousEpisode").Value = Coalesce(episodeDataTable.Rows(i)("TotalSSP"), 0)
            RadGridViewEpisode.Rows(iGrid).Cells("nombreSousEpisodeReponse").Value = Coalesce(episodeDataTable.Rows(i)("TotalSER"), 0)


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
                Dim patient As Patient
                'patientDao.SetPatient(patient, patientId)
                patient = patientDao.GetPatient(patientId)
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
            element.Checkmark.CheckElement.ForeColor = Color.Red
        End If
    End Sub

    Private Sub RadBtnRefresh_Click(sender As Object, e As EventArgs) Handles RadBtnRefresh.Click
        ChargementEpisode()
    End Sub
End Class
