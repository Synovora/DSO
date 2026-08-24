Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par MailDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestMailDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "sendMailKey", "sendMailTo", "sendMailCc", "sendMailBcc", "sendMailFrom", "sendMailSender",
        "sendMailSubject", "sendMailMessage", "sendMailPath", "date_creation", "user_creation",
        "sendMailSent"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = MailDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"sendMailKey", 101L},
            {"sendMailTo", "valeur_2"},
            {"sendMailCc", "valeur_3"},
            {"sendMailBcc", "valeur_4"},
            {"sendMailFrom", "valeur_5"},
            {"sendMailSender", "valeur_6"},
            {"sendMailSubject", "valeur_7"},
            {"sendMailMessage", "valeur_8"},
            {"sendMailPath", "valeur_9"},
            {"date_creation", New Date(2024, 11, 11)},
            {"user_creation", 111L},
            {"sendMailSent", "valeur_12"}}))

        Assert.AreEqual(101L, b.sendMailKey)
        Assert.AreEqual("valeur_2", b.sendMailTo)
        Assert.AreEqual("valeur_3", b.sendMailCc)
        Assert.AreEqual("valeur_4", b.sendMailBcc)
        Assert.AreEqual("valeur_5", b.sendMailFrom)
        Assert.AreEqual("valeur_6", b.sendMailSender)
        Assert.AreEqual("valeur_7", b.sendMailSubject)
        Assert.AreEqual("valeur_8", b.sendMailMessage)
        Assert.AreEqual("valeur_9", b.sendMailPath)
        Assert.AreEqual(New Date(2024, 11, 11), b.dateCreation)
        Assert.AreEqual(111L, b.userCreation)
        Assert.AreEqual("valeur_12", b.sendMailSent)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSanssendMailKeyEstUneErreur()
        ' sendMailKey n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"sendMailKey", 101L},
            {"sendMailTo", "valeur_2"},
            {"sendMailCc", "valeur_3"},
            {"sendMailBcc", "valeur_4"},
            {"sendMailFrom", "valeur_5"},
            {"sendMailSender", "valeur_6"},
            {"sendMailSubject", "valeur_7"},
            {"sendMailMessage", "valeur_8"},
            {"sendMailPath", "valeur_9"},
            {"date_creation", New Date(2024, 11, 11)},
            {"user_creation", 111L},
            {"sendMailSent", "valeur_12"}}
        valeurs.Remove("sendMailKey")
        MailDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
