Imports System.Configuration
Imports Oasis_Common

Public Class FrmTacheDetail_vb
    Private tache As Tache
    Private tacheDao As TacheDao = New TacheDao
    Private tacheBeanAssocie As TacheBeanAssocie
    Private _isActionEffectuee1 As Boolean = False

    Public Sub New(idTache As Long)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        initControls(idTache)

    End Sub

    Public Property IsActionEffectuee As Boolean
        Get
            Return _isActionEffectuee1
        End Get
        Set(value As Boolean)
            _isActionEffectuee1 = value
        End Set
    End Property

    Private Sub InitControls(idTache As Long)
        ' -- recupération de la tache
        tache = tacheDao.GetTacheById(idTache, True)

        ' --- centrage et chgt de style du titre du formulaire
        afficheTitleForm(Me, tache.Type.ToString & If(tache.Nature.ToString <> tache.Type.ToString, "/" + tache.Nature.ToString, "") &
            " - créée le " & tache.HorodatageCreation &
            " - Etat : " & tache.Etat.ToString)

        ' -- recherche beans associes à la tache et init des controls
        tacheBeanAssocie = tacheDao.GetTacheBeanAssocie(tache)
        With tacheBeanAssocie
            Me.TxtUtilisateur.Text = .UserEmetteur.UtilisateurPrenom + " " + .UserEmetteur.UtilisateurNom
            If Not IsNothing(.FonctionEmetteur) Then Me.TxtFonction.Text = .FonctionEmetteur.Designation
            Me.TxtCommentaire.Text = tache.EmetteurCommentaire
            Me.TxtUniteSan.Text = .UniteSanitaire.Oa_unite_sanitaire_description
            Me.TxtSite.Text = .Site.Oa_site_description
            Me.TxtPatientNom.Text = .Patient.PatientPrenom + " " + .Patient.PatientNom + If(.Patient.PatientNomMarital <> "", "(" + .Patient.PatientNomMarital + ")", "")
            Me.TxtPatientNir.Text = .Patient.PatientNir
            Select Case tache.Type
                Case Tache.TypeTache.RDV.ToString, Tache.TypeTache.RDV_MISSION.ToString, Tache.TypeTache.REUNION_STAFF.ToString, Tache.TypeTache.RDV_SPECIALISTE.ToString
                    Me.LblTypeRDV.Text = tache.Type.Replace("_", " ")
                    If tache.Etat <> Tache.EtatTache.ANNULEE.ToString AndAlso tache.Etat <> Tache.EtatTache.TERMINEE.ToString Then
                        metEnExergue(Me.LblTypeRDV, Me.TxtTypeRDV)
                    End If
                    Me.TxtTypeRDV.Text = tache.DateRendezVous.ToLongDateString &
                               " à " & Strings.Left(tache.DateRendezVous.ToLongTimeString, Strings.Len(tache.DateRendezVous.ToLongTimeString) - 3) &
                               " - Durée " & tache.Duree & " mn"
                Case Tache.TypeTache.RDV_DEMANDE.ToString, Tache.TypeTache.MISSION_DEMANDE.ToString
                    If Coalesce(tache.TypedemandeRendezVous, "").ToString().Length > 0 Then
                        metEnExergue(Me.LblTypeRDV, Me.TxtTypeRDV)
                        Select Case tache.TypedemandeRendezVous
                            Case Tache.EnumDemandeRendezVous.ANNEE.ToString
                                Me.TxtTypeRDV.Text = "dans l'année " & tache.DateRendezVous.Year & " - Durée " & tache.Duree & " mn"
                            Case Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString
                                Me.TxtTypeRDV.Text = "dans le courant de " & tache.DateRendezVous.Month.ToString("00") & "/" & tache.DateRendezVous.Year &
                                    " - Durée " & tache.Duree & " mn"
                        End Select
                    End If
                Case Tache.TypeTache.AVIS_EPISODE.ToString, Tache.TypeTache.AVIS_SOUS_EPISODE.ToString
                    Me.LblTypeRDV.Text = StrConv(tache.Type.ToLower, VbStrConv.ProperCase).Replace("_", " ").Replace("E", "É")   ' camel case 
                    If tache.Etat <> Tache.EtatTache.ANNULEE.ToString AndAlso tache.Etat <> Tache.EtatTache.TERMINEE.ToString Then
                        metEnExergue(Me.LblTypeRDV, Me.TxtTypeRDV)
                    End If
                    Me.TxtTypeRDV.Text = StrConv(tache.Nature.ToString.ToLower, VbStrConv.ProperCase)   ' camel case  
            End Select
            Me.lblTypeTache.Text = tache.GetLibelleTacheNature
            Me.TxtSpecialite.Text = If(IsNothing(.Specialite), "", .Specialite.Description)
            Me.TxtIntervenant.Text = .Intervenant
            ' -- on efface le conteneur parcours si pas de parcours
            RadGroupParcours.Visible = Not IsNothing(.Parcours)
            ' -- cible
            If Not IsNothing(tacheBeanAssocie.FonctionTraiteur) Then TxtFonctionCible.Text = tacheBeanAssocie.FonctionTraiteur.Designation
            If tache.TraiteUserId Then
                TxtNomCible.Text = tacheBeanAssocie.UserTraiteur.UtilisateurPrenom + " " + tacheBeanAssocie.UserTraiteur.UtilisateurNom
            End If
        End With

        ' ---- boutons d'action

        ' ---- Attribution possible si tache pas encore attribuée et que mes fonctions me le permettent
        BtnAttribution.Visible = tache.IsAttribuable(userLog) OrElse tache.IsDesattribuable(userLog)
        ' -- mode attribution
        If (tache.IsAttribuable(userLog)) Then
            BtnAttribution.Text = "M'attribuer la tâche"
        End If
        ' -- mode désattribution
        If tache.IsDesattribuable(userLog) Then
            BtnAttribution.Text = "Désattribuer la tâche"
            BtnAttribution.ForeColor = Color.DarkRed
        End If

        ' ---- annulation possible si je suis emetteur et tache pas encore attribuée
        BtnAnnulation.Visible = tache.IsAnnulable(userLog)
        ' ---- 
        BtnFixeRDV.Visible = tache.IsRendezVousAFixer(userLog)
        ' ---
        BtnEpidode.Visible = ((tache.Type = Tache.TypeTache.AVIS_EPISODE.ToString AndAlso tache.EpisodeId <> 0) OrElse (tache.IsUnRdv())) AndAlso userLog.IsFonctionIdPossible(tache.TraiteFonctionId)
        If tache.EpisodeId <> 0 Then
            BtnEpidode.ForeColor = Color.Red
            BtnEpidode.Font = New Font(BtnEpidode.Font, FontStyle.Bold)
        End If
        If tache.isUnRdv() Then
            BtnEpidode.Location = New Point(571, 51)
            BtnEpidode.Width = 148
        End If

    End Sub

    Private Sub metEnExergue(label As Label, txtBox As TextBox)
        label.ForeColor = Color.DarkRed
        txtBox.Enabled = True
        txtBox.ForeColor = Color.DarkRed
        txtBox.BackColor = Color.MistyRose
    End Sub

    Private Sub BtnPatient_Click(sender As Object, e As EventArgs) Handles BtnPatient.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Me.Enabled = False
            Using vFFPatientDetailEdit As New RadFPatientDetailEdit
                vFFPatientDetailEdit.SelectedPatientId = tacheBeanAssocie.Patient.patientId
                vFFPatientDetailEdit.SelectedPatient = tacheBeanAssocie.Patient
                vFFPatientDetailEdit.UtilisateurConnecte = userLog
                vFFPatientDetailEdit.ShowDialog() 'Modal
            End Using
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    Private Sub BtnSynthese_Click(sender As Object, e As EventArgs) Handles BtnSynthese.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Me.Enabled = False
            Using vFSynthese As New RadFSynthese
                vFSynthese.SelectedPatient = tacheBeanAssocie.Patient
                If tache.isUnRdv() Then
                    vFSynthese.RendezVousId = tache.Id
                End If
                vFSynthese.UtilisateurConnecte = userLog
                'Il est important d'appeler le Form en Modal, car on ne doit pas écraser les données du patient stockées en session et peut être déjà en cours d'utilisation
                vFSynthese.ShowDialog() 'Modal
                If vFSynthese.IsRendezVousCloture Then
                    IsActionEffectuee = True
                    Me.Close()
                End If
            End Using
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    Private Sub BtnDetailIntervenant_Click(sender As Object, e As EventArgs) Handles BtnDetailIntervenant.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Me.Enabled = False
            Using vFRorDetailEdit As New RadFRorDetailEdit
                vFRorDetailEdit.SelectedRorId = tacheBeanAssocie.Parcours.RorId
                vFRorDetailEdit.SelectedSpecialiteId = tacheBeanAssocie.Specialite.SpecialiteId
                vFRorDetailEdit.ShowDialog() 'Modal
            End Using
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub
    Private Sub BtnEpisode_Click(sender As Object, e As EventArgs) Handles BtnEpidode.Click

        Me.Cursor = Cursors.WaitCursor
        Try
            Me.Enabled = False
            If tache.isUnRdv() Then
                Dim episodeDao As EpisodeDao = New EpisodeDao
                IsActionEffectuee = episodeDao.CallEpisode(tacheBeanAssocie.Patient, tache.Id)
            Else
                Using vRadFEpisodeDetail As New RadFEpisodeDetail
                    vRadFEpisodeDetail.SelectedEpisodeId = tache.EpisodeId
                    vRadFEpisodeDetail.SelectedPatient = tacheBeanAssocie.Patient
                    vRadFEpisodeDetail.UtilisateurConnecte = userLog
                    vRadFEpisodeDetail.ShowDialog()
                    If vRadFEpisodeDetail.IsRendezVousCloture Then
                        IsActionEffectuee = True
                        Me.Close()
                    End If

                End Using
            End If
            ' -- si passage de la tache à l'etat final => on sort du formulaire
            Dim exEtat = tache.Etat
            tache = tacheDao.GetTacheById(tache.Id, True)
            If tache.Etat <> exEtat And tache.IsStatutFinal Then
                _isActionEffectuee1 = True
                Me.Close()
            End If
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try
    End Sub

    Private Sub BtnAnnulation_Click(sender As Object, e As EventArgs) Handles BtnAnnulation.Click
        Interaction.Beep()
        If MsgBox("Etes-vous sur de vouloir annuler cette tâche ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Annulation Tâche") = MsgBoxResult.Yes Then
            Try
                tacheDao.AnnulationTache(tache.Id)
                _isActionEffectuee1 = True
                Me.Close()
            Catch err As Exception
                MsgBox(err.Message)
                Return
            End Try
        End If
    End Sub

    Private Sub BtnAttribution_Click(sender As Object, e As EventArgs) Handles BtnAttribution.Click
        ' --- mode attribution
        If tache.IsAttribuable(userLog) Then
            Try
                If tacheDao.AttribueTacheToUserLog(tache.Id) Then
                    InitControls(tache.Id)
                    IsActionEffectuee = True
                    'Me.Close()
                End If
            Catch err As Exception
                MsgBox(err.Message)
                Return
            End Try
        Else
            ' --- mode désattribution
            Interaction.Beep()
            If MsgBox("Etes-vous sur de vouloir désattribuer cette tâche ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Désattribution Tâche") = MsgBoxResult.Yes Then
                Try
                    If tacheDao.desattribueTache(tache.Id) Then
                        IsActionEffectuee = True
                        Me.Close()
                    End If
                Catch err As Exception
                    MsgBox(err.Message)
                    Return
                End Try
            End If
        End If
    End Sub

    Private Sub RadButtonAbandon_Click(sender As Object, e As EventArgs) Handles RadButtonAbandon.Click
        Me.Close()
    End Sub

    Private Sub BtnFixeRDV_Click(sender As Object, e As EventArgs) Handles BtnFixeRDV.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Me.Enabled = False
            Using frmChoixDateHeure As New FrmChoixDateHeureDuree(tache.EmetteurCommentaire)
                frmChoixDateHeure.ShowDialog()
                If frmChoixDateHeure.DateChoisie <> Nothing Then
                    Dim dureeRendezVous As Integer
                    If IsNumeric(ConfigurationManager.AppSettings("dureeRendezVous")) Then
                        dureeRendezVous = ConfigurationManager.AppSettings("dureeRendezVous")
                    End If
                    '     Public Sub createRendezVous(patient As PatientBase, parcours As Parcours, typeTache As TypeTache, dateRDV As Date, duree As Integer, commentaire As String, Optional tacheParent As Tache = Nothing)
                    tacheDao.CreateRendezVous(tacheBeanAssocie.Patient, tacheBeanAssocie.Parcours, tache.GetTypeRdvFromDemande(), frmChoixDateHeure.DateChoisie, dureeRendezVous, frmChoixDateHeure.Commentaire, tache)
                    IsActionEffectuee = True
                    Close()
                End If
            End Using
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    Private Sub RadIdTache_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadIdTache.ToolTipTextNeeded
        e.ToolTipText = tache.Id
    End Sub
End Class
