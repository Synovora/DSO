Public Class RadFEpisodeProtocoleAiguDetail
    Private _episodeActeParamedicalId As Long
    Private _codeRetour As Boolean

    Dim episodeActeParamedicalDao As New EpisodeActeParamedicalDao

    Private Sub RadFEpisodeProtocoleAiguDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementObservation()

    End Sub

    Private Sub ChargementObservation()

    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click

    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
