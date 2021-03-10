Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFWkfDemandeAvisHisto
    Private _selectedEpisodeId As Long
    Private _selectedPatient As Patient

    Public Property SelectedEpisodeId As Long
        Get
            Return _selectedEpisodeId
        End Get
        Set(value As Long)
            _selectedEpisodeId = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return _selectedPatient
        End Get
        Set(value As Patient)
            _selectedPatient = value
        End Set
    End Property

    Dim tacheDao As New TacheDao
    Dim tacheDT As DataTable

    Private Sub RadFWkfDemandeAvisHisto_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Historique des workflows de demande d'avis de l'épisode", userLog)

        ChargementEtatCivil()
        ChargementHisto()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadFWkfDemandeAvisHisto_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadHistoDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing Then
            e.ToolTipText = cell.Value.ToString()
        End If
    End Sub

    Private Sub ChargementHisto()
        Dim histoWorkflow As DataTable
        Dim tacheDao As New TacheDao
        histoWorkflow = tacheDao.GetWorkflowHistoByEpisode(SelectedEpisodeId)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = histoWorkflow.Rows.Count - 1
        Dim iGrid As Integer = -1
        Dim clotureWorkflow As Boolean
        Dim naturePrecedente As String = ""

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            iGrid += 1
            RadHistoDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadHistoDataGridView.Rows(iGrid).Cells("emetteur").Value = histoWorkflow.Rows(i)("user_emetteur_prenom") & " " &
                                histoWorkflow.Rows(i)("user_emetteur_nom") & " / " & histoWorkflow.Rows(i)("user_emetteur_profil")

            If Coalesce(histoWorkflow.Rows(i)("traite_user_id"), 0) = 0 Then
                RadHistoDataGridView.Rows(iGrid).Cells("traitePar").Value = "En attente de traitement"
                RadHistoDataGridView.Rows(iGrid).Cells("traitePar").Style.ForeColor = Color.Red
            Else
                RadHistoDataGridView.Rows(iGrid).Cells("traitePar").Value = histoWorkflow.Rows(i)("user_traite_prenom") & " " &
                                histoWorkflow.Rows(i)("user_traite_nom") & " / " & histoWorkflow.Rows(i)("user_traite_profil")
            End If

            RadHistoDataGridView.Rows(iGrid).Cells("destinataire").Value = histoWorkflow.Rows(i)("user_destinataire_fonction")

            RadHistoDataGridView.Rows(iGrid).Cells("commentaire").Value = histoWorkflow.Rows(i)("emetteur_commentaire")
            'RadHistoDataGridView.Rows(iGrid).Cells("commentaire")

            RadHistoDataGridView.Rows(iGrid).Cells("dateCreation").Value = histoWorkflow.Rows(i)("horodate_creation").ToString()
            RadHistoDataGridView.Rows(iGrid).Cells("dateTraitement").Value = histoWorkflow.Rows(i)("horodate_cloture").ToString()

            Select Case histoWorkflow.Rows(i)("nature")
                Case Tache.NatureTache.DEMANDE.ToString
                    Select Case naturePrecedente
                        Case Tache.NatureTache.COMPLEMENT.ToString
                            RadHistoDataGridView.Rows(iGrid).Cells("nature").Value = "Précision rendue"
                        Case Tache.NatureTache.REPONSE.ToString
                            RadHistoDataGridView.Rows(iGrid).Cells("nature").Value = "Relande de la demande d'avis"
                        Case Else
                            RadHistoDataGridView.Rows(iGrid).Cells("nature").Value = "Demande d'avis (début de workflow)"
                            RadHistoDataGridView.Rows(iGrid).Cells("nature").Style.ForeColor = Color.Blue
                    End Select
                Case Tache.NatureTache.COMPLEMENT.ToString
                    RadHistoDataGridView.Rows(iGrid).Cells("nature").Value = "Demande de précision"
                Case Tache.NatureTache.REPONSE.ToString
                    RadHistoDataGridView.Rows(iGrid).Cells("nature").Value = "Demande d'avis rendue"
            End Select

            naturePrecedente = histoWorkflow.Rows(i)("nature")

            clotureWorkflow = Coalesce(histoWorkflow.Rows(i)("cloture"), False)
            If clotureWorkflow = True Then
                iGrid += 1
                RadHistoDataGridView.Rows.Add(iGrid)
                RadHistoDataGridView.Rows(iGrid).Cells("emetteur").Value = histoWorkflow.Rows(i)("user_traite_prenom") & " " &
                                histoWorkflow.Rows(i)("user_traite_nom") & " / " & histoWorkflow.Rows(i)("user_traite_profil")
                RadHistoDataGridView.Rows(iGrid).Cells("traitePar").Value = ""
                RadHistoDataGridView.Rows(iGrid).Cells("destinataire").Value = ""
                RadHistoDataGridView.Rows(iGrid).Cells("commentaire").Value = ""
                RadHistoDataGridView.Rows(iGrid).Cells("dateCreation").Value = ""
                RadHistoDataGridView.Rows(iGrid).Cells("dateTraitement").Value = ""
                RadHistoDataGridView.Rows(iGrid).Cells("nature").Value = "Avis validé (workflow terminé)"
                RadHistoDataGridView.Rows(iGrid).Cells("nature").Style.ForeColor = Color.Red
                naturePrecedente = ""
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadHistoDataGridView.Rows.Count > 0 Then
            Me.RadHistoDataGridView.CurrentRow = RadHistoDataGridView.Rows(0)
        End If
    End Sub
    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)

        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
