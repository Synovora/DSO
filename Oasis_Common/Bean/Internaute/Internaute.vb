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

    Public Shared Function CryptePwd(login As String, pwd As String) As String
        Dim UniEnc As New Text.UnicodeEncoding
        Dim bitPass() As Byte = UniEnc.GetBytes("U23cGt'r8c" + login + pwd) 'TODO: Put SALT in var
        Using sha As New SHA1CryptoServiceProvider 'TODO: Don't use Sha1, prefer Sha3 aka Keccak
            Return Convert.ToBase64String(sha.ComputeHash(bitPass))
        End Using
    End Function

    Public Function CryptePwd() As String
        Me.Password = CryptePwd(Email.ToString(), Password)
        Return Password
    End Function

    Public Function Clone() As Internaute
        Dim newInstance As Internaute = DirectCast(Me.MemberwiseClone(), Internaute)
        Return newInstance
    End Function

End Class
