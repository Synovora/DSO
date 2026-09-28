Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' TacheDao contre la base : cycle de vie des tâches. Création (avec clôture de la
''' tâche parente dans la même transaction), prise en charge et remise en attente,
''' clôture, annulation, circuit des demandes d'avis (réponse, validation, relance,
''' complément), rendez-vous et demandes de rendez-vous automatiques, et les gardes
''' de concurrence (tâche déjà prise ou déjà close par quelqu'un d'autre).
'''
''' Tout est appelé par le client lourd : Compte.Client. Les lectures et listes sont
''' dans TacheDaoTest.
''' </summary>
<TestClass()> Public Class TacheDaoWorkflowTest
    Inherits TestIntegration

    Private ReadOnly dao As New TacheDao

    Private Const TacheAbsente As Long = 987654321

    Private Function Relire(idTache As Long) As Tache
        Return dao.GetTacheById(CInt(idTache), True)
    End Function

    Private Shared Function CompteDe(idUtilisateur As Long, Optional profilId As String = "IDE") As Utilisateur
        Return UtilisateurPourTache(idUtilisateur, profilId)
    End Function

    Private Shared Function EtatEnBase(idTache As Long) As String
        Return CStr(ColonneTache(idTache, "etat"))
    End Function

    Private Shared Sub VerifierCollision(appel As System.Action)
        Dim erreur = Assert.ThrowsException(Of Exception)(appel)
        StringAssert.Contains(erreur.Message, "Collision")
    End Sub

    Private Shared Sub VerifierMaintenant(valeur As Object, avant As Date)
        Assert.AreNotEqual(DBNull.Value, valeur)
        Dim quand = CDate(valeur)
        Assert.IsTrue(quand >= avant.AddSeconds(-1) AndAlso quand <= Date.Now.AddSeconds(1), "horodatage attendu à l'instant, lu " & quand)
    End Sub

    ''' <summary>Demande d'avis en attente pour cette fonction, par CreateTache. Renvoie l'id.</summary>
    Private Shared Function Avis(idPatient As Long, idEmetteur As Long, idFonction As Long,
                                 Optional idEpisode As Long = 0, Optional fEmettrice As Long = 0) As Long
        Return EnregistrerTache(TacheDeTest(idPatient, idEmetteur, traiteFonctionId:=idFonction, destinataireFonctionId:=idFonction,
                                            episodeId:=idEpisode, emetteurFonctionId:=fEmettrice))
    End Function

    ' ---------------------------------------------------------------------
    ' CreateTache et clôture de la tâche parente
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreateTache_RenvoieVraiEtAjouteUneLigne()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Assert.IsTrue(dao.CreateTache(TacheDeTest(idPatient, idEmetteur, traiteFonctionId:=CreerFonction("IT f1")), CompteDe(idEmetteur)))

        Assert.AreEqual(1, NombreDeTaches(idPatient))
        Assert.AreEqual("EN_ATTENTE", EtatEnBase(DerniereTache()))
    End Sub

    <TestMethod()> Public Sub CreateTache_AvecParent_TermineLeParentAuNomDeLAuteur()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fMedecin = CreerFonction("IT medecin")
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim parent = Avis(idPatient, idDemandeur, fMedecin, fEmettrice:=fIde)
        dao.AttribueTacheToUserLog(parent, CompteDe(idMedecin))
        Dim avant = Date.Now

        Dim fille = EnregistrerTache(TacheDeTest(idPatient, idMedecin, natureDeTache:=Tache.NatureTache.REPONSE, parentId:=parent,
                                                 emetteurFonctionId:=fMedecin, traiteFonctionId:=fIde, destinataireFonctionId:=fIde),
                                     auteurId:=idMedecin)

        Dim lueParent = Relire(parent)
        Assert.AreEqual("TERMINEE", lueParent.Etat)
        Assert.IsFalse(lueParent.Cloture, "une étape du circuit, pas sa fin")
        Assert.AreEqual(idMedecin, lueParent.TraiteUserId)
        VerifierMaintenant(ColonneTache(parent, "horodate_cloture"), avant)
        Dim lueFille = Relire(fille)
        Assert.AreEqual(parent, lueFille.ParentId)
        Assert.AreEqual("EN_ATTENTE", lueFille.Etat)
        Assert.AreEqual("REPONSE", lueFille.Nature)
        Assert.AreEqual(fIde, lueFille.TraiteFonctionId)
        Assert.AreEqual(0L, lueFille.TraiteUserId)
    End Sub

    <TestMethod()> Public Sub CreateTache_ParentPrisParUnAutre_RienNestCree()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idIntrus = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fMedecin = CreerFonction("IT medecin")
        Dim parent = Avis(idPatient, idDemandeur, fMedecin)
        dao.AttribueTacheToUserLog(parent, CompteDe(idMedecin))

        Dim reponse = TacheDeTest(idPatient, idIntrus, natureDeTache:=Tache.NatureTache.REPONSE, parentId:=parent, traiteFonctionId:=fMedecin)
        VerifierCollision(Sub() dao.CreateTache(reponse, CompteDe(idIntrus)))

        Assert.AreEqual(1, NombreDeTaches(idPatient), "la fille n'est pas insérée")
        Dim lueParent = Relire(parent)
        Assert.AreEqual("EN_COURS", lueParent.Etat)
        Assert.AreEqual(idMedecin, lueParent.TraiteUserId)
    End Sub

    <TestMethod()> Public Sub CreateTache_ParentDejaTermine_RienNestCree()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fMedecin = CreerFonction("IT medecin")
        Dim parent = Avis(idPatient, idDemandeur, fMedecin)
        dao.AttribueTacheToUserLog(parent, CompteDe(idMedecin))
        dao.ClotureTache(parent, False, CompteDe(idMedecin))

        ' Deux réponses à la même demande : la seconde est refusée.
        Dim reponse = TacheDeTest(idPatient, idMedecin, natureDeTache:=Tache.NatureTache.REPONSE, parentId:=parent, traiteFonctionId:=fMedecin)
        VerifierCollision(Sub() dao.CreateTache(reponse, CompteDe(idMedecin)))

        Assert.AreEqual(1, NombreDeTaches(idPatient))
    End Sub

    <TestMethod()> Public Sub CreateTache_ParentLibre_EstTermineEtAttribueALAuteur()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idAutre = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fMedecin = CreerFonction("IT medecin")
        Dim parent = Avis(idPatient, idDemandeur, fMedecin)

        ' Comportement actuel : un parent que personne n'a pris (traite_user_id NULL)
        ' est clos par n'importe qui, qui en devient le traitant.
        EnregistrerTache(TacheDeTest(idPatient, idAutre, natureDeTache:=Tache.NatureTache.REPONSE, parentId:=parent, traiteFonctionId:=fMedecin),
                         auteurId:=idAutre)

        Dim lueParent = Relire(parent)
        Assert.AreEqual("TERMINEE", lueParent.Etat)
        Assert.AreEqual(idAutre, lueParent.TraiteUserId)
    End Sub

    <TestMethod()> Public Sub CreateTache_InsertionEnEchec_LaClotureDuParentEstAnnulee()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fMedecin = CreerFonction("IT medecin")
        Dim parent = Avis(idPatient, idDemandeur, fMedecin)
        dao.AttribueTacheToUserLog(parent, CompteDe(idMedecin))
        ' Commentaire à Nothing : AddWithValue envoie un paramètre sans valeur, l'INSERT
        ' échoue après la clôture du parent, dans la même transaction.
        Dim reponse = TacheDeTest(idPatient, idMedecin, natureDeTache:=Tache.NatureTache.REPONSE, parentId:=parent, traiteFonctionId:=fMedecin)
        reponse.EmetteurCommentaire = Nothing

        Assert.ThrowsException(Of Exception)(Sub() dao.CreateTache(reponse, CompteDe(idMedecin)))

        Assert.AreEqual(1, NombreDeTaches(idPatient))
        Assert.AreEqual("EN_COURS", EtatEnBase(parent), "la clôture du parent est défaite")
        Assert.AreEqual(DBNull.Value, ColonneTache(parent, "horodate_cloture"))
    End Sub

    ' ---------------------------------------------------------------------
    ' Garde anti-doublon des rendez-vous (CreateRendezVous(tache, userLog))
    ' ---------------------------------------------------------------------

    Private Function RendezVousBean(idPatient As Long, idEmetteur As Long, idParcours As Long,
                                    Optional typeDeTache As Tache.TypeTache = Tache.TypeTache.RDV) As Tache
        Return TacheDeTest(idPatient, idEmetteur, typeDeTache:=typeDeTache, parcoursId:=idParcours,
                           traiteFonctionId:=CreerFonction("IT ide " & NouveauLogin()), dateRendezVous:=New Date(2030, 2, 1, 9, 0, 0))
    End Function

    <TestMethod()> Public Sub RendezVousBean_SansRendezVousOuvert_EstInsere()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)

        Assert.IsTrue(dao.CreateRendezVous(RendezVousBean(idPatient, idEmetteur, idParcours), CompteDe(idEmetteur)))

        Assert.AreEqual(1, NombreDeTaches(idPatient))
        Assert.AreEqual(New Date(2030, 2, 1, 9, 0, 0), Relire(DerniereTache()).DateRendezVous)
    End Sub

    <TestMethod()> Public Sub RendezVousBean_RendezVousOuvertSurLeParcours_RienNestInsere()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idEmetteur)
        For Each bloquant In {Tache.TypeTache.RDV, Tache.TypeTache.RDV_SPECIALISTE, Tache.TypeTache.RDV_DEMANDE}
            Dim idPatient = CreerPatient()
            Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)
            Dim existant = EnregistrerTache(RendezVousBean(idPatient, idEmetteur, idParcours, bloquant))
            If bloquant = Tache.TypeTache.RDV_DEMANDE Then dao.AttribueTacheToUserLog(existant, auteur)

            ' Comportement actuel : rien n'est inséré, et la méthode renvoie tout de même
            ' True ; l'appelant ne peut pas savoir que le rendez-vous n'existe pas.
            Assert.IsTrue(dao.CreateRendezVous(RendezVousBean(idPatient, idEmetteur, idParcours), auteur), bloquant.ToString())
            Assert.AreEqual(1, NombreDeTaches(idPatient), "bloqué par " & bloquant.ToString())
        Next
    End Sub

    <TestMethod()> Public Sub RendezVousBean_RendezVousClosOuAilleurs_NeBloquePas()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idEmetteur)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim idParcours = CreerParcoursPourTache(idPatient, idEmetteur)
        Dim idAutreParcours = CreerParcoursPourTache(idPatient, idEmetteur, specialiteId:=SpecialiteTacheNonOasis)
        dao.ClotureTache(EnregistrerTache(RendezVousBean(idPatient, idEmetteur, idParcours)), True, auteur)
        dao.AnnulationTache(EnregistrerTache(RendezVousBean(idPatient, idEmetteur, idParcours)), auteur)
        EnregistrerTache(RendezVousBean(idPatient, idEmetteur, idParcours, Tache.TypeTache.RDV_MISSION))
        EnregistrerTache(RendezVousBean(idPatient, idEmetteur, idAutreParcours))
        EnregistrerTache(RendezVousBean(idAutrePatient, idEmetteur, idParcours))
        Dim avant = NombreDeTaches(idPatient)

        dao.CreateRendezVous(RendezVousBean(idPatient, idEmetteur, idParcours), auteur)

        Assert.AreEqual(avant + 1, NombreDeTaches(idPatient))
    End Sub

    <TestMethod()> Public Sub RendezVousBean_SansParcours_NeBloqueJamais()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim auteur = CompteDe(idEmetteur)

        ' Comportement actuel : parcours 0 est écrit NULL, et « parcours_id = NULL »
        ' n'est jamais vrai ; deux rendez-vous ouverts sans parcours passent.
        dao.CreateRendezVous(RendezVousBean(idPatient, idEmetteur, 0), auteur)
        dao.CreateRendezVous(RendezVousBean(idPatient, idEmetteur, 0), auteur)

        Assert.AreEqual(2, NombreDeTaches(idPatient))
    End Sub

    ' ---------------------------------------------------------------------
    ' Prise en charge et remise en attente
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub Attribue_PasseEnCoursAvecTraitantEtHorodate()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))
        Dim avant = Date.Now

        Assert.IsTrue(dao.AttribueTacheToUserLog(idTache, CompteDe(idTraitant)))

        Dim lue = Relire(idTache)
        Assert.AreEqual("EN_COURS", lue.Etat)
        Assert.AreEqual(idTraitant, lue.TraiteUserId)
        VerifierMaintenant(ColonneTache(idTache, "horodate_attrib"), avant)
        Assert.AreEqual(Date.MinValue, lue.HorodatageCloture)
    End Sub

    <TestMethod()> Public Sub Attribue_TacheDejaPriseParUnAutrePoste_Collision()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPremier = CreerUtilisateur(avecCle:=False)
        Dim idSecond = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))
        ' Deux postes, chacun son DAO : le premier arrivé l'emporte.
        Dim postePremier As New TacheDao
        Dim posteSecond As New TacheDao
        postePremier.AttribueTacheToUserLog(idTache, CompteDe(idPremier))
        Dim prise = ColonneTache(idTache, "horodate_attrib")

        VerifierCollision(Sub() posteSecond.AttribueTacheToUserLog(idTache, CompteDe(idSecond)))
        ' Le même utilisateur ne la reprend pas non plus.
        VerifierCollision(Sub() postePremier.AttribueTacheToUserLog(idTache, CompteDe(idPremier)))

        Dim lue = Relire(idTache)
        Assert.AreEqual(idPremier, lue.TraiteUserId)
        Assert.AreEqual(CDate(prise), CDate(ColonneTache(idTache, "horodate_attrib")))
    End Sub

    <TestMethod()> Public Sub Attribue_TacheCloseAnnuleeOuAbsente_Collision()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idEmetteur)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim terminee = Avis(idPatient, idEmetteur, f1)
        dao.ClotureTache(terminee, True, auteur)
        Dim annulee = Avis(idPatient, idEmetteur, f1)
        dao.AnnulationTache(annulee, auteur)

        VerifierCollision(Sub() dao.AttribueTacheToUserLog(terminee, auteur))
        VerifierCollision(Sub() dao.AttribueTacheToUserLog(annulee, auteur))
        VerifierCollision(Sub() dao.AttribueTacheToUserLog(TacheAbsente, auteur))
        Assert.AreEqual("TERMINEE", EtatEnBase(terminee))
        Assert.AreEqual("ANNULEE", EtatEnBase(annulee))
    End Sub

    <TestMethod()> Public Sub Attribue_TacheCreeeAvecUnTraitant_Collision()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idDesigne = CreerUtilisateur(avecCle:=False)
        Dim idTache = EnregistrerTache(TacheDeTest(CreerPatient(), idEmetteur, traiteFonctionId:=CreerFonction("IT f1"), traiteUserId:=idDesigne))

        ' Comportement actuel : une tâche en attente qui porte déjà un traitant ne peut
        ' être prise par personne, pas même par ce traitant.
        VerifierCollision(Sub() dao.AttribueTacheToUserLog(idTache, CompteDe(idDesigne)))
        Assert.AreEqual("EN_ATTENTE", EtatEnBase(idTache))
    End Sub

    <TestMethod()> Public Sub Desattribue_RemetEnAttenteSansTraitant()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim f1 = CreerFonction("IT f1")
        Dim idTache = Avis(CreerPatient(), idEmetteur, f1)
        dao.AttribueTacheToUserLog(idTache, CompteDe(idTraitant))

        ' La méthode ne reçoit aucun utilisateur : le droit de rendre la tâche est
        ' vérifié par l'écran (Tache.IsDesattribuable), pas par la base.
        Assert.IsTrue(dao.DesattribueTache(idTache))

        Assert.AreEqual("EN_ATTENTE", EtatEnBase(idTache))
        Assert.AreEqual(DBNull.Value, ColonneTache(idTache, "traite_user_id"))
        Assert.AreEqual(DBNull.Value, ColonneTache(idTache, "horodate_attrib"))
        CollectionAssert.Contains(IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe())), idTache)
    End Sub

    <TestMethod()> Public Sub Desattribue_TacheNonPriseOuClose_RenvoieFaux()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim libre = Avis(idPatient, idEmetteur, f1)
        Dim terminee = Avis(idPatient, idEmetteur, f1)
        dao.AttribueTacheToUserLog(terminee, CompteDe(idEmetteur))
        dao.ClotureTache(terminee, True, CompteDe(idEmetteur))

        ' Pas d'exception : l'échec est rendu par la valeur de retour.
        Assert.IsFalse(dao.DesattribueTache(libre))
        Assert.IsFalse(dao.DesattribueTache(terminee))
        Assert.IsFalse(dao.DesattribueTache(TacheAbsente))
        Assert.AreEqual("EN_ATTENTE", EtatEnBase(libre))
        Assert.AreEqual("TERMINEE", EtatEnBase(terminee))
    End Sub

    <TestMethod()> Public Sub Transfert_UneTacheRenduePasseAUnAutreUtilisateur()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idPremier = CreerUtilisateur(avecCle:=False)
        Dim idSecond = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))
        dao.AttribueTacheToUserLog(idTache, CompteDe(idPremier))
        dao.DesattribueTache(idTache)

        dao.AttribueTacheToUserLog(idTache, CompteDe(idSecond))

        Assert.AreEqual(idSecond, Relire(idTache).TraiteUserId)
        ' L'ancien traitant ne peut plus la clore.
        VerifierCollision(Sub() dao.ClotureTache(idTache, True, CompteDe(idPremier)))
        Assert.IsTrue(dao.ClotureTache(idTache, True, CompteDe(idSecond)))
        Assert.AreEqual("TERMINEE", EtatEnBase(idTache))
    End Sub

    ' ---------------------------------------------------------------------
    ' Clôture
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub Cloture_TermineeAvecOuSansIndicateurDeCloture()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim traitant = CompteDe(idTraitant)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim close = Avis(idPatient, idEmetteur, f1)
        dao.AttribueTacheToUserLog(close, traitant)
        Dim etape = Avis(idPatient, idEmetteur, f1)
        dao.AttribueTacheToUserLog(etape, traitant)
        Dim avant = Date.Now

        Assert.IsTrue(dao.ClotureTache(close, True, traitant))
        Assert.IsTrue(dao.ClotureTache(etape, False, traitant))

        Dim lue = Relire(close)
        Assert.AreEqual("TERMINEE", lue.Etat)
        Assert.IsTrue(lue.Cloture)
        Assert.AreEqual(idTraitant, lue.TraiteUserId)
        VerifierMaintenant(ColonneTache(close, "horodate_cloture"), avant)
        Assert.AreEqual("TERMINEE", EtatEnBase(etape))
        Assert.IsFalse(Relire(etape).Cloture)
    End Sub

    <TestMethod()> Public Sub Cloture_ParUnAutreQueLeTraitant_Collision()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))
        dao.AttribueTacheToUserLog(idTache, CompteDe(idTraitant))

        VerifierCollision(Sub() dao.ClotureTache(idTache, True, CompteDe(idEmetteur)))

        Dim lue = Relire(idTache)
        Assert.AreEqual("EN_COURS", lue.Etat)
        Assert.AreEqual(idTraitant, lue.TraiteUserId)
        Assert.AreEqual(DBNull.Value, ColonneTache(idTache, "horodate_cloture"))
    End Sub

    <TestMethod()> Public Sub Cloture_TacheLibre_TermineeAuNomDeCeluiQuiClot()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))

        dao.ClotureTache(idTache, True, CompteDe(idEmetteur))

        Dim lue = Relire(idTache)
        Assert.AreEqual("TERMINEE", lue.Etat)
        Assert.AreEqual(idEmetteur, lue.TraiteUserId)
        ' horodate_attrib n'est pas posé par la clôture.
        Assert.AreEqual(DBNull.Value, ColonneTache(idTache, "horodate_attrib"))
    End Sub

    <TestMethod()> Public Sub Cloture_DejaTermineeOuAbsente_Collision()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idEmetteur)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))
        dao.ClotureTache(idTache, False, auteur)
        Dim premiere = CDate(ColonneTache(idTache, "horodate_cloture"))

        VerifierCollision(Sub() dao.ClotureTache(idTache, True, auteur))
        VerifierCollision(Sub() dao.ClotureTache(TacheAbsente, True, auteur))

        Assert.IsFalse(Relire(idTache).Cloture, "la seconde clôture n'a rien écrit")
        Assert.AreEqual(premiere, CDate(ColonneTache(idTache, "horodate_cloture")))
    End Sub

    ' ---------------------------------------------------------------------
    ' Annulation
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub Annulation_ParIdOuParBean_AnnuleeEtClose()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idEmetteur)
        Dim idPatient = CreerPatient()
        Dim f1 = CreerFonction("IT f1")
        Dim parId = Avis(idPatient, idEmetteur, f1)
        Dim parBean = Avis(idPatient, idEmetteur, f1)
        Dim avant = Date.Now

        Assert.IsTrue(dao.AnnulationTache(parId, auteur))
        Assert.IsTrue(dao.AnnulationTache(Relire(parBean), auteur))

        For Each idTache In {parId, parBean}
            Dim lue = Relire(idTache)
            Assert.AreEqual("ANNULEE", lue.Etat)
            Assert.IsTrue(lue.Cloture)
            Assert.AreEqual(idEmetteur, lue.TraiteUserId)
            VerifierMaintenant(ColonneTache(idTache, "horodate_cloture"), avant)
        Next
        Assert.AreEqual(0, dao.GetAllTacheATraiter(FonctionsTache(f1), FiltreTacheDe()).Rows.Count)
    End Sub

    <TestMethod()> Public Sub Annulation_TachePriseParUnAutre_Collision()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTraitant = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))
        dao.AttribueTacheToUserLog(idTache, CompteDe(idTraitant))

        ' L'émetteur ne peut plus annuler une tâche prise en charge.
        VerifierCollision(Sub() dao.AnnulationTache(idTache, CompteDe(idEmetteur)))

        Assert.AreEqual("EN_COURS", EtatEnBase(idTache))
        Assert.IsTrue(dao.AnnulationTache(idTache, CompteDe(idTraitant)), "le traitant, lui, le peut")
    End Sub

    <TestMethod()> Public Sub Annulation_DejaAnnulee_Collision()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))
        dao.AnnulationTache(idTache, CompteDe(idEmetteur))

        VerifierCollision(Sub() dao.AnnulationTache(idTache, CompteDe(idEmetteur)))
    End Sub

    <TestMethod()> Public Sub Annulation_TacheTerminee_PasseAnnulee()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idEmetteur)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))
        dao.ClotureTache(idTache, True, auteur)

        ' Comportement actuel : ClosTache n'exclut que l'état visé ; une tâche
        ' terminée peut encore être annulée par son traitant.
        dao.AnnulationTache(idTache, auteur)

        Assert.AreEqual("ANNULEE", EtatEnBase(idTache))
    End Sub

    ' ---------------------------------------------------------------------
    ' ClosTache sur une connexion fournie par l'appelant
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ClosTache_DansUneTransactionAnnulee_RienNeChange()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))

        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using transaction = connexion.BeginTransaction()
                dao.ClosTache(connexion, idTache, Tache.EtatTache.TERMINEE, True, CompteDe(idEmetteur), transaction)
                transaction.Rollback()
            End Using
        End Using

        Assert.AreEqual("EN_ATTENTE", EtatEnBase(idTache))
        Assert.AreEqual(DBNull.Value, ColonneTache(idTache, "traite_user_id"))
    End Sub

    <TestMethod()> Public Sub ClosTache_SansTransaction_EcritPuisRefuseLeMemeEtat()
        Dim idEmetteur = CreerUtilisateur(avecCle:=False)
        Dim idTache = Avis(CreerPatient(), idEmetteur, CreerFonction("IT f1"))

        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            dao.ClosTache(connexion, idTache, Tache.EtatTache.ANNULEE, False, CompteDe(idEmetteur))
            VerifierCollision(Sub() dao.ClosTache(connexion, idTache, Tache.EtatTache.ANNULEE, False, CompteDe(idEmetteur)))
        End Using

        Assert.AreEqual("ANNULEE", EtatEnBase(idTache))
        Assert.IsFalse(Relire(idTache).Cloture)
    End Sub

    ' ---------------------------------------------------------------------
    ' Demandes d'avis et circuit complet
    ' ---------------------------------------------------------------------

    ''' <summary>Demande d'avis comme la crée RadFWkfDemandeAvis, par CreationDemandeAvis. Renvoie l'id.</summary>
    Private Function DemanderAvis(idPatient As Long, idDemandeur As Long, idEpisode As Long, fEmettrice As Long, fDestinataire As Long,
                                  Optional commentaire As String = "Avis sur la plaie") As Long
        Dim nouvelle = TacheDeTest(idPatient, idDemandeur, episodeId:=idEpisode, emetteurFonctionId:=fEmettrice,
                                   traiteFonctionId:=fDestinataire, destinataireFonctionId:=fDestinataire,
                                   priorite:=Tache.EnumPriorite.MOYENNE, commentaire:=commentaire)
        Assert.IsTrue(dao.CreationDemandeAvis(nouvelle, CompteDe(idDemandeur)))
        Return DerniereTache()
    End Function

    ''' <summary>
    ''' Étape suivante du circuit, comme RadFWkfDemandeAvis : fille de la tâche lue,
    ''' fonctions émettrice et destinataire inversées, créée par CreateTache.
    ''' </summary>
    Private Function Repondre(idParent As Long, idAuteur As Long, natureDeTache As Tache.NatureTache) As Long
        Dim lueParent = Relire(idParent)
        Dim suivante = TacheDeTest(lueParent.PatientId, idAuteur, natureDeTache:=natureDeTache, episodeId:=lueParent.EpisodeId,
                                   emetteurFonctionId:=lueParent.DestinataireFonctionId,
                                   traiteFonctionId:=lueParent.EmetteurFonctionId,
                                   destinataireFonctionId:=lueParent.EmetteurFonctionId,
                                   parentId:=idParent, priorite:=lueParent.Priorite)
        Assert.IsTrue(dao.CreateTache(suivante, CompteDe(idAuteur)))
        Return DerniereTache()
    End Function

    <TestMethod()> Public Sub DemandeAvis_EnregistreLaDemande()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idDemandeur)
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim fMedecin = CreerFonction("IT medecin")

        Dim idTache = DemanderAvis(idPatient, idDemandeur, idEpisode, fIde, fMedecin)

        Dim lue = Relire(idTache)
        Assert.AreEqual(idPatient, lue.PatientId)
        Assert.AreEqual(idEpisode, lue.EpisodeId)
        Assert.AreEqual(idDemandeur, lue.EmetteurUserId)
        Assert.AreEqual(fIde, lue.EmetteurFonctionId)
        Assert.AreEqual(fMedecin, lue.TraiteFonctionId)
        Assert.AreEqual(fMedecin, lue.DestinataireFonctionId)
        Assert.AreEqual("AVIS_EPISODE", lue.Type)
        Assert.AreEqual("DEMANDE", lue.Nature)
        Assert.AreEqual("EN_ATTENTE", lue.Etat)
        Assert.AreEqual(CInt(Tache.EnumPriorite.MOYENNE), lue.Priorite)
        Assert.AreEqual("Avis sur la plaie", lue.EmetteurCommentaire)
        Assert.AreEqual(0L, lue.ParentId)
        ' Cet INSERT ne porte pas la colonne date_traitement_demande_rendez_vous.
        Assert.AreEqual(DBNull.Value, ColonneTache(idTache, "date_traitement_demande_rendez_vous"))
    End Sub

    <TestMethod()> Public Sub DemandeAvis_DemandeDejaOuverteSurLEpisode_Collision()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idDemandeur)
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim fMedecin = CreerFonction("IT medecin")
        Dim premiere = DemanderAvis(idPatient, idDemandeur, idEpisode, fIde, fMedecin)
        Dim seconde = TacheDeTest(idPatient, idDemandeur, episodeId:=idEpisode, traiteFonctionId:=fMedecin)

        VerifierCollision(Sub() dao.CreationDemandeAvis(seconde, CompteDe(idDemandeur)))
        ' Prise en charge : toujours ouverte.
        dao.AttribueTacheToUserLog(premiere, CompteDe(idMedecin))
        VerifierCollision(Sub() dao.CreationDemandeAvis(seconde, CompteDe(idDemandeur)))

        Assert.AreEqual(1, NombreDeTaches(idPatient))
    End Sub

    <TestMethod()> Public Sub DemandeAvis_AutreEpisodeOuDemandeClose_Acceptee()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idDemandeur)
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim fMedecin = CreerFonction("IT medecin")
        Dim premiere = DemanderAvis(idPatient, idDemandeur, idEpisode, fIde, fMedecin)
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        DemanderAvis(idAutrePatient, idDemandeur, CreerEpisode(idAutrePatient, idDemandeur), fIde, fMedecin)
        dao.AnnulationTache(premiere, CompteDe(idDemandeur))

        DemanderAvis(idPatient, idDemandeur, idEpisode, fIde, fMedecin, "Seconde demande")

        Assert.AreEqual(2, NombreDeTaches(idPatient))
    End Sub

    <TestMethod()> Public Sub DemandeAvis_ReponseEnAttente_BloqueUneNouvelleDemande()
        Dim idDemandeur = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idDemandeur)
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim fMedecin = CreerFonction("IT medecin")
        Dim demandeAvis = DemanderAvis(idPatient, idDemandeur, idEpisode, fIde, fMedecin)
        dao.AttribueTacheToUserLog(demandeAvis, CompteDe(idMedecin))
        Repondre(demandeAvis, idMedecin, Tache.NatureTache.REPONSE)

        ' Comportement actuel : la réponse est elle aussi de type AVIS_EPISODE et en
        ' attente ; tant que le demandeur ne l'a pas validée, aucune autre demande
        ' d'avis n'est possible sur l'épisode, vers quelque fonction que ce soit.
        Dim autre = TacheDeTest(idPatient, idDemandeur, episodeId:=idEpisode, traiteFonctionId:=CreerFonction("IT autre"))
        VerifierCollision(Sub() dao.CreationDemandeAvis(autre, CompteDe(idDemandeur)))
    End Sub

    <TestMethod()> Public Sub CircuitAvis_DemandeReponseValidation()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idIde)
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim fMedecin = CreerFonction("IT medecin")

        ' L'IDE demande, le médecin prend la demande et répond.
        Dim demandeAvis = DemanderAvis(idPatient, idIde, idEpisode, fIde, fMedecin)
        CollectionAssert.AreEqual({demandeAvis}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(fMedecin), FiltreTacheDe())))
        dao.AttribueTacheToUserLog(demandeAvis, CompteDe(idMedecin))
        Dim reponse = Repondre(demandeAvis, idMedecin, Tache.NatureTache.REPONSE)

        ' La réponse revient à la fonction de l'IDE, qui la prend et valide.
        Assert.AreEqual(0, dao.GetAllTacheATraiter(FonctionsTache(fMedecin), FiltreTacheDe()).Rows.Count)
        CollectionAssert.AreEqual({reponse}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(fIde), FiltreTacheDe())))
        dao.AttribueTacheToUserLog(reponse, CompteDe(idIde))
        dao.ClotureTache(reponse, True, CompteDe(idIde))

        Dim histo = dao.GetWorkflowHistoByEpisode(idEpisode)
        CollectionAssert.AreEqual({demandeAvis, reponse}, IdsTaches(histo))
        Assert.AreEqual("TERMINEE", CStr(LigneTache(histo, demandeAvis)("etat")))
        Assert.IsFalse(CBool(LigneTache(histo, demandeAvis)("cloture")))
        Assert.AreEqual(idMedecin, CLng(LigneTache(histo, demandeAvis)("traite_user_id")))
        Assert.AreEqual("TERMINEE", CStr(LigneTache(histo, reponse)("etat")))
        Assert.IsTrue(CBool(LigneTache(histo, reponse)("cloture")), "fin du circuit")
        Assert.AreEqual(idIde, CLng(LigneTache(histo, reponse)("traite_user_id")))
        Assert.AreEqual(fMedecin, Relire(reponse).EmetteurFonctionId)
        Assert.AreEqual(fIde, Relire(reponse).DestinataireFonctionId)
        ' Circuit terminé : une nouvelle demande est de nouveau possible.
        DemanderAvis(idPatient, idIde, idEpisode, fIde, fMedecin, "Nouvelle demande")
    End Sub

    <TestMethod()> Public Sub CircuitAvis_RelanceApresReponse()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idIde)
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim fMedecin = CreerFonction("IT medecin")
        Dim demandeAvis = DemanderAvis(idPatient, idIde, idEpisode, fIde, fMedecin)
        dao.AttribueTacheToUserLog(demandeAvis, CompteDe(idMedecin))
        Dim reponse = Repondre(demandeAvis, idMedecin, Tache.NatureTache.REPONSE)
        dao.AttribueTacheToUserLog(reponse, CompteDe(idIde))

        ' Réponse insuffisante : l'IDE relance le médecin par une nouvelle demande.
        Dim relance = Repondre(reponse, idIde, Tache.NatureTache.DEMANDE)

        Assert.AreEqual("TERMINEE", EtatEnBase(reponse))
        Assert.IsFalse(Relire(reponse).Cloture)
        Dim lueRelance = Relire(relance)
        Assert.AreEqual(reponse, lueRelance.ParentId)
        Assert.AreEqual("DEMANDE", lueRelance.Nature)
        Assert.AreEqual("EN_ATTENTE", lueRelance.Etat)
        Assert.AreEqual(fMedecin, lueRelance.TraiteFonctionId)
        CollectionAssert.AreEqual({relance}, IdsTaches(dao.GetAllTacheATraiter(FonctionsTache(fMedecin), FiltreTacheDe())))
        CollectionAssert.AreEqual({demandeAvis, reponse, relance}, IdsTaches(dao.GetWorkflowHistoByEpisode(idEpisode)))
    End Sub

    <TestMethod()> Public Sub CircuitAvis_DemandeDeComplementPuisReponse()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idIde)
        Dim fIde = CreerFonction("IT ide", typeFonction:="PARAMEDICAL")
        Dim fMedecin = CreerFonction("IT medecin")
        Dim demandeAvis = DemanderAvis(idPatient, idIde, idEpisode, fIde, fMedecin)
        dao.AttribueTacheToUserLog(demandeAvis, CompteDe(idMedecin))

        Dim complement = Repondre(demandeAvis, idMedecin, Tache.NatureTache.COMPLEMENT)
        dao.AttribueTacheToUserLog(complement, CompteDe(idIde))
        Dim precision = Repondre(complement, idIde, Tache.NatureTache.DEMANDE)

        Assert.AreEqual("COMPLEMENT", Relire(complement).Nature)
        Assert.AreEqual("TERMINEE", EtatEnBase(complement))
        Assert.AreEqual(fMedecin, Relire(precision).TraiteFonctionId)
        CollectionAssert.AreEqual({demandeAvis, complement, precision}, IdsTaches(dao.GetWorkflowHistoByEpisode(idEpisode)))
    End Sub

    ' ---------------------------------------------------------------------
    ' CreateRendezVous(patient, parcours, ...)
    ' ---------------------------------------------------------------------

    Private Shared Function PatientPourTache(idPatient As Long, Optional idUnite As Long = 0, Optional idSite As Long = 0) As Patient
        Return New Patient With {.PatientId = CInt(idPatient), .PatientUniteSanitaireId = CInt(idUnite), .PatientSiteId = CInt(idSite)}
    End Function

    <TestMethod()> Public Sub CreateRendezVous_SansParent_RendezVousEnAttente()
        Dim idMedecin = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idUnite = CreerUniteSanitaire("Unite RDV")
        Dim idSite = CreerSite("Site RDV", idUnite)
        Dim idParcours = CreerParcoursPourTache(idPatient, idMedecin, sousCategorieId:=EnumSousCategoriePPS.IDE)
        Dim quand As New Date(2030, 3, 4, 10, 30, 0)
        Dim avant = Date.Now

        dao.CreateRendezVous(PatientPourTache(idPatient, idUnite, idSite), LireParcoursPourTache(idParcours), Tache.TypeTache.RDV,
                             quand, 20, "Pansement", CompteDe(idMedecin, "MEDECIN"))

        Dim lue = Relire(DerniereTache())
        Assert.AreEqual(idPatient, lue.PatientId)
        Assert.AreEqual(idParcours, lue.ParcoursId)
        Assert.AreEqual(idUnite, lue.UniteSanitaireId)
        Assert.AreEqual(idSite, lue.SiteId)
        Assert.AreEqual(0L, lue.ParentId)
        Assert.AreEqual(0L, lue.EpisodeId)
        Assert.AreEqual(idMedecin, lue.EmetteurUserId)
        Assert.AreEqual(CLng(FonctionDao.EnumFonction.MEDECIN), lue.EmetteurFonctionId)
        Assert.AreEqual(CLng(FonctionDao.EnumFonction.IDE), lue.DestinataireFonctionId)
        Assert.AreEqual(CLng(FonctionDao.EnumFonction.IDE), lue.TraiteFonctionId)
        Assert.AreEqual(0L, lue.TraiteUserId)
        Assert.AreEqual(CInt(Tache.EnumPriorite.BASSE), lue.Priorite)
        Assert.AreEqual(20, lue.OrdreAffichage)
        Assert.AreEqual("SOIN", lue.Categorie)
        Assert.AreEqual("RDV", lue.Type)
        Assert.AreEqual("RDV", lue.Nature)
        Assert.AreEqual(20, lue.Duree)
        Assert.AreEqual("Pansement", lue.EmetteurCommentaire)
        Assert.AreEqual("EN_ATTENTE", lue.Etat)
        Assert.AreEqual(quand, lue.DateRendezVous)
        Assert.AreEqual("", lue.TypedemandeRendezVous)
        VerifierMaintenant(ColonneTache(lue.Id, "horodate_creation"), avant)
    End Sub

    <TestMethod()> Public Sub CreateRendezVous_DepuisUneDemande_TermineLaDemande()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
        Dim demandeRdv = EnregistrerTache(TacheDeTest(idPatient, idIde, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, parcoursId:=idParcours,
                                                      traiteFonctionId:=CreerFonction("IT ide"), dateRendezVous:=New Date(2030, 3, 1),
                                                      typeDemandeRendezVous:="ANNEEMOIS"))
        dao.AttribueTacheToUserLog(demandeRdv, CompteDe(idIde))

        dao.CreateRendezVous(PatientPourTache(idPatient), LireParcoursPourTache(idParcours), Tache.TypeTache.RDV,
                             New Date(2030, 3, 12, 9, 0, 0), 15, "Fixe", CompteDe(idIde), Relire(demandeRdv))

        Dim lueDemande = Relire(demandeRdv)
        Assert.AreEqual("TERMINEE", lueDemande.Etat)
        Assert.IsFalse(lueDemande.Cloture)
        Dim rdv = Relire(DerniereTache())
        Assert.AreEqual(demandeRdv, rdv.ParentId)
        Assert.AreEqual("RDV", rdv.Type)
        Assert.AreEqual("EN_ATTENTE", rdv.Etat)
        Assert.AreEqual(CLng(FonctionDao.EnumFonction.IDE), rdv.EmetteurFonctionId)
    End Sub

    <TestMethod()> Public Sub CreateRendezVous_MissionDepuisUneDemandeDeMission()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
        Dim demandeMission = EnregistrerTache(TacheDeTest(idPatient, idIde, typeDeTache:=Tache.TypeTache.MISSION_DEMANDE,
                                                          parcoursId:=idParcours, traiteFonctionId:=CreerFonction("IT ide")))
        dao.AttribueTacheToUserLog(demandeMission, CompteDe(idIde))

        dao.CreateRendezVous(PatientPourTache(idPatient), LireParcoursPourTache(idParcours), Tache.TypeTache.RDV_MISSION,
                             New Date(2030, 4, 2, 9, 0, 0), 60, "Mission", CompteDe(idIde), Relire(demandeMission))

        Dim mission = Relire(DerniereTache())
        Assert.AreEqual("RDV_MISSION", mission.Type)
        Assert.AreEqual("RDV_MISSION", mission.Nature)
        Assert.AreEqual(demandeMission, mission.ParentId)
        Assert.AreEqual("TERMINEE", EtatEnBase(demandeMission))
    End Sub

    <TestMethod()> Public Sub CreateRendezVous_TypeQuiNEstPasUnRendezVous_Exception()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim parcoursLu = LireParcoursPourTache(CreerParcoursPourTache(idPatient, idIde))

        For Each refuse In {Tache.TypeTache.RDV_DEMANDE, Tache.TypeTache.MISSION_DEMANDE, Tache.TypeTache.REUNION_STAFF, Tache.TypeTache.AVIS_EPISODE}
            Dim typeRefuse = refuse
            Dim erreur = Assert.ThrowsException(Of Exception)(
                Sub() dao.CreateRendezVous(PatientPourTache(idPatient), parcoursLu, typeRefuse, New Date(2030, 1, 1), 15, "x", CompteDe(idIde)))
            StringAssert.Contains(erreur.Message, "Pas de rendez-vous possible sur ce type de tache", refuse.ToString())
        Next
        Assert.AreEqual(0, NombreDeTaches(idPatient))
    End Sub

    <TestMethod()> Public Sub CreateRendezVous_ParentIncompatible_Exception()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
        Dim parcoursLu = LireParcoursPourTache(idParcours)
        Dim f1 = CreerFonction("IT ide")
        Dim demandeRdv = Relire(EnregistrerTache(TacheDeTest(idPatient, idIde, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, parcoursId:=idParcours, traiteFonctionId:=f1)))
        Dim demandeMission = Relire(EnregistrerTache(TacheDeTest(idPatient, idIde, typeDeTache:=Tache.TypeTache.MISSION_DEMANDE, parcoursId:=idParcours, traiteFonctionId:=f1)))
        Dim avant = NombreDeTaches(idPatient)

        For Each combinaison In {Tuple.Create(Tache.TypeTache.RDV_MISSION, demandeRdv),
                                 Tuple.Create(Tache.TypeTache.RDV, demandeMission),
                                 Tuple.Create(Tache.TypeTache.RDV_SPECIALISTE, demandeRdv)}
            Dim cas = combinaison
            Dim erreur = Assert.ThrowsException(Of Exception)(
                Sub() dao.CreateRendezVous(PatientPourTache(idPatient), parcoursLu, cas.Item1, New Date(2030, 1, 1), 15, "x", CompteDe(idIde), cas.Item2))
            StringAssert.Contains(erreur.Message, "type de tache parent")
        Next
        Assert.AreEqual(avant, NombreDeTaches(idPatient))
        Assert.AreEqual("EN_ATTENTE", EtatEnBase(demandeRdv.Id))
    End Sub

    <TestMethod()> Public Sub CreateRendezVous_DemandePriseParUnAutre_RienNestCree()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idCollegue = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
        Dim demandeRdv = EnregistrerTache(TacheDeTest(idPatient, idIde, typeDeTache:=Tache.TypeTache.RDV_DEMANDE, parcoursId:=idParcours,
                                                      traiteFonctionId:=CreerFonction("IT ide")))
        dao.AttribueTacheToUserLog(demandeRdv, CompteDe(idCollegue))

        VerifierCollision(Sub() dao.CreateRendezVous(PatientPourTache(idPatient), LireParcoursPourTache(idParcours), Tache.TypeTache.RDV,
                                                     New Date(2030, 3, 12), 15, "x", CompteDe(idIde), Relire(demandeRdv)))

        Assert.AreEqual(1, NombreDeTaches(idPatient))
        Assert.AreEqual(idCollegue, Relire(demandeRdv).TraiteUserId)
    End Sub

    <TestMethod()> Public Sub CreateRendezVous_SansGardeAntiDoublon()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim parcoursLu = LireParcoursPourTache(CreerParcoursPourTache(idPatient, idIde))

        ' Comportement actuel : cette surcharge appelle CreateTache sans le garde-fou
        ' de CreateRendezVous(tache) ; deux rendez-vous ouverts sur le même parcours.
        dao.CreateRendezVous(PatientPourTache(idPatient), parcoursLu, Tache.TypeTache.RDV, New Date(2030, 1, 1), 15, "un", CompteDe(idIde))
        dao.CreateRendezVous(PatientPourTache(idPatient), parcoursLu, Tache.TypeTache.RDV, New Date(2030, 1, 2), 15, "deux", CompteDe(idIde))

        Assert.AreEqual(2, NombreDeTaches(idPatient))
    End Sub

    ' ---------------------------------------------------------------------
    ' Modifications de rendez-vous et de demandes de rendez-vous
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ModificationDemandeRendezVous_DemandePrise_EstModifiee()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idTache = EnregistrerTache(TacheDeTest(CreerPatient(), idIde, typeDeTache:=Tache.TypeTache.RDV_DEMANDE,
                                                   traiteFonctionId:=CreerFonction("IT ide"), dateRendezVous:=New Date(2030, 1, 1),
                                                   typeDemandeRendezVous:="ANNEEMOIS", commentaire:="Avant"))
        dao.AttribueTacheToUserLog(idTache, CompteDe(idIde))
        Dim modifiee = Relire(idTache)
        modifiee.TypedemandeRendezVous = Tache.EnumDemandeRendezVous.ANNEE.ToString()
        modifiee.DateRendezVous = New Date(2031, 1, 1)
        modifiee.EmetteurCommentaire = "Apres"

        Assert.IsTrue(dao.ModificationDemandeRendezVous(modifiee))

        Dim lue = Relire(idTache)
        Assert.AreEqual("ANNEE", lue.TypedemandeRendezVous)
        Assert.AreEqual(New Date(2031, 1, 1), lue.DateRendezVous)
        Assert.AreEqual("Apres", lue.EmetteurCommentaire)
        Assert.AreEqual("EN_COURS", lue.Etat)
    End Sub

    <TestMethod()> Public Sub ModificationDemandeRendezVous_DemandeNonPrise_Collision()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idTache = EnregistrerTache(TacheDeTest(CreerPatient(), idIde, typeDeTache:=Tache.TypeTache.RDV_DEMANDE,
                                                   traiteFonctionId:=CreerFonction("IT ide"), dateRendezVous:=New Date(2030, 1, 1),
                                                   typeDemandeRendezVous:="ANNEEMOIS", commentaire:="Avant"))
        Dim modifiee = Relire(idTache)
        modifiee.DateRendezVous = New Date(2031, 1, 1)
        modifiee.EmetteurCommentaire = "Apres"

        VerifierCollision(Sub() dao.ModificationDemandeRendezVous(modifiee))

        Assert.AreEqual("Avant", Relire(idTache).EmetteurCommentaire)
        Assert.AreEqual(New Date(2030, 1, 1), Relire(idTache).DateRendezVous)
    End Sub

    <TestMethod()> Public Sub ModificationRendezVous_EtatAttendu_EstModifie()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idTache = EnregistrerTache(TacheDeTest(CreerPatient(), idIde, typeDeTache:=Tache.TypeTache.RDV,
                                                   traiteFonctionId:=CreerFonction("IT ide"), dateRendezVous:=New Date(2030, 1, 1, 9, 0, 0),
                                                   commentaire:="Avant"))
        Dim modifie = Relire(idTache)
        modifie.DateRendezVous = New Date(2030, 1, 2, 14, 0, 0)
        modifie.EmetteurCommentaire = "Deplace"

        Assert.IsTrue(dao.ModificationRendezVous(modifie, "EN_ATTENTE"))

        Dim lu = Relire(idTache)
        Assert.AreEqual(New Date(2030, 1, 2, 14, 0, 0), lu.DateRendezVous)
        Assert.AreEqual("Deplace", lu.EmetteurCommentaire)
        Assert.AreEqual("EN_ATTENTE", lu.Etat, "l'état n'est pas touché")
    End Sub

    <TestMethod()> Public Sub ModificationRendezVous_EtatChangeEntreTemps_Collision()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idTache = EnregistrerTache(TacheDeTest(CreerPatient(), idIde, typeDeTache:=Tache.TypeTache.RDV,
                                                   traiteFonctionId:=CreerFonction("IT ide"), dateRendezVous:=New Date(2030, 1, 1, 9, 0, 0),
                                                   commentaire:="Avant"))
        Dim modifie = Relire(idTache)
        ' Un autre poste le prend pendant que l'écran est ouvert.
        dao.AttribueTacheToUserLog(idTache, CompteDe(CreerUtilisateur(avecCle:=False)))
        modifie.DateRendezVous = New Date(2030, 1, 2)
        modifie.EmetteurCommentaire = "Deplace"

        VerifierCollision(Sub() dao.ModificationRendezVous(modifie, "EN_ATTENTE"))

        Assert.AreEqual(New Date(2030, 1, 1, 9, 0, 0), Relire(idTache).DateRendezVous)
        Assert.AreEqual("Avant", Relire(idTache).EmetteurCommentaire)
    End Sub

    ' ---------------------------------------------------------------------
    ' CreationAutomatiqueDeDemandeRendezVous
    ' ---------------------------------------------------------------------

    ''' <summary>Parcours relu de la base, avec base et rythme posés en mémoire.</summary>
    Private Shared Function ParcoursPour(idParcours As Long, base As String, rythme As Integer) As Parcours
        Dim lu = LireParcoursPourTache(idParcours)
        lu.Base = base
        lu.Rythme = rythme
        Return lu
    End Function

    <TestMethod()> Public Sub Automatique_SansRythme_RienEtFaux()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde)

        Assert.IsFalse(dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), ParcoursPour(idParcours, ParcoursDao.EnumParcoursBaseCode.ParAn, 0),
                                                                  New Date(2030, 1, 1), CompteDe(idIde)))
        Assert.AreEqual(0, NombreDeTaches(idPatient))
    End Sub

    <TestMethod()> Public Sub Automatique_BaseInconnue_Exception()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde)

        Dim erreur = Assert.ThrowsException(Of Exception)(
            Sub() dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), ParcoursPour(idParcours, "INCONNUE", 1),
                                                             New Date(2030, 1, 1), CompteDe(idIde)))

        ' Comportement actuel : le message annonce une demande créée à 30 jours, mais
        ' l'exception part avant, et rien n'est créé.
        StringAssert.Contains(erreur.Message, "Base de calcul")
        Assert.AreEqual(0, NombreDeTaches(idPatient))
    End Sub

    <TestMethod()> Public Sub Automatique_ParAn_PremierDuMoisEtDelaiDeLaSpecialite()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idUnite = CreerUniteSanitaire("Unite auto")
        Dim idSite = CreerSite("Site auto", idUnite)
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde, specialiteId:=SpecialiteTacheNonOasis,
                                                sousCategorieId:=EnumSousCategoriePPS.IDE, base:=ParcoursDao.EnumParcoursBaseCode.ParAn, rythme:=1)
        Dim avant = Date.Now

        Assert.IsTrue(dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient, idUnite, idSite), LireParcoursPourTache(idParcours),
                                                                 New Date(2030, 6, 15), CompteDe(idIde)))

        ' 15/06/2030 + 365 jours = 15/06/2031, ramené au premier du mois ; traitement
        ' à ce jour moins le délai de prise en charge de la spécialité.
        Dim lue = Relire(DerniereTache())
        Assert.AreEqual(New Date(2031, 6, 1), lue.DateRendezVous)
        Assert.AreEqual(New Date(2031, 6, 1).AddDays(-DelaiSpecialiteTacheNonOasis), lue.DateTraitementDemandeRendezVous)
        Assert.AreEqual("RDV_DEMANDE", lue.Type)
        Assert.AreEqual("RDV_DEMANDE", lue.Nature)
        Assert.AreEqual("ANNEEMOIS", lue.TypedemandeRendezVous)
        Assert.AreEqual("EN_ATTENTE", lue.Etat)
        Assert.AreEqual("SOIN", lue.Categorie)
        Assert.AreEqual(CInt(Tache.EnumPriorite.BASSE), lue.Priorite)
        Assert.AreEqual(20, lue.OrdreAffichage)
        Assert.AreEqual("", lue.EmetteurCommentaire)
        Assert.IsFalse(lue.Cloture)
        ' appSettings IdUserAuto, FonctionEmetteurAutoId et DureeRendezVousParDefaut
        ' absents de app.config : valeurs de repli 1, 14 et 15 (celles de oasis/App.config).
        Assert.AreEqual(1L, lue.EmetteurUserId)
        Assert.AreEqual(CLng(FonctionDao.EnumFonction.INCONNU), lue.EmetteurFonctionId)
        Assert.AreEqual(15, lue.Duree)
        Assert.AreEqual(CLng(FonctionDao.EnumFonction.IDE), lue.DestinataireFonctionId)
        Assert.AreEqual(CLng(FonctionDao.EnumFonction.IDE), lue.TraiteFonctionId)
        Assert.AreEqual(0L, lue.TraiteUserId)
        Assert.AreEqual(idPatient, lue.PatientId)
        Assert.AreEqual(idParcours, lue.ParcoursId)
        Assert.AreEqual(idUnite, lue.UniteSanitaireId)
        Assert.AreEqual(idSite, lue.SiteId)
        Assert.AreEqual(0L, lue.EpisodeId)
        VerifierMaintenant(ColonneTache(lue.Id, "horodate_creation"), avant)
    End Sub

    <TestMethod()> Public Sub Automatique_PremierRendezVous_TraitementRameneAAujourdhui()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde, specialiteId:=SpecialiteTacheNonOasis)
        Dim cible = Date.Now.AddDays(30)

        dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), LireParcoursPourTache(idParcours),
                                                   New Date(2020, 1, 1), CompteDe(idIde), PremierRDV:=True)

        ' Premier rendez-vous : dans 30 jours quelle que soit la date de départ ; moins
        ' 400 jours de délai, le traitement tomberait dans le passé : ramené à maintenant.
        Dim lue = Relire(DerniereTache())
        Assert.AreEqual(New Date(cible.Year, cible.Month, 1), lue.DateRendezVous)
        Assert.AreEqual(Date.Today, lue.DateTraitementDemandeRendezVous.Date)
    End Sub

    <TestMethod()> Public Sub Automatique_TousLes2Ans_SansRythmeEtSansSpecialiteConnue()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
        Dim parcoursLu = ParcoursPour(idParcours, ParcoursDao.EnumParcoursBaseCode.TousLes2Ans, 0)
        ' Spécialité posée en mémoire seulement : absente du référentiel, délai 0.
        parcoursLu.SpecialiteId = SpecialiteTacheAbsente

        Assert.IsTrue(dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), parcoursLu, New Date(2030, 1, 10), CompteDe(idIde)))

        Dim lue = Relire(DerniereTache())
        Assert.AreEqual(New Date(2032, 1, 1), lue.DateRendezVous)
        Assert.AreEqual(New Date(2032, 1, 1), lue.DateTraitementDemandeRendezVous)
    End Sub

    <TestMethod()> Public Sub Automatique_SpecialiteSansDelai_DelaiParDefaut()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde, specialiteId:=SpecialiteTacheOasis,
                                                base:=ParcoursDao.EnumParcoursBaseCode.ParMois, rythme:=1)

        dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), LireParcoursPourTache(idParcours), New Date(2030, 6, 15), CompteDe(idIde))

        ' 15/06/2030 + 30 jours, premier du mois. Délai de la spécialité à 0 : délai
        ' par défaut de Table_specialite, 30 jours sans appSetting SpecialiteDelaiPriseEnCharge
        ' (oasis/App.config en déclare 90).
        Dim lue = Relire(DerniereTache())
        Assert.AreEqual(New Date(2030, 7, 1), lue.DateRendezVous)
        Assert.AreEqual(New Date(2030, 7, 1).AddDays(-30), lue.DateTraitementDemandeRendezVous)
    End Sub

    <TestMethod()> Public Sub Automatique_EcartSelonLaBaseEtLeRythme()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim cas = New List(Of Tuple(Of String, Integer, Date, Date)) From {
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.Quotidien, 1, New Date(2030, 6, 30), New Date(2030, 7, 1)),
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.Quotidien, 1, New Date(2030, 6, 29), New Date(2030, 6, 1)),
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.Hebdomadaire, 2, New Date(2030, 6, 29), New Date(2030, 7, 1)),
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.Hebdomadaire, 1, New Date(2030, 6, 20), New Date(2030, 6, 1)),
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.Hebdomadaire, 1, New Date(2030, 6, 24), New Date(2030, 7, 1)),
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.ParMois, 2, New Date(2030, 6, 16), New Date(2030, 7, 1)),
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.ParMois, 2, New Date(2030, 6, 15), New Date(2030, 6, 1)),
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.ParAn, 12, New Date(2030, 6, 1), New Date(2030, 7, 1)),
            Tuple.Create(ParcoursDao.EnumParcoursBaseCode.TousLes5Ans, 0, New Date(2030, 1, 10), New Date(2035, 1, 1))}

        For Each ligne In cas
            ' Un parcours par cas : une seule demande ouverte par patient et parcours.
            Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
            dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), ParcoursPour(idParcours, ligne.Item1, ligne.Item2),
                                                       ligne.Item3, CompteDe(idIde))
            Assert.AreEqual(ligne.Item4, Relire(DerniereTache()).DateRendezVous, ligne.Item1 & " " & ligne.Item2 & " depuis " & ligne.Item3)
        Next
    End Sub

    <TestMethod()> Public Sub Automatique_FonctionsSelonLaSousCategorie()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim cas = New List(Of Long()) From {
            New Long() {EnumSousCategoriePPS.medecinReferent, SpecialiteTacheOasis, FonctionDao.EnumFonction.MEDECIN, FonctionDao.EnumFonction.MEDECIN},
            New Long() {EnumSousCategoriePPS.sageFemme, EnumSpecialiteOasis.sageFemmeOasis, FonctionDao.EnumFonction.SAGE_FEMME, FonctionDao.EnumFonction.SAGE_FEMME},
            New Long() {EnumSousCategoriePPS.sageFemme, SpecialiteTacheNonOasis, FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS, FonctionDao.EnumFonction.IDE},
            New Long() {EnumSousCategoriePPS.specialiste, SpecialiteTacheNonOasis, FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS, FonctionDao.EnumFonction.IDE},
            New Long() {99, SpecialiteTacheOasis, FonctionDao.EnumFonction.INCONNU, FonctionDao.EnumFonction.IDE}}

        For Each ligne In cas
            Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
            Dim parcoursLu = ParcoursPour(idParcours, ParcoursDao.EnumParcoursBaseCode.ParAn, 1)
            ' Sous-catégorie et spécialité posées en mémoire : seules ces valeurs comptent.
            parcoursLu.SousCategorieId = CInt(ligne(0))
            parcoursLu.SpecialiteId = CInt(ligne(1))
            dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), parcoursLu, New Date(2030, 1, 1), CompteDe(idIde))
            Dim lue = Relire(DerniereTache())
            Assert.AreEqual(ligne(2), lue.DestinataireFonctionId, "destinataire, sous-catégorie " & ligne(0))
            Assert.AreEqual(ligne(3), lue.TraiteFonctionId, "traitant, sous-catégorie " & ligne(0))
        Next
    End Sub

    <TestMethod()> Public Sub Automatique_RendezVousOuvertSurLeParcours_RienEtFaux()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idIde)
        For Each bloquant In {Tache.TypeTache.RDV, Tache.TypeTache.RDV_SPECIALISTE, Tache.TypeTache.RDV_DEMANDE}
            Dim idPatient = CreerPatient()
            Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
            Dim existant = EnregistrerTache(TacheDeTest(idPatient, idIde, typeDeTache:=bloquant, parcoursId:=idParcours,
                                                        dateRendezVous:=New Date(2030, 1, 1)))
            If bloquant = Tache.TypeTache.RDV_SPECIALISTE Then dao.AttribueTacheToUserLog(existant, auteur)

            Assert.IsFalse(dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), LireParcoursPourTache(idParcours),
                                                                      New Date(2030, 6, 1), auteur), bloquant.ToString())
            Assert.AreEqual(1, NombreDeTaches(idPatient), bloquant.ToString())
        Next
    End Sub

    <TestMethod()> Public Sub Automatique_RendezVousClosOuMission_NeBloquePas()
        Dim idIde = CreerUtilisateur(avecCle:=False)
        Dim auteur = CompteDe(idIde)
        Dim idPatient = CreerPatient()
        Dim idParcours = CreerParcoursPourTache(idPatient, idIde)
        dao.ClotureTache(EnregistrerTache(TacheDeTest(idPatient, idIde, typeDeTache:=Tache.TypeTache.RDV, parcoursId:=idParcours,
                                                      dateRendezVous:=New Date(2030, 1, 1))), True, auteur)
        EnregistrerTache(TacheDeTest(idPatient, idIde, typeDeTache:=Tache.TypeTache.RDV_MISSION, parcoursId:=idParcours,
                                     dateRendezVous:=New Date(2030, 1, 1)))

        Assert.IsTrue(dao.CreationAutomatiqueDeDemandeRendezVous(PatientPourTache(idPatient), LireParcoursPourTache(idParcours),
                                                                 New Date(2030, 6, 1), auteur))
        Assert.AreEqual(3, NombreDeTaches(idPatient))
    End Sub

End Class
