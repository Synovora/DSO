Imports Oasis_Common

''' <summary>
''' NosCompetenceExclusiveDao (fichier R40_CompetenceExclusiveDao.vb) contre la base
''' de test. Il lit oasis.ans_nos_r40_competence_exclusive, nomenclature NOS R40 de
''' l'ANS (compétences exclusives), que l'application ne fait que lire ; JeuxTheriaque la
''' remplit. Seul appelant : RadFAnnuaireProfessionnelSelect, écran du client lourd,
''' donc sous oasis_client.
''' </summary>
<TestClass()> Public Class NosCompetenceExclusiveDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New NosCompetenceExclusiveDao

    <TestMethod()> Public Sub GetCompetenceExclusiveById_RelitLaLigneDuCode()
        CreerCompetenceExclusiveNos("ZZ41", "Compétence de test", "1.2.250.1.999.40.1")
        CreerCompetenceExclusiveNos("ZZ42", "Autre compétence de test", "1.2.250.1.999.40.2")

        Dim lu = dao.GetCompetenceExclusiveById("ZZ41")

        Assert.AreEqual("1.2.250.1.999.40.1", lu.Oid)
        Assert.AreEqual("ZZ41", lu.Code)
        Assert.AreEqual("Compétence de test", lu.Libelle)
    End Sub

    <TestMethod()> Public Sub GetCompetenceExclusiveById_LibelleNull_ChaineVide()
        If Not ColonneNullableTheriaque("oasis.ans_nos_r40_competence_exclusive", "libelle") Then
            Assert.Inconclusive("oasis.ans_nos_r40_competence_exclusive.libelle est NOT NULL dans ce schéma.")
        End If
        CreerCompetenceExclusiveNos("ZZ41", Nothing, "1.2.250.1.999.40.1")

        Assert.AreEqual("", dao.GetCompetenceExclusiveById("ZZ41").Libelle)
    End Sub

    <TestMethod()> Public Sub GetCompetenceExclusiveById_CodeInconnu_LeveArgumentException()
        CreerCompetenceExclusiveNos("ZZ41", "Compétence de test", "1.2.250.1.999.40.1")

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetCompetenceExclusiveById("ZZ49"))
        Assert.AreEqual("Compétence exclusive inexistante !", erreur.Message)
    End Sub

End Class
