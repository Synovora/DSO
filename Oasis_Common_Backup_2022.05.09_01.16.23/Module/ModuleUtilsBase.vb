﻿Imports System.Configuration
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.Win32

Public Module ModuleUtilsBase
    Public Const JoursAAjouterPourCalculAgePreScolaire As Integer = 4

    Private Const chaineMystere As String = "mAisOuEstDoncOrNic@r!421"
    Private ReadOnly SALT As Byte() = New Byte() {&H26, &HDC, &HFF, &H0, &HAD, &HED, &H7A, &HEE, &HC5, &HFE, &H7, &HAF, &H4D, &H8, &H22, &H3C}
    Private Const chaineStartCrypt As String = "#PE#"
    Public ReadOnly messageFormatPassword As String = "Au moins 8 caractères, une majuscule, une minuscule, un chiffre et un caractère spécial"
    Public ReadOnly MAX_TRY As Integer = 5

    Public Function GetConnectionString() As String
        Dim SqlConnection As String
        SqlConnection = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString
        Return SqlConnection
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function GetConnectionStringOasis() As String

        Return ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString

    End Function

    Public Function GetSqlCommandTextForLogs(cmd As SqlCommand) As String
        Dim text = cmd.CommandText
        For Each parameter As SqlParameter In cmd.Parameters
            text = text.Replace(parameter.ParameterName, parameter.Value.ToString())
        Next
        Return text
    End Function

    Public Function N2N(Of T)(ByVal Parameter As Object, Optional DefaultValue As Object = Nothing)
        If Parameter IsNot Nothing Then
            Return CType(Parameter, T)
        End If
        If DefaultValue IsNot Nothing Then
            Return DefaultValue
        End If
        Return DBNull.Value
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

    Public Function HasColumn(Reader As DbDataReader, ColumnName As String) As Boolean
        For Each row As DataRow In Reader.GetSchemaTable().Rows
            If row("ColumnName").ToString() = ColumnName Then
                Return True
            End If
        Next
        Return False
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

    Public Function verifPassword(login As String, password As String) As Utilisateur

        Dim userDao As UserDao = New UserDao
        Dim userLog = userDao.getUserByLoginPassword(login, password)
        Return userLog

    End Function

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


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function IsPermission(Optional incremente As Boolean = False) As Boolean
        Dim ret = ReadPermTry()
        If incremente Then
            ret += 1
            WritePermTry(ret)
        End If
        Return ret <= MAX_TRY
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub ResetPermission()
        WritePermTry(0)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function ReadPermTry() As Integer
        '-- creation eventuelle de la clé
        Using uediSurveyKey = Registry.CurrentUser.CreateSubKey("Software\USYNSurvey")
        End Using
        Dim readValue = "" & My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\USYNSurvey", "USS", EncryptString("" & 0))
        Dim ret As New Integer
        If Integer.TryParse(DecryptString(readValue), ret) = False Then
            ret = Integer.MaxValue - 1 ' -- on n'arrive pas à decoder => le pirate a trafiqué la valeur à la main ds la base de registre et là : je mets le nbre d'essais au MAX !
        End If
        Return ret
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value"></param>
    Private Sub WritePermTry(value As Integer)
        Using uediSurveyKey = Registry.CurrentUser.CreateSubKey("Software\USYNSurvey")
            uediSurveyKey.SetValue("USS", EncryptString("" & value))
        End Using
    End Sub

    Public Function CalculAgeEnAnnee(dateNaissance As Date) As Integer
        Dim datetimenow = DateTime.Now
        Dim age As Integer

        age = CInt(Now.Year - dateNaissance.Year)

        If dateNaissance.Month > Now.Month Then
            age -= 1
        End If

        If ((dateNaissance.Month = Now.Month) And (dateNaissance.Day > Now.Day)) Then
            age -= 1
        End If

        Return age
    End Function

    Public Function CalculAgeEnmois(dateNaissance As Date) As Integer
        Dim datetimenow = DateTime.Now
        Dim mois As Integer

        mois = DateDiff("m", dateNaissance, datetimenow)

        Return mois
    End Function

    Public Function CalculAgeEnJour(dateNaissance As Date) As Integer
        Dim datetimenow = DateTime.Now
        Dim Jour As Integer

        Jour = DateDiff("d", dateNaissance, datetimenow)

        Return Jour
    End Function

    Public Function CalculAgeEnAnneeEtMoisString(DateNaissance As Date) As String
        Dim lMois As Integer
        Dim Age As String
        Dim PatientMoisRestant, PatientAn As Integer
        lMois = CalculAgeEnmois(DateNaissance)
        If lMois > 35 Then
            PatientMoisRestant = lMois Mod 12
            lMois -= PatientMoisRestant
            PatientAn = lMois / 12
        Else
            Dim lJour = CalculAgeEnJour(DateNaissance)
            lJour += JoursAAjouterPourCalculAgePreScolaire
            Dim lJourRestant = lJour Mod 30.4375
            lJour -= lJourRestant
            lMois = lJour \ 30.4375
        End If
        Select Case lMois
            Case 0 To 35
                If lMois <> 0 Then
                    Age = lMois & " mois"
                Else
                    Age = "Nouveau né"
                End If
            Case 36 To 119
                Age = PatientAn & " ans " & PatientMoisRestant & " mois"
            Case Else
                Age = PatientAn.ToString & " ans"
        End Select

        Return Age
    End Function

End Module