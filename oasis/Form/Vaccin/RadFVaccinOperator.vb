Public Class RadFVaccinOperator

    Property SelectedRorId As Long
    Property CodeRetour As Boolean
    'Property SelectedRorId As Long
    'Property OperatorText As String

    Private Sub RadFVaccinOperator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Operateur", userLog)
    End Sub

    'Private Sub BtnPro_Click(sender As Object, e As EventArgs) Handles BtnPro.Click
    '    Try
    '        Using form As New RadFAnnuaireProfessionnelSelect
    '            form.ShowDialog()
    '            'SelectedRorId = form.Selec
    '        End Using
    '    Catch ex As Exception
    '        MsgBox(ex.Message())
    '    End Try
    'End Sub

    Private Sub BtnUser_Click(sender As Object, e As EventArgs) Handles BtnUser.Click
        Try

            Using radFOperatorSelect As New RadFOperatorSelect
                radFOperatorSelect.ShowDialog()
                If radFOperatorSelect.CodeRetour = True Then
                    SelectedRorId = radFOperatorSelect.SelectedRorId
                    CodeRetour = True
                    Me.Close()

                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BtnDone_Click(sender As Object, e As EventArgs) Handles BtnDone.Click
        CodeRetour = True
        Me.Close()
    End Sub
End Class
