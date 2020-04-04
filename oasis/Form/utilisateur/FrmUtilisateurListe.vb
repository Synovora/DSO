Imports Telerik.WinControls.UI.Localization

Public Class FrmUtilisateurListe

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        ' -- permet de mettre toutes les popup standard grid en français
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        'cette ligne de code charge les données dans la table 'DataSetUtilisateur.v_user_full'. Vous pouvez la déplacer ou la supprimer selon les besoins.
        Me.V_user_fullTableAdapter.Fill(Me.DataSetUtilisateur.v_user_full)
        Me.RadGridView1.Dock = DockStyle.Fill

    End Sub

End Class
