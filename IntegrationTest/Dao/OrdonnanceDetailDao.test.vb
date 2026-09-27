Imports Oasis_Common

''' <summary>
''' OrdonnanceDetailDao contre la base de test. Toutes ses méthodes sont appelées
''' par le client lourd (écrans d'ordonnance, impression) et tournent donc sous
''' oasis_client ; la lecture des lignes sert aussi à /Sign/Check, sous oasis_web.
''' </summary>
<TestClass()> Public Class OrdonnanceDetailDaoTest
    Inherits TestIntegration

    Private ReadOnly daoDetail As New OrdonnanceDetailDao

    Private Const LigneAbsente As Integer = 987654321

    ''' <summary>Ordonnance sans ligne, pour un patient et un prescripteur neufs.</summary>
    Private Shared Function OrdonnanceVide() As Long
        Return CreerOrdonnance(CreerPatient(), CreerUtilisateur(avecCle:=False), nbLignes:=0)
    End Function

    Private Function Lignes(idOrdonnance As Long) As List(Of OrdonnanceDetail)
        Return daoDetail.GetOrdonnanceLigneByOrdonnanceId(CInt(idOrdonnance))
    End Function

    Private Function Relire(idLigne As Long) As OrdonnanceDetail
        Return daoDetail.GetOrdonnanceLigneById(CInt(idLigne))
    End Function

    ' --- Création et lecture -------------------------------------------------------

    <TestMethod()> Public Sub UneLigneCreeeEstRelueChampParChamp()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idTraitement = CreerTraitement(idPatient, idUtilisateur, 1)
        Dim idOrdonnance = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0)
        Dim saisie As New OrdonnanceDetail With {
            .OrdonnanceId = CInt(idOrdonnance),
            .Traitement = True,
            .TraitementId = CInt(idTraitement),
            .OrdreAffichage = 7,
            .Ald = True,
            .ADelivrer = False,
            .MedicamentCis = 61234567,
            .MedicamentDci = "PARACETAMOL",
            .DateDebut = New Date(2026, 9, 1),
            .DateFin = New Date(2026, 9, 30),
            .Duree = 30,
            .Posologie = "1. 1. 1",
            .PosologieBase = Traitement.EnumBaseCode.HEBDOMADAIRE,
            .PosologieRythme = 2,
            .PosologieMatin = 1,
            .PosologieMidi = 2,
            .PosologieApresMidi = 3,
            .PosologieSoir = 4,
            .FractionMatin = Traitement.EnumFraction.Quart,
            .FractionMidi = Traitement.EnumFraction.Demi,
            .FractionApresMidi = Traitement.EnumFraction.TroisQuart,
            .FractionSoir = Traitement.EnumFraction.Non,
            .PosologieCommentaire = "pendant le repas",
            .Commentaire = "si douleur",
            .Fenetre = True,
            .FenetreDateDebut = New Date(2026, 9, 10),
            .FenetreDateFin = New Date(2026, 9, 15),
            .FenetreCommentaire = "pause",
            .Inactif = True
        }

        Dim retour = daoDetail.CreationOrdonnanceDetail(saisie)

        ' L'INSERT ne lit pas SCOPE_IDENTITY : la valeur renvoyée est toujours 0.
        Assert.AreEqual(0, retour)
        Dim relue = Relire(DerniereLigne(idOrdonnance))
        Assert.IsTrue(relue.LigneId > 0)
        Assert.IsTrue(relue.Traitement)
        Assert.AreEqual(CInt(idTraitement), relue.TraitementId)
        Assert.AreEqual(7, relue.OrdreAffichage)
        Assert.IsTrue(relue.Ald)
        Assert.IsFalse(relue.ADelivrer)
        Assert.AreEqual(61234567, relue.MedicamentCis)
        Assert.AreEqual("PARACETAMOL", relue.MedicamentDci)
        Assert.AreEqual(New Date(2026, 9, 1), relue.DateDebut)
        Assert.AreEqual(New Date(2026, 9, 30), relue.DateFin)
        Assert.AreEqual(30, relue.Duree)
        Assert.AreEqual("1. 1. 1", relue.Posologie)
        Assert.AreEqual("H", relue.PosologieBase)
        Assert.AreEqual(2, relue.PosologieRythme)
        Assert.AreEqual(1, relue.PosologieMatin)
        Assert.AreEqual(2, relue.PosologieMidi)
        Assert.AreEqual(3, relue.PosologieApresMidi)
        Assert.AreEqual(4, relue.PosologieSoir)
        Assert.AreEqual("1/4", relue.FractionMatin)
        Assert.AreEqual("1/2", relue.FractionMidi)
        Assert.AreEqual("3/4", relue.FractionApresMidi)
        Assert.AreEqual("0", relue.FractionSoir)
        Assert.AreEqual("pendant le repas", relue.PosologieCommentaire)
        Assert.AreEqual("si douleur", relue.Commentaire)
        Assert.IsTrue(relue.Fenetre)
        Assert.AreEqual(New Date(2026, 9, 10), relue.FenetreDateDebut)
        Assert.AreEqual(New Date(2026, 9, 15), relue.FenetreDateFin)
        ' Écrit à False quelle que soit la saisie.
        Assert.IsFalse(relue.Inactif)
        ' Ni l'ordonnance ni le commentaire de fenêtre ne sont relus : ils valent donc
        ' 0 et "" dans toute charge signée.
        Assert.AreEqual(0, relue.OrdonnanceId)
        Assert.IsNull(relue.FenetreCommentaire)
    End Sub

    <TestMethod()> Public Sub UneLigneDeCommentaireCommeLaSaisitLEcranDOrdonnance()
        ' Valeurs posées par RadFOrdonnanceDetail pour une ligne ajoutée à la main :
        ' pas de traitement, dates à Date.MaxValue, posologie "0". AddWithValue
        ' envoie une Date en SqlDbType.DateTime, que Date.MaxValue peut dépasser une
        ' fois arrondi au 1/300 s : une SqlTypeException ici viendrait de l'écran, pas
        ' du test.
        Dim idOrdonnance = OrdonnanceVide()
        Dim saisie As New OrdonnanceDetail With {
            .OrdonnanceId = CInt(idOrdonnance),
            .TraitementId = 0,
            .Traitement = False,
            .OrdreAffichage = 1000,
            .Ald = False,
            .ADelivrer = False,
            .MedicamentCis = 0,
            .MedicamentDci = "",
            .DateDebut = Date.MaxValue,
            .DateFin = Date.MaxValue,
            .Duree = 0,
            .Posologie = "0",
            .PosologieBase = "",
            .PosologieRythme = 0,
            .PosologieMatin = 0,
            .PosologieMidi = 0,
            .PosologieApresMidi = 0,
            .PosologieSoir = 0,
            .FractionMatin = "",
            .FractionMidi = "",
            .FractionApresMidi = "",
            .FractionSoir = "",
            .PosologieCommentaire = "Bandelettes urinaires",
            .Commentaire = "",
            .Fenetre = False,
            .FenetreDateDebut = Date.MaxValue,
            .FenetreDateFin = Date.MaxValue,
            .Inactif = False
        }

        daoDetail.CreationOrdonnanceDetail(saisie)

        Dim relue = Relire(DerniereLigne(idOrdonnance))
        Assert.IsFalse(relue.Traitement)
        Assert.AreEqual(0, relue.TraitementId)
        Assert.AreEqual(1000, relue.OrdreAffichage)
        Assert.AreEqual("Bandelettes urinaires", relue.PosologieCommentaire)
        Assert.AreEqual("0", relue.Posologie)
        Assert.AreEqual(Date.MaxValue.Date, relue.DateDebut.Date)
        Assert.AreEqual(Date.MaxValue.Date, relue.FenetreDateFin.Date)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub UneLigneInexistanteLeveUneErreur()
        daoDetail.GetOrdonnanceLigneById(LigneAbsente)
    End Sub

    <TestMethod()> Public Sub LesLignesSontTrieesParOrdreDAffichagePuisParCreation()
        Dim idOrdonnance = OrdonnanceVide()
        Dim troisiemeA = AjouterLigne(idOrdonnance, 3)
        Dim premiere = AjouterLigne(idOrdonnance, 1)
        Dim troisiemeB = AjouterLigne(idOrdonnance, 3)
        Dim deuxieme = AjouterLigne(idOrdonnance, 2)

        Dim lues = Lignes(idOrdonnance)

        CollectionAssert.AreEqual(New Long() {premiere, deuxieme, troisiemeA, troisiemeB},
                                  lues.Select(Function(l) CLng(l.LigneId)).ToArray())
        CollectionAssert.AreEqual(New Integer() {1, 2, 3, 3}, lues.Select(Function(l) l.OrdreAffichage).ToArray())
    End Sub

    <TestMethod()> Public Sub UneOrdonnanceSansLigneDonneUneListeVide()
        Assert.AreEqual(0, Lignes(OrdonnanceVide()).Count)
    End Sub

    <TestMethod()> Public Sub LesLignesDUneAutreOrdonnanceNeSontPasRendues()
        Dim idOrdonnance = OrdonnanceVide()
        Dim idAutre = OrdonnanceVide()
        Dim retenue = AjouterLigne(idOrdonnance, 1)
        AjouterLigne(idAutre, 1)
        AjouterLigne(idAutre, 2)

        Dim lues = Lignes(idOrdonnance)

        Assert.AreEqual(1, lues.Count)
        Assert.AreEqual(retenue, CLng(lues(0).LigneId))
    End Sub

    <TestMethod()> Public Sub LeServeurLitLesLignesDUneOrdonnance()
        ' /Sign/Check relit les lignes quand la charge signée est absente.
        Dim idOrdonnance = OrdonnanceVide()
        AjouterLigne(idOrdonnance, 1)
        AjouterLigne(idOrdonnance, 2)

        UtiliserCompte(Compte.Web)
        Dim lues = Lignes(idOrdonnance)

        Assert.AreEqual(2, lues.Count)
        Assert.AreEqual("DCI TEST 1", lues(0).MedicamentDci)
        Assert.AreEqual("DCI TEST 2", lues(1).MedicamentDci)
    End Sub

    ' --- Sélection ALD / hors ALD (impression) -------------------------------------

    <TestMethod()> Public Sub LesLignesSontSepareesEntreALDEtHorsALD()
        Dim idOrdonnance = OrdonnanceVide()
        Dim aldDeux = AjouterLigne(idOrdonnance, 2, enAld:=True)
        Dim horsAld = AjouterLigne(idOrdonnance, 3, enAld:=False)
        Dim aldUn = AjouterLigne(idOrdonnance, 1, enAld:=True)

        Dim tableAld = daoDetail.GetAllOrdonnanceLigneSelectAldByOrdonnanceId(CInt(idOrdonnance), True)
        Dim tableHorsAld = daoDetail.GetAllOrdonnanceLigneSelectAldByOrdonnanceId(CInt(idOrdonnance), False)

        Assert.AreEqual(2, tableAld.Rows.Count)
        Assert.AreEqual(aldUn, CLng(tableAld.Rows(0)("oa_ordonnance_ligne_id")))
        Assert.AreEqual(aldDeux, CLng(tableAld.Rows(1)("oa_ordonnance_ligne_id")))
        Assert.AreEqual("DCI TEST 1", CStr(tableAld.Rows(0)("oa_traitement_medicament_dci")))
        Assert.IsTrue(CBool(tableAld.Rows(0)("oa_traitement_ald")))
        Assert.AreEqual(1, tableHorsAld.Rows.Count)
        Assert.AreEqual(horsAld, CLng(tableHorsAld.Rows(0)("oa_ordonnance_ligne_id")))
        Assert.IsFalse(CBool(tableHorsAld.Rows(0)("oa_traitement_ald")))
    End Sub

    <TestMethod()> Public Sub UneOrdonnanceSansLigneALDDonneUneTableVide()
        Dim idOrdonnance = OrdonnanceVide()
        AjouterLigne(idOrdonnance, 1, enAld:=False)

        Assert.AreEqual(0, daoDetail.GetAllOrdonnanceLigneSelectAldByOrdonnanceId(CInt(idOrdonnance), True).Rows.Count)
    End Sub

    ' --- Modifications -------------------------------------------------------------

    <TestMethod()> Public Sub LaMentionALDEstPoseePuisRetiree()
        Dim idOrdonnance = OrdonnanceVide()
        Dim cible = AjouterLigne(idOrdonnance, 1)
        Dim voisine = AjouterLigne(idOrdonnance, 2)

        daoDetail.ModificationOrdonnanceDetailALD(CInt(cible), True)
        Assert.IsTrue(Relire(cible).Ald)
        Assert.IsFalse(Relire(voisine).Ald, "seule la ligne visée change")

        daoDetail.ModificationOrdonnanceDetailALD(CInt(cible), False)
        Assert.IsFalse(Relire(cible).Ald)
    End Sub

    <TestMethod()> Public Sub LaDelivranceEstRetireePuisRemise()
        Dim idOrdonnance = OrdonnanceVide()
        Dim cible = AjouterLigne(idOrdonnance, 1)
        Dim voisine = AjouterLigne(idOrdonnance, 2)

        daoDetail.ModificationOrdonnanceDetailDelivrance(CInt(cible), False)
        Assert.IsFalse(Relire(cible).ADelivrer)
        Assert.IsTrue(Relire(voisine).ADelivrer, "seule la ligne visée change")

        daoDetail.ModificationOrdonnanceDetailDelivrance(CInt(cible), True)
        Assert.IsTrue(Relire(cible).ADelivrer)
    End Sub

    <TestMethod()> Public Sub LaPosologieLaDureeEtLeCommentaireSontModifies()
        Dim idOrdonnance = OrdonnanceVide()
        Dim cible = AjouterLigne(idOrdonnance, 1)
        Dim avant = Relire(cible)

        daoDetail.ModificationOrdonnanceDetail(CInt(cible), "au coucher", 14, " 0. 0. 2")

        Dim apres = Relire(cible)
        Assert.AreEqual("au coucher", apres.PosologieCommentaire)
        Assert.AreEqual(14, apres.Duree)
        Assert.AreEqual(" 0. 0. 2", apres.Posologie)
        ' Le reste de la ligne ne bouge pas.
        Assert.AreEqual(avant.MedicamentDci, apres.MedicamentDci)
        Assert.AreEqual(avant.MedicamentCis, apres.MedicamentCis)
        Assert.AreEqual(avant.DateDebut, apres.DateDebut)
        Assert.AreEqual(avant.DateFin, apres.DateFin)
        Assert.AreEqual(avant.PosologieMatin, apres.PosologieMatin)
        Assert.AreEqual(avant.Ald, apres.Ald)
        Assert.AreEqual(avant.ADelivrer, apres.ADelivrer)
    End Sub

    <TestMethod()> Public Sub ModifierUneLigneInexistanteNeChangeRien()
        Dim idOrdonnance = OrdonnanceVide()
        Dim existante = AjouterLigne(idOrdonnance, 1)

        ' Aucune de ces méthodes ne vérifie le nombre de lignes touchées.
        daoDetail.ModificationOrdonnanceDetailALD(LigneAbsente, True)
        daoDetail.ModificationOrdonnanceDetailDelivrance(LigneAbsente, False)
        daoDetail.ModificationOrdonnanceDetail(LigneAbsente, "x", 1, "x")

        Dim relue = Relire(existante)
        Assert.IsFalse(relue.Ald)
        Assert.IsTrue(relue.ADelivrer)
        Assert.AreEqual(" 1. 0. 1", relue.Posologie)
    End Sub

    ' --- Suppression ---------------------------------------------------------------

    <TestMethod()> Public Sub LeClientSupprimeUneLigneEtGardeLesAutres()
        ' Appelée par l'écran d'ordonnance du client lourd, donc sous oasis_client.
        ' La migration retrait-suppression-client n'a rendu DELETE qu'aux tables dont
        ' le code contenait « DELETE FROM » ; cette requête s'écrit « DELETE oasis... »
        ' sans FROM. Une erreur 229 ici signale un droit manquant sur
        ' oa_patient_ordonnance_detail, pas un défaut du test.
        Dim idOrdonnance = OrdonnanceVide()
        Dim supprimee = AjouterLigne(idOrdonnance, 1)
        Dim gardee = AjouterLigne(idOrdonnance, 2)

        daoDetail.SuppressionOrdonnanceDetailByDrcId(supprimee)

        Dim restantes = Lignes(idOrdonnance)
        Assert.AreEqual(1, restantes.Count)
        Assert.AreEqual(gardee, CLng(restantes(0).LigneId))
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_ligne_id = @p0",
                                          supprimee)))
    End Sub

End Class
