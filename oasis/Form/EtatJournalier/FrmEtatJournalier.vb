Imports Oasis_Common
Imports Telerik.Data
Imports Telerik.WinControls.Data
Imports Telerik.WinControls.UI.Export
Imports Telerik.WinControls.UI
Imports Telerik.WinControls
Imports System.IO

Public Class FrmEtatJournalier
    ReadOnly episodeDao As New EpisodeDao

    Private Sub FrmEpisode_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Liste Etats Journaliers", userLog)

        Dim parametreOasisDao As ParametreOasisDao = New ParametreOasisDao
        parametreOasisDao.TraitementContexte()

        ChargementPatient()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ChargementPatient()
        Dim patientDataTable As DataTable

        RadGridView.Rows.Clear()

        patientDataTable = episodeDao.GetAllEpisodeClosedByDate(RadDateTimePicker.Value)

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = patientDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            Debug.WriteLine(patientDataTable.Rows(i).ToString())
            RadGridView.Rows.Add(iGrid)
            RadGridView.Rows(iGrid).Cells("nom").Value = patientDataTable.Rows(i)("oa_patient_nom")
            RadGridView.Rows(iGrid).Cells("prenom").Value = patientDataTable.Rows(i)("oa_patient_prenom")
            RadGridView.Rows(iGrid).Cells("dn").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_INS"), "")
            RadGridView.Rows(iGrid).Cells("nir").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_nir"), "")
            RadGridView.Rows(iGrid).Cells("type").Value = Coalesce(patientDataTable.Rows(i)("type"), "")
            RadGridView.Rows(iGrid).Cells("site").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_site_id"), "")
            RadGridView.Rows(iGrid).Cells("episodeId").Value = Coalesce(patientDataTable.Rows(i)("episode_id"), "")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridView.Rows.Count > 0 Then
            Me.RadGridView.CurrentRow = RadGridView.Rows(0)
        End If
    End Sub

    Private Sub RadDateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles RadDateTimePicker.ValueChanged
        ChargementPatient()
    End Sub

    Private Sub RadButton_Click(sender As Object, e As EventArgs) Handles RadButton.Click
        'Dim exporter As ExportToCSV = New ExportToCSV(Me.RadGridView)
        'Dim fileName As String = "C:\\ExportedData.csv"
        'exporter.RunExport(fileName)

    End Sub
End Class
