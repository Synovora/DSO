Imports Oasis_Common

''' <summary>
''' ParcoursDao contre la base de test. Le parcours de soins est créé, modifié et
''' annulé par le client lourd (RadFParcoursDetailEdit, création de patient et
''' d'épisode), qui le lit aussi dans la synthèse, les rendez-vous et les courriers :
''' sous oasis_client. GetAllParcoursbyPatient sert en plus au portail
''' (SyntheseController, RDVController, DashboardController) : il est aussi
''' éprouvé sous oasis_web.
'''
''' L'historique écrit à chaque création, ExistIntervenantByPatientId et
''' GetAllParcoursbyPatient nomment la base ([oasis].[oasis]) : les tests qui les
''' touchent commencent par ExigerBaseOasis.
''' </summary>
<TestClass()> Public Class ParcoursDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ParcoursDao

    Private Shared Function Auteur(utilisateurId As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}
    End Function

    Private Shared Function IdsDe(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("oa_parcours_id"))).ToArray()
    End Function

    Private Shared Function LigneDe(table As DataTable, parcoursId As Long) As DataRow
        Return table.Rows.Cast(Of DataRow)().Single(Function(r) CLng(r("oa_parcours_id")) = parcoursId)
    End Function

    Private Shared Function DateSynthese(patientId As Long) As Object
        Return Scalaire("SELECT oa_patient_synthese_date_maj FROM oasis.oa_patient WHERE oa_patient_id = @p0", patientId)
    End Function

    Private Sub Annuler(parcoursId As Long, utilisateurId As Long)
        dao.AnnulationIntervenantParcours(dao.GetParcoursById(CInt(parcoursId)), Auteur(utilisateurId))
    End Sub

    ' --- Création, lecture, modification, annulation --------------------------------

    <TestMethod()> Public Sub CreateIntervenantParcours_PuisGetParcoursById_RelitLeParcoursEtLHistorise()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idRor = CreerRorParcours("DUPONT Cardio")
        PoserDateMajSynthese(idPatient, Nothing)

        Dim idParcours = CreerParcoursPatient(idPatient, idRor, commentaire:="Suivi cardiologique", baseCalcul:="PAR_MOIS",
                                              rythme:=2, cacher:=True, utilisateurId:=idUtilisateur)

        Assert.IsTrue(idParcours > 0)
        Dim lu = dao.GetParcoursById(CInt(idParcours))
        Assert.AreEqual(CInt(idParcours), lu.Id)
        Assert.AreEqual(CInt(idPatient), lu.PatientId)
        Assert.AreEqual(SpecialiteParcoursTest, lu.SpecialiteId)
        Assert.AreEqual(CategorieParcoursSuivi, lu.CategorieId)
        Assert.AreEqual(SousCategorieParcoursSpecialiste, lu.SousCategorieId)
        Assert.IsFalse(lu.IntervenantOasis)
        Assert.AreEqual(CInt(idRor), lu.RorId)
        Assert.AreEqual("Suivi cardiologique", lu.Commentaire)
        Assert.AreEqual("PAR_MOIS", lu.Base)
        Assert.AreEqual(2, lu.Rythme)
        Assert.IsTrue(lu.Cacher)
        Assert.IsFalse(lu.Inactif)
        Assert.AreEqual(CInt(idUtilisateur), lu.UserCreation)
        Assert.AreEqual(Date.Today, lu.DateCreation.Date)
        Assert.AreEqual(0, lu.UserModification)
        Assert.AreEqual(Date.MinValue, lu.DateModification)

        Dim histo = HistoriqueParcours(idParcours)
        Assert.AreEqual(1, histo.Rows.Count)
        Assert.AreEqual(1, CInt(histo.Rows(0)("oa_parcours_histo_etat")), "Creation")
        Assert.AreEqual(idUtilisateur, CLng(histo.Rows(0)("oa_parcours_histo_user_historisation")))
        Assert.AreEqual("Suivi cardiologique", CStr(histo.Rows(0)("oa_parcours_commentaire")))
        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
    End Sub

    <TestMethod()> Public Sub GetParcoursById_ParcoursAbsent_LeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetParcoursById(987654321))
        StringAssert.Contains(erreur.Message, "parcours inexistant")
    End Sub

    <TestMethod()> Public Sub ModificationIntervenantParcours_ReecritLeParcoursEtLHistorise()
        ExigerBaseOasis()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPatient(idPatient, CreerRorParcours("AVANT"), utilisateurId:=idCreateur)
        Dim autreRor = CreerRorParcours("APRES")
        PoserDateMajSynthese(idPatient, Nothing)
        Dim fiche = dao.GetParcoursById(CInt(idParcours))
        fiche.SpecialiteId = SpecialiteParcoursAutre
        fiche.SousCategorieId = 5
        fiche.RorId = CInt(autreRor)
        fiche.Commentaire = "Modifie"
        fiche.Base = "TOUS_LES_2_ANS"
        fiche.Rythme = 3
        fiche.Cacher = True
        fiche.IntervenantOasis = True

        Assert.IsTrue(dao.ModificationIntervenantParcours(fiche, Auteur(idModificateur)))

        Dim relu = dao.GetParcoursById(CInt(idParcours))
        Assert.AreEqual(SpecialiteParcoursAutre, relu.SpecialiteId)
        Assert.AreEqual(5, relu.SousCategorieId)
        Assert.AreEqual(CInt(autreRor), relu.RorId)
        Assert.AreEqual("Modifie", relu.Commentaire)
        Assert.AreEqual("TOUS_LES_2_ANS", relu.Base)
        Assert.AreEqual(3, relu.Rythme)
        Assert.IsTrue(relu.Cacher)
        Assert.IsTrue(relu.IntervenantOasis)
        Assert.IsFalse(relu.Inactif)
        Assert.AreEqual(CInt(idCreateur), relu.UserCreation, "la création garde son auteur")
        Assert.AreEqual(CInt(idModificateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
        Dim histo = HistoriqueParcours(idParcours)
        Assert.AreEqual(2, histo.Rows.Count)
        Assert.AreEqual(2, CInt(histo.Rows(1)("oa_parcours_histo_etat")), "Modification")
        Assert.AreEqual(idModificateur, CLng(histo.Rows(1)("oa_parcours_histo_user_historisation")))
        Assert.AreEqual("TOUS_LES_2_ANS", CStr(histo.Rows(1)("oa_parcours_base")))
    End Sub

    <TestMethod()> Public Sub AnnulationIntervenantParcours_RendLeParcoursInactifEtLHistorise()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idRor = CreerRorParcours("INTERVENANT")
        Dim idParcours = CreerParcoursPatient(idPatient, idRor)
        Dim autre = CreerParcoursPatient(idPatient, idRor, specialiteId:=SpecialiteParcoursAutre)

        Assert.IsTrue(dao.AnnulationIntervenantParcours(dao.GetParcoursById(CInt(idParcours)), Auteur(idUtilisateur)))

        Dim relu = dao.GetParcoursById(CInt(idParcours))
        Assert.IsTrue(relu.Inactif, "toujours lisible par son id")
        Assert.AreEqual(CInt(idUtilisateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.IsFalse(dao.GetParcoursById(CInt(autre)).Inactif)
        Dim histo = HistoriqueParcours(idParcours)
        Assert.AreEqual(2, histo.Rows.Count)
        Assert.AreEqual(4, CInt(histo.Rows(1)("oa_parcours_histo_etat")), "Annulation")
        Assert.IsTrue(CBool(histo.Rows(1)("oa_parcours_inactif")))
    End Sub

    ' --- Parcours d'un patient -----------------------------------------------------

    ''' <summary>
    ''' Trois parcours actifs, un annulé, un d'un autre patient, et des rendez-vous
    ''' sur le parcours « B ». Rend les ids dans l'ordre attendu (sous-catégorie puis
    ''' spécialité), et les dates des rendez-vous par référence.
    ''' </summary>
    Private Function PreparerParcoursAvecRendezVous(idUtilisateur As Long, idPatient As Long,
                                                    ByRef dernierRdv As Date, ByRef prochainRdv As Date,
                                                    ByRef demandeRdv As Date) As Long()
        Dim parcoursIde = CreerParcoursPatient(idPatient, RorIdeOasis, specialiteId:=SpecialiteIdeOasis,
                                               sousCategorieId:=SousCategorieParcoursIde, intervenantOasis:=True)
        Dim parcoursA = CreerParcoursPatient(idPatient, CreerRorParcours("ALBERT", specialiteId:=SpecialiteParcoursAutre),
                                             specialiteId:=SpecialiteParcoursAutre)
        Dim parcoursB = CreerParcoursPatient(idPatient, CreerRorParcours("BERNARD", structureNom:="Clinique B"),
                                             specialiteId:=SpecialiteParcoursTest)
        Dim annule = CreerParcoursPatient(idPatient, CreerRorParcours("ANNULE"), specialiteId:=SpecialiteParcoursTest)
        Annuler(annule, idUtilisateur)
        CreerParcoursPatient(CreerPatient("AUTRE", "Patient"), CreerRorParcours("AUTRE"))

        dernierRdv = New Date(2026, 3, 5, 9, 0, 0)
        prochainRdv = Date.Today.AddDays(10).AddHours(14)
        demandeRdv = Date.Today.AddDays(60)
        Dim rdv = Oasis_Common.Tache.TypeTache.RDV
        Dim terminee = Oasis_Common.Tache.EtatTache.TERMINEE
        Dim enAttente = Oasis_Common.Tache.EtatTache.EN_ATTENTE
        CreerTacheRendezVousParcours(idPatient, parcoursB, idUtilisateur, rdv, terminee, New Date(2026, 1, 10, 9, 0, 0))
        CreerTacheRendezVousParcours(idPatient, parcoursB, idUtilisateur, rdv, terminee, dernierRdv)
        CreerTacheRendezVousParcours(idPatient, parcoursB, idUtilisateur, rdv, Oasis_Common.Tache.EtatTache.ANNULEE, Date.Today.AddDays(-2))
        CreerTacheRendezVousParcours(idPatient, parcoursB, idUtilisateur, Oasis_Common.Tache.TypeTache.RDV_SPECIALISTE, enAttente,
                                     Date.Today.AddDays(20))
        CreerTacheRendezVousParcours(idPatient, parcoursB, idUtilisateur, rdv, enAttente, prochainRdv)
        CreerTacheRendezVousParcours(idPatient, parcoursB, idUtilisateur, Oasis_Common.Tache.TypeTache.RDV_DEMANDE, enAttente, demandeRdv)

        Return New Long() {parcoursIde, parcoursB, parcoursA}
    End Function

    Private Shared Sub VerifierParcoursAvecRendezVous(table As DataTable, attendus() As Long,
                                                      dernierRdv As Date, prochainRdv As Date, demandeRdv As Date)
        CollectionAssert.AreEqual(attendus, IdsDe(table), "sous-catégorie puis spécialité, sans l'annulé ni l'autre patient")
        Dim ide = LigneDe(table, attendus(0))
        Assert.AreEqual("IDE OASIS DE TEST", CStr(ide("oa_ror_nom")))
        Assert.IsTrue(CBool(ide("oa_parcours_intervenant_oasis")))
        Dim avecRdv = LigneDe(table, attendus(1))
        Assert.AreEqual("BERNARD", CStr(avecRdv("oa_ror_nom")))
        Assert.AreEqual("Clinique B", CStr(avecRdv("oa_ror_structure_nom")))
        Assert.AreEqual(dernierRdv, CDate(avecRdv("LastRendezVous")), "dernier rendez-vous terminé, l'annulé ne compte pas")
        Assert.AreEqual(prochainRdv, CDate(avecRdv("NextRendezVous")), "le plus proche des rendez-vous en attente")
        Assert.AreEqual(demandeRdv, CDate(avecRdv("DateDemandeRdv")))
        Assert.AreEqual("ANNEEMOIS", CStr(avecRdv("TypeDemandeRdv")))
        Dim sansRdv = LigneDe(table, attendus(2))
        Assert.AreEqual(DBNull.Value, sansRdv("LastRendezVous"))
        Assert.AreEqual(DBNull.Value, sansRdv("NextRendezVous"))
        Assert.AreEqual(DBNull.Value, sansRdv("DateDemandeRdv"))
    End Sub

    <TestMethod()> Public Sub GetAllParcoursbyPatient_SousLeCompteClient_RendLesParcoursActifsEtLeursRendezVous()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim dernierRdv, prochainRdv, demandeRdv As Date
        Dim attendus = PreparerParcoursAvecRendezVous(idUtilisateur, idPatient, dernierRdv, prochainRdv, demandeRdv)

        Dim table = dao.GetAllParcoursbyPatient(CInt(idPatient))

        VerifierParcoursAvecRendezVous(table, attendus, dernierRdv, prochainRdv, demandeRdv)
    End Sub

    <TestMethod()> Public Sub GetAllParcoursbyPatient_SousLeCompteWeb_RendLaMemeChoseAuPortail()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim dernierRdv, prochainRdv, demandeRdv As Date
        Dim attendus = PreparerParcoursAvecRendezVous(idUtilisateur, idPatient, dernierRdv, prochainRdv, demandeRdv)
        ' Données créées comme le client lourd, lues comme le portail.
        UtiliserCompte(Compte.Web)

        Dim table = dao.GetAllParcoursbyPatient(CInt(idPatient))

        VerifierParcoursAvecRendezVous(table, attendus, dernierRdv, prochainRdv, demandeRdv)
    End Sub

    <TestMethod()> Public Sub GetAllParcoursbyPatient_PatientSansParcours_RendUneTableVide()
        ExigerBaseOasis()
        Assert.AreEqual(0, dao.GetAllParcoursbyPatient(CInt(CreerPatient())).Rows.Count)
    End Sub

    <TestMethod()> Public Sub GetParcoursIDEbyPatient_TrouveLIdeOasisActif()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        CreerParcoursPatient(idPatient, CreerRorParcours("SPECIALISTE"))
        Dim ide = CreerParcoursPatient(idPatient, RorIdeOasis, specialiteId:=SpecialiteIdeOasis,
                                       sousCategorieId:=SousCategorieParcoursIde, intervenantOasis:=True)

        Dim trouve = dao.GetParcoursIDEbyPatient(CInt(idPatient))

        Assert.AreEqual(CInt(ide), trouve.Id)
        Assert.AreEqual(CInt(RorIdeOasis), trouve.RorId)
    End Sub

    <TestMethod()> Public Sub GetParcoursIDEbyPatient_IdeHorsOasisOuAnnule_NestPasTrouve()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim horsOasis = CreerPatient()
        Dim annule = CreerPatient("ANNULE", "Patient")
        CreerParcoursPatient(horsOasis, CreerRorParcours("IDE LIBERAL"), specialiteId:=SpecialiteIdeOasis,
                             sousCategorieId:=SousCategorieParcoursIde, intervenantOasis:=False)
        Annuler(CreerParcoursPatient(annule, RorIdeOasis, specialiteId:=SpecialiteIdeOasis,
                                     sousCategorieId:=SousCategorieParcoursIde, intervenantOasis:=True), idUtilisateur)

        ' RadFEpisodeDetailCreation reconnaît ce cas au début du message.
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetParcoursIDEbyPatient(CInt(horsOasis)))
        Assert.IsTrue(erreur.Message.StartsWith("parcours inexistant"))
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetParcoursIDEbyPatient(CInt(annule)))
    End Sub

    <TestMethod()> Public Sub GetListOfIntervenantNonOasisByPatient_RendNomStructureEtLibelleDeSpecialite()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim rorCardio = CreerRorParcours("CARDIO", structureNom:="Cabinet cardio")
        Dim rorSageFemme = CreerRorParcours("SAGE FEMME", specialiteId:=SpecialiteParcoursAutre, structureNom:="Maternite")
        Dim rorInactive = CreerRorParcours("SPECIALITE INACTIVE", specialiteId:=SpecialiteParcoursInactive)
        CreerParcoursPatient(idPatient, rorCardio, specialiteId:=SpecialiteParcoursTest)
        CreerParcoursPatient(idPatient, rorInactive, specialiteId:=SpecialiteParcoursInactive)
        CreerParcoursPatient(idPatient, rorSageFemme, specialiteId:=SpecialiteParcoursAutre, sousCategorieId:=5)
        CreerParcoursPatient(idPatient, RorIdeOasis, specialiteId:=SpecialiteIdeOasis, sousCategorieId:=SousCategorieParcoursIde,
                             intervenantOasis:=True)
        Annuler(CreerParcoursPatient(idPatient, CreerRorParcours("ANNULE")), idUtilisateur)

        Dim liste = dao.GetListOfIntervenantNonOasisByPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {rorSageFemme, rorCardio, rorInactive}, liste.Select(Function(i) i.IntervenantId).ToArray(),
                                  "sous-catégorie puis spécialité, sans l'intervenant Oasis ni l'annulé")
        Assert.IsTrue(liste.All(Function(i) i.PatientId = idPatient))
        Assert.AreEqual("SAGE FEMME", liste(0).Nom)
        Assert.AreEqual("Maternite", liste(0).[Structure])
        Assert.AreEqual(SpecialiteParcoursAutreLibelle, liste(0).Specialite)
        Assert.AreEqual("CARDIO", liste(1).Nom)
        Assert.AreEqual("Cabinet cardio", liste(1).[Structure])
        Assert.AreEqual(SpecialiteParcoursTestLibelle, liste(1).Specialite)
        ' Le libellé vient du singleton Table_specialite, qui ignore les spécialités inactives.
        Assert.AreEqual("", liste(2).Specialite)
    End Sub

    <TestMethod()> Public Sub GetListOfIntervenantNonOasisByPatient_ParcoursSansIntervenant_LeveUneErreur()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        CreerParcoursPatient(idPatient, 0)

        ' Comportement actuel : chaque parcours hors Oasis passe par GetRorById, qui
        ' lève pour un intervenant 0 ; FrmSousEpisode ne l'intercepte pas.
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetListOfIntervenantNonOasisByPatient(CInt(idPatient)))
        StringAssert.Contains(erreur.Message, "ROR inexistant")
    End Sub

    <TestMethod()> Public Sub GetAllIntervenantOasisByPatient_NeRendQueLesIntervenantsOasisActifs()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim medecin = CreerParcoursPatient(idPatient, RorMedecinReferentOasis, specialiteId:=SpecialiteMedecinReferentOasis,
                                           sousCategorieId:=SousCategorieParcoursMedecinReferent, intervenantOasis:=True)
        Dim ide = CreerParcoursPatient(idPatient, RorIdeOasis, specialiteId:=SpecialiteIdeOasis,
                                       sousCategorieId:=SousCategorieParcoursIde, intervenantOasis:=True)
        CreerParcoursPatient(idPatient, CreerRorParcours("LIBERAL"))
        Annuler(CreerParcoursPatient(idPatient, RorIdeOasis, specialiteId:=SpecialiteParcoursAutre,
                                     sousCategorieId:=SousCategorieParcoursSpecialiste, intervenantOasis:=True), idUtilisateur)
        CreerParcoursPatient(CreerPatient("AUTRE", "Patient"), RorIdeOasis, specialiteId:=SpecialiteIdeOasis,
                             sousCategorieId:=SousCategorieParcoursIde, intervenantOasis:=True)

        CollectionAssert.AreEqual(New Long() {ide, medecin}, IdsDe(dao.GetAllIntervenantOasisByPatient(CInt(idPatient))))
    End Sub

    <TestMethod()> Public Sub ExistIntervenantByPatientId_CompareCategorieSousCategorieEtSpecialite()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        CreerParcoursPatient(idPatient, CreerRorParcours("CARDIO"))
        Annuler(CreerParcoursPatient(autrePatient, CreerRorParcours("ANNULE")), idUtilisateur)
        Dim cat = CategorieParcoursSuivi
        Dim sousCat = SousCategorieParcoursSpecialiste

        Assert.IsTrue(dao.ExistIntervenantByPatientId(CInt(idPatient), cat, sousCat, SpecialiteParcoursTest))
        Assert.IsFalse(dao.ExistIntervenantByPatientId(CInt(idPatient), cat, sousCat, SpecialiteParcoursAutre), "autre spécialité")
        Assert.IsFalse(dao.ExistIntervenantByPatientId(CInt(idPatient), cat, 5, SpecialiteParcoursTest), "autre sous-catégorie")
        Assert.IsFalse(dao.ExistIntervenantByPatientId(CInt(idPatient), 1, sousCat, SpecialiteParcoursTest), "autre catégorie")
        Assert.IsFalse(dao.ExistIntervenantByPatientId(CInt(autrePatient), cat, sousCat, SpecialiteParcoursTest), "annulé")
    End Sub

    ' --- Parcours par défaut --------------------------------------------------------

    ''' <summary>Vérifie un parcours par défaut et sa demande de rendez-vous automatique.</summary>
    Private Shared Sub VerifierParcoursParDefaut(ligne As DataRow, idUtilisateur As Long, specialiteAttendue As Integer, sousCategorie As Integer,
                                                 rorId As Long, baseCalcul As String, fonctionAttendue As Integer)
        Assert.AreEqual(specialiteAttendue, CInt(ligne("oa_parcours_specialite")))
        Assert.AreEqual(CategorieParcoursSuivi, CInt(ligne("oa_parcours_categorie_id")))
        Assert.AreEqual(sousCategorie, CInt(ligne("oa_parcours_sous_categorie_id")))
        Assert.IsTrue(CBool(ligne("oa_parcours_intervenant_oasis")))
        Assert.AreEqual(rorId, CLng(ligne("oa_parcours_ror_id")))
        Assert.AreEqual("", CStr(ligne("oa_parcours_commentaire")))
        Assert.AreEqual(baseCalcul, CStr(ligne("oa_parcours_base")))
        Assert.AreEqual(1, CInt(ligne("oa_parcours_rythme")))
        Assert.IsFalse(CBool(ligne("oa_parcours_cacher")))
        Assert.IsFalse(CBool(ligne("oa_parcours_inactif")))
        Assert.AreEqual(idUtilisateur, CLng(ligne("oa_parcours_utilisateur_creation")))

        Dim idParcours = CLng(ligne("oa_parcours_id"))
        Assert.AreEqual(1, HistoriqueParcours(idParcours).Rows.Count)
        Dim taches = TachesDuParcours(idParcours)
        Assert.AreEqual(1, taches.Rows.Count, "première demande de rendez-vous")
        Dim demande = taches.Rows(0)
        Dim dansUnMois = Date.Now.AddDays(30)
        Assert.AreEqual("RDV_DEMANDE", CStr(demande("type")))
        Assert.AreEqual("RDV_DEMANDE", CStr(demande("nature")))
        Assert.AreEqual("EN_ATTENTE", CStr(demande("etat")))
        Assert.AreEqual("SOIN", CStr(demande("categorie")))
        Assert.AreEqual("ANNEEMOIS", CStr(demande("type_demande_rendez_vous")))
        Assert.AreEqual(New Date(dansUnMois.Year, dansUnMois.Month, 1), CDate(demande("date_rendez_vous")),
                        "premier du mois, trente jours plus tard")
        Assert.AreEqual(Date.Today, CDate(demande("date_traitement_demande_rendez_vous")).Date,
                        "délai de prise en charge (60 jours) déjà dépassé : aujourd'hui")
        ' Valeurs par défaut du DAO, IdUserAuto, FonctionEmetteurAutoId et
        ' DureeRendezVousParDefaut étant absents d'app.config (mêmes valeurs qu'en production).
        Assert.AreEqual(1L, CLng(demande("emetteur_user_id")))
        Assert.AreEqual(14L, CLng(demande("emetteur_fonction_id")))
        Assert.AreEqual(15, CInt(demande("duree_mn")))
        Assert.AreEqual(CLng(fonctionAttendue), CLng(demande("traite_fonction_id")))
        Assert.AreEqual(CLng(fonctionAttendue), CLng(demande("destinataire_fonction_id")))
    End Sub

    <TestMethod()> Public Sub CreateIntervenantOasisByPatient_CreeMedecinReferentEtIdeAvecLeurDemandeDeRendezVous()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Assert.IsTrue(dao.CreateIntervenantOasisByPatient(CInt(idPatient), Auteur(idUtilisateur)))

        Dim lignes = ParcoursDuPatient(idPatient)
        Assert.AreEqual(2, lignes.Rows.Count)
        VerifierParcoursParDefaut(lignes.Rows(0), idUtilisateur, SpecialiteMedecinReferentOasis, SousCategorieParcoursMedecinReferent,
                                  RorMedecinReferentOasis, "TOUS_LES_3_ANS", FonctionDao.EnumFonction.MEDECIN)
        VerifierParcoursParDefaut(lignes.Rows(1), idUtilisateur, SpecialiteIdeOasis, SousCategorieParcoursIde,
                                  RorIdeOasis, "PAR_AN", FonctionDao.EnumFonction.IDE)
    End Sub

    <TestMethod()> Public Sub CreateIntervenantOasisByPatient_DeuxFois_NeCreeRienDePlus()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        dao.CreateIntervenantOasisByPatient(CInt(idPatient), Auteur(idUtilisateur))

        Assert.IsTrue(dao.CreateIntervenantOasisByPatient(CInt(idPatient), Auteur(idUtilisateur), False))

        Assert.AreEqual(2, ParcoursDuPatient(idPatient).Rows.Count)
        Assert.AreEqual(2, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_tache WHERE patient_id = @p0", idPatient)))
    End Sub

    <TestMethod()> Public Sub CreateIntervenantOasisByPatient_MedecinReferentDejaPresent_NeCreeQueLIde()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim medecin = CreerParcoursPatient(idPatient, RorMedecinReferentOasis, specialiteId:=SpecialiteMedecinReferentOasis,
                                           sousCategorieId:=SousCategorieParcoursMedecinReferent, intervenantOasis:=True)

        dao.CreateIntervenantOasisByPatient(CInt(idPatient), Auteur(idUtilisateur))

        Dim lignes = ParcoursDuPatient(idPatient)
        Assert.AreEqual(2, lignes.Rows.Count)
        Assert.AreEqual(medecin, CLng(lignes.Rows(0)("oa_parcours_id")))
        Assert.AreEqual(0, TachesDuParcours(medecin).Rows.Count, "le médecin référent existant ne reçoit pas de demande")
        VerifierParcoursParDefaut(lignes.Rows(1), idUtilisateur, SpecialiteIdeOasis, SousCategorieParcoursIde,
                                  RorIdeOasis, "PAR_AN", FonctionDao.EnumFonction.IDE)
    End Sub

    <TestMethod()> Public Sub CreationPatient_AvecDateDEntree_CreeLeParcoursParDefaut()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim fiche = PatientDeTest("ENTRE", "Patient")
        fiche.PatientDateEntree = Date.Today

        ' Le chemin que JeuxPatient évite : PatientDao.CreationPatient appelle
        ' CreateIntervenantOasisByPatient dès que la date d'entrée est renseignée.
        Dim daoPatient As New PatientDao
        daoPatient.CreationPatient(fiche, Auteur(idUtilisateur))

        Dim idPatient = CLng(Scalaire("SELECT MAX(oa_patient_id) FROM oasis.oa_patient WHERE oa_patient_nir = @p0", fiche.PatientNir))
        Dim lignes = ParcoursDuPatient(idPatient)
        Assert.AreEqual(2, lignes.Rows.Count)
        VerifierParcoursParDefaut(lignes.Rows(0), idUtilisateur, SpecialiteMedecinReferentOasis, SousCategorieParcoursMedecinReferent,
                                  RorMedecinReferentOasis, "TOUS_LES_3_ANS", FonctionDao.EnumFonction.MEDECIN)
        VerifierParcoursParDefaut(lignes.Rows(1), idUtilisateur, SpecialiteIdeOasis, SousCategorieParcoursIde,
                                  RorIdeOasis, "PAR_AN", FonctionDao.EnumFonction.IDE)
    End Sub

    <TestMethod()> Public Sub ModificationPatient_EntreeSansSortie_CreeLeParcoursParDefaut()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim entre = CreerPatient("ENTRE", "Patient")
        Dim sorti = CreerPatient("SORTI", "Patient")
        Dim daoPatient As New PatientDao
        Dim ficheEntre = daoPatient.GetPatient(CInt(entre))
        ficheEntre.PatientDateEntree = Date.Today
        Dim ficheSorti = daoPatient.GetPatient(CInt(sorti))
        ficheSorti.PatientDateEntree = Date.Today.AddYears(-1)
        ficheSorti.PatientDateSortie = Date.Today

        daoPatient.ModificationPatient(ficheEntre, Auteur(idUtilisateur))
        daoPatient.ModificationPatient(ficheSorti, Auteur(idUtilisateur))

        Assert.AreEqual(2, ParcoursDuPatient(entre).Rows.Count)
        Assert.AreEqual(0, ParcoursDuPatient(sorti).Rows.Count, "patient sorti du dispositif")
    End Sub

    ' --- Autres lectures -----------------------------------------------------------

    <TestMethod()> Public Sub GetAllIntervenantSansRendezVous_RendDesLignesSansDateTriees()
        ExigerBaseOasis()
        Dim premier = CreerPatient()
        Dim second = CreerPatient("SECOND", "Patient")
        CreerParcoursPatient(second, CreerRorParcours("B"))
        CreerParcoursPatient(premier, CreerRorParcours("A"))
        CreerParcoursPatient(premier, RorIdeOasis, specialiteId:=SpecialiteIdeOasis, sousCategorieId:=SousCategorieParcoursIde,
                             intervenantOasis:=True)

        ' La vue v_intervenant_sans_rdv n'est pas dans le dépôt : seul le contrat de
        ' la requête est vérifié (DATE_RDV vide, tri par patient puis intervenant).
        Dim table = dao.GetAllIntervenantSansRendezVous()

        Assert.IsTrue(table.Columns.Contains("DATE_RDV"))
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) r("DATE_RDV") Is DBNull.Value))
        Dim cles = table.Rows.Cast(Of DataRow)().Select(Function(r) Tuple.Create(CLng(r("oa_parcours_patient_id")), CLng(r("oa_parcours_ror_id")))).ToList()
        CollectionAssert.AreEqual(cles.OrderBy(Function(c) c.Item1).ThenBy(Function(c) c.Item2).ToList(), cles)
    End Sub

    <TestMethod()> Public Sub CloneParcours_PuisCompare_DetecteChaqueDifference()
        ExigerBaseOasis()
        Dim idParcours = CreerParcoursPatient(CreerPatient(), CreerRorParcours("INTERVENANT"))
        Dim lu = dao.GetParcoursById(CInt(idParcours))

        Dim copie = dao.CloneParcours(lu)

        Assert.AreNotSame(lu, copie)
        Assert.IsTrue(dao.Compare(lu, copie))
        copie.DateCreation = lu.DateCreation.Date.AddHours(23)
        Assert.IsTrue(dao.Compare(lu, copie), "les dates sont comparées au jour près")
        copie.Rythme = lu.Rythme + 1
        Assert.IsFalse(dao.Compare(lu, copie))
        copie = dao.CloneParcours(lu)
        copie.Commentaire = "autre"
        Assert.IsFalse(dao.Compare(lu, copie))
        copie = dao.CloneParcours(lu)
        copie.Inactif = Not lu.Inactif
        Assert.IsFalse(dao.Compare(lu, copie))
    End Sub

    <TestMethod()> Public Sub GetAllEmailParcoursbyPatient_RendLesIntervenantsHorsOasisDesParcoursActifs()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim rorCardio = CreerRorParcours("MARTIN Cardio", email:="martin@exemple.fr")
        Dim rorDermato = CreerRorParcours("ALBERT Dermato", specialiteId:=SpecialiteParcoursAutre, email:="")
        Dim rorAnnule = CreerRorParcours("ZOE Annule", email:="zoe@exemple.fr")
        CreerParcoursPatient(idPatient, rorCardio, specialiteId:=SpecialiteParcoursTest)
        CreerParcoursPatient(idPatient, rorCardio, specialiteId:=SpecialiteParcoursTest, sousCategorieId:=5)
        CreerParcoursPatient(idPatient, rorDermato, specialiteId:=SpecialiteParcoursAutre)
        Annuler(CreerParcoursPatient(idPatient, rorAnnule, specialiteId:=SpecialiteParcoursTest), idUtilisateur)
        CreerParcoursPatient(idPatient, RorMedecinReferentOasis, specialiteId:=SpecialiteMedecinReferentOasis,
                             sousCategorieId:=SousCategorieParcoursMedecinReferent, intervenantOasis:=True)

        ' La jointure vers ans_annuaire_professionnel_sante_bal (identifiant_pp comparé
        ' à CAST(oa_ror_rpps AS CHAR), sans longueur) ne trouve rien : l'adresse vient
        ' de la fiche du ROR.
        Dim table = dao.GetAllEmailParcoursbyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {rorCardio, rorDermato},
                                  table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("oa_ror_id"))).ToArray(),
                                  "une ligne par intervenant et spécialité, triées par spécialité puis nom, sans l'Oasis ni l'annulé")
        Assert.AreEqual(SpecialiteParcoursTestLibelle, CStr(table.Rows(0)("oa_r_specialite_description")))
        Assert.AreEqual("MARTIN Cardio", CStr(table.Rows(0)("oa_ror_nom")))
        Assert.AreEqual("Intervenant", CStr(table.Rows(0)("oa_ror_type")))
        Assert.AreEqual("martin@exemple.fr", CStr(table.Rows(0)("email")))
        Assert.AreEqual(SpecialiteParcoursAutreLibelle, CStr(table.Rows(1)("oa_r_specialite_description")))
        Assert.AreEqual("", CStr(table.Rows(1)("email")))
    End Sub

End Class
