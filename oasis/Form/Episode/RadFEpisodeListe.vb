Public Class RadFEpisodeListe
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Private Sub RadFEpisodeListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub RadBtnEpisode_Click(sender As Object, e As EventArgs) Handles RadBtnEpisode.Click
        Using vRadFEpisodeDetail As New RadFEpisodeDetail
            vRadFEpisodeDetail.SelectedPatient = Me.SelectedPatient
            vRadFEpisodeDetail.UtilisateurConnecte = Me.UtilisateurConnecte
            vRadFEpisodeDetail.ShowDialog() 'Modal
        End Using
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
