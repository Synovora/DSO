Imports Oasis_Common

''' <summary>
''' AldCim10Dao contre la base de test. Il lit oa_ald_cim10, liste des codes CIM-10
''' ouvrant droit à chaque ALD, que l'application ne fait que lire ; JeuxTheriaque la
''' remplit, rattachée aux ALD de 29-reference-theriaque.sql. Appelants :
''' RadFAntecedentDetailEdit et RadFAldCim10Selecteur, écrans du client lourd, donc
''' sous oasis_client.
''' </summary>
<TestClass()> Public Class AldCim10DaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AldCim10Dao

    Private Const Cim10Absent As Integer = 987654321

    <TestMethod()> Public Sub GetAldCim10ById_RelitToutesLesColonnes()
        Dim id = CreerAldCim10Reference(AldReferenceDiabeteId, AldReferenceDiabeteCode, "E11", "Diabète de type 2")
        CreerAldCim10Reference(AldReferenceDiabeteId, AldReferenceDiabeteCode, "E10", "Diabète de type 1")

        Dim lu = dao.GetAldCim10ById(CInt(id))

        Assert.AreEqual(CInt(id), lu.AldCim10Id)
        Assert.AreEqual(AldReferenceDiabeteId, lu.AldCim10AldId)
        Assert.AreEqual(AldReferenceDiabeteCode, lu.AldCim10AldCode)
        Assert.AreEqual("E11", lu.AldCim10Code)
        Assert.AreEqual("Diabète de type 2", lu.AldCim10Description)
    End Sub

    <TestMethod()> Public Sub GetAldCim10ById_ColonnesNull_DonnentDesValeursParDefaut()
        For Each colonne In {"oa_ald_cim10_ald_id", "oa_ald_cim10_ald_code", "oa_ald_cim10_code", "oa_ald_cim10_description"}
            If Not ColonneNullableTheriaque("oasis.oa_ald_cim10", colonne) Then
                Assert.Inconclusive("oa_ald_cim10." & colonne & " est NOT NULL dans ce schéma.")
            End If
        Next
        Dim id = CreerAldCim10Reference(Nothing, Nothing, Nothing, Nothing)

        Dim lu = dao.GetAldCim10ById(CInt(id))

        Assert.AreEqual(0, lu.AldCim10AldId)
        Assert.AreEqual("", lu.AldCim10AldCode)
        Assert.AreEqual("", lu.AldCim10Code)
        Assert.AreEqual("", lu.AldCim10Description)
    End Sub

    <TestMethod()> Public Sub GetAldCim10ById_Inexistant_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetAldCim10ById(Cim10Absent))
        Assert.AreEqual("ALD Cim10 inexistante !", erreur.Message)
    End Sub

    <TestMethod()> Public Sub GetAllAldCim10ByAldId_SeulementLesCodesDeLAld()
        Dim idE11 = CreerAldCim10Reference(AldReferenceDiabeteId, AldReferenceDiabeteCode, "E11", "Diabète de type 2")
        Dim idE10 = CreerAldCim10Reference(AldReferenceDiabeteId, AldReferenceDiabeteCode, "E10", "Diabète de type 1")
        CreerAldCim10Reference(AldReferenceCardiaqueId, AldReferenceCardiaqueCode, "I50", "Insuffisance cardiaque")

        Dim liste = dao.GetAllAldCim10ByAldId(AldReferenceDiabeteId)

        ' Pas d'ORDER BY : on compare sans ordre.
        CollectionAssert.AreEquivalent(New Integer() {CInt(idE11), CInt(idE10)}, liste.Select(Function(c) c.AldCim10Id).ToArray())
        CollectionAssert.AreEquivalent(New String() {"E11", "E10"}, liste.Select(Function(c) c.AldCim10Code).ToArray())
        Assert.IsTrue(liste.All(Function(c) c.AldCim10AldId = AldReferenceDiabeteId))
    End Sub

    <TestMethod()> Public Sub GetAllAldCim10ByAldId_AldSansCode_ListeVide()
        CreerAldCim10Reference(AldReferenceDiabeteId, AldReferenceDiabeteCode, "E11", "Diabète de type 2")

        Assert.AreEqual(0, dao.GetAllAldCim10ByAldId(AldReferenceCardiaqueId).Count)
    End Sub

End Class
