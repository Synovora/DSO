Imports Oasis_Common

''' <summary>
''' AntecedentChangementOrdreDao contre la base de test : les boutons haut et bas de
''' la synthèse (RadFSynthese, RadFEpisodeDetail, RadFAntecedentOrdreSelecteur)
''' donnent un nouvel ordre à l'antécédent (UpdateAntecedent, plus ou moins 30) puis
''' renumérotent ses frères de 20 en 20 (AntecedentReorganisationOrdre). Tout tourne
''' sous oasis_client.
''' </summary>
<TestClass()> Public Class AntecedentChangementOrdreDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AntecedentChangementOrdreDao

    Private idUtilisateur As Long
    Private idPatient As Long

    <TestInitialize>
    Public Sub PreparerPatient()
        idUtilisateur = CreerUtilisateur(avecCle:=False)
        idPatient = CreerPatient()
    End Sub

    Private Function Place(niveau As Integer, idNiveau1 As Long, idNiveau2 As Long,
                           ordre1 As Integer, ordre2 As Integer, ordre3 As Integer,
                           Optional statut As String = "P") As Long
        Dim id = CreerAntecedent(idPatient, idUtilisateur, statutAffichage:=statut)
        PlacerAntecedent(id, niveau, idNiveau1, idNiveau2, ordre1, ordre2, ordre3)
        Return id
    End Function

    ' --- UpdateAntecedent ----------------------------------------------------------

    <TestMethod()> Public Sub UpdateAntecedent_NeChangeQueLOrdreDuNiveauDemande()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(3, pere, pere, 20, 20, 20)

        Assert.IsTrue(dao.UpdateAntecedent(CInt(fils), 50, 1))
        Assert.AreEqual($"3/{pere}/{pere}/50/20/20", PositionAntecedent(fils))
        Assert.IsTrue(dao.UpdateAntecedent(CInt(fils), 60, 2))
        Assert.AreEqual($"3/{pere}/{pere}/50/60/20", PositionAntecedent(fils))
        Assert.IsTrue(dao.UpdateAntecedent(CInt(fils), 70, 3))
        Assert.AreEqual($"3/{pere}/{pere}/50/60/70", PositionAntecedent(fils))
    End Sub

    <TestMethod()> Public Sub UpdateAntecedent_NiveauInconnu_RenvoieFauxSansRienChanger()
        Dim id = Place(1, 0, 0, 20, 0, 0)

        Assert.IsFalse(dao.UpdateAntecedent(CInt(id), 50, 4))

        Assert.AreEqual("1/0/0/20/0/0", PositionAntecedent(id))
    End Sub

    ' --- AntecedentReorganisationOrdre -------------------------------------------

    <TestMethod()> Public Sub Monter_Niveau1_RenumeroteEtEntraineLesDescendants()
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim b = Place(1, 0, 0, 40, 0, 0)
        Dim c = Place(1, 0, 0, 60, 0, 0)
        Dim cache = Place(1, 0, 0, 50, 0, 0, statut:="C")
        Dim filsDeC = Place(2, c, 0, 60, 20, 0)
        Dim petitFilsDeC = Place(3, c, filsDeC, 60, 20, 20)

        ' Bouton haut sur C : 60 - 30.
        dao.UpdateAntecedent(CInt(c), 30, 1)
        Assert.IsTrue(dao.AntecedentReorganisationOrdre(1, idPatient, 0, 1, "P"))

        Assert.AreEqual("1/0/0/20/0/0", PositionAntecedent(a))
        Assert.AreEqual("1/0/0/40/0/0", PositionAntecedent(c))
        Assert.AreEqual("1/0/0/60/0/0", PositionAntecedent(b))
        Assert.AreEqual($"2/{c}/0/40/20/0", PositionAntecedent(filsDeC))
        Assert.AreEqual($"3/{c}/{filsDeC}/40/20/20", PositionAntecedent(petitFilsDeC))
        Assert.AreEqual("1/0/0/50/0/0", PositionAntecedent(cache), "caché, hors du périmètre en mode publié")
    End Sub

    <TestMethod()> Public Sub Descendre_Niveau1_AvecLesCaches()
        Dim a = Place(1, 0, 0, 20, 0, 0)
        Dim cache = Place(1, 0, 0, 40, 0, 0, statut:="C")
        Dim b = Place(1, 0, 0, 60, 0, 0)
        Dim annule = Place(1, 0, 0, 30, 0, 0)
        Dim daoAntecedent As New AntecedentDao
        Dim lu = daoAntecedent.GetAntecedentById(CInt(annule))
        daoAntecedent.AnnulationAntecedent(lu, lu, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)})

        ' Bouton bas sur A : 20 + 30.
        dao.UpdateAntecedent(CInt(a), 50, 1)
        dao.AntecedentReorganisationOrdre(1, idPatient, 0, 1, "C")

        Assert.AreEqual("1/0/0/20/0/0", PositionAntecedent(cache))
        Assert.AreEqual("1/0/0/40/0/0", PositionAntecedent(a))
        Assert.AreEqual("1/0/0/60/0/0", PositionAntecedent(b))
        Assert.AreEqual("1/0/0/30/0/0", PositionAntecedent(annule), "inactif, ignoré")
    End Sub

    <TestMethod()> Public Sub Monter_Niveau2_NeRenumeroteQueLesFreresEtEntraineLesPetitsFils()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim autrePere = Place(1, 0, 0, 40, 0, 0)
        Dim fils1 = Place(2, pere, 0, 20, 20, 0)
        Dim fils2 = Place(2, pere, 0, 20, 40, 0)
        Dim filsAutre = Place(2, autrePere, 0, 40, 5, 0)
        Dim petitFils1 = Place(3, pere, fils1, 20, 20, 20)

        dao.UpdateAntecedent(CInt(fils2), 10, 2)
        Assert.IsTrue(dao.AntecedentReorganisationOrdre(2, idPatient, pere, 2, "P"))

        Assert.AreEqual($"2/{pere}/0/20/20/0", PositionAntecedent(fils2))
        Assert.AreEqual($"2/{pere}/0/20/40/0", PositionAntecedent(fils1))
        Assert.AreEqual($"3/{pere}/{fils1}/20/40/20", PositionAntecedent(petitFils1))
        Assert.AreEqual($"2/{autrePere}/0/40/5/0", PositionAntecedent(filsAutre))
    End Sub

    <TestMethod()> Public Sub Monter_Niveau3_RenumeroteLesPetitsFilsDuPere()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, pere, 0, 20, 20, 0)
        Dim petitFils1 = Place(3, pere, fils, 20, 20, 20)
        Dim petitFils2 = Place(3, pere, fils, 20, 20, 40)

        dao.UpdateAntecedent(CInt(petitFils2), 10, 3)
        Assert.IsTrue(dao.AntecedentReorganisationOrdre(3, idPatient, fils, 3, "P"))

        Assert.AreEqual($"3/{pere}/{fils}/20/20/20", PositionAntecedent(petitFils2))
        Assert.AreEqual($"3/{pere}/{fils}/20/20/40", PositionAntecedent(petitFils1))
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidOperationException))>
    Public Sub AntecedentReorganisationOrdre_NiveauInconnu_Leve()
        ' Comportement actuel : pour un niveau inconnu la requête reste vide et
        ' SqlDataAdapter.Fill lève, au lieu de renvoyer Faux comme le laisse
        ' croire CodeRetour.
        Place(1, 0, 0, 20, 0, 0)
        dao.AntecedentReorganisationOrdre(4, idPatient, 0, 4, "P")
    End Sub

    ' --- AffectationOrdreAntecedenetsLies ------------------------------------------

    <TestMethod()> Public Sub AffectationOrdreAntecedenetsLies_Niveau1_DonneLOrdreAuxDescendants()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, pere, 0, 20, 20, 0)
        Dim petitFils = Place(3, pere, fils, 20, 20, 20)

        Assert.IsTrue(CBool(dao.AffectationOrdreAntecedenetsLies(CInt(pere), 1, 80, idPatient, 1, "P")))

        Assert.AreEqual($"2/{pere}/0/80/20/0", PositionAntecedent(fils))
        Assert.AreEqual($"3/{pere}/{fils}/80/20/20", PositionAntecedent(petitFils))
    End Sub

    <TestMethod()> Public Sub AffectationOrdreAntecedenetsLies_Niveau2_DonneLOrdreAuxPetitsFils()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Dim fils = Place(2, pere, 0, 20, 20, 0)
        Dim petitFils = Place(3, pere, fils, 20, 20, 20)
        Dim petitFilsCache = Place(3, pere, fils, 20, 20, 40, statut:="C")

        Assert.IsTrue(CBool(dao.AffectationOrdreAntecedenetsLies(CInt(fils), 2, 80, idPatient, 2, "P")))

        Assert.AreEqual($"3/{pere}/{fils}/20/80/20", PositionAntecedent(petitFils))
        Assert.AreEqual($"3/{pere}/{fils}/20/20/40", PositionAntecedent(petitFilsCache))
    End Sub

    <TestMethod()> Public Sub AffectationOrdreAntecedenetsLies_Niveau3_RenvoieFaux()
        Dim pere = Place(1, 0, 0, 20, 0, 0)
        Assert.IsFalse(CBool(dao.AffectationOrdreAntecedenetsLies(CInt(pere), 3, 80, idPatient, 3, "P")))
    End Sub

End Class
