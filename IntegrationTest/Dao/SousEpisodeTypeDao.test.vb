Imports Oasis_Common

''' <summary>
''' SousEpisodeTypeDao contre la base. Le client lourd lit les types dans
''' FrmSousEpisode et FrmAdminTemplateSousEpisode : tout tourne sous oasis_client.
''' Create n'a aucun appelant.
''' </summary>
<TestClass()> Public Class SousEpisodeTypeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SousEpisodeTypeDao

    Private Shared Function NombreDeTypes() As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_sous_episode_type"))
    End Function

    <TestMethod()> Public Sub getLstSousEpisodeType_RenvoieChaqueTypeAvecSesSousTypes()
        Dim liste = dao.getLstSousEpisodeType()

        Assert.AreEqual(NombreDeTypes(), liste.Count)
        Dim courrier = liste.Single(Function(t) t.Id = TypeSeCourrier)
        Assert.AreEqual("COURRIER", courrier.Category)
        Assert.AreEqual(LibelleTypeSeCourrier, courrier.Libelle)
        Assert.AreEqual(HorodateReferentielSe, courrier.HorodateCreation)
        Assert.IsTrue(courrier.IsWithDestinataire)
        CollectionAssert.AreEquivalent(New Long() {SousTypeSeAdressage, SousTypeSeCompteRendu},
                                       courrier.LstSousEpisodeSousType.Select(Function(s) s.Id).ToArray())

        Dim certificat = liste.Single(Function(t) t.Id = TypeSeCertificat)
        Assert.AreEqual("CERTIFICAT", certificat.Category)
        Assert.IsFalse(certificat.IsWithDestinataire)
        CollectionAssert.AreEquivalent(New Long() {SousTypeSeCertificat},
                                       certificat.LstSousEpisodeSousType.Select(Function(s) s.Id).ToArray())
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeType_ColonnesFacultativesNullesDonnentLesValeursParDefaut()
        Dim idType = CreerTypeSousEpisode("Type sans categorie")

        Dim relu = dao.getLstSousEpisodeType().Single(Function(t) t.Id = idType)

        Assert.AreEqual("", relu.Category)
        Assert.IsFalse(relu.IsWithDestinataire)
        Assert.AreEqual("Type sans categorie", relu.Libelle)
        Assert.AreEqual(0, relu.LstSousEpisodeSousType.Count, "aucun sous-type rattaché")
    End Sub

    <TestMethod()> Public Sub getTableSousEpisodeType_RenvoieLesColonnesDuReferentiel()
        Dim table = dao.getTableSousEpisodeType()

        Assert.AreEqual(NombreDeTypes(), table.Rows.Count)
        For Each colonne In {"id", "categorie", "horodate_creation", "libelle", "is_with_destinataire"}
            Assert.IsTrue(table.Columns.Contains(colonne), "colonne " & colonne)
        Next
        Assert.AreEqual(5, table.Columns.Count)
    End Sub

    <TestMethod()> Public Sub Create_TableSansSchema_EchoueSansRienEcrire()
        ' Comportement actuel : l'INSERT vise oa_r_sous_episode_type sans le préfixe
        ' oasis. Les utilisateurs de base créés par la migration comptes-sql-separes
        ' ont dbo pour schéma par défaut : la table est introuvable. Aucun écran
        ' n'appelle Create aujourd'hui.
        Dim avant = NombreDeTypes()
        Try
            dao.Create(New SousEpisodeType With {
                .Category = "COURRIER", .HorodateCreation = Date.Now,
                .Libelle = "Nouveau type", .IsWithDestinataire = False})
            Assert.Fail("L'INSERT sans schéma devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try
        Assert.AreEqual(avant, NombreDeTypes())
    End Sub

End Class
