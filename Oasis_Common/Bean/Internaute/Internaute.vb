﻿Imports System.Security.Cryptography

Public Class Internaute

    Public Property Username As Integer
    Public Property Password As String

    Public Shared Function CryptePwd(login As String, pwd As String) As String
        Dim UniEnc As New Text.UnicodeEncoding
        Dim bitPass() As Byte = UniEnc.GetBytes("U23cGt'r8c" + login + pwd)
        Using sha As New SHA1CryptoServiceProvider
            Return Convert.ToBase64String(sha.ComputeHash(bitPass))
        End Using
    End Function

    Public Function CryptePwd() As String
        Me.Password = CryptePwd(Username, Password)
        Return Password
    End Function

    Public Function Clone() As Internaute
        Dim newInstance As Internaute = DirectCast(Me.MemberwiseClone(), Internaute)
        Return newInstance
    End Function

End Class
