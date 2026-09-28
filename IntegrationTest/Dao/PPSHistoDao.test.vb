Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' PPSHistoDao contre la base de test : la liste d'historique d'un PPS
''' (RadFPPSHistoListe), sous oasis_client. L'historique est écrit par PpsDao, qui
''' formate la date de début selon la culture courante : les tests tournent en
''' français comme le client lourd.
''' </summary>
<TestClass()> Public Class PPSHistoDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New PPSHistoDao

    Private cultureInitiale As CultureInfo

    <TestInitialize>
    Public Sub PasserEnFrancais()
        cultureInitiale = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
    End Sub

    <TestCleanup>
    Public Sub RetablirLaCulture()
        If cultureInitiale IsNot Nothing Then Thread.CurrentThread.CurrentCulture = cultureInitiale
    End Sub

    <TestMethod()> Public Sub GetAllPPSHistobyPPSId_DeLaPlusRecenteALaPlusAncienneAvecLeLibelleDrc()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrcInitiale = CreerDrc("Surpoids")
        Dim idDrcRevue = CreerDrc("Obésité")
        Dim idPps = CreerPps(idPatient, idUtilisateur, 1, 1, drcId:=idDrcInitiale, commentaire:="Initial")
        Dim autre = CreerPps(idPatient, idUtilisateur, 2, 2)
        Dim daoPps As New PpsDao
        Dim auteur As New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
        Dim modifie = daoPps.getPpsById(CInt(idPps))
        modifie.DrcId = CInt(idDrcRevue)
        modifie.Commentaire = "Revu"
        daoPps.ModificationPPS(modifie, auteur)
        modifie.ArretCommentaire = "Atteint"
        daoPps.AnnulationPrevention(modifie, auteur)

        Dim table = dao.GetAllPPSHistobyPPSId(CInt(idPps))

        Assert.AreEqual(3, table.Rows.Count)
        Dim etats = table.Rows.Cast(Of DataRow)().Select(Function(r) CInt(r("oa_pps_histo_etat_historisation"))).ToArray()
        CollectionAssert.AreEqual(New Integer() {4, 2, 1}, etats)
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) CLng(r("oa_pps_id")) = idPps))
        Assert.AreEqual("Obésité", CStr(table.Rows(0)("oa_drc_libelle")))
        Assert.AreEqual("Atteint", CStr(table.Rows(0)("oa_pps_commentaire_arret")))
        Assert.IsTrue(CBool(table.Rows(0)("oa_pps_inactif")))
        Assert.AreEqual("Revu", CStr(table.Rows(1)("oa_pps_commentaire")))
        Assert.AreEqual("Surpoids", CStr(table.Rows(2)("oa_drc_libelle")))
        Assert.AreEqual("Initial", CStr(table.Rows(2)("oa_pps_commentaire")))
        Assert.AreEqual(idUtilisateur, CLng(table.Rows(2)("oa_pps_histo_utilisateur_historisation")))
        Assert.AreEqual(1, dao.GetAllPPSHistobyPPSId(CInt(autre)).Rows.Count)
    End Sub

    <TestMethod()> Public Sub GetAllPPSHistobyPPSId_SansHistorique_TableVide()
        Assert.AreEqual(0, dao.GetAllPPSHistobyPPSId(987654321).Rows.Count)
    End Sub

End Class
