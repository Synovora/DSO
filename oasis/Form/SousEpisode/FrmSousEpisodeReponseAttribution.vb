Imports Oasis_Common
Imports Telerik.WinControls.UI

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
    Sub New()
        refreshGrid()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub refreshGrid()
        Me.Cursor = Cursors.WaitCursor
        Dim sousEpisodeReponseMailDao As SousEpisodeReponseMailDao = New SousEpisodeReponseMailDao
        Try
            Dim lstMail As List(Of SousEpisodeReponseMail) = sousEpisodeReponseMailDao.GetLstSousEpisodeReponseMail()
            Dim numRowGrid As Integer = 0
            ' -- recup eventuelle precedente selectionnée
            'If RadSousEpisodeGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadSousEpisodeGrid.CurrentRow) Then
            '    exId = Me.RadSousEpisodeGrid.CurrentRow.Cells("IdSousEpisode").Value
            '    exPosit = Me.RadSousEpisodeGrid.CurrentRow.Index
            'End If
            RadMailGridView.Rows.Clear()
            For Each mail In lstMail
                Dim newRow As GridViewRowInfo = RadMailGridView.Rows.NewRow()
                '------------------- Alimentation du DataGridView
                With newRow
                    .Cells("auteur").Value = mail.Auteur

                End With
                RadMailGridView.Rows.Add(newRow)
                numRowGrid += 1

            Next
            Me.Cursor = Cursors.Default
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
End Class
