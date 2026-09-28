Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Oasis_Common
Imports Oasis_Web

''' <summary>
''' /api/docfileupload : dépôt d'un document par le client lourd, sous le compte du
''' serveur. Le corps multipart passe par le flux brut de la requête, comme sous IIS
''' (voir DocumentsDeTest.AppelerApiAvecCorpsDeTest). Tout est écrit dans
''' C:\OasisTests\Documents et effacé après chaque test.
''' </summary>
<TestClass()> Public Class DocFileUploadControllerTest
    Inherits TestIntegration

    <TestInitialize>
    Public Sub PreparerZone()
        OuvrirContexteHttp()
        OuvrirZoneDocumentsDeTest()
    End Sub

    <TestCleanup>
    Public Sub NettoyerZone()
        Try
            FermerZoneDocumentsDeTest()
        Finally
            FermerContexteHttp()
        End Try
    End Sub

    Private Shared Function Deposer(entete As AuthenticationHeaderValue, corps As MultipartFormDataContent) As HttpResponseMessage
        UtiliserCompte(Compte.Web)
        Return AppelerApiAvecCorpsDeTest(Of DocFileUploadController)(entete, "Upload",
            Function(c) c.Upload().Result, corps)
    End Function

    Private Shared Function DeposerFichier(login As String, nom As String, contenu As Byte()) As HttpResponseMessage
        Return Deposer(EnteteDocumentsDeTest(login), CorpsDepotDeTest(nom, contenu))
    End Function

    ' --- Cas nominal ---------------------------------------------------------------

    <TestMethod()> Public Sub UnDocumentDeposeEstEcritALOctetPres()
        Dim idMedecin = CreerCompteDocumentsDeTest("up.medecin")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        Dim contenu = OctetsDocumentDeTest(100000, 5)

        Dim reponse = DeposerFichier("up.medecin", dossier.NomDocument, contenu)

        ' Comportement actuel : 202 Accepted, seul code que ApiOasis.uploadFile accepte.
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode, CorpsDe(reponse))
        Assert.IsTrue(CorpsDe(reponse).Contains("true"))
        CollectionAssert.AreEqual(contenu, LireDansZoneDeTest(dossier.NomDocument))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest(), "la partie reçue a été déplacée, pas copiée")
    End Sub

    <TestMethod()> Public Sub UnDocumentDeposePuisTelechargeRevientIdentique()
        CreerCompteDocumentsDeTest("up.allerretour")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))
        Dim contenu = OctetsDocumentDeTest(4096, 11)

        Assert.AreEqual(HttpStatusCode.Accepted, DeposerFichier("up.allerretour", dossier.NomDocument, contenu).StatusCode)

        UtiliserCompte(Compte.Web)
        Dim telechargement = AppelerApi(Of Global.Oasis_Web.Controllers.DocFileDownloadController)(
            EnteteDocumentsDeTest("up.allerretour"), "PostValue",
            Function(c) c.PostValue(New DownloadRequest With {.FileName = dossier.NomDocument}))
        Assert.AreEqual(HttpStatusCode.Accepted, telechargement.StatusCode)
        CollectionAssert.AreEqual(contenu, telechargement.Content.ReadAsByteArrayAsync().Result)
    End Sub

    <TestMethod()> Public Sub UnNouveauDepotRemplaceLAncien()
        CreerCompteDocumentsDeTest("up.remplace")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))
        DeposerFichierDeTest(dossier.NomDocument, OctetsDocumentDeTest(5000, 1))
        Dim nouveau = OctetsDocumentDeTest(200, 2)

        Assert.AreEqual(HttpStatusCode.Accepted, DeposerFichier("up.remplace", dossier.NomDocument, nouveau).StatusCode)

        CollectionAssert.AreEqual(nouveau, LireDansZoneDeTest(dossier.NomDocument), "remplacé, pas complété")
    End Sub

    <TestMethod()> Public Sub LeDepotEstJournaliseAvecLePatient()
        Dim idMedecin = CreerCompteDocumentsDeTest("up.journal")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)

        DeposerFichier("up.journal", dossier.NomDocument, OctetsDocumentDeTest(10))

        Assert.AreEqual(1, CompterJournalDocumentsDeTest(idMedecin, dossier.PatientId,
                                                         "MODIFICATION : Dépôt document " & dossier.NomDocument))
    End Sub

    <TestMethod()> Public Sub UnModeleSeDeposeSansJournal()
        Dim idMedecin = CreerCompteDocumentsDeTest("up.modele")
        Dim nom = NomModeleDocumentDeTest(TypeSeCertificat, SousTypeSeCertificat)
        Dim contenu = OctetsDocumentDeTest(800)

        Dim reponse = DeposerFichier("up.modele", nom, contenu)

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        CollectionAssert.AreEqual(contenu, LireDansZoneDeTest(nom))
        Assert.AreEqual(0, CompterJournalCommencantParDeTest(idMedecin, "MODIFICATION"))
    End Sub

    <TestMethod()> Public Sub UnIdentifiantDeReponseNEstPasVerifie()
        CreerCompteDocumentsDeTest("up.reponse")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))
        Dim nom = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 424242, "pdf")

        Dim reponse = DeposerFichier("up.reponse", nom, OctetsDocumentDeTest(30))

        ' Comportement actuel : seul le couple épisode / sous-épisode est contrôlé. Un
        ' identifiant de réponse qui n'existe pas en base est accepté, et le fichier
        ' s'ajoute au dossier sans ligne de réponse correspondante.
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse WHERE id = @p0", 424242)))
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.IsTrue(ExisteDansZoneDeTest(nom))
    End Sub

    <TestMethod()> Public Sub UnSoignantEtrangerAuDossierYDeposeQuandMeme()
        Dim idEtranger = CreerCompteDocumentsDeTest("up.etranger")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))

        Dim reponse = DeposerFichier("up.etranger", dossier.NomDocument, OctetsDocumentDeTest(30))

        ' Comportement actuel : PeutAccederAuPatient n'a pas de règle de périmètre
        ' (DÉCISION 3). Tout profil soignant écrit dans tout dossier ; la trace porte
        ' le déposant et le patient.
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.AreEqual(1, CompterJournalDocumentsDeTest(idEtranger, dossier.PatientId,
                                                         "MODIFICATION : Dépôt document " & dossier.NomDocument))
    End Sub

    ' --- Authentification et profil ------------------------------------------------

    <TestMethod()> Public Sub SansEnTeteRienNEstEcrit()
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))

        Dim reponse = Deposer(Nothing, CorpsDepotDeTest(dossier.NomDocument, OctetsDocumentDeTest(30)))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.IsFalse(ExisteDansZoneDeTest(dossier.NomDocument))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
    End Sub

    <TestMethod()> Public Sub UnMauvaisMotDePasseNEcritRien()
        CreerCompteDocumentsDeTest("up.mdp")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))

        Dim reponse = Deposer(EnteteBasic("up.mdp", "Mauvais!2026"),
                              CorpsDepotDeTest(dossier.NomDocument, OctetsDocumentDeTest(30)))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.IsFalse(ExisteDansZoneDeTest(dossier.NomDocument))
    End Sub

    <TestMethod()> Public Sub UnProfilDeGestionNEcritRien()
        Dim idGestion = CreerCompteDocumentsDeTest("up.gestion", "GESTION")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))

        Dim reponse = DeposerFichier("up.gestion", dossier.NomDocument, OctetsDocumentDeTest(30))

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual("Accès refusé", CorpsDe(reponse))
        Assert.IsFalse(ExisteDansZoneDeTest(dossier.NomDocument))
        ' Refus avant toute lecture du corps : pas même de fichier temporaire.
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
        Assert.AreEqual(0, CompterJournalCommencantParDeTest(idGestion, "MODIFICATION"))
    End Sub

    ' --- Noms refusés --------------------------------------------------------------

    <TestMethod()> Public Sub LaRemonteeDeRepertoireNEcritRienHorsDeLaZone()
        CreerCompteDocumentsDeTest("up.remonte")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))
        Dim hors = Path.Combine(Path.GetDirectoryName(RacineZoneDocumentsDeTest()), "Intrus.DOCX")

        For Each nom In {"..\Intrus.DOCX",
                         "SousEpisode\..\..\Intrus.DOCX",
                         dossier.NomDocument & "\..\..\..\Intrus.DOCX",
                         "C:\OasisTests\Intrus.DOCX",
                         "Intrus.DOCX"}
            Dim reponse = DeposerFichier("up.remonte", nom, OctetsDocumentDeTest(30))
            Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode, nom)
            Assert.AreEqual("Document introuvable", CorpsDe(reponse), nom)
            Assert.IsFalse(File.Exists(hors), nom)
        Next
        Assert.IsFalse(ExisteDansZoneDeTest("Intrus.DOCX"))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
    End Sub

    <TestMethod()> Public Sub UnSousEpisodeInconnuNEstPasEcrit()
        CreerCompteDocumentsDeTest("up.inconnu")
        Dim nom = NomDocumentSousEpisodeDeTest(987654, 876543)

        Dim reponse = DeposerFichier("up.inconnu", nom, OctetsDocumentDeTest(30))

        Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode)
        Assert.IsFalse(ExisteDansZoneDeTest(nom))
    End Sub

    <TestMethod()> Public Sub UnNomForgeVersLEpisodeDUnAutrePatientNEstPasEcrit()
        Dim idAuteur = CreerCompteDocumentsDeTest("up.croise")
        Dim dossierA = CreerDossierDocumentDeTest(idAuteur, "PATIENTA")
        Dim dossierB = CreerDossierDocumentDeTest(idAuteur, "PATIENTB")
        Dim forge = NomDocumentSousEpisodeDeTest(dossierB.EpisodeId, dossierA.SousEpisodeId)
        Dim documentB = OctetsDocumentDeTest(50, 4)
        DeposerFichierDeTest(dossierB.NomDocument, documentB)

        Dim reponse = DeposerFichier("up.croise", forge, OctetsDocumentDeTest(30))

        Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode)
        Assert.IsFalse(ExisteDansZoneDeTest(forge))
        CollectionAssert.AreEqual(documentB, LireDansZoneDeTest(dossierB.NomDocument), "le document de B est intact")
    End Sub

    <TestMethod()> Public Sub UneExtensionHorsListeEstRefusee()
        CreerCompteDocumentsDeTest("up.exe")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))
        Dim nom = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "exe")

        Dim reponse = DeposerFichier("up.exe", nom, OctetsDocumentDeTest(30))

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.AreEqual("Nom de fichier invalide", CorpsDe(reponse))
        Assert.IsFalse(ExisteDansZoneDeTest(nom))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
    End Sub

    <TestMethod()> Public Sub UnModeleAvecUneExtensionHorsListeEstRefuse()
        CreerCompteDocumentsDeTest("up.modexe")
        Dim nom = NomModeleDocumentDeTest(TypeSeCourrier, SousTypeSeAdressage, "js")

        Dim reponse = DeposerFichier("up.modexe", nom, OctetsDocumentDeTest(30))

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.IsFalse(ExisteDansZoneDeTest(nom))
    End Sub

    ' --- Forme du formulaire --------------------------------------------------------

    <TestMethod()> Public Sub SansFichierLeDepotEstRefuse()
        CreerCompteDocumentsDeTest("up.sansfichier")
        Dim corps As New MultipartFormDataContent()
        corps.Add(New StringContent("rien"), "commentaire")

        Dim reponse = Deposer(EnteteDocumentsDeTest("up.sansfichier"), corps)

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.AreEqual("Un seul fichier doit être posté", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub DeuxFichiersSontRefusesEtAucunNEstEcrit()
        CreerCompteDocumentsDeTest("up.deux")
        Dim idAuteur = CreerCompteDocumentsDeTest("up.auteur")
        Dim dossierA = CreerDossierDocumentDeTest(idAuteur, "PATIENTA")
        Dim dossierB = CreerDossierDocumentDeTest(idAuteur, "PATIENTB")
        Dim corps = CorpsDepotDeTest(dossierA.NomDocument, OctetsDocumentDeTest(30))
        corps.Add(New ByteArrayContent(OctetsDocumentDeTest(30)), "filekey2", dossierB.NomDocument)

        Dim reponse = Deposer(EnteteDocumentsDeTest("up.deux"), corps)

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.IsFalse(ExisteDansZoneDeTest(dossierA.NomDocument))
        Assert.IsFalse(ExisteDansZoneDeTest(dossierB.NomDocument))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest(), "les deux parties reçues sont supprimées")
    End Sub

    <TestMethod()> Public Sub UnDocumentDePlusDe50MoEstRefuse()
        CreerCompteDocumentsDeTest("up.gros")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))

        Dim reponse = DeposerFichier("up.gros", dossier.NomDocument, OctetsDocumentDeTest(50 * 1024 * 1024 + 1))

        Assert.AreEqual(HttpStatusCode.RequestEntityTooLarge, reponse.StatusCode)
        Assert.AreEqual("Document trop volumineux", CorpsDe(reponse))
        Assert.IsFalse(ExisteDansZoneDeTest(dossier.NomDocument))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
    End Sub

    <TestMethod()> Public Sub UnDocumentDe50MoPileEstAccepte()
        CreerCompteDocumentsDeTest("up.limite")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("up.auteur"))

        Dim reponse = DeposerFichier("up.limite", dossier.NomDocument, OctetsDocumentDeTest(50 * 1024 * 1024))

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.AreEqual(50L * 1024L * 1024L, New FileInfo(CheminZoneDocumentsDeTest(dossier.NomDocument)).Length)
    End Sub

End Class
