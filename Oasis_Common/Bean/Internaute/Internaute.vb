Imports System.Data.SqlClient
Imports System.Security.Cryptography

Public Class Internaute

    Public Property Id As Integer
    Public Property Password As String
    Public Property Username As String
    Public Property Email As String
    Public Property Recovery As String
    Public Property Code As String
    ''' <summary>Date d'expiration de la clé de récupération. Nothing si aucune demande en cours.</summary>
    Public Property RecoveryExpiration As Date?
    ''' <summary>Nombre d'échecs d'authentification consécutifs (verrouillage côté serveur).</summary>
    Public Property Tentatives As Integer
    ''' <summary>Date jusqu'à laquelle le compte est verrouillé. Nothing si non verrouillé.</summary>
    Public Property VerrouJusqua As Date?

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Username = reader("username")
        Me.Password = Coalesce(reader("password"), Nothing)
        Me.Email = reader("email")
        Me.Recovery = Coalesce(reader("recovery"), Nothing)
        Me.Code = Coalesce(reader("code"), Nothing)
        If HasColumn(reader, "recovery_expiration") Then
            Dim exp = Coalesce(reader("recovery_expiration"), Nothing)
            Me.RecoveryExpiration = If(exp Is Nothing, CType(Nothing, Date?), CDate(exp))
        End If
        If HasColumn(reader, "tentatives") Then
            Me.Tentatives = Coalesce(reader("tentatives"), 0)
        End If
        If HasColumn(reader, "verrou_jusqua") Then
            Dim verrou = Coalesce(reader("verrou_jusqua"), Nothing)
            Me.VerrouJusqua = If(verrou Is Nothing, CType(Nothing, Date?), CDate(verrou))
        End If
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

    Public Function Clone() As Internaute
        Dim newInstance As Internaute = DirectCast(Me.MemberwiseClone(), Internaute)
        Return newInstance
    End Function

End Class
