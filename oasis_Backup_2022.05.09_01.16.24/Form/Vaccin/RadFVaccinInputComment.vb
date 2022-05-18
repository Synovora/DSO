Public Class RadFVaccinInputComment

    Property codeRetour As Boolean
    Property ProgramId As Long
    Property Lock As Boolean

    Private Sub RadFVaccinInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Information", userLog)
        Init()
    End Sub

    Private Sub Init()
        If Lock Then
            DTPExp.Enabled = False
            TextLot.Enabled = False
        End If
        DTPExp.Value = If(DTPExp.Value = Nothing, Date.Now(), DTPExp.Value)
        DTPExp.Format = DateTimePickerFormat.Custom
        DTPExp.CustomFormat = "MM/yyyy"
    End Sub

    Private Sub RadBtnCancel_Click(sender As Object, e As EventArgs) Handles RadBtnCancel.Click
        codeRetour = False
        Me.Close()
    End Sub

    Private Sub RadBtnDone_Click(sender As Object, e As EventArgs) Handles RadBtnDone.Click
        codeRetour = True
        Me.Close()
    End Sub

    Private Sub TextLot_TextChanged(sender As Object, e As EventArgs) Handles TextLot.TextChanged
        If TextLot.Text IsNot Nothing Then
            RadBtnDone.Enabled = True
        Else
            RadBtnDone.Enabled = False
        End If
    End Sub
End Class
