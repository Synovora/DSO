Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne de paramètre de courriel par ParametreMailDao.BuildBean.
''' La colonne smtp_params porte le compte SMTP de la structure. Le compte SQL
''' du client lourd ne peut pas la lire, et la lecture ne doit la restituer
''' que sur demande explicite, côté serveur.
''' </summary>
<TestClass()> Public Class TestParametreMailDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "siege_id", "type_mail_param", "smtp_params", "objet", "body", "is_body_html"}

    Private Shared Function Complet() As Dictionary(Of String, Object)
        Return New Dictionary(Of String, Object) From {
            {"id", 3L}, {"siege_id", 1L}, {"type_mail_param", "ORDONNANCE"},
            {"smtp_params", "smtp.exemple.fr;465;compte;secret"},
            {"objet", "Votre ordonnance"}, {"body", "<p>Bonjour</p>"}, {"is_body_html", True}}
    End Function

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim p = ParametreMailDao.BuildBean(LigneDeTest.Ligne(Colonnes, Complet()), inclureSmtp:=True)

        Assert.AreEqual(3L, p.Id)
        Assert.AreEqual(1L, p.SiegeId)
        Assert.AreEqual(ParametreMail.TypeMailParams.ORDONNANCE, p.TypeMailParam)
        Assert.AreEqual("smtp.exemple.fr;465;compte;secret", p.SmtpParams)
        Assert.AreEqual("Votre ordonnance", p.Objet)
        Assert.AreEqual("<p>Bonjour</p>", p.Body)
        Assert.IsTrue(p.IsBodyHtml)
    End Sub

    <TestMethod()> Public Sub SansLeDrapeauLeCompteSmtpNEstPasRestitue()
        ' Même si la requête rapporte la colonne, le bean ne la porte pas.
        Dim p = ParametreMailDao.BuildBean(LigneDeTest.Ligne(Colonnes, Complet()), inclureSmtp:=False)
        Assert.AreEqual("", p.SmtpParams)
        Assert.AreEqual("Votre ordonnance", p.Objet, "le reste de la ligne est lu normalement")
    End Sub

    <TestMethod()> Public Sub UneLigneMinimaleDonneLesValeursParDefaut()
        Dim p = ParametreMailDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 1L}, {"type_mail_param", "SYNTHESE"}, {"is_body_html", False}}), inclureSmtp:=True)

        Assert.AreEqual(0L, p.SiegeId)
        Assert.AreEqual("", p.SmtpParams)
        Assert.AreEqual("", p.Objet)
        Assert.AreEqual("", p.Body)
        Assert.IsFalse(p.IsBodyHtml)
    End Sub

    <TestMethod()> Public Sub ChaqueTypeDeCourrielSeLit()
        For Each nom In [Enum].GetNames(GetType(ParametreMail.TypeMailParams))
            Dim valeurs = Complet()
            valeurs("type_mail_param") = nom
            Dim p = ParametreMailDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs), inclureSmtp:=False)
            Assert.AreEqual(nom, p.TypeMailParam.ToString(), nom)
        Next
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub UnTypeDeCourrielInconnuEstUneErreur()
        Dim valeurs = Complet()
        valeurs("type_mail_param") = "PAS_UN_TYPE"
        ParametreMailDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs), inclureSmtp:=False)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        Dim valeurs = Complet()
        valeurs.Remove("id")
        ParametreMailDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs), inclureSmtp:=False)
    End Sub

End Class
