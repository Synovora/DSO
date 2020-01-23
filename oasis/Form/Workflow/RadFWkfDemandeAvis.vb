Imports Oasis_Common
Public Class RadFWkfDemandeAvis
    Private _selectedEpisodeId As Long
    Private _SelectedTacheId As Long
    Private _selectedPatient As Patient
    Private _Creation As Boolean
    Private _codeRetour As Boolean

    Public Property SelectedEpisodeId As Long
        Get
            Return _selectedEpisodeId
        End Get
        Set(value As Long)
            _selectedEpisodeId = value
        End Set
    End Property

    Public Property SelectedTacheId As Long
        Get
            Return _SelectedTacheId
        End Get
        Set(value As Long)
            _SelectedTacheId = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return _selectedPatient
        End Get
        Set(value As Patient)
            _selectedPatient = value
        End Set
    End Property

    Public Property Creation As Boolean
        Get
            Return _Creation
        End Get
        Set(value As Boolean)
            _Creation = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _codeRetour
        End Get
        Set(value As Boolean)
            _codeRetour = value
        End Set
    End Property

    Dim tacheDao As New TacheDao
    Dim episodeDao As New EpisodeDao
    Dim fonctionDao As New FonctionDao
    Dim userDao As New UserDao

    Dim tache As Tache
    Dim episode As Episode
    Dim user As New Utilisateur
    Dim fonction As Fonction

    Dim FonctionDescription(3) As String
    Dim FonctionId(3) As Long

    Dim TypeEpisode, typeActiviteEpisode, DescriptionActiviteEpisode, UserCreation, DateCreation, UserModification, DateModification As String

    Public Structure EnumAction
        Const CREATION = "Création de demande d'avis"
        Const REPONSE_AVIS = "Réponse à la demande d'avis"
        Const COMPLEMENT = "Demande de complément d'information"
        Const REPONSE_COMPLEMENT = "Réponse à la demande de complément d'information"
        Const VALIDATION = "Validation"
        Const DEMANDE_AVIS = "Relande de la demande d'avis"
    End Structure


    Private Sub RadFWkfDemandeAvis_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afficheTitleForm(Me, "Demande d'avis")
        Me.CodeRetour = False

        ChargementEtatCivil()
        ChargementCaracteristiquesEpisode()
        ChargementDemandeAvis()

    End Sub

    Private Sub ChargementDemandeAvis()
        afficheTitleForm(Me, "Demande d'avis")

        If Creation = True Then
            LblLabelTypeTache.Text = "Création demande d'avis"
            RadGrpCommentaireEmetteur.Hide()
            CbxAction.Items.Clear()
            CbxAction.Items.Add(EnumAction.CREATION)
            CbxAction.Text = EnumAction.CREATION
            CbxAction.Hide()
            'Choix destinataire à remplir dans le comboBox (CbxFonctionDestinataire)
            Dim parcoursDao As New ParcoursDao
            Dim parcoursDT As DataTable
            'Récupération des intervenant Oasis du patient (qui sont les destinaires potentiels)
            parcoursDT = parcoursDao.GetAllIntervenantOasisByPatient(SelectedPatient.patientId)
            Dim i, indice As Integer
            Dim rowCount As Integer = parcoursDT.Rows.Count - 1
            indice = -1
            For i = 0 To rowCount Step 1
                Dim RorId As Integer = Coalesce(parcoursDT.Rows(i)("oa_parcours_ror_id"), 0)
                fonction = fonctionDao.getFonctionByRorId(RorId)
                If fonction.Id <> userLog.FonctionParDefautId Then
                    indice += 1
                    FonctionDescription(indice) = fonction.Designation
                    FonctionId(indice) = fonction.Id
                End If
            Next
            'Chargement du comboBox
            For i = 0 To indice Step 1
                CbxFonctionDestinataire.Items.Clear()
                CbxFonctionDestinataire.Items.Add(FonctionDescription(i))
            Next
            CbxFonctionDestinataire.Text = FonctionDescription(0)
            RadioBtnAsynchrone.Checked = True
            'Chargement de l'emtteur avec le user connecté en création
            Dim userEmetteur As Utilisateur
            userEmetteur = userDao.getUserById(userLog.UtilisateurId)
            LblLabelEmetteur.Text = userEmetteur.UtilisateurPrenom & " " & userEmetteur.UtilisateurNom & " - " & fonction.Type & " / " & fonction.Designation
        Else
            tache = tacheDao.getTacheById(SelectedTacheId)
            Select Case tache.Nature
                Case TacheDao.NatureTache.DEMANDE.ToString
                    LblLabelTypeTache.Text = "Demande d'avis à traiter"
                    CbxAction.Items.Clear()
                    CbxAction.Items.Add(EnumAction.REPONSE_AVIS)
                    CbxAction.Items.Add(EnumAction.COMPLEMENT)
                    CbxAction.Text = EnumAction.REPONSE_AVIS
                Case TacheDao.NatureTache.REPONSE.ToString
                    LblLabelTypeTache.Text = "Rendu d'avis à valider"
                    CbxAction.Items.Clear()
                    CbxAction.Items.Add(EnumAction.VALIDATION)
                    CbxAction.Items.Add(EnumAction.DEMANDE_AVIS)
                    CbxAction.Text = EnumAction.VALIDATION
                Case TacheDao.NatureTache.COMPLEMENT.ToString
                    LblLabelTypeTache.Text = "Demande de complément d'information à traiter"
                    CbxAction.Items.Clear()
                    CbxAction.Items.Add(EnumAction.REPONSE_COMPLEMENT)
                    CbxAction.Text = EnumAction.REPONSE_COMPLEMENT
                    CbxAction.Hide()
            End Select
            fonction = fonctionDao.getFonctionById(tache.EmetteurFonctionId)
            CbxFonctionDestinataire.Text = fonction.Designation
            CbxFonctionDestinataire.Enabled = False
            TxtCommentaireEmetteur.Text = tache.EmetteurCommentaire
            Select Case tache.Priorite
                Case 100
                    RadioBtnAvisUrgent.Checked = True
                Case 200
                    RadioBtnSynchrone.Checked = True
                Case 300
                    RadioBtnAsynchrone.Checked = True
                Case Else
                    RadioBtnAsynchrone.Checked = True
            End Select
            Dim userEmetteur As Utilisateur
            userEmetteur = userDao.getUserById(tache.EmetteurUserId)
            LblLabelEmetteur.Text = userEmetteur.UtilisateurPrenom & " " & userEmetteur.UtilisateurNom & " - " & fonction.Type & " / " & fonction.Designation
        End If
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        Dim NewTache As New Tache
        InitNewTache(NewTache)
        If Creation = True Then
            'Création tache de demande d'avis (DEMANDE)
            NewTache.ParentId = 0
            NewTache.Nature = TacheDao.NatureTache.DEMANDE.ToString

            'Récupération fonction destinataire du comboBox
            Dim i, indice As Integer
            indice = CbxFonctionDestinataire.Items.Count - 1
            For i = 0 To indice Step 1
                If CbxFonctionDestinataire.Text = FonctionDescription(i) Then
                    NewTache.DestinataireFonctionId = FonctionId(i)
                    NewTache.TraiteFonctionId = FonctionId(i)
                    Exit For
                End If
            Next

            If tacheDao.CreationDemandeAvis(NewTache) = True Then
                MessageBox.Show("Demande d'avis créée")
                CodeRetour = True
                Close()
            End If
        Else
            tache = tacheDao.getTacheById(SelectedTacheId)
            NewTache.ParentId = tache.Id
            NewTache.EmetteurFonctionId = tache.DestinataireFonctionId
            NewTache.DestinataireFonctionId = tache.EmetteurFonctionId
            NewTache.TraiteFonctionId = tache.EmetteurFonctionId

            Select Case tache.Nature
                Case TacheDao.NatureTache.DEMANDE.ToString
                    Select Case CbxAction.Text
                        Case EnumAction.REPONSE_AVIS
                            'Création tâche de réponse sur demande d'avis (REPONSE_AVIS) et cloture de la tâche en cours pris en charge par le CreateTache (transaction)
                            NewTache.Nature = TacheDao.NatureTache.REPONSE.ToString
                            If tacheDao.CreateTache(NewTache) = True Then
                                MessageBox.Show("Réponse à la demande d'avis envoyée")
                                CodeRetour = True
                                Close()
                            End If
                            'If tacheDao.ClotureTache(SelectedTacheId, False) = True Then
                            'NewTache.Nature = TacheDao.NatureTache.REPONSE.ToString
                            'If tacheDao.CreateTache(NewTache) = True Then
                            'MessageBox.Show("Réponse à la demande d'avis envoyée")
                            'CodeRetour = True
                            'Close()
                            'End If
                            'End If
                        Case EnumAction.COMPLEMENT
                            'Création tâche de demande de complément d'information (COMPLEMENT) et cloture de la tâche en cours
                            NewTache.Nature = TacheDao.NatureTache.COMPLEMENT.ToString
                            If tacheDao.CreateTache(NewTache) = True Then
                                MessageBox.Show("Demande de complément d'information envoyée")
                                CodeRetour = True
                                Close()
                            End If
                            'If tacheDao.ClotureTache(SelectedTacheId, False) = True Then
                            'NewTache.Nature = TacheDao.NatureTache.COMPLEMENT.ToString
                            'If tacheDao.CreateTache(NewTache) = True Then
                            'MessageBox.Show("Demande de complément d'information envoyée")
                            'CodeRetour = True
                            'Close()
                            'End If
                            'End If
                    End Select
                Case TacheDao.NatureTache.REPONSE.ToString
                    Select Case CbxAction.Text
                        Case EnumAction.VALIDATION
                            'Validation et fin du Workflow, cloture de la tâche en cours
                            If tacheDao.ClotureTache(SelectedTacheId, True) = True Then
                                MessageBox.Show("Demande d'avis terminée")
                                CodeRetour = True
                                Close()
                            End If
                        Case EnumAction.DEMANDE_AVIS
                            'Création tâche de demande d'avis (DEMANDE) et cloture de la tâche en cours
                            NewTache.Nature = TacheDao.NatureTache.DEMANDE.ToString
                            If tacheDao.CreateTache(NewTache) = True Then
                                MessageBox.Show("Réponse à la demande d'avis envoyée")
                                CodeRetour = True
                                Close()
                            End If
                            'If tacheDao.ClotureTache(SelectedTacheId, False) = True Then
                            'NewTache.Nature = TacheDao.NatureTache.DEMANDE.ToString
                            'If tacheDao.CreateTache(NewTache) = True Then
                            'MessageBox.Show("Réponse à la demande d'avis envoyée")
                            'CodeRetour = True
                            'Close()
                            'End If
                            'End If
                    End Select
                Case TacheDao.NatureTache.COMPLEMENT.ToString
                    Select Case CbxAction.Text
                        Case EnumAction.REPONSE_COMPLEMENT
                            'Création tâche de demande d'avis (DEMANDE) et cloture de la tâche en cours
                            NewTache.Nature = TacheDao.NatureTache.DEMANDE.ToString
                            If tacheDao.CreateTache(NewTache) = True Then
                                MessageBox.Show("Relance de la demande d'avis envoyée")
                                CodeRetour = True
                                Close()
                            End If
                            'If tacheDao.ClotureTache(SelectedTacheId, False) = True Then
                            'NewTache.Nature = TacheDao.NatureTache.DEMANDE.ToString
                            'If tacheDao.CreateTache(NewTache) = True Then
                            'MessageBox.Show("Relance de la demande d'avis envoyée")
                            'CodeRetour = True
                            'Close()
                            'End If
                            'End If
                    End Select
                Case Else
                    MessageBox.Show("Erreur de traitement, nature de tâche inconnue : " & tache.Nature)
            End Select
        End If
    End Sub

    Private Sub InitNewTache(NewTache As Tache)
        NewTache.PatientId = _selectedPatient.patientId
        NewTache.EpisodeId = _selectedEpisodeId
        NewTache.EmetteurUserId = userLog.UtilisateurId
        NewTache.EmetteurFonctionId = userLog.FonctionParDefautId
        NewTache.UniteSanitaireId = SelectedPatient.PatientUniteSanitaireId
        NewTache.SiteId = SelectedPatient.PatientSiteId
        NewTache.ParcoursId = 0
        If RadioBtnAvisUrgent.Checked = True Then
            NewTache.Priorite = TacheDao.Priorite.HAUTE
        Else
            If RadioBtnSynchrone.Checked = True Then
                NewTache.Priorite = TacheDao.Priorite.MOYENNE
            Else
                NewTache.Priorite = TacheDao.Priorite.BASSE
            End If
        End If
        NewTache.OrdreAffichage = 10
        NewTache.Categorie = TacheDao.CategorieTache.SOIN.ToString
        NewTache.Type = TacheDao.TypeTache.AVIS_EPISODE.ToString
        NewTache.EmetteurCommentaire = TxtCommentaireDemande.Text
        NewTache.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString
        NewTache.Cloture = False
        NewTache.TypedemandeRendezVous = ""
        NewTache.HorodatageCreation = Date.Now()
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)

        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub ChargementCaracteristiquesEpisode()
        episode = episodeDao.GetEpisodeById(Me.SelectedEpisodeId)
        If episode.DescriptionActivite = "" Then
            LblTypeEpisode.Text = episode.Type.Trim & " / " & episode.TypeActivite
        Else
            LblTypeEpisode.Text = episode.Type.Trim & " / " & episode.TypeActivite & " / " & episode.DescriptionActivite
        End If
        TypeEpisode = episode.Type
        typeActiviteEpisode = episode.TypeActivite
        DescriptionActiviteEpisode = episode.DescriptionActivite
        TxtCommentaireEpisode.Text = episode.Commentaire

        UtilisateurDao.SetUtilisateur(user, episode.UserCreation)
        LblUserCreation.Text = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
        UserCreation = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim & " - " & user.UtilisateurProfilId & " / " & user.TypeProfil
        DateCreation = episode.DateCreation.ToString("dd/MM/yyyy HH:mm")

        If episode.UserModification <> 0 Then
            UtilisateurDao.SetUtilisateur(user, episode.UserModification)
            LBlUserModification.Text = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
            UserModification = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
        Else
            LblLabelUserModification.Text = ""
            LBlUserModification.Text = ""
        End If

        If episode.DateModification <> Nothing Then
            LblDateModification.Text = episode.DateModification.ToString("dd/MM/yyyy HH:mm")
            DateModification = LblDateModification.Text
        Else
            LblLabelDateModification.Text = ""
            LblDateModification.Text = ""
        End If
    End Sub

End Class
