Imports Oasis_Common

''' <summary>
''' AntecedentDao contre la base de test. Le client lourd crée, modifie, annule et
''' lit les antécédents : ces appels tournent sous oasis_client. La synthèse web
''' (SyntheseController) lit aussi GetAllAntecedentbyPatient et
''' GetContextebyPatient, éprouvés en plus sous oasis_web.
'''
''' Un antécédent n'est jamais supprimé : l'annulation le passe inactif. Aucune
''' méthode de ce DAO n'émet de DELETE.
''' </summary>
<TestClass()> Public Class AntecedentDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AntecedentDao

    Private Const AntecedentAbsent As Integer = 987654321

    Private Shared Function Auteur(idUtilisateur As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
    End Function

    Private Shared Function Ids(liste As IEnumerable(Of Antecedent)) As Long()
        Return liste.Select(Function(a) CLng(a.Id)).ToArray()
    End Function

    Private Shared Function IdsTable(table As DataTable, Optional colonne As String = "oa_antecedent_id") As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r(colonne))).ToArray()
    End Function

    ''' <summary>Change le statut d'affichage par ModificationAntecedent, comme la fenêtre d'édition.</summary>
    Private Sub ChangerStatut(idAntecedent As Long, statut As String, idUtilisateur As Long)
        Dim lu = dao.GetAntecedentById(CInt(idAntecedent))
        Dim modifie = dao.Clone(lu)
        modifie.StatutAffichage = statut
        dao.ModificationAntecedent(modifie, lu, Auteur(idUtilisateur))
    End Sub

    Private Sub Annuler(idAntecedent As Long, idUtilisateur As Long)
        Dim lu = dao.GetAntecedentById(CInt(idAntecedent))
        dao.AnnulationAntecedent(lu, lu, Auteur(idUtilisateur))
    End Sub

    Private Shared Sub ReculerDateSynthese(idPatient As Long)
        Executer("UPDATE oasis.oa_patient SET oa_patient_synthese_date_maj = @p0 WHERE oa_patient_id = @p1",
                 New Date(2000, 1, 1), idPatient)
    End Sub

    Private Shared Function DateSynthese(idPatient As Long) As Date
        Return CDate(Scalaire("SELECT oa_patient_synthese_date_maj FROM oasis.oa_patient WHERE oa_patient_id = @p0", idPatient))
    End Function

    ' --- Création et lecture -------------------------------------------------------

    <TestMethod()> Public Sub CreationAntecedent_EcritLesValeursFixesDuDao()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc("Asthme")

        Dim idAntecedent = CreerAntecedent(idPatient, idUtilisateur, idDrc, "Asthme depuis l'enfance",
                                           diagnostic:=2, dateDebut:=New Date(2019, 5, 20))

        Assert.IsTrue(idAntecedent > 0)
        Dim lu = dao.GetAntecedentById(CInt(idAntecedent))
        Assert.AreEqual(CInt(idAntecedent), lu.Id)
        Assert.AreEqual(CInt(idPatient), lu.PatientId)
        Assert.AreEqual("A", lu.Type)
        Assert.AreEqual(CInt(idDrc), lu.DrcId)
        Assert.AreEqual("Asthme depuis l'enfance", lu.Description)
        Assert.AreEqual(CInt(idUtilisateur), lu.UserCreation)
        Assert.AreEqual(Date.Today, lu.DateCreation.Date)
        Assert.AreEqual(0, lu.UserModification)
        Assert.AreEqual(2, lu.Diagnostic)
        Assert.AreEqual(New Date(2019, 5, 20), lu.DateDebut)
        Assert.AreEqual(Date.MinValue, lu.DateFin, "date de fin non écrite")
        Assert.AreEqual(1, lu.Niveau)
        Assert.AreEqual(0, lu.Niveau1Id)
        Assert.AreEqual(0, lu.Niveau2Id)
        Assert.AreEqual(980, lu.Ordre1, "un nouvel antécédent arrive en fin de liste")
        Assert.AreEqual(0, lu.Ordre2)
        Assert.AreEqual(0, lu.Ordre3)
        Assert.AreEqual("Patient", lu.Nature)
        Assert.AreEqual("P", lu.StatutAffichage)
        Assert.IsFalse(lu.Inactif)
        Assert.IsFalse(lu.Arret)
        Assert.AreEqual(0L, lu.EpisodeId)
        Assert.IsNull(lu.CategorieContexte)
        Assert.AreEqual(Date.Today.AddMonths(6), lu.ChaineEpisodeDateFin.Date)
    End Sub

    <TestMethod()> Public Sub CreationAntecedent_SansAld_ForceLesChampsAld()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim saisie As New Antecedent With {
            .PatientId = CInt(idPatient),
            .DrcId = CInt(CreerDrc()),
            .Description = "Diabète",
            .DateDebut = DateDebutAntecedentDeTest,
            .StatutAffichage = "P",
            .Diagnostic = 1,
            .AldId = 0,
            .AldCim10Id = 12,
            .AldValide = True,
            .AldDateDebut = New Date(2020, 1, 1),
            .AldDateFin = New Date(2025, 1, 1),
            .AldDemandeEnCours = True,
            .AldDateDemande = New Date(2019, 12, 1),
            .ChaineEpisodeDateFin = Date.Today.AddMonths(6)
        }

        Dim idAntecedent = dao.CreationAntecedent(saisie, Auteur(idUtilisateur))

        Dim lu = dao.GetAntecedentById(CInt(idAntecedent))
        Assert.AreEqual(0, lu.AldId)
        Assert.AreEqual(0, lu.AldCim10Id)
        Assert.IsFalse(lu.AldValide)
        Assert.IsFalse(lu.AldDemandeEnCours)
        Assert.AreEqual(Date.MaxValue.Date, lu.AldDateDebut.Date)
        Assert.AreEqual(Date.MaxValue.Date, lu.AldDateFin.Date)
        Assert.AreEqual(Date.MaxValue.Date, lu.AldDateDemande.Date)
        ' Comportement actuel : l'objet transmis est modifié en place.
        Assert.IsFalse(saisie.AldValide)
        Assert.AreEqual(Date.MaxValue, saisie.AldDateDebut)
    End Sub

    <TestMethod()> Public Sub CreationAntecedent_EcritUneLigneDHistoriqueDeCreation()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()

        Dim idAntecedent = CreerAntecedent(idPatient, idUtilisateur, idDrc, "Fracture du poignet",
                                           diagnostic:=3, dateDebut:=New Date(2018, 2, 3))

        Dim histo = HistoriqueAntecedent(idAntecedent)
        Assert.AreEqual(1, histo.Rows.Count)
        Dim ligne = histo.Rows(0)
        Assert.AreEqual(1, CInt(ligne("oa_antecedent_histo_etat_historisation")), "CreationAntecedent")
        Assert.AreEqual(idUtilisateur, CLng(ligne("oa_antecedent_histo_utilisateur_historisation")))
        Assert.AreEqual(Date.Today, CDate(ligne("oa_antecedent_histo_date_historisation")).Date)
        Assert.AreEqual(idPatient, CLng(ligne("oa_antecedent_patient_id")))
        Assert.AreEqual("A", CStr(ligne("oa_antecedent_type")))
        Assert.AreEqual(idDrc, CLng(ligne("oa_antecedent_drc_id")))
        Assert.AreEqual("Fracture du poignet", CStr(ligne("oa_antecedent_description")))
        Assert.AreEqual(New Date(2018, 2, 3), CDate(ligne("oa_antecedent_date_debut")))
        ' Comportement actuel : la date de fin absente est écrite telle quelle, 0001-01-01
        ' (colonne supposée date ou datetime2).
        Assert.AreEqual(New Date(1, 1, 1), CDate(ligne("oa_antecedent_date_fin")).Date)
        Assert.IsFalse(CBool(ligne("oa_antecedent_arret")))
        Assert.AreEqual("", CStr(ligne("oa_antecedent_arret_commentaire")))
        Assert.AreEqual("Patient", CStr(ligne("oa_antecedent_nature")))
        Assert.AreEqual(1, CInt(ligne("oa_antecedent_niveau")))
        Assert.AreEqual(0, CInt(ligne("oa_antecedent_id_niveau1")))
        Assert.AreEqual(0, CInt(ligne("oa_antecedent_id_niveau2")))
        Assert.AreEqual(980, CInt(ligne("oa_antecedent_ordre_affichage1")))
        Assert.AreEqual(0, CInt(ligne("oa_antecedent_ordre_affichage2")))
        Assert.AreEqual(0, CInt(ligne("oa_antecedent_ordre_affichage3")))
        Assert.AreEqual("P", CStr(ligne("oa_antecedent_statut_affichage")))
        Assert.AreEqual("", CStr(ligne("oa_antecedent_categorie_contexte")))
        Assert.IsFalse(CBool(ligne("oa_antecedent_inactif")))
        Assert.AreEqual(3, CInt(ligne("oa_antecedent_diagnostic")))
        Assert.AreEqual(0, CInt(ligne("oa_antecedent_ald_id")))
        Assert.IsFalse(CBool(ligne("oa_antecedent_ald_valide")))
        Assert.IsFalse(CBool(ligne("oa_antecedent_ald_demande_en_cours")))
        Assert.AreEqual(Date.MaxValue.Date, CDate(ligne("oa_antecedent_ald_date_debut")).Date)
        ' Comportement actuel : l'historique ne recopie pas la fin de chaîne de
        ' l'antécédent mais écrit maintenant + ChaineEpisodePeriode mois (6 dans
        ' app.config, comme en production). Les deux valeurs coïncident ici ; le test
        ' de modification plus bas, qui saisit 2030, les distingue.
        Assert.AreEqual(Date.Today.AddMonths(6), CDate(ligne("oa_chaine_episode_date_fin")).Date)
    End Sub

    <TestMethod()> Public Sub CreationAntecedent_MetAJourLaDateDeSyntheseDuPatient()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        ReculerDateSynthese(idPatient)

        CreerAntecedent(idPatient, idUtilisateur)

        Assert.AreEqual(Date.Today, DateSynthese(idPatient).Date)
    End Sub

    <TestMethod()> Public Sub CreationAntecedent_SansFinDeChaineDEpisodes_EchoueSansRienEcrire()
        ' La fenêtre renseigne toujours ChaineEpisodeDateFin. Laissée à sa valeur par
        ' défaut (01/01/0001), elle sort de la plage d'un datetime SQL et
        ' l'insertion échoue avant d'atteindre la base.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim saisie As New Antecedent With {
            .PatientId = CInt(idPatient),
            .DrcId = CInt(CreerDrc()),
            .Description = "Sans fin de chaîne",
            .DateDebut = DateDebutAntecedentDeTest,
            .StatutAffichage = "P",
            .Diagnostic = 1
        }

        Dim erreur As Exception = Nothing
        Try
            dao.CreationAntecedent(saisie, Auteur(idUtilisateur))
        Catch ex As Exception
            erreur = ex
        End Try

        Assert.IsNotNull(erreur)
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_antecedent WHERE oa_antecedent_patient_id = @p0", idPatient)))
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetAntecedentById_Inexistant_LeveUneErreur()
        dao.GetAntecedentById(AntecedentAbsent)
    End Sub

    ' --- Recherches par DRC --------------------------------------------------------

    <TestMethod()> Public Sub GetByDrcId_RetrouveLAntecedentDuPatientSurCetteDrc()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim idDrc = CreerDrc()
        CreerContexteMedical(idPatient, idUtilisateur, idDrc)
        CreerAntecedent(idAutrePatient, idUtilisateur, idDrc)
        Dim attendu = CreerAntecedent(idPatient, idUtilisateur, idDrc, "Sur la DRC")

        Dim trouve = dao.GetByDrcId(idPatient, idDrc)

        Assert.IsNotNull(trouve)
        Assert.AreEqual(CInt(attendu), trouve.Id)
        Assert.AreEqual("A", trouve.Type)
        Assert.AreEqual("Sur la DRC", trouve.Description)
    End Sub

    <TestMethod()> Public Sub GetByDrcId_SansAntecedentSurCetteDrc_RenvoieNothing()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()
        ' Un contexte sur la même DRC ne compte pas.
        CreerContexteMedical(idPatient, idUtilisateur, idDrc)
        CreerAntecedent(idPatient, idUtilisateur)

        Assert.IsNull(dao.GetByDrcId(idPatient, idDrc))
    End Sub

    <TestMethod()> Public Sub GetListByDrc_RenvoieTousLesAntecedentsDuPatientSurLaDrcMemeInactifs()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim idDrc = CreerDrc()
        Dim premier = CreerAntecedent(idPatient, idUtilisateur, idDrc)
        Dim annule = CreerAntecedent(idPatient, idUtilisateur, idDrc)
        Annuler(annule, idUtilisateur)
        CreerAntecedent(idPatient, idUtilisateur)
        CreerContexteMedical(idPatient, idUtilisateur, idDrc)
        CreerAntecedent(idAutrePatient, idUtilisateur, idDrc)

        Dim liste = dao.GetListByDrc(idPatient, idDrc)

        CollectionAssert.AreEquivalent(New Long() {premier, annule}, Ids(liste))
    End Sub

    <TestMethod()> Public Sub GetListByDrc_SansCorrespondance_RenvoieUneListeVide()
        Dim idPatient = CreerPatient()
        Assert.AreEqual(0, dao.GetListByDrc(idPatient, CreerDrc()).Count)
    End Sub

    <TestMethod()> Public Sub GetList_RenvoieToutesLesLignesDeLaTable()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim antecedent1 = CreerAntecedent(idPatient, idUtilisateur)
        Dim contexte1 = CreerContexteMedical(idPatient, idUtilisateur)
        Dim antecedent2 = CreerAntecedent(idAutrePatient, idUtilisateur)
        Annuler(antecedent2, idUtilisateur)

        CollectionAssert.AreEquivalent(New Long() {antecedent1, contexte1, antecedent2}, Ids(dao.GetList()))
    End Sub

    ' --- GetListByPatient et ses filtres ------------------------------------------

    <TestMethod()> Public Sub GetListByPatient_SansFiltre_RenvoieAntecedentsEtContextesDuPatient()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim actif = CreerAntecedent(idPatient, idUtilisateur)
        Dim cache = CreerAntecedent(idPatient, idUtilisateur, statutAffichage:="C")
        Dim annule = CreerAntecedent(idPatient, idUtilisateur)
        Annuler(annule, idUtilisateur)
        Dim contexte = CreerContexteMedical(idPatient, idUtilisateur)
        CreerAntecedent(idAutrePatient, idUtilisateur)

        CollectionAssert.AreEquivalent(New Long() {actif, cache, annule, contexte}, Ids(dao.GetListByPatient(CInt(idPatient))))
    End Sub

    <TestMethod()> Public Sub GetListByPatient_FiltreSurLeType()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim antecedentA = CreerAntecedent(idPatient, idUtilisateur)
        Dim contexteC = CreerContexteMedical(idPatient, idUtilisateur)

        CollectionAssert.AreEqual(New Long() {antecedentA}, Ids(dao.GetListByPatient(CInt(idPatient), "A")))
        CollectionAssert.AreEqual(New Long() {contexteC}, Ids(dao.GetListByPatient(CInt(idPatient), "C")))
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetListByPatient_TypeInconnu_LeveUneErreur()
        dao.GetListByPatient(CInt(CreerPatient()), "A' OR 1=1 --")
    End Sub

    <TestMethod()> Public Sub GetListByPatient_FiltreSurLeStatutDAffichage()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim publie = CreerAntecedent(idPatient, idUtilisateur)
        Dim cache = CreerAntecedent(idPatient, idUtilisateur, statutAffichage:="C")
        Dim occulte = CreerAntecedent(idPatient, idUtilisateur)
        ChangerStatut(occulte, "O", idUtilisateur)

        CollectionAssert.AreEquivalent(New Long() {publie, cache, occulte},
                                       Ids(dao.GetListByPatient(CInt(idPatient), "A", inclureCaches:=Nothing)))
        CollectionAssert.AreEquivalent(New Long() {publie, cache},
                                       Ids(dao.GetListByPatient(CInt(idPatient), "A", inclureCaches:=True)))
        CollectionAssert.AreEquivalent(New Long() {publie},
                                       Ids(dao.GetListByPatient(CInt(idPatient), "A", inclureCaches:=False)))
    End Sub

    <TestMethod()> Public Sub GetListByPatient_ExclutLesContextesArretesSurDemande()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim enCours = CreerContexteMedical(idPatient, idUtilisateur)
        Dim arrete = CreerContexteMedical(idPatient, idUtilisateur)
        ArreterContexteMedical(arrete)

        CollectionAssert.AreEquivalent(New Long() {enCours, arrete}, Ids(dao.GetListByPatient(CInt(idPatient), "C")))
        CollectionAssert.AreEquivalent(New Long() {enCours},
                                       Ids(dao.GetListByPatient(CInt(idPatient), "C", exclureArretes:=True)))
    End Sub

    <TestMethod()> Public Sub GetListByPatient_ExclutLesInactifsSurDemande()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim actif = CreerAntecedent(idPatient, idUtilisateur)
        Dim annule = CreerAntecedent(idPatient, idUtilisateur)
        Annuler(annule, idUtilisateur)

        CollectionAssert.AreEquivalent(New Long() {actif},
                                       Ids(dao.GetListByPatient(CInt(idPatient), "A", exclureInactifs:=True)))
    End Sub

    <TestMethod()> Public Sub GetListByPatient_TrieParOrdreDAffichageSurDemande()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim troisieme = CreerAntecedent(idPatient, idUtilisateur)
        Dim premier = CreerAntecedent(idPatient, idUtilisateur)
        Dim fils = CreerAntecedent(idPatient, idUtilisateur)
        Dim petitFils = CreerAntecedent(idPatient, idUtilisateur)
        PlacerAntecedent(troisieme, 1, 0, 0, 40, 0, 0)
        PlacerAntecedent(premier, 1, 0, 0, 20, 0, 0)
        PlacerAntecedent(fils, 2, premier, 0, 20, 20, 0)
        PlacerAntecedent(petitFils, 3, premier, fils, 20, 20, 20)

        Dim liste = dao.GetListByPatient(CInt(idPatient), "A", trierParOrdreAffichage:=True)

        CollectionAssert.AreEqual(New Long() {premier, fils, petitFils, troisieme}, Ids(liste))
    End Sub

    ' --- Synthèse : GetAllAntecedentbyPatient ------------------------------------

    ''' <summary>
    ''' Quatre antécédents actifs du patient dont un caché, un annulé, un contexte et
    ''' un antécédent d'un autre patient. Renvoie {majeur, fils, cache, dernier} ;
    ''' ordre d'affichage : majeur, fils, cache, dernier ; ordre de date de début :
    ''' dernier, cache, fils, majeur.
    ''' </summary>
    Private Function PreparerSynthese(idPatient As Long, idUtilisateur As Long, idDrcMajeur As Long) As Long()
        Dim majeur = CreerAntecedent(idPatient, idUtilisateur, idDrcMajeur, "Majeur", dateDebut:=New Date(2021, 1, 1))
        Dim fils = CreerAntecedent(idPatient, idUtilisateur, dateDebut:=New Date(2020, 1, 1))
        Dim cache = CreerAntecedent(idPatient, idUtilisateur, statutAffichage:="C", dateDebut:=New Date(2019, 1, 1))
        Dim dernier = CreerAntecedent(idPatient, idUtilisateur, dateDebut:=New Date(2018, 1, 1))
        PlacerAntecedent(majeur, 1, 0, 0, 20, 0, 0)
        PlacerAntecedent(fils, 2, majeur, 0, 20, 20, 0)
        PlacerAntecedent(cache, 1, 0, 0, 40, 0, 0)
        PlacerAntecedent(dernier, 1, 0, 0, 60, 0, 0)
        Dim annule = CreerAntecedent(idPatient, idUtilisateur)
        Annuler(annule, idUtilisateur)
        CreerContexteMedical(idPatient, idUtilisateur)
        CreerAntecedent(CreerPatient("AUTRE", "Patient"), idUtilisateur)
        Return {majeur, fils, cache, dernier}
    End Function

    <TestMethod()> Public Sub GetAllAntecedentbyPatient_PublieParPriorite_SuitLOrdreDAffichage()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc("Insuffisance cardiaque")
        Dim a = PreparerSynthese(idPatient, idUtilisateur, idDrc)

        Dim table = dao.GetAllAntecedentbyPatient(CInt(idPatient), True, True)

        CollectionAssert.AreEqual(New Long() {a(0), a(1), a(3)}, IdsTable(table))
        Dim ligne = table.Rows(0)
        Assert.AreEqual("Insuffisance cardiaque", CStr(ligne("oa_drc_libelle")))
        Assert.AreEqual("Majeur", CStr(ligne("oa_antecedent_description")))
        Assert.AreEqual(idDrc, CLng(ligne("oa_antecedent_drc_id")))
        Assert.AreEqual(1, CInt(ligne("oa_antecedent_niveau")))
        Assert.IsTrue(IsDBNull(ligne("oa_ald_cim10_description")), "sans ALD, la jointure ne ramène rien")
        Assert.AreEqual(a(0), CLng(table.Rows(1)("oa_antecedent_id_niveau1")))
    End Sub

    <TestMethod()> Public Sub GetAllAntecedentbyPatient_PublieParDate_SuitLaDateDeDebut()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim a = PreparerSynthese(idPatient, idUtilisateur, CreerDrc())

        CollectionAssert.AreEqual(New Long() {a(3), a(1), a(0)},
                                  IdsTable(dao.GetAllAntecedentbyPatient(CInt(idPatient), True, False)))
    End Sub

    <TestMethod()> Public Sub GetAllAntecedentbyPatient_NonPublie_AjouteLesCaches()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim a = PreparerSynthese(idPatient, idUtilisateur, CreerDrc())

        CollectionAssert.AreEqual(New Long() {a(0), a(1), a(2), a(3)},
                                  IdsTable(dao.GetAllAntecedentbyPatient(CInt(idPatient), False, True)))
        CollectionAssert.AreEqual(New Long() {a(3), a(2), a(1), a(0)},
                                  IdsTable(dao.GetAllAntecedentbyPatient(CInt(idPatient), False, False)))
    End Sub

    <TestMethod()> Public Sub GetAllAntecedentbyPatient_LuParLeServeur()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim a = PreparerSynthese(idPatient, idUtilisateur, CreerDrc())

        UtiliserCompte(Compte.Web)
        CollectionAssert.AreEqual(New Long() {a(0), a(1), a(3)},
                                  IdsTable(dao.GetAllAntecedentbyPatient(CInt(idPatient), True, True)))
    End Sub

    <TestMethod()> Public Sub GetAllAntecedentbyPatient_PatientSansAntecedent_TableVide()
        Assert.AreEqual(0, dao.GetAllAntecedentbyPatient(CInt(CreerPatient()), False, True).Rows.Count)
    End Sub

    ' --- Synthèse : GetContextebyPatient -----------------------------------------

    <TestMethod()> Public Sub GetContextebyPatient_Publie_ExclutCachesArretesInactifs()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc("Isolement social")
        Dim bio = CreerContexteMedical(idPatient, idUtilisateur, idDrc, "Vit seul", categorie:="B")
        Dim medical1 = CreerContexteMedical(idPatient, idUtilisateur, categorie:="M")
        Dim medical2 = CreerContexteMedical(idPatient, idUtilisateur, categorie:="M")
        CreerContexteMedical(idPatient, idUtilisateur, statutAffichage:="C")
        Dim arrete = CreerContexteMedical(idPatient, idUtilisateur)
        ArreterContexteMedical(arrete)
        Dim annule = CreerContexteMedical(idPatient, idUtilisateur)
        Annuler(annule, idUtilisateur)
        CreerAntecedent(idPatient, idUtilisateur)
        CreerContexteMedical(CreerPatient("AUTRE", "Patient"), idUtilisateur)

        Dim table = dao.GetContextebyPatient(CInt(idPatient), True)

        ' Catégorie décroissante (M avant B), puis le plus récent d'abord.
        CollectionAssert.AreEqual(New Long() {medical2, medical1, bio}, IdsTable(table))
        Dim ligne = table.Rows(2)
        Assert.AreEqual("Isolement social", CStr(ligne("oa_drc_libelle")))
        Assert.AreEqual("Vit seul", CStr(ligne("oa_antecedent_description")))
        Assert.AreEqual("B", CStr(ligne("oa_antecedent_categorie_contexte")))
        Assert.AreEqual(FinContexteDeTest, CDate(ligne("oa_antecedent_date_fin")).Date)
    End Sub

    <TestMethod()> Public Sub GetContextebyPatient_NonPublie_AjouteLesCaches()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim publie = CreerContexteMedical(idPatient, idUtilisateur)
        Dim cache = CreerContexteMedical(idPatient, idUtilisateur, statutAffichage:="C")

        CollectionAssert.AreEqual(New Long() {cache, publie}, IdsTable(dao.GetContextebyPatient(CInt(idPatient), False)))
    End Sub

    <TestMethod()> Public Sub GetContextebyPatient_LuParLeServeur()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim publie = CreerContexteMedical(idPatient, idUtilisateur)

        UtiliserCompte(Compte.Web)
        CollectionAssert.AreEqual(New Long() {publie}, IdsTable(dao.GetContextebyPatient(CInt(idPatient), True)))
    End Sub

    ' --- Courrier : GetListOfAntecedentPatient ------------------------------------

    <TestMethod()> Public Sub GetListOfAntecedentPatient_MetEnFormeIndentationEtDiagnostic()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim longue = New String("x"c, 150)
        Dim majeur = CreerAntecedent(idPatient, idUtilisateur, description:="Hypertension", diagnostic:=1)
        Dim fils = CreerAntecedent(idPatient, idUtilisateur, description:="Asthme", diagnostic:=2)
        Dim petitFils = CreerAntecedent(idPatient, idUtilisateur, description:="Ligne 1" & vbCrLf & "Ligne 2", diagnostic:=3)
        Dim cache = CreerAntecedent(idPatient, idUtilisateur, description:=longue, statutAffichage:="C")
        PlacerAntecedent(majeur, 1, 0, 0, 20, 0, 0)
        PlacerAntecedent(fils, 2, majeur, 0, 20, 20, 0)
        PlacerAntecedent(petitFils, 3, majeur, fils, 20, 20, 20)
        PlacerAntecedent(cache, 1, 0, 0, 40, 0, 0)
        Dim annule = CreerAntecedent(idPatient, idUtilisateur)
        Annuler(annule, idUtilisateur)

        Dim liste = dao.GetListOfAntecedentPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {majeur, fils, petitFils, cache}, liste.Select(Function(c) c.Id).ToArray())
        Assert.IsTrue(liste.All(Function(c) c.PatientId = idPatient))
        Assert.AreEqual(" Hypertension", liste(0).Description)
        Assert.AreEqual(New String(" "c, 11) & "> Suspicion de :  Asthme", liste(1).Description)
        Assert.AreEqual(New String(" "c, 24) & ">> Notion de :  Ligne 1 Ligne 2", liste(2).Description)
        Assert.AreEqual(" " & New String("x"c, 100), liste(3).Description, "description tronquée à 100 caractères")
    End Sub

    <TestMethod()> Public Sub GetListOfAntecedentPatient_PatientSansAntecedent_ListeVide()
        Assert.AreEqual(0, dao.GetListOfAntecedentPatient(CInt(CreerPatient())).Count)
    End Sub

    ' --- Modification et annulation ------------------------------------------------

    <TestMethod()> Public Sub ModificationAntecedent_EnregistreLesChampsEtLHistorique()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAntecedent = CreerAntecedent(idPatient, idCreateur)
        Dim idNouvelleDrc = CreerDrc()
        Dim lu = dao.GetAntecedentById(CInt(idAntecedent))
        Dim modifie = dao.Clone(lu)
        modifie.DrcId = CInt(idNouvelleDrc)
        modifie.Description = "Description revue"
        modifie.DateDebut = New Date(2021, 6, 1)
        modifie.Diagnostic = 3
        modifie.StatutAffichage = "C"
        modifie.ChaineEpisodeDateFin = New Date(2030, 1, 1)
        ReculerDateSynthese(idPatient)

        Assert.IsTrue(dao.ModificationAntecedent(modifie, lu, Auteur(idModificateur)))

        Dim relu = dao.GetAntecedentById(CInt(idAntecedent))
        Assert.AreEqual(CInt(idNouvelleDrc), relu.DrcId)
        Assert.AreEqual("Description revue", relu.Description)
        Assert.AreEqual(New Date(2021, 6, 1), relu.DateDebut)
        Assert.AreEqual(3, relu.Diagnostic)
        Assert.AreEqual("C", relu.StatutAffichage)
        Assert.AreEqual(New Date(2030, 1, 1), relu.ChaineEpisodeDateFin.Date)
        Assert.AreEqual(CInt(idModificateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(CInt(idCreateur), relu.UserCreation, "le créateur ne change pas")
        Assert.AreEqual(980, relu.Ordre1, "la position ne change pas")
        Assert.AreEqual(Date.Today, DateSynthese(idPatient).Date)

        Dim histo = HistoriqueAntecedent(idAntecedent)
        Assert.AreEqual(2, histo.Rows.Count)
        Dim ligne = histo.Rows(1)
        Assert.AreEqual(2, CInt(ligne("oa_antecedent_histo_etat_historisation")), "ModificationAntecedent")
        Assert.AreEqual(idModificateur, CLng(ligne("oa_antecedent_histo_utilisateur_historisation")))
        Assert.AreEqual(idNouvelleDrc, CLng(ligne("oa_antecedent_drc_id")))
        Assert.AreEqual("Description revue", CStr(ligne("oa_antecedent_description")))
        Assert.AreEqual(New Date(2021, 6, 1), CDate(ligne("oa_antecedent_date_debut")))
        Assert.AreEqual("C", CStr(ligne("oa_antecedent_statut_affichage")))
        Assert.AreEqual(3, CInt(ligne("oa_antecedent_diagnostic")))
        Assert.AreEqual(980, CInt(ligne("oa_antecedent_ordre_affichage1")))
        Assert.AreEqual("Patient", CStr(ligne("oa_antecedent_nature")))
        Assert.IsFalse(CBool(ligne("oa_antecedent_inactif")))
        ' Comportement actuel : fin de chaîne de l'historique = maintenant +
        ' ChaineEpisodePeriode (6 mois dans app.config), et non la valeur saisie (2030).
        Assert.AreEqual(Date.Today.AddMonths(6), CDate(ligne("oa_chaine_episode_date_fin")).Date)
    End Sub

    <TestMethod()> Public Sub ModificationAntecedent_SansAld_RemetLesChampsAldAZero()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idAntecedent = CreerAntecedent(CreerPatient(), idUtilisateur)
        Dim lu = dao.GetAntecedentById(CInt(idAntecedent))
        Dim modifie = dao.Clone(lu)
        modifie.AldId = 0
        modifie.AldValide = True
        modifie.AldCim10Id = 7
        modifie.AldDemandeEnCours = True
        modifie.AldDateDebut = New Date(2020, 1, 1)
        modifie.AldDateDemande = New Date(2020, 1, 1)

        dao.ModificationAntecedent(modifie, lu, Auteur(idUtilisateur))

        Dim relu = dao.GetAntecedentById(CInt(idAntecedent))
        Assert.AreEqual(0, relu.AldCim10Id)
        Assert.IsFalse(relu.AldValide)
        Assert.IsFalse(relu.AldDemandeEnCours)
        Assert.AreEqual(Date.MaxValue.Date, relu.AldDateDebut.Date)
        Assert.AreEqual(Date.MaxValue.Date, relu.AldDateDemande.Date)
    End Sub

    <TestMethod()> Public Sub AnnulationAntecedent_DesactiveSansSupprimer()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idAnnulateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAntecedent = CreerAntecedent(idPatient, idCreateur)
        Dim lu = dao.GetAntecedentById(CInt(idAntecedent))
        ReculerDateSynthese(idPatient)

        Assert.IsTrue(dao.AnnulationAntecedent(lu, lu, Auteur(idAnnulateur)))

        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_antecedent WHERE oa_antecedent_id = @p0", idAntecedent)),
                        "un antécédent annulé reste en base")
        Dim relu = dao.GetAntecedentById(CInt(idAntecedent))
        Assert.IsTrue(relu.Inactif)
        Assert.AreEqual(CInt(idAnnulateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual("P", relu.StatutAffichage)
        Assert.AreEqual(Date.Today, DateSynthese(idPatient).Date)
        Assert.AreEqual(0, dao.GetAllAntecedentbyPatient(CInt(idPatient), False, True).Rows.Count)

        Dim histo = HistoriqueAntecedent(idAntecedent)
        Assert.AreEqual(2, histo.Rows.Count)
        Dim ligne = histo.Rows(1)
        Assert.AreEqual(4, CInt(ligne("oa_antecedent_histo_etat_historisation")), "AnnulationAntecedent")
        Assert.AreEqual(idAnnulateur, CLng(ligne("oa_antecedent_histo_utilisateur_historisation")))
        Assert.IsTrue(CBool(ligne("oa_antecedent_inactif")))
    End Sub

    ' --- Clone et Compare ----------------------------------------------------------

    <TestMethod()> Public Sub Clone_CopieToutesLesValeursEtCompareLesReconnait()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim lu = dao.GetAntecedentById(CInt(CreerAntecedent(CreerPatient(), idUtilisateur)))

        Dim copie = dao.Clone(lu)

        Assert.AreNotSame(lu, copie)
        Assert.IsTrue(dao.Compare(copie, lu))
        copie.DateCreation = lu.DateCreation.Date.AddHours(23)
        Assert.IsTrue(dao.Compare(copie, lu), "seul le jour des dates compte")
        copie.Description = "Autre"
        Assert.IsFalse(dao.Compare(copie, lu))
    End Sub

End Class
