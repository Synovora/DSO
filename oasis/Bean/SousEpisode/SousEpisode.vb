﻿Imports Oasis_Common
Public Class SousEpisode
    Property Id As Long
    Property EpisodeId As Long
    Property IdSousEpisodeType As Long
    Property IdSousEpisodeSousType As Long
    Property CreateUserId As Long
    Property HorodateCreation As DateTime
    Property LastUpdateUserId As Long
    Property HorodateLastUpdate As DateTime
    Property ValidateUserId As Long
    Property HorodateValidate As DateTime
    Property NomFichier As String
    Property Commentaire As String
    Property isALD As Boolean

    Public Sub New()
    End Sub

    '''
    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.EpisodeId = row("episode_id")
        Me.IdSousEpisodeType = row("id_sous_episode_type")
        Me.IdSousEpisodeSousType = row("id_sous_episode_sous_type")

        Me.CreateUserId = row("create_user_id")
        Me.HorodateCreation = row("horodate_creation")

        Me.LastUpdateUserId = Coalesce(row("last_update_user_id"), 0)
        Me.HorodateCreation = Coalesce(row("horodate_last_update"), Nothing)

        Me.ValidateUserId = Coalesce(row("Validate_user_id"), 0)
        Me.HorodateValidate = Coalesce(row("horodate_Validate"), Nothing)

        Me.NomFichier = Coalesce(row("nom_fichier"), "")
        Me.Commentaire = Coalesce(row("commentaire"), "")

        Me.isALD = row("is_ald")
    End Sub

End Class
