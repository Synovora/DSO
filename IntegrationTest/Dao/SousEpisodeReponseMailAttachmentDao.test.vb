Imports Oasis_Common

''' <summary>
''' SousEpisodeReponseMailAttachmentDao contre la base. Le client lourd liste et
''' ouvre les pièces jointes d'un courriel de réponse dans
''' FrmSousEpisodeReponseAttribution : tout tourne sous oasis_client. Aucun DAO
''' n'écrit ces lignes, JeuxSousEpisode les insère.
''' </summary>
<TestClass()> Public Class SousEpisodeReponseMailAttachmentDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SousEpisodeReponseMailAttachmentDao

    Private Const PieceAbsente As Long = 987654321

    <TestMethod()> Public Sub GetByMailId_NeRenvoieQueLesPiecesDuMail()
        Dim idMail = CreerMailReponseSousEpisode()
        Dim autreMail = CreerMailReponseSousEpisode()
        Dim premiere = CreerPieceJointeMailSousEpisode(idMail, 1, "bilan.pdf")
        Dim seconde = CreerPieceJointeMailSousEpisode(idMail, 2, "imagerie.jpg")
        CreerPieceJointeMailSousEpisode(autreMail, 1, "autre.pdf")

        Dim liste = dao.GetSousEpisodeReponseMailAttachmentByMailId(idMail)

        CollectionAssert.AreEquivalent(New Long() {premiere, seconde}, liste.Select(Function(p) p.Id).ToArray())
        Assert.IsTrue(liste.All(Function(p) p.MailId = idMail))
        Dim relue = liste.Single(Function(p) p.Id = seconde)
        Assert.AreEqual("imagerie.jpg", relue.Filename)
        Assert.AreEqual(2L, relue.Part)
    End Sub

    <TestMethod()> Public Sub GetByMailId_MailSansPiece_DonneUneListeVide()
        Dim idMail = CreerMailReponseSousEpisode()
        Assert.AreEqual(0, dao.GetSousEpisodeReponseMailAttachmentByMailId(idMail).Count)
    End Sub

    <TestMethod()> Public Sub GetById_RelitLaPiece()
        Dim idMail = CreerMailReponseSousEpisode()
        Dim id = CreerPieceJointeMailSousEpisode(idMail, 3, "compte-rendu.docx")

        Dim relue = dao.GetSousEpisodeReponseMailAttachmentById(id)

        Assert.AreEqual(id, relue.Id)
        Assert.AreEqual(idMail, relue.MailId)
        Assert.AreEqual("compte-rendu.docx", relue.Filename)
        Assert.AreEqual(3L, relue.Part)
    End Sub

    <TestMethod()> Public Sub GetById_NomDeFichierNull_DonneNothing()
        Dim id = CreerPieceJointeMailSousEpisode(CreerMailReponseSousEpisode(), 1, Nothing)
        Assert.IsNull(dao.GetSousEpisodeReponseMailAttachmentById(id).Filename)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetById_Inexistante_Leve()
        dao.GetSousEpisodeReponseMailAttachmentById(PieceAbsente)
    End Sub

End Class
