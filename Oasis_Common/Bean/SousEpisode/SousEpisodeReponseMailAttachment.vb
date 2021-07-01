
Public Class SousEpisodeReponseMailAttachment
    Property Id As Long
    Property MailId As Long
    Property Filename As String
    Property Part As Long

    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.MailId = row("mailId")
        Me.Filename = Coalesce(row("filename"), Nothing)
        Me.Part = row("part")
    End Sub

    Public Function GetLocalName()
        Return MailId & "_" & Part & New IO.FileInfo(Filename).Extension
    End Function
End Class
