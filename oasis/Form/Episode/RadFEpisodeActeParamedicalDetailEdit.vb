Public Class RadFEpisodeActeParamedicalDetailEdit
    Private _episodeActeParamedicalId As Long
    Private _codeRetour As Boolean

    Public Property EpisodeActeParamedicalId As Long
        Get
            Return _episodeActeParamedicalId
        End Get
        Set(value As Long)
            _episodeActeParamedicalId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _codeRetour
        End Get
        Set(value As Boolean)
            _codeRetour = value
        End Set
    End Property

    Dim episodeActeParamedicalDao As New EpisodeActeParamedicalDao
    Dim episodeActeParamedical As EpisodeActeParamedical


    Private Sub RadFEpisodeActeParamedicalDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CodeRetour = False
        chargementObservation()

    End Sub

    Private Sub chargementObservation()
        episodeActeParamedical = episodeActeParamedicalDao.GetEpisodeActeParamedicalById(EpisodeActeParamedicalId)
        TxtObservation.Text = episodeActeParamedical.Observation
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        episodeActeParamedicalDao.ModificationEpisodeActeParamedicalObservation(EpisodeActeParamedicalId, TxtObservation.Text)
        CodeRetour = True
        Close()
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub TxtObservation_TextChanged(sender As Object, e As EventArgs) Handles TxtObservation.TextChanged
        episodeActeParamedical.Observation = TxtObservation.Text
    End Sub

End Class
