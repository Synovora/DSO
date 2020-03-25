﻿Public Class RadFTestMethodes

    Dim parcoursDao As New ParcoursDao
    Dim contexteDao As New ContexteDao

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnIntervenant_Click(sender As Object, e As EventArgs) Handles RadBtnIntervenant.Click
        parcoursDao.GetListOfIntervenantNonOasisByPatient(1)
    End Sub

    Private Sub RadBtnContexte_Click(sender As Object, e As EventArgs) Handles RadBtnContexte.Click
        contexteDao.GetListOfContextebyPatient(1)
    End Sub
End Class
