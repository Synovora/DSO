Public Class FmUniteSanitaire
    Private Sub FmUniteSanitaire_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'cette ligne de code charge les données dans la table 'UniteSanitaireDataSet.oa_site'. Vous pouvez la déplacer ou la supprimer selon les besoins.
        Me.Oa_siteTableAdapter.Fill(Me.UniteSanitaireDataSet.oa_site)
        'cette ligne de code charge les données dans la table 'UniteSanitaireDataSet.oa_unite_sanitaire'. Vous pouvez la déplacer ou la supprimer selon les besoins.
        Me.Oa_unite_sanitaireTableAdapter.Fill(Me.UniteSanitaireDataSet.oa_unite_sanitaire)

    End Sub
End Class