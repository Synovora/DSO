
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class FrmSousEpisode
    Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
    Dim episode As Episode, patient As Patient

    Sub New(episode As Episode, patient As Patient)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTitleForm(Me, Me.Text)
        ' -- episode en cours
        Me.episode = episode
        Me.patient = patient
        ' 
        RadSousEpisodeGrid.Dock = DockStyle.Fill

        ' ---- init su sub grid
        RadSousEpisodeGrid.MasterTemplate.Templates(0).HierarchyDataProvider = New GridViewEventDataProvider(RadSousEpisodeGrid.MasterTemplate.Templates(0))
        AddHandler RadSousEpisodeGrid.RowSourceNeeded, AddressOf RadSousEpisodeGrid_RowSourceNeeded

        refreshGrid()

    End Sub

    Private Sub refreshGrid()
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = sousEpisodeDao.getTableSousEpisode(episode.Id, True)

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
                    .Cells("ValidationProfilTypes").Value = row("validation_profil_types")
                    .Cells("IsReponseRecue").Value = Coalesce(row("is_reponse_recue"), False)
                    .Cells("HorodateLastRecu").Value = row("horodate_last_recu")

                    ' -- on garnit le tag pour affichage tooltip
                    RadSousEpisodeGrid.Rows.Last.Tag = " << " & .Cells("type").Value & " >>" & vbCrLf & If(row("is_ald"), " --> ALD" & vbCrLf, "") &
                                 row("commentaire") & vbCrLf
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

    Private Sub RadSousEpisodeGrid_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadSousEpisodeGrid.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            ' e.CellElement.Padding = New Padding(5, 0, 0, 0)
            Try
                If e.Row.Tag <> Nothing Then e.CellElement.ToolTipText = e.Row.Tag '.ToString
            Catch ex As Exception

            End Try
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RadSousEpisodeGrid_SelectionChanged(sender As Object, e As EventArgs) Handles RadSousEpisodeGrid.SelectionChanged
        If Me.RadSousEpisodeGrid.Rows.Count = 0 OrElse Me.RadSousEpisodeGrid.CurrentRow.IsSelected = False Then
            BtnValidate.Visible = False
            BtnDetail.Visible = False
        Else
            BtnDetail.Visible = True
            BtnValidate.Visible = Me.RadSousEpisodeGrid.CurrentRow.Cells("HorodateValidate").Value Is Nothing AndAlso
                                  SousEpisodeSousType.isUserLogAutorise(Me.RadSousEpisodeGrid.CurrentRow.Cells("ValidationProfilTypes").Value)
        End If

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnValidate_Click(sender As Object, e As EventArgs) Handles BtnValidate.Click
        If MsgBox("Etes-vous sur de vouloir valider ce sous_épisode ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Validation Sous-Episode") = MsgBoxResult.Yes Then
            Dim sousEpisodeDao = New SousEpisodeDao
            sousEpisodeDao.updateValidation(Me.RadSousEpisodeGrid.CurrentRow.Cells("Id").Value)
            refreshGrid()
        End If
    End Sub

    Private Sub RadSousEpisodeGrid_RowSourceNeeded(sender As Object, e As GridViewRowSourceNeededEventArgs) Handles RadSousEpisodeGrid.RowSourceNeeded
        If Me.RadSousEpisodeGrid.Rows.Count > 0 AndAlso Me.RadSousEpisodeGrid.CurrentRow.IsSelected Then
            refreshReponseSubGrid()
        End If

    End Sub

    Private Sub refreshReponseSubGrid()
        Me.Cursor = Cursors.WaitCursor
        Dim sousEpisodeReponseDao = New SousEpisodeReponseDao
        Dim rowInfo As New GridViewDataRowInfo(RadSousEpisodeGrid.MasterTemplate.Templates(0).MasterViewInfo)
        Try
            Dim data As DataTable = sousEpisodeReponseDao.getTableSousEpisodeReponse(Me.RadSousEpisodeGrid.CurrentRow.Cells("Id").Value)

            Dim numRowGrid As Integer = 0

            RadSousEpisodeGrid.Templates(0).Rows.Clear()

            For Each row In data.Rows
                With rowInfo
                    .Cells("id").Value = row("id")
                    .Cells("IdSousEpisode").Value = row("id_sous_episode")
                    .Cells("HorodateCreation").Value = row("horodate_creation")
                    .Cells("CreateUser").Value = row("user_create")
                    .Cells("commentaire").Value = row("commentaire")
                    .Cells("NomFichier").Value = row("nom_fichier")

                    ' -- on garnit le tag pour affichage tooltip
                    'RadSousEpisodeGrid.Templates(0).Rows.Last.Tag = " << " & .Cells("type").Value & " >>" & vbCrLf & If(row("is_ald"), " --> ALD" & vbCrLf, "") &
                    ' row("commentaire") & vbCrLf
                End With
                RadSousEpisodeGrid.Templates(0).Rows.Add(numRowGrid, rowInfo)                '------------------- Alimentation du DataGridView

                numRowGrid += 1

            Next
            Me.Cursor = Cursors.Default


        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnDetail_Click(sender As Object, e As EventArgs) Handles BtnDetail.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using frm = New FrmTestRichText()
                frm.ShowDialog()
                frm.Dispose()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

End Class
