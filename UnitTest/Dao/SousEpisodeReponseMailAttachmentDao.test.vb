Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par SousEpisodeReponseMailAttachmentDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestSousEpisodeReponseMailAttachmentDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "mailId", "filename", "part"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = SousEpisodeReponseMailAttachmentDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"mailId", 102L},
            {"filename", "valeur_3"},
            {"part", 104L}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.MailId)
        Assert.AreEqual("valeur_3", b.Filename)
        Assert.AreEqual(104L, b.Part)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = SousEpisodeReponseMailAttachmentDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"mailId", 102L},
            {"part", 103L}}))

        Assert.IsNull(b.Filename)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"mailId", 102L},
            {"part", 103L}}
        valeurs.Remove("id")
        SousEpisodeReponseMailAttachmentDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
