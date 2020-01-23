Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common

Public Class FPatientPPSListeGestion
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
    Dim drcdao As New DrcDao


    Private Sub FPPSGestion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        ChargementObjectifSante()
        ChargementPrevention()
        ChargementSuivi()
        ChargementStrategie()
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
            ToolTipPPS.SetToolTip(LblALD, StringTooltip)
        End If
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
    '======================= Objectif santé ===================
    '==========================================================

    Private Sub ChargementObjectifSante()
        Dim ObjectifDateReader As SqlDataReader
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String = "select * from oasis.oa_patient_pps where oa_pps_categorie = 1 and oa_pps_sous_categorie = 1 and oa_pps_patient_id = " + SelectedPatient.patientId.ToString + ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        PPSObjectifId = 0
        conxn.Open()
        ObjectifDateReader = myCommand.ExecuteReader()
        If ObjectifDateReader.Read() Then
            ObjectifExiste = True
            PPSObjectifId = ObjectifDateReader("oa_pps_id")
            TxtObjectifCommentaire.Text = ""
            If ObjectifDateReader("oa_pps_commentaire") IsNot DBNull.Value Then
                TxtObjectifCommentaire.Text = ObjectifDateReader("oa_pps_commentaire")
            End If

            TxtObjectifCodeDrc.Text = ""
            If ObjectifDateReader("oa_pps_drc_id") IsNot DBNull.Value Then
                TxtObjectifCodeDrc.Text = ObjectifDateReader("oa_pps_drc_id")
                Dim Drc As Drc = New Drc()
                If drcdao.GetDrc(Drc, TxtObjectifCodeDrc.Text) = True Then
                    TxtObjectifDrcLibelle.Text = Drc.DrcLibelle
                Else
                    TxtObjectifDrcLibelle.Text = ""
                End If
            End If
        End If

        BtnValidationObjectif.Hide()

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()
    End Sub

    Private Sub TxtObjectifCodeDrc_DoubleClick(sender As Object, e As EventArgs) Handles TxtObjectifCodeDrc.DoubleClick
        'Appel du sélecteur de code DRC
        Dim vFDrcSelecteur As New FDrcSelecteur
        vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
        vFDrcSelecteur.CategorieOasis = 4 'Catégorie Oasis "Objectif"
        vFDrcSelecteur.ShowDialog() 'Modal

        Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
        vFDrcSelecteur.Dispose()
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
    End Sub

    'Appel Sélecteur DRC/ORC
    Private Sub BtnSelectionDrcObjectif_Click(sender As Object, e As EventArgs) Handles BtnSelectionDrcObjectif.Click
        Dim vFDrcSelecteur As New FDrcSelecteur
        vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
        vFDrcSelecteur.CategorieOasis = 4 'Catégorie Oasis "Objectif"
        vFDrcSelecteur.ShowDialog() 'Modal
        Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
        vFDrcSelecteur.Dispose()
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
    End Sub

    'Initialiser le code DRC/ORC existant
    Private Sub BtnInitDrcObjectif_Click(sender As Object, e As EventArgs) Handles BtnInitDrcObjectif.Click
        TxtObjectifCodeDrc.Text = ""
        TxtObjectifDrcLibelle.Text = ""
    End Sub

    'Validation des données saisies
    Private Sub BtnValidationObjectif_Click(sender As Object, e As EventArgs) Handles BtnValidationObjectif.Click
        'Contrôle que le code saisi est valide
        If ObjectifExiste = True Then
            'Modification
            ModificationObjectif()
        Else
            'Création
            CreationObjectif()
        End If
        BtnValidationObjectif.Hide()
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

            'Lecture de l'antecedent créé avec toutes ses données pour communiquer le DataReader à la fonction dédiée
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
        End If

        Return codeRetour
    End Function

    'Historique des modifications
    Private Sub HistoriqueDesModificationsObjectifToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JHistoriqueDesModificationsToolStripMenuItem.Click
        'TODO: Appel de l'historique des modifications de l'objectif santé

    End Sub

    Private Sub TxtObjectifCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtObjectifCommentaire.TextChanged
        BtnValidationObjectif.Show()
    End Sub

    Private Sub TxtObjectifCodeDrc_TextChanged(sender As Object, e As EventArgs) Handles TxtObjectifCodeDrc.TextChanged
        BtnValidationObjectif.Show()
    End Sub


    '==========================================================
    '======================= Préventions ======================
    '==========================================================

    Private Sub ChargementPrevention()
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim PreventionDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim PreventionDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0) and oa_pps_categorie = 2 and oa_pps_sous_categorie = 2 and oa_pps_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_pps_priorite;"

        'Lecture des données en base
        PreventionDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        PreventionDataAdapter.Fill(PreventionDataTable)
        conxn.Open()

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
            PreventionDataGridView.Rows.Insert(iGrid)
            PreventionDataGridView("preventionId", iGrid).Value = PreventionDataTable.Rows(i)("oa_pps_id")
            PreventionDataGridView("preventionPriorite", iGrid).Value = Priorite
            PreventionDataGridView("preventionDrcDescription", iGrid).Value = DrcDescription
            PreventionDataGridView("preventionCommentaire", iGrid).Value = Commentaire
        Next
        conxn.Close()
        PreventionDataAdapter.Dispose()

        'Enlève le focus sur la première ligne
        PreventionDataGridView.ClearSelection()
    End Sub

    'Créer une mesure préventive
    Private Sub CréerUneMesurePréventiveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneMesurePréventiveToolStripMenuItem.Click
        Dim vFFPPSMesurePreventive As New FPPSMesurePreventive
        vFFPPSMesurePreventive.PreventionId = 0
        vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
        vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
        vFFPPSMesurePreventive.ShowDialog() 'Modal
        If vFFPPSMesurePreventive.CodeRetour = True Then
            PreventionDataGridView.Rows.Clear()
            ChargementPrevention()
        End If
        vFFPPSMesurePreventive.Dispose()
    End Sub

    'Appel modification mesure préventive
    Private Sub PreventionDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles PreventionDataGridView.CellDoubleClick
        If PreventionDataGridView.CurrentRow IsNot Nothing Then
            Dim PreventionId As Integer = PreventionDataGridView.Rows(PreventionDataGridView.CurrentRow.Index).Cells("preventionId").Value
            Dim vFFPPSMesurePreventive As New FPPSMesurePreventive
            vFFPPSMesurePreventive.PreventionId = PreventionId
            vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
            vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
            vFFPPSMesurePreventive.ShowDialog() 'Modal
            If vFFPPSMesurePreventive.CodeRetour = True Then
                PreventionDataGridView.Rows.Clear()
                ChargementPrevention()
            End If
            vFFPPSMesurePreventive.Dispose()
        End If
    End Sub

    'Historique des modifications des mesures préventives
    Private Sub HistoriqueDesModificationsPreventionToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem.Click
        'TODO: Appel de l'historique des modifications des mesures préventives

    End Sub

    '==========================================================
    '======================= Suivi ============================
    '==========================================================

    Private Sub ChargementSuivi()
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SuiviDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim SuiviDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0) and (oa_pps_affichage_synthese is Null or oa_pps_affichage_synthese = 1) and oa_pps_categorie = 2 and oa_pps_sous_categorie <> 2 and oa_pps_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_pps_sous_categorie;"

        'Lecture des données en base
        SuiviDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        SuiviDataAdapter.Fill(SuiviDataTable)
        conxn.Open()

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
            If SuiviDataTable.Rows(i)("oa_pps_nom_intervenant") IsNot DBNull.Value Then
                NomIntervenant = SuiviDataTable.Rows(i)("oa_pps_nom_intervenant")
            End If

            NomStructure = ""
            If SuiviDataTable.Rows(i)("oa_pps_nom_structure") IsNot DBNull.Value Then
                NomStructure = SuiviDataTable.Rows(i)("oa_pps_nom_structure")
            End If

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
            SuiviDataGridView.Rows.Insert(iGrid)
            SuiviDataGridView("suiviId", iGrid).Value = SuiviDataTable.Rows(i)("oa_pps_id")
            SuiviDataGridView("suiviSpecialite", iGrid).Value = Specialite
            SuiviDataGridView("suiviCommentaire", iGrid).Value = Commentaire
            SuiviDataGridView("suiviBase", iGrid).Value = BaseDesignation
            SuiviDataGridView("suiviRythme", iGrid).Value = Rythme
            SuiviDataGridView("suiviNomIntervenant", iGrid).Value = NomIntervenant
            SuiviDataGridView("suiviNomStructure", iGrid).Value = NomStructure
            SuiviDataGridView("suiviAfficheSynthese", iGrid).Value = AffichageSyntheseString
        Next
        conxn.Close()
        SuiviDataAdapter.Dispose()

        'Enlève le focus sur la première ligne
        SuiviDataGridView.ClearSelection()
    End Sub

    'Créer un suivi
    Private Sub CréerUnSuiviToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnSuiviToolStripMenuItem.Click

    End Sub

    'Appel modification suivi
    Private Sub SuiviDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles SuiviDataGridView.CellDoubleClick

    End Sub

    'Appel de l'historique des modifications de suivi
    Private Sub HistoriqueDesModificationsSuiviToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem1.Click

    End Sub

    '==========================================================
    '======================= Stratégies =======================
    '==========================================================

    Private Sub ChargementStrategie()
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim StrategieDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim StrategieDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_pps where (oa_pps_inactif is Null or oa_pps_inactif = 0) and oa_pps_categorie = 3 and oa_pps_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_pps_priorite;"

        'Lecture des données en base
        StrategieDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        StrategieDataAdapter.Fill(StrategieDataTable)
        conxn.Open()

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
            StrategieDataGridView.Rows.Insert(iGrid)
            StrategieDataGridView("strategieId", iGrid).Value = StrategieDataTable.Rows(i)("oa_pps_id")
            StrategieDataGridView("strategieDate", iGrid).Value = DateModificationDescription
            StrategieDataGridView("strategieSousCategorie", iGrid).Value = sousCategorieDescription
            StrategieDataGridView("strategieDrcDescription", iGrid).Value = DrcDescription
            StrategieDataGridView("strategieCommentaire", iGrid).Value = Commentaire
        Next
        conxn.Close()
        StrategieDataAdapter.Dispose()

        'Enlève le focus sur la première ligne
        StrategieDataGridView.ClearSelection()
    End Sub

    'Créer une stratégie
    Private Sub CréerUneStratégieContextuelleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneStratégieContextuelleToolStripMenuItem.Click
        Dim vFPPSStrategie As New FPPSStrategie
        vFPPSStrategie.StrategieId = 0
        vFPPSStrategie.SelectedPatient = Me.SelectedPatient
        vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
        vFPPSStrategie.ShowDialog() 'Modal
        If vFPPSStrategie.CodeRetour = True Then
            StrategieDataGridView.Rows.Clear()
            ChargementStrategie()
        End If
        vFPPSStrategie.Dispose()
    End Sub

    'Appel modification stratégie
    Private Sub StrategieDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles StrategieDataGridView.CellDoubleClick
        If StrategieDataGridView.CurrentRow IsNot Nothing Then
            Dim StrategieId As Integer = StrategieDataGridView.Rows(StrategieDataGridView.CurrentRow.Index).Cells("strategieId").Value
            Dim vFPPSStrategie As New FPPSStrategie
            vFPPSStrategie.StrategieId = StrategieId
            vFPPSStrategie.SelectedPatient = Me.SelectedPatient
            vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPPSStrategie.ShowDialog() 'Modal
            If vFPPSStrategie.CodeRetour = True Then
                StrategieDataGridView.Rows.Clear()
                ChargementStrategie()
            End If
            vFPPSStrategie.Dispose()
        End If
    End Sub

    'Appel historique des modifications des stratégies
    Private Sub HistoriqueDesModificationsStrategieToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem2.Click

    End Sub

End Class