Imports Oasis_Common

''' <summary>
''' SousEpisodeSousSousTypeDao contre la base. Le client lourd lit les
''' sous-sous-types dans FrmSousEpisode, FrmSousEpisodeReponseAttribution,
''' FrmAdminTemplateSousEpisode et RadFEpisodeDetail : tout tourne sous
''' oasis_client. Create n'a aucun appelant.
'''
''' SousEpisodeSousSousType.vb, rangé dans le dossier Dao, est un simple bean : il
''' ne contient aucun accès à la base, son constructeur est exercé ici au travers
''' du DAO.
''' </summary>
<TestClass()> Public Class SousEpisodeSousSousTypeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SousEpisodeSousSousTypeDao

    Private Shared Function NombreDeSousSousTypes() As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_sous_episode_sous_sous_type"))
    End Function

    <TestMethod()> Public Sub getLstSousEpisodeSousSousType_SansFiltre_RenvoieTout()
        Dim liste = dao.getLstSousEpisodeSousSousType()

        Assert.AreEqual(NombreDeSousSousTypes(), liste.Count)
        CollectionAssert.IsSubsetOf(New Long() {SousSousTypeSeBilan, SousSousTypeSeImagerie, SousSousTypeSeAptitude},
                                    liste.Select(Function(s) s.Id).ToList())
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeSousSousType_FiltreParSousType_RelitLesColonnes()
        Dim liste = dao.getLstSousEpisodeSousSousType(SousTypeSeAdressage)

        CollectionAssert.AreEquivalent(New Long() {SousSousTypeSeBilan, SousSousTypeSeImagerie},
                                       liste.Select(Function(s) s.Id).ToArray())
        Dim bilan = liste.Single(Function(s) s.Id = SousSousTypeSeBilan)
        Assert.AreEqual(SousTypeSeAdressage, bilan.IdSousEpisodeSousType)
        Assert.AreEqual(HorodateReferentielSe, bilan.HorodateCreation)
        Assert.AreEqual("Bilan biologique", bilan.Libelle)
        Assert.AreEqual("Bilan", bilan.Commentaire)
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeSousSousType_SousTypeSansDetail_DonneUneListeVide()
        Assert.AreEqual(0, dao.getLstSousEpisodeSousSousType(SousTypeSeCompteRendu).Count)
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeSousSousType_CommentaireNullDonneUneChaineVide()
        Dim id = CreerSousSousTypeSousEpisode(SousTypeSeCompteRendu, "Sans commentaire")

        Dim relu = dao.getLstSousEpisodeSousSousType(SousTypeSeCompteRendu).Single()

        Assert.AreEqual(id, relu.Id)
        Assert.AreEqual("", relu.Commentaire)
    End Sub

    <TestMethod()> Public Sub getTableSousEpisodeSousSousType_FiltreEtColonnes()
        Dim table = dao.getTableSousEpisodeSousSousType(SousTypeSeCertificat)

        Assert.AreEqual(1, table.Rows.Count)
        Assert.AreEqual(SousSousTypeSeAptitude, CLng(table.Rows(0)("id")))
        For Each colonne In {"id", "id_sous_episode_sous_type", "horodate_creation", "libelle", "commentaire"}
            Assert.IsTrue(table.Columns.Contains(colonne), "colonne " & colonne)
        Next
        Assert.AreEqual(NombreDeSousSousTypes(), dao.getTableSousEpisodeSousSousType().Rows.Count)
    End Sub

    <TestMethod()> Public Sub Create_TableSansSchema_EchoueSansRienEcrire()
        ' Comportement actuel : l'INSERT vise oa_r_sous_episode_sous_sous_type sans
        ' le préfixe oasis ; le schéma par défaut des comptes est dbo, la table est
        ' introuvable. Aucun écran n'appelle Create aujourd'hui.
        Dim avant = NombreDeSousSousTypes()
        Try
            dao.Create(New SousEpisodeSousSousType With {
                .IdSousEpisodeSousType = SousTypeSeAdressage, .HorodateCreation = Date.Now,
                .Libelle = "Nouveau detail", .Commentaire = ""})
            Assert.Fail("L'INSERT sans schéma devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try
        Assert.AreEqual(avant, NombreDeSousSousTypes())
    End Sub

End Class
