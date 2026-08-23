Imports Oasis_Common

Public Class RadFParametreSelecteur
    Private _selectedParametre As Parametre
    Private _isSelected As Boolean
    Private _ListeParametreExistant As List(Of Long)

    Dim parametreDao As New ParametreDao

    Dim SelectedParametreId As Long = 0

    Public Property SelectedParametre As Parametre
        Get
            Return _selectedParametre
        End Get
        Set(value As Parametre)
            _selectedParametre = value
        End Set
    End Property

    Public Property IsSelected As Boolean
        Get
            Return _isSelected
        End Get
        Set(value As Boolean)
            _isSelected = value
        End Set
    End Property

    Public Property ListeParametreExistant As List(Of Long)
        Get
            Return _ListeParametreExistant
        End Get
        Set(value As List(Of Long))
            _ListeParametreExistant = value
        End Set
    End Property

    Private Sub RadFParametreSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGbxSelection.Hide()
        IsSelected = False

        Dim parmDataTable As DataTable
        parmDataTable = parametreDao.GetAllParametre()
        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = parmDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Dim parametreId As Long = parmDataTable.Rows(i)("id")
            If ListeParametreExistant.Contains(parametreId) Then
                Continue For
            End If
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadGridViewParm.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadGridViewParm.Rows(iGrid).Cells("id").Value = parmDataTable.Rows(i)("id")
            RadGridViewParm.Rows(iGrid).Cells("description").Value = parmDataTable.Rows(i)("description")
            RadGridViewParm.Rows(iGrid).Cells("unite").Value = parmDataTable.Rows(i)("unite")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewParm.Rows.Count > 0 Then
            Me.RadGridViewParm.CurrentRow = RadGridViewParm.ChildRows(0)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        IsSelected = False
        Close()
    End Sub

    Private Sub RadBtnSelection_Click(sender As Object, e As EventArgs) Handles RadBtnSelection.Click
        SelectionConfirmation()
    End Sub

    Private Sub SelectionConfirmation()
        Dim parametre As Parametre = New Parametre
        parametre = parametreDao.GetParametreById(SelectedParametreId)
        SelectedParametre = parametre
        IsSelected = True
        Close()
    End Sub

    Private Sub RadGridViewParm_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridViewParm.CellClick
        Selection()
    End Sub

    Private Sub Selection()
        Dim aRow, maxRow As Integer

        aRow = Me.RadGridViewParm.Rows.IndexOf(Me.RadGridViewParm.CurrentRow)
        maxRow = RadGridViewParm.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            'If RadDrcDataGridView.CurrentRow IsNot Nothing Then
            SelectedParametreId = RadGridViewParm.Rows(aRow).Cells("Id").Value

            TbxDescription.Text = RadGridViewParm.Rows(aRow).Cells("description").Value
            TbxUnite.Text = RadGridViewParm.Rows(aRow).Cells("unite").Value

            If TbxDescription.Text <> "" Then
                RadGbxSelection.Show()
            End If
        End If
    End Sub

    Private Sub RadGridViewParm_DoubleClick(sender As Object, e As EventArgs) Handles RadGridViewParm.DoubleClick
        Selection()
        SelectionConfirmation()
    End Sub
End Class
