Imports Oasis_Common

''' <summary>
''' TraitementDao contre la base de test. Le client lourd crée, modifie, arrête,
''' annule et supprime les traitements, déclare les allergies et contre-indications
''' et lit toutes les listes : ces appels tournent sous oasis_client. Oasis_Web lit
''' aussi un traitement (/Sign/Check), les traitements en cours et les traitements
''' arrêtés (SyntheseController) : ces lectures sont rejouées sous oasis_web.
''' </summary>
<TestClass()> Public Class TraitementDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New TraitementDao

    Private Const TraitementAbsent As Integer = 987654321

    Private Shared Function Auteur(idUtilisateur As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
    End Function

    Private Shared Function ValeurTraitement(colonne As String, idTraitement As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_traitement WHERE oa_traitement_id = @p0", idTraitement)
    End Function

    Private Shared Function NombreHistorisations(idTraitement As Long, etat As TraitementHistoDao.EnumEtatTraitementHisto) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0" &
                             " AND oa_traitement_histo_etat_historisation = @p1", idTraitement, CInt(etat)))
    End Function

    Private Shared Function DateSynthese(idPatient As Long) As Object
        Return Scalaire("SELECT oa_patient_synthese_date_maj FROM oasis.oa_patient WHERE oa_patient_id = @p0", idPatient)
    End Function

    Private Shared Function IdsDe(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(ligne) CLng(ligne("oa_traitement_id"))).ToArray()
    End Function

    Private Shared Function Creer(idPatient As Long, idUtilisateur As Long, rang As Integer,
                                  Optional dateDebut As Date? = Nothing, Optional dateFin As Date? = Nothing) As Long
        Return EnregistrerTraitement(TraitementDeTest(idPatient, rang, dateDebut, dateFin), idUtilisateur)
    End Function

    ''' <summary>
    ''' Comme l'écran de saisie : le traitement relu en base, l'historique initialisé
    ''' à partir de lui au chargement (InitClasseTraitementHistorisation).
    ''' </summary>
    Private Function Charger(idTraitement As Long, idUtilisateur As Long, ByRef histo As TraitementHisto) As Traitement
        Dim lu = dao.GetTraitementById(CInt(idTraitement))
        histo = New TraitementHisto
        TraitementHistoDao.InitClasseTraitementHistorisation(lu, Auteur(idUtilisateur), histo)
        Return lu
    End Function

    Private Sub Arreter(idTraitement As Long, idUtilisateur As Long, dateFin As Date,
                        Optional allergie As Boolean = False, Optional contreIndication As Boolean = False,
                        Optional commentaire As String = "Arrêt de test")
        Dim histo As TraitementHisto = Nothing
        Dim cible = Charger(idTraitement, idUtilisateur, histo)
        cible.DateFin = dateFin
        cible.ArretCommentaire = commentaire
        cible.Allergie = allergie
        cible.ContreIndication = contreIndication
        cible.UserModification = CInt(idUtilisateur)
        cible.DateModification = Date.Now
        Assert.IsTrue(dao.ArretTraitement(cible, histo, Auteur(idUtilisateur)))
    End Sub

    Private Sub Annuler(idTraitement As Long, idUtilisateur As Long, Optional commentaire As String = "Saisie erronée")
        Dim histo As TraitementHisto = Nothing
        Dim cible = Charger(idTraitement, idUtilisateur, histo)
        cible.AnnulationCommentaire = commentaire
        cible.UserModification = CInt(idUtilisateur)
        cible.DateModification = Date.Now
        Assert.IsTrue(dao.AnnulationTraitement(cible, histo, Auteur(idUtilisateur)))
    End Sub

    ''' <summary>
    ''' Un patient avec un traitement de chaque sorte. Clés : ordre1, ordre2, finAujourdhui
    ''' (en cours), arrete, annule, termine, allergie (déclarée), autrePatient.
    ''' </summary>
    Private Function TraitementsVaries(idPatient As Long, idUtilisateur As Long) As Dictionary(Of String, Long)
        Dim ids As New Dictionary(Of String, Long)
        ids("ordre2") = Creer(idPatient, idUtilisateur, 2)
        ids("ordre1") = Creer(idPatient, idUtilisateur, 1)
        ids("finAujourdhui") = Creer(idPatient, idUtilisateur, 3, Date.Today.AddDays(-5), Date.Today)
        ids("arrete") = Creer(idPatient, idUtilisateur, 4)
        Arreter(ids("arrete"), idUtilisateur, Date.Today.AddDays(10))
        ids("annule") = Creer(idPatient, idUtilisateur, 5)
        Annuler(ids("annule"), idUtilisateur)
        ids("termine") = Creer(idPatient, idUtilisateur, 6, Date.Today.AddDays(-30), Date.Today.AddDays(-1))
        ids("allergie") = DeclarerAllergieOuCI(idPatient, idUtilisateur, CisDeTest + 7, "DCI ALLERGENE", allergie:=True)
        ids("autrePatient") = Creer(CreerPatient("AUTRE", "Patient"), idUtilisateur, 1)
        Return ids
    End Function

    ' --- Correspondances code / libellé --------------------------------------------

    <TestMethod()> Public Sub GetBaseCodeByItem_ChaqueLibelleDonneSonCode()
        Assert.AreEqual("J", dao.GetBaseCodeByItem(Traitement.EnumBaseItem.JOURNALIER))
        Assert.AreEqual("H", dao.GetBaseCodeByItem(Traitement.EnumBaseItem.HEBDOMADAIRE))
        Assert.AreEqual("M", dao.GetBaseCodeByItem(Traitement.EnumBaseItem.MENSUEL))
        Assert.AreEqual("A", dao.GetBaseCodeByItem(Traitement.EnumBaseItem.ANNUEL))
        Assert.AreEqual("C", dao.GetBaseCodeByItem(Traitement.EnumBaseItem.CONDITIONNEL))
        Assert.AreEqual("", dao.GetBaseCodeByItem("Quotidien"))
    End Sub

    <TestMethod()> Public Sub GetBseItemByCode_ChaqueCodeDonneSonLibelle()
        Assert.AreEqual("Journalier", dao.GetBseItemByCode("J"))
        Assert.AreEqual("Hebdomadaire", dao.GetBseItemByCode("H"))
        Assert.AreEqual("Mensuel", dao.GetBseItemByCode("M"))
        Assert.AreEqual("Annuel", dao.GetBseItemByCode("A"))
        Assert.AreEqual("Conditionnel", dao.GetBseItemByCode("C"))
        Assert.AreEqual("", dao.GetBseItemByCode("X"))
    End Sub

    <TestMethod()> Public Sub GetBaseDescription_DonneLePrefixeDeLaPosologie()
        Assert.AreEqual("Conditionnel : ", dao.GetBaseDescription("C"))
        Assert.AreEqual("Hebdo : ", dao.GetBaseDescription("H"))
        Assert.AreEqual("Mensuel : ", dao.GetBaseDescription("M"))
        Assert.AreEqual("Annuel : ", dao.GetBaseDescription("A"))
        ' Comportement actuel : le journalier n'a pas de cas, les appelants ne
        ' demandent le préfixe que pour les autres bases.
        Assert.AreEqual("Base inconnue ! ", dao.GetBaseDescription("J"))
    End Sub

    ' --- Création et lecture ---------------------------------------------------------

    <TestMethod()> Public Sub CreationTraitement_SousClient_EnregistreLeTraitementSonHistoriqueEtLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim saisi = TraitementDeTest(idPatient, 3, Date.Today.AddDays(-2), Date.Today.AddDays(28))
        saisi.FractionMatin = Traitement.EnumFraction.Demi
        saisi.PosologieMidi = 2
        Dim histo As New TraitementHisto

        Assert.IsTrue(dao.CreationTraitement(saisi, histo, Auteur(idUtilisateur)))

        Dim id = CLng(Scalaire("SELECT MAX(oa_traitement_id) FROM oasis.oa_traitement WHERE oa_traitement_patient_id = @p0", idPatient))
        Assert.AreEqual(CInt(id), histo.HistorisationTraitementId, "l'id créé revient dans l'historique")
        Dim relu = dao.GetTraitementById(CInt(id))
        Assert.AreEqual(CInt(id), relu.TraitementId)
        Assert.AreEqual(CInt(idPatient), relu.PatientId)
        Assert.AreEqual(CisDeTest + 3, relu.MedicamentId)
        Assert.AreEqual("DCI TEST 3", relu.MedicamentDci)
        Assert.AreEqual("MEDICAMENT TEST 3 500 mg, comprimé", relu.DenominationLongue)
        Assert.AreEqual("N02BE01", relu.ClasseAtc)
        Assert.AreEqual(CInt(idUtilisateur), relu.UserCreation)
        Assert.AreEqual(0, relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateCreation.Date)
        Assert.AreEqual(Date.Today.AddDays(-2), relu.DateDebut)
        Assert.AreEqual(Date.Today.AddDays(28), relu.DateFin)
        Assert.AreEqual(3, relu.OrdreAffichage)
        Assert.AreEqual("J", relu.PosologieBase)
        Assert.AreEqual(0, relu.PosologieRythme)
        Assert.AreEqual(1, relu.PosologieMatin)
        Assert.AreEqual(2, relu.PosologieMidi)
        Assert.AreEqual(0, relu.PosologieApresMidi)
        Assert.AreEqual(1, relu.PosologieSoir)
        Assert.AreEqual("1/2", relu.FractionMatin)
        Assert.AreEqual("0", relu.FractionMidi)
        Assert.AreEqual("0", relu.FractionApresMidi)
        Assert.AreEqual("0", relu.FractionSoir)
        Assert.AreEqual("Pendant le repas", relu.PosologieCommentaire)
        Assert.AreEqual("Commentaire 3", relu.Commentaire)
        Assert.IsFalse(relu.Allergie)
        Assert.IsFalse(relu.ContreIndication)
        Assert.IsFalse(relu.DeclaratifHorsTraitement)
        Assert.IsFalse(relu.Fenetre)
        Assert.AreEqual("", relu.Arret)
        Assert.AreEqual("", relu.Annulation)
        Assert.IsFalse(CBool(ValeurTraitement("oa_traitement_medicament_monographie", id)))

        Assert.AreEqual(1, NombreHistorisations(id, TraitementHistoDao.EnumEtatTraitementHisto.CreationTraitement))
        Assert.AreEqual(CInt(idUtilisateur), CInt(Scalaire("SELECT oa_traitement_histo_utilisateur_historisation FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0", id)))
        Assert.AreEqual("DCI TEST 3", CStr(Scalaire("SELECT oa_traitement_medicament_dci FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0", id)))
        ' Comportement actuel : InitClasseTraitementHistorisation recopie la date de
        ' début du traitement dans la date de début de fenêtre de l'historique.
        Assert.AreEqual(Date.Today.AddDays(-2),
                        CDate(Scalaire("SELECT oa_traitement_fenetre_date_debut FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0", id)).Date)
        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetTraitementById_Inexistant_LeveUneErreur()
        dao.GetTraitementById(TraitementAbsent)
    End Sub

    <TestMethod()> Public Sub GetTraitementById_SousWeb_RelitLeTraitement()
        ' SignController relit le traitement de chaque ligne d'une ordonnance signée.
        Dim idPatient = CreerPatient()
        Dim id = Creer(idPatient, CreerUtilisateur(avecCle:=False), 4)

        UtiliserCompte(Compte.Web)
        Dim relu = dao.GetTraitementById(CInt(id))

        Assert.AreEqual(CInt(idPatient), relu.PatientId)
        Assert.AreEqual("DCI TEST 4", relu.MedicamentDci)
        Assert.AreEqual(Date.Today.AddDays(60), relu.DateFin)
    End Sub

    ' --- Traitements en cours --------------------------------------------------------

    <TestMethod()> Public Sub GetTraitementsEnCoursbyPatient_RetientLesTraitementsActifsParOrdreDAffichage()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ids = TraitementsVaries(idPatient, idUtilisateur)

        Dim enCours = dao.GetTraitementsEnCoursbyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {ids("ordre1"), ids("ordre2"), ids("finAujourdhui")},
                                  enCours.Select(Function(t) CLng(t.TraitementId)).ToArray(),
                                  "ni arrêté, ni annulé, ni terminé hier, ni allergie, ni autre patient")
        Assert.AreEqual("DCI TEST 1", enCours(0).MedicamentDci)
        Assert.AreEqual(Date.Today, enCours(2).DateFin, "une fin aujourd'hui est encore en cours")
    End Sub

    <TestMethod()> Public Sub GetTraitementsEnCoursbyPatient_PatientSansTraitement_ListeVide()
        Assert.AreEqual(0, dao.GetTraitementsEnCoursbyPatient(CInt(CreerPatient())).Count)
    End Sub

    <TestMethod()> Public Sub GetTraitementEnCoursbyPatient_SousClient_MemeFiltreEnTable()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ids = TraitementsVaries(idPatient, idUtilisateur)

        Dim table = dao.GetTraitementEnCoursbyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {ids("ordre1"), ids("ordre2"), ids("finAujourdhui")}, IdsDe(table))
        Dim premiere = table.Rows(0)
        Assert.AreEqual("DCI TEST 1", CStr(premiere("oa_traitement_medicament_dci")))
        Assert.AreEqual("MEDICAMENT TEST 1 500 mg, comprimé", CStr(premiere("oa_traitement_denomination_longue")))
        Assert.AreEqual("N02BE01", CStr(premiere("oa_traitement_classe_atc")))
        Assert.AreEqual("J", CStr(premiere("oa_traitement_posologie_base")))
    End Sub

    <TestMethod()> Public Sub GetTraitementEnCoursbyPatient_SousWeb_LitLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ids = TraitementsVaries(idPatient, idUtilisateur)

        UtiliserCompte(Compte.Web)
        Dim table = dao.GetTraitementEnCoursbyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {ids("ordre1"), ids("ordre2"), ids("finAujourdhui")}, IdsDe(table))
    End Sub

    <TestMethod()> Public Sub GetTraitementEnCoursbyPatient_PatientSansTraitement_TableVide()
        Assert.AreEqual(0, dao.GetTraitementEnCoursbyPatient(CInt(CreerPatient())).Rows.Count)
    End Sub

    ' --- Allergies et contre-indications -----------------------------------------------

    <TestMethod()> Public Sub GetAllTraitementCIbyPatient_RetientLesContreIndicationsNonAnnuleesParFinDecroissante()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ciTot = Creer(idPatient, idUtilisateur, 1)
        Arreter(ciTot, idUtilisateur, Date.Today.AddDays(-20), contreIndication:=True)
        Dim ciTard = Creer(idPatient, idUtilisateur, 2)
        Arreter(ciTard, idUtilisateur, Date.Today.AddDays(-2), contreIndication:=True)
        Dim ciDeclaree = DeclarerAllergieOuCI(idPatient, idUtilisateur, CisDeTest + 3, "DCI CI", allergie:=False)
        Dim ciAnnulee = Creer(idPatient, idUtilisateur, 4)
        Arreter(ciAnnulee, idUtilisateur, Date.Today.AddDays(-1), contreIndication:=True)
        Annuler(ciAnnulee, idUtilisateur)
        DeclarerAllergieOuCI(idPatient, idUtilisateur, CisDeTest + 5, "DCI ALLERGENE", allergie:=True)
        Creer(idPatient, idUtilisateur, 6)
        DeclarerAllergieOuCI(CreerPatient("AUTRE", "Patient"), idUtilisateur, CisDeTest + 7, "DCI CI AUTRE", allergie:=False)

        Dim table = dao.GetAllTraitementCIbyPatient(CInt(idPatient))

        ' La déclaration n'a pas de date de fin : NULL passe en dernier dans un tri décroissant.
        CollectionAssert.AreEqual(New Long() {ciTard, ciTot, ciDeclaree}, IdsDe(table))
        Assert.IsTrue(CBool(table.Rows(0)("oa_traitement_contre_indication")))
        Assert.AreEqual("Arrêt de test", CStr(table.Rows(0)("oa_traitement_arret_commentaire")))
    End Sub

    <TestMethod()> Public Sub GetAllTraitementAllergiebyPatient_RetientLesAllergiesNonAnnulees()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim allergieArret = Creer(idPatient, idUtilisateur, 1)
        Arreter(allergieArret, idUtilisateur, Date.Today.AddDays(-3), allergie:=True)
        Dim allergieDeclaree = DeclarerAllergieOuCI(idPatient, idUtilisateur, CisDeTest + 2, "DCI ALLERGENE", allergie:=True)
        Dim allergieAnnulee = Creer(idPatient, idUtilisateur, 3)
        Arreter(allergieAnnulee, idUtilisateur, Date.Today.AddDays(-1), allergie:=True)
        Annuler(allergieAnnulee, idUtilisateur)
        DeclarerAllergieOuCI(idPatient, idUtilisateur, CisDeTest + 4, "DCI CI", allergie:=False)

        Dim table = dao.GetAllTraitementAllergiebyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {allergieArret, allergieDeclaree}, IdsDe(table))
        Assert.IsTrue(CBool(table.Rows(1)("oa_traitement_allergie")))
    End Sub

    <TestMethod()> Public Sub GetAllTraitementAllergiebyPatient_PatientSansAllergie_TableVide()
        Dim idPatient = CreerPatient()
        Creer(idPatient, CreerUtilisateur(avecCle:=False), 1)
        Assert.AreEqual(0, dao.GetAllTraitementAllergiebyPatient(CInt(idPatient)).Rows.Count)
    End Sub

    ' --- Traitements obsolètes et arrêtés -------------------------------------------

    <TestMethod()> Public Sub GetAllTraitementObsoletebyPatient_RetientLesFinsEntreLeFiltreEtAujourdhui()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fin3 = Creer(idPatient, idUtilisateur, 1, Date.Today.AddDays(-60), Date.Today.AddDays(-3))
        Dim fin10 = Creer(idPatient, idUtilisateur, 2, Date.Today.AddDays(-60), Date.Today.AddDays(-10))
        Dim fin30 = Creer(idPatient, idUtilisateur, 3, Date.Today.AddDays(-60), Date.Today.AddDays(-30))
        Creer(idPatient, idUtilisateur, 4, Date.Today.AddDays(-60), Date.Today.AddDays(-31))
        Creer(idPatient, idUtilisateur, 5, Date.Today, Date.Today.AddDays(30))
        Dim annule = Creer(idPatient, idUtilisateur, 6)
        Annuler(annule, idUtilisateur)
        Creer(CreerPatient("AUTRE", "Patient"), idUtilisateur, 7, Date.Today.AddDays(-60), Date.Today.AddDays(-3))

        Dim table = dao.GetAllTraitementObsoletebyPatient(CInt(idPatient), Date.Today.AddDays(-30))

        ' Comportement actuel : l'annulation fixe la fin à maintenant, le traitement
        ' annulé figure donc parmi les obsolètes.
        CollectionAssert.AreEqual(New Long() {annule, fin3, fin10, fin30}, IdsDe(table))
        Assert.AreEqual("A", CStr(table.Rows(0)("oa_traitement_annulation")))
        Assert.AreEqual(CisDeTest + 1, CInt(table.Rows(1)("oa_traitement_medicament_cis")))
    End Sub

    <TestMethod()> Public Sub GetAllTraitementObsoletebyPatient_AucunTraitementTermine_TableVide()
        Dim idPatient = CreerPatient()
        Creer(idPatient, CreerUtilisateur(avecCle:=False), 1)
        Assert.AreEqual(0, dao.GetAllTraitementObsoletebyPatient(CInt(idPatient), Date.Today.AddDays(-365)).Rows.Count)
    End Sub

    <TestMethod()> Public Sub GetAllTraitementArreteByPatient_RetientLesArretsDuPlusRecentAuPlusAncien()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ancien = Creer(idPatient, idUtilisateur, 1)
        Arreter(ancien, idUtilisateur, Date.Today, commentaire:="Intolérance")
        FixerDateModificationTraitement(ancien, Date.Today.AddDays(-2))
        Dim recent = Creer(idPatient, idUtilisateur, 2)
        Arreter(recent, idUtilisateur, Date.Today, commentaire:="Inefficace")
        FixerDateModificationTraitement(recent, Date.Today.AddDays(-1))
        Creer(idPatient, idUtilisateur, 3)
        Dim autre = Creer(CreerPatient("AUTRE", "Patient"), idUtilisateur, 4)
        Arreter(autre, idUtilisateur, Date.Today)

        Dim table = dao.GetAllTraitementArreteByPatient(CInt(idPatient))

        Assert.AreEqual(2, table.Rows.Count)
        Assert.AreEqual("DCI TEST 2", CStr(table.Rows(0)("oa_traitement_medicament_dci")))
        Assert.AreEqual("Inefficace", CStr(table.Rows(0)("oa_traitement_arret_commentaire")))
        Assert.AreEqual(Date.Today.AddDays(-1), CDate(table.Rows(0)("oa_traitement_date_modification")).Date)
        Assert.AreEqual("DCI TEST 1", CStr(table.Rows(1)("oa_traitement_medicament_dci")))
        Assert.AreEqual("Intolérance", CStr(table.Rows(1)("oa_traitement_arret_commentaire")))
    End Sub

    <TestMethod()> Public Sub GetAllTraitementArreteByPatient_SousWeb_LitLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim arrete = Creer(idPatient, idUtilisateur, 1)
        Arreter(arrete, idUtilisateur, Date.Today, commentaire:="Intolérance")

        UtiliserCompte(Compte.Web)
        Dim table = dao.GetAllTraitementArreteByPatient(CInt(idPatient))

        Assert.AreEqual(1, table.Rows.Count)
        Assert.AreEqual("Intolérance", CStr(table.Rows(0)("oa_traitement_arret_commentaire")))
    End Sub

    ' --- Modification ------------------------------------------------------------------

    <TestMethod()> Public Sub ModificationTraitement_SousClient_EnregistreLaSaisieEtHistorise()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = Creer(idPatient, idUtilisateur, 1)
        Dim histo As TraitementHisto = Nothing
        Dim cible = Charger(id, idModificateur, histo)
        cible.MedicamentId = CisDeTest + 50
        cible.MedicamentDci = "DCI MODIFIEE"
        cible.DenominationLongue = "MEDICAMENT MODIFIE 1 g"
        cible.ClasseAtc = "IGNOREE"
        cible.OrdreAffichage = 9
        cible.PosologieBase = Traitement.EnumBaseCode.HEBDOMADAIRE
        cible.PosologieRythme = 2
        cible.PosologieMatin = 0
        cible.PosologieSoir = 0
        cible.FractionSoir = Traitement.EnumFraction.Quart
        cible.PosologieCommentaire = "Le lundi"
        cible.Commentaire = "Nouveau commentaire"
        cible.DateDebut = Date.Today.AddDays(1)
        cible.DateFin = Date.Today.AddDays(90)
        cible.UserModification = CInt(idModificateur)
        cible.DateModification = Date.Now

        Assert.IsTrue(dao.ModificationTraitement(cible, histo, Auteur(idModificateur)))

        Dim relu = dao.GetTraitementById(CInt(id))
        Assert.AreEqual(CisDeTest + 50, relu.MedicamentId)
        Assert.AreEqual("DCI MODIFIEE", relu.MedicamentDci)
        Assert.AreEqual("MEDICAMENT MODIFIE 1 g", relu.DenominationLongue)
        Assert.AreEqual("N02BE01", relu.ClasseAtc, "la classe ATC n'est pas dans l'UPDATE")
        Assert.AreEqual(9, relu.OrdreAffichage)
        Assert.AreEqual("H", relu.PosologieBase)
        Assert.AreEqual(2, relu.PosologieRythme)
        Assert.AreEqual(0, relu.PosologieMatin)
        Assert.AreEqual(0, relu.PosologieSoir)
        Assert.AreEqual("1/4", relu.FractionSoir)
        Assert.AreEqual("Le lundi", relu.PosologieCommentaire)
        Assert.AreEqual("Nouveau commentaire", relu.Commentaire)
        Assert.AreEqual(Date.Today.AddDays(1), relu.DateDebut)
        Assert.AreEqual(Date.Today.AddDays(90), relu.DateFin)
        Assert.AreEqual(CInt(idModificateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(CInt(idUtilisateur), relu.UserCreation, "le créateur ne change pas")
        Assert.AreEqual(1, NombreHistorisations(id, TraitementHistoDao.EnumEtatTraitementHisto.ModificationTraitement))
        Assert.AreEqual("DCI MODIFIEE", CStr(Scalaire(
            "SELECT oa_traitement_medicament_dci FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0" &
            " AND oa_traitement_histo_etat_historisation = 2", id)))
    End Sub

    <TestMethod()> Public Sub ModificationTraitement_Inexistant_RenvoieVraiEtHistoriseQuandMeme()
        ' Comportement actuel : le nombre de lignes modifiées n'est pas contrôlé ;
        ' l'historique reçoit une modification d'un traitement qui n'existe pas.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fantome = TraitementDeTest(idPatient, 1)
        fantome.TraitementId = TraitementAbsent
        fantome.DateModification = Date.Now
        Dim histo As New TraitementHisto
        TraitementHistoDao.InitClasseTraitementHistorisation(fantome, Auteur(idUtilisateur), histo)

        Assert.IsTrue(dao.ModificationTraitement(fantome, histo, Auteur(idUtilisateur)))

        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_traitement WHERE oa_traitement_id = @p0", TraitementAbsent)))
        Assert.AreEqual(1, NombreHistorisations(TraitementAbsent, TraitementHistoDao.EnumEtatTraitementHisto.ModificationTraitement))
    End Sub

    ' --- Arrêt, annulation, suppression ---------------------------------------------

    <TestMethod()> Public Sub ArretTraitement_SousClient_MarqueLArretEtHistorise()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = Creer(idPatient, idUtilisateur, 1)

        Arreter(id, idUtilisateur, Date.Today.AddDays(-1), allergie:=True, commentaire:="Urticaire")

        Dim relu = dao.GetTraitementById(CInt(id))
        Assert.AreEqual("A", relu.Arret)
        Assert.AreEqual("Urticaire", relu.ArretCommentaire)
        Assert.AreEqual(Date.Today.AddDays(-1), relu.DateFin.Date)
        Assert.IsTrue(relu.Allergie)
        Assert.IsFalse(relu.ContreIndication)
        Assert.AreEqual(CInt(idUtilisateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(1, NombreHistorisations(id, TraitementHistoDao.EnumEtatTraitementHisto.ArretTraitement))
        Assert.AreEqual(0, dao.GetTraitementsEnCoursbyPatient(CInt(idPatient)).Count)
    End Sub

    <TestMethod()> Public Sub AnnulationTraitement_SousClient_MarqueLAnnulationEtFinitAujourdhui()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = Creer(idPatient, idUtilisateur, 1)

        Annuler(id, idUtilisateur, "Doublon")

        Dim relu = dao.GetTraitementById(CInt(id))
        Assert.AreEqual("A", relu.Annulation)
        Assert.AreEqual("Doublon", relu.AnnulationCommentaire)
        Assert.AreEqual(Date.Today, relu.DateFin.Date)
        Assert.AreEqual(CInt(idUtilisateur), relu.UserModification)
        Assert.AreEqual(1, NombreHistorisations(id, TraitementHistoDao.EnumEtatTraitementHisto.AnnulationTraitement))
        Assert.AreEqual("A", CStr(Scalaire(
            "SELECT oa_traitement_annulation FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0" &
            " AND oa_traitement_histo_etat_historisation = 4", id)))
        Assert.AreEqual(0, dao.GetTraitementsEnCoursbyPatient(CInt(idPatient)).Count)
    End Sub

    <TestMethod()> Public Sub AnnulationTraitement_HistoriqueNonInitialise_AnnuleSansHistoriser()
        ' Comportement actuel : l'UPDATE est fait avant l'historique ; un historique
        ' dont les textes sont vides (Nothing) fait ensuite échouer CreationTraitementHisto,
        ' et le traitement reste annulé sans trace ni mise à jour de la synthèse.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = Creer(idPatient, idUtilisateur, 1)
        Executer("UPDATE oasis.oa_patient SET oa_patient_synthese_date_maj = @p0 WHERE oa_patient_id = @p1",
                 Date.Today.AddDays(-10), idPatient)
        Dim cible = dao.GetTraitementById(CInt(id))
        cible.AnnulationCommentaire = "Sans historique"

        Assert.ThrowsException(Of NullReferenceException)(
            Sub() dao.AnnulationTraitement(cible, New TraitementHisto, Auteur(idUtilisateur)))

        Assert.AreEqual("A", CStr(ValeurTraitement("oa_traitement_annulation", id)))
        Assert.AreEqual(0, NombreHistorisations(id, TraitementHistoDao.EnumEtatTraitementHisto.AnnulationTraitement))
        Assert.AreEqual(Date.Today.AddDays(-10), CDate(DateSynthese(idPatient)).Date)
    End Sub

    <TestMethod()> Public Sub SuppressionTraitement_SousClient_SupprimeLaLigneEtGardeLHistorique()
        ' DELETE sur oa_traitement est accordé à oasis_client (retrait-suppression-client).
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = Creer(idPatient, idUtilisateur, 1)
        Dim garde = Creer(idPatient, idUtilisateur, 2)
        Dim histo As TraitementHisto = Nothing
        Dim cible = Charger(id, idUtilisateur, histo)

        Assert.IsTrue(dao.SuppressionTraitement(cible, histo, Auteur(idUtilisateur)))

        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_traitement WHERE oa_traitement_id = @p0", id)))
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_traitement WHERE oa_traitement_id = @p0", garde)))
        Assert.AreEqual(1, NombreHistorisations(id, TraitementHistoDao.EnumEtatTraitementHisto.CreationTraitement))
        Assert.AreEqual(1, NombreHistorisations(id, TraitementHistoDao.EnumEtatTraitementHisto.SuppressionTraitement))
    End Sub

    <TestMethod()> Public Sub SuppressionTraitement_Inexistant_RenvoieVrai()
        ' Comportement actuel : aucune ligne supprimée, aucune erreur, l'historique est écrit.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim fantome = TraitementDeTest(CreerPatient(), 1)
        fantome.TraitementId = TraitementAbsent
        Dim histo As New TraitementHisto
        TraitementHistoDao.InitClasseTraitementHistorisation(fantome, Auteur(idUtilisateur), histo)

        Assert.IsTrue(dao.SuppressionTraitement(fantome, histo, Auteur(idUtilisateur)))
        Assert.AreEqual(1, NombreHistorisations(TraitementAbsent, TraitementHistoDao.EnumEtatTraitementHisto.SuppressionTraitement))
    End Sub

    ' --- Déclaration d'allergie ou de contre-indication ------------------------------

    <TestMethod()> Public Sub DeclarationTraitementAllergieOuCI_Allergie_EnregistreUneLigneDeclarative()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Dim id = DeclarerAllergieOuCI(idPatient, idUtilisateur, CisDeTest + 9, "AMOXICILLINE", allergie:=True, commentaire:="Oedème")

        Dim relu = dao.GetTraitementById(CInt(id))
        Assert.AreEqual(CInt(idPatient), relu.PatientId)
        Assert.AreEqual(CisDeTest + 9, relu.MedicamentId)
        Assert.AreEqual("AMOXICILLINE", relu.MedicamentDci)
        Assert.IsTrue(relu.Allergie)
        Assert.IsFalse(relu.ContreIndication)
        Assert.IsTrue(relu.DeclaratifHorsTraitement)
        Assert.AreEqual("Oedème", relu.ArretCommentaire)
        Assert.AreEqual(CInt(idUtilisateur), relu.UserCreation)
        Assert.AreEqual(Date.Today, relu.DateCreation.Date)
        Assert.AreEqual(DBNull.Value, ValeurTraitement("oa_traitement_date_debut", id))
        Assert.AreEqual(DBNull.Value, ValeurTraitement("oa_traitement_date_fin", id))
        Assert.AreEqual(Date.MinValue, relu.DateFin, "fin absente relue comme date vide")
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0", id)),
                        "la déclaration n'est pas historisée")
        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
    End Sub

    <TestMethod()> Public Sub DeclarationTraitementAllergieOuCI_ContreIndication_EnregistreLeDrapeauCI()
        Dim idPatient = CreerPatient()

        Dim id = DeclarerAllergieOuCI(idPatient, CreerUtilisateur(avecCle:=False), CisDeTest + 8, "IBUPROFENE", allergie:=False)

        Dim relu = dao.GetTraitementById(CInt(id))
        Assert.IsFalse(relu.Allergie)
        Assert.IsTrue(relu.ContreIndication)
        Assert.IsTrue(relu.DeclaratifHorsTraitement)
    End Sub

    ' --- Traitements pour courrier ---------------------------------------------------

    <TestMethod()> Public Sub GetListOfTraitementPatient_FormateLaPosologieDeChaqueTraitementEnCours()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim simple = Creer(idPatient, idUtilisateur, 1)

        Dim fractionne = TraitementDeTest(idPatient, 2)
        fractionne.FractionMatin = Traitement.EnumFraction.Demi
        fractionne.FractionMidi = Traitement.EnumFraction.Quart
        fractionne.PosologieApresMidi = 1
        fractionne.PosologieSoir = 0
        Dim idFractionne = EnregistrerTraitement(fractionne, idUtilisateur)

        Dim hebdo = TraitementDeTest(idPatient, 3)
        hebdo.PosologieBase = Traitement.EnumBaseCode.HEBDOMADAIRE
        hebdo.PosologieRythme = 2
        Dim idHebdo = EnregistrerTraitement(hebdo, idUtilisateur)

        Dim conditionnel = TraitementDeTest(idPatient, 4)
        conditionnel.PosologieBase = Traitement.EnumBaseCode.CONDITIONNEL
        conditionnel.PosologieRythme = 0
        conditionnel.FractionMatin = Traitement.EnumFraction.Demi
        Dim idConditionnel = EnregistrerTraitement(conditionnel, idUtilisateur)

        Dim enFenetre = Creer(idPatient, idUtilisateur, 5)
        PoserFenetreTherapeutique(enFenetre, Date.Today.AddDays(-1), Date.Today.AddDays(5))

        Dim arrete = Creer(idPatient, idUtilisateur, 6)
        Arreter(arrete, idUtilisateur, Date.Today.AddDays(10))

        Dim liste = dao.GetListOfTraitementPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {simple, idFractionne, idHebdo, idConditionnel, enFenetre},
                                  liste.Select(Function(t) CLng(t.TraitementId)).ToArray())
        Assert.IsTrue(liste.All(Function(t) t.PatientId = CInt(idPatient)))
        Assert.AreEqual("DCI TEST 1", liste(0).Denomination)
        Assert.AreEqual(" 1. 0. 1", liste(0).Posologie)
        Assert.AreEqual("1+1/2. 1/4. 1. 0", liste(1).Posologie)
        Assert.AreEqual("Hebdo : 2", liste(2).Posologie)
        Assert.AreEqual("Conditionnel : 1/2", liste(3).Posologie)
        ' Comportement actuel : les dates de fenêtre ne sont jamais lues dans la ligne
        ' (variables locales restées à Date.MinValue), la fenêtre en cours n'est donc
        ' jamais signalée et la posologie normale est imprimée.
        Assert.AreEqual(" 1. 0. 1", liste(4).Posologie)
    End Sub

    <TestMethod()> Public Sub GetListOfTraitementPatient_PatientSansTraitement_ListeVide()
        Assert.AreEqual(0, dao.GetListOfTraitementPatient(CInt(CreerPatient())).Count)
    End Sub

End Class
