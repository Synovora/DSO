Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' Utilitaires de outils.vb et ModuleUtilsBase que les premiers tests ne
''' couvraient pas : génération de mots de passe et d'identifiants, libellés
''' d'utilisateur, texte SQL pour les journaux.
''' </summary>
<TestClass()> Public Class TestOutilsComplements

    <TestMethod()> Public Sub LeMotDePasseGenereALaLongueurDemandeeEtEviteLesCaracteresAmbigus()
        For Each longueur In {1, 8, 12, 32}
            Dim mdp As String = outils.GenPassword(longueur)
            Assert.AreEqual(longueur, mdp.Length)
            For Each c In mdp
                Assert.IsTrue("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz".Contains(c), "caractère ambigu : " & c)
            Next
        Next
    End Sub

    <TestMethod()> Public Sub DeuxMotsDePasseGeneresDifferent()
        Assert.AreNotEqual(outils.GenPassword(16), outils.GenPassword(16))
    End Sub

    <TestMethod()> Public Sub LIdentifiantCourtFaitSixCaracteresSurs()
        For i = 1 To 20
            Dim uid As String = outils.GetBase64UID()
            Assert.AreEqual(6, uid.Length)
            Assert.IsFalse(uid.Contains("="))
            Assert.IsFalse(uid.Contains("+"))
        Next
    End Sub

    <TestMethod()> Public Sub LeLibelleDUtilisateurEstCompose()
        Dim u As New Utilisateur With {.UtilisateurPrenom = " Jean ", .UtilisateurNom = "Dupont ", .UtilisateurProfilId = "MED_GEN", .TypeProfil = "MEDICAL"}
        Assert.AreEqual("(Jean Dupont -  med gen / medical)", outils.GetProfilUserString(u))
        Assert.AreEqual("(Jean Dupont -  med gen)", outils.GetProfilUserString2(u))
    End Sub

    <TestMethod()> Public Sub SansUtilisateurLeLibelleEstVide()
        Assert.AreEqual("", outils.GetProfilUserString(CType(Nothing, Utilisateur)))
        Assert.AreEqual("", outils.GetProfilUserString2(Nothing))
    End Sub

    <TestMethod()> Public Sub LeTexteSqlDeJournalRemplaceLesParametresParLeursValeurs()
        Dim cmd As New SqlCommand("SELECT * FROM oasis.oa_patient WHERE oa_patient_id = @id AND oa_patient_nom = @nom")
        cmd.Parameters.AddWithValue("@id", 42)
        cmd.Parameters.AddWithValue("@nom", "Dupont")
        Assert.AreEqual("SELECT * FROM oasis.oa_patient WHERE oa_patient_id = 42 AND oa_patient_nom = Dupont",
                        ModuleUtilsBase.GetSqlCommandTextForLogs(cmd))
    End Sub

    <TestMethod()> Public Sub SansPayloadLOrdonnanceSigneeEstAbsente()
        Assert.IsNull(VerificationSignature.OrdonnanceSignee(Nothing))
        Assert.IsNull(VerificationSignature.OrdonnanceSignee(New Ordonnance()))
        Assert.IsNull(VerificationSignature.OrdonnanceSignee(New Ordonnance With {.SignaturePayload = New Byte() {1, 2, 3}}), "une charge illisible ne lève pas, elle est absente")
    End Sub

    <TestMethod()> Public Sub UnePayloadValideRendLOrdonnanceSignee()
        Dim signee As New OrdonnanceFull With {
            .Ordonnance = New Ordonnance With {.Id = 55, .PatientId = 42, .Commentaire = "Renouvelable"},
            .Details = New List(Of OrdonnanceDetail) From {New OrdonnanceDetail With {.MedicamentDci = "PARACETAMOL", .Duree = 7}}}
        Dim relue = VerificationSignature.OrdonnanceSignee(New Ordonnance With {.Id = 55, .SignaturePayload = signee.Serialize()})

        Assert.IsNotNull(relue)
        Assert.AreEqual(55L, relue.Ordonnance.Id)
        Assert.AreEqual("Renouvelable", relue.Ordonnance.Commentaire)
        Assert.AreEqual(1, relue.Details.Count)
        Assert.AreEqual("PARACETAMOL", relue.Details(0).MedicamentDci)
    End Sub

End Class
