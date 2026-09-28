Imports Oasis_Common

''' <summary>
''' TacheDao contre la base : lectures, listes des écrans de tâches et leurs filtres,
''' compteur de rafraîchissement, recherches par patient, parcours et épisode.
''' Le cycle de vie (création, attribution, clôture, annulation, relance, rendez-vous)
''' est dans TacheDaoWorkflowTest.
'''
''' Seul le client lourd appelle TacheDao (Oasis_Web instancie le DAO sans s'en
''' servir, ParcoursDao l'appelle pour le client) : tout tourne sous Compte.Client.
''' Les requêtes qui nomment la base ([oasis].[oasis].oa_tache) sont gardées par
''' ExigerBaseOasis. Les règles d'état du bean et FiltreTache sont couvertes par
''' UnitTest ; ici on éprouve le SQL, y compris les clauses IN que construisent
''' Fonction, UniteSanitaire et Site.
''' </summary>
<TestClass()> Public Class TacheDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New TacheDao

    Private Const TacheAbsente As Integer = 987654321

    Private Function Relire(idTache As Long) As Tache
        Return dao.GetTacheById(CInt(idTache), True)
    End Function

    Private Shared Function CompteDe(idUtilisateur As Long, Optional profilId As String = "IDE") As Utilisateur
        Return UtilisateurPourTache(idUtilisateur, profilId)
    End Function

    ''' <summary>Tâche en attente pour cette fonction, enregistrée par CreateTache.</summary>
    Private Shared Function Demande(idPatient As Long, idEmetteur As Long, idFonction As Long,
                                    Optional priorite As Integer = Tache.EnumPriorite.BASSE,
                                    Optional ordreAffichage As Integer = 10,
                                    Optional horodatage As Date? = Nothing,
                                    Optional uniteSanitaireId As Long = 0,
                                    Optional siteId As Long = 0,
                                    Optional typeDeTache As Tache.TypeTache = Tache.TypeTache.AVIS_EPISODE,
                                    Optional dateRendezVous As Date? = Nothing) As Long
        Return EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=typeDeTache,
                                            traiteFonctionId:=idFonction, destinataireFonctionId:=idFonction,
                                            priorite:=priorite, ordreAffichage:=ordreAffichage,
                                            horodatage:=horodatage, uniteSanitaireId:=uniteSanitaireId,
                                            siteId:=siteId, dateRendezVous:=dateRendezVous))
    End Function

    ' ---------------------------------------------------------------------
    ' Libellés de nature et répartition des fonctions (sans SQL)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub CodeNature_ChaqueLibelleDonneSonCode()
        Dim paires = New Dictionary(Of String, String) From {
            {Tache.EnumNatureTacheItem.DEMANDE, "DEMANDE"},
            {Tache.EnumNatureTacheItem.COMPLEMENT, "COMPLEMENT"},
            {Tache.EnumNatureTacheItem.MISSION_DEMANDE, "MISSION_DEMANDE"},
            {Tache.EnumNatureTacheItem.RDV, "RDV"},
            {Tache.EnumNatureTacheItem.RDV_DEMANDE, "RDV_DEMANDE"},
            {Tache.EnumNatureTacheItem.RDV_SPECIALISTE, "RDV_SPECIALISTE"},
            {Tache.EnumNatureTacheItem.REPONSE, "REPONSE"},
            {Tache.EnumNatureTacheItem.REUNION_STAFF, "REUNION_STAFF"}}

        For Each paire In paires
            Assert.AreEqual(paire.Value, dao.GetCodeNatureTacheByItem(paire.Key), paire.Key)
            Assert.AreEqual(paire.Key, dao.GetItemNatureTacheByCode(paire.Value), paire.Value)
        Next
    End Sub

    <TestMethod()> Public Sub CodeNature_ValeurInconnue_DonneInconnue()
        Assert.AreEqual("Inconnue", dao.GetCodeNatureTacheByItem("Libelle absent"))
        Assert.AreEqual("Inconnue", dao.GetItemNatureTacheByCode("CODE_ABSENT"))
        Assert.AreEqual("Inconnue", dao.GetItemNatureTacheByCode("demande"), "la casse compte")
    End Sub

    <TestMethod()> Public Sub FonctionsParSousCategorie_EmetteurSelonLeProfil()
        Dim attendus = New Dictionary(Of String, Long) From {
            {"IDE", FonctionDao.EnumFonction.IDE},
            {"IDE_REMPLACANT", FonctionDao.EnumFonction.IDE_REMPLACANT},
            {"MEDECIN", FonctionDao.EnumFonction.MEDECIN},
            {" SAGE_FEMME ", FonctionDao.EnumFonction.SAGE_FEMME},
            {"CADRE_SANTE", FonctionDao.EnumFonction.CADRE_SANTE},
            {"SECRETAIRE_MEDICALE", FonctionDao.EnumFonction.SECRETAIRE_MEDICALE},
            {"ADMINISTRATIF", FonctionDao.EnumFonction.ADMINISTRATIF},
            {"IT_MEDECIN", FonctionDao.EnumFonction.INCONNU}}

        For Each paire In attendus
            Dim repartition = dao.SetTacheEmetteurEtDestinatiareBySpecialiteEtSousCategorie(0, EnumSousCategoriePPS.IDE, CompteDe(1, paire.Key))
            Assert.AreEqual(paire.Value, repartition.EmetteurFonctionId, paire.Key)
        Next
    End Sub

    <TestMethod()> Public Sub FonctionsParSousCategorie_DestinataireEtTraitantSelonLeParcours()
        Dim auteur = CompteDe(1, "IDE")
        Dim cas = New List(Of Long()) From {
            New Long() {EnumSousCategoriePPS.medecinReferent, 0, FonctionDao.EnumFonction.MEDECIN, FonctionDao.EnumFonction.MEDECIN},
            New Long() {EnumSousCategoriePPS.IDE, 0, FonctionDao.EnumFonction.IDE, FonctionDao.EnumFonction.IDE},
            New Long() {EnumSousCategoriePPS.sageFemme, EnumSpecialiteOasis.sageFemmeOasis, FonctionDao.EnumFonction.SAGE_FEMME, FonctionDao.EnumFonction.SAGE_FEMME},
            New Long() {EnumSousCategoriePPS.sageFemme, SpecialiteTacheNonOasis, FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS, FonctionDao.EnumFonction.IDE},
            New Long() {EnumSousCategoriePPS.specialiste, SpecialiteTacheNonOasis, FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS, FonctionDao.EnumFonction.IDE},
            New Long() {99, 0, FonctionDao.EnumFonction.INCONNU, FonctionDao.EnumFonction.IDE}}

        For Each ligne In cas
            Dim repartition = dao.SetTacheEmetteurEtDestinatiareBySpecialiteEtSousCategorie(ligne(1), ligne(0), auteur)
            Assert.AreEqual(ligne(2), repartition.DestinataireFonctionId, "destinataire, sous-catégorie " & ligne(0))
            Assert.AreEqual(ligne(3), repartition.TraiteFonctionId, "traitant, sous-catégorie " & ligne(0))
        Next
    End Sub

    ' ---------------------------------------------------------------------
    ' GetTacheById et lecture des colonnes
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetTacheById_RelitChaqueColonneEcrite()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient("TACHE", "Lecture")
        Dim idEpisode = CreerEpisode(idPatient, idEmetteur)
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)
        Dim idUnite = CreerUniteSanitaire("Unite lecture")
        Dim idSite = CreerSite("Site lecture", idUnite)
        Dim fEmettrice = CreerFonction("IT emettrice")
        Dim fTraitante = CreerFonction("IT traitante")
        Dim fDestinataire = CreerFonction("IT destinataire")
        Dim creation As New Date(2026, 3, 2, 9, 15, 0)
        Dim rendezVous As New Date(2026, 4, 10, 14, 30, 0)
        Dim nouvelle = TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV,
                                   traiteFonctionId:=fTraitante, emetteurFonctionId:=fEmettrice,
                                   destinataireFonctionId:=fDestinataire, traiteUserId:=idTraitant,
                                   priorite:=Tache.EnumPriorite.MOYENNE, ordreAffichage:=20,
                                   episodeId:=idEpisode, parcoursId:=idParcours,
                                   uniteSanitaireId:=idUnite, siteId:=idSite,
                                   dateRendezVous:=rendezVous, horodatage:=creation,
                                   commentaire:="A planifier", duree:=30,
                                   typeDemandeRendezVous:=Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString())
        nouvelle.DateTraitementDemandeRendezVous = New Date(2026, 3, 20)

        Dim idTache = EnregistrerTache(nouvelle)

        Dim relue = dao.GetTacheById(CInt(idTache))
        Assert.AreEqual(idTache, relue.Id)
        Assert.AreEqual(0L, relue.ParentId)
        Assert.AreEqual(idEmetteur, relue.EmetteurUserId)
        Assert.AreEqual(fEmettrice, relue.EmetteurFonctionId)
        Assert.AreEqual(idUnite, relue.UniteSanitaireId)
        Assert.AreEqual(idSite, relue.SiteId)
        Assert.AreEqual(idPatient, relue.PatientId)
        Assert.AreEqual(idParcours, relue.ParcoursId)
        Assert.AreEqual(idEpisode, relue.EpisodeId)
        Assert.AreEqual(0L, relue.SousEpisodeId)
        Assert.AreEqual(idTraitant, relue.TraiteUserId)
        Assert.AreEqual(fTraitante, relue.TraiteFonctionId)
        Assert.AreEqual(fDestinataire, relue.DestinataireFonctionId)
        Assert.AreEqual(CInt(Tache.EnumPriorite.MOYENNE), relue.Priorite)
        Assert.AreEqual(20, relue.OrdreAffichage)
        Assert.AreEqual("SOIN", relue.Categorie)
        Assert.AreEqual("RDV", relue.Type)
        Assert.AreEqual("RDV", relue.Nature)
        Assert.AreEqual(30, relue.Duree)
        Assert.AreEqual("A planifier", relue.EmetteurCommentaire)
        Assert.AreEqual(creation, relue.HorodatageCreation)
        Assert.AreEqual(Date.MinValue, relue.HorodatageAttribution)
        Assert.AreEqual(Date.MinValue, relue.HorodatageCloture)
        Assert.AreEqual("EN_ATTENTE", relue.Etat)
        Assert.IsFalse(relue.Cloture)
        Assert.AreEqual("ANNEEMOIS", relue.TypedemandeRendezVous)
        Assert.AreEqual(rendezVous, relue.DateRendezVous)
        Assert.AreEqual(New Date(2026, 3, 20), relue.DateTraitementDemandeRendezVous)
    End Sub

    <TestMethod()> Public Sub GetTacheById_ValeursAZero_SontEcritesNullEtRelues()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim nouvelle = TacheDeTest(idPatient, idEmetteur, traiteFonctionId:=CreerFonction("IT traitante"))

        Dim idTache = EnregistrerTache(nouvelle)

        ' CreateTache écrit NULL pour les identifiants à 0, la durée à 0, le type de
        ' demande vide et les dates non renseignées.
        For Each colonne In {"parent_id", "unite_sanitaire_id", "site_id", "parcours_id", "episode_id", "sous_episode_id",
                             "traite_user_id", "destinataire_fonction_id", "duree_mn", "type_demande_rendez_vous",
                             "date_rendez_vous", "date_traitement_demande_rendez_vous", "horodate_attrib", "horodate_cloture"}
            Assert.AreEqual(DBNull.Value, ColonneTache(idTache, colonne), colonne)
        Next
        ' Pas l'émetteur : sa fonction à 0 est écrite telle quelle.
        Assert.AreEqual(0L, CLng(ColonneTache(idTache, "emetteur_fonction_id")))

        Dim relue = Relire(idTache)
        Assert.AreEqual(0L, relue.ParentId)
        Assert.AreEqual(0L, relue.UniteSanitaireId)
        Assert.AreEqual(0L, relue.SiteId)
        Assert.AreEqual(0L, relue.TraiteUserId)
        Assert.AreEqual(0, relue.Duree)
        Assert.AreEqual("", relue.TypedemandeRendezVous)
        Assert.AreEqual(Date.MinValue, relue.DateRendezVous)
        Assert.AreEqual(Date.MinValue, relue.DateTraitementDemandeRendezVous)
        Assert.AreEqual("DEMANDE", relue.Nature)
        Assert.AreEqual("AVIS_EPISODE", relue.Type)
    End Sub

    <TestMethod()> Public Sub GetTacheById_TacheAnnulee_NestRelueQueSurDemande()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTache = Demande(CreerPatient(), idEmetteur, CreerFonction("IT traitante"))
        dao.AnnulationTache(idTache, CompteDe(idEmetteur))

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetTacheById(CInt(idTache)))
        Assert.AreEqual("ANNULEE", dao.GetTacheById(CInt(idTache), True).Etat)
    End Sub

    <TestMethod()> Public Sub GetTacheById_TacheTerminee_EstRelueSansDemande()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTache = Demande(CreerPatient(), idEmetteur, CreerFonction("IT traitante"))
        dao.ClotureTache(idTache, True, CompteDe(idEmetteur))

        Assert.AreEqual("TERMINEE", dao.GetTacheById(CInt(idTache)).Etat)
    End Sub

    <TestMethod()> Public Sub GetTacheById_Inexistante_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetTacheById(TacheAbsente, True))
        StringAssert.Contains(erreur.Message, "non retrouvée")
    End Sub

    ' ---------------------------------------------------------------------
    ' GetAllTacheATraiter et GetAllTacheATraiterChk
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ATraiter_SansFonction_AucuneTacheEtCompteurNul()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Demande(CreerPatient(), idEmetteur, CreerFonction("IT traitante"))

        Assert.AreEqual(0, dao.GetAllTacheATraiter(New List(Of Fonction), FiltreTacheDe()).Rows.Count)
        Assert.AreEqual(0, dao.GetAllTacheATraiterChk(New List(Of Fonction), FiltreTacheDe()))
    End Sub

    <TestMethod()> Public Sub ATraiter_ListeDeFonctionsAbsente_LeveNullReference()
        ' Comportement actuel : contrairement à GetAllTacheEnCours, pas de test de
        ' Nothing sur la liste ; l'écran passe toujours une liste.
        Assert.ThrowsException(Of NullReferenceException)(Sub() dao.GetAllTacheATraiter(Nothing, FiltreTacheDe()))
    End Sub

    <TestMethod()> Public Sub ATraiter_SeulesLesTachesEnAttenteDesFonctionsChoisies()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim f2 = CreerFonction("IT f2")
        Dim f3 = CreerFonction("IT f3")
        Dim horodate As New Date(2026, 1, 1, 8, 0, 0)
        Dim pourF1 = Demande(idPatient, idEmetteur, f1, horodatage:=horodate)
        Dim pourF2 = Demande(idPatient, idEmetteur, f2, horodatage:=horodate.AddHours(1))
        Demande(idPatient, idEmetteur, f3)
        Dim prise = Demande(idPatient, idEmetteur, f1)
        dao.AttribueTacheToUserLog(prise, CompteDe(idTraitant))
        Dim terminee = Demande(idPatient, idEmetteur, f1)
        dao.ClotureTache(terminee, True, CompteDe(idEmetteur))
        Dim annulee = Demande(idPatient, idEmetteur, f1)
        dao.AnnulationTache(annulee, CompteDe(idEmetteur))
        ' Les rendez-vous hors Oasis ne passent jamais par cette liste.
        Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_SPECIALISTE, dateRendezVous:=Date.Today)

        CollectionAssert.AreEqual({pourF1}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe())))
        ' Clause IN à deux identifiants.
        CollectionAssert.AreEqual({pourF1, pourF2}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1, f2), FiltreTacheDe())))
        CollectionAssert.AreEqual({pourF2}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f2), FiltreTacheDe())))
    End Sub

    <TestMethod()> Public Sub ATraiter_RendezVousDesJoursSuivantsExclus()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim aujourdhui = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=Date.Today.AddHours(23))
        Dim passe = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=Date.Today.AddDays(-3))
        Dim demain = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=Date.Today.AddDays(1))
        Dim missionDemain = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_MISSION, dateRendezVous:=Date.Today.AddDays(1))
        Dim missionAujourdhui = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_MISSION, dateRendezVous:=Date.Today)
        ' Seuls RDV et RDV_MISSION attendent leur jour : les autres types restent visibles.
        Dim staffDemain = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.REUNION_STAFF, dateRendezVous:=Date.Today.AddDays(1))
        Dim demandeFuture = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, dateRendezVous:=Date.Today.AddMonths(3))

        Dim ids = IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe()))

        CollectionAssert.AreEquivalent({aujourdhui, passe, missionAujourdhui, staffDemain, demandeFuture}, ids)
        CollectionAssert.DoesNotContain(ids, demain)
        CollectionAssert.DoesNotContain(ids, missionDemain)
    End Sub

    <TestMethod()> Public Sub ATraiter_OrdreParPrioriteOrdreAffichagePuisDate()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim a = Demande(idPatient, idEmetteur, f1, priorite:=300, ordreAffichage:=10, horodatage:=New Date(2026, 1, 1))
        Dim b = Demande(idPatient, idEmetteur, f1, priorite:=100, ordreAffichage:=20, horodatage:=New Date(2026, 1, 3))
        Dim c = Demande(idPatient, idEmetteur, f1, priorite:=100, ordreAffichage:=10, horodatage:=New Date(2026, 1, 5))
        Dim d = Demande(idPatient, idEmetteur, f1, priorite:=100, ordreAffichage:=10, horodatage:=New Date(2026, 1, 4))
        ' Créé après d mais daté par son rendez-vous : COALESCE(date_rendez_vous, horodate_creation).
        Dim e = Demande(idPatient, idEmetteur, f1, priorite:=100, ordreAffichage:=10, horodatage:=New Date(2026, 1, 10),
                        typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2026, 1, 2))

        CollectionAssert.AreEqual({e, d, c, b, a}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe())))
    End Sub

    <TestMethod()> Public Sub ATraiter_ColonnesJointesDuSiteDuPatientEtDeLEmetteur()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient("DUPONT", "Marie")
        Dim idUnite = CreerUniteSanitaire("Unite A")
        Dim idSite = CreerSite("Site des tests", idUnite)
        Dim f1 = CreerFonction("IT f1")
        Dim fEmettrice = CreerFonction("IT emettrice", designation:="Infirmier emetteur")
        Dim complete = EnregistrerTache(TacheDeTest(idPatient, idEmetteur, traiteFonctionId:=f1, emetteurFonctionId:=fEmettrice,
                                                    uniteSanitaireId:=idUnite, siteId:=idSite, commentaire:="Voir le patient",
                                                    priorite:=Tache.EnumPriorite.HAUTE, horodatage:=New Date(2026, 2, 1, 10, 0, 0)))
        Dim nue = Demande(idPatient, idEmetteur, f1)

        Dim table = dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe())

        Dim ligne = LigneTache(table, complete)
        Assert.AreEqual("EN_ATTENTE", CStr(ligne("etat")))
        Assert.AreEqual("Site des tests", CStr(ligne("site_description")))
        Assert.AreEqual("DUPONT", CStr(ligne("patient_nom")))
        Assert.AreEqual("Marie", CStr(ligne("patient_prenom")))
        Assert.AreEqual("AVIS_EPISODE", CStr(ligne("type")))
        Assert.AreEqual("DEMANDE", CStr(ligne("nature")))
        Assert.AreEqual(fEmettrice, CLng(ligne("emetteur_fonction_id")))
        Assert.AreEqual("Infirmier emetteur", CStr(ligne("emetteur_fonction")))
        Assert.AreEqual("Voir le patient", CStr(ligne("emetteur_commentaire")))
        Assert.AreEqual(New Date(2026, 2, 1, 10, 0, 0), CDate(ligne("horodate_creation")))
        Assert.AreEqual(CInt(Tache.EnumPriorite.HAUTE), CInt(ligne("priorite")))
        Assert.AreEqual(10, CInt(ligne("ordre_affichage")))
        Assert.AreEqual(DBNull.Value, ligne("date_rendez_vous"))

        Dim ligneNue = LigneTache(table, nue)
        Assert.AreEqual("", CStr(ligneNue("emetteur_fonction")), "fonction émettrice absente : chaîne vide")
        Assert.AreEqual(DBNull.Value, ligneNue("site_description"))
    End Sub

    <TestMethod()> Public Sub ATraiter_FiltreParUniteSanitaire()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim u1 = CreerUniteSanitaire("Unite 1")
        Dim u2 = CreerUniteSanitaire("Unite 2")
        Dim s1 = CreerSite("Site 1", u1)
        Dim s2 = CreerSite("Site 2", u2)
        Dim dansU1 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s1, horodatage:=New Date(2026, 1, 1))
        Dim dansU2 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u2, siteId:=s2, horodatage:=New Date(2026, 1, 2))
        Dim sansUnite = Demande(idPatient, idEmetteur, f1, horodatage:=New Date(2026, 1, 3))

        ' Unité sans site retenu : tous ses sites.
        CollectionAssert.AreEqual({dansU1}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1)))))
        CollectionAssert.AreEqual({dansU1, dansU2},
            IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1), UnitePourFiltreTache(u2)))))
        ' Filtre vide : les tâches sans unité restent visibles.
        CollectionAssert.AreEqual({dansU1, dansU2, sansUnite}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe())))
    End Sub

    <TestMethod()> Public Sub ATraiter_FiltreParSite()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim u1 = CreerUniteSanitaire("Unite 1")
        Dim s1 = CreerSite("Site 1", u1)
        Dim s2 = CreerSite("Site 2", u1)
        Dim s3 = CreerSite("Site 3", u1)
        Dim surS1 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s1, horodatage:=New Date(2026, 1, 1))
        Dim surS2 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s2, horodatage:=New Date(2026, 1, 2))
        Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s3, horodatage:=New Date(2026, 1, 3))

        CollectionAssert.AreEqual({surS1}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1, s1)))))
        CollectionAssert.AreEqual({surS1, surS2},
            IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1, s1, s2)))))
    End Sub

    <TestMethod()> Public Sub ATraiter_SitesRetenusParUnite_UniteSansSiteGardeToutesSesTaches()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim u1 = CreerUniteSanitaire("Unite 1")
        Dim u2 = CreerUniteSanitaire("Unite 2")
        Dim u3 = CreerUniteSanitaire("Unite 3")
        Dim s1 = CreerSite("Site 1", u1)
        Dim s1bis = CreerSite("Site 1 bis", u1)
        Dim s2 = CreerSite("Site 2", u2)
        Dim s3 = CreerSite("Site 3", u3)
        Dim surS1 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s1, horodatage:=New Date(2026, 1, 1))
        Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s1bis, horodatage:=New Date(2026, 1, 2))
        Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, horodatage:=New Date(2026, 1, 3))
        Dim surS2 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u2, siteId:=s2, horodatage:=New Date(2026, 1, 4))
        Dim u2SansSite = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u2, horodatage:=New Date(2026, 1, 5))
        Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u3, siteId:=s3, horodatage:=New Date(2026, 1, 6))

        ' Unité 1 : seulement le site retenu (pas le site bis, pas la tâche sans site).
        ' Unité 2, cochée sans site : tous ses sites, et ses tâches sans site comme
        ' lorsqu'elle est seule dans le filtre. Unité 3, non cochée : rien.
        Dim filtre = FiltreTacheDe(UnitePourFiltreTache(u1, s1), UnitePourFiltreTache(u2))
        CollectionAssert.AreEqual({surS1, surS2, u2SansSite}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), filtre)))
        Assert.AreEqual(Checksum(surS1, surS2, u2SansSite), dao.GetAllTacheATraiterChk(FonctionsTache(f1), filtre))
    End Sub

    <TestMethod()> Public Sub ATraiter_ChaqueUniteAvecSonSiteRetenu()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim u1 = CreerUniteSanitaire("Unite 1")
        Dim u2 = CreerUniteSanitaire("Unite 2")
        Dim s1 = CreerSite("Site 1", u1)
        Dim s2 = CreerSite("Site 2", u2)
        Dim surS1 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s1, horodatage:=New Date(2026, 1, 1))
        Dim surS2 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u2, siteId:=s2, horodatage:=New Date(2026, 1, 2))

        Dim filtre = FiltreTacheDe(UnitePourFiltreTache(u1, s1), UnitePourFiltreTache(u2, s2))
        CollectionAssert.AreEqual({surS1, surS2}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), filtre)))
        Assert.AreEqual(Checksum(surS1, surS2), dao.GetAllTacheATraiterChk(FonctionsTache(f1), filtre))
    End Sub

    <TestMethod()> Public Sub ATraiterChk_EgalAuChecksumDesTachesListees()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim f2 = CreerFonction("IT f2")
        Dim u1 = CreerUniteSanitaire("Unite 1")
        Dim a = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1)
        Dim b = Demande(idPatient, idEmetteur, f1)
        Demande(idPatient, idEmetteur, f2)
        Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=Date.Today.AddDays(2))

        Dim avant = dao.GetAllTacheATraiterChk(FonctionsTache(f1), FiltreTacheDe())

        Assert.AreEqual(Checksum(a, b), avant)
        ' Mêmes filtres que la liste : unité sanitaire.
        Assert.AreEqual(Checksum(a), dao.GetAllTacheATraiterChk(FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1))))

        ' Une tâche prise sort de la liste : le compteur change, et l'écran se rafraîchit.
        dao.AttribueTacheToUserLog(a, CompteDe(idTraitant))
        Dim apres = dao.GetAllTacheATraiterChk(FonctionsTache(f1), FiltreTacheDe())
        Assert.AreEqual(Checksum(b), apres)
        Assert.AreNotEqual(avant, apres)
    End Sub

    <TestMethod()> Public Sub ATraiterChk_AucuneTache_DonneZero()
        Assert.AreEqual(0, dao.GetAllTacheATraiterChk(FonctionsTache(CreerFonction("IT vide")), FiltreTacheDe()))
    End Sub

    Private Shared Function Checksum(ParamArray ids() As Long) As Integer
        Return CInt(Scalaire("SELECT CHECKSUM_AGG(CAST(id AS INT)) FROM oasis.oa_tache WHERE id IN (" &
                             String.Join(",", ids) & ")"))
    End Function

    ' ---------------------------------------------------------------------
    ' GetAllTacheEnCours
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub EnCours_MesTaches_SeulesCellesQueJeTraite()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idMoi = CreerUtilisateur(avecCle:=False)
        Dim idAutre = CreerUtilisateur(avecCle:=False)
        RenommerUtilisateurPourTache(idMoi, "MARTIN", "Paul")
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1", designation:="Medecin traitant")
        Dim moi = CompteDe(idMoi)
        Dim mienne = Demande(idPatient, idEmetteur, f1)
        dao.AttribueTacheToUserLog(mienne, moi)
        Dim autre = Demande(idPatient, idEmetteur, f1)
        dao.AttribueTacheToUserLog(autre, CompteDe(idAutre))
        Demande(idPatient, idEmetteur, f1)
        Dim terminee = Demande(idPatient, idEmetteur, f1)
        dao.AttribueTacheToUserLog(terminee, moi)
        dao.ClotureTache(terminee, True, moi)
        Dim specialiste = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_SPECIALISTE, dateRendezVous:=Date.Today)
        dao.AttribueTacheToUserLog(specialiste, moi)

        ' isWithNonAttribue ne joue pas pour mes tâches ; fonctions et filtre ignorés.
        Dim table = dao.GetAllTacheEnCours(True, Nothing, Nothing, True, moi)

        CollectionAssert.AreEqual({mienne}, IdsTaches(table))
        Dim ligne = table.Rows(0)
        Assert.AreEqual("EN_COURS", CStr(ligne("etat")))
        Assert.AreEqual("MARTIN", CStr(ligne("user_traiteur_nom")))
        Assert.AreEqual("Paul", CStr(ligne("user_traiteur_prenom")))
        Assert.AreEqual("Medecin traitant", CStr(ligne("traite_fonction")))
        Assert.AreEqual("", CStr(ligne("emetteur_fonction")))
    End Sub

    <TestMethod()> Public Sub EnCours_ParFonction_AvecOuSansLesNonAttribuees()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim f2 = CreerFonction("IT f2")
        Dim f3 = CreerFonction("IT f3")
        Dim prise = Demande(idPatient, idEmetteur, f1, horodatage:=New Date(2026, 1, 1))
        dao.AttribueTacheToUserLog(prise, CompteDe(idTraitant))
        Dim libre = Demande(idPatient, idEmetteur, f1, horodatage:=New Date(2026, 1, 2))
        Dim priseF2 = Demande(idPatient, idEmetteur, f2, horodatage:=New Date(2026, 1, 3))
        dao.AttribueTacheToUserLog(priseF2, CompteDe(idTraitant))
        Dim priseF3 = Demande(idPatient, idEmetteur, f3)
        dao.AttribueTacheToUserLog(priseF3, CompteDe(idTraitant))
        Dim moi = CompteDe(CreerUtilisateur(avecCle:=False))

        CollectionAssert.AreEqual({prise}, IdsTaches(dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(), False, moi)))
        CollectionAssert.AreEqual({prise, libre}, IdsTaches(dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(), True, moi)))
        CollectionAssert.AreEqual({prise, libre, priseF2},
            IdsTaches(dao.GetAllTacheEnCours(False, FonctionsTache(f1, f2), FiltreTacheDe(), True, moi)))

        Dim ligneLibre = LigneTache(dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(), True, moi), libre)
        Assert.AreEqual("", CStr(ligneLibre("user_traiteur_nom")), "pas encore de traitant")
        Assert.AreEqual("", CStr(ligneLibre("user_traiteur_prenom")))
    End Sub

    <TestMethod()> Public Sub EnCours_ParFonction_SansFonctionOuSansFiltre_Rien()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim f1 = CreerFonction("IT f1")
        Dim prise = Demande(CreerPatient(), idEmetteur, f1)
        dao.AttribueTacheToUserLog(prise, CompteDe(CreerUtilisateur(avecCle:=False)))
        Dim moi = CompteDe(idEmetteur)

        Assert.AreEqual(0, dao.GetAllTacheEnCours(False, Nothing, FiltreTacheDe(), True, moi).Rows.Count)
        Assert.AreEqual(0, dao.GetAllTacheEnCours(False, New List(Of Fonction), FiltreTacheDe(), True, moi).Rows.Count)
        Assert.AreEqual(0, dao.GetAllTacheEnCours(False, FonctionsTache(f1), Nothing, True, moi).Rows.Count)
        Assert.AreEqual(1, dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(), True, moi).Rows.Count)
    End Sub

    <TestMethod()> Public Sub EnCours_ParFonction_FiltresUniteEtSite()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim u1 = CreerUniteSanitaire("Unite 1")
        Dim u2 = CreerUniteSanitaire("Unite 2")
        Dim s1 = CreerSite("Site 1", u1)
        Dim s2 = CreerSite("Site 2", u1)
        Dim s3 = CreerSite("Site 3", u2)
        Dim surS1 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s1, horodatage:=New Date(2026, 1, 1))
        Dim surS2 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u1, siteId:=s2, horodatage:=New Date(2026, 1, 2))
        Dim surS3 = Demande(idPatient, idEmetteur, f1, uniteSanitaireId:=u2, siteId:=s3, horodatage:=New Date(2026, 1, 3))
        Dim moi = CompteDe(idEmetteur)

        CollectionAssert.AreEqual({surS1, surS2},
            IdsTaches(dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1)), True, moi)))
        CollectionAssert.AreEqual({surS2},
            IdsTaches(dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1, s2)), True, moi)))
        CollectionAssert.AreEqual({surS1, surS2, surS3},
            IdsTaches(dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1), UnitePourFiltreTache(u2)), True, moi)))
        ' Le site retenu de l'unité 1 ne masque pas l'unité 2, cochée sans site.
        CollectionAssert.AreEqual({surS2, surS3},
            IdsTaches(dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1, s2), UnitePourFiltreTache(u2)), True, moi)))
    End Sub

    <TestMethod()> Public Sub EnCours_OrdreParPrioriteOrdreAffichagePuisDate()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim a = Demande(idPatient, idEmetteur, f1, priorite:=200, ordreAffichage:=10, horodatage:=New Date(2026, 1, 1))
        Dim b = Demande(idPatient, idEmetteur, f1, priorite:=100, ordreAffichage:=20, horodatage:=New Date(2026, 1, 2))
        Dim c = Demande(idPatient, idEmetteur, f1, priorite:=100, ordreAffichage:=10, horodatage:=New Date(2026, 1, 9))
        Dim d = Demande(idPatient, idEmetteur, f1, priorite:=100, ordreAffichage:=10, horodatage:=New Date(2026, 1, 8))

        Dim ids = IdsTaches(dao.GetAllTacheEnCours(False, FonctionsTache(f1), FiltreTacheDe(), True, CompteDe(idEmetteur)))

        CollectionAssert.AreEqual({d, c, b, a}, ids)
    End Sub

    ' ---------------------------------------------------------------------
    ' GetAllRendezVousEnCours
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub RendezVousEnCours_RendezVousPrisParDate()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        RenommerUtilisateurPourTache(idTraitant, "LEROY", "Anne")
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1", designation:="IDE de secteur")
        Dim traitant = CompteDe(idTraitant)
        Dim rdv = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 5, 3))
        Dim specialiste = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_SPECIALISTE, dateRendezVous:=New Date(2030, 5, 1))
        Dim demandeRdv = EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV_DEMANDE,
                                                      traiteFonctionId:=f1, dateRendezVous:=New Date(2030, 5, 2),
                                                      typeDemandeRendezVous:="ANNEEMOIS"))
        Dim mission = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_MISSION, dateRendezVous:=New Date(2030, 5, 4))
        Dim staff = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.REUNION_STAFF, dateRendezVous:=New Date(2030, 5, 5))
        For Each idTache In {rdv, specialiste, demandeRdv, mission, staff}
            dao.AttribueTacheToUserLog(idTache, traitant)
        Next
        Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 4, 1))

        Dim table = dao.GetAllRendezVousEnCours()

        ' Aucun filtre de patient ni d'utilisateur : toute la base, rendez-vous pris seulement.
        CollectionAssert.AreEqual({specialiste, demandeRdv, rdv}, IdsTaches(table))
        Dim ligne = LigneTache(table, demandeRdv)
        Assert.AreEqual("ANNEEMOIS", CStr(ligne("type_demande_rendez_vous")))
        Assert.AreEqual("LEROY", CStr(ligne("user_traiteur_nom")))
        Assert.AreEqual("Anne", CStr(ligne("user_traiteur_prenom")))
        Assert.AreEqual("IDE de secteur", CStr(ligne("traite_fonction")))
        Assert.AreEqual(New Date(2030, 5, 2), CDate(ligne("date_rendez_vous")))
    End Sub

    ' ---------------------------------------------------------------------
    ' GetAgendaMyRDV
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub Agenda_MesRendezVousDeLaPeriode()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idMoi = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim moi = CompteDe(idMoi)
        Dim premierJour = Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 3, 10, 0, 0, 0)), moi)
        Dim dernierJour = Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 3, 12, 23, 0, 0)), moi)
        Dim minuitSuivant = Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 3, 13, 0, 0, 0)), moi)
        Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 3, 13, 8, 0, 0)), moi)
        Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 3, 9, 23, 59, 0)), moi)
        Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 3, 11)), CompteDe(idEmetteur))
        Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 3, 11))

        Dim table = dao.GetAgendaMyRDV(New Date(2030, 3, 10), New Date(2030, 3, 12), True, Nothing, Nothing, True, moi)

        ' Comportement actuel : la borne haute est le lendemain de dateFin à minuit,
        ' comparée par BETWEEN, donc incluse : un rendez-vous à 0 h le 13 sort dans
        ' l'agenda du 10 au 12.
        CollectionAssert.AreEqual({premierJour, dernierJour, minuitSuivant}, IdsTaches(table))
        Assert.AreEqual(New Date(2030, 3, 12, 23, 0, 0), CDate(LigneTache(table, dernierJour)("date_rendez_vous")))
    End Sub

    <TestMethod()> Public Sub Agenda_SeulsRendezVousMissionsEtReunions()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idMoi = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim moi = CompteDe(idMoi)
        Dim jour As New Date(2030, 6, 1, 9, 0, 0)
        Dim rdv = Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=jour), moi)
        Dim mission = Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_MISSION, dateRendezVous:=jour.AddHours(1)), moi)
        Dim staff = Pris(EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.REUNION_STAFF,
                                                      traiteFonctionId:=f1, dateRendezVous:=jour.AddHours(2), duree:=45)), moi)
        Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_SPECIALISTE, dateRendezVous:=jour), moi)
        Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, dateRendezVous:=jour), moi)

        Dim table = dao.GetAgendaMyRDV(jour.Date, jour.Date, True, Nothing, Nothing, False, moi)

        CollectionAssert.AreEqual({rdv, mission, staff}, IdsTaches(table))
        Assert.AreEqual(45, CInt(LigneTache(table, staff)("duree_mn")))
        Assert.AreEqual(DBNull.Value, LigneTache(table, rdv)("duree_mn"))
    End Sub

    <TestMethod()> Public Sub Agenda_ParFonction_NonAttribuesEtFiltres()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim f2 = CreerFonction("IT f2")
        Dim u1 = CreerUniteSanitaire("Unite 1")
        Dim s1 = CreerSite("Site 1", u1)
        Dim s2 = CreerSite("Site 2", u1)
        Dim jour As New Date(2030, 7, 1, 9, 0, 0)
        Dim prisS1 = Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=jour,
                                  uniteSanitaireId:=u1, siteId:=s1), CompteDe(idTraitant))
        Dim libreS2 = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=jour.AddHours(1),
                              uniteSanitaireId:=u1, siteId:=s2)
        Pris(Demande(idPatient, idEmetteur, f2, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=jour.AddHours(2)), CompteDe(idTraitant))
        Dim moi = CompteDe(idEmetteur)

        CollectionAssert.AreEqual({prisS1}, IdsTaches(dao.GetAgendaMyRDV(jour.Date, jour.Date, False, FonctionsTache(f1), FiltreTacheDe(), False, moi)))
        CollectionAssert.AreEqual({prisS1, libreS2}, IdsTaches(dao.GetAgendaMyRDV(jour.Date, jour.Date, False, FonctionsTache(f1), FiltreTacheDe(), True, moi)))
        CollectionAssert.AreEqual({libreS2},
            IdsTaches(dao.GetAgendaMyRDV(jour.Date, jour.Date, False, FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1, s2)), True, moi)))
        CollectionAssert.AreEqual({prisS1, libreS2},
            IdsTaches(dao.GetAgendaMyRDV(jour.Date, jour.Date, False, FonctionsTache(f1), FiltreTacheDe(UnitePourFiltreTache(u1)), True, moi)))
        Assert.AreEqual(0, dao.GetAgendaMyRDV(jour.Date, jour.Date, False, New List(Of Fonction), FiltreTacheDe(), True, moi).Rows.Count)
        Assert.AreEqual(0, dao.GetAgendaMyRDV(jour.Date, jour.Date, False, FonctionsTache(f1), Nothing, True, moi).Rows.Count)
    End Sub

    <TestMethod()> Public Sub Agenda_SiteRetenuDuneUniteNeMasquePasUneAutreUnite()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim u1 = CreerUniteSanitaire("Unite 1")
        Dim u2 = CreerUniteSanitaire("Unite 2")
        Dim s1 = CreerSite("Site 1", u1)
        Dim s1bis = CreerSite("Site 1 bis", u1)
        Dim s2 = CreerSite("Site 2", u2)
        Dim jour As New Date(2030, 7, 1, 9, 0, 0)
        Dim surS1 = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=jour,
                            uniteSanitaireId:=u1, siteId:=s1)
        Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=jour.AddHours(1),
                uniteSanitaireId:=u1, siteId:=s1bis)
        Dim surS2 = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=jour.AddHours(2),
                            uniteSanitaireId:=u2, siteId:=s2)
        Dim moi = CompteDe(idEmetteur)

        Dim filtre = FiltreTacheDe(UnitePourFiltreTache(u1, s1), UnitePourFiltreTache(u2))
        CollectionAssert.AreEqual({surS1, surS2}, IdsTaches(dao.GetAgendaMyRDV(jour.Date, jour.Date, False, FonctionsTache(f1), filtre, True, moi)))
    End Sub

    ''' <summary>Attribue la tâche à ce compte et renvoie son id.</summary>
    Private Function Pris(idTache As Long, traitant As Utilisateur) As Long
        dao.AttribueTacheToUserLog(idTache, traitant)
        Return idTache
    End Function

    ' ---------------------------------------------------------------------
    ' GetAllTacheEmise
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub Emises_ToutesOuSeulementLesNonFinales()
        Dim idMoi = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim moi = CompteDe(idMoi)
        Dim enAttente = Demande(idPatient, idMoi, f1, horodatage:=New Date(2026, 1, 1))
        Dim enCours = Pris(Demande(idPatient, idMoi, f1, horodatage:=New Date(2026, 1, 2)), CompteDe(idTraitant))
        Dim terminee = Demande(idPatient, idMoi, f1, horodatage:=New Date(2026, 1, 3))
        dao.ClotureTache(terminee, True, moi)
        Dim annulee = Demande(idPatient, idMoi, f1, horodatage:=New Date(2026, 1, 4))
        dao.AnnulationTache(annulee, moi)
        Demande(idPatient, idTraitant, f1)

        CollectionAssert.AreEqual({enAttente, enCours}, IdsTaches(dao.GetAllTacheEmise(True, moi)))
        CollectionAssert.AreEqual({enAttente, enCours, terminee, annulee}, IdsTaches(dao.GetAllTacheEmise(False, moi)))
    End Sub

    <TestMethod()> Public Sub Emises_OrdreEtColonnesDuTraitant()
        Dim idMoi = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        RenommerUtilisateurPourTache(idTraitant, "BERNARD", "Luc")
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1", designation:="Sage-femme de garde")
        Dim fEmettrice = CreerFonction("IT emettrice", designation:="IDE emettrice")
        Dim a = EnregistrerTache(TacheDeTest(idPatient, idMoi, traiteFonctionId:=f1, emetteurFonctionId:=fEmettrice,
                                             priorite:=300, horodatage:=New Date(2026, 1, 1)))
        Dim b = Demande(idPatient, idMoi, f1, priorite:=100, ordreAffichage:=20, horodatage:=New Date(2026, 1, 2))
        Dim c = Demande(idPatient, idMoi, f1, priorite:=100, ordreAffichage:=10, horodatage:=New Date(2026, 1, 4))
        Dim d = Demande(idPatient, idMoi, f1, priorite:=100, ordreAffichage:=10, horodatage:=New Date(2026, 1, 3))
        Pris(a, CompteDe(idTraitant))

        Dim table = dao.GetAllTacheEmise(False, CompteDe(idMoi))

        CollectionAssert.AreEqual({d, c, b, a}, IdsTaches(table))
        Dim ligne = LigneTache(table, a)
        Assert.AreEqual("BERNARD", CStr(ligne("user_traiteur_nom")))
        Assert.AreEqual("Luc", CStr(ligne("user_traiteur_prenom")))
        Assert.AreEqual("Sage-femme de garde", CStr(ligne("user_traiteur_fonction")))
        Assert.AreEqual("IDE emettrice", CStr(ligne("emetteur_fonction")))
        Assert.AreEqual("", CStr(LigneTache(table, b)("user_traiteur_nom")))
    End Sub

    ' ---------------------------------------------------------------------
    ' GetWorkflowHistoByEpisode
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub WorkflowHisto_DemandesDAvisDeLEpisodeParId()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        RenommerUtilisateurPourTache(idDemandeur, "PETIT", "Ines")
        RenommerUtilisateurPourTache(idMedecin, "GRAND", "Hugo")
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idDemandeur)
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim idAutreEpisode = CreerEpisode(idAutrePatient, idDemandeur)
        Dim fMedecin = CreerFonction("IT medecin", designation:="Medecin de garde")
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim demandeAvis = EnregistrerTache(TacheDeTest(idPatient, idDemandeur, episodeId:=idEpisode, emetteurFonctionId:=fIde,
                                                       traiteFonctionId:=fMedecin, destinataireFonctionId:=fMedecin))
        dao.AttribueTacheToUserLog(demandeAvis, CompteDe(idMedecin))
        Dim reponse = EnregistrerTache(TacheDeTest(idPatient, idMedecin, natureDeTache:=Tache.NatureTache.REPONSE, episodeId:=idEpisode,
                                                   emetteurFonctionId:=fMedecin, traiteFonctionId:=fIde, destinataireFonctionId:=fIde,
                                                   parentId:=demandeAvis), auteurId:=idMedecin)
        Dim annulee = EnregistrerTache(TacheDeTest(idPatient, idDemandeur, episodeId:=idEpisode, traiteFonctionId:=fMedecin))
        dao.AnnulationTache(annulee, CompteDe(idDemandeur))
        EnregistrerTache(TacheDeTest(idPatient, idDemandeur, typeDeTache:=Tache.TypeTache.RDV, episodeId:=idEpisode, traiteFonctionId:=fMedecin))
        EnregistrerTache(TacheDeTest(idPatient, idDemandeur, episodeId:=idEpisode, traiteFonctionId:=fMedecin, categorie:=Tache.CategorieTache.LOGISTIQUE))
        EnregistrerTache(TacheDeTest(idAutrePatient, idDemandeur, episodeId:=idAutreEpisode, traiteFonctionId:=fMedecin))

        Dim table = dao.GetWorkflowHistoByEpisode(idEpisode)

        CollectionAssert.AreEqual({demandeAvis, reponse}, IdsTaches(table))
        Dim premiere = LigneTache(table, demandeAvis)
        Assert.AreEqual("TERMINEE", CStr(premiere("etat")))
        Assert.AreEqual("DEMANDE", CStr(premiere("nature")))
        Assert.IsFalse(CBool(premiere("cloture")))
        Assert.AreNotEqual(DBNull.Value, premiere("horodate_cloture"))
        Assert.AreEqual("PETIT", CStr(premiere("user_emetteur_nom")))
        Assert.AreEqual("Ines", CStr(premiere("user_emetteur_prenom")))
        Assert.AreEqual(ProfilDeTest, CStr(premiere("user_emetteur_profil")))
        Assert.AreEqual("GRAND", CStr(premiere("user_traite_nom")))
        Assert.AreEqual("Hugo", CStr(premiere("user_traite_prenom")))
        Assert.AreEqual("Medecin de garde", CStr(premiere("user_destinataire_fonction")))
        Dim seconde = LigneTache(table, reponse)
        Assert.AreEqual("EN_ATTENTE", CStr(seconde("etat")))
        Assert.AreEqual("REPONSE", CStr(seconde("nature")))
        Assert.AreEqual("", CStr(seconde("user_traite_nom")), "réponse pas encore prise")
        Assert.AreEqual("", CStr(seconde("user_traite_profil")))
    End Sub

    <TestMethod()> Public Sub WorkflowHisto_EpisodeSansDemande_TableVide()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idEpisode = CreerEpisode(CreerPatient(), idUtilisateur)
        Assert.AreEqual(0, dao.GetWorkflowHistoByEpisode(idEpisode).Rows.Count)
    End Sub

    ' ---------------------------------------------------------------------
    ' GetRDVByPatient et GetRDVHistoriqueByPatient
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub RdvParPatient_RendezVousOuvertsDuPlusLointainAuPlusProche()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim f1 = CreerFonction("IT f1")
        Dim rdv = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 1, 10))
        Dim specialiste = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_SPECIALISTE, dateRendezVous:=New Date(2030, 3, 10))
        Dim mission = Pris(Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_MISSION, dateRendezVous:=New Date(2030, 2, 10)),
                           CompteDe(idEmetteur))
        Dim demandeRdv = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, dateRendezVous:=New Date(2030, 4, 1))
        Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.REUNION_STAFF, dateRendezVous:=New Date(2030, 1, 11))
        Demande(idPatient, idEmetteur, f1)
        Dim termine = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2029, 1, 1))
        dao.ClotureTache(termine, False, CompteDe(idEmetteur))
        EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV, traiteFonctionId:=f1,
                                     dateRendezVous:=New Date(2030, 1, 12), categorie:=Tache.CategorieTache.LOGISTIQUE))
        Dim dejaClos = TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV, traiteFonctionId:=f1, dateRendezVous:=New Date(2030, 1, 13))
        dejaClos.Cloture = True
        EnregistrerTache(dejaClos)
        Demande(idAutrePatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 1, 10))

        Dim table = dao.GetRDVByPatient(idPatient)

        CollectionAssert.AreEqual({demandeRdv, specialiste, mission, rdv}, IdsTaches(table))
        Dim ligneMission = LigneTache(table, mission)
        Assert.AreEqual("EN_COURS", CStr(ligneMission("etat")))
        Assert.AreEqual(idEmetteur, CLng(ligneMission("traite_user_id")))
        Assert.AreEqual("TEST", CStr(ligneMission("oa_utilisateur_nom")))
        Assert.AreEqual("Utilisateur", CStr(ligneMission("oa_utilisateur_prenom")))
        Dim ligneRdv = LigneTache(table, rdv)
        Assert.AreEqual(0L, CLng(ligneRdv("ror_id")), "sans parcours")
        Assert.AreEqual("", CStr(ligneRdv("oa_ror_nom")))
        Assert.AreEqual(0L, CLng(ligneRdv("oa_ror_specialite_id")))
        Assert.AreEqual("", CStr(ligneRdv("oa_utilisateur_nom")))
    End Sub

    <TestMethod()> Public Sub RdvParPatient_IntervenantDuParcours()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idRor = CreerRorParcours("Dr ROR Test", specialiteId:=SpecialiteTacheNonOasis, structureNom:="Clinique du Test", utilisateurId:=idEmetteur)
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur, specialiteId:=SpecialiteTacheNonOasis,
                                                sousCategorieId:=EnumSousCategoriePPS.specialiste, rorId:=idRor, intervenantOasis:=False)
        Dim rdv = EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV_SPECIALISTE,
                                               parcoursId:=idParcours, dateRendezVous:=New Date(2030, 1, 10)))

        Dim ligne = LigneTache(dao.GetRDVByPatient(idPatient), rdv)

        Assert.AreEqual(idParcours, CLng(ligne("parcours_id")))
        Assert.AreEqual(idRor, CLng(ligne("ror_id")))
        Assert.AreEqual("Dr ROR Test", CStr(ligne("oa_ror_nom")))
        Assert.AreEqual(CLng(SpecialiteTacheNonOasis), CLng(ligne("oa_ror_specialite_id")))
        Assert.AreEqual("Clinique du Test", CStr(ligne("oa_ror_structure_nom")))
    End Sub

    <TestMethod()> Public Sub RdvParPatient_PatientSansRendezVous_TableVide()
        Assert.AreEqual(0, dao.GetRDVByPatient(CreerPatient()).Rows.Count)
    End Sub

    <TestMethod()> Public Sub RdvHistorique_RendezVousClosDuParcoursDuPlusRecent()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idEmetteur)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)
        Dim idAutreParcours = CreerParcoursPourTache(idPatient, idEmetteur, specialiteId:=SpecialiteTacheNonOasis)
        Dim clos = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2025, 1, 10))
        dao.ClotureTache(clos, True, auteur)
        Dim closSpecialiste = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_SPECIALISTE, New Date(2025, 6, 10))
        dao.ClotureTache(closSpecialiste, True, auteur)
        Dim termineNonClos = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2025, 8, 1))
        dao.ClotureTache(termineNonClos, False, auteur)
        Dim mission = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_MISSION, New Date(2025, 9, 1))
        dao.ClotureTache(mission, True, auteur)
        Dim annule = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2025, 10, 1))
        dao.AnnulationTache(annule, auteur)
        Dim autreParcours = RdvDuParcours(idPatient, idEmetteur, idAutreParcours, Tache.TypeTache.RDV, New Date(2025, 11, 1))
        dao.ClotureTache(autreParcours, True, auteur)
        RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2025, 12, 1))

        Dim table = dao.GetRDVHistoriqueByPatient(idPatient, idParcours)

        Assert.AreEqual(1, table.Columns.Count)
        CollectionAssert.AreEqual({New Date(2025, 6, 10), New Date(2025, 1, 10)},
                                  table.Rows.Cast(Of DataRow)().Select(Function(r) CDate(r("date_rendez_vous"))).ToArray())
    End Sub

    Private Shared Function RdvDuParcours(idPatient As Long, idEmetteur As Long, idParcours As Long, typeDeTache As Tache.TypeTache,
                                quand As Date) As Long
        Return EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=typeDeTache, parcoursId:=idParcours,
                                            dateRendezVous:=quand))
    End Function

    ' ---------------------------------------------------------------------
    ' Recherches en [oasis].[oasis] par patient, parcours et épisode
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub DernierRendezVous_LePlusRecentDesTermines()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idEmetteur)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)
        Dim ancien = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2025, 1, 10))
        dao.ClotureTache(ancien, True, auteur)
        Dim recent = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_SPECIALISTE, New Date(2025, 5, 10))
        dao.ClotureTache(recent, False, auteur)
        RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2026, 1, 10))
        Dim mission = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_MISSION, New Date(2025, 9, 1))
        dao.ClotureTache(mission, True, auteur)
        Dim annule = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2025, 10, 1))
        dao.AnnulationTache(annule, auteur)
        ' Type RDV mais nature d'avis : écarté par le filtre sur la nature.
        Dim autreNature = EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV, natureDeTache:=Tache.NatureTache.DEMANDE,
                                                       parcoursId:=idParcours, dateRendezVous:=New Date(2025, 12, 1)))
        dao.ClotureTache(autreNature, True, auteur)

        Dim trouve = dao.GetDernierRenezVousByPatientId(idPatient, idParcours)

        Assert.AreEqual(recent, trouve.Id)
        Assert.AreEqual("TERMINEE", trouve.Etat)
        Assert.AreEqual(New Date(2025, 5, 10), trouve.DateRendezVous)
        Assert.AreEqual("RDV_SPECIALISTE", trouve.Type)
    End Sub

    <TestMethod()> Public Sub DernierRendezVous_Aucun_DonneUneTacheVide()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)
        RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2026, 1, 10))

        Dim trouve = dao.GetDernierRenezVousByPatientId(idPatient, idParcours)

        Assert.AreEqual(0L, trouve.Id)
        Assert.AreEqual("", trouve.Etat)
        Assert.AreEqual("", trouve.TypedemandeRendezVous)
        Assert.AreEqual(Date.MinValue, trouve.DateRendezVous)
    End Sub

    <TestMethod()> Public Sub ProchainRendezVousDuParcours_LeRendezVousOuvertLePlusLointain()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)
        Dim idAutreParcours = CreerParcoursPourTache(idPatient, idEmetteur, specialiteId:=SpecialiteTacheNonOasis)
        RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2030, 1, 10))
        Dim ouvert = Pris(RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_SPECIALISTE, New Date(2030, 2, 10)), CompteDe(idEmetteur))
        RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_DEMANDE, New Date(2030, 6, 1))
        RdvDuParcours(idPatient, idEmetteur, idAutreParcours, Tache.TypeTache.RDV, New Date(2030, 9, 1))
        Dim termine = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2030, 12, 1))
        dao.ClotureTache(termine, True, CompteDe(idEmetteur))

        ' Comportement actuel : « prochain » est le plus lointain des rendez-vous
        ' ouverts (ORDER BY date_rendez_vous DESC), pas le plus proche.
        Dim trouve = dao.GetProchainRendezVousByPatientIdEtParcours(idPatient, idParcours)

        Assert.AreEqual(ouvert, trouve.Id)
        Assert.AreEqual("EN_COURS", trouve.Etat)
        Assert.AreEqual(New Date(2030, 2, 10), trouve.DateRendezVous)
    End Sub

    <TestMethod()> Public Sub ProchainRendezVousDuParcours_Aucun_DonneUneTacheVide()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)

        Dim trouve = dao.GetProchainRendezVousByPatientIdEtParcours(idPatient, idParcours)

        Assert.AreEqual(0L, trouve.Id)
        Assert.AreEqual("", trouve.Etat)
        Assert.AreEqual(Date.MinValue, trouve.DateRendezVous)
    End Sub

    <TestMethod()> Public Sub ProchainRendezVousOasisParFonction_SeulsLesRendezVousEnAttenteVersCetteFonction()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim f2 = CreerFonction("IT f2")
        Dim proche = RdvVers(idPatient, idEmetteur, f1, New Date(2030, 1, 10))
        Dim lointain = RdvVers(idPatient, idEmetteur, f1, New Date(2030, 3, 10))
        ' Pris : écarté, seul l'état EN_ATTENTE compte.
        Pris(RdvVers(idPatient, idEmetteur, f1, New Date(2030, 5, 10)), CompteDe(idEmetteur))
        RdvVers(idPatient, idEmetteur, f2, New Date(2030, 6, 10))

        Dim trouve = dao.GetProchainRendezVousOasisByPatientIdEtFonctionId(idPatient, f1)

        Assert.AreEqual(lointain, trouve.Id)
        Assert.AreEqual(f1, trouve.DestinataireFonctionId)
        Assert.AreNotEqual(proche, trouve.Id)
        Assert.AreEqual(0L, dao.GetProchainRendezVousOasisByPatientIdEtFonctionId(idPatient, CreerFonction("IT f3")).Id)
    End Sub

    Private Shared Function RdvVers(idPatient As Long, idEmetteur As Long, idFonction As Long, quand As Date) As Long
        Return EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV, traiteFonctionId:=idFonction,
                                            destinataireFonctionId:=idFonction, dateRendezVous:=quand))
    End Function

    <TestMethod()> Public Sub ProchaineDemandeDeRendezVous_LaDemandeOuverteDuParcours()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)
        Dim idAutreParcours = CreerParcoursPourTache(idPatient, idEmetteur, specialiteId:=SpecialiteTacheNonOasis)
        Assert.AreEqual(0L, dao.GetProchaineDemandeRendezVousByPatientId(idPatient, idParcours).Id, "aucune demande")

        Dim ouverte = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_DEMANDE, New Date(2030, 4, 1))
        RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV, New Date(2030, 8, 1))
        RdvDuParcours(idPatient, idEmetteur, idAutreParcours, Tache.TypeTache.RDV_DEMANDE, New Date(2030, 9, 1))
        Dim terminee = RdvDuParcours(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_DEMANDE, New Date(2030, 12, 1))
        dao.ClotureTache(terminee, False, CompteDe(idEmetteur))

        Dim trouve = dao.GetProchaineDemandeRendezVousByPatientId(idPatient, idParcours)

        Assert.AreEqual(ouverte, trouve.Id)
        Assert.AreEqual("RDV_DEMANDE", trouve.Nature)
        Assert.AreEqual(New Date(2030, 4, 1), trouve.DateRendezVous)
    End Sub

    <TestMethod()> Public Sub DemandeEnCoursParEpisode_LaDemandeDAvisOuverte()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idEmetteur)
        Dim f1 = CreerFonction("IT f1")
        Dim annulee = EnregistrerTache(TacheDeTest(idPatient, idEmetteur, episodeId:=idEpisode, traiteFonctionId:=f1))
        dao.AnnulationTache(annulee, CompteDe(idEmetteur))
        EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV, episodeId:=idEpisode, traiteFonctionId:=f1))
        Dim ouverte = Pris(EnregistrerTache(TacheDeTest(idPatient, idEmetteur, episodeId:=idEpisode, traiteFonctionId:=f1)), CompteDe(idEmetteur))

        Dim trouve = dao.GetDemandeEnCoursByEpisode(idEpisode)

        Assert.AreEqual(ouverte, trouve.Id)
        Assert.AreEqual("EN_COURS", trouve.Etat)
        Assert.AreEqual(idEpisode, trouve.EpisodeId)
    End Sub

    <TestMethod()> Public Sub DemandeEnCoursParEpisode_Aucune_DonneUneTacheVide()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idEpisode = CreerEpisode(CreerPatient(), idEmetteur)

        Dim trouve = dao.GetDemandeEnCoursByEpisode(idEpisode)

        Assert.AreEqual(0L, trouve.Id)
        Assert.AreEqual("", trouve.Etat)
        Assert.AreEqual("", trouve.Nature)
        ' Comportement actuel : le type de tâche est rangé dans TypedemandeRendezVous.
        Assert.AreEqual("AVIS_EPISODE", trouve.TypedemandeRendezVous)
    End Sub

    <TestMethod()> Public Sub ExisteDemandeAvisMedical_DeParamedicalVersMedical()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim fMedecin = CreerFonction("IT medecin", typeFonction:="MEDICAL")

        ' Demande d'une IDE vers un médecin, même terminée : oui.
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idEmetteur)
        Dim demandeAvis = AvisDe(idPatient, idEmetteur, idEpisode, fIde, fMedecin)
        dao.ClotureTache(demandeAvis, True, CompteDe(idEmetteur))
        Assert.IsTrue(dao.ExisteDemandeAvisMedicalByEpisode(idEpisode))

        ' Annulée : non.
        Dim idPatient2 = CreerPatient()
        Dim idEpisode2 = CreerEpisode(idPatient2, idEmetteur)
        dao.AnnulationTache(AvisDe(idPatient2, idEmetteur, idEpisode2, fIde, fMedecin), CompteDe(idEmetteur))
        Assert.IsFalse(dao.ExisteDemandeAvisMedicalByEpisode(idEpisode2))

        ' Dans l'autre sens, ou entre médecins : non.
        Dim idPatient3 = CreerPatient()
        Dim idEpisode3 = CreerEpisode(idPatient3, idEmetteur)
        AvisDe(idPatient3, idEmetteur, idEpisode3, fMedecin, fIde)
        AvisDe(idPatient3, idEmetteur, idEpisode3, fMedecin, fMedecin)
        Assert.IsFalse(dao.ExisteDemandeAvisMedicalByEpisode(idEpisode3))

        ' Seule une tâche fille (parent renseigné) va de l'IDE au médecin : non.
        Dim idPatient4 = CreerPatient()
        Dim idEpisode4 = CreerEpisode(idPatient4, idEmetteur)
        Dim racine = AvisDe(idPatient4, idEmetteur, idEpisode4, fMedecin, fIde)
        EnregistrerTache(TacheDeTest(idPatient4, idEmetteur, natureDeTache:=Tache.NatureTache.REPONSE, episodeId:=idEpisode4,
                                     emetteurFonctionId:=fIde, traiteFonctionId:=fMedecin, destinataireFonctionId:=fMedecin,
                                     parentId:=racine))
        Assert.IsFalse(dao.ExisteDemandeAvisMedicalByEpisode(idEpisode4))
    End Sub

    Private Shared Function AvisDe(idPatient As Long, idEmetteur As Long, idEpisode As Long, fEmettrice As Long, fDestinataire As Long) As Long
        Return EnregistrerTache(TacheDeTest(idPatient, idEmetteur, episodeId:=idEpisode, emetteurFonctionId:=fEmettrice,
                                            traiteFonctionId:=fDestinataire, destinataireFonctionId:=fDestinataire))
    End Function

    <TestMethod()> Public Sub DerniereDemandeDeRendezVous_LePlusGrandId()
        ExigerBaseOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim f1 = CreerFonction("IT f1")
        Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, dateRendezVous:=New Date(2030, 1, 1))
        Dim derniere = Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, dateRendezVous:=New Date(2029, 1, 1))
        dao.AnnulationTache(derniere, CompteDe(idEmetteur))
        Demande(idPatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV, dateRendezVous:=New Date(2030, 1, 1))
        Demande(idAutrePatient, idEmetteur, f1, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, dateRendezVous:=New Date(2030, 1, 1))

        ' Tous états confondus, annulées comprises.
        Assert.AreEqual(derniere, dao.GetLastDemandeRendezVousByPatient(idPatient))
    End Sub

    <TestMethod()> Public Sub DerniereDemandeDeRendezVous_Aucune_LeveInvalidCast()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()

        ' Comportement actuel : MAX(id) rend une ligne NULL, lue telle quelle dans un
        ' Integer. La connexion reste ouverte (pas de Finally).
        Assert.ThrowsException(Of InvalidCastException)(Sub() dao.GetLastDemandeRendezVousByPatient(idPatient))
    End Sub

    ' ---------------------------------------------------------------------
    ' GetTacheBeanAssocie
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub BeanAssocie_SansParcours_LitLesFichesDeLaTache()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        RenommerUtilisateurPourTache(idEmetteur, "EMETTEUR", "Eve")
        RenommerUtilisateurPourTache(idTraitant, "TRAITANT", "Tom")
        Dim idPatient = CreerPatient("ASSOCIE", "Patient")
        Dim idUnite = CreerUniteSanitaire("Unite associee")
        Dim idSite = CreerSite("Site associe", idUnite)
        Dim fEmettrice = CreerFonction("IT emettrice", designation:="IDE emettrice")
        Dim fTraitante = CreerFonction("IT traitante", designation:="Medecin traitant")
        Dim idTache = EnregistrerTache(TacheDeTest(idPatient, idEmetteur, emetteurFonctionId:=fEmettrice, traiteFonctionId:=fTraitante,
                                                   uniteSanitaireId:=idUnite, siteId:=idSite))
        dao.AttribueTacheToUserLog(idTache, CompteDe(idTraitant))

        Dim associe = dao.GetTacheBeanAssocie(Relire(idTache))

        Assert.AreEqual("EMETTEUR", associe.UserEmetteur.UtilisateurNom)
        Assert.AreEqual(fEmettrice, associe.FonctionEmetteur.Id)
        Assert.AreEqual("IDE emettrice", associe.FonctionEmetteur.Designation)
        Assert.AreEqual(CInt(idUnite), associe.UniteSanitaire.Oa_unite_sanitaire_id)
        Assert.AreEqual("Unite associee", associe.UniteSanitaire.Oa_unite_sanitaire_description)
        Assert.AreEqual(idSite, associe.Site.Oa_site_id)
        Assert.AreEqual("Site associe", associe.Site.Oa_site_description)
        Assert.AreEqual(CInt(idPatient), associe.Patient.PatientId)
        Assert.AreEqual("ASSOCIE", associe.Patient.PatientNom)
        Assert.AreEqual(fTraitante, associe.FonctionTraiteur.Id)
        Assert.AreEqual("TRAITANT", associe.UserTraiteur.UtilisateurNom)
        Assert.IsNull(associe.Parcours)
        Assert.IsNull(associe.Specialite)
        Assert.AreEqual("Oasis", associe.Intervenant)
        ' Comportement actuel : l'épisode et le sous-épisode ne sont jamais chargés.
        Assert.IsNull(associe.Episode)
        Assert.IsNull(associe.SousEpisode)
    End Sub

    <TestMethod()> Public Sub BeanAssocie_TacheNonAttribueeSansFonctionEmettrice()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idUnite = CreerUniteSanitaire("Unite")
        Dim idSite = CreerSite("Site", idUnite)
        Dim idTache = EnregistrerTache(TacheDeTest(CreerPatient(), idEmetteur, traiteFonctionId:=CreerFonction("IT traitante"),
                                                   uniteSanitaireId:=idUnite, siteId:=idSite))

        Dim associe = dao.GetTacheBeanAssocie(Relire(idTache))

        Assert.IsNull(associe.FonctionEmetteur)
        Assert.IsNull(associe.UserTraiteur)
        Assert.IsNotNull(associe.FonctionTraiteur)
    End Sub

    <TestMethod()> Public Sub BeanAssocie_ParcoursHorsOasis_IntervenantDuRor()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idUnite = CreerUniteSanitaire("Unite")
        Dim idSite = CreerSite("Site", idUnite)
        Dim idRor = CreerRorParcours("Dr Externe", specialiteId:=SpecialiteTacheNonOasis, utilisateurId:=idEmetteur)
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur, specialiteId:=SpecialiteTacheNonOasis,
                                                sousCategorieId:=EnumSousCategoriePPS.specialiste, rorId:=idRor, intervenantOasis:=False)
        Dim idTache = EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV_SPECIALISTE,
                                                   traiteFonctionId:=CreerFonction("IT traitante"), parcoursId:=idParcours,
                                                   uniteSanitaireId:=idUnite, siteId:=idSite, dateRendezVous:=New Date(2030, 1, 1)))

        Dim associe = dao.GetTacheBeanAssocie(Relire(idTache))

        Assert.AreEqual(CInt(idParcours), associe.Parcours.Id)
        Assert.AreEqual(CLng(SpecialiteTacheNonOasis), associe.Specialite.SpecialiteId)
        Assert.IsFalse(associe.Specialite.Oasis)
        Assert.AreEqual("Dr Externe", associe.Intervenant)
    End Sub

    <TestMethod()> Public Sub BeanAssocie_ParcoursOasis_IntervenantOasis()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idUnite = CreerUniteSanitaire("Unite")
        Dim idSite = CreerSite("Site", idUnite)
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur, specialiteId:=SpecialiteTacheOasis)
        Dim idTache = EnregistrerTache(TacheDeTest(idPatient, idEmetteur, typeDeTache:=Tache.TypeTache.RDV,
                                                   traiteFonctionId:=CreerFonction("IT traitante"), parcoursId:=idParcours,
                                                   uniteSanitaireId:=idUnite, siteId:=idSite, dateRendezVous:=New Date(2030, 1, 1)))

        Dim associe = dao.GetTacheBeanAssocie(Relire(idTache))

        Assert.AreEqual(CLng(SpecialiteTacheOasis), associe.Specialite.SpecialiteId)
        Assert.IsTrue(associe.Specialite.Oasis)
        Assert.AreEqual("Oasis", associe.Intervenant)
    End Sub

    <TestMethod()> Public Sub BeanAssocie_SansUniteSanitaire_LeveArgumentException()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTache = EnregistrerTache(TacheDeTest(CreerPatient(), idEmetteur, traiteFonctionId:=CreerFonction("IT traitante")))

        ' Comportement actuel : l'unité sanitaire et le site sont lus sans tester 0 ;
        ' une tâche sans unité (patient sans rattachement) ne s'ouvre pas.
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetTacheBeanAssocie(Relire(idTache)))
    End Sub

    ' ---------------------------------------------------------------------
    ' TestMultipleSelect
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub TestMultipleSelect_DeuxTablesRempliesDeuxFois()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        RenommerUtilisateurPourTache(idUtilisateur, "MULTIPLE", "Select")
        CreerPatient()
        Dim nbPatients = Math.Min(100, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient")))
        Dim nbUtilisateurs = Math.Min(100, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_utilisateur")))

        Dim tables = dao.TestMultipleSelect()

        Assert.AreEqual(2, tables.Count)
        Assert.IsTrue(tables(0).Columns.Contains("oa_patient_nir"))
        Assert.IsTrue(tables(1).Columns.Contains("oa_utilisateur_nom"))
        Assert.IsTrue(tables(1).Rows.Cast(Of DataRow)().Any(Function(r) Not IsDBNull(r("oa_utilisateur_nom")) AndAlso CStr(r("oa_utilisateur_nom")) = "MULTIPLE"))
        ' Comportement actuel : après le Fill du DataSet, chaque table est remplie une
        ' seconde fois avec le premier jeu de résultats (les patients). La première
        ' contient donc chaque patient deux fois, la seconde reçoit une colonne
        ' oa_patient_nir et une ligne par patient en plus des utilisateurs.
        Assert.AreEqual(2 * nbPatients, tables(0).Rows.Count)
        Assert.IsTrue(tables(1).Columns.Contains("oa_patient_nir"))
        Assert.AreEqual(nbUtilisateurs + nbPatients, tables(1).Rows.Count)
    End Sub

End Class
