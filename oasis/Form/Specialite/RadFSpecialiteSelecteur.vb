﻿Imports System.Collections.Specialized

Public Class RadFSpecialiteSelecteur
    Private _SelectedSpecialiteId As Integer
    Private _selectedTypeSpecialite As String
    Private _ListProfilOasis As List(Of Integer)

    Public Property SelectedSpecialiteId As Integer
        Get
            Return _SelectedSpecialiteId
        End Get
        Set(value As Integer)
            _SelectedSpecialiteId = value
        End Set
    End Property

    Public Property SelectedTypeSpecialite As String
        Get
            Return _selectedTypeSpecialite
        End Get
        Set(value As String)
            _selectedTypeSpecialite = value
        End Set
    End Property

    Public Property ListProfilOasis As List(Of Integer)
        Get
            Return _ListProfilOasis
        End Get
        Set(value As List(Of Integer))
            _ListProfilOasis = value
        End Set
    End Property

    Dim IdSelected As Integer

    Private Sub RadFSpecialiteSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: cette ligne de code charge les données dans la table 'DS_Specialite.oa_r_specialite'. Vous pouvez la déplacer ou la supprimer selon les besoins.

        Dim FiltreOasis As String = ""
        If ListProfilOasis IsNot Nothing Then
            FiltreOasis = String.Join(",", ListProfilOasis)
        End If

        If FiltreOasis = "" Then
            Me.OarspecialiteBindingSource.Filter = "(oa_r_specialite_inactif = 'False' or oa_r_specialite_inactif is NULL) and oa_r_parcours = 'True'"
        Else
            Me.OarspecialiteBindingSource.Filter = "(oa_r_specialite_inactif = 'False' or oa_r_specialite_inactif is NULL) and oa_r_parcours = 'True' and oa_r_specialite_id not in (" & FiltreOasis & ")"
        End If

        Me.Oa_r_specialiteTableAdapter.Fill(Me.DS_Specialite.oa_r_specialite)

        Me.RadSpecialiteDataGridView.SortDescriptors.Expression = "oa_r_specialite_description"

        InitSelect()

    End Sub

    Private Sub RadGridView1_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadSpecialiteDataGridView.CellClick
        Dim aRow, maxRow As Integer
        aRow = Me.RadSpecialiteDataGridView.Rows.IndexOf(Me.RadSpecialiteDataGridView.CurrentRow)
        maxRow = RadSpecialiteDataGridView.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            RadGbxSelect.Show()
            LblSpecialite.Text = RadSpecialiteDataGridView.Rows(aRow).Cells("oa_r_specialite_description").Value
            LblNature.Text = RadSpecialiteDataGridView.Rows(aRow).Cells("oa_r_specialite_nature").Value
            IdSelected = RadSpecialiteDataGridView.Rows(aRow).Cells("oa_r_specialite_id").Value
        End If
    End Sub

    Private Sub RadBtnSelection_Click(sender As Object, e As EventArgs) Handles RadBtnSelection.Click
        SelectedSpecialiteId = IdSelected
        SelectedTypeSpecialite = LblNature.Text
        Close()
    End Sub

    Private Sub RadSpecialiteDataGridView_FilterChanged(sender As Object, e As Telerik.WinControls.UI.GridViewCollectionChangedEventArgs) Handles RadSpecialiteDataGridView.FilterChanged
        InitSelect()
    End Sub

    Private Sub InitSelect()
        RadGbxSelect.Hide()
        LblSpecialite.Text = ""
        LblNature.Text = ""
        IdSelected = 0
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        SelectedSpecialiteId = 0
        SelectedTypeSpecialite = ""
        Close()
    End Sub
End Class
