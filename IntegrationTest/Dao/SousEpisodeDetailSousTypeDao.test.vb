Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' SousEpisodeDetailSousTypeDao contre la base. Le client lourd lit les lignes de
''' détail dans FrmSousEpisode, FrmSousEpisodeListe, FrmSousEpisodeReponseAttribution
''' et RadFEpisodeDetail ; il les écrit par SousEpisodeDao.Create, qui appelle
''' Create dans sa transaction. Tout tourne sous oasis_client.
''' </summary>
<TestClass()> Public Class SousEpisodeDetailSousTypeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SousEpisodeDetailSousTypeDao

    ''' <summary>Sous-épisode sans détail, dans un épisode d'un patient neuf.</summary>
    Private Shared Function NouveauSousEpisode(Optional sousSousTypes As Long() = Nothing) As Long
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idEpisode = CreerEpisode(CreerPatient(), idUtilisateur)
        Return CreerSousEpisode(idEpisode, idUtilisateur, sousSousTypes:=sousSousTypes)
    End Function

    Private Shared Function NombreDeDetails(idSousEpisode As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_sous_episode_detail WHERE id_sous_episode = @p0", idSousEpisode))
    End Function

    <TestMethod()> Public Sub Create_HorsTransaction_EnregistreLaLigne()
        Dim idSousEpisode = NouveauSousEpisode()
        Dim detail As New SousEpisodeDetailSousType With {
            .IdSousEpisode = idSousEpisode, .IdSousEpisodeSousSousType = SousSousTypeSeImagerie, .IsALD = True}

        Assert.IsTrue(dao.Create(Nothing, detail, Nothing))

        Assert.IsTrue(detail.Id > 0, "l'id attribué revient sur le bean")
        Dim relu = dao.getLstSousEpisodeDetailSousType(idSousEpisode).Single()
        Assert.AreEqual(detail.Id, relu.Id)
        Assert.AreEqual(idSousEpisode, relu.IdSousEpisode)
        Assert.AreEqual(SousSousTypeSeImagerie, relu.IdSousEpisodeSousSousType)
        Assert.IsTrue(relu.IsALD)
    End Sub

    <TestMethod()> Public Sub Create_DansUneTransactionAnnulee_NeLaissePasDeLigne()
        Dim idSousEpisode = NouveauSousEpisode()
        Dim detail As New SousEpisodeDetailSousType With {
            .IdSousEpisode = idSousEpisode, .IdSousEpisodeSousSousType = SousSousTypeSeBilan, .IsALD = False}

        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using transaction = connexion.BeginTransaction()
                Assert.IsTrue(dao.Create(connexion, detail, transaction))
                Assert.IsTrue(detail.Id > 0)
                transaction.Rollback()
            End Using
        End Using

        Assert.AreEqual(0, NombreDeDetails(idSousEpisode))
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeDetailSousType_NeRenvoieQueLesDetailsDuSousEpisode()
        Dim idSousEpisode = NouveauSousEpisode({SousSousTypeSeBilan, SousSousTypeSeImagerie})
        NouveauSousEpisode({SousSousTypeSeBilan})

        Dim liste = dao.getLstSousEpisodeDetailSousType(idSousEpisode)

        CollectionAssert.AreEquivalent(New Long() {SousSousTypeSeBilan, SousSousTypeSeImagerie},
                                       liste.Select(Function(d) d.IdSousEpisodeSousSousType).ToArray())
        Assert.IsTrue(liste.All(Function(d) d.IdSousEpisode = idSousEpisode))
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeDetailSousType_SansDetail_DonneUneListeVide()
        Assert.AreEqual(0, dao.getLstSousEpisodeDetailSousType(NouveauSousEpisode()).Count)
    End Sub

    <TestMethod()> Public Sub getTableSousEpisodeDetailSousType_JointLeLibelleDuSousSousType()
        Dim idSousEpisode = NouveauSousEpisode({SousSousTypeSeBilan})

        Dim table = dao.getTableSousEpisodeDetailSousType(idSousEpisode)

        Assert.AreEqual(1, table.Rows.Count)
        Dim ligne = table.Rows(0)
        Assert.AreEqual(idSousEpisode, CLng(ligne("id_sous_episode")))
        Assert.AreEqual(SousSousTypeSeBilan, CLng(ligne("id_sous_episode_sous_sous_type")))
        Assert.IsFalse(CBool(ligne("is_ald")))
        Assert.AreEqual("Bilan biologique", CStr(ligne("libelle")))
        Assert.AreEqual(5, table.Columns.Count)
    End Sub

End Class
