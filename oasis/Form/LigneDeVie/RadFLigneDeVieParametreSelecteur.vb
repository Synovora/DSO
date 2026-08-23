Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Oasis_Common
Imports System.Configuration
Public Class RadFLigneDeVieParametreSelecteur
    Public Property ListeParametreaAfficher As List(Of Long)

    Dim parametreDao As New ParametreDao

    Private Sub RadFLigneDeVieParametreSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementParametres()
        ChargementParametresDisponibles()
    End Sub

    Private Sub ChargementParametres()
        RadGridViewParm.Rows.Clear()

        If listeParametreaAfficher.Count >= 5 Then
            RadBtnSelect.Hide()
        Else
            RadBtnSelect.Show()
        End If

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim ParametreEnumerator As List(Of Long).Enumerator = ListeParametreaAfficher.GetEnumerator()

        While ParametreEnumerator.MoveNext()
            Dim parametre As Parametre
            Dim parametreId As Long = ParametreEnumerator.Current
            parametre = parametreDao.GetParametreById(parametreId)
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadGridViewParm.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadGridViewParm.Rows(iGrid).Cells("id").Value = parametre.Id
            RadGridViewParm.Rows(iGrid).Cells("description").Value = parametre.Description
            RadGridViewParm.Rows(iGrid).Cells("unite").Value = parametre.Unite
        End While

        'Positionnement du grid sur la première occurrence
        If RadGridViewParm.Rows.Count > 0 Then
            Me.RadGridViewParm.CurrentRow = RadGridViewParm.ChildRows(0)
        End If
    End Sub

    Private Sub ChargementParametresDisponibles()
        RadGridViewParmDispo.Rows.Clear()

        Dim parmDataTable As DataTable
        parmDataTable = parametreDao.GetAllParametre()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = parmDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Dim parametreId As Long = parmDataTable.Rows(i)("id")
            If listeParametreaAfficher.Contains(parametreId) Then
                Continue For
            End If
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadGridViewParmDispo.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadGridViewParmDispo.Rows(iGrid).Cells("id").Value = parmDataTable.Rows(i)("id")
            RadGridViewParmDispo.Rows(iGrid).Cells("description").Value = parmDataTable.Rows(i)("description")
            RadGridViewParmDispo.Rows(iGrid).Cells("unite").Value = parmDataTable.Rows(i)("unite")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewParmDispo.Rows.Count > 0 Then
            Me.RadGridViewParmDispo.CurrentRow = RadGridViewParmDispo.ChildRows(0)
        End If
    End Sub

    'Ajouter un paramètre
    Private Sub RadBtnSelect_Click(sender As Object, e As EventArgs) Handles RadBtnSelect.Click
        Dim aRow, maxRow As Integer

        aRow = Me.RadGridViewParmDispo.Rows.IndexOf(Me.RadGridViewParmDispo.CurrentRow)
        maxRow = RadGridViewParmDispo.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            Dim SelectedParametreId As Long = RadGridViewParmDispo.Rows(aRow).Cells("Id").Value
            ListeParametreaAfficher.Add(SelectedParametreId)

            ChargementParametres()
            ChargementParametresDisponibles()
        End If
    End Sub

    'Enlever un paramètre
    Private Sub RadBtnSuppprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSuppprimer.Click
        Dim aRow, maxRow As Integer

        aRow = Me.RadGridViewParm.Rows.IndexOf(Me.RadGridViewParm.CurrentRow)
        maxRow = RadGridViewParm.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            Dim SelectedParametreId As Long = RadGridViewParm.Rows(aRow).Cells("Id").Value
            ListeParametreaAfficher.Remove(SelectedParametreId)

            ChargementParametres()
            ChargementParametresDisponibles()
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
