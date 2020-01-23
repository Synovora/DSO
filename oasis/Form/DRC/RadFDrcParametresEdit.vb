Imports Telerik.WinControls.UI
Imports Oasis_Common

Public Class RadFDrcParametresEdit
    Private _drcId As Long

    Public Property DrcId As Long
        Get
            Return _drcId
        End Get
        Set(value As Long)
            _drcId = value
        End Set
    End Property

    Dim parametreDrcDao As New ParametreDrcDao

    Private Sub RadFDrcParametresEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim drcdao As New DrcDao
        Dim drc As New Drc
        drcdao.GetDrc(drc, DrcId)
        LblDrcDenomination.Text = drc.DrcLibelle
        ChargementParametres()
    End Sub

    Private Sub ChargementParametres()
        Dim parametreDao As New ParametreDao
        Dim parametreDrc As ParametreDrc
        Dim parametreDt As DataTable
        parametreDt = parametreDao.GetAllParametre()
        Dim ParametreId As Long

        Dim i As Integer
        Dim rowCount As Integer = parametreDt.Rows.Count - 1
        Dim iGrid As Integer = -1

        For i = 0 To rowCount Step 1
            iGrid += 1

            RadGridViewParm.Rows.Add(iGrid)
            RadGridViewParm.Rows(iGrid).Cells("id").Value = parametreDt.Rows(i)("id")
            RadGridViewParm.Rows(iGrid).Cells("description").Value = parametreDt.Rows(i)("description")
            RadGridViewParm.Rows(iGrid).Cells("unite").Value = parametreDt.Rows(i)("unite")
            ParametreId = Coalesce(parametreDt.Rows(i)("id"), 0)
            parametreDrc = parametreDrcDao.GetParametreByDrcAndId(Me.DrcId, ParametreId)
            If parametreDrc.Id <> 0 Then
                RadGridViewParm.Rows(iGrid).Cells("selection").Value = True
            Else
                RadGridViewParm.Rows(iGrid).Cells("selection").Value = False
            End If
        Next
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        Dim selection As Boolean
        Dim ParametreId As Long
        'Dim lastRow As GridViewRowInfo = RadGridViewParm.Rows(RadGridViewParm.Rows.Count - 1)
        'lastRow.IsSelected = True

        'Suppression de tous les parametres DRC
        parametreDrcDao.SuppressionParametreDrcByDrcId(Me.DrcId)

        For Each rowInfo As GridViewRowInfo In RadGridViewParm.Rows
            ParametreId = 0
            selection = False
            For Each cellInfo As GridViewCellInfo In rowInfo.Cells
                If (cellInfo.ColumnInfo.Name = "selection") Then
                    If cellInfo.Value <> Nothing Then
                        selection = cellInfo.Value
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "id") Then
                    If cellInfo.Value <> Nothing Then
                        ParametreId = cellInfo.Value
                    End If
                End If
            Next
            If selection = True And ParametreId <> 0 Then
                'Création des parametres DRC sélectionnés
                Dim parametredrc As ParametreDrc = New ParametreDrc
                parametredrc.Id = 0
                parametredrc.DrcId = Me.DrcId
                parametredrc.ParametreId = ParametreId
                parametreDrcDao.CreationParametreDrc(parametredrc)
            End If
        Next

        MessageBox.Show("Les paramètres ont été mis à jour")
        Close()

    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
