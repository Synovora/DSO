Imports System.Globalization
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFVaccinInput

    ReadOnly vaccinDao As VaccinDao = New VaccinDao
    Property Vaccins As List(Of VaccinValence)
    Property VaccinPrograms As List(Of VaccinProgramRelation)

    Private Sub RadFVaccinInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Administrer", userLog)
        DTPRealisation.Format = DateTimePickerFormat.Custom
        DTPRealisation.CustomFormat = "dd/MM/yyyy"
        If (VaccinPrograms.Count > 0 AndAlso VaccinPrograms(0).RealisationDate <> Nothing) Then
            DTPRealisation.Value = VaccinPrograms(0).RealisationDate
            DTPRealisation_ValueChanged(Nothing, Nothing)
        End If
        Refresh()
    End Sub

    Private Sub Refresh()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        GVVaccin.Rows.Clear()
        Dim iGrid As Integer = 0

        For Each vaccin As VaccinValence In Vaccins.GroupBy(Function(x) x.Code).Select(Function(x) x.First).ToList
            GVVaccin.Rows.Add(iGrid)
            GVVaccin.Rows(iGrid).Cells("id").Value = vaccin.Id
            GVVaccin.Rows(iGrid).Cells("dci").Value = vaccin.Dci
            GVVaccin.Rows(iGrid).Cells("relation_id").Value = VaccinPrograms.Find(Function(x) x.Vaccin = vaccin.Id).Id
            Dim vaccinProgramAdmin = vaccinDao.GetVaccinProgramAdministrationByRelation(VaccinPrograms.Find(Function(x) x.Vaccin = vaccin.Id).Id)
            If (vaccinProgramAdmin IsNot Nothing) Then
                GVVaccin.Rows(iGrid).Cells("program_id").Value = vaccinProgramAdmin.Id
                GVVaccin.Rows(iGrid).Cells("lot").Value = vaccinProgramAdmin.Lot
                GVVaccin.Rows(iGrid).Cells("expiration").Value = vaccinProgramAdmin.Expiration
                GVVaccin.Rows(iGrid).Cells("comment").Value = vaccinProgramAdmin.Comment
            End If
            iGrid += 1
        Next

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub GVVaccin_Click(sender As Object, ByVal e As GridViewCellEventArgs) Handles GVVaccin.CellClick
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Dim row As Integer = e.RowIndex
        Dim valenceCol As Integer = e.ColumnIndex

        If row >= 0 Then
            Using radFVaccinInputComment As New RadFVaccinInputComment
                radFVaccinInputComment.TextLot.Text =
                     Coalesce(GVVaccin.Rows(row).Cells("lot").Value, "")
                radFVaccinInputComment.DTPExp.Value = Coalesce(GVVaccin.Rows(row).Cells("expiration").Value, Date.Now())
                radFVaccinInputComment.RTEComment.Text = Coalesce(GVVaccin.Rows(row).Cells("comment").Value, "")
                radFVaccinInputComment.ProgramId = GVVaccin.Rows(row).Cells("program_id").Value
                radFVaccinInputComment.ShowDialog()
                If (radFVaccinInputComment.codeRetour) Then
                    If (radFVaccinInputComment.ProgramId = Nothing) Then
                        vaccinDao.CreateVaccinProgramAdministration(New VaccinProgramAdmin With {
                    .Lot = radFVaccinInputComment.TextLot.Text,
                    .Comment = radFVaccinInputComment.RTEComment.Text,
                    .Expiration = radFVaccinInputComment.DTPExp.Value,
                    .VaccinProgramRelation = GVVaccin.Rows(row).Cells("relation_id").Value
                                                            })
                        Refresh()
                    Else
                        vaccinDao.UpdateVaccinProgramAdministration(New VaccinProgramAdmin With {
                              .Id = radFVaccinInputComment.ProgramId,
                              .Lot = radFVaccinInputComment.TextLot.Text,
                              .Comment = radFVaccinInputComment.RTEComment.Text,
                              .Expiration = radFVaccinInputComment.DTPExp.Value,
                              .VaccinProgramRelation = GVVaccin.Rows(row).Cells("relation_id").Value
                                                      })
                        Refresh()
                    End If
                End If
            End Using
        End If

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        For Each vaccinProgram In VaccinPrograms
            vaccinDao.UpdateVaccinProgramRelation(New VaccinProgramRelation With {
                                                  .Id = vaccinProgram.Id,
                                                  .[Date] = vaccinProgram.Date,
                                                  .Patient = vaccinProgram.Patient,
                                                  .Vaccin = vaccinProgram.Vaccin,
                                                  .RealisationDate = DTPRealisation.Value})
        Next
        Cursor.Current = Cursors.Default
        Me.Enabled = True
        Me.Close()
    End Sub

    Private Sub DTPRealisation_ValueChanged(sender As Object, e As EventArgs) Handles DTPRealisation.ValueChanged
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        For Each row In GVVaccin.Rows
            row.Cells("realisation").Value = DTPRealisation.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) & " - "
        Next

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub
End Class
