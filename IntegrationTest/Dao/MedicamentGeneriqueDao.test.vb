Imports System.Collections.Specialized
Imports Oasis_Common

''' <summary>
''' MedicamentGeneriqueDao (module) contre la base de test. Il lit la base
''' médicamenteuse externe reprise dans le schéma (vue v_medoc, table
''' oa_medicament_gener des groupes génériques). La vue v_medoc repose sur des tables
''' que l'application ne remplit pas : elle est vide dans la base de test, et le
''' comptage n'est éprouvé que contre la même requête écrite à la main. Les groupes
''' génériques sont remplis par JeuxTraitement.
'''
''' GetCountMedicamentParDci est appelé par RadFMedocSelecteur (dossier Obsolete,
''' toujours compilé) : sous oasis_client. TraitementAllergies n'a aucun appelant ;
''' il tourne aussi sous oasis_client.
''' </summary>
<TestClass()> Public Class MedicamentGeneriqueDaoTest
    Inherits TestIntegration

    Private Const CisAllergene As Integer = 61000001
    Private Const CisGeneriqueA As Integer = 61000002
    Private Const CisGeneriqueB As Integer = 61000003
    Private Const CisSansRapport As Integer = 61000004
    Private Const CisSansGroupe As Integer = 61000005

    Private Shared Function Elements(collection As StringCollection) As List(Of String)
        Return collection.Cast(Of String)().ToList()
    End Function

    ' --- Comptage par DCI ---------------------------------------------------------------

    <TestMethod()> Public Sub GetCountMedicamentParDci_SansDebut_CompteToutLaVue()
        Dim total = CInt(Scalaire("SELECT COUNT(*) FROM oasis.v_medoc"))

        Assert.AreEqual(total, MedicamentGeneriqueDao.GetCountMedicamentParDci(""))
        Assert.AreEqual(total, MedicamentGeneriqueDao.GetCountMedicamentParDci(Nothing))
    End Sub

    <TestMethod()> Public Sub GetCountMedicamentParDci_Debut_CompteLesDciQuiCommencentAinsi()
        Dim attendu = CInt(Scalaire("SELECT COUNT(*) FROM oasis.v_medoc WHERE oa_medicament_dci LIKE @p0", "PARA%"))
        Assert.AreEqual(attendu, MedicamentGeneriqueDao.GetCountMedicamentParDci("PARA"))
    End Sub

    <TestMethod()> Public Sub GetCountMedicamentParDci_DebutInconnu_Zero()
        Assert.AreEqual(0, MedicamentGeneriqueDao.GetCountMedicamentParDci("ZZQXW IT"))
    End Sub

    <TestMethod()> Public Sub GetCountMedicamentParDci_JokersSaisis_SontPrisALaLettre()
        ' La saisie passe par EchapperLike : « % » ne désigne plus n'importe quelle DCI.
        Dim litteral = CInt(Scalaire("SELECT COUNT(*) FROM oasis.v_medoc WHERE oa_medicament_dci LIKE @p0", "[%]%"))
        Assert.AreEqual(litteral, MedicamentGeneriqueDao.GetCountMedicamentParDci("%"))
    End Sub

    ' --- Groupes génériques des allergies --------------------------------------------

    <TestMethod()> Public Sub TraitementAllergies_AjouteTousLesMedicamentsDesGroupesDeLAllergene()
        AjouterAuGroupeGenerique(940001, CisAllergene)
        AjouterAuGroupeGenerique(940001, CisGeneriqueA)
        AjouterAuGroupeGenerique(940002, CisAllergene)
        AjouterAuGroupeGenerique(940002, CisGeneriqueB)
        AjouterAuGroupeGenerique(940003, CisSansRapport)
        Dim dossier As New Patient
        dossier.PatientAllergieCis.Add(CisAllergene.ToString())

        MedicamentGeneriqueDao.TraitementAllergies(dossier)

        ' Comportement actuel : un CIS présent dans deux groupes est ajouté une fois
        ' par groupe, l'allergène compris ; aucun dédoublonnage.
        Dim trouves = Elements(dossier.PatientAllergiesGénériquesCis)
        Assert.AreEqual(4, trouves.Count)
        Assert.AreEqual(2, trouves.Where(Function(c) c = CisAllergene.ToString()).Count())
        CollectionAssert.Contains(trouves, CisGeneriqueA.ToString())
        CollectionAssert.Contains(trouves, CisGeneriqueB.ToString())
        CollectionAssert.DoesNotContain(trouves, CisSansRapport.ToString())
    End Sub

    <TestMethod()> Public Sub TraitementAllergies_PlusieursAllergenes_CumuleLeursGroupes()
        AjouterAuGroupeGenerique(940011, CisAllergene)
        AjouterAuGroupeGenerique(940011, CisGeneriqueA)
        AjouterAuGroupeGenerique(940012, CisSansRapport)
        AjouterAuGroupeGenerique(940012, CisGeneriqueB)
        Dim dossier As New Patient
        dossier.PatientAllergieCis.Add(CisAllergene.ToString())
        dossier.PatientAllergieCis.Add(CisSansRapport.ToString())

        MedicamentGeneriqueDao.TraitementAllergies(dossier)

        CollectionAssert.AreEquivalent(
            New String() {CisAllergene.ToString(), CisGeneriqueA.ToString(), CisSansRapport.ToString(), CisGeneriqueB.ToString()},
            Elements(dossier.PatientAllergiesGénériquesCis))
    End Sub

    <TestMethod()> Public Sub TraitementAllergies_AllergeneSansGroupe_VideLaListe()
        AjouterAuGroupeGenerique(940021, CisGeneriqueA)
        Dim dossier As New Patient
        dossier.PatientAllergieCis.Add(CisSansGroupe.ToString())
        dossier.PatientAllergiesGénériquesCis.Add("99999999")

        MedicamentGeneriqueDao.TraitementAllergies(dossier)

        Assert.AreEqual(0, dossier.PatientAllergiesGénériquesCis.Count, "la liste précédente est effacée")
    End Sub

    <TestMethod()> Public Sub TraitementAllergies_SansAllergie_VideLaListe()
        Dim dossier As New Patient
        dossier.PatientAllergiesGénériquesCis.Add("99999999")

        MedicamentGeneriqueDao.TraitementAllergies(dossier)

        Assert.AreEqual(0, dossier.PatientAllergiesGénériquesCis.Count)
    End Sub

End Class
