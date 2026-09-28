Imports Oasis_Common

''' <summary>
''' ParametreDrcDao contre la base : paramètres rattachés à une DRC (groupe de
''' paramètres). Le client lourd lit, remplace et crée ces liens (RadFDrcParametresEdit,
''' RadFEpisodeParametresCreation) sous oasis_client, qui a le droit de supprimer
''' dans oa_drc_parametre (migration 2026-09-27). GetParametreDrcById et
''' ModificationParametreDrc n'ont pas d'appelant et tournent sous le même compte.
''' </summary>
<TestClass()> Public Class ParametreDrcDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ParametreDrcDao

    Private Function Associer(drcId As Long, parametreId As Long) As Long
        dao.CreationParametreDrc(New ParametreDrc With {.DrcId = drcId, .ParametreId = parametreId})
        Return CLng(Scalaire("SELECT MAX(id) FROM oasis.oa_drc_parametre WHERE drc_id = @p0 AND parametre_id = @p1",
                             drcId, parametreId))
    End Function

    Private Shared Function NombreDeLiens(drcId As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_drc_parametre WHERE drc_id = @p0", drcId))
    End Function

    <TestMethod()> Public Sub CreationParametreDrc_SousClient_EnregistreLeLien()
        Dim drcId = CreerDrc()
        Dim parametreId = CreerParametreDeMesure("IT poids")

        Dim id = Associer(drcId, parametreId)

        Assert.IsTrue(id > 0)
        Assert.AreEqual(1, NombreDeLiens(drcId))
    End Sub

    <TestMethod()> Public Sub GetParametreDrcById_SousClient_RelitLeLien()
        Dim drcId = CreerDrc()
        Dim parametreId = CreerParametreDeMesure("IT taille")
        Dim id = Associer(drcId, parametreId)

        Dim lu = dao.GetParametreDrcById(CInt(id))

        Assert.AreEqual(id, lu.Id)
        Assert.AreEqual(drcId, lu.DrcId)
        Assert.AreEqual(parametreId, lu.ParametreId)
    End Sub

    <TestMethod()> Public Sub GetParametreDrcById_Inexistant_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetParametreDrcById(987654321))
        StringAssert.Contains(erreur.Message, "Paramètre inexistant")
    End Sub

    <TestMethod()> Public Sub GetParametreByDrcAndId_SousClient_LienExistant_EstRenvoye()
        Dim drcId = CreerDrc()
        Dim parametreId = CreerParametreDeMesure("IT tension")
        Dim autreParametre = CreerParametreDeMesure("IT pouls")
        Dim id = Associer(drcId, parametreId)
        Associer(drcId, autreParametre)

        Dim lu = dao.GetParametreByDrcAndId(drcId, parametreId)

        Assert.AreEqual(id, lu.Id)
        Assert.AreEqual(drcId, lu.DrcId)
        Assert.AreEqual(parametreId, lu.ParametreId)
    End Sub

    <TestMethod()> Public Sub GetParametreByDrcAndId_SousClient_LienAbsent_RenvoieUnLienVide()
        ' L'écran teste Id <> 0 pour cocher la case : un lien absent est un bean à zéro.
        Dim drcId = CreerDrc()
        Dim autreDrc = CreerDrc()
        Dim parametreId = CreerParametreDeMesure("IT temperature")
        Associer(autreDrc, parametreId)

        Dim lu = dao.GetParametreByDrcAndId(drcId, parametreId)

        Assert.AreEqual(0L, lu.Id)
        Assert.AreEqual(0L, lu.DrcId)
        Assert.AreEqual(0L, lu.ParametreId)
    End Sub

    <TestMethod()> Public Sub ModificationParametreDrc_SousClient_DeplaceLeLien()
        Dim drcId = CreerDrc()
        Dim autreDrc = CreerDrc()
        Dim parametreId = CreerParametreDeMesure("IT saturation")
        Dim autreParametre = CreerParametreDeMesure("IT frequence")
        Dim id = Associer(drcId, parametreId)

        dao.ModificationParametreDrc(New ParametreDrc With {.Id = id, .DrcId = autreDrc, .ParametreId = autreParametre})

        Dim lu = dao.GetParametreDrcById(CInt(id))
        Assert.AreEqual(autreDrc, lu.DrcId)
        Assert.AreEqual(autreParametre, lu.ParametreId)
        Assert.AreEqual(0, NombreDeLiens(drcId))
    End Sub

    <TestMethod()> Public Sub ModificationParametreDrc_LienInexistant_NeFaitRien()
        Dim drcId = CreerDrc()
        Dim parametreId = CreerParametreDeMesure("IT glycemie")
        Associer(drcId, parametreId)

        dao.ModificationParametreDrc(New ParametreDrc With {.Id = 987654321, .DrcId = drcId, .ParametreId = parametreId})

        Assert.AreEqual(1, NombreDeLiens(drcId))
    End Sub

    <TestMethod()> Public Sub SuppressionParametreDrcByDrcId_SousClient_RetireLesLiensDeLaSeuleDrc()
        Dim drcId = CreerDrc()
        Dim autreDrc = CreerDrc()
        Dim premier = CreerParametreDeMesure("IT un")
        Dim second = CreerParametreDeMesure("IT deux")
        Associer(drcId, premier)
        Associer(drcId, second)
        Associer(autreDrc, premier)

        dao.SuppressionParametreDrcByDrcId(drcId)

        Assert.AreEqual(0, NombreDeLiens(drcId))
        Assert.AreEqual(1, NombreDeLiens(autreDrc))
    End Sub

    <TestMethod()> Public Sub SuppressionPuisCreation_CommeLEcran_RemplaceLaSelection()
        ' RadFDrcParametresEdit supprime tout puis recrée les paramètres cochés.
        Dim drcId = CreerDrc()
        Dim ancien = CreerParametreDeMesure("IT ancien")
        Dim nouveau = CreerParametreDeMesure("IT nouveau")
        Associer(drcId, ancien)

        dao.SuppressionParametreDrcByDrcId(drcId)
        Associer(drcId, nouveau)

        Assert.AreEqual(0L, dao.GetParametreByDrcAndId(drcId, ancien).Id)
        Assert.AreNotEqual(0L, dao.GetParametreByDrcAndId(drcId, nouveau).Id)
    End Sub

    <TestMethod()> Public Sub GetParametresByDrcId_SousClient_RenvoieLesLiensDeLaDrc()
        Dim drcId = CreerDrc()
        Dim autreDrc = CreerDrc()
        Dim premier = CreerParametreDeMesure("IT alpha")
        Dim second = CreerParametreDeMesure("IT beta")
        Associer(drcId, premier)
        Associer(drcId, second)
        Associer(autreDrc, premier)

        Dim table = dao.GetParametresByDrcId(drcId)

        Dim parametres = table.Rows.Cast(Of DataRow)().Select(Function(ligne) CLng(ligne("parametre_id"))).ToArray()
        CollectionAssert.AreEquivalent(New Long() {premier, second}, parametres)
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(ligne) CLng(ligne("drc_id")) = drcId))
    End Sub

    <TestMethod()> Public Sub GetParametresByDrcId_DrcSansParametre_TableVide()
        Dim drcId = CreerDrc()
        Assert.AreEqual(0, dao.GetParametresByDrcId(drcId).Rows.Count)
    End Sub

End Class
