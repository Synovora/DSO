Imports Oasis_Common

''' <summary>
''' SpecialiteDao contre la base de test. La liste du ROR du client lourd
''' (RadFRorListe) lit la spécialité choisie : sous oasis_client. Les lignes lues
''' viennent de Schema/28-reference-parcours.sql : oa_r_specialite alimente aussi le
''' singleton Table_specialite, aucun test n'y écrit.
''' </summary>
<TestClass()> Public Class SpecialiteDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SpecialiteDao

    <TestMethod()> Public Sub GetSpecialiteById_SpecialiteDeReference_RelueAvecToutesSesColonnes()
        Dim lue = dao.GetSpecialiteById(SpecialiteParcoursTest)

        Assert.AreEqual(CLng(SpecialiteParcoursTest), lue.SpecialiteId)
        Assert.AreEqual("ITCA", lue.Code)
        Assert.AreEqual(SpecialiteParcoursTestLibelle, lue.Description)
        Assert.AreEqual("MEDICAL", lue.Nature)
        Assert.IsTrue(lue.Parcours)
        Assert.IsFalse(lue.Oasis)
        Assert.AreEqual("", lue.Genre)
        Assert.AreEqual(0, lue.AgeMin)
        Assert.AreEqual(0, lue.AgeMax)
        Assert.AreEqual(45, lue.DelaiPriseEnCharge)
        Assert.AreEqual(10, lue.NosG15CodeProfession)
        Assert.AreEqual("S", lue.NosR40TypeSavoirFaire)
        Assert.AreEqual("SM04", lue.NosCodeSavoirFaire)
    End Sub

    <TestMethod()> Public Sub GetSpecialiteById_SpecialiteOasis_PorteLIndicateurOasis()
        Dim lue = dao.GetSpecialiteById(SpecialiteIdeOasis)

        Assert.AreEqual("IDE de test", lue.Description)
        Assert.AreEqual("PARAMEDICAL", lue.Nature)
        Assert.IsTrue(lue.Oasis)
        Assert.AreEqual(60, lue.DelaiPriseEnCharge)
    End Sub

    <TestMethod()> Public Sub GetSpecialiteById_SpecialiteInactive_ResteLisible()
        ' Le DAO ne filtre pas l'indicateur d'inactivité, contrairement au singleton.
        Dim lue = dao.GetSpecialiteById(SpecialiteParcoursInactive)

        Assert.AreEqual("Spécialité inactive de test", lue.Description)
        Assert.IsFalse(lue.Parcours)
        Assert.AreEqual("F", lue.Genre)
        Assert.AreEqual(12, lue.AgeMin)
        Assert.AreEqual(60, lue.AgeMax)
    End Sub

    <TestMethod()> Public Sub GetSpecialiteById_SpecialiteAbsente_LeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetSpecialiteById(987654))
        StringAssert.Contains(erreur.Message, "inexistante")
    End Sub

End Class
