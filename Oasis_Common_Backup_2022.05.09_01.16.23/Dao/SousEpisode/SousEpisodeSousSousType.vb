
Public Class SousEpisodeSousSousType

    Property Id As Long
    Property IdSousEpisodeSousType As Long
    Property HorodateCreation As DateTime
    Property Libelle As String
    Property Commentaire As String

    Public Sub New()
    End Sub

    '''
    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.IdSousEpisodeSousType = row("id_sous_episode_sous_type")
        Me.HorodateCreation = row("horodate_creation")
        Me.Libelle = row("libelle")
        Me.Commentaire = Coalesce(row("commentaire"), "")
    End Sub



End Class
