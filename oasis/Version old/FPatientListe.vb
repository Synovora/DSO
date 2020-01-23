'Liste des patients

'Imports MySql.Data.MySqlClient
Imports Oasis_WF
Imports System.Data.SqlClient
Imports Oasis_Common

Public Class FPatientListe
    'Properties alimentées par l'écran d'authentification
    Private privateUtilisateurConnecte As Utilisateur
    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    'Le DataAdapter a pour objet de récupérer les données de la BDD et permettre le renvoi des modifications à la BDD
    'Dim patientDataAdapter As MySqlDataAdapter = Nothing
    Dim patientDataAdapter As SqlDataAdapter = Nothing

    'Le DataTable contient les données que le Grid va afficher (on pourrait utiliser un Dataset si on utilise plusieurs tables)
    Dim patientDataTable As DataTable = Nothing

    'Instanciation du patient pour le fournir aux Forms qui seront appelées depuis cette Form
    Dim SelectedPatient As New Patient

    Private Sub FPatientListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChkPatientOasis.Checked = True
        If UtilisateurConnecte.UtilisateurAdmin = False Then
            BtnAdmin.Hide()
        End If
        'Initialisation des données du patient en session et dans le Form
        InitZonesSelectionPatient()
        InitFilters()

        'Permet de gérer la touche entrée
        KeyPreview = True

        'Récupération des données de la table [oa_patient] dans un DataTable et liason du DataTable avec la grid
        BindGrid()
    End Sub

    Private Sub BindGrid()
        patientDataAdapter = New SqlDataAdapter()
        patientDataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Cursor = Cursors.WaitCursor

        Dim SQLString As String
        Dim dateSortie As Date
        SQLString = getSQLString()

        'The select command is responsible for retrieving the data only. This one has no parameters because we want all rows from the database.
        patientDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        patientDataAdapter.Fill(patientDataTable)

        'Ajout d'une colonne 'oa_patient_age' dans le DataTable de Patient
        patientDataTable.Columns.Add("oa_patient_age", Type.GetType("System.Int64"))

        'Parcours du DataTable pour alimenter les colonnes à calculer (hors BDD) : dans ce cas il s'agit du cacul de l'age à l'aide de la date de naissance du DataTable
        Dim i As Integer
        Dim rowCount As Integer = patientDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            If patientDataTable.Rows(i)("oa_patient_date_naissance") Is DBNull.Value Then
                patientDataTable.Rows(i)("oa_patient_age") = 0
            Else
                patientDataTable.Rows(i)("oa_patient_age") = outils.CalculAge(patientDataTable.Rows(i)("oa_patient_date_naissance"))
            End If

            If patientDataTable.Rows(i)("oa_patient_date_sortie_oasis") IsNot DBNull.Value Then
                dateSortie = patientDataTable.Rows(i)("oa_patient_date_sortie_oasis")
                If dateSortie > Date.Now() Then
                    patientDataTable.Rows(i)("oa_patient_date_sortie_oasis") = DBNull.Value
                End If
            End If
        Next

        'Pour terminer, alimentation de la Grid avec le DataTable
        PatientGridView.DataSource = patientDataTable

        conxn.Close()
        patientDataAdapter.Dispose()
        patientDataTable.Dispose()

        Cursor = Cursors.Default
    End Sub

    'Gestion de la sortie de l'écran
    Private Sub FPatientListe_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        patientDataAdapter.Dispose()
    End Sub

    'Action: rechargement de la liste des patients
    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        InitZonesSelectionPatient()
        BindGrid()
    End Sub

    'Action: rechargement de la liste des patients en fonction de l'alimentation des filtres
    Private Sub BtnDisplay_Click(sender As Object, e As EventArgs) Handles BtnDisplay.Click
        InitZonesSelectionPatient()
        BindGrid()
    End Sub

    Private Sub FPatientListe_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            InitZonesSelectionPatient()
            BindGrid()
        End If
    End Sub

    'Action: Initialisation des zones de filtre
    Private Sub BtnInitialize_Click(sender As Object, e As EventArgs) Handles BtnInitialize.Click
        InitFilters()
        InitZonesSelectionPatient()
        BindGrid()
    End Sub

    Private Function getSQLString() As String
        Dim SQLString As String
        Dim clausePrenom As String
        Dim clauseNom As String
        Dim clauseDate As String

        If TxtPrenom.Text = "" Then
            clausePrenom = "1 = 1"
        Else
            clausePrenom = "oa_patient_prenom LIKE '%" & TxtPrenom.Text & "%' "
        End If

        If TxtNom.Text = "" Then
            clauseNom = "1 = 1"
        Else
            clauseNom = "oa_patient_nom LIKE '%" & TxtNom.Text & "%' "
        End If

        If DteFiltreDateNaissance.Value = DteFiltreDateNaissance.MinDate Then
            clauseDate = "1 = 1"
        Else
            clauseDate = "oa_patient_date_naissance = '" & DteFiltreDateNaissance.Value.ToString("yyyy/MM/dd") & "'"
        End If

        If ChkPatientOasis.Checked = True Then
            SQLString = "SELECT oa_patient_id, oa_patient_nir, oa_patient_prenom, oa_patient_nom, oa_patient_date_naissance," &
                " oa_patient_date_sortie_oasis, oa_patient_date_entree_oasis FROM oasis.oa_patient" &
                " WHERE oa_patient_date_entree_oasis is not NULL and (oa_patient_date_sortie_oasis is NULL or oa_patient_date_sortie_oasis > CURRENT_TIMESTAMP) and " &
                clausePrenom & " AND " & clauseNom & " AND " & clauseDate & ";"
        Else
            If ChkPatientNonOasis.Checked = True Then
                SQLString = "SELECT oa_patient_id, oa_patient_nir, oa_patient_date_sortie_oasis, oa_patient_prenom, oa_patient_nom, oa_patient_date_naissance," &
                    " oa_patient_date_sortie_oasis, oa_patient_date_entree_oasis FROM oasis.oa_patient" &
                    " WHERE (oa_patient_date_entree_oasis is NULL or oa_patient_date_sortie_oasis <= CURRENT_TIMESTAMP) and " &
                    clausePrenom & " AND " & clauseNom & " AND " & clauseDate & ";"
            Else
                SQLString = "SELECT oa_patient_id, oa_patient_nir, oa_patient_prenom, oa_patient_nom, oa_patient_date_naissance," &
                    " oa_patient_date_sortie_oasis, oa_patient_date_entree_oasis FROM oasis.oa_patient WHERE " &
                    clausePrenom & " AND " & clauseNom & " AND " & clauseDate & ";"
            End If
        End If
        getSQLString = SQLString
    End Function

    Private Sub InitFilters()
        TxtNom.Text = ""
        TxtPrenom.Text = ""
        DteFiltreDateNaissance.Value = DteFiltreDateNaissance.MinDate
        DteFiltreDateNaissance.Format = DateTimePickerFormat.Custom
        DteFiltreDateNaissance.CustomFormat = " "
    End Sub

    'Sélectiond'un patient
    Private Sub PatientGridView_DoubleClick(sender As Object, e As EventArgs) Handles PatientGridView.DoubleClick
        Dim aRow As Integer
        Dim maxRow As Integer
        Dim DateSortie, DateEntree, dateNaissance As Date
        Dim DateIllimite As Date = "31/12/2999"

        aRow = PatientGridView.CurrentRow.Index
        maxRow = patientDataTable.Rows.Count - 1

        If aRow <= maxRow Then
            TxtIdSelected.Text = patientDataTable.Rows(aRow)("oa_patient_id")
            If patientDataTable.Rows(aRow)("oa_patient_nir") Is DBNull.Value Then
                TxtNirSelected.Text = 0
            Else
                TxtNirSelected.Text = patientDataTable.Rows(aRow)("oa_patient_nir")
            End If
            TxtPrenomSelected.Text = patientDataTable.Rows(aRow)("oa_patient_prenom")
            TxtNomSelected.Text = patientDataTable.Rows(aRow)("oa_patient_nom")

            If patientDataTable.Rows(aRow)("oa_patient_date_naissance") IsNot DBNull.Value Then
                dateNaissance = patientDataTable.Rows(aRow)("oa_patient_date_naissance")
                LblDateNaissanceSelected.Text = dateNaissance.ToString("dd.MM.yyyy")
                LblDateNaissanceSelected.Show()
                LblAgeSelected.Text = patientDataTable.Rows(aRow)("oa_patient_age").ToString + " an(s)"
                LblAgeSelected.Show()
            Else
                LblDateNaissanceSelected.Hide()
                LblAgeSelected.Hide()
            End If

            DateSortie = DateIllimite
            If patientDataTable.Rows(aRow)("oa_patient_date_sortie_oasis") IsNot DBNull.Value Then
                DateSortie = patientDataTable.Rows(aRow)("oa_patient_date_sortie_oasis")
            End If

            DateEntree = DateIllimite
            If patientDataTable.Rows(aRow)("oa_patient_date_entree_oasis") IsNot DBNull.Value Then
                DateEntree = patientDataTable.Rows(aRow)("oa_patient_date_entree_oasis")
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
            BtnPatientDetail.Show()
            BtnSynthese.Show()
        Else
            InitZonesSelectionPatient()
        End If

    End Sub

    Private Sub PatientGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles PatientGridView.CellClick
        Dim aRow As Integer
        Dim maxRow As Integer
        Dim DateSortie, DateEntree, dateNaissance As Date
        Dim DateIllimite As Date = "31/12/2999"

        aRow = PatientGridView.CurrentRow.Index
        maxRow = patientDataTable.Rows.Count - 1

        If aRow <= maxRow Then
            TxtIdSelected.Text = patientDataTable.Rows(aRow)("oa_patient_id")
            If patientDataTable.Rows(aRow)("oa_patient_nir") Is DBNull.Value Then
                TxtNirSelected.Text = 0
            Else
                TxtNirSelected.Text = patientDataTable.Rows(aRow)("oa_patient_nir")
            End If
            TxtPrenomSelected.Text = patientDataTable.Rows(aRow)("oa_patient_prenom")
            TxtNomSelected.Text = patientDataTable.Rows(aRow)("oa_patient_nom")

            If patientDataTable.Rows(aRow)("oa_patient_date_naissance") IsNot DBNull.Value Then
                dateNaissance = patientDataTable.Rows(aRow)("oa_patient_date_naissance")
                LblDateNaissanceSelected.Text = dateNaissance.ToString("dd.MM.yyyy")
                LblDateNaissanceSelected.Show()
                LblAgeSelected.Text = patientDataTable.Rows(aRow)("oa_patient_age").ToString + " an(s)"
                LblAgeSelected.Show()
            Else
                LblDateNaissanceSelected.Hide()
                LblAgeSelected.Hide()
            End If

            DateSortie = DateIllimite
            If patientDataTable.Rows(aRow)("oa_patient_date_sortie_oasis") IsNot DBNull.Value Then
                DateSortie = patientDataTable.Rows(aRow)("oa_patient_date_sortie_oasis")
            End If

            DateEntree = DateIllimite
            If patientDataTable.Rows(aRow)("oa_patient_date_entree_oasis") IsNot DBNull.Value Then
                DateEntree = patientDataTable.Rows(aRow)("oa_patient_date_entree_oasis")
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
            BtnPatientDetail.Show()
            BtnSynthese.Show()
        Else
            InitZonesSelectionPatient()
        End If
    End Sub

    'Sélection d'un patient dans la liste et affichage du patient sélectionné
    Private Sub BtnPatientDetail_Click(sender As Object, e As EventArgs) Handles BtnPatientDetail.Click
        If TxtIdSelected.Text <> "" Then
            'Initialisation du patient sélectionné
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            PatientDao.SetPatient(Me.SelectedPatient, patientId)

            Dim vFFPatientDetailEdit As New FPatientDetailEdit
            vFFPatientDetailEdit.SelectedPatientId = patientId
            vFFPatientDetailEdit.SelectedPatient = Me.SelectedPatient
            vFFPatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

            vFFPatientDetailEdit.ShowDialog() 'Modal

            'Si le patient a été créé, on recharge la grid
            If vFFPatientDetailEdit.CodeRetour = True Then
                InitZonesSelectionPatient()
                BindGrid()
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
            Dim vFSynthese As New FSynthese

            PatientDao.SetPatient(Me.SelectedPatient, patientId)
            vFSynthese.SelectedPatient = Me.SelectedPatient
            vFSynthese.UtilisateurConnecte = Me.UtilisateurConnecte

            'Il est important d'appeler le Form en Modal, car on ne doit pas écraser les données du patient stockées en session et peut être déjà en cours d'utilisation
            vFSynthese.ShowDialog() 'Modal

            vFSynthese.Dispose()
        Else
            MessageBox.Show("Vous devez sélectionner un patient pour lancer cette option")
        End If
    End Sub

    'Appel de la création d'un patient
    Private Sub BtnCreatePatient_Click(sender As Object, e As EventArgs) Handles BtnCreatePatient.Click
        Dim vFFPatientDetailEdit As New FPatientDetailEdit
        PatientDao.SetPatient(Me.SelectedPatient, 0)
        vFFPatientDetailEdit.SelectedPatientId = 0
        vFFPatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
        vFFPatientDetailEdit.SelectedPatient = Me.SelectedPatient
        vFFPatientDetailEdit.ShowDialog() 'Modal

        'Si le patient a été créé, on recharge la grid
        If vFFPatientDetailEdit.CodeRetour = True Then
            InitZonesSelectionPatient()
            BindGrid()
        End If

        vFFPatientDetailEdit.Dispose()
    End Sub

    'Appel de l'écran d'affichage du référentiel médicamenteux
    Private Sub BtnMedoc_Click(sender As Object, e As EventArgs) Handles BtnMedoc.Click
        Dim vForm6 As New FMedocListe
        vForm6.ShowDialog() 'Modal
        vForm6.Dispose()
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
        BtnPatientDetail.Hide()
        BtnSynthese.Hide()
    End Sub

    Private Sub ChkPatientOasis_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPatientOasis.CheckedChanged
        If ChkPatientOasis.Checked = True Then
            ChkPatientTous.Checked = False
            ChkPatientNonOasis.Checked = False
            InitZonesSelectionPatient()
            BindGrid()
        End If
    End Sub

    Private Sub ChkPatientTous_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPatientTous.CheckedChanged
        If ChkPatientTous.Checked = True Then
            ChkPatientOasis.Checked = False
            ChkPatientNonOasis.Checked = False
            InitZonesSelectionPatient()
            BindGrid()
        End If
    End Sub

    Private Sub ChkPatientNonOasis_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPatientNonOasis.CheckedChanged
        If ChkPatientNonOasis.Checked = True Then
            ChkPatientOasis.Checked = False
            ChkPatientTous.Checked = False
            InitZonesSelectionPatient()
            BindGrid()
        End If
    End Sub

    Private Sub BtnAdmin_Click(sender As Object, e As EventArgs) Handles BtnAdmin.Click
        Dim vFMenuAdmin As New FMenuAdmin
        PatientDao.SetPatient(Me.SelectedPatient, 0)
        vFMenuAdmin.UtilisateurConnecte = Me.UtilisateurConnecte
        vFMenuAdmin.ShowDialog() 'Modal
        vFMenuAdmin.Dispose()
    End Sub

    Private Sub DteFiltreDateNaissance_DropDown(sender As Object, e As EventArgs) Handles DteFiltreDateNaissance.DropDown
        If DteFiltreDateNaissance.Value = DteFiltreDateNaissance.MinDate Then
            DteFiltreDateNaissance.Value = Date.Now
            DteFiltreDateNaissance.Format = DateTimePickerFormat.Long
        End If
    End Sub

End Class