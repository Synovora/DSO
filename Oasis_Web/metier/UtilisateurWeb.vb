Imports System.Security.Cryptography
Imports Oasis_Web

Public Class Utilisateur
    Private privateUtilisateurId As Integer
    Private privateUtilisateurNom As String
    Private privateUtilisateurPrenom As String
    Private privateUtilisateurProfilId As String
    Private privateUtilisateurPosteId As String
    Private privateUtilisateurAdmin As Boolean
    Private privateUtilisateurLogin As String
    Private privateUtilisateurSiteId As Integer
    Private privateUtilisateurUniteSanitaireId As Integer
    Private privateUtilisateurNiveauAcces As Integer
    Private _typeProfil As String
    Private _FonctionParDefautId As Long
    Private _password As String
    Private _lstFonction As List(Of Fonction)

    Public Sub New()
        Me.UtilisateurId = 0
        Me.UtilisateurNom = ""
        Me.UtilisateurPrenom = ""
        Me.UtilisateurProfilId = ""
        Me.UtilisateurPosteId = ""
        Me.UtilisateurAdmin = False
        Me.UtilisateurLogin = ""
        Me.UtilisateurSiteId = 0
        Me.UtilisateurUniteSanitaireId = 0
        Me.UtilisateurNiveauAcces = 0
    End Sub
    Public Property UtilisateurId As Integer
        Get
            Return privateUtilisateurId
        End Get
        Set(value As Integer)
            privateUtilisateurId = value
        End Set
    End Property

    Public Property UtilisateurNom As String
        Get
            Return privateUtilisateurNom
        End Get
        Set(value As String)
            privateUtilisateurNom = value
        End Set
    End Property

    Public Property UtilisateurPrenom As String
        Get
            Return privateUtilisateurPrenom
        End Get
        Set(value As String)
            privateUtilisateurPrenom = value
        End Set
    End Property

    Public Property UtilisateurProfilId As String
        Get
            Return privateUtilisateurProfilId
        End Get
        Set(value As String)
            privateUtilisateurProfilId = value
        End Set
    End Property

    Public Property UtilisateurPosteId As String
        Get
            Return privateUtilisateurPosteId
        End Get
        Set(value As String)
            privateUtilisateurPosteId = value
        End Set
    End Property

    Public Property UtilisateurAdmin As Boolean
        Get
            Return privateUtilisateurAdmin
        End Get
        Set(value As Boolean)
            privateUtilisateurAdmin = value
        End Set
    End Property

    Public Property UtilisateurLogin As String
        Get
            Return privateUtilisateurLogin
        End Get
        Set(value As String)
            privateUtilisateurLogin = value
        End Set
    End Property

    Public Property UtilisateurSiteId As Integer
        Get
            Return privateUtilisateurSiteId
        End Get
        Set(value As Integer)
            privateUtilisateurSiteId = value
        End Set
    End Property

    Public Property UtilisateurUniteSanitaireId As Integer
        Get
            Return privateUtilisateurUniteSanitaireId
        End Get
        Set(value As Integer)
            privateUtilisateurUniteSanitaireId = value
        End Set
    End Property

    Public Property UtilisateurNiveauAcces As Integer
        Get
            Return privateUtilisateurNiveauAcces
        End Get
        Set(value As Integer)
            privateUtilisateurNiveauAcces = value
        End Set
    End Property


    Public Property Password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property

    Public Property LstFonction As List(Of Fonction)
        Get
            Return _lstFonction
        End Get
        Set(value As List(Of Fonction))
            _lstFonction = value
        End Set
    End Property

    Public Property TypeProfil As String
        Get
            Return _typeProfil
        End Get
        Set(value As String)
            _typeProfil = value
        End Set
    End Property

    Public Property FonctionParDefautId As Long
        Get
            Return _FonctionParDefautId
        End Get
        Set(value As Long)
            _FonctionParDefautId = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="login"></param>
    ''' <param name="pwd"></param>
    ''' <returns></returns>
    Public Shared Function cryptePwd(login As String, pwd As String) As String
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
    Public Function cryptePwd() As String
        Me.Password = cryptePwd(privateUtilisateurLogin, Password)
        Return Password
    End Function

    Public Function Clone() As Utilisateur
        Dim newInstance As Utilisateur = DirectCast(Me.MemberwiseClone(), Utilisateur)
        Return newInstance
    End Function

    Public Function isFonctionIdPossible(idFonction As Long) As Boolean
        If IsNothing(LstFonction) Then Return False
        For Each fonction In LstFonction
            If fonction.Id = idFonction Then Return True
        Next
        Return False
    End Function

End Class
