Imports System.Data.SqlClient
Imports Oasis_Common

Public Class RadFAldCim10Selecteur
    Property UtilisateurConnecte As Utilisateur
    Property SelectedAldId As Integer
    Property SelectedAldCim10Id As Integer

    ReadOnly aldCim10Dao As AldCim10Dao = New AldCim10Dao()
    Dim aldCim10Id As Integer

    Private Sub RadFAldCim10Selecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitAffichageLabel()
        ChargementAld()
    End Sub

    Private Sub ChargementAld()
        Dim AldDataTable As List(Of AldCim10) = aldCim10Dao.GetAllAldCim10ByAldId(SelectedAldId)
        RadAldDataGridView.DataSource = AldDataTable
        RadAldDataGridView.ClearSelection()
    End Sub

    Private Sub InitAffichageLabel()
        LblCim10Code.Text = ""
        LblCim10Description.Text = ""
        PnlSelection.Hide()
        RadBtnSelection.Hide()
    End Sub

    Private Sub BtnSelection_Click(sender As Object, e As EventArgs) Handles RadBtnSelection.Click
        SelectionConfirmation()
    End Sub

    Private Sub SelectionConfirmation()
        Me.SelectedAldCim10Id = aldCim10Id
        Me.Close()
    End Sub

    Private Sub FDrcSelecteur_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            ChargementAld()
            InitAffichageLabel()
        End If
    End Sub

    Private Sub RadAldDataGridView_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadAldDataGridView.CellClick
        Selection()
    End Sub

    Private Sub Selection()
        If RadAldDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAldDataGridView.Rows.IndexOf(Me.RadAldDataGridView.CurrentRow)
            If aRow >= 0 Then
                aldCim10Id = RadAldDataGridView.Rows(aRow).Cells("AldCim10Id").Value
                LblCim10Code.Text = RadAldDataGridView.Rows(aRow).Cells("AldCim10Code").Value
                LblCim10Description.Text = RadAldDataGridView.Rows(aRow).Cells("AldCim10Description").Value

                If LblCim10Code.Text <> "" Then
                    RadBtnSelection.Show()
                    PnlSelection.Show()
                End If
            End If
        End If
    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Me.SelectedAldCim10Id = 0
        Close()
    End Sub

    Private Sub RadAldDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles RadAldDataGridView.DoubleClick
        Selection()
        SelectionConfirmation()
    End Sub
End Class
