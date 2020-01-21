Imports System.Collections.Specialized
Imports Oasis_WF
Imports Telerik.WinControls.UI

Public Class RadFOrdonnanceListeDetail
    Private _SelectedPatient As Patient
    Private _SelectedEpisode As Episode
    Private _UtilisateurConnecte As Utilisateur
    Private _SelectedOrdonnanceId As Integer
    Private _commentaireOrdonnance As String
    Private _Allergie As Boolean
    Private _ContreIndication As Boolean
    Private _CodeRetour As Boolean

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

    Public Property SelectedOrdonnanceId As Integer
        Get
            Return _SelectedOrdonnanceId
        End Get
        Set(value As Integer)
            _SelectedOrdonnanceId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _CodeRetour
        End Get
        Set(value As Boolean)
            _CodeRetour = value
        End Set
    End Property

    Public Property Allergie As Boolean
        Get
            Return _Allergie
        End Get
        Set(value As Boolean)
            _Allergie = value
        End Set
    End Property

    Public Property ContreIndication As Boolean
        Get
            Return _ContreIndication
        End Get
        Set(value As Boolean)
            _ContreIndication = value
        End Set
    End Property

    Public Property CommentaireOrdonnance As String
        Get
            Return _commentaireOrdonnance
        End Get
        Set(value As String)
            _commentaireOrdonnance = value
        End Set
    End Property

    Public Property SelectedEpisode As Episode
        Get
            Return _SelectedEpisode
        End Get
        Set(value As Episode)
            _SelectedEpisode = value
        End Set
    End Property

    Dim aldDao As New AldDao
    Dim ordonnanceDao As New OrdonnanceDao
    Dim ordonnanceDetailDao As New OrdonnanceDetailDao

    Dim ordonnance As Ordonnance

    Dim CommentaireModified As Boolean = False
    Dim RenouvellementModified As Boolean = False
    Dim iGridALD As Integer
    Dim iGridNonALD As Integer
    Dim PatientALD As Boolean = False
    Dim TraitementALD As Boolean

    Private Sub RadFOrdonnanceDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Si patient Non ALD, on cache la partie concernant les traitements ALD
        If aldDao.IsPatientALD(Me.SelectedPatient.patientId) = False Then
            SplitPanel3.Hide()
            Me.RadSplitContainer1.MoveSplitter(Me.RadSplitContainer1.Splitters(2), RadDirection.Up)
            RadGbxTraitement.Text = "Prescription"
            BasculerEnALDToolStripMenuItem.Visible = False
        Else
            PatientALD = True
        End If

        ChargementEtatCivil()
        ChargementOrdonnance()
        ChargementOrdonnanceDetail()
    End Sub

    Private Sub ChargementOrdonnance()
        ordonnance = ordonnanceDao.getOrdonnaceById(SelectedOrdonnanceId)
        TxtCommentaire.Text = ordonnance.Commentaire
        NumRenouvellement.Value = ordonnance.Renouvellement
        If ordonnance.DateValidation = Nothing Then
            If userLog.TypeProfil <> FonctionDao.enumTypeFonction.MEDICAL.ToString Then
                RadBtnValidation.Enabled = False
            End If
            RadBtnImprimer.Enabled = False
        Else
            RadBtnValidation.Hide()
        End If
    End Sub

    Private Sub ChargementOrdonnanceDetail()
        RadAldGridView.Rows.Clear()
        RadNonAldGridView.Rows.Clear()
        iGridALD = -1
        iGridNonALD = -1

        Dim ordonnanceDataTable As DataTable
        Dim ordonnanceDaoDetail As OrdonnanceDetailDao = New OrdonnanceDetailDao
        ordonnanceDataTable = ordonnanceDaoDetail.getAllOrdonnanceLigneByOrdonnanceId(Me.SelectedOrdonnanceId)
        If ordonnanceDataTable.Rows.Count > 0 Then
            'RadBtnCreationLignes.Hide()
            RadBtnValidation.Show()
            RadBtnImprimer.Show()
        Else
            'RadBtnCreationLignes.Show()
            RadBtnValidation.Hide()
            RadBtnImprimer.Hide()
        End If

        Dim i As Integer
        Dim rowCount As Integer = ordonnanceDataTable.Rows.Count - 1
        Dim Base As String
        Dim Posologie As String
        Dim dateFin, dateDebut As Date
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim FenetreTherapeutiqueEnCours As Boolean
        Dim FenetreTherapeutiqueAVenir As Boolean

        'Dim Allergie As Boolean = False
        Dim FenetreDateDebut, FenetreDateFin As Date

        Allergie = False

        ContreIndication = False
        LblAllergie.Visible = False
        lblContreIndication.Visible = False
        SelectedPatient.PatientAllergieCis.Clear()
        SelectedPatient.PatientAllergieDci.Clear()
        SelectedPatient.PatientContreIndicationCis.Clear()
        SelectedPatient.PatientContreIndicationDci.Clear()
        SelectedPatient.PatientMedicamentsPrescritsCis.Clear()

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            Dim ordonnanceDetailGrid As New OrdonnanceDetailGrid
            'Date de fin
            If ordonnanceDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = ordonnanceDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If
            Dim DateFinCalcul As Date
            If dateFin > Date.Now.AddDays(29) Then
                DateFinCalcul = Date.Now.AddDays(29)
            Else
                DateFinCalcul = dateFin
            End If

            'Date début
            If ordonnanceDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                dateDebut = ordonnanceDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If
            Dim DateDebutCalcul As Date
            If dateDebut.Date < Date.Now.Date Then
                DateDebutCalcul = Date.Now
            Else
                DateDebutCalcul = dateDebut
            End If

            Dim duree As String = outils.CalculDureeTraitement(DateDebutCalcul, DateFinCalcul)

            'Exclusion de l'affichage des traitements dont la date de fin est <à la date du jour
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications dans la StringCollection quel que soit leur date de fin
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                'Continue For
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique active et à venir
            FenetreTherapeutiqueEnCours = False
            FenetreTherapeutiqueAVenir = False

            If ordonnanceDataTable.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                FenetreDateDebut = ordonnanceDataTable.Rows(i)("oa_traitement_fenetre_date_debut")
            Else
                FenetreDateDebut = "31/12/2999"
            End If


            If ordonnanceDataTable.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                FenetreDateFin = ordonnanceDataTable.Rows(i)("oa_traitement_fenetre_date_fin")
            Else
                FenetreDateFin = "01/01/1900"
            End If

            Posologie = ""

            'Existence d'une fenêtre thérapeutique
            Dim FenetreTherapeutiqueExiste As Boolean = False
            Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If ordonnanceDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If ordonnanceDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
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

                FractionMatin = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_fraction_matin"), TraitementDao.EnumFraction.Non)
                FractionMidi = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_fraction_midi"), TraitementDao.EnumFraction.Non)
                FractionApresMidi = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), TraitementDao.EnumFraction.Non)
                FractionSoir = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_fraction_soir"), TraitementDao.EnumFraction.Non)

                posologieMatin = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_Posologie_matin"), 0)
                posologieMidi = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_Posologie_midi"), 0)
                posologieApresMidi = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_Posologie_apres_midi"), 0)
                posologieSoir = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_Posologie_soir"), 0)

                PosologieBase = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_Posologie_base"), "")

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
                If ordonnanceDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = ordonnanceDataTable.Rows(i)("oa_traitement_posologie_rythme")
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
                            Select Case ordonnanceDataTable.Rows(i)("oa_traitement_posologie_base")
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

            ordonnanceDetailGrid.TraitementId = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_id"), 0)
            ordonnanceDetailGrid.OrdonnanceLigneId = ordonnanceDataTable.Rows(i)("oa_ordonnance_ligne_id")
            ordonnanceDetailGrid.MedicamentDci = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_medicament_dci"), "")
            ordonnanceDetailGrid.MedicamentCis = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_medicament_cis"), 0)
            ordonnanceDetailGrid.Posologie = Posologie
            ordonnanceDetailGrid.CommentairePosologie = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")
            ordonnanceDetailGrid.Duree = duree
            ordonnanceDetailGrid.ADelivrer = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_a_delivrer"), False)
            ordonnanceDetailGrid.Ald = Coalesce(ordonnanceDataTable.Rows(i)("oa_traitement_ald"), False)

            'Aiguillage ALD / Non ALD
            If PatientALD = True Then
                If ordonnanceDetailGrid.Ald = True Then
                    ChargementGridALD(ordonnanceDetailGrid)
                Else
                    ChargementGridNonALD(ordonnanceDetailGrid)
                End If
            Else
                ChargementGridNonALD(ordonnanceDetailGrid)
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadAldGridView.Rows.Count > 0 Then
            Me.RadAldGridView.CurrentRow = RadAldGridView.ChildRows(0)
        End If

        If RadNonAldGridView.Rows.Count > 0 Then
            Me.RadNonAldGridView.CurrentRow = RadNonAldGridView.ChildRows(0)
        End If
    End Sub

    Private Sub ChargementGridALD(ordonnanceDetailGrid As OrdonnanceDetailGrid)
        iGridALD += 1
        'Ajout d'une ligne au DataGridView
        RadAldGridView.Rows.Add(iGridALD)
        'Alimentation du DataGridView
        'DCI
        RadAldGridView.Rows(iGridALD).Cells("medicamentDci").Value = ordonnanceDetailGrid.MedicamentDci
        'Posologie
        RadAldGridView.Rows(iGridALD).Cells("posologie").Value = ordonnanceDetailGrid.Posologie

        'Fenêtre thérapeutique existe (en cours ou à venir ou obsolète)
        If ordonnanceDetailGrid.FenetreTherapeutique = True Then
            RadAldGridView.Rows(iGridALD).Cells("fenetreTherapeutique").Value = "O"
        Else
            RadAldGridView.Rows(iGridALD).Cells("fenetreTherapeutique").Value = ""
        End If

        'Identifiant du traitement
        RadAldGridView.Rows(iGridALD).Cells("traitementId").Value = ordonnanceDetailGrid.TraitementId
        RadAldGridView.Rows(iGridALD).Cells("ordonnanceLigneId").Value = ordonnanceDetailGrid.OrdonnanceLigneId

        'CIS du médicament
        RadAldGridView.Rows(iGridALD).Cells("medicamentCis").Value = ordonnanceDetailGrid.MedicamentCis

        RadAldGridView.Rows(iGridALD).Cells("duree").Value = ordonnanceDetailGrid.Duree
        RadAldGridView.Rows(iGridALD).Cells("commentairePosologie").Value = ordonnanceDetailGrid.CommentairePosologie

        If ordonnanceDetailGrid.ADelivrer = True Then
            RadAldGridView.Rows(iGridALD).Cells("delivrance").Value = OrdonnanceDetailDao.EnumDelivrance.A_DELIVRER
        Else
            RadAldGridView.Rows(iGridALD).Cells("delivrance").Value = OrdonnanceDetailDao.EnumDelivrance.NE_PAS_DELIVRER
        End If
    End Sub

    Private Sub ChargementGridNonALD(ordonnanceDetailGrid As OrdonnanceDetailGrid)
        iGridNonALD += 1
        'Ajout d'une ligne au DataGridView
        RadNonAldGridView.Rows.Add(iGridNonALD)
        'Alimentation du DataGridView
        'DCI
        RadNonAldGridView.Rows(iGridNonALD).Cells("medicamentDci").Value = ordonnanceDetailGrid.MedicamentDci
        'Posologie
        RadNonAldGridView.Rows(iGridNonALD).Cells("posologie").Value = ordonnanceDetailGrid.Posologie

        'Fenêtre thérapeutique existe (en cours ou à venir ou obsolète)
        If ordonnanceDetailGrid.FenetreTherapeutique = True Then
            RadNonAldGridView.Rows(iGridNonALD).Cells("fenetreTherapeutique").Value = "O"
        Else
            RadNonAldGridView.Rows(iGridNonALD).Cells("fenetreTherapeutique").Value = ""
        End If

        'Identifiant du traitement
        RadNonAldGridView.Rows(iGridNonALD).Cells("traitementId").Value = ordonnanceDetailGrid.TraitementId
        RadNonAldGridView.Rows(iGridNonALD).Cells("ordonnanceLigneId").Value = ordonnanceDetailGrid.OrdonnanceLigneId

        'CIS du médicament
        RadNonAldGridView.Rows(iGridNonALD).Cells("medicamentCis").Value = ordonnanceDetailGrid.MedicamentCis

        RadNonAldGridView.Rows(iGridNonALD).Cells("duree").Value = ordonnanceDetailGrid.Duree
        RadNonAldGridView.Rows(iGridNonALD).Cells("commentairePosologie").Value = ordonnanceDetailGrid.CommentairePosologie

        If ordonnanceDetailGrid.ADelivrer = True Then
            RadNonAldGridView.Rows(iGridNonALD).Cells("delivrance").Value = OrdonnanceDetailDao.EnumDelivrance.A_DELIVRER
        Else
            RadNonAldGridView.Rows(iGridNonALD).Cells("delivrance").Value = OrdonnanceDetailDao.EnumDelivrance.NE_PAS_DELIVRER
        End If
    End Sub

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
        If SelectedPatient.PharmacienId <> 0 Then
            Dim rordao As New RorDao
            Dim ror As Ror
            ror = rordao.getRorById(SelectedPatient.PharmacienId)
            LblPharmacienNom.Text = ror.Nom & " " & ror.Ville
        Else
            LblPharmacienNom.Text = "Pas de pharmacie référencée pour ce patient"
        End If

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If

        ChargementAllergie()
    End Sub

    Private Sub ChargementAllergie()
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
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        'Traitement : afficher les allergies dans un popup
        If Allergie = True Then
            Using vFPatientAllergieListe As New RadFPatientAllergieListe
                vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
                vFPatientAllergieListe.SelectedPatientId = Me.SelectedPatient.patientId
                vFPatientAllergieListe.SelectedPatientAllergieCis = Me.SelectedPatient.PatientAllergieCis
                vFPatientAllergieListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientAllergieListe.ShowDialog() 'Modal
            End Using
        End If
    End Sub

    Private Sub lblContreIndication_Click(sender As Object, e As EventArgs) Handles lblContreIndication.Click
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientContreIndicationListe.SelectedPatientCICis = Me.SelectedPatient.PatientContreIndicationCis
            vFPatientContreIndicationListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientContreIndicationListe.ShowDialog() 'Modal
        End Using
    End Sub

    Private Sub RadBtnCreationLignes_Click(sender As Object, e As EventArgs)
        Dim ordonnanceDao As New OrdonnanceDao
        ordonnanceDao.CreateNewOrdonnanceDetail(SelectedPatient.patientId, SelectedOrdonnanceId, SelectedEpisode)
        RadAldGridView.Rows.Clear()
        ChargementOrdonnanceDetail()
        RadBtnValidation.Show()
        RadBtnImprimer.Show()
        'RadBtnCreationLignes.Hide()
    End Sub


    'Modification d'une ligne d'ordonnance
    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadAldGridView.CellDoubleClick
        If RadAldGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAldGridView.Rows.IndexOf(Me.RadAldGridView.CurrentRow)
            If aRow >= 0 Then
                Dim OrdonnanceId As Integer = RadAldGridView.Rows(aRow).Cells("ordonnanceLigneId").Value
                'Tester si l'ordonnance sélectionnée est à valider

            End If
        Else
            MessageBox.Show("Veuillez sélectionner une ligne d'ordonnance")
        End If
    End Sub

    Private Sub TxtCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaire.TextChanged
        CommentaireModified = True
    End Sub

    Private Sub TxtCommentaire_Leave(sender As Object, e As EventArgs) Handles TxtCommentaire.Leave
        ModificationCommentaire()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumRenouvellement.ValueChanged
        RenouvellementModified = True
    End Sub

    Private Sub NumericUpDown1_Leave(sender As Object, e As EventArgs) Handles NumRenouvellement.Leave
        ModificationRenouvellement()
    End Sub

    Private Sub RadFOrdonnanceListeDetail_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ModificationCommentaire()
        ModificationRenouvellement()
    End Sub

    Private Sub ModificationCommentaire()
        If CommentaireModified = True Then
            'Appel mise à jour de l'ordonnance
            ordonnanceDao.ModificationOrdonnanceCommentaire(SelectedOrdonnanceId, TxtCommentaire.Text)
            CommentaireModified = False
        End If
    End Sub

    Private Sub ModificationRenouvellement()
        If RenouvellementModified = True Then
            'Appel mise à jour de l'ordonnance
            ordonnanceDao.ModificationOrdonnanceRenouvellement(SelectedOrdonnanceId, NumRenouvellement.Value)
            RenouvellementModified = False
        End If
    End Sub

    Private Sub RadBtnAnnulerOrdonnance_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulerOrdonnance.Click
        If ordonnanceDao.AnnulerOrdonnance(SelectedOrdonnanceId) = True Then
            Dim form As New RadFNotification()
            form.Message = "L'ordonnance a été annulée"
            form.Show()
            Close()
        End If
    End Sub

    '==========================================================================
    '====== Option Grid ALD
    '==========================================================================

    'Basculer en Non ALD
    Private Sub BasculerEnNonALDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BasculerEnNonALDToolStripMenuItem.Click
        If RadAldGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAldGridView.Rows.IndexOf(Me.RadAldGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ordonnanceLigneId As Integer = RadAldGridView.Rows(aRow).Cells("ordonnanceLigneId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                If ordonnanceDetailDao.ModificationOrdonnanceDetailALD(ordonnanceLigneId, False) = True Then
                    ChargementOrdonnanceDetail()
                End If
                Cursor.Current = Cursors.Default
                Me.Enabled = True
            End If
        End If
    End Sub

    'Création d'une ligne de commentaire en ALD
    Private Sub CréerUneLigneDeCommentaireToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneLigneDeCommentaireToolStripMenuItem.Click

    End Sub

    'Suppression d'une ligne de commentaire en ALD
    Private Sub SupprimerUneLigneToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupprimerUneLigneToolStripMenuItem.Click
        If RadAldGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAldGridView.Rows.IndexOf(Me.RadAldGridView.CurrentRow)
            If aRow >= 0 Then
                Dim OrdonnanceId As Integer = RadAldGridView.Rows(aRow).Cells("ordonnanceId").Value
                'Tester si l'ordonnance sélectionnée est à valider

            End If
        Else
            MessageBox.Show("Veuillez sélectionner une ligne d'ordonnance")
        End If
    End Sub

    'Bascule Délivrer / Ne pas délivrer
    Private Sub BasculerADélivrerANePasDélivrerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BasculerADélivrerANePasDélivrerToolStripMenuItem.Click
        If RadAldGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAldGridView.Rows.IndexOf(Me.RadAldGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ordonnanceLigneId As Integer = RadAldGridView.Rows(aRow).Cells("ordonnanceLigneId").Value
                Dim delivrance As String = RadAldGridView.Rows(aRow).Cells("delivrance").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Dim aDelivrer As Boolean
                If delivrance = OrdonnanceDetailDao.EnumDelivrance.A_DELIVRER Then
                    aDelivrer = False
                Else
                    aDelivrer = True
                End If
                If ordonnanceDetailDao.ModificationOrdonnanceDetailDelivrance(ordonnanceLigneId, aDelivrer) = True Then
                    ChargementOrdonnanceDetail()
                End If
                Cursor.Current = Cursors.Default
                Me.Enabled = True
            End If
        End If
    End Sub


    '==========================================================================
    '====== Option Grid Non ALD
    '==========================================================================
    Private Sub CréerUneLigneDeCommentaireToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CréerUneLigneDeCommentaireToolStripMenuItem1.Click

    End Sub

    Private Sub SupprimerUneLigneDeCommentaireToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupprimerUneLigneDeCommentaireToolStripMenuItem.Click

    End Sub

    Private Sub BasculerADélivrerANePasDélivrerToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles BasculerADélivrerANePasDélivrerToolStripMenuItem1.Click
        If RadNonAldGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadNonAldGridView.Rows.IndexOf(Me.RadNonAldGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ordonnanceLigneId As Integer = RadNonAldGridView.Rows(aRow).Cells("ordonnanceLigneId").Value
                Dim delivrance As String = RadNonAldGridView.Rows(aRow).Cells("delivrance").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Dim aDelivrer As Boolean
                If delivrance = OrdonnanceDetailDao.EnumDelivrance.A_DELIVRER Then
                    aDelivrer = False
                Else
                    aDelivrer = True
                End If
                If ordonnanceDetailDao.ModificationOrdonnanceDetailDelivrance(ordonnanceLigneId, aDelivrer) = True Then
                    ChargementOrdonnanceDetail()
                End If
                Cursor.Current = Cursors.Default
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub BasculerEnALDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BasculerEnALDToolStripMenuItem.Click
        If RadNonAldGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadNonAldGridView.Rows.IndexOf(Me.RadNonAldGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ordonnanceLigneId As Integer = RadNonAldGridView.Rows(aRow).Cells("ordonnanceLigneId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                If ordonnanceDetailDao.ModificationOrdonnanceDetailALD(ordonnanceLigneId, True) = True Then
                    ChargementOrdonnanceDetail()
                End If
                Cursor.Current = Cursors.Default
                Me.Enabled = True
            End If
        End If
    End Sub
End Class
