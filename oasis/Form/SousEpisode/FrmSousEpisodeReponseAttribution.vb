Public Class FrmSousEpisodeReponseAttribution
    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RadButton2_Click(sender As Object, e As EventArgs) Handles RadButton2.Click

    End Sub

    Private Sub RadLabel1_Click(sender As Object, e As EventArgs) 

    End Sub

    Private Sub RadLabel2_Click(sender As Object, e As EventArgs) 

    End Sub

    Private Sub RadLabel3_Click(sender As Object, e As EventArgs)

    End Sub

    'Private Sub refreshGrid()
    '    Dim exId As Long, index As Integer = -1, exPosit = 0
    '    Me.Cursor = Cursors.WaitCursor
    '    Try
    '        Dim data As DataTable = sousEpisodeReponseDao.getTableSousEpisodeReponse(sousEpisode.Id)

    '        Dim numRowGrid As Integer = 0

    '        ' -- recup eventuelle precedente selectionnée
    '        If RadReponseGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadReponseGrid.CurrentRow) Then
    '            exId = Me.RadReponseGrid.CurrentRow.Cells("Id").Value
    '            exPosit = Me.RadReponseGrid.CurrentRow.Index
    '        End If
    '        RadReponseGrid.Rows.Clear()

    '        For Each row In data.Rows
    '            RadReponseGrid.Rows.Add(numRowGrid)
    '            '------------------- Alimentation du DataGridView
    '            With RadReponseGrid.Rows(numRowGrid)
    '                .Cells("Id").Value = row("id")
    '                If .Cells("Id").Value = exId Then index = numRowGrid   ' position exact
    '                If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
    '                .Cells("IdSousEpisode").Value = row("id_sous_episode")
    '                .Cells("HorodateCreation").Value = row("horodate_creation")
    '                .Cells("CreateUser").Value = row("user_create")
    '                .Cells("commentaire").Value = row("commentaire")
    '                .Cells("NomFichier").Value = row("nom_fichier")
    '                .Cells("NomFichier").Style.ForeColor = If(row("validate_state") = "v", Color.Green, If(row("validate_state") = "!", Color.Red, If(row("validate_state") = "m", Color.Orange, Color.Black)))
    '                '.Cells("Valider").Value = "Tests"
    '                .Cells("AskValider").ColumnInfo.IsVisible = If(userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString, True, False)

    '                'userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString
    '                ' -- on garnit le tag pour affichage tooltip
    '                '                    RadTacheToTreatGrid.Rows.Last.Tag = " << " & .Cells("type").Value & " >>" & vbCrLf &
    '                '                    If (Coalesce(row("is_ald"), False), " --> ALD" & vbCrLf, "") &
    '                '                                "Fichier : " & row("nom_fichier") & vbCrLf &
    '                '                    If (Coalesce(row("is_reponse"), False), " ... Réponse requise sous " & row("delai_since_validation") & " j à partir de la date de validation" & vbCrLf, "") &
    '                '                    If (Coalesce(row("is_reponse_recue"), False), " ... Dernière reçue le  " & row("horodate_last_recu"), " ... NON REÇUE ...") & vbCrLf &
    '                '                                " ------------------------------------------" & vbCrLf &
    '                '                    row("commentaire") & vbCrLfThenThenThen
    '            End With
    '            numRowGrid += 1
    '        Next
    '        ' -- positionnement a la ligne la plus proche de la precedente
    '        If data.Rows.Count > 0 Then
    '            Me.RadReponseGrid.CurrentRow = RadReponseGrid.Rows(If(index >= 0, index, exPosit))
    '        End If
    '        Me.Cursor = Cursors.Default


    '    Catch err As Exception
    '        MsgBox(err.Message)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try


    'End Sub
End Class
