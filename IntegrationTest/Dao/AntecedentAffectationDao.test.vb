Imports Oasis_Common

''' <summary>
''' AntecedentAffectationDao contre la base de test : les flèches gauche et droite
''' de la synthèse et de l'épisode (RadFSynthese, RadFEpisodeDetail) font changer un
''' antécédent de niveau et de père. Tout tourne sous oasis_client.
'''
''' Une position s'écrit « niveau/idNiveau1/idNiveau2/ordre1/ordre2/ordre3 »
''' (JeuxAntecedent.PositionAntecedent). Les valeurs attendues sont celles que le
''' code écrit, déroulé appel par appel ; les renumérotations vont de 20 en 20.
''' </summary>
<TestClass()> Public Class AntecedentAffectationDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AntecedentAffectationDao

    Private idUtilisateur As Long
    Private idPatient As Long

    <TestInitialize>
    Public Sub PreparerPatient()
        idUtilisateur = CreerUtilisateur(avecCle:=False)
        idPatient = CreerPatient()
    End Sub

    ''' <summary>Antécédent du patient du test, placé à la position donnée.</summary>
    Private Function Place(niveau As Integer, idNiveau1 As Long, idNiveau2 As Long,
                           ordre1 As Integer, ordre2 As Integer, ordre3 As Integer,
                           Optional statut As String = "P") As Long
        Dim id = CreerAntecedent(idPatient, idUtilisateur, statutAffichage:=statut)
        PlacerAntecedent(id, niveau, idNiveau1, idNiveau2, ordre1, ordre2, ordre3)
        Return id
    End Function

    Private Function PatientDuTest() As Patient
        Return New Patient With {.PatientId = CInt(idPatient)}
    End Function

    Private Sub Annuler(idAntecedent As Long)
        Dim daoAntecedent As New AntecedentDao
        Dim lu = daoAntecedent.GetAntecedentById(CInt(idAntecedent))
        daoAntecedent.AnnulationAntecedent(lu, lu, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)})
    End Sub

    ' --- UpdateAntecedentaAffecter ------------------------------------------------

    <TestMethod()> Public Sub UpdateAntecedentaAffecter_EcritNiveauPeresEtOrdres()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, pere, 0, 20, 20, 0)
        Dim id = CreerAntecedent(idPatient, idUtilisateur)

        Assert.IsTrue(dao.UpdateAntecedentaAffecter(CInt(id), 3, CInt(pere), CInt(fils), 20, 20, 990))

        Assert.AreEqual($"3/{pere}/{fils}/20/20/990", PositionAntecedent(id))
        Assert.AreEqual("P", StatutAntecedent(id), "le statut ne change pas")
    End Sub

    ' --- AntecedentReorganisationOrdre -------------------------------------------

    <TestMethod()> Public Sub AntecedentReorganisationOrdre_Niveau1_RenumeroteEtPropageAuxDescendants()
        Dim deuxieme = Place(1, 0, 0, 50, 0, 0)
        Dim premier = Place(1, 0, 0, 10, 0, 0)
        Dim cache = Place(1, 0, 0, 30, 0, 0, statut:="C")
        Dim annule = Place(1, 0, 0, 5, 0, 0)
        Annuler(annule)
        Dim fils = Place(2, deuxieme, 0, 50, 20, 0)
        Dim petitFils = Place(3, deuxieme, fils, 50, 20, 20)
        Dim autrePatient = CreerAntecedent(CreerPatient("AUTRE", "Patient"), idUtilisateur)
        PlacerAntecedent(autrePatient, 1, 0, 0, 7, 0, 0)

        Assert.IsTrue(dao.AntecedentReorganisationOrdre(0, 1, idPatient, "P"))

        Assert.AreEqual("1/0/0/20/0/0", PositionAntecedent(premier))
        Assert.AreEqual("1/0/0/40/0/0", PositionAntecedent(deuxieme))
        Assert.AreEqual($"2/{deuxieme}/0/40/20/0", PositionAntecedent(fils))
        Assert.AreEqual($"3/{deuxieme}/{fils}/40/20/20", PositionAntecedent(petitFils))
        Assert.AreEqual("1/0/0/30/0/0", PositionAntecedent(cache), "caché, hors du périmètre en mode publié")
        Assert.AreEqual("1/0/0/5/0/0", PositionAntecedent(annule), "inactif, ignoré")
        Assert.AreEqual("1/0/0/7/0/0", PositionAntecedent(autrePatient))
    End Sub

    <TestMethod()> Public Sub AntecedentReorganisationOrdre_Niveau1_AvecLesCaches()
        Dim deuxieme = Place(1, 0, 0, 50, 0, 0)
        Dim premier = Place(1, 0, 0, 10, 0, 0)
        Dim cache = Place(1, 0, 0, 30, 0, 0, statut:="C")

        dao.AntecedentReorganisationOrdre(0, 1, idPatient, "C")

        Assert.AreEqual("1/0/0/20/0/0", PositionAntecedent(premier))
        Assert.AreEqual("1/0/0/40/0/0", PositionAntecedent(cache))
        Assert.AreEqual("1/0/0/60/0/0", PositionAntecedent(deuxieme))
    End Sub

    <TestMethod()> Public Sub AntecedentReorganisationOrdre_Niveau2_NeTouchQueLesFilsDuPere()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim autrePere = Place(1, 0, 0, 40, 0, 0)
        Dim fils1 = Place(2, pere, 0, 20, 70, 0)
        Dim fils2 = Place(2, pere, 0, 20, 30, 0)
        Dim filsAutre = Place(2, autrePere, 0, 40, 10, 0)
        Dim petitFils = Place(3, pere, fils1, 20, 70, 20)

        Assert.IsTrue(dao.AntecedentReorganisationOrdre(CInt(pere), 2, idPatient, "P"))

        Assert.AreEqual($"2/{pere}/0/20/20/0", PositionAntecedent(fils2))
        Assert.AreEqual($"2/{pere}/0/20/40/0", PositionAntecedent(fils1))
        Assert.AreEqual($"3/{pere}/{fils1}/20/40/20", PositionAntecedent(petitFils))
        Assert.AreEqual($"2/{autrePere}/0/40/10/0", PositionAntecedent(filsAutre))
    End Sub

    <TestMethod()> Public Sub AntecedentReorganisationOrdre_Niveau3_RenumeroteLesPetitsFils()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, pere, 0, 20, 20, 0)
        Dim petitFils1 = Place(3, pere, fils, 20, 20, 50)
        Dim petitFils2 = Place(3, pere, fils, 20, 20, 10)

        Assert.IsTrue(dao.AntecedentReorganisationOrdre(CInt(fils), 3, idPatient, "P"))

        Assert.AreEqual($"3/{pere}/{fils}/20/20/20", PositionAntecedent(petitFils2))
        Assert.AreEqual($"3/{pere}/{fils}/20/20/40", PositionAntecedent(petitFils1))
    End Sub

    <TestMethod()> Public Sub AntecedentReorganisationOrdre_NiveauInconnu_RenvoieFauxSansRienChanger()
        Dim id = Place(1, 0, 0, 50, 0, 0)

        Assert.IsFalse(dao.AntecedentReorganisationOrdre(0, 4, idPatient, "P"))

        Assert.AreEqual("1/0/0/50/0/0", PositionAntecedent(id))
    End Sub

    ' --- AffectationOrdreAntecedenetsLies ------------------------------------------

    <TestMethod()> Public Sub AffectationOrdreAntecedenetsLies_Niveau1_DonneLOrdre1AuxDescendantsPublies()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, pere, 0, 20, 20, 0)
        Dim filsCache = Place(2, pere, 0, 20, 40, 0, statut:="C")
        Dim petitFils = Place(3, pere, fils, 20, 20, 20)

        Assert.IsTrue(dao.AffectationOrdreAntecedenetsLies(CInt(pere), 1, 60, idPatient, "P"))

        Assert.AreEqual($"2/{pere}/0/60/20/0", PositionAntecedent(fils))
        Assert.AreEqual($"3/{pere}/{fils}/60/20/20", PositionAntecedent(petitFils))
        Assert.AreEqual($"2/{pere}/0/20/40/0", PositionAntecedent(filsCache))
        Assert.AreEqual("1/0/0/20/0/0", PositionAntecedent(pere), "le père lui-même n'est pas touché")
    End Sub

    <TestMethod()> Public Sub AffectationOrdreAntecedenetsLies_Niveau2_DonneLOrdre2AuxPetitsFils()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, pere, 0, 20, 20, 0)
        Dim petitFils = Place(3, pere, fils, 20, 20, 20)

        Assert.IsTrue(dao.AffectationOrdreAntecedenetsLies(CInt(fils), 2, 80, idPatient, "P"))

        Assert.AreEqual($"3/{pere}/{fils}/20/80/20", PositionAntecedent(petitFils))
    End Sub

    <TestMethod()> Public Sub AffectationOrdreAntecedenetsLies_Niveau3_RenvoieFaux()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Assert.IsFalse(dao.AffectationOrdreAntecedenetsLies(CInt(pere), 3, 80, idPatient, "P"))
    End Sub

    ' --- AffectationAntecedenetsLies -----------------------------------------------
    ' Comportement actuel : la fonction renvoie toujours Faux, même quand elle a
    ' modifié des lignes (CodeRetour n'est jamais passé à Vrai).

    <TestMethod()> Public Sub AffectationAntecedenetsLies_Traitement1_LesFilsDeviennentPetitsFilsDeLaCible()
        Dim cible = Place(1, 0, 0, 20, 0, 0)
        Dim deplace = Place(2, cible, 0, 20, 990, 0)
        Dim fils = Place(2, deplace, 0, 40, 20, 0)

        Assert.IsFalse(dao.AffectationAntecedenetsLies(1, CInt(deplace), CInt(cible), 20, idPatient, "P"))

        Assert.AreEqual($"3/{cible}/{deplace}/20/990/990", PositionAntecedent(fils))
    End Sub

    <TestMethod()> Public Sub AffectationAntecedenetsLies_Traitement2_LesPetitsFilsChangentDeGrandPere()
        Dim ancien = Place(1, 0, 0, 20, 0, 0)
        Dim cible = Place(1, 0, 0, 40, 0, 0)
        Dim deplace = Place(2, cible, 0, 40, 990, 0)
        Dim petitFils = Place(3, ancien, deplace, 20, 20, 20)

        dao.AffectationAntecedenetsLies(2, CInt(deplace), CInt(cible), 40, idPatient, "P")

        Assert.AreEqual($"3/{cible}/{deplace}/40/990/990", PositionAntecedent(petitFils))
    End Sub

    <TestMethod()> Public Sub AffectationAntecedenetsLies_Traitement3_LesPetitsFilsRemontentEnFils()
        Dim grandPere = Place(1, 0, 0, 20, 0, 0)
        Dim deplace = Place(1, 0, 0, 990, 0, 0)
        Dim petitFils = Place(3, grandPere, deplace, 20, 20, 20)

        dao.AffectationAntecedenetsLies(3, CInt(deplace), 0, 990, idPatient, "P")

        Assert.AreEqual($"2/{deplace}/0/990/990/0", PositionAntecedent(petitFils))
    End Sub

    <TestMethod()> Public Sub AffectationAntecedenetsLies_Traitement4_OcculteLesFils()
        Dim deplace = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, deplace, 0, 20, 20, 0)
        Dim petitFils = Place(3, deplace, fils, 20, 20, 20)

        dao.AffectationAntecedenetsLies(4, CInt(deplace), 0, 0, idPatient, "P")

        Assert.AreEqual("1/0/0/0/0/0", PositionAntecedent(fils))
        Assert.AreEqual("O", StatutAntecedent(fils))
        Assert.AreEqual($"3/{deplace}/{fils}/20/20/20", PositionAntecedent(petitFils), "le traitement 4 ne vise que le niveau 2")
        Assert.AreEqual("P", StatutAntecedent(petitFils))
    End Sub

    <TestMethod()> Public Sub AffectationAntecedenetsLies_Traitement5_OcculteLesPetitsFilsParLeNiveau1()
        Dim deplace = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, deplace, 0, 20, 20, 0)
        Dim petitFils = Place(3, deplace, fils, 20, 20, 20)

        dao.AffectationAntecedenetsLies(5, CInt(deplace), 0, 0, idPatient, "P")

        Assert.AreEqual("1/0/0/0/0/0", PositionAntecedent(petitFils))
        Assert.AreEqual("O", StatutAntecedent(petitFils))
        Assert.AreEqual("P", StatutAntecedent(fils))
    End Sub

    <TestMethod()> Public Sub AffectationAntecedenetsLies_Traitement6_OcculteLesPetitsFilsParLeNiveau2()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim deplace = Place(2, pere, 0, 20, 20, 0)
        Dim petitFils = Place(3, pere, deplace, 20, 20, 20)
        Dim petitFilsCache = Place(3, pere, deplace, 20, 20, 40, statut:="C")

        dao.AffectationAntecedenetsLies(6, CInt(deplace), 0, 0, idPatient, "P")

        Assert.AreEqual("1/0/0/0/0/0", PositionAntecedent(petitFils))
        Assert.AreEqual("O", StatutAntecedent(petitFils))
        Assert.AreEqual($"3/{pere}/{deplace}/20/20/40", PositionAntecedent(petitFilsCache), "caché, hors du périmètre en mode publié")
    End Sub

    <TestMethod()> Public Sub AffectationAntecedenetsLies_TraitementInconnu_NeChangeRien()
        Dim deplace = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, deplace, 0, 20, 20, 0)

        Assert.IsFalse(dao.AffectationAntecedenetsLies(7, CInt(deplace), 0, 0, idPatient, "P"))

        Assert.AreEqual($"2/{deplace}/0/20/20/0", PositionAntecedent(fils))
    End Sub

    ' --- AntecedentModificationNiveau : les flèches de la synthèse -----------------

    <TestMethod()> Public Sub ModificationNiveau_1Vers2_DevientFilsDuPrecedentEtEntraineSesFils()
        ' Flèche droite sur B, niveau 1 : B passe sous A, son fils C passe en niveau 3.
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim b = Place(1, 0, 0, 40, 0, 0)
        Dim c = Place(2, b, 0, 40, 20, 0)

        dao.AntecedentModificationNiveau(b, a, 1, 2, a, 0, 20, 990, 0, PatientDuTest(), "P")

        Assert.AreEqual("1/0/0/20/0/0", PositionAntecedent(a))
        Assert.AreEqual($"2/{a}/0/20/20/0", PositionAntecedent(b))
        Assert.AreEqual($"3/{a}/{b}/20/20/20", PositionAntecedent(c))
    End Sub

    <TestMethod()> Public Sub ModificationNiveau_2Vers1_RedevientMajeurEtRemonteSesFils()
        ' Flèche gauche sur B, niveau 2 : l'inverse du cas précédent, B passe après A.
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim b = Place(2, a, 0, 20, 20, 0)
        Dim c = Place(3, a, b, 20, 20, 20)

        dao.AntecedentModificationNiveau(b, 0, 2, 1, 0, 0, 990, 0, 0, PatientDuTest(), "P")

        Assert.AreEqual("1/0/0/20/0/0", PositionAntecedent(a))
        Assert.AreEqual("1/0/0/40/0/0", PositionAntecedent(b))
        Assert.AreEqual($"2/{b}/0/40/20/0", PositionAntecedent(c))
    End Sub

    <TestMethod()> Public Sub ModificationNiveau_2Vers3_OcculteLesFilsQuiPasseraientAuNiveau4()
        ' Flèche droite sur B2, niveau 2 : B2 passe sous B1 ; son fils D ne peut
        ' descendre plus bas et sort de la hiérarchie, occulté.
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim b1 = Place(2, a, 0, 20, 20, 0)
        Dim b2 = Place(2, a, 0, 20, 40, 0)
        Dim d = Place(3, a, b2, 20, 40, 20)

        dao.AntecedentModificationNiveau(b2, b1, 2, 3, a, b1, 20, 20, 990, PatientDuTest(), "P")

        Assert.AreEqual($"3/{a}/{b1}/20/20/20", PositionAntecedent(b2))
        Assert.AreEqual($"2/{a}/0/20/20/0", PositionAntecedent(b1))
        Assert.AreEqual("1/0/0/0/0/0", PositionAntecedent(d))
        Assert.AreEqual("O", StatutAntecedent(d))
        Assert.AreEqual("P", StatutAntecedent(b2))
    End Sub

    <TestMethod()> Public Sub ModificationNiveau_3Vers2_RemonteSousLeGrandPereEnDernier()
        ' Flèche gauche sur C, niveau 3 : C devient frère de B sous A, après lui.
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim b = Place(2, a, 0, 20, 20, 0)
        Dim c = Place(3, a, b, 20, 20, 20)

        dao.AntecedentModificationNiveau(c, a, 3, 2, a, 0, 20, 990, 0, PatientDuTest(), "P")

        Assert.AreEqual($"2/{a}/0/20/40/0", PositionAntecedent(c))
        Assert.AreEqual($"2/{a}/0/20/20/0", PositionAntecedent(b))
    End Sub

    <TestMethod()> Public Sub ModificationNiveau_2Vers2_ChangeDePereAvecSesFils()
        Dim a1 = Place(1, 0, 0, 20, 0, 0)
        Dim a2 = Place(1, 0, 0, 40, 0, 0)
        Dim b = Place(2, a1, 0, 20, 20, 0)
        Dim g = Place(3, a1, b, 20, 20, 20)

        dao.AntecedentModificationNiveau(b, a2, 2, 2, a2, 0, 40, 990, 0, PatientDuTest(), "P")

        Assert.AreEqual($"2/{a2}/0/40/20/0", PositionAntecedent(b))
        Assert.AreEqual($"3/{a2}/{b}/40/20/20", PositionAntecedent(g))
    End Sub

    <TestMethod()> Public Sub ModificationNiveau_1Vers3_OcculteFilsEtPetitsFils()
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim p = Place(2, a, 0, 20, 20, 0)
        Dim b = Place(1, 0, 0, 40, 0, 0)
        Dim c = Place(2, b, 0, 40, 20, 0)
        Dim g = Place(3, b, c, 40, 20, 20)

        dao.AntecedentModificationNiveau(b, p, 1, 3, a, p, 20, 20, 990, PatientDuTest(), "P")

        Assert.AreEqual($"3/{a}/{p}/20/20/20", PositionAntecedent(b))
        Assert.AreEqual("1/0/0/0/0/0", PositionAntecedent(c))
        Assert.AreEqual("O", StatutAntecedent(c))
        Assert.AreEqual("1/0/0/0/0/0", PositionAntecedent(g))
        Assert.AreEqual("O", StatutAntecedent(g))
    End Sub

    <TestMethod()> Public Sub ModificationNiveau_3Vers3_ChangeDePereEnDernier()
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim b1 = Place(2, a, 0, 20, 20, 0)
        Dim b2 = Place(2, a, 0, 20, 40, 0)
        Dim g = Place(3, a, b1, 20, 20, 20)
        Dim g2 = Place(3, a, b2, 20, 40, 20)

        dao.AntecedentModificationNiveau(g, b2, 3, 3, a, b2, 20, 40, 990, PatientDuTest(), "P")

        Assert.AreEqual($"3/{a}/{b2}/20/40/40", PositionAntecedent(g))
        Assert.AreEqual($"3/{a}/{b2}/20/40/20", PositionAntecedent(g2))
    End Sub

    <TestMethod()> Public Sub ModificationNiveau_3Vers1_DevientMajeurEnDernier()
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim b = Place(2, a, 0, 20, 20, 0)
        Dim g = Place(3, a, b, 20, 20, 20)

        dao.AntecedentModificationNiveau(g, 0, 3, 1, 0, 0, 990, 0, 0, PatientDuTest(), "P")

        Assert.AreEqual("1/0/0/40/0/0", PositionAntecedent(g))
        Assert.AreEqual($"2/{a}/0/20/20/0", PositionAntecedent(b))
    End Sub

End Class
