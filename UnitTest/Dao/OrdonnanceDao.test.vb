Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne d'ordonnance par OrdonnanceDao.BuildBean. Les deux
''' colonnes de signature (charge signée, adresse du signataire) ne sont
''' présentes que dans les requêtes qui en ont besoin : leur absence ne doit
''' pas empêcher la lecture, leur présence doit être restituée octet pour octet,
''' sans quoi la vérification de signature échoue.
''' </summary>
<TestClass()> Public Class TestOrdonnanceDaoLecture

    Private Shared ReadOnly ColonnesDeBase As String() = {
        "oa_ordonnance_id", "oa_ordonnance_patient_id", "oa_ordonnance_episode_id",
        "oa_ordonnance_utilisateur_creation", "oa_ordonnance_date_creation", "oa_ordonnance_date_validation",
        "oa_ordonnance_user_validation", "oa_ordonnance_date_edition", "oa_ordonnance_commentaire",
        "oa_ordonnance_renouvellement", "oa_ordonnance_inactif", "oa_ordonnance_signature"}

    Private Shared ReadOnly ColonnesSignature As String() = {
        "oa_ordonnance_signature_payload", "oa_ordonnance_signature_adresse"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim charge = New Byte() {1, 2, 3, 250, 255}
        Dim o = OrdonnanceDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase.Concat(ColonnesSignature), New Dictionary(Of String, Object) From {
            {"oa_ordonnance_id", 55L}, {"oa_ordonnance_patient_id", 42L}, {"oa_ordonnance_episode_id", 100L},
            {"oa_ordonnance_utilisateur_creation", 5L}, {"oa_ordonnance_date_creation", New Date(2024, 1, 2)},
            {"oa_ordonnance_date_validation", New Date(2024, 1, 3)}, {"oa_ordonnance_user_validation", 6L},
            {"oa_ordonnance_date_edition", New Date(2024, 1, 4)}, {"oa_ordonnance_commentaire", "Renouvelable"},
            {"oa_ordonnance_renouvellement", 2}, {"oa_ordonnance_inactif", True}, {"oa_ordonnance_signature", "sig"},
            {"oa_ordonnance_signature_payload", charge}, {"oa_ordonnance_signature_adresse", "0xabc"}}))

        Assert.AreEqual(55L, o.Id)
        Assert.AreEqual(42L, o.PatientId)
        Assert.AreEqual(100L, o.EpisodeId)
        Assert.AreEqual(5L, o.UtilisateurCreation)
        Assert.AreEqual(New Date(2024, 1, 2), o.DateCreation)
        Assert.AreEqual(New Date(2024, 1, 3), o.DateValidation)
        Assert.AreEqual(6L, o.UserValidation)
        Assert.AreEqual(New Date(2024, 1, 4), o.DateEdition)
        Assert.AreEqual("Renouvelable", o.Commentaire)
        Assert.AreEqual(2, o.Renouvellement)
        Assert.IsTrue(o.Inactif)
        Assert.AreEqual("sig", o.Signature)
        CollectionAssert.AreEqual(charge, o.SignaturePayload)
        Assert.AreEqual("0xabc", o.SignatureAdresse)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim o = OrdonnanceDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase, New Dictionary(Of String, Object) From {{"oa_ordonnance_id", 1L}}))

        Assert.AreEqual(1L, o.Id)
        Assert.AreEqual(0L, o.PatientId)
        Assert.AreEqual(0L, o.EpisodeId)
        Assert.AreEqual(0L, o.UtilisateurCreation)
        Assert.AreEqual(0L, o.UserValidation)
        Assert.AreEqual(Date.MinValue, o.DateCreation)
        Assert.AreEqual(Date.MinValue, o.DateValidation)
        Assert.AreEqual(Date.MinValue, o.DateEdition)
        Assert.AreEqual("", o.Commentaire)
        Assert.AreEqual(0, o.Renouvellement)
        Assert.IsFalse(o.Inactif)
        Assert.AreEqual("", o.Signature)
    End Sub

    <TestMethod()> Public Sub SansColonnesDeSignatureLaChargeEtLAdresseRestentAbsentes()
        Dim o = OrdonnanceDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase, New Dictionary(Of String, Object) From {{"oa_ordonnance_id", 1L}}))
        Assert.IsNull(o.SignaturePayload)
        Assert.IsNull(o.SignatureAdresse)
    End Sub

    <TestMethod()> Public Sub DesColonnesDeSignatureNullesDonnentUneChargeAbsente()
        ' Une ordonnance jamais signée : la charge doit rester Nothing pour que
        ' VerificationSignature la déclare non vérifiable, pas vide.
        Dim o = OrdonnanceDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase.Concat(ColonnesSignature), New Dictionary(Of String, Object) From {{"oa_ordonnance_id", 1L}}))
        Assert.IsNull(o.SignaturePayload)
        Assert.AreEqual("", o.SignatureAdresse)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        OrdonnanceDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase, Nothing))
    End Sub

End Class
