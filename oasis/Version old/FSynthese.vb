' Outil de synthèse

Imports System.Collections.Specialized
Imports System.Data.SqlClient
Public Class FSynthese
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

    Dim Allergie As Boolean
    Dim ContreIndication As Boolean
    Dim AllergieText, ContreIndicationText As String
    Dim InitPublie, InitParPriorite, InitContextePublie, InitContexteBioPublie As Boolean
    Dim PPSSuiviIdeExiste, PPSSuiviSageFemmeExiste, PPSSuiviMedecinExiste As Boolean

    Private Sub Synthese_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initZones()
        GestionDroitsAcces()
        ChargementEtatCivil()
        'provoque le chargement des antécédents
        InitPublie = False
        InitParPriorite = False
        ChkPublie.Checked = True
        ChkParPriorite.Checked = True
        InitContextePublie = False
        ChkContextePublie.Checked = True
        InitContexteBioPublie = False
        ChargementAntecedent()
        ChargementTraitement()
        ChargementParcoursDeSoin()
        ChargementContexte()
        ChargementPPS()

    End Sub

    '==========================================================
    '======================= Etat civil =======================
    '==========================================================

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
        'Recherche si ALD
        LblALD.Hide()

        'Vérification de l'existence d'ALD
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If

        'If outils.ControleExistenceALD(Me.SelectedPatient.patientId) = True Then
        'LblALD.Show()
        'End If
    End Sub

    'Affichage de l'adresse du patient dans Google Maps
    Private Sub ToolStripMenuItemMaps_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemMaps.Click
        'Etat civil : lancer l'URL pour afficher l'adresse dans Google Maps
        Dim MonURL As String
        MonURL = "http://www.google.fr/maps/place/" + LblPatientAdresse1.Text + " " + LblPatientCodePostal.Text + " " + LblPatientVille.Text
        Process.Start(MonURL)
        'Dim vFGoogleMaps As New FGoogleMaps
        'vFGoogleMaps.Show() 'Non Modal
    End Sub

    Private Sub DétailPatientToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DétailPatientToolStripMenuItem.Click
        'Initialisation du patient sélectionné
        Dim vFFPatientDetailEdit As New FPatientDetailEdit
        vFFPatientDetailEdit.SelectedPatientId = SelectedPatient.patientId
        vFFPatientDetailEdit.SelectedPatient = Me.SelectedPatient
        vFFPatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
        vFFPatientDetailEdit.ShowDialog() 'Modal
        vFFPatientDetailEdit.Dispose()
    End Sub

    Private Sub ListeDesALDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDesALDToolStripMenuItem.Click
        Dim vFPatientAldListe As New FPatientAldListe
        vFPatientAldListe.SelectedPatient = Me.SelectedPatient
        vFPatientAldListe.SelectedPatientId = Me.SelectedPatient.patientId
        vFPatientAldListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vFPatientAldListe.ShowDialog() 'Modal
        vFPatientAldListe.Dispose()

        'Vérification de l'existence d'ALD
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
        'If outils.ControleExistenceALD(Me.SelectedPatient.patientId) = True Then
        'LblALD.Show()
        'End If
    End Sub

    Private Sub LblALD_Click(sender As Object, e As EventArgs) Handles LblALD.Click
        Dim vFPatientAldListe As New FPatientAldListe
        vFPatientAldListe.SelectedPatient = Me.SelectedPatient
        vFPatientAldListe.SelectedPatientId = Me.SelectedPatient.patientId
        vFPatientAldListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vFPatientAldListe.ShowDialog() 'Modal
        vFPatientAldListe.Dispose()
    End Sub

    '==========================================================
    '======================= Antécédent =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementAntecedent()
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String
        If ChkPublie.Checked = True Then
            If ChkParPriorite.Checked = True Then
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3;"
            Else
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_antecedent_date_creation desc;"
            End If
        Else
            If ChkParPriorite.Checked = True Then
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and (oa_antecedent_statut_affichage = 'P' or oa_antecedent_statut_affichage = 'C')and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3;"
            Else
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and (oa_antecedent_statut_affichage = 'P' or oa_antecedent_statut_affichage = 'C')and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_antecedent_date_creation desc;"
            End If
        End If

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim indentation As String
        Dim dateDateModification As Date
        Dim AfficheDateModification As String
        Dim diagnostic As String
        Dim antecedentCache As Boolean

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            If ChkParPriorite.Checked = True Then
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
                AfficheDateModification = " (" + dateDateModification.ToString("MM.yyyy") + ")"
            Else
                If antecedentDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                    dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_creation")
                    AfficheDateModification = " (" + dateDateModification.ToString("MM.yyyy") + ")"
                End If
            End If

            'Identification si l'antécédent est caché
            antecedentCache = False
            If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    antecedentCache = True
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            AntecedentDataGridView.Rows.Insert(iGrid)
            'Alimentation du DataGridView
            diagnostic = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 4 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                'Récupération du libellé de la DRC/ORC
                Dim Drc As Drc = New Drc(antecedentDataTable.Rows(i)("oa_antecedent_drc_id"))
                AntecedentDataGridView("antecedent", iGrid).Value = indentation + diagnostic + Drc.DrcLibelle + AfficheDateModification
                AntecedentDataGridView("antecedentDescription", iGrid).Value = Drc.DrcLibelle
            Else
                AntecedentDataGridView("antecedent", iGrid).Value = indentation + diagnostic + antecedentDataTable.Rows(i)("oa_antecedent_description") + AfficheDateModification
                AntecedentDataGridView("antecedentDescription", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_description")
            End If

            If antecedentCache = True Then
                AntecedentDataGridView("antecedent", iGrid).Style.ForeColor = Color.Red
            End If

            'Id antécédent
            AntecedentDataGridView("antecedentId", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_id")
            AntecedentDataGridView("antecedentDrcId", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_drc_id")
            AntecedentDataGridView("antecedentNiveau", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_niveau")

            'Détermination de l'antécédent pere si niveau 2 et 3
            Select Case CInt(antecedentDataTable.Rows(i)("oa_antecedent_niveau"))
                Case 2
                    AntecedentDataGridView("antecedentPereId", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1")
                Case 3
                    AntecedentDataGridView("antecedentPereId", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2")
                Case Else
                    AntecedentDataGridView("antecedentPereId", iGrid).Value = 0
            End Select
        Next

        'Enlève le focus sur la première ligne de la Grid
        AntecedentDataGridView.ClearSelection()

        'Libération des ressources
        conxn.Close()
        antecedentDataAdapter.Dispose()
    End Sub

    'Affichage du détail d'un antécédent dans un popup
    Private Sub AntecedentDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AntecedentDataGridView.CellDoubleClick
        Dim antecedentId As Integer
        antecedentId = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentId").Value

        Dim vFAntecedentDetailEdit As New FAntecedentDetailEdit
        vFAntecedentDetailEdit.SelectedAntecedentId = antecedentId
        vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
        vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
        vFAntecedentDetailEdit.SelectedDrcId = 0

        vFAntecedentDetailEdit.ShowDialog() 'Modal
        If vFAntecedentDetailEdit.CodeRetour = True Then
            AntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
            If vFAntecedentDetailEdit.Reactivation = True Then
                'Rechargement des contextes si réactivation
                ContexteDataGridView.Rows.Clear()
                ChargementContexte()
            End If
        End If
        vFAntecedentDetailEdit.Dispose()
    End Sub

    'Créer un antécédent
    Private Sub CréerAntecedentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerAntecedentToolStripMenuItem.Click
        Dim vFDrcSelecteur As New FDrcSelecteur
        vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
        vFDrcSelecteur.CategorieOasis = 1       'Catégorie Oasis : "Antécédent et Contexte"
        vFDrcSelecteur.ShowDialog()             'Modal

        Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
        vFDrcSelecteur.Dispose()
        'Si un médicament a été sélectionné
        If SelectedDrcId <> 0 Then
            Dim vFAntecedentDetailEdit As New FAntecedentDetailEdit
            vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
            vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFAntecedentDetailEdit.SelectedDrcId = SelectedDrcId
            vFAntecedentDetailEdit.SelectedAntecedentId = 0

            vFAntecedentDetailEdit.ShowDialog() 'Modal

            'Si le traitement a été créé, on recharge la grid
            If vFAntecedentDetailEdit.CodeRetour = True Then
                AntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            End If

            vFAntecedentDetailEdit.Dispose()
        End If
    End Sub

    'Historique des modifications d'un antécédent
    Private Sub HistoriqueDesModificationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem.Click
        If AntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim AntecedentId As Integer = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentId").Value

            Dim vFAntecedenttHistoListe As New FAntecedentHistoListe
            vFAntecedenttHistoListe.SelectedAntecedentId = AntecedentId
            vFAntecedenttHistoListe.SelectedPatient = Me.SelectedPatient
            vFAntecedenttHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte

            vFAntecedenttHistoListe.ShowDialog() 'Modal
            vFAntecedenttHistoListe.Dispose()
        End If
    End Sub

    'Modifier l'ordre d'un antécédent
    Private Sub ModifierLordreDunAntécédentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModifierLordreDunAntécédentToolStripMenuItem.Click
        'Appel de la gestion de la modification de l'ordre d'un antécédent
        If AntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim AntecedentId As Integer = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentId").Value

            Dim vFAntecedentOrdreSelecteur As New FAntecedentOrdreSelecteur
            vFAntecedentOrdreSelecteur.SelectedPatient = Me.SelectedPatient
            vFAntecedentOrdreSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
            vFAntecedentOrdreSelecteur.AntecedentIdaOrdonner = AntecedentId
            vFAntecedentOrdreSelecteur.AntecedentDescriptionAOrdonner = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentDescription").Value
            vFAntecedentOrdreSelecteur.NiveauAntecedentAOrdonner = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentNiveau").Value
            vFAntecedentOrdreSelecteur.AntecedentIdPere = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentPereId").Value

            vFAntecedentOrdreSelecteur.ShowDialog() 'Modal

            'Si le traitement a été modifié, on recharge la grid
            If vFAntecedentOrdreSelecteur.CodeRetour = True Then
                AntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            End If

            vFAntecedentOrdreSelecteur.Dispose()
        End If
    End Sub

    'Changer l'affectation d'un antécédent (changement d'association)
    Private Sub ChangerLaffectationDunAntecedentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangerLaffectationDunAntecedentToolStripMenuItem.Click
        If AntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim AntecedentId As Integer = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentId").Value

            Dim vFAntecedentAffectationSelecteur As New FAntecedentAffectationSelecteur
            vFAntecedentAffectationSelecteur.SelectedPatient = Me.SelectedPatient
            vFAntecedentAffectationSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
            vFAntecedentAffectationSelecteur.AntecedentIdaAffecter = AntecedentId
            vFAntecedentAffectationSelecteur.AntecedentDescriptionaAffecter = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentDescription").Value
            vFAntecedentAffectationSelecteur.NiveauAntecedentaAffecter = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentNiveau").Value
            vFAntecedentAffectationSelecteur.AntecedentIdPere = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentPereId").Value

            vFAntecedentAffectationSelecteur.ShowDialog() 'Modal

            'Si le traitement a été modifié, on recharge la grid
            If vFAntecedentAffectationSelecteur.CodeRetour = True Then
                AntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            End If

            vFAntecedentAffectationSelecteur.Dispose()
        End If
    End Sub

    Private Sub ChkPublie_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPublie.CheckedChanged
        If ChkPublie.Checked = True Then
            ChkTous.Checked = False
            If InitPublie = True Then
                AntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            Else
                InitPublie = True
            End If
        Else
            If ChkTous.Checked = False Then
                ChkPublie.Checked = True
            End If
        End If
    End Sub

    Private Sub ChkTous_CheckedChanged(sender As Object, e As EventArgs) Handles ChkTous.CheckedChanged
        If ChkTous.Checked = True Then
            ChkPublie.Checked = False
            AntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        Else
            If ChkPublie.Checked = False Then
                ChkTous.Checked = True
            End If
        End If
    End Sub

    Private Sub ChkParPriorite_CheckedChanged(sender As Object, e As EventArgs) Handles ChkParPriorite.CheckedChanged
        If ChkParPriorite.Checked = True Then
            ChkParChronologie.Checked = False
            If InitParPriorite = True Then
                AntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            Else
                InitParPriorite = True
            End If
        Else
            If ChkParChronologie.Checked = False Then
                ChkParPriorite.Checked = True
            End If
        End If
    End Sub

    Private Sub ChkParChronologie_CheckedChanged(sender As Object, e As EventArgs) Handles ChkParChronologie.CheckedChanged
        If ChkParChronologie.Checked = True Then
            ChkParPriorite.Checked = False
            AntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        Else
            If ChkParPriorite.Checked = False Then
                ChkParChronologie.Checked = True
            End If
        End If
    End Sub

    '==========================================================
    '======================= Traitement =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim traitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim traitementDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        'Exlusion des traitements : traitements déclarés 'annulé' 
        SQLString = "select oa_traitement_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_ordre_affichage, oa_traitement_date_creation, oa_traitement_date_modification, oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_fenetre, oa_traitement_fenetre_date_debut, oa_traitement_fenetre_date_fin, oa_traitement_arret, oa_traitement_allergie, oa_traitement_contre_indication from oasis.oa_traitement where (oa_traitement_annulation is Null or oa_traitement_annulation = '') and oa_traitement_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_traitement_ordre_affichage;"

        traitementDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        traitementDataAdapter.Fill(traitementDataTable)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim Base As String
        Dim Posologie As String
        Dim dateFin, dateDebut, dateModification, dateCreation As Date
        Dim jours As Integer
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim FenetreTherapeutiqueEnCours As Boolean
        Dim FenetreTherapeutiqueAVenir As Boolean

        'Dim Allergie As Boolean = False
        Dim FenetreDateDebut, FenetreDateFin As Date

        Allergie = False

        ContreIndication = False
        LblAllergie.Visible = False
        TxtAllergies.Hide()
        LblContreIndication.Visible = False
        SelectedPatient.PatientAllergieCis.Clear()
        SelectedPatient.PatientAllergieDci.Clear()
        SelectedPatient.PatientContreIndicationCis.Clear()
        SelectedPatient.PatientContreIndicationDci.Clear()
        SelectedPatient.PatientMedicamentsPrescritsCis.Clear()

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Récupération des médicaments déclarés 'allergique' et exclusion de l'affichage
            If traitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    Allergie = True
                    SelectedPatient.PatientAllergieDci.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_dci"))
                    SelectedPatient.PatientAllergieCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))
                    Continue For
                End If
            End If

            'Récupération des médicaments déclarés 'contre-indiqué' et exclusion de l'affichage
            If traitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    ContreIndication = True
                    SelectedPatient.PatientContreIndicationDci.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_dci"))
                    SelectedPatient.PatientContreIndicationCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))
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
                If traitementDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = traitementDataTable.Rows(i)("oa_traitement_posologie_rythme")
                    Select Case traitementDataTable.Rows(i)("oa_traitement_posologie_base")
                        Case "J"
                            Base = "Journalier : "
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_matin") <> 0 Then
                                posologieMatin = traitementDataTable.Rows(i)("oa_traitement_posologie_matin")
                            Else
                                posologieMatin = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_midi") <> 0 Then
                                posologieMidi = traitementDataTable.Rows(i)("oa_traitement_posologie_midi")
                            Else
                                posologieMidi = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_soir") <> 0 Then
                                posologieSoir = traitementDataTable.Rows(i)("oa_traitement_posologie_soir")
                            Else
                                posologieSoir = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi") <> 0 Then
                                posologieApresMidi = traitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi")
                                Posologie = Base + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieApresMidi.ToString + "." + posologieSoir.ToString
                            Else
                                Posologie = Base + " " + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieSoir.ToString
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
            End If

            'Stockage des médicaments prescrits (pour contrôle lors de la selection d'un médicament dans le cadre d'un nouveau traitement
            SelectedPatient.PatientMedicamentsPrescritsCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            TraitementDataGridView.Rows.Insert(iGrid)
            'Alimentation du DataGridView
            'DCI
            TraitementDataGridView("medicamentDci", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")
            'Posologie
            TraitementDataGridView("posologie", iGrid).Value = Posologie

            If Posologie = "Fenêtre Th." Then
                TraitementDataGridView("posologie", iGrid).Style.ForeColor = Color.Red
            End If

            'Fenêtre thérapeutique existe (en cours ou à venir ou obsolète)
            If FenetreTherapeutiqueExiste = True Then
                TraitementDataGridView("fenetreTherapeutique", iGrid).Value = "O"
            Else
                TraitementDataGridView("fenetreTherapeutique", iGrid).Value = ""
            End If

            'Traitement du format d'affichage de la fin du traitement
            If dateDebut = "31/12/2999" Then
                TraitementDataGridView("dateDebut", iGrid).Value = "Date non définie"
            Else
                jours = (Date.Now - dateDebut).TotalDays
                If jours > 30 Then
                    TraitementDataGridView("dateDebut", iGrid).Value = dateDebut.ToString("MM.yyyy")
                Else
                    TraitementDataGridView("dateDebut", iGrid).Value = dateDebut.ToString("dd.MM.yyyy")
                End If
            End If

            'Traitement du format d'affichage de modification du traitement
            If dateModification = "01/01/1900" Then
                TraitementDataGridView("dateModification", iGrid).Value = "Date non définie"
            Else
                jours = (Date.Now - dateModification).TotalDays
                If jours > 30 Then
                    TraitementDataGridView("dateModification", iGrid).Value = dateModification.ToString("MM.yyyy")
                Else
                    TraitementDataGridView("dateModification", iGrid).Value = dateModification.ToString("dd.MM.yyyy")
                End If
            End If

            'Identifiant du traitement
            TraitementDataGridView("traitementId", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_id")

            'CIS du médicament
            TraitementDataGridView("MedicamentCis", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_medicament_cis")

            'Bouton gérer fenêtre thérapeutique
            If FenetreTherapeutiqueAVenir = True Or FenetreTherapeutiqueEnCours = True Then
                TraitementDataGridView("posologie", iGrid).Style.ForeColor = Color.Red
                'TODO: Bouton caché à présent code à supprimer si validation
                Dim bouton As DataGridViewButtonCell = TraitementDataGridView("BtnFenetreTherapeutique", iGrid)
                bouton.FlatStyle = FlatStyle.Flat
                bouton.Style.ForeColor = Color.DarkOrange
            End If
        Next

        'Si allergie, affichage des substances allergiques
        If Allergie = True Then
            LblAllergie.Visible = True
            TxtAllergies.Show()
            Dim premierPassage As Boolean = True
            Dim LongueurChaine, LongueurSub As Integer
            Dim AllergieTooltip As String
            Dim LongueurMax As Integer = 10

            'Chargement du TextBox
            Dim allergieString As String
            'Dim allergieEnumerator As StringEnumerator = SelectedPatient.PatientAllergieDci.GetEnumerator()
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
                    TxtAllergies.Text = allergieString.Substring(0, LongueurSub)
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
                    TxtAllergies.Text = TxtAllergies.Text & " *** " & allergieString.Substring(0, LongueurSub)
                    AllergieTooltip = AllergieTooltip + vbCrLf + allergieString
                End If
            End While
            ToolTip.SetToolTip(TxtAllergies, AllergieTooltip)
            'Chargement des médicaments génériques associés aux médicaments allergiques déclarés
            TraitementAllergies(Me.SelectedPatient)
        End If

        If ContreIndication = True Then
            LblContreIndication.Show()
            'Chargement des médicaments génériques associés aux médicaments contre-indiqués déclarés
        End If

        conxn.Close()
        traitementDataAdapter.Dispose()
        'Enlève le focus sur la première ligne de la Grid
        TraitementDataGridView.ClearSelection()
    End Sub

    'Affichage des allergies dans un popup depuis le label "Allergie"
    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        'Traitement : afficher les allergies dans un popup
        If Allergie = True Then
            Dim vFPatientAllergieListe As New FPatientAllergieListe
            vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
            vFPatientAllergieListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientAllergieListe.SelectedPatientAllergieCis = Me.SelectedPatient.PatientAllergieCis
            vFPatientAllergieListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientAllergieListe.ShowDialog() 'Modal
            vFPatientAllergieListe.Dispose()
        End If
        If Allergie = True Then
            'Dim vFTraitementAllergieEtCI As New FTraitementAllergieEtCI
            'vFTraitementAllergieEtCI.SelectedPatient = Me.SelectedPatient
            'vFTraitementAllergieEtCI.UtilisateurConnecte = Me.UtilisateurConnecte
            'vFTraitementAllergieEtCI.AllergieOuContreIndication = EnumAllergieOuContreIndication.Allergie
            'vFTraitementAllergieEtCI.ShowDialog() 'Modal
            'vFTraitementAllergieEtCI.Dispose()
        End If
    End Sub

    'Affichage des contre-indications dans un popup depuis le label "Contre-indication"
    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles LblContreIndication.Click
        Dim vFPatientContreIndicationListe As New FPatientContreIndicationListe
        vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
        vFPatientContreIndicationListe.SelectedPatientId = Me.SelectedPatient.patientId
        vFPatientContreIndicationListe.SelectedPatientCICis = Me.SelectedPatient.PatientContreIndicationCis
        vFPatientContreIndicationListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vFPatientContreIndicationListe.ShowDialog() 'Modal
        vFPatientContreIndicationListe.Dispose()
        'Traitement : afficher les contre-indications dans un popup
        'Dim vFTraitementAllergieEtCI As New FTraitementAllergieEtCI
        'vFTraitementAllergieEtCI.SelectedPatient = Me.SelectedPatient
        'vFTraitementAllergieEtCI.UtilisateurConnecte = Me.UtilisateurConnecte
        'vFTraitementAllergieEtCI.AllergieOuContreIndication = EnumAllergieOuContreIndication.ContreIndication
        'vFTraitementAllergieEtCI.ShowDialog() 'Modal
        'vFTraitementAllergieEtCI.Dispose()
    End Sub

    'Affichage des traitements obsolètes
    Private Sub TraitementsObsoletesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TraitementsObsoletesToolStripMenuItem.Click
        'Traitement : afficher les traitement stoppés dans un popup dédié
        Dim vFTraitementObsoletes As New FTraitementObsoletes
        vFTraitementObsoletes.SelectedPatient = Me.SelectedPatient
        vFTraitementObsoletes.UtilisateurConnecte = Me.UtilisateurConnecte

        vFTraitementObsoletes.ShowDialog() 'Modal

        vFTraitementObsoletes.Dispose()
    End Sub

    'Appel du form pour déclarer une allergie
    Private Sub DéclarerUneAllergieToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DéclarerUneAllergieToolStripMenuItem.Click
        'TODO: Appel de la déclaration d'une allergie
    End Sub

    'Liste de l'historique des modifications d'un traitement donné
    Private Sub HistoModificationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueToolStripMenuItem.Click
        If TraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim TraitementId As Integer = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("traitementId").Value

            Dim vFTraitementHistoListe As New FTraitementHistoListe
            vFTraitementHistoListe.SelectedTraitementId = TraitementId
            vFTraitementHistoListe.SelectedPatient = Me.SelectedPatient
            vFTraitementHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFTraitementHistoListe.MedicamentDenomination = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("medicamentDci").Value

            vFTraitementHistoListe.ShowDialog() 'Modal
            vFTraitementHistoListe.Dispose()
        End If
    End Sub

    'Affichage du détail d'un traitement dans un popup
    Private Sub TraitementDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles TraitementDataGridView.CellDoubleClick
        Dim TraitementId, SelectedMedicamentCis As Integer
        TraitementId = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("TraitementId").Value
        SelectedMedicamentCis = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("MedicamentCis").Value

        Dim vFTraitementDetailEdit As New FTraitementDetailEdit
        vFTraitementDetailEdit.SelectedTraitementId = TraitementId
        vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
        vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
        vFTraitementDetailEdit.SelectedMedicamentCis = SelectedMedicamentCis

        vFTraitementDetailEdit.ShowDialog() 'Modal

        If vFTraitementDetailEdit.CodeRetour = True Then
            TraitementDataGridView.Rows.Clear()
            ChargementTraitement()
        End If

        vFTraitementDetailEdit.Dispose()
    End Sub

    'Affichage de la gestion des traitements
    Private Sub GérerLesTraitementsToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim vFTraitementGestion As New FTraitementGestion
        vFTraitementGestion.SelectedPatient = Me.SelectedPatient
        vFTraitementGestion.UtilisateurConnecte = Me.UtilisateurConnecte

        vFTraitementGestion.ShowDialog() 'Modal
        vFTraitementGestion.Dispose()

        TraitementDataGridView.Rows.Clear()
        ChargementTraitement()
    End Sub

    'Création d'un traitement
    Private Sub CréerUnTraitementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnTraitementToolStripMenuItem.Click

        Dim vFMedocSelecteur As New FMedocSelecteur
        vFMedocSelecteur.SelectedPatient = Me.SelectedPatient
        vFMedocSelecteur.Allergie = Me.Allergie
        vFMedocSelecteur.ContreIndication = Me.ContreIndication

        vFMedocSelecteur.ShowDialog() 'Modal

        Dim SelectedMedicamentCis As Integer = vFMedocSelecteur.SelectedMedicamentCis
        vFMedocSelecteur.Dispose()
        'Si un médicament a été sélectionné
        If SelectedMedicamentCis <> 0 Then
            Dim vFTraitementDetailEdit As New FTraitementDetailEdit
            vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
            vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFTraitementDetailEdit.SelectedMedicamentCis = SelectedMedicamentCis
            vFTraitementDetailEdit.SelectedTraitementId = 0

            vFTraitementDetailEdit.ShowDialog() 'Modal

            'Si le traitement a été créé, on recharge la grid
            If vFTraitementDetailEdit.CodeRetour = True Then
                TraitementDataGridView.Rows.Clear()
                ChargementTraitement()
            End If

            vFTraitementDetailEdit.Dispose()
        End If
    End Sub

    'Appel gestion fenêtre thérapeutique depuis la grid
    Private Sub TraitementDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles TraitementDataGridView.CellClick
        Dim iGrid As Point
        Dim SelectedTraitementId As Integer
        iGrid = TraitementDataGridView.CurrentCellAddress()
        Dim iRow = iGrid.Y
        Dim iColumn = iGrid.X
        If iColumn = 5 Then
            SelectedTraitementId = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("traitementId").Value
            Dim vFTraitementFenetreTh As New FTraitementFenetreTh
            vFTraitementFenetreTh.SelectedPatient = Me.SelectedPatient
            vFTraitementFenetreTh.UtilisateurConnecte = Me.UtilisateurConnecte
            vFTraitementFenetreTh.SelectedTraitementId = SelectedTraitementId
            Dim fenetreTherapeutiqueExiste As Char = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("fenetreTherapeutique").Value
            If fenetreTherapeutiqueExiste = "O" Then
                vFTraitementFenetreTh.FenetreTherapeutiqueExiste = True
            Else
                vFTraitementFenetreTh.FenetreTherapeutiqueExiste = False
            End If

            vFTraitementFenetreTh.ShowDialog() 'Modal

            'Si le traitement a été créé, on recharge la grid
            If vFTraitementFenetreTh.CodeRetour = True Then
                TraitementDataGridView.Rows.Clear()
                ChargementTraitement()
            End If
        End If
    End Sub

    'Appel gestion fenêtre thérapeutique depuis le ToolStrip
    Private Sub GérerUneFenêtreThérapeutiqueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GérerUneFenêtreThérapeutiqueToolStripMenuItem.Click
        If TraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim SelectedTraitementId As Integer
            SelectedTraitementId = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("TraitementId").Value

            Dim vFTraitementFenetreTh As New FTraitementFenetreTh
            vFTraitementFenetreTh.SelectedPatient = Me.SelectedPatient
            vFTraitementFenetreTh.UtilisateurConnecte = Me.UtilisateurConnecte
            vFTraitementFenetreTh.SelectedTraitementId = SelectedTraitementId
            Dim fenetreTherapeutiqueExiste As Char = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("fenetreTherapeutique").Value
            If fenetreTherapeutiqueExiste = "O" Then
                vFTraitementFenetreTh.FenetreTherapeutiqueExiste = True
            Else
                vFTraitementFenetreTh.FenetreTherapeutiqueExiste = False
            End If

            vFTraitementFenetreTh.ShowDialog() 'Modal

            'Si le traitement a été créé, on recharge la grid
            If vFTraitementFenetreTh.CodeRetour = True Then
                TraitementDataGridView.Rows.Clear()
                ChargementTraitement()
            End If
        End If
    End Sub

    Private Sub ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Click
        Dim vFTraitementAllergieEtCI As New FTraitementAllergieEtCI
        vFTraitementAllergieEtCI.SelectedPatient = Me.SelectedPatient
        vFTraitementAllergieEtCI.UtilisateurConnecte = Me.UtilisateurConnecte
        vFTraitementAllergieEtCI.AllergieOuContreIndication = EnumAllergieOuContreIndication.Allergie
        vFTraitementAllergieEtCI.ShowDialog() 'Modal
        vFTraitementAllergieEtCI.Dispose()
    End Sub

    'Génération ordonnance à partir des traitements en cours
    Private Sub OrdonnanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrdonnanceToolStripMenuItem.Click
        'TODO: gestion ordonnance

    End Sub

    '================================================================
    '======================= Parcours de soin =======================
    '================================================================

    'Chargement de la Grid
    Private Sub ChargementParcoursDeSoin()

    End Sub

    '================================================================
    '======================= Contexte ===============================
    '================================================================

    'Chargement de la Grid
    Private Sub ChargementContexte()
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim contexteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim contexteDataTable As DataTable = New DataTable()
        Dim SQLString As String
        If ChkContextePublie.Checked = True Then
            SQLString = "select oa_antecedent_id, oa_antecedent_drc_id, oa_antecedent_description, oa_antecedent_diagnostic, oa_antecedent_statut_affichage, oa_antecedent_categorie_contexte, oa_antecedent_niveau, oa_antecedent_date_creation, oa_antecedent_date_modification, oa_antecedent_date_debut, oa_antecedent_date_fin, oa_antecedent_ordre_affichage1 from oasis.oa_antecedent where oa_antecedent_type = 'C' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and (oa_antecedent_arret = '0' or oa_antecedent_arret is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_antecedent_categorie_contexte desc, oa_antecedent_date_modification desc;"
        Else
            SQLString = "select oa_antecedent_id, oa_antecedent_drc_id, oa_antecedent_description, oa_antecedent_diagnostic, oa_antecedent_statut_affichage, oa_antecedent_categorie_contexte, oa_antecedent_niveau, oa_antecedent_date_creation, oa_antecedent_date_modification, oa_antecedent_date_debut, oa_antecedent_date_fin, oa_antecedent_ordre_affichage1 from oasis.oa_antecedent where oa_antecedent_type = 'C' and (oa_antecedent_statut_affichage = 'P' or oa_antecedent_statut_affichage = 'C') and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and (oa_antecedent_arret = '0' or oa_antecedent_arret is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_antecedent_categorie_contexte desc, oa_antecedent_date_modification desc;"
        End If

        'Lecture des données en base
        contexteDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        contexteDataAdapter.Fill(contexteDataTable)
        conxn.Open()

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
                Case "M"
                    categorieContexteString = "Médical"
                Case "B"
                    categorieContexteString = "Bio-environnemental"
                Case Else
                    categorieContexteString = ""
            End Select

            'DateFin
            If contexteDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                dateFin = contexteDataTable.Rows(i)("oa_antecedent_date_fin")
            Else
                dateFin = "31/12/9999"
            End If

            'Les traitements avec une date de fin ou une date d'arrêt inférieure à la date du jour ne sont pas affichés
            'If dateFin < Date.Now Then
            'Continue For
            'End If

            'Recherche si le contexte a été modifié (médical uniquement)
            AfficheDateModification = ""
            If categorieContexte = "M" Then
                If contexteDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = dateModification.ToString("MM.yyyy") + " : "
                Else
                    If contexteDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = dateModification.ToString("MM.yyyy") + " : "
                    End If
                End If
            End If

            'Affichage de l'ordre d'affichage
            If contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                ordreAffichage = contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            Else
                ordreAffichage = 0
            End If

            'prefixeContexte = "(Ordre : " + ordreAffichage.ToString + ") - "

            'Contexte caché
            contexteCache = False
            If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    contexteCache = True
                End If
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            ContexteDataGridView.Rows.Insert(iGrid)
            'Alimentation du DataGridView
            ContexteDataGridView("categorieContexte", iGrid).Value = categorieContexteString

            diagnostic = ""
            If contexteDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 4 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            If contexteDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or contexteDataTable.Rows(i)("oa_antecedent_description") = "" Then
                'Récupération du libellé de la DRC/ORC
                Dim Drc As Drc = New Drc(contexteDataTable.Rows(i)("oa_antecedent_drc_id"))
                ContexteDataGridView("contexte", iGrid).Value = AfficheDateModification + diagnostic + Drc.DrcLibelle
            Else
                ContexteDataGridView("contexte", iGrid).Value = AfficheDateModification + diagnostic + contexteDataTable.Rows(i)("oa_antecedent_description")
            End If

            If contexteCache = True Then
                ContexteDataGridView("contexte", iGrid).Style.ForeColor = Color.Red
            End If

            'Identifiant contexte
            ContexteDataGridView("contexteId", iGrid).Value = contexteDataTable.Rows(i)("oa_antecedent_id")
        Next
        conxn.Close()
        contexteDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        ContexteDataGridView.ClearSelection()
    End Sub
    'Appel détail contexte
    Private Sub ContexteDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ContexteDataGridView.CellDoubleClick
        Dim ContexteId As Integer
        ContexteId = ContexteDataGridView.Rows(ContexteDataGridView.CurrentRow.Index).Cells("ContexteId").Value

        Dim vFContexteDetailEdit As New FContexteDetailEdit
        vFContexteDetailEdit.SelectedContexteId = ContexteId
        vFContexteDetailEdit.SelectedPatient = Me.SelectedPatient
        vFContexteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
        vFContexteDetailEdit.SelectedDrcId = 0
        vFContexteDetailEdit.CategorieContexte = "M"

        vFContexteDetailEdit.ShowDialog() 'Modal
        If vFContexteDetailEdit.CodeRetour = True Then
            ContexteDataGridView.Rows.Clear()
            ChargementContexte()
            If vFContexteDetailEdit.ContexteTransformeEnAntecedent = True Then
                'Rechargement des contextes si réactivation
                AntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            End If
        End If
        vFContexteDetailEdit.Dispose()
    End Sub

    'Créer un contexte
    Private Sub CreerContexteToolStripMenu_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Dim vFDrcSelecteur As New FDrcSelecteur
        vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
        vFDrcSelecteur.CategorieOasis = 1       'Catégorie Oasis : "Antécédent et Contexte"
        vFDrcSelecteur.ShowDialog()             'Modal

        Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
        vFDrcSelecteur.Dispose()
        'Si un médicament a été sélectionné, on appelle le Formulaire de création
        If SelectedDrcId <> 0 Then
            Dim vFContexteDetailEdit As New FContexteDetailEdit
            vFContexteDetailEdit.SelectedPatient = Me.SelectedPatient
            vFContexteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFContexteDetailEdit.SelectedDrcId = SelectedDrcId
            vFContexteDetailEdit.SelectedContexteId = 0
            vFContexteDetailEdit.CategorieContexte = "M"

            vFContexteDetailEdit.ShowDialog() 'Modal

            'Si le traitement a été créé, on recharge la grid
            If vFContexteDetailEdit.CodeRetour = True Then
                ContexteDataGridView.Rows.Clear()
                ChargementContexte()
            End If

            vFContexteDetailEdit.Dispose()
        End If
    End Sub

    'Suivi de l'historique des modifications d'un contexte
    Private Sub HistoriqueDesModificationsContexteToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem1.Click
        If ContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim ContexteId As Integer = ContexteDataGridView.Rows(ContexteDataGridView.CurrentRow.Index).Cells("ContexteId").Value

            Dim vFAntecedenttHistoListe As New FAntecedentHistoListe
            vFAntecedenttHistoListe.SelectedAntecedentId = ContexteId
            vFAntecedenttHistoListe.SelectedPatient = Me.SelectedPatient
            vFAntecedenttHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte

            vFAntecedenttHistoListe.ShowDialog() 'Modal
            vFAntecedenttHistoListe.Dispose()
        End If
    End Sub

    Private Sub ChkContextePublie_CheckedChanged(sender As Object, e As EventArgs) Handles ChkContextePublie.CheckedChanged
        If ChkContextePublie.Checked = True Then
            ChkContexteTous.Checked = False
            If InitContextePublie = True Then
                ContexteDataGridView.Rows.Clear()
                ChargementContexte()
            Else
                InitContextePublie = True
            End If
        Else
            If ChkContexteTous.Checked = False Then
                ChkContextePublie.Checked = True
            End If
        End If
    End Sub

    Private Sub ChkContexteTous_CheckedChanged(sender As Object, e As EventArgs) Handles ChkContexteTous.CheckedChanged
        If ChkContexteTous.Checked = True Then
            ChkContextePublie.Checked = False
            ContexteDataGridView.Rows.Clear()
            ChargementContexte()
        Else
            If ChkContextePublie.Checked = False Then
                ChkContexteTous.Checked = True
            End If
        End If
    End Sub


    '===================================================
    '======================= PPS =======================
    '===================================================

    'Chargement de la Grid
    Private Sub ChargementPPS()
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim ppsDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim ppsDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0) and (oa_pps_affichage_synthese is Null or oa_pps_affichage_synthese = 1) and oa_pps_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_pps_categorie, oa_pps_sous_categorie;"

        'Lecture des données en base
        ppsDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        ppsDataAdapter.Fill(ppsDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateDebut, dateModification As Date
        Dim rowCount As Integer = ppsDataTable.Rows.Count - 1
        Dim categoriePPS, sousCategoriePPS, Rythme, SpecialiteId As Integer
        Dim ppsArret As Boolean
        Dim NaturePPS, CommentairePPS, AffichePPS, AfficheDateModification, BasePPS, BaseDescription, SpecialiteDescription As String

        PPSSuiviIdeExiste = False
        PPSSuiviMedecinExiste = False
        PPSSuiviSageFemmeExiste = False

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categoriePPS = 0
            If ppsDataTable.Rows(i)("oa_pps_categorie") IsNot DBNull.Value Then
                categoriePPS = ppsDataTable.Rows(i)("oa_pps_categorie")
            End If

            sousCategoriePPS = 0
            If ppsDataTable.Rows(i)("oa_pps_sous_categorie") IsNot DBNull.Value Then
                sousCategoriePPS = ppsDataTable.Rows(i)("oa_pps_sous_categorie")
            End If

            'Date de début
            If ppsDataTable.Rows(i)("oa_pps_date_debut") IsNot DBNull.Value Then
                dateDebut = ppsDataTable.Rows(i)("oa_pps_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Rythme
            Rythme = 0
            If ppsDataTable.Rows(i)("oa_pps_rythme") IsNot DBNull.Value Then
                Rythme = ppsDataTable.Rows(i)("oa_pps_rythme")
            End If

            'Base
            BaseDescription = ""
            BasePPS = ""
            If ppsDataTable.Rows(i)("oa_pps_base") IsNot DBNull.Value Then
                BasePPS = ppsDataTable.Rows(i)("oa_pps_base")
            End If
            Select Case BasePPS
                Case "A"
                    BaseDescription = "/an"
                Case "M"
                    BaseDescription = "/mois"
                Case "S"
                    BaseDescription = "/semaine"
            End Select


            'Commentaire
            CommentairePPS = ""
            If ppsDataTable.Rows(i)("oa_pps_commentaire") IsNot DBNull.Value Then
                CommentairePPS = ppsDataTable.Rows(i)("oa_pps_commentaire")
            End If

            'Détecter si les occurrences qui doivent être uniques existent pour ce patient
            If categoriePPS = 2 Then
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
            AfficheDateModification = ""
            If ppsDataTable.Rows(i)("oa_pps_date_modification") IsNot DBNull.Value Then
                dateModification = ppsDataTable.Rows(i)("oa_pps_date_modification")
                AfficheDateModification = dateModification.ToString("MM.yyyy") + " : "
            Else
                If ppsDataTable.Rows(i)("oa_pps_date_creation") IsNot DBNull.Value Then
                    dateModification = ppsDataTable.Rows(i)("oa_pps_date_creation")
                    AfficheDateModification = dateModification.ToString("MM.yyyy") + " : "
                End If
            End If

            'PPS caché
            ppsArret = False
            If ppsDataTable.Rows(i)("oa_pps_arret") IsNot DBNull.Value Then
                If ppsDataTable.Rows(i)("oa_pps_arret") = "1" Then
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

            SpecialiteDescription = ""
            'Présentation PPS : Suivi
            If categoriePPS = 2 Then
                If sousCategoriePPS = 2 Then
                    'Suivi mesures préventives (Code DRC, libellé DRC, commentaire)
                    NaturePPS = "Mesures préventives : "
                    AffichePPS = NaturePPS + " " + CommentairePPS
                Else
                    'Suivi IDE, Médecin référent, Sage-femme et Spécialiste (Base, Rythme, Commentaire)
                    Select Case sousCategoriePPS
                        Case 3
                            NaturePPS = "Suivi IDE : "
                        Case 4
                            NaturePPS = "Suivi médecin référent : "
                        Case 5
                            NaturePPS = "Suivi sage-femme : "
                        Case 6
                            'Récupération spécialité
                            If ppsDataTable.Rows(i)("oa_pps_specialite") IsNot DBNull.Value Then
                                SpecialiteId = ppsDataTable.Rows(i)("oa_pps_specialite")
                                SpecialiteDescription = Table_specialite.GetSpecialiteDescription(SpecialiteId)
                            End If
                            'TODO: récupération spécialité
                            NaturePPS = "Suivi " + SpecialiteDescription + " : "
                        Case Else
                            NaturePPS = "Inconnue "
                    End Select
                    AffichePPS = NaturePPS + Rythme.ToString + BaseDescription + " " + CommentairePPS + " " + AfficheDateModification
                End If
            End If

            'Présentation PPS : Stratégie contextuelle (Base, Rythme, Commentaire)
            If categoriePPS = 3 Then
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
                AffichePPS = AfficheDateModification + NaturePPS + " " + CommentairePPS
            End If


            'Ajout d'une ligne au DataGridView
            iGrid += 1
            PPSDataGridView.Rows.Insert(iGrid)
            'Alimentation du DataGridView
            If ppsArret = True Then
                PPSDataGridView("pps", iGrid).Style.ForeColor = Color.Red
            End If

            'Affichage du PPS
            PPSDataGridView("pps", iGrid).Value = AffichePPS

            'Identifiant pps
            PPSDataGridView("ppsId", iGrid).Value = ppsDataTable.Rows(i)("oa_pps_id")
        Next
        conxn.Close()
        ppsDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        PPSDataGridView.ClearSelection()
    End Sub

    Private Sub GestionDuPlanPersonnaliséDeSantéToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestionDuPlanPersonnaliséDeSantéToolStripMenuItem.Click
        Dim vFPPSGetion As New FPatientPPSListeGestion
        vFPPSGetion.SelectedPatient = Me.SelectedPatient
        vFPPSGetion.UtilisateurConnecte = Me.UtilisateurConnecte
        vFPPSGetion.ShowDialog() 'Modal
        vFPPSGetion.Dispose()
        PPSDataGridView.Rows.Clear()
        ChargementPPS()
    End Sub

    '===========================================================
    '======================= Généralités =======================
    '===========================================================

    'Initialisation de l'écran
    Private Sub initZones()
        'ToolTip affichant les allergies et les contre-indications des traitements est présenté en forme de bulle
        'Etat civil
        LblPatientNIR.Text = ""
        LblPatientPrenom.Text = ""
        LblPatientNom.Text = ""
        LblPatientAge.Text = ""
        LblPatientGenre.Text = ""
        LblPatientAdresse1.Text = ""
        LblPatientAdresse2.Text = ""
        LblPatientCodePostal.Text = ""
        LblPatientVille.Text = ""
        LblPatientTel1.Text = ""
        LblPatientTel2.Text = ""
        LblPatientSite.Text = ""
        LblPatientUniteSanitaire.Text = ""
        LblPatientDateMaj.Text = ""
        'Antécédents
        AntecedentDataGridView.Rows.Clear()
        'Traitements
        TraitementDataGridView.Rows.Clear()
        'Parcours de soin
        ParcoursDeSoinDataGridView.Rows.Clear()
        'Contexte
        ContexteDataGridView.Rows.Clear()
        'PPS
        PPSDataGridView.Rows.Clear()
    End Sub

    Private Sub GestionDroitsAcces()
        'Accès à la création d'un traitement via le TraitementContexteMenuStrip selon les droits de l'utilisateur connecté
        If UtilisateurConnecte.UtilisateurNiveauAcces = 1 Or UtilisateurConnecte.UtilisateurAdmin = True Then
            CréerUnTraitementToolStripMenuItem.Available = True
        Else
            CréerUnTraitementToolStripMenuItem.Available = False
        End If
    End Sub

    'Gestion des vaccins
    Private Sub BtnVaccins_Click(sender As Object, e As EventArgs) Handles BtnVaccins.Click
        Dim vFPatientNoteVaccinListe As New FPatientNoteVaccinListe
        vFPatientNoteVaccinListe.SelectedPatientId = Me.SelectedPatient.patientId
        vFPatientNoteVaccinListe.SelectedPatient = Me.SelectedPatient
        vFPatientNoteVaccinListe.UtilisateurConnecte = Me.UtilisateurConnecte

        vFPatientNoteVaccinListe.ShowDialog() 'Modal
        vFPatientNoteVaccinListe.Dispose()
    End Sub

    'Gestion des directives anticipées
    Private Sub BtnDirectives_Click(sender As Object, e As EventArgs) Handles BtnDirectives.Click

    End Sub

    'Gestion du volet Social
    Private Sub BtnSocial_Click(sender As Object, e As EventArgs) Handles BtnSocial.Click
        Dim vFPatientNoteSocialListe As New FPatientNoteSocialListe
        vFPatientNoteSocialListe.SelectedPatientId = Me.SelectedPatient.patientId
        vFPatientNoteSocialListe.SelectedPatient = Me.SelectedPatient
        vFPatientNoteSocialListe.UtilisateurConnecte = Me.UtilisateurConnecte

        vFPatientNoteSocialListe.ShowDialog() 'Modal
        vFPatientNoteSocialListe.Dispose()
    End Sub

    'Gestion de la ligne de vie
    Private Sub BtnLigneDeVie_Click(sender As Object, e As EventArgs) Handles BtnLigneDeVie.Click

    End Sub

    'Gestion notes médicales
    Private Sub BtnNotesMedicales_Click(sender As Object, e As EventArgs) Handles BtnNotesMedicales.Click
        Dim vFPatientNoteMedicalListe As New FPatientNoteMedicalListe
        vFPatientNoteMedicalListe.SelectedPatientId = Me.SelectedPatient.patientId
        vFPatientNoteMedicalListe.SelectedPatient = Me.SelectedPatient
        vFPatientNoteMedicalListe.UtilisateurConnecte = Me.UtilisateurConnecte

        vFPatientNoteMedicalListe.ShowDialog() 'Modal
        vFPatientNoteMedicalListe.Dispose()
    End Sub

    'Gestion épisode de soin
    Private Sub BtnEpisode_Click(sender As Object, e As EventArgs) Handles BtnEpisode.Click
        'TODO: Appel liste des épisodes de soin
    End Sub

End Class