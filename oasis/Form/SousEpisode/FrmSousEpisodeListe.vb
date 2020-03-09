
Imports System.IO
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class FrmSousEpisodeListe
    Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
    Dim sousEpisodeReponseDao As SousEpisodeReponseDao = New SousEpisodeReponseDao
    Dim episode As Episode, patient As Patient

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="episode"></param>
    ''' <param name="patient"></param>
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
        RadSousEpisodeGrid.Templates(0).HierarchyDataProvider = New GridViewEventDataProvider(RadSousEpisodeGrid.Templates(0))
        AddHandler RadSousEpisodeGrid.RowSourceNeeded, AddressOf RadSousEpisodeGrid_RowSourceNeeded
        '-- handler sur bouton sous_grid
        AddHandler RadSousEpisodeGrid.CommandCellClick, AddressOf subGridReponse_CommandCellClick
        refreshGrid()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub refreshGrid()
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = sousEpisodeDao.getTableSousEpisode(episode.Id,, True)

            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadSousEpisodeGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadSousEpisodeGrid.CurrentRow) Then
                exId = Me.RadSousEpisodeGrid.CurrentRow.Cells("IdSousEpisode").Value
                exPosit = Me.RadSousEpisodeGrid.CurrentRow.Index
            End If
            RadSousEpisodeGrid.Rows.Clear()

            For Each row In data.Rows
                RadSousEpisodeGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadSousEpisodeGrid.Rows(numRowGrid)
                    .Cells("IdSousEpisode").Value = row("id")
                    If .Cells("IdSousEpisode").Value = exId Then index = numRowGrid   ' position exact
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
                    .Cells("IsReponse").Value = row("is_reponse")
                    .Cells("ValidationProfilTypes").Value = row("validation_profil_types")
                    .Cells("IsReponseRecue").Value = Coalesce(row("is_reponse_recue"), False)
                    .Cells("HorodateLastRecu").Value = row("horodate_last_recu")

                    ' -- on garnit le tag pour affichage tooltip
                    RadSousEpisodeGrid.Rows.Last.Tag = " << " & .Cells("type").Value & " >>" & vbCrLf &
                                If(Coalesce(row("is_ald"), False), " --> ALD" & vbCrLf, "") &
                                "Fichier : " & row("nom_fichier") & vbCrLf &
                                If(Coalesce(row("is_reponse"), False), " ... Réponse requise sous " & row("delai_since_validation") & " j à partir de la date de validation" & vbCrLf, "") &
                                If(Coalesce(row("is_reponse_recue"), False), " ... Dernière reçue le  " & row("horodate_last_recu"), " ... NON REÇUE ...") & vbCrLf &
                                " ------------------------------------------" & vbCrLf &
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
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
        If Me.RadSousEpisodeGrid.CurrentRow Is Nothing _
                OrElse Me.RadSousEpisodeGrid.Rows.Count = 0 _
                OrElse Me.RadSousEpisodeGrid.CurrentRow.IsSelected = False Then
            BtnValidate.Visible = False
            BtnDetail.Visible = False
        Else
            BtnDetail.Visible = True
            BtnValidate.Visible = TryCast(Me.RadSousEpisodeGrid.CurrentRow.Parent, MasterGridViewTemplate) IsNot Nothing _
                                  AndAlso Me.RadSousEpisodeGrid.CurrentRow.Cells("HorodateValidate").Value Is Nothing _
                                  AndAlso SousEpisodeSousType.isUserLogAutorise(Me.RadSousEpisodeGrid.CurrentRow.Cells("ValidationProfilTypes").Value)
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
            sousEpisodeDao.updateValidation(Me.RadSousEpisodeGrid.CurrentRow.Cells("IdSousEpisode").Value)
            refreshGrid()
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RadSousEpisodeGrid_RowSourceNeeded(sender As Object, e As GridViewRowSourceNeededEventArgs) Handles RadSousEpisodeGrid.RowSourceNeeded
        If Me.RadSousEpisodeGrid.Rows.Count > 0 AndAlso Me.RadSousEpisodeGrid.CurrentRow.IsSelected Then
            refreshReponseSubGrid(e)
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Sub subGridReponse_CommandCellClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim gce As GridCommandCellElement = (TryCast(sender, GridCommandCellElement))
        'MessageBox.Show("Telecharger fichier " & gce.RowInfo.Cells("NomFichier").Value & " : " & gce.RowInfo.Cells("IdSousEpisode").Value & "_" & gce.RowInfo.Cells("Id").Value)
        Dim sousEpisodeReponse As SousEpisodeReponse
        Try
            Me.Cursor = Cursors.WaitCursor
            sousEpisodeReponse = sousEpisodeReponseDao.getById(gce.RowInfo.Cells("Id").Value)

            Dim tbl As Byte() = sousEpisodeReponseDao.getContenu(episode.Id, sousEpisodeReponse)
            Me.Cursor = Cursors.Default
            SaveFileDialog1.FileName = sousEpisodeReponse.NomFichier
            Select Case (SaveFileDialog1.ShowDialog())
                Case DialogResult.Abort, DialogResult.Cancel
                    Notification.show("Réponse Sous-épisode", "Téléchargement abandonné !")
                Case DialogResult.OK, DialogResult
                    File.WriteAllBytes(SaveFileDialog1.FileName, tbl)
                    Notification.show("Réponse Sous-épisode", "Téléchargement de " & SaveFileDialog1.FileName & " Terminé !")
            End Select

        Catch err As Exception
            MsgBox(err.Message())
            Return
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    Private Sub refreshReponseSubGrid(ByVal e As GridViewRowSourceNeededEventArgs)
        Me.Cursor = Cursors.WaitCursor
        Dim rowView As DataRowView = TryCast(e.ParentRow.DataBoundItem, DataRowView)

        Try
            Dim data As DataTable = sousEpisodeReponseDao.getTableSousEpisodeReponse(Me.RadSousEpisodeGrid.CurrentRow.Cells("IdSousEpisode").Value)
            e.SourceCollection.Clear()

            For Each row In data.Rows
                Dim rowInfo As GridViewRowInfo = e.Template.Rows.NewRow()
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
                e.SourceCollection.Add(rowInfo)                '------------------- Alimentation du DataGridView
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
    Private Sub BtnCreate_Click(sender As Object, e As EventArgs) Handles BtnCreate.Click
        Dim sousEpisode = New SousEpisode
        ficheSousEpisode(sousEpisode, userLog.UtilisateurPrenom + " " + userLog.UtilisateurNom, Nothing, Nothing)

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RadSousEpisodeGrid_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RadSousEpisodeGrid.MouseDoubleClick
        editSousEpisode()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnDetail_Click(sender As Object, e As EventArgs) Handles BtnDetail.Click
        editSousEpisode()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub editSousEpisode()
        If Me.RadSousEpisodeGrid.Rows.Count = 0 OrElse Me.RadSousEpisodeGrid.CurrentRow.IsSelected = False Then Return

        Dim sousEpisode As SousEpisode
        Try
            Me.Cursor = Cursors.WaitCursor
            sousEpisode = sousEpisodeDao.getById(Me.RadSousEpisodeGrid.CurrentRow.Cells("IdSousEpisode").Value)
        Catch err As Exception
            MsgBox(err.Message())
            Return
        Finally
            Me.Cursor = Cursors.Default
        End Try
        ' -- si selection = subgrid => on prend le parent
        Dim gridViewRow As GridViewRowInfo = If(TryCast(Me.RadSousEpisodeGrid.CurrentRow.Parent, MasterGridViewTemplate) Is Nothing, Me.RadSousEpisodeGrid.CurrentRow.Parent, Me.RadSousEpisodeGrid.CurrentRow)
        With gridViewRow
            ficheSousEpisode(sousEpisode, .Cells("CreateUser").Value, .Cells("LastUpdateUser").Value, .Cells("ValidateUser").Value)
        End With

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sousEpisode"></param>
    ''' <param name="userCreateNom"></param>
    ''' <param name="userUpdateNom"></param>
    ''' <param name="userValidateNom"></param>
    Private Sub ficheSousEpisode(sousEpisode As SousEpisode, userCreateNom As String, userUpdateNom As String, userValidateNom As String)

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using frm = New FrmSousEpisode(episode, patient, sousEpisode, userCreateNom, userUpdateNom, userValidateNom)
                frm.ShowDialog()
                frm.Dispose()
            End Using
            refreshGrid()
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

End Class
