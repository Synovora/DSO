Imports Oasis_Common

Public Class FrmUtilisateur
    Dim utilisateur As Utilisateur

    Public Sub New(utilisateur As Utilisateur)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTitleForm(Me, Me.Text)
        Me.utilisateur = utilisateur


    End Sub
End Class
