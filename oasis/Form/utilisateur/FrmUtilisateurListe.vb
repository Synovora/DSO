Imports Telerik.WinControls.UI.Localization

Public Class FrmUtilisateurListe
    Private Sub FrmUtilisateurListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' -- permet de mettre toutes les popup standard grid en français
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        'cette ligne de code charge les données dans la table 'DataSetUtilisateur.v_user_full'. Vous pouvez la déplacer ou la supprimer selon les besoins.
        Me.V_user_fullTableAdapter.Fill(Me.DataSetUtilisateur.v_user_full)
        Me.RadGridView1.Dock = DockStyle.Fill
    End Sub
End Class
