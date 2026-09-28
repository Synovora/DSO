Imports Oasis_Common

''' <summary>
''' ParcoursHistoDao contre la base de test. L'historique est écrit par ParcoursDao
''' à chaque création, modification et annulation d'intervenant, et lu par
''' RadFParcoursHistoListe : sous oasis_client. L'INSERT nomme la base
''' ([oasis].[oasis]) : les tests qui écrivent commencent par ExigerBaseOasis.
''' </summary>
<TestClass()> Public Class ParcoursHistoDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ParcoursHistoDao

    Private Shared Function HistoDeTest(parcoursId As Long, patientId As Long) As ParcoursHisto
        Return New ParcoursHisto With {
            .Id = CInt(parcoursId),
            .PatientId = CInt(patientId),
            .SpecialiteId = SpecialiteParcoursAutre,
            .CategorieId = CategorieParcoursSuivi,
            .SousCategorieId = SousCategorieParcoursSpecialiste,
            .IntervenantOasis = True,
            .RorId = 77,
            .Commentaire = "Historique de test",
            .Base = "PAR_MOIS",
            .Rythme = 2,
            .Cacher = True,
            .Inactif = False,
            .HistorisationUtilisateurId = 999,
            .HistorisationEtat = 9
        }
    End Function

    <TestMethod()> Public Sub CreationParcoursHisto_EcritLaLigneAvecLEtatEtLAuteurDonnes()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPatient(idPatient, CreerRorParcours("INTERVENANT"))

        Assert.IsTrue(dao.CreationParcoursHisto(HistoDeTest(idParcours, idPatient), New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)},
                                                ParcoursHistoDao.EnumEtatParcoursHisto.Modification))

        Dim lignes = HistoriqueParcours(idParcours)
        Assert.AreEqual(2, lignes.Rows.Count, "création du parcours, puis la ligne écrite ici")
        Dim ligne = lignes.Rows(1)
        Assert.AreEqual(2, CInt(ligne("oa_parcours_histo_etat")), "l'état vient du paramètre, pas de l'objet")
        Assert.AreEqual(idUtilisateur, CLng(ligne("oa_parcours_histo_user_historisation")), "l'auteur est l'utilisateur connecté")
        Assert.AreEqual(Date.Today, CDate(ligne("oa_parcours_histo_date_historisation")).Date)
        Assert.AreEqual(idPatient, CLng(ligne("oa_parcours_patient_id")))
        Assert.AreEqual(SpecialiteParcoursAutre, CInt(ligne("oa_parcours_specialite")))
        Assert.AreEqual(CategorieParcoursSuivi, CInt(ligne("oa_parcours_categorie_id")))
        Assert.AreEqual(SousCategorieParcoursSpecialiste, CInt(ligne("oa_parcours_sous_categorie_id")))
        Assert.IsTrue(CBool(ligne("oa_parcours_intervenant_oasis")))
        Assert.AreEqual(77, CInt(ligne("oa_parcours_ror_id")))
        Assert.AreEqual("Historique de test", CStr(ligne("oa_parcours_commentaire")))
        Assert.AreEqual("PAR_MOIS", CStr(ligne("oa_parcours_base")))
        Assert.AreEqual(2, CInt(ligne("oa_parcours_rythme")))
        Assert.IsTrue(CBool(ligne("oa_parcours_cacher")))
        Assert.IsFalse(CBool(ligne("oa_parcours_inactif")))
    End Sub

    <TestMethod()> Public Sub CreationParcoursHisto_SansCommentaire_LeveUneErreurAvantLEcriture()
        Dim idPatient = CreerPatient()
        Dim histo = HistoDeTest(123, idPatient)
        histo.Commentaire = Nothing

        ' Comportement actuel : Commentaire.ToString est évalué hors du Try, sur Nothing.
        Assert.ThrowsException(Of NullReferenceException)(
            Sub() dao.CreationParcoursHisto(histo, New Utilisateur With {.UtilisateurId = 0}, ParcoursHistoDao.EnumEtatParcoursHisto.Creation))

        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_parcours_histo WHERE oa_parcours_patient_id = @p0", idPatient)))
    End Sub

    <TestMethod()> Public Sub getAllParcoursHistobyParcoursId_RendLesLignesDuParcoursDeLaPlusRecenteALaPlusAncienne()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idRor = CreerRorParcours("INTERVENANT")
        Dim idParcours = CreerParcoursPatient(idPatient, idRor)
        Dim autreParcours = CreerParcoursPatient(idPatient, idRor, specialiteId:=SpecialiteParcoursAutre)
        Dim auteur As New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
        dao.CreationParcoursHisto(HistoDeTest(idParcours, idPatient), auteur, ParcoursHistoDao.EnumEtatParcoursHisto.Modification)
        dao.CreationParcoursHisto(HistoDeTest(idParcours, idPatient), auteur, ParcoursHistoDao.EnumEtatParcoursHisto.Annulation)

        Dim table = dao.getAllParcoursHistobyParcoursId(CInt(idParcours))

        Assert.AreEqual(3, table.Rows.Count)
        CollectionAssert.AreEqual(New Integer() {4, 2, 1},
                                  table.Rows.Cast(Of DataRow)().Select(Function(r) CInt(r("oa_parcours_histo_etat"))).ToArray())
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) CLng(r("oa_parcours_id")) = idParcours))
        Assert.AreEqual(1, dao.getAllParcoursHistobyParcoursId(CInt(autreParcours)).Rows.Count)
    End Sub

    <TestMethod()> Public Sub getAllParcoursHistobyParcoursId_ParcoursSansHistorique_RendUneTableVide()
        Assert.AreEqual(0, dao.getAllParcoursHistobyParcoursId(987654321).Rows.Count)
    End Sub

End Class
