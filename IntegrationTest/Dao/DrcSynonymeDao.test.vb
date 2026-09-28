Imports Oasis_Common

''' <summary>
''' DrcSynonymeDao contre la base de test. La liste des DRC (RadFDrcListe) en lit
''' les synonymes : sous oasis_client. Les synonymes sont écrits par l'écran
''' RadFDrcDetailEdit, reproduit par JeuxDrc.AjouterSynonymeDrc.
''' </summary>
<TestClass()> Public Class DrcSynonymeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New DrcSynonymeDao

    <TestMethod()> Public Sub getAllSynonymebyDrc_DonneLesSynonymesDeLaDrcDansLOrdreDeCreation()
        Dim idDrc = CreerDrc("Hypertension artérielle")
        Dim autreDrc = CreerDrc("Asthme")
        Dim premier = AjouterSynonymeDrc(idDrc, "HTA")
        AjouterSynonymeDrc(autreDrc, "Bronchospasme")
        Dim second = AjouterSynonymeDrc(idDrc, "Tension élevée")

        Dim table = dao.getAllSynonymebyDrc(CInt(idDrc))

        CollectionAssert.AreEqual(New String() {"oa_drc_synonyme_id", "oa_drc_synonyme_libelle"},
                                  table.Columns.Cast(Of DataColumn)().Select(Function(c) c.ColumnName).ToArray())
        Assert.AreEqual(2, table.Rows.Count)
        Assert.AreEqual(premier, CLng(table.Rows(0)("oa_drc_synonyme_id")))
        Assert.AreEqual("HTA", CStr(table.Rows(0)("oa_drc_synonyme_libelle")))
        Assert.AreEqual(second, CLng(table.Rows(1)("oa_drc_synonyme_id")))
        Assert.AreEqual("Tension élevée", CStr(table.Rows(1)("oa_drc_synonyme_libelle")))
    End Sub

    <TestMethod()> Public Sub getAllSynonymebyDrc_DrcSansSynonyme_TableVide()
        Assert.AreEqual(0, dao.getAllSynonymebyDrc(CInt(CreerDrc())).Rows.Count)
    End Sub

End Class
