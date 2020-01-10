Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Telerik.WinControls.UI.Localization

Public Class RadFTraitementDetailEdit
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedMedicamentCis As Integer
    Private privateSelectedTraitementId As Integer
    Private privateAllergie As Boolean
    Private privateContreIndication As Boolean
    Private privateCodeRetour As Boolean
    Private _positionGaucheDroite As Integer

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

    'Déclaration des variables de travail
    Dim EditMode As Char = ""
    Dim ActionEnCours As Integer
    Dim conxn As New SqlConnection(getConnectionString())
    Dim SQLString As String
    Dim medicament_selecteur_cis As Integer
    Private utilisateurCreation As Integer
    Private dateCreationTraitement As Date
    Private traitementArrete As Boolean = False
    Dim TraitementHistoACreer As New TraitementHisto
    Dim UtilisateurHisto As Utilisateur = New Utilisateur()

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

        'Allergie
        If Allergie = True Then
            LblAllergie.Show()
        Else
            LblAllergie.Hide()
        End If

        'Contre-indication
        If ContreIndication = True Then
            lblContreIndication.Show()
        Else
            lblContreIndication.Hide()
        End If

        'Si allergie, affichage des substances allergiques
        If Allergie = True Then
            LblAllergie.Visible = True
            Dim premierPassage As Boolean = True
            Dim LongueurChaine, LongueurSub As Integer
            Dim AllergieTooltip As String
            Dim LongueurMax As Integer = 10

            'Chargement du TextBox
            Dim allergieString As String
            Dim SubstancesAllergiques As New StringCollection()
            SubstancesAllergiques = MedocDao.ListeSubstancesAllergiques(SelectedPatient.PatientAllergieCis)
            Dim allergieEnumerator As StringEnumerator = SubstancesAllergiques.GetEnumerator()
            While allergieEnumerator.MoveNext()
                If premierPassage = True Then
                    allergieString = allergieEnumerator.Current.ToString
                    LongueurChaine = allergieString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    AllergieTooltip = allergieString
                    premierPassage = False
                Else
                    allergieString = allergieEnumerator.Current.ToString
                    LongueurChaine = allergieString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    AllergieTooltip = AllergieTooltip + vbCrLf + allergieString
                End If
            End While
            ToolTip.SetToolTip(LblAllergie, AllergieTooltip)
            'Chargement des médicaments génériques associés aux médicaments allergiques déclarés
            TraitementAllergies(Me.SelectedPatient)
        End If

        If ContreIndication = True Then
            lblContreIndication.Show()
            'Chargement des médicaments génériques associés aux médicaments contre-indiqués déclarés
            Dim premierPassage As Boolean = True
            Dim LongueurChaine, LongueurSub As Integer
            Dim CITooltip As String
            Dim LongueurMax As Integer = 10

            'Chargement du TextBox
            Dim CIString As String
            Dim SubstancesCI As New StringCollection()
            SubstancesCI = MedocDao.ListeSubstancesCI(SelectedPatient.PatientContreIndicationCis)
            Dim CIEnumerator As StringEnumerator = SubstancesCI.GetEnumerator()
            While CIEnumerator.MoveNext()
                If premierPassage = True Then
                    CIString = CIEnumerator.Current.ToString
                    LongueurChaine = CIString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    CITooltip = CIString
                    premierPassage = False
                Else
                    CIString = CIEnumerator.Current.ToString
                    LongueurChaine = CIString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    CITooltip = CITooltip + vbCrLf + CIString
                End If
            End While
            ToolTip.SetToolTip(lblContreIndication, CITooltip)
            'Chargement des médicaments génériques associés aux médicaments allergiques déclarés
            'TraitementAllergies(Me.SelectedPatient)
        End If

        'Chargement de l'écran
        If SelectedTraitementId = 0 Then
            EditMode = "C" 'Création
            afficheTitleForm(Me, "Création traitement patient")
            LblstatutTraitement.Hide()
            RadPnlStatutTraitement.Hide()
            'Récupération du cis du médicament sélectionné
            medicament_selecteur_cis = SelectedMedicamentCis
            'Chargement des données du médicament sélectionné
            ChargementMedoc()
            'Cacher la posologie journalière car la base n'a pas encore été saisie
            CacherPosologieJournaliere()
            CbxTraitementBase.Text = TraitementDao.EnumBaseItem.JOURNALIER
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
            ActionEnCours = EnumAction.Creation
        Else
            EditMode = "M" 'Modification
            afficheTitleForm(Me, "Modification traitement patient")
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
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub ChargementTraitementExistant()
        Dim traitementDao As TraitementDao = New TraitementDao
        Dim traitement As Traitement

        Try
            traitement = traitementDao.getTraitementById(SelectedTraitementId)
        Catch ex As Exception
            MessageBox.Show("Traitement : " + ex.Message, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try


        'Stockage de l'utilisateur qui a créé le traitement pour le contrôle en cas de demande de suppression du traitement
        utilisateurCreation = traitement.UserCreation
        medicament_selecteur_cis = traitement.MedicamentCis
        LblTraitementMedicamentCIS.Text = traitement.MedicamentCis
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
            'traitementArrete = True
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
            Select Case BaseSelection
                Case TraitementDao.EnumBaseCode.JOURNALIER, TraitementDao.EnumBaseCode.CONDITIONNEL
                    Select Case BaseSelection
                        Case TraitementDao.EnumBaseCode.JOURNALIER
                            Base = TraitementDao.EnumBaseItem.JOURNALIER & " : "
                            CbxTraitementBase.Text = TraitementDao.EnumBaseItem.JOURNALIER
                        Case TraitementDao.EnumBaseCode.CONDITIONNEL
                            Base = TraitementDao.EnumBaseItem.CONDITIONNEL & " : "
                            CbxTraitementBase.Text = TraitementDao.EnumBaseItem.CONDITIONNEL
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
                    If traitement.PosologieApresMidi <> 0 Then
                        posologieApresMidi = traitement.PosologieApresMidi
                        NumRythmeApresMidi.Value = traitement.PosologieApresMidi
                    Else
                        posologieApresMidi = 0
                        NumRythmeApresMidi.Value = 0
                    End If
                    If traitement.PosologieSoir <> 0 Then
                        posologieSoir = traitement.PosologieSoir
                        NumRythmeSoir.Value = traitement.PosologieSoir
                    Else
                        posologieSoir = 0
                        NumRythmeSoir.Value = 0
                    End If
                Case TraitementDao.EnumBaseCode.HEBDOMADAIRE
                    Base = TraitementDao.EnumBaseItem.HEBDOMADAIRE & " : "
                    CbxTraitementBase.Text = TraitementDao.EnumBaseItem.HEBDOMADAIRE
                    CacherPosologieJournaliere()
                Case TraitementDao.EnumBaseCode.MENSUEL
                    Base = TraitementDao.EnumBaseItem.MENSUEL & " : "
                    CbxTraitementBase.Text = TraitementDao.EnumBaseItem.MENSUEL
                    CacherPosologieJournaliere()
                Case TraitementDao.EnumBaseCode.ANNUEL
                    Base = TraitementDao.EnumBaseItem.ANNUEL & " : "
                    CbxTraitementBase.Text = TraitementDao.EnumBaseItem.ANNUEL
                    CacherPosologieJournaliere()
                Case Else
                    Base = "Base inconnue ! "
                    CbxTraitementBase.Text = ""
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
                    LblTraitementDuree.Text = CalculDureeTraitement(dateDebut, dateFin)
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
                SetUtilisateur(UtilisateurHisto, traitement.UserCreation)
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
            SetUtilisateur(UtilisateurHisto, traitement.UserModification)
            LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        'Initialisation classe Historisation traitement 
        InitClasseTraitementHistorisation(traitement, UtilisateurConnecte, TraitementHistoACreer)
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

    'Affichage de l'écran détaillant un médicament
    Private Sub RadBtnMedoc_Click(sender As Object, e As EventArgs) Handles RadBtnMedoc.Click
        Using vFMedocDetail As New RadFMedocDetail
            vFMedocDetail.MedicamentCis = LblTraitementMedicamentCIS.Text
            vFMedocDetail.ShowDialog() 'Modal
        End Using
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
        If CbxTraitementBase.Text = "" Then
            Valide = False
            LblTraitementBase.ForeColor = Color.Red
            messageErreur1 = "- La saisie de la Base de la posologie est obligatoire"
        Else
            Select Case CbxTraitementBase.Text
                Case TraitementDao.EnumBaseItem.JOURNALIER, TraitementDao.EnumBaseItem.HEBDOMADAIRE, TraitementDao.EnumBaseItem.MENSUEL, TraitementDao.EnumBaseItem.ANNUEL, TraitementDao.EnumBaseItem.CONDITIONNEL
                    LblTraitementBase.ForeColor = Color.Black
                Case Else
                    LblTraitementBase.ForeColor = Color.Red
                    Valide = False
                    messageErreur1 = "- La saisie de la Base de la posologie est obligatoire"
            End Select
        End If

        'Si rythme journalier, une posologie matin, midi, après-midi ou soir est requise
        If CbxTraitementBase.Text = TraitementDao.EnumBaseItem.JOURNALIER OrElse CbxTraitementBase.Text = TraitementDao.EnumBaseItem.CONDITIONNEL Then
            If NumRythmeMatin.Value = 0 AndAlso NumRythmeMidi.Value = 0 AndAlso NumRythmeApresMidi.Value = 0 AndAlso NumRythmeSoir.Value = 0 Then
                LblTraitementRythme.ForeColor = Color.Red
                Valide = False
                messageErreur2 = "- La saisie des périodes d'application (matin, midi, après-mid ou soir) de la posologie journalière est obligatoire"
            Else
                LblTraitementRythme.ForeColor = Color.Black
            End If
        Else
            'Rythme obligatoire
            If NumRythmeMatin.Value = 0 Then
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
        If CbxTraitementBase.Text = TraitementDao.EnumBaseItem.CONDITIONNEL Then
            If TxtTraitementPosologieCommentaire.Text = "" Then
                Valide = False
                messageErreur5 = "- Le commentaire de la posologie est obligatoire quand la base est : '" & TraitementDao.EnumBaseItem.CONDITIONNEL & "'"
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

            MessageBox.Show(messageErreur)
        End If

        'Contrôler qu'une données a au moins été modifiée
        'TODO: contrôle qu'une données a au moins été modifiée

        Return Valide
    End Function

    'Traitement de l'arrêt du traitement
    Private Sub TraitementArret()
        'TODO: Contrôle date de fin
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
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Cursor.Current = Cursors.WaitCursor

        'Définition de la base du traitement
        Dim baseTraitement As Char = traitementDao.GetBaseCodeByItem(CbxTraitementBase.Text)

        'Définition posologie journalière
        Dim PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir As Integer
        Dim Rythme As Integer

        If NumRythmeMatin.Value <> 0 Then
            Rythme = NumRythmeMatin.Value
        Else
            Rythme = 0
        End If

        If baseTraitement = TraitementDao.EnumBaseCode.JOURNALIER OrElse baseTraitement = TraitementDao.EnumBaseCode.CONDITIONNEL Then
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
            MessageBox.Show("Traitement patient créé")
        Catch ex As Exception
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

            Dim traitementDao As TraitementDao = New TraitementDao
            Dim traitementCree As Traitement

            Try
                traitementCree = traitementDao.getTraitementById(TraitementHistoACreer.HistorisationTraitementId)
            Catch ex As Exception
                MessageBox.Show("Traitement : " + ex.Message, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
            InitClasseTraitementHistorisation(traitementCree, UtilisateurConnecte, TraitementHistoACreer)


            'Création dans l'historique des traitements du traitement créé
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.CreationTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Cursor.Current = Cursors.Default

        Return codeRetour

    End Function

    'Modification d'un traitement en base de données
    Private Function ModificationTraitement() As Boolean

        Cursor.Current = Cursors.WaitCursor

        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        'Définition de la base du traitement
        Dim baseTraitement As Char = traitementDao.GetBaseCodeByItem(CbxTraitementBase.Text)

        'Définition posologie journalière
        Dim PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir As Integer
        Dim Rythme As Integer

        If NumRythmeMatin.Value <> 0 Then
            Rythme = NumRythmeMatin.Value
        Else
            Rythme = 0
        End If

        If baseTraitement = TraitementDao.EnumBaseCode.JOURNALIER OrElse baseTraitement = TraitementDao.EnumBaseCode.CONDITIONNEL Then
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

            MessageBox.Show("Traitement patient modifié")
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

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Cursor.Current = Cursors.Default

        Return codeRetour
    End Function

    'Arrêt traitement en base de données
    Private Function ArretTraitement() As Boolean
        Cursor.Current = Cursors.WaitCursor

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

            MessageBox.Show("Traitement patient arrêté")
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
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.ArretTraitement
            TraitementHistoACreer.HistorisationDateFin = DteTraitementDateFin.Value
            TraitementHistoACreer.HistorisationArretCommentaire = TxtCommentaireArret.Text
            TraitementHistoACreer.HistorisationAllergie = allergie
            TraitementHistoACreer.HistorisationContreIndication = contreIndication
            TraitementHistoACreer.HistorisationArret = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.ArretTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Cursor.Current = Cursors.Default

        Return codeRetour
    End Function

    'Annulation d'un traitement en base de données
    Private Function AnnulationTraitement() As Boolean
        Cursor.Current = Cursors.WaitCursor

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

            MessageBox.Show("Traitement patient annulé")
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
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.AnnulationTraitement
            TraitementHistoACreer.HistorisationAnnulationCommentaire = TxtCommentaireAnnulation.Text
            TraitementHistoACreer.HistorisationAnnulation = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.AnnulationTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Cursor.Current = Cursors.Default

        Return codeRetour
    End Function

    'Suppression traitement
    Private Function SuppressionTraitement() As Boolean

        Cursor.Current = Cursors.WaitCursor

        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "delete from oasis.oa_traitement where oa_traitement_id = @traitementId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@traitementId", SelectedTraitementId.ToString)
        End With

        Try
            conxn.Open()
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
            MessageBox.Show("Traitement patient supprimé")
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
            TraitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.AnnulationTraitement
            TraitementHistoACreer.HistorisationAnnulationCommentaire = TxtCommentaireArret.Text
            TraitementHistoACreer.HistorisationAnnulation = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(TraitementHistoACreer, UtilisateurConnecte, EnumEtatTraitementHisto.SuppressionTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
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
                LblTraitementDuree.Text = CalculDureeTraitement(DateDebut, DateFin)
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

        BaseSelection = traitementDao.GetBaseCodeByItem(CbxTraitementBase.Text)

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
        CbxTraitementBase.Items.Clear()
        CbxTraitementBase.Items.Add(TraitementDao.EnumBaseItem.JOURNALIER)
        CbxTraitementBase.Items.Add(TraitementDao.EnumBaseItem.HEBDOMADAIRE)
        CbxTraitementBase.Items.Add(TraitementDao.EnumBaseItem.MENSUEL)
        CbxTraitementBase.Items.Add(TraitementDao.EnumBaseItem.ANNUEL)
        CbxTraitementBase.Items.Add(TraitementDao.EnumBaseItem.CONDITIONNEL)

        ActionEnCours = EnumAction.Sans
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
        RadGbxFenetreTherapeutique.Hide()
        CacherZonesAnnulation()
        CacherZonesArret()
    End Sub

    Private Sub DroitAcces()
        'Si l'utilisateur n'a pas les droits requis ou que le traitement a été arrêté, les zones de saisie ne sont pas modifiables 
        If UtilisateurConnecte.UtilisateurNiveauAcces <> 1 Then
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
        'BtnValidationAnnulation.Visible = False
    End Sub

    Private Sub InhiberZonesDeSaisieAnnulation()
        TxtCommentaireAnnulation.Enabled = False
        'BtnValidationAnnulation.Visible = False
    End Sub

    Private Sub InhiberZonesDeSaisie()
        RadBtnAnnulerTraitement.Visible = False
        RadBtnArretTraitement.Visible = False
        RadBtnSupprimerTraitement.Visible = False
        RadBtnValidation.Visible = False
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
        'Fixe le focus sur le bouton "Abandonner"
        RadBtnRetour.Select()
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
        If CbxTraitementBase.Text = TraitementDao.EnumBaseItem.JOURNALIER OrElse CbxTraitementBase.Text = TraitementDao.EnumBaseItem.CONDITIONNEL Then
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
                vFPatientAllergieListe.SelectedPatientId = Me.SelectedPatient.patientId
                vFPatientAllergieListe.SelectedPatientAllergieCis = Me.SelectedPatient.PatientAllergieCis
                vFPatientAllergieListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientAllergieListe.ShowDialog()
            End Using
            Me.Enabled = True
        End If
    End Sub

    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles lblContreIndication.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientContreIndicationListe.SelectedPatientCICis = Me.SelectedPatient.PatientContreIndicationCis
            vFPatientContreIndicationListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientContreIndicationListe.ShowDialog()
        End Using
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
        baseTraitement = traitementDao.GetBaseCodeByItem(CbxTraitementBase.Text)

        Return baseTraitement
    End Function

    'Traitement du formatage de la posologie
    Private Function FormatagePosologie(Rythme As Integer, BaseSelection As Char, PosologieMatin As Integer, PosologieMidi As Integer, PosologieApresMidi As Integer, PosologieSoir As Integer) As String
        Dim Base, Posologie As String
        Posologie = ""

        Select Case BaseSelection
            Case TraitementDao.EnumBaseCode.JOURNALIER
                Base = ""
                If PosologieMatin <> 0 OrElse PosologieMidi <> 0 OrElse PosologieApresMidi <> 0 OrElse PosologieSoir <> 0 Then
                    If PosologieApresMidi <> 0 Then
                        Posologie = Base + PosologieMatin.ToString + "." + PosologieMidi.ToString + "." + PosologieApresMidi.ToString + "." + PosologieSoir.ToString
                    Else
                        Posologie = Base + " " + PosologieMatin.ToString + "." + PosologieMidi.ToString + "." + PosologieSoir.ToString
                    End If
                End If
            Case TraitementDao.EnumBaseCode.CONDITIONNEL
                Base = "Conditionnel : "
                Dim sommePosologie = PosologieMatin + PosologieMidi + PosologieApresMidi + PosologieSoir
                If sommePosologie = PosologieMatin OrElse sommePosologie = PosologieMidi OrElse sommePosologie = PosologieApresMidi OrElse sommePosologie = PosologieSoir Then
                    'Un seul item de saisie
                    Posologie = Base + sommePosologie.ToString
                Else
                    If PosologieMatin <> 0 OrElse PosologieMidi <> 0 OrElse PosologieApresMidi <> 0 OrElse PosologieSoir <> 0 Then
                        If PosologieApresMidi <> 0 Then
                            Posologie = Base + PosologieMatin.ToString + "." + PosologieMidi.ToString + "." + PosologieApresMidi.ToString + "." + PosologieSoir.ToString
                        Else
                            Posologie = Base + " " + PosologieMatin.ToString + "." + PosologieMidi.ToString + "." + PosologieSoir.ToString
                        End If
                    End If
                End If
            Case Else
                If Rythme <> 0 Then
                    Select Case BaseSelection
                        Case TraitementDao.EnumBaseCode.HEBDOMADAIRE
                            Base = "Hebdo : "
                            Posologie = Base + Rythme.ToString
                        Case TraitementDao.EnumBaseCode.MENSUEL
                            Base = "Mensuel : "
                            Posologie = Base + Rythme.ToString
                        Case TraitementDao.EnumBaseCode.ANNUEL
                            Base = "Annuel : "
                            Posologie = Base + Rythme.ToString
                        Case Else
                            Base = "Base inconnue ! "
                            Posologie = Base + Rythme.ToString
                    End Select
                End If
        End Select

        Return Posologie
    End Function

End Class
