Imports System.Web.Mvc
Imports Nethereum.Signer
Imports Oasis_Common

''' <summary>
''' /Sign/Check/{signature} : page publique de vérification d'une ordonnance.
''' On teste le résultat de l'action (vue choisie, ViewData), pas le rendu HTML.
''' </summary>
<TestClass()> Public Class TestControleurSign
    Inherits TestIntegration

    Private Const VueInactif As String = "~/Views/Sign/Inactif.vbhtml"
    Private Const VueInvalide As String = "~/Views/Sign/Invalide.vbhtml"

    <TestInitialize>
    Public Sub PreparerServeur()
        UtiliserCompte(Compte.Web)
    End Sub

    Private Shared Function ConsulterPage(id As String) As ViewResult
        Dim resultat = New Oasis_Web.Controllers.SignController().Check(id)
        Assert.IsInstanceOfType(resultat, GetType(ViewResult))
        Return DirectCast(resultat, ViewResult)
    End Function

    ''' <summary>Identifiant d'URL d'une ordonnance signée : sa signature en base64url.</summary>
    Private Shared Function IdentifiantPublic(idOrdonnance As Long) As String
        Dim sigHex = CStr(Scalaire("SELECT oa_ordonnance_signature FROM oasis.oa_patient_ordonnance WHERE oa_ordonnance_id = @p0",
                                   idOrdonnance))
        Assert.IsTrue(sigHex.StartsWith("0x"), sigHex)
        Dim hex = sigHex.Substring(2)
        Dim octets(hex.Length \ 2 - 1) As Byte
        For i = 0 To octets.Length - 1
            octets(i) = Convert.ToByte(hex.Substring(i * 2, 2), 16)
        Next
        Return Base64Url.Encoder(octets)
    End Function

    ''' <summary>Les DTO de la vue sont des types anonymes : lecture par réflexion.</summary>
    Private Shared Function Propriete(objet As Object, nom As String) As Object
        Return objet.GetType().GetProperty(nom).GetValue(objet)
    End Function

    Private Shared Function OrdonnanceSigneeDeTest(loginPrescripteur As String) As Long
        Dim idPatient = CreerPatient("CONTROLE", "Signature")
        Dim idPrescripteur = CreerUtilisateur(loginPrescripteur)
        Return CreerOrdonnanceSignee(idPatient, idPrescripteur)
    End Function

    <TestMethod()> Public Sub UneSignatureConnueAfficheLOrdonnanceSignee()
        Dim idPatient = CreerPatient("CONTROLE", "Signature")
        Dim idPrescripteur = CreerUtilisateur("sign.prescripteur")
        Dim idOrdonnance = CreerOrdonnanceSignee(idPatient, idPrescripteur)

        Dim page = ConsulterPage(IdentifiantPublic(idOrdonnance))

        Assert.AreEqual("", page.ViewName, "vue par défaut, ni Inactif ni Invalide")
        Assert.IsTrue(CBool(page.ViewData("SignatureVerifiee")))

        Dim affichee = DirectCast(page.ViewData("Ordonnance"), Ordonnance)
        Assert.AreEqual(idOrdonnance, affichee.Id)
        Assert.AreEqual(idPatient, affichee.PatientId)
        Assert.AreEqual(idPrescripteur, affichee.UserValidation)

        Dim lignes = DirectCast(page.ViewData("OrdonnanceDetail"), List(Of OrdonnanceDetail))
        Dim lignesEnBase = CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_id = @p0",
                                         idOrdonnance))
        Assert.IsTrue(lignesEnBase > 0, "le jeu de données doit fournir des lignes")
        Assert.AreEqual(lignesEnBase, lignes.Count)
        Assert.AreEqual(lignes.Count, DirectCast(page.ViewData("Traitements"), List(Of Traitement)).Count)

        Dim patientVue = page.ViewData("Patient")
        Assert.AreEqual(CStr(Scalaire("SELECT oa_patient_nom FROM oasis.oa_patient WHERE oa_patient_id = @p0", idPatient)).Trim(),
                        CStr(Propriete(patientVue, "PatientNom")).Trim())
        Assert.IsNull(patientVue.GetType().GetProperty("PatientNir"), "pas de NIR sur une page publique")

        Dim prescripteurVue = page.ViewData("User")
        Assert.AreEqual(CStr(Scalaire("SELECT oa_utilisateur_nom FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idPrescripteur)).Trim(),
                        CStr(Propriete(prescripteurVue, "UtilisateurNom")).Trim())
        Assert.IsNull(prescripteurVue.GetType().GetProperty("Password"))
        Assert.IsNull(prescripteurVue.GetType().GetProperty("UtilisateurClePrivee"))
    End Sub

    <TestMethod()> Public Sub UneSignatureInconnueRendLaPageInactive()
        Dim octets(64) As Byte
        Call New Random(2026).NextBytes(octets)

        Dim page = ConsulterPage(Base64Url.Encoder(octets))

        ' Comportement actuel : pas de 404. L'ordonnance introuvable lève dans le DAO
        ' et le Catch global rend la page « ordonnance inactive ».
        Assert.AreEqual(VueInactif, page.ViewName)
    End Sub

    <TestMethod()> Public Sub UnIdentifiantMalformeRendLaPageInactiveSansException()
        Dim malformes = New String() {"a", "!!!", "abc$def", "%%%", "", Nothing}
        For Each valeur In malformes
            Dim page = ConsulterPage(valeur)
            Assert.AreEqual(VueInactif, page.ViewName, "identifiant : " & If(valeur, "Nothing"))
        Next
    End Sub

    <TestMethod()> Public Sub UneOrdonnanceInactiveRendLaPageInactive()
        Dim idOrdonnance = OrdonnanceSigneeDeTest("sign.inactive")
        Executer("UPDATE oasis.oa_patient_ordonnance SET oa_ordonnance_inactif = 1 WHERE oa_ordonnance_id = @p0", idOrdonnance)

        Assert.AreEqual(VueInactif, ConsulterPage(IdentifiantPublic(idOrdonnance)).ViewName)
    End Sub

    <TestMethod()> Public Sub UneAdresseQuiNeCorrespondPasRendLaPageInvalide()
        Dim idOrdonnance = OrdonnanceSigneeDeTest("sign.adresse")
        Executer("UPDATE oasis.oa_patient_ordonnance SET oa_ordonnance_signature_adresse = @p0 WHERE oa_ordonnance_id = @p1",
                 "0x0000000000000000000000000000000000000001", idOrdonnance)

        Assert.AreEqual(VueInvalide, ConsulterPage(IdentifiantPublic(idOrdonnance)).ViewName)
    End Sub

    <TestMethod()> Public Sub UneChargeAltereeRendLaPageInvalide()
        Dim idOrdonnance = OrdonnanceSigneeDeTest("sign.charge")
        Dim charge = DirectCast(Scalaire("SELECT oa_ordonnance_signature_payload FROM oasis.oa_patient_ordonnance WHERE oa_ordonnance_id = @p0",
                                         idOrdonnance), Byte())
        charge(charge.Length \ 2) = charge(charge.Length \ 2) Xor CByte(&HFF)
        Executer("UPDATE oasis.oa_patient_ordonnance SET oa_ordonnance_signature_payload = @p0 WHERE oa_ordonnance_id = @p1",
                 charge, idOrdonnance)

        Assert.AreEqual(VueInvalide, ConsulterPage(IdentifiantPublic(idOrdonnance)).ViewName)
    End Sub

    <TestMethod()> Public Sub UneModificationApresSignatureNeChangePasLeContenuAffiche()
        Dim idOrdonnance = OrdonnanceSigneeDeTest("sign.modifiee")
        Executer("UPDATE oasis.oa_patient_ordonnance SET oa_ordonnance_commentaire = @p0 WHERE oa_ordonnance_id = @p1",
                 "modifié après signature", idOrdonnance)

        Dim page = ConsulterPage(IdentifiantPublic(idOrdonnance))

        Assert.AreEqual("", page.ViewName)
        Assert.IsTrue(CBool(page.ViewData("SignatureVerifiee")))
        Assert.AreNotEqual("modifié après signature", DirectCast(page.ViewData("Ordonnance"), Ordonnance).Commentaire,
                           "la page montre le contenu signé, pas la ligne vivante")
    End Sub

    <TestMethod()> Public Sub UneOrdonnanceSansChargeEstAfficheeSansEtreDiteVerifiee()
        Dim idOrdonnance = OrdonnanceSigneeDeTest("sign.ancienne")
        Executer("UPDATE oasis.oa_patient_ordonnance SET oa_ordonnance_signature_payload = NULL WHERE oa_ordonnance_id = @p0",
                 idOrdonnance)

        Dim page = ConsulterPage(IdentifiantPublic(idOrdonnance))

        ' Ordonnance signée avant la conservation de la charge : affichée, mais
        ' jamais présentée comme authentifiée.
        Assert.AreEqual("", page.ViewName)
        Assert.IsFalse(CBool(page.ViewData("SignatureVerifiee")))
        Assert.AreEqual(idOrdonnance, DirectCast(page.ViewData("Ordonnance"), Ordonnance).Id)
    End Sub

    <TestMethod()> Public Sub UneRotationDeCleNInvalidePasLesOrdonnancesDejaSignees()
        Dim idPatient = CreerPatient("CONTROLE", "Rotation")
        Dim idPrescripteur = CreerUtilisateur("sign.rotation")
        Dim idOrdonnance = CreerOrdonnanceSignee(idPatient, idPrescripteur)

        Dim nouvelleCle = EthECKey.GenerateKey()
        Dim daoUtilisateur As New UserDao
        daoUtilisateur.EnregistrerCleSignature(CInt(idPrescripteur),
                                        "0x" & BitConverter.ToString(nouvelleCle.GetPrivateKeyAsBytes()).Replace("-", ""),
                                        nouvelleCle.GetPublicAddress())

        Dim page = ConsulterPage(IdentifiantPublic(idOrdonnance))

        Assert.AreEqual("", page.ViewName)
        Assert.IsTrue(CBool(page.ViewData("SignatureVerifiee")))
    End Sub

End Class
