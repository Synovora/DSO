﻿Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common
Public Class FAldCim10Selecteur
    Private _UtilisateurConnecte As Utilisateur
    Private _SelectedAldId As Integer
    Private _SelectedAldCim10Id As Integer


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

    Public Property SelectedAldId As Integer
        Get
            Return _SelectedAldId
        End Get
        Set(value As Integer)
            _SelectedAldId = value
        End Set
    End Property

    Public Property SelectedAldCim10Id As Integer
        Get
            Return _SelectedAldCim10Id
        End Get
        Set(value As Integer)
            _SelectedAldCim10Id = value
        End Set
    End Property

    Dim aldCim10Id As Integer
    Private Sub FDrcSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitAffichageLabel()
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

        SQLString = "SELECT oa_ald_cim10_id, oa_ald_cim10_ald_code, oa_ald_cim10_code, oa_ald_cim10_description FROM oasis.oa_ald_cim10 WHERE oa_ald_cim10_ald_id = " & SelectedAldId.ToString & ";"

        getSQLStringDRC = SQLString
    End Function

    Private Sub InitAffichageLabel()
        LblCim10Code.Text = ""
        LblCim10Description.Text = ""
        PnlSelection.Hide()
        BtnSelection.Hide()
    End Sub

    Private Sub BtnSelection_Click(sender As Object, e As EventArgs) Handles BtnSelection.Click
        Me.SelectedAldCim10Id = aldCim10Id
        Me.Close()
    End Sub

    Private Sub FDrcSelecteur_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            ChargementAld()
            InitAffichageLabel()
        End If
    End Sub

    Private Sub BtnFiltrer_Click(sender As Object, e As EventArgs)
        ChargementAld()
        InitAffichageLabel()
    End Sub

    Private Sub DrcDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles AldDataGridView.CellClick
        If AldDataGridView.CurrentRow IsNot Nothing Then
            aldCim10Id = AldDataGridView.Rows(AldDataGridView.CurrentRow.Index).Cells("oa_ald_cim10_id").Value
            LblCim10Code.Text = AldDataGridView.Rows(AldDataGridView.CurrentRow.Index).Cells("oa_ald_cim10_code").Value
            LblCim10Description.Text = AldDataGridView.Rows(AldDataGridView.CurrentRow.Index).Cells("oa_ald_cim10_description").Value

            If LblCim10Code.Text <> "" Then
                BtnSelection.Show()
                PnlSelection.Show()
            End If
        End If
    End Sub
End Class