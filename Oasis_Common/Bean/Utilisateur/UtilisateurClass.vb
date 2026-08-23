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
    ''' <summary>Nombre d'échecs d'authentification consécutifs (verrouillage serveur).</summary>
    Public Property Tentatives As Integer
    ''' <summary>Date jusqu'à laquelle le compte est verrouillé. Nothing si non verrouillé.</summary>
    Public Property VerrouJusqua As Date?

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
    ''' Ancien calcul d'empreinte (SHA-1, poivre constant, sans sel). Conservé
    ''' UNIQUEMENT pour vérifier les empreintes déjà en base et les migrer à la
    ''' première connexion. Ne jamais l'utiliser pour enregistrer un mot de passe :
    ''' voir MotDePasse.Hacher.
    ''' </summary>
    Public Shared Function CryptePwd(login As String, pwd As String) As String
        Dim UniEnc As New Text.UnicodeEncoding
        Dim bitPass() As Byte = UniEnc.GetBytes("U23cGt'r8c" + login + pwd)
        Using sha As New SHA1CryptoServiceProvider
            Return Convert.ToBase64String(sha.ComputeHash(bitPass))
        End Using
    End Function

    ''' <summary>
    ''' Remplace le mot de passe en clair porté par le bean par son empreinte
    ''' PBKDF2, prête à être enregistrée.
    ''' </summary>
    Public Function CryptePwd() As String
        Me.Password = MotDePasse.Hacher(Password)
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

    ''' <summary>
    ''' Signe les données avec la clé privée de l'utilisateur.
    ''' Échoue si aucune clé n'est enregistrée : signer avec une clé de repli
    ''' rendrait la signature falsifiable par quiconque connaît cette clé.
    ''' </summary>
    Public Function Sign(data As Byte()) As String
        If String.IsNullOrWhiteSpace(UtilisateurClePrivee) Then
            Throw New InvalidOperationException(
                "Aucune clé de signature n'est enregistrée pour l'utilisateur " &
                UtilisateurLogin & " (id " & UtilisateurId & "). " &
                "Générez une clé depuis la fiche utilisateur avant de signer.")
        End If
        Dim signer As MessageSigner = New MessageSigner()
        Return signer.HashAndSign(data, UtilisateurClePrivee)
    End Function

End Class
