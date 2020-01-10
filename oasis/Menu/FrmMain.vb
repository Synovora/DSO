
Public Class FrmMain
    Private Sub RadTileElementUtilisateur_Click(sender As Object, e As EventArgs) Handles RadTileElementUtilisateur.Click
        Me.Cursor = Cursors.WaitCursor
        Using frmUtilisateurListe As New FrmUtilisateurListe
            frmUtilisateurListe.ShowDialog()
        End Using
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadTileElementAPropos_Click(sender As Object, e As EventArgs) Handles RadTileElementAPropos.Click
        Me.Cursor = Cursors.WaitCursor
        Using frm As New FrmAbout
            frm.ShowDialog()
        End Using
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadTileElement1_Click(sender As Object, e As EventArgs) Handles RadTileElement1.Click
        Me.Cursor = Cursors.WaitCursor
        Using VFMUniteSanitaire As New FmUniteSanitaire
            VFMUniteSanitaire.ShowDialog() 'Modal
        End Using
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadTileDRCORC_Click(sender As Object, e As EventArgs) Handles RadTileDRCORC.Click
        Me.Cursor = Cursors.WaitCursor
        Using vFDRCListe As New RadFDrcListe
            vFDRCListe.UtilisateurConnecte = userLog
            vFDRCListe.ShowDialog() 'Modal
        End Using
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadTileRefMed_Click(sender As Object, e As EventArgs) Handles RadTileRefMed.Click
        Me.Cursor = Cursors.WaitCursor
        Using vForm6 As New FMedocListe
            vForm6.ShowDialog() 'Modal
        End Using
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadTileElementRepublication_Click(sender As Object, e As EventArgs) Handles RadTileElementRepublication.Click
        Me.Cursor = Cursors.WaitCursor
        Using vFAntecedentOccultesListe As New FAntecedentOccultesListe
            vFAntecedentOccultesListe.UtilisateurConnecte = userLog
            vFAntecedentOccultesListe.ShowDialog() 'Modal
        End Using
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadTileDRCNouveau_Click(sender As Object, e As EventArgs) Handles RadTileDRCNouveau.Click
        Me.Cursor = Cursors.WaitCursor

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadTileMedicament_Click(sender As Object, e As EventArgs) Handles RadTileMedicament.Click
        Me.Cursor = Cursors.WaitCursor
        Using vFrmMedocListe As New FMedocListe
            vFrmMedocListe.ShowDialog()
        End Using
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub RadTilePatient_Click(sender As Object, e As EventArgs) Handles RadTilePatient.Click

    End Sub

    Private Sub RadTileROR_Click(sender As Object, e As EventArgs) Handles RadTileROR.Click
        Using vRadFRorListe As New RadFRorListe
            vRadFRorListe.ShowDialog()
        End Using
    End Sub
End Class
