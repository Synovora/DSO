'Affichage des allergies ou des contre_indications d'un patient

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports Oasis_Common

Public Class FTraitementAllergieEtCI

    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateAllergieOuContreIndication As EnumAllergieOuContreIndication



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

    Public Property AllergieOuContreIndication As Integer
        Get
            Return privateAllergieOuContreIndication
        End Get
        Set(value As Integer)
            privateAllergieOuContreIndication = value
        End Set
    End Property

    Private Sub FTraitementGestion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initZones()
        ChargementEtatCivil()
        ChargementTraitement()
    End Sub

    '==========================================================
    '======================= Etat civil =======================
    '==========================================================

    'Chargement des données dans les labels dédiés
    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString

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

    '==========================================================
    '======================= Traitement =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim traitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim traitementDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        Select Case Me.AllergieOuContreIndication
            Case 1 'Allergie
                SQLString = "select oa_traitement_id, oa_traitement_medicament_dci, oa_traitement_arret, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_arret_commentaire, oa_traitement_allergie, oa_traitement_contre_indication from oasis.oa_traitement where (oa_traitement_annulation is Null or oa_traitement_annulation = '') and oa_traitement_allergie = '1' and oa_traitement_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_traitement_date_fin desc;"
                Me.Text = "Liste des allergies du patient"
            Case 2 'Contre-indication
                SQLString = "select oa_traitement_id, oa_traitement_medicament_dci, oa_traitement_arret, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_arret_commentaire, oa_traitement_allergie, oa_traitement_contre_indication from oasis.oa_traitement where (oa_traitement_annulation is Null or oa_traitement_annulation = '') and oa_traitement_contre_indication = '1' and oa_traitement_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_traitement_date_fin desc;"
                Me.Text = "Liste des contre-indications"
            Case Else
                Close()
                traitementDataAdapter.Dispose()
        End Select

        traitementDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        traitementDataAdapter.Fill(traitementDataTable)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim allergie, contreIndication As Boolean
        Dim dateFin As Date
        Dim DateDebut As Date
        Dim remarque As String
        Dim traitementArret As Boolean = False


        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Initialisation de la date de fin de traitement
            If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Initialisation de la date de début de traitement
            If traitementDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                DateDebut = traitementDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                DateDebut = Nothing
            End If

            'Identification si le traitement a été arrêté
            If traitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    traitementArret = True
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            TraitementDataGridView.Rows.Insert(iGrid)

            '------------------- Alimentation du DataGridView
            'DCI
            TraitementDataGridView("dci", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")

            'Date arrêt
            TraitementDataGridView("dateArret", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_date_fin")

            'Commentaire arrêt
            TraitementDataGridView("commentaireArret", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_arret_commentaire")

            'Clé unique du traitement
            TraitementDataGridView("traitementId", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_id")

            'Remarque
            remarque = ""
            If allergie = True Then
                remarque = "Allergie"
            End If
            If contreIndication = True Then
                remarque = "Contre-indication"
            End If
            If traitementArret = True Then
                If remarque = "" Then
                    remarque = "Traitement arrêté"
                Else
                    remarque = remarque + " - Traitement arrêté"
                End If
            End If
            TraitementDataGridView("remarque", iGrid).Value = remarque
        Next

        conxn.Close()
        traitementDataAdapter.Dispose()

        'Enlève le focus sur la première ligne du Grid
        TraitementDataGridView.ClearSelection()
    End Sub

    '===========================================================
    '======================= Généralités =======================
    '===========================================================

    'Initialisation de l'écran
    Private Sub initZones()
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
        'Traitements
        TraitementDataGridView.Rows.Clear()
    End Sub

    Private Sub TraitementDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles TraitementDataGridView.DoubleClick
        Dim TraitementId As Integer
        TraitementId = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("TraitementId").Value

        Dim vFTraitementDetailEdit As New FTraitementDetailEdit
        vFTraitementDetailEdit.SelectedTraitementId = TraitementId
        vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
        vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

        vFTraitementDetailEdit.ShowDialog() 'Modal

        vFTraitementDetailEdit.Dispose()

        'TraitementDataGridView.Rows.Clear()
        'ChargementTraitement()
    End Sub

    Private Sub DteHorizonAffichage_ValueChanged(sender As Object, e As EventArgs)
        'Rechargement grid
        TraitementDataGridView.Rows.Clear()
        ChargementTraitement()
    End Sub
End Class