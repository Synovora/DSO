Imports System.Data.SqlClient
Imports System.Security.Cryptography

Public Class Internaute

    Public Property Id As Integer
    Public Property Password As String
    Public Property Username As String
    Public Property Email As String
    Public Property Recovery As String
    Public Property Code As String

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Username = reader("username")
        Me.Password = Coalesce(reader("password"), Nothing)
        Me.Email = reader("email")
        Me.Recovery = Coalesce(reader("recovery"), Nothing)
        Me.Code = Coalesce(reader("code"), Nothing)
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
