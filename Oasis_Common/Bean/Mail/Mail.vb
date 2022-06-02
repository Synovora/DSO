﻿Imports System.IO

Public Class Mail

    Property AliasFrom As String = ""
    Property AdressTo As String = ""
    Property Subject As String = ""
    Property Body As String = ""
    Property Filename As String = ""
    Property Contenu As Byte()
    Property IsHTML As Boolean = False

    Public Function IsWithContenu() As Boolean
        Return Filename <> Nothing AndAlso Not IsNothing(Contenu) AndAlso Contenu.Length > 0
    End Function

    Public Sub ConvertToPdf(Optional filename As String = "file")
        GemBox.Document.ComponentInfo.SetLicense("FREE-LIMITED-KEY")
        AddHandler GemBox.Document.ComponentInfo.FreeLimitReached, Sub(sender, e) e.FreeLimitReachedAction = GemBox.Document.FreeLimitReachedAction.ContinueAsTrial
        Using stream As New MemoryStream(Me.Contenu)
            Dim document = GemBox.Document.DocumentModel.Load(stream)
            Using outstream As New MemoryStream
                document.Save(outstream, GemBox.Document.SaveOptions.PdfDefault)
                Me.Contenu = outstream.ToArray
                Me.Filename = filename & ".pdf"
            End Using
        End Using
    End Sub

    Public Sub Send(loginRequestLog As LoginRequest)
        Using apiOasis As New ApiOasis()
            Dim ret = apiOasis.sendMailRest(loginRequestLog.login,
                              loginRequestLog.password,
                              Me)
        End Using
    End Sub
End Class
