﻿Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Telerik.WinControls
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common

Public Class RadFTraitementDetailEdit
    Private privateSelectedPatient As Patient
    'Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedMedicamentId As Integer
    Private privateSelectedTraitementId As Integer
    Private privateAllergie As Boolean
    Private privateContreIndication As Boolean
    Private privateCodeRetour As Boolean
    Private _positionGaucheDroite As Integer
    Dim patientDao As New PatientDao

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    'Public Property UtilisateurConnecte As Utilisateur
    'Get
    'Return privateUtilisateurConnecte
    'End Get
    'Set(value As Utilisateur)
    'privateUtilisateurConnecte = value
    'Set
    'End Property

    Public Property SelectedMedicamentId As Integer
        Get
            Return privateSelectedMedicamentId
        End Get
        Set(value As Integer)
            privateSelectedMedicamentId = value
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

    Public Property Allergie As Boolean
        Get
            Return privateAllergie
        End Get
        Set(value As Boolean)
            privateAllergie = value
        End Set
    End Property

    Public Property ContreIndication As Boolean
        Get
            Return privateContreIndication
        End Get
        Set(value As Boolean)
            privateContreIndication = value
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

    Public Property PositionGaucheDroite As Integer
        Get
            Return _positionGaucheDroite
        End Get
        Set(value As Integer)
            _positionGaucheDroite = value
        End Set
    End Property

    Enum EnumAction
        Creation = 1
        Modification = 2
        ArretTraitement = 3
        AnnulerTraitement = 4
        Sans = 5
    End Enum

    Dim traitementDao As New TraitementDao
    Dim TheriaqueDao As New TheriaqueDao

    'Déclaration des variables de travail
    Dim EditMode As Char = ""
    Dim ActionEnCours As Integer
    Dim conxn As New SqlConnection(getConnectionString())
    Dim SQLString As String
    Dim medicament_selecteur_cis As Integer
    Dim medicamentMonographie As Integer
    Private utilisateurCreation As Integer
    Private dateCreationTraitement As Date
    Private traitementArrete As Boolean = False
    Dim TraitementHistoACreer As New TraitementHisto
    Dim UtilisateurHisto As Utilisateur = New Utilisateur()

    Dim classeAtc As String

    Private Sub RadFTraitementDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()
        If PositionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If

        InitZone()
        DroitAcces()
        ChargementEtatCivil()

        'Si allergie, affichage des substances allergiques
        If Allergie = True Then
        End If

        If ContreIndication = True Then
            lblContreIndication.Show()
        End If

        'Chargement de l'écran
        If SelectedTraitementId = 0 Then
            EditMode = "C" 'Création
            AfficheTitleForm(Me, "Création traitement patient", userLog)
            LblstatutTraitement.Hide()
            RadPnlStatutTraitement.Hide()
            'Récupération du cis du médicament sélectionné
            medicament_selecteur_cis = SelectedMedicamentId
            'Chargement des données du médicament sélectionné
            ChargementMedoc()
            'Cacher la posologie journalière car la base n'a pas encore été saisie
            CacherPosologieJournaliere()
            CbxTraitement.Text = Traitement.EnumBaseItem.JOURNALIER
            CbxFractionMatin.Text = Traitement.EnumFraction.Non
            CbxFractionMidi.Text = Traitement.EnumFraction.Non
            CbxFractionApresMidi.Text = Traitement.EnumFraction.Non
            CbxFractionSoir.Text = Traitement.EnumFraction.Non

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
            LblTraitementDateCreation.Hide()
            LblTraitementDateModification.Hide()
            LblUtilisateurCreation.Hide()
            LblUtilisateurModification.Hide()
            LblLabelTraitementDateModification.Hide()
            LblLabelTraitementParModification.Hide()
            LblLabelTraitementDateCreation.Hide()
            LblLabelTraitementParCreation.Hide()
            'Cacher les boutons : Supprimer, Arrêt et Annuler
            RadBtnAnnulerTraitement.Hide()
            RadBtnArretTraitement.Hide()
            RadBtnSupprimerTraitement.Hide()
            RadBtnHistorique.Hide()
            ActionEnCours = EnumAction.Creation
        Else
            EditMode = "M" 'Modification
            AfficheTitleForm(Me, "Modification traitement patient", userLog)
            LblstatutTraitement.Hide()
            RadPnlStatutTraitement.Hide()
            'Chargement du traitement à modifier
            ChargementTraitementExistant()
            'Chargement des données du médicament du traitement
            ChargementMedoc()
            ActionEnCours = EnumAction.Modification
        End If

        Cursor.Current = Cursors.Default
    End Sub


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones de l'écran ============
    '=============================================================================================
    'Chargement des données dans les labels dédiés
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

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.PatientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If

        'Contre-indication
        GetContreIndication()

        'Allergie
        GetAllergie()
    End Sub

    Private Sub GetContreIndication()
        Dim StringContreIndicationToolTip As String = patientDao.GetStringContreIndicationByPatient(SelectedPatient.PatientId)
        If StringContreIndicationToolTip = "" Then
            lblContreIndication.Hide()
        Else
            lblContreIndication.Show()
            ToolTip.SetToolTip(lblContreIndication, StringContreIndicationToolTip)
        End If
    End Sub

    Private Sub GetAllergie()
        Dim StringAllergieToolTip As String = patientDao.GetStringAllergieByPatient(SelectedPatient.PatientId)
        If StringAllergieToolTip = "" Then
            LblAllergie.Hide()
        Else
            LblAllergie.Show()
            ToolTip.SetToolTip(LblAllergie, StringAllergieToolTip)
        End If
    End Sub

    Private Sub ChargementTraitementExistant()
        Dim traitementDao As TraitementDao = New TraitementDao
        Dim traitement As Traitement

        Try
            traitement = traitementDao.GetTraitementById(SelectedTraitementId)
        Catch ex As Exception
            MessageBox.Show("Traitement : " + ex.Message, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        'Stockage de l'utilisateur qui a créé le traitement pour le contrôle en cas de demande de suppression du traitement
        utilisateurCreation = traitement.UserCreation
        medicament_selecteur_cis = traitement.MedicamentId
        LblTraitementMedicamentId.Text = traitement.MedicamentId
        LblMedicamentDCI.Text = traitement.MedicamentDci

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
        RadGbxFenetreTherapeutique.Hide()

        'Récupération de la période d'application du traitement
        dateFin = traitement.DateFin
        If dateFin = Nothing Then
            dateFin = "31/12/2999"
        End If

        'Si le traitement a été déclaré arrêté, ce traitement ne doit pas pouvoir être modifié
        If traitement.Arret = "A" Then
            traitementArrete = True
            Me.Text = "Visualisation détail traitement patient (traitement arrêté)"
            LblstatutTraitement.Text = "Visualisation détail traitement patient (traitement arrêté)"
            LblstatutTraitement.Show()
            RadPnlStatutTraitement.Show()
            InhiberZonesDeSaisie()
            AfficherZonesArret()
            InhiberZonesDeSaisieArret()
            ChargerZonesArret(traitement)
        End If

        'Si le traitement a été déclaré allergie ou contre-indication, ce traitement ne doit pas pouvoir être modifié

        If traitement.DeclaratifHorsTraitement = True Then
            Me.Text = "Visualisation détail traitement (Déclaration allergie ou contre-indication)"
            LblstatutTraitement.Text = "Visualisation détail traitement (Déclaration allergie ou contre-indication)"
            LblstatutTraitement.Show()
            RadPnlStatutTraitement.Show()
            InhiberZonesDeSaisie()
            AfficherZonesArret()
            InhiberZonesDeSaisieArret()
            ChargerZonesArret(traitement)
        End If


        'Si le traitement a été déclaré annulé, ce traitement ne doit pas pouvoir être modifié

        If traitement.Annulation = "A" Then
            traitementArrete = True
            Me.Text = "Visualisation détail traitement patient (traitement annulé)"
            LblstatutTraitement.Text = "Visualisation détail traitement patient (traitement annulé)"
            LblstatutTraitement.Show()
            RadPnlStatutTraitement.Show()
            InhiberZonesDeSaisie()
            AfficherZonesAnnulation()
            InhiberZonesDeSaisieAnnulation()
            ChargerZonesAnnulation(traitement)
        End If


        'Si la date de fin est inférieure à la date du jour, ce traitement est terminé et ne doit pas pouvoir être modifié
        Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
        If (dateFinaComparer < dateJouraComparer) Then
            If traitementArrete = False Then
                Me.Text = "Visualisation détail traitement patient (traitement terminé)"
                LblstatutTraitement.Text = "Visualisation détail traitement patient (traitement terminé)"
                LblstatutTraitement.Show()
                RadPnlStatutTraitement.Show()
                InhiberZonesDeSaisie()
            End If
        End If

        'Vérification de l'existence d'une fenêtre thérapeutique
        Fenetre = False
        If traitement.FenetreDateDebut <> Nothing Then
            FenetreDateDebut = traitement.FenetreDateDebut
            LblFenetreDateDebut.Text = FenetreDateDebut.ToString("dd.MM.yyyy")
        Else
            FenetreDateDebut = "31/12/2999"
        End If
        If traitement.DateFin <> Nothing Then
            FenetreDateFin = traitement.FenetreDateFin
            LblFenetreDateFin.Text = FenetreDateFin.ToString("dd.MM.yyyy")
        Else
            FenetreDateFin = "01/01/1900"
        End If

        Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
        Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
        If traitement.Fenetre = True Then
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
            RadGbxFenetreTherapeutique.Show()
        End If

        'Traitement de la posologie
        If traitement.PosologieBase <> "" Then
            Rythme = traitement.PosologieRythme
            NumRythmeMatin.Value = Rythme
            BaseSelection = traitement.PosologieBase
            Select Case traitement.FractionMatin
                Case Traitement.EnumFraction.Non
                    CbxFractionMatin.Text = Traitement.EnumFraction.Non
                Case Traitement.EnumFraction.Quart
                    CbxFractionMatin.Text = Traitement.EnumFraction.Quart
                Case Traitement.EnumFraction.Demi
                    CbxFractionMatin.Text = Traitement.EnumFraction.Demi
                Case Traitement.EnumFraction.TroisQuart
                    CbxFractionMatin.Text = Traitement.EnumFraction.TroisQuart
                Case Else
                    CbxFractionMatin.Text = Traitement.EnumFraction.Non
            End Select

            Select Case BaseSelection
                Case Traitement.EnumBaseCode.JOURNALIER
                    Select Case BaseSelection
                        Case Traitement.EnumBaseCode.JOURNALIER
                            Base = Traitement.EnumBaseItem.JOURNALIER & " : "
                            CbxTraitement.Text = Traitement.EnumBaseItem.JOURNALIER
                    End Select
                    MontrerPosologieJournaliere()
                    If traitement.PosologieMatin <> 0 Then
                        posologieMatin = traitement.PosologieMatin
                        NumRythmeMatin.Value = traitement.PosologieMatin
                    Else
                        posologieMatin = 0
                        NumRythmeMatin.Value = 0
                    End If

                    If traitement.PosologieMidi <> 0 Then
                        posologieMidi = traitement.PosologieMidi
                        NumRythmeMidi.Value = traitement.PosologieMidi
                    Else
                        posologieMidi = 0
                        NumRythmeMidi.Value = 0
                    End If
                    Select Case traitement.FractionMidi
                        Case Traitement.EnumFraction.Non
                            CbxFractionMidi.Text = Traitement.EnumFraction.Non
                        Case Traitement.EnumFraction.Quart
                            CbxFractionMidi.Text = Traitement.EnumFraction.Quart
                        Case Traitement.EnumFraction.Demi
                            CbxFractionMidi.Text = Traitement.EnumFraction.Demi
                        Case Traitement.EnumFraction.TroisQuart
                            CbxFractionMidi.Text = Traitement.EnumFraction.TroisQuart
                        Case Else
                            CbxFractionMidi.Text = Traitement.EnumFraction.Non
                    End Select
                    If traitement.PosologieApresMidi <> 0 Then
                        posologieApresMidi = traitement.PosologieApresMidi
                        NumRythmeApresMidi.Value = traitement.PosologieApresMidi
                    Else
                        posologieApresMidi = 0
                        NumRythmeApresMidi.Value = 0
                    End If
                    Select Case traitement.FractionApresMidi
                        Case Traitement.EnumFraction.Non
                            CbxFractionApresMidi.Text = Traitement.EnumFraction.Non
                        Case Traitement.EnumFraction.Quart
                            CbxFractionApresMidi.Text = Traitement.EnumFraction.Quart
                        Case Traitement.EnumFraction.Demi
                            CbxFractionApresMidi.Text = Traitement.EnumFraction.Demi
                        Case Traitement.EnumFraction.TroisQuart
                            CbxFractionApresMidi.Text = Traitement.EnumFraction.TroisQuart
                        Case Else
                            CbxFractionApresMidi.Text = Traitement.EnumFraction.Non
                    End Select
                    If traitement.PosologieSoir <> 0 Then
                        posologieSoir = traitement.PosologieSoir
                        NumRythmeSoir.Value = traitement.PosologieSoir
                    Else
                        posologieSoir = 0
                        NumRythmeSoir.Value = 0
                    End If
                    Select Case traitement.FractionSoir
                        Case Traitement.EnumFraction.Non
                            CbxFractionSoir.Text = Traitement.EnumFraction.Non
                        Case Traitement.EnumFraction.Quart
                            CbxFractionSoir.Text = Traitement.EnumFraction.Quart
                        Case Traitement.EnumFraction.Demi
                            CbxFractionSoir.Text = Traitement.EnumFraction.Demi
                        Case Traitement.EnumFraction.TroisQuart
                            CbxFractionSoir.Text = Traitement.EnumFraction.TroisQuart
                        Case Else
                            CbxFractionSoir.Text = Traitement.EnumFraction.Non
                    End Select
                Case Traitement.EnumBaseCode.HEBDOMADAIRE
                    Base = Traitement.EnumBaseItem.HEBDOMADAIRE & " : "
                    CbxTraitement.Text = Traitement.EnumBaseItem.HEBDOMADAIRE
                    CacherPosologieJournaliere()
                Case Traitement.EnumBaseCode.MENSUEL
                    Base = Traitement.EnumBaseItem.MENSUEL & " : "
                    CbxTraitement.Text = Traitement.EnumBaseItem.MENSUEL
                    CacherPosologieJournaliere()
                Case Traitement.EnumBaseCode.ANNUEL
                    Base = Traitement.EnumBaseItem.ANNUEL & " : "
                    CbxTraitement.Text = Traitement.EnumBaseItem.ANNUEL
                    CacherPosologieJournaliere()
                Case Traitement.EnumBaseCode.CONDITIONNEL
                    Base = Traitement.EnumBaseItem.CONDITIONNEL & " : "
                    CbxTraitement.Text = Traitement.EnumBaseItem.CONDITIONNEL
                    CacherPosologieJournaliere()
                Case Else
                    Base = "Base inconnue ! "
                    CbxTraitement.Text = ""
                    CacherPosologieJournaliere()
            End Select


            'Alimentation des données à afficher
            LblTraitementPosologie.Text = FormatagePosologie(Rythme, BaseSelection, posologieMatin, posologieMidi, posologieApresMidi, posologieSoir)

            TxtTraitementPosologieCommentaire.Text = traitement.PosologieCommentaire

            If traitement.DateDebut <> Nothing Then
                dateDebut = traitement.DateDebut
                DteTraitementDateDebut.Value = dateDebut
            End If

            NumNumeroOrdre.Value = traitement.OrdreAffichage

            If traitement.DateFin <> Nothing Then
                dateFin = traitement.DateFin
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

            TxtTraitementCommentaire.Text = traitement.Commentaire

            If traitement.DateCreation <> Nothing Then
                dateCreation = traitement.DateCreation
                dateCreationTraitement = traitement.DateCreation
                LblTraitementDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
            Else
                LblTraitementDateCreation.Text = ""
                LblLabelTraitementDateCreation.Hide()
                LblLabelTraitementParCreation.Hide()
            End If

            LblUtilisateurCreation.Text = ""

            If traitement.UserCreation <> 0 Then
                Dim userDao As New UserDao
                UtilisateurHisto = userDao.GetUserById(traitement.UserCreation)
                'SetUtilisateur(UtilisateurHisto, traitement.UserCreation)
                LblUtilisateurCreation.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
            End If

            'Contrôle si on peut traiter la suppression, la suppression est permise si la date de création = date de suppression
            Dim dateCreationaComparer As New Date(dateCreationTraitement.Year, dateCreationTraitement.Month, dateCreationTraitement.Day, 0, 0, 0)
            If dateCreationaComparer <> dateJouraComparer Then
                RadBtnSupprimerTraitement.Hide()
            End If

            If traitement.DateModification <> Nothing Then
                dateModification = traitement.DateModification
                LblTraitementDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblTraitementDateModification.Text = ""
                LblLabelTraitementDateModification.Hide()
                LblLabelTraitementParModification.Hide()
            End If
        End If

        LblUtilisateurModification.Text = ""
        If traitement.UserModification <> 0 Then
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.GetUserById(traitement.UserModification)
            'SetUtilisateur(UtilisateurHisto, traitement.UserModification)
            LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        'Initialisation classe Historisation traitement 
        InitClasseTraitementHistorisation(traitement, userLog, TraitementHistoACreer)
    End Sub

    Private Sub ChargementMedoc()
        If EditMode = "C" Then
            LblTraitementMedicamentId.Text = SelectedMedicamentId.ToString
        End If

        Dim dt As DataTable
        dt = TheriaqueDao.GetSpecialiteByArgument(SelectedMedicamentId.ToString, TheriaqueDao.EnumGetSpecialite.ID_THERIAQUE, TheriaqueDao.EnumMonoVir.NULL)
        If dt.Rows.Count > 0 Then
            LblMedicamentDCI.Text = dt.Rows(0)("SP_NOM")
            LblMedicamentDCI.Text = LblMedicamentDCI.Text.Replace(" §", "")
            LblMedicamentDenominationLongue.Text = dt.Rows(0)("SP_NOMLONG")
            LblMedicamentDenominationLongue.Text = LblMedicamentDenominationLongue.Text.Replace("(MEDICAMENT VIRTUEL)", "")
            Dim ATCDenomination As String = TheriaqueDao.GetATCDenominationById(Coalesce(dt.Rows(0)("SP_CATC_CODE_FK"), ""))
            LblATC.Text = Coalesce(dt.Rows(0)("SP_CATC_CODE_FK"), "") & " - " & ATCDenomination
            Dim monographie As Boolean = Coalesce(dt.Rows(0)("MONO_VIR"), 0)
            If monographie = True Then
                medicamentMonographie = 1
            Else
                medicamentMonographie = 0
            End If
            classeAtc = dt.Rows(0)("SP_CATC_CODE_FK")
        Else
            LblMedicamentDCI.Text = ""
            LblMedicamentDenominationLongue.Text = ""
            LblMedicamentAdministration.Text = ""
            LblMedicamentTitulaire.Text = ""
            LblATC.Text = ""
            medicamentMonographie = 0
            classeAtc = ""
        End If
    End Sub

    Private Sub ChargerZonesArret(traitement As Traitement)
        TxtCommentaireArret.Text = traitement.ArretCommentaire
        ChkAllergie.Checked = False
        If traitement.Allergie = True Then
            ChkAllergie.Checked = True
            ChkAllergie.ForeColor = Color.Red
        Else
            ChkAllergie.ForeColor = Color.Black
        End If

        ChkContreIndication.Checked = False
        If traitement.ContreIndication = True Then
            ChkContreIndication.Checked = True
            ChkContreIndication.ForeColor = Color.Red
        Else
            ChkContreIndication.ForeColor = Color.Black
        End If
    End Sub

    Private Sub ChargerZonesAnnulation(traitement As Traitement)
        TxtCommentaireAnnulation.Text = traitement.AnnulationCommentaire
    End Sub


    '=============================================================================================
    '==================================== Gestion des boutons d'action ===========================
    '=============================================================================================

    'Validation de l'action en cours
    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        'Création ou mise à jour de la table
        Select Case EditMode
            Case "C"
                '-------------Traitement de la création
                If ControleValiditationDonnees() = True Then
                    If CreationTraitement() = True Then
                        Me.CodeRetour = True
                        Close()
                    Else
                        MessageBox.Show("Erreur de mise à jour")
                    End If
                End If
            Case "M"
                Select Case ActionEnCours
                    Case EnumAction.Modification
                        '-------------Traitement de la modification
                        If ControleValiditationDonnees() = True Then
                            If ModificationTraitement() = True Then
                                Me.CodeRetour = True
                                Close()
                            End If
                        End If
                    Case EnumAction.ArretTraitement
                        '-------------Traitement de l'arrêt d'un traitement
                        TraitementArret()
                    Case EnumAction.AnnulerTraitement
                        '-------------Traitement de l'annulation d'un traitement
                        If TxtCommentaireAnnulation.Text <> "" Then
                            If AnnulationTraitement() = True Then
                                Me.CodeRetour = True
                                Close()
                            End If
                        Else
                            MessageBox.Show("La saisie du commentaire d'annulation est obligatoire" + vbCrLf + vbCrLf + "=> Solution : Saisissez un commentaire ou appuyer sur ""Retour""")
                        End If
                End Select
        End Select
    End Sub

    'Retour sur l'écran précédent
    Private Sub BtnRetour_Click(sender As Object, e As EventArgs)
        Close()
    End Sub

    'Arrêt du traitement
    Private Sub BtnArretTraitement_Click(sender As Object, e As EventArgs) Handles RadBtnArretTraitement.Click
        InhiberZonesDeSaisie()
        AfficherZonesArret()
        DteTraitementDateFin.Enabled = True
        ActionEnCours = EnumAction.ArretTraitement
        RadBtnValidation.Visible = True
        TxtCommentaireArret.Focus()
        TxtCommentaireArret.Select()
    End Sub

    'Annulation du traitement
    Private Sub BtnAnnulerTraitement_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulerTraitement.Click
        If MsgBox("Attention, l'annulation d'un traitement est à réaliser dans le cas où le patient n'a pas pris le traitement, confirmez-vous l'annulation", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            InhiberZonesDeSaisie()
            AfficherZonesAnnulation()
            ActionEnCours = EnumAction.AnnulerTraitement
            RadBtnValidation.Visible = True
            TxtCommentaireAnnulation.Focus()
            TxtCommentaireAnnulation.Select()
        End If
    End Sub

    'Appel de la suppression du traitement affiché
    Private Sub BtnSupprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimerTraitement.Click
        'Contrôle si on doit traiter la suppression ou l'annulation
        'Traitement de la Suppression si la date de création = date de suppression et même auteur, sinon annulation
        If MsgBox("Attention, vous allez supprimer un traitement, confirmez la suppression", MsgBoxStyle.YesNo Or MsgBoxStyle.Critical Or MsgBoxStyle.DefaultButton2, "") = MsgBoxResult.Yes Then
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

    'Inhiber date de fin de traitement
    Private Sub RadBtnInhiberDateFin_Click(sender As Object, e As EventArgs) Handles RadBtnInhiberDateFin.Click
        DteTraitementDateFin.Format = DateTimePickerFormat.Custom
        DteTraitementDateFin.CustomFormat = " "
        DteTraitementDateFin.Value = New Date(2999, 12, 31, 0, 0, 0)
    End Sub

    '=============================================================================================
    '==================================== Contrôle des données avant mise à jour de la BDD =======
    '=============================================================================================

    'Contrôle de la validation des données avant mise à jour de la base de données
    Private Function ControleValiditationDonnees() As Boolean
        Dim Valide As Boolean = True
        Dim messageErreur As String = ""
        Dim messageErreur1 As String = ""
        Dim messageErreur2 As String = ""
        Dim messageErreur3 As String = ""
        Dim messageErreur4 As String = ""
        Dim messageErreur5 As String = ""

        'Base obligatoire
        If CbxTraitement.Text = "" Then
            Valide = False
            LblTraitement.ForeColor = Color.Red
            messageErreur1 = "- La saisie de la Base de la posologie est obligatoire"
        Else
            Select Case CbxTraitement.Text
                Case Traitement.EnumBaseItem.JOURNALIER, Traitement.EnumBaseItem.HEBDOMADAIRE, Traitement.EnumBaseItem.MENSUEL, Traitement.EnumBaseItem.ANNUEL, Traitement.EnumBaseItem.CONDITIONNEL
                    LblTraitement.ForeColor = Color.Black
                Case Else
                    LblTraitement.ForeColor = Color.Red
                    Valide = False
                    messageErreur1 = "- La saisie de la Base de la posologie est obligatoire"
            End Select
        End If

        'Si rythme journalier, une posologie matin, midi, après-midi ou soir est requise
        If CbxTraitement.Text = Traitement.EnumBaseItem.JOURNALIER Then
            If NumRythmeMatin.Value = 0 AndAlso
                NumRythmeMidi.Value = 0 AndAlso
                NumRythmeApresMidi.Value = 0 AndAlso
                NumRythmeSoir.Value = 0 AndAlso
                CbxFractionMatin.Text = Traitement.EnumFraction.Non AndAlso
                CbxFractionMidi.Text = Traitement.EnumFraction.Non AndAlso
                CbxFractionApresMidi.Text = Traitement.EnumFraction.Non AndAlso
                CbxFractionSoir.Text = Traitement.EnumFraction.Non Then
                LblTraitementRythme.ForeColor = Color.Red
                Valide = False
                messageErreur2 = "- La saisie des périodes d'application (matin, midi, après-mid ou soir) de la posologie journalière est obligatoire"
            Else
                LblTraitementRythme.ForeColor = Color.Black
            End If
        Else
            'Rythme obligatoire
            If NumRythmeMatin.Value = 0 AndAlso
                CbxFractionMatin.Text = Traitement.EnumFraction.Non Then
                LblTraitementRythme.ForeColor = Color.Red
                Valide = False
                messageErreur3 = "- La saisie du rythme de la posologie est obligatoire"
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

        'Si la base est 'Conditionnel', le commentaire de la posologie est obligatoire
        If CbxTraitement.Text = Traitement.EnumBaseItem.CONDITIONNEL Then
            If TxtTraitementPosologieCommentaire.Text = "" Then
                Valide = False
                messageErreur5 = "- Le commentaire de la posologie est obligatoire quand la base est : '" & Traitement.EnumBaseItem.CONDITIONNEL & "'"
            End If
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

            If messageErreur5 <> "" Then
                messageErreur = messageErreur + messageErreur5 + vbCrLf
            End If

            messageErreur = messageErreur + vbCrLf + vbCrLf + "/!\ Validation impossible, des données sont incorrectes"

            MessageBox.Show(messageErreur, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        'Contrôler qu'une données a au moins été modifiée
        'TODO: Traitement détail - contrôle qu'une données a au moins été modifiée

        Return Valide
    End Function

    'Traitement de l'arrêt du traitement
    Private Sub TraitementArret()
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

    'Création d'un traitement en base de données
    Private Function CreationTraitement() As Boolean
        Cursor.Current = Cursors.WaitCursor

        Dim codeRetour As Boolean

        'Définition de la base du traitement
        Dim baseTraitement As Char = traitementDao.GetBaseCodeByItem(CbxTraitement.Text)

        Dim traitementaCreer As New Traitement With {
            .TraitementId = SelectedTraitementId,
            .PatientId = SelectedPatient.PatientId,
            .MedicamentId = medicament_selecteur_cis,
            .MedicamentDci = LblMedicamentDCI.Text,
            .DenominationLongue = LblMedicamentDenominationLongue.Text,
            .Allergie = False,
            .ContreIndication = False,
            .PosologieBase = traitementDao.GetBaseCodeByItem(CbxTraitement.Text),
            .PosologieRythme = NumRythmeMatin.Value,
            .PosologieMatin = NumRythmeMatin.Value,
            .PosologieMidi = NumRythmeMidi.Value,
            .PosologieApresMidi = NumRythmeApresMidi.Value,
            .PosologieSoir = NumRythmeSoir.Value,
            .FractionMatin = CbxFractionMatin.Text,
            .FractionMidi = CbxFractionMidi.Text,
            .FractionApresMidi = CbxFractionApresMidi.Text,
            .FractionSoir = CbxFractionSoir.Text,
            .DateModification = Date.Now.Date,
            .OrdreAffichage = NumNumeroOrdre.Value,
            .UserCreation = userLog.UtilisateurId,
            .DateCreation = Date.Now(),
            .PosologieCommentaire = TxtTraitementPosologieCommentaire.Text,
            .Commentaire = TxtTraitementCommentaire.Text,
            .DateDebut = DteTraitementDateDebut.Value,
            .DateFin = DteTraitementDateFin.Value,
            .MedicamentMonographie = medicamentMonographie,
            .ClasseAtc = classeAtc
        }

        codeRetour = traitementDao.CreationTraitement(traitementaCreer, TraitementHistoACreer, userLog)
        If codeRetour = True Then
            Dim form As New RadFNotification()
            form.Message = "Traitement patient créé"
            form.Show()
        End If

        Cursor.Current = Cursors.Default

        Return codeRetour
    End Function

    'Modification d'un traitement en base de données
    Private Function ModificationTraitement() As Boolean
        Cursor.Current = Cursors.WaitCursor

        Dim codeRetour As Boolean = True

        'Définition de la base du traitement
        Dim baseTraitement As Char = traitementDao.GetBaseCodeByItem(CbxTraitement.Text)

        Dim traitementaModifier As New Traitement
        traitementaModifier.TraitementId = SelectedTraitementId
        traitementaModifier.MedicamentId = medicament_selecteur_cis
        traitementaModifier.MedicamentDci = LblMedicamentDCI.Text
        traitementaModifier.DenominationLongue = LblMedicamentDenominationLongue.Text
        traitementaModifier.PatientId = SelectedPatient.PatientId
        traitementaModifier.PosologieBase = traitementDao.GetBaseCodeByItem(CbxTraitement.Text)
        traitementaModifier.PosologieRythme = NumRythmeMatin.Value
        traitementaModifier.PosologieMatin = NumRythmeMatin.Value
        traitementaModifier.PosologieMidi = NumRythmeMidi.Value
        traitementaModifier.PosologieApresMidi = NumRythmeApresMidi.Value
        traitementaModifier.PosologieSoir = NumRythmeSoir.Value
        traitementaModifier.FractionMatin = CbxFractionMatin.Text
        traitementaModifier.FractionMidi = CbxFractionMidi.Text
        traitementaModifier.FractionApresMidi = CbxFractionApresMidi.Text
        traitementaModifier.FractionSoir = CbxFractionSoir.Text
        traitementaModifier.DateModification = Date.Now.Date
        traitementaModifier.OrdreAffichage = NumNumeroOrdre.Value
        traitementaModifier.UserModification = userLog.UtilisateurId
        traitementaModifier.DateModification = Date.Now()
        traitementaModifier.PosologieCommentaire = TxtTraitementPosologieCommentaire.Text
        traitementaModifier.Commentaire = TxtTraitementCommentaire.Text
        traitementaModifier.DateDebut = DteTraitementDateDebut.Value
        traitementaModifier.DateFin = DteTraitementDateFin.Value

        codeRetour = traitementDao.ModificationTraitement(traitementaModifier, TraitementHistoACreer, userLog)
        If codeRetour = True Then
            Dim form As New RadFNotification()
            form.Message = "Traitement patient modifié"
            form.Show()
        End If

        Cursor.Current = Cursors.Default
        Return codeRetour
    End Function

    'Arrêt traitement en base de données
    Private Function ArretTraitement() As Boolean
        Cursor.Current = Cursors.WaitCursor

        Dim codeRetour As Boolean = True
        Dim allergie, contreIndication As Integer

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

        Dim traitementaArreter As New Traitement
        traitementaArreter.TraitementId = SelectedTraitementId
        traitementaArreter.PatientId = SelectedPatient.PatientId
        traitementaArreter.DateModification = Date.Now.Date
        traitementaArreter.UserModification = userLog.UtilisateurId
        traitementaArreter.DateModification = Date.Now()
        traitementaArreter.ArretCommentaire = TxtCommentaireArret.Text
        traitementaArreter.DateFin = DteTraitementDateFin.Value
        traitementaArreter.Allergie = allergie
        traitementaArreter.ContreIndication = contreIndication

        codeRetour = traitementDao.ArretTraitement(traitementaArreter, TraitementHistoACreer, userLog)

        'Déclaration de traitement arrêté pour contre-indication
        If contreIndication = 1 Then
            Using formSelecteur As New RadF_CI_ATC_Selecteur
                formSelecteur.SelectedPatient = Me.SelectedPatient
                formSelecteur.SelectedSpecialiteId = CInt(LblTraitementMedicamentId.Text)
                formSelecteur.ShowDialog()
            End Using
        End If

        'Déclaration de traitement arrêté pour allergie
        If allergie = 1 Then
            Using formSelecteur As New RadF_AllergieSelecteur
                formSelecteur.SelectedPatient = Me.SelectedPatient
                formSelecteur.SelectedSpecialiteId = CInt(LblTraitementMedicamentId.Text)
                formSelecteur.ShowDialog()
            End Using
        End If

        If codeRetour = True Then
            Dim form As New RadFNotification()
            form.Message = "Traitement patient arrêté"
            form.Show()
        End If

        Cursor.Current = Cursors.Default
        Return codeRetour
    End Function

    'Annulation d'un traitement en base de données
    Private Function AnnulationTraitement() As Boolean
        Cursor.Current = Cursors.WaitCursor

        Dim codeRetour As Boolean = True

        Dim traitementaAnnuler As New Traitement
        traitementaAnnuler.TraitementId = SelectedTraitementId
        traitementaAnnuler.PatientId = SelectedPatient.PatientId
        traitementaAnnuler.DateModification = Date.Now.Date
        traitementaAnnuler.UserModification = userLog.UtilisateurId
        traitementaAnnuler.DateModification = Date.Now()
        traitementaAnnuler.AnnulationCommentaire = TxtCommentaireAnnulation.Text
        traitementaAnnuler.DateFin = DteTraitementDateFin.Value

        codeRetour = traitementDao.AnnulationTraitement(traitementaAnnuler, TraitementHistoACreer, userLog)
        If codeRetour = True Then
            Dim form As New RadFNotification()
            form.Message = "Traitement patient annulé"
            form.Show()
        End If

        Cursor.Current = Cursors.Default
        Return codeRetour
    End Function

    'Suppression traitement
    Private Function SuppressionTraitement() As Boolean
        Cursor.Current = Cursors.WaitCursor

        Dim codeRetour As Boolean = True

        Dim traitementaAnnuler As New Traitement
        traitementaAnnuler.TraitementId = SelectedTraitementId
        traitementaAnnuler.PatientId = SelectedPatient.PatientId
        traitementaAnnuler.UserModification = userLog.UtilisateurId

        codeRetour = traitementDao.SuppressionTraitement(traitementaAnnuler, TraitementHistoACreer, userLog)
        If codeRetour = True Then
            Dim form As New RadFNotification()
            form.Message = "Traitement patient supprimé"
            form.Show()
        End If

        Cursor.Current = Cursors.Default
        Return codeRetour
    End Function


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones de l'écran ============
    '=============================================================================================

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

        BaseSelection = traitementDao.GetBaseCodeByItem(CbxTraitement.Text)

        If NumRythmeMatin.Value <> 0 Then
            Rythme = NumRythmeMatin.Value
        Else
            Rythme = 0
        End If

        PosologieMatin = NumRythmeMatin.Value
        PosologieMidi = NumRythmeMidi.Value
        PosologieApresMidi = NumRythmeApresMidi.Value
        PosologieSoir = NumRythmeSoir.Value

        LblTraitementPosologie.Text = FormatagePosologie(Rythme, BaseSelection, PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir)
    End Sub

    Private Sub InitZone()
        CbxTraitement.Items.Clear()
        CbxTraitement.Items.Add(Traitement.EnumBaseItem.JOURNALIER)
        CbxTraitement.Items.Add(Traitement.EnumBaseItem.HEBDOMADAIRE)
        CbxTraitement.Items.Add(Traitement.EnumBaseItem.MENSUEL)
        CbxTraitement.Items.Add(Traitement.EnumBaseItem.ANNUEL)
        CbxTraitement.Items.Add(Traitement.EnumBaseItem.CONDITIONNEL)

        CbxFractionMatin.Items.Clear()
        CbxFractionMatin.Items.Add(Traitement.EnumFraction.Non)
        CbxFractionMatin.Items.Add(Traitement.EnumFraction.Quart)
        CbxFractionMatin.Items.Add(Traitement.EnumFraction.Demi)
        CbxFractionMatin.Items.Add(Traitement.EnumFraction.TroisQuart)

        CbxFractionMidi.Items.Clear()
        CbxFractionMidi.Items.Add(Traitement.EnumFraction.Non)
        CbxFractionMidi.Items.Add(Traitement.EnumFraction.Quart)
        CbxFractionMidi.Items.Add(Traitement.EnumFraction.Demi)
        CbxFractionMidi.Items.Add(Traitement.EnumFraction.TroisQuart)

        CbxFractionApresMidi.Items.Clear()
        CbxFractionApresMidi.Items.Add(Traitement.EnumFraction.Non)
        CbxFractionApresMidi.Items.Add(Traitement.EnumFraction.Quart)
        CbxFractionApresMidi.Items.Add(Traitement.EnumFraction.Demi)
        CbxFractionApresMidi.Items.Add(Traitement.EnumFraction.TroisQuart)

        CbxFractionSoir.Items.Clear()
        CbxFractionSoir.Items.Add(Traitement.EnumFraction.Non)
        CbxFractionSoir.Items.Add(Traitement.EnumFraction.Quart)
        CbxFractionSoir.Items.Add(Traitement.EnumFraction.Demi)
        CbxFractionSoir.Items.Add(Traitement.EnumFraction.TroisQuart)

        ActionEnCours = EnumAction.Sans
        Me.CodeRetour = False
        LblMedicamentDCI.Text = ""
        LblTraitementMedicamentId.Text = ""
        LblMedicamentDenominationLongue.Text = ""
        LblMedicamentAdministration.Text = ""
        LblMedicamentTitulaire.Text = ""
        LblTraitementPosologie.Text = ""
        LblTraitementDateCreation.Text = ""
        LblTraitementDateModification.Text = ""
        LblFenetreTherapeutique.Hide()
        LblFenetreTherapeutiqueAu.Hide()
        LblFenetreDateFin.Hide()
        LblFenetreDateDebut.Hide()
        RadGbxFenetreTherapeutique.Hide()
        CacherZonesAnnulation()
        CacherZonesArret()
    End Sub

    Private Sub DroitAcces()
        'Si l'utilisateur n'a pas les droits requis ou que le traitement a été arrêté, les zones de saisie ne sont pas modifiables 
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Me.Text = "Visualisation détail traitement patient"
            LblstatutTraitement.Text = "Visualisation détail traitement patient"
            LblstatutTraitement.Hide()
            RadPnlStatutTraitement.Hide()
            InhiberZonesDeSaisie()
        End If
    End Sub

    Private Sub AfficherZonesArret()
        TxtCommentaireArret.Visible = True
        ChkAllergie.Visible = True
        ChkContreIndication.Visible = True
        GbxArretTraitement.Visible = True
        'BtnValidationArret.Visible = True
        'Initialisation à la date du jour de la date de fin correspondant à l'arrêt, elle peut-être modifiée à la date j-1
        DteTraitementDateFin.Value = Date.Now()
        CacherZonesAnnulation()
    End Sub

    Private Sub CacherZonesArret()
        TxtCommentaireArret.Visible = False
        ChkAllergie.Visible = False
        ChkContreIndication.Visible = False
        GbxArretTraitement.Visible = False
        'BtnValidationArret.Visible = False
    End Sub

    Private Sub InhiberZonesDeSaisieArret()
        TxtCommentaireArret.Enabled = False
        ChkAllergie.Enabled = False
        ChkContreIndication.Enabled = False
        'BtnValidationArret.Visible = False
    End Sub

    Private Sub AfficherZonesAnnulation()
        TxtCommentaireAnnulation.Visible = True
        GbxAnnulationTraitement.Visible = True
        'BtnValidationAnnulation.Visible = True
        CacherZonesArret()
    End Sub

    Private Sub CacherZonesAnnulation()
        TxtCommentaireAnnulation.Visible = False
        GbxAnnulationTraitement.Visible = False
    End Sub

    Private Sub InhiberZonesDeSaisieAnnulation()
        TxtCommentaireAnnulation.Enabled = False
    End Sub

    Private Sub InhiberZonesDeSaisie()
        RadBtnAnnulerTraitement.Visible = False
        RadBtnArretTraitement.Visible = False
        RadBtnSupprimerTraitement.Visible = False
        RadBtnValidation.Visible = False
        CbxTraitement.Enabled = False
        NumRythmeMatin.Enabled = False
        NumNumeroOrdre.Enabled = False
        NumRythmeMatin.Enabled = False
        NumRythmeMidi.Enabled = False
        NumRythmeApresMidi.Enabled = False
        NumRythmeSoir.Enabled = False
        CbxFractionMatin.Enabled = False
        CbxFractionMidi.Enabled = False
        CbxFractionApresMidi.Enabled = False
        CbxFractionSoir.Enabled = False
        DteTraitementDateDebut.Enabled = False
        DteTraitementDateFin.Enabled = False
        TxtTraitementCommentaire.ReadOnly = True
        TxtTraitementPosologieCommentaire.ReadOnly = True
        'Fixe le focus sur le bouton "Abandonner"
        RadBtnRetour.Select()
    End Sub

    Private Sub CacherPosologieJournaliere()
        LblRythmeMatin.Hide()
        NumRythmeMidi.Hide()
        LblRythmeMidi.Hide()
        CbxFractionMidi.Hide()
        NumRythmeApresMidi.Hide()
        LblRythmeApresMidi.Hide()
        CbxFractionApresMidi.Hide()
        NumRythmeSoir.Hide()
        LblRythmeSoir.Hide()
        CbxFractionSoir.Hide()
    End Sub

    Private Sub MontrerPosologieJournaliere()
        LblRythmeMatin.Show()
        NumRythmeMidi.Show()
        LblRythmeMidi.Show()
        NumRythmeMidi.Value = 0
        CbxFractionMidi.Show()
        CbxFractionMidi.Text = Traitement.EnumFraction.Non
        NumRythmeApresMidi.Show()
        LblRythmeApresMidi.Show()
        NumRythmeApresMidi.Value = 0
        CbxFractionApresMidi.Show()
        CbxFractionApresMidi.Text = Traitement.EnumFraction.Non
        NumRythmeSoir.Show()
        LblRythmeSoir.Show()
        NumRythmeSoir.Value = 0
        CbxFractionSoir.Show()
        CbxFractionSoir.Text = Traitement.EnumFraction.Non
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
    Private Sub CbxTraitement_TextChanged(sender As Object, e As EventArgs) Handles CbxTraitement.TextChanged
        'Gestion l'affichage des zones de saisie de la posologie journalière
        If CbxTraitement.Text = Traitement.EnumBaseItem.JOURNALIER Then
            MontrerPosologieJournaliere()
        Else
            CacherPosologieJournaliere()
        End If
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

    Private Sub CbxFractionMatin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxFractionMatin.SelectedIndexChanged
        AppelFormatagePosologie()
    End Sub

    Private Sub CbxFractionMidi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxFractionMidi.SelectedIndexChanged
        AppelFormatagePosologie()
    End Sub

    Private Sub CbxFractionApresMidi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxFractionApresMidi.SelectedIndexChanged
        AppelFormatagePosologie()
    End Sub

    Private Sub CbxFractionSoir_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxFractionSoir.SelectedIndexChanged
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

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        'Traitement : afficher les allergies dans un popup
        Cursor.Current = Cursors.WaitCursor
        If Allergie = True Then
            Me.Enabled = False
            Using vFPatientAllergieListe As New RadFPatientAllergieListe
                vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
                vFPatientAllergieListe.ShowDialog()
            End Using
            GetAllergie()
            Me.Enabled = True
        End If
    End Sub

    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles lblContreIndication.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.ShowDialog()
        End Using
        GetContreIndication()
        Me.Enabled = True
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTip.SetToolTip(LblId, "Id : " + SelectedTraitementId.ToString)
    End Sub

    Private Sub FTraitementDetailEdit_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        conxn.Dispose()
    End Sub

    '=============================================================================================
    '==================================== Traitement divers ======================================
    '=============================================================================================

    'Détermination de la base traitement à partir du combo box
    Private Function DeterminationBaseTraitement() As Char
        Dim baseTraitement As Char
        baseTraitement = traitementDao.GetBaseCodeByItem(CbxTraitement.Text)

        Return baseTraitement
    End Function

    'Traitement du formatage de la posologie
    Private Function FormatagePosologie(Rythme As Integer, BaseSelection As Char, PosologieMatin As Integer, PosologieMidi As Integer, PosologieApresMidi As Integer, PosologieSoir As Integer) As String
        Dim Base, Posologie As String
        Dim PosologieMatinString, PosologieMidiString, PosologieApresMidiString, PosologieSoirString As String
        Posologie = ""

        If CbxFractionMatin.Text <> "" AndAlso CbxFractionMatin.Text <> Traitement.EnumFraction.Non Then
            If PosologieMatin <> 0 Then
                PosologieMatinString = PosologieMatin.ToString & "+" & CbxFractionMatin.Text
            Else
                PosologieMatinString = CbxFractionMatin.Text
            End If
        Else
            If PosologieMatin <> 0 Then
                PosologieMatinString = PosologieMatin.ToString
            Else
                PosologieMatinString = "0"
            End If
        End If

        If CbxFractionMidi.Text <> "" AndAlso CbxFractionMidi.Text <> Traitement.EnumFraction.Non Then
            If PosologieMidi <> 0 Then
                PosologieMidiString = PosologieMidi.ToString & "+" & CbxFractionMidi.Text
            Else
                PosologieMidiString = CbxFractionMidi.Text
            End If
        Else
            If PosologieMidi <> 0 Then
                PosologieMidiString = PosologieMidi.ToString
            Else
                PosologieMidiString = "0"
            End If
        End If

        PosologieApresMidiString = ""
        If CbxFractionApresMidi.Text <> "" AndAlso CbxFractionApresMidi.Text <> Traitement.EnumFraction.Non Then
            If PosologieApresMidi <> 0 Then
                PosologieApresMidiString = PosologieApresMidi.ToString & "+" & CbxFractionApresMidi.Text
            Else
                PosologieApresMidiString = CbxFractionApresMidi.Text
            End If
        Else
            If PosologieApresMidi <> 0 Then
                PosologieApresMidiString = PosologieApresMidi.ToString
            End If
        End If

        If CbxFractionSoir.Text <> "" AndAlso CbxFractionSoir.Text <> Traitement.EnumFraction.Non Then
            If PosologieSoir <> 0 Then
                PosologieSoirString = PosologieSoir.ToString & "+" & CbxFractionSoir.Text
            Else
                PosologieSoirString = CbxFractionSoir.Text
            End If
        Else
            If PosologieSoir <> 0 Then
                PosologieSoirString = PosologieSoir.ToString
            Else
                PosologieSoirString = "0"
            End If
        End If

        Select Case BaseSelection
            Case Traitement.EnumBaseCode.JOURNALIER
                Base = ""
                If PosologieMatin <> 0 OrElse
                    PosologieMidi <> 0 OrElse
                    PosologieApresMidi <> 0 OrElse
                    PosologieSoir <> 0 OrElse
                    CbxFractionMatin.Text <> "" OrElse
                    CbxFractionMidi.Text <> "" OrElse
                    CbxFractionApresMidi.Text <> "" OrElse
                    CbxFractionSoir.Text <> "" Then
                    If PosologieApresMidi <> 0 OrElse CbxFractionApresMidi.Text <> Traitement.EnumFraction.Non Then
                        Posologie = Base + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieApresMidiString + ". " + PosologieSoirString
                    Else
                        Posologie = Base + " " + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieSoirString
                    End If
                End If
            Case Else
                If Rythme <> 0 Or CbxFractionMatin.Text <> "" Then
                    Dim RythmeString As String = ""
                    If CbxFractionMatin.Text <> "" AndAlso CbxFractionMatin.Text <> Traitement.EnumFraction.Non Then
                        If Rythme <> 0 Then
                            RythmeString = Rythme.ToString & "+" & CbxFractionMatin.Text
                        Else
                            RythmeString = CbxFractionMatin.Text
                        End If
                    Else
                        If Rythme <> 0 Then
                            RythmeString = Rythme.ToString
                        End If
                    End If

                    Base = traitementDao.GetBaseDescription(BaseSelection)
                    Posologie = Base + RythmeString
                End If
        End Select

        Return Posologie
    End Function

    Private Sub RadBtnRetour_Click(sender As Object, e As EventArgs) Handles RadBtnRetour.Click
        Close()
    End Sub

    Private Sub RadBtnPharmacocinetique_Click(sender As Object, e As EventArgs) Handles RadBtnPharmacocinetique.Click
        Dim PharmacoCinetique As String = TheriaqueDao.GetPharmacoCinetiqueBySpecialite(SelectedMedicamentId)
        Me.Enabled = False
        Using form As New RadFAffichaeInfo
            form.InfoToDisplay = PharmacoCinetique
            form.Titre = "Information Pharmacocinétique"
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnParmacodynamique_Click(sender As Object, e As EventArgs) Handles RadBtnParmacodynamique.Click
        Dim PharmacoCinetique As String = TheriaqueDao.GetPharmacoDynamiqueBySpecialite(SelectedMedicamentId)
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
            form.MedicamentId = SelectedMedicamentId
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnSubstance_Click(sender As Object, e As EventArgs) Handles RadBtnSubstance.Click
        Me.Enabled = False
        Using form As New RadFSubstancesListe
            form.SelectedSpecialite = SelectedMedicamentId
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnHistorique_Click(sender As Object, e As EventArgs) Handles RadBtnHistorique.Click
        Me.Enabled = False
        Try
            Using vFTraitementHistoListe As New RadFTraitementHistoListe
                vFTraitementHistoListe.SelectedTraitementId = SelectedTraitementId
                vFTraitementHistoListe.SelectedPatient = Me.SelectedPatient
                vFTraitementHistoListe.UtilisateurConnecte = userLog
                vFTraitementHistoListe.MedicamentDenomination = LblMedicamentDCI.Text
                vFTraitementHistoListe.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Me.Enabled = True
    End Sub

    Private Sub RadBtnModifierMedicament_Click(sender As Object, e As EventArgs) Handles RadBtnModifierMedicament.Click
        Using form As New RadFMedicamentSelecteur
            form.SelectedPatient = SelectedPatient
            form.SelectedClasseAtc = classeAtc
            form.ShowDialog() 'Modal
            medicament_selecteur_cis = form.SelectedSpecialiteId
            'Si un médicament a été sélectionné
            If medicament_selecteur_cis <> 0 Then
                SelectedMedicamentId = medicament_selecteur_cis
                LblTraitementMedicamentId.Text = medicament_selecteur_cis
                ChargementMedoc()
            End If
        End Using
    End Sub
End Class