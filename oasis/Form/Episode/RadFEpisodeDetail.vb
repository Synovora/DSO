Imports System.Collections.Specialized
Imports System.Configuration
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Oasis_Common
Imports Telerik.WinControls.UI.Docking

Public Class RadFEpisodeDetail
    Private _SelectedEpisodeId As Long
    Private _SelectedPatient As Patient
    Private _rendezVousId As Long
    Private _isRendezVousCloture As Boolean
    Private _UtilisateurConnecte As Utilisateur
    Private _ecranPrecedent As EnumAccesEcranPrecedent

    Public Property SelectedEpisodeId As Long
        Get
            Return _SelectedEpisodeId
        End Get
        Set(value As Long)
            _SelectedEpisodeId = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return _UtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            _UtilisateurConnecte = value
        End Set
    End Property

    Public Property RendezVousId As Long
        Get
            Return _rendezVousId
        End Get
        Set(value As Long)
            _rendezVousId = value
        End Set
    End Property

    Public Property IsRendezVousCloture As Boolean
        Get
            Return _isRendezVousCloture
        End Get
        Set(value As Boolean)
            _isRendezVousCloture = value
        End Set
    End Property

    Friend Property EcranPrecedent As EnumAccesEcranPrecedent
        Get
            Return _ecranPrecedent
        End Get
        Set(value As EnumAccesEcranPrecedent)
            _ecranPrecedent = value
        End Set
    End Property

    Dim episodeDao As New EpisodeDao
    Dim tacheDao As New TacheDao
    Dim fonctionDao As New FonctionDao
    Dim drcDao As New DrcDao
    Dim ordonnaceDao As New OrdonnanceDao
    'Dim parcoursDao As New ParcoursDao
    Dim ParcoursConsigneDao As New ParcoursConsigneDao
    Dim UserDao As New UserDao
    Dim drc As Drc
    Dim episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
    Dim episodeParametreDao As New EpisodeParametreDao
    Dim episodeActeParamedicalDao As New EpisodeActeParamedicalDao
    Dim episodeContexteDao As New EpisodeContexteDao
    Dim sousEpisodeDao As New SousEpisodeDao
    Dim theriaqueDao As New TheriaqueDao

    Dim antecedentChangementOrdreDao As New AntecedentChangementOrdreDao
    Dim antecedentAffectationDao As New AntecedentAffectationDao
    Dim antecedentDao As New AntecedentDao

    Dim log As Log
    Dim episode As Episode
    Dim tache As Tache
    Dim user As New Utilisateur
    Dim utilisateurHisto As Utilisateur = New Utilisateur()

    Dim ParcoursListProfilsOasis As New List(Of Integer)
    Dim ListeParametreExistant As New List(Of Long)

    Dim InitPublie, InitParPriorite, InitMajeur, InitContextePublie, InitParcoursNonCache As Boolean
    Dim PPSSuiviIdeExiste, PPSSuiviSageFemmeExiste, PPSSuiviMedecinExiste As Boolean
    Dim PatientAllergie, PatientContreIndication As Boolean
    Dim ObservationMedicaleModifie As Boolean = False
    Dim ObservationParamedicaleModifie As Boolean = False
    Dim IsTraitementLoaded As Boolean = False
    Dim IsParcoursLoaded As Boolean = False
    Dim IsContexteLoaded As Boolean = False
    Dim IsPPSLoaded As Boolean = False
    Dim FinChargementActesParamedicauxParamedical As Boolean = False
    Dim FinChargementActesParamedicauxMedical As Boolean = False
    Dim commentaireConclusionIdeModified As Boolean = False
    Dim RadioTypeConclusionIdeModified As Boolean = False
    Dim ChargementConclusionEnCours As Boolean

    Dim ControleProtocoleAiguExiste As Boolean
    Dim ControleActeParamedicalExiste As Boolean
    Dim ControleWorkflowEnCoursExistant As Boolean
    Dim ControleDemandeAvisMedicalExiste As Boolean
    Dim ControleConclusionMedicaleExiste As Boolean
    Dim ControleOrdonnanceValide As Boolean = False
    Dim ControleOrdonnanceExiste As Boolean = False

    Dim OptionWorkflow As TacheDao.EnumOptionWorkflow

    Dim ControleAjoutConclusion As Boolean = True

    Dim LongueurStringAllergie As Integer
    Dim drcIdConclusionIde As Integer

    Dim TypeEpisode, typeActiviteEpisode, typeProfilEpisode, DescriptionActiviteEpisode, CommentaireEpisode, UserCreation, DateCreation, UserModification, DateModification As String

    'Antécédent
    Dim iGridMax As Integer
    Dim NouveauOrdreAffichage As Integer
    Dim NiveauAntecedentAOrdonner As Integer
    Dim antecedentIdADeplacer, IndexAntecedentADeplacer As Integer

    'Saisie observations spécifiques
    Dim ObsRowCount As Integer

    'Contrôle accès épisode
    Dim RemoveEpisode As Boolean

    Private Sub RadFEpisodeDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Contrôle d'accès aux écran Synthèse, épisode et ligne de vie
        Environnement.ControleAccesForm.addFormToControl(EnumForm.EPISODE.ToString)
        If Environnement.ControleAccesForm.IsAccessToFormOK(EnumForm.LIGNE_DE_VIE.ToString) = False Then
            RadBtnLigneDeVie.Hide()
        End If
        If Environnement.ControleAccesForm.IsAccessToFormOK(EnumForm.SYNTHESE.ToString) = False Then
            RadBtnSynthèse.Hide()
        End If

        'Contrôle d'accès épisode
        If Environnement.ControleAccesEpisode.IsAccessToEpisodeOK(SelectedEpisodeId) Then
            Environnement.ControleAccesEpisode.AddEpisodeToControl(SelectedEpisodeId)
            RemoveEpisode = True
        Else
            MessageBox.Show("Accès interdit, cet épisode est déjà ouvert !", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Error)
            RemoveEpisode = False
            Close()
        End If

        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        Dim actiondao As New ActionDao
        Dim action As New Action
        action.UtilisateurId = userLog.UtilisateurId
        action.PatientId = SelectedPatient.patientId
        action.Fonction = ActionDao.EnumFonctionCode.EPISODE
        action.FonctionId = Me.SelectedEpisodeId
        action.Action = "Accès épisode patient n° " & SelectedEpisodeId
        actiondao.CreationAction(action)

        RadBtnUp.Text = Char.ConvertFromUtf32(8593)
        RadBtnDown.Text = Char.ConvertFromUtf32(8595)
        RadBtnRight.Text = Char.ConvertFromUtf32(8594)
        RadBtnLeft.Text = Char.ConvertFromUtf32(8592)

        InitParametre()
        LblTypeEpisode.Text = ""
        LblTypeProfil.Text = ""
        LblPatientNIR.Text = ""
        LblPatientPrenom.Text = ""
        LblPatientNom.Text = ""
        LblPatientAge.Text = ""
        LblPatientGenre.Text = ""
        LblPatientSite.Text = ""
        LblPatientDateMaj.Text = ""
        LblLabelEtatEpisode.Text = ""

        afficheTitleForm(Me, "Détail épisode")

        RadGridViewObsIde.TableElement.RowHeight = 35
        RadGridViewObsMed.TableElement.RowHeight = 35

        SplitPanelObservation.SizeInfo.SizeMode = SplitPanelSizeMode.Absolute
        SplitPanelObservation.SizeInfo.AbsoluteSize = New Size(0, 450)

        SplitPanelSyntheseBouton.SizeInfo.SizeMode = SplitPanelSizeMode.Absolute
        SplitPanelSyntheseBouton.SizeInfo.AbsoluteSize = New Size(0, 40)

        RadBtnWorkflowIde.Hide()
        RadBtnWorkflowMed.Hide()

        ClotureAutomatiqueRendezVous()
        ChargementParametreApplication()
        initZones()
        ChargementEtatCivil()

        'Episode
        ChargementCaracteristiquesEpisode()
        DroitAcces()

        ChargementAffichageBlocWorkflow()
        If episode.TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.SOCIAL Or episode.Type = EpisodeDao.EnumTypeEpisode.VIRTUEL.ToString Then
            SplitPanelObsMedProtocole.Hide()
            Me.RadSplitContainerObsMed.MoveSplitter(Me.RadSplitContainerObsMed.Splitters(0), RadDirection.Up)
            SplitPanelObsIdeProtocole.Hide()
            Me.RadSplitContainerObsIde.MoveSplitter(Me.RadSplitContainerObsIde.Splitters(0), RadDirection.Up)
            RadObsSpeIdeDataGridView.Hide()
            RadBtnParametre.Hide()
            RadGbxParametre.Text = ""
        Else
            ChargementObservationSpecifique()
            ChargementParametres()
        End If

        ChargementSousEpisode()

        ChargementObservationLibre()
        ChargementConclusion()

        'Vérification si l'épisode est clôturé et donc non modifiable
        ControleEpisodeCloture()

        'Synthèse, chargement de la page par défaut
        ChargementAntecedent()

        refreshButtonSousEpisodeProperties()

        Cursor.Current = Cursors.Default
    End Sub



    '=========================================================
    '=== Traitement des rendez-vous
    '=========================================================

    Private Sub ClotureAutomatiqueRendezVous()
        Me.IsRendezVousCloture = False
        If Me.RendezVousId <> 0 Then
            Dim tacheRendezVous As Tache
            tacheRendezVous = tacheDao.getTacheById(RendezVousId)
            If tacheRendezVous.isUnRdv Then
                'Controler que l'utilisateur est celui qui s'est attribué la tâche
                If tacheRendezVous.isMyTacheATraiter = False Then
                    If tacheRendezVous.isAttribuable Then
                        'Tâche non encore attribuée et l'utilisateur n'est pas encore propriétaire du rendez-vous, proposition d'attribution de la tâche à l'utilisateur
                        If MsgBox("Vous allez vous attribuer le traitement du rendez-vous qui sera honoré à la sortie de l'épisode. Confirmez-vous son attribution", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                            Try
                                tacheDao.attribueTacheToUserLog(tacheRendezVous.Id)
                            Catch ex As Exception
                                MessageBox.Show(ex.ToString)
                                Exit Sub
                            End Try
                        Else
                            Exit Sub
                        End If
                    Else
                        Exit Sub
                    End If
                End If
                Try
                    If tacheDao.ClotureTache(RendezVousId, True) = True Then
                        Me.IsRendezVousCloture = True
                        'Généreration automatique d'une demande de rendez-vous suite à la cloture du rendez-vous en cours
                        GenerationDemandeRendezVous(tacheRendezVous)
                    End If
                Catch ex As Exception
                    If ex.ToString.StartsWith("Collision") Then
                        Dim Description As String = "Traitement annulé, le rendez-vous (tâche n° " & Me.RendezVousId.ToString & ") était déjà honoré"
                        CreateLog(Description, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
                    Else
                        MessageBox.Show(ex.ToString)
                        Dim Description As String = "Le traitement pour honorer le rendez-vous (tâche n° " & Me.RendezVousId.ToString & ") a échouée"
                        CreateLog(Description, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
                    End If
                End Try
            End If
        Else
            'Si le rendez-vous n'a pas été communiqué à l'épisode, on recherche si un rendez-vous en attente existe pour la fonction de l'utilisateur avec une date <= date du jour
            Dim tacheRendezVous As Tache
            tacheRendezVous = tacheDao.GetProchainRendezVousOasisByPatientIdEtEpisode(SelectedPatient.patientId, userLog.TypeProfil)
            'Si RDV Oasis existe et que la date du DRV est <= date du jour
            If tacheRendezVous.Id <> 0 AndAlso tacheRendezVous.DateRendezVous.Date <= Date.Now.Date Then
                If tacheRendezVous.isAttribuable Then
                    Dim fonctiondao As New FonctionDao
                    Dim fonction As Fonction
                    fonction = fonctiondao.getFonctionById(tacheRendezVous.DestinataireFonctionId)
                    'Tâche non encore attribuée et l'utilisateur n'est pas encore propriétaire du rendez-vous, proposition d'attribution de la tâche à l'utilisateur
                    Dim message As String = "Un rendez-vous de type '" & userLog.TypeProfil & ", destiné à la fonction " & fonction.Designation &
                        "', a été programmé pour le " & tacheRendezVous.DateRendezVous.ToString("dd.MM.yyyy") &
                        " pour ce patient." & vbCrLf & vbCrLf &
                        "Le rendez-vous sera automatiquement déclaré honoré après confirmation de cette notification." & vbCrLf & vbCrLf &
                        "Si vous ne confirmez pas cette notification, le rendez-vous restera en attente" & vbCrLf & vbCrLf &
                        "Confirmation de déclarer le rendez-vous honoré ?"
                    If MsgBox(message, MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                        Try
                            tacheDao.attribueTacheToUserLog(tacheRendezVous.Id)
                        Catch ex As Exception
                            If ex.ToString.StartsWith("Collision") Then
                                Dim Description As String = "Attribution annulé, le rendez-vous (tâche n° " & Me.RendezVousId.ToString & ") était déjà honoré"
                                CreateLog(Description, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
                                MessageBox.Show(Description)
                            Else
                                Dim Description As String = "L'attribution du rendez-vous (tâche n° " & Me.RendezVousId.ToString & ") a échouée" & ex.ToString
                                CreateLog(Description, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
                                MessageBox.Show(Description)
                            End If
                            MessageBox.Show(ex.ToString)
                            Exit Sub
                        End Try
                    Else
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
                ClotureRendezVous(tacheRendezVous)
                'Généreration automatique d'une demande de rendez-vous suite à la cloture du rendez-vous en cours
                GenerationDemandeRendezVous(tacheRendezVous)
            End If
        End If
    End Sub

    Private Sub GenerationDemandeRendezVous(tacheRendezVous As Tache)
        Dim parcoursId As Long = tacheRendezVous.ParcoursId
        If parcoursId <> 0 Then
            Dim parcoursDao As New ParcoursDao
            Dim parcours As Parcours = parcoursDao.getParcoursById(tacheRendezVous.ParcoursId)
            If parcours.Rythme <> 0 Then
                If tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, parcours, tacheRendezVous.DateRendezVous.Date) = True Then
                    Me.RadDesktopAlert1.CaptionText = "Notification demande de rendez-vous"
                    Me.RadDesktopAlert1.ContentText = "Une demande de rendez-vous a été automatiquement générée pour cet intervenant"
                    Me.RadDesktopAlert1.Show()
                End If
            End If
        End If
    End Sub

    Private Sub ClotureRendezVous(tacheRendezVous As Tache)
        Try
            If tacheDao.ClotureTache(tacheRendezVous.Id, True) = True Then
                Dim form As New RadFNotification()
                form.Titre = "Rendez-vous patient honoré"
                form.Message = "Le rendez-vous de type '" & userLog.TypeProfil & "', programmé le " & tacheRendezVous.DateRendezVous.ToString("dd.MM.yyyy") & " est honoré"
                form.Show()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Dim Description As String = "La cloture du rendez-vous n° " & Me.RendezVousId.ToString & ") a échouée, information système : " & ex.ToString
            CreateLog(Description, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
        End Try
    End Sub


    '=========================================================
    '=== Etat civil patient
    '=========================================================

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = Me.SelectedPatient.PatientNir
        LblPatientPrenom.Text = Me.SelectedPatient.PatientPrenom
        LblPatientNom.Text = Me.SelectedPatient.PatientNom
        LblPatientAge.Text = Me.SelectedPatient.PatientAge
        LblDateNaissance.Text = Me.SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
        LblPatientGenre.Text = Me.SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(Me.SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = Me.SelectedPatient.PatientSyntheseDateMaj.ToString("dd.MM.yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        ChargementToolTipAld()

        'Contre-indication
        GetContreIndication()

        'Allergie
        GetAllergie()
    End Sub

    Private Sub GetContreIndication()
        Dim StringContreIndicationToolTip As String = PatientDao.GetStringContreIndicationByPatient(SelectedPatient.patientId)
        If StringContreIndicationToolTip = "" Then
            LblContreIndication.Hide()
            PatientContreIndication = False
            ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = False
        Else
            LblContreIndication.Show()
            ToolTip.SetToolTip(LblContreIndication, StringContreIndicationToolTip)
            PatientContreIndication = True
            ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub GetAllergie()
        Dim StringAllergieToolTip As String = PatientDao.GetStringAllergieByPatient(SelectedPatient.patientId)
        If StringAllergieToolTip = "" Then
            PatientAllergie = False
            LblAllergie.Hide()
            LblSubstance.Hide()
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = False
        Else
            PatientAllergie = True
            LblAllergie.Show()
            LblSubstance.Hide()
            'LblSubstance.Text = StringAllergieToolTip.Replace(vbCrLf, ", ")
            ToolTip.SetToolTip(LblAllergie, StringAllergieToolTip)
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = True
        End If
    End Sub

    '=========================================================
    '=== Caractéristiques épisode
    '=========================================================

    'Caractéristiques épisode
    Private Sub ChargementCaracteristiquesEpisode()
        episode = episodeDao.GetEpisodeById(Me.SelectedEpisodeId)

        If episode.Type = EpisodeDao.EnumTypeEpisode.VIRTUEL.ToString Then
            If episode.DescriptionActivite = "" Then
                LblTypeEpisode.Text = episode.Type.Trim
            Else
                LblTypeEpisode.Text = episode.Type.Trim & " / " & episode.DescriptionActivite
            End If
        Else
            If episode.DescriptionActivite = "" Then
                LblTypeEpisode.Text = episode.Type.Trim & " / " & episode.TypeActivite
            Else
                LblTypeEpisode.Text = episode.Type.Trim & " / " & episode.TypeActivite & " / " & episode.DescriptionActivite
            End If
        End If

        TypeEpisode = episode.Type
        typeActiviteEpisode = episode.TypeActivite
        DescriptionActiviteEpisode = episode.DescriptionActivite
        typeProfilEpisode = episode.TypeProfil
        LblTypeProfil.Text = episode.TypeProfil
        CommentaireEpisode = episode.Commentaire

        Dim userDao As New UserDao
        user = userDao.getUserById(episode.UserCreation)
        'UtilisateurDao.SetUtilisateur(user, episode.UserCreation)
        UserCreation = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
        DateCreation = episode.DateCreation.ToString("dd/MM/yyyy HH:mm")
        DateModification = episode.DateModification.ToString("dd/MM/yyyy HH:mm")

        If episode.UserModification <> 0 Then
            user = userDao.getUserById(episode.UserModification)
            'UtilisateurDao.SetUtilisateur(user, episode.UserModification)
            UserModification = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
        End If

        ChargementEtatEpisode()
    End Sub

    Private Sub ChargementEtatEpisode()
        Dim DateValidationOrdonnance As Date = Nothing
        Dim DateCreationOrdonnance As Date = Nothing
        Dim dt As DataTable
        dt = ordonnaceDao.getOrdonnanceValidebyPatient(SelectedPatient.patientId, SelectedEpisodeId)
        If dt.Rows.Count > 0 Then
            OrdonnanceToolStripMenuItem.ForeColor = Color.Red
            'RadPageView1.Pages(1).Item.ForeColor = Color.Orange
            RadPageView1.Pages(1).Item.DrawFill = True
            RadPageView1.Pages(1).Item.BackColor = Color.LightSalmon
            RadPageView1.Pages(1).Item.GradientStyle = GradientStyles.Solid
            RadBtnOrdonnance.BackColor = Color.LightSalmon
            ToolTip.SetToolTip(RadBtnOrdonnance, "Ordonnance existante en attente de validation médicale")
            If dt.Rows.Count > 0 Then
                ControleOrdonnanceExiste = True
                DateValidationOrdonnance = Coalesce(dt.Rows(0)("oa_ordonnance_date_validation"), Nothing)
                DateCreationOrdonnance = Coalesce(dt.Rows(0)("oa_ordonnance_date_creation"), Nothing)
                If DateValidationOrdonnance <> Nothing Then
                    ControleOrdonnanceValide = True
                    'RadPageView1.Pages(1).Item.ForeColor = Color.Red
                    RadPageView1.Pages(1).Item.DrawFill = True
                    RadPageView1.Pages(1).Item.BackColor = Color.LightGreen
                    RadPageView1.Pages(1).Item.GradientStyle = GradientStyles.Solid
                    RadBtnOrdonnance.BackColor = Color.LightGreen
                    ToolTip.SetToolTip(RadBtnOrdonnance, "Ordonnance existante et valide (signature médicale)")
                Else
                    ControleOrdonnanceValide = False
                End If
            End If
        Else
            OrdonnanceToolStripMenuItem.ForeColor = Color.Black
            'RadPageView1.Pages(1).Item.ForeColor = Color.Black
            RadPageView1.Pages(1).Item.DrawFill = True
            RadPageView1.Pages(1).Item.BackColor = Color.FromArgb(191, 219, 255)
            RadPageView1.Pages(1).Item.GradientStyle = GradientStyles.Solid
            RadBtnOrdonnance.BackColor = Color.FromArgb(233, 240, 249)
            ToolTip.SetToolTip(RadBtnOrdonnance, "Création ordonnance")
            ControleOrdonnanceExiste = False
            ControleOrdonnanceValide = False
        End If

        Select Case episode.Etat
            Case EpisodeDao.EnumEtatEpisode.EN_COURS.ToString
                If ControleOrdonnanceExiste = True Then
                    If ControleOrdonnanceValide = True Then
                        LblLabelEtatEpisode.Text = "EPISODE EN COURS - ORDONNANCE VALIDEE LE " & DateValidationOrdonnance.ToString("dd.MM.yyyy hh:mm")
                    Else
                        LblLabelEtatEpisode.Text = "EPISODE EN COURS - ORDONNANCE CREEE LE " & DateCreationOrdonnance.ToString("dd.MM.yyyy hh:mm") & ", EN ATTENTE DE VALIDATION !"
                    End If
                Else
                    LblLabelEtatEpisode.Text = "Episode en cours"
                End If
            Case EpisodeDao.EnumEtatEpisode.CLOTURE.ToString
                If episode.DateModification.Date < Date.Now.Date Then
                    LblLabelEtatEpisode.Text = "EPISODE CLOTURE LE " & episode.DateModification.ToString("dd.MM.yyyy") & " (non modifiable, hormis l'ajout de pièces dans les sous-épisodes)"
                Else
                    LblLabelEtatEpisode.Text = "EPISODE CLOTURE AUJOURD'HUI (Modification possible pour la journée en cours)"
                End If
            Case Else
                LblLabelEtatEpisode.Text = "Etat inconnu !"
        End Select
    End Sub

    Private Sub RadGroupBox3_MouseHover(sender As Object, e As EventArgs) Handles RadGroupBox3.MouseHover
        AfficheTooltipCaracetristiqueEpisode(sender)
    End Sub

    Private Sub LblTypeEpisode_MouseHover(sender As Object, e As EventArgs) Handles LblTypeEpisode.MouseHover
        AfficheTooltipCaracetristiqueEpisode(sender)
    End Sub

    Private Sub AfficheTooltipCaracetristiqueEpisode(sender As Object)
        ToolTip.SetToolTip(sender, ConstitutionNotification())

    End Sub

    Private Sub LblTypeEpisode_Click(sender As Object, e As EventArgs) Handles LblTypeEpisode.Click
        Dim form As New RadFNotification()
        form.Titre = "Caractéristique épisode patient"
        form.Message = ConstitutionNotification()
        form.Show()
    End Sub

    Private Function ConstitutionNotification() As String
        Dim message As String

        message = "Episode # " & Me.SelectedEpisodeId.ToString() & vbCrLf &
                            "Type : " & TypeEpisode & vbCrLf &
                            "Type activité : " & typeActiviteEpisode & " " & DescriptionActiviteEpisode & vbCrLf &
                            "Commentaire : " & CommentaireEpisode & vbCrLf &
                            "Créé par : " & UserCreation & " (" & typeProfilEpisode & ")" & " Le : " & DateCreation & vbCrLf &
                            "Modifié par : " & UserModification & " Le : " & DateModification & vbCrLf &
                            "Temps écoulé depuis sa création (" & outils.CalculDureeEnJourEtHeureString(DateCreation, Date.Now) & ")" & vbCrLf &
                            LblLabelEtatEpisode.Text
        Return message
    End Function


    '====================================================================================================================================
    '=== Paramètres
    '====================================================================================================================================

    Private Sub InitParametre()
        LblLabelPoids.Text = ""
        LblParmPoids.Text = ""
        LblLabelTemperature.Text = ""
        LblParmTemperature.Text = ""
        LblLabelFC.Text = ""
        LblParmFC.Text = ""
        LblLabelFR.Text = ""
        LblParmFR.Text = ""

        LblLabelTaille.Text = ""
        LblParmTaille.Text = ""
        LblLabelDextro.Text = ""
        LblParmDextro.Text = ""
        LblLabelPAS.Text = ""
        LblParmPAS.Text = ""
        LblLabelSat.Text = ""
        LblParmSat.Text = ""

        LblLabelIMC.Text = ""
        LblParmIMC.Text = ""
        LblLabelObservance.Text = ""
        LblParmObservance.Text = ""
        LblLabelPAD.Text = ""
        LblParmPAD.Text = ""
        LblLabelDEP.Text = ""
        LblParmDEP.Text = ""

        LblLabelPerimetreCranien.Text = ""
        LblParmPerimetreCranien.Text = ""
        LblLabelINR.Text = ""
        LblParmINR.Text = ""
        LblLabelPAM.Text = ""
        LblParmPAM.Text = ""
        LblLabelEVA.Text = ""
        LblParmEVA.Text = ""

        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        LblParametre4.Text = ""
    End Sub


    'Chargement des paramètres
    Private Sub ChargementParametres()
        Dim ValeurParametreNonSaisie As Boolean = False
        Dim parmDataTable As DataTable
        parmDataTable = episodeParametreDao.getAllParametreEpisodeByEpisodeId(SelectedEpisodeId)
        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = parmDataTable.Rows.Count - 1
        Dim entier, nombreDecimal, longueurString, idString As Integer
        Dim Valeur, ValeurIMC, ValeurPAM As Decimal
        Dim description, unite, valeurString, parametreString As String
        Dim ParametreId, EpisodeParametreId, EpisodeParametreIdIMC, EpisodeParametreIdPAM As Long
        Dim valeurPoids, valeurTaille, valeurPAS, valeurPAD As Decimal
        Dim uniteIMC, unitePAM As String
        Dim IMCaCalculer As Boolean = False
        Dim PAMaCalculer As Boolean = False

        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        uniteIMC = ""
        unitePAM = ""
        longueurString = 0
        idString = 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Valeur = parmDataTable.Rows(i)("valeur")
            If Valeur = 0 Then
                ValeurParametreNonSaisie = True
            End If
            description = parmDataTable.Rows(i)("description")
            entier = parmDataTable.Rows(i)("entier")
            nombreDecimal = parmDataTable.Rows(i)("decimal")
            unite = parmDataTable.Rows(i)("unite")
            ParametreId = parmDataTable.Rows(i)("parametre_id")
            EpisodeParametreId = parmDataTable.Rows(i)("episode_parametre_id")
            valeurString = ""

            If ParametreId = 2 Then
                If Valeur = 0 Then
                    Valeur = SelectedPatient.Taille
                End If
            End If

            Select Case entier
                Case 1
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("0")
                        Case 1
                            valeurString = Valeur.ToString("0.0")
                        Case 2
                            valeurString = Valeur.ToString("0.00")
                        Case 3
                            valeurString = Valeur.ToString("0.000")
                    End Select
                Case 2
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("#0")
                        Case 1
                            valeurString = Valeur.ToString("#0.0")
                        Case 2
                            valeurString = Valeur.ToString("#0.00")
                        Case 3
                            valeurString = Valeur.ToString("#0.000")
                    End Select
                Case 3
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("##0")
                        Case 1
                            valeurString = Valeur.ToString("##0.0")
                        Case 2
                            valeurString = Valeur.ToString("##0.00")
                        Case 3
                            valeurString = Valeur.ToString("##0.000")
                    End Select
            End Select

            Select Case ParametreId
                Case ParametreDao.enumParametreId.POIDS
                    LblLabelPoids.Text = "Poids"
                    LblParmPoids.Text = valeurString & " " & unite
                    valeurPoids = Valeur
                Case ParametreDao.enumParametreId.TAILLE
                    LblLabelTaille.Text = "Taille"
                    LblParmTaille.Text = valeurString & " " & unite
                    valeurTaille = Valeur
                    If valeurTaille = 0 Then
                        valeurTaille = SelectedPatient.Taille
                    End If
                Case ParametreDao.enumParametreId.IMC
                    LblLabelIMC.Text = "IMC"
                    uniteIMC = unite
                    EpisodeParametreIdIMC = EpisodeParametreId
                    ValeurIMC = Valeur
                    IMCaCalculer = True
                Case 4
                    LblLabelPerimetreCranien.Text = "Per. cranien"
                    LblParmPerimetreCranien.Text = valeurString & " " & unite
                Case 5
                    LblLabelFC.Text = "FC"
                    LblParmFC.Text = valeurString & " " & unite
                Case ParametreDao.enumParametreId.PAS
                    LblLabelPAS.Text = "PAS"
                    LblParmPAS.Text = valeurString & " " & unite
                    valeurPAS = Valeur
                Case ParametreDao.enumParametreId.PAD
                    LblLabelPAD.Text = "PAD"
                    LblParmPAD.Text = valeurString & " " & unite
                    valeurPAD = Valeur
                Case ParametreDao.enumParametreId.PAM
                    LblLabelPAM.Text = "PAM"
                    unitePAM = unite
                    EpisodeParametreIdPAM = EpisodeParametreId
                    ValeurPAM = Valeur
                    PAMaCalculer = True
                Case 9
                    LblLabelFR.Text = "FR"
                    LblParmFR.Text = valeurString & " " & unite
                Case 10
                    LblLabelSat.Text = "Sat"
                    LblParmSat.Text = valeurString & " " & unite
                Case 11
                    LblLabelDEP.Text = "DEP"
                    LblParmDEP.Text = valeurString & " " & unite
                Case 12
                    LblLabelDextro.Text = "Dextro"
                    LblParmDextro.Text = valeurString & " " & unite
                Case 15
                    LblLabelINR.Text = "INR"
                    LblParmINR.Text = valeurString & " " & unite
                Case 16
                    LblLabelEVA.Text = "EVA"
                    LblParmEVA.Text = valeurString & " " & unite
                Case 17
                    LblLabelObservance.Text = "Observance"
                    LblParmObservance.Text = valeurString & " " & unite
                Case 20
                    LblLabelTemperature.Text = "Temperature"
                    LblParmTemperature.Text = valeurString & " " & unite
                Case Else
                    parametreString = "          " & description & " : " & valeurString & " " & unite
                    longueurString += parametreString.Length
                    Select Case idString
                        Case 1
                            If LblParametre1.Text = "" Then
                                LblParametre1.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre1.Text += parametreString
                            End If
                            idString = 2
                        Case 2
                            If LblParametre2.Text = "" Then
                                LblParametre2.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre2.Text += parametreString
                            End If
                            idString = 3
                        Case 3
                            If LblParametre3.Text = "" Then
                                LblParametre3.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre3.Text += parametreString
                            End If
                            idString = 4
                        Case 4
                            If LblParametre4.Text = "" Then
                                LblParametre4.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre4.Text += parametreString
                            End If
                            idString = 1
                    End Select
            End Select
        Next

        '20/02/2020 - BGA - Si la taille n'est pas traitée et qu'elle est stockée dans la table Patient on l'affiche à l'écran
        If LblLabelTaille.Text = "" Then
            Dim parametre As Parametre
            Dim parametreDao As New ParametreDao
            parametre = parametreDao.GetParametreById(ParametreDao.enumParametreId.TAILLE) 'Taille
            LblLabelTaille.Text = "Taille"
            valeurString = SelectedPatient.Taille.ToString("##0")
            LblParmTaille.Text = valeurString & " " & parametre.Unite
        End If

        If IMCaCalculer = True Then
            If valeurTaille = 0 Then
                valeurTaille = SelectedPatient.Taille
            End If
            If valeurPoids <> 0 And valeurTaille <> 0 Then
                Dim ValeurCalcul As Decimal = valeurPoids / ((valeurTaille * valeurTaille) / 10000)
                Dim ValeurCalculAComparer As Decimal = Decimal.Round(ValeurCalcul, 3)
                'Mise à jour du paramètre déduit
                If ValeurIMC <> ValeurCalculAComparer Then
                    ValeurIMC = ValeurCalculAComparer
                    episodeParametreDao.ModificationValeurEpisodeParametre(EpisodeParametreIdIMC, ValeurIMC)
                End If
            Else
                Valeur = 0
            End If
            valeurString = ValeurIMC.ToString("#0.0")
            LblParmIMC.Text = valeurString & " " & uniteIMC
        End If

        If PAMaCalculer = True Then
            If valeurPAD <> 0 And valeurPAS <> 0 Then
                Dim ValeurCalcul As Decimal = (valeurPAS + (2 * valeurPAD)) / 3
                Dim ValeurCalculAComparer As Decimal = Decimal.Round(ValeurCalcul, 3)
                'Mise à jour du paramètre déduit
                If ValeurPAM <> ValeurCalculAComparer Then
                    ValeurPAM = ValeurCalculAComparer
                    episodeParametreDao.ModificationValeurEpisodeParametre(EpisodeParametreIdPAM, ValeurPAM)
                End If
            Else
                Valeur = 0
            End If
            valeurString = ValeurPAM.ToString("##0")
            LblParmPAM.Text = valeurString & " " & unitePAM
        End If

        If ValeurParametreNonSaisie = True Then
            RadBtnParametre.ForeColor = Color.Red
            RadBtnParametre.Font = New Font(RadBtnParametre.Font, FontStyle.Bold)
            ToolTip.SetToolTip(RadBtnParametre, "Des paramètres requis ne sont pas saisis")
        Else
            RadBtnParametre.ForeColor = Color.FromArgb(21, 66, 139)
            RadBtnParametre.Font = New Font(RadBtnParametre.Font, FontStyle.Regular)
            ToolTip.SetToolTip(RadBtnParametre, "")
        End If

    End Sub

    'Appel saisie des paramètres
    Private Sub RadBtnParametre_Click(sender As Object, e As EventArgs) Handles RadBtnParametre.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vRadFParametreDetailEdit As New RadFEpisodeParametreDetailEdit
            vRadFParametreDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
            vRadFParametreDetailEdit.SelectedPatient = Me.SelectedPatient
            vRadFParametreDetailEdit.ShowDialog()
            If vRadFParametreDetailEdit.CodeRetour = True Then
                InitParametre()
                ChargementParametres()
            End If
        End Using
        Me.Enabled = True
    End Sub

    '====================================================================================================================================
    '=== Sous-épisodes
    '====================================================================================================================================
    Private Sub ChargementSousEpisode()
        Dim dt As DataTable
        dt = sousEpisodeDao.getTableSousEpisode(SelectedEpisodeId,, True)

        RadGridViewSousEpisode.Rows.Clear()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewSousEpisode.Rows.Add(iGrid)
            RadGridViewSousEpisode.Rows(iGrid).Cells("id").Value = dt.Rows(i)("id")
            RadGridViewSousEpisode.Rows(iGrid).Cells("sousType").Value = dt.Rows(i)("sous_type_libelle")
            RadGridViewSousEpisode.Rows(iGrid).Cells("CreateUser").Value = dt.Rows(i)("user_create")
            RadGridViewSousEpisode.Rows(iGrid).Cells("LastUpdateUser").Value = dt.Rows(i)("user_update")
            RadGridViewSousEpisode.Rows(iGrid).Cells("ValidateUser").Value = dt.Rows(i)("user_validate")
            RadGridViewSousEpisode.Rows(iGrid).Cells("isSigne").Value = Not IsDBNull(dt.Rows(i)("horodate_validate"))
            RadGridViewSousEpisode.Rows(iGrid).Cells("isReponseRecue").Value = Coalesce(dt.Rows(i)("is_reponse_recue"), False)
            If RadGridViewSousEpisode.Rows(iGrid).Cells("isReponseRecue").Value = True Then
                'Vert
                RadGridViewSousEpisode.Rows(iGrid).Cells("sousType").Style.ForeColor = Color.Green
            Else
                If RadGridViewSousEpisode.Rows(iGrid).Cells("isSigne").Value = True Then
                    'Noir

                Else
                    'Rouge
                    RadGridViewSousEpisode.Rows(iGrid).Cells("sousType").Style.ForeColor = Color.Red
                End If
            End If
            ' -- on garnit le tag pour affichage tooltip
            RadGridViewSousEpisode.Rows(iGrid).Tag = "Créé le " & dt.Rows(i)("horodate_creation") & " par " & dt.Rows(i)("user_create") & vbCrLf &
                        If(IsDBNull(dt.Rows(i)("horodate_last_update")), "Non modifié.", "Modifié le " & dt.Rows(i)("horodate_last_update") & " par " & dt.Rows(i)("user_update")) & vbCrLf &
                        If(IsDBNull(dt.Rows(i)("horodate_validate")), "Non Signé.", "Signé le " & dt.Rows(i)("horodate_validate") & " par " & dt.Rows(i)("user_validate")) & vbCrLf &
                        If(Coalesce(dt.Rows(i)("is_ald"), False), " ... ALD" & vbCrLf, "") &
                        If(Coalesce(dt.Rows(i)("is_reponse"), False) AndAlso Coalesce(dt.Rows(i)("is_reponse_recue"), False) = False,
                                         " ... Résultat requis sous " & dt.Rows(i)("delai_since_validation") & " j à partir de la date de signature" & vbCrLf, "") &
                        If(Coalesce(dt.Rows(i)("is_reponse_recue"), False) AndAlso Coalesce(dt.Rows(i)("is_reponse"), False),
                                        " ... Dernier résultat reçu le  " & dt.Rows(i)("horodate_last_recu"),
                                        If(Coalesce(dt.Rows(i)("is_reponse"), False), " ... Résultat NON reçu ..." & vbCrLf, ""))
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewSousEpisode.Rows.Count > 0 Then
            RadGridViewSousEpisode.CurrentRow = RadGridViewSousEpisode.ChildRows(0)
            RadGridViewSousEpisode.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub RadGridViewSousEpisode_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewSousEpisode.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            Try
                If e.Row.Tag <> Nothing Then e.CellElement.ToolTipText = e.Row.Tag
            Catch ex As Exception

            End Try
        End If
        ' --- on enleve le carre des checkbox
        Dim checkBoxCell As GridCheckBoxCellElement = TryCast(e.CellElement, GridCheckBoxCellElement)
        If checkBoxCell IsNot Nothing Then
            Dim editor As RadCheckBoxEditor = TryCast(checkBoxCell.Editor, RadCheckBoxEditor)
            Dim element As RadCheckBoxEditorElement = TryCast(editor.EditorElement, RadCheckBoxEditorElement)
            element.Checkmark.Border.Visibility = ElementVisibility.Collapsed
            element.Checkmark.Fill.Visibility = ElementVisibility.Collapsed
        End If
    End Sub

    'Détail sous-épisode
    Private Sub RadGridViewSousEpisode_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewSousEpisode.CellDoubleClick
        If Me.RadGridViewSousEpisode.Rows.Count = 0 OrElse Me.RadGridViewSousEpisode.CurrentRow.IsSelected = False Then Return

        Dim sousEpisode As SousEpisode
        Try
            Me.Cursor = Cursors.WaitCursor
            sousEpisode = sousEpisodeDao.getById(Me.RadGridViewSousEpisode.CurrentRow.Cells("Id").Value)
        Catch err As Exception
            MsgBox(err.Message())
            Return
        Finally
            Me.Cursor = Cursors.Default
        End Try

        With RadGridViewSousEpisode.CurrentRow
            FicheSousEpisode(sousEpisode, .Cells("CreateUser").Value, .Cells("LastUpdateUser").Value, .Cells("ValidateUser").Value)
        End With
    End Sub

    Private Sub FicheSousEpisode(sousEpisode As SousEpisode, userCreateNom As String, userUpdateNom As String, userValidateNom As String)
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using frm = New FrmSousEpisode(episode, SelectedPatient, sousEpisode, userCreateNom, userUpdateNom, userValidateNom)
                frm.ShowDialog()
                frm.Dispose()
            End Using
            ChargementSousEpisode()
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    '====================================================================================================================================
    '=== Observations spécifiques
    '====================================================================================================================================
    Private Sub ChargementObservationSpecifique()
        ControleProtocoleAiguExiste = False
        ControleActeParamedicalExiste = False
        ChargementEpisodeActesParamedicauxParamedical()
        ChargementEpisodeActesParamedicauxMedical()
        'Vérification si le type de conclusion IDE n'a pas changé (pour les épisodes de type PARAMEDICAL)
        ControleMAJTypeConclusionIDE()
    End Sub


    '=========================================================
    '=== Observations spécifiques (paramédical)
    '=========================================================
    'Chargement des actes paramédicaux associés à l'épisode
    Private Sub ChargementEpisodeActesParamedicauxParamedical(Optional Index As Integer = -1)
        FinChargementActesParamedicauxParamedical = False
        Dim acteParamedicalDataTable As DataTable
        acteParamedicalDataTable = episodeActeParamedicalDao.getAllEpisodeActeParamedicalByEpisodeId(SelectedEpisodeId, ProfilDao.EnumProfilType.PARAMEDICAL.ToString)

        RadObsSpeIdeDataGridView.Rows.Clear()

        Dim iGrid As Integer = -1
        ObsRowCount = acteParamedicalDataTable.Rows.Count - 1
        If acteParamedicalDataTable.Rows.Count > 0 Then
            ControleActeParamedicalExiste = True
        End If
        For i = 0 To ObsRowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadObsSpeIdeDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadObsSpeIdeDataGridView.Rows(iGrid).Cells("episodeActeParamedicalId").Value = acteParamedicalDataTable.Rows(i)("oa_episode_acte_paramedical_id")
            RadObsSpeIdeDataGridView.Rows(iGrid).Cells("drcId").Value = acteParamedicalDataTable.Rows(i)("drc_id")
            RadObsSpeIdeDataGridView.Rows(iGrid).Cells("drcDescription").Value = Coalesce(acteParamedicalDataTable.Rows(i)("oa_drc_libelle"), "")
            RadObsSpeIdeDataGridView.Rows(iGrid).Cells("observation").Value = Coalesce(acteParamedicalDataTable.Rows(i)("observation"), "")
            RadObsSpeIdeDataGridView.Rows(iGrid).Cells("observationInput").Value = Coalesce(acteParamedicalDataTable.Rows(i)("observation"), "")
            RadObsSpeIdeDataGridView.Rows(iGrid).Cells("drcCommentaire").Value = Coalesce(acteParamedicalDataTable.Rows(i)("oa_drc_dur_prob_epis"), "")
            RadObsSpeIdeDataGridView.Rows(iGrid).Cells("categorieOasis").Value = Coalesce(acteParamedicalDataTable.Rows(i)("oa_drc_oasis_categorie"), 0)
            If RadObsSpeIdeDataGridView.Rows(iGrid).Cells("categorieOasis").Value = DrcDao.EnumCategorieOasisCode.ProtocoleAigu Then
                RadObsSpeIdeDataGridView.Rows(iGrid).Cells("drcDescription").Style.ForeColor = Color.Red
                ControleProtocoleAiguExiste = True
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If Index <> -1 Then
            If Index < RadObsSpeIdeDataGridView.Rows.Count Then
                Me.RadObsSpeIdeDataGridView.CurrentRow = RadObsSpeIdeDataGridView.ChildRows(Index)
                Me.RadObsSpeIdeDataGridView.Rows(Index).IsCurrent = True
                Me.RadObsSpeIdeDataGridView.Columns(5).IsCurrent = True
                Me.RadObsSpeIdeDataGridView.BeginEdit()
            End If
        Else
            If RadObsSpeIdeDataGridView.Rows.Count > 0 Then
                Me.RadObsSpeIdeDataGridView.CurrentRow = RadObsSpeIdeDataGridView.ChildRows(0)
            End If
        End If

        If RadObsSpeIdeDataGridView.Rows.Count > 0 Then
            'RadObsSpeIdeDataGridView.CurrentRow = RadObsSpeIdeDataGridView.ChildRows(0)
            'RadObsSpeIdeDataGridView.TableElement.VScrollBar.Value = 0
        End If

        If userLog.TypeProfil <> ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            RadObsSpeIdeDataGridView.AllowEditRow = False
        End If

        FinChargementActesParamedicauxParamedical = True
    End Sub

    Private Sub MasterTemplate_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles RadObsSpeIdeDataGridView.CellValueChanged
        If FinChargementActesParamedicauxParamedical = True Then
            ValidationSaisieObservationParamedicale()
        End If
    End Sub

    Private Sub ValidationSaisieObservationParamedicale()
        Dim MiseAJour As Boolean = False
        Dim ObsRowIndex As Integer = 0

        For Each rowInfo As GridViewRowInfo In RadObsSpeIdeDataGridView.Rows
            Dim observationInput As String = ""
            Dim observation As String = ""
            Dim id As Long
            For Each cellInfo As GridViewCellInfo In rowInfo.Cells
                If (cellInfo.ColumnInfo.Name = "observationInput") Then
                    If cellInfo.Value <> Nothing Then
                        observationInput = cellInfo.Value
                    Else
                        observationInput = ""
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "observation") Then
                    If cellInfo.Value <> Nothing Then
                        observation = cellInfo.Value
                    Else
                        observation = ""
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "episodeActeParamedicalId") Then
                    If cellInfo.Value <> Nothing Then
                        id = cellInfo.Value
                    Else
                        id = 0
                    End If
                End If
            Next
            If observationInput <> observation Then
                'Mise à jour de l'observation
                Console.WriteLine("Id : " & id.ToString & " Observation saisie : " & observationInput & " observation initiale : " & observation)
                episodeActeParamedicalDao.ModificationEpisodeActeParamedicalObservation(id, observationInput)
                MiseAJour = True
                If rowInfo.Index() >= ObsRowCount Then
                    ObsRowIndex = 0
                Else
                    ObsRowIndex = rowInfo.Index() + 1
                End If
            End If
        Next

        If MiseAJour = True Then
            Me.RadDesktopAlert1.CaptionText = "Notification saisie observation spécifique patient"
            Me.RadDesktopAlert1.ContentText = "Observation spécifique mise à jour"
            Me.RadDesktopAlert1.Show()
            'Dim form As New RadFNotification()
            'Form.Titre = "Notification saisie observation spécifique patient"
            'Form.Message = "Observation spécifique mise à jour"
            'Form.Show()
            'Rechargement grid
            ChargementEpisodeActesParamedicauxParamedical(ObsRowIndex)
        End If
    End Sub

    'Saisie observation spécifique via l'écran dédié selon le type de la DORC

    Private Sub RadMenuItemObsSpeIdeSaisieObservation_Click(sender As Object, e As EventArgs) Handles RadMenuItemObsSpeIdeSaisieObservation.Click
        SaisieObservation()
    End Sub
    Private Sub SaisieObservationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaisieObservationToolStripMenuItem.Click
        SaisieObservation()
    End Sub

    Private Sub SaisieObservation()
        If userLog.TypeProfil <> ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            Exit Sub
        End If

        If RadObsSpeIdeDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadObsSpeIdeDataGridView.Rows.IndexOf(Me.RadObsSpeIdeDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim episodeActeParamedicalId As Integer = RadObsSpeIdeDataGridView.Rows(aRow).Cells("episodeActeParamedicalId").Value
                Dim categorieOasis As Integer = RadObsSpeIdeDataGridView.Rows(aRow).Cells("categorieOasis").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Select Case categorieOasis
                    Case DrcDao.EnumCategorieOasisCode.ProtocoleAigu
                        Using form As New RadFEpisodeProtocoleAiguDetail
                            form.EpisodeActeParamedicalId = episodeActeParamedicalId
                            form.ShowDialog()
                            If form.CodeRetour = True Then
                                ChargementEpisodeActesParamedicauxParamedical()
                            End If
                        End Using

                    Case DrcDao.EnumCategorieOasisCode.ActeParamedical,
                         DrcDao.EnumCategorieOasisCode.Prevention
                        Using form As New RadFEpisodeActeParamedicalDetailEdit
                            form.EpisodeActeParamedicalId = episodeActeParamedicalId
                            form.ShowDialog()
                            If form.CodeRetour = True Then
                                ChargementEpisodeActesParamedicauxParamedical()
                            End If
                        End Using
                End Select
                Me.Enabled = True
            End If
        End If
    End Sub

    'Tooltip
    Private Sub MasterTemplate_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadObsSpeIdeDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub MasterTemplate_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadObsSpeIdeDataGridView.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "drcDescription" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("drcCommentaire").Value
        End If
    End Sub

    'Ajout protocole aigue (pour les épisodes de type "Pathologie aiguë")
    Private Sub AjoutProtocoleAiguToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AjoutProtocoleAiguToolStripMenuItem.Click
        AjoutProtocoleAigue()
    End Sub

    Private Sub RadMenuItemObseSpeIdeAjoutProtocoleAigue_Click(sender As Object, e As EventArgs) Handles RadMenuItemObseSpeIdeAjoutProtocoleAigue.Click
        AjoutProtocoleAigue()
    End Sub

    Private Sub AjoutProtocoleAigue()
        Dim SelectedDrcId As Integer
        Cursor.Current = Cursors.WaitCursor
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = DrcDao.EnumCategorieOasisCode.ProtocoleAigu
            vFDrcSelecteur.ShowDialog()
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si une DORC a été sélectionnée, on appelle le Formulaire de création
            If SelectedDrcId <> 0 Then
                Cursor.Current = Cursors.WaitCursor
                'Création observation spécifique
                Dim episodeActeParamedical As New EpisodeActeParamedical
                episodeActeParamedical.DrcId = SelectedDrcId
                episodeActeParamedical.EpisodeId = SelectedEpisodeId
                episodeActeParamedical.PatientId = SelectedPatient.patientId
                episodeActeParamedical.TypeObservation = FonctionDao.enumTypeFonction.PARAMEDICAL.ToString
                episodeActeParamedical.Observation = ""
                episodeActeParamedical.UserId = userLog.UtilisateurId
                episodeActeParamedical.Inactif = False
                Dim episodeActeParamedicalId As Long
                episodeActeParamedicalId = episodeActeParamedicalDao.CreateEpisodeActeParamedical(episodeActeParamedical)
                If episodeActeParamedicalId <> 0 Then
                    Using form As New RadFEpisodeProtocoleAiguDetail
                        form.EpisodeActeParamedicalId = episodeActeParamedicalId
                        form.ShowDialog()
                        If form.CodeRetour = True Then
                            ChargementObservationSpecifique()
                        End If
                    End Using
                Else
                    MessageBox.Show("Le protocole aigu sélectionné existe déjà pour cet épisode !")
                End If
            End If
        End Using
    End Sub

    Private Sub RadObsSpeParDataGridView_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadObsSpeIdeDataGridView.CellDoubleClick
        Dim SelectedCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If SelectedCell IsNot Nothing AndAlso SelectedCell.ColumnInfo.Name = "drcDescription" Then
            SaisieObservation()
        End If
    End Sub


    '=========================================================
    '=== Observations spécifiques (médical)
    '=========================================================
    'Chargement des actes paramédicaux associés à l'épisode
    Private Sub ChargementEpisodeActesParamedicauxMedical()
        FinChargementActesParamedicauxMedical = False
        Dim acteParamedicalDataTable As DataTable
        acteParamedicalDataTable = episodeActeParamedicalDao.getAllEpisodeActeParamedicalByEpisodeId(SelectedEpisodeId, ProfilDao.EnumProfilType.MEDICAL.ToString)

        RadObsSpeMedDataGridView.Rows.Clear()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = acteParamedicalDataTable.Rows.Count - 1
        If acteParamedicalDataTable.Rows.Count > 0 Then
            ControleActeParamedicalExiste = True
        End If
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadObsSpeMedDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("episodeActeParamedicalId").Value = acteParamedicalDataTable.Rows(i)("oa_episode_acte_paramedical_id")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("drcId").Value = acteParamedicalDataTable.Rows(i)("drc_id")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("drcDescription").Value = Coalesce(acteParamedicalDataTable.Rows(i)("oa_drc_libelle"), "")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("observation").Value = Coalesce(acteParamedicalDataTable.Rows(i)("observation"), "")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("observationInput").Value = Coalesce(acteParamedicalDataTable.Rows(i)("observation"), "")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("drcCommentaire").Value = Coalesce(acteParamedicalDataTable.Rows(i)("oa_drc_dur_prob_epis"), "")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("categorieOasis").Value = Coalesce(acteParamedicalDataTable.Rows(i)("oa_drc_oasis_categorie"), 0)
            If RadObsSpeMedDataGridView.Rows(iGrid).Cells("categorieOasis").Value = DrcDao.EnumCategorieOasisCode.ProtocoleAigu Then
                RadObsSpeMedDataGridView.Rows(iGrid).Cells("drcDescription").Style.ForeColor = Color.Red
                ControleProtocoleAiguExiste = True
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadObsSpeMedDataGridView.Rows.Count > 0 Then
            RadObsSpeMedDataGridView.CurrentRow = RadObsSpeMedDataGridView.ChildRows(0)
            RadObsSpeMedDataGridView.TableElement.VScrollBar.Value = 0
        End If

        If userLog.TypeProfil <> ProfilDao.EnumProfilType.MEDICAL.ToString Then
            RadObsSpeMedDataGridView.AllowEditRow = False
            SaisieObservationSpecifiqueMedicaleItem.Enabled = False
            AttributionDesObservationsSpécifiquesToolStripMenuItem.Enabled = False
        End If

        FinChargementActesParamedicauxMedical = True
    End Sub

    Private Sub RadObsSpeMedDataGridView_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles RadObsSpeMedDataGridView.CellValueChanged
        If FinChargementActesParamedicauxMedical = True Then
            ValidationSaisieObservationMedicale()
        End If
    End Sub

    Private Sub RadObsSpeMedDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadObsSpeMedDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SaisieObservationSpecifiqueMedicaleItem.Click
        If userLog.TypeProfil <> ProfilDao.EnumProfilType.MEDICAL.ToString Then
            Exit Sub
        End If

        If RadObsSpeMedDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadObsSpeMedDataGridView.Rows.IndexOf(Me.RadObsSpeMedDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim episodeActeParamedicalId As Integer = RadObsSpeMedDataGridView.Rows(aRow).Cells("episodeActeParamedicalId").Value
                Dim categorieOasis As Integer = RadObsSpeMedDataGridView.Rows(aRow).Cells("categorieOasis").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Select Case categorieOasis
                    Case DrcDao.EnumCategorieOasisCode.ProtocoleAigu
                        Using form As New RadFEpisodeProtocoleAiguDetail
                            form.EpisodeActeParamedicalId = episodeActeParamedicalId
                            form.ShowDialog()
                            If form.CodeRetour = True Then
                                ChargementEpisodeActesParamedicauxParamedical()
                            End If
                        End Using

                    Case DrcDao.EnumCategorieOasisCode.ActeParamedical,
                         DrcDao.EnumCategorieOasisCode.Prevention
                        Using form As New RadFEpisodeActeParamedicalDetailEdit
                            form.EpisodeActeParamedicalId = episodeActeParamedicalId
                            form.ShowDialog()
                            If form.CodeRetour = True Then
                                ChargementEpisodeActesParamedicauxParamedical()
                            End If
                        End Using
                End Select
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub MasterTemplate_ToolTipTextNeeded_1(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadObsSpeMedDataGridView.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "drcDescription" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("drcCommentaire").Value
        End If
    End Sub

    Private Sub AttributionDesObservationsSpécifiquesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AttributionDesObservationsSpécifiquesToolStripMenuItem.Click
        'Modification du type de PARAMEDICAL en MEDICAL, puis rechargement des deux grids
        episodeActeParamedicalDao.PutAllEpisodeActeParamedicalToMedicalByEpisodeId(SelectedEpisodeId)
        ChargementObservationSpecifique()
    End Sub

    Private Sub ValidationSaisieObservationMedicale()
        Dim MiseAJour As Boolean = False

        For Each rowInfo As GridViewRowInfo In RadObsSpeMedDataGridView.Rows
            Dim observationInput As String = ""
            Dim observation As String = ""
            Dim id As Long
            For Each cellInfo As GridViewCellInfo In rowInfo.Cells
                If (cellInfo.ColumnInfo.Name = "observationInput") Then
                    If cellInfo.Value <> Nothing Then
                        observationInput = cellInfo.Value
                    Else
                        observationInput = ""
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "observation") Then
                    If cellInfo.Value <> Nothing Then
                        observation = cellInfo.Value
                    Else
                        observation = ""
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "episodeActeParamedicalId") Then
                    If cellInfo.Value <> Nothing Then
                        id = cellInfo.Value
                    Else
                        id = 0
                    End If
                End If
            Next
            If observationInput <> observation Then
                'Mise à jour de l'observation
                Console.WriteLine("Id : " & id.ToString & " Observation saisie : " & observationInput & " observation initiale : " & observation)
                episodeActeParamedicalDao.ModificationEpisodeActeParamedicalObservation(id, observationInput)
                MiseAJour = True
            End If
        Next

        If MiseAJour = True Then
            Dim form As New RadFNotification()
            form.Titre = "Notification saisie observation spécifique patient"
            form.Message = "Observation spécifique mise à jour"
            form.Show()
            'Rechargement grid
            ChargementEpisodeActesParamedicauxMedical()
        End If
    End Sub


    '=====================================================================================================================================
    '=== Observations libres
    '=====================================================================================================================================

    'Chargement des observations libres associés à l'épisode
    Private Sub ChargementObservationLibre()
        Dim episodeObservationDao As New EpisodeObservationDao
        Dim ObservationSpe As DataTable
        ObservationSpe = episodeObservationDao.GetEpisodeObservationLibreByEpisode(Me.SelectedEpisodeId)

        RadGridViewObsMed.Rows.Clear()
        RadGridViewObsIde.Rows.Clear()

        Dim AfficheDateCreation, Auteur As String
        Dim DateCreation As Date

        Dim i As Integer
        Dim rowCount As Integer = ObservationSpe.Rows.Count - 1
        Dim iGridMed As Integer = -1
        Dim iGridIde As Integer = -1

        For i = 0 To rowCount Step 1
            'Utilisateur creation
            Auteur = ""
            If Coalesce(ObservationSpe.Rows(i)("user_id"), 0) <> 0 Then
                Dim UtilisateurCreation As Utilisateur
                Dim userDao As New UserDao
                UtilisateurCreation = userDao.getUserById(ObservationSpe.Rows(i)("user_id"))
                'SetUtilisateur(UtilisateurCreation, ObservationSpe.Rows(i)("user_id"))
                Auteur = UtilisateurCreation.UtilisateurPrenom & " " & UtilisateurCreation.UtilisateurNom
            End If

            'Date création
            AfficheDateCreation = ""
            If Coalesce(ObservationSpe.Rows(i)("date_creation"), Nothing) <> Nothing Then
                DateCreation = ObservationSpe.Rows(i)("date_creation")
                AfficheDateCreation = DateCreation.ToString("dd.MM.yy HH:mm")
            End If

            Select Case ObservationSpe.Rows(i)("type_observation")
                Case EpisodeObservationDao.EnumTypeEpisodeObservation.MEDICAL.ToString
                    iGridMed += 1
                    RadGridViewObsMed.Rows.Add(iGridMed)
                    RadGridViewObsMed.Rows(iGridMed).Cells("observation").Value = ObservationSpe.Rows(i)("observation")
                    RadGridViewObsMed.Rows(iGridMed).Cells("Identification").Value = Auteur & vbCrLf & AfficheDateCreation
                    RadGridViewObsMed.Rows(iGridMed).Cells("observationId").Value = ObservationSpe.Rows(i)("episode_observation_id")
                Case EpisodeObservationDao.EnumTypeEpisodeObservation.PARAMEDICAL.ToString()
                    iGridIde += 1
                    RadGridViewObsIde.Rows.Add(iGridIde)
                    RadGridViewObsIde.Rows(iGridIde).Cells("observation").Value = ObservationSpe.Rows(i)("observation")
                    RadGridViewObsIde.Rows(iGridIde).Cells("Identification").Value = Auteur & vbCrLf & AfficheDateCreation
                    RadGridViewObsIde.Rows(iGridIde).Cells("observationId").Value = ObservationSpe.Rows(i)("episode_observation_id")
            End Select
        Next

        If RadGridViewObsMed.Rows.Count > 0 Then
            Me.RadGridViewObsMed.CurrentRow = RadGridViewObsMed.Rows(0)
            RadGridViewObsMed.TableElement.VScrollBar.Value = 0
            If episode.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
                ControleDemandeAvisMedicalExiste = True
                ControleMAJTypeConclusionIDE()
            End If
        End If

        If RadGridViewObsIde.Rows.Count > 0 Then
            Me.RadGridViewObsIde.CurrentRow = RadGridViewObsIde.Rows(0)
            RadGridViewObsIde.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub RadGridViewObsIde_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewObsIde.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub RadGridViewObsMed_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewObsMed.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    'Créer une observation libre
    Private Sub RadBtnAddObsLibreIde_Click(sender As Object, e As EventArgs) Handles RadBtnAddObsLibreIde.Click
        CreationObservationLibre()
    End Sub

    Private Sub RadBtnAddObsLibreMed_Click(sender As Object, e As EventArgs) Handles RadBtnAddObsLibreMed.Click
        CreationObservationLibre()
    End Sub

    Private Sub CréerUneObservationToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CréerUneObservationToolStripMenuItem1.Click
        CreationObservationLibre()
    End Sub

    Private Sub CreationObservationLibre()
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vRadFEpisodeObservationDetailEdit As New RadFEpisodeObservationDetailEdit
            vRadFEpisodeObservationDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
            vRadFEpisodeObservationDetailEdit.SelectedPatient = Me.SelectedPatient
            vRadFEpisodeObservationDetailEdit.SelectedObservationId = 0
            vRadFEpisodeObservationDetailEdit.ShowDialog()
            If vRadFEpisodeObservationDetailEdit.CodeRetour = True Then
                ChargementObservationLibre()
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Modifier / Visualiser une observation libre paramédicale
    Private Sub RadGridViewObsIde_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewObsIde.CellDoubleClick
        If RadGridViewObsIde.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewObsIde.Rows.IndexOf(Me.RadGridViewObsIde.CurrentRow)
            If aRow >= 0 Then
                Dim ObservationId As Integer = RadGridViewObsIde.Rows(aRow).Cells("observationId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vRadFEpisodeObservationDetailEdit As New RadFEpisodeObservationDetailEdit
                    vRadFEpisodeObservationDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
                    vRadFEpisodeObservationDetailEdit.SelectedPatient = Me.SelectedPatient
                    vRadFEpisodeObservationDetailEdit.SelectedObservationId = ObservationId
                    vRadFEpisodeObservationDetailEdit.ShowDialog()
                    If vRadFEpisodeObservationDetailEdit.CodeRetour = True Then
                        ChargementObservationLibre()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Modifier / Visualiser une observation libre médicale
    Private Sub RadGridViewObsMed_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewObsMed.CellDoubleClick
        If RadGridViewObsMed.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewObsMed.Rows.IndexOf(Me.RadGridViewObsMed.CurrentRow)
            If aRow >= 0 Then
                Dim ObservationId As Integer = RadGridViewObsMed.Rows(aRow).Cells("observationId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vRadFEpisodeObservationDetailEdit As New RadFEpisodeObservationDetailEdit
                    vRadFEpisodeObservationDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
                    vRadFEpisodeObservationDetailEdit.SelectedPatient = Me.SelectedPatient
                    vRadFEpisodeObservationDetailEdit.SelectedObservationId = ObservationId
                    vRadFEpisodeObservationDetailEdit.ShowDialog()
                    If vRadFEpisodeObservationDetailEdit.CodeRetour = True Then
                        ChargementObservationLibre()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub


    '=============================================================================================================================================
    '=== Workflow
    '=============================================================================================================================================

    'Affichage Workflow
    Private Sub ChargementAffichageBlocWorkflow()
        'Identifier si Workflow en cours
        LblPriorité.Hide()
        tache = tacheDao.GetDemandeEnCoursByEpisode(SelectedEpisodeId)
        If tache.Id <> 0 Then
            Select Case tache.Priorite
                Case 100
                    LblPriorité.Text = "-- Urgent --"
                    LblPriorité.ForeColor = Color.Red
                    LblPriorité.Font = New Font(LblPriorité.Font, FontStyle.Bold)
                Case 200
                    LblPriorité.Text = "- Synchrone -"
                    LblPriorité.ForeColor = Color.DarkBlue
                    LblPriorité.Font = New Font(LblPriorité.Font, FontStyle.Bold)
                Case 300
                    LblPriorité.Text = "Asynchrone"
                    LblPriorité.ForeColor = Color.Black
                    LblPriorité.Font = New Font(LblPriorité.Font, FontStyle.Regular)
                Case Else
                    LblPriorité.Text = ""
                    LblPriorité.ForeColor = Color.Black
                    LblPriorité.Font = New Font(LblPriorité.Font, FontStyle.Regular)
            End Select
            LblPriorité.Show()
            ControleWorkflowEnCoursExistant = True
            OptionWorkflow = TacheDao.EnumOptionWorkflow.NULL
            Dim fonctionDestinataire As Fonction
            fonctionDestinataire = fonctionDao.getFonctionById(tache.DestinataireFonctionId)
            Select Case fonctionDestinataire.Type
                Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                    RadBtnWorkflowIde.Show()
                    RadBtnWorkflowIde.Enabled = True
                    RadBtnWorkflowIde.ForeColor = Color.Red
                    RadBtnWorkflowIde.Font = New Font(RadBtnWorkflowIde.Font, FontStyle.Bold)
                    Select Case tache.Nature
                        Case TacheDao.NatureTache.DEMANDE.ToString
                            'LblWorkflowIDE.Text = "Demande d'avis à traiter"
                            'LblWorkflowMed.Text = "Attente rendu de demande d'avis"
                            RadBtnWorkflowIde.Text = "Réponse à rendre"
                            RadBtnWorkflowMed.Text = "Avis demandé"
                        Case TacheDao.NatureTache.REPONSE.ToString
                            'LblWorkflowIDE.Text = "Réponse à valider"
                            'LblWorkflowMed.Text = "Demande d'avis rendue"
                            RadBtnWorkflowIde.Text = "Avis à valider"
                            RadBtnWorkflowMed.Text = "Avis rendu"
                        Case TacheDao.NatureTache.COMPLEMENT.ToString
                            'LblWorkflowIDE.Text = "Demande de complément d'information"
                            'LblWorkflowMed.Text = "Attente complément d'information"
                            RadBtnWorkflowIde.Text = "Précision à rendre"
                            RadBtnWorkflowMed.Text = "Demande précision"
                    End Select
                    'RadBtnWorkflowMed.Hide()
                    RadBtnWorkflowMed.Show()
                    RadBtnWorkflowMed.Enabled = False
                    RadBtnWorkflowMed.ForeColor = Color.FromArgb(21, 66, 139)
                    RadBtnWorkflowMed.Font = New Font(RadBtnWorkflowMed.Font, FontStyle.Regular)
                    LblWorkFlow.Text = "<-----------------------------------"
                    LblWorkFlow.Show()
                    TextBoxIDECommentaireWorkflow.Hide()
                    TextBoxMedCommentaireWorkflow.Text = tache.EmetteurCommentaire
                    TextBoxMedCommentaireWorkflow.Show()
                Case ProfilDao.EnumProfilType.MEDICAL.ToString
                    'RadBtnWorkflowMed.Text = "Traiter"
                    RadBtnWorkflowMed.Show()
                    RadBtnWorkflowMed.Enabled = True
                    RadBtnWorkflowMed.ForeColor = Color.Red
                    RadBtnWorkflowMed.Font = New Font(RadBtnWorkflowMed.Font, FontStyle.Bold)
                    Select Case tache.Nature
                        Case TacheDao.NatureTache.DEMANDE.ToString
                            'LblWorkflowMed.Text = "Demande d'avis à traiter"
                            'LblWorkflowIDE.Text = "Attente rendu de demande d'avis"
                            RadBtnWorkflowMed.Text = "Réponse à rendre"
                            RadBtnWorkflowIde.Text = "Avis demandé"
                        Case TacheDao.NatureTache.REPONSE.ToString
                            'LblWorkflowMed.Text = "Réponse à valider"
                            'LblWorkflowIDE.Text = "Demande d'avis rendue"
                            RadBtnWorkflowMed.Text = "Avis à valider"
                            RadBtnWorkflowIde.Text = "Avis rendu"
                        Case TacheDao.NatureTache.COMPLEMENT.ToString
                            'LblWorkflowMed.Text = "Demande de complément d'information"
                            'LblWorkflowIDE.Text = "Attente complément d'information"
                            RadBtnWorkflowMed.Text = "Précision à rendre"
                            RadBtnWorkflowIde.Text = "Demande de précision"
                    End Select
                    'RadBtnWorkflowIde.Hide()
                    RadBtnWorkflowIde.Show()
                    RadBtnWorkflowIde.Enabled = False
                    RadBtnWorkflowIde.ForeColor = Color.FromArgb(21, 66, 139)
                    RadBtnWorkflowIde.Font = New Font(RadBtnWorkflowIde.Font, FontStyle.Regular)
                    LblWorkFlow.Text = "----------------------------------->"
                    LblWorkFlow.Show()
                    TextBoxMedCommentaireWorkflow.Hide()
                    TextBoxIDECommentaireWorkflow.Text = tache.EmetteurCommentaire
                    TextBoxIDECommentaireWorkflow.Show()
            End Select
            Select Case userLog.TypeProfil
                Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                    RadBtnWorkflowMed.Enabled = False
                Case ProfilDao.EnumProfilType.MEDICAL.ToString
                    RadBtnWorkflowIde.Enabled = False
            End Select
        Else
            LblWorkFlow.Text = ""
            ControleWorkflowEnCoursExistant = False
            If episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString AndAlso episode.DateModification.Date < Date.Now.Date Then
                RadBtnWorkflowMed.Hide()
                RadBtnWorkflowIde.Hide()
                LblWorkFlow.Hide()
                LblWorkFlow.Text = ""
            Else
                RadBtnWorkflowIde.ForeColor = Color.FromArgb(21, 66, 139)
                RadBtnWorkflowIde.Font = New Font(RadBtnWorkflowIde.Font, FontStyle.Regular)
                RadBtnWorkflowMed.ForeColor = Color.FromArgb(21, 66, 139)
                RadBtnWorkflowMed.Font = New Font(RadBtnWorkflowMed.Font, FontStyle.Regular)
                Select Case userLog.TypeProfil
                    Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                        'RadBtnWorkflowIde.Text = "Créer"
                        RadBtnWorkflowIde.Show()
                        RadBtnWorkflowIde.Text = "Demande d'avis"
                        RadBtnWorkflowIde.Enabled = True
                        'LblWorkflowIDE.Text = "Créer une demande d'avis"
                        'LblWorkflowIDE.Show()
                        RadBtnWorkflowMed.Hide()
                        'LblWorkflowMed.Hide()
                    Case ProfilDao.EnumProfilType.MEDICAL.ToString
                        'RadBtnWorkflowMed.Text = "Créer"
                        RadBtnWorkflowMed.Show()
                        RadBtnWorkflowMed.Text = "demande d'avis"
                        RadBtnWorkflowMed.Enabled = True
                        'LblWorkflowMed.Text = "Créer une demande d'avis"
                        'LblWorkflowMed.Show()
                        RadBtnWorkflowIde.Hide()
                        'LblWorkflowIDE.Hide()
                End Select
            End If
        End If

        If episode.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            'Controle si Workflow de demande d'avis médical (en cours ou terminé) existe
            If tacheDao.ExisteDemandeAvisMedicalByEpisode(SelectedEpisodeId) = True Then
                ControleDemandeAvisMedicalExiste = True
                ControleMAJTypeConclusionIDE()
            End If
        End If
    End Sub

    'Traiter une tâche de la demande d'avis (Workflow)
    Private Sub RadBtnWorkflowIde_Click(sender As Object, e As EventArgs) Handles RadBtnWorkflowIde.Click
        ControleGestionWorkflow()
        GestionClotureAutomatique()
    End Sub

    Private Sub RadBtnWorkflowMed_Click(sender As Object, e As EventArgs) Handles RadBtnWorkflowMed.Click
        ControleGestionWorkflow()
    End Sub

    Private Sub ControleGestionWorkflow()
        tache = tacheDao.GetDemandeEnCoursByEpisode(SelectedEpisodeId)
        If tache.Id <> 0 Then
            If ControleWorkflowEnCoursExistant = False Then
                ChargementAffichageBlocWorkflow()
                MessageBox.Show("Opération annulée, une demande d'avis vient d'être créée pour cet épisode par un autre utilisateur")
                Exit Sub
            End If

            If tache.TraiteUserId <> 0 Then
                If tache.TraiteUserId <> userLog.UtilisateurId Then
                    Dim userTraitement As Utilisateur
                    userTraitement = UserDao.getUserById(tache.TraiteUserId)
                    MessageBox.Show("Accès impossible, le traitement de cette demande d'avis est déjà attribué à " & userTraitement.UtilisateurPrenom & " " & userTraitement.UtilisateurNom & " (" & userTraitement.UtilisateurProfilId & ")")
                    Exit Sub
                End If
            Else
                'Tâche non encore attribuée, proposition d'attribution de la tâche à l'utilisateur
                If MsgBox("Par cette action, vous allez vous attribuer le traitement de cette demande d'avis. Confirmez-vous son attribution", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                    Try
                        tacheDao.attribueTacheToUserLog(tache.Id)
                    Catch ex As Exception
                        Exit Sub
                    End Try
                Else
                    Exit Sub
                End If
            End If
        End If

        If tache.Id <> 0 Then
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vRadFWkfDemandeAvis As New RadFWkfDemandeAvis
                vRadFWkfDemandeAvis.SelectedEpisodeId = Me.SelectedEpisodeId
                vRadFWkfDemandeAvis.SelectedPatient = Me.SelectedPatient
                vRadFWkfDemandeAvis.SelectedTacheId = tache.Id
                vRadFWkfDemandeAvis.Creation = False
                vRadFWkfDemandeAvis.Provenance = RadFWkfDemandeAvis.EnumProvenance.EPISODE
                vRadFWkfDemandeAvis.ShowDialog()
            End Using
            Me.Enabled = True
        Else
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vRadFWkfDemandeAvis As New RadFWkfDemandeAvis
                vRadFWkfDemandeAvis.SelectedEpisodeId = Me.SelectedEpisodeId
                vRadFWkfDemandeAvis.SelectedPatient = Me.SelectedPatient
                vRadFWkfDemandeAvis.SelectedTacheId = 0
                vRadFWkfDemandeAvis.Creation = True
                vRadFWkfDemandeAvis.Provenance = RadFWkfDemandeAvis.EnumProvenance.EPISODE
                vRadFWkfDemandeAvis.ShowDialog()
            End Using
            Me.Enabled = True
        End If

        ChargementAffichageBlocWorkflow()
    End Sub

    'Liste des workflows de l'épisode
    Private Sub RadBtnHistoWorkflow_Click(sender As Object, e As EventArgs) Handles RadBtnHistoWorkflowMed.Click
        HistoWorkflow()
    End Sub

    Private Sub RadBtnHistoriqueAvisIde_Click(sender As Object, e As EventArgs) Handles RadBtnHistoriqueAvisIde.Click
        HistoWorkflow()
    End Sub

    Private Sub HistoWorkflow()
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vRadFWkfDemandeAvishisto As New RadFWkfDemandeAvisHisto
            vRadFWkfDemandeAvishisto.SelectedEpisodeId = Me.SelectedEpisodeId
            vRadFWkfDemandeAvishisto.SelectedPatient = Me.SelectedPatient
            vRadFWkfDemandeAvishisto.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub


    'Avant la fermeture de l'écran : si un utilisateur s'est attribué le traitement de demande d'avis (Workflow) et qu'il sort de l'épisode sans l'avoir traité, on lui désattibue le tâche
    Private Sub RadFEpisodeDetail_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        tache = tacheDao.GetDemandeEnCoursByEpisode(SelectedEpisodeId)
        If tache.isAttribue AndAlso tache.TraiteUserId = userLog.UtilisateurId Then
            tacheDao.desattribueTache(tache.Id)
            Dim form As New RadFNotification()
            form.Titre = "Notification épisode"
            form.Message = "La sortie de l'épisode sans traiter la demande d'avis a automatiquement annulé l'attribution de la tâche"
            form.Show()
        End If

        'Mise à jour base de données si commentaire IDE modifié
        ModificationCommentaireConclusionIde()

        'Mise à jour base de données si bouton radio type conclusion médicale modifiée
        ModificationRadioTypeConclusionIDE()

        Environnement.ControleAccesForm.RemoveFormToControl(EnumForm.EPISODE.ToString)
        If RemoveEpisode = True Then
            Environnement.ControleAccesEpisode.RemoveEpisodeToControl(SelectedEpisodeId)
        End If
    End Sub


    '===========================================================================================================================================
    '=== Conclusion épisode 
    '===========================================================================================================================================

    'Chargement conclusion médicale et paramédicale 
    Private Sub ChargementConclusion()
        ChargementConclusionEnCours = True
        'ControleTypeConclusionParamedicaleSurDemandeAvis()

        Select Case episode.TypeProfil
            Case ProfilDao.EnumProfilType.MEDICAL.ToString
                RadPanelConclusionIdeType.Hide()
                episode.ConclusionIdeType = ""
            Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                Select Case episode.ConclusionIdeType
                    Case EpisodeDao.EnumTypeConclusionParamedicale.ROLE_PROPRE.ToString
                        RadioBtnRolePropre.Checked = True
                        RadioTypeConclusionIdeModified = False
                    Case EpisodeDao.EnumTypeConclusionParamedicale.SUR_PROTOCOLE.ToString
                        RadioBtnSurProtocole.Checked = True
                        RadioTypeConclusionIdeModified = False
                    Case EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString
                        RadioBtnDemandeAvis.Checked = True
                        RadioTypeConclusionIdeModified = False
                End Select
        End Select

        'Chargement commentaire de conclusion Paramédicale
        TxtConclusionIDE.Text = Coalesce(episode.ObservationParamedical, "")

        'Chargement conclusion consigne IDE
        If episode.ConclusionMedConsigneDrcId <> 0 Then
            'drc = drcDao.getDrcById(episode.ConclusionMedConsigneDrcId)
            TxtConsigneMedicale.Text = episode.ConclusionMedConsigneDenomination
            RadGrpConsigneIDE.Show()
            ToolTip.SetToolTip(TxtConsigneMedicale, episode.ConclusionMedConsigneDenomination)
        Else
            TxtConsigneMedicale.Text = ""
            RadGrpConsigneIDE.Hide()
        End If

        'Chargement contexte(s) de conclusion
        ChargementEpisodeContexteConclusion()

        ChargementConclusionEnCours = False
        ControleMAJTypeConclusionIDE()
    End Sub

    Private Sub ControleMAJTypeConclusionIDE()
        If episode.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            'Si demande d'avis, conclusion IDE obligatoirement sur demande d'avis
            If ControleDemandeAvisMedicalExiste = True Then 'Un épisode paramédical est considéré sur demande d'avis si Workflow de demande d'avis et/ou si observation libre et/ou su conclusion médicale
                'Boutons radio non modifiables
                RadioBtnDemandeAvis.Enabled = False
                RadioBtnRolePropre.Enabled = False
                RadioBtnSurProtocole.Enabled = False
                'Si l'épisode n'avait pas la même valeur, on met à jour l'épisode
                If episode.ConclusionIdeType <> EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString Then
                    RadioBtnDemandeAvis.Checked = True
                    episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString
                    episodeDao.ModificationEpisode(episode)
                    RadioTypeConclusionIdeModified = False
                End If
                TxtConclusionIDE.Hide()
                TxtConclusionIDE.Text = ""
            Else
                If ControleProtocoleAiguExiste = True OrElse ControleActeParamedicalExiste = True Then
                    'On positionne les boutons radio à indisponible par défaut
                    RadioBtnDemandeAvis.Enabled = False
                    RadioBtnRolePropre.Enabled = False
                    RadioBtnSurProtocole.Enabled = False
                    'L'épisode est a minima sur protocole, mais l'IDE peut le déclarer sur demande d'avis
                    If Not (episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.SUR_PROTOCOLE.ToString OrElse
                            episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString) Then
                        RadioBtnSurProtocole.Checked = True
                        episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.SUR_PROTOCOLE.ToString
                        episodeDao.ModificationEpisode(episode)
                        RadioTypeConclusionIdeModified = False
                    End If
                    'Seules les options 'Demande d'avis' et 'Protocole' sont possibles si le profil utilisateur est IDE et si l'épisode n'est pas clôturé depuis plus d'un jour
                    If userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
                        If Not (episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString AndAlso episode.DateModification.Date < Date.Now.Date) Then
                            RadioBtnDemandeAvis.Enabled = True
                            RadioBtnSurProtocole.Enabled = True
                        End If
                    End If
                    RadioBtnRolePropre.Enabled = False
                End If
                TxtConclusionIDE.Show()
            End If
            'Si l'épisode n'avait pas de valeur de définie, alors rôle propre par défaut
            If RadioBtnSurProtocole.Checked = False AndAlso RadioBtnRolePropre.Checked = False AndAlso RadioBtnDemandeAvis.Checked = False Then
                RadioBtnRolePropre.Checked = True
                episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.ROLE_PROPRE.ToString
                episodeDao.ModificationEpisode(episode)
                RadioTypeConclusionIdeModified = False
                If userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
                    If Not (episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString AndAlso episode.DateModification.Date < Date.Now.Date) Then
                        RadioBtnDemandeAvis.Enabled = True
                        RadioBtnSurProtocole.Enabled = True
                        RadioBtnRolePropre.Enabled = True
                    End If
                End If
            End If
            'Autre cas, si l'utilisateur a modifié le type de conclusion
            If RadioBtnRolePropre.Checked = True Then
                If episode.ConclusionIdeType <> EpisodeDao.EnumTypeConclusionParamedicale.ROLE_PROPRE.ToString Then
                    episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.ROLE_PROPRE.ToString
                    episodeDao.ModificationEpisode(episode)
                    RadioTypeConclusionIdeModified = False
                End If
            Else
                If RadioBtnSurProtocole.Checked = True Then
                    If episode.ConclusionIdeType <> EpisodeDao.EnumTypeConclusionParamedicale.SUR_PROTOCOLE.ToString Then
                        episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.SUR_PROTOCOLE.ToString
                        episodeDao.ModificationEpisode(episode)
                        RadioTypeConclusionIdeModified = False
                    End If
                Else
                    If RadioBtnDemandeAvis.Checked = True Then
                        If episode.ConclusionIdeType <> EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString Then
                            episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString
                            episodeDao.ModificationEpisode(episode)
                            RadioTypeConclusionIdeModified = False
                        End If
                    End If
                End If
            End If
        End If
    End Sub


    '======================================================
    '===== Conclusion Médicale
    '======================================================
    'Chargement Contexte conclusion médicale
    Private Sub ChargementEpisodeContexteConclusion()
        Dim AfficheDateModification, diagnostic, categorieContexte As String

        Dim episodeContexteDt As DataTable
        episodeContexteDt = episodeContexteDao.GetAllEpisodeContexteByEpisodeId(SelectedEpisodeId)

        'Booléen pour déterminer si la conclusion médicale existe (au - un contexte)
        If episodeContexteDt.Rows.Count > 0 Then
            ControleConclusionMedicaleExiste = True
            ControleDemandeAvisMedicalExiste = True  'Pour un épisode Paramédical, On considère que l'épisode est sur demande d'avis quand il y a une conclusion médicale
        End If

        RadGridViewContexteEpisode.Rows.Clear()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = episodeContexteDt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            categorieContexte = ""
            If episodeContexteDt.Rows(i)("oa_antecedent_categorie_contexte") IsNot DBNull.Value Then
                categorieContexte = episodeContexteDt.Rows(i)("oa_antecedent_categorie_contexte")
            End If
            'Recherche si le contexte a été modifié (médical uniquement)
            AfficheDateModification = ""
            If categorieContexte = "M" Then
                If episodeContexteDt.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    DateModification = episodeContexteDt.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = FormatageDateAffichage(DateModification) + " : "
                Else
                    If episodeContexteDt.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        DateModification = episodeContexteDt.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = FormatageDateAffichage(DateModification) + " : "
                    End If
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewContexteEpisode.Rows.Add(iGrid)
            'Alimentation du DataGridView
            diagnostic = ""
            If episodeContexteDt.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(episodeContexteDt.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(episodeContexteDt.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            Dim longueurString As Integer
            Dim longueurMax As Integer = 150
            Dim contexteDescription As String
            contexteDescription = Coalesce(episodeContexteDt.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                longueurString = contexteDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                contexteDescription.Substring(0, longueurString)
            End If

            RadGridViewContexteEpisode.Rows(iGrid).Cells("contexte_id").Value = Coalesce(episodeContexteDt.Rows(i)("contexte_id"), 0)
            RadGridViewContexteEpisode.Rows(iGrid).Cells("episode_contexte_id").Value = episodeContexteDt.Rows(i)("episode_contexte_id")
            RadGridViewContexteEpisode.Rows(iGrid).Cells("contexte").Value = diagnostic & " " & contexteDescription
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewContexteEpisode.Rows.Count > 0 Then
            RadGridViewContexteEpisode.CurrentRow = RadGridViewContexteEpisode.ChildRows(0)
            RadGridViewContexteEpisode.TableElement.VScrollBar.Value = 0
        End If

        ControleEpisodeCloture()
        If ControleAjoutConclusion = True Then
            RadBtnConclusion.Enabled = True
        End If
    End Sub

    'Gérer les contextes de conclusion
    Private Sub RadBtnConclusion_Click(sender As Object, e As EventArgs) Handles RadBtnConclusion.Click
        Using form As New RadFEpisodeConclusionContextePatient
            form.SelectedEpisode = episode
            form.ShowDialog()
            If form.CodeRetour = True Then
                ChargementCaracteristiquesEpisode()
                DroitAcces()
                ChargementConclusion()
            End If
        End Using
    End Sub

    'Modifier un contexte de conclusion médicale
    Private Sub RadGridViewContexteEpisode_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewContexteEpisode.CellDoubleClick
        If RadGridViewContexteEpisode.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewContexteEpisode.Rows.IndexOf(Me.RadGridViewContexteEpisode.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadGridViewContexteEpisode.Rows(aRow).Cells("contexte_id").Value
                If ContexteId <> 0 Then
                    Cursor.Current = Cursors.WaitCursor
                    Me.Enabled = False
                    Using vFContexteDetailEdit As New RadFContextedetailEdit
                        vFContexteDetailEdit.SelectedContexteId = ContexteId
                        vFContexteDetailEdit.SelectedPatient = Me.SelectedPatient
                        vFContexteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFContexteDetailEdit.SelectedDrcId = 0
                        vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                        vFContexteDetailEdit.ShowDialog()
                        If vFContexteDetailEdit.CodeRetour = True Then
                            episodeDao.MajEpisodeConclusionMedicale(SelectedEpisodeId)
                            ChargementEpisodeContexteConclusion()
                            ChargementContexte()
                            ChargementCaracteristiquesEpisode()
                        End If
                    End Using
                    Me.Enabled = True
                End If
            End If
        End If
    End Sub


    '=======================================================================================
    ' Consigne IDE
    '=======================================================================================

    'Créer une consigne IDE 
    Private Sub RadBtnConclusionCreerConsigne_Click(sender As Object, e As EventArgs) Handles RadBtnConclusionCreerConsigne.Click
        Dim SelectedDrcId As Integer
        Cursor.Current = Cursors.WaitCursor
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = DrcDao.EnumCategorieOasisCode.ActeParamedical
            vFDrcSelecteur.ShowDialog()
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si une DORC a été sélectionnée, on créé la consigne médicale
            If SelectedDrcId <> 0 Then
                'Création consigne médicale
                Using form As New RadFEpisodeConsigneIdeDetail
                    form.DrcId = SelectedDrcId
                    form.ShowDialog()
                    If form.CodeRetour = True Then
                        episode.ConclusionMedConsigneDrcId = SelectedDrcId
                        episode.ConclusionMedConsigneDenomination = form.TxtDenominationConsigneIde.Text
                        If episodeDao.ModificationEpisode(episode) = True Then
                            ChargementConclusion()
                        End If
                    End If
                End Using
            End If
        End Using
        Cursor.Current = Cursors.Default
    End Sub


    '======================================================
    '===== Conclusion Paramédicale
    '======================================================

    'Modification commentaire IDE
    Private Sub TxtConclusionIDE_TextChanged(sender As Object, e As EventArgs) Handles TxtConclusionIDE.TextChanged
        commentaireConclusionIdeModified = True
    End Sub

    Private Sub TxtConclusionIDE_Leave(sender As Object, e As EventArgs) Handles TxtConclusionIDE.Leave
        If commentaireConclusionIdeModified = True Then
            ModificationCommentaireConclusionIde()
        End If
    End Sub

    Private Sub ModificationCommentaireConclusionIde()
        If commentaireConclusionIdeModified = True Then
            episode.ObservationParamedical = TxtConclusionIDE.Text
            episodeDao.ModificationEpisode(episode)
            commentaireConclusionIdeModified = False
        End If
    End Sub

    'Modification type conclusion paramédicale
    Private Sub RadioBtnRolePropre_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnRolePropre.CheckedChanged
        If ChargementConclusionEnCours = False Then
            RadioTypeConclusionIdeModified = True
            episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.ROLE_PROPRE.ToString
            ControleMAJTypeConclusionIDE()
        End If
    End Sub

    Private Sub RadioBtnSurProtocole_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnSurProtocole.CheckedChanged
        If ChargementConclusionEnCours = False Then
            RadioTypeConclusionIdeModified = True
            episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.SUR_PROTOCOLE.ToString
            ControleMAJTypeConclusionIDE()
        End If
    End Sub

    Private Sub RadioBtnDemandeAvis_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnDemandeAvis.CheckedChanged
        If ChargementConclusionEnCours = False Then
            RadioTypeConclusionIdeModified = True
            episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString
            ControleMAJTypeConclusionIDE()
        End If
    End Sub

    Private Sub ModificationRadioTypeConclusionIDE()
        If RadioTypeConclusionIdeModified = True Then
            Dim TypeConclusionIde As String
            If RadioBtnDemandeAvis.Checked = True Then
                TypeConclusionIde = EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString
            Else
                If RadioBtnRolePropre.Checked = True Then
                    TypeConclusionIde = EpisodeDao.EnumTypeConclusionParamedicale.ROLE_PROPRE.ToString
                Else
                    TypeConclusionIde = EpisodeDao.EnumTypeConclusionParamedicale.SUR_PROTOCOLE.ToString
                End If
            End If

            episode.ConclusionIdeType = TypeConclusionIde
            episodeDao.ModificationEpisode(episode)
            RadioTypeConclusionIdeModified = False
        End If
    End Sub


    '===========================================================================================================================================
    '=== Clôture de l'épisode
    '===========================================================================================================================================

    Private Sub RadBtnCloture_Click(sender As Object, e As EventArgs) Handles RadBtnCloture.Click
        'Si une demande d'avis est en cours (médicale ou paramédicale), on ne peut pas clôturer l'épisode
        tache = tacheDao.GetDemandeEnCoursByEpisode(SelectedEpisodeId)
        If tache.Id <> 0 Then
            ControleWorkflowEnCoursExistant = True
            MessageBox.Show("Cet épisode ne peut être clôturé tant qu'une demande d'avis est en cours")
            Exit Sub
        End If

        'L'utilisateur doit posséder le même profil que celui qui a créé l'épisode
        If userLog.TypeProfil <> episode.TypeProfil Then
            MessageBox.Show("Vous devez disposer d'un profil '" & episode.TypeProfil &
                            "', pour pouvoir clôturer cet épisode patient" & vbCrLf &
                            "(Vous disposez d'un profil de type '" & userLog.TypeProfil & "')")
            Exit Sub
        End If

        'Si l'épisode a été créé par un profil Médical, la conclusion médicale est requise pour clôturer l'épisode
        If episode.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Then
            If ControleConclusionMedicaleExiste = False Then
                MessageBox.Show("Episode créé par un profil 'Médical', la clôture de l'épisode est impossible tant que la conclusion médicale n'est pas réalisée" & vbCrLf &
                                "(Une conclusion médicale implique d'associer au moins un contexte à l'épisode")
                Exit Sub
            End If
        End If

        'Si une demande d'avis médical existe (en cours ou terminée), une conclusion médicale est requise pour réaliser la clôture
        If ControleDemandeAvisMedicalExiste = True Then
            If ControleConclusionMedicaleExiste = False Then
                MessageBox.Show("Suite à une demande d'avis médical, la clôture de l'épisode est impossible tant que la conclusion médicale n'est pas réalisée" & vbCrLf &
                                "(Une conclusion médicale implique d'associer au moins un contexte à l'épisode")
                Exit Sub
            End If
        End If

        'Si l'épisode a été créé par un profil Paramédical, en cas de Rôle propre ou sur protocole, le commentaire de conclusion paramédicale est requis pour clôturer l'épisode
        If episode.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            If RadioBtnRolePropre.Checked = True Or RadioBtnSurProtocole.Checked = True Then
                If TxtConclusionIDE.Text = "" Then
                    MessageBox.Show("Pour un épisode de type 'Paramédical', en cas de conclusion paramédicale de type 'Rôle propre' ou 'Protocole'," & vbCrLf &
                                    " le commentaire de conclusion paramédical est requis pour assurer la clôture de l'épisode.")
                    Exit Sub
                End If
            End If
            'Si l'épisode est PARAMEDICAL et le type de conclusion IDE n'est renseigné (rôle propre, protocole ou sur avis) n'est pas renseigné => Erreur
            If RadioBtnRolePropre.Checked = False AndAlso RadioBtnSurProtocole.Checked = False AndAlso RadioBtnDemandeAvis.Checked = False Then
                MessageBox.Show("Pour un épisode de type 'Paramédical', le type de conclusion paramédicale est requis ('Rôle propre' ou 'Protocole' ou" & vbCrLf &
                                    " 'Demande d'avis') pour assurer la clôture de l'épisode.")
                Exit Sub
            End If
        End If

        'Si une ordonnance existe et que celle-ci n'est pas signée la clôture n'est pas possible
        If ControleOrdonnanceExiste = True Then
            If ControleOrdonnanceValide = False Then
                MessageBox.Show("Une ordonnance existe et n'est pas encore validée, la signature médicale de l'ordonnance est requise pour assurer la clôture de l'épisode.")
                Exit Sub
            End If
        End If

        ClotureEpisode()
    End Sub

    'Vérification si l'épisode peut être clôturé automatiquement
    Private Sub GestionClotureAutomatique()
        If userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            If episode.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
                If ControleDemandeAvisMedicalExiste = True Then
                    If ControleWorkflowEnCoursExistant = False Then
                        If ControleConclusionMedicaleExiste = True Then
                            If ControleOrdonnanceExiste = True AndAlso ControleOrdonnanceValide = False Then
                                'Alerte : si ordonnance existe et n'est pas signée, clôture annulée
                                MessageBox.Show("L'épisode ne peut pas être clôturé tant que l'ordonnance médicale en cours n'est pas signée")
                                Exit Sub
                            End If
                            ClotureEpisode()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ClotureEpisode()
        episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString
        episode.DateModification = Date.Now()
        episode.UserModification = userLog.UtilisateurId
        If episodeDao.ModificationEpisode(episode) = True Then
            If episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.SUR_PROTOCOLE.ToString Then
                Dim contexte As New Antecedent
                Dim contexteHisto As New AntecedentHisto
                Dim contexteDao As New ContexteDao
                contexte.PatientId = episode.PatientId
                contexte.EpisodeId = episode.Id
                contexte.Nature = "Patient"
                contexte.Type = "C"
                contexte.CategorieContexte = "M"
                contexte.StatutAffichage = "P"
                contexte.StatutAffichageTransformation = "P"
                contexte.DrcId = drcIdConclusionIde
                contexte.DateDebut = Date.Now()
                contexte.DateFin = New Date(2999, 12, 31, 0, 0, 0)
                contexte.DateCreation = Date.Now()
                contexte.UserCreation = userLog.UtilisateurId
                contexte.Diagnostic = 1
                contexte.Description = "Contexte IDE : " & episode.ObservationParamedical
                contexteDao.CreationContexte(contexte, contexteHisto)
            End If
            Dim form As New RadFNotification()
            form.Titre = "Notification épisode patient"
            form.Message = "=== Episode clôturé ==="
            form.Show()
            Close()
        End If
    End Sub

    'Annulation de l'épisode
    Private Sub RadBtnAnnulerEpisode_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulerEpisode.Click
        'Annulation interdite si demande d'avis médical existe
        If ControleDemandeAvisMedicalExiste = True Then
            MessageBox.Show("Une demande d'avis médical existe pour cet épisode, l'annulation de l'épisode n'est pas permise.")
            Exit Sub
        End If

        'Annulation interdite si workflow en cours existant
        If ControleWorkflowEnCoursExistant = True Then
            MessageBox.Show("Une demande d'avis est en cours pour cet épisode, l'annulation de l'épisode n'est pas permise.")
            Exit Sub
        End If

        'Annulation interdite si conclusion médicale existe
        If ControleConclusionMedicaleExiste = True Then
            MessageBox.Show("La conclusion médicale a été réalisée pour cet épisode, l'annulation de l'épisode n'est pas permise.")
            Exit Sub
        End If

        'Annulation impossible si ordonnance en cours (signée ou non) existe
        If ControleOrdonnanceExiste = True Then
            MessageBox.Show("Une ordonnance existe pour cet épisode, l'annulation de l'épisode n'est pas permise.")
            Exit Sub
        End If

        AnnulationEpisode()
    End Sub

    Private Sub AnnulationEpisode()
        episode.Etat = EpisodeDao.EnumEtatEpisode.ANNULE.ToString
        episode.Inactif = True
        episode.DateModification = Date.Now()
        episode.UserModification = userLog.UtilisateurId
        If episodeDao.ModificationEpisode(episode) = True Then
            Notification.show("Notification épisode patient", "=== Episode annulé ===")
            Close()
        End If
    End Sub


    '===========================================================================================================================================
    '======================= Gestion des actions ==============
    '===========================================================================================================================================
    '=========================================================
    '=== Génération actes paramédicaux et paramètres
    '=========================================================

    'Génération protocoles et paramètres
    Private Sub RadBtnGenProtocole_Click(sender As Object, e As EventArgs) Handles RadBtnGenProtocole.Click
        If MsgBox("Attention, le traitement va entrainer la suppression des éventuels paramètres et actes paramédicaux existants, confirmez-vous le traitement", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            Cursor.Current = Cursors.WaitCursor
            'Suppression préalable des paramètres existants
            episodeParametreDao.SuppressionEpisodeParametreByEpisodeId(SelectedEpisodeId)
            'Suppression préalable des actes paramédicaux existants
            episodeActeParamedicalDao.SuppressionEpisodeActeParamedicalByEpisodeId(SelectedEpisodeId)
            'Génération paramètres et actes paramédicaux
            episodeProtocoleCollaboratifDao.GenerateParametreEtProtocoleCollaboratifByEpisode(episode)
            Cursor.Current = Cursors.Default
            Refresh()
            Dim form As New RadFNotification()
            form.Titre = "Notification épisode"
            form.Message = "Traitement de génération des paramètres et des protocoles terminé"
            form.Show()
        End If
    End Sub


    '=========================================================
    '=== Refresh épisode
    '=========================================================

    'Refresh épisode
    Private Sub RadBtnRefresh_Click(sender As Object, e As EventArgs) Handles RadBtnRefresh.Click
        Refresh()
    End Sub

    Private Sub Refresh()
        Cursor.Current = Cursors.WaitCursor
        ChargementCaracteristiquesEpisode()
        ChargementAffichageBlocWorkflow()
        InitParametre()
        ChargementParametres()
        ChargementSousEpisode()
        refreshButtonSousEpisodeProperties()
        ChargementObservationSpecifique()
        ChargementObservationLibre()
        ChargementConclusion()
        ControleEpisodeCloture()
        Cursor.Current = Cursors.Default
    End Sub


    '===========================================================================================================================================
    '======================= Synthèse ==============================================================================================
    '===========================================================================================================================================

    'Chargement des pages de la synthèse en fonction de leur demande (sauf antécédent qui est la page par défaut)
    Private Sub RadPageView1_SelectedPageChanged(sender As Object, e As EventArgs) Handles RadPageView1.SelectedPageChanged
        'Console.WriteLine("Page sélectionnée : " & RadPageView1.SelectedPage.Name)
        Select Case RadPageView1.SelectedPage.Name
            Case "PgvTraitement"
                If IsTraitementLoaded = False Then
                    ChargementTraitement()
                    IsTraitementLoaded = True
                    'Console.WriteLine("Chargement page : " & RadPageView1.SelectedPage.Name)
                End If
            Case "PgvParcours"
                If IsParcoursLoaded = False Then
                    ChargementParcoursDeSoin()
                    IsParcoursLoaded = True
                    'Console.WriteLine("Chargement page : " & RadPageView1.SelectedPage.Name)
                End If
            Case "PgvContexte"
                If IsContexteLoaded = False Then
                    ChargementContexte()
                    IsContexteLoaded = True
                    'Console.WriteLine("Chargement page : " & RadPageView1.SelectedPage.Name)
                End If
            Case "PgvPPS"
                If IsPPSLoaded = False Then
                    ChargementPPS()
                    IsPPSLoaded = True
                    'Console.WriteLine("Chargement page : " & RadPageView1.SelectedPage.Name)
                End If
        End Select
    End Sub


    '==========================================================
    '======================= Antécédent =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementAntecedent()
        Dim antecedentDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao

        Cursor.Current = Cursors.WaitCursor
        RadAntecedentDataGridView.Rows.Clear()

        If RadChkPublie.Checked = True Then
            If RadChkParPriorite.Checked = True Then
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, True, True)
            Else
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, True, False)
            End If
        Else
            If RadChkParPriorite.Checked = True Then
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, False, True)
            Else
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, False, False)
            End If
        End If

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim indentation As String
        Dim dateDateModification, AldDateFin As Date
        Dim AfficheDateModification As String
        Dim diagnostic As String
        Dim antecedentCache, AldValide, AldValideOK, AldDemandeEnCours As Boolean
        Dim antecedentIdPrecedent1, antecedentIdPrecedent2 As Long
        antecedentIdPrecedent1 = 0
        antecedentIdPrecedent2 = 0

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1

            If RadChkMajeurSeul.Checked = True Then
                If antecedentDataTable.Rows(i)("oa_antecedent_niveau") <> 1 Then
                    Continue For
                End If
            End If

            If RadChkParPriorite.Checked = True Then
                'Détermination de l'indentation à appliquer pour l'affichage de l'antécédent si affichage par priorité
                Select Case antecedentDataTable.Rows(i)("oa_antecedent_niveau")
                    Case 1
                        indentation = ""
                    Case 2
                        indentation = "           > "
                    Case 3
                        indentation = "                        >> "
                    Case Else
                        indentation = ""
                End Select
            Else
                indentation = ""
            End If

            'Recherche si le contexte a été modifié
            AfficheDateModification = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_modification")
                AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
            Else
                If antecedentDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                    dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_creation")
                    AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
                End If
            End If

            'Identification si l'antécédent est caché
            antecedentCache = False
            If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    antecedentCache = True
                End If
            End If

            AldValide = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_valide"), False)
            AldDateFin = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_date_fin"), Nothing)
            AldValideOK = False
            If AldValide = True Then
                If AldDateFin > Date.Now() Then
                    AldValideOK = True
                End If
            End If
            AldDemandeEnCours = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_demande_en_cours"), False)

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadAntecedentDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            diagnostic = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de : "
                Else
                    If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de : "
                    End If
                End If
            End If

            'Dim longueurString As Integer
            'Dim longueurMax As Integer = 100
            Dim antecedentDescription As String

            '===== Affichage antécédent
            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                antecedentDescription = ""
            Else
                antecedentDescription = antecedentDataTable.Rows(i)("oa_antecedent_description")
                antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
                'longueurString = antecedentDescription.Length
                'If longueurString > longueurMax Then
                'longueurString = longueurMax
                'End If
                'antecedentDescription = antecedentDescription.Substring(0, longueurString)
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentDescription").Value = antecedentDataTable.Rows(i)("oa_antecedent_description")
            End If

            Dim DescriptionDrcAld As String = ""
            If AldValideOK Or AldDemandeEnCours Then
                'DescriptionDrcAld = Coalesce(antecedentDataTable.Rows(i)("oa_ald_cim10_description"), "")
            End If

            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription
            '==========

            If antecedentCache = True Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.CornflowerBlue
            Else
                If AldValideOK = True Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.Red
                Else
                    If AldDemandeEnCours = True Then
                        RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.DarkOrange
                    End If
                End If
            End If

            If AldValideOK = True Or AldDemandeEnCours = True Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentAld").Value = "X"
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentAld").Value = ""
            End If

            'Id antécédent
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId").Value = antecedentDataTable.Rows(i)("oa_antecedent_id")

            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentDrcId").Value = antecedentDataTable.Rows(i)("oa_antecedent_drc_id")
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentNiveau").Value = antecedentDataTable.Rows(i)("oa_antecedent_niveau")
            RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage1").Value = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1"), 0)
            RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage2").Value = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage2"), 0)
            RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage3").Value = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage3"), 0)
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentIdNiveau1").Value = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1"), 0)
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentIdNiveau2").Value = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2"), 0)

            'Déplacement horizontal, détermination de l'antécédent précédent
            Select Case antecedentDataTable.Rows(i)("oa_antecedent_niveau")
                Case 1
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentIdPrecedent").Value = antecedentIdPrecedent1
                    antecedentIdPrecedent1 = antecedentDataTable.Rows(i)("oa_antecedent_id")
                    antecedentIdPrecedent2 = 0
                Case 2
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentIdPrecedent").Value = antecedentIdPrecedent2
                    antecedentIdPrecedent2 = antecedentDataTable.Rows(i)("oa_antecedent_id")
                Case 3
                    'Non concerné
            End Select

            'Récupération de l'index du dernier antécédent déplacé pour lui remettre le focus lors du réaffichage de la grid
            If antecedentIdADeplacer <> 0 AndAlso antecedentIdADeplacer = antecedentDataTable.Rows(i)("oa_antecedent_id") Then
                IndexAntecedentADeplacer = iGrid
                antecedentIdADeplacer = 0
            End If

            'Déplacement vertical, détermination de l'antécédent pere si niveau 2 et 3
            Select Case CInt(antecedentDataTable.Rows(i)("oa_antecedent_niveau"))
                Case 2
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentPereId").Value = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1")
                Case 3
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentPereId").Value = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2")
                Case Else
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentPereId").Value = 0
            End Select
        Next

        'Récupération du nombre de lignes stockées dans la Grid
        iGridMax = iGrid

        'Positionnement du grid sur la première occurrence
        If RadAntecedentDataGridView.Rows.Count > 0 Then
            Me.RadAntecedentDataGridView.CurrentRow = RadAntecedentDataGridView.Rows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RafraichirLaListeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RafraichirLaListeToolStripMenuItem.Click
        ChargementAntecedent()
    End Sub

    'Appel de la modification d'un antécédent
    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadAntecedentDataGridView.CellDoubleClick
        ModificationAntecedent()
    End Sub

    Private Sub ModifierUnAntécédentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModifierUnAntécédentToolStripMenuItem.Click
        ModificationAntecedent()
    End Sub

    Private Sub ModificationAntecedent()
        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow, antecedentId As Integer
            aRow = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            If aRow >= 0 Then
                Cursor.Current = Cursors.WaitCursor
                antecedentId = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                Me.Enabled = False
                Using vFAntecedentDetailEdit As New RadFAntecedentDetailEdit
                    vFAntecedentDetailEdit.SelectedAntecedentId = antecedentId
                    vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedentDetailEdit.SelectedDrcId = 0
                    vFAntecedentDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFAntecedentDetailEdit.ShowDialog() 'Modal
                    If vFAntecedentDetailEdit.CodeRetour = True Then
                        ChargementAntecedent()
                        If vFAntecedentDetailEdit.Reactivation = True Then
                            'Rechargement des contextes si réactivation
                            ChargementContexte()
                        End If
                        ChargementToolTipAld()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Créer un antécédent
    Private Sub RadBtnCreationAntecedent_Click(sender As Object, e As EventArgs) Handles RadBtnCreationAntecedent.Click
        CreationAntecedent()
    End Sub

    Private Sub CreerAntecedentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerAntecedentToolStripMenuItem.Click
        CreationAntecedent()
    End Sub

    Private Sub CreationAntecedent()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Dim SelectedDrcId As Integer
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = 1       'Catégorie Oasis : "Antécédent et Contexte"
            vFDrcSelecteur.ShowDialog()             'Modal
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si un DRC a été sélectionné
            If SelectedDrcId <> 0 Then
                Using vFAntecedentDetailEdit As New RadFAntecedentDetailEdit
                    vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedentDetailEdit.SelectedDrcId = SelectedDrcId
                    vFAntecedentDetailEdit.SelectedDrc = vFDrcSelecteur.SelectedDrc
                    vFAntecedentDetailEdit.SelectedAntecedentId = 0
                    vFAntecedentDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFAntecedentDetailEdit.ShowDialog() 'Modal
                    'Si le traitement a été créé, on recharge la grid
                    If vFAntecedentDetailEdit.CodeRetour = True Then
                        ChargementAntecedent()
                        ChargementToolTipAld()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Historique des modifications d'un antécédent
    Private Sub HistoriqueDesModificationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem.Click
        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim AntecedentId As Integer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFAntecedenttHistoListe As New RadFAntecedentHistoListe
                    vFAntecedenttHistoListe.SelectedAntecedentId = AntecedentId
                    vFAntecedenttHistoListe.SelectedPatient = Me.SelectedPatient
                    vFAntecedenttHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedenttHistoListe.ShowDialog() 'Modal
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Traitement du déplacement vertical des antécédents
    'Up
    Private Sub RadBtnUp_Click(sender As Object, e As EventArgs) Handles RadBtnUp.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Dim CodeRetour As Boolean = False
        Dim antecedentId, antecedentIdPere As Integer
        Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
        If aRow >= 0 Then
            Cursor.Current = Cursors.WaitCursor
            antecedentId = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
            antecedentIdADeplacer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
            NiveauAntecedentAOrdonner = Coalesce(RadAntecedentDataGridView.Rows(aRow).Cells("antecedentNiveau").Value, 0)
            antecedentIdPere = Coalesce(RadAntecedentDataGridView.Rows(aRow).Cells("antecedentPereId").Value, 0)
            Select Case NiveauAntecedentAOrdonner
                Case 1
                    RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage1").Value -= 30
                    NouveauOrdreAffichage = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage1").Value
                Case 2
                    RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage2").Value -= 30
                    NouveauOrdreAffichage = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage2").Value
                Case 3
                    RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage3").Value -= 30
                    NouveauOrdreAffichage = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage3").Value
                Case Else
                    Exit Sub
            End Select
            Dim Cacher As String = "P"
            If RadChkTous.Checked = True Then
                Cacher = "C"
            End If
            antecedentChangementOrdreDao.UpdateAntecedent(antecedentId, NouveauOrdreAffichage, NiveauAntecedentAOrdonner)

            AntecedentModificationOrdre(NiveauAntecedentAOrdonner)
            CodeRetour = antecedentChangementOrdreDao.AntecedentReorganisationOrdre(NiveauAntecedentAOrdonner, SelectedPatient.patientId, antecedentIdPere, NiveauAntecedentAOrdonner, Cacher)
            If CodeRetour = True Then
                ChargementAntecedentAvecPositionnementCurseur()
            End If
            Cursor.Current = Cursors.Default
        End If
    End Sub

    'Down
    Private Sub RadBtnDown_Click(sender As Object, e As EventArgs) Handles RadBtnDown.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Dim CodeRetour As Boolean = False
        Dim antecedentId, antecedentIdPere As Integer
        Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
        If aRow >= 0 Then
            Cursor.Current = Cursors.WaitCursor
            antecedentId = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
            antecedentIdADeplacer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
            NiveauAntecedentAOrdonner = Coalesce(RadAntecedentDataGridView.Rows(aRow).Cells("antecedentNiveau").Value, 0)
            antecedentIdPere = Coalesce(RadAntecedentDataGridView.Rows(aRow).Cells("antecedentPereId").Value, 0)
            Select Case NiveauAntecedentAOrdonner
                Case 1
                    RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage1").Value += 30
                    NouveauOrdreAffichage = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage1").Value
                Case 2
                    RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage2").Value += 30
                    NouveauOrdreAffichage = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage2").Value
                Case 3
                    RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage3").Value += 30
                    NouveauOrdreAffichage = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage3").Value
                Case Else
                    Exit Sub
            End Select
            Dim Cacher As String = "P"
            If RadChkTous.Checked = True Then
                Cacher = "C"
            End If
            antecedentChangementOrdreDao.UpdateAntecedent(antecedentId, NouveauOrdreAffichage, NiveauAntecedentAOrdonner)

            AntecedentModificationOrdre(NiveauAntecedentAOrdonner)
            CodeRetour = antecedentChangementOrdreDao.AntecedentReorganisationOrdre(NiveauAntecedentAOrdonner, SelectedPatient.patientId, antecedentIdPere, NiveauAntecedentAOrdonner, Cacher)
            If CodeRetour = True Then
                ChargementAntecedentAvecPositionnementCurseur()
            End If
            Cursor.Current = Cursors.Default
        End If
    End Sub

    'Traitement du changement d'ordre vertical
    Private Sub AntecedentModificationOrdre(NiveauAntecedentAOrdonner As Integer)

        For i = 0 To iGridMax Step 1
            Dim ordreAffichage As Integer
            If RadAntecedentDataGridView.Rows(i).Cells("antecedentNiveau").Value = NiveauAntecedentAOrdonner Then
                Select Case NiveauAntecedentAOrdonner
                    Case 1
                        ordreAffichage = RadAntecedentDataGridView.Rows(i).Cells("ordreAffichage1").Value
                    Case 2
                        ordreAffichage = RadAntecedentDataGridView.Rows(i).Cells("ordreAffichage2").Value
                    Case 3
                        ordreAffichage = RadAntecedentDataGridView.Rows(i).Cells("ordreAffichage3").Value
                    Case Else
                        Exit Sub
                End Select
                Dim AntecedentId As Integer = CInt(RadAntecedentDataGridView.Rows(i).Cells("antecedentId").Value)
                antecedentChangementOrdreDao.UpdateAntecedent(AntecedentId, ordreAffichage, NiveauAntecedentAOrdonner)
            End If
        Next
    End Sub

    'Modifier l'ordre d'un antécédent
    Private Sub ModifierLordreDunAntecedentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModifierLordreDunAntécédentToolStripMenuItem.Click
        'Appel de la gestion de la modification de l'ordre d'un antécédent
        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim AntecedentId As Integer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFAntecedentOrdreSelecteur As New RadFAntecedentOrdreSelecteur
                    vFAntecedentOrdreSelecteur.SelectedPatient = Me.SelectedPatient
                    vFAntecedentOrdreSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedentOrdreSelecteur.AntecedentIdaOrdonner = AntecedentId
                    vFAntecedentOrdreSelecteur.AntecedentDescriptionAOrdonner = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentDescription").Value
                    vFAntecedentOrdreSelecteur.NiveauAntecedentAOrdonner = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentNiveau").Value
                    vFAntecedentOrdreSelecteur.AntecedentIdPere = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentPereId").Value
                    vFAntecedentOrdreSelecteur.PositionGaucheDroite = EnumPosition.Gauche
                    vFAntecedentOrdreSelecteur.ShowDialog() 'Modal
                    'Si le traitement a été modifié, on recharge la grid
                    If vFAntecedentOrdreSelecteur.CodeRetour = True Then
                        ChargementAntecedent()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Traitement du déplacement horizontal des antécédents
    'Flèche droite : recherche de l'antécédent précédent de même niveau, l'antécédent sélectionné devient le fils de l'antécédent précédent
    'Pas d'effet sur un niveau 3 et s'il n'y a pas d'antécédent précédent
    Private Sub RadBtnRight_Click(sender As Object, e As EventArgs) Handles RadBtnRight.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            Cursor.Current = Cursors.WaitCursor
            If aRow >= 0 Then
                Dim antecedentPere As Antecedent
                Dim antecedentId, antecedentIdPere, niveauActuel As Integer
                Dim NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3 As Integer
                Dim Cacher As String = "P"
                If RadChkTous.Checked = True Then
                    Cacher = "C"
                End If
                antecedentId = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                antecedentIdADeplacer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                antecedentIdPere = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentIdPrecedent").Value
                niveauActuel = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentNiveau").Value
                If antecedentIdPere <> 0 Then
                    antecedentPere = antecedentDao.GetAntecedentById(antecedentIdPere)
                    Select Case niveauActuel
                        Case 1 'Passe de niveau 1 à niveau 2 sur antécédent niveau 1 précédent si existe
                            NiveauCible = 2
                            AntecedentId1 = antecedentIdPere
                            AntecedentId2 = 0
                            ordre1 = antecedentPere.Ordre1
                            ordre2 = 990
                            ordre3 = 0
                            antecedentAffectationDao.AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3, SelectedPatient, Cacher)
                            ChargementAntecedentAvecPositionnementCurseur()
                        Case 2 'Passe de niveau 2 à niveau 3 sur antécédent niveau 2 précédent si existe
                            NiveauCible = 3
                            AntecedentId1 = antecedentPere.Niveau1Id
                            AntecedentId2 = antecedentIdPere
                            ordre1 = antecedentPere.Ordre1
                            ordre2 = antecedentPere.Ordre2
                            ordre3 = 990
                            antecedentAffectationDao.AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3, SelectedPatient, Cacher)
                            ChargementAntecedentAvecPositionnementCurseur()
                        Case 3
                            'Pas d'effet
                    End Select
                End If
            End If
            Cursor.Current = Cursors.Default
        End If
    End Sub

    'Flèche gauche : recherche de l'antécédent précédent de même niveau, l'antécédent sélectionné partage le même niveau et le même antécédent père que l'antécédent précédent 
    'Particularité : pas d'antécédent père pour un antécédent de niveau 2 qui passe par conséquent en niveau 1
    'Pas d'effet sur un niveau 1 et s'il n'y a pas d'antécédent précédent
    Private Sub RadBtnLeft_Click(sender As Object, e As EventArgs) Handles RadBtnLeft.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            Cursor.Current = Cursors.WaitCursor
            If aRow >= 0 Then
                Dim antecedentId, antecedentIdPere, niveauActuel As Integer
                Dim NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3 As Integer
                Dim Cacher As String = "P"
                If RadChkTous.Checked = True Then
                    Cacher = "C"
                End If
                antecedentId = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                antecedentIdADeplacer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                antecedentIdPere = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentIdPrecedent").Value
                niveauActuel = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentNiveau").Value
                Select Case niveauActuel
                    Case 1
                        'Pas d'effet
                    Case 2 'Passe du niveau 2 au niveau 1 (Majeur)
                        NiveauCible = 1
                        AntecedentId1 = 0
                        AntecedentId2 = 0
                        antecedentIdPere = 0
                        ordre1 = 990
                        ordre2 = 0
                        ordre3 = 0
                        antecedentAffectationDao.AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3, SelectedPatient, Cacher)
                        ChargementAntecedentAvecPositionnementCurseur()
                    Case 3 'Passe du niveau 3 au niveau 2
                        NiveauCible = 2
                        AntecedentId1 = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentIdNiveau1").Value
                        AntecedentId2 = 0
                        antecedentIdPere = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentIdNiveau1").Value
                        ordre1 = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage1").Value
                        ordre2 = 990
                        ordre3 = 0
                        antecedentAffectationDao.AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3, SelectedPatient, Cacher)
                        ChargementAntecedentAvecPositionnementCurseur()
                End Select
            End If
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub ChargementAntecedentAvecPositionnementCurseur()
        ChargementAntecedent()
        If IndexAntecedentADeplacer <> 0 AndAlso IndexAntecedentADeplacer <= iGridMax Then
            Me.RadAntecedentDataGridView.Rows(IndexAntecedentADeplacer).IsCurrent = True
            Me.RadAntecedentDataGridView.CurrentRow = RadAntecedentDataGridView.Rows(IndexAntecedentADeplacer)
            IndexAntecedentADeplacer = 0
        End If
    End Sub

    'Gestion des options d'affichage des antécédents sur évènement
    Private Sub RadChkPublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPublie.ToggleStateChanged
        If RadChkPublie.Checked = True Then
            RadChkTous.Checked = False
            If InitPublie = True Then
                Application.DoEvents()
                RadAntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            Else
                InitPublie = True
            End If
        Else
            If RadChkTous.Checked = False Then
                RadChkPublie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkTous.ToggleStateChanged
        If RadChkTous.Checked = True Then
            RadChkPublie.Checked = False
            Application.DoEvents()
            RadAntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        Else
            If RadChkPublie.Checked = False Then
                RadChkTous.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkParPriorite_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkParPriorite.ToggleStateChanged
        If RadChkParPriorite.Checked = True Then
            RadChkParChronologie.Checked = False
            If InitParPriorite = True Then
                Application.DoEvents()
                RadAntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            Else
                InitParPriorite = True
            End If
        Else
            If RadChkParChronologie.Checked = False Then
                RadChkParPriorite.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkParChronologie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkParChronologie.ToggleStateChanged
        If RadChkParChronologie.Checked = True Then
            RadChkParPriorite.Checked = False
            Application.DoEvents()
            RadAntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        Else
            If RadChkParPriorite.Checked = False Then
                RadChkParChronologie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkMajeurSeul_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkMajeurSeul.ToggleStateChanged
        If RadChkMajeurSeul.Checked = True Then
            RadChkMajeurTous.Checked = False
            Application.DoEvents()
            RadAntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        Else
            If RadChkMajeurTous.Checked = False Then
                RadChkMajeurSeul.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkMajeurTous_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkMajeurTous.ToggleStateChanged
        If RadChkMajeurTous.Checked = True Then
            RadChkMajeurSeul.Checked = False
            If InitMajeur = True Then
                Application.DoEvents()
                RadAntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            Else
                InitMajeur = True
            End If
        Else
            If RadChkMajeurSeul.Checked = False Then
                RadChkMajeurTous.Checked = True
            End If
        End If
    End Sub

    Private Sub ChargementToolTipAld()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub RadAntecedentDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadAntecedentDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub


    '==========================================================
    '======================= Traitement =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim traitementDataTable As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao

        Cursor.Current = Cursors.WaitCursor
        RadTraitementDataGridView.Rows.Clear()

        traitementDataTable = traitementDao.getTraitementEnCoursbyPatient(Me.SelectedPatient.patientId)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim Base As String
        Dim Posologie As String
        Dim dateFin, dateDebut, dateModification, dateCreation As Date
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim FenetreTherapeutiqueEnCours As Boolean
        Dim FenetreTherapeutiqueAVenir As Boolean

        'Dim Allergie As Boolean = False
        Dim FenetreDateDebut, FenetreDateFin As Date

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Récupération des médicaments déclarés 'allergique' et exclusion de l'affichage
            If traitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    'Allergie = True
                    'SelectedPatient.PatientAllergieDci.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_dci"))
                    'SelectedPatient.PatientAllergieCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))
                    Continue For
                End If
            End If

            'Récupération des médicaments déclarés 'contre-indiqué' et exclusion de l'affichage
            If traitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    'ContreIndication = True
                    'SelectedPatient.PatientContreIndicationDci.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_dci"))
                    'SelectedPatient.PatientContreIndicationCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))
                    Continue For
                End If
            End If

            'Exclusion de l'affichage des traitements déclarés 'arrêté'
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications arrêtés dans la StringCollection
            If traitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    Continue For
                End If
            End If

            'Date de fin
            If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Date début
            If traitementDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                dateDebut = traitementDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Date création
            If traitementDataTable.Rows(i)("oa_traitement_date_creation") IsNot DBNull.Value Then
                dateCreation = traitementDataTable.Rows(i)("oa_traitement_date_creation")
            Else
                dateCreation = "01/01/1900"
            End If

            'Date modification
            If traitementDataTable.Rows(i)("oa_traitement_date_modification") IsNot DBNull.Value Then
                dateModification = traitementDataTable.Rows(i)("oa_traitement_date_modification")
            Else
                dateModification = dateCreation
            End If

            'Exclusion de l'affichage des traitements dont la date de fin est <à la date du jour
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications dans la StringCollection quel que soit leur date de fin
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                Continue For
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique active et à venir
            FenetreTherapeutiqueEnCours = False
            FenetreTherapeutiqueAVenir = False

            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                FenetreDateDebut = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut")
            Else
                FenetreDateDebut = "31/12/2999"
            End If

            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                FenetreDateFin = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin")
            Else
                FenetreDateFin = "01/01/1900"
            End If

            Posologie = ""

            'Existence d'une fenêtre thérapeutique
            Dim FenetreTherapeutiqueExiste As Boolean = False
            Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If traitementDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
                    'Fenêtre thérapeutique en cours, à venir ou obsolète
                    FenetreTherapeutiqueExiste = True
                    If FenetreDateDebut <= dateJouraComparer And FenetreDateFin >= dateJouraComparer Then
                        'Fenêtre thérapeutique en cours
                        FenetreTherapeutiqueEnCours = True
                        Posologie = "Fenêtre Th."
                    Else
                        If FenetreDateDebut > dateJouraComparer Then
                            FenetreTherapeutiqueAVenir = True
                        End If
                    End If
                End If
            End If

            'Formatage de la posologie
            If FenetreTherapeutiqueEnCours = False Then
                Dim PosologieMatinString, PosologieMidiString, PosologieApresMidiString, PosologieSoirString As String
                Dim FractionMatin, FractionMidi, FractionApresMidi, FractionSoir As String
                Dim PosologieBase As String

                FractionMatin = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_matin"), TraitementDao.EnumFraction.Non)
                FractionMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_midi"), TraitementDao.EnumFraction.Non)
                FractionApresMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), TraitementDao.EnumFraction.Non)
                FractionSoir = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_soir"), TraitementDao.EnumFraction.Non)

                posologieMatin = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_matin"), 0)
                posologieMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_midi"), 0)
                posologieApresMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_apres_midi"), 0)
                posologieSoir = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_soir"), 0)

                PosologieBase = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_base"), "")

                If FractionMatin <> "" AndAlso FractionMatin <> TraitementDao.EnumFraction.Non Then
                    If posologieMatin <> 0 Then
                        PosologieMatinString = posologieMatin.ToString & "+" & FractionMatin
                    Else
                        PosologieMatinString = FractionMatin
                    End If
                Else
                    If posologieMatin <> 0 Then
                        PosologieMatinString = posologieMatin.ToString
                    Else
                        PosologieMatinString = "0"
                    End If
                End If

                If FractionMidi <> "" AndAlso FractionMidi <> TraitementDao.EnumFraction.Non Then
                    If posologieMidi <> 0 Then
                        PosologieMidiString = posologieMidi.ToString & "+" & FractionMidi
                    Else
                        PosologieMidiString = FractionMidi
                    End If
                Else
                    If posologieMidi <> 0 Then
                        PosologieMidiString = posologieMidi.ToString
                    Else
                        PosologieMidiString = "0"
                    End If
                End If

                PosologieApresMidiString = ""
                If FractionApresMidi <> "" AndAlso FractionApresMidi <> TraitementDao.EnumFraction.Non Then
                    If posologieApresMidi <> 0 Then
                        PosologieApresMidiString = posologieApresMidi.ToString & "+" & FractionApresMidi
                    Else
                        PosologieApresMidiString = FractionApresMidi
                    End If
                Else
                    If posologieApresMidi <> 0 Then
                        PosologieApresMidiString = posologieApresMidi.ToString
                    End If
                End If

                If FractionSoir <> "" AndAlso FractionSoir <> TraitementDao.EnumFraction.Non Then
                    If posologieSoir <> 0 Then
                        PosologieSoirString = posologieSoir.ToString & "+" & FractionSoir
                    Else
                        PosologieSoirString = FractionSoir
                    End If
                Else
                    If posologieSoir <> 0 Then
                        PosologieSoirString = posologieSoir.ToString
                    Else
                        PosologieSoirString = "0"
                    End If
                End If
                If traitementDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = traitementDataTable.Rows(i)("oa_traitement_posologie_rythme")
                    Select Case PosologieBase
                        Case TraitementDao.EnumBaseCode.JOURNALIER
                            Base = ""
                            If posologieApresMidi <> 0 OrElse FractionApresMidi <> TraitementDao.EnumFraction.Non Then
                                Posologie = Base + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieApresMidiString + ". " + PosologieSoirString
                            Else
                                Posologie = Base + " " + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieSoirString
                            End If
                        Case Else
                            Dim RythmeString As String = ""
                            If FractionMatin <> "" AndAlso FractionMatin <> TraitementDao.EnumFraction.Non Then
                                If Rythme <> 0 Then
                                    RythmeString = Rythme.ToString & "+" & FractionMatin
                                Else
                                    RythmeString = FractionMatin
                                End If
                            Else
                                If Rythme <> 0 Then
                                    RythmeString = Rythme.ToString
                                End If
                            End If
                            Select Case traitementDataTable.Rows(i)("oa_traitement_posologie_base")
                                Case TraitementDao.EnumBaseCode.CONDITIONNEL
                                    Base = "Conditionnel : "
                                Case TraitementDao.EnumBaseCode.HEBDOMADAIRE
                                    Base = "Hebdo : "
                                Case TraitementDao.EnumBaseCode.MENSUEL
                                    Base = "Mensuel : "
                                Case TraitementDao.EnumBaseCode.ANNUEL
                                    Base = "Annuel : "
                                Case Else
                                    Base = "Base inconnue ! "
                            End Select
                            Posologie = Base + RythmeString
                    End Select
                End If
            End If

            Dim commentaire As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_commentaire"), "")
            Dim commentairePosologie As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")

            'Stockage des médicaments prescrits (pour contrôle lors de la selection d'un médicament dans le cadre d'un nouveau traitement
            SelectedPatient.PatientMedicamentsPrescritsCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadTraitementDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            'DCI
            RadTraitementDataGridView.Rows(iGrid).Cells("medicamentDci").Value = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")
            'Posologie
            RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Value = Posologie
            RadTraitementDataGridView.Rows(iGrid).Cells("commentaire").Value = commentaire
            RadTraitementDataGridView.Rows(iGrid).Cells("commentairePosologie").Value = commentairePosologie

            If Posologie = "Fenêtre Th." Then
                RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If

            'Fenêtre thérapeutique existe (en cours ou à venir ou obsolète)
            If FenetreTherapeutiqueExiste = True Then
                RadTraitementDataGridView.Rows(iGrid).Cells("fenetreTherapeutique").Value = "O"
            Else
                RadTraitementDataGridView.Rows(iGrid).Cells("fenetreTherapeutique").Value = ""
            End If

            'Traitement du format d'affichage de la fin du traitement
            If dateDebut = "31/12/2999" Then
                RadTraitementDataGridView.Rows(iGrid).Cells("dateDebut").Value = "Date non définie"
            Else
                RadTraitementDataGridView.Rows(iGrid).Cells("dateDebut").Value = FormatageDateAffichage(dateDebut, True)
            End If

            'Traitement du format d'affichage de modification du traitement
            If dateModification = "01/01/1900" Then
                RadTraitementDataGridView.Rows(iGrid).Cells("dateModification").Value = "Date non définie"
            Else
                RadTraitementDataGridView.Rows(iGrid).Cells("dateModification").Value = FormatageDateAffichage(dateModification, True)
            End If

            'Identifiant du traitement
            RadTraitementDataGridView.Rows(iGrid).Cells("traitementId").Value = traitementDataTable.Rows(i)("oa_traitement_id")

            'CIS du médicament
            RadTraitementDataGridView.Rows(iGrid).Cells("medicamentCis").Value = traitementDataTable.Rows(i)("oa_traitement_medicament_cis")

            'Bouton gérer fenêtre thérapeutique
            If FenetreTherapeutiqueAVenir = True Or FenetreTherapeutiqueEnCours = True Then
                RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If
        Next

        'Traitements arrêtés
        Dim isTraitementArret As Boolean = False
        Dim TraitementArretTooltip As String = ""
        Dim DateArretString, TraitementArretString, TraitementArretMedicament, TraitementArretCommentaire As String
        Dim DateArret As Date
        Dim traitementArretDatatable As DataTable
        traitementArretDatatable = traitementDao.getAllTraitementArreteByPatient(Me.SelectedPatient.patientId)
        rowCount = traitementArretDatatable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            isTraitementArret = True
            TraitementArretMedicament = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_medicament_dci"), "")
            TraitementArretCommentaire = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_arret_commentaire"), "")
            DateArret = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_date_modification"), Nothing)
            DateArretString = outils.FormatageDateAffichage(DateArret, True)
            TraitementArretString = TraitementArretMedicament & " (" & DateArretString & ")  " & TraitementArretCommentaire & vbCrLf
            TraitementArretTooltip += TraitementArretString
        Next
        If isTraitementArret Then
            ToolTip.SetToolTip(LblTraitementArret, TraitementArretTooltip)
            LblTraitementArret.Show()
        Else
            LblTraitementArret.Hide()
        End If

        'Positionnement du grid sur la première occurrence
        If RadTraitementDataGridView.Rows.Count > 0 Then
            Me.RadTraitementDataGridView.CurrentRow = RadTraitementDataGridView.Rows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub RafraichirLécranToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RafraichirLécranToolStripMenuItem.Click
        ChargementTraitement()
    End Sub


    'Tooltip commentaire sur posologie
    Private Sub RadTraitementDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadTraitementDataGridView.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "posologie" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("commentairePosologie").Value
        End If
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "dateModification" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("commentaire").Value
        End If
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "medicamentDci" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("medicamentDci").Value
        End If
    End Sub

    'Création d'un traitement
    Private Sub RadBtnCreationTraitement_Click(sender As Object, e As EventArgs) Handles RadBtnCreationTraitement.Click
        CreationTraitement()
    End Sub

    Private Sub CréerUnTraitementToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CréerUnTraitementToolStripMenuItem1.Click
        CreationTraitement()
    End Sub

    Private Sub CreationTraitement()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Dim SelectedMedicamentCis As Integer
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFMedicamentSelecteur
            form.SelectedPatient = SelectedPatient
            form.ShowDialog() 'Modal
            SelectedMedicamentCis = form.SelectedSpecialiteId
            'Si un médicament a été sélectionné
            If SelectedMedicamentCis <> 0 Then
                Using vFTraitementDetailEdit As New RadFTraitementDetailEdit
                    vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
                    'vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementDetailEdit.SelectedMedicamentId = SelectedMedicamentCis
                    vFTraitementDetailEdit.Allergie = Me.PatientAllergie
                    vFTraitementDetailEdit.ContreIndication = Me.PatientContreIndication
                    vFTraitementDetailEdit.SelectedTraitementId = 0
                    vFTraitementDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFTraitementDetailEdit.ShowDialog() 'Modal
                    'Si le traitement a été créé, on recharge la grid
                    If vFTraitementDetailEdit.CodeRetour = True Then
                        ChargementTraitement()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub


    'Affichage du détail d'un traitement dans un popup
    Private Sub RadTraitementDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadTraitementDataGridView.CellDoubleClick
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId, SelectedMedicamentCis As Integer
                TraitementId = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                SelectedMedicamentCis = RadTraitementDataGridView.Rows(aRow).Cells("MedicamentCis").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False
                Using vFTraitementDetailEdit As New RadFTraitementDetailEdit
                    vFTraitementDetailEdit.SelectedTraitementId = TraitementId
                    vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
                    'vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementDetailEdit.SelectedMedicamentId = SelectedMedicamentCis
                    vFTraitementDetailEdit.Allergie = Me.PatientAllergie
                    vFTraitementDetailEdit.ContreIndication = Me.PatientContreIndication
                    vFTraitementDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFTraitementDetailEdit.ShowDialog() 'Modal
                    If vFTraitementDetailEdit.CodeRetour = True Then
                        ChargementTraitement()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Liste des traitements obsolètes
    Private Sub TraitementsObsoletesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TraitementsObsoletesToolStripMenuItem.Click
        'Traitement : afficher les traitement stoppés dans un popup dédié
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFTraitementObsoletes As New RadFTraitementObsoletes
            vFTraitementObsoletes.SelectedPatient = Me.SelectedPatient
            vFTraitementObsoletes.UtilisateurConnecte = Me.UtilisateurConnecte
            vFTraitementObsoletes.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    'Liste des substances contre-indiquées
    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles LblContreIndication.Click
        ListeContreIndication()
    End Sub

    Private Sub ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Click
        If PatientContreIndication = True Then
            ListeContreIndication()
        End If
    End Sub

    Private Sub ListeContreIndication()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    'Liste des substances allergiques
    Private Sub ListeDesMedicamentsDeclaresAllergiquesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Click
        ListeAllergie()
    End Sub

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        'Traitement : afficher les allergies dans un popup
        If PatientAllergie = True Then
            ListeAllergie()
        End If
    End Sub

    Private Sub ListeAllergie()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientAllergieListe As New RadFPatientAllergieListe
            vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
            vFPatientAllergieListe.ShowDialog() 'Modal
        End Using
        GetAllergie()
        Me.Enabled = True
    End Sub

    'Déclaration d'une contre-indication
    Private Sub DéclarationContreindicationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DéclarationAllergieOuContreindicationToolStripMenuItem.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using formSelecteur As New RadF_CI_ATC_Selecteur
            formSelecteur.SelectedPatient = Me.SelectedPatient
            formSelecteur.ShowDialog() 'Modal
            If formSelecteur.CodeRetour = True Then
                'Contrôle des contre-indications pour les traitements en cours
                Dim MessageContreIndication As String = ""
                Dim PremierPassage As Boolean = True
                Dim contreIndication As Boolean = False
                Dim rowCount As Integer = RadTraitementDataGridView.Rows.Count - 1
                For i = 0 To rowCount Step 1
                    Dim SpecialiteId As Integer = RadTraitementDataGridView.Rows(i).Cells("medicamentCis").Value
                    Dim specialiteContreIndique As SpecialiteContreIndique = TheriaqueDao.IsSpecialiteContreIndique(SelectedPatient, SpecialiteId)
                    If specialiteContreIndique.ContreIndication = True Then
                        contreIndication = True
                        If PremierPassage = True Then
                            PremierPassage = False
                        Else
                            MessageContreIndication += vbCrLf & vbCrLf
                        End If
                        MessageContreIndication += specialiteContreIndique.MessageContreIndication
                    End If
                Next
                If contreIndication = True Then
                    MessageBox.Show(MessageContreIndication)
                End If
            End If
        End Using
        Me.Enabled = True

        GetContreIndication()
    End Sub

    'Déclaration d'une allergie
    Private Sub DéclarationAllergieToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DéclarationAllergieToolStripMenuItem.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using formSelecteur As New RadF_AllergieSelecteur
            formSelecteur.SelectedPatient = Me.SelectedPatient
            formSelecteur.ShowDialog() 'Modal
            If formSelecteur.CodeRetour = True Then
                'Contrôle des allergies pour les traitements en cours
                Dim MessageAllergie As String = ""
                Dim PremierPassage As Boolean = True
                Dim allergie As Boolean = False
                Dim rowCount As Integer = RadTraitementDataGridView.Rows.Count - 1
                For i = 0 To rowCount Step 1
                    Dim SpecialiteId As Integer = RadTraitementDataGridView.Rows(i).Cells("medicamentCis").Value
                    Dim specialiteAllergique As SpecialiteAllergique = TheriaqueDao.IsSpecialiteAllergique(SelectedPatient, SpecialiteId)
                    If specialiteAllergique.Allergie = True Then
                        allergie = True
                        If PremierPassage = True Then
                            PremierPassage = False
                        Else
                            MessageAllergie += vbCrLf & vbCrLf
                        End If
                        MessageAllergie += specialiteAllergique.MessageAllergie
                    End If
                Next
                If allergie = True Then
                    MessageBox.Show(MessageAllergie)
                End If
            End If
        End Using
        Me.Enabled = True

        GetAllergie()
    End Sub

    'Visualisation de l'historique des actions réalisées sur un traitement
    Private Sub HistoriqueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueToolStripMenuItem.Click
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("traitementId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFTraitementHistoListe As New RadFTraitementHistoListe
                    vFTraitementHistoListe.SelectedTraitementId = TraitementId
                    vFTraitementHistoListe.SelectedPatient = Me.SelectedPatient
                    vFTraitementHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementHistoListe.MedicamentDenomination = RadTraitementDataGridView.Rows(aRow).Cells("medicamentDci").Value
                    vFTraitementHistoListe.ShowDialog() 'Modal
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Gestion d'une fenêtre thérapeutique pour un traitement donné
    Private Sub GérerUneFenetreTherapeutiqueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GérerUneFenêtreThérapeutiqueToolStripMenuItem.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim SelectedTraitementId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFTraitementFenetreTh As New RadFTraitementFenetreTh
                    vFTraitementFenetreTh.SelectedPatient = Me.SelectedPatient
                    vFTraitementFenetreTh.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementFenetreTh.SelectedTraitementId = SelectedTraitementId
                    Dim fenetreTherapeutiqueExiste As Char = RadTraitementDataGridView.Rows(aRow).Cells("fenetreTherapeutique").Value
                    If fenetreTherapeutiqueExiste = "O" Then
                        vFTraitementFenetreTh.FenetreTherapeutiqueExiste = True
                    Else
                        vFTraitementFenetreTh.FenetreTherapeutiqueExiste = False
                    End If
                    vFTraitementFenetreTh.ShowDialog() 'Modal
                    'Si le traitement a été créé, on recharge la grid
                    If vFTraitementFenetreTh.CodeRetour = True Then
                        ChargementTraitement()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Ordonnance
    Private Sub RadBtnOrdonnance_Click(sender As Object, e As EventArgs) Handles RadBtnOrdonnance.Click
        GetOrdonnance()
    End Sub

    Private Sub OrdonnanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrdonnanceToolStripMenuItem.Click
        GetOrdonnance()
    End Sub

    Private Sub GetOrdonnance()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Dim OrdonnanceId As Long
        Dim dt As DataTable
        dt = ordonnaceDao.getOrdonnanceValidebyPatient(SelectedPatient.patientId, SelectedEpisodeId)
        If dt.Rows.Count > 0 Then
            'Ordonnance existante
            OrdonnanceId = dt.Rows(0)("oa_ordonnance_id")
            AfficheOrdonnance(OrdonnanceId)
        Else
            If episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString Then
                If episode.DateModification.Date < Date.Now.Date Then
                    MessageBox.Show("Il n'y a pas d'ordonnance de créée pour cet épisode clôturé !")
                    Cursor.Current = Cursors.Default
                    Me.Enabled = True
                    Exit Sub
                End If
            End If
            OrdonnanceId = ordonnaceDao.CreateOrdonnance(SelectedPatient.patientId, SelectedEpisodeId)
            If OrdonnanceId <> 0 Then
                If ordonnaceDao.CreateNewOrdonnanceDetail(SelectedPatient.patientId, OrdonnanceId, episode) = True Then
                    AfficheOrdonnance(OrdonnanceId)
                Else
                    'Erreur, l'ordonnance détail n'a pa été créée
                End If
            Else
                'Erreur, l'ordonnance n'a pa été créée
            End If
        End If
        ChargementEtatEpisode()
        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub AfficheOrdonnance(OrdonnanceId As Long)
        Using vFOrdonnanceListeDetail As New RadFOrdonnanceListeDetail
            vFOrdonnanceListeDetail.SelectedOrdonnanceId = OrdonnanceId
            vFOrdonnanceListeDetail.SelectedPatient = Me.SelectedPatient
            vFOrdonnanceListeDetail.SelectedEpisode = episode
            vFOrdonnanceListeDetail.UtilisateurConnecte = Me.UtilisateurConnecte
            vFOrdonnanceListeDetail.Allergie = Me.PatientAllergie
            vFOrdonnanceListeDetail.ContreIndication = Me.PatientContreIndication
            vFOrdonnanceListeDetail.CommentaireOrdonnance = ""
            vFOrdonnanceListeDetail.ShowDialog()
        End Using
    End Sub


    '================================================================
    '======================= Parcours de soin =======================
    '================================================================

    'Chargement de la Grid
    Private Sub ChargementParcoursDeSoin()
        RadParcoursDataGridView.Rows.Clear()

        Dim ParcoursDataTable As DataTable
        Dim parcoursDao As New ParcoursDao
        Dim tacheDao As New TacheDao
        Dim SousCategorie, SpecialiteId As Integer
        Dim IntervenantOasis As Boolean

        Cursor.Current = Cursors.WaitCursor
        ParcoursDataTable = parcoursDao.getAllParcoursbyPatient(SelectedPatient.patientId)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = ParcoursDataTable.Rows.Count - 1
        Dim SpecialiteDescription As String
        Dim ParcoursCacher, ParcoursConsigneEnRouge As Boolean

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            Dim rorId As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_ror_id"), 0)
            ParcoursCacher = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_cacher"), False)
            If RadChkParcoursNonCache.Checked = True Then
                If ParcoursCacher = True Then
                    Continue For
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadParcoursDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadParcoursDataGridView.Rows(iGrid).Cells("parcoursId").Value = ParcoursDataTable.Rows(i)("oa_parcours_id")

            SpecialiteId = ParcoursDataTable.Rows(i)("oa_parcours_specialite")
            SpecialiteDescription = Environnement.Table_specialite.GetSpecialiteDescription(SpecialiteId)
            RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Value = SpecialiteDescription

            'Nom intervenant et Structure
            IntervenantOasis = False
            ParcoursConsigneEnRouge = False
            SousCategorie = ParcoursDataTable.Rows(i)("oa_parcours_sous_categorie_id")
            Select Case SousCategorie
                Case EnumSousCategoriePPS.medecinReferent
                    IntervenantOasis = True
                    ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.medecinReferent)
                Case EnumSousCategoriePPS.IDE
                    IntervenantOasis = True
                    ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.IDE)
                    Dim pacoursConsigneDao As New ParcoursConsigneDao
                    If pacoursConsigneDao.ExisteParcoursConsigne(ParcoursDataTable.Rows(i)("oa_parcours_id")) = False Then
                        ParcoursConsigneEnRouge = True
                    End If
                Case EnumSousCategoriePPS.sageFemme
                    If ParcoursDataTable.Rows(i)("oa_parcours_intervenant_oasis") = True Then
                        IntervenantOasis = True
                        ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.sageFemmeOasis)
                    End If
                Case EnumSousCategoriePPS.specialiste
            End Select

            If IntervenantOasis = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Value = "Oasis"
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Value = "Oasis"
            Else
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_ror_nom"), "")
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_ror_structure_nom"), "")
            End If

            'Recherche de la dernière consultation
            Dim dateLast, dateNext As Date
            Dim TypeDemandeRdv As String
            'Dim tache As Tache

            RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Value = "-"
            dateLast = Coalesce(ParcoursDataTable.Rows(i)("LastRendezVous"), Nothing)
            If dateLast <> Nothing Then
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Value = outils.FormatageDateAffichage(dateLast, True)
            End If

            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = "-"
            dateNext = Coalesce(ParcoursDataTable.Rows(i)("NextRendezVous"), Nothing)
            If dateNext <> Nothing Then
                'Rendez-vous planifiée
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("dd.MM.yyyy")
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNextHeure").Value = dateNext.ToString("HH:mm")
            Else
                'Recherche si existe demande de rendez-vous
                dateNext = Coalesce(ParcoursDataTable.Rows(i)("DateDemandeRdv"), Nothing)
                If dateNext <> Nothing Then
                    'Rendez-vous prévisionnel, demande en cours
                    TypeDemandeRdv = Coalesce(ParcoursDataTable.Rows(i)("TypeDemandeRdv"), "")
                    Select Case TypeDemandeRdv
                        Case TacheDao.TypeDemandeRendezVous.ANNEE.ToString
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("yyyy")
                        Case TacheDao.TypeDemandeRendezVous.ANNEEMOIS.ToString
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("MM.yyyy")
                        Case Else
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, True)
                    End Select
                Else
                    Dim Rythme As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_rythme"), 0)
                    Dim Base As String = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_base"), 0)
                    If Rythme <> 0 And Base <> "" Then
                        If dateLast <> Nothing Then
                            'Rendez-vous prévisionnel, calculé selon le rythme saisi et le dernier rendez-vous passé
                            dateNext = CalculProchainRendezVous(dateLast, Rythme, Base)
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, False)
                        Else
                            Dim DateCreation As Date = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_date_creation"), Nothing)
                            If DateCreation <> Nothing Then
                                'Rendez-vous prévisionnel, calculé selon le rythme saisi et la date de création de l'intervenant dans le parcours de soin du patient
                                dateNext = CalculProchainRendezVous(DateCreation, Rythme, Base)
                                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, False)
                            Else
                                'Rendez-vous à venir non calculable
                            End If
                        End If
                    Else
                        'Pas de rendez-vous à venir pour cet intervenant
                    End If
                End If
            End If

            RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_commentaire"), "")

            If ParcoursCacher = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationnext").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.CornflowerBlue
            End If

            If ParcoursConsigneEnRouge = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationnext").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.Red
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadParcoursDataGridView.Rows.Count > 0 Then
            Me.RadParcoursDataGridView.CurrentRow = RadParcoursDataGridView.Rows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub MasterTemplate_ToolTipTextNeeded_2(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadParcoursDataGridView.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "consultationNext" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("consultationNextHeure").Value
        End If
    End Sub

    Private Sub RafraichirLaffichageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RafraichirLaffichageToolStripMenuItem.Click
        ChargementParcoursDeSoin()
    End Sub

    'Créer un inetervenant
    Private Sub RadBtnCreationParcours_Click(sender As Object, e As EventArgs) Handles RadBtnCreationParcours.Click
        CreationIntervenant()
    End Sub

    Private Sub CréerUnIntervenantToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnIntervenantToolStripMenuItem.Click
        CreationIntervenant()
    End Sub

    Private Sub CreationIntervenant()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vFSpecialiteSelecteur As New RadFSpecialiteSelecteur
            vFSpecialiteSelecteur.ListProfilOasis = ParcoursListProfilsOasis
            vFSpecialiteSelecteur.ShowDialog()                  'Sélection de spécialité
            If vFSpecialiteSelecteur.SelectedSpecialiteId <> 0 Then
                Using vRadFRorListe As New RadFRorListe
                    vRadFRorListe.Selecteur = True
                    vRadFRorListe.PatientId = Me.SelectedPatient.patientId
                    vRadFRorListe.SpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                    vRadFRorListe.TypeRor = "Intervenant"
                    vRadFRorListe.ShowDialog()                  'Sélection d'un professionnel de santé
                    If vRadFRorListe.CodeRetour = True Then
                        Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                            vFParcoursDetailEdit.SelectedParcoursId = 0
                            vFParcoursDetailEdit.SelectedRorId = vRadFRorListe.SelectedRorId
                            vFParcoursDetailEdit.SelectedSpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                            vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                            'vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFParcoursDetailEdit.RythmeObligatoire = False
                            vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                            vFParcoursDetailEdit.ShowDialog()   'Gestion de l'intervenant
                        End Using
                        ChargementParcoursDeSoin()
                        ChargementPPS()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Modifier un intervenant
    Private Sub RadParcoursDataGridView_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadParcoursDataGridView.CellDoubleClick
        If RadParcoursDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadParcoursDataGridView.Rows.IndexOf(Me.RadParcoursDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ParcoursId As Integer = RadParcoursDataGridView.Rows(aRow).Cells("parcoursId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Try
                    Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                        vFParcoursDetailEdit.SelectedParcoursId = ParcoursId
                        vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                        'vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                        vFParcoursDetailEdit.ShowDialog() 'Modal
                    End Using
                    ChargementParcoursDeSoin()
                    ChargementPPS()
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try
                Me.Enabled = True
            End If
        End If
    End Sub

    'Historique
    Private Sub HistoriqueDesModificationsToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem2.Click
        If RadParcoursDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadParcoursDataGridView.Rows.IndexOf(Me.RadParcoursDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ParcoursId As Integer = RadParcoursDataGridView.Rows(aRow).Cells("parcoursId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vRadFParcoursHistoListe As New RadFParcoursHistoListe
                    vRadFParcoursHistoListe.SelectedParcoursId = ParcoursId
                    vRadFParcoursHistoListe.SelectedPatient = Me.SelectedPatient
                    vRadFParcoursHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vRadFParcoursHistoListe.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadChkParcoursNonCache_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkParcoursNonCache.ToggleStateChanged
        If RadChkParcoursNonCache.Checked = True Then
            RadChkParcoursTous.Checked = False
            If InitParcoursNonCache = True Then
                Application.DoEvents()
                RadParcoursDataGridView.Rows.Clear()
                ChargementParcoursDeSoin()
            Else
                InitParcoursNonCache = True
            End If
        Else
            If RadChkParcoursTous.Checked = False Then
                RadChkParcoursNonCache.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkParcoursTous_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkParcoursTous.ToggleStateChanged
        If RadChkParcoursTous.Checked = True Then
            RadChkParcoursNonCache.Checked = False
            Application.DoEvents()
            RadParcoursDataGridView.Rows.Clear()
            ChargementParcoursDeSoin()
        Else
            If RadChkParcoursNonCache.Checked = False Then
                RadChkParcoursTous.Checked = True
            End If
        End If
    End Sub


    Private Sub RadParcoursDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadParcoursDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    '================================================================
    '======================= Contexte ===============================
    '================================================================

    'Chargement de la Grid
    Private Sub ChargementContexte()
        Dim contexteDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao

        Cursor.Current = Cursors.WaitCursor
        If RadChkContextePublie.Checked = True Then
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedPatient.patientId, True)
        Else
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedPatient.patientId, False)
        End If

        RadContexteDataGridView.Rows.Clear()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateFin, dateModification As Date
        Dim AfficheDateModification, diagnostic As String
        Dim ordreAffichage As Integer
        Dim rowCount As Integer = contexteDataTable.Rows.Count - 1
        Dim categorieContexte, categorieContexteString As String
        Dim contexteCache As Boolean

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categorieContexte = ""
            If contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte") IsNot DBNull.Value Then
                categorieContexte = contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte")
            End If
            Select Case categorieContexte
                Case ContexteDao.EnumParcoursBaseCode.Medical
                    categorieContexteString = ContexteDao.EnumParcoursBaseItem.Medical
                Case ContexteDao.EnumParcoursBaseCode.BioEnvironnemental
                    categorieContexteString = ContexteDao.EnumParcoursBaseItem.BioEnvironnemental
                Case Else
                    categorieContexteString = ""
            End Select

            'DateFin
            If contexteDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                dateFin = contexteDataTable.Rows(i)("oa_antecedent_date_fin")
            Else
                dateFin = "31/12/9999"
            End If

            'Recherche si le contexte a été modifié (médical uniquement)
            AfficheDateModification = ""
            If categorieContexte = "M" Then
                If contexteDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                Else
                    If contexteDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                    End If
                End If
            End If

            'Ordre d'affichage
            If contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                ordreAffichage = contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            Else
                ordreAffichage = 0
            End If

            'Contexte caché
            contexteCache = False
            If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    contexteCache = True
                End If
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadContexteDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadContexteDataGridView.Rows(iGrid).Cells("categorieContexte").Value = categorieContexteString

            diagnostic = ""
            If contexteDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            'Préparation de l'affichage du contexte
            Dim longueurString As Integer
            Dim longueurMax As Integer = 150
            Dim contexteDescription As String
            contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                longueurString = contexteDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                contexteDescription.Substring(0, longueurString)
            End If

            RadContexteDataGridView.Rows(iGrid).Cells("contexte").Value = AfficheDateModification & diagnostic & " " & contexteDescription

            If contexteCache = True Then
                RadContexteDataGridView.Rows(iGrid).Cells("contexte").Style.ForeColor = Color.CornflowerBlue
            End If

            'Identifiant contexte
            RadContexteDataGridView.Rows(iGrid).Cells("contexteId").Value = contexteDataTable.Rows(i)("oa_antecedent_id")
        Next

        'Positionnement du grid sur la première occurrence
        If RadContexteDataGridView.Rows.Count > 0 Then
            Me.RadContexteDataGridView.CurrentRow = RadContexteDataGridView.ChildRows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub RafraichirLaffichageToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RafraichirLaffichageToolStripMenuItem1.Click
        ChargementContexte()
    End Sub

    'Appel détail contexte
    Private Sub MasterTemplate_CellDoubleClick_1(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadContexteDataGridView.CellDoubleClick
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False
                Using vFContexteDetailEdit As New RadFContextedetailEdit
                    vFContexteDetailEdit.SelectedContexteId = ContexteId
                    vFContexteDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFContexteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFContexteDetailEdit.SelectedDrcId = 0
                    vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFContexteDetailEdit.ShowDialog()
                    If vFContexteDetailEdit.CodeRetour = True Then
                        ChargementContexte()
                        ChargementEpisodeContexteConclusion()
                        If vFContexteDetailEdit.ContexteTransformeEnAntecedent = True Then
                            'Rechargement des contextes si réactivation
                            ChargementAntecedent()
                        End If
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Créer un contexte
    Private Sub RadBtnCreationContexte_Click(sender As Object, e As EventArgs) Handles RadBtnCreationContexte.Click
        CreationContexte()
    End Sub

    Private Sub CreerUnContexteMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        CreationContexte()
    End Sub

    Private Sub CreationContexte()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Dim SelectedDrcId As Integer
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = DrcDao.EnumCategorieOasisCode.Contexte
            vFDrcSelecteur.ShowDialog()
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si un médicament a été sélectionné, on appelle le Formulaire de création
            If SelectedDrcId <> 0 Then
                Using vFContexteDetailEdit As New RadFContextedetailEdit
                    vFContexteDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFContexteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFContexteDetailEdit.SelectedDrcId = SelectedDrcId
                    vFContexteDetailEdit.SelectedContexteId = 0
                    vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFContexteDetailEdit.ShowDialog()
                    'Si le traitement a été créé, on recharge la grid
                    If vFContexteDetailEdit.CodeRetour = True Then
                        ChargementContexte()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub HistoriqueDesModificationsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem1.Click
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFAntecedenttHistoListe As New RadFAntecedentHistoListe
                    vFAntecedenttHistoListe.SelectedAntecedentId = ContexteId
                    vFAntecedenttHistoListe.SelectedPatient = Me.SelectedPatient
                    vFAntecedenttHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedenttHistoListe.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadChkContextePublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkContextePublie.ToggleStateChanged
        If RadChkContextePublie.Checked = True Then
            RadChkContexteTous.Checked = False
            If InitContextePublie = True Then
                Application.DoEvents()
                RadContexteDataGridView.Rows.Clear()
                ChargementContexte()
            Else
                InitContextePublie = True
            End If
        Else
            If RadChkContexteTous.Checked = False Then
                RadChkContextePublie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkContexteTous_CheckStateChanged(sender As Object, e As EventArgs) Handles RadChkContexteTous.CheckStateChanged
        If RadChkContexteTous.Checked = True Then
            RadChkContextePublie.Checked = False
            Application.DoEvents()
            RadContexteDataGridView.Rows.Clear()
            ChargementContexte()
        Else
            If RadChkContextePublie.Checked = False Then
                RadChkContexteTous.Checked = True
            End If
        End If
    End Sub


    Private Sub RadContexteDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadContexteDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub


    '===================================================
    '======================= PPS =======================
    '===================================================

    'Chargement de la Grid
    Private Sub ChargementPPS()
        Dim PPSDataTable As DataTable
        Dim PPSDao As PpsDao = New PpsDao

        Cursor.Current = Cursors.WaitCursor
        PPSDataTable = PPSDao.getAllPPSbyPatient(SelectedPatient.patientId)

        RadPPSDataGridView.Rows.Clear()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateDebut, dateModification As Date
        Dim rowCount As Integer = PPSDataTable.Rows.Count - 1
        Dim categoriePPS, sousCategoriePPS, Rythme, SpecialiteId As Integer
        Dim ppsArret As Boolean
        Dim mesureMax As Boolean = False
        Dim NaturePPS, CommentairePPS, commentaireParcours, AffichePPS, AfficheDateModificationPPS, AfficheDateModificationParcours, Base, BaseItem, SpecialiteDescription As String

        PPSSuiviIdeExiste = False
        PPSSuiviMedecinExiste = False
        PPSSuiviSageFemmeExiste = False

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categoriePPS = 0
            If PPSDataTable.Rows(i)("oa_r_pps_categorie_id") IsNot DBNull.Value Then
                categoriePPS = PPSDataTable.Rows(i)("oa_r_pps_categorie_id")
            End If

            sousCategoriePPS = 0
            If PPSDataTable.Rows(i)("oa_r_pps_sous_categorie_id") IsNot DBNull.Value Then
                sousCategoriePPS = PPSDataTable.Rows(i)("oa_r_pps_sous_categorie_id")
            End If

            'Date de début
            If PPSDataTable.Rows(i)("oa_pps_date_debut") IsNot DBNull.Value Then
                dateDebut = PPSDataTable.Rows(i)("oa_pps_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Rythme
            Rythme = Coalesce(PPSDataTable.Rows(i)("oa_parcours_rythme"), 0)
            Base = Coalesce(PPSDataTable.Rows(i)("oa_parcours_base"), "")
            Select Case Base
                Case ParcoursDao.EnumParcoursBaseCode.Quotidien
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.Quotidien
                Case ParcoursDao.EnumParcoursBaseCode.Hebdomadaire
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.Hebdomadaire
                Case ParcoursDao.EnumParcoursBaseCode.ParMois
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.ParMois
                Case ParcoursDao.EnumParcoursBaseCode.ParAn
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.ParAn
                Case ParcoursDao.EnumParcoursBaseCode.TousLes2Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes2Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes3Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes4Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes4Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes5Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes5Ans
                Case Else
                    BaseItem = ""
            End Select

            CommentairePPS = Coalesce(PPSDataTable.Rows(i)("oa_pps_commentaire"), "")
            commentaireParcours = Coalesce(PPSDataTable.Rows(i)("oa_parcours_commentaire"), "")

            'Détecter si les occurrences qui doivent être uniques existent pour ce patient
            If categoriePPS = 3 Then
                Select Case sousCategoriePPS
                    Case 3
                        PPSSuiviIdeExiste = True
                    Case 4
                        PPSSuiviMedecinExiste = True
                    Case 5
                        PPSSuiviSageFemmeExiste = True
                End Select
            End If

            'Recherche si le pps a été modifié
            AfficheDateModificationPPS = ""
            If PPSDataTable.Rows(i)("oa_pps_date_modification") IsNot DBNull.Value Then
                dateModification = PPSDataTable.Rows(i)("oa_pps_date_modification")
                AfficheDateModificationPPS = FormatageDateAffichage(dateModification) + " : "
            Else
                If PPSDataTable.Rows(i)("oa_pps_date_creation") IsNot DBNull.Value Then
                    dateModification = PPSDataTable.Rows(i)("oa_pps_date_creation")
                    AfficheDateModificationPPS = FormatageDateAffichage(dateModification) + " : "
                End If
            End If

            'Recherche si le parcours a été modifié
            AfficheDateModificationParcours = ""
            If PPSDataTable.Rows(i)("oa_parcours_date_modification") IsNot DBNull.Value Then
                dateModification = PPSDataTable.Rows(i)("oa_parcours_date_modification")
                AfficheDateModificationParcours = FormatageDateAffichage(dateModification) + " : "
            Else
                If PPSDataTable.Rows(i)("oa_parcours_date_creation") IsNot DBNull.Value Then
                    dateModification = PPSDataTable.Rows(i)("oa_parcours_date_creation")
                    AfficheDateModificationParcours = FormatageDateAffichage(dateModification) + " : "
                End If
            End If

            'PPS caché
            ppsArret = False
            If PPSDataTable.Rows(i)("oa_pps_arret") IsNot DBNull.Value Then
                If PPSDataTable.Rows(i)("oa_pps_arret") = "1" Then
                    ppsArret = True
                End If
            End If

            NaturePPS = ""
            AffichePPS = ""
            'Présentation PPS : Cible/Objectif de santé (commentaire)
            If categoriePPS = 1 Then
                NaturePPS = "Objectif santé : "
                AffichePPS = NaturePPS + " " + CommentairePPS
            End If

            If categoriePPS = 2 Then
                'Suivi mesures préventives (Code DRC, libellé DRC, commentaire)
                NaturePPS = "Mesures préventives : "
                AffichePPS = NaturePPS & " " & CommentairePPS
            End If

            SpecialiteDescription = ""
            'Présentation PPS : Suivi
            If categoriePPS = 3 Then
                'Un parcours caché ne doit être affiché
                Dim parcoursCache As Boolean = Coalesce(PPSDataTable.Rows(i)("oa_parcours_cacher"), False)
                If parcoursCache = True Then
                    'Continue For
                End If
                'Un suivi intervenant sans rythme ne doit pas être affiché dans le PPS
                If Rythme = 0 Then
                    Continue For
                End If

                'Suivi IDE, Médecin référent, Sage-femme et Spécialiste (Base, Rythme, Commentaire)
                Select Case sousCategoriePPS
                    Case 3
                        NaturePPS = "Suivi IDE : "
                    Case 4
                        NaturePPS = "Suivi médecin télémédecine : "
                    Case 5
                        NaturePPS = "Suivi sage-femme : "
                    Case 6
                        'Récupération spécialité
                        If PPSDataTable.Rows(i)("oa_parcours_specialite") IsNot DBNull.Value Then
                            SpecialiteId = PPSDataTable.Rows(i)("oa_parcours_specialite")
                            SpecialiteDescription = Table_specialite.GetSpecialiteDescription(SpecialiteId)
                        End If
                        NaturePPS = "Suivi " + SpecialiteDescription + " : "
                    Case Else
                        NaturePPS = "Inconnue "
                End Select
                If Base = ParcoursDao.EnumParcoursBaseCode.Hebdomadaire _
                    Or Base = ParcoursDao.EnumParcoursBaseCode.ParMois _
                    Or Base = ParcoursDao.EnumParcoursBaseCode.ParAn Then
                    AffichePPS = NaturePPS + Rythme.ToString + " / " + BaseItem + " " + CommentairePPS
                Else
                    AffichePPS = NaturePPS + BaseItem + " " + CommentairePPS
                End If
            End If

            'Présentation PPS : Stratégie contextuelle (Base, Rythme, Commentaire)
            If categoriePPS = 4 Then
                Select Case sousCategoriePPS
                    Case 7
                        NaturePPS = "Démarche prophylactique "
                    Case 8
                        NaturePPS = "Démarche sociale "
                    Case 9
                        NaturePPS = "Démarche symptomatique "
                    Case 10
                        NaturePPS = "Démarche curative "
                    Case 11
                        NaturePPS = "Démarche diagnostique "
                    Case 12
                        NaturePPS = "Démarche palliative "
                    Case Else
                        NaturePPS = "Inconnue "
                End Select
                AffichePPS = AfficheDateModificationPPS + NaturePPS + " " + CommentairePPS
            End If

            'Transformation des "Tab" et "Return" en espace pour afficher les éléments correctement
            AffichePPS = Replace(AffichePPS, vbTab, " ")
            AffichePPS = Replace(AffichePPS, vbCrLf, " ")

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadPPSDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            If ppsArret = True Then
                RadPPSDataGridView.Rows(iGrid).Cells("pps").Style.ForeColor = Color.Red
            End If

            'Affichage du PPS
            RadPPSDataGridView.Rows(iGrid).Cells("pps").Value = AffichePPS

            'Identifiant pps
            RadPPSDataGridView.Rows(iGrid).Cells("ppsId").Value = PPSDataTable.Rows(i)("oa_pps_id")
            RadPPSDataGridView.Rows(iGrid).Cells("parcoursId").Value = PPSDataTable.Rows(i)("oa_parcours_id")

            RadPPSDataGridView.Rows(iGrid).Cells("categorieId").Value = categoriePPS
            RadPPSDataGridView.Rows(iGrid).Cells("sousCategorieId").Value = sousCategoriePPS
            RadPPSDataGridView.Rows(iGrid).Cells("specialiteId").Value = SpecialiteId
        Next

        'Positionnement du grid sur la première occurrence
        If RadPPSDataGridView.Rows.Count > 0 Then
            Me.RadPPSDataGridView.CurrentRow = RadPPSDataGridView.ChildRows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub RafraichirLaffichageToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles RafraichirLaffichageToolStripMenuItem2.Click
        ChargementPPS()
    End Sub

    Private Sub RadPPSDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadPPSDataGridView.CellDoubleClick
        'Appeler selon la nature du PPS le DetailEdit correspondant
        If RadPPSDataGridView.CurrentRow IsNot Nothing Then
            Dim PPSId, ParcoursId, categoriePPS, sousCategoriePPS, SpecialiteId As Integer
            Dim aRow As Integer = Me.RadPPSDataGridView.Rows.IndexOf(Me.RadPPSDataGridView.CurrentRow)
            If aRow >= 0 Then
                PPSId = RadPPSDataGridView.Rows(aRow).Cells("ppsId").Value
                ParcoursId = RadPPSDataGridView.Rows(aRow).Cells("parcoursId").Value
                categoriePPS = RadPPSDataGridView.Rows(aRow).Cells("categorieId").Value
                sousCategoriePPS = RadPPSDataGridView.Rows(aRow).Cells("sousCategorieId").Value
                SpecialiteId = RadPPSDataGridView.Rows(aRow).Cells("specialiteId").Value

                Select Case categoriePPS
                    Case 1 'Objectif de santé
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using vRadFPPSObjectifSanteDetail As New RadFPPSDetailEdit
                            vRadFPPSObjectifSanteDetail.PPSId = PPSId
                            vRadFPPSObjectifSanteDetail.CategoriePPS = EnumCategoriePPS.Objectif
                            vRadFPPSObjectifSanteDetail.SelectedPatient = Me.SelectedPatient
                            vRadFPPSObjectifSanteDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                            vRadFPPSObjectifSanteDetail.PositionGaucheDroite = EnumPosition.Gauche
                            vRadFPPSObjectifSanteDetail.ShowDialog() 'Modal
                            If vRadFPPSObjectifSanteDetail.CodeRetour = True Then
                                ChargementPPS()
                            End If
                        End Using
                        Me.Enabled = True
                    Case 2 'Mesure préventive
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
                            vFFPPSMesurePreventive.PPSId = PPSId
                            vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
                            vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
                            vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFFPPSMesurePreventive.PositionGaucheDroite = EnumPosition.Gauche
                            vFFPPSMesurePreventive.ShowDialog() 'Modal
                            If vFFPPSMesurePreventive.CodeRetour = True Then
                                ChargementPPS()
                            End If
                        End Using
                        Me.Enabled = True
                    Case 3 'Suivi intervenant
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                            vFParcoursDetailEdit.SelectedParcoursId = ParcoursId
                            vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                            'vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFParcoursDetailEdit.RythmeObligatoire = False
                            vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                            vFParcoursDetailEdit.ShowDialog() 'Modal
                            If vFParcoursDetailEdit.CodeRetour = True Then
                                ChargementParcoursDeSoin()
                                ChargementPPS()
                            End If
                        End Using
                        Me.Enabled = True
                    Case 4 'Stratégie
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using vFPPSStrategie As New RadFPPSDetailEdit
                            vFPPSStrategie.PPSId = PPSId
                            vFPPSStrategie.CategoriePPS = EnumCategoriePPS.Strategie
                            vFPPSStrategie.SelectedPatient = Me.SelectedPatient
                            vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFPPSStrategie.PositionGaucheDroite = EnumPosition.Gauche
                            vFPPSStrategie.ShowDialog() 'Modal
                            If vFPPSStrategie.CodeRetour = True Then
                                ChargementPPS()
                            End If
                        End Using
                        Me.Enabled = True
                End Select
            End If
        End If
    End Sub

    'Création Objectif
    Private Sub RadBtnCreationPPSObjectif_Click(sender As Object, e As EventArgs) Handles RadBtnCreationPPSObjectif.Click
        CreationPPSObjectif()
    End Sub

    Private Sub CréerUnObjectifDeSantéToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnObjectifDeSantéToolStripMenuItem.Click
        CreationPPSObjectif()
    End Sub

    Private Sub CreationPPSObjectif()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        'Contrôler si un objectif de santé valide existe
        Dim ppsdao As New PpsDao
        If ppsdao.ExistPPSObjectifByPatientId(SelectedPatient.patientId) = False Then
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vRadFPPSObjectifSanteDetail As New RadFPPSDetailEdit
                vRadFPPSObjectifSanteDetail.PPSId = 0
                vRadFPPSObjectifSanteDetail.CategoriePPS = EnumCategoriePPS.Objectif
                vRadFPPSObjectifSanteDetail.SelectedPatient = Me.SelectedPatient
                vRadFPPSObjectifSanteDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                vRadFPPSObjectifSanteDetail.PositionGaucheDroite = EnumPosition.Gauche
                vRadFPPSObjectifSanteDetail.ShowDialog() 'Modal
                If vRadFPPSObjectifSanteDetail.CodeRetour = True Then
                    ChargementPPS()
                End If
            End Using
            Me.Enabled = True
        Else
            MessageBox.Show("Création impossible, un Objectif de santé existe déjà pour ce patient")
        End If
    End Sub

    'Création mesure préventive
    Private Sub RadBtnCreationPPSMesure_Click(sender As Object, e As EventArgs) Handles RadBtnCreationPPSMesure.Click
        CreationMesurePreventive()
    End Sub

    Private Sub CréerUneMesurePréventiveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneMesurePréventiveToolStripMenuItem.Click
        CreationMesurePreventive()
    End Sub

    Private Sub CreationMesurePreventive()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
            vFFPPSMesurePreventive.PPSId = 0
            vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
            vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
            vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
            vFFPPSMesurePreventive.PositionGaucheDroite = EnumPosition.Gauche
            vFFPPSMesurePreventive.ShowDialog() 'Modal
            If vFFPPSMesurePreventive.CodeRetour = True Then
                ChargementPPS()
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Création stratégie contextuelle
    Private Sub RadBtnCreationPPSStrategie_Click(sender As Object, e As EventArgs) Handles RadBtnCreationPPSStrategie.Click
        CreationStrategieContextuelle()
    End Sub

    Private Sub CréerUneStratégieContextuelleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneStratégieContextuelleToolStripMenuItem.Click
        CreationStrategieContextuelle()
    End Sub

    Private Sub CreationStrategieContextuelle()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPPSStrategie As New RadFPPSDetailEdit
            vFPPSStrategie.PPSId = 0
            vFPPSStrategie.CategoriePPS = EnumCategoriePPS.Strategie
            vFPPSStrategie.SelectedPatient = Me.SelectedPatient
            vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPPSStrategie.PositionGaucheDroite = EnumPosition.Gauche
            vFPPSStrategie.ShowDialog() 'Modal
            If vFPPSStrategie.CodeRetour = True Then
                ChargementPPS()
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Création Suivi intervenant
    Private Sub RadBtnCreationPPSSuivi_Click(sender As Object, e As EventArgs) Handles RadBtnCreationPPSSuivi.Click
        CreationSuiviIntervenant()
    End Sub

    Private Sub CréerUnSuiviToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnSuiviToolStripMenuItem.Click
        CreationSuiviIntervenant()
    End Sub

    Private Sub CreationSuiviIntervenant()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using form As New RadFPPSListeParcours
                form.SelectedPatient = Me.SelectedPatient
                form.ShowDialog()
            End Using
            ChargementParcoursDeSoin()
            ChargementPPS()
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub HistoriqueDesModificationsToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem3.Click
        If RadPPSDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadPPSDataGridView.Rows.IndexOf(Me.RadPPSDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim categoriePPS As Integer = RadPPSDataGridView.Rows(aRow).Cells("categorieId").Value
                If categoriePPS = 3 Then
                    Dim ParcoursId As Integer = RadPPSDataGridView.Rows(aRow).Cells("parcoursId").Value
                    Me.Enabled = False
                    Cursor.Current = Cursors.WaitCursor
                    Using vRadFParcoursHistoListe As New RadFParcoursHistoListe
                        vRadFParcoursHistoListe.SelectedParcoursId = ParcoursId
                        vRadFParcoursHistoListe.SelectedPatient = Me.SelectedPatient
                        vRadFParcoursHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                        vRadFParcoursHistoListe.ShowDialog()
                    End Using
                    Me.Enabled = True
                Else
                    Dim PPSId As Integer = RadPPSDataGridView.Rows(aRow).Cells("ppsId").Value
                    Me.Enabled = False
                    Cursor.Current = Cursors.WaitCursor
                    Using vFPPSHistoListe As New RadFPPSHistoListe
                        vFPPSHistoListe.SelectedPPSId = PPSId
                        vFPPSHistoListe.SelectedPatient = Me.SelectedPatient
                        vFPPSHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFPPSHistoListe.ShowDialog()
                    End Using
                    Me.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub RadPPSDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadPPSDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub


    '===========================================================
    '======================= Généralités =======================
    '===========================================================
    Private Sub InitZones()
        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        'Etat civil
        LblPatientNIR.Text = ""
        LblPatientPrenom.Text = ""
        LblPatientNom.Text = ""
        LblPatientAge.Text = ""
        LblPatientGenre.Text = ""
        LblPatientSite.Text = ""
        LblPatientDateMaj.Text = ""
        'Initialisation des filtres d'affichage pour les antécédents et les contextes
        InitPublie = False
        InitParPriorite = False
        InitMajeur = False
        RadChkPublie.Checked = True
        RadChkParPriorite.Checked = True
        RadChkMajeurTous.Checked = True
        InitContextePublie = False
        RadChkContextePublie.Checked = True
        'Antécédents
        RadAntecedentDataGridView.Rows.Clear()
        'Traitements
        RadTraitementDataGridView.Rows.Clear()
        'Parcours de soin
        InitParcoursNonCache = False
        RadChkParcoursNonCache.Checked = True
        RadParcoursDataGridView.Rows.Clear()
        'Contexte
        RadContexteDataGridView.Rows.Clear()
        'PPS
        RadPPSDataGridView.Rows.Clear()
    End Sub

    Private Sub ChargementParametreApplication()
        'Récupération du nom de l'organisation dans les paramètres de l'application
        Dim LongueurStringAllergieString As String = ConfigurationManager.AppSettings("longueurStringAllergie")
        If IsNumeric(LongueurStringAllergieString) Then
            LongueurStringAllergie = CInt(LongueurStringAllergieString)
        Else
            LongueurStringAllergie = 12
            CreateLog("Paramètre application 'longueurStringAllergie' non trouvé !", "Episode", LogDao.EnumTypeLog.ERREUR.ToString)
        End If

        Dim drcIdConclusionIdeString As String = ConfigurationManager.AppSettings("drcIdConclusionIde")
        If IsNumeric(drcIdConclusionIdeString) Then
            drcIdConclusionIde = CInt(drcIdConclusionIdeString)
        Else
            drcIdConclusionIde = 128001
            CreateLog("Paramètre application 'drcIdConclusionIde' non trouvé !", "Episode", LogDao.EnumTypeLog.ERREUR.ToString)
        End If
    End Sub

    Private Sub RadPanel12_Paint(sender As Object, e As PaintEventArgs) Handles RadPanel12.Paint

    End Sub

    '===========================================================
    '======================= Droits d'accès ====================
    '===========================================================
    Private Sub DroitAcces()
        If episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString Then
            RadBtnCloture.Enabled = False
        End If

        If episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString AndAlso episode.DateModification.Date < Date.Now.Date Then
            InhibeAccesIDE()
            InhibeAccesMed()
        Else
            Select Case userLog.TypeProfil
                Case ProfilDao.EnumProfilType.MEDICAL.ToString
                    InhibeAccesIDE()
                Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                    InhibeAccesMed()
                    If episode.TypeActivite <> EpisodeDao.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE Then
                        AjoutProtocoleAiguToolStripMenuItem.Visible = False
                    End If
            End Select

            If userLog.UtilisateurAdmin = False Then
                RadBtnGenProtocole.Hide()
            End If
        End If

        If episode.Etat <> EpisodeDao.EnumEtatEpisode.CLOTURE.ToString Then
            If userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Or userLog.UtilisateurAdmin = True Then
                'LibereAccesMed()
            End If

            If userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Or userLog.UtilisateurAdmin = True Then
                'LibereAccesIde()
            End If

            If userLog.UtilisateurAdmin = False Then
                RadBtnGenProtocole.Hide()
            End If
        End If

        'Synthèse
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            'Antécédent
            RadBtnCreationAntecedent.Enabled = False
            RadBtnUp.Enabled = False
            RadBtnDown.Enabled = False
            RadBtnRight.Enabled = False
            RadBtnLeft.Enabled = False
            CréerAntecedentToolStripMenuItem.Enabled = False
            ModifierUnAntécédentToolStripMenuItem.Enabled = False
            ModifierLordreDunAntécédentToolStripMenuItem.Enabled = False

            'Traitement
            RadBtnCreationTraitement.Enabled = False
            CréerUnTraitementToolStripMenuItem1.Enabled = False
            GérerUneFenêtreThérapeutiqueToolStripMenuItem.Enabled = False
            DéclarationAllergieOuContreindicationToolStripMenuItem.Enabled = False

            'Parcours
            RadBtnCreationParcours.Enabled = False
            CréerUnIntervenantToolStripMenuItem.Enabled = False

            'Contexte
            RadBtnCreationContexte.Enabled = False
            ToolStripMenuItem1.Enabled = False
            CréerUnIntervenantToolStripMenuItem.Enabled = False

            'PPS
            RadBtnCreationPPSMesure.Enabled = False
            RadBtnCreationPPSObjectif.Enabled = False
            RadBtnCreationPPSStrategie.Enabled = False
            RadBtnCreationPPSSuivi.Enabled = False
            CréerUnObjectifDeSantéToolStripMenuItem.Enabled = False
            CréerUneMesurePréventiveToolStripMenuItem.Enabled = False
            CréerUneStratégieContextuelleToolStripMenuItem.Enabled = False
            CréerUnSuiviToolStripMenuItem.Enabled = False
        End If
    End Sub

    'Contrôle si l'épisode est clôturé et non modifiable (si la date de clôture est < date du jour)
    Private Sub ControleEpisodeCloture()
        If episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString Then
            'Clôture épisode
            RadBtnCloture.Enabled = False

            If episode.DateModification.Date < Date.Now.Date Then
                'Workflow
                RadBtnWorkflowIde.Enabled = False
                RadBtnWorkflowMed.Enabled = False

                'Saisie paramètre
                RadBtnParametre.Enabled = False

                'Regénération paramètres et actes paramédicaux
                RadBtnGenProtocole.Hide()

                'Observations spécifiques IDE
                RadDropDownBtnObseSpeIde.Enabled = False
                SaisieObservationToolStripMenuItem.Enabled = False
                AjoutProtocoleAiguToolStripMenuItem.Enabled = False

                'Observations spécifiques Médicale
                SaisieObservationSpecifiqueMedicaleItem.Enabled = False
                AttributionDesObservationsSpécifiquesToolStripMenuItem.Enabled = False

                'Observations libres (IDE et médicale)
                CréerUneObservationToolStripMenuItem.Enabled = False
                CréerUneObservationToolStripMenuItem1.Enabled = False
                RadBtnAddObsLibreIde.Enabled = False
                RadBtnAddObsLibreMed.Enabled = False
                ObsContextMenuStrip.Visible = False

                'Saisie commentaire conclusion (Paramédicale)
                TxtConclusionIDE.Enabled = False

                'Choix conclusion épisode paramédical (Paramédicale)
                RadioBtnDemandeAvis.Enabled = False
                RadioBtnRolePropre.Enabled = False
                RadioBtnSurProtocole.Enabled = False

                'Consigne IDE (médical)
                RadBtnConclusionCreerConsigne.Enabled = False

                'Contexte de conclusion (Médicale)
                RadBtnConclusion.Enabled = False
                RadBtnConclusionCreerConsigne.Enabled = False
            End If
        End If
    End Sub

    Private Sub InhibeAccesIDE()
        RadBtnWorkflowIde.Enabled = False
        'RadPnlWorkflowIDE.Enabled = False

        TxtConclusionIDE.Enabled = False
        RadioBtnDemandeAvis.Enabled = False
        RadioBtnRolePropre.Enabled = False
        RadioBtnSurProtocole.Enabled = False

        RadObsSpeIdeDataGridView.TableElement.BackColor = Color.WhiteSmoke
        RadGridViewObsIde.TableElement.BackColor = Color.WhiteSmoke

        RadioBtnDemandeAvis.Enabled = False
        RadioBtnRolePropre.Enabled = False
        RadioBtnSurProtocole.Enabled = False
        TxtConclusionIDE.Enabled = False

        'Observation spécifique
        SaisieObservationToolStripMenuItem.Enabled = False
        AjoutProtocoleAiguToolStripMenuItem.Enabled = False
        RadDropDownBtnObseSpeIde.Enabled = False

        'Observation libre
        RadBtnAddObsLibreIde.Enabled = False
    End Sub

    Private Sub InhibeAccesMed()
        RadBtnWorkflowMed.Enabled = False
        RadBtnConclusionCreerConsigne.Enabled = False
        RadBtnConclusion.Enabled = False
        ControleAjoutConclusion = False

        'Observation libre
        RadBtnAddObsLibreMed.Enabled = False

        'Consigne IDE (médical)
        RadBtnConclusionCreerConsigne.Enabled = False

        RadObsSpeMedDataGridView.TableElement.BackColor = Color.WhiteSmoke
        RadGridViewObsMed.TableElement.BackColor = Color.WhiteSmoke
        RadGridViewContexteEpisode.TableElement.BackColor = Color.WhiteSmoke
    End Sub

    Private Sub LibereAccesIde()
        RadBtnWorkflowIde.Enabled = True
        TxtConclusionIDE.Enabled = True
        If episode.ConclusionIdeType = EpisodeDao.EnumTypeConclusionParamedicale.DEMANDE_AVIS.ToString Then
            If ControleDemandeAvisMedicalExiste = True Then
                Exit Sub
            End If
        End If

        RadioBtnDemandeAvis.Enabled = True
        RadioBtnRolePropre.Enabled = True
        RadioBtnSurProtocole.Enabled = True

        RadObsSpeIdeDataGridView.Enabled = True
        RadGridViewObsIde.Enabled = True
        TxtConclusionIDE.Enabled = True
        RadPanelConclusionIdeType.Enabled = True
    End Sub

    Private Sub LibereAccesMed()
        RadBtnWorkflowMed.Enabled = True
        RadBtnConclusionCreerConsigne.Enabled = True
        RadBtnConclusion.Enabled = True

        RadObsSpeMedDataGridView.Enabled = True
        RadGridViewObsMed.Enabled = True
        RadGridViewContexteEpisode.Enabled = True
    End Sub


    '===========================================================
    '======================= Généralités =======================
    '===========================================================

    Private Sub RadBtnSocial_Click(sender As Object, e As EventArgs) Handles RadBtnSocial.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientNoteListe As New RadFPatientNoteListe
            vFPatientNoteListe.TypeNote = EnumTypeNote.Social
            vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
            vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientNoteListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnVaccins_Click(sender As Object, e As EventArgs) Handles RadBtnVaccins.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientNoteListe As New RadFPatientNoteListe
            vFPatientNoteListe.TypeNote = EnumTypeNote.Vaccin
            vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
            vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientNoteListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnNotesMedicales_Click(sender As Object, e As EventArgs) Handles RadBtnNotesMedicales.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientNoteListe As New RadFPatientNoteListe
            vFPatientNoteListe.TypeNote = EnumTypeNote.Medicale
            vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
            vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientNoteListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnDirectives_Click(sender As Object, e As EventArgs) Handles RadBtnDirectives.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientNoteListe As New RadFPatientNoteListe
            vFPatientNoteListe.TypeNote = EnumTypeNote.Directive
            vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
            vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientNoteListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    'Ligne de vie
    Private Sub RadBtnLigneDeVie_Click(sender As Object, e As EventArgs) Handles RadBtnLigneDeVie.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using form As New RadFEpisodeLigneDeVie
            form.SelectedPatient = Me.SelectedPatient
            form.UtilisateurConnecte = Me.UtilisateurConnecte
            form.EpisodeIdDejaOuvert = Me.SelectedEpisodeId
            form.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub
    Private Sub RadBtnSousEpisode_Click(sender As Object, e As EventArgs) Handles RadBtnSousEpisode.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using frm = New FrmSousEpisodeListe(episode, SelectedPatient)
                frm.ShowDialog()
            End Using
            ChargementSousEpisode()
            refreshButtonSousEpisodeProperties()
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    'Appel synthèse
    Private Sub RadBtnSynthèse_Click(sender As Object, e As EventArgs) Handles RadBtnSynthèse.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFSynthese
            form.SelectedPatient = Me.SelectedPatient
            form.UtilisateurConnecte = userLog
            form.EcranPrecedent = EnumAccesEcranPrecedent.SANS
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    'Abandon
    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RefreshButtonSousEpisodeProperties()
        Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
        Dim i = sousEpisodeDao.CountSousEpisode(SelectedEpisodeId)
        If i > 0 Then
            Me.RadBtnSousEpisode.Text = "Sous-épisode (" & i & ")"
            RadBtnSousEpisode.ForeColor = Color.Red
            RadBtnSousEpisode.Font = New Font(RadBtnParametre.Font, FontStyle.Bold)
            ToolTip.SetToolTip(RadBtnSousEpisode, "")
        Else
            RadBtnSousEpisode.ForeColor = Color.FromArgb(21, 66, 139)
            RadBtnSousEpisode.Font = New Font(RadBtnParametre.Font, FontStyle.Regular)
            ToolTip.SetToolTip(RadBtnSousEpisode, "")
        End If


    End Sub

    Private Sub RadBtnSousEpisode_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadBtnSousEpisode.ToolTipTextNeeded
        Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
        e.ToolTipText = sousEpisodeDao.ResumeSousEpisode(SelectedEpisodeId)

    End Sub


End Class
