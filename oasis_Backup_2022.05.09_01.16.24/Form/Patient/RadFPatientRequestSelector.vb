Public Class RadFPatientRequestSelector

    Private Sub RadFPatientRequestSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Liste des requetes", userLog)
    End Sub

    Private Sub BtnDRC_Click(sender As Object, e As EventArgs) Handles BtnDRC.Click
        Try
            Using form As New RadFPatientRequest
                form.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BtnEtatJournalier_Click(sender As Object, e As EventArgs) Handles BtnEtatJournalier.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using formT As New FrmEtatJournalier()
                formT.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try
    End Sub
End Class
