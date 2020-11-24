Imports System.Security.Cryptography

Public Class Internaute

    Public Property Id As Integer
    Public Property PatientId As Integer
    Public Property Password As String
    Public Property Patient As Patient

    Public Shared Function CryptePwd(login As String, pwd As String) As String
        Dim UniEnc As New Text.UnicodeEncoding
        Dim bitPass() As Byte = UniEnc.GetBytes("U23cGt'r8c" + login + pwd)
        Using sha As New SHA1CryptoServiceProvider 'TODO: Don't use Sha1, prefer Sha3 aka Keccak
            Return Convert.ToBase64String(sha.ComputeHash(bitPass))
        End Using
    End Function

    Public Function CryptePwd() As String
        Me.Password = CryptePwd(PatientId.ToString(), Password)
        Return Password
    End Function

    Public Function Clone() As Internaute
        Dim newInstance As Internaute = DirectCast(Me.MemberwiseClone(), Internaute)
        Return newInstance
    End Function

End Class
