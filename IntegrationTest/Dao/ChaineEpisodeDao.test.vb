Imports Oasis_Common

''' <summary>
''' ChaineEpisodeDao contre la base de test. Les chaînes d'épisodes relient un
''' contexte (antecedent_id) aux antécédents qui le prolongent (chaine_id), et un
''' épisode aux chaînes qu'il suit (oa_relation_chaine_episode). Tout est écrit et
''' lu par le client lourd (RadFContextedetailEdit, RadFEpisodeDetailCreation,
''' RadFEpisodeConclusionContextePatient, RadFEpisodeLigneDeVie) : sous
''' oasis_client, suppressions comprises, que les migrations accordent à ce compte.
''' </summary>
<TestClass()> Public Class ChaineEpisodeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ChaineEpisodeDao

    Private idUtilisateur As Long
    Private idPatient As Long

    Private Sub PreparerPatient()
        idUtilisateur = CreerUtilisateur(avecCle:=False)
        idPatient = CreerPatient()
    End Sub

    Private Function Chainer(contexteId As Long, antecedentId As Long) As Long
        Return dao.Create(New ChaineEpisode With {.AntecedentId = contexteId, .ChaineId = antecedentId})
    End Function

    Private Shared Function Ids(liste As List(Of ChaineEpisode)) As Long()
        Return liste.Select(Function(c) c.Id).OrderBy(Function(i) i).ToArray()
    End Function

    Private Shared Function Trie(ParamArray valeurs() As Long) As Long()
        Return valeurs.OrderBy(Function(i) i).ToArray()
    End Function

    Private Shared Function NombreDeRelations(episodeId As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_relation_chaine_episode WHERE episode_id = @p0", episodeId))
    End Function

    ' --- Chaînes -------------------------------------------------------------------

    <TestMethod()> Public Sub Create_PuisGetById_RelitLaChaine()
        PreparerPatient()
        Dim idContexte = CreerContexteMedical(idPatient, idUtilisateur)
        Dim idAntecedent = CreerAntecedent(idPatient, idUtilisateur)

        Dim idChaine = Chainer(idContexte, idAntecedent)

        Assert.IsTrue(idChaine > 0)
        Dim lue = dao.GetById(idChaine)
        Assert.AreEqual(idChaine, lue.Id)
        Assert.AreEqual(idContexte, lue.AntecedentId)
        Assert.AreEqual(idAntecedent, lue.ChaineId)
    End Sub

    <TestMethod()> Public Sub GetById_ChaineAbsente_LeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetById(987654321))
        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub GetList_ParAntecedent_NeRendQueLesChainesDuContexte()
        PreparerPatient()
        Dim contexteA = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte A")
        Dim contexteB = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte B")
        Dim antecedent1 = CreerAntecedent(idPatient, idUtilisateur)
        Dim antecedent2 = CreerAntecedent(idPatient, idUtilisateur)
        Dim chaineA1 = Chainer(contexteA, antecedent1)
        Dim chaineA2 = Chainer(contexteA, antecedent2)
        Chainer(contexteB, antecedent1)

        ' Surcharge à valeurs simples, celle de RadFContextedetailEdit.RefreshChaineEpisode.
        Dim liste = dao.GetList(contexteA)

        CollectionAssert.AreEqual(Trie(chaineA1, chaineA2), Ids(liste))
        Assert.IsTrue(liste.All(Function(c) c.AntecedentId = contexteA))
    End Sub

    <TestMethod()> Public Sub GetList_ParChaine_NeRendQueLesContextesDeCetAntecedent()
        PreparerPatient()
        Dim contexteA = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte A")
        Dim contexteB = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte B")
        Dim antecedent1 = CreerAntecedent(idPatient, idUtilisateur)
        Dim antecedent2 = CreerAntecedent(idPatient, idUtilisateur)
        Dim chaineA1 = Chainer(contexteA, antecedent1)
        Chainer(contexteA, antecedent2)
        Dim chaineB1 = Chainer(contexteB, antecedent1)

        Dim liste = dao.GetList(0L, antecedent1)

        CollectionAssert.AreEqual(Trie(chaineA1, chaineB1), Ids(liste))
    End Sub

    <TestMethod()> Public Sub GetList_ValeursAZero_RendToutesLesChaines()
        PreparerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim unContexte = CreerContexteMedical(idPatient, idUtilisateur)
        Dim contexteAutre = CreerContexteMedical(autrePatient, idUtilisateur)
        Dim unAntecedent = CreerAntecedent(idPatient, idUtilisateur)
        Dim chaine1 = Chainer(unContexte, unAntecedent)
        Dim chaine2 = Chainer(contexteAutre, unAntecedent)

        ' Comportement actuel : « If chaineId Then » lit 0 comme Faux, le filtre
        ' « = NULL » prévu pour 0 n'est jamais atteint. Zéro ne filtre pas.
        Dim liste = dao.GetList(0L, 0L)

        CollectionAssert.AreEqual(Trie(chaine1, chaine2), Ids(liste))
    End Sub

    <TestMethod()> Public Sub GetList_ParListes_FiltreSurLesIdsDonnes()
        PreparerPatient()
        Dim contexteA = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte A")
        Dim contexteB = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte B")
        Dim contexteC = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte C")
        Dim antecedent1 = CreerAntecedent(idPatient, idUtilisateur)
        Dim antecedent2 = CreerAntecedent(idPatient, idUtilisateur)
        Dim chaineA1 = Chainer(contexteA, antecedent1)
        Dim chaineB2 = Chainer(contexteB, antecedent2)
        Dim chaineC1 = Chainer(contexteC, antecedent1)

        ' Surcharge à listes, celle de RadFEpisodeLigneDeVie (liste de chaînes seule).
        CollectionAssert.AreEqual(Trie(chaineA1, chaineC1),
                                  Ids(dao.GetList(Nothing, New List(Of Long) From {antecedent1})))
        CollectionAssert.AreEqual(Trie(chaineA1, chaineB2),
                                  Ids(dao.GetList(New List(Of Long) From {contexteA, contexteB}, Nothing)))
        CollectionAssert.AreEqual(Trie(chaineC1),
                                  Ids(dao.GetList(New List(Of Long) From {contexteB, contexteC}, New List(Of Long) From {antecedent1})))
        ' Listes vides : aucun filtre.
        CollectionAssert.AreEqual(Trie(chaineA1, chaineB2, chaineC1),
                                  Ids(dao.GetList(New List(Of Long), New List(Of Long))))
    End Sub

    <TestMethod()> Public Sub GetListByPatient_FiltreLePatientPuisLaChaineEtLAntecedent()
        PreparerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim contexteA = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte A")
        Dim contexteB = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte B")
        Dim contexteAutre = CreerContexteMedical(autrePatient, idUtilisateur)
        Dim antecedent1 = CreerAntecedent(idPatient, idUtilisateur)
        Dim antecedent2 = CreerAntecedent(idPatient, idUtilisateur)
        Dim chaineA1 = Chainer(contexteA, antecedent1)
        Dim chaineA2 = Chainer(contexteA, antecedent2)
        Dim chaineB1 = Chainer(contexteB, antecedent1)
        Chainer(contexteAutre, antecedent1)

        CollectionAssert.AreEqual(Trie(chaineA1, chaineA2, chaineB1), Ids(dao.GetListByPatient(CInt(idPatient))))
        CollectionAssert.AreEqual(Trie(chaineA1, chaineB1), Ids(dao.GetListByPatient(CInt(idPatient), chaineId:=antecedent1)))
        CollectionAssert.AreEqual(Trie(chaineA1, chaineA2), Ids(dao.GetListByPatient(CInt(idPatient), antecedentId:=contexteA)))
        CollectionAssert.AreEqual(Trie(chaineA2), Ids(dao.GetListByPatient(CInt(idPatient), antecedent2, contexteA)))
        ' Comportement actuel : 0 vaut Nothing pour un Long, la branche « IS NULL » n'est jamais prise.
        CollectionAssert.AreEqual(Trie(chaineA1, chaineA2, chaineB1), Ids(dao.GetListByPatient(CInt(idPatient), 0, 0)))
    End Sub

    <TestMethod()> Public Sub GetListByPatient_PatientSansChaine_RendUneListeVide()
        PreparerPatient()
        Assert.AreEqual(0, dao.GetListByPatient(CInt(idPatient)).Count)
    End Sub

    <TestMethod()> Public Sub Delete_SupprimeLaChainePuisLeveUneErreurDeConversion()
        PreparerPatient()
        Dim unContexte = CreerContexteMedical(idPatient, idUtilisateur)
        Dim antecedent1 = CreerAntecedent(idPatient, idUtilisateur)
        Dim antecedent2 = CreerAntecedent(idPatient, idUtilisateur)
        Dim chaineSupprimee = Chainer(unContexte, antecedent1)
        Dim chaineGardee = Chainer(unContexte, antecedent2)

        ' Comportement actuel : le DELETE est suivi de « SELECT SCOPE_IDENTITY() », qui
        ' vaut NULL après une suppression ; ExecuteScalar rend DBNull et l'affectation à
        ' un Long lève InvalidCastException (ChaineEpisodeDao.vb, ligne 192), après la
        ' suppression. RadFContextedetailEdit n'intercepte pas l'erreur : décocher une
        ' chaîne en modification de contexte fait planter l'écran.
        Assert.ThrowsException(Of InvalidCastException)(
            Sub() dao.Delete(New ChaineEpisode With {.AntecedentId = unContexte, .ChaineId = antecedent1}))

        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_chaine_episode WHERE id = @p0", chaineSupprimee)))
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_chaine_episode WHERE id = @p0", chaineGardee)))
    End Sub

    <TestMethod()> Public Sub Delete_ChaineAbsente_LeveLaMemeErreurSansRienSupprimer()
        PreparerPatient()
        Dim unContexte = CreerContexteMedical(idPatient, idUtilisateur)
        Dim unAntecedent = CreerAntecedent(idPatient, idUtilisateur)
        Chainer(unContexte, unAntecedent)

        ' Comportement actuel : même InvalidCastException, qu'une ligne ait été supprimée ou non.
        Assert.ThrowsException(Of InvalidCastException)(
            Sub() dao.Delete(New ChaineEpisode With {.AntecedentId = unAntecedent, .ChaineId = unContexte}))

        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_chaine_episode WHERE antecedent_id = @p0", unContexte)))
    End Sub

    ' --- Relations épisode et chaîne -----------------------------------------------

    <TestMethod()> Public Sub AddRelation_RendLeNombreDeLignesEtEnregistreLaRelation()
        PreparerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim unContexte = CreerContexteMedical(idPatient, idUtilisateur)

        ' Comportement actuel : ExecuteNonQuery rend le nombre de lignes insérées (1),
        ' pas l'id que promet le « SELECT SCOPE_IDENTITY() » de la requête.
        Dim retour = dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = idEpisode, .ChaineId = unContexte})

        Assert.AreEqual(1L, retour)
        Dim relations = dao.GetRelationListByEpisode(New Episode With {.Id = idEpisode, .PatientId = idPatient})
        Assert.AreEqual(1, relations.Count)
        Assert.IsTrue(relations(0).Id > 0)
        Assert.AreEqual(idEpisode, relations(0).EpisodeId)
        Assert.AreEqual(unContexte, relations(0).ChaineId)
    End Sub

    <TestMethod()> Public Sub GetRelationListByEpisode_NeRendQueLesRelationsDeLEpisode()
        PreparerPatient()
        Dim episode1 = CreerEpisode(idPatient, idUtilisateur)
        CloturerEpisode(episode1, idUtilisateur)
        Dim episode2 = CreerEpisode(idPatient, idUtilisateur)
        Dim unContexte = CreerContexteMedical(idPatient, idUtilisateur)
        Dim unAntecedent = CreerAntecedent(idPatient, idUtilisateur)
        dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = episode1, .ChaineId = unContexte})
        dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = episode1, .ChaineId = unAntecedent})
        dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = episode2, .ChaineId = unContexte})

        Dim relations = dao.GetRelationListByEpisode(New Episode With {.Id = episode1})

        CollectionAssert.AreEquivalent(New Long() {unContexte, unAntecedent}, relations.Select(Function(r) r.ChaineId).ToArray())
        Assert.IsTrue(relations.All(Function(r) r.EpisodeId = episode1))
        Assert.AreEqual(0, dao.GetRelationListByEpisode(New Episode With {.Id = 987654321}).Count)
    End Sub

    <TestMethod()> Public Sub DeleteRelation_RetireLaSeuleRelationVisee()
        PreparerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim unContexte = CreerContexteMedical(idPatient, idUtilisateur)
        Dim unAntecedent = CreerAntecedent(idPatient, idUtilisateur)
        dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = idEpisode, .ChaineId = unContexte})
        dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = idEpisode, .ChaineId = unAntecedent})

        Dim retour = dao.DeleteRelation(New RelationChaineEpisode With {.EpisodeId = idEpisode, .ChaineId = unContexte})

        Assert.AreEqual(1L, retour, "nombre de lignes supprimées")
        Dim restantes = dao.GetRelationListByEpisode(New Episode With {.Id = idEpisode})
        CollectionAssert.AreEqual(New Long() {unAntecedent}, restantes.Select(Function(r) r.ChaineId).ToArray())
    End Sub

    <TestMethod()> Public Sub DeleteRelation_RelationAbsente_RendZero()
        PreparerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim unContexte = CreerContexteMedical(idPatient, idUtilisateur)
        dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = idEpisode, .ChaineId = unContexte})

        Assert.AreEqual(0L, dao.DeleteRelation(New RelationChaineEpisode With {.EpisodeId = idEpisode, .ChaineId = unContexte + 1000}))

        Assert.AreEqual(1, NombreDeRelations(idEpisode))
    End Sub

    <TestMethod()> Public Sub GetRelationListByPatient_JointLIdDeLaChaineALIdDeLAntecedent()
        PreparerPatient()
        DecalerIdentiteChaineEpisode()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim unContexte = CreerContexteMedical(idPatient, idUtilisateur)
        Dim unAntecedent = CreerAntecedent(idPatient, idUtilisateur)
        Dim idChaine = Chainer(unContexte, unAntecedent)
        Assert.IsTrue(idChaine > 1000000, "identité décalée")
        dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = idEpisode, .ChaineId = unContexte})
        dao.AddRelation(New RelationChaineEpisode With {.EpisodeId = idEpisode, .ChaineId = unAntecedent})

        ' Comportement actuel (« TODO: FIX error » dans le DAO) : la requête joint
        ' oa_chaine_episode.id à oa_antecedent_id et la relation à l'id de la chaîne,
        ' alors que les relations portent l'id de l'antécédent ou du contexte. Un
        ' patient dont les relations sont bien enregistrées n'en obtient aucune.
        ' Aucun écran n'appelle cette méthode.
        Dim relations = dao.GetRelationListByPatient(New Patient With {.PatientId = CInt(idPatient)})

        Assert.AreEqual(0, relations.Count)
        Assert.AreEqual(2, NombreDeRelations(idEpisode))
    End Sub

End Class
