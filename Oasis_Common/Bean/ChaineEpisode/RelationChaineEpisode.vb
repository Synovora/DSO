Imports System.Data.SqlClient

Public Class RelationChaineEpisode
    Property Id As Long
    Property EpisodeId As Long
    Property ChaineId As Long

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.EpisodeId = reader("episode_id")
        Me.ChaineId = reader("chaine_id")
    End Sub

End Class
