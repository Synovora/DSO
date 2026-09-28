Imports Oasis_Common

''' <summary>
''' AntecedentDeplacementDao contre la base de test. GetAntecedentDeplacementById
''' n'a plus d'appelant dans le client lourd ni dans Oasis_Web ; il tourne sous
''' oasis_client, le compte qui lit oa_antecedent.
''' </summary>
<TestClass()> Public Class AntecedentDeplacementDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AntecedentDeplacementDao

    <TestMethod()> Public Sub GetAntecedentDeplacementById_RelitNiveauPeresEtOrdres()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim pere = CreerAntecedent(idPatient, idUtilisateur)
        Dim fils = CreerAntecedent(idPatient, idUtilisateur)
        Dim idAntecedent = CreerAntecedent(idPatient, idUtilisateur)
        PlacerAntecedent(idAntecedent, 3, pere, fils, 20, 40, 60)

        Dim lu = dao.GetAntecedentDeplacementById(CInt(idAntecedent))

        Assert.AreEqual(CInt(idAntecedent), lu.Id)
        Assert.AreEqual(3, lu.Niveau)
        Assert.AreEqual(CInt(pere), lu.Niveau1Id)
        Assert.AreEqual(CInt(fils), lu.Niveau2Id)
        Assert.AreEqual(20, lu.Ordre1)
        Assert.AreEqual(40, lu.Ordre2)
        Assert.AreEqual(60, lu.Ordre3)
    End Sub

    <TestMethod()> Public Sub GetAntecedentDeplacementById_AntecedentNeuf_ValeursDeCreation()
        Dim idAntecedent = CreerAntecedent(CreerPatient(), CreerUtilisateur(avecCle:=False))

        Dim lu = dao.GetAntecedentDeplacementById(CInt(idAntecedent))

        Assert.AreEqual(1, lu.Niveau)
        Assert.AreEqual(0, lu.Niveau1Id, "colonne non écrite à la création, NULL lu comme 0")
        Assert.AreEqual(0, lu.Niveau2Id)
        Assert.AreEqual(980, lu.Ordre1)
        Assert.AreEqual(0, lu.Ordre2)
        Assert.AreEqual(0, lu.Ordre3)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetAntecedentDeplacementById_Inexistant_LeveUneErreur()
        dao.GetAntecedentDeplacementById(987654321)
    End Sub

End Class
