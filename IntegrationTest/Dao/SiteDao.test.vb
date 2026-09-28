Imports Oasis_Common

''' <summary>
''' SiteDao contre la base. Les sites délimitent ce qu'un utilisateur voit : le
''' filtre des tâches (FrmTacheMain.initFiltre, FrmFiltreTacheATraiter) part du site
''' ou de l'unité sanitaire de l'utilisateur connecté, la fiche utilisateur
''' (FrmUtilisateur) propose les sites d'une unité, les éditions (PrtOrdonnance,
''' OasisTextTools) lisent le site du patient. Tous ces appelants sont dans le client
''' lourd : tout tourne sous oasis_client.
''' </summary>
<TestClass()> Public Class SiteDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SiteDao

    Private Shared Function IdsDe(liste As List(Of Site)) As List(Of Long)
        Return liste.Select(Function(s) s.Oa_site_id).ToList()
    End Function

    ' --- getSiteById -------------------------------------------------------------

    <TestMethod()> Public Sub getSiteById_SiteActif_RenvoieToutesSesColonnes()
        Dim idUnite = CreerUniteSanitaire("IT Unite du site")
        Dim idSite = CreerSite("IT Site complet", uniteSanitaireId:=idUnite, territoireId:=7)

        Dim lu = dao.getSiteById(CInt(idSite))

        Assert.AreEqual(idSite, lu.Oa_site_id)
        Assert.AreEqual("IT Site complet", lu.Oa_site_description)
        Assert.AreEqual(7, CInt(lu.Oa_site_territoire_id))
        Assert.AreEqual(idUnite, lu.Oa_site_unite_sanitaire_id)
        Assert.AreEqual("3 chemin du Site", lu.Oa_site_adresse1)
        Assert.AreEqual("Lieu-dit Test", lu.Oa_site_adresse2)
        Assert.AreEqual("Sada", lu.Oa_site_ville)
        Assert.AreEqual("97640", lu.Oa_site_code_postal)
        Assert.AreEqual("0269000021", lu.Telephone)
        Assert.AreEqual("site@exemple.fr", lu.Mail)
        Assert.AreEqual("0269000022", lu.Fax)
        Assert.IsFalse(lu.Oa_site_inactif)
    End Sub

    <TestMethod()> Public Sub getSiteById_SiteInactif_EstIgnoreSaufSiDemande()
        Dim idSite = CreerSite("IT Site ferme", inactif:=True)

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getSiteById(CInt(idSite)))

        ' TacheDao relit le site d'une tâche avec isWithInactif à True.
        Dim lu = dao.getSiteById(CInt(idSite), True)
        Assert.AreEqual(idSite, lu.Oa_site_id)
        Assert.IsTrue(lu.Oa_site_inactif)
    End Sub

    <TestMethod()> Public Sub getSiteById_SiteInexistant_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.getSiteById(987654321, True))
        StringAssert.Contains(erreur.Message, "Site non retrouvé")
    End Sub

    <TestMethod()> Public Sub getSiteById_IdZero_LeveArgumentException()
        ' Patient ou utilisateur sans site : les éditions appellent quand même getSiteById(0).
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getSiteById(0))
    End Sub

    <TestMethod()> Public Sub getSiteById_ColonnesFacultativesNulles_DonnentLesValeursParDefaut()
        ' inactif NULL : le site compte comme actif (COALESCE à 0 dans la requête).
        Dim idSite = CreerSite("IT Site sans rattachement", inactif:=Nothing)

        Dim lu = dao.getSiteById(CInt(idSite))

        Assert.AreEqual(0, CInt(lu.Oa_site_territoire_id))
        Assert.AreEqual(0L, lu.Oa_site_unite_sanitaire_id)
        Assert.IsFalse(lu.Oa_site_inactif)
    End Sub

    ' --- getList -----------------------------------------------------------------

    <TestMethod()> Public Sub getList_SansInactifs_ExclutLesSitesFermesEtTrieParDescription()
        Dim idB = CreerSite("IT Liste B")
        Dim idA = CreerSite("IT Liste A")
        Dim idNull = CreerSite("IT Liste C", inactif:=Nothing)
        Dim idFerme = CreerSite("IT Liste D", inactif:=True)

        Dim ids = IdsDe(dao.getList(False))

        CollectionAssert.DoesNotContain(ids, idFerme)
        Dim miens = ids.Where(Function(i) i = idA OrElse i = idB OrElse i = idNull).ToList()
        CollectionAssert.AreEqual(New List(Of Long) From {idA, idB, idNull}, miens, "ORDER BY oa_site_description")
    End Sub

    <TestMethod()> Public Sub getList_AvecInactifs_RenvoieAussiLesSitesFermes()
        Dim idOuvert = CreerSite("IT Tous A")
        Dim idFerme = CreerSite("IT Tous B", inactif:=True)

        Dim ids = IdsDe(dao.getList(True))

        CollectionAssert.Contains(ids, idOuvert)
        CollectionAssert.Contains(ids, idFerme)
    End Sub

    <TestMethod()> Public Sub getList_ParUniteSanitaire_NeRenvoieQueLesSitesActifsDeCetteUnite()
        ' Cas de FrmTacheMain.initFiltre : utilisateur rattaché à une unité sans site précis.
        Dim idUnite = CreerUniteSanitaire("IT Unite filtree")
        Dim idAutreUnite = CreerUniteSanitaire("IT Unite voisine")
        Dim idS2 = CreerSite("IT Filtre 2", uniteSanitaireId:=idUnite)
        Dim idS1 = CreerSite("IT Filtre 1", uniteSanitaireId:=idUnite)
        CreerSite("IT Filtre ferme", uniteSanitaireId:=idUnite, inactif:=True)
        CreerSite("IT Filtre voisin", uniteSanitaireId:=idAutreUnite)
        CreerSite("IT Filtre sans unite")

        Dim liste = dao.getList(False, CInt(idUnite))

        CollectionAssert.AreEqual(New List(Of Long) From {idS1, idS2}, IdsDe(liste))
        Assert.IsTrue(liste.All(Function(s) s.Oa_site_unite_sanitaire_id = idUnite))
    End Sub

    <TestMethod()> Public Sub getList_ParUniteSanitaireAvecInactifs_AjouteLesSitesFermesDeCetteUniteSeulement()
        ' Fiche utilisateur en modification : isWithInactif à True, le filtre d'unité
        ' s'ajoute alors par un WHERE et non par un AND.
        Dim idUnite = CreerUniteSanitaire("IT Unite fiche")
        Dim idAutreUnite = CreerUniteSanitaire("IT Unite autre fiche")
        Dim idOuvert = CreerSite("IT Fiche A", uniteSanitaireId:=idUnite)
        Dim idFerme = CreerSite("IT Fiche B", uniteSanitaireId:=idUnite, inactif:=True)
        CreerSite("IT Fiche C", uniteSanitaireId:=idAutreUnite, inactif:=True)

        Dim liste = dao.getList(True, CInt(idUnite))

        CollectionAssert.AreEqual(New List(Of Long) From {idOuvert, idFerme}, IdsDe(liste))
    End Sub

    <TestMethod()> Public Sub getList_UniteSansSiteActif_RenvoieUneListeVide()
        ' Une liste vide donne " in ( ) " dans Site.GetQueryInForIds : le filtre de
        ' tâches qui la reçoit produirait une requête invalide.
        Dim idUnite = CreerUniteSanitaire("IT Unite sans site ouvert")
        CreerSite("IT Seul site ferme", uniteSanitaireId:=idUnite, inactif:=True)

        Dim liste = dao.getList(False, CInt(idUnite))

        Assert.AreEqual(0, liste.Count)
    End Sub

    <TestMethod()> Public Sub getList_SitesDUneUnite_AlimententLeFiltreEnClauseIn()
        Dim idUnite = CreerUniteSanitaire("IT Unite clause")
        Dim idS1 = CreerSite("IT Clause 1", uniteSanitaireId:=idUnite)
        Dim idS2 = CreerSite("IT Clause 2", uniteSanitaireId:=idUnite)

        Dim clause = Site.GetQueryInForIds(dao.getList(False, CInt(idUnite)))

        Assert.AreEqual(" in ( " & idS1 & "," & idS2 & ") ", clause)
        Dim nombre = CInt(ScalaireSous(Compte.Client, "SELECT COUNT(*) FROM oasis.oa_site WHERE oa_site_id" & clause))
        Assert.AreEqual(2, nombre)
    End Sub

End Class
