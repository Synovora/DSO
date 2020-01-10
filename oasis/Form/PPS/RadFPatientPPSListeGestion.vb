Imports System.Configuration
Imports System.Data.SqlClient
Imports Telerik.WinControls.UI

Public Class RadFPatientPPSListeGestion
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur

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

    Dim conxn As New SqlConnection(getConnectionString())
    Dim ObjectifExiste As Boolean = False
    Dim PPSHistoACreer As New PpsHisto
    Dim PPSObjectifId As Integer
    Dim SuiviSpecialiteExistante As New HashSet(Of Integer)
    'Paramètre application
    Dim Organisation As String
    Dim drcdao As New DrcDao


    Private Sub RadFPatientPPSListeGestion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementParametreApplication()
        ChargementEtatCivil()
        ChargementObjectifSante()
        ChargementPrevention()
        ChargementSuivi()
        ChargementStrategie()
    End Sub

    Private Sub ChargementParametreApplication()
        'Récupération du nom de l'organisation dans les paramètres de l'application
        Organisation = ConfigurationManager.AppSettings("organisation")
        If Organisation = "" Then
            Organisation = "Oasis"
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
            ToolTipPPS.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub


    '==========================================================
    '======================= Objectif santé ===================
    '==========================================================

    Private Sub ChargementObjectifSante()
        Dim ppsdao As New PpsDao()
        Dim pps As Pps

        pps = ppsdao.getPpsObjectifByPatientId(SelectedPatient.patientId)

        PPSObjectifId = 0
        ObjectifExiste = True
        PPSObjectifId = pps.Id
        TxtObjectifCommentaire.Text = pps.Commentaire
        TxtObjectifCodeDrc.Text = ""
        TxtObjectifCodeDrc.Text = pps.DrcId
        Dim Drc As Drc = New Drc()
        If drcdao.GetDrc(Drc, TxtObjectifCodeDrc.Text) = True Then
            TxtObjectifDrcLibelle.Text = Drc.DrcLibelle
        Else
            TxtObjectifDrcLibelle.Text = ""
        End If

        RadBtnValidationObjectif.Hide()

    End Sub

    Private Sub TxtObjectifCodeDrc_DoubleClick(sender As Object, e As EventArgs) Handles TxtObjectifCodeDrc.DoubleClick
        SelectDrc()
    End Sub

    'Appel Sélecteur DRC/ORC
    Private Sub RadBtnSelectionDrcObjectif_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionDrcObjectif.Click
        SelectDrc()
    End Sub

    Private Sub SelectDrc()
        'Appel du sélecteur de code DRC
        Using vFDrcSelecteur As New FDrcSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = 4 'Catégorie Oasis "Objectif"
            vFDrcSelecteur.ShowDialog() 'Modal
            Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
            'Si un médicament a été sélectionné
            If SelectedDrcId <> 0 Then
                TxtObjectifCodeDrc.Text = SelectedDrcId
                Dim Drc As Drc = New Drc()
                If DrcDao.GetDrc(Drc, SelectedDrcId) = True Then
                    TxtObjectifDrcLibelle.Text = Drc.DrcLibelle
                Else
                    TxtObjectifDrcLibelle.Text = ""
                End If
            End If
        End Using
    End Sub

    Private Sub RadBtnInitDrcObjectif_Click(sender As Object, e As EventArgs) Handles RadBtnInitDrcObjectif.Click
        'Contrôle que le code saisi est valide
        If ObjectifExiste = True Then
            'Modification
            ModificationObjectif()
        Else
            'Création
            CreationObjectif()
        End If
        RadBtnValidationObjectif.Hide()
    End Sub

    'Validation des données saisies
    Private Sub RadBtnValidationObjectif_Click(sender As Object, e As EventArgs) Handles RadBtnValidationObjectif.Click
        'Contrôle que le code saisi est valide
        If ObjectifExiste = True Then
            'Modification
            ModificationObjectif()
        Else
            'Création
            CreationObjectif()
        End If
        RadBtnValidationObjectif.Hide()
    End Sub

    Private Function CreationObjectif() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "insert into oasis.oa_patient_pps (oa_pps_patient_id, oa_pps_categorie, oa_pps_sous_categorie, oa_pps_specialite, oa_pps_drc_id, oa_pps_commentaire, oa_pps_utilisateur_creation, oa_pps_date_creation, oa_pps_inactif, oa_pps_affichage_synthese) VALUES (@patientId, @categorie, @sousCategorie, @specialite, @drcId, @commentaire, @utilisateurCreation, @dateCreation, @inactif, @affichageSynthese)"
        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
            .AddWithValue("@categorie", 1)
            .AddWithValue("@sousCategorie", 1)
            .AddWithValue("@specialite", 0)
            .AddWithValue("@drcId", TxtObjectifCodeDrc.Text)
            .AddWithValue("@commentaire", TxtObjectifCommentaire.Text)
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateCreation", Date.Now.ToString)
            .AddWithValue("@inactif", 0)
            .AddWithValue("@affichageSynthese", 1)
        End With

        Try
            conxn.Open()
            Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
            MessageBox.Show("Objectif santé créé")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation antecedent
            PPSHistoACreer.HistorisationDate = DateTime.Now()
            PPSHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            PPSHistoACreer.HistorisationEtat = EnumEtatPPSHisto.Creation
            PPSHistoACreer.PatientId = SelectedPatient.patientId.ToString
            PPSHistoACreer.Categorie = 1
            PPSHistoACreer.SousCategorie = 1
            PPSHistoACreer.Inactif = 0
            PPSHistoACreer.AffichageSynthese = 1
            PPSHistoACreer.DrcId = TxtObjectifCodeDrc.Text
            PPSHistoACreer.Commentaire = TxtObjectifCommentaire.Text

            'Récupération de l'identifiant du antecedent créé
            Dim PPSLastDataReader As SqlDataReader
            SQLstring = "select max(oa_pps_id) from oasis.oa_patient_pps where oa_pps_patient_id = " & SelectedPatient.patientId & ";"
            Dim PPSLastCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            PPSLastDataReader = PPSLastCommand.ExecuteReader()
            If PPSLastDataReader.HasRows Then
                PPSLastDataReader.Read()
                'Récupération de la clé de l'enregistrement créé
                PPSHistoACreer.PpsId = PPSLastDataReader(0)

                'Libération des ressources d'accès aux données
                conxn.Close()
                PPSLastCommand.Dispose()
            End If

            'Lecture du PPS créé avec toutes ses données pour communiquer le DataReader à la fonction dédiée
            Dim PPSCreeDataReader As SqlDataReader
            SQLstring = "Select * from oasis.oa_patient_pps where oa_pps_id = " & PPSHistoACreer.PpsId & ";"
            Dim antecedentCreeCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            PPSCreeDataReader = antecedentCreeCommand.ExecuteReader()
            If PPSCreeDataReader.Read() Then
                'Initialisation classe Historisation PPS
                InitClassePPStHistorisation(PPSCreeDataReader, UtilisateurConnecte, PPSHistoACreer)

                'Libération des ressources d'accès aux données
                conxn.Close()
                antecedentCreeCommand.Dispose()
            End If

            'Création dans l'historique des antecedents du antecedent créé
            CreationPPSHisto(PPSHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Return codeRetour

    End Function

    Private Function ModificationObjectif() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_pps set oa_pps_date_modification = @dateModification, oa_pps_utilisateur_modification = @utilisateurModification, oa_pps_drc_id = @drcId, oa_pps_commentaire = @commentaire where oa_pps_id = @ppsId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@commentaire", TxtObjectifCommentaire.Text)
            .AddWithValue("@drcId", TxtObjectifCodeDrc.Text)
            .AddWithValue("@ppsId", PPSObjectifId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Objectif santé modifié")
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            PPSHistoACreer.HistorisationDate = Date.Now()
            PPSHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            PPSHistoACreer.HistorisationEtat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent
            PPSHistoACreer.Categorie = 1
            PPSHistoACreer.SousCategorie = 1
            PPSHistoACreer.Inactif = 0
            PPSHistoACreer.AffichageSynthese = 1
            PPSHistoACreer.PatientId = SelectedPatient.patientId.ToString
            PPSHistoACreer.PpsId = PPSObjectifId.ToString
            PPSHistoACreer.Commentaire = TxtObjectifCommentaire.Text
            PPSHistoACreer.DrcId = TxtObjectifCodeDrc.Text

            'Création dans l'historique des modifications de l'antecedent
            CreationPPSHisto(PPSHistoACreer, UtilisateurConnecte, EnumEtatPPSHisto.Modification)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Return codeRetour
    End Function

    Private Sub TxtObjectifCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtObjectifCommentaire.TextChanged
        RadBtnValidationObjectif.Show()
    End Sub

    Private Sub TxtObjectifCodeDrc_TextChanged(sender As Object, e As EventArgs) Handles TxtObjectifCodeDrc.TextChanged
        RadBtnValidationObjectif.Show()
    End Sub

    'Historique des modifications
    Private Sub JHistoriqueDesModificationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JHistoriqueDesModificationsToolStripMenuItem.Click
        'TODO: Appel de l'historique des modifications de l'objectif santé

    End Sub


    '==========================================================
    '======================= Préventions ======================
    '==========================================================

    Private Sub ChargementPrevention()
        'Déclaration des données de connexion
        Dim ppsdao As PpsDao = New PpsDao()
        Dim PreventionDataTable As DataTable = New DataTable()
        PreventionDataTable = ppsdao.getAllPPSPreventionbyPatient(SelectedPatient.patientId)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = PreventionDataTable.Rows.Count - 1
        Dim Priorite, DrcId As Integer
        Dim DrcDescription, Commentaire As String

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1

            If PreventionDataTable.Rows(i)("oa_pps_priorite") IsNot DBNull.Value Then
                Priorite = PreventionDataTable.Rows(i)("oa_pps_priorite")
            Else
                Priorite = 0
            End If

            DrcDescription = ""
            If PreventionDataTable.Rows(i)("oa_pps_drc_id") IsNot DBNull.Value Then
                DrcId = PreventionDataTable.Rows(i)("oa_pps_drc_id")
                Dim Drc As Drc = New Drc(DrcId)
                DrcDescription = Drc.DrcLibelle
            End If

            Commentaire = ""
            If PreventionDataTable.Rows(i)("oa_pps_commentaire") IsNot DBNull.Value Then
                Commentaire = PreventionDataTable.Rows(i)("oa_pps_commentaire")
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadPreventionDataGridView.Rows.Add(iGrid)
            RadPreventionDataGridView.Rows(iGrid).Cells("preventionId").Value = PreventionDataTable.Rows(i)("oa_pps_id")
            RadPreventionDataGridView.Rows(iGrid).Cells("preventionPriorite").Value = Priorite
            RadPreventionDataGridView.Rows(iGrid).Cells("preventionDrcDescription").Value = DrcDescription
            RadPreventionDataGridView.Rows(iGrid).Cells("preventionCommentaire").Value = Commentaire
        Next

        'Positionnement du grid sur la première occurrence
        If RadPreventionDataGridView.Rows.Count > 0 Then
            Me.RadPreventionDataGridView.CurrentRow = RadPreventionDataGridView.ChildRows(0)
        End If
    End Sub

    'Créer une mesure préventive
    Private Sub CréerUneMesurePréventiveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneMesurePréventiveToolStripMenuItem.Click
        Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
            vFFPPSMesurePreventive.PPSId = 0
            vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
            vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
            vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
            vFFPPSMesurePreventive.ShowDialog() 'Modal
            If vFFPPSMesurePreventive.CodeRetour = True Then
                RadPreventionDataGridView.Rows.Clear()
                ChargementPrevention()
            End If
        End Using
    End Sub

    'Appel modification mesure préventive

    Private Sub RadPreventionDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadPreventionDataGridView.CellDoubleClick
        If RadPreventionDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadPreventionDataGridView.Rows.IndexOf(Me.RadPreventionDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim PreventionId As Integer = RadPreventionDataGridView.Rows(aRow).Cells("preventionId").Value
                Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
                    vFFPPSMesurePreventive.PPSId = PreventionId
                    vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
                    vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
                    vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFFPPSMesurePreventive.ShowDialog() 'Modal
                    If vFFPPSMesurePreventive.CodeRetour = True Then
                        RadPreventionDataGridView.Rows.Clear()
                        ChargementPrevention()
                    End If
                End Using
            End If
        End If
    End Sub

    'Historique des modifications des mesures préventives
    Private Sub HistoriqueDesModificationsPreventionToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem.Click
        'TODO: Appel de l'historique des modifications des mesures préventives

    End Sub

    Private Sub RadPreventionDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadPreventionDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing AndAlso (cell.ColumnInfo.Name = "preventionCommentaire") Then
            e.ToolTipText = cell.Value.ToString()
        End If
    End Sub


    '==========================================================
    '======================= Suivi ============================
    '==========================================================

    Private Sub ChargementSuivi()
        'Déclaration des données de connexion
        Dim ppsdao As PpsDao = New PpsDao()
        Dim SuiviDataTable As DataTable = New DataTable()
        SuiviDataTable = ppsdao.getAllPPSSuivibyPatient(SelectedPatient.patientId)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = SuiviDataTable.Rows.Count - 1
        Dim Commentaire, Specialite, Base, BaseDesignation, NomIntervenant, NomStructure, AffichageSyntheseString As String
        Dim SpecialiteId, Rythme As Integer
        Dim AffichageSynthese As Boolean

        SuiviSpecialiteExistante.Clear() 'Vidage de la collection des spécialités actives pour ce patient

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Specialite = "Inconnue"
            Select Case SuiviDataTable.Rows(i)("oa_pps_sous_categorie")
                Case 3
                    Specialite = "IDE"
                Case 4
                    Specialite = "Généraliste"
                Case 5
                    Specialite = "Sage-femme"
                Case 6
                    If SuiviDataTable.Rows(i)("oa_pps_specialite") IsNot DBNull.Value Then
                        SpecialiteId = SuiviDataTable.Rows(i)("oa_pps_specialite")
                        Specialite = Table_specialite.GetSpecialiteDescription(SpecialiteId)
                        'On collecte la spécialité dans un HashSet (ensemble des spécialités existantes pour le patient)
                        SuiviSpecialiteExistante.Add(SpecialiteId)
                    Else
                        SpecialiteId = 0
                    End If
            End Select

            'Les PPS de Suivi qui ne sont pas rythmés (base et rythme) sont affichés dans le Parcours de soin
            If SuiviDataTable.Rows(i)("oa_pps_base") IsNot DBNull.Value Then
                If SuiviDataTable.Rows(i)("oa_pps_base") = "" Then
                    Continue For
                End If
            Else
                Continue For
            End If

            Commentaire = ""
            If SuiviDataTable.Rows(i)("oa_pps_commentaire") IsNot DBNull.Value Then
                Commentaire = SuiviDataTable.Rows(i)("oa_pps_commentaire")
            End If

            NomIntervenant = ""
            Select Case SuiviDataTable.Rows(i)("oa_pps_sous_categorie")
                Case 3, 4
                    NomIntervenant = Organisation
                Case Else
                    If SuiviDataTable.Rows(i)("oa_pps_nom_intervenant") IsNot DBNull.Value Then
                        NomIntervenant = SuiviDataTable.Rows(i)("oa_pps_nom_intervenant")
                    End If
            End Select

            NomStructure = ""
            Select Case SuiviDataTable.Rows(i)("oa_pps_sous_categorie")
                Case 3, 4
                    NomStructure = "Oasis"
                Case Else
                    If SuiviDataTable.Rows(i)("oa_pps_nom_structure") IsNot DBNull.Value Then
                        NomStructure = SuiviDataTable.Rows(i)("oa_pps_nom_structure")
                    End If
            End Select

            BaseDesignation = ""
            If SuiviDataTable.Rows(i)("oa_pps_base") IsNot DBNull.Value Then
                Base = SuiviDataTable.Rows(i)("oa_pps_base")
                Select Case Base
                    Case "J"
                        BaseDesignation = "Journalier"
                    Case "H"
                        BaseDesignation = "Hebdomadaire"
                    Case "M"
                        BaseDesignation = "Mensuel"
                    Case "A"
                        BaseDesignation = "Annuel"
                End Select
            End If

            Rythme = 0
            If SuiviDataTable.Rows(i)("oa_pps_rythme") IsNot DBNull.Value Then
                Rythme = SuiviDataTable.Rows(i)("oa_pps_rythme")
            End If

            AffichageSynthese = False
            If SuiviDataTable.Rows(i)("oa_pps_affichage_synthese") IsNot DBNull.Value Then
                If SuiviDataTable.Rows(i)("oa_pps_affichage_synthese") = 1 Then
                    AffichageSynthese = True
                End If
            Else
                AffichageSynthese = True
            End If
            If AffichageSynthese = True Then
                AffichageSyntheseString = "Oui"
            Else
                AffichageSyntheseString = "Non"
            End If

            'TODO: Récupérer la date du dernier rendez-vous

            'TODO: Récupérer la date du prochain rendez-vous

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadSuiviDataGridView.Rows.Add(iGrid)
            RadSuiviDataGridView.Rows(iGrid).Cells("suiviId").Value = SuiviDataTable.Rows(i)("oa_pps_id")
            RadSuiviDataGridView.Rows(iGrid).Cells("suiviSpecialite").Value = Specialite
            RadSuiviDataGridView.Rows(iGrid).Cells("suiviCommentaire").Value = Commentaire
            RadSuiviDataGridView.Rows(iGrid).Cells("suiviBase").Value = Rythme
            RadSuiviDataGridView.Rows(iGrid).Cells("suiviRythme").Value = BaseDesignation
            RadSuiviDataGridView.Rows(iGrid).Cells("suiviNomIntervenant").Value = NomIntervenant
            RadSuiviDataGridView.Rows(iGrid).Cells("suiviNomStructure").Value = NomStructure
            RadSuiviDataGridView.Rows(iGrid).Cells("suiviAfficheSynthese").Value = AffichageSyntheseString
        Next

        'Positionnement du grid sur la première occurrence
        If RadSuiviDataGridView.Rows.Count > 0 Then
            Me.RadSuiviDataGridView.CurrentRow = RadSuiviDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub RadSuiviDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadSuiviDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing AndAlso (cell.ColumnInfo.Name = "suiviCommentaire") Then
            e.ToolTipText = cell.Value.ToString()
        End If
    End Sub

    Private Sub RadSuiviDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadSuiviDataGridView.CellDoubleClick

    End Sub

    Private Sub CréerUnSuiviToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnSuiviToolStripMenuItem.Click

    End Sub


    Private Sub HistoriqueDesModificationsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem1.Click

    End Sub




    '==========================================================
    '======================= Stratégies =======================
    '==========================================================
    Private Sub ChargementStrategie()
        'Déclaration des données de connexion
        Dim ppsdao As PpsDao = New PpsDao()
        Dim StrategieDataTable As DataTable = New DataTable()
        StrategieDataTable = ppsdao.getAllPPSStrategiePatient(SelectedPatient.patientId)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = StrategieDataTable.Rows.Count - 1
        Dim DrcId, sousCategorie As Integer
        Dim DrcDescription, Commentaire, sousCategorieDescription, DateModificationDescription As String
        Dim DateModification As Date
        Dim StrategieExistante(10) As Integer 'Tableau pour gérer l'unicité des stratégie pat sous-catégorie --> à valider si utile
        Dim Indice As Integer = -1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            DrcDescription = ""
            If StrategieDataTable.Rows(i)("oa_pps_drc_id") IsNot DBNull.Value Then
                DrcId = StrategieDataTable.Rows(i)("oa_pps_drc_id")
                Dim Drc As Drc = New Drc(DrcId)
                DrcDescription = Drc.DrcLibelle
            End If

            sousCategorieDescription = ""
            If StrategieDataTable.Rows(i)("oa_pps_sous_categorie") IsNot DBNull.Value Then
                sousCategorie = StrategieDataTable.Rows(i)("oa_pps_sous_categorie")
                Select Case sousCategorie
                    Case 7
                        sousCategorieDescription = "Prophylactique"
                        Indice += 1
                        StrategieExistante(Indice) = 7
                    Case 8
                        sousCategorieDescription = "Sociale"
                        Indice += 1
                        StrategieExistante(Indice) = 8
                    Case 9
                        sousCategorieDescription = "Symptomatique"
                        Indice += 1
                        StrategieExistante(Indice) = 9
                    Case 10
                        sousCategorieDescription = "Curative"
                        Indice += 1
                        StrategieExistante(Indice) = 10
                    Case 11
                        sousCategorieDescription = "Diagnostique"
                        Indice += 1
                        StrategieExistante(Indice) = 11
                    Case 12
                        sousCategorieDescription = "Palliative"
                        Indice += 1
                        StrategieExistante(Indice) = 12
                End Select
            End If

            Commentaire = ""
            If StrategieDataTable.Rows(i)("oa_pps_commentaire") IsNot DBNull.Value Then
                Commentaire = StrategieDataTable.Rows(i)("oa_pps_commentaire")
            End If


            If StrategieDataTable.Rows(i)("oa_pps_date_modification") IsNot DBNull.Value Then
                DateModification = StrategieDataTable.Rows(i)("oa_pps_date_modification")
                DateModificationDescription = FormatageDateAffichage(DateModification)
            Else
                If StrategieDataTable.Rows(i)("oa_pps_date_creation") IsNot DBNull.Value Then
                    DateModification = StrategieDataTable.Rows(i)("oa_pps_date_creation")
                    DateModificationDescription = FormatageDateAffichage(DateModification)
                Else
                    DateModificationDescription = "Date inconnue"
                End If
            End If


            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadStrategieDataGridView.Rows.Add(iGrid)
            RadStrategieDataGridView.Rows(iGrid).Cells("strategieId").Value = StrategieDataTable.Rows(i)("oa_pps_id")
            RadStrategieDataGridView.Rows(iGrid).Cells("strategieDate").Value = DateModificationDescription
            RadStrategieDataGridView.Rows(iGrid).Cells("strategieSousCategorie").Value = sousCategorieDescription
            RadStrategieDataGridView.Rows(iGrid).Cells("strategieDrcDescription").Value = DrcDescription
            RadStrategieDataGridView.Rows(iGrid).Cells("strategieCommentaire").Value = Commentaire
        Next

        'Positionnement du grid sur la première occurrence
        If RadStrategieDataGridView.Rows.Count > 0 Then
            Me.RadStrategieDataGridView.CurrentRow = RadStrategieDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub RadStrategieDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadStrategieDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing AndAlso (cell.ColumnInfo.Name = "strategieCommentaire") Then
            e.ToolTipText = cell.Value.ToString()
        End If
    End Sub


    Private Sub CréerUneStratégieContextuelleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneStratégieContextuelleToolStripMenuItem.Click
        Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
            vFFPPSMesurePreventive.PPSId = 0
            vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.Strategie
            vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
            vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
            vFFPPSMesurePreventive.ShowDialog() 'Modal
            If vFFPPSMesurePreventive.CodeRetour = True Then
                RadPreventionDataGridView.Rows.Clear()
                ChargementPrevention()
            End If
        End Using
    End Sub

    Private Sub RadStrategieDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadStrategieDataGridView.CellDoubleClick
        If RadStrategieDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadStrategieDataGridView.Rows.IndexOf(Me.RadStrategieDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim StrategieId As Integer = RadStrategieDataGridView.Rows(aRow).Cells("strategieId").Value
                Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
                    vFFPPSMesurePreventive.PPSId = StrategieId
                    vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.Strategie
                    vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
                    vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFFPPSMesurePreventive.ShowDialog() 'Modal
                    If vFFPPSMesurePreventive.CodeRetour = True Then
                        RadPreventionDataGridView.Rows.Clear()
                        ChargementPrevention()
                    End If
                End Using
            End If
        End If
    End Sub

    Private Sub HistoriqueDesModificationsToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem2.Click

    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub

End Class
