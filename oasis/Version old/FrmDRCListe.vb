Imports Telerik.WinControls.UI

Public Class FrmDRCListe
    Private privateUtilisateurConnecte As Utilisateur
    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property
    Private Sub RadForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: cette ligne de code charge les données dans la table 'DatSetDRC.oa_drc'. Vous pouvez la déplacer ou la supprimer selon les besoins.

        Me.Oa_drcTableAdapter.Fill(Me.DatSetDRC.oa_drc)

    End Sub

    Private Sub RadGridView1_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadDRCDataGridView.CellDoubleClick
        Dim aRow, maxRow As Integer
        aRow = Me.RadDRCDataGridView.Rows.IndexOf(Me.RadDRCDataGridView.CurrentRow)
        maxRow = RadDRCDataGridView.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            Dim drcId As Integer = RadDRCDataGridView.Rows(aRow).Cells("oa_drc_id").Value
            Dim vFDRCDetailEdit As New FDRCDetailEdit
            vFDRCDetailEdit.SelectedDRCId = drcId
            vFDRCDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFDRCDetailEdit.ShowDialog() 'Modal
            If vFDRCDetailEdit.CodeRetour = True Then
                Me.Oa_drcTableAdapter.Fill(Me.DatSetDRC.oa_drc)
            End If
            vFDRCDetailEdit.Dispose()
        End If
    End Sub

    Private Sub RadDRCDataGridView_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadDRCDataGridView.CellFormatting
        Dim column As GridViewDataColumn = TryCast(e.CellElement.ColumnInfo, GridViewDataColumn)

        If column IsNot Nothing AndAlso column.FieldName = "oa_drc_oasis_categorie" Then
            Dim columnName As String = e.Column.Name
            Dim value As Object = e.Row.Cells(e.Column.Name).Value
            Select Case value
                Case 1
                    e.CellElement.Text = "Antécédent & Contexte"
                Case 2
                    e.CellElement.Text = "Stratégie"
                Case 3
                    e.CellElement.Text = "Prévention"
                Case 4
                    e.CellElement.Text = "Objectif"
            End Select
        End If
    End Sub
End Class
