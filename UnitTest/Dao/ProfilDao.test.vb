Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne de profil par ProfilDao.LireLigne. buildBean garde sa
''' signature avec la connexion, qu'il n'utilise pas, et délègue ici.
''' </summary>
<TestClass()> Public Class TestProfilDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_r_profil_id", "oa_r_profil_designation", "oa_r_profil_type",
        "oa_r_profil_fonction_id_defaut", "oa_r_profil_niveau_acces", "oa_r_profil_inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim p = ProfilDao.LireLigne(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_r_profil_id", "MED"}, {"oa_r_profil_designation", "Médecin"}, {"oa_r_profil_type", "MEDICAL"},
            {"oa_r_profil_fonction_id_defaut", 8L}, {"oa_r_profil_niveau_acces", 1}, {"oa_r_profil_inactif", True}}))

        Assert.AreEqual("MED", p.Id)
        Assert.AreEqual("Médecin", p.Designation)
        Assert.AreEqual("MEDICAL", p.Type)
        Assert.AreEqual(8L, p.FonctionParDefautId)
        Assert.AreEqual(1, p.NiveauAcces)
        Assert.AreEqual("True", p.Inactif, "Inactif est une chaîne qui tient lieu de booléen")
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim p = ProfilDao.LireLigne(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {{"oa_r_profil_id", "X"}}))

        Assert.AreEqual("", p.Designation)
        Assert.AreEqual("", p.Type)
        Assert.AreEqual(0L, p.FonctionParDefautId)
        Assert.AreEqual(0, p.NiveauAcces)
        Assert.AreEqual("False", p.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        ProfilDao.LireLigne(LigneDeTest.Ligne(Colonnes, Nothing))
    End Sub

End Class
