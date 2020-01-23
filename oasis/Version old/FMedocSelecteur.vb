'Liste des médicaments

Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class FMedocSelecteur
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedMedicamentCis As Integer
    Private privateAllergie As Boolean
    Private privateContreIndication As Boolean

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

    'Le DataAdapter a pour objet de récupérer les données de la BDD et permettre le renvoi des modifications à la BDD
    Dim medicamentDataAdapter As SqlDataAdapter = New SqlDataAdapter()

    'Le DataTable contient les données que le Grid va afficher (on pourrait utiliser un Dataset si on utilise plusieurs tables)
    Dim medicamentDataTable As DataTable = New DataTable()
    Private Sub FMedocSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Permet de gérer la touche entrée
        KeyPreview = True
        InitAffichageLabel()
        Me.SelectedMedicamentCis = 0
        'Récupération des données de la table [oa_patient] dans un DataTable et liason du DataTable avec la grid
        ChargementMedicaments()
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
    End Sub

    Private Sub ChargementMedicaments()
        Dim medicamentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim medicamentDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        'Exlusion des traitements : traitements déclarés 'annulé' 
        SQLString = getSQLString()

        medicamentDataTable.Clear()

        medicamentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        medicamentDataAdapter.Fill(medicamentDataTable)

        '============== dummy
        MedicamentGridView.DataSource = medicamentDataTable
        '==============

        conxn.Close()
        medicamentDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        MedicamentGridView.ClearSelection()
    End Sub

    Private Sub dummy()
        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = medicamentDataTable.Rows.Count - 1
        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            MedicamentGridView.Rows.Insert(iGrid)
            'Chargement des données
            If medicamentDataTable.Rows(i)("oa_medicament_cis") IsNot DBNull.Value Then
                MedicamentGridView("col_medicament_cis", iGrid).Value = medicamentDataTable.Rows(i)("oa_medicament_cis")
            Else
                MedicamentGridView("col_medicament_cis", iGrid).Value = ""
            End If
            If medicamentDataTable.Rows(i)("oa_medicament_dci") IsNot DBNull.Value Then
                MedicamentGridView("col_medicament_dci", iGrid).Value = medicamentDataTable.Rows(i)("oa_medicament_dci")
            Else
                MedicamentGridView("col_medicament_dci", iGrid).Value = ""
            End If
            If medicamentDataTable.Rows(i)("oa_medicament_forme") IsNot DBNull.Value Then
                MedicamentGridView("col_medicament_forme", iGrid).Value = medicamentDataTable.Rows(i)("oa_medicament_forme")
            Else
                MedicamentGridView("col_medicament_forme", iGrid).Value = ""
            End If
            If medicamentDataTable.Rows(i)("oa_medicament_voie_administration") IsNot DBNull.Value Then
                MedicamentGridView("col_medicament_voie_administration", iGrid).Value = medicamentDataTable.Rows(i)("oa_medicament_voie_administration")
            Else
                MedicamentGridView("col_medicament_voie_administration", iGrid).Value = ""
            End If
            If medicamentDataTable.Rows(i)("oa_medicament_etat_commercialisation") IsNot DBNull.Value Then
                MedicamentGridView("col_medicament_etat_commercialisation", iGrid).Value = medicamentDataTable.Rows(i)("oa_medicament_etat_commercialisation")
            Else
                MedicamentGridView("col_medicament_etat_commercialisation", iGrid).Value = ""
            End If
            If medicamentDataTable.Rows(i)("oa_medicament_titulaire") IsNot DBNull.Value Then
                MedicamentGridView("col_medicament_titulaire", iGrid).Value = medicamentDataTable.Rows(i)("oa_medicament_titulaire")
            Else
                MedicamentGridView("col_medicament_titulaire", iGrid).Value = ""
            End If
        Next
    End Sub

    Private Function getSQLString() As String
        Dim SQLString As String
        Dim clauseDCI As String
        Dim clauseCIS As String
        Dim clauseLabo As String

        If TxtDCI.Text = "" Then
            clauseDCI = "1 = 1"
        Else
            clauseDCI = "oa_medicament_dci LIKE '%" & TxtDCI.Text & "%' "
        End If

        If TxtCIS.Text = "" Then
            clauseCIS = "1 = 1"
        Else
            clauseCIS = "oa_medicament_cis LIKE '%" & TxtCIS.Text & "%' "
        End If

        If TxtLabo.Text = "" Then
            clauseLabo = "1 = 1"
        Else
            clauseLabo = "oa_medicament_titulaire LIKE '%" & TxtLabo.Text & "%' "
        End If

        SQLString = "SELECT oa_medicament_cis, oa_medicament_dci, oa_medicament_forme, oa_medicament_voie_administration, oa_medicament_etat_commercialisation, oa_medicament_titulaire FROM oasis.oa_r_medicament WHERE " & clauseCIS & " AND " & clauseDCI & " AND " & clauseLabo & ";"
        getSQLString = SQLString
    End Function

    Private Sub Form6_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        medicamentDataAdapter.Dispose()
    End Sub

    Private Sub BtnFiltrer_Click(sender As Object, e As EventArgs) Handles BtnFiltrer.Click
        ChargementMedicaments()
        InitAffichageLabel()
    End Sub

    Private Sub BtnInitialiser_Click(sender As Object, e As EventArgs) Handles BtnInitialiser.Click
        TxtCIS.Text = ""
        TxtDCI.Text = ""
        TxtLabo.Text = ""
    End Sub

    Private Sub MedicamentGridView_DoubleClick(sender As Object, e As EventArgs) Handles MedicamentGridView.DoubleClick
        Dim medicamentAllergique As Boolean = False
        Dim medicamentContreIndication As Boolean = False
        Dim medicamentDejaPrescrit As Boolean = False

        InitAffichageLabel()
        LblMedicamentCis.Text = MedicamentGridView.Rows(MedicamentGridView.CurrentRow.Index).Cells("col_medicament_cis").Value
        LblMedicamentDci.Text = MedicamentGridView.Rows(MedicamentGridView.CurrentRow.Index).Cells("col_medicament_dci").Value
        LblMedicamentForme.Text = MedicamentGridView.Rows(MedicamentGridView.CurrentRow.Index).Cells("col_medicament_forme").Value

        If LblMedicamentCis.Text <> "" Then
            'Contrôle que le médicament n'est pas déjà prescrit
            Dim medicamentsPrescritsEnumerator As StringEnumerator = SelectedPatient.PatientMedicamentsPrescritsCis.GetEnumerator()
            While medicamentsPrescritsEnumerator.MoveNext()
                If LblMedicamentCis.Text = medicamentsPrescritsEnumerator.Current.ToString Then
                    medicamentDejaPrescrit = True
                    Exit While
                End If
            End While

            If medicamentDejaPrescrit = True Then
                LblMedicamentAlerte.Visible = True
                LblMedicamentAlerte.Text = "Attention, cette dénomination fait déjà l'objet d'un traitement en cours pour ce patient"
            End If

            'Contrôle que le médicament sélectionné n'est pas allergique
            'TraitementAllergies(Me.SelectedPatient) '==>Réalisé dans la fiche de synthèse une seule fois plutôt qu'à chaque sélection de médicament
            'Dim allergieEnumerator As StringEnumerator = SelectedPatient.PatientAllergiesGénériquesCis.GetEnumerator()
            'While allergieEnumerator.MoveNext()
            'If LblMedicamentCis.Text = allergieEnumerator.Current.ToString Then
            'medicamentAllergique = True
            'Exit While
            'End If
            'End While

            'Dim contreIndicationEnumerator As StringEnumerator = SelectedPatient.PatientContreIndicationCis.GetEnumerator()
            'While contreIndicationEnumerator.MoveNext()
            'If LblMedicamentCis.Text = contreIndicationEnumerator.Current.ToString Then
            'medicamentContreIndication = True
            'Exit While
            'End If
            'End While

            BtnSelect.Show()
            PnlSelectedMedicament.Show()

            'If medicamentAllergique = False Then
            'If medicamentContreIndication = True Then
            'LblMedicamentAlerte.Visible = True
            'LblMedicamentAlerte.Text = "Attention, cette dénomination a été contre-indiquée pour ce patient"
            'End If
            'Else
            'LblMedicamentAlerte.Visible = True
            'LblMedicamentAlerte.Text = "(Allergie) => Cette dénomination ne peut pas être prescrite"
            'BtnSelect.Hide()
            'End If
        End If
    End Sub

    Private Sub InitAffichageLabel()
        LblMedicamentCis.Text = ""
        LblMedicamentDci.Text = ""
        LblMedicamentForme.Text = ""
        LblMedicamentAlerte.Text = ""
        PnlSelectedMedicament.Hide()
        LblMedicamentAlerte.Visible = False
        BtnSelect.Hide()
    End Sub

    Private Sub BtnSelect_Click(sender As Object, e As EventArgs) Handles BtnSelect.Click
        Dim medicamentCIS As Integer

        medicamentCIS = CInt(LblMedicamentCis.Text)
        Me.SelectedMedicamentCis = medicamentCIS
        Me.Close()
    End Sub

    Private Sub FMedocSelecteur_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            ChargementMedicaments()
            InitAffichageLabel()
        End If
    End Sub

    Private Sub ContextMenuStripMedicament_Click(sender As Object, e As EventArgs) Handles ContextMenuStripMedicament.Click
        'Détail médicament
        Dim TraitementMedicamentCIS As Integer = MedicamentGridView.Rows(MedicamentGridView.CurrentRow.Index).Cells("col_medicament_cis").Value

        Dim vFMedocDetail As New FMedocDetail
        vFMedocDetail.MedicamentCis = TraitementMedicamentCIS
        vFMedocDetail.ShowDialog() 'Modal
        vFMedocDetail.Dispose()
    End Sub

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        'Traitement : afficher les allergies dans un popup
        Dim vFTraitementAllergieEtCI As New FTraitementAllergieEtCI
            vFTraitementAllergieEtCI.SelectedPatient = Me.SelectedPatient
            vFTraitementAllergieEtCI.UtilisateurConnecte = Me.UtilisateurConnecte
            vFTraitementAllergieEtCI.AllergieOuContreIndication = EnumAllergieOuContreIndication.Allergie

            vFTraitementAllergieEtCI.ShowDialog() 'Modal

        vFTraitementAllergieEtCI.Dispose()
    End Sub

    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles lblContreIndication.Click
        'Traitement : afficher les contre-indications dans un popup
        Dim vFTraitementAllergieEtCI As New FTraitementAllergieEtCI
        vFTraitementAllergieEtCI.SelectedPatient = Me.SelectedPatient
        vFTraitementAllergieEtCI.UtilisateurConnecte = Me.UtilisateurConnecte
        vFTraitementAllergieEtCI.AllergieOuContreIndication = EnumAllergieOuContreIndication.ContreIndication

        vFTraitementAllergieEtCI.ShowDialog() 'Modal

        vFTraitementAllergieEtCI.Dispose()
    End Sub
End Class