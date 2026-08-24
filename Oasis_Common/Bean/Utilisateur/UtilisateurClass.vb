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
    ''' Signature confiée au serveur, installée par le client lourd au démarrage.
    ''' Reçoit la charge à signer, renvoie la signature et l'adresse du signataire.
    '''
    ''' Le client n'a plus la clé privée : elle ne figure plus dans la réponse de
    ''' /api/login et la lecture de la colonne lui est refusée par la base. C'est
    ''' le même mécanisme de crochet que StandardDao.DemanderNouvelEssaiConnexion,
    ''' de sorte que les appelants de Sign n'ont pas à savoir où la clé se trouve.
    ''' </summary>
    Public Shared Property SignataireDistant As Func(Of Byte(), SignatureResponse) = Nothing

    ''' <summary>
    ''' Signe les données au nom de l'utilisateur.
    '''
    ''' Côté serveur, la clé privée est chargée et la signature calculée sur place.
    ''' Côté client, elle est demandée à /api/signature via SignataireDistant.
    ''' Échoue dans tous les cas si aucune clé n'est disponible : signer avec une
    ''' clé de repli rendrait la signature falsifiable par quiconque la connaît.
    ''' </summary>
    Public Function Sign(data As Byte()) As String
        If Not String.IsNullOrWhiteSpace(UtilisateurClePrivee) Then
            Dim signer As MessageSigner = New MessageSigner()
            Return signer.HashAndSign(data, UtilisateurClePrivee)
        End If

        Dim distant = SignataireDistant
        If distant Is Nothing Then
            Throw New InvalidOperationException(
                "Aucune clé de signature n'est disponible pour l'utilisateur " &
                UtilisateurLogin & " (id " & UtilisateurId & "). " &
                "Générez une clé depuis la fiche utilisateur avant de signer.")
        End If

        Dim reponse = distant(data)
        If reponse Is Nothing OrElse String.IsNullOrWhiteSpace(reponse.Signature) Then
            Throw New InvalidOperationException(
                "Le serveur n'a pas pu signer pour l'utilisateur " & UtilisateurLogin & ".")
        End If

        ' L'adresse renvoyée par le serveur fait foi : c'est celle de la clé qui
        ' vient effectivement de signer. Elle est enregistrée à côté de la
        ' signature, donc une rotation entre la connexion et la signature ne rend
        ' pas l'ordonnance invérifiable.
        Me.UtilisateurAddress = reponse.Adresse
        Return reponse.Signature
    End Function

End Class
