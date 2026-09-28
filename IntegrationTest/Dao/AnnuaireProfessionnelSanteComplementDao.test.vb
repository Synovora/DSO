Imports Oasis_Common

''' <summary>
''' AnnuaireProfessionnelSanteComplementDao contre la base : complément de
''' coordonnées rattaché à une entrée de l'annuaire de référence. La méthode n'a
''' aucun appelant dans le code actuel ; la classe vit avec les écrans d'annuaire du
''' client lourd, d'où l'exécution sous oasis_client.
''' </summary>
<TestClass()> Public Class AnnuaireProfessionnelSanteComplementDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AnnuaireProfessionnelSanteComplementDao

    <TestMethod()> Public Sub GetAnnuaireProfessionnelById_ComplementExistant_RelitSesColonnes()
        Dim daoReference As New AnnuaireReferenceDao
        Dim cle = daoReference.CreationAnnuaireReference(ProfessionnelDeTest("ITCOMPLEMENT"), New Utilisateur)
        CreerComplementAnnuaire(cle, "MAISON DE SANTE DE TEST")

        Dim lu = dao.GetAnnuaireProfessionnelById(CInt(cle))

        Assert.AreEqual(CInt(cle), lu.Cle_entree)
        Assert.AreEqual("MAISON DE SANTE DE TEST", lu.RaisonSociale)
        Assert.AreEqual("4 allee du Complement", lu.Adresse1)
        Assert.AreEqual("Complement d'adresse", lu.Adresse2)
        Assert.AreEqual("0269000041", lu.Telephone)
        Assert.AreEqual("0269000042", lu.Telecopie)
        Assert.AreEqual("complement@exemple.fr", lu.EmailStructure)
    End Sub

    <TestMethod()> Public Sub GetAnnuaireProfessionnelById_EntreeSansComplement_LeveArgumentException()
        ' L'entrée existe dans l'annuaire de référence, mais sans complément.
        Dim daoReference As New AnnuaireReferenceDao
        Dim cle = daoReference.CreationAnnuaireReference(ProfessionnelDeTest("ITSANSCOMPLEMENT"), New Utilisateur)

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetAnnuaireProfessionnelById(CInt(cle)))
        StringAssert.Contains(erreur.Message, "Professionnel de santé inexistant")
    End Sub

End Class
