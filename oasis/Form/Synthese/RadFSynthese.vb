﻿Imports System.Configuration
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Imports System.Diagnostics

Public Class RadFSynthese
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private _rendezVousId As Long
    Private _IsRendezVousCloture As Boolean
    Private _ecranPrecedent As EnumAccesEcranPrecedent

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
            Return _IsRendezVousCloture
        End Get
        Set(value As Boolean)
            _IsRendezVousCloture = value
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

    Public Enum EnumOrigineAppel
        AUTRE = 0
        EPISODE = 2
    End Enum

    ReadOnly antecedentChangementOrdreDao As New AntecedentChangementOrdreDao
    ReadOnly antecedentAffectationDao As New AntecedentAffectationDao
    ReadOnly antecedentDao As New AntecedentDao
    ReadOnly theriaqueDao As New TheriaqueDao
    ReadOnly ordonnaceDao As New OrdonnanceDao
    ReadOnly episodeDao As New EpisodeDao

    Dim InitPublie, InitParPriorite, InitMajeur, InitContextePublie, InitParcoursNonCache, InitContexteBioPublie As Boolean
    Dim PatientAllergie, PatientContreIndication As Boolean
    Dim PPSSuiviIdeExiste, PPSSuiviSageFemmeExiste, PPSSuiviMedecinExiste As Boolean
    Dim LongueurStringAllergie As Integer
    ReadOnly ParcoursListProfilsOasis As New List(Of Integer)

    'Antécédent
    Dim iGridMax As Integer
    Dim NouveauOrdreAffichage As Integer
    Dim NiveauAntecedentAOrdonner As Integer
    Dim antecedentIdADeplacer, IndexAntecedentADeplacer As Integer

    Private Sub RadFSynthese_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Contrôle d'accès aux écran Synthèse, épisode et ligne de vie
        Environnement.ControleAccesForm.AddFormToControl(EnumForm.SYNTHESE.ToString)

        If Environnement.ControleAccesForm.IsAccessToFormOK(EnumForm.LIGNE_DE_VIE.ToString) = False Then
            RadBtnLigneDeVie.Hide()
        End If

        If Environnement.ControleAccesForm.IsAccessToFormOK(EnumForm.EPISODE.ToString) = False Then
            RadBtnEpisode.Hide()
        End If

        RadBtnUp.Text = Char.ConvertFromUtf32(8593)
        RadBtnDown.Text = Char.ConvertFromUtf32(8595)
        RadBtnRight.Text = Char.ConvertFromUtf32(8594)
        RadBtnLeft.Text = Char.ConvertFromUtf32(8592)

        Dim actiondao As New ActionDao
        Dim action As New Action With {
            .UtilisateurId = userLog.UtilisateurId,
            .PatientId = SelectedPatient.PatientId,
            .Action = "Accès synthèse patient",
            .Fonction = ActionDao.EnumFonctionCode.SYNTHESE,
            .FonctionId = 0
        }
        actiondao.CreationAction(action)

        AfficheTitleForm(Me, "Synthèse patient", userLog)

        'CreateLog("Test depuis synthese, utilisateur : " & userLog.UtilisateurId.ToString, Me.Name, LogDao.EnumTypeLog.INFO.ToString)
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()
        ChargementParametreApplication()
        InitZones()
        GestionDroitsAcces()
        ChargementEtatCivil()
        ChargementAntecedent()
        ChargementTraitement()
        ChargementParcoursDeSoin()
        ChargementContexte()
        ChargementPPS()
        ControleExistenceEpisode()
    End Sub

    Private Sub ChargementParametreApplication()
        'Récupération du nom de l'organisation dans les paramètres de l'application
        Dim LongueurStringAllergieString As String = ConfigurationManager.AppSettings("longueurStringAllergie")
        If IsNumeric(LongueurStringAllergieString) Then
            LongueurStringAllergie = CInt(LongueurStringAllergieString)
        Else
            CreateLog("Paramètre application 'longueurStringAllergie' non trouvé !", "Synthese", Log.EnumTypeLog.ERREUR.ToString, userLog)
            LongueurStringAllergie = 12
        End If
    End Sub

    Private Sub ControleExistenceEpisode()
        Dim episodeDao As New EpisodeDao
        Dim episode As Episode
        episode = episodeDao.GetEpisodeEnCoursByPatientId(Me.SelectedPatient.PatientId)
        Dim BtnColor As New Color
        BtnColor = RadBtnEpisode.BackColor

        If EcranPrecedent = EnumAccesEcranPrecedent.EPISODE OrElse
            EcranPrecedent = EnumAccesEcranPrecedent.EPISODE_ET_SYNTHESE OrElse
            EcranPrecedent = EnumAccesEcranPrecedent.LIGNE_DE_VIE_ET_EPISODE Then
            RadBtnEpisode.Hide()
        Else
            If episode.Id <> 0 Then
                RadBtnEpisode.ForeColor = Color.Red
                RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Bold)
                ToolTip.SetToolTip(RadBtnEpisode, "Episode en cours existant pour ce patient")
            End If
        End If
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
        LblDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj, True)

        Dim DateMaxValue = New Date(9998, 12, 31, 0, 0, 0)
        Dim DateMinValue = New Date(1, 1, 1, 0, 0, 0)
        If SelectedPatient.PatientDateEntree = DateMaxValue OrElse SelectedPatient.PatientDateEntree = DateMinValue OrElse SelectedPatient.PatientDateSortie < Date.Now Then
            LblNonOasis.Show()
        Else
            LblNonOasis.Hide()
        End If


        'Vérification de l'existence d'ALD
        LblALD.Hide()
        ChargementToolTipAld()

        'Contre-indication
        GetContreIndication()

        'Allergie
        GetAllergie()

    End Sub

    Private Sub GetContreIndication()
        Dim patientDao As New PatientDao
        Dim StringContreIndicationToolTip As String = patientDao.GetStringContreIndicationByPatient(SelectedPatient.PatientId)
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
        Dim patientDao As New PatientDao
        Dim StringAllergieToolTip As String = patientDao.GetStringAllergieByPatient(SelectedPatient.PatientId)
        If StringAllergieToolTip = "" Then
            PatientAllergie = False
            LblAllergie.Hide()
            LblSubstance.Hide()
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = False
        Else
            PatientAllergie = True
            LblAllergie.Show()
            ToolTip.SetToolTip(LblAllergie, StringAllergieToolTip)
            LblSubstance.Show()
            LblSubstance.Text = StringAllergieToolTip.Replace(vbCrLf, " ")
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub ToolStripMenuItemMaps_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemMaps.Click
        'Etat civil : lancer l'URL pour afficher l'adresse dans Google Maps
        Dim MonURL As String
        MonURL = "http://www.google.fr/maps/place/" + SelectedPatient.PatientAdresse1 + " " + SelectedPatient.PatientCodePostal + " " + SelectedPatient.PatientVille
        Process.Start(MonURL)
    End Sub

    Private Sub DétailPatientToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DétailPatientToolStripMenuItem.Click
        'Initialisation du patient sélectionné
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFFPatientDetailEdit As New RadFPatientDetailEdit
                vFFPatientDetailEdit.SelectedPatientId = SelectedPatient.PatientId
                vFFPatientDetailEdit.SelectedPatient = Me.SelectedPatient
                vFFPatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                vFFPatientDetailEdit.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub


    Private Sub ListeDesALDToolStripMenuItem_Click(sender As Object, e As EventArgs)
        ChargementToolTipAld()
    End Sub

    '==========================================================
    '======================= Antécédent =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementAntecedent()
        RadAntecedentDataGridView.Rows.Clear()

        Dim antecedentDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao
        If RadChkPublie.Checked = True Then
            If RadChkParPriorite.Checked = True Then
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.PatientId, True, True)
            Else
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.PatientId, True, False)
            End If
        Else
            If RadChkParPriorite.Checked = True Then
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.PatientId, False, True)
            Else
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.PatientId, False, False)
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
            If ChkChaineEpisodeActive.Checked AndAlso (IsDBNull(antecedentDataTable.Rows(i)("oa_chaine_episode_date_fin")) OrElse antecedentDataTable.Rows(i)("oa_chaine_episode_date_fin") < Date.Now()) Then
                Continue For
            End If
            If RadChkMajeurSeul.Checked AndAlso antecedentDataTable.Rows(i)("oa_antecedent_niveau") <> 1 Then
                Continue For
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

            Dim antecedentDescription As String

            '===== Affichage antécédent
            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                antecedentDescription = ""
            Else
                antecedentDescription = antecedentDataTable.Rows(i)("oa_antecedent_description")
                antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
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
            RadAntecedentDataGridView.CurrentRow = RadAntecedentDataGridView.Rows(0)
            RadAntecedentDataGridView.TableElement.VScrollBar.Value = 0
        End If
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

                Try
                    Using vFAntecedentDetailEdit As New RadFAntecedentDetailEdit
                        vFAntecedentDetailEdit.SelectedAntecedentId = antecedentId
                        vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
                        vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFAntecedentDetailEdit.SelectedDrcId = 0
                        vFAntecedentDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                        vFAntecedentDetailEdit.ShowDialog() 'Modal
                        If vFAntecedentDetailEdit.CodeRetour = True Then
                            RadAntecedentDataGridView.Rows.Clear()
                            ChargementAntecedent()
                            If vFAntecedentDetailEdit.Reactivation = True Then
                                'Rechargement des contextes si réactivation
                                ChargementContexte()
                            End If
                            ChargementToolTipAld()
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

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
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Dim SelectedDrcId As Integer
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFDrcSelecteur As New RadFDRCSelecteur
                vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
                vFDrcSelecteur.CategorieOasis = Drc.EnumCategorieOasisCode.Contexte       'Catégorie Oasis : "Antécédent et Contexte"
                vFDrcSelecteur.ShowDialog()             'Modal
                SelectedDrcId = vFDrcSelecteur.SelectedDrcId
                'Si un DRC a été sélectionné
                If SelectedDrcId <> 0 Then

                    Try
                        Using vFAntecedentDetailEdit As New RadFAntecedentDetailEdit
                            vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
                            vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFAntecedentDetailEdit.SelectedDrcId = SelectedDrcId
                            vFAntecedentDetailEdit.SelectedDrc = vFDrcSelecteur.SelectedDrc
                            vFAntecedentDetailEdit.SelectedAntecedentId = 0
                            vFAntecedentDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                            vFAntecedentDetailEdit.ShowDialog() 'Modal
                            'Si le traitement a été créé, on recharge la grid
                            If vFAntecedentDetailEdit.CodeRetour = True Then
                                ChargementAntecedent()
                                ChargementToolTipAld()
                            End If
                        End Using
                    Catch ex As Exception
                        MsgBox(ex.Message())
                    End Try

                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

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

                Try
                    Using vFAntecedenttHistoListe As New RadFAntecedentHistoListe
                        vFAntecedenttHistoListe.SelectedAntecedentId = AntecedentId
                        vFAntecedenttHistoListe.SelectedPatient = Me.SelectedPatient
                        vFAntecedenttHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFAntecedenttHistoListe.ShowDialog()
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
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

                Try
                    Using vFAntecedentOrdreSelecteur As New RadFAntecedentOrdreSelecteur
                        vFAntecedentOrdreSelecteur.SelectedPatient = Me.SelectedPatient
                        vFAntecedentOrdreSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFAntecedentOrdreSelecteur.AntecedentIdaOrdonner = AntecedentId
                        vFAntecedentOrdreSelecteur.AntecedentDescriptionAOrdonner = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentDescription").Value
                        vFAntecedentOrdreSelecteur.NiveauAntecedentAOrdonner = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentNiveau").Value
                        vFAntecedentOrdreSelecteur.AntecedentIdPere = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentPereId").Value
                        vFAntecedentOrdreSelecteur.PositionGaucheDroite = EnumPosition.Droite
                        vFAntecedentOrdreSelecteur.ShowDialog() 'Modal
                        'Si le traitement a été modifié, on recharge la grid
                        If vFAntecedentOrdreSelecteur.CodeRetour = True Then
                            ChargementAntecedent()
                        End If
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub


    'Déplacement vertical des antécédents (Up et Down)
    'Up
    Private Sub RadBtnUp_Click(sender As Object, e As EventArgs) Handles RadBtnUp.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Dim CodeRetour As Boolean = False
        Dim antecedentId, antecedentIdPere As Integer
        Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
        If aRow >= 0 Then
            Cursor.Current = Cursors.WaitCursor

            Try
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
                Dim Cacher As String = Antecedent.EnumStatutAffichage.PUBLIE
                If RadChkTous.Checked = True Then
                    Cacher = Antecedent.EnumStatutAffichage.CACHE
                End If
                antecedentChangementOrdreDao.UpdateAntecedent(antecedentId, NouveauOrdreAffichage, NiveauAntecedentAOrdonner)

                AntecedentModificationOrdre(NiveauAntecedentAOrdonner)
                CodeRetour = antecedentChangementOrdreDao.AntecedentReorganisationOrdre(NiveauAntecedentAOrdonner, SelectedPatient.PatientId, antecedentIdPere, NiveauAntecedentAOrdonner, Cacher)
                If CodeRetour = True Then
                    ChargementAntecedentAvecPositionnementCurseur()
                End If
            Catch ex As Exception
                MsgBox(ex.Message())
            End Try

            Cursor.Current = Cursors.Default
        End If
    End Sub

    'Down
    Private Sub RadBtnDown_Click(sender As Object, e As EventArgs) Handles RadBtnDown.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Dim CodeRetour As Boolean = False
        Dim antecedentId, antecedentIdPere As Integer
        Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
        If aRow >= 0 Then
            Cursor.Current = Cursors.WaitCursor

            Try
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
                Dim Cacher As String = Antecedent.EnumStatutAffichage.PUBLIE
                If RadChkTous.Checked = True Then
                    Cacher = Antecedent.EnumStatutAffichage.CACHE
                End If
                antecedentChangementOrdreDao.UpdateAntecedent(antecedentId, NouveauOrdreAffichage, NiveauAntecedentAOrdonner)

                AntecedentModificationOrdre(NiveauAntecedentAOrdonner)
                CodeRetour = antecedentChangementOrdreDao.AntecedentReorganisationOrdre(NiveauAntecedentAOrdonner, SelectedPatient.PatientId, antecedentIdPere, NiveauAntecedentAOrdonner, Cacher)
                If CodeRetour = True Then
                    ChargementAntecedentAvecPositionnementCurseur()
                End If
            Catch ex As Exception
                MsgBox(ex.Message())
            End Try

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

    'Traitement du déplacement horizontal des antécédents
    'Flèche droite : recherche de l'antécédent précédent de même niveau, l'antécédent sélectionné devient le fils de l'antécédent précédent
    'Pas d'effet sur un niveau 3 et s'il n'y a pas d'antécédent précédent
    Private Sub RadBtnRight_Click(sender As Object, e As EventArgs) Handles RadBtnRight.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            Cursor.Current = Cursors.WaitCursor
            Try
                If aRow >= 0 Then
                    Dim antecedentPere As Antecedent
                    Dim antecedentId, antecedentIdPere, niveauActuel As Integer
                    Dim NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3 As Integer
                    Dim Cacher As String = Antecedent.EnumStatutAffichage.PUBLIE
                    If RadChkTous.Checked = True Then
                        Cacher = Antecedent.EnumStatutAffichage.CACHE
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
                                'AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3)
                                ChargementAntecedentAvecPositionnementCurseur()
                            Case 2 'Passe de niveau 2 à niveau 3 sur antécédent niveau 2 précédent si existe
                                NiveauCible = 3
                                AntecedentId1 = antecedentPere.Niveau1Id
                                AntecedentId2 = antecedentIdPere
                                ordre1 = antecedentPere.Ordre1
                                ordre2 = antecedentPere.Ordre2
                                ordre3 = 990
                                antecedentAffectationDao.AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3, SelectedPatient, Cacher)
                                'AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3)
                                ChargementAntecedentAvecPositionnementCurseur()
                            Case 3
                                'Pas d'effet
                        End Select
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.Message())
            End Try
            Cursor.Current = Cursors.Default
        End If
    End Sub

    'Flèche gauche : recherche de l'antécédent précédent de même niveau, l'antécédent sélectionné partage le même niveau et le même antécédent père que l'antécédent précédent 
    'Particularité : pas d'antécédent père pour un antécédent de niveau 2 qui passe par conséquent en niveau 1
    'Pas d'effet sur un niveau 1 et s'il n'y a pas d'antécédent précédent
    Private Sub RadBtnLeft_Click(sender As Object, e As EventArgs) Handles RadBtnLeft.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Cursor.Current = Cursors.WaitCursor
        Try
            If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
                Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
                If aRow >= 0 Then
                    Dim antecedentId, antecedentIdPere, niveauActuel As Integer
                    Dim NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3 As Integer
                    Dim Cacher As String = Antecedent.EnumStatutAffichage.PUBLIE
                    If RadChkTous.Checked = True Then
                        Cacher = Antecedent.EnumStatutAffichage.CACHE
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
                            'AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3)
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
                            'AntecedentModificationNiveau(antecedentId, antecedentIdPere, niveauActuel, NiveauCible, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3)
                            ChargementAntecedentAvecPositionnementCurseur()
                    End Select
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Cursor.Current = Cursors.Default
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
            ChargementAntecedent()
        Else
            If RadChkMajeurTous.Checked = False Then
                RadChkMajeurSeul.Checked = True
            End If
        End If
    End Sub

    Private Sub ChkChaineEpisodeActive_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles ChkChaineEpisodeActive.ToggleStateChanged
        'ChkChaineEpisodeActive.Checked = Not ChkChaineEpisodeActive.Checked
        Application.DoEvents()
        ChargementAntecedent()
    End Sub

    Private Sub RadChkMajeurTous_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkMajeurTous.ToggleStateChanged
        If RadChkMajeurTous.Checked = True Then
            RadChkMajeurSeul.Checked = False
            If InitMajeur = True Then
                Application.DoEvents()
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

        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.PatientId)
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
        RadTraitementDataGridView.Rows.Clear()

        Dim traitementDataTable As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao
        traitementDataTable = traitementDao.GetTraitementEnCoursbyPatient(Me.SelectedPatient.PatientId)

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

            '===========================================================================> Obsolète début
            'Exclusion de l'affichage des traitements dont la date de fin est < à la date du jour
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications dans la StringCollection quel que soit leur date de fin
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                Continue For
            End If
            '===========================================================================> Obsolète fin

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

                FractionMatin = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_matin"), Traitement.EnumFraction.Non)
                FractionMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_midi"), Traitement.EnumFraction.Non)
                FractionApresMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), Traitement.EnumFraction.Non)
                FractionSoir = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_soir"), Traitement.EnumFraction.Non)

                posologieMatin = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_matin"), 0)
                posologieMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_midi"), 0)
                posologieApresMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_apres_midi"), 0)
                posologieSoir = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_soir"), 0)

                PosologieBase = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_base"), "")

                If FractionMatin <> "" AndAlso FractionMatin <> Traitement.EnumFraction.Non Then
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

                If FractionMidi <> "" AndAlso FractionMidi <> Traitement.EnumFraction.Non Then
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
                If FractionApresMidi <> "" AndAlso FractionApresMidi <> Traitement.EnumFraction.Non Then
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

                If FractionSoir <> "" AndAlso FractionSoir <> Traitement.EnumFraction.Non Then
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
                        Case Traitement.EnumBaseCode.JOURNALIER
                            Base = ""
                            If posologieApresMidi <> 0 OrElse FractionApresMidi <> Traitement.EnumFraction.Non Then
                                Posologie = Base + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieApresMidiString + ". " + PosologieSoirString
                            Else
                                Posologie = Base + " " + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieSoirString
                            End If
                        Case Else
                            Dim RythmeString As String = ""
                            If FractionMatin <> "" AndAlso FractionMatin <> Traitement.EnumFraction.Non Then
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
                            Base = traitementDao.GetBaseDescription(traitementDataTable.Rows(i)("oa_traitement_posologie_base"))
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
        traitementArretDatatable = traitementDao.GetAllTraitementArreteByPatient(Me.SelectedPatient.PatientId)
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
            RadTraitementDataGridView.CurrentRow = RadTraitementDataGridView.Rows(0)
            RadTraitementDataGridView.TableElement.VScrollBar.Value = 0
        End If
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
        Dim episodeEnCours = episodeDao.GetEpisodeEnCoursByPatientId(SelectedPatient.PatientId)
        If episodeEnCours IsNot Nothing Then
            Dim dt = ordonnaceDao.GetOrdonnanceValideByPatient(SelectedPatient.PatientId, episodeEnCours.Id)
            If dt.Count > 0 AndAlso MsgBox("Vous allez apporter des modifications aux traitements prescrits à ce patient alors qu'une ordonnance a été produite et en attente de validation sur l'episode [" + episodeEnCours.Type + " - " + episodeEnCours.TypeActivite + "]. Sans l'annulation de l'ordonnance en cours et sa re-génération, l'ordonnance ne sera pas alignée avec vos modifications.", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Création d'un traitement") = MsgBoxResult.No Then
                Return
            End If
        End If
        CreationTraitement()
    End Sub

    Private Sub CréerUnTraitementToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CréerUnTraitementToolStripMenuItem1.Click
        CreationTraitement()
    End Sub

    Private Sub CreationTraitement()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Dim SelectedMedicamentId As Integer
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using form As New RadFMedicamentSelecteur
                form.SelectedPatient = SelectedPatient
                form.ShowDialog() 'Modal
                SelectedMedicamentId = form.SelectedSpecialiteId
                'Si un médicament a été sélectionné
                If SelectedMedicamentId <> 0 Then

                    Try
                        Using vFTraitementDetailEdit As New RadFTraitementDetailEdit
                            vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
                            'vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFTraitementDetailEdit.SelectedMedicamentId = SelectedMedicamentId
                            vFTraitementDetailEdit.Allergie = Me.PatientAllergie
                            vFTraitementDetailEdit.ContreIndication = Me.PatientContreIndication
                            vFTraitementDetailEdit.SelectedTraitementId = 0
                            vFTraitementDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                            vFTraitementDetailEdit.ShowDialog() 'Modal
                            'Si le traitement a été créé, on recharge la grid
                            If vFTraitementDetailEdit.CodeRetour = True Then
                                ChargementTraitement()
                            End If
                        End Using
                    Catch ex As Exception
                        MsgBox(ex.Message())
                    End Try
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    'Affichage du détail d'un traitement dans un popup
    Private Sub RadTraitementDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadTraitementDataGridView.CellDoubleClick
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim episodeEnCours = episodeDao.GetEpisodeEnCoursByPatientId(SelectedPatient.PatientId)
            If episodeEnCours IsNot Nothing Then
                Dim dt = ordonnaceDao.GetOrdonnanceValideByPatient(SelectedPatient.PatientId, episodeEnCours.Id)
                If dt.Count > 0 AndAlso MsgBox("Vous allez apporter des modifications aux traitements prescrits à ce patient alors qu'une ordonnance a été produite et en attente de validation sur l'episode [" + episodeEnCours.Type + " - " + episodeEnCours.TypeActivite + "]. Sans l'annulation de l'ordonnance en cours et sa re-génération, l'ordonnance ne sera pas alignée avec vos modifications.", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Modification d'un traitement") = MsgBoxResult.No Then
                    Return
                End If
            End If
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId, SelectedMedicamentCis As Integer
                TraitementId = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                SelectedMedicamentCis = RadTraitementDataGridView.Rows(aRow).Cells("MedicamentCis").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False

                Try
                    Using vFTraitementDetailEdit As New RadFTraitementDetailEdit
                        vFTraitementDetailEdit.SelectedTraitementId = TraitementId
                        vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
                        'vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFTraitementDetailEdit.SelectedMedicamentId = SelectedMedicamentCis
                        vFTraitementDetailEdit.Allergie = Me.PatientAllergie
                        vFTraitementDetailEdit.ContreIndication = Me.PatientContreIndication
                        vFTraitementDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                        vFTraitementDetailEdit.ShowDialog() 'Modal
                        If vFTraitementDetailEdit.CodeRetour = True Then
                            ChargementTraitement()
                            GetAllergie()
                            GetContreIndication()
                        End If
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub

    'Liste des traitements obsolètes
    Private Sub TraitementsObsoletesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TraitementsObsoletesToolStripMenuItem.Click
        'Traitement : afficher les traitement stoppés dans un popup dédié
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try

            Using vFTraitementObsoletes As New RadFTraitementObsoletes
                    vFTraitementObsoletes.SelectedPatient = Me.SelectedPatient
                    vFTraitementObsoletes.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementObsoletes.ShowDialog() 'Modal
                End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    'Liste des substances contre-indiquées
    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles LblContreIndication.Click
        ListeMedicamentCI()
    End Sub

    Private Sub ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Click
        If PatientContreIndication = True Then
            ListeMedicamentCI()
        End If
    End Sub

    Private Sub ListeMedicamentCI()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
                vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
                vFPatientContreIndicationListe.ShowDialog() 'Modal
            End Using
            GetContreIndication()
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    'Liste des substances allergiques
    Private Sub ListeDesMedicamentsDeclaresAllergiquesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Click
        ListeMedicamentAllergie()
    End Sub

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        'Traitement : afficher les allergies dans un popup
        If PatientAllergie = True Then
            ListeMedicamentAllergie()
        End If
    End Sub

    Private Sub ListeMedicamentAllergie()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFPatientAllergieListe As New RadFPatientAllergieListe
                vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
                vFPatientAllergieListe.ShowDialog()
            End Using
            GetAllergie()
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    'Déclaration d'une contre-indication
    Private Sub DéclarationContreindicationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DéclarationAllergieOuContreindicationToolStripMenuItem.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
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
                        Dim specialiteContreIndique As SpecialiteContreIndique = theriaqueDao.IsSpecialiteContreIndique(SelectedPatient, SpecialiteId)
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
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        GetContreIndication()
        Me.Enabled = True
    End Sub

    'Déclaration d'une allergie
    Private Sub DéclarationAllergieToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DéclarationAllergieToolStripMenuItem.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
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
                        Dim specialiteAllergique As SpecialiteAllergique = theriaqueDao.IsSpecialiteAllergique(SelectedPatient, SpecialiteId)
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
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        GetAllergie()
        Me.Enabled = True
    End Sub

    'Visualisation de l'historique des actions réalisées sur un traitement
    Private Sub HistoriqueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueToolStripMenuItem.Click
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("traitementId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor

                Try
                    Using vFTraitementHistoListe As New RadFTraitementHistoListe
                        vFTraitementHistoListe.SelectedTraitementId = TraitementId
                        vFTraitementHistoListe.SelectedPatient = Me.SelectedPatient
                        vFTraitementHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFTraitementHistoListe.MedicamentDenomination = RadTraitementDataGridView.Rows(aRow).Cells("medicamentDci").Value
                        vFTraitementHistoListe.ShowDialog() 'Modal
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub

    'Gestion d'une fenêtre thérapeutique pour un traitement donné
    Private Sub GérerUneFenetreTherapeutiqueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GérerUneFenêtreThérapeutiqueToolStripMenuItem.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim SelectedTraitementId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor

                Try
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
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
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

        ParcoursDataTable = parcoursDao.GetAllParcoursbyPatient(SelectedPatient.PatientId)

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
            SpecialiteDescription = Table_specialite.GetSpecialiteDescription(SpecialiteId)
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
                    If pacoursConsigneDao.IsExistParcoursConsigne(ParcoursDataTable.Rows(i)("oa_parcours_id")) = False Then
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
                        Case Tache.EnumDemandeRendezVous.ANNEE.ToString
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("yyyy")
                        Case Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString
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
            RadParcoursDataGridView.CurrentRow = RadParcoursDataGridView.ChildRows(0)
            RadParcoursDataGridView.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub RadParcoursDataGridView_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadParcoursDataGridView.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "consultationNext" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("consultationNextHeure").Value
        End If
    End Sub

    'Créer un inetervenant
    Private Sub RadBtnCreationParcours_Click(sender As Object, e As EventArgs) Handles RadBtnCreationParcours.Click
        CreationIntervenant()
    End Sub

    Private Sub CréerUnIntervenantToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnIntervenantToolStripMenuItem.Click
        CreationIntervenant()
    End Sub

    Private Sub CreationIntervenant()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFSpecialiteSelecteur As New RadFSpecialiteSelecteur
                vFSpecialiteSelecteur.ListProfilOasis = ParcoursListProfilsOasis
                vFSpecialiteSelecteur.ShowDialog()                  'Sélection de spécialité
                If vFSpecialiteSelecteur.SelectedSpecialiteId <> 0 Then

                    Try
                        Using vRadFRorListe As New RadFRorListe
                            vRadFRorListe.Selecteur = True
                            vRadFRorListe.PatientId = Me.SelectedPatient.PatientId
                            vRadFRorListe.SpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                            vRadFRorListe.TypeRor = "Intervenant"
                            vRadFRorListe.ShowDialog()                  'Sélection d'un professionnel de santé
                            If vRadFRorListe.CodeRetour = True Then

                                Try
                                    Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                                        vFParcoursDetailEdit.SelectedParcoursId = 0
                                        vFParcoursDetailEdit.SelectedRorId = vRadFRorListe.SelectedRorId
                                        vFParcoursDetailEdit.SelectedSpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                                        vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                                        'vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                                        vFParcoursDetailEdit.RythmeObligatoire = False
                                        vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                                        vFParcoursDetailEdit.ShowDialog()   'Gestion de l'intervenant
                                    End Using
                                    ChargementParcoursDeSoin()
                                    ChargementPPS()
                                Catch ex As Exception
                                    MessageBox.Show(ex.Message)
                                End Try

                            End If
                        End Using
                    Catch ex As Exception
                        MsgBox(ex.Message())
                    End Try

                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Cursor.Current = Cursors.Default
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
                        vFParcoursDetailEdit.RythmeObligatoire = False
                        vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                        vFParcoursDetailEdit.ShowDialog() 'Modal
                        ChargementParcoursDeSoin()
                        ChargementPPS()
                    End Using
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

                Try
                    Using vRadFParcoursHistoListe As New RadFParcoursHistoListe
                        vRadFParcoursHistoListe.SelectedParcoursId = ParcoursId
                        vRadFParcoursHistoListe.SelectedPatient = Me.SelectedPatient
                        vRadFParcoursHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                        vRadFParcoursHistoListe.ShowDialog()
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadChkParcoursNonCache_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkParcoursNonCache.ToggleStateChanged
        If RadChkParcoursNonCache.Checked = True Then
            RadChkParcoursTous.Checked = False
            If InitParcoursNonCache = True Then
                Application.DoEvents()
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
        RadContexteDataGridView.Rows.Clear()

        Dim contexteDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao
        If RadChkContextePublie.Checked = True Then
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedPatient.PatientId, True)
        Else
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedPatient.PatientId, False)
        End If

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
                Case ContexteCourrier.EnumParcoursBaseCode.Medical
                    categorieContexteString = ContexteCourrier.EnumParcoursBaseItem.Medical
                Case ContexteCourrier.EnumParcoursBaseCode.BioEnvironnemental
                    categorieContexteString = ContexteCourrier.EnumParcoursBaseItem.BioEnvironnemental
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

            Dim contexteDescription As String
            contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
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
            RadContexteDataGridView.CurrentRow = RadContexteDataGridView.ChildRows(0)
            RadContexteDataGridView.TableElement.VScrollBar.Value = 0
        End If

    End Sub

    'Appel détail contexte
    Private Sub MasterTemplate_CellDoubleClick_1(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadContexteDataGridView.CellDoubleClick
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False

                Try
                    Using Form As New RadFContextedetailEdit
                        Form.SelectedContexteId = ContexteId
                        Form.SelectedPatient = Me.SelectedPatient
                        Form.UtilisateurConnecte = Me.UtilisateurConnecte
                        Form.SelectedDrcId = 0
                        Form.PositionGaucheDroite = EnumPosition.Droite
                        Form.ShowDialog()
                        If Form.CodeRetour = True Then
                            ChargementContexte()
                            If Form.ContexteTransformeEnAntecedent = True Then
                                'Rechargement des contextes si réactivation
                                ChargementAntecedent()
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub

    'Création contexte
    Private Sub RadBtnCreationContexte_Click(sender As Object, e As EventArgs) Handles RadBtnCreationContexte.Click
        CreationContexte()
    End Sub

    Private Sub CreerUnContexteMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        CreationContexte()
    End Sub

    Private Sub CreationContexte()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Dim SelectedDrcId As Integer
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFDrcSelecteur As New RadFDRCSelecteur
                vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
                vFDrcSelecteur.CategorieOasis = Drc.EnumCategorieOasisCode.Contexte
                vFDrcSelecteur.ShowDialog()
                SelectedDrcId = vFDrcSelecteur.SelectedDrcId
                'Si un médicament a été sélectionné, on appelle le Formulaire de création
                If SelectedDrcId <> 0 Then

                    Try
                        Using Fom As New RadFContextedetailEdit
                            Fom.SelectedPatient = Me.SelectedPatient
                            Fom.UtilisateurConnecte = Me.UtilisateurConnecte
                            Fom.SelectedDrcId = SelectedDrcId
                            Fom.SelectedContexteId = 0
                            Fom.PositionGaucheDroite = EnumPosition.Droite

                            Fom.ShowDialog()
                            'Si le traitement a été créé, on recharge la grid
                            If Fom.CodeRetour = True Then
                                ChargementContexte()
                            End If
                        End Using
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try

                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    Private Sub HistoriqueDesModificationsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem1.Click
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor

                Try
                    Using Form As New RadFAntecedentHistoListe
                        Form.SelectedAntecedentId = ContexteId
                        Form.SelectedPatient = Me.SelectedPatient
                        Form.UtilisateurConnecte = Me.UtilisateurConnecte
                        Form.ShowDialog()
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadChkContextePublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkContextePublie.ToggleStateChanged
        If RadChkContextePublie.Checked = True Then
            RadChkContexteTous.Checked = False
            If InitContextePublie = True Then
                Application.DoEvents()
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
        RadPPSDataGridView.Rows.Clear()

        Dim PPSDataTable As DataTable
        Dim PPSDao As PpsDao = New PpsDao
        PPSDataTable = PPSDao.getAllPPSbyPatient(SelectedPatient.PatientId, " And (oa_pps_date_fin Is NULL Or oa_pps_date_fin >= '" & Date.Now().ToString("yyyy-MM-dd") & "')")

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i, mesureCount As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateDebut, dateModification As Date
        Dim rowCount As Integer = PPSDataTable.Rows.Count - 1
        Dim categoriePPS, sousCategoriePPS, Rythme, SpecialiteId As Integer
        Dim ppsArret As Boolean
        Dim NaturePPS, CommentairePPS, commentaireParcours, AffichePPS, AfficheDateModificationPPS, AfficheDateModificationParcours, Base, BaseItem, SpecialiteDescription As String

        PPSSuiviIdeExiste = False
        PPSSuiviMedecinExiste = False
        PPSSuiviSageFemmeExiste = False

        RadChkMesureMax.Hide()

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
            If categoriePPS = Pps.EnumCategoriePPS.SUIVI_INTERVENANT Then
                Select Case sousCategoriePPS
                    Case EnumSousCategoriePPS.IDE
                        PPSSuiviIdeExiste = True
                    Case EnumSousCategoriePPS.medecinReferent
                        PPSSuiviMedecinExiste = True
                    Case EnumSousCategoriePPS.sageFemme
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

            AffichePPS = ""
            'Présentation PPS : Cible/Objectif de santé (commentaire)
            If categoriePPS = EnumCategoriePPS.Objectif Then
                NaturePPS = "Objectif santé : "
                AffichePPS = NaturePPS + " " + CommentairePPS
            End If

            If categoriePPS = EnumCategoriePPS.MesurePreventive Then
                mesureCount += 1
                If mesureCount > 2 Then
                    RadChkMesureMax.Show()
                    If RadChkMesureMax.CheckState = False Then
                        Continue For
                    End If
                End If
                'Suivi mesures préventives (Code DRC, libellé DRC, commentaire)
                NaturePPS = "Mesures préventives : "
                AffichePPS = NaturePPS & " " & CommentairePPS
            End If

            SpecialiteDescription = ""
            'Présentation PPS : Suivi
            If categoriePPS = EnumCategoriePPS.Suivi Then
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
                    Case EnumSousCategoriePPS.IDE
                        NaturePPS = "Suivi IDE : "
                    Case EnumSousCategoriePPS.medecinReferent
                        NaturePPS = "Suivi médecin télémédecine : "
                    Case EnumSousCategoriePPS.sageFemme
                        NaturePPS = "Suivi sage-femme : "
                    Case EnumSousCategoriePPS.specialiste
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
            If categoriePPS = EnumCategoriePPS.Strategie Then
                Select Case sousCategoriePPS
                    'TODO: Synthese -> Déclarer ces sous-catégories PPS dans une Enum
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
            RadPPSDataGridView.CurrentRow = RadPPSDataGridView.Rows(0)
            RadPPSDataGridView.TableElement.VScrollBar.Value = 0
        End If
    End Sub


    Private Sub RadChkMesureMax_CheckStateChanged(sender As Object, e As EventArgs) Handles RadChkMesureMax.CheckStateChanged
        Application.DoEvents()
        RadPPSDataGridView.Rows.Clear()
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
                    Case Pps.EnumCategoriePPS.OBJECTIF_SANTE
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False

                        Try
                            Using vRadFPPSObjectifSanteDetail As New RadFPPSDetailEdit
                                vRadFPPSObjectifSanteDetail.PPSId = PPSId
                                vRadFPPSObjectifSanteDetail.CategoriePPS = EnumCategoriePPS.Objectif
                                vRadFPPSObjectifSanteDetail.SelectedPatient = Me.SelectedPatient
                                vRadFPPSObjectifSanteDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                                vRadFPPSObjectifSanteDetail.PositionGaucheDroite = EnumPosition.Droite
                                vRadFPPSObjectifSanteDetail.ShowDialog() 'Modal
                                If vRadFPPSObjectifSanteDetail.CodeRetour = True Then
                                    ChargementPPS()
                                End If
                            End Using
                        Catch ex As Exception
                            MsgBox(ex.Message())
                        End Try

                        Me.Enabled = True
                    Case Pps.EnumCategoriePPS.MESURE_PREVENTIVE
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False

                        Try
                            Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
                                vFFPPSMesurePreventive.PPSId = PPSId
                                vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
                                vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
                                vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
                                vFFPPSMesurePreventive.PositionGaucheDroite = EnumPosition.Droite
                                vFFPPSMesurePreventive.ShowDialog() 'Modal
                                If vFFPPSMesurePreventive.CodeRetour = True Then
                                    ChargementPPS()
                                End If
                            End Using
                        Catch ex As Exception
                            MsgBox(ex.Message())
                        End Try

                        Me.Enabled = True
                    Case Pps.EnumCategoriePPS.SUIVI_INTERVENANT
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False

                        Try
                            Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                                vFParcoursDetailEdit.SelectedParcoursId = ParcoursId
                                vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                                'vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                                vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                                vFParcoursDetailEdit.ShowDialog() 'Modal
                                If vFParcoursDetailEdit.CodeRetour = True Then
                                    ChargementParcoursDeSoin()
                                    ChargementPPS()
                                End If
                            End Using
                        Catch ex As Exception
                            MsgBox(ex.Message())
                        End Try

                        Me.Enabled = True
                    Case Pps.EnumCategoriePPS.STRATEGIE
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False

                        Try
                            Using vFPPSStrategie As New RadFPPSDetailEdit
                                vFPPSStrategie.PPSId = PPSId
                                vFPPSStrategie.CategoriePPS = EnumCategoriePPS.Strategie
                                vFPPSStrategie.SelectedPatient = Me.SelectedPatient
                                vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
                                vFPPSStrategie.PositionGaucheDroite = EnumPosition.Droite
                                vFPPSStrategie.ShowDialog() 'Modal
                                If vFPPSStrategie.CodeRetour = True Then
                                    ChargementPPS()
                                End If
                            End Using
                        Catch ex As Exception
                            MsgBox(ex.Message())
                        End Try

                        Me.Enabled = True
                End Select
            End If
        End If
    End Sub

    'Creation objectif de santé
    Private Sub RadBtnCreationPPSObjectif_Click(sender As Object, e As EventArgs) Handles RadBtnCreationPPSObjectif.Click
        CreationObjectifSante()
    End Sub


    Private Sub CréerUnObjectifDeSantéToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnObjectifDeSantéToolStripMenuItem.Click
        CreationObjectifSante()
    End Sub

    Private Sub CreationObjectifSante()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        'Contrôler si un objectif de santé valide existe
        Dim ppsdao As New PpsDao
        If ppsdao.ExistPPSObjectifByPatientId(SelectedPatient.PatientId) = False Then
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor

            Try
                Using vRadFPPSObjectifSanteDetail As New RadFPPSDetailEdit
                    vRadFPPSObjectifSanteDetail.PPSId = 0
                    vRadFPPSObjectifSanteDetail.CategoriePPS = EnumCategoriePPS.Objectif
                    vRadFPPSObjectifSanteDetail.SelectedPatient = Me.SelectedPatient
                    vRadFPPSObjectifSanteDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                    vRadFPPSObjectifSanteDetail.PositionGaucheDroite = EnumPosition.Droite
                    vRadFPPSObjectifSanteDetail.ShowDialog() 'Modal
                    If vRadFPPSObjectifSanteDetail.CodeRetour = True Then
                        ChargementPPS()
                    End If
                End Using
            Catch ex As Exception
                MsgBox(ex.Message())
            End Try

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
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
                vFFPPSMesurePreventive.PPSId = 0
                vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
                vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
                vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
                vFFPPSMesurePreventive.PositionGaucheDroite = EnumPosition.Droite
                vFFPPSMesurePreventive.ShowDialog() 'Modal
                If vFFPPSMesurePreventive.CodeRetour = True Then
                    ChargementPPS()
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

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
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFPPSStrategie As New RadFPPSDetailEdit
                vFPPSStrategie.PPSId = 0
                vFPPSStrategie.CategoriePPS = EnumCategoriePPS.Strategie
                vFPPSStrategie.SelectedPatient = Me.SelectedPatient
                vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPPSStrategie.PositionGaucheDroite = EnumPosition.Droite
                vFPPSStrategie.ShowDialog() 'Modal
                If vFPPSStrategie.CodeRetour = True Then
                    ChargementPPS()
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

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
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
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

                    Try
                        Using vRadFParcoursHistoListe As New RadFParcoursHistoListe
                            vRadFParcoursHistoListe.SelectedParcoursId = ParcoursId
                            vRadFParcoursHistoListe.SelectedPatient = Me.SelectedPatient
                            vRadFParcoursHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                            vRadFParcoursHistoListe.ShowDialog()
                        End Using
                    Catch ex As Exception
                        MsgBox(ex.Message())
                    End Try

                    Me.Enabled = True
                Else
                    Dim PPSId As Integer = RadPPSDataGridView.Rows(aRow).Cells("ppsId").Value
                    Me.Enabled = False
                    Cursor.Current = Cursors.WaitCursor

                    Try
                        Using vFPPSHistoListe As New RadFPPSHistoListe
                            vFPPSHistoListe.SelectedPPSId = PPSId
                            vFPPSHistoListe.SelectedPatient = Me.SelectedPatient
                            vFPPSHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFPPSHistoListe.ShowDialog()
                        End Using
                    Catch ex As Exception
                        MsgBox(ex.Message())
                    End Try

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

    'Initialisation de l'écran
    Private Sub InitZones()
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
        InitContexteBioPublie = False
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

        RadBtnImprimer.Enabled = SelectedPatient.BlocageMedical
        BtnMail.Enabled = SelectedPatient.BlocageMedical
    End Sub

    Private Sub GestionDroitsAcces()
        'Accès à la création d'un traitement via le TraitementContexteMenuStrip selon les droits de l'utilisateur connecté
        If userLog.UtilisateurNiveauAcces = 1 Or userLog.UtilisateurAdmin = True Then
            CréerUnTraitementToolStripMenuItem1.Enabled = True
        Else
            CréerUnTraitementToolStripMenuItem1.Enabled = False
        End If

        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
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

    Private Sub RadFSynthese_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Environnement.ControleAccesForm.RemoveFormToControl(EnumForm.SYNTHESE.ToString)
    End Sub

    Private Sub BtnMail_Click(sender As Object, e As EventArgs) Handles BtnMail.Click
        ' -- 1) creation du tableau de byte représentant l'ordonnance en pdf
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Dim tblByte As Byte()
        Try
            Dim printPdf As New PrtSynthese
            printPdf.SelectedPatient = SelectedPatient
            tblByte = printPdf.ExportDocumenttoPdfBytes()
        Catch ex As Exception
            MessageBox.Show(ex.Message())
            Me.Enabled = True
            Return
        Finally
            Cursor.Current = Cursors.Default
        End Try

        Dim mailOasis As New MailOasis(ParametreMail.TypeMailParams.SYNTHESE, SelectedPatient)
        mailOasis.Contenu = tblByte
        mailOasis.Filename = "SynthesePatient.pdf"

        ' -- 2) lancement du formulaire de choix du destinataire
        Try
            Cursor.Current = Cursors.WaitCursor
            Using frm = New FrmMailSousEpisodeOuSynthese(SelectedPatient, Nothing, mailOasis)
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    Private Sub AfficherLesPPSObsolètesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AfficherLesPPSObsolètesToolStripMenuItem.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFPPSObsoletes As New RadFPPSObsoletes
                vFPPSObsoletes.SelectedPatient = Me.SelectedPatient
                vFPPSObsoletes.UtilisateurConnecte = userLog
                vFPPSObsoletes.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnSocial_Click(sender As Object, e As EventArgs) Handles RadBtnSocial.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFPatientNoteListe As New RadFPatientNoteListe
                vFPatientNoteListe.TypeNote = EnumTypeNote.Social
                vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.PatientId
                vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
                vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientNoteListe.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnVaccins_Click(sender As Object, e As EventArgs) Handles RadBtnVaccins.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Using radFCPV As New RadFCPV
            radFCPV.Patient = SelectedPatient
            radFCPV.ShowDialog()
            'ChargementValence()
        End Using

        Me.Enabled = True
    End Sub

    Private Sub RadBtnNotesMedicales_Click(sender As Object, e As EventArgs) Handles RadBtnNotesMedicales.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFPatientNoteListe As New RadFPatientNoteListe
                vFPatientNoteListe.TypeNote = EnumTypeNote.Medicale
                vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.PatientId
                vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
                vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientNoteListe.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnDirectives_Click(sender As Object, e As EventArgs) Handles RadBtnDirectives.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFPatientNoteListe As New RadFPatientNoteListe
                vFPatientNoteListe.TypeNote = EnumTypeNote.Directive
                vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.PatientId
                vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
                vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientNoteListe.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    'Imprimer la synthèse du patient
    Private Sub RadBtnImprimer_Click(sender As Object, e As EventArgs) Handles RadBtnImprimer.Click
        Cursor.Current = Cursors.WaitCursor

        Try
            Dim print As New PrtSynthese With {
                .SelectedPatient = SelectedPatient
            }
            print.PrintDocument()
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Cursor.Current = Cursors.Default
    End Sub

    'Ligne de vie
    Private Sub RadBtnLigneDeVie_Click(sender As Object, e As EventArgs) Handles RadBtnLigneDeVie.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vadFEpisodeListe As New RadFEpisodeLigneDeVie
                vadFEpisodeListe.SelectedPatient = Me.SelectedPatient
                vadFEpisodeListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vadFEpisodeListe.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

    'Gestion épisode en cours ou création nouvel épisode
    Private Sub RadBtnEpisode_Click(sender As Object, e As EventArgs) Handles RadBtnEpisode.Click
        Dim episodeDao As New EpisodeDao
        Dim episode As Episode

        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Me.IsRendezVousCloture = EpisodeUtils.CallEpisode(SelectedPatient, RendezVousId, userLog)
        Me.Enabled = True
        episode = episodeDao.GetEpisodeEnCoursByPatientId(Me.SelectedPatient.PatientId)
        If episode.Id <> 0 Then
            RadBtnEpisode.ForeColor = Color.Red
            RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Bold)
            Dim TypeActiviteEpisode As String
            TypeActiviteEpisode = episodeDao.GetItemTypeActiviteByCode(episode.TypeActivite)
            ToolTip.SetToolTip(RadBtnEpisode, "Un épisode de type : " & episode.Type & " " & TypeActiviteEpisode & " est en cours pour ce patient")
        Else
            RadBtnEpisode.ForeColor = Color.FromArgb(21, 66, 139)
            RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Regular)
            ToolTip.SetToolTip(RadBtnEpisode, "")
        End If
    End Sub

    Private Sub RadBtnRefresh_Click(sender As Object, e As EventArgs) Handles RadBtnRefresh.Click
        Cursor.Current = Cursors.WaitCursor

        ChargementEtatCivil()
        ChargementAntecedent()
        ChargementTraitement()
        ChargementParcoursDeSoin()
        ChargementContexte()
        ChargementPPS()
        ControleExistenceEpisode()

        Cursor.Current = Cursors.Default
    End Sub

    'Sortie de l'écran
    Private Sub RadButtonAbandon_Click(sender As Object, e As EventArgs) Handles RadButtonAbandon.Click
        Close()
    End Sub
End Class
