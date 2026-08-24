Imports System.Data.SqlClient

Public Class RelationChaineEpisode
    Property Id As Long
    Property EpisodeId As Long
    Property ChaineId As Long

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.EpisodeId = record("episode_id")
        Me.ChaineId = record("chaine_id")
    End Sub

End Class
