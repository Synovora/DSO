Imports System.Data.SqlClient
Imports Oasis_Common
Public Class RadFTraitementFenetreTh

    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedTraitementId As Integer
    Private privateFenetreTherapeutiqueExiste As Boolean
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

    Public Property FenetreTherapeutiqueExiste As Boolean
        Get
            Return privateFenetreTherapeutiqueExiste
        End Get
        Set(value As Boolean)
            privateFenetreTherapeutiqueExiste = value
        End Set
    End Property

    Dim theriaqueDao As New TheriaqueDao

    'Déclaration des variables de travail
    Dim EditMode As Char = ""
    Dim conxn As New SqlConnection(getConnectionString())
    Dim SQLString As String
    Dim medicament_selecteur_cis As Integer
    Private dateCreationTraitement As Date
    Dim TraitementHistoACreer As New TraitementHisto
    Dim UtilisateurHisto As Utilisateur = New Utilisateur()

    Private Sub RadFTraitementFenetreTh_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitZone()
        ChargementEtatCivil()
        If Me.FenetreTherapeutiqueExiste = False Then
            EditMode = "C" 'Création fenêtre thérapeutique
            Me.Text = "Création fenêtre thérapeutique"
            LblFentreTherapeutiqueExistante.Hide()
            GbxFenetreTherapeutiqueExistante.Hide()
            'Initialisation des données en saisie
            RadBtnSupprimerFenetre.Hide()
        Else
            EditMode = "M" 'Modification fenêtre thérapeutique
            Me.Text = "Mise à jour fenêtre thérapeutique"
            LblFentreTherapeutiqueExistante.Show()
            GbxFenetreTherapeutiqueExistante.Show()
        End If
        'Chargement du traitement
        ChargementTraitementExistant()
        'Chargement des données du médicament du traitement
        ChargementMedoc()
    End Sub

    Private Sub RadBtnValidationFenetre_Click(sender As Object, e As EventArgs) Handles RadBtnValidationFenetre.Click
        'Contrôle des zones saisies
        If ControleValiditationDonnees() = True Then
            'Création ou mise à jour de la table
            Select Case EditMode
                Case "C"
                    '-------------Traitement de la création
                    If MajFenetreTherapeutique() = True Then
                        Me.CodeRetour = True
                        Close()
                    End If
                Case "M"
                    '-------------Traitement de la modification
                    If MajFenetreTherapeutique() = True Then
                        Me.CodeRetour = True
                        Close()
                    End If
            End Select
        End If
    End Sub

    Private Sub RadBtnSupprimerFenetre_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimerFenetre.Click
        'Contrôle si on doit traiter la suppression ou l'annulation
        'Traitement de la Suppression si la date de création = date de suppression et même auteur, sinon annulation
        Dim dateCreationaComparer As New Date(dateCreationTraitement.Year, dateCreationTraitement.Month, dateCreationTraitement.Day, 0, 0, 0)
        Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
        If dateCreationaComparer = dateJouraComparer Then
            If SuppressionFenetreTherapeutique() = True Then
                Me.CodeRetour = True
                Close()
            End If
        Else
            MessageBox.Show("Suppression impossible, une fenêtre thérapeutique en cours ne peut être supprimée que si sa création à été réalisé dans la journée" &
                            vbCrLf & vbCrLf &
                            "=> Solution : arrêter le traitement ou demander l'aide d'un Admin")
        End If
    End Sub

    'Retour sur la Form appelante
    Private Sub BtnRetour_Click(sender As Object, e As EventArgs) Handles RadBtnRetour.Click
        Close()
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

        ControleAffichageDureeTraitement()
        Dim DateFenetreDebut, DatefenetreFin, DateTraitementDebut, DateTraitementFin As Date

        DateFenetreDebut = CDate(DteFenetreTherapeutiqueDateDebut.Value)
        DatefenetreFin = CDate(DteFenetreTherapeutiqueDateFin.Value)
        DateTraitementDebut = CDate(DteTraitementDateDebut.Value)
        DateTraitementFin = CDate(DteTraitementDateFin.Value)

        Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
        Dim DateFenetreDebutaComparer As New Date(DateFenetreDebut.Year, DateFenetreDebut.Month, DateFenetreDebut.Day, 0, 0, 0)
        Dim DateFenetreFinaComparer As New Date(DatefenetreFin.Year, DatefenetreFin.Month, DatefenetreFin.Day, 0, 0, 0)
        Dim DateTraitementDebutaComparer As New Date(DateTraitementDebut.Year, DateTraitementDebut.Month, DateTraitementDebut.Day, 0, 0, 0)
        Dim DateTraitementFinaComparer As New Date(DateTraitementFin.Year, DateTraitementFin.Month, DateTraitementFin.Day, 0, 0, 0)

        'La date de fin de la fenêtre doit être supérieure ou égale à la date de début
        If (DateFenetreFinaComparer < DateFenetreDebutaComparer) Then
            Valide = False
            messageErreur2 = "- La date de fin de la fenêtre thérapeutique doit être supérieure ou égale à la date de début"
        End If

        'La date de début de la fenêtre doit être supérieure ou égale à la date de début de traitement
        If (DateFenetreDebutaComparer < DateTraitementDebutaComparer) Then
            Valide = False
            messageErreur3 = "- La date de début de la fenêtre thérapeutique doit être supérieure ou égale à la date de début du traitement"
        End If

        'La date de fin de la fenêtre doit être inférieure ou égale à la date de fin du traitement
        If (DateFenetreFinaComparer > DateTraitementFinaComparer) Then
            Valide = False
            messageErreur4 = "- La date de fin de la fenêtre thérapeutique doit être inférieure ou égale à la date de fin du traitement"
        End If

        'Préparation de l'affichage des erreurs
        If Valide = False Then
            If messageErreur1 <> "" Then
                messageErreur = messageErreur1 + vbCrLf
            End If

            If messageErreur2 <> "" Then
                messageErreur = messageErreur + messageErreur2 + vbCrLf
            End If

            If messageErreur3 <> "" Then
                messageErreur = messageErreur + messageErreur3 + vbCrLf
            End If

            If messageErreur4 <> "" Then
                messageErreur = messageErreur + messageErreur4 + vbCrLf
            End If

            messageErreur = messageErreur + vbCrLf + vbCrLf + "/!\ Validation impossible, des données sont incorrectes"

            MessageBox.Show(messageErreur)
        End If

        Return Valide
    End Function

    'Modification fenêtre thérapeutique
    Private Function MajFenetreTherapeutique() As Boolean
        'On demande confirmation, si la date de début de la fenêtre est inférieure à la date du jour
        Dim DateFenetreDebut As Date
        DateFenetreDebut = CDate(DteFenetreTherapeutiqueDateDebut.Value)
        Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
        Dim DateFenetreDebutaComparer As New Date(DateFenetreDebut.Year, DateFenetreDebut.Month, DateFenetreDebut.Day, 0, 0, 0)

        If (DateFenetreDebutaComparer < dateJouraComparer) Then
            Dim Result As DialogResult
            Result = MessageBox.Show(Me, "Attention, la date de début de la fenêtre thérapeutique est inférieure à la date du jour, merci de confirmer",
                                     "Validation date de début de fenêtre thérapeutique", MessageBoxButtons.YesNo)
            If Result = DialogResult.Yes Then
            Else
                Return False
            End If
        End If

        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        'Définition de la base du traitement
        Dim baseTraitement As Char = DeterminationBaseTraitement()

        'Définition posologie journalière
        Dim PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir As Integer
        Dim Rythme As Integer

        If NumTraitementRythme.Value <> 0 Then
            Rythme = NumTraitementRythme.Value
        Else
            Rythme = 0
        End If

        If baseTraitement = "J" Then
            If ChkPosologieMatin.Checked = True Then
                PosologieMatin = Rythme
            End If

            If ChkPosologieMidi.Checked = True Then
                PosologieMidi = Rythme
            End If

            If ChkPosologieApresMidi.Checked = True Then
                PosologieApresMidi = Rythme
            End If

            If ChkPosologieSoir.Checked = True Then
                PosologieSoir = Rythme
            End If
        Else
            PosologieMatin = 0
            PosologieMidi = 0
            PosologieApresMidi = 0
            PosologieSoir = 0
        End If

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_traitement set oa_traitement_identifiant_modification = @utilisateurModification," &
        " oa_traitement_ordre_affichage = @ordreAffichage, oa_traitement_date_modification = @dateModification, oa_traitement_fenetre = @fenetreExiste," &
        " oa_traitement_fenetre_commentaire = @fenetreCommentaire, oa_traitement_fenetre_date_debut = @dateFenetreDebut," &
        " oa_traitement_fenetre_date_fin = @dateFenetreFin where oa_traitement_id = @traitementId;"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", SelectedPatient.patientId.ToString)
            .AddWithValue("@ordreAffichage", NumNumeroOrdre.Value.ToString)
            .AddWithValue("@dateModification", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@fenetreExiste", "1")
            .AddWithValue("@fenetreCommentaire", TxtFenetreTherapeutiqueCommentaire.Text)
            .AddWithValue("@datefenetreDebut", DteFenetreTherapeutiqueDateDebut.Value)
            .AddWithValue("@datefenetreFin", DteFenetreTherapeutiqueDateFin.Value)
            .AddWithValue("@traitementId", SelectedTraitementId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            If FenetreTherapeutiqueExiste = True Then
                MessageBox.Show("Fenêtre thérapeutique modifiée")
            Else
                MessageBox.Show("Fenêtre thérapeutique créée")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement
            TraitementHistoACreer.HistorisationDate = Date.Now()
            TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.ModificationFenetreTherapeutique
            TraitementHistoACreer.HistorisationFenetreDateDebut = DteFenetreTherapeutiqueDateDebut.Value
            TraitementHistoACreer.HistorisationFenetreDateFin = DteFenetreTherapeutiqueDateFin.Value
            TraitementHistoACreer.HistorisationFenetreCommentaire = TxtFenetreTherapeutiqueCommentaire.Text
            TraitementHistoACreer.HistorisationFenetre = True

            'Création dans l'historique des modifications de traitement
            Select Case EditMode
                Case "C"
                    TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.CreationFenetreTherapeutique
                    CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.CreationFenetreTherapeutique)
                Case "M"
                    TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.ModificationFenetreTherapeutique
                    CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.ModificationFenetreTherapeutique)
                Case Else
                    '?
            End Select
        End If

        Return codeRetour
    End Function

    'Suppression fenêtre thérapeutique
    Private Function SuppressionFenetreTherapeutique() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date
        Dim FenetreDateDebut, FenetreDateFin As Date
        FenetreDateDebut = "31/12/2999"
        FenetreDateFin = "01/01/1900"

        Dim SQLstring As String = "update oasis.oa_traitement set oa_traitement_identifiant_modification = @utilisateurModification," &
        " oa_traitement_date_modification = @dateModification, oa_traitement_fenetre_commentaire = @fenetreCommentaire, oa_traitement_fenetre = @fenetreExiste," &
        " oa_traitement_fenetre_date_debut = @datefenetreDebut, oa_traitement_fenetre_date_fin = @datefenetreFin where oa_traitement_id = @traitementId"

        Dim cmd As New SqlCommand(SQLstring, conxn)


        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@fenetreExiste", "0")
            .AddWithValue("@fenetreCommentaire", "")
            .AddWithValue("@datefenetreDebut", FenetreDateDebut)
            .AddWithValue("@datefenetreFin", FenetreDateFin)
            .AddWithValue("@traitementId", SelectedTraitementId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()

            MessageBox.Show("Fenêtre thérapeutique supprimée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement
            TraitementHistoACreer.HistorisationDate = Date.Now()
            TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.SuppressionFenetreTherapeutique
            TraitementHistoACreer.HistorisationFenetreDateDebut = DteFenetreTherapeutiqueDateDebut.Value
            TraitementHistoACreer.HistorisationFenetreDateFin = DteFenetreTherapeutiqueDateFin.Value
            TraitementHistoACreer.HistorisationFenetreCommentaire = TxtFenetreTherapeutiqueCommentaire.Text
            TraitementHistoACreer.HistorisationFenetre = False

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.SuppressionFenetreTherapeutique)
        End If

        Return codeRetour
    End Function


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones de l'écran ============
    '=============================================================================================

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")
    End Sub

    Private Sub ChargementTraitementExistant()
        Dim traitementDataReader As SqlDataReader
        SQLString = "select * from oasis.oa_traitement where oa_traitement_id = " & SelectedTraitementId & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        conxn.Open()
        traitementDataReader = myCommand.ExecuteReader()
        If traitementDataReader.Read() Then
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
            Dim BaseSaisie As Char
            Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
            Dim Rythme As Integer
            Dim FenetreDateDebut, FenetreDateFin As Date
            Dim dateFin, dateDebut, dateCreation, dateModification As Date

            'Vérification de l'existence d'une fenêtre thérapeutique
            If FenetreTherapeutiqueExiste = True Then
                If traitementDataReader("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                    FenetreDateDebut = traitementDataReader("oa_traitement_fenetre_date_debut")
                    DteFenetreTherapeutiqueDateDebut.Value = FenetreDateDebut.ToString("dd.MM.yyyy")
                Else
                    FenetreDateDebut = "31/12/2999"
                End If
                If traitementDataReader("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                    FenetreDateFin = traitementDataReader("oa_traitement_fenetre_date_fin")
                    DteFenetreTherapeutiqueDateFin.Value = FenetreDateFin.ToString("dd.MM.yyyy")
                Else
                    FenetreDateFin = "01/01/1900"
                End If
            Else
                DteFenetreTherapeutiqueDateDebut.Value = Date.Now
                DteFenetreTherapeutiqueDateFin.Value = Date.Now
            End If

            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If traitementDataReader("oa_traitement_fenetre") IsNot DBNull.Value Then
                If traitementDataReader("oa_traitement_fenetre") = "1" Then
                    If dateDebutFenetreaComparer <= dateJouraComparer And dateFinFenetreaComparer > dateJouraComparer Then
                        LblFentreTherapeutiqueExistante.Text = "--- Fenêtre en cours ---"
                    Else
                        If FenetreDateDebut > dateJouraComparer Then
                            LblFentreTherapeutiqueExistante.Text = "--- Fenêtre à venir ---"
                        Else
                            LblFentreTherapeutiqueExistante.Text = "(Fenêtre obsolète)"
                        End If
                    End If
                End If
            End If

            If FenetreTherapeutiqueExiste = True Then
                'Posologie = "Fenêtre Th."
            End If

            If traitementDataReader("oa_traitement_fenetre_commentaire") IsNot DBNull.Value Then
                TxtFenetreTherapeutiqueCommentaire.Text = traitementDataReader("oa_traitement_fenetre_commentaire")
            Else
                TxtFenetreTherapeutiqueCommentaire.Text = ""
            End If

            'Traitement de la posologie
            If traitementDataReader("oa_traitement_posologie_base") IsNot DBNull.Value Then
                Rythme = traitementDataReader("oa_traitement_posologie_rythme")
                NumTraitementRythme.Value = Rythme
                BaseSaisie = traitementDataReader("oa_traitement_posologie_base")
                Select Case traitementDataReader("oa_traitement_posologie_base")
                    Case "J"
                        CbxTraitement.Text = "Journalier"
                        MontrerPosologieJournaliere()
                        If traitementDataReader("oa_traitement_posologie_matin") <> 0 Then
                            posologieMatin = Rythme
                            ChkPosologieMatin.Checked = True
                        Else
                            posologieMatin = 0
                            ChkPosologieMatin.Checked = False
                        End If
                        If traitementDataReader("oa_traitement_posologie_midi") <> 0 Then
                            posologieMidi = Rythme
                            ChkPosologieMidi.Checked = True
                        Else
                            posologieMidi = 0
                            ChkPosologieMidi.Checked = False
                        End If
                        If traitementDataReader("oa_traitement_posologie_soir") <> 0 Then
                            posologieSoir = Rythme
                            ChkPosologieSoir.Checked = True
                        Else
                            posologieSoir = 0
                            ChkPosologieSoir.Checked = False
                        End If
                        If traitementDataReader("oa_traitement_posologie_apres_midi") <> 0 Then
                            posologieApresMidi = Rythme
                            ChkPosologieApresMidi.Checked = True
                        Else
                            ChkPosologieApresMidi.Checked = False
                        End If
                    Case "H"
                        CbxTraitement.Text = "Hebdomadaire"
                        CacherPosologieJournaliere()
                    Case "M"
                        CbxTraitement.Text = "Mensuel"
                        CacherPosologieJournaliere()
                    Case "A"
                        CbxTraitement.Text = "Annuel"
                        CacherPosologieJournaliere()
                    Case Else
                        CbxTraitement.Text = ""
                        CacherPosologieJournaliere()
                End Select
            End If

            'Alimentation des données à afficher
            LblTraitementPosologie.Text = FormatagePosologie(Rythme, BaseSaisie, posologieMatin, posologieMidi, posologieApresMidi, posologieSoir)

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
                    LblTraitementDuree.Text = CalculDureeTraitementEnJourString(dateDebut, dateFin)
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

            'Utilisateur et date création
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
                Dim userDao As New UserDao
                UtilisateurHisto = userDao.getUserById(traitementDataReader("oa_traitement_identifiant_creation"))
                'SetUtilisateur(UtilisateurHisto, traitementDataReader("oa_traitement_identifiant_creation"))
                LblUtilisateurCreation.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
            End If

            If traitementDataReader("oa_traitement_date_modification") IsNot DBNull.Value Then
                dateModification = traitementDataReader("oa_traitement_date_modification")
                LblTraitementDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblTraitementDateModification.Text = ""
                LblLabelTraitementDateModification.Hide()
                LblLabelTraitementParModification.Hide()
            End If

            LblUtilisateurModification.Text = ""
            If traitementDataReader("oa_traitement_identifiant_modification") IsNot DBNull.Value Then
                If traitementDataReader("oa_traitement_identifiant_modification") <> 0 Then
                    Dim userDao As New UserDao
                    UtilisateurHisto = userDao.getUserById(traitementDataReader("oa_traitement_identifiant_modification"))
                    'SetUtilisateur(UtilisateurHisto, traitementDataReader("oa_traitement_identifiant_modification"))
                    LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
                End If
            End If
        End If

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()
    End Sub

    Private Sub ChargementMedoc()
        If EditMode = "C" Then
            LblTraitementMedicamentCIS.Text = medicament_selecteur_cis.ToString
        End If

        Dim medicamentDataReader As SqlDataReader
        SQLString = "select * from oasis.oa_r_medicament where oa_medicament_cis = " & medicament_selecteur_cis.ToString & ";"
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
                LblTraitementDuree.Text = CalculDureeTraitementEnJourString(DateDebut, DateFin)
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

        If NumTraitementRythme.Value <> 0 Then
            Rythme = NumTraitementRythme.Value
        Else
            Rythme = 0
        End If

        Select Case CbxTraitement.Text
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

        If ChkPosologieMatin.Checked = True Then
            PosologieMatin = Rythme
        End If

        If ChkPosologieMidi.Checked = True Then
            PosologieMidi = Rythme
        End If

        If ChkPosologieApresMidi.Checked = True Then
            PosologieApresMidi = Rythme
        End If

        If ChkPosologieSoir.Checked = True Then
            PosologieSoir = Rythme
        End If

        LblTraitementPosologie.Text = FormatagePosologie(Rythme, BaseSelection, PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir)
    End Sub

    Private Sub CacherPosologieJournaliere()
        ChkPosologieMatin.Hide()
        ChkPosologieMatin.Checked = False
        ChkPosologieMidi.Hide()
        ChkPosologieMidi.Checked = False
        ChkPosologieApresMidi.Hide()
        ChkPosologieApresMidi.Checked = False
        ChkPosologieSoir.Hide()
        ChkPosologieSoir.Checked = False
    End Sub

    Private Sub MontrerPosologieJournaliere()
        ChkPosologieMatin.Show()
        ChkPosologieMatin.Checked = False
        ChkPosologieMidi.Show()
        ChkPosologieMidi.Checked = False
        ChkPosologieApresMidi.Show()
        ChkPosologieApresMidi.Checked = False
        ChkPosologieSoir.Show()
        ChkPosologieSoir.Checked = False
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
        'Si l'utilisateur n'a pas les droits requis ou que le traitement a été arrêté, les zones de saisie ne sont pas modifiables 
        If UtilisateurConnecte.UtilisateurNiveauAcces <> 1 Then
            Me.Text = "Visualisation détail traitement patient"
            InhiberZonesDeSaisie()
        End If
    End Sub

    Private Sub InhiberZonesDeSaisie()
        RadBtnSupprimerFenetre.Visible = False
        RadBtnValidationFenetre.Visible = False
        CbxTraitement.Enabled = False
        NumTraitementRythme.Enabled = False
        NumNumeroOrdre.Enabled = False
        ChkPosologieMatin.Enabled = False
        ChkPosologieMidi.Enabled = False
        ChkPosologieApresMidi.Enabled = False
        ChkPosologieSoir.Enabled = False
        DteTraitementDateDebut.Enabled = False
        DteTraitementDateFin.Enabled = False
        TxtTraitementCommentaire.ReadOnly = True
        TxtTraitementPosologieCommentaire.ReadOnly = True
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

    '=============================================================================================
    '==================================== Traitement divers ======================================
    '=============================================================================================

    'Détermination de la base traitement à partir du combo box
    Private Function DeterminationBaseTraitement() As Char
        Dim baseTraitement As Char
        Select Case CbxTraitement.Text
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

    Private Sub RadBtnPharmacocinetique_Click(sender As Object, e As EventArgs) Handles RadBtnPharmacocinetique.Click
        Dim PharmacoCinetique As String = theriaqueDao.GetPharmacoCinetiqueBySpecialite(medicament_selecteur_cis)
        Me.Enabled = False
        Using form As New RadFAffichaeInfo
            form.InfoToDisplay = PharmacoCinetique
            form.Titre = "Information Pharmacocinétique"
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnParmacodynamique_Click(sender As Object, e As EventArgs) Handles RadBtnParmacodynamique.Click
        Dim PharmacoCinetique As String = theriaqueDao.GetPharmacoDynamiqueBySpecialite(medicament_selecteur_cis)
        Me.Enabled = False
        Using form As New RadFAffichaeInfo
            form.InfoToDisplay = PharmacoCinetique
            form.Titre = "Information Pharmacodynamique"
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnEffetIndesirable_Click(sender As Object, e As EventArgs) Handles RadBtnEffetIndesirable.Click
        Me.Enabled = False
        Using form As New RadFEffetSecondaire
            form.MedicamentId = medicament_selecteur_cis
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnSubstance_Click(sender As Object, e As EventArgs) Handles RadBtnSubstance.Click
        Me.Enabled = False
        Using form As New RadFSubstancesListe
            form.SelectedSpecialite = medicament_selecteur_cis
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

End Class
