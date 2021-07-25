Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFEpisodeConclusionContextePatient
    Private _selectedEpisode As Episode
    Private _codeRetour As Boolean
    Dim InitPublie As Boolean

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

    Dim SelectedPatient As Patient
    Dim chaineEpisodeDao As New ChaineEpisodeDao

    Private Sub RadFEpisodeSelecteurContextePatient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadChkPublie.Checked = True
        ChargementEtatCivil()
        RadChkContextePublie.Checked = True
        ChargementConclusion()
        ChargementContexte()
        RefreshChaineEpisode()
    End Sub

    ReadOnly episodeContexteDao As New EpisodeContexteDao
    ReadOnly episodeDao As New EpisodeDao

    ReadOnly ListConclusion As List(Of Long) = New List(Of Long)

    Private Sub ChargementEtatCivil()
        Dim patientDao As New PatientDao
        SelectedPatient = patientDao.GetPatient(SelectedEpisode.PatientId)
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

        If SelectedEpisode.Etat = Episode.EnumEtatEpisode.CLOTURE.ToString Then
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

                Try
                    Using vFContexteDetailEdit As New RadFContextedetailEdit
                        vFContexteDetailEdit.SelectedContexteId = ContexteId
                        vFContexteDetailEdit.SelectedPatient = SelectedPatient
                        vFContexteDetailEdit.UtilisateurConnecte = userLog
                        vFContexteDetailEdit.SelectedDrcId = 0
                        vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                        vFContexteDetailEdit.Episode = SelectedEpisode
                        vFContexteDetailEdit.ShowDialog() 'Modal
                        If vFContexteDetailEdit.CodeRetour = True Then
                            CodeRetour = True
                            Select Case vFContexteDetailEdit.CodeResultat
                                Case EnumResultat.AnnulationOK
                                    Dim form As New RadFNotification With {
                                        .Titre = "Notification contexte patient",
                                        .Message = "Contexte patient annulé"
                                    }
                                    form.Show()
                                Case EnumResultat.ModificationOK
                                    Dim form As New RadFNotification With {
                                        .Titre = "Notification contexte patient",
                                        .Message = "Contexte patient modifié"
                                    }
                                    form.Show()
                            End Select
                            ChargementConclusion()
                            ChargementContexte()
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub

    'Créer un contexte
    Private Sub CréerUnContexteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnContexteToolStripMenuItem.Click
        CreationContexte()
    End Sub

    Private Sub RadBtnCreation_Click(sender As Object, e As EventArgs) Handles RadBtnCreation.Click
        CreationContexte()
    End Sub

    Private Sub CreationContexte()
        Dim SelectedDrcId As Integer
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFDrcSelecteur As New RadFDRCSelecteur
                vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
                vFDrcSelecteur.CategorieOasis = Drc.EnumCategorieOasisCode.Contexte
                vFDrcSelecteur.ShowDialog()
                SelectedDrcId = vFDrcSelecteur.SelectedDrcId
                'Si un médicament a été sélectionné, on appelle le Formulaire de création
                If SelectedDrcId <> 0 Then
                    Try
                        Using vFContexteDetailEdit As New RadFContextedetailEdit
                            vFContexteDetailEdit.SelectedPatient = SelectedPatient
                            vFContexteDetailEdit.UtilisateurConnecte = userLog
                            vFContexteDetailEdit.SelectedDrcId = SelectedDrcId
                            vFContexteDetailEdit.SelectedContexteId = 0
                            vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                            vFContexteDetailEdit.Episode = SelectedEpisode
                            vFContexteDetailEdit.ShowDialog()
                            'Si le traitement a été créé, on recharge la grid
                            If vFContexteDetailEdit.CodeRetour = True Then
                                CodeRetour = True
                                Dim form As New RadFNotification With {
                                    .Titre = "Notification contexte patient",
                                    .Message = "Contexte patient créé"
                                }
                                form.Show()
                                ChargementConclusion()
                                ChargementContexte()
                            End If
                        End Using
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnSelect_Click(sender As Object, e As EventArgs) Handles RadBtnSelect.Click
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Dim episodeContexte As New EpisodeContexte With {
                    .ContexteId = ContexteId,
                    .EpisodeId = SelectedEpisode.Id,
                    .PatientId = SelectedEpisode.PatientId,
                    .UserCreation = userLog.UtilisateurId,
                    .DateCreation = Date.Now()
                }
                episodeContexteDao.CreateEpisodeContexte(episodeContexte, userLog)
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
                episodeContexteDao.SuppressionEpisodeContexteById(episodeContexteId)
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

    Private Sub RadGridViewChaineEpisode_Click(sender As Object, e As EventArgs) Handles RadGridViewChaineEpisodeAntecedent.Click
        Dim chainEpisodeId = RadGridViewChaineEpisodeAntecedent.Rows(Me.RadGridViewChaineEpisodeAntecedent.Rows.IndexOf(Me.RadGridViewChaineEpisodeAntecedent.CurrentRow)).Cells("id").Value
        Dim isChecked = RadGridViewChaineEpisodeAntecedent.Rows(Me.RadGridViewChaineEpisodeAntecedent.Rows.IndexOf(Me.RadGridViewChaineEpisodeAntecedent.CurrentRow)).Cells("selected").Value
        Dim relation As New RelationChaineEpisode With {
            .Id = 0,
            .ChaineId = chainEpisodeId,
            .EpisodeId = SelectedEpisode.Id
        }
        If (isChecked) Then
            chaineEpisodeDao.DeleteRelation(relation)
        Else
            chaineEpisodeDao.AddRelation(relation)
        End If
        RefreshChaineEpisode()
    End Sub

    Private Sub RadGridViewChaineEpisodeContexte_Click(sender As Object, e As EventArgs) Handles RadGridViewChaineEpisodeContexte.Click
        Dim chainEpisodeId = RadGridViewChaineEpisodeContexte.Rows(Me.RadGridViewChaineEpisodeContexte.Rows.IndexOf(Me.RadGridViewChaineEpisodeContexte.CurrentRow)).Cells("id").Value
        Dim isChecked = RadGridViewChaineEpisodeContexte.Rows(Me.RadGridViewChaineEpisodeContexte.Rows.IndexOf(Me.RadGridViewChaineEpisodeContexte.CurrentRow)).Cells("selected").Value
        Dim relation As New RelationChaineEpisode With {
            .Id = 0,
            .ChaineId = chainEpisodeId,
            .EpisodeId = SelectedEpisode.Id
        }
        If (isChecked) Then
            chaineEpisodeDao.DeleteRelation(relation)
        Else
            chaineEpisodeDao.AddRelation(relation)
        End If
        RefreshChaineEpisode()
    End Sub

    Private Sub RefreshChaineEpisode()
        Dim relationChaineEpisodes = chaineEpisodeDao.GetRelationListByEpisode(SelectedEpisode)
        Dim filter
        If RadChkPublie.Checked = False Then
            filter = " AND (oasis.oa_antecedent.oa_antecedent_statut_affichage = 'P' OR oasis.oa_antecedent.oa_antecedent_statut_affichage = 'C')"
        Else
            filter = " AND oasis.oa_antecedent.oa_antecedent_statut_affichage = 'P' "
        End If
        filter += " AND (oasis.oa_antecedent.oa_antecedent_inactif = '0' OR oasis.oa_antecedent.oa_antecedent_inactif is Null) ORDER BY oasis.oa_antecedent.oa_antecedent_ordre_affichage1, oasis.oa_antecedent.oa_antecedent_ordre_affichage2, oasis.oa_antecedent.oa_antecedent_ordre_affichage3;"

        Dim chaineEpsiodes = chaineEpisodeDao.GetListByPatient(SelectedPatient.PatientId, 0, 0, " AND oasis.oa_antecedent.oa_antecedent_type = 'A'" & filter)

        RadGridViewChaineEpisodeAntecedent.Rows.Clear()
        For Each chaineEpisode In chaineEpsiodes
            RadGridViewChaineEpisodeAntecedent.Rows.Add(chaineEpsiodes.IndexOf(chaineEpisode))
            RadGridViewChaineEpisodeAntecedent.Rows(chaineEpsiodes.IndexOf(chaineEpisode)).Cells("id").Value = chaineEpisode.Id
            RadGridViewChaineEpisodeAntecedent.Rows(chaineEpsiodes.IndexOf(chaineEpisode)).Cells("name").Value = chaineEpisode.Antecedent.Description
            RadGridViewChaineEpisodeAntecedent.Rows(chaineEpsiodes.IndexOf(chaineEpisode)).Cells("selected").Value = relationChaineEpisodes.Any(Function(myObject) myObject.ChaineId = chaineEpisode.Id)
        Next

        chaineEpsiodes = chaineEpisodeDao.GetListByPatient(SelectedPatient.PatientId, 0, 0, " AND oasis.oa_antecedent.oa_antecedent_type = 'C' AND (oasis.oa_antecedent.oa_antecedent_arret = '0' OR oasis.oa_antecedent.oa_antecedent_arret is Null) " & filter)

        RadGridViewChaineEpisodeContexte.Rows.Clear()
        For Each chaineEpisode In chaineEpsiodes
            RadGridViewChaineEpisodeContexte.Rows.Add(chaineEpsiodes.IndexOf(chaineEpisode))
            RadGridViewChaineEpisodeContexte.Rows(chaineEpsiodes.IndexOf(chaineEpisode)).Cells("id").Value = chaineEpisode.Id
            RadGridViewChaineEpisodeContexte.Rows(chaineEpsiodes.IndexOf(chaineEpisode)).Cells("name").Value = chaineEpisode.Antecedent.Description
            RadGridViewChaineEpisodeContexte.Rows(chaineEpsiodes.IndexOf(chaineEpisode)).Cells("selected").Value = relationChaineEpisodes.Any(Function(myObject) myObject.ChaineId = chaineEpisode.Id)
        Next
    End Sub

    Private Sub RadChkPublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPublie.ToggleStateChanged
        If RadChkPublie.Checked = True Then
            RadChkTous.Checked = False
            If InitPublie = True Then
                Application.DoEvents()
                RefreshChaineEpisode()
            Else
                InitPublie = True
            End If
        Else
            If RadChkTous.Checked = False Then
                RadChkPublie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkTous.ToggleStateChanged
        If RadChkTous.Checked = True Then
            RadChkPublie.Checked = False
            Application.DoEvents()
            RefreshChaineEpisode()
        Else
            If RadChkPublie.Checked = False Then
                RadChkTous.Checked = True
            End If
        End If
    End Sub
End Class
