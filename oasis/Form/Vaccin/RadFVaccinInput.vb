Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFVaccinInput
    Property Vaccins As List(Of VaccinValence)
    Private Sub RadFVaccinInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Administrer", userLog)
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        GVVaccin.Rows.Clear()
        Dim iGrid As Integer = 0

        For Each vaccin As VaccinValence In Vaccins.GroupBy(Function(x) x.Code).Select(Function(x) x.First).ToList
            GVVaccin.Rows.Add(iGrid)
            GVVaccin.Rows(iGrid).Cells("id").Value = vaccin.Id
            GVVaccin.Rows(iGrid).Cells("dci").Value = vaccin.Dci
            iGrid += 1
        Next

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub GVVaccin_Click(sender As Object, ByVal e As GridViewCellEventArgs) Handles GVVaccin.CellClick
        Dim row As Integer = e.RowIndex
        Dim valenceCol As Integer = e.ColumnIndex

        If row >= 0 AndAlso valenceCol = 5 Then
            Using radFVaccinInputComment As New RadFVaccinInputComment
                radFVaccinInputComment.ShowDialog()
            End Using
        ElseIf row >= 0 AndAlso valenceCol = 4 Then
            Using radFVaccinInputComment As New RadFVaccinInputRealisation
                radFVaccinInputComment.ShowDialog()
            End Using
        End If

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub
End Class
