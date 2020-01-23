Imports Oasis_Common
Public Class FMenuAdmin
    'Properties alimentées par l'écran d'authentification
    Private privateUtilisateurConnecte As Utilisateur
    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property


    Private Sub UniteSanitaireToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnitéSanitaireToolStripMenuItem.Click
        Dim VFMUniteSanitaire As New FmUniteSanitaire
        VFMUniteSanitaire.ShowDialog() 'Modal
        VFMUniteSanitaire.Dispose()
    End Sub

    Private Sub SiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SiteToolStripMenuItem.Click
        MessageBox.Show("Appel gestion site")
    End Sub

    Private Sub TerritoireToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TerritoireToolStripMenuItem.Click
        MessageBox.Show("Appel gestion térritoire")
    End Sub

    'Appel gestion DRC/ORC
    Private Sub DRCORCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DRCORCToolStripMenuItem.Click
        Dim vFDRCListe As New FDRCListe
        vFDRCListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vFDRCListe.ShowDialog() 'Modal
        vFDRCListe.Dispose()
    End Sub

    Private Sub RéférentielMedicamenteuxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RéférentielMédicamenteuxToolStripMenuItem.Click
        Dim vForm6 As New FMedocListe
        vForm6.ShowDialog() 'Modal
        vForm6.Dispose()
    End Sub

    Private Sub RepublicationAntecedentsOccultesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RepublicationDantécédentsOccultésToolStripMenuItem.Click
        Dim vFAntecedentOccultesListe As New FAntecedentOccultesListe
        vFAntecedentOccultesListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vFAntecedentOccultesListe.ShowDialog() 'Modal
        vFAntecedentOccultesListe.Dispose()
    End Sub

    Private Sub ParametresMetierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ParamètresMétierToolStripMenuItem.Click

    End Sub

    Private Sub DRCNouveauToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles DRCNouveauToolStripMenuItem.Click
        Dim vFDRCListe As New FmDRCListe
        vFDRCListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vFDRCListe.ShowDialog() 'Modal
        vFDRCListe.Dispose()
    End Sub

    Private Sub MédicamentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MédicamentsToolStripMenuItem.Click
        Dim vFrmMedocListe As New FrmDRCListe
        vFrmMedocListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vFrmMedocListe.ShowDialog() 'Modal
        vFrmMedocListe.Dispose()
    End Sub

    Private Sub SelecteurDRCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelecteurDRCToolStripMenuItem.Click
        Dim vRadFDRCSelecteur As New RadFDRCSelecteur
        'vFrmMedocListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vRadFDRCSelecteur.ShowDialog() 'Modal
    End Sub

    Private Sub SelecteurDRCCatégorie1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelecteurDRCCatégorie1ToolStripMenuItem.Click
        Dim vRadFDRCSelecteur As New RadFDRCSelecteur
        vRadFDRCSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
        vRadFDRCSelecteur.CategorieOasis = 1
        vRadFDRCSelecteur.ShowDialog() 'Modal
    End Sub

    Private Sub UtilisateursToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UtilisateursToolStripMenuItem.Click
        Using frmUtilisateurListe As New FrmUtilisateurListe
            frmUtilisateurListe.ShowDialog()
        End Using

    End Sub
End Class