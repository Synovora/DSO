Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFVaccinInfo
    Property SelectedPatient As Patient
    Property SelectedCGVDate As CGVDate
    Property SelectedValences As List(Of CGVValence)
    Property Vaccins As List(Of VaccinValence)

    ReadOnly vaccinDao As New VaccinDao

    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Information", userLog)
        Dim valenceIds As List(Of Long) = New List(Of Long)
        For Each valences As CGVValence In SelectedValences
            valenceIds.Add(valences.Valence)
        Next
        Vaccins = vaccinDao.getFromValences(valenceIds)
        ChargementEtatCivil()
        ChargementInformation()
        ChargementValences()
        ChargementVaccins()
    End Sub

    Private Sub ChargementInformation()
        LblAgeVaccination.Text = CGVDate.DaysToDate(SelectedCGVDate.Days)
        LblDate.Text = Date.Now
        LblOperator.Text = GetProfilUserString(userLog)
    End Sub

    Private Sub ChargementVaccins()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        GVVaccin.Rows.Clear()
        Dim iGrid As Integer = 0

        For Each vaccin As VaccinValence In Vaccins.GroupBy(Function(x) x.Code).Select(Function(x) x.First).ToList
            GVVaccin.Rows.Add(iGrid)
            GVVaccin.Rows(iGrid).Cells("id").Value = vaccin.Id
            GVVaccin.Rows(iGrid).Cells("checked").Value = False
            GVVaccin.Rows(iGrid).Cells("dci").Value = vaccin.Dci
            GVVaccin.Rows(iGrid).Cells("valence").Value = vaccin.Valence
            iGrid += 1
        Next

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub ColorVaccins()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor


        For Each row As GridViewRowInfo In GVVaccin.Rows
            If row.Cells("checked").Value = False Then
                If GVValence.Rows.Any(Function(x) x.Cells("checked").Value = True AndAlso x.Cells("valence").Value = row.Cells("valence").Value) Then
                    row.Cells("dci").Style.ForeColor = Color.Red
                Else
                    row.Cells("dci").Style.ForeColor = Color.Black
                End If
            Else
                row.Cells("dci").Style.ForeColor = Color.Black
            End If
        Next

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub ChargementValences()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        GVValence.Rows.Clear()
        Dim iGrid As Integer = 0

        For Each valence As CGVValence In SelectedValences
            GVValence.Rows.Add(iGrid)
            GVValence.Rows(iGrid).Cells("id").Value = valence.Id
            GVValence.Rows(iGrid).Cells("valence").Value = valence.Valence
            GVValence.Rows(iGrid).Cells("checked").Value = GVVaccin.Rows.Any(Function(x) x.Cells("checked").Value = True AndAlso x.Cells("valence").Value = valence.Valence.ToString())
            GVValence.Rows(iGrid).Cells("nom").Value = valence.Description
            iGrid += 1
        Next
        ColorVaccins()
        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj, True)

        Dim DateMaxValue = New Date(9998, 12, 31, 0, 0, 0)
        Dim DateMinValue = New Date(1, 1, 1, 0, 0, 0)
        If SelectedPatient.PatientDateEntree = DateMaxValue OrElse SelectedPatient.PatientDateEntree = DateMinValue OrElse SelectedPatient.PatientDateSortie < Date.Now Then
            LblNonOasis.Show()
        Else
            LblNonOasis.Hide()
        End If


        'Vérification de l'existence d'ALD
        LblALD.Hide()
        ChargementToolTipAld()

        'Contre-indication
        GetContreIndication()

        'Allergie
        GetAllergie()

    End Sub

    Private Sub ChargementToolTipAld()
        Dim StringTooltip As String
        Dim aldDao As New AldDao

        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.PatientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub GetAllergie()
        Dim patientDao As New PatientDao
        Dim StringAllergieToolTip As String = patientDao.GetStringAllergieByPatient(SelectedPatient.PatientId)
        If StringAllergieToolTip = "" Then
            LblAllergie.Hide()
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = False
        Else
            LblAllergie.Show()
            ToolTip.SetToolTip(LblAllergie, StringAllergieToolTip)
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub GetContreIndication()
        Dim patientDao As New PatientDao
        Dim StringContreIndicationToolTip As String = patientDao.GetStringContreIndicationByPatient(SelectedPatient.PatientId)
        If StringContreIndicationToolTip = "" Then
            LblContreIndication.Hide()
            ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = False
        Else
            LblContreIndication.Show()
            ToolTip.SetToolTip(LblContreIndication, StringContreIndicationToolTip)
            ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub GVVaccin_Click(sender As Object, ByVal e As GridViewCellEventArgs) Handles GVVaccin.CellClick
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim row As Integer = e.RowIndex
            Dim valenceCol As Integer = e.ColumnIndex

            If row >= 0 AndAlso valenceCol = 0 Then
                'If GVValence.Rows.Any(Function(x) x.Cells("checked").Value = True AndAlso x.Cells("valence").Value = GVVaccin.Rows(row).Cells("valence").Value) Then
                'GVVaccin.Rows(row).Cells("dci").Style.ForeColor = Color.Red

                GVVaccin.Rows(row).Cells("checked").Value = Not GVVaccin.Rows(row).Cells("checked").Value
                    GVVaccin.Refresh()
                    GVVaccin.Update()
                    ChargementValences()
                    'End If
                End If
        End If
        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub BtnAdminVaccin_Click(sender As Object, e As EventArgs) Handles BtnAdminVaccin.Click
        Using radFVaccinInput As New RadFVaccinInput
            radFVaccinInput.Vaccins = Vaccins
            radFVaccinInput.ShowDialog()
        End Using
    End Sub
End Class
