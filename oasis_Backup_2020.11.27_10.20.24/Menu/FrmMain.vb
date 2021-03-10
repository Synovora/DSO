
Imports Oasis_Common

Public Class FrmMain
    Private Sub RadTileElementUtilisateur_Click(sender As Object, e As EventArgs) Handles RadTileElementUtilisateur.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using frm As New FrmUtilisateurListe
                frm.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadTileElementAPropos_Click(sender As Object, e As EventArgs) Handles RadTileElementAPropos.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using frm As New FrmAbout
                frm.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadTileElement1_Click(sender As Object, e As EventArgs) Handles RadTileElement1.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using frm As New FmUniteSanitaire
                frm.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadTileDRCORC_Click(sender As Object, e As EventArgs) Handles RadTileDRCORC.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using frm As New RadFDrcListe
                frm.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadTileRefMed_Click(sender As Object, e As EventArgs) Handles RadTileRefMed.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using frm As New RadFMedicamentSelecteur
                frm.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadTileElementRepublication_Click(sender As Object, e As EventArgs) Handles RadTileElementRepublication.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using frm As New FAntecedentOccultesListe
                frm.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadTileDRCNouveau_Click(sender As Object, e As EventArgs) Handles RadTileDRCNouveau.Click
        Me.Cursor = Cursors.WaitCursor

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadTileMedicament_Click(sender As Object, e As EventArgs) Handles RadTileMedicament.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using frm As New FMedocListe
                frm.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub RadTilePatient_Click(sender As Object, e As EventArgs) Handles RadTilePatient.Click

    End Sub

    Private Sub RadTileROR_Click(sender As Object, e As EventArgs) Handles RadTileROR.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Try
            Using frm As New RadFRorListe
                frm.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadTileElementTemplateSE_Click(sender As Object, e As EventArgs) Handles RadTileElementTemplateSE.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using formT As New FrmAdminTemplateSousEpisode()
                formT.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    Private Sub RadTileElement4_Click(sender As Object, e As EventArgs) Handles RadTileElement4.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Using formT As New FrmEtatJournalier()
                formT.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            Me.Enabled = True
        End Try
    End Sub
End Class
