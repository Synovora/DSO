Imports System.Configuration
Imports System.Security.Cryptography
Imports System.Text
Imports Oasis_Common

Public Class FrmAdministrateur

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        AfficheTry()
    End Sub

    Private Sub BtnDebloque_Click(sender As Object, e As EventArgs) Handles BtnDebloque.Click
        Dim empreinteAttendue = ConfigurationManager.AppSettings("AdminUnlockPasswordSha256")

        If String.IsNullOrWhiteSpace(empreinteAttendue) Then
            MsgBox("Aucun mot de passe administrateur n'est configuré." & vbCrLf &
                   "Renseignez 'AdminUnlockPasswordSha256' dans OasisAdmini.exe.config." & vbCrLf &
                   "Voir la section 'Cryptographic key' du README.",
                   MsgBoxStyle.Exclamation)
            Return
        End If

        If Not EmpreinteSha256Egale(TxtPassword.Text, empreinteAttendue) Then
            MsgBox("Mot de passe admin incorrect")
            Return
        End If

        ResetPermission()
        MsgBox("Poste débloqué")
        AfficheTry()

    End Sub

    Private Sub AfficheTry()
        Dim nb = ReadPermTry()
        LblNbTry.Text = "Nbre essai(s) en cours : " & nb & "/" & MAX_TRY
        LblNbTry.ForeColor = If(nb >= MAX_TRY, Color.LightSalmon, Color.White)
    End Sub

End Class