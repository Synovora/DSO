Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFEpisodeLigneDeVie
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

    Dim ordonnanceDao As New OrdonnanceDao
    Dim episodeDao As New EpisodeDao
    Dim parametreDao As New ParametreDao
    Dim episodeParametreDao As New EpisodeParametreDao
    Dim patientParametreLdvDao As New PatientParametreLdvDao

    Dim patientParametreLdv As PatientParametreLdv
    Dim ligneDeVie As New LigneDeVie

    Dim listeParametreaAfficher As New List(Of Long)

    Dim Parametre1Id As Long
    Dim Parametre2Id As Long
    Dim Parametre3Id As Long
    Dim Parametre4Id As Long
    Dim Parametre5Id As Long

    Dim ConfigurationParametreExiste As Boolean

    Private Sub RadFEpisodeListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afficheTitleForm(Me, "Ligne de vie du patient")
        GetParametresEtFiltres()
        InitFiltre()
        ChargementEtatCivil()
        ChargementEpisode(ligneDeVie)
    End Sub

    Private Sub GetParametresEtFiltres()
        ConfigurationParametreExiste = True
        Try
            patientParametreLdv = patientParametreLdvDao.GetEpisodeByPatientId(SelectedPatient.patientId)
        Catch ex As Exception
            Dim messageErreur As String = ex.Message
            If messageErreur.StartsWith("RNF") Then
                ConfigurationParametreExiste = False
                patientParametreLdv = New PatientParametreLdv()
                patientParametreLdv.PatientId = SelectedPatient.patientId
            End If
        End Try
    End Sub

    Private Sub InitFiltre()
        Dim limiteAgeEnfant As Integer
        Dim limiteAgeEnfantString As Integer = ConfigurationManager.AppSettings("limiteAgeEnfant")
        If IsNumeric(limiteAgeEnfantString) Then
            limiteAgeEnfant = CInt(limiteAgeEnfantString)
        Else
            limiteAgeEnfant = 16
        End If

        Dim AgeMinPreventionFemme As Integer
        Dim AgeMinPreventionFemmeString As Integer = ConfigurationManager.AppSettings("AgeMinPreventionFemme")
        If IsNumeric(AgeMinPreventionFemmeString) Then
            AgeMinPreventionFemme = CInt(AgeMinPreventionFemmeString)
        Else
            AgeMinPreventionFemme = 12
        End If

        ChkTypeConsultation.Checked = True
        ChkTypeVirtuel.Checked = True

        ChkEnfantPreScolaire.Checked = True
        ChkEnfantScolaire.Checked = True
        ChkPathologieAigue.Checked = True
        ChkPreventionAutre.Checked = True
        ChkSocial.Checked = True
        ChkSuiviChronique.Checked = True
        ChkSuiviGrossesse.Checked = True
        ChkSuiviGynecologique.Checked = True

        ChkProfilMedical.Checked = True
        ChkProfilParamedical.Checked = True

        ligneDeVie.TypeConsultation = True
        ligneDeVie.TypeVirtuel = True

        ligneDeVie.ActivitePathologieAigue = True
        ligneDeVie.ActivitePreventionAutre = True
        ligneDeVie.ActivitePreventionEnfantPreScolaire = True
        ligneDeVie.ActivitePreventionEnfantScolaire = True
        ligneDeVie.ActiviteSocial = True
        ligneDeVie.ActiviteSuiviChronique = True
        ligneDeVie.ActiviteSuiviGrossesse = True
        ligneDeVie.ActiviteSuiviGyncologique = True

        ligneDeVie.ProfilMedical = True
        ligneDeVie.ProfilParamedical = True

        Dim Age As Integer = outils.CalculAgeEnAnnee(SelectedPatient.PatientDateNaissance)
        If Age > limiteAgeEnfant Then
            ligneDeVie.ActivitePreventionEnfantPreScolaire = False
            ligneDeVie.ActivitePreventionEnfantScolaire = False
            ChkEnfantPreScolaire.Hide()
            ChkEnfantScolaire.Hide()
            If SelectedPatient.PatientGenreId = PatientDao.EnumGenreId.Feminin OrElse Age >= AgeMinPreventionFemme Then
                ChkSuiviGrossesse.Location = New Point(427, 44)
                ChkSuiviGynecologique.Location = New Point(589, 44)
            End If
        End If

        If SelectedPatient.PatientGenreId = PatientDao.EnumGenreId.Masculin OrElse Age < AgeMinPreventionFemme Then
            ligneDeVie.ActiviteSuiviGrossesse = False
            ligneDeVie.ActiviteSuiviGyncologique = False
            ChkSuiviGrossesse.Hide()
            ChkSuiviGynecologique.Hide()
        End If

        Lblparametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        LblParametre4.Text = ""
        LblParametre5.Text = ""

        Parametre1Id = 0
        Parametre2Id = 0
        Parametre3Id = 0
        Parametre4Id = 0
        Parametre5Id = 0

        DteDepuis.Value = Date.Now()

        DteJusqua.Value = Date.Now().AddYears(-2)

        LblLabelParametre.Hide()

        If ConfigurationParametreExiste = True Then
            ChkTypeConsultation.Checked = patientParametreLdv.TypeConsultation
            ChkTypeVirtuel.Checked = patientParametreLdv.TypeVirtuel

            ChkEnfantPreScolaire.Checked = patientParametreLdv.ActivitePreventionEnfantPreScolaire
            ChkEnfantScolaire.Checked = patientParametreLdv.ActivitePreventionEnfantScolaire
            ChkPathologieAigue.Checked = patientParametreLdv.ActivitePathologieAigue
            ChkPreventionAutre.Checked = patientParametreLdv.ActivitePreventionAutre
            ChkSocial.Checked = patientParametreLdv.ActiviteSocial
            ChkSuiviChronique.Checked = patientParametreLdv.ActiviteSuiviChronique
            ChkSuiviGrossesse.Checked = patientParametreLdv.ActiviteSuiviGrossesse
            ChkSuiviGynecologique.Checked = patientParametreLdv.ActiviteSuiviGynecologique

            ChkProfilMedical.Checked = patientParametreLdv.ProfilMedical
            ChkProfilParamedical.Checked = patientParametreLdv.ProfilParamedical

            ligneDeVie.TypeConsultation = patientParametreLdv.TypeConsultation
            ligneDeVie.TypeVirtuel = patientParametreLdv.TypeVirtuel

            ligneDeVie.ActivitePathologieAigue = patientParametreLdv.ActivitePathologieAigue
            ligneDeVie.ActivitePreventionAutre = patientParametreLdv.ActivitePreventionAutre
            ligneDeVie.ActivitePreventionEnfantPreScolaire = patientParametreLdv.ActivitePreventionEnfantPreScolaire
            ligneDeVie.ActivitePreventionEnfantScolaire = patientParametreLdv.ActivitePreventionEnfantScolaire
            ligneDeVie.ActiviteSocial = patientParametreLdv.ActiviteSocial
            ligneDeVie.ActiviteSuiviChronique = patientParametreLdv.ActiviteSuiviChronique
            ligneDeVie.ActiviteSuiviGrossesse = patientParametreLdv.ActiviteSuiviGrossesse
            ligneDeVie.ActiviteSuiviGyncologique = patientParametreLdv.ActiviteSuiviGynecologique

            ligneDeVie.ProfilMedical = patientParametreLdv.ProfilMedical
            ligneDeVie.ProfilParamedical = patientParametreLdv.ProfilParamedical

            If patientParametreLdv.Parametre1 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre1)
            End If
            If patientParametreLdv.Parametre2 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre2)
            End If
            If patientParametreLdv.Parametre3 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre3)
            End If
            If patientParametreLdv.Parametre4 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre4)
            End If
            If patientParametreLdv.Parametre5 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre5)
            End If

            AfficheParametres()
        End If

    End Sub

    Private Sub AfficheParametres()
        Lblparametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        LblParametre4.Text = ""
        LblParametre5.Text = ""

        Parametre1Id = 0
        Parametre2Id = 0
        Parametre3Id = 0
        Parametre4Id = 0
        Parametre5Id = 0

        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 5 Then
            RadBtnParametre.Hide()
        Else
            RadBtnParametre.Show()
        End If

        If nombreParametre > 0 Then
            LblLabelParametre.Show()
        Else
            LblLabelParametre.Hide()
        End If

        Dim i As Integer = 0
        Dim ParametreEnumerator As List(Of Long).Enumerator = listeParametreaAfficher.GetEnumerator()
        While ParametreEnumerator.MoveNext()
            i += 1
            If i <= 5 Then
                Dim parametre As Parametre
                Dim parametreId As Long = ParametreEnumerator.Current
                parametre = parametreDao.GetParametreById(parametreId)
                Select Case i
                    Case 1
                        Lblparametre1.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre1Id = parametre.Id
                    Case 2
                        LblParametre2.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre2Id = parametre.Id
                    Case 3
                        LblParametre3.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre3Id = parametre.Id
                    Case 4
                        LblParametre4.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre4Id = parametre.Id
                    Case 5
                        LblParametre5.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre5Id = parametre.Id
                End Select
            Else
                Exit Sub
            End If
        End While
    End Sub

    Private Sub ChargementEpisode(ligneDeVie As LigneDeVie)
        Cursor.Current = Cursors.WaitCursor
        RadGridViewEpisode.Rows.Clear()

        Dim dt As DataTable
        Dim episodeDao As New EpisodeDao
        dt = episodeDao.GetAllEpisodeByPatient(SelectedPatient.patientId, DteDepuis.Value, DteJusqua.Value, ligneDeVie)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateCreation As Date
        Dim conclusionMedicale As String
        Dim rowCount As Integer = dt.Rows.Count - 1

        Dim etatCode As String
        Dim DatePrecedente As Date = Date.Now()

        If Lblparametre1.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre1").HeaderText = Lblparametre1.Text
            RadGridViewEpisode.Columns.Item("parametre1").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre1").IsVisible = False
        End If
        If LblParametre2.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre2").HeaderText = LblParametre2.Text
            RadGridViewEpisode.Columns.Item("parametre2").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre2").IsVisible = False
        End If
        If LblParametre3.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre3").HeaderText = LblParametre3.Text
            RadGridViewEpisode.Columns.Item("parametre3").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre3").IsVisible = False
        End If
        If LblParametre4.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre4").HeaderText = LblParametre4.Text
            RadGridViewEpisode.Columns.Item("parametre4").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre4").IsVisible = False
        End If
        If LblParametre5.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre5").HeaderText = LblParametre5.Text
            RadGridViewEpisode.Columns.Item("parametre5").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre5").IsVisible = False
        End If

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadGridViewEpisode.Rows.Add(iGrid)
            'Alimentation du DataGridView
            Dim episodeId As Long = dt.Rows(i)("episode_id")

            RadGridViewEpisode.Rows(iGrid).Cells("episode_id").Value = dt.Rows(i)("episode_id")
            RadGridViewEpisode.Rows(iGrid).Cells("type").Value = Coalesce(dt.Rows(i)("type"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = episodeDao.GetItemTypeActiviteByCode(Coalesce(dt.Rows(i)("type_activite"), ""))
            RadGridViewEpisode.Rows(iGrid).Cells("type_profil").Value = Coalesce(dt.Rows(i)("type_profil"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("description_activite").Value = Coalesce(dt.Rows(i)("description_activite"), "")

            'Conclusion selon le type de l'épisode
            conclusionMedicale = Coalesce(dt.Rows(i)("observation_medical"), "")
            If conclusionMedicale <> "" Then
                RadGridViewEpisode.Rows(iGrid).Cells("conclusion").Value = conclusionMedicale
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("conclusion").Value = Coalesce(dt.Rows(i)("observation_paramedical"), "")
            End If

            If RadGridViewEpisode.Columns.Item("parametre1").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre1").Value = ""
                If Parametre1Id <> 0 Then
                    Dim episodeParametre As EpisodeParametre
                    episodeParametre = episodeParametreDao.GetEpisodeParametreByParametreIdAndEpisodeId(Parametre1Id, episodeId)
                    If episodeParametre.Id <> 0 Then
                        If episodeParametre.Valeur <> 0 Then
                            RadGridViewEpisode.Rows(iGrid).Cells("parametre1").Value = episodeParametre.Valeur
                        End If
                    End If
                End If
            End If

            If RadGridViewEpisode.Columns.Item("parametre2").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre2").Value = ""
                If Parametre2Id <> 0 Then
                    Dim episodeParametre As EpisodeParametre
                    episodeParametre = episodeParametreDao.GetEpisodeParametreByParametreIdAndEpisodeId(Parametre2Id, episodeId)
                    If episodeParametre.Id <> 0 Then
                        If episodeParametre.Valeur <> 0 Then
                            RadGridViewEpisode.Rows(iGrid).Cells("parametre2").Value = episodeParametre.Valeur
                        End If
                    End If
                End If
            End If

            If RadGridViewEpisode.Columns.Item("parametre3").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre3").Value = ""
                If Parametre3Id <> 0 Then
                    Dim episodeParametre As EpisodeParametre
                    episodeParametre = episodeParametreDao.GetEpisodeParametreByParametreIdAndEpisodeId(Parametre3Id, episodeId)
                    If episodeParametre.Id <> 0 Then
                        If episodeParametre.Valeur <> 0 Then
                            RadGridViewEpisode.Rows(iGrid).Cells("parametre3").Value = episodeParametre.Valeur
                        End If
                    End If
                End If
            End If

            If RadGridViewEpisode.Columns.Item("parametre4").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre4").Value = ""
                If Parametre4Id <> 0 Then
                    Dim episodeParametre As EpisodeParametre
                    episodeParametre = episodeParametreDao.GetEpisodeParametreByParametreIdAndEpisodeId(Parametre4Id, episodeId)
                    If episodeParametre.Id <> 0 Then
                        If episodeParametre.Valeur <> 0 Then
                            RadGridViewEpisode.Rows(iGrid).Cells("parametre4").Value = episodeParametre.Valeur
                        End If
                    End If
                End If
            End If

            If RadGridViewEpisode.Columns.Item("parametre5").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre5").Value = ""
                If Parametre5Id <> 0 Then
                    Dim episodeParametre As EpisodeParametre
                    episodeParametre = episodeParametreDao.GetEpisodeParametreByParametreIdAndEpisodeId(Parametre5Id, episodeId)
                    If episodeParametre.Id <> 0 Then
                        If episodeParametre.Valeur <> 0 Then
                            RadGridViewEpisode.Rows(iGrid).Cells("parametre5").Value = episodeParametre.Valeur
                        End If
                    End If
                End If
            End If

            'TODO: Episode détail (ligne de vie) - Traiter le calcul de l'IMC et du PAS qui dépendent de deux autres paramètres

            dateCreation = Coalesce(dt.Rows(i)("date_creation"), Nothing)
            If dateCreation <> Nothing Then
                RadGridViewEpisode.Rows(iGrid).Cells("date_creation").Value = dateCreation.ToString("dd.MM.yyyy")
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("date_creation").Value = ""
            End If

            Dim periode As String = CalculDureeEnJourString(dateCreation, DatePrecedente)
            RadGridViewEpisode.Rows(iGrid).Cells("periode").Value = periode
            DatePrecedente = dateCreation

            etatCode = Coalesce(dt.Rows(i)("etat"), "")
            Select Case etatCode
                Case EpisodeDao.EnumEtatEpisode.EN_COURS.ToString
                    RadGridViewEpisode.Rows(iGrid).Cells("etat").Value = "En cours"
                    RadGridViewEpisode.Rows(iGrid).Cells("etat").Style.ForeColor = Color.Red
                Case EpisodeDao.EnumEtatEpisode.CLOTURE.ToString
                    RadGridViewEpisode.Rows(iGrid).Cells("etat").Value = "Clôturé"
                Case Else
                    RadGridViewEpisode.Rows(iGrid).Cells("etat").Value = "Inconnu !"
            End Select
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewEpisode.Rows.Count > 0 Then
            Me.RadGridViewEpisode.CurrentRow = RadGridViewEpisode.ChildRows(0)
        End If
        Cursor.Current = Cursors.Default
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

    Private Sub OrdonnanceMédicaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrdonnanceMédicaleToolStripMenuItem.Click
        If RadGridViewEpisode.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)
            If aRow >= 0 Then
                Dim EpisodeId As Integer = RadGridViewEpisode.Rows(aRow).Cells("episode_Id").Value
                Dim episode As Episode = episodeDao.GetEpisodeById(EpisodeId)
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Dim OrdonnanceId As Long
                Dim dt As DataTable
                dt = ordonnanceDao.getOrdonnanceValidebyPatient(SelectedPatient.patientId, EpisodeId)
                If dt.Rows.Count > 0 Then
                    'Ordonnance existante
                    OrdonnanceId = dt.Rows(0)("oa_ordonnance_id")
                    AfficheOrdonnance(OrdonnanceId, episode)
                Else
                    If episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString OrElse episode.Etat = EpisodeDao.EnumEtatEpisode.ANNULE.ToString Then
                        If episode.DateModification.Date < Date.Now.Date Then
                            MessageBox.Show("Il n'y a pas d'ordonnance de créée pour cet épisode clôturé !")
                            Cursor.Current = Cursors.Default
                            Me.Enabled = True
                            Exit Sub
                        End If
                    End If
                    OrdonnanceId = ordonnanceDao.CreateOrdonnance(SelectedPatient.patientId, EpisodeId)
                    If OrdonnanceId <> 0 Then
                        If ordonnanceDao.CreateNewOrdonnanceDetail(SelectedPatient.patientId, OrdonnanceId, episode) = True Then
                            AfficheOrdonnance(OrdonnanceId, episode)
                        Else
                            'Erreur, l'ordonnance détail n'a pa été créée
                        End If
                    Else
                        'Erreur, l'ordonnance n'a pa été créée
                    End If
                End If
                Cursor.Current = Cursors.Default
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub AfficheOrdonnance(OrdonnanceId As Long, episode As Episode)
        Using vFOrdonnanceListeDetail As New RadFOrdonnanceListeDetail
            vFOrdonnanceListeDetail.SelectedOrdonnanceId = OrdonnanceId
            vFOrdonnanceListeDetail.SelectedPatient = Me.SelectedPatient
            vFOrdonnanceListeDetail.SelectedEpisode = episode
            vFOrdonnanceListeDetail.UtilisateurConnecte = Me.UtilisateurConnecte
            'vFOrdonnanceListeDetail.Allergie = Me.Allergie
            'vFOrdonnanceListeDetail.ContreIndication = Me.ContreIndication
            vFOrdonnanceListeDetail.CommentaireOrdonnance = ""
            vFOrdonnanceListeDetail.ShowDialog()
        End Using
    End Sub

    Private Sub RadBtnParametre_Click(sender As Object, e As EventArgs) Handles RadBtnParametre.Click
        Me.Enabled = False
        Using form As New RadFParametreSelecteur
            form.ListeParametreExistant = listeParametreaAfficher
            form.ShowDialog()
            If form.IsSelected = True Then
                listeParametreaAfficher.Add(form.SelectedParametre.Id)
                AfficheParametres()
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub Lblparametre1_Click(sender As Object, e As EventArgs) Handles Lblparametre1.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 1 Then
            listeParametreaAfficher.RemoveAt(0)
            AfficheParametres()
        End If
    End Sub

    Private Sub LblParametre2_Click(sender As Object, e As EventArgs) Handles LblParametre2.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 2 Then
            listeParametreaAfficher.RemoveAt(1)
            AfficheParametres()
        End If
    End Sub

    Private Sub LblParametre3_Click(sender As Object, e As EventArgs) Handles LblParametre3.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 3 Then
            listeParametreaAfficher.RemoveAt(2)
            AfficheParametres()
        End If
    End Sub

    Private Sub LblParametre4_Click(sender As Object, e As EventArgs) Handles LblParametre4.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 4 Then
            listeParametreaAfficher.RemoveAt(3)
            AfficheParametres()
        End If
    End Sub

    Private Sub LblParametre5_Click(sender As Object, e As EventArgs) Handles LblParametre5.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 5 Then
            listeParametreaAfficher.RemoveAt(4)
            AfficheParametres()
        End If
    End Sub

    Private Sub RadBtnParametreValidation_Click(sender As Object, e As EventArgs) Handles RadBtnParametreValidation.Click
        Dim dateDebut As Date = DteDepuis.Value
        Dim DateFin As Date = DteJusqua.Value
        If dateDebut.Date < DateFin.Date Then
            MessageBox.Show("Sélection incorrecte : La date 'depuis' " & dateDebut.ToString("dd.MM.yyyy") &
                            " ne doit pas être inférieure à la date 'jusqu'à' " & DateFin.ToString("dd.MM.yyyy") & " !")
            Exit Sub
        End If

        'Activité
        If ChkEnfantPreScolaire.Checked = True Then
            ligneDeVie.ActivitePreventionEnfantPreScolaire = True
        Else
            ligneDeVie.ActivitePreventionEnfantPreScolaire = False
        End If

        If ChkEnfantScolaire.Checked = True Then
            ligneDeVie.ActivitePreventionEnfantScolaire = True
        Else
            ligneDeVie.ActivitePreventionEnfantScolaire = False
        End If

        If ChkPathologieAigue.Checked = True Then
            ligneDeVie.ActivitePathologieAigue = True
        Else
            ligneDeVie.ActivitePathologieAigue = False
        End If

        If ChkPreventionAutre.Checked = True Then
            ligneDeVie.ActivitePreventionAutre = True
        Else
            ligneDeVie.ActivitePreventionAutre = False
        End If

        If ChkSocial.Checked = True Then
            ligneDeVie.ActiviteSocial = True
        Else
            ligneDeVie.ActiviteSocial = False
        End If

        If ChkSuiviChronique.Checked = True Then
            ligneDeVie.ActiviteSuiviChronique = True
        Else
            ligneDeVie.ActiviteSuiviChronique = False
        End If

        If ChkSuiviGrossesse.Checked = True Then
            ligneDeVie.ActiviteSuiviGrossesse = True
        Else
            ligneDeVie.ActiviteSuiviGrossesse = False
        End If

        If ChkSuiviGynecologique.Checked = True Then
            ligneDeVie.ActiviteSuiviGyncologique = True
        Else
            ligneDeVie.ActiviteSuiviGyncologique = False
        End If

        'Type
        If ChkTypeConsultation.Checked = True Then
            ligneDeVie.TypeConsultation = True
        Else
            ligneDeVie.TypeConsultation = False
        End If

        If ChkTypeVirtuel.Checked = True Then
            ligneDeVie.TypeVirtuel = True
        Else
            ligneDeVie.TypeVirtuel = False
        End If

        'Profil
        If ChkProfilMedical.Checked = True Then
            ligneDeVie.ProfilMedical = True
        Else
            ligneDeVie.ProfilMedical = False
        End If

        If ChkProfilParamedical.Checked = True Then
            ligneDeVie.ProfilParamedical = True
        Else
            ligneDeVie.ProfilParamedical = False
        End If

        ChargementEpisode(ligneDeVie)
    End Sub

    Private Sub MasterTemplate_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadGridViewEpisode.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing Then
            If hoveredCell.ColumnInfo.Name = "type_activite" Then
                e.ToolTipText = hoveredCell.RowInfo.Cells("type_activite").Value & " " & hoveredCell.RowInfo.Cells("description_activite").Value
            Else
                e.ToolTipText = hoveredCell.Value.ToString()
            End If
        End If
    End Sub

    Private Sub RadBtnChart_Click(sender As Object, e As EventArgs) Handles RadBtnChart.Click
        Using Form As New RadFLigneDeVieGraphe
            Form.ShowDialog()
        End Using
    End Sub

    'Sauvegarder la configuration des filtres et des paramètres
    Private Sub RadBtnConfiguration_Click(sender As Object, e As EventArgs) Handles RadBtnConfiguration.Click
        If ChkEnfantPreScolaire.Checked = True Then
            patientParametreLdv.ActivitePreventionEnfantPreScolaire = True
        Else
            patientParametreLdv.ActivitePreventionEnfantPreScolaire = False
        End If

        If ChkEnfantScolaire.Checked = True Then
            patientParametreLdv.ActivitePreventionEnfantScolaire = True
        Else
            patientParametreLdv.ActivitePreventionEnfantScolaire = False
        End If

        If ChkPathologieAigue.Checked = True Then
            patientParametreLdv.ActivitePathologieAigue = True
        Else
            patientParametreLdv.ActivitePathologieAigue = False
        End If

        If ChkPreventionAutre.Checked = True Then
            patientParametreLdv.ActivitePreventionAutre = True
        Else
            patientParametreLdv.ActivitePreventionAutre = False
        End If

        If ChkSocial.Checked = True Then
            patientParametreLdv.ActiviteSocial = True
        Else
            patientParametreLdv.ActiviteSocial = False
        End If

        If ChkSuiviChronique.Checked = True Then
            patientParametreLdv.ActiviteSuiviChronique = True
        Else
            patientParametreLdv.ActiviteSuiviChronique = False
        End If

        If ChkSuiviGrossesse.Checked = True Then
            patientParametreLdv.ActiviteSuiviGrossesse = True
        Else
            patientParametreLdv.ActiviteSuiviGrossesse = False
        End If

        If ChkSuiviGynecologique.Checked = True Then
            patientParametreLdv.ActiviteSuiviGynecologique = True
        Else
            patientParametreLdv.ActiviteSuiviGynecologique = False
        End If

        'Type
        If ChkTypeConsultation.Checked = True Then
            patientParametreLdv.TypeConsultation = True
        Else
            patientParametreLdv.TypeConsultation = False
        End If

        If ChkTypeVirtuel.Checked = True Then
            patientParametreLdv.TypeVirtuel = True
        Else
            patientParametreLdv.TypeVirtuel = False
        End If

        'Profil
        If ChkProfilMedical.Checked = True Then
            patientParametreLdv.ProfilMedical = True
        Else
            patientParametreLdv.ProfilMedical = False
        End If

        If ChkProfilParamedical.Checked = True Then
            patientParametreLdv.ProfilParamedical = True
        Else
            patientParametreLdv.ProfilParamedical = False
        End If

        patientParametreLdv.Parametre1 = Coalesce(Parametre1Id, 0)
        patientParametreLdv.Parametre2 = Coalesce(Parametre2Id, 0)
        patientParametreLdv.Parametre3 = Coalesce(Parametre3Id, 0)
        patientParametreLdv.Parametre4 = Coalesce(Parametre4Id, 0)
        patientParametreLdv.Parametre5 = Coalesce(Parametre5Id, 0)

        If ConfigurationParametreExiste = True Then
            patientParametreLdvDao.UpdateConfigurationParametre(patientParametreLdv)
            Dim form As New RadFNotification()
            form.Titre = "Notification configuration filtre et paramètre de la ligne de vie"
            form.Message = "Configuration filtre et paramètre modifiée"
            form.Show()
        Else
            ConfigurationParametreExiste = True
            patientParametreLdvDao.CreateConfigurationParametre(patientParametreLdv)
            Dim form As New RadFNotification()
            form.Titre = "Notification configuration filtre et paramètre de la ligne de vie"
            form.Message = "Configuration filtre et paramètre créée"
            form.Show()
        End If

    End Sub
End Class
