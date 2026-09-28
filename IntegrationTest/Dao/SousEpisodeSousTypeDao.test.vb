Imports Oasis_Common

''' <summary>
''' SousEpisodeSousTypeDao contre la base. Le client lourd lit les sous-types dans
''' FrmSousEpisode, FrmSousEpisodeReponseAttribution, FrmAdminTemplateSousEpisode et
''' RadFEpisodeDetail, et SousEpisodeDao.ResumeSousEpisode en tire un
''' dictionnaire : tout tourne sous oasis_client. Create n'a aucun appelant.
''' </summary>
<TestClass()> Public Class SousEpisodeSousTypeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SousEpisodeSousTypeDao

    Private Shared Function NombreDeSousTypes() As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_sous_episode_sous_type"))
    End Function

    <TestMethod()> Public Sub getLstSousEpisodeSousType_SansFiltre_RenvoieTousLesSousTypes()
        Dim liste = dao.getLstSousEpisodeSousType()

        Assert.AreEqual(NombreDeSousTypes(), liste.Count)
        Dim ids = liste.Select(Function(s) s.Id).ToList()
        CollectionAssert.IsSubsetOf(New Long() {SousTypeSeAdressage, SousTypeSeCompteRendu, SousTypeSeCertificat}, ids)
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeSousType_FiltreParType_RelitToutesLesColonnes()
        Dim liste = dao.getLstSousEpisodeSousType(TypeSeCourrier)

        CollectionAssert.AreEquivalent(New Long() {SousTypeSeAdressage, SousTypeSeCompteRendu},
                                       liste.Select(Function(s) s.Id).ToArray())
        Dim adressage = liste.Single(Function(s) s.Id = SousTypeSeAdressage)
        Assert.AreEqual(TypeSeCourrier, adressage.IdSousEpisodeType)
        Assert.AreEqual(HorodateReferentielSe, adressage.HorodateCreation)
        Assert.AreEqual(LibelleSousTypeSeAdressage, adressage.Libelle)
        Assert.AreEqual("MEDICAL,PARAMEDICAL", adressage.RedactionProfilTypes)
        Assert.AreEqual("MEDICAL", adressage.ValidationProfilTypes)
        Assert.IsTrue(adressage.IsALDPossible)
        Assert.IsTrue(adressage.IsReponseRequise)
        Assert.AreEqual(10, adressage.DelaiReponse)
        Assert.AreEqual("Adressage a un specialiste", adressage.Commentaire)

        Dim compteRendu = liste.Single(Function(s) s.Id = SousTypeSeCompteRendu)
        Assert.IsFalse(compteRendu.IsALDPossible)
        Assert.IsFalse(compteRendu.IsReponseRequise)
        Assert.AreEqual(15, compteRendu.DelaiReponse)
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeSousType_TypeSansSousType_DonneUneListeVide()
        Dim idType = CreerTypeSousEpisode("Type vide", "COURRIER", False)
        Assert.AreEqual(0, dao.getLstSousEpisodeSousType(idType).Count)
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeSousType_ColonnesFacultativesNulles()
        Dim idSousType = CreerSousTypeSousEpisode(TypeSeCertificat, "Sous-type incomplet")

        Dim relu = dao.getLstSousEpisodeSousType(TypeSeCertificat).Single(Function(s) s.Id = idSousType)

        Assert.IsFalse(relu.IsReponseRequise)
        ' Délai absent : valeur DelaiDefautReponseSousEpisode de la configuration (15).
        Assert.AreEqual(15, relu.DelaiReponse)
        ' Comportement actuel : un commentaire NULL devient la chaîne "False", le bean
        ' passant False comme valeur de repli à Coalesce au lieu d'une chaîne vide.
        Assert.AreEqual("False", relu.Commentaire)
    End Sub

    <TestMethod()> Public Sub getDictSousEpisodeSousType_IndexeParId()
        Dim tout = dao.getDictSousEpisodeSousType()
        Assert.AreEqual(NombreDeSousTypes(), tout.Count)
        Assert.AreEqual(LibelleSousTypeSeCertificat, tout(SousTypeSeCertificat).Libelle)
        Assert.AreEqual(TypeSeCertificat, tout(SousTypeSeCertificat).IdSousEpisodeType)

        Dim courriers = dao.getDictSousEpisodeSousType(TypeSeCourrier)
        CollectionAssert.AreEquivalent(New Long() {SousTypeSeAdressage, SousTypeSeCompteRendu}, courriers.Keys.ToArray())
        Assert.AreEqual(LibelleSousTypeSeCompteRendu, courriers(SousTypeSeCompteRendu).Libelle)
    End Sub

    <TestMethod()> Public Sub getTableSousEpisodeSousType_FiltreEtColonnes()
        Dim table = dao.getTableSousEpisodeSousType(TypeSeCertificat)

        Assert.AreEqual(1, table.Rows.Count)
        Assert.AreEqual(SousTypeSeCertificat, CLng(table.Rows(0)("id")))
        For Each colonne In {"id", "id_sous_episode_type", "horodate_creation", "libelle", "redaction_profil_types",
                             "validation_profil_types", "is_ald_possible", "is_reponse_requise", "delai_reponse", "commentaire"}
            Assert.IsTrue(table.Columns.Contains(colonne), "colonne " & colonne)
        Next
        Assert.AreEqual(NombreDeSousTypes(), dao.getTableSousEpisodeSousType().Rows.Count)
    End Sub

    <TestMethod()> Public Sub Create_TableSansSchema_EchoueSansRienEcrire()
        ' Comportement actuel : l'INSERT vise oa_r_sous_episode_sous_type sans le
        ' préfixe oasis ; le schéma par défaut des comptes est dbo, la table est
        ' introuvable. Aucun écran n'appelle Create aujourd'hui.
        Dim avant = NombreDeSousTypes()
        Try
            dao.Create(New SousEpisodeSousType With {
                .IdSousEpisodeType = TypeSeCourrier, .HorodateCreation = Date.Now, .Libelle = "Nouveau sous-type",
                .RedactionProfilTypes = "MEDICAL", .ValidationProfilTypes = "MEDICAL",
                .IsALDPossible = False, .IsReponseRequise = False, .DelaiReponse = 15, .Commentaire = ""})
            Assert.Fail("L'INSERT sans schéma devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try
        Assert.AreEqual(avant, NombreDeSousTypes())
    End Sub

End Class
