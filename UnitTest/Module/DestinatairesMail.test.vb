Imports Oasis_Common

''' <summary>
''' Contrôle des destinataires de courriel.
'''
''' /api/sendMail expédiait vers n'importe quelle adresse avec le compte SMTP de
''' la structure : un canal d'exfiltration et une plateforme d'hameçonnage. Le
''' filtre par domaine est la seule branche vérifiable sans base de données ;
''' MailDomainesAutorises est renseigné dans app.config pour ces tests.
'''
''' Les adresses inconnues descendent jusqu'à la base, qui est absente ici : la
''' connexion échoue, l'exception est avalée et la réponse est un refus. C'est
''' exactement le comportement attendu d'un contrôle qui ne peut pas s'exécuter,
''' et les tests le vérifient.
''' </summary>
<TestClass()> Public Class TestDestinatairesMail

    <TestMethod()> Public Sub LaListeEstSepareeParVirgulesEtNettoyee()
        CollectionAssert.AreEqual(New List(Of String) From {"a@exemple.fr", "b@exemple.fr"},
                                  DestinatairesMail.Separer(" a@exemple.fr , b@exemple.fr "))
    End Sub

    <TestMethod()> Public Sub LesEntreesVidesSontEcartees()
        Assert.AreEqual(0, DestinatairesMail.Separer(Nothing).Count)
        Assert.AreEqual(0, DestinatairesMail.Separer("").Count)
        Assert.AreEqual(0, DestinatairesMail.Separer(" , , ").Count)
        Assert.AreEqual(1, DestinatairesMail.Separer("a@exemple.fr,,").Count)
    End Sub

    <TestMethod()> Public Sub UnDomaineConfigureEstAccepte()
        Assert.IsTrue(DestinatairesMail.EstAutorise("medecin@exemple.fr", 0))
        Assert.IsTrue(DestinatairesMail.EstAutorise("secretariat@autorise.test", 0))
    End Sub

    <TestMethod()> Public Sub LaComparaisonDeDomaineIgnoreLaCasse()
        Assert.IsTrue(DestinatairesMail.EstAutorise("Medecin@EXEMPLE.FR", 0))
    End Sub

    <TestMethod()> Public Sub UnSousDomaineNEstPasLeDomaineAutorise()
        ' exemple.fr autorisé n'autorise pas exemple.fr.attaquant.test, ni
        ' sous.exemple.fr : la comparaison est une égalité, pas un suffixe.
        Assert.IsFalse(DestinatairesMail.EstAutorise("medecin@sous.exemple.fr", 0))
        Assert.IsFalse(DestinatairesMail.EstAutorise("medecin@exemple.fr.attaquant.test", 0))
    End Sub

    <TestMethod()> Public Sub UneAdresseAbsenteOuMalFormeeEstRefusee()
        For Each adresse In {Nothing, "", "   ", "pas-une-adresse", "@exemple.fr", "medecin@", "medecin@exemple."}
            Assert.IsFalse(DestinatairesMail.EstAutorise(adresse, 0), If(adresse, "(Nothing)"))
        Next
    End Sub

    <TestMethod()> Public Sub UneAdressePorteuseDeSautDeLigneEstRefusee()
        ' Injection d'en-têtes : une adresse qui porte un CR ou un LF ajoute des
        ' destinataires cachés au message.
        For Each adresse In {"medecin@exemple.fr" & vbCr & "Bcc: cible@ailleurs.test",
                             "medecin@exemple.fr" & vbLf & "Bcc: cible@ailleurs.test",
                             "medecin@exemple.fr" & vbCrLf & "Subject: faux",
                             "medecin@exemple.fr" & ChrW(0)}
            Assert.IsFalse(DestinatairesMail.EstAutorise(adresse, 0), adresse)
        Next
    End Sub

    <TestMethod()> Public Sub SansBaseDeDonneesUneAdresseInconnueEstRefusee()
        ' Le refus par défaut : la vérification en base ne peut pas s'exécuter.
        Assert.IsFalse(DestinatairesMail.EstAutorise("inconnu@ailleurs.test", 0))
        Assert.IsFalse(DestinatairesMail.EstAutorise("inconnu@ailleurs.test", 42))
    End Sub

End Class
