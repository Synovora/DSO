Imports Oasis_Common

''' <summary>
''' ValenceDao contre la base. Seul le client lourd l'appelle (RadFVaccin,
''' RadFValenceCreation, RadFValenceSelecteur, RadFVaccinInfo) : tout tourne sous
''' Compte.Client, qui a le droit de supprimer dans oa_valence,
''' oa_relation_vaccin_valence et les tables oa_vaccin_cgv_* que Delete vide en
''' cascade.
''' </summary>
<TestClass()> Public Class ValenceDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ValenceDao

    Private Const ValenceAbsente As Integer = 987654321

    Private Function Relue(idValence As Long) As Valence
        Return dao.GetById(CInt(idValence))
    End Function

    Private Shared Function NombreDeValences() As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_valence"))
    End Function

    ' --- Create et GetById ---------------------------------------------------------

    <TestMethod()> Public Sub Create_EnregistreUneValenceActiveEtInvisible()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)

        Dim idValence = dao.Create(New Valence With {
            .Code = "ROR", .Description = "Rougeole, oreillons, rubeole", .Precaution = "Grossesse",
            .UtilisateurCreation = idAuteur, .UtilisateurModification = idModificateur})

        Assert.IsTrue(idValence > 0)
        Dim lue = Relue(idValence)
        Assert.AreEqual(idValence, lue.Id)
        Assert.AreEqual("ROR", lue.Code)
        Assert.AreEqual("Rougeole, oreillons, rubeole", lue.Description)
        Assert.AreEqual("Grossesse", lue.Precaution)
        Assert.AreEqual(idAuteur, lue.UtilisateurCreation)
        Assert.AreEqual(idModificateur, lue.UtilisateurModification)
        Assert.IsTrue(lue.Actif)
        Assert.IsFalse(lue.Visible, "une valence créée n'apparaît pas encore au calendrier")
        ' Dates absentes du bean : l'instant présent.
        Assert.AreEqual(Date.Today, lue.DateCreation.Date)
        Assert.AreEqual(Date.Today, lue.DateModification.Date)
    End Sub

    <TestMethod()> Public Sub Create_ConserveLesDatesFournies()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim creation As New Date(2024, 3, 4)
        Dim modification As New Date(2025, 5, 6)

        Dim idValence = dao.Create(New Valence With {
            .Code = "DTP", .Description = "d", .Precaution = "p",
            .DateCreation = creation, .DateModification = modification,
            .UtilisateurCreation = idAuteur, .UtilisateurModification = idAuteur})

        Dim lue = Relue(idValence)
        Assert.AreEqual(creation, lue.DateCreation)
        Assert.AreEqual(modification, lue.DateModification)
    End Sub

    <TestMethod()> Public Sub Create_PremiereValenceEnPositionZeroPuisALaSuite()
        Assert.AreEqual(0, NombreDeValences(), "l'instantané ne contient aucune valence")
        Dim idAuteur = CreerUtilisateur(avecCle:=False)

        Dim premiere = CreerValence(utilisateurId:=idAuteur)
        Dim deuxieme = CreerValence(utilisateurId:=idAuteur)
        Dim troisieme = CreerValence(utilisateurId:=idAuteur)

        Assert.AreEqual(0, Relue(premiere).Ordre)
        Assert.AreEqual(1, Relue(deuxieme).Ordre)
        Assert.AreEqual(2, Relue(troisieme).Ordre)
    End Sub

    <TestMethod()> Public Sub Create_SePlaceApresLaPlusGrandePosition()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim premiere = CreerValence(utilisateurId:=idAuteur)
        dao.SetOrder(premiere, 40)

        Dim suivante = CreerValence(utilisateurId:=idAuteur)

        Assert.AreEqual(41, Relue(suivante).Ordre)
    End Sub

    <TestMethod()> Public Sub GetById_Inexistante_LeveArgumentException()
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetById(ValenceAbsente))
    End Sub

    ' --- Listes et positions -------------------------------------------------------

    <TestMethod()> Public Sub GetList_RenvoieToutesLesValencesParPositionCroissante()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim a = CreerValence(utilisateurId:=idAuteur)
        Dim b = CreerValence(utilisateurId:=idAuteur)
        Dim c = CreerValence(utilisateurId:=idAuteur)
        ' Visibles ou non, toutes reviennent : GetList ne filtre pas. SetVisibility
        ' déplace la valence, d'où les positions posées après.
        dao.SetVisibility(c, True)
        dao.SetOrder(a, 30)
        dao.SetOrder(b, 10)
        dao.SetOrder(c, 20)

        Dim ids = dao.GetList().Select(Function(v) v.Id).Where(Function(i) i = a OrElse i = b OrElse i = c).ToArray()

        CollectionAssert.AreEqual(New Long() {b, c, a}, ids)
    End Sub

    <TestMethod()> Public Sub GetList_SansValence_ListeVide()
        Assert.AreEqual(0, NombreDeValences(), "l'instantané ne contient aucune valence")
        Assert.AreEqual(0, dao.GetList().Count)
    End Sub

    <TestMethod()> Public Sub GetLastOrder_RenvoieLaValenceDePlusGrandePosition()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim a = CreerValence(utilisateurId:=idAuteur)
        Dim b = CreerValence(utilisateurId:=idAuteur)
        dao.SetOrder(a, 50)
        dao.SetOrder(b, 7)

        Dim derniere = dao.GetLastOrder()

        Assert.AreEqual(a, derniere.Id)
        Assert.AreEqual(50, derniere.Ordre)
    End Sub

    <TestMethod()> Public Sub GetLastOrder_SansValence_Nothing()
        Assert.AreEqual(0, NombreDeValences(), "l'instantané ne contient aucune valence")
        Assert.IsNull(dao.GetLastOrder())
    End Sub

    <TestMethod()> Public Sub GetByOrder_TrouveLaValenceDeCettePosition()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim a = CreerValence(utilisateurId:=idAuteur)
        Dim b = CreerValence(utilisateurId:=idAuteur)
        dao.SetOrder(a, 12)
        dao.SetOrder(b, 13)

        Assert.AreEqual(b, dao.GetByOrder(13).Id)
        Assert.AreEqual(a, dao.GetByOrder(12).Id)
    End Sub

    <TestMethod()> Public Sub GetByOrder_PositionInoccupee_Nothing()
        CreerValence()
        Assert.IsNull(dao.GetByOrder(999))
    End Sub

    <TestMethod()> Public Sub GetListFromOrder_RetientLaBorneEtAuDela()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim avant = CreerValence(utilisateurId:=idAuteur)
        Dim borne = CreerValence(utilisateurId:=idAuteur)
        Dim apres = CreerValence(utilisateurId:=idAuteur)
        dao.SetOrder(avant, 4)
        dao.SetOrder(borne, 5)
        dao.SetOrder(apres, 6)

        Dim ids = dao.GetListFromOrder(5).Select(Function(v) v.Id).ToList()

        CollectionAssert.AreEquivalent(New Long() {borne, apres}, ids)
    End Sub

    <TestMethod()> Public Sub GetListFromOrder_AuDelaDeLaDernierePosition_ListeVide()
        CreerValence()
        Assert.AreEqual(0, dao.GetListFromOrder(1000).Count)
    End Sub

    ' --- Modifications -------------------------------------------------------------

    <TestMethod()> Public Sub Update_ModifieLesChampsEditablesEtHorodate()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)
        Dim idValence = dao.Create(New Valence With {
            .Code = "AVANT", .Description = "d avant", .Precaution = "p avant",
            .DateCreation = New Date(2024, 1, 2), .DateModification = New Date(2024, 1, 2),
            .UtilisateurCreation = idAuteur, .UtilisateurModification = idAuteur})
        Dim modifiee = Relue(idValence)
        modifiee.Code = "APRES"
        modifiee.Description = "d apres"
        modifiee.Precaution = "p apres"
        modifiee.UtilisateurModification = idModificateur
        modifiee.Visible = True
        modifiee.Ordre = 9
        ' Ignorés par l'UPDATE.
        modifiee.DateModification = New Date(2000, 1, 1)
        modifiee.UtilisateurCreation = idModificateur
        modifiee.Actif = False

        Assert.AreEqual(idValence, dao.Update(modifiee))

        Dim lue = Relue(idValence)
        Assert.AreEqual("APRES", lue.Code)
        Assert.AreEqual("d apres", lue.Description)
        Assert.AreEqual("p apres", lue.Precaution)
        Assert.AreEqual(idModificateur, lue.UtilisateurModification)
        Assert.IsTrue(lue.Visible)
        Assert.AreEqual(9, lue.Ordre)
        Assert.AreEqual(Date.Today, lue.DateModification.Date, "date de modification : l'instant présent")
        Assert.AreEqual(New Date(2024, 1, 2), lue.DateCreation)
        Assert.AreEqual(idAuteur, lue.UtilisateurCreation)
        Assert.IsTrue(lue.Actif)
    End Sub

    <TestMethod()> Public Sub SetOrder_ChangeSeulementLaPosition()
        Dim idValence = CreerValence(code:="HEPB")

        Assert.AreEqual(idValence, dao.SetOrder(idValence, 17))

        Dim lue = Relue(idValence)
        Assert.AreEqual(17, lue.Ordre)
        Assert.AreEqual("HEPB", lue.Code)
        Assert.IsFalse(lue.Visible)
    End Sub

    <TestMethod()> Public Sub SetVisibility_Visible_PlaceApresLaDernierePosition()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim autre = CreerValence(utilisateurId:=idAuteur)
        Dim idValence = CreerValence(utilisateurId:=idAuteur)
        dao.SetOrder(autre, 20)
        dao.SetOrder(idValence, 3)

        Assert.AreEqual(idValence, dao.SetVisibility(idValence, True))

        Dim lue = Relue(idValence)
        Assert.IsTrue(lue.Visible)
        Assert.AreEqual(21, lue.Ordre)
    End Sub

    <TestMethod()> Public Sub SetVisibility_ValenceDejaEnDernierePosition_AvanceDUnCran()
        Dim idValence = CreerValence()
        dao.SetOrder(idValence, 5)

        dao.SetVisibility(idValence, True)

        ' Comportement actuel : GetLastOrder compte la valence elle-même, qui
        ' passe donc de 5 à 6 alors qu'elle était déjà la dernière.
        Assert.AreEqual(6, Relue(idValence).Ordre)
    End Sub

    <TestMethod()> Public Sub SetVisibility_Invisible_RemetLaPositionAZero()
        Dim idValence = CreerValence()
        dao.SetVisibility(idValence, True)
        dao.SetOrder(idValence, 8)

        dao.SetVisibility(idValence, False)

        Dim lue = Relue(idValence)
        Assert.IsFalse(lue.Visible)
        Assert.AreEqual(0, lue.Ordre)
    End Sub

    ' --- Suppression en cascade ----------------------------------------------------

    <TestMethod()> Public Sub Delete_SupprimeLaValenceEtSesRattachements_PasCeuxDesAutres()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim cible = CreerValence(utilisateurId:=idAuteur)
        Dim autre = CreerValence(utilisateurId:=idAuteur)
        Dim codeVaccin = NouveauCodeVaccin()
        CreerVaccin(codeVaccin, utilisateurId:=idAuteur)
        LierVaccinValence(codeVaccin, cible)
        LierVaccinValence(codeVaccin, autre)
        CreerValenceCgv(cible, 0)
        CreerValenceCgv(cible, idPatient)
        CreerValenceCgv(autre, idPatient)
        Dim idDate = CreerDateCgv(60, idPatient)
        LierValenceDateCgv(idDate, cible, idPatient)
        LierValenceDateCgv(idDate, autre, idPatient)

        Assert.AreEqual(cible, dao.Delete(New Valence With {.Id = cible}))

        Assert.AreEqual(0, CompterLignesVaccin("oa_valence", "id = @p0", cible))
        Assert.AreEqual(0, CompterLignesVaccin("oa_relation_vaccin_valence", "valence = @p0", cible))
        Assert.AreEqual(0, CompterLignesVaccin("oa_vaccin_cgv_valence", "valence = @p0", cible))
        Assert.AreEqual(0, CompterLignesVaccin("oa_vaccin_cgv_relation_valence_date", "valence = @p0", cible))
        Assert.AreEqual(1, CompterLignesVaccin("oa_valence", "id = @p0", autre))
        Assert.AreEqual(1, CompterLignesVaccin("oa_relation_vaccin_valence", "valence = @p0", autre))
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_cgv_valence", "valence = @p0", autre))
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_cgv_relation_valence_date", "valence = @p0", autre))
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_cgv_date", "id = @p0", idDate), "la date du calendrier reste")
    End Sub

    <TestMethod()> Public Sub Delete_ValenceInexistante_NeLevePasDErreur()
        Dim existante = CreerValence()

        Assert.AreEqual(CLng(ValenceAbsente), dao.Delete(New Valence With {.Id = ValenceAbsente}))

        Assert.AreEqual(1, CompterLignesVaccin("oa_valence", "id = @p0", existante))
    End Sub

    ' --- Relations vaccin / valence ------------------------------------------------

    <TestMethod()> Public Sub CreateRelation_EstRelueParVaccinEtParValence()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim codeA = NouveauCodeVaccin()
        Dim codeB = NouveauCodeVaccin()

        Dim r1 = dao.CreateRelation(New RelationVaccinValence With {.Vaccin = codeA, .Valence = v1})
        Dim r2 = dao.CreateRelation(New RelationVaccinValence With {.Vaccin = codeA, .Valence = v2})
        Dim r3 = dao.CreateRelation(New RelationVaccinValence With {.Vaccin = codeB, .Valence = v1})

        Assert.IsTrue(r1 > 0 AndAlso r2 > 0 AndAlso r3 > 0)
        Dim parVaccin = dao.GetRelationListByVaccin(codeA)
        CollectionAssert.AreEquivalent(New Long() {r1, r2}, parVaccin.Select(Function(r) r.Id).ToList())
        Assert.IsTrue(parVaccin.All(Function(r) r.Vaccin = codeA))
        CollectionAssert.AreEquivalent(New Long() {v1, v2}, parVaccin.Select(Function(r) r.Valence).ToList())

        Dim parValence = dao.GetRelationListByValence(v1)
        CollectionAssert.AreEquivalent(New Long() {r1, r3}, parValence.Select(Function(r) r.Id).ToList())
        CollectionAssert.AreEquivalent(New Long() {codeA, codeB}, parValence.Select(Function(r) r.Vaccin).ToList())

        CollectionAssert.AreEquivalent(New Long() {r1, r2, r3}, dao.GetRelationList().Select(Function(r) r.Id).ToList())
    End Sub

    <TestMethod()> Public Sub GetRelationList_SansRelation_ListesVides()
        Dim idValence = CreerValence()
        Assert.AreEqual(0, dao.GetRelationList().Count)
        Assert.AreEqual(0, dao.GetRelationListByValence(idValence).Count)
        Assert.AreEqual(0, dao.GetRelationListByVaccin(NouveauCodeVaccin()).Count)
    End Sub

    <TestMethod()> Public Sub DeleteRelation_SupprimeSeulementLeCoupleVaccinValence()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim codeA = NouveauCodeVaccin()
        Dim codeB = NouveauCodeVaccin()
        LierVaccinValence(codeA, v1)
        Dim gardee1 = LierVaccinValence(codeA, v2)
        Dim gardee2 = LierVaccinValence(codeB, v1)

        ' RadFVaccin ne renseigne pas l'id : le DAO le renvoie tel quel, donc 0.
        Assert.AreEqual(0L, dao.DeleteRelation(New RelationVaccinValence With {.Valence = v1, .Vaccin = codeA}))

        CollectionAssert.AreEquivalent(New Long() {gardee1, gardee2}, dao.GetRelationList().Select(Function(r) r.Id).ToList())
        Assert.AreEqual(1, CompterLignesVaccin("oa_valence", "id = @p0", v1), "la valence elle-même reste")
    End Sub

End Class
