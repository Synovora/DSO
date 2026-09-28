Imports Oasis_Common

''' <summary>
''' AntecedentHistoDao contre la base de test : la liste d'historique d'un
''' antécédent (RadFAntecedentHistoListe), sous oasis_client.
''' </summary>
<TestClass()> Public Class AntecedentHistoDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AntecedentHistoDao

    <TestMethod()> Public Sub GetAllAntecedentHisto_DeLaPlusRecenteALaPlusAncienne()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAntecedent = CreerAntecedent(idPatient, idUtilisateur, description:="Initiale")
        Dim autre = CreerAntecedent(idPatient, idUtilisateur)
        Dim daoAntecedent As New AntecedentDao
        Dim auteur As New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
        Dim lu = daoAntecedent.GetAntecedentById(CInt(idAntecedent))
        Dim modifie = daoAntecedent.Clone(lu)
        modifie.Description = "Revue"
        daoAntecedent.ModificationAntecedent(modifie, lu, auteur)
        daoAntecedent.AnnulationAntecedent(modifie, daoAntecedent.GetAntecedentById(CInt(idAntecedent)), auteur)

        Dim table = dao.getAllAntecedentHistobyAntecedentId(CInt(idAntecedent))

        Assert.AreEqual(3, table.Rows.Count)
        Dim etats = table.Rows.Cast(Of DataRow)().Select(Function(r) CInt(r("oa_antecedent_histo_etat_historisation"))).ToArray()
        CollectionAssert.AreEqual(New Integer() {4, 2, 1}, etats)
        Dim idsHisto = table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("oa_antecedent_histo_id"))).ToArray()
        Assert.IsTrue(idsHisto(0) > idsHisto(1) AndAlso idsHisto(1) > idsHisto(2))
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) CLng(r("oa_antecedent_id")) = idAntecedent))
        Assert.AreEqual("Revue", CStr(table.Rows(1)("oa_antecedent_description")))
        Assert.AreEqual("Initiale", CStr(table.Rows(2)("oa_antecedent_description")))
        Assert.IsTrue(CBool(table.Rows(0)("oa_antecedent_inactif")))
        Assert.AreEqual(1, dao.getAllAntecedentHistobyAntecedentId(CInt(autre)).Rows.Count)
    End Sub

    <TestMethod()> Public Sub GetAllAntecedentHisto_SansHistorique_TableVide()
        Assert.AreEqual(0, dao.getAllAntecedentHistobyAntecedentId(987654321).Rows.Count)
    End Sub

End Class
