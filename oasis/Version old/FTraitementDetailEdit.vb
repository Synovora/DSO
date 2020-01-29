'Détail d'un traitement pour un patient donné

Imports System.Data.SqlClient
Imports Oasis_Common

Public Class FTraitementDetailEdit

    'Variables d'échange avec la Form appelante
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedMedicamentCis As Integer
    Private privateSelectedTraitementId As Integer
    Private privateCodeRetour As Boolean

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

    Public Property SelectedMedicamentCis As Integer
        Get
            Return privateSelectedMedicamentCis
        End Get
        Set(value As Integer)
            privateSelectedMedicamentCis = value
        End Set
    End Property

    Public Property SelectedTraitementId As Integer
        Get
            Return privateSelectedTraitementId
        End Get
        Set(value As Integer)
            privateSelectedTraitementId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return privateCodeRetour
        End Get
        Set(value As Boolean)
            privateCodeRetour = value
        End Set
    End Property

    'Déclaration des variables de travail
    Dim EditMode As Char = ""
    Dim conxn As New SqlConnection(getConnectionString())
    Dim SQLString As String
    Dim medicament_selecteur_cis As Integer
    Private utilisateurCreation As Integer
    Private dateCreationTraitement As Date
    Private traitementArrete As Boolean = False
    Dim TraitementHistoACreer As New TraitementHisto
    Dim UtilisateurHisto As Utilisateur = New Utilisateur()

    Private Sub TraitementDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitZone()
        ChargementEtatCivil()
        If SelectedTraitementId = 0 Then
            EditMode = "C" 'Création
            Me.Text = "Création traitement patient"
            LblstatutTraitement.Hide()
            PnlStatutTraitement.Hide()
            'Récupération du cis du médicament sélectionné
            medicament_selecteur_cis = SelectedMedicamentCis
            'Chargement des données du médicament sélectionné
            ChargementMedoc()
            'Cacher la posologie journalière car la base n'a pas encore été saisie
            CacherPosologieJournaliere()
            '------Initialisation des dates
            'Initialisation de la date de début de traitement à aujourd'hui
            DteTraitementDateDebut.Value = Date.Now
            'Cacher la date de fin de traitement et l'initialiser à la date virtuelle infinie
            DteTraitementDateFin.Format = DateTimePickerFormat.Custom
            DteTraitementDateFin.CustomFormat = " "
            DteTraitementDateFin.Value = New Date(2999, 12, 31, 0, 0, 0)
            'Initialisation de la date de création du traitement à aujourd'hui
            LblTraitementDateCreation.Text = Date.Now.Date.ToString("dd/MM/yyyy")
            'Initialiser et cacher la durée du traitement
            LblLabelTraitementDuree.Hide()
            LblTraitementDuree.Hide()
            'Cacher la date de modification du traitement
            LblTraitementDateModification.Hide()
            LblLabelTraitementDateModification.Hide()
            LblLabelTraitementParModification.Hide()
            LblLabelTraitementDateCreation.Hide()
            LblLabelTraitementParCreation.Hide()
            'Cacher les boutons : Supprimer, Arrêt et Annuler
            BtnAnnulerTraitement.Hide()
            BtnArretTraitement.Hide()
            BtnSupprimerTraitement.Hide()
        Else
            EditMode = "M" 'Modification
            Me.Text = "Mise à jour traitement patient"
            LblstatutTraitement.Hide()
            PnlStatutTraitement.Hide()
            'Chargement du traitement à modifier
            ChargementTraitementExistant()
            'Chargement des données du médicament du traitement
            ChargementMedoc()
        End If

    End Sub


    '=============================================================================================
    '==================================== Gestion des boutons d'action ===========================
    '=============================================================================================

    'Validation des données pour mise à jour de la base de données
    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click
        'Contrôle des zones saisies
        If ControleValiditationDonnees() = True Then
            'Création ou mise à jour de la table
            Select Case EditMode
                Case "C"
                    '-------------Traitement de la création
                    If CreationTraitement() = True Then
                        Me.CodeRetour = True
                        Close()
                    Else
                        MessageBox.Show("Erreur de mise à jour")
                    End If
                Case "M"
                    '-------------Traitement de la modification
                    If ModificationTraitement() = True Then
                        Me.CodeRetour = True
                        Close()
                    End If
            End Select
        End If
    End Sub

    'Appel de la suppression du traitement affiché
    Private Sub BtnSupprimer_Click(sender As Object, e As EventArgs) Handles BtnSupprimerTraitement.Click
        'Contrôle si on doit traiter la suppression ou l'annulation
        'Traitement de la Suppression si la date de création = date de suppression et même auteur, sinon annulation
        If MsgBox("Attention, la suppression d'un traitement est irréversible, confirmez-vous l'annulation", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            Dim dateCreationaComparer As New Date(dateCreationTraitement.Year, dateCreationTraitement.Month, dateCreationTraitement.Day, 0, 0, 0)
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            If dateCreationaComparer = dateJouraComparer Then
                If SuppressionTraitement() = True Then
                    Me.CodeRetour = True
                    Close()
                End If
            Else
                MessageBox.Show("Suppression impossible, un traitement ne peut être supprimé que si sa création à été réalisé dans la journée" + vbCrLf + vbCrLf + "=> Solution : arrêter le traitement ou demander l'aide d'un Admin")
            End If
        End If
    End Sub

    'Retour sur l'écran précédent
    Private Sub BtnRetour_Click(sender As Object, e As EventArgs) Handles BtnRetour.Click
        Close()
    End Sub

    'Affichage de l'écran détaillant un médicament
    Private Sub BtnMedoc_Click(sender As Object, e As EventArgs) Handles BtnMedoc.Click
        Dim vFMedocDetail As New FMedocDetail
        vFMedocDetail.MedicamentCis = LblTraitementMedicamentCIS.Text

        vFMedocDetail.ShowDialog() 'Modal
        vFMedocDetail.Dispose()
    End Sub

    'Annulation du traitement
    Private Sub BtnAnnulerTraitement_Click(sender As Object, e As EventArgs) Handles BtnAnnulerTraitement.Click
        If MsgBox("Attention, l'annulation d'un traitement est à réaliser dans le cas où le patient n'a pas pris le traitement, confirmez-vous l'annulation", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            InhiberZonesDeSaisie()
            AfficherZonesAnnulation()
        End If
    End Sub

    'Validation de l'annulation
    Private Sub BtnValidationAnnulation_Click(sender As Object, e As EventArgs) Handles BtnValidationAnnulation.Click
        If TxtCommentaireAnnulation.Text <> "" Then
            If AnnulationTraitement() = True Then
                Me.CodeRetour = True
                Close()
            End If
        Else
            MessageBox.Show("La saisie du commentaire d'annulation est obligatoire" + vbCrLf + vbCrLf + "=> Solution : Saisissez un commentaire ou appuyer sur ""Retour""")
        End If
    End Sub

    'Arrêt du traitement
    Private Sub BtnArretTraitement_Click(sender As Object, e As EventArgs) Handles BtnArretTraitement.Click
        InhiberZonesDeSaisie()
        AfficherZonesArret()
        DteTraitementDateFin.Enabled = True
    End Sub

    'Confirmation de l'arrêt du traitement
    Private Sub BtnValidationArret_Click(sender As Object, e As EventArgs) Handles BtnValidationArret.Click
        Dim Valide As Boolean = True
        Dim messageErreur As String = ""

        'Vérification que la date de fin de traitement est supérieure ou égale à la date de début
        ControleAffichageDureeTraitement()
        Dim DateDebut, DateFin As Date
        DateDebut = CDate(DteTraitementDateDebut.Value)
        DateFin = CDate(DteTraitementDateFin.Value)
        Dim dateFinaComparer As New Date(DateFin.Year, DateFin.Month, DateFin.Day, 0, 0, 0)
        Dim dateDebutaComparer As New Date(DateDebut.Year, DateDebut.Month, DateDebut.Day, 0, 0, 0)
        Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)

        'Calcul du nombre de jours de différence au cas ou seules les heures sont différentes
        If dateFinaComparer < dateDebutaComparer Then
            Valide = False
            messageErreur = "La date de fin de traitement doit être supérieure ou égale à la date de début"
        End If

        If (dateFinaComparer = dateJouraComparer) Or (dateFinaComparer = dateJouraComparer.AddDays(-1)) Then
        Else
            messageErreur = "La date de fin de traitement doit être égale à la date du jour ou de la veille (J-1)"
            Valide = False
        End If

        'Lancement du traitement de l'arrêt si les données ont correctes
        If Valide = False Then
            MessageBox.Show(messageErreur)
        Else
            If ArretTraitement() = True Then
                Me.CodeRetour = True
                Close()
            End If
        End If
    End Sub

    '=============================================================================================
    '==================================== Mise à jour de la base de données ======================
    '=============================================================================================

    'Contrôle de la validation des données avant mise à jour de la base de données
    Private Function ControleValiditationDonnees() As Boolean
        Dim Valide As Boolean = True
        Dim messageErreur As String = ""
        Dim messageErreur1 As String = ""
        Dim messageErreur2 As String = ""
        Dim messageErreur3 As String = ""
        Dim messageErreur4 As String = ""

        'Base obligatoire
        If CbxTraitementBase.Text = "" Then
            Valide = False
            LblTraitementBase.ForeColor = Color.Red
            messageErreur1 = "- La saisie de la Base de la posologie est obligatoire"
        Else
            Select Case CbxTraitementBase.Text
                Case "Journalier", "Hebdomadaire", "Mensuel", "Annuel"
                    LblTraitementBase.ForeColor = Color.Black
                Case Else
                    LblTraitementBase.ForeColor = Color.Red
                    Valide = False
                    messageErreur1 = "- La saisie de la Base de la posologie est obligatoire"
            End Select
        End If

        'Rythme obligatoire
        If NumRythmeMatin.Value = 0 Then
            LblTraitementRythme.ForeColor = Color.Red
            Valide = False
            messageErreur2 = "- La saisie du rythme de la posologie est obligatoire"
        Else
            LblTraitementRythme.ForeColor = Color.Black
        End If

        'Si rythme journalier, une posologie matin, midi, après-midi ou soir est requise
        If CbxTraitementBase.Text = "Journalier" Then
            If NumRythmeMatin.Value = 0 And NumRythmeMidi.Value = 0 And NumRythmeApresMidi.Value = 0 And NumRythmeSoir.Value = 0 Then
                LblTraitementRythme.ForeColor = Color.Red
                Valide = False
                messageErreur3 = "- La saisie des périodes d'application (matin, midi, après-mid ou soir) de la posologie est obligatoire"
            Else
                LblTraitementRythme.ForeColor = Color.Black
            End If
        End If

        'Vérification que la date de fin de traitement est supérieure ou égale à la date de début
        ControleAffichageDureeTraitement()
        Dim DateDebut, DateFin As Date
        DateDebut = CDate(DteTraitementDateDebut.Value)
        DateFin = CDate(DteTraitementDateFin.Value)
        If Not (DateFin.Year = 2999 And DateFin.Month = 12 And DateFin.Day = 31) Then
            'Calcul du nombre de jours de différence au cas ou seules les heures sont différentes
            Dim DateDifference = DateDiff(DateInterval.Day, DateDebut, DateFin)
            If (DateFin < DateDebut And DateDifference <> 0) Then
                Valide = False
                messageErreur4 = "- La date de fin de traitement doit être supérieure ou égale à la date de début"
            End If
        End If

        'Préparation de l'affichage des erreurs
        If Valide = False Then
            If messageErreur1 <> "" Then
                messageErreur = messageErreur1 + " 
"
            End If

            If messageErreur2 <> "" Then
                messageErreur = messageErreur + messageErreur2 + " 
"
            End If

            If messageErreur3 <> "" Then
                messageErreur = messageErreur + messageErreur3 + " 
"
            End If

            If messageErreur4 <> "" Then
                messageErreur = messageErreur + messageErreur4 + " 
"
            End If

            messageErreur = messageErreur + vbCrLf + vbCrLf + "/!\ Validation impossible, des données sont incorrectes"

            MessageBox.Show(messageErreur)
        End If

        'Contrôler qu'une données a au moins été modifiée
        'contrôle qu'une données a au moins été modifiée

        Return Valide
    End Function

    'Création d'un traitement en base de données
    Private Function CreationTraitement() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        'Initialisation de la barre de progression
        PgbMiseAJour.Show()
        PgbMiseAJour.Style = ProgressBarStyle.Marquee
        PgbMiseAJour.MarqueeAnimationSpeed = 60

        'Définition de la base du traitement
        Dim baseTraitement As Char = DeterminationBaseTraitement()

        'Définition posologie journalière
        Dim PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir As Integer
        Dim Rythme As Integer

        If NumRythmeMatin.Value <> 0 Then
            Rythme = NumRythmeMatin.Value
        Else
            Rythme = 0
        End If

        If baseTraitement = "J" Then
            If NumRythmeMatin.Value <> 0 Then
                PosologieMatin = NumRythmeMatin.Value
            Else
                PosologieMatin = 0
            End If
            If NumRythmeMidi.Value <> 0 Then
                PosologieMidi = NumRythmeMidi.Value
            Else
                PosologieMidi = 0
            End If
            If NumRythmeApresMidi.Value <> 0 Then
                PosologieApresMidi = NumRythmeApresMidi.Value
            Else
                PosologieApresMidi = 0
            End If
            If NumRythmeSoir.Value <> 0 Then
                PosologieSoir = NumRythmeSoir.Value
            Else
                PosologieSoir = 0
            End If
        Else
            PosologieMatin = 0
            PosologieMidi = 0
            PosologieApresMidi = 0
            PosologieSoir = 0
        End If

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_traitement (oa_traitement_patient_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci, oa_traitement_allergie, oa_traitement_contre_indication, oa_traitement_identifiant_creation, oa_traitement_identifiant_modification, oa_traitement_ordre_affichage, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_date_creation, oa_traitement_posologie_commentaire, oa_traitement_commentaire, oa_traitement_date_debut, oa_traitement_date_fin) VALUES (@patientId, @cis, @dci, @allergie, @contreIndication, @utilisateurCreation, @utilisateurModification, @ordreAffichage, @posologieBase, @posologierythme, @PosologieMatin, @PosologieMidi, @PosologieApresMidi, @PosologieSoir, @dateCreation, @posologieCommentaire, @traitementCommentaire, @dateDebut, @dateFin)"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
            .AddWithValue("@cis", medicament_selecteur_cis.ToString)
            .AddWithValue("@dci", LblMedicamentDCI.Text)
            .AddWithValue("@allergie", 0)
            .AddWithValue("@contreIndication", 0)
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@utilisateurModification", 0)
            .AddWithValue("@ordreAffichage", NumNumeroOrdre.Value.ToString)
            .AddWithValue("@posologieBase", baseTraitement)
            .AddWithValue("@posologieRythme", NumRythmeMatin.Value.ToString)
            .AddWithValue("@posologieMatin", PosologieMatin.ToString)
            .AddWithValue("@posologieMidi", PosologieMidi.ToString)
            .AddWithValue("@posologieApresMidi", PosologieApresMidi.ToString)
            .AddWithValue("@posologieSoir", PosologieSoir.ToString)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@posologieCommentaire", TxtTraitementPosologieCommentaire.Text)
            .AddWithValue("@traitementCommentaire", TxtTraitementCommentaire.Text)
            .AddWithValue("@dateDebut", DteTraitementDateDebut.Value)
            .AddWithValue("@dateFin", DteTraitementDateFin.Value)
        End With

        Try
            conxn.Open()
            Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
            PgbMiseAJour.Hide()
            MessageBox.Show("Traitement patient créé")
        Catch ex As Exception
            PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation traitement
            TraitementHistoACreer.HistorisationDate = DateTime.Now()
            TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.CreationTraitement
            TraitementHistoACreer.HistorisationOrdreAffichage = NumNumeroOrdre.Value
            TraitementHistoACreer.HistorisationPosologieBase = baseTraitement
            TraitementHistoACreer.HistorisationPosologieRythme = NumRythmeMatin.Value
            TraitementHistoACreer.HistorisationPosologieMatin = PosologieMatin
            TraitementHistoACreer.HistorisationPosologieMidi = PosologieMidi
            TraitementHistoACreer.HistorisationPosologieApresMidi = PosologieApresMidi
            TraitementHistoACreer.HistorisationPosologieSoir = PosologieSoir
            TraitementHistoACreer.HistorisationPosologieCommentaire = TxtTraitementPosologieCommentaire.Text
            TraitementHistoACreer.HistorisationCommentaire = TxtTraitementCommentaire.Text
            TraitementHistoACreer.HistorisationDateDebut = DteTraitementDateDebut.Value
            TraitementHistoACreer.HistorisationDateFin = DteTraitementDateFin.Value
            TraitementHistoACreer.HistorisationAllergie = False
            TraitementHistoACreer.HistorisationContreIndication = False

            'Récupération de l'identifiant du traitement créé
            Dim traitementLastDataReader As SqlDataReader
            SQLstring = "select max(oa_traitement_id) from oasis.oa_traitement where oa_traitement_patient_id = " & SelectedPatient.patientId & ";"
            Dim traitementLastCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            traitementLastDataReader = traitementLastCommand.ExecuteReader()
            If traitementLastDataReader.HasRows Then
                traitementLastDataReader.Read()
                'Récupération de la clé de l'enregistrement créé
                TraitementHistoACreer.HistorisationTraitementId = traitementLastDataReader(0)

                'Libération des ressources d'accès aux données
                conxn.Close()
                traitementLastCommand.Dispose()
            End If

            'Lecture du traitement créé avec toutes ses données pour communiquer le DataReader à la fonction dédiée
            Dim traitementCreeDataReader As SqlDataReader
            SQLstring = "select * from oasis.oa_traitement where oa_traitement_id = " & TraitementHistoACreer.HistorisationTraitementId & ";"
            Dim traitementCreeCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            traitementCreeDataReader = traitementCreeCommand.ExecuteReader()
            If traitementCreeDataReader.Read() Then
                'Initialisation classe Historisation traitement 
                'Inhibé pour changement de stratégie==================================================
                'InitClasseTraitementHistorisation(traitementCreeDataReader, UtilisateurConnecte, TraitementHistoACreer)

                'Libération des ressources d'accès aux données
                conxn.Close()
                traitementCreeCommand.Dispose()
            End If

            'Création dans l'historique des traitements du traitement créé
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.CreationTraitement)
        End If

        Return codeRetour

    End Function

    'Modification d'un traitement en base de données
    Private Function ModificationTraitement() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        'Initialisation de la barre de progression
        PgbMiseAJour.Show()
        PgbMiseAJour.Style = ProgressBarStyle.Marquee
        PgbMiseAJour.Refresh()

        'Définition de la base du traitement
        Dim baseTraitement As Char = DeterminationBaseTraitement()

        'Définition posologie journalière
        Dim PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir As Integer
        Dim Rythme As Integer

        If NumRythmeMatin.Value <> 0 Then
            Rythme = NumRythmeMatin.Value
        Else
            Rythme = 0
        End If

        If baseTraitement = "J" Then
            If NumRythmeMatin.Value <> 0 Then
                PosologieMatin = NumRythmeMatin.Value
            Else
                PosologieMatin = 0
            End If
            If NumRythmeMidi.Value <> 0 Then
                PosologieMidi = NumRythmeMidi.Value
            Else
                PosologieMidi = 0
            End If
            If NumRythmeApresMidi.Value <> 0 Then
                PosologieApresMidi = NumRythmeApresMidi.Value
            Else
                PosologieApresMidi = 0
            End If
            If NumRythmeSoir.Value <> 0 Then
                PosologieSoir = NumRythmeSoir.Value
            Else
                PosologieSoir = 0
            End If
        Else
            PosologieMatin = 0
            PosologieMidi = 0
            PosologieApresMidi = 0
            PosologieSoir = 0
        End If

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_traitement set oa_traitement_identifiant_modification = @utilisateurModification, oa_traitement_ordre_affichage = @ordreAffichage, oa_traitement_posologie_base = @posologieBase, oa_traitement_posologie_rythme = @posologieRythme, oa_traitement_posologie_matin = @posologieMatin, oa_traitement_posologie_midi = @posologieMidi, oa_traitement_posologie_apres_midi = @posologieApresMidi, oa_traitement_posologie_soir = @posologieSoir, oa_traitement_date_modification = @dateModification, oa_traitement_posologie_commentaire = @posologieCommentaire, oa_traitement_commentaire = @traitementCommentaire, oa_traitement_date_debut = @dateDebut, oa_traitement_date_fin = @dateFin where oa_traitement_id = @traitementId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@ordreAffichage", NumNumeroOrdre.Value.ToString)
            .AddWithValue("@posologieBase", baseTraitement)
            .AddWithValue("@posologieRythme", NumRythmeMatin.Value.ToString)
            .AddWithValue("@posologieMatin", PosologieMatin.ToString)
            .AddWithValue("@posologieMidi", PosologieMidi.ToString)
            .AddWithValue("@posologieApresMidi", PosologieApresMidi.ToString)
            .AddWithValue("@posologieSoir", PosologieSoir.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@posologieCommentaire", TxtTraitementPosologieCommentaire.Text)
            .AddWithValue("@traitementCommentaire", TxtTraitementCommentaire.Text)
            .AddWithValue("@dateDebut", DteTraitementDateDebut.Value)
            .AddWithValue("@dateFin", DteTraitementDateFin.Value)
            .AddWithValue("@traitementId", SelectedTraitementId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()

            'Dim zTimer = New System.Timers.Timer()
            'zTimer.Interval = 9000
            'Application.DoEvents()
            'zTimer.Enabled = True
            'zTimer.Start()

            PgbMiseAJour.Hide()
            MessageBox.Show("Traitement patient modifié")
        Catch ex As Exception
            PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement
            TraitementHistoACreer.HistorisationDate = Date.Now()
            TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.ModificationTraitement
            TraitementHistoACreer.HistorisationOrdreAffichage = NumNumeroOrdre.Value
            TraitementHistoACreer.HistorisationPosologieBase = baseTraitement
            TraitementHistoACreer.HistorisationPosologieRythme = NumRythmeMatin.Value
            TraitementHistoACreer.HistorisationPosologieMatin = PosologieMatin
            TraitementHistoACreer.HistorisationPosologieMidi = PosologieMidi
            TraitementHistoACreer.HistorisationPosologieApresMidi = PosologieApresMidi
            TraitementHistoACreer.HistorisationPosologieSoir = PosologieSoir
            TraitementHistoACreer.HistorisationPosologieCommentaire = TxtTraitementPosologieCommentaire.Text
            TraitementHistoACreer.HistorisationCommentaire = TxtTraitementCommentaire.Text
            TraitementHistoACreer.HistorisationDateDebut = DteTraitementDateDebut.Value
            TraitementHistoACreer.HistorisationDateFin = DteTraitementDateFin.Value

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.ModificationTraitement)
        End If

        Return codeRetour
    End Function


    'Arrêt traitement en base de données
    Private Function ArretTraitement() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim allergie, contreIndication As Integer

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_traitement set oa_traitement_identifiant_modification = @utilisateurModification, oa_traitement_date_modification = @dateModification, oa_traitement_date_fin = @dateFin, oa_traitement_arret_commentaire = @commentaireArret, oa_traitement_allergie = @allergie, oa_traitement_contre_indication = @contreIndication, oa_traitement_arret = @arret where oa_traitement_id = @traitementId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        'Traitement des informations d'annulation
        If ChkContreIndication.Checked = True Then
            contreIndication = 1
            allergie = 0
        End If

        If ChkAllergie.Checked = True Then
            ChkAllergie.ForeColor = Color.Red
            ChkContreIndication.ForeColor = Color.Black
            allergie = 1
            contreIndication = 0
        End If

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@datefin", DteTraitementDateFin.Value)                 's'appuyer sur la sate saisie !!!!!!!!!!!!!!!
            .AddWithValue("@commentaireArret", TxtCommentaireArret.Text)
            .AddWithValue("@allergie", allergie)
            .AddWithValue("@contreIndication", contreIndication)
            .AddWithValue("@arret", "A")
            .AddWithValue("@traitementId", SelectedTraitementId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()

            PgbMiseAJour.Hide()
            MessageBox.Show("Traitement patient arrêté")
        Catch ex As Exception
            PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement
            TraitementHistoACreer.HistorisationDate = Date.Now()
            TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.ArretTraitement
            TraitementHistoACreer.HistorisationDateFin = DteTraitementDateFin.Value
            TraitementHistoACreer.HistorisationArretCommentaire = TxtCommentaireArret.Text
            TraitementHistoACreer.HistorisationAllergie = allergie
            TraitementHistoACreer.HistorisationContreIndication = contreIndication
            TraitementHistoACreer.HistorisationArret = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.ArretTraitement)
        End If

        Return codeRetour
    End Function

    'Annulation d'un traitement en base de données
    Private Function AnnulationTraitement() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_traitement set oa_traitement_identifiant_modification = @utilisateurModification, oa_traitement_date_modification = @dateModification, oa_traitement_date_fin = @dateFin, oa_traitement_annulation_commentaire = @commentaireAnnulation, oa_traitement_annulation = @annulation where oa_traitement_id = @traitementId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateFin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@commentaireAnnulation", TxtCommentaireAnnulation.Text)
            .AddWithValue("@annulation", "A")
            .AddWithValue("@traitementId", SelectedTraitementId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()

            PgbMiseAJour.Hide()
            MessageBox.Show("Traitement patient annulé")
        Catch ex As Exception
            PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement 
            TraitementHistoACreer.HistorisationDate = Date.Now()
            TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.AnnulationTraitement
            TraitementHistoACreer.HistorisationAnnulationCommentaire = TxtCommentaireAnnulation.Text
            TraitementHistoACreer.HistorisationAnnulation = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.AnnulationTraitement)
        End If

        Return codeRetour
    End Function


    Private Function SuppressionTraitement() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        'Initialisation de la barre de progression
        PgbMiseAJour.Show()
        PgbMiseAJour.Value = 1
        PgbMiseAJour.Step = 1


        Dim SQLstring As String = "delete from oasis.oa_traitement where oa_traitement_id = @traitementId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@traitementId", SelectedTraitementId.ToString)
        End With

        Try
            conxn.Open()
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
            PgbMiseAJour.Hide()
            MessageBox.Show("Traitement patient supprimé")
        Catch ex As Exception
            PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement 
            TraitementHistoACreer.HistorisationDate = Date.Now()
            TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.AnnulationTraitement
            TraitementHistoACreer.HistorisationAnnulationCommentaire = TxtCommentaireArret.Text
            TraitementHistoACreer.HistorisationAnnulation = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.SuppressionTraitement)
        End If

        Return codeRetour
    End Function


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones de l'écran ============
    '=============================================================================================

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
    End Sub

    Private Sub ChargementTraitementExistant()
        Dim traitementDataReader As SqlDataReader
        SQLString = "select * from oasis.oa_traitement where oa_traitement_id = " & SelectedTraitementId & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        conxn.Open()
        traitementDataReader = myCommand.ExecuteReader()
        If traitementDataReader.Read() Then
            'Stockage de l'utilisateur qui a créé le traitement pour le contrôle en cas de demande de suppression du traitement
            If traitementDataReader("oa_traitement_identifiant_creation") Is DBNull.Value Then
                utilisateurCreation = 0
            Else
                utilisateurCreation = CInt(traitementDataReader("oa_traitement_identifiant_creation"))
            End If

            If traitementDataReader("oa_traitement_medicament_cis") Is DBNull.Value Then
                LblTraitementMedicamentCIS.Text = ""
                medicament_selecteur_cis = 0
            Else
                LblTraitementMedicamentCIS.Text = traitementDataReader("oa_traitement_medicament_cis")
                medicament_selecteur_cis = CInt(traitementDataReader("oa_traitement_medicament_cis"))
            End If

            If traitementDataReader("oa_traitement_medicament_dci") Is DBNull.Value Then
                LblMedicamentDCI.Text = ""
            Else
                LblMedicamentDCI.Text = traitementDataReader("oa_traitement_medicament_dci")
            End If

            'Formatage de la posologie
            Dim Base As String
            Dim BaseSelection As Char
            Dim Posologie As String
            Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
            Dim Rythme As Integer
            Dim Fenetre As Boolean
            Dim FenetreDateDebut, FenetreDateFin As Date
            Dim dateFin, dateDebut, dateCreation, dateModification As Date
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)

            'Initialisation des données à afficher
            LblFenetreTherapeutique.Hide()
            LblFenetreTherapeutiqueAu.Hide()
            LblFenetreDateDebut.Hide()
            LblFenetreDateFin.Hide()
            GbxFenetreTherapeutique.Hide()

            'Récupération de la période d'application du traitement
            If traitementDataReader("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataReader("oa_traitement_date_fin")
                'TxtTraitementDateFin.Text = dateFin.ToString("dd.MM.yyyy")
            Else
                dateFin = "31/12/2999"
            End If

            'Si le traitement a été déclaré arrêté, ce traitement ne doit pas pouvoir être modifié
            If traitementDataReader("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataReader("oa_traitement_arret") = "A" Then
                    traitementArrete = True
                    Me.Text = "Visualisation détail traitement patient (traitement arrêté)"
                    LblstatutTraitement.Text = "Visualisation détail traitement patient (traitement arrêté)"
                    LblstatutTraitement.Show()
                    PnlStatutTraitement.Show()
                    InhiberZonesDeSaisie()
                    AfficherZonesArret()
                    InhiberZonesDeSaisieArret()
                    ChargerZonesArret(traitementDataReader)
                End If
            End If

            'Si le traitement a été déclaré allergie ou contre-indication, ce traitement ne doit pas pouvoir être modifié
            If traitementDataReader("oa_traitement_declaratif_hors_traitement") IsNot DBNull.Value Then
                If traitementDataReader("oa_traitement_declaratif_hors_traitement") = "1" Then
                    'traitementArrete = True
                    Me.Text = "Visualisation détail traitement (Déclaration allergie ou contre-indication)"
                    LblstatutTraitement.Text = "Visualisation détail traitement (Déclaration allergie ou contre-indication)"
                    LblstatutTraitement.Show()
                    PnlStatutTraitement.Show()
                    InhiberZonesDeSaisie()
                    AfficherZonesArret()
                    InhiberZonesDeSaisieArret()
                    ChargerZonesArret(traitementDataReader)
                End If
            End If


            'Si le traitement a été déclaré annulé, ce traitement ne doit pas pouvoir être modifié
            If traitementDataReader("oa_traitement_annulation") IsNot DBNull.Value Then
                If traitementDataReader("oa_traitement_annulation") = "A" Then
                    traitementArrete = True
                    Me.Text = "Visualisation détail traitement patient (traitement annulé)"
                    LblstatutTraitement.Text = "Visualisation détail traitement patient (traitement annulé)"
                    LblstatutTraitement.Show()
                    PnlStatutTraitement.Show()
                    InhiberZonesDeSaisie()
                    AfficherZonesAnnulation()
                    InhiberZonesDeSaisieAnnulation()
                    ChargerZonesAnnulation(traitementDataReader)
                End If
            End If

            'Si la date de fin est inférieure à la date du jour, ce traitement est terminé et ne doit pas pouvoir être modifié
            Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                If traitementArrete = False Then
                    Me.Text = "Visualisation détail traitement patient (traitement terminé)"
                    LblstatutTraitement.Text = "Visualisation détail traitement patient (traitement terminé)"
                    LblstatutTraitement.Show()
                    PnlStatutTraitement.Show()
                    InhiberZonesDeSaisie()
                End If
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique
            Fenetre = False
            If traitementDataReader("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                FenetreDateDebut = traitementDataReader("oa_traitement_fenetre_date_debut")
                LblFenetreDateDebut.Text = FenetreDateDebut.ToString("dd.MM.yyyy")
            Else
                FenetreDateDebut = "31/12/2999"
            End If
            If traitementDataReader("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                FenetreDateFin = traitementDataReader("oa_traitement_fenetre_date_fin")
                LblFenetreDateFin.Text = FenetreDateFin.ToString("dd.MM.yyyy")
            Else
                FenetreDateFin = "01/01/1900"
            End If

            Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If traitementDataReader("oa_traitement_fenetre") IsNot DBNull.Value Then
                If traitementDataReader("oa_traitement_fenetre") = "1" Then
                    Fenetre = True
                    Posologie = "Fenêtre Th."
                    If dateDebutFenetreaComparer <= dateJouraComparer And dateFinFenetreaComparer > dateJouraComparer Then
                        LblFenetreActive.Text = "--- Fenêtre thérapeutique en cours ---"
                    Else
                        If FenetreDateDebut > dateJouraComparer Then
                            LblFenetreActive.Text = "--- Fenêtre thérapeutique à venir ---"
                        Else
                            LblFenetreActive.Text = "(Fenêtre thérapeutique obsolète)"
                        End If
                    End If
                    LblFenetreTherapeutique.Show()
                    LblFenetreTherapeutiqueAu.Show()
                    LblFenetreDateDebut.Show()
                    LblFenetreDateFin.Show()
                    GbxFenetreTherapeutique.Show()
                End If
            End If

            'Traitement de la posologie
            If traitementDataReader("oa_traitement_posologie_base") IsNot DBNull.Value Then
                Rythme = traitementDataReader("oa_traitement_posologie_rythme")
                NumRythmeMatin.Value = Rythme
                BaseSelection = traitementDataReader("oa_traitement_posologie_base")
                Select Case traitementDataReader("oa_traitement_posologie_base")
                    Case "J"
                        Base = "Journalier : "
                        CbxTraitementBase.Text = "Journalier"
                        MontrerPosologieJournaliere()
                        If traitementDataReader("oa_traitement_posologie_matin") <> 0 Then
                            posologieMatin = traitementDataReader("oa_traitement_posologie_matin")
                            NumRythmeMatin.Value = traitementDataReader("oa_traitement_posologie_matin")
                        Else
                            posologieMatin = 0
                            NumRythmeMatin.Value = 0
                        End If
                        If traitementDataReader("oa_traitement_posologie_midi") <> 0 Then
                            posologieMidi = traitementDataReader("oa_traitement_posologie_midi")
                            NumRythmeMidi.Value = traitementDataReader("oa_traitement_posologie_midi")
                        Else
                            posologieMidi = 0
                            NumRythmeMidi.Value = 0
                        End If
                        If traitementDataReader("oa_traitement_posologie_apres_midi") <> 0 Then
                            posologieApresMidi = traitementDataReader("oa_traitement_posologie_apres_midi")
                            NumRythmeApresMidi.Value = traitementDataReader("oa_traitement_posologie_apres_midi")
                        Else
                            posologieApresMidi = 0
                            NumRythmeApresMidi.Value = 0
                        End If
                        If traitementDataReader("oa_traitement_posologie_soir") <> 0 Then
                            posologieSoir = traitementDataReader("oa_traitement_posologie_soir")
                            NumRythmeSoir.Value = traitementDataReader("oa_traitement_posologie_soir")
                        Else
                            posologieSoir = 0
                            NumRythmeSoir.Value = 0
                        End If
                    Case "H"
                        Base = "Hebdo : "
                        CbxTraitementBase.Text = "Hebdomadaire"
                        CacherPosologieJournaliere()
                    Case "M"
                        Base = "Mensuel : "
                        CbxTraitementBase.Text = "Mensuel"
                        CacherPosologieJournaliere()
                    Case "A"
                        CbxTraitementBase.Text = "Annuel"
                        Base = "Annuel : "
                        CacherPosologieJournaliere()
                    Case Else
                        Base = "Base inconnue ! "
                        CbxTraitementBase.Text = ""
                        CacherPosologieJournaliere()
                End Select
            End If

            'Alimentation des données à afficher
            LblTraitementPosologie.Text = FormatagePosologie(Rythme, BaseSelection, posologieMatin, posologieMidi, posologieApresMidi, posologieSoir)

            If traitementDataReader("oa_traitement_posologie_commentaire") Is DBNull.Value Then
                TxtTraitementPosologieCommentaire.Text = ""
            Else
                TxtTraitementPosologieCommentaire.Text = traitementDataReader("oa_traitement_posologie_commentaire")
            End If

            If traitementDataReader("oa_traitement_date_debut") IsNot DBNull.Value Then
                dateDebut = traitementDataReader("oa_traitement_date_debut")
                DteTraitementDateDebut.Value = dateDebut
            End If

            If traitementDataReader("oa_traitement_ordre_affichage") IsNot DBNull.Value Then
                NumNumeroOrdre.Value = CInt(traitementDataReader("oa_traitement_ordre_affichage"))
            Else
                NumNumeroOrdre.Value = 0
            End If

            If traitementDataReader("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataReader("oa_traitement_date_fin")
                DteTraitementDateFin.Value = dateFin
                Dim DateSansLimite As New Date(2999, 12, 31, 0, 0, 0)
                If DteTraitementDateFin.Value <> DateSansLimite Then
                    DteTraitementDateFin.Format = DateTimePickerFormat.Long
                    'Calcul durée
                    LblTraitementDuree.Text = CalculDureeTraitementString(dateDebut, dateFin)
                Else
                    'TxtTraitementDateDebut.Text = ""
                    DteTraitementDateFin.Format = DateTimePickerFormat.Custom
                    DteTraitementDateFin.CustomFormat = " "
                    LblLabelTraitementDuree.Hide()
                    LblTraitementDuree.Hide()
                End If
            End If

            If traitementDataReader("oa_traitement_commentaire") Is DBNull.Value Then
                TxtTraitementCommentaire.Text = ""
            Else
                TxtTraitementCommentaire.Text = traitementDataReader("oa_traitement_commentaire")
            End If

            If traitementDataReader("oa_traitement_date_creation") IsNot DBNull.Value Then
                dateCreation = traitementDataReader("oa_traitement_date_creation")
                dateCreationTraitement = traitementDataReader("oa_traitement_date_creation")
                LblTraitementDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
            Else
                LblTraitementDateCreation.Text = ""
                LblLabelTraitementDateCreation.Hide()
                LblLabelTraitementParCreation.Hide()
            End If

            LblUtilisateurCreation.Text = ""
            If traitementDataReader("oa_traitement_identifiant_creation") IsNot DBNull.Value Then
                If traitementDataReader("oa_traitement_identifiant_creation") <> 0 Then
                    SetUtilisateur(UtilisateurHisto, traitementDataReader("oa_traitement_identifiant_creation"))
                    LblUtilisateurCreation.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
                End If
            End If

            'Contrôle si on peut traiter la suppression, la suppression est permise si la date de création = date de suppression
            Dim dateCreationaComparer As New Date(dateCreationTraitement.Year, dateCreationTraitement.Month, dateCreationTraitement.Day, 0, 0, 0)
            If dateCreationaComparer <> dateJouraComparer Then
                BtnSupprimerTraitement.Hide()
            End If

            If traitementDataReader("oa_traitement_date_modification") IsNot DBNull.Value Then
                dateModification = traitementDataReader("oa_traitement_date_modification")
                LblTraitementDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblTraitementDateModification.Text = ""
                LblLabelTraitementDateModification.Hide()
                LblLabelTraitementParModification.Hide()
            End If
        End If

        LblUtilisateurModification.Text = ""
        If traitementDataReader("oa_traitement_identifiant_modification") IsNot DBNull.Value Then
            If traitementDataReader("oa_traitement_identifiant_modification") <> 0 Then
                SetUtilisateur(UtilisateurHisto, traitementDataReader("oa_traitement_identifiant_modification"))
                LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
            End If
        End If

        'Initialisation classe Historisation traitement 
        'Inhibé pour changement de stratégie==================================================
        'InitClasseTraitementHistorisation(traitementDataReader, UtilisateurConnecte, TraitementHistoACreer)

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()
    End Sub

    Private Sub ChargementMedoc()
        If EditMode = "C" Then
            LblTraitementMedicamentCIS.Text = SelectedMedicamentCis.ToString
        End If

        Dim medicamentDataReader As SqlDataReader
        SQLString = "select * from oasis.oa_r_medicament where oa_medicament_cis = " & SelectedMedicamentCis.ToString & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        conxn.Open()
        medicamentDataReader = myCommand.ExecuteReader()
        If medicamentDataReader.Read() Then
            If medicamentDataReader("oa_medicament_dci") Is DBNull.Value Then
                LblMedicamentDCI.Text = ""
            Else
                LblMedicamentDCI.Text = medicamentDataReader("oa_medicament_dci")
            End If

            If medicamentDataReader("oa_medicament_forme") Is DBNull.Value Then
                LblMedicamentForme.Text = ""
            Else
                LblMedicamentForme.Text = medicamentDataReader("oa_medicament_forme")
            End If

            If medicamentDataReader("oa_medicament_voie_administration") Is DBNull.Value Then
                LblMedicamentAdministration.Text = ""
            Else
                LblMedicamentAdministration.Text = medicamentDataReader("oa_medicament_voie_administration")
            End If

            If medicamentDataReader("oa_medicament_titulaire") Is DBNull.Value Then
                LblMedicamentTitulaire.Text = ""
            Else
                LblMedicamentTitulaire.Text = medicamentDataReader("oa_medicament_titulaire")
            End If
        End If
        conxn.Close()
        myCommand.Dispose()
    End Sub

    'Gestion de l'affichage de l'affichage de la durée et de son calcul, on affiche et on calcule la durée si la date de fin de traitement est différente de la valeur virtuelle "Infinie"
    Private Sub ControleAffichageDureeTraitement()
        Dim DateDebut, DateFin As Date
        DateDebut = CDate(DteTraitementDateDebut.Value)
        DateFin = CDate(DteTraitementDateFin.Value)

        LblTraitementDateFin.ForeColor = Color.Black

        If Not (DateFin.Year = 2999 And DateFin.Month = 12 And DateFin.Day = 31) Then
            Dim dateDebutaComparer As New Date(DateDebut.Year, DateDebut.Month, DateDebut.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(DateFin.Year, DateFin.Month, DateFin.Day, 0, 0, 0)
            'Calcul du nombre de jours de différence au cas ou seules les heures sont différentes
            If (dateFinaComparer < dateDebutaComparer) Then
                LblTraitementDateFin.ForeColor = Color.Red
                LblLabelTraitementDuree.Hide()
                LblTraitementDuree.Hide()
            Else
                'Calcul durée
                LblTraitementDuree.Text = CalculDureeTraitementString(DateDebut, DateFin)
                LblLabelTraitementDuree.Show()
                LblTraitementDuree.Show()
            End If
        Else
            LblLabelTraitementDuree.Hide()
            LblTraitementDuree.Hide()
        End If
    End Sub

    'Gestion du formatage de la posologie à afficher
    Private Sub AppelFormatagePosologie()
        Dim Rythme As Integer
        Dim BaseSelection As Char
        Dim PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir As Integer

        If NumRythmeMatin.Value <> 0 Then
            Rythme = NumRythmeMatin.Value
        Else
            Rythme = 0
        End If

        Select Case CbxTraitementBase.Text
            Case "Journalier"
                BaseSelection = "J"
            Case "Hebdomadaire"
                BaseSelection = "H"
            Case "Mensuel"
                BaseSelection = "M"
            Case "Annuel"
                BaseSelection = "A"
            Case Else
                BaseSelection = ""
        End Select

        PosologieMatin = NumRythmeMatin.Value
        PosologieMidi = NumRythmeMidi.Value
        PosologieApresMidi = NumRythmeApresMidi.Value
        PosologieSoir = NumRythmeSoir.Value

        LblTraitementPosologie.Text = FormatagePosologie(Rythme, BaseSelection, PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir)
    End Sub

    Private Sub CacherPosologieJournaliere()
        LblRythmeMatin.Hide()
        NumRythmeMidi.Hide()
        LblRythmeMidi.Hide()
        NumRythmeApresMidi.Hide()
        LblRythmeApresMidi.Hide()
        NumRythmeSoir.Hide()
        LblRythmeSoir.Hide()
    End Sub

    Private Sub MontrerPosologieJournaliere()
        LblRythmeMatin.Show()
        NumRythmeMidi.Show()
        LblRythmeMidi.Show()
        NumRythmeMidi.Value = 0
        NumRythmeApresMidi.Show()
        LblRythmeApresMidi.Show()
        NumRythmeApresMidi.Value = 0
        NumRythmeSoir.Show()
        LblRythmeSoir.Show()
        NumRythmeSoir.Value = 0
    End Sub

    Private Sub InitZone()
        Me.CodeRetour = False
        LblMedicamentDCI.Text = ""
        LblTraitementMedicamentCIS.Text = ""
        LblMedicamentForme.Text = ""
        LblMedicamentAdministration.Text = ""
        LblMedicamentTitulaire.Text = ""
        LblTraitementPosologie.Text = ""
        LblTraitementDateCreation.Text = ""
        LblTraitementDateModification.Text = ""
        LblFenetreTherapeutique.Hide()
        LblFenetreTherapeutiqueAu.Hide()
        LblFenetreDateFin.Hide()
        LblFenetreDateDebut.Hide()
        GbxFenetreTherapeutique.Hide()
        PgbMiseAJour.Hide()
        CacherZonesAnnulation()
        CacherZonesArret()
        'Si l'utilisateur n'a pas les droits requis ou que le traitement a été arrêté, les zones de saisie ne sont pas modifiables 
        If UtilisateurConnecte.UtilisateurNiveauAcces <> 1 Then
            Me.Text = "Visualisation détail traitement patient"
            LblstatutTraitement.Text = "Visualisation détail traitement patient"
            LblstatutTraitement.Hide()
            PnlStatutTraitement.Hide()
            InhiberZonesDeSaisie()
        End If
    End Sub

    Private Sub AfficherZonesArret()
        TxtCommentaireArret.Visible = True
        ChkAllergie.Visible = True
        ChkContreIndication.Visible = True
        GbxArretTraitement.Visible = True
        BtnValidationArret.Visible = True
        'Initialisation à la date du jour de la date de fin correspondant à l'arrêt, elle peut-être modifiée à la date j-1
        DteTraitementDateFin.Value = Date.Now()
        CacherZonesAnnulation()
    End Sub

    Private Sub CacherZonesArret()
        TxtCommentaireArret.Visible = False
        ChkAllergie.Visible = False
        ChkContreIndication.Visible = False
        GbxArretTraitement.Visible = False
        BtnValidationArret.Visible = False
    End Sub

    Private Sub AfficherZonesAnnulation()
        TxtCommentaireAnnulation.Visible = True
        GbxAnnulationTraitement.Visible = True
        BtnValidationAnnulation.Visible = True
        CacherZonesArret()
    End Sub

    Private Sub CacherZonesAnnulation()
        TxtCommentaireAnnulation.Visible = False
        GbxAnnulationTraitement.Visible = False
        BtnValidationAnnulation.Visible = False
    End Sub

    Private Sub InhiberZonesDeSaisie()
        BtnAnnulerTraitement.Visible = False
        BtnArretTraitement.Visible = False
        BtnSupprimerTraitement.Visible = False
        BtnValidation.Visible = False
        CbxTraitementBase.Enabled = False
        NumRythmeMatin.Enabled = False
        NumNumeroOrdre.Enabled = False
        NumRythmeMatin.Enabled = False
        NumRythmeMidi.Enabled = False
        NumRythmeApresMidi.Enabled = False
        NumRythmeSoir.Enabled = False
        DteTraitementDateDebut.Enabled = False
        DteTraitementDateFin.Enabled = False
        TxtTraitementCommentaire.ReadOnly = True
        TxtTraitementPosologieCommentaire.ReadOnly = True
    End Sub

    Private Sub InhiberZonesDeSaisieArret()
        TxtCommentaireArret.Enabled = False
        ChkAllergie.Enabled = False
        ChkContreIndication.Enabled = False
        BtnValidationArret.Visible = False
    End Sub

    Private Sub InhiberZonesDeSaisieAnnulation()
        TxtCommentaireAnnulation.Enabled = False
        BtnValidationAnnulation.Visible = False
    End Sub

    Private Sub ChargerZonesArret(traitementDataReader As SqlDataReader)
        Dim arretTraitementDataReader As SqlDataReader = traitementDataReader

        If arretTraitementDataReader("oa_traitement_arret_commentaire") IsNot DBNull.Value Then
            TxtCommentaireArret.Text = arretTraitementDataReader("oa_traitement_arret_commentaire").ToString
        Else
            TxtCommentaireArret.Text = ""
        End If

        ChkAllergie.Checked = False
        If arretTraitementDataReader("oa_traitement_allergie") IsNot DBNull.Value Then
            If arretTraitementDataReader("oa_traitement_allergie") = "1" Then
                ChkAllergie.Checked = True
                ChkAllergie.ForeColor = Color.Red
            Else
                ChkAllergie.ForeColor = Color.Black
            End If
        End If

        ChkContreIndication.Checked = False
        If arretTraitementDataReader("oa_traitement_contre_indication") IsNot DBNull.Value Then
            If arretTraitementDataReader("oa_traitement_contre_indication") = "1" Then
                ChkContreIndication.Checked = True
                ChkContreIndication.ForeColor = Color.Red
            Else
                ChkContreIndication.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub ChargerZonesAnnulation(traitementDataReader As SqlDataReader)
        Dim arretTraitementDataReader As SqlDataReader = traitementDataReader

        If arretTraitementDataReader("oa_traitement_annulation_commentaire") IsNot DBNull.Value Then
            TxtCommentaireAnnulation.Text = arretTraitementDataReader("oa_traitement_annulation_commentaire").ToString
        Else
            TxtCommentaireAnnulation.Text = ""
        End If
    End Sub

    '=============================================================================================
    '==================================== Gestion des évènements =================================
    '=============================================================================================

    'Si l'utilisateur lance le dateTimePicker de la date de fin alors que celle-ci est virtuellement "infinie", on initialise sa valeur avec la date du jour  

    Private Sub DteTraitementDateFin_DropDown(sender As Object, e As EventArgs) Handles DteTraitementDateFin.DropDown
        Dim DateInfinie As New Date(2999, 12, 31, 0, 0, 0)
        If DteTraitementDateFin.Value = DateInfinie Then
            DteTraitementDateFin.Value = DteTraitementDateDebut.Value
            DteTraitementDateFin.Format = DateTimePickerFormat.Long
            ControleAffichageDureeTraitement()
        End If
    End Sub

    'Quand l'utilisateur modifie la date de début de traitement, on lance le calcul de la durée
    Private Sub DteTraitementDateDebut_ValueChanged(sender As Object, e As EventArgs) Handles DteTraitementDateDebut.ValueChanged
        ControleAffichageDureeTraitement()
    End Sub

    'Quand l'utilisateur modifie la date de fin de traitement, on lance le calcul de la durée
    Private Sub DteTraitementDateFin_ValueChanged(sender As Object, e As EventArgs) Handles DteTraitementDateFin.ValueChanged
        Dim DateInfinie As New Date(2999, 12, 31, 0, 0, 0)
        If DteTraitementDateFin.Value <> DateInfinie Then
            DteTraitementDateFin.Format = DateTimePickerFormat.Long
            ControleAffichageDureeTraitement()
        End If
    End Sub

    'Traitement de l'affichage des zones liées à la base de la posologie quand celle-ci est modifiée
    Private Sub CbxTraitementBase_TextChanged(sender As Object, e As EventArgs) Handles CbxTraitementBase.TextChanged
        'Gestion l'affichage des zones de saisie de la posologie journalière
        If CbxTraitementBase.Text <> "Journalier" Then
            CacherPosologieJournaliere()
        Else
            MontrerPosologieJournaliere()
        End If
        'calcul de la posologie
        AppelFormatagePosologie()
    End Sub

    'Traitement de l'affichage de la posologie quand le rythme est modifié
    Private Sub NumTraitementRythme_ValueChanged(sender As Object, e As EventArgs) Handles NumRythmeMatin.ValueChanged
        AppelFormatagePosologie()
    End Sub

    Private Sub NumRythmeMidi_ValueChanged(sender As Object, e As EventArgs) Handles NumRythmeMidi.ValueChanged
        AppelFormatagePosologie()
    End Sub

    Private Sub NumRythmeApresMidi_ValueChanged(sender As Object, e As EventArgs) Handles NumRythmeApresMidi.ValueChanged
        AppelFormatagePosologie()
    End Sub

    Private Sub NumRythmeSoir_ValueChanged(sender As Object, e As EventArgs) Handles NumRythmeSoir.ValueChanged
        AppelFormatagePosologie()
    End Sub

    'Arrêt traitement : si on clique sur allergie, la contre-indication n'est pas sélectionnable
    Private Sub ChkAllergie_Click(sender As Object, e As EventArgs) Handles ChkAllergie.Click
        If ChkAllergie.Checked = True Then
            ChkContreIndication.Checked = False
            ChkContreIndication.ForeColor = Color.Black
            ChkAllergie.ForeColor = Color.Red
        End If
    End Sub
    'Arrêt traitement : si on clique sur contre-indication, l'allergie n'est pas sélectionnable
    Private Sub ChkContreIndication_Click(sender As Object, e As EventArgs) Handles ChkContreIndication.Click
        If ChkContreIndication.Checked = True Then
            ChkAllergie.Checked = False
            ChkContreIndication.ForeColor = Color.Red
            ChkAllergie.ForeColor = Color.Black
        End If
    End Sub

    '=============================================================================================
    '==================================== Traitement divers ======================================
    '=============================================================================================

    'Détermination de la base traitement à partir du combo box
    Private Function DeterminationBaseTraitement() As Char
        Dim baseTraitement As Char
        Select Case CbxTraitementBase.Text
            Case "Journalier"
                baseTraitement = "J"
            Case "Hebdomadaire"
                baseTraitement = "H"
            Case "Mensuel"
                baseTraitement = "M"
            Case "Annuel"
                baseTraitement = "A"
            Case Else
                baseTraitement = ""
        End Select
        Return baseTraitement
    End Function

    'Traitement du formatage de la posologie
    Private Function FormatagePosologie(Rythme As Integer, BaseSelection As Char, PosologieMatin As Integer, PosologieMidi As Integer, PosologieApresMidi As Integer, PosologieSoir As Integer) As String
        Dim Base, Posologie As String
        Posologie = ""
        If Rythme <> 0 Then
            Select Case BaseSelection
                Case "J"
                    Base = "Journalier : "
                    If PosologieApresMidi <> 0 Then
                        Posologie = Base + PosologieMatin.ToString + "." + PosologieMidi.ToString + "." + PosologieApresMidi.ToString + "." + PosologieSoir.ToString
                    Else
                        Posologie = Base + " " + PosologieMatin.ToString + "." + PosologieMidi.ToString + "." + PosologieSoir.ToString
                    End If
                Case "H"
                    Base = "Hebdo : "
                    Posologie = Base + Rythme.ToString
                Case "M"
                    Base = "Mensuel : "
                    Posologie = Base + Rythme.ToString
                Case "A"
                    Base = "Annuel : "
                    Posologie = Base + Rythme.ToString
                Case Else
                    Base = "Base inconnue ! "
                    Posologie = Base + Rythme.ToString
            End Select
        End If
        Return Posologie
    End Function

    Private Sub FTraitementDetailEdit_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        conxn.Dispose()
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTip1.SetToolTip(LblId, "Id : " + SelectedTraitementId.ToString)
    End Sub

End Class