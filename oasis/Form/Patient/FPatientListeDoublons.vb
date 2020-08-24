'Liste des patients

Imports System.Data.SqlClient
Imports Oasis_Common
Public Class FPatientListeDoublons
    'Properties alimentées par l'écran d'authentification
    Private privateUtilisateurConnecte As Utilisateur
    Private _ListeDataTable As DataTable
    Private _CodeRetour As Boolean
    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Public Property ListeDataTable As DataTable
        Get
            Return _ListeDataTable
        End Get
        Set(value As DataTable)
            _ListeDataTable = value
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

    'Instanciation du patient pour le fournir aux Forms qui seront appelées depuis cette Form
    Dim SelectedPatient As New Patient

    Private Sub FPatientListeDoublons_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Initialisation des données du patient en session et dans le Form
        InitZonesSelectionPatient()

        'Récupération des données de la table [oa_patient] dans un DataTable et liason du DataTable avec la grid
        ChargementListePatients()
    End Sub

    Private Sub ChargementListePatients()
        CodeRetour = False
        Dim dateSortie As Date

        'Ajout d'une colonne 'oa_patient_age' dans le DataTable de Patient
        ListeDataTable.Columns.Add("oa_patient_age", Type.GetType("System.Int64"))

        'Parcours du DataTable
        Dim i As Integer
        Dim rowCount As Integer = ListeDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            If ListeDataTable.Rows(i)("oa_patient_date_naissance") Is DBNull.Value Then
                ListeDataTable.Rows(i)("oa_patient_age") = 0
            Else
                ListeDataTable.Rows(i)("oa_patient_age") = outils.CalculAgeEnAnnee(ListeDataTable.Rows(i)("oa_patient_date_naissance"))
            End If

            If ListeDataTable.Rows(i)("oa_patient_date_sortie_oasis") IsNot DBNull.Value Then
                dateSortie = ListeDataTable.Rows(i)("oa_patient_date_sortie_oasis")
                If dateSortie > Date.Now() Then
                    ListeDataTable.Rows(i)("oa_patient_date_sortie_oasis") = DBNull.Value
                End If
            End If
        Next

        'Pour terminer, alimentation de la Grid avec le DataTable
        PatientGridView.DataSource = ListeDataTable

        ListeDataTable.Dispose()
    End Sub

    'Sélectiond'un patient
    Private Sub PatientGridView_DoubleClick(sender As Object, e As EventArgs) Handles PatientGridView.DoubleClick
        Dim aRow As Integer
        Dim maxRow As Integer
        Dim DateSortie, DateEntree, dateNaissance As Date
        Dim DateIllimite As Date = "31/12/2999"

        aRow = PatientGridView.CurrentRow.Index
        maxRow = ListeDataTable.Rows.Count - 1

        If aRow <= maxRow Then
            TxtIdSelected.Text = ListeDataTable.Rows(aRow)("oa_patient_id")
            If ListeDataTable.Rows(aRow)("oa_patient_nir") Is DBNull.Value Then
                TxtNirSelected.Text = 0
            Else
                TxtNirSelected.Text = ListeDataTable.Rows(aRow)("oa_patient_nir")
            End If
            TxtPrenomSelected.Text = ListeDataTable.Rows(aRow)("oa_patient_prenom")
            TxtNomSelected.Text = ListeDataTable.Rows(aRow)("oa_patient_nom")

            If ListeDataTable.Rows(aRow)("oa_patient_date_naissance") IsNot DBNull.Value Then
                dateNaissance = ListeDataTable.Rows(aRow)("oa_patient_date_naissance")
                LblDateNaissanceSelected.Text = dateNaissance.ToString("dd.MM.yyyy")
                LblDateNaissanceSelected.Show()
                LblAgeSelected.Text = ListeDataTable.Rows(aRow)("oa_patient_age").ToString + " an(s)"
                LblAgeSelected.Show()
            Else
                LblDateNaissanceSelected.Hide()
                LblAgeSelected.Hide()
            End If

            DateSortie = DateIllimite
            If ListeDataTable.Rows(aRow)("oa_patient_date_sortie_oasis") IsNot DBNull.Value Then
                DateSortie = ListeDataTable.Rows(aRow)("oa_patient_date_sortie_oasis")
            End If

            DateEntree = DateIllimite
            If ListeDataTable.Rows(aRow)("oa_patient_date_entree_oasis") IsNot DBNull.Value Then
                DateEntree = ListeDataTable.Rows(aRow)("oa_patient_date_entree_oasis")
            End If

            If DateSortie < Date.Now() Then
                LblDateSortie.Text = DateSortie.ToString("dd.MM.yyyy")
                LblLabelDateSortie.Show()
                LblDateSortie.Show()
                LblPatientSorti.Text = "Attention, ce patient est sorti du dispositif Oasis"
                LblPatientSorti.Show()
            Else
                LblLabelDateSortie.Hide()
                LblDateSortie.Hide()
                LblPatientSorti.Hide()
            End If

            If DateEntree > Date.Now() Then
                LblPatientSorti.Text = "Attention, ce patient ne fait pas partie du dispositif Oasis"
                LblPatientSorti.Show()
            End If

            TxtIdSelected.Show()
            TxtNirSelected.Show()
            TxtPrenomSelected.Show()
            TxtNomSelected.Show()
            PnlSelectedPatient.Show()
            BtnSynthese.Show()
        Else
            InitZonesSelectionPatient()
        End If

    End Sub

    'Sélection d'un patient dans la liste et affichage du patient sélectionné
    Private Sub BtnPatientDetail_Click(sender As Object, e As EventArgs)
        If TxtIdSelected.Text <> "" Then
            'Initialisation du patient sélectionné
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            'PatientDao.SetPatient(Me.SelectedPatient, patientId)
            Me.SelectedPatient = PatientDao.getPatientById(patientId)

            Dim vFFPatientDetailEdit As New RadFPatientDetailEdit
            vFFPatientDetailEdit.SelectedPatientId = patientId
            vFFPatientDetailEdit.SelectedPatient = Me.SelectedPatient
            vFFPatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

            vFFPatientDetailEdit.ShowDialog() 'Modal

            'Si le patient a été créé, on recharge la grid
            If vFFPatientDetailEdit.CodeRetour = True Then
                InitZonesSelectionPatient()
                ChargementListePatients()
            End If

            vFFPatientDetailEdit.Dispose()
        Else
            MessageBox.Show("Vous devez sélectionner un patient pour lancer cette option")
        End If
    End Sub

    'Appel de l'écran de synthèse du patient
    Private Sub BtnSynthese_Click(sender As Object, e As EventArgs) Handles BtnSynthese.Click
        If TxtIdSelected.Text <> "" Then
            'Création instance patient
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            Dim vFSynthese As New RadFSynthese

            'PatientDao.SetPatient(Me.SelectedPatient, patientId)
            Me.SelectedPatient = PatientDao.getPatientById(patientId)

            vFSynthese.SelectedPatient = Me.SelectedPatient
            vFSynthese.UtilisateurConnecte = Me.UtilisateurConnecte

            'Il est important d'appeler le Form en Modal, car on ne doit pas écraser les données du patient stockées en session et peut être déjà en cours d'utilisation
            vFSynthese.ShowDialog() 'Modal

            vFSynthese.Dispose()
        Else
            MessageBox.Show("Vous devez sélectionner un patient pour lancer cette option")
        End If
    End Sub

    'Gestion de l'affichage des zones d'écran
    Private Sub InitZonesSelectionPatient()
        'Initialisation des TextBox
        TxtIdSelected.Text = ""
        TxtNirSelected.Text = ""
        TxtPrenomSelected.Text = ""
        TxtNomSelected.Text = ""

        'Cacher les TextBox
        TxtIdSelected.Hide()
        TxtNirSelected.Hide()
        TxtPrenomSelected.Hide()
        TxtNomSelected.Hide()
        LblDateNaissanceSelected.Hide()
        LblAgeSelected.Hide()
        PnlSelectedPatient.Hide()

        'Cacher les boutons d'appel
        BtnSynthese.Hide()
    End Sub

    Private Sub BtnCreationPatient_Click(sender As Object, e As EventArgs) Handles BtnCreationPatient.Click
        Me.CodeRetour = False
        Close()
    End Sub

    Private Sub BtnRetourListePatient_Click(sender As Object, e As EventArgs) Handles BtnRetourListePatient.Click
        Me.CodeRetour = True
        Close()
    End Sub
End Class