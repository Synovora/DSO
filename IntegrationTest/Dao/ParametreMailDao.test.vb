Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' ParametreMailDao contre la base : modèles de courriel et compte SMTP.
'''
''' Le client lourd lit le modèle (FrmMailOrdonnance, et MailOasis appelé par la
''' synthèse, le carnet vaccinal, les sous-épisodes, la fiche patient) sans jamais
''' demander smtp_params : ces lectures tournent sous oasis_client, qui n'a pas le
''' droit de lire cette colonne. Seul Oasis_Web (SendMailController,
''' AuthController) passe inclureSmtp:=True, sous oasis_web.
'''
''' Chaque test vide d'abord les paramètres du type qu'il lit : TOP 1 départagerait
''' sinon deux lignes sans siège dans un ordre que la requête ne fixe pas.
''' </summary>
<TestClass()> Public Class ParametreMailDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ParametreMailDao

    Private Shared Sub VerifierRefusSql(erreur As SqlException)
        Assert.IsTrue(erreur.Number = 229 OrElse erreur.Number = 230,
                      "refus de permission attendu, erreur " & erreur.Number & " : " & erreur.Message)
    End Sub

    <TestMethod()> Public Sub SousClient_ModeleSansSiege_EstLuSansLeCompteSmtp()
        ViderParametresMail("ORDONNANCE")
        Dim id = CreerParametreMailSiege(0, "ORDONNANCE", "Votre ordonnance", "Bonjour @PatientPrenom", html:=True,
                                         smtpParams:="SMTPServer=smtp.exemple.fr" & vbCrLf & "SMTPPassword=secret")

        Dim lu = dao.GetParametreMailBySiegeIdTypeMailParam(0, ParametreMail.TypeMailParams.ORDONNANCE)

        Assert.AreEqual(id, lu.Id)
        Assert.AreEqual(0L, lu.SiegeId)
        Assert.AreEqual(ParametreMail.TypeMailParams.ORDONNANCE, lu.TypeMailParam)
        Assert.AreEqual("Votre ordonnance", lu.Objet)
        Assert.AreEqual("Bonjour @PatientPrenom", lu.Body)
        Assert.IsTrue(lu.IsBodyHtml)
        Assert.AreEqual("", lu.SmtpParams, "le poste ne reçoit jamais le compte SMTP")
    End Sub

    <TestMethod()> Public Sub SousClient_AvecCompteSmtp_EstRefuseParLaBase()
        ' Aucun appel du client lourd ne passe inclureSmtp:=True ; si l'un venait à le
        ' faire, la base le refuserait.
        ViderParametresMail("SMTP_PARAMETERS")
        CreerParametreMailSiege(0, "SMTP_PARAMETERS", Nothing, Nothing)

        VerifierRefusSql(Assert.ThrowsException(Of SqlException)(
            Sub() dao.GetParametreMailBySiegeIdTypeMailParam(0, ParametreMail.TypeMailParams.SMTP_PARAMETERS, inclureSmtp:=True)))
    End Sub

    <TestMethod()> Public Sub SousWeb_AvecCompteSmtp_RenvoieLesParametresSmtp()
        UtiliserCompte(Compte.Web)
        ViderParametresMail("SMTP_PARAMETERS")
        Dim parametres = "SMTPServer=smtp.exemple.fr" & vbCrLf & "SMTPPort=587" & vbCrLf & "SMTPUser=cabinet"
        CreerParametreMailSiege(0, "SMTP_PARAMETERS", Nothing, Nothing, smtpParams:=parametres)

        Dim lu = dao.GetParametreMailBySiegeIdTypeMailParam(Nothing, ParametreMail.TypeMailParams.SMTP_PARAMETERS, inclureSmtp:=True)

        Assert.AreEqual(parametres, lu.SmtpParams)
        Assert.AreEqual("smtp.exemple.fr", lu.GetSMTPServerUrl())
        Assert.AreEqual(587, lu.GetSMTPPort())
        Assert.AreEqual("cabinet", lu.GetSMTPUser(False))
    End Sub

    <TestMethod()> Public Sub SousWeb_SansCompteSmtpDemande_NeLitPasLaColonne()
        UtiliserCompte(Compte.Web)
        ViderParametresMail("SMTP_PARAMETERS")
        CreerParametreMailSiege(0, "SMTP_PARAMETERS", Nothing, Nothing, smtpParams:="SMTPServer=smtp.exemple.fr")

        Assert.AreEqual("", dao.GetParametreMailBySiegeIdTypeMailParam(0, ParametreMail.TypeMailParams.SMTP_PARAMETERS).SmtpParams)
    End Sub

    <TestMethod()> Public Sub SousClient_ModeleDuSiege_PasseAvantLeModeleCommun()
        ViderParametresMail("ORDONNANCE")
        Dim siegeA = CreerSiegeCourriel("Siege courriel A")
        CreerParametreMailSiege(0, "ORDONNANCE", "Commun", "Corps commun")
        Dim propre = CreerParametreMailSiege(siegeA, "ORDONNANCE", "Du siege", "Corps du siege")

        Dim lu = dao.GetParametreMailBySiegeIdTypeMailParam(siegeA, ParametreMail.TypeMailParams.ORDONNANCE)

        Assert.AreEqual(propre, lu.Id)
        Assert.AreEqual(siegeA, lu.SiegeId)
        Assert.AreEqual("Du siege", lu.Objet)
    End Sub

    <TestMethod()> Public Sub SousClient_ModeleDUnAutreSiege_EstIgnore()
        ViderParametresMail("ORDONNANCE")
        Dim siegeA = CreerSiegeCourriel("Siege courriel A")
        Dim siegeB = CreerSiegeCourriel("Siege courriel B")
        Dim commun = CreerParametreMailSiege(0, "ORDONNANCE", "Commun", "Corps commun")
        CreerParametreMailSiege(siegeB, "ORDONNANCE", "Autre", "Corps autre")

        Assert.AreEqual(commun, dao.GetParametreMailBySiegeIdTypeMailParam(siegeA, ParametreMail.TypeMailParams.ORDONNANCE).Id)
        Assert.AreEqual(commun, dao.GetParametreMailBySiegeIdTypeMailParam(0, ParametreMail.TypeMailParams.ORDONNANCE).Id)
    End Sub

    <TestMethod()> Public Sub SousClient_FiltreSurLeType()
        ViderParametresMail("ORDONNANCE")
        ViderParametresMail("SYNTHESE")
        CreerParametreMailSiege(0, "ORDONNANCE", "Ordonnance", "Corps ordonnance")
        Dim idSynthese = CreerParametreMailSiege(0, "SYNTHESE", "Synthese", "Corps synthese")

        Dim lu = dao.GetParametreMailBySiegeIdTypeMailParam(0, ParametreMail.TypeMailParams.SYNTHESE)

        Assert.AreEqual(idSynthese, lu.Id)
        Assert.AreEqual(ParametreMail.TypeMailParams.SYNTHESE, lu.TypeMailParam)
    End Sub

    <TestMethod()> Public Sub SousClient_TypeAbsent_LeveArgumentException()
        ViderParametresMail("CARNET_VACCINAL")
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(
            Sub() dao.GetParametreMailBySiegeIdTypeMailParam(0, ParametreMail.TypeMailParams.CARNET_VACCINAL))
        StringAssert.Contains(erreur.Message, "Paramètre Mail inexistant")
    End Sub

    <TestMethod()> Public Sub SousClient_ObjetEtCorpsNuls_DonnentDesChainesVides()
        ViderParametresMail("SOUS_EPISODE")
        CreerParametreMailSiege(0, "SOUS_EPISODE", Nothing, Nothing)

        Dim lu = dao.GetParametreMailBySiegeIdTypeMailParam(0, ParametreMail.TypeMailParams.SOUS_EPISODE)

        Assert.AreEqual("", lu.Objet)
        Assert.AreEqual("", lu.Body)
        Assert.IsFalse(lu.IsBodyHtml)
    End Sub

    <TestMethod()> Public Sub MailOasis_SousClient_ChargeLeModeleEtRemplaceLaDate()
        ' Le constructeur de MailOasis, appelé par les écrans du client lourd, lit le
        ' modèle sans siège ni compte SMTP.
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Try
            Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
            ViderParametresMail("SYNTHESE")
            CreerParametreMailSiege(0, "SYNTHESE", "Synthese du @DateCreation", "Voici la synthese du @DateCreation.", html:=True)

            Dim courriel As New MailOasis(ParametreMail.TypeMailParams.SYNTHESE)

            Dim jour = Date.Now.ToString("dd/MM/yyyy")
            Assert.AreEqual("Synthese du " & jour, courriel.Subject)
            Assert.AreEqual("Voici la synthese du " & jour & ".", courriel.Body)
            Assert.IsTrue(courriel.IsHTML)
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Sub

End Class
