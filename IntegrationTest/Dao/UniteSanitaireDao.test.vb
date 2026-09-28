Imports Oasis_Common

''' <summary>
''' UniteSanitaireDao contre la base. L'unité sanitaire est le premier niveau du
''' périmètre d'un utilisateur : FrmTacheMain.initFiltre relit celle de
''' l'utilisateur connecté, FrmFiltreTacheATraiter liste les unités actives,
''' FrmUtilisateur propose les unités d'un siège, TacheDao et les éditions relisent
''' l'unité d'une tâche ou d'un patient. Tous ces appelants sont dans le client
''' lourd : tout tourne sous oasis_client.
''' </summary>
<TestClass()> Public Class UniteSanitaireDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New UniteSanitaireDao

    Private Shared Function IdsDe(liste As List(Of UniteSanitaire)) As List(Of Integer)
        Return liste.Select(Function(u) u.Oa_unite_sanitaire_id).ToList()
    End Function

    ' --- getUniteSanitaireById ---------------------------------------------------

    <TestMethod()> Public Sub getUniteSanitaireById_UniteActive_RenvoieToutesSesColonnes()
        Dim idSiege = CreerSiege("IT Siege de l'unite")
        Dim idUnite = CreerUniteSanitaire("IT Unite complete", siegeId:=idSiege, numeroStructure:=97601)

        Dim lue = dao.getUniteSanitaireById(CInt(idUnite))

        Assert.AreEqual(CInt(idUnite), lue.Oa_unite_sanitaire_id)
        Assert.AreEqual("IT Unite complete", lue.Oa_unite_sanitaire_description)
        Assert.AreEqual(CInt(idSiege), lue.Oa_unite_sanitaire_siege_id)
        Assert.AreEqual("2 rue de l'Unite", lue.Oa_unite_sanitaire_adresse1)
        Assert.AreEqual("Batiment B", lue.Oa_unite_sanitaire_adresse2)
        Assert.AreEqual("Dzaoudzi", lue.Oa_unite_sanitaire_ville)
        Assert.AreEqual("97615", lue.Oa_unite_sanitaire_code_postal)
        Assert.AreEqual("0269000011", lue.Telephone)
        Assert.AreEqual("unite@exemple.fr", lue.Mail)
        Assert.AreEqual("0269000012", lue.Fax)
        Assert.IsFalse(lue.Oa_unite_sanitaire_inactif)
        Assert.AreEqual(97601L, lue.NumeroStructure)
        Assert.IsNull(lue.LstSite, "les sites ne sont pas chargés par le DAO")
    End Sub

    <TestMethod()> Public Sub getUniteSanitaireById_UniteInactive_EstIgnoreeSaufSiDemande()
        ' FrmTacheMain.initFiltre lit sans les inactives : un utilisateur rattaché à une
        ' unité fermée tombe sur l'exception (affichée en MsgBox) et n'a aucun filtre.
        Dim idUnite = CreerUniteSanitaire("IT Unite fermee", inactif:=True)

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUniteSanitaireById(CInt(idUnite)))

        Dim lue = dao.getUniteSanitaireById(CInt(idUnite), True)
        Assert.AreEqual(CInt(idUnite), lue.Oa_unite_sanitaire_id)
        Assert.IsTrue(lue.Oa_unite_sanitaire_inactif)
    End Sub

    <TestMethod()> Public Sub getUniteSanitaireById_UniteInexistante_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.getUniteSanitaireById(987654321, True))
        StringAssert.Contains(erreur.Message, "Unité sanitaire non retrouvée")
    End Sub

    <TestMethod()> Public Sub getUniteSanitaireById_ColonnesFacultativesNulles_DonnentLesValeursParDefaut()
        ' inactif NULL : l'unité compte comme active (COALESCE à 0 dans la requête).
        Dim idUnite = CreerUniteSanitaire("IT Unite sans siege", inactif:=Nothing)

        Dim lue = dao.getUniteSanitaireById(CInt(idUnite))

        Assert.AreEqual(0, lue.Oa_unite_sanitaire_siege_id)
        Assert.IsFalse(lue.Oa_unite_sanitaire_inactif)
    End Sub

    ' --- getList -----------------------------------------------------------------

    <TestMethod()> Public Sub getList_SansInactives_ExclutLesUnitesFermeesEtTrieParDescription()
        Dim idB = CreerUniteSanitaire("IT Liste unite B")
        Dim idA = CreerUniteSanitaire("IT Liste unite A")
        Dim idNull = CreerUniteSanitaire("IT Liste unite C", inactif:=Nothing)
        Dim idFermee = CreerUniteSanitaire("IT Liste unite D", inactif:=True)

        Dim ids = IdsDe(dao.getList(False))

        CollectionAssert.DoesNotContain(ids, CInt(idFermee))
        Dim miennes = ids.Where(Function(i) i = idA OrElse i = idB OrElse i = idNull).ToList()
        CollectionAssert.AreEqual(New List(Of Integer) From {CInt(idA), CInt(idB), CInt(idNull)}, miennes,
                                  "ORDER BY oa_unite_sanitaire_description")
    End Sub

    <TestMethod()> Public Sub getList_AvecInactives_RenvoieAussiLesUnitesFermees()
        Dim idOuverte = CreerUniteSanitaire("IT Toutes A")
        Dim idFermee = CreerUniteSanitaire("IT Toutes B", inactif:=True)

        Dim ids = IdsDe(dao.getList(True))

        CollectionAssert.Contains(ids, CInt(idOuverte))
        CollectionAssert.Contains(ids, CInt(idFermee))
    End Sub

    <TestMethod()> Public Sub getList_ParSiege_NeRenvoieQueLesUnitesActivesDeCeSiege()
        ' Création d'un utilisateur (FrmUtilisateur, isWithInactif à False) : les
        ' unités proposées sont celles du siège choisi.
        Dim idSiege = CreerSiege("IT Siege filtre")
        Dim idAutreSiege = CreerSiege("IT Siege voisin")
        Dim idU2 = CreerUniteSanitaire("IT Par siege 2", siegeId:=idSiege)
        Dim idU1 = CreerUniteSanitaire("IT Par siege 1", siegeId:=idSiege)
        CreerUniteSanitaire("IT Par siege fermee", siegeId:=idSiege, inactif:=True)
        CreerUniteSanitaire("IT Par siege voisine", siegeId:=idAutreSiege)
        CreerUniteSanitaire("IT Par siege aucun")

        Dim liste = dao.getList(False, CInt(idSiege))

        CollectionAssert.AreEqual(New List(Of Integer) From {CInt(idU1), CInt(idU2)}, IdsDe(liste))
    End Sub

    <TestMethod()> Public Sub getList_ParSiegeAvecInactives_AjouteLesUnitesFermeesDeCeSiegeSeulement()
        Dim idSiege = CreerSiege("IT Siege fiche")
        Dim idAutreSiege = CreerSiege("IT Siege autre fiche")
        Dim idOuverte = CreerUniteSanitaire("IT Fiche unite A", siegeId:=idSiege)
        Dim idFermee = CreerUniteSanitaire("IT Fiche unite B", siegeId:=idSiege, inactif:=True)
        CreerUniteSanitaire("IT Fiche unite C", siegeId:=idAutreSiege, inactif:=True)

        Dim liste = dao.getList(True, CInt(idSiege))

        CollectionAssert.AreEqual(New List(Of Integer) From {CInt(idOuverte), CInt(idFermee)}, IdsDe(liste))
    End Sub

    <TestMethod()> Public Sub getList_SiegeSansUnite_RenvoieUneListeVide()
        Dim idSiege = CreerSiege("IT Siege vide")

        Assert.AreEqual(0, dao.getList(False, CInt(idSiege)).Count)
    End Sub

    <TestMethod()> Public Sub getList_UnitesActives_AlimententLeFiltreEnClauseIn()
        ' FrmFiltreTacheATraiter : unités actives, puis TacheDao filtre par
        ' UniteSanitaire.GetQueryInForIds.
        Dim idSiege = CreerSiege("IT Siege clause")
        Dim idU1 = CreerUniteSanitaire("IT Clause unite 1", siegeId:=idSiege)
        Dim idU2 = CreerUniteSanitaire("IT Clause unite 2", siegeId:=idSiege)

        Dim clause = UniteSanitaire.GetQueryInForIds(dao.getList(False, CInt(idSiege)))

        Assert.AreEqual(" in ( " & idU1 & "," & idU2 & ") ", clause)
        Dim nombre = CInt(ScalaireSous(Compte.Client,
            "SELECT COUNT(*) FROM oasis.oa_unite_sanitaire WHERE oa_unite_sanitaire_id" & clause))
        Assert.AreEqual(2, nombre)
    End Sub

End Class
