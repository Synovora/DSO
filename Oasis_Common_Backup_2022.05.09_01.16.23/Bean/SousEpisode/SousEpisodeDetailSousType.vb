Public Class SousEpisodeDetailSousType

    Property Id As Long
    Property IdSousEpisode As Long
    Property IdSousEpisodeSousSousType As Long
    Property IsALD As Boolean

    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.IdSousEpisode = row("id_sous_episode")
        Me.IdSousEpisodeSousSousType = row("id_sous_episode_sous_sous_type")
        Me.IsALD = row("is_ald")
    End Sub

End Class
