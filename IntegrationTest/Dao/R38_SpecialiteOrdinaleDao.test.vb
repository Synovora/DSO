Imports Oasis_Common

''' <summary>
''' NosSpecialiteOrdinaleDao (fichier R38_SpecialiteOrdinaleDao.vb) contre la base de
''' test. Il lit oasis.ans_nos_r38_specialite_ordinale, nomenclature NOS R38 de l'ANS
''' (spécialités ordinales), que l'application ne fait que lire ; JeuxTheriaque la
''' remplit. Seul appelant : RadFAnnuaireProfessionnelSelect, écran du client lourd,
''' donc sous oasis_client.
''' </summary>
<TestClass()> Public Class NosSpecialiteOrdinaleDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New NosSpecialiteOrdinaleDao

    <TestMethod()> Public Sub GetSpecialiteOrdinaleById_RelitLaLigneDuCode()
        CreerSpecialiteOrdinaleNos("ZZ01", "Spécialité de test", "1.2.250.1.999.38.1")
        CreerSpecialiteOrdinaleNos("ZZ02", "Autre spécialité de test", "1.2.250.1.999.38.2")

        Dim lu = dao.GetSpecialiteOrdinaleById("ZZ01")

        Assert.AreEqual("1.2.250.1.999.38.1", lu.Oid)
        Assert.AreEqual("ZZ01", lu.Code)
        Assert.AreEqual("Spécialité de test", lu.Libelle)
    End Sub

    <TestMethod()> Public Sub GetSpecialiteOrdinaleById_LibelleNull_ChaineVide()
        If Not ColonneNullableTheriaque("oasis.ans_nos_r38_specialite_ordinale", "libelle") Then
            Assert.Inconclusive("oasis.ans_nos_r38_specialite_ordinale.libelle est NOT NULL dans ce schéma.")
        End If
        CreerSpecialiteOrdinaleNos("ZZ01", Nothing, "1.2.250.1.999.38.1")

        Assert.AreEqual("", dao.GetSpecialiteOrdinaleById("ZZ01").Libelle)
    End Sub

    <TestMethod()> Public Sub GetSpecialiteOrdinaleById_CodeInconnu_LeveArgumentException()
        CreerSpecialiteOrdinaleNos("ZZ01", "Spécialité de test", "1.2.250.1.999.38.1")

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetSpecialiteOrdinaleById("ZZ99"))
        Assert.AreEqual("Spécialité ordinale inexistante !", erreur.Message)
    End Sub

End Class
