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

        If Not EmpreinteEgale(TxtPassword.Text, empreinteAttendue) Then
            MsgBox("Mot de passe admin incorrect")
            Return
        End If

        ResetPermission()
        MsgBox("Poste débloqué")
        AfficheTry()

    End Sub

    ''' <summary>
    ''' Compare le mot de passe saisi à l'empreinte SHA-256 attendue (hexadécimal).
    ''' Le mot de passe était auparavant écrit en clair dans ce fichier, et cette
    ''' valeur est désormais publique : elle doit être changée, pas seulement déplacée.
    ''' </summary>
    Private Shared Function EmpreinteEgale(saisie As String, empreinteAttendue As String) As Boolean
        If saisie Is Nothing Then Return False

        Dim calculee As String
        Using sha As SHA256 = SHA256.Create()
            calculee = BitConverter.ToString(
                sha.ComputeHash(Encoding.UTF8.GetBytes(saisie))).Replace("-", "")
        End Using

        Dim attendue = empreinteAttendue.Trim().Replace("-", "")
        If calculee.Length <> attendue.Length Then Return False

        ' Comparaison à temps constant : ne pas révéler la position du premier écart.
        Dim ecart As Integer = 0
        For i = 0 To calculee.Length - 1
            ecart = ecart Or (Asc(Char.ToUpperInvariant(calculee(i))) Xor
                              Asc(Char.ToUpperInvariant(attendue(i))))
        Next
        Return ecart = 0
    End Function

    Private Sub AfficheTry()
        Dim nb = ReadPermTry()
        LblNbTry.Text = "Nbre essai(s) en cours : " & nb & "/" & MAX_TRY
        LblNbTry.ForeColor = If(nb >= MAX_TRY, Color.LightSalmon, Color.White)
    End Sub

End Class