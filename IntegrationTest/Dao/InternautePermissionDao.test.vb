Imports Oasis_Common

''' <summary>
''' InternautePermissionDao contre la base : lien entre un compte du portail et
''' le dossier patient qu'il peut consulter. Le client lourd crée le lien et le
''' relit par patient (RadFPatientDetailEdit) sous oasis_client ; le portail relit
''' les dossiers d'un internaute (PortailController) sous oasis_web.
''' </summary>
<TestClass()> Public Class InternautePermissionDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New InternautePermissionDao

    Private Function Lier(internauteId As Long, patientId As Long, Optional niveau As Integer = 1) As Long
        Return dao.Create(New InternautePermission With {
            .Internaute = CInt(internauteId), .Patient = CInt(patientId), .Permission = niveau})
    End Function

    <TestMethod()> Public Sub Create_SousClient_EnregistreLeLienEtRenvoieSonId()
        Dim internauteId = CreerInternaute()
        Dim patientId = CreerPatient()

        Dim id = Lier(internauteId, patientId, 1)

        Assert.IsTrue(id > 0)
        Dim filtre = " FROM oasis.oa_internaute_permission WHERE id = @p0"
        Assert.AreEqual(internauteId, CLng(Scalaire("SELECT internaute" & filtre, id)))
        Assert.AreEqual(patientId, CLng(Scalaire("SELECT patient" & filtre, id)))
        Assert.AreEqual(1, CInt(Scalaire("SELECT permission" & filtre, id)))
    End Sub

    <TestMethod()> Public Sub GetPermissionsByPatient_SousClient_RenvoieLesLiensDuSeulPatient()
        ' La fiche patient prend le premier lien pour retrouver le compte à réinitialiser.
        Dim internauteId = CreerInternaute()
        Dim autreInternaute = CreerInternaute()
        Dim patientId = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim premier = Lier(internauteId, patientId, 1)
        Dim second = Lier(autreInternaute, patientId, 2)
        Lier(internauteId, autrePatient)

        Dim liens = dao.GetPermissionsByPatient(patientId)

        CollectionAssert.AreEquivalent(New Long() {premier, second}, liens.Select(Function(l) CLng(l.Id)).ToArray())
        Dim lu = liens.Single(Function(l) l.Id = premier)
        Assert.AreEqual(CInt(internauteId), lu.Internaute)
        Assert.AreEqual(CInt(patientId), lu.Patient)
        Assert.AreEqual(1, lu.Permission)
        Assert.AreEqual(2, liens.Single(Function(l) l.Id = second).Permission)
    End Sub

    <TestMethod()> Public Sub GetPermissionsByPatient_SousClient_PatientSansCompte_ListeVide()
        Dim patientId = CreerPatient()
        Assert.AreEqual(0, dao.GetPermissionsByPatient(patientId).Count)
    End Sub

    <TestMethod()> Public Sub GetPermissionsByInternaute_SousWeb_RenvoieLesDossiersDuSeulInternaute()
        UtiliserCompte(Compte.Web)
        Dim internauteId = CreerInternaute()
        Dim autreInternaute = CreerInternaute()
        Dim patientId = CreerPatient()
        Dim enfant = CreerPatient("TEST", "Enfant")
        Dim premier = Lier(internauteId, patientId)
        Dim second = Lier(internauteId, enfant)
        Lier(autreInternaute, patientId)

        Dim liens = dao.GetPermissionsByInternaute(CInt(internauteId))

        CollectionAssert.AreEquivalent(New Long() {premier, second}, liens.Select(Function(l) CLng(l.Id)).ToArray())
        CollectionAssert.AreEquivalent(New Integer() {CInt(patientId), CInt(enfant)}, liens.Select(Function(l) l.Patient).ToArray())
        Assert.IsTrue(liens.All(Function(l) l.Internaute = CInt(internauteId)))
    End Sub

    <TestMethod()> Public Sub GetPermissionsByInternaute_SousWeb_InternauteSansDossier_ListeVide()
        UtiliserCompte(Compte.Web)
        Dim internauteId = CreerInternaute()
        Assert.AreEqual(0, dao.GetPermissionsByInternaute(CInt(internauteId)).Count)
    End Sub

End Class
