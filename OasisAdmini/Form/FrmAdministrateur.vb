Imports Oasis_Common
Public Class FrmAdministrateur

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTry()
    End Sub

    Private Sub BtnDebloque_Click(sender As Object, e As EventArgs) Handles BtnDebloque.Click
        If TxtPassword.Text <> "Oasis-689" Then
            MsgBox("Mot de passe admin incorrect")
            Return
        End If
        MsgBox("Poste débloqué")
        afficheTry()

    End Sub

    Private Sub afficheTry()
        Dim nb = ReadPermTry()
        LblNbTry.Text = "Nbre essai(s) en cours : " & nb & "/" & MAX_TRY
        LblNbTry.ForeColor = If(nb >= MAX_TRY, Color.LightSalmon, Color.White)
    End Sub

End Class