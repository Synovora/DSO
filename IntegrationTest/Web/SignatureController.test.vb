Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports Nethereum.Signer
Imports Nethereum.Util
Imports Oasis_Common
Imports Oasis_Web

''' <summary>
''' /api/signature et /api/signature/cle : la clé privée reste sur le serveur,
''' qui signe pour le compte authentifié et génère les paires de clés.
''' </summary>
<TestClass()> Public Class TestControleurSignature
    Inherits TestIntegration

    Private Shared ReadOnly ChargeDeTest As Byte() = Encoding.UTF8.GetBytes("ordonnance de test")

    <TestInitialize>
    Public Sub PreparerServeur()
        UtiliserCompte(Compte.Web)
        OuvrirContexteHttp()
    End Sub

    <TestCleanup>
    Public Sub FermerServeur()
        FermerContexteHttp()
    End Sub

    Private Shared Function DemanderSignature(login As String, motDePasse As String,
                                              demande As SignatureRequest) As HttpResponseMessage
        Return AppelerApi(Of SignatureController)(EnteteBasic(login, motDePasse), "Signer",
                                                  Function(c) c.Signer(demande))
    End Function

    Private Shared Function DemanderCle(login As String, motDePasse As String,
                                        demande As CleSignatureRequest) As HttpResponseMessage
        Return AppelerApi(Of SignatureController)(EnteteBasic(login, motDePasse), "GenererCle",
                                                  Function(c) c.GenererCle(demande))
    End Function

    Private Shared Function DemandeDe(octets As Byte()) As SignatureRequest
        Return New SignatureRequest With {.Charge = Convert.ToBase64String(octets)}
    End Function

    ''' <summary>Adresse du signataire, retrouvée comme le fait VerificationSignature.</summary>
    Private Shared Function AdresseRecuperee(octets As Byte(), sigHex As String) As String
        Dim signataire As New MessageSigner()
        Return signataire.EcRecover(signataire.Hash(octets), sigHex)
    End Function

    Private Shared Function MemeAdresse(a As String, b As String) As Boolean
        Return AddressUtil.Current.AreAddressesTheSame(a, b)
    End Function

    Private Shared Function AdresseEnBase(idUtilisateur As Long) As String
        Return CStr(Scalaire("SELECT COALESCE(cle_publique, '') FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0",
                             idUtilisateur))
    End Function

    Private Shared Function ClePriveeEnBase(idUtilisateur As Long) As String
        Return CStr(Scalaire("SELECT COALESCE(cle_privee, '') FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0",
                             idUtilisateur))
    End Function

    ' --- Signature déléguée

    <TestMethod()> Public Sub LaSignatureDelegueeSeVerifieAvecLaCleDuCompte()
        Dim idUtilisateur = CreerUtilisateur("sig.titulaire")

        Dim reponse = DemanderSignature("sig.titulaire", MotDePasseParDefaut, DemandeDe(ChargeDeTest))

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Dim lu = LireJson(Of SignatureResponse)(reponse)
        Dim adresse = AdresseEnBase(idUtilisateur)
        Assert.IsFalse(String.IsNullOrEmpty(adresse), "le jeu de données doit fournir une clé")
        Assert.IsTrue(MemeAdresse(adresse, lu.Adresse))
        Assert.IsTrue(MemeAdresse(adresse, AdresseRecuperee(ChargeDeTest, lu.Signature)))
        Assert.IsFalse(CorpsDe(reponse).Contains(ClePriveeEnBase(idUtilisateur)))
    End Sub

    <TestMethod()> Public Sub ChaqueCompteSigneAvecSaPropreCle()
        Dim idPremier = CreerUtilisateur("sig.premier")
        Dim idSecond = CreerUtilisateur("sig.second")

        Dim lu = LireJson(Of SignatureResponse)(DemanderSignature("sig.premier", MotDePasseParDefaut, DemandeDe(ChargeDeTest)))

        Dim recuperee = AdresseRecuperee(ChargeDeTest, lu.Signature)
        Assert.IsTrue(MemeAdresse(AdresseEnBase(idPremier), recuperee))
        Assert.IsFalse(MemeAdresse(AdresseEnBase(idSecond), recuperee))
    End Sub

    <TestMethod()> Public Sub SansCleLaSignatureEstRefusee()
        CreerUtilisateur("sig.sanscle", avecCle:=False)

        Dim reponse = DemanderSignature("sig.sanscle", MotDePasseParDefaut, DemandeDe(ChargeDeTest))

        Assert.AreEqual(HttpStatusCode.Conflict, reponse.StatusCode)
        Assert.AreEqual("Aucune cle de signature pour ce compte", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UneChargeIllisibleOuDeTailleInvalideEstRefusee()
        CreerUtilisateur("sig.charge")

        Dim illisible = DemanderSignature("sig.charge", MotDePasseParDefaut, New SignatureRequest With {.Charge = "pas du base64!"})
        Assert.AreEqual(HttpStatusCode.BadRequest, illisible.StatusCode)
        Assert.AreEqual("Charge illisible", CorpsDe(illisible))

        Dim vide = DemanderSignature("sig.charge", MotDePasseParDefaut, New SignatureRequest With {.Charge = ""})
        Assert.AreEqual(HttpStatusCode.BadRequest, vide.StatusCode)
        Assert.AreEqual("Charge de taille invalide", CorpsDe(vide))

        Dim absente = DemanderSignature("sig.charge", MotDePasseParDefaut, New SignatureRequest())
        Assert.AreEqual(HttpStatusCode.BadRequest, absente.StatusCode)

        Dim tropGrande(1024 * 1024) As Byte
        Dim excessive = DemanderSignature("sig.charge", MotDePasseParDefaut, DemandeDe(tropGrande))
        Assert.AreEqual(HttpStatusCode.BadRequest, excessive.StatusCode)
        Assert.AreEqual("Charge de taille invalide", CorpsDe(excessive))
    End Sub

    <TestMethod()> Public Sub UnCorpsIllisibleEstRefuse()
        CreerUtilisateur("sig.corps")

        ' Web API remet Nothing à l'action quand le JSON ne se lit pas.
        Dim reponse = DemanderSignature("sig.corps", MotDePasseParDefaut, Nothing)

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.AreEqual("Requête incomplète", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub SansAuthentificationLaSignatureEstRefusee()
        CreerUtilisateur("sig.intrus")

        Dim anonyme = AppelerApi(Of SignatureController)(Nothing, "Signer",
                                                         Function(c) c.Signer(DemandeDe(ChargeDeTest)))
        Assert.AreEqual(HttpStatusCode.Unauthorized, anonyme.StatusCode)

        Dim mauvais = DemanderSignature("sig.intrus", "Mauvais!2026", DemandeDe(ChargeDeTest))
        Assert.AreEqual(HttpStatusCode.Unauthorized, mauvais.StatusCode)
    End Sub

    ' --- Génération et rotation de clé

    <TestMethod()> Public Sub LaGenerationEnregistreLaPaireEtNeRenvoieQueLAdresse()
        Dim idUtilisateur = CreerUtilisateur("cle.nouvelle", avecCle:=False)

        Dim reponse = DemanderCle("cle.nouvelle", MotDePasseParDefaut,
                                  New CleSignatureRequest With {.UtilisateurId = CInt(idUtilisateur)})

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Dim lu = LireJson(Of CleSignatureResponse)(reponse)
        Assert.IsTrue(MemeAdresse(AdresseEnBase(idUtilisateur), lu.Adresse))
        Dim clePrivee = ClePriveeEnBase(idUtilisateur)
        Assert.IsTrue(clePrivee.StartsWith("0x"), clePrivee)
        Assert.AreEqual(66, clePrivee.Length)
        Assert.IsFalse(CorpsDe(reponse).ToLowerInvariant().Contains(clePrivee.Substring(2).ToLowerInvariant()))

        ' La nouvelle clé signe aussitôt.
        Dim signe = LireJson(Of SignatureResponse)(DemanderSignature("cle.nouvelle", MotDePasseParDefaut, DemandeDe(ChargeDeTest)))
        Assert.IsTrue(MemeAdresse(lu.Adresse, AdresseRecuperee(ChargeDeTest, signe.Signature)))
    End Sub

    <TestMethod()> Public Sub UneCleExistanteNEstPasRemplaceeSansDemande()
        Dim idUtilisateur = CreerUtilisateur("cle.existante")
        Dim adresseAvant = AdresseEnBase(idUtilisateur)
        Dim cleAvant = ClePriveeEnBase(idUtilisateur)

        Dim reponse = DemanderCle("cle.existante", MotDePasseParDefaut,
                                  New CleSignatureRequest With {.UtilisateurId = CInt(idUtilisateur), .Remplacer = False})

        Assert.AreEqual(HttpStatusCode.Conflict, reponse.StatusCode)
        Assert.AreEqual(adresseAvant, AdresseEnBase(idUtilisateur))
        Assert.AreEqual(cleAvant, ClePriveeEnBase(idUtilisateur))
    End Sub

    <TestMethod()> Public Sub LaRotationRemplaceLaCleEtLAncienneNeSignePlus()
        Dim idUtilisateur = CreerUtilisateur("cle.rotation")
        Dim adresseAvant = AdresseEnBase(idUtilisateur)
        Dim cleAvant = ClePriveeEnBase(idUtilisateur)

        Dim reponse = DemanderCle("cle.rotation", MotDePasseParDefaut,
                                  New CleSignatureRequest With {.UtilisateurId = CInt(idUtilisateur), .Remplacer = True})

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Dim adresseApres = LireJson(Of CleSignatureResponse)(reponse).Adresse
        Assert.IsFalse(MemeAdresse(adresseAvant, adresseApres))
        Assert.IsTrue(MemeAdresse(adresseApres, AdresseEnBase(idUtilisateur)))
        Assert.AreNotEqual(cleAvant, ClePriveeEnBase(idUtilisateur))

        ' Le filtre recharge l'utilisateur à chaque appel : la signature suivante
        ' part de la nouvelle clé.
        Dim signe = LireJson(Of SignatureResponse)(DemanderSignature("cle.rotation", MotDePasseParDefaut, DemandeDe(ChargeDeTest)))
        Dim recuperee = AdresseRecuperee(ChargeDeTest, signe.Signature)
        Assert.IsTrue(MemeAdresse(adresseApres, recuperee))
        Assert.IsFalse(MemeAdresse(adresseAvant, recuperee))
        Assert.IsTrue(MemeAdresse(adresseApres, signe.Adresse))
    End Sub

    <TestMethod()> Public Sub UnNonAdministrateurNePeutPasGenererLaCleDUnAutre()
        CreerUtilisateur("cle.appelant")
        Dim idCible = CreerUtilisateur("cle.cible", avecCle:=False)

        Dim reponse = DemanderCle("cle.appelant", MotDePasseParDefaut,
                                  New CleSignatureRequest With {.UtilisateurId = CInt(idCible), .Remplacer = True})

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual("", AdresseEnBase(idCible))
        Assert.AreEqual("", ClePriveeEnBase(idCible))
    End Sub

    <TestMethod()> Public Sub UnAdministrateurPeutGenererLaCleDUnAutre()
        CreerUtilisateur("cle.admin", admin:=True)
        Dim idCible = CreerUtilisateur("cle.administre", avecCle:=False)

        Dim reponse = DemanderCle("cle.admin", MotDePasseParDefaut,
                                  New CleSignatureRequest With {.UtilisateurId = CInt(idCible)})

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.IsTrue(MemeAdresse(LireJson(Of CleSignatureResponse)(reponse).Adresse, AdresseEnBase(idCible)))
        Assert.AreNotEqual("", ClePriveeEnBase(idCible))
    End Sub

    <TestMethod()> Public Sub UneDemandeDeCleIllisibleEstRefusee()
        CreerUtilisateur("cle.corps")

        Dim reponse = DemanderCle("cle.corps", MotDePasseParDefaut, Nothing)

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub UneDemandeDeCleSansAuthentificationEstRefusee()
        Dim idCible = CreerUtilisateur("cle.anonyme", avecCle:=False)

        Dim reponse = AppelerApi(Of SignatureController)(Nothing, "GenererCle",
            Function(c) c.GenererCle(New CleSignatureRequest With {.UtilisateurId = CInt(idCible)}))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual("", AdresseEnBase(idCible))
    End Sub

    <TestMethod()> Public Sub UneCleDemandeePourUnUtilisateurInexistantEchoueEn500()
        CreerUtilisateur("cle.fantome", admin:=True)

        Dim reponse = DemanderCle("cle.fantome", MotDePasseParDefaut,
                                  New CleSignatureRequest With {.UtilisateurId = 999999999})

        ' Comportement actuel : EnregistrerCleSignature lève ArgumentException,
        ' que le contrôleur rend en 500 et non en 404.
        Assert.AreEqual(HttpStatusCode.InternalServerError, reponse.StatusCode)
        Assert.AreEqual("Erreur interne au serveur", CorpsDe(reponse))
    End Sub

End Class
