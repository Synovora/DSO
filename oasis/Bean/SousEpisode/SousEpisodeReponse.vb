Imports System.Configuration
Imports Oasis_Common
Public Class SousEpisodeReponse
    Property Id As Long
    Property IdSousEpisode As Long
    Property CreateUserId As Long
    Property HorodateCreation As DateTime
    Property NomFichier As String
    Property Commentaire As String


    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.IdSousEpisode = row("id_sous_episode")

        Me.CreateUserId = row("create_user_id")
        Me.HorodateCreation = row("horodate_creation")

        Me.NomFichier = Coalesce(row("nom_fichier"), "")
        Me.Commentaire = Coalesce(row("commentaire"), "")

    End Sub

End Class
