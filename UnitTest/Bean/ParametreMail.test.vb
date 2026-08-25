Imports Oasis_Common

''' <summary>
''' Lecture des paramètres SMTP stockés en base sous forme de lignes clé=valeur.
''' Ces valeurs sont le compte d'envoi de la structure ; côté client la colonne
''' n'est pas lisible, donc ce code ne s'exécute que sur le serveur.
''' </summary>
<TestClass()> Public Class TestParametreMailSmtp

    Private Shared Function Parametres(contenu As String) As ParametreMail
        Return New ParametreMail With {.TypeMailParam = ParametreMail.TypeMailParams.SMTP_PARAMETERS, .SmtpParams = contenu}
    End Function

    Private Const Complet As String =
        "SMTPServer=smtp.exemple.fr" & vbCrLf &
        "SMTPPort=465" & vbCrLf &
        "SMTPUser=compte" & vbCrLf &
        "SMTPPassword=secret=avec=egal" & vbCrLf &
        "SMTPFrom=noreply@exemple.fr" & vbCrLf &
        "SMTPUserSousEpisode=courrier" & vbCrLf &
        "SMTPPasswordSousEpisode=autre" & vbCrLf &
        "SMTPFromSousEpisode=courrier@exemple.fr"

    <TestMethod()> Public Sub ChaqueCleEstLue()
        Dim p = Parametres(Complet)
        Assert.AreEqual("smtp.exemple.fr", p.GetSMTPServerUrl())
        Assert.AreEqual(465, p.GetSMTPPort())
        Assert.AreEqual("compte", p.GetSMTPUser(False))
        Assert.AreEqual("noreply@exemple.fr", p.GetSMTPFrom(False))
    End Sub

    <TestMethod()> Public Sub UneValeurPeutContenirLeSigneEgal()
        ' Un mot de passe avec un = ne doit pas être tronqué au premier.
        Assert.AreEqual("secret=avec=egal", Parametres(Complet).GetSMTPPassword(False))
    End Sub

    <TestMethod()> Public Sub LeCompteSousEpisodeEstDistinct()
        Dim p = Parametres(Complet)
        Assert.AreEqual("courrier", p.GetSMTPUser(True))
        Assert.AreEqual("autre", p.GetSMTPPassword(True))
        Assert.AreEqual("courrier@exemple.fr", p.GetSMTPFrom(True))
    End Sub

    <TestMethod()> Public Sub LesEspacesAutourDesValeursSontIgnores()
        Dim p = Parametres("  SMTPServer  =   smtp.exemple.fr   " & vbCrLf & "SMTPPort = 587 ")
        Assert.AreEqual("smtp.exemple.fr", p.GetSMTPServerUrl())
        Assert.AreEqual(587, p.GetSMTPPort())
    End Sub

    <TestMethod()> Public Sub LesLignesSansEgalSontIgnorees()
        Dim p = Parametres("# commentaire" & vbCrLf & "" & vbCrLf & "SMTPServer=smtp.exemple.fr")
        Assert.AreEqual("smtp.exemple.fr", p.GetSMTPServerUrl())
    End Sub

    <TestMethod()> Public Sub UneCleAbsenteEstUneErreurNommee()
        Try
            Parametres("SMTPServer=smtp.exemple.fr").GetSMTPPort()
            Assert.Fail("aurait dû lever")
        Catch ex As Exception
            StringAssert.Contains(ex.Message, "SMTPPort")
        End Try
    End Sub

    <TestMethod()> Public Sub DesParametresVidesSontUneErreur()
        For Each contenu In {"", "   "}
            Try
                Parametres(contenu).GetSMTPServerUrl()
                Assert.Fail("aurait dû lever")
            Catch ex As Exception
                Assert.AreEqual("Parametres technique SMTP vides", ex.Message)
            End Try
        Next
    End Sub

    <TestMethod()> Public Sub UnParametreDUnAutreTypeNAPasDeCompteSmtp()
        Dim p = New ParametreMail With {.TypeMailParam = ParametreMail.TypeMailParams.ORDONNANCE, .SmtpParams = Complet}
        For Each lecture In New Func(Of Object)() {
                Function() p.GetSMTPServerUrl(), Function() p.GetSMTPPort(), Function() p.GetSMTPUser(False),
                Function() p.GetSMTPPassword(False), Function() p.GetSMTPFrom(False)}
            Try
                lecture()
                Assert.Fail("aurait dû lever")
            Catch ex As Exception
                StringAssert.StartsWith(ex.Message, "Pas de parametres technique SMTP")
            End Try
        Next
    End Sub

End Class
