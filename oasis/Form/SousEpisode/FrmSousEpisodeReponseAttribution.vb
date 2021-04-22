Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class FrmSousEpisodeReponseAttribution

    Dim InitForm As Boolean
    ReadOnly patientDao As New PatientDao

    Private Sub FrmSousEpisodeReponseAttribution_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Liste des mails a traiter", userLog)
        InitForm = True
        refreshGrid()
        ChargementPatient()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click

    End Sub

    Private Sub RadLabel1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RadLabel2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RadLabel3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub refreshGrid()
        Me.Cursor = Cursors.WaitCursor
        Dim sousEpisodeReponseMailDao As SousEpisodeReponseMailDao = New SousEpisodeReponseMailDao
        Try
            Dim lstMail As List(Of SousEpisodeReponseMail) = sousEpisodeReponseMailDao.GetLstSousEpisodeReponseMail()
            Dim numRowGrid As Integer = 0
            ' -- recup eventuelle precedente selectionnée
            'If RadSousEpisodeGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadSousEpisodeGrid.CurrentRow) Then
            '    exId = Me.RadSousEpisodeGrid.CurrentRow.Cells("IdSousEpisode").Value
            '    exPosit = Me.RadSousEpisodeGrid.CurrentRow.Index
            'End If
            RadMailGridView.Rows.Clear()
            For Each mail In lstMail
                Dim newRow As GridViewRowInfo = RadMailGridView.Rows.NewRow()
                With newRow
                    .Cells("objet").Value = mail.Objet
                    .Cells("auteur").Value = mail.Auteur
                    .Cells("date").Value = mail.HorodateCreation
                End With
                RadMailGridView.Rows.Add(newRow)
                numRowGrid += 1
            Next
            Me.Cursor = Cursors.Default
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ChargementPatient()
        Dim patientDataTable As DataTable

        Dim Tous As Boolean
        Dim PatientOasis As Boolean

        RadGridView3.Rows.Clear()

        patientDataTable = PatientDao.GetAllPatient(Tous, PatientOasis)

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = patientDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridView3.Rows.Add(iGrid)
            'Alimentation du DataGridView
            'RadPatientGridView.Rows(iGrid).Cells("oa_patient_id").Value = patientDataTable.Rows(i)("oa_patient_id")
            'Dim NIR As Long = Coalesce(patientDataTable.Rows(i)("oa_patient_nir"), 0)
            'If NIR <> 0 Then
            '    RadPatientGridView.Rows(iGrid).Cells("oa_patient_nir").Value = NIR
            'Else
            '    RadPatientGridView.Rows(iGrid).Cells("oa_patient_nir").Value = ""
            'End If

            RadGridView3.Rows(iGrid).Cells("prenom").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_prenom"), "")
            RadGridView3.Rows(iGrid).Cells("nom").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_nom"), "")

            '    Dim DateNaissance As Date = Coalesce(patientDataTable.Rows(i)("oa_patient_date_naissance"), Nothing)
            '    If DateNaissance <> Nothing Then
            '        RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_naissance").Value = DateNaissance.ToString("dd/MM/yyyy")
            '        RadPatientGridView.Rows(iGrid).Cells("age").Value = CalculAgeEnAnneeEtMoisString(DateNaissance)
            '    Else
            '        RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_naissance").Value = ""
            '        RadPatientGridView.Rows(iGrid).Cells("age").Value = ""
            '    End If
            '    RadPatientGridView.Rows(iGrid).Cells("oa_patient_lieu_naissance").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_lieu_naissance"), "")

            '    Dim DateEntreeOasis As Date = Coalesce(patientDataTable.Rows(i)("oa_patient_date_entree_oasis"), Nothing)
            '    If DateEntreeOasis <> Nothing Then
            '        RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_entree_oasis").Value = DateEntreeOasis.ToString("dd/MM/yyyy")
            '    Else
            '        RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_entree_oasis").Value = ""
            '    End If

            '    Dim DateSortieOasis As Date = Coalesce(patientDataTable.Rows(i)("oa_patient_date_sortie_oasis"), Nothing)
            '    If DateSortieOasis <> Nothing Then
            '        RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_sortie_oasis").Value = DateSortieOasis.ToString("dd/MM/yyyy")
            '    Else
            '        RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_sortie_oasis").Value = ""
            '    End If

            '    Dim SiteId As Long = Coalesce(patientDataTable.Rows(i)("oa_patient_site_id"), 0)
            '    If SiteId <> 0 Then
            '        RadPatientGridView.Rows(iGrid).Cells("site").Value = Environnement.Table_site.GetSiteDescription(SiteId)
            '    Else
            '        RadPatientGridView.Rows(iGrid).Cells("site").Value = ""
            '    End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridView3.Rows.Count > 0 Then
                Me.RadGridView3.CurrentRow = RadGridView3.Rows(0)
            End If
    End Sub

    Private Sub RadButton4_Click(sender As Object, e As EventArgs) Handles RadButton4.Click

    End Sub

    Private Sub RadMailGridView_Click(sender As Object, e As EventArgs) Handles RadMailGridView.Click

    End Sub

    Private Sub MasterTemplate_Click(sender As Object, e As EventArgs) Handles RadGridView3.Click

    End Sub
End Class
