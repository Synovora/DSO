Public Class RadFDrcAideEnLigne
    Property commentaireDrc As String
    Property descriptionDrc As String

    Private Sub RadFDrcAideEnLigne_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtDescriptionDrc.Text = descriptionDrc
        TxtCommentaireDrc.Text = commentaireDrc

        RadBtnAbandonner.Select()
    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub
End Class
