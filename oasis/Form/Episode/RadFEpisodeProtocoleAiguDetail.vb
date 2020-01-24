Public Class RadFEpisodeProtocoleAiguDetail
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
    Dim drcDao As New DrcDao

    Dim episodeActeParamedical As EpisodeActeParamedical
    Dim drc As Drc

    Private Sub RadFEpisodeProtocoleAiguDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementObservation()

    End Sub

    Private Sub ChargementObservation()
        episodeActeParamedical = episodeActeParamedicalDao.GetEpisodeActeParamedicalById(EpisodeActeParamedicalId)
        drc = drcDao.getDrcById(episodeActeParamedical.DrcId)
        TxtObservation.Text = episodeActeParamedical.Observation
        TxtGuide.Text = drc.Commentaire
        TxtReponseCommentee.Text = drc.ReponseCommentee
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If episodeActeParamedicalDao.ModificationEpisodeActeParamedicalObservation(EpisodeActeParamedicalId, TxtObservation.Text) = True Then
            Dim form As New RadFNotification()
            form.Titre = "OBservation spécifique - Protocole aigu"
            form.Message = "Observation mise à jour"
            form.Show()
            CodeRetour = True
            Close()
        Else
            MessageBox.Show("Anomalie, la mise à jour n'a pas aboutie !")
        End If

    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
