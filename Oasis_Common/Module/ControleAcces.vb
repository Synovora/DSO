''' <summary>
''' Garde-fous contre la double ouverture des écrans principaux (Synthèse,
''' Épisode, Ligne de vie) et d'un même épisode. Chaque écran s'inscrit à
''' l'ouverture et se retire à la fermeture ; un second appel pour la même clé
''' est refusé par IsAccessTo...OK.
''' </summary>
Public Class ControleAccesForm
    Private Shared ReadOnly form_acces As New List(Of String)()

    Public Shared Function IsAccessToFormOK(formAcces As String) As Boolean
        Return Not form_acces.Contains(formAcces)
    End Function

    Public Shared Sub AddFormToControl(formAcces As String)
        If Not form_acces.Contains(formAcces) Then form_acces.Add(formAcces)
    End Sub

    Public Shared Sub RemoveFormToControl(formAcces As String)
        form_acces.Remove(formAcces)
    End Sub

End Class

Public Class ControleAccesEpisode
    Private Shared ReadOnly episode_acces As New List(Of Long)()

    Public Shared Function IsAccessToEpisodeOK(episodeAcces As Long) As Boolean
        Return Not episode_acces.Contains(episodeAcces)
    End Function

    Public Shared Sub AddEpisodeToControl(episodeAcces As Long)
        If Not episode_acces.Contains(episodeAcces) Then episode_acces.Add(episodeAcces)
    End Sub

    Public Shared Sub RemoveEpisodeToControl(episodeAcces As Long)
        episode_acces.Remove(episodeAcces)
    End Sub

End Class
