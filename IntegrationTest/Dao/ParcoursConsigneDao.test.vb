Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' ParcoursConsigneDao contre la base de test. Les consignes d'un parcours sont
''' saisies dans RadFParcoursConsigneDetailEdit, listées dans RadFParcoursDetailEdit
''' et la synthèse, et lues à la création d'épisode par
''' EpisodeProtocoleCollaboratifDao : tout tourne sous oasis_client.
''' IsExistParcoursConsigne et la création d'un parcours (historique) nomment la
''' base ([oasis].[oasis]) : ces tests commencent par ExigerBaseOasis.
''' </summary>
<TestClass()> Public Class ParcoursConsigneDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ParcoursConsigneDao

    Private Shared Function IdsDe(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("oa_parcours_consigne_id"))).ToArray()
    End Function

    Private Shared Function ParcoursDeTest(idPatient As Long) As Long
        Return CreerParcoursPatient(idPatient, CreerRorParcours("INTERVENANT"))
    End Function

    ' --- Lecture et écriture unitaires ---------------------------------------------

    <TestMethod()> Public Sub CreateParcoursConsigne_PuisGetById_RelitChaqueColonne()
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()

        Dim idConsigne = CreerConsigneParcours(0, idPatient, idDrc, "SUIVI_CHRONIQUE", ordre:=3, commentaire:="Tous les trois mois",
                                               dateDebut:=New Date(2024, 1, 1), dateFin:=New Date(2030, 12, 31),
                                               ageMin:=18, ageMax:=75, ageUnite:="A")

        Dim lue = dao.GetParcoursConsigneById(CInt(idConsigne))
        Assert.AreEqual(idConsigne, lue.Id)
        Assert.AreEqual(0L, lue.ParcoursId)
        Assert.AreEqual(idPatient, lue.PatientId)
        Assert.AreEqual(idDrc, lue.DrcId)
        Assert.AreEqual("SUIVI_CHRONIQUE", lue.TypeEpisode)
        Assert.AreEqual("Tous les trois mois", lue.Commentaire)
        Assert.AreEqual(3, lue.Ordre)
        Assert.AreEqual(18, lue.AgeMin)
        Assert.AreEqual(75, lue.AgeMax)
        Assert.AreEqual("A", lue.AgeUnite)
        Assert.AreEqual(New Date(2024, 1, 1), lue.DateDebut.Date)
        Assert.AreEqual(New Date(2030, 12, 31), lue.DateFin.Date)
        Assert.IsFalse(lue.Inactif)
    End Sub

    <TestMethod()> Public Sub GetParcoursConsigneById_ConsigneAbsente_LeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetParcoursConsigneById(987654321))
        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub ModificationParcoursConsigne_ReecritChaqueColonne()
        Dim idPatient = CreerPatient()
        Dim idConsigne = CreerConsigneParcours(0, idPatient, CreerDrc())
        Dim autreDrc = CreerDrc()
        Dim fiche = dao.GetParcoursConsigneById(CInt(idConsigne))
        fiche.DrcId = autreDrc
        fiche.TypeEpisode = "PREVENTION_AUTRE"
        fiche.Commentaire = "Modifiee"
        fiche.Ordre = 9
        fiche.AgeMin = 2
        fiche.AgeMax = 24
        fiche.AgeUnite = "M"
        fiche.DateDebut = New Date(2025, 2, 1)
        fiche.DateFin = New Date(2027, 3, 1)
        fiche.Inactif = True

        dao.ModificationParcoursConsigne(fiche)

        Dim relue = dao.GetParcoursConsigneById(CInt(idConsigne))
        Assert.AreEqual(idPatient, relue.PatientId)
        Assert.AreEqual(autreDrc, relue.DrcId)
        Assert.AreEqual("PREVENTION_AUTRE", relue.TypeEpisode)
        Assert.AreEqual("Modifiee", relue.Commentaire)
        Assert.AreEqual(9, relue.Ordre)
        Assert.AreEqual(2, relue.AgeMin)
        Assert.AreEqual(24, relue.AgeMax)
        Assert.AreEqual("M", relue.AgeUnite)
        Assert.AreEqual(New Date(2025, 2, 1), relue.DateDebut.Date)
        Assert.AreEqual(New Date(2027, 3, 1), relue.DateFin.Date)
        Assert.IsTrue(relue.Inactif)
    End Sub

    <TestMethod()> Public Sub AnnulationParcoursConsigne_NeChangeQueLIndicateurInactif()
        Dim idPatient = CreerPatient()
        Dim idConsigne = CreerConsigneParcours(0, idPatient, CreerDrc(), commentaire:="Garde son texte")
        Dim autre = CreerConsigneParcours(0, idPatient, CreerDrc())
        Dim fiche = dao.GetParcoursConsigneById(CInt(idConsigne))
        ' L'objet passé ne compte que par son id.
        fiche.Commentaire = "Ignore"

        dao.AnnulationParcoursConsigne(fiche)

        Dim relue = dao.GetParcoursConsigneById(CInt(idConsigne))
        Assert.IsTrue(relue.Inactif)
        Assert.AreEqual("Garde son texte", relue.Commentaire)
        Assert.IsFalse(dao.GetParcoursConsigneById(CInt(autre)).Inactif)
    End Sub

    ' --- Listes d'un parcours ------------------------------------------------------

    <TestMethod()> Public Sub GetAllConsignebyParcoursId_RendLesConsignesEnCoursParOrdre()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idParcours = ParcoursDeTest(idPatient)
        Dim autreParcours = ParcoursDeTest(idPatient)
        Dim drcSeconde = CreerDrc("DRC SECONDE")
        Dim seconde = CreerConsigneParcours(idParcours, idPatient, drcSeconde, ordre:=2)
        Dim premiere = CreerConsigneParcours(idParcours, idPatient, CreerDrc("DRC PREMIERE"), ordre:=1)
        CreerConsigneParcours(idParcours, idPatient, CreerDrc(), ordre:=3, inactif:=True)
        CreerConsigneParcours(idParcours, idPatient, CreerDrc(), ordre:=4, dateFin:=Date.Today.AddDays(-1))
        CreerConsigneParcours(autreParcours, idPatient, CreerDrc(), ordre:=0)

        Dim table = dao.GetAllConsignebyParcoursId(CInt(idParcours))

        CollectionAssert.AreEqual(New Long() {premiere, seconde}, IdsDe(table))
        Assert.AreEqual("DRC PREMIERE", CStr(table.Rows(0)("oa_drc_libelle")))
        Assert.AreEqual("DRC SECONDE", CStr(table.Rows(1)("oa_drc_libelle")))
        Assert.AreEqual(drcSeconde, CLng(table.Rows(1)("oa_parcours_consigne_drc_id")))
        Assert.AreEqual("PATHOLOGIE_AIGUE", CStr(table.Rows(1)("activite_type_episode")))
    End Sub

    <TestMethod()> Public Sub GetAllConsignebyParcoursId_ConsigneFinissantAujourdhui_EstDejaExclue()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idParcours = ParcoursDeTest(idPatient)
        Dim demain = CreerConsigneParcours(idParcours, idPatient, CreerDrc(), dateFin:=Date.Today.AddDays(1))
        CreerConsigneParcours(idParcours, idPatient, CreerDrc(), dateFin:=Date.Today)

        ' Comportement actuel : la fin est comparée à GETDATE(), date et heure. Une
        ' consigne saisie jusqu'à aujourd'hui (minuit) disparaît dès le matin.
        CollectionAssert.AreEqual(New Long() {demain}, IdsDe(dao.GetAllConsignebyParcoursId(CInt(idParcours))))
    End Sub

    <TestMethod()> Public Sub GetConsigneParamedicalebyParcoursId_NeRetientQueLesActesParamedicaux()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idParcours = ParcoursDeTest(idPatient)
        Dim acte2 = CreerConsigneParcours(idParcours, idPatient, CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.ActeParamedical), ordre:=2)
        Dim acte1 = CreerConsigneParcours(idParcours, idPatient, CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.ActeParamedical), ordre:=1)
        CreerConsigneParcours(idParcours, idPatient, CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Prevention), ordre:=0)
        CreerConsigneParcours(idParcours, idPatient, CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.ActeParamedical), ordre:=3, inactif:=True)
        CreerConsigneParcours(idParcours, idPatient, CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.ActeParamedical), ordre:=4,
                              dateFin:=Date.Today.AddDays(-1))

        CollectionAssert.AreEqual(New Long() {acte1, acte2}, IdsDe(dao.GetConsigneParamedicalebyParcoursId(CInt(idParcours))))
    End Sub

    <TestMethod()> Public Sub GetAllConsignebyParcoursId_ParcoursSansConsigne_RendUneTableVide()
        Assert.AreEqual(0, dao.GetAllConsignebyParcoursId(987654321).Rows.Count)
        Assert.AreEqual(0, dao.GetConsigneParamedicalebyParcoursId(987654321).Rows.Count)
    End Sub

    ' --- Consignes d'un patient par type d'activité ---------------------------------

    <TestMethod()> Public Sub GetDrcConsigneByActiviteEtPatientId_RequeteSansFrom_Echoue()
        Dim idPatient = CreerPatient()
        CreerConsigneParcours(0, idPatient, CreerDrc())

        ' Comportement actuel : la requête n'a pas de clause FROM (le LEFT JOIN suit
        ' directement le SELECT) et SQL Server la refuse. Aucun appelant.
        Assert.ThrowsException(Of SqlException)(Sub() dao.GetDrcConsigneByActiviteEtPatientId("PATHOLOGIE_AIGUE", idPatient))
    End Sub

    <TestMethod()> Public Sub GetDrcPatientByTypeActiviteEtPatient_FiltreActiviteEtPatientSansRegarderLaFin()
        Dim idPatient = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim idDrc = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.ProtocoleCollaboratif)
        Dim enCours = CreerConsigneParcours(0, idPatient, idDrc, "SUIVI_CHRONIQUE", ageMin:=40, ageMax:=80,
                                            dateDebut:=New Date(2024, 1, 1), dateFin:=New Date(2030, 1, 1))
        Dim echue = CreerConsigneParcours(0, idPatient, CreerDrc(), "SUIVI_CHRONIQUE", dateFin:=New Date(2020, 1, 1))
        CreerConsigneParcours(0, idPatient, CreerDrc(), "SUIVI_CHRONIQUE", inactif:=True)
        CreerConsigneParcours(0, idPatient, CreerDrc(), "PATHOLOGIE_AIGUE")
        CreerConsigneParcours(0, autrePatient, CreerDrc(), "SUIVI_CHRONIQUE")

        Dim table = dao.GetDrcPatientByTypeActiviteEtPatient("SUIVI_CHRONIQUE", idPatient)

        ' Comportement actuel : pas de filtre sur la date de fin, une consigne échue
        ' est encore proposée ; EpisodeProtocoleCollaboratifDao compare lui-même les dates.
        CollectionAssert.AreEquivalent(New Long() {enCours, echue}, IdsDe(table))
        Dim ligne = table.Rows.Cast(Of DataRow)().Single(Function(r) CLng(r("oa_parcours_consigne_id")) = enCours)
        Assert.AreEqual("SUIVI_CHRONIQUE", CStr(ligne("activite_type_episode")))
        Assert.AreEqual(CInt(Drc.EnumCategorieOasisCode.ProtocoleCollaboratif), CInt(ligne("oa_drc_oasis_categorie")))
        Assert.AreEqual(idDrc, CLng(ligne("oa_parcours_consigne_drc_id")))
        Assert.AreEqual(40, CInt(ligne("oa_parcours_age_min")))
        Assert.AreEqual(80, CInt(ligne("oa_parcours_age_max")))
        Assert.AreEqual(New Date(2024, 1, 1), CDate(ligne("oa_parcours_consigne_date_debut")).Date)
        Assert.AreEqual(New Date(2030, 1, 1), CDate(ligne("oa_parcours_consigne_date_fin")).Date)
    End Sub

    <TestMethod()> Public Sub GetDrcPatientByTypeActiviteEtPatient_SansConsigne_RendUneTableVide()
        Assert.AreEqual(0, dao.GetDrcPatientByTypeActiviteEtPatient("SUIVI_CHRONIQUE", CreerPatient()).Rows.Count)
    End Sub

    ' --- Existence ------------------------------------------------------------------

    <TestMethod()> Public Sub IsExistParcoursConsigne_ParcoursAvecOuSansConsigne()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim avecConsigne = ParcoursDeTest(idPatient)
        Dim sansConsigne = ParcoursDeTest(idPatient)
        Dim consigneAnnulee = ParcoursDeTest(idPatient)
        CreerConsigneParcours(avecConsigne, idPatient, CreerDrc())
        CreerConsigneParcours(consigneAnnulee, idPatient, CreerDrc(), inactif:=True)

        Assert.IsTrue(dao.IsExistParcoursConsigne(avecConsigne))
        Assert.IsFalse(dao.IsExistParcoursConsigne(sansConsigne))
        ' Comportement actuel : une consigne annulée compte encore ; la synthèse
        ' n'affiche pas le parcours comme « sans consigne ».
        Assert.IsTrue(dao.IsExistParcoursConsigne(consigneAnnulee))
    End Sub

End Class
