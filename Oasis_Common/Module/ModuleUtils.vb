Imports System.Configuration
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Module ModuleUtils

    Private Const chaineMystere As String = "mAisOuEstDoncOrNic@r!421"
    Private ReadOnly SALT As Byte() = New Byte() {&H26, &HDC, &HFF, &H0, &HAD, &HED, &H7A, &HEE, &HC5, &HFE, &H7, &HAF, &H4D, &H8, &H22, &H3C}
    Private Const chaineStartCrypt As String = "#PE#"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function GetConnectionStringOasis() As String

        Return ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Parameters"></param>
    ''' <returns></returns>
    Public Function Coalesce(ByVal ParamArray Parameters As Object()) As Object
        For Each Parameter As Object In Parameters
            If Not Parameter Is Nothing And Not IsDBNull(Parameter) Then
                Return Parameter
            End If
        Next
        Return Nothing
    End Function

    Public Function EncryptString(ByVal clearText As String) As String
        Dim plainText As Byte() = Encoding.UTF8.GetBytes(clearText)
        Dim CipherBytes As Byte() = Encrypt(plainText, chaineMystere)
        Return Convert.ToBase64String(CipherBytes)
    End Function

    Public Function DecryptString(ByVal cipherText As String) As String
        Dim cipheredData As Byte() = Convert.FromBase64String(cipherText)
        Dim plainTextData As Byte() = Decrypt(cipheredData, chaineMystere)
        Return Encoding.UTF8.GetString(plainTextData, 0, plainTextData.Length)
    End Function

    Private Function Encrypt(ByVal plain As Byte(), ByVal password As String) As Byte()
        Dim memoryStream As MemoryStream
        Dim cryptoStream As CryptoStream
        Dim rijndael As Rijndael = Rijndael.Create()
        Dim pdb As Rfc2898DeriveBytes = New Rfc2898DeriveBytes(password, SALT)
        rijndael.Key = pdb.GetBytes(32)
        rijndael.IV = pdb.GetBytes(16)
        memoryStream = New MemoryStream()
        cryptoStream = New CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write)
        cryptoStream.Write(plain, 0, plain.Length)
        cryptoStream.Close()
        Return memoryStream.ToArray()
    End Function

    Private Function Decrypt(ByVal cipher As Byte(), ByVal password As String) As Byte()
        Dim memoryStream As MemoryStream
        Dim cryptoStream As CryptoStream
        Dim rijndael As Rijndael = Rijndael.Create()
        Dim pdb As Rfc2898DeriveBytes = New Rfc2898DeriveBytes(password, SALT)
        rijndael.Key = pdb.GetBytes(32)
        rijndael.IV = pdb.GetBytes(16)
        memoryStream = New MemoryStream()
        cryptoStream = New CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write)
        cryptoStream.Write(cipher, 0, cipher.Length)
        cryptoStream.Close()
        Return memoryStream.ToArray()
    End Function

    Public Sub verifPassword(login As String, password As String)
        Dim userDao As UserDao = New UserDao
        Dim userLog = Nothing
        userLog = userDao.getUserByLoginPassword(login, password)
        Return

    End Sub

    Public Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr = New System.Net.Mail.MailAddress(email)
            If email.LastIndexOf(".") < email.LastIndexOf("@") OrElse email.LastIndexOf(".") = email.Length - 1 Then Return False
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Function isValidePassword(pwd As String, Optional ByVal minLength As Integer = 8,
                                                    Optional ByVal numUpper As Integer = 1,
                                                    Optional ByVal numLower As Integer = 1,
                                                    Optional ByVal numNumbers As Integer = 1,
                                                    Optional ByVal numSpecial As Integer = 1) As Boolean

        Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
        Dim lower As New System.Text.RegularExpressions.Regex("[a-z]")
        Dim number As New System.Text.RegularExpressions.Regex("[0-9]")
        ' Special is "none of the above".
        Dim special As New System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")

        ' Check length.
        If Len(pwd) < minLength Then Return False
        ' Check minimum number of occurrences.
        If upper.Matches(pwd).Count < numUpper Then Return False
        If lower.Matches(pwd).Count < numLower Then Return False
        If number.Matches(pwd).Count < numNumbers Then Return False
        If special.Matches(pwd).Count < numSpecial Then Return False

        ' Passed all checks.
        Return True
    End Function

End Module
