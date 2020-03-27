Imports Telerik.WinControls.UI

Public Class FrmActionDoc

    Public Property ActionChoisie As FrmEditDocxSousEpisode.ActionDOC = FrmEditDocxSousEpisode.ActionDOC.RETOUR
    Private Sub BtnAnnulation_Click(sender As Object, e As EventArgs) Handles BtnAnnulation.Click
        ActionChoisie = FrmEditDocxSousEpisode.ActionDOC.RETOUR
        Close()
    End Sub

    Private Sub RBtnQuitterSansEnreg_Click(sender As Object, e As EventArgs) Handles RBtnQuitterSansEnreg.Click
        ActionChoisie = FrmEditDocxSousEpisode.ActionDOC.QUITTER
        Close()
    End Sub

    Private Sub BtnEnregistrer_Click(sender As Object, e As EventArgs) Handles BtnEnregistrer.Click
        ActionChoisie = FrmEditDocxSousEpisode.ActionDOC.ENREGISTRER
        Close()
    End Sub
End Class
