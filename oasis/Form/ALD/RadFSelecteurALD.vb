Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common
Public Class RadFSelecteurALD
    Private _UtilisateurConnecte As Utilisateur
    Private _SelectedAldCode As String
    Private _SelectedAldId As Integer


    'Le DataAdapter a pour objet de récupérer les données de la BDD et permettre le renvoi des modifications à la BDD
    Dim AldDataAdapter As SqlDataAdapter = New SqlDataAdapter()

    'Le DataTable contient les données que le Grid va afficher (on pourrait utiliser un Dataset si on utilise plusieurs tables)
    Dim AldDataTable As DataTable = New DataTable()

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return _UtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            _UtilisateurConnecte = value
        End Set
    End Property

    Public Property SelectedAldCode As String
        Get
            Return _SelectedAldCode
        End Get
        Set(value As String)
            _SelectedAldCode = value
        End Set
    End Property

    Public Property SelectedAldId As Integer
        Get
            Return _SelectedAldId
        End Get
        Set(value As Integer)
            _SelectedAldId = value
        End Set
    End Property

    Dim aldId As Integer
    Private Sub FDrcSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitAffichageLabel()
        Me.SelectedAldCode = ""
        Me.SelectedAldId = 0
        'Récupération des données de la table [oa_ald] dans un DataTable et liason du DataTable avec la grid
        ChargementAld()
    End Sub

    Private Sub ChargementAld()
        Dim AldDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim AldDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        SQLString = getSQLStringDRC()

        AldDataTable.Clear()

        AldDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        AldDataAdapter.Fill(AldDataTable)

        AldDataGridView.DataSource = AldDataTable

        conxn.Close()
        AldDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        AldDataGridView.ClearSelection()
    End Sub

    Private Function getSQLStringDRC() As String
        Dim SQLString As String

        SQLString = "SELECT oa_ald_id, oa_ald_code, oa_ald_description FROM oasis.oa_ald;"

        getSQLStringDRC = SQLString
    End Function

    Private Sub InitAffichageLabel()
        LblAldCode.Text = ""
        LblAldDescription.Text = ""
        PnlSelection.Hide()
        BtnSelection.Hide()
    End Sub

    Private Sub BtnSelection_Click(sender As Object, e As EventArgs) Handles BtnSelection.Click
        Me.SelectedAldId = aldId
        Me.SelectedAldCode = LblAldCode.Text
        Me.Close()
    End Sub

    Private Sub BtnFiltrer_Click(sender As Object, e As EventArgs)
        ChargementAld()
        InitAffichageLabel()
    End Sub

    Private Sub AldDataGridView_Click(sender As Object, e As EventArgs) Handles AldDataGridView.Click
        If AldDataGridView.CurrentRow IsNot Nothing Then
            aldId = AldDataGridView.Rows(AldDataGridView.CurrentRow.Index).Cells("oa_ald_id").Value
            LblAldCode.Text = AldDataGridView.Rows(AldDataGridView.CurrentRow.Index).Cells("oa_ald_code").Value
            LblAldDescription.Text = AldDataGridView.Rows(AldDataGridView.CurrentRow.Index).Cells("oa_ald_description").Value

            If LblAldCode.Text <> "" Then
                BtnSelection.Show()
                PnlSelection.Show()
            End If
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
