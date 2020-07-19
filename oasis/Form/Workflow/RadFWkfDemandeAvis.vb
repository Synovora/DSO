Imports Oasis_Common
Public Class RadFWkfDemandeAvis
    Private _selectedEpisodeId As Long
    Private _SelectedTacheId As Long
    Private _selectedPatient As PatientBase
    Private _Creation As Boolean
    Private _codeRetour As Boolean
    Private _provenance As EnumProvenance


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

    Public Property SelectedPatient As PatientBase
        Get
            Return _selectedPatient
        End Get
        Set(value As PatientBase)
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

    Public Property Provenance As EnumProvenance
        Get
            Return _provenance
        End Get
        Set(value As EnumProvenance)
            _provenance = value
        End Set
    End Property

    Public Enum EnumProvenance
        AUTRE
        GESTION_TACHE
        EPISODE
        SYNTHESE
    End Enum

    Dim tacheDao As New TacheDao
    'Dim episodeDao As New EpisodeDao
    Dim fonctionDao As New FonctionDao
    'Dim userDao As New UserDao

    Dim tache As Tache
    Dim episode As Episode
    Dim user As New Utilisateur
    Dim fonction As Fonction

    Dim FonctionDescription(3) As String
    Dim FonctionId(3) As Long

    Public Structure EnumAction
        Const CREATION = "Création de demande d'avis"
        Const REPONSE_AVIS = "Réponse à la demande d'avis"
        Const COMPLEMENT = "Demande de précision"
        Const REPONSE_COMPLEMENT = "Réponse à la demande de précision"
        Const VALIDATION = "Validation"
        Const DEMANDE_AVIS = "Relande de la demande d'avis"
    End Structure


    Private Sub RadFWkfDemandeAvis_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Demande d'avis", userLog)
        Me.CodeRetour = False

        'Placement de la fenêtre en bas à droite de l'écran parent
        Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)

        'Affichage des boutons si la fenêtre est appelée depuis un écran autre que épisode, sous-épisode, synthèse et ligne de vie
        If Provenance <> EnumProvenance.AUTRE Then
            RadBtnEpisode.Hide()
            RadBtnSousEpisode.Hide()
            RadBtnSynthèse.Hide()
            RadBtnLigneDeVie.Hide()
        End If

        ChargementEtatCivil()
        ChargementDemandeAvis()
    End Sub

    Private Sub ChargementDemandeAvis()
        AfficheTitleForm(Me, "Demande d'avis", userLog)

        If Creation = True Then
            '===========================================================================================================
            ' Création d'un nouveau Workflow de demande d'avis =========================================================
            '===========================================================================================================
            LblWorkflowDescription.Text =
                "Nouveau Workflow de demande d'avis du " &
                Date.Now().ToString("dd.MM.yyyy") &
                " à " &
                Date.Now().ToString("HH.mm")
            LblLabelTypeTache.Text = "Création demande d'avis"
            'RadPanelEmetteur.Hide()
            LblVersDestinataire.Hide()

            CheckBox1.Hide()
            CheckBox2.Hide()

            RadBtnMessagePrecedent.Hide()

            'Liste des destinataires potentiels à alimenter dans le ComboBox (CbxFonctionDestinataire)
            Dim parcoursDao As New ParcoursDao
            Dim parcoursDT As DataTable

            'Récupération des intervenant Oasis du patient (qui sont les destinaires potentiels)
            parcoursDT = parcoursDao.GetAllIntervenantOasisByPatient(SelectedPatient.patientId)
            Dim i, indice As Integer
            Dim rowCount As Integer = parcoursDT.Rows.Count - 1
            indice = -1
            For i = 0 To rowCount Step 1
                Dim RorId As Integer = Coalesce(parcoursDT.Rows(i)("oa_parcours_ror_id"), 0)
                fonction = fonctionDao.GetFonctionByRorId(RorId)
                If fonction.Id <> userLog.FonctionParDefautId Then
                    indice += 1
                    FonctionDescription(indice) = fonction.Designation
                    FonctionId(indice) = fonction.Id
                End If
            Next

            'Si une seule occurrence est éligible, on l'affiche dans un Label au lieu d'afficher un ComboBox
            If indice = 0 Then
                CbxDestinataireFonction.Items.Clear()
                CbxDestinataireFonction.Items.Add(FonctionDescription(0))
                CbxDestinataireFonction.Text = FonctionDescription(0)
                CbxDestinataireFonction.Hide()
                LblDestinataireFonction.Location = New Point(365, 13)
                LblDestinataireFonction.Text = FonctionDescription(0)
                LblDestinataireLocalisation.Text = GetLocalisation(fonction)
            Else
                LblDestinataireFonction.Hide()
                'Chargement du comboBox
                CbxDestinataireFonction.Items.Clear()
                For i = 0 To indice Step 1
                    CbxDestinataireFonction.Items.Add(FonctionDescription(i))
                Next
                CbxDestinataireFonction.Text = FonctionDescription(0)
                fonction = fonctionDao.GetFonctionById(FonctionId(0))
                LblDestinataireLocalisation.Text = GetLocalisation(fonction)
            End If

            'LblDestinataireNom.Text = "Destinataire de la demande d'avis :"

            RadioBtnAsynchrone.Checked = True
        Else
            '===========================================================================================================
            ' Workflow en cours ========================================================================================
            '===========================================================================================================
            tache = tacheDao.getTacheById(SelectedTacheId)

            '===========================================================================================================
            ' Détermination de la nature du Workflow en cours
            '===========================================================================================================
            Select Case tache.Nature
                Case Tache.NatureTache.DEMANDE.ToString
                    LblLabelTypeTache.Text = "Demande d'avis à traiter"
                    CheckBox1.Text = EnumAction.REPONSE_AVIS
                    CheckBox2.Text = EnumAction.COMPLEMENT
                Case Tache.NatureTache.REPONSE.ToString
                    LblLabelTypeTache.Text = "Rendu d'avis à valider"
                    CheckBox1.Text = EnumAction.VALIDATION
                    CheckBox2.Text = EnumAction.DEMANDE_AVIS
                Case Tache.NatureTache.COMPLEMENT.ToString
                    LblLabelTypeTache.Text = "Demande de précision à traiter"
                    CheckBox1.Text = EnumAction.REPONSE_COMPLEMENT
                    CheckBox1.Hide()
                    CheckBox2.Hide()
            End Select

            '===========================================================================================================
            'Chargement de l'émetteur
            '===========================================================================================================
            'L'émetteur est l'utilisateur qui a émis la tâche : emetteur_user_id de la table Tâche
            'Dim userEmetteur As Utilisateur
            'userEmetteur = userDao.getUserById(tache.EmetteurUserId)
            'LblEmetteurNom.Text = userEmetteur.UtilisateurPrenom.Trim() & " " & userEmetteur.UtilisateurNom.Trim()
            'fonction = fonctionDao.getFonctionById(tache.EmetteurFonctionId)
            'LblEmetteurFonction.Text = fonction.Designation

            'Déterminer la localisation de l'émetteur selon sa fonction (médecin ou sage-femme : fonction libellé, IDE : site)
            'LblEmetteurLocalisation.Text = GetLocalisation(fonction)

            'Commentaire émetteur
            'ToolTip.SetToolTip(RadPanelEmetteur, tache.EmetteurCommentaire)

            '===========================================================================================================
            'Chargement du destinataire
            '===========================================================================================================
            'LblEmetteurNom.Text = userEmetteur.UtilisateurPrenom.Trim() & " " & userEmetteur.UtilisateurNom.Trim()
            'LblDestinataireNom.Text = ""
            fonction = fonctionDao.GetFonctionById(tache.EmetteurFonctionId)
            LblDestinataireFonction.Text = fonction.Designation
            CbxDestinataireFonction.Hide()
            LblDestinataireFonction.Location = New Point(11, 13)

            'Déterminer la localisation du destinataire selon sa fonction (médecin ou sage-femme : fonction libellé, IDE : site)
            LblDestinataireLocalisation.Text = GetLocalisation(fonction)

            'Commentaire émetteur
            ToolTip.SetToolTip(RadPanelDestinataire, tache.EmetteurCommentaire)

            'Nom du destinataire (celui qui traite la tâche)
            'Dim userDestinataire As Utilisateur
            'userDestinataire = userDao.getUserById(tache.TraiteUserId)
            'LblDestinataireNom.Text = userDestinataire.UtilisateurPrenom.Trim() & " " & userDestinataire.UtilisateurNom.Trim()

            'Fonction du destinataire
            'fonction = fonctionDao.getFonctionById(tache.DestinataireFonctionId)
            'CbxDestinataireFonction.Hide()
            'LblDestinataireFonction.Location = New Point(365, 13)
            'LblDestinataireFonction.Text = fonction.Designation

            'Localisation du destinataire selon sa fonction (médecin ou sage-femme : fonction libellé, IDE : site)
            'LblDestinataireLocalisation.Text = GetLocalisation(fonction)

            '===========================================================================================================
            ' Priorité du Workflow
            '===========================================================================================================
            Dim DescriptionPriorite As String
            Select Case tache.Priorite
                Case 100
                    RadioBtnAvisUrgent.Checked = True
                    DescriptionPriorite = " (Urgent)"
                Case 200
                    RadioBtnSynchrone.Checked = True
                    DescriptionPriorite = " (Synchrone)"
                Case 300
                    RadioBtnAsynchrone.Checked = True
                    DescriptionPriorite = " (Asynchrone)"
                Case Else
                    RadioBtnAsynchrone.Checked = True
                    DescriptionPriorite = ""
            End Select

            '===========================================================================================================
            ' Description du Workflow
            '===========================================================================================================
            LblWorkflowDescription.Text = "Workflow n°(" & tache.Id & ") du " &
                tache.HorodatageCreation.ToString("dd.MM.yyyy") &
                " à " &
                tache.HorodatageCreation.ToString("HH.mm") &
                DescriptionPriorite
        End If
    End Sub

    'Détermination de la localisation (destinataire et émetteur du Workflow)
    Private Function GetLocalisation(fonction As Fonction) As String
        Dim DestinataireLocalisation As String

        Select Case fonction.Type
            Case FonctionDao.EnumTypeFonction.PARAMEDICAL.ToString
                DestinataireLocalisation = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
            Case FonctionDao.EnumTypeFonction.MEDICAL.ToString
                DestinataireLocalisation = "TLM"
            Case Else
                DestinataireLocalisation = ""
        End Select

        Return DestinataireLocalisation
    End Function

    'Sélection destinataire (en création)
    Private Sub CbxDestinataireFonction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxDestinataireFonction.SelectedIndexChanged
        fonction = fonctionDao.GetFonctionById(FonctionId(CbxDestinataireFonction.SelectedIndex))
        LblDestinataireLocalisation.Text = GetLocalisation(fonction)
    End Sub


    '===========================================================================================================
    ' Validation du Workflow ===================================================================================
    '===========================================================================================================
    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        Dim NewTache As New Tache
        InitNewTache(NewTache)
        If Creation = True Then
            'Création tache de demande d'avis (DEMANDE)
            NewTache.ParentId = 0
            NewTache.Nature = Tache.NatureTache.DEMANDE.ToString

            'Récupération fonction destinataire du comboBox
            Dim i, indice As Integer
            indice = CbxDestinataireFonction.Items.Count - 1
            For i = 0 To indice Step 1
                If CbxDestinataireFonction.Text = FonctionDescription(i) Then
                    NewTache.DestinataireFonctionId = FonctionId(i)
                    NewTache.TraiteFonctionId = FonctionId(i)
                    Exit For
                End If
            Next

            If tacheDao.CreationDemandeAvis(NewTache, userLog) = True Then
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
                Case Tache.NatureTache.DEMANDE.ToString
                    If CheckBox1.Checked = True Then
                        'Création tâche de réponse sur demande d'avis (REPONSE_AVIS) et cloture de la tâche en cours pris en charge par le CreateTache (transaction)
                        NewTache.Nature = Tache.NatureTache.REPONSE.ToString
                        If tacheDao.CreateTache(NewTache, userLog) = True Then
                            MessageBox.Show("Réponse à la demande d'avis envoyée")
                            CodeRetour = True
                            Close()
                        End If
                    Else
                        If CheckBox2.Checked = True Then
                            'Création tâche de demande de complément d'information (COMPLEMENT) et cloture de la tâche en cours
                            NewTache.Nature = Tache.NatureTache.COMPLEMENT.ToString
                            If tacheDao.CreateTache(NewTache, userLog) = True Then
                                MessageBox.Show("Demande de précision envoyée")
                                CodeRetour = True
                                Close()
                            End If
                        Else
                            MessageBox.Show("Vous devez choisir une option pour valider le Workflow")
                        End If
                    End If
                Case Tache.NatureTache.REPONSE.ToString
                    If CheckBox1.Checked = True Then
                        'Validation et fin du Workflow, cloture de la tâche en cours
                        If tacheDao.ClotureTache(SelectedTacheId, True, userLog) = True Then
                            MessageBox.Show("Validation de la réponse rendue, demande d'avis terminée")
                            CodeRetour = True
                            Close()
                        End If
                    Else
                        If CheckBox2.Checked = True Then
                            'Création tâche de demande d'avis (DEMANDE) et cloture de la tâche en cours
                            NewTache.Nature = Tache.NatureTache.DEMANDE.ToString
                            If tacheDao.CreateTache(NewTache, userLog) = True Then
                                MessageBox.Show("Relance de la demande d'avis envoyée")
                                CodeRetour = True
                                Close()
                            End If
                        Else
                            MessageBox.Show("Vous devez choisir une option pour valider le Workflow")
                        End If
                    End If
                Case Tache.NatureTache.COMPLEMENT.ToString
                    'Création tâche de demande d'avis (DEMANDE) et cloture de la tâche en cours
                    NewTache.Nature = Tache.NatureTache.DEMANDE.ToString
                    If tacheDao.CreateTache(NewTache, userLog) = True Then
                        MessageBox.Show("Réponse à la demande de précision envoyée")
                        CodeRetour = True
                        Close()
                    End If
                Case Else
                    MessageBox.Show("Erreur de traitement, nature de tâche inconnue : " & tache.Nature)
            End Select
        End If
    End Sub


    '===========================================================================================================
    ' Création d'un nouveau Workflow
    '===========================================================================================================
    Private Sub InitNewTache(NewTache As Tache)
        NewTache.PatientId = _selectedPatient.patientId
        NewTache.EpisodeId = _selectedEpisodeId
        NewTache.EmetteurUserId = userLog.UtilisateurId
        NewTache.EmetteurFonctionId = userLog.FonctionParDefautId
        NewTache.UniteSanitaireId = SelectedPatient.PatientUniteSanitaireId
        NewTache.SiteId = SelectedPatient.PatientSiteId
        NewTache.ParcoursId = 0
        If RadioBtnAvisUrgent.Checked = True Then
            NewTache.Priorite = Tache.EnumPriorite.HAUTE
        Else
            If RadioBtnSynchrone.Checked = True Then
                NewTache.Priorite = Tache.EnumPriorite.MOYENNE
            Else
                NewTache.Priorite = Tache.EnumPriorite.BASSE
            End If
        End If
        NewTache.OrdreAffichage = 10
        NewTache.Categorie = Tache.CategorieTache.SOIN.ToString
        NewTache.Type = Tache.TypeTache.AVIS_EPISODE.ToString
        NewTache.EmetteurCommentaire = TxtCommentaireDemande.Text
        NewTache.Etat = Tache.EtatTache.EN_ATTENTE.ToString
        NewTache.Cloture = False
        NewTache.TypedemandeRendezVous = ""
        NewTache.HorodatageCreation = Date.Now()
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNom.Text = SelectedPatient.PatientPrenom & " " & SelectedPatient.PatientNom
        LblPatientDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
        LblPatientAge.Text = SelectedPatient.PatientAge
    End Sub


    '===========================================================================================================
    ' Gestion des boutons d'action
    '===========================================================================================================
    Private Sub RadBtnEpisode_Click(sender As Object, e As EventArgs) Handles RadBtnEpisode.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using form As New RadFEpisodeDetail
            form.SelectedEpisodeId = SelectedEpisodeId
            form.SelectedPatient = Me.SelectedPatient
            form.UtilisateurConnecte = userLog
            form.ShowDialog()
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
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadBtnSynthèse_Click(sender As Object, e As EventArgs) Handles RadBtnSynthèse.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFSynthese
            form.SelectedPatient = SelectedPatient
            form.UtilisateurConnecte = userLog
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnLigneDeVie_Click(sender As Object, e As EventArgs) Handles RadBtnLigneDeVie.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vadFEpisodeListe As New RadFEpisodeLigneDeVie
            vadFEpisodeListe.SelectedPatient = SelectedPatient
            vadFEpisodeListe.UtilisateurConnecte = userLog
            vadFEpisodeListe.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnMessagePrecedent_Click(sender As Object, e As EventArgs) Handles RadBtnMessagePrecedent.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using form As New RadFWkfCommentaire
            form.WorkflowId = SelectedTacheId
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    'Gestion de la réponse
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            CheckBox2.Checked = False
            CheckBox1.Font = New Font(CheckBox1.Font, FontStyle.Bold)
            CheckBox1.ForeColor = Color.Red
            CheckBox2.Font = New Font(CheckBox2.Font, FontStyle.Regular)
            CheckBox2.ForeColor = Color.Black
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            CheckBox1.Checked = False
            CheckBox2.Font = New Font(CheckBox2.Font, FontStyle.Bold)
            CheckBox2.ForeColor = Color.Red
            CheckBox1.Font = New Font(CheckBox1.Font, FontStyle.Regular)
            CheckBox1.ForeColor = Color.Black
        End If
    End Sub
End Class
