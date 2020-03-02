Public Class FrmSousEpisode

    Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
    Dim episode As Episode, patient As Patient, sousEpisode As SousEpisode
    Dim isCreation As Boolean


    Sub New(episode As Episode, patient As Patient, sousEpisode As SousEpisode)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTitleForm(Me, Me.Text)
        ' -- episode en cours
        Me.episode = episode
        Me.patient = patient
        Me.sousEpisode = sousEpisode
        '  -- sous en mode creation (sinon mode update)
        isCreation = If(sousEpisode.Id = 0, True, False)

    End Sub

End Class
