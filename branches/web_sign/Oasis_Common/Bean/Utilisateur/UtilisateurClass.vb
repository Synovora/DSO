Imports System.Security.Cryptography
Imports Nethereum.Signer

Public Class Utilisateur
    Public Property UtilisateurId As Integer
    Public Property UtilisateurNom As String
    Public Property UtilisateurPrenom As String
    Public Property UtilisateurProfilId As String
    Public Property UtilisateurRPPS As String
    Public Property UtilisateurAdmin As Boolean
    Public Property UtilisateurLogin As String
    Public Property UtilisateurSiegeId As Integer
    Public Property UtilisateurSiteId As Integer
    Public Property UtilisateurUniteSanitaireId As Integer
    Public Property UtilisateurNiveauAcces As Integer
    Public Property Password As String
    Public Property IsPasswordUniqueUsage As Boolean
    Public Property LstFonction As List(Of Fonction)
    Public Property TypeProfil As String
    Public Property FonctionParDefautId As Long
    Public Property UtilisateurTelephone As String
    Public Property UtilisateurFax As String
    Public Property UtilisateurMail As String
    Public Property UtilisateurClePrivee As String
    Public Property UtilisateurAddress As String


    Public Sub New()
        Me.UtilisateurId = 0
        Me.UtilisateurSiteId = 0
        Me.UtilisateurUniteSanitaireId = 0
        Me.UtilisateurNiveauAcces = 0
        Me.UtilisateurNom = ""
        Me.UtilisateurPrenom = ""
        Me.UtilisateurProfilId = ""
        Me.UtilisateurAdmin = False
        Me.UtilisateurLogin = ""
        Me.UtilisateurTelephone = ""
        Me.UtilisateurFax = ""
        Me.UtilisateurMail = ""
        Me.UtilisateurClePrivee = ""
        Me.UtilisateurAddress = ""
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="login"></param>
    ''' <param name="pwd"></param>
    ''' <returns></returns>
    Public Shared Function CryptePwd(login As String, pwd As String) As String
        Dim UniEnc As New System.Text.UnicodeEncoding
        Dim bitPass() As Byte = UniEnc.GetBytes("U23cGt'r8c" + login + pwd)
        Using sha As New SHA1CryptoServiceProvider
            Return Convert.ToBase64String(sha.ComputeHash(bitPass))
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function CryptePwd() As String
        Me.Password = CryptePwd(UtilisateurLogin, Password)
        Return Password
    End Function

    Public Function Clone() As Utilisateur
        Dim newInstance As Utilisateur = DirectCast(Me.MemberwiseClone(), Utilisateur)
        Return newInstance
    End Function

    Public Function IsFonctionIdPossible(idFonction As Long) As Boolean
        If IsNothing(LstFonction) Then Return False
        For Each fonction In LstFonction
            If fonction.Id = idFonction Then Return True
        Next
        Return False
    End Function

    Public Function Sign(data As Byte()) As String
        Dim signer As MessageSigner = New MessageSigner()
        Dim signature As String = signer.HashAndSign(data, UtilisateurClePrivee)

        'Verification de la signature
        'If signer.EcRecover(hash, signature) <> UtilisateurAddress Then
        'signature = ""
        'End 
        Return signature
    End Function

End Class
