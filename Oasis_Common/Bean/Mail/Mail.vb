Imports System.Configuration
Imports System.IO

Public Class MailDB
    Property sendMailKey As Long
    Property sendMailTo As String
    Property sendMailCc As String
    Property sendMailBcc As String
    Property sendMailFrom As String
    Property sendMailSender As String
    Property sendMailSubject As String
    Property sendMailMessage As String
    Property sendMailPath As String
    Property dateCreation As DateTime
    Property userCreation As Long
    Property sendMailSent As String

End Class

Public Class Mail

    Property AliasFrom As String = ""
    Property AddressTo As String = ""
    Property Subject As String = ""
    Property Body As String = ""
    Property Filename As String = ""
    Property Contenu As Byte()
    Property IsHTML As Boolean = False

    Public Function IsWithContenu() As Boolean
        Return Filename <> Nothing AndAlso Not IsNothing(Contenu) AndAlso Contenu.Length > 0
    End Function

    Public Sub ConvertToPdf(Optional filename As String = "file")
        ' Clé de licence GemBox lue en configuration. En mode d'évaluation, la
        ' bibliothèque insère une mention dans le document produit : le PDF envoyé
        ' au patient ne correspondait alors pas à ce que le praticien avait validé.
        Dim cleGemBox = ConfigurationManager.AppSettings("GemBoxLicense")
        If String.IsNullOrWhiteSpace(cleGemBox) Then cleGemBox = "FREE-LIMITED-KEY"
        GemBox.Document.ComponentInfo.SetLicense(cleGemBox)
        AddHandler GemBox.Document.ComponentInfo.FreeLimitReached,
            Sub(sender, e)
                ' Ne jamais produire silencieusement un document filigrané.
                e.FreeLimitReachedAction = GemBox.Document.FreeLimitReachedAction.[Stop]
            End Sub
        Using stream As New MemoryStream(Me.Contenu)
            Dim document = GemBox.Document.DocumentModel.Load(stream)
            Using outstream As New MemoryStream
                document.Save(outstream, GemBox.Document.SaveOptions.PdfDefault)
                Me.Contenu = outstream.ToArray
                Me.Filename = filename & ".pdf"
            End Using
        End Using
    End Sub

    ''' <param name="patientId">
    ''' Dossier concerné, 0 si l'envoi n'en vise aucun. Le serveur restreint les
    ''' destinataires aux adresses connues de ce dossier et de l'annuaire.
    ''' </param>
    Public Sub Send(loginRequestLog As LoginRequest, Optional patientId As Long = 0)
        Using apiOasis As New ApiOasis()
            Dim ret = apiOasis.sendMailRest(loginRequestLog, Me, patientId)
        End Using
    End Sub
End Class
