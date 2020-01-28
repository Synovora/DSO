Imports Oasis_Common

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
        ChargementEtatCivil()
        ChargementEpisode()

    End Sub

    Private Sub ChargementEpisode()
        RadGridViewEpisode.Rows.Clear()

        Dim dt As DataTable
        Dim episodeDao As New EpisodeDao
        dt = episodeDao.GetAllEpisodeByPatient(SelectedPatient.patientId)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateCreation As Date
        Dim rowCount As Integer = dt.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadGridViewEpisode.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadGridViewEpisode.Rows(iGrid).Cells("episode_id").Value = dt.Rows(i)("episode_id")
            RadGridViewEpisode.Rows(iGrid).Cells("type").Value = Coalesce(dt.Rows(i)("type"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = Coalesce(dt.Rows(i)("type_activite"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("type_profil").Value = Coalesce(dt.Rows(i)("type_profil"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("description_activite").Value = Coalesce(dt.Rows(i)("description_activite"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("commentaire").Value = Coalesce(dt.Rows(i)("commentaire"), "")
            dateCreation = Coalesce(dt.Rows(i)("date_creation"), Nothing)
            If dateCreation <> Nothing Then
                RadGridViewEpisode.Rows(iGrid).Cells("date_creation").Value = dateCreation.ToString("dd.MM.yyyy")
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("date_creation").Value = ""
            End If
            RadGridViewEpisode.Rows(iGrid).Cells("etat").Value = Coalesce(dt.Rows(i)("etat"), "")
            If Coalesce(dt.Rows(i)("etat"), "") = EpisodeDao.EnumEtatEpisode.EN_COURS.ToString Then
                RadGridViewEpisode.Rows(iGrid).Cells("etat").Style.ForeColor = Color.Red
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewEpisode.Rows.Count > 0 Then
            Me.RadGridViewEpisode.CurrentRow = RadGridViewEpisode.ChildRows(0)
        End If
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip1.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub RadBtnEpisode_Click(sender As Object, e As EventArgs) Handles RadBtnEpisode.Click
        DetailEpisode()
    End Sub

    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridViewEpisode.CellDoubleClick
        DetailEpisode()
    End Sub

    Private Sub DetailEpisode()
        If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Or userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString) Then
            Dim Message1 As String = "Votre profil de type (" & userLog.TypeProfil & ") ne vous permet pas de gérer un épisode patient, processus annulé"
            Dim Message2 As String = "Les types de profil autorisés sont : " & ProfilDao.EnumProfilType.MEDICAL.ToString() & " et " & ProfilDao.EnumProfilType.PARAMEDICAL.ToString()
            MessageBox.Show(Message1 & vbCrLf & Message2)
            Exit Sub
        End If

        If RadGridViewEpisode.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)
            If aRow >= 0 Then
                Dim EpisodeId As Integer = RadGridViewEpisode.Rows(aRow).Cells("episode_Id").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using form As New RadFEpisodeDetail
                    form.SelectedEpisodeId = EpisodeId
                    form.SelectedPatient = Me.SelectedPatient
                    form.UtilisateurConnecte = Me.UtilisateurConnecte
                    form.ShowDialog() 'Modal
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
