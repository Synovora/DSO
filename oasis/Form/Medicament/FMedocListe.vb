'Liste des médicaments

Imports System.Data.SqlClient

Public Class FMedocListe
    'Le DataAdapter a pour objet de récupérer les données de la BDD et permettre le renvoi des modifications à la BDD
    Dim medicamentDataAdapter As SqlDataAdapter = New SqlDataAdapter()

    'Le DataTable contient les données que le Grid va afficher (on pourrait utiliser un Dataset si on utilise plusieurs tables)
    Dim medicamentDataTable As DataTable = New DataTable()

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        'Récupération des données de la table [oa_patient] dans un DataTable et liason du DataTable avec la grid
        BindGrid()

    End Sub

    Private Sub BindGrid()
        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        SQLString = getSQLString()

        'The select command is responsible for retrieving the data only. This one has no parameters because we want all rows from the database.
        medicamentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        medicamentDataAdapter.Fill(medicamentDataTable)

        'Pour terminer, alimentation de la Grid avec le DataTable (ou DataSet selon le cas)
        MedicamentGridView.DataSource = medicamentDataTable

        conxn.Close()
        medicamentDataAdapter.Dispose()
        medicamentDataTable.Dispose()

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

        SQLString = "SELECT oa_medicament_cis, oa_medicament_dci, oa_medicament_forme, oa_medicament_voie_administration," &
            "oa_medicament_etat_commercialisation, oa_medicament_titulaire FROM oasis.oa_r_medicament" &
            " WHERE " & clauseCIS & " AND " & clauseDCI & " AND " & clauseLabo & ";"

        getSQLString = SQLString

    End Function

    Private Sub Form6_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        medicamentDataAdapter.Dispose()
    End Sub

    Private Sub BtnFiltrer_Click(sender As Object, e As EventArgs) Handles BtnFiltrer.Click
        BindGrid()
    End Sub

    Private Sub BtnInitialiser_Click(sender As Object, e As EventArgs) Handles BtnInitialiser.Click
        TxtCIS.Text = ""
        TxtDCI.Text = ""
        TxtLabo.Text = ""
    End Sub
End Class