Imports Oasis_Common

''' <summary>
''' MailDao contre la base : file d'envoi oasis.send_mail_trigger. Le client lourd
''' y dépose un courriel (RadFMailEdit, CreateMail) sous oasis_client.
''' GetProfessionSanteById, mal nommé, relit une ligne de cette file ; il n'a aucun
''' appelant et tourne sous le même compte.
'''
''' CreateMail envoie la date en texte (yyyy-MM-dd HH:mm:ss) : la valeur stockée
''' dépend de la langue de la session et du type de la colonne. Les tests comparent
''' donc à ce que SQL Server tire du même texte (JeuxInternaute.ConversionSqlDeTexte).
''' </summary>
<TestClass()> Public Class MailDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New MailDao

    Private Const FormatDateMail As String = "yyyy-MM-dd HH:mm:ss"

    Private Shared Function Courriel(sujet As String) As MailDB
        Return New MailDB With {
            .sendMailTo = "destinataire@exemple.fr",
            .sendMailCc = "copie@exemple.fr",
            .sendMailBcc = "",
            .sendMailFrom = "cabinet@exemple.fr",
            .sendMailSender = "Cabinet de test",
            .sendMailSubject = sujet,
            .sendMailMessage = "Corps du message",
            .sendMailPath = "C:\OasisTests\Documents\piece.pdf",
            .sendMailSent = "ignoré par CreateMail"}
    End Function

    Private Shared Function CleDuCourriel(sujet As String) As Long
        Dim cle = Scalaire("SELECT MAX(sendMailKey) FROM oasis.send_mail_trigger WHERE sendMailSubject = @p0", sujet)
        Assert.IsFalse(cle Is Nothing OrElse cle Is DBNull.Value, "courriel « " & sujet & " » absent de la file")
        Return CLng(cle)
    End Function

    Private Shared Function ValeurCourriel(colonne As String, cle As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.send_mail_trigger WHERE sendMailKey = @p0", cle)
    End Function

    <TestMethod()> Public Sub CreateMail_SousClient_DeposeLeCourrielDansLaFile()
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim sujet = "Sujet " & Guid.NewGuid().ToString("N")
        Dim avant = Date.Now
        Dim erreur As Exception = Nothing
        Dim retour As Boolean

        Try
            retour = dao.CreateMail(Courriel(sujet), New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
        Catch ex As Exception
            erreur = ex
        End Try

        Dim possibles = ConversionsSqlPossibles(Compte.Client, "oasis.send_mail_trigger", "date_creation",
                                                avant, Date.Now, Function(d) d.ToString(FormatDateMail))
        If possibles.Count = 0 Then
            ' La session lit yyyy-MM-dd autrement (langue en jj/mm, colonne datetime) :
            ' l'INSERT échoue sur la conversion de date.
            Assert.IsNotNull(erreur, "l'écriture de la date devait échouer pour cette session")
            Return
        End If
        Assert.IsNull(erreur, If(erreur Is Nothing, "", erreur.Message))
        Assert.IsTrue(retour)

        Dim cle = CleDuCourriel(sujet)
        Assert.AreEqual("destinataire@exemple.fr", CStr(ValeurCourriel("sendMailTo", cle)))
        Assert.AreEqual("copie@exemple.fr", CStr(ValeurCourriel("sendMailCc", cle)))
        Assert.AreEqual("", CStr(ValeurCourriel("sendMailBcc", cle)))
        Assert.AreEqual("cabinet@exemple.fr", CStr(ValeurCourriel("sendMailFrom", cle)))
        Assert.AreEqual("Cabinet de test", CStr(ValeurCourriel("sendMailSender", cle)))
        Assert.AreEqual("Corps du message", CStr(ValeurCourriel("sendMailMessage", cle)))
        Assert.AreEqual("C:\OasisTests\Documents\piece.pdf", CStr(ValeurCourriel("sendMailPath", cle)))
        Assert.AreEqual(utilisateurId, CLng(ValeurCourriel("user_creation", cle)))
        CollectionAssert.Contains(possibles, ValeurCourriel("date_creation", cle))
        ' sendMailSent part toujours vide, quelle que soit la valeur du bean.
        Assert.AreNotEqual("ignoré par CreateMail", Convert.ToString(ValeurCourriel("sendMailSent", cle)))
    End Sub

    <TestMethod()> Public Sub CreateMail_ChampAbsent_Echoue()
        ' Comportement actuel : AddWithValue avec Nothing n'envoie pas le paramètre ;
        ' l'écran doit renseigner chaque champ, chaîne vide comprise.
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim sujet = "Sans copie " & Guid.NewGuid().ToString("N")
        Dim incomplet = Courriel(sujet)
        incomplet.sendMailCc = Nothing

        Assert.ThrowsException(Of Exception)(
            Sub() dao.CreateMail(incomplet, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}))
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.send_mail_trigger WHERE sendMailSubject = @p0", sujet)))
    End Sub

    <TestMethod()> Public Sub GetProfessionSanteById_SousClient_RelitLeCourriel()
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim sujet = "Relu " & Guid.NewGuid().ToString("N")
        Try
            dao.CreateMail(Courriel(sujet), New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
        Catch ex As Exception
            Assert.Inconclusive("CreateMail n'a pas pu écrire pour cette session (voir CreateMail_SousClient_DeposeLeCourrielDansLaFile) : " & ex.Message)
        End Try
        Dim cle = CleDuCourriel(sujet)

        Dim lu = dao.GetProfessionSanteById(CInt(cle))

        Assert.AreEqual(cle, lu.sendMailKey)
        Assert.AreEqual(sujet, lu.sendMailSubject)
        Assert.AreEqual("destinataire@exemple.fr", lu.sendMailTo)
        Assert.AreEqual("copie@exemple.fr", lu.sendMailCc)
        Assert.AreEqual("", lu.sendMailBcc)
        Assert.AreEqual("cabinet@exemple.fr", lu.sendMailFrom)
        Assert.AreEqual("Cabinet de test", lu.sendMailSender)
        Assert.AreEqual("Corps du message", lu.sendMailMessage)
        Assert.AreEqual("C:\OasisTests\Documents\piece.pdf", lu.sendMailPath)
        Assert.AreEqual(utilisateurId, lu.userCreation)
        Assert.AreEqual(CDate(ValeurCourriel("date_creation", cle)), lu.dateCreation)
        Assert.AreEqual(Convert.ToString(ValeurCourriel("sendMailSent", cle)), lu.sendMailSent)
    End Sub

    <TestMethod()> Public Sub GetProfessionSanteById_Inexistant_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetProfessionSanteById(987654321))
        StringAssert.Contains(erreur.Message, "Mail inexistant")
    End Sub

End Class
