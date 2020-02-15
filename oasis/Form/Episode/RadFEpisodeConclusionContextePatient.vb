Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFEpisodeConclusionContextePatient
    Private _selectedEpisode As Episode
    Private _codeRetour As Boolean

    Public Property SelectedEpisode As Episode
        Get
            Return _selectedEpisode
        End Get
        Set(value As Episode)
            _selectedEpisode = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _codeRetour
        End Get
        Set(value As Boolean)
            _codeRetour = value
        End Set
    End Property

    Dim InitContextePublie As Boolean = False
    Dim SelectedContexteId As Long = 0

    Dim SelectedPatient As Patient

    Private Sub RadFEpisodeSelecteurContextePatient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        RadChkContextePublie.Checked = True
        ChargementConclusion()
        ChargementContexte()
    End Sub

    Dim episodeContexteDao As New EpisodeContexteDao
    Dim episodeDao As New EpisodeDao

    Dim ListConclusion As List(Of Long) = New List(Of Long)

    Private Sub ChargementEtatCivil()
        SelectedPatient = PatientDao.getPatientById(SelectedEpisode.PatientId)
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
    End Sub

    Private Sub ChargementContexte()
        RadContexteDataGridView.Rows.Clear()
        SelectedContexteId = 0

        Dim contexteDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao
        If RadChkContextePublie.Checked = True Then
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedEpisode.PatientId, True)
        Else
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedEpisode.PatientId, False)
        End If

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim diagnostic As String
        Dim rowCount As Integer = contexteDataTable.Rows.Count - 1
        Dim contexteCache As Boolean

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Contexte caché
            contexteCache = False
            If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    contexteCache = True
                End If
            End If

            If ListConclusion.Contains(contexteDataTable.Rows(i)("oa_antecedent_id")) = True Then
                Continue For
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadContexteDataGridView.Rows.Add(iGrid)
            diagnostic = ""
            If contexteDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            'Affichage contexte ==========================
            Dim longueurString As Integer
            Dim longueurMax As Integer = 150
            Dim contexteDescription As String
            contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                longueurString = contexteDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                contexteDescription.Substring(0, longueurString)
            End If

            RadContexteDataGridView.Rows(iGrid).Cells("contexte").Value = diagnostic & " " & contexteDescription
            '============================================

            If contexteCache = True Then
                RadContexteDataGridView.Rows(iGrid).Cells("contexte").Style.ForeColor = Color.CornflowerBlue
            End If

            'Identifiant contexte
            RadContexteDataGridView.Rows(iGrid).Cells("contexteId").Value = contexteDataTable.Rows(i)("oa_antecedent_id")
        Next

        'Positionnement du grid sur la première occurrence
        If RadContexteDataGridView.Rows.Count > 0 Then
            Me.RadContexteDataGridView.CurrentRow = RadContexteDataGridView.ChildRows(0)
        End If
    End Sub


    Private Sub RadChkContextePublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkContextePublie.ToggleStateChanged
        If RadChkContextePublie.Checked = True Then
            RadChkContexteTous.Checked = False
            If InitContextePublie = True Then
                Application.DoEvents()
                ChargementContexte()
            Else
                InitContextePublie = True
            End If
        Else
            If RadChkContexteTous.Checked = False Then
                RadChkContextePublie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkContexteTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkContexteTous.ToggleStateChanged
        If RadChkContexteTous.Checked = True Then
            RadChkContextePublie.Checked = False
            Application.DoEvents()
            ChargementContexte()
        Else
            If RadChkContextePublie.Checked = False Then
                RadChkContexteTous.Checked = True
            End If
        End If
    End Sub

    Private Sub ChargementConclusion()
        Dim diagnostic As String

        Dim dt As DataTable
        dt = episodeContexteDao.GetAllEpisodeContexteByEpisodeId(SelectedEpisode.Id)

        RadConclusionGridView.Rows.Clear()
        ListConclusion.Clear()

        If SelectedEpisode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString Then
            If dt.Rows.Count <= 1 Then
                RadBtnSuppprimer.Enabled = False
            Else
                RadBtnSuppprimer.Enabled = True
            End If
        End If

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadConclusionGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            diagnostic = ""
            If dt.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(dt.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(dt.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            Dim longueurString As Integer
            Dim longueurMax As Integer = 150
            Dim contexteDescription As String
            contexteDescription = Coalesce(dt.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                longueurString = contexteDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                contexteDescription.Substring(0, longueurString)
            End If

            RadConclusionGridView.Rows(iGrid).Cells("contexte_id").Value = Coalesce(dt.Rows(i)("contexte_id"), 0)
            RadConclusionGridView.Rows(iGrid).Cells("episode_contexte_id").Value = dt.Rows(i)("episode_contexte_id")
            RadConclusionGridView.Rows(iGrid).Cells("contexte").Value = diagnostic & " " & contexteDescription
            If Coalesce(dt.Rows(i)("contexte_id"), 0) <> 0 Then
                If ListConclusion.Contains(Coalesce(dt.Rows(i)("contexte_id"), 0)) = False Then
                    ListConclusion.Add(Coalesce(dt.Rows(i)("contexte_id"), 0))
                End If
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadConclusionGridView.Rows.Count > 0 Then
            RadConclusionGridView.CurrentRow = RadConclusionGridView.ChildRows(0)
            RadConclusionGridView.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub RadContexteDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadContexteDataGridView.CellDoubleClick
        ModificationContexte()
    End Sub


    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadConclusionGridView.CellDoubleClick
        ModificationContexte()
    End Sub

    Private Sub ModificationContexte()
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False
                Using vFContexteDetailEdit As New RadFContextedetailEdit
                    vFContexteDetailEdit.SelectedContexteId = ContexteId
                    vFContexteDetailEdit.SelectedPatient = SelectedPatient
                    vFContexteDetailEdit.UtilisateurConnecte = userLog
                    vFContexteDetailEdit.SelectedDrcId = 0
                    vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                    vFContexteDetailEdit.ShowDialog() 'Modal
                    If vFContexteDetailEdit.CodeRetour = True Then
                        CodeRetour = True
                        Select Case vFContexteDetailEdit.CodeResultat
                            Case EnumResultat.AnnulationOK
                                Dim form As New RadFNotification()
                                form.Titre = "Notification contexte patient"
                                form.Message = "Contexte patient annulé"
                                form.Show()
                            Case EnumResultat.ModificationOK
                                Dim form As New RadFNotification()
                                form.Titre = "Notification contexte patient"
                                form.Message = "Contexte patient modifié"
                                form.Show()
                        End Select
                        ChargementConclusion()
                        ChargementContexte()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Créer un contexte
    Private Sub RadBtnCreation_MouseCaptureChanged(sender As Object, e As EventArgs) Handles RadBtnCreation.MouseCaptureChanged
        CreationContexte()
    End Sub

    Private Sub CréerUnContexteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnContexteToolStripMenuItem.Click
        CreationContexte()
    End Sub

    Private Sub CreationContexte()
        Dim SelectedDrcId As Integer
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = DrcDao.EnumCategorieOasisCode.Contexte
            vFDrcSelecteur.ShowDialog()
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si un médicament a été sélectionné, on appelle le Formulaire de création
            If SelectedDrcId <> 0 Then
                Using vFContexteDetailEdit As New RadFContextedetailEdit
                    vFContexteDetailEdit.SelectedPatient = SelectedPatient
                    vFContexteDetailEdit.UtilisateurConnecte = userLog
                    vFContexteDetailEdit.SelectedDrcId = SelectedDrcId
                    vFContexteDetailEdit.SelectedContexteId = 0
                    vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                    vFContexteDetailEdit.ShowDialog()
                    'Si le traitement a été créé, on recharge la grid
                    If vFContexteDetailEdit.CodeRetour = True Then
                        CodeRetour = True
                        Dim form As New RadFNotification()
                        form.Titre = "Notification contexte patient"
                        form.Message = "Contexte patient créé"
                        form.Show()
                        ChargementConclusion()
                        ChargementContexte()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnSelect_Click(sender As Object, e As EventArgs) Handles RadBtnSelect.Click
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Dim episodeContexte As New EpisodeContexte
                episodeContexte.ContexteId = ContexteId
                episodeContexte.EpisodeId = SelectedEpisode.Id
                episodeContexte.PatientId = SelectedEpisode.PatientId
                episodeContexte.UserCreation = userLog.UtilisateurId
                episodeContexte.DateCreation = Date.Now()
                episodeContexteDao.CreateEpisodeContexte(episodeContexte)
                'episodeDao.MajEpisodeConclusionMedicale(SelectedEpisode.Id)
                ChargementConclusion()
                ChargementContexte()
                CodeRetour = True
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadBtnSuppprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSuppprimer.Click
        If RadConclusionGridView.CurrentRow IsNot Nothing Then
            Dim aRow, episodeContexteId As Integer
            aRow = Me.RadConclusionGridView.Rows.IndexOf(Me.RadConclusionGridView.CurrentRow)
            If aRow >= 0 Then
                Cursor.Current = Cursors.WaitCursor
                episodeContexteId = RadConclusionGridView.Rows(aRow).Cells("episode_contexte_id").Value
                EpisodeContexteDao.SuppressionEpisodeContexteById(episodeContexteId)
                'episodeDao.MajEpisodeConclusionMedicale(SelectedEpisode.Id)
                ChargementConclusion()
                ChargementContexte()
                CodeRetour = True
            End If
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadFEpisodeConclusionContextePatient_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        episodeDao.MajEpisodeConclusionMedicale(SelectedEpisode.Id)
    End Sub
End Class
