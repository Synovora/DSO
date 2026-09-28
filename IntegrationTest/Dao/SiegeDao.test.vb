Imports Oasis_Common

''' <summary>
''' SiegeDao contre la base. Le siège est le sommet du périmètre (siège, unités
''' sanitaires, sites). Le client lourd lit le siège du patient pour les éditions et
''' les courriels (PrtOrdonnance, OasisTextTools, FrmMailOrdonnance) et la liste des
''' sièges dans FrmUtilisateur : sous oasis_client. MailOasis.Process lit aussi le
''' siège du patient et peut tourner côté serveur : getSiegeById est donc éprouvé
''' sous oasis_web également.
''' </summary>
<TestClass()> Public Class SiegeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SiegeDao

    Private Shared Function IdsDe(table As DataTable) As List(Of Long)
        Dim ids As New List(Of Long)
        For Each ligne As DataRow In table.Rows
            ids.Add(CLng(ligne("oa_siege_id")))
        Next
        Return ids
    End Function

    ' --- getSiegeById ------------------------------------------------------------

    <TestMethod()> Public Sub getSiegeById_SiegeActif_RenvoieToutesSesColonnes()
        Dim idSiege = CreerSiege("IT Siege complet")

        Dim lu = dao.getSiegeById(CInt(idSiege))

        Assert.AreEqual(idSiege, lu.SiegeId)
        Assert.AreEqual("IT Siege complet", lu.SiegeDescription)
        Assert.AreEqual("1 place du Siege", lu.SiegeAdresse1)
        Assert.AreEqual("BP 1", lu.SiegeAdresse2)
        Assert.AreEqual("Mamoudzou", lu.SiegeVille)
        Assert.AreEqual("97600", lu.SiegeCodePostal)
        Assert.AreEqual("0269000001", lu.SiegeTelephone)
        Assert.AreEqual("siege@exemple.fr", lu.SiegeMail)
        Assert.AreEqual("0269000002", lu.SiegeFax)
        Assert.AreEqual("A", lu.SiegeStatut.Trim())
    End Sub

    <TestMethod()> Public Sub getSiegeById_SousWeb_RenvoieLeSiege()
        UtiliserCompte(Compte.Web)
        Dim idSiege = CreerSiege("IT Siege lu par le serveur")

        Dim lu = dao.getSiegeById(CInt(idSiege))

        Assert.AreEqual(idSiege, lu.SiegeId)
        Assert.AreEqual("IT Siege lu par le serveur", lu.SiegeDescription)
    End Sub

    <TestMethod()> Public Sub getSiegeById_StatutNul_CompteCommeActif()
        ' Comportement actuel : un statut NULL passe le filtre (COALESCE à 'A'), et
        ' BuildBean le remplace par False, que la propriété String reçoit en "False".
        Dim idSiege = CreerSiege("IT Siege sans statut", statut:=Nothing)

        Dim lu = dao.getSiegeById(CInt(idSiege))

        Assert.AreEqual(idSiege, lu.SiegeId)
        Assert.AreEqual("False", lu.SiegeStatut)
    End Sub

    <TestMethod()> Public Sub getSiegeById_SiegeInactif_EstIgnoreSaufSiDemande()
        Dim idSiege = CreerSiege("IT Siege ferme", statut:="I")

        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getSiegeById(CInt(idSiege)))

        Dim lu = dao.getSiegeById(CInt(idSiege), True)
        Assert.AreEqual(idSiege, lu.SiegeId)
        Assert.AreEqual("I", lu.SiegeStatut.Trim())
    End Sub

    <TestMethod()> Public Sub getSiegeById_SiegeInexistant_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.getSiegeById(987654321, True))
        StringAssert.Contains(erreur.Message, "Siege non retrouvé")
    End Sub

    <TestMethod()> Public Sub getSiegeById_PatientSansSiege_LeveArgumentException()
        ' MailOasis.Process et FrmMailOrdonnance appellent getSiegeById(PatientSiegeId)
        ' sans garde : un patient sans siège (0) fait échouer la composition du courriel.
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getSiegeById(0))
    End Sub

    ' --- getTableSiege -----------------------------------------------------------

    <TestMethod()> Public Sub getTableSiege_SansInactifs_ExclutLesSiegesFermesEtTrieParDescription()
        Dim idB = CreerSiege("IT Table siege B")
        Dim idA = CreerSiege("IT Table siege A")
        Dim idNul = CreerSiege("IT Table siege C", statut:=Nothing)
        Dim idFerme = CreerSiege("IT Table siege D", statut:="I")

        Dim ids = IdsDe(dao.getTableSiege())

        CollectionAssert.DoesNotContain(ids, idFerme)
        Dim miens = ids.Where(Function(i) i = idA OrElse i = idB OrElse i = idNul).ToList()
        CollectionAssert.AreEqual(New List(Of Long) From {idA, idB, idNul}, miens, "ORDER BY oa_siege_description")
    End Sub

    <TestMethod()> Public Sub getTableSiege_AvecInactifs_RenvoieAussiLesSiegesFermes()
        Dim idOuvert = CreerSiege("IT Tous sieges A")
        Dim idFerme = CreerSiege("IT Tous sieges B", statut:="I")

        Dim table = dao.getTableSiege(True)

        Dim ids = IdsDe(table)
        CollectionAssert.Contains(ids, idOuvert)
        CollectionAssert.Contains(ids, idFerme)
        Dim ligne = table.Select("oa_siege_id = " & idFerme).Single()
        Assert.AreEqual("IT Tous sieges B", CStr(ligne("oa_siege_description")))
    End Sub

    ' --- getLstSiege -------------------------------------------------------------

    <TestMethod()> Public Sub getLstSiege_AvecDesSieges_LesLitTous()
        ' Régression du commit e431700f : BuildBean était passé à IDataRecord alors
        ' que getLstSiege lui donnait une DataRow, et FrmUtilisateur plantait à
        ' l'ouverture dès qu'un siège existait.
        Dim idActif = CreerSiege("IT Siege en liste")
        Dim idInactif = CreerSiege("IT Siege en liste inactif", statut:="I")

        Dim actifs = dao.getLstSiege()
        Assert.IsTrue(actifs.Exists(Function(s) s.SiegeId = idActif))
        Assert.IsFalse(actifs.Exists(Function(s) s.SiegeId = idInactif))
        Assert.AreEqual("IT Siege en liste", actifs.Find(Function(s) s.SiegeId = idActif).SiegeDescription)

        Dim tous = dao.getLstSiege(True)
        Assert.IsTrue(tous.Exists(Function(s) s.SiegeId = idActif))
        Assert.IsTrue(tous.Exists(Function(s) s.SiegeId = idInactif))
    End Sub

End Class
