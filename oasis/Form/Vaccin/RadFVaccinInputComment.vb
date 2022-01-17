Public Class RadFVaccinInputComment

    Private Sub RadFVaccinInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Information", userLog)
        Init()
    End Sub

    Private Sub Init()
        DTPExp.Value = Date.Now() 'DTPExp.MinDate
        DTPExp.Format = DateTimePickerFormat.Custom
        DTPExp.CustomFormat = "mm/yyyy"
    End Sub

End Class
