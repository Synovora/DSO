Imports Oasis_Common

''' <summary>
''' NosProfessionSanteDao (fichier G15_ProfessionSanteDao.vb) contre la base de test.
''' Il lit oasis.ans_nos_g15_profession_sante, nomenclature NOS G15 de l'ANS
''' (professions de santé), que l'application ne fait que lire ; JeuxTheriaque la
''' remplit. Seul appelant : RadFAnnuaireProfessionnelSelect, écran du client lourd,
''' donc sous oasis_client.
''' </summary>
<TestClass()> Public Class NosProfessionSanteDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New NosProfessionSanteDao

    <TestMethod()> Public Sub GetProfessionSanteById_RelitLaLigneDuCode()
        CreerProfessionSanteNos(9910, "Profession de test", "1.2.250.1.999.15.1")
        CreerProfessionSanteNos(9920, "Autre profession de test", "1.2.250.1.999.15.2")

        Dim lu = dao.GetProfessionSanteById(9910)

        Assert.AreEqual("1.2.250.1.999.15.1", lu.Oid)
        Assert.AreEqual(9910, lu.Code)
        Assert.AreEqual("Profession de test", lu.Libelle)
    End Sub

    <TestMethod()> Public Sub GetProfessionSanteById_LibelleNull_ChaineVide()
        If Not ColonneNullableTheriaque("oasis.ans_nos_g15_profession_sante", "libelle") Then
            Assert.Inconclusive("oasis.ans_nos_g15_profession_sante.libelle est NOT NULL dans ce schéma.")
        End If
        CreerProfessionSanteNos(9910, Nothing, "1.2.250.1.999.15.1")

        Assert.AreEqual("", dao.GetProfessionSanteById(9910).Libelle)
    End Sub

    <TestMethod()> Public Sub GetProfessionSanteById_CodeInconnu_LeveArgumentException()
        CreerProfessionSanteNos(9910, "Profession de test", "1.2.250.1.999.15.1")

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetProfessionSanteById(9990))
        Assert.AreEqual("Profession de santé inexistante !", erreur.Message)
    End Sub

End Class
