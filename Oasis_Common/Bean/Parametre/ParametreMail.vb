Imports System.IO

Public Class ParametreMail

    Public Enum TypeMailParams
        SMTP_PARAMETERS
        ORDONNANCE
        SYNTHESE
        SOUS_EPISODE
    End Enum

    Property Id As Long

    Property SiegeId As Long

    Property TypeMailParam As TypeMailParams

    Property SmtpParams As String

    Property Objet As String

    Property Body As String

    Property IsBodyHtml As Boolean

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function GetSMTPServerUrl() As String
        If Me.TypeMailParam <> TypeMailParams.SMTP_PARAMETERS Then
            Throw New Exception("Pas de parametres technique SMTP sur ce type de paramètre")
        End If
        Return parseParameter("SMTPServer")
    End Function

    Public Function GetSMTPPort() As Integer
        If Me.TypeMailParam <> TypeMailParams.SMTP_PARAMETERS Then
            Throw New Exception("Pas de parametres technique SMTP sur ce type de paramètre")
        End If
        Return parseParameter("SMTPPort")
    End Function

    Public Function GetSMTPUser(isSousEpisode As Boolean) As String
        If Me.TypeMailParam <> TypeMailParams.SMTP_PARAMETERS Then
            Throw New Exception("Pas de parametres technique SMTP sur ce type de paramètre")
        End If
        Return parseParameter(If(isSousEpisode, "SMTPUserSousEpisode", "SMTPUser"))
    End Function

    Public Function GetSMTPPassword(isSousEpisode As Boolean) As String
        If Me.TypeMailParam <> TypeMailParams.SMTP_PARAMETERS Then
            Throw New Exception("Pas de parametres technique SMTP sur ce type de paramètre")
        End If
        Return parseParameter(If(isSousEpisode, "SMTPPasswordSousEpisode", "SMTPPassword"))
    End Function

    Public Function GetSMTPFrom(isSousEpisode As Boolean) As String
        If Me.TypeMailParam <> TypeMailParams.SMTP_PARAMETERS Then
            Throw New Exception("Pas de parametres technique SMTP sur ce type de paramètre")
        End If
        Return parseParameter(If(isSousEpisode, "SMTPFromSousEpisode", "SMTPFrom"))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    Private Function parseParameter(key As String) As String
        If Me.SmtpParams.Trim = "" Then
            Throw New Exception("Parametres technique SMTP vides")
        End If

        Using reader = New StringReader(SmtpParams)
            Dim line = reader.ReadLine
            Do While Not (line Is Nothing)
                Dim i = line.IndexOf("=")
                If i > -1 Then
                    Dim keylu = Left(line, i)
                    If keylu.Trim() = key Then
                        Return Mid(line, i + 2).Trim
                    End If
                End If
                line = reader.ReadLine
            Loop
        End Using

        Throw New Exception("Parametre " + key + " non retrouvé")

    End Function

End Class

