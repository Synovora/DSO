
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class FrmSousEpisode
    Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
    Dim idEpisode As Long

    Sub New(_idEpisode As Long)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTitleForm(Me, Me.Text)
        ' -- episode en cours
        Me.idEpisode = _idEpisode

        refreshGrid()

    End Sub

    Private Sub refreshGrid()
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Dim strToolTip As String
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = sousEpisodeDao.getTableSousEpisode(idEpisode, True)

            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadSousEpisodeGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadSousEpisodeGrid.CurrentRow) Then
                exId = Me.RadSousEpisodeGrid.CurrentRow.Cells("id").Value
                exPosit = Me.RadSousEpisodeGrid.CurrentRow.Index
            End If
            RadSousEpisodeGrid.Rows.Clear()

            For Each row In data.Rows
                RadSousEpisodeGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadSousEpisodeGrid.Rows(numRowGrid)
                    .Cells("id").Value = row("id")
                    If .Cells("id").Value = exId Then index = numRowGrid   ' position exact
                    If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
                    .Cells("HorodateCreation").Value = row("horodate_creation")
                    .Cells("CreateUser").Value = row("user_create")
                    .Cells("type").Value = row("type_libelle") & If(row("sous_type_libelle") <> row("type_libelle"), "/" + row("sous_type_libelle"), "")
                    .Cells("HorodateLastUpdate").Value = row("horodate_last_update")
                    .Cells("LastUpdateUser").Value = row("user_update")
                    .Cells("HorodateValidate").Value = row("horodate_validate")
                    .Cells("ValidateUser").Value = row("user_validate")
                    .Cells("commentaire").Value = row("commentaire")
                    .Cells("NomFichier").Value = row("nom_fichier")
                    .Cells("IsAld").Value = row("is_ald")
                    strToolTip = " << " & .Cells("type").Value & " >>" & vbCrLf

                    strToolTip += row("commentaire") & vbCrLf
                    RadSousEpisodeGrid.Rows.Last.Tag = strToolTip
                End With

                numRowGrid += 1

            Next
            ' -- positionnement a la ligne la plus proche de la precedente
            If data.Rows.Count > 0 Then
                'Me.RadSousEpisodeGrid.CurrentRow = RadSousEpisodeGrid.ChildRows(If(index >= 0, index, exPosit))
                Me.RadSousEpisodeGrid.CurrentRow = RadSousEpisodeGrid.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default


        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Close()
    End Sub

    Private Sub RadSousEpisodeGrid_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadSousEpisodeGrid.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            ' e.CellElement.Padding = New Padding(5, 0, 0, 0)
            Try
                If e.Row.Tag <> Nothing Then e.CellElement.ToolTipText = e.Row.Tag '.ToString
            Catch ex As Exception

            End Try
        End If

    End Sub

    '    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
    '    Try
    '    Me.Cursor = Cursors.WaitCursor
    '    Me.Enabled = False
    '    Using frm = New FrmTestRichText()
    '                frm.ShowDialog()
    '                frm.Dispose()
    '    End Using
    '    Catch err As Exception
    '            MsgBox(err.Message())
    '    Finally
    '    Me.Enabled = True
    '    Me.Cursor = Cursors.Default
    '    End Try

    '    End Sub


End Class
