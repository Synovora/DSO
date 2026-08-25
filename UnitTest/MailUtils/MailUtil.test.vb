Imports MimeKit
Imports Oasis_Common

''' <summary>
''' Composition du courriel sortant, sans serveur SMTP. C'est la dernière
''' barrière avant l'envoi par le compte de la structure : chaque destinataire
''' est validé ici, quelle que soit la route qui a fourni la liste.
''' </summary>
<TestClass()> Public Class TestMailUtil

    Private Shared Function Outil() As MailUtil
        Return New MailUtil("smtp.exemple.fr", 465, "compte", "secret", "noreply@exemple.fr")
    End Function

    <TestMethod()> Public Sub LeMessageEstComposeAvecChaqueChamp()
        Dim message = Outil().ComposerMessage(New MailOasis With {
            .AliasFrom = "Oasis", .AddressTo = "a@exemple.fr, b@exemple.fr", .Subject = "Objet", .Body = "<p>Bonjour</p>",
            .IsHTML = True, .Filename = "compte-rendu.pdf", .Contenu = New Byte() {37, 80, 68, 70}})

        Dim expediteur = DirectCast(message.From(0), MailboxAddress)
        Assert.AreEqual("Oasis", expediteur.Name)
        Assert.AreEqual("noreply@exemple.fr", expediteur.Address)
        CollectionAssert.AreEqual({"a@exemple.fr", "b@exemple.fr"}, message.To.Mailboxes.Select(Function(m) m.Address).ToArray())
        Assert.AreEqual("Objet", message.Subject)
        Assert.AreEqual("<p>Bonjour</p>", message.HtmlBody)
        Assert.IsNull(message.TextBody)
        Dim pieces = message.Attachments.OfType(Of MimePart)().ToList()
        Assert.AreEqual(1, pieces.Count)
        Assert.AreEqual("compte-rendu.pdf", pieces(0).FileName)
    End Sub

    <TestMethod()> Public Sub UnCorpsTexteResteDuTexte()
        Dim message = Outil().ComposerMessage(New MailOasis With {.AliasFrom = "Oasis", .AddressTo = "a@exemple.fr", .Subject = "s", .Body = "Bonjour", .IsHTML = False})
        Assert.AreEqual("Bonjour", message.TextBody)
        Assert.IsNull(message.HtmlBody)
        Assert.AreEqual(0, message.Attachments.Count())
    End Sub

    <TestMethod()> Public Sub UneAdresseInvalideEstRefusee()
        For Each destinataires In {"pas-une-adresse", "a@exemple.fr, pas-une-adresse", "a@exemple", "@exemple.fr"}
            Try
                Outil().ComposerMessage(New MailOasis With {.AliasFrom = "Oasis", .AddressTo = destinataires, .Subject = "s", .Body = "b"})
                Assert.Fail("accepté à tort : " & destinataires)
            Catch ex As ArgumentException
                StringAssert.StartsWith(ex.Message, "Adresse de destinataire invalide")
            End Try
        Next
    End Sub

    <TestMethod()> Public Sub UneInjectionDEnTeteEstRefusee()
        For Each destinataires In {"a@exemple.fr" & vbCrLf & "Bcc: cible@ailleurs.test", "a@exemple.fr" & vbLf & "x", "a@exemple.fr" & ChrW(0)}
            Try
                Outil().ComposerMessage(New MailOasis With {.AliasFrom = "Oasis", .AddressTo = destinataires, .Subject = "s", .Body = "b"})
                Assert.Fail("accepté à tort")
            Catch ex As ArgumentException
                ' attendu
            End Try
        Next
    End Sub

    <TestMethod()> Public Sub SansDestinataireValideLeMessageNEstPasCompose()
        For Each destinataires In {"", "   ", " , , ", Nothing}
            Try
                Outil().ComposerMessage(New MailOasis With {.AliasFrom = "Oasis", .AddressTo = destinataires, .Subject = "s", .Body = "b"})
                Assert.Fail("accepté à tort")
            Catch ex As ArgumentException
                Assert.AreEqual("Aucun destinataire valide.", ex.Message)
            End Try
        Next
    End Sub

    <TestMethod()> Public Sub LaLiaisonSmtpEstEnTlsImpliciteParDefaut()
        ' MailSecuriteSmtp est absent de app.config : le mode doit être
        ' SslOnConnect, pas une négociation STARTTLS.
        Assert.AreEqual(MailKit.Security.SecureSocketOptions.SslOnConnect, MailUtil.SecuriteSmtp())
    End Sub

End Class
