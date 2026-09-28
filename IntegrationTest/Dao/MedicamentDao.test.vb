Imports Oasis_Common

''' <summary>
''' MedicamentDao contre la base de test. Il lit oa_r_medicament, reprise de la base
''' publique des médicaments que l'application ne fait que lire ; JeuxTheriaque la
''' remplit. Seul appelant : RadFAllergieEtCISuppressionDetail, écran du client
''' lourd, donc sous oasis_client.
''' </summary>
<TestClass()> Public Class MedicamentDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New MedicamentDao

    Private Const CisComplet As Integer = 63000001
    Private Const CisVoisin As Integer = 63000002
    Private Const CisSansDetail As Integer = 63000003
    Private Const CisInconnu As Integer = 63000009

    <TestMethod()> Public Sub GetMedicamentById_RelitToutesLesColonnes()
        CreerMedicamentReference(CisComplet, "PARACETAMOL TEST 500 mg", "comprimé", "LABORATOIRE TEST", "orale")
        CreerMedicamentReference(CisVoisin, "IBUPROFENE TEST 200 mg", "gélule", "AUTRE LABORATOIRE", "orale")

        Dim lu = dao.GetMedicamentById(CisComplet)

        Assert.AreEqual(CisComplet, lu.MedicamentCis)
        Assert.AreEqual("PARACETAMOL TEST 500 mg", lu.MedicamentDci)
        Assert.AreEqual("comprimé", lu.Forme)
        Assert.AreEqual("LABORATOIRE TEST", lu.Titulaire)
        Assert.AreEqual("orale", lu.VoieAdministration)
    End Sub

    <TestMethod()> Public Sub GetMedicamentById_ColonnesNull_DonnentDesChainesVides()
        For Each colonne In {"oa_medicament_dci", "oa_medicament_forme", "oa_medicament_titulaire", "oa_medicament_voie_administration"}
            If Not ColonneNullableTheriaque("oasis.oa_r_medicament", colonne) Then
                Assert.Inconclusive("oa_r_medicament." & colonne & " est NOT NULL dans ce schéma.")
            End If
        Next
        CreerMedicamentReference(CisSansDetail, Nothing, Nothing, Nothing, Nothing)

        Dim lu = dao.GetMedicamentById(CisSansDetail)

        Assert.AreEqual(CisSansDetail, lu.MedicamentCis)
        Assert.AreEqual("", lu.MedicamentDci)
        Assert.AreEqual("", lu.Forme)
        Assert.AreEqual("", lu.Titulaire)
        Assert.AreEqual("", lu.VoieAdministration)
    End Sub

    <TestMethod()> Public Sub GetMedicamentById_Inconnu_LeveArgumentException()
        CreerMedicamentReference(CisComplet, "PARACETAMOL TEST 500 mg", "comprimé", "LABORATOIRE TEST", "orale")

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetMedicamentById(CisInconnu))
        Assert.AreEqual("Médicament inexistant !", erreur.Message)
    End Sub

End Class
