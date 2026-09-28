Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Oasis_Common

''' <summary>
''' /api/docfiledownload : lecture d'un document de la zone de dépôt par le client
''' lourd, sous le compte du serveur, derrière le filtre d'authentification.
''' Les règles pures (motif des noms, profils) sont couvertes par UnitTest ; ici,
''' les noms renvoient à de vraies lignes d'épisode et de sous-épisode et les
''' octets à de vrais fichiers de C:\OasisTests\Documents.
''' </summary>
<TestClass()> Public Class DocFileDownloadControllerTest
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

    Private Shared Function Telecharger(entete As AuthenticationHeaderValue, nom As String) As HttpResponseMessage
        UtiliserCompte(Compte.Web)
        Return AppelerApi(Of Global.Oasis_Web.Controllers.DocFileDownloadController)(entete, "PostValue",
            Function(c) c.PostValue(New DownloadRequest With {.FileName = nom}))
    End Function

    Private Shared Function OctetsRecus(reponse As HttpResponseMessage) As Byte()
        Return reponse.Content.ReadAsByteArrayAsync().Result
    End Function

    ' --- Cas nominal ---------------------------------------------------------------

    <TestMethod()> Public Sub UnDocumentDuDossierEstRenduALOctetPres()
        Dim idMedecin = CreerCompteDocumentsDeTest("dl.medecin")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        Dim contenu = OctetsDocumentDeTest(70000, 3)
        DeposerFichierDeTest(dossier.NomDocument, contenu)

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.medecin"), dossier.NomDocument)

        ' Comportement actuel : 202 Accepted, seul code que ApiOasis.downloadFile accepte.
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        CollectionAssert.AreEqual(contenu, OctetsRecus(reponse))
        Assert.AreEqual(CLng(contenu.Length), reponse.Content.Headers.ContentLength)
        Assert.AreEqual("attachment", reponse.Content.Headers.ContentDisposition.DispositionType)
        ' Le nom seul, jamais le chemin du serveur.
        Assert.AreEqual(Path.GetFileName(dossier.NomDocument), reponse.Content.Headers.ContentDisposition.FileName.Trim(""""c))
    End Sub

    <TestMethod()> Public Sub UnePieceDeReponseGardeSonTypeMime()
        Dim idMedecin = CreerCompteDocumentsDeTest("dl.reponse")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, idMedecin)
        Dim nom = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, idReponse, "pdf")
        Dim contenu = OctetsDocumentDeTest(512)
        DeposerFichierDeTest(nom, contenu)

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.reponse"), nom)

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        Assert.AreEqual("application/pdf", reponse.Content.Headers.ContentType.MediaType)
        CollectionAssert.AreEqual(contenu, OctetsRecus(reponse))
    End Sub

    <TestMethod()> Public Sub LaConsultationEstJournaliseeAvecLePatient()
        Dim idMedecin = CreerCompteDocumentsDeTest("dl.journal")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        DeposerFichierDeTest(dossier.NomDocument, OctetsDocumentDeTest(10))

        Telecharger(EnteteDocumentsDeTest("dl.journal"), dossier.NomDocument)

        Assert.AreEqual(1, CompterJournalDocumentsDeTest(idMedecin, dossier.PatientId,
                                                         "CONSULTATION : Téléchargement document " & dossier.NomDocument))
    End Sub

    <TestMethod()> Public Sub LesBarresObliquesSontAcceptees()
        Dim idMedecin = CreerCompteDocumentsDeTest("dl.barres")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        Dim contenu = OctetsDocumentDeTest(64)
        DeposerFichierDeTest(dossier.NomDocument, contenu)

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.barres"), "/" & dossier.NomDocument.Replace("\"c, "/"c))

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        CollectionAssert.AreEqual(contenu, OctetsRecus(reponse))
    End Sub

    <TestMethod()> Public Sub UnModeleSeTelechargeSansPatientNiJournal()
        Dim idAccueil = CreerCompteDocumentsDeTest("dl.accueil", "ACCUEIL")
        Dim nom = NomModeleDocumentDeTest(TypeSeCourrier, SousTypeSeAdressage)
        Dim contenu = OctetsDocumentDeTest(300, 9)
        DeposerFichierDeTest(nom, contenu)

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.accueil"), nom)

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        CollectionAssert.AreEqual(contenu, OctetsRecus(reponse))
        Assert.AreEqual(0, CompterJournalCommencantParDeTest(idAccueil, "CONSULTATION"))
    End Sub

    <TestMethod()> Public Sub LesProfilsParamedicalEtAccueilLisentLesDocuments()
        CreerCompteDocumentsDeTest("dl.para", "PARAMEDICAL")
        CreerCompteDocumentsDeTest("dl.accueil2", "ACCUEIL")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("dl.auteur"))
        DeposerFichierDeTest(dossier.NomDocument, OctetsDocumentDeTest(20))

        Assert.AreEqual(HttpStatusCode.Accepted, Telecharger(EnteteDocumentsDeTest("dl.para"), dossier.NomDocument).StatusCode)
        Assert.AreEqual(HttpStatusCode.Accepted, Telecharger(EnteteDocumentsDeTest("dl.accueil2"), dossier.NomDocument).StatusCode)
    End Sub

    <TestMethod()> Public Sub UnAdministrateurDeGestionLitLesDocuments()
        CreerCompteDocumentsDeTest("dl.admin", "GESTION", admin:=True)
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("dl.auteur"))
        DeposerFichierDeTest(dossier.NomDocument, OctetsDocumentDeTest(20))

        Assert.AreEqual(HttpStatusCode.Accepted, Telecharger(EnteteDocumentsDeTest("dl.admin"), dossier.NomDocument).StatusCode)
    End Sub

    <TestMethod()> Public Sub UnSoignantEtrangerAuDossierLeLitQuandMeme()
        Dim idAuteur = CreerCompteDocumentsDeTest("dl.auteur")
        Dim idAutre = CreerCompteDocumentsDeTest("dl.etranger")
        Dim dossier = CreerDossierDocumentDeTest(idAuteur)
        Dim contenu = OctetsDocumentDeTest(40)
        DeposerFichierDeTest(dossier.NomDocument, contenu)

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.etranger"), dossier.NomDocument)

        ' Comportement actuel : aucune règle de périmètre patient (PeutAccederAuPatient,
        ' DÉCISION 3 du modèle d'habilitations). Tout profil soignant lit tout dossier ;
        ' seule la trace, au nom du lecteur et du patient, rend l'accès constatable.
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        CollectionAssert.AreEqual(contenu, OctetsRecus(reponse))
        Assert.AreEqual(1, CompterJournalDocumentsDeTest(idAutre, dossier.PatientId,
                                                         "CONSULTATION : Téléchargement document " & dossier.NomDocument))
    End Sub

    ' --- Authentification et profil ------------------------------------------------

    <TestMethod()> Public Sub SansEnTeteLAppelEstRefuse()
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("dl.auteur"))
        DeposerFichierDeTest(dossier.NomDocument, OctetsDocumentDeTest(20))

        Dim reponse = Telecharger(Nothing, dossier.NomDocument)

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub UnMauvaisMotDePasseEstRefuse()
        CreerCompteDocumentsDeTest("dl.mdp")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("dl.auteur"))
        DeposerFichierDeTest(dossier.NomDocument, OctetsDocumentDeTest(20))

        Dim reponse = Telecharger(EnteteBasic("dl.mdp", "Mauvais!2026"), dossier.NomDocument)

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual("Identifiant et/ou mot de passe erroné !", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UnProfilDeGestionEstRefuse()
        Dim idGestion = CreerCompteDocumentsDeTest("dl.gestion", "GESTION")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("dl.auteur"))
        DeposerFichierDeTest(dossier.NomDocument, OctetsDocumentDeTest(20))

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.gestion"), dossier.NomDocument)

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual("Accès refusé", CorpsDe(reponse))
        Assert.AreEqual(0, CompterJournalCommencantParDeTest(idGestion, "CONSULTATION"))
    End Sub

    <TestMethod()> Public Sub SansCorpsLaRequeteEstIncomplete()
        CreerCompteDocumentsDeTest("dl.vide")

        UtiliserCompte(Compte.Web)
        Dim reponse = AppelerApi(Of Global.Oasis_Web.Controllers.DocFileDownloadController)(
            EnteteDocumentsDeTest("dl.vide"), "PostValue", Function(c) c.PostValue(Nothing))

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.AreEqual("Requête incomplète", CorpsDe(reponse))
    End Sub

    ' --- Noms qui ne désignent pas un document enregistré ------------------------------

    <TestMethod()> Public Sub UnFichierAbsentDuDisqueRepond404()
        CreerCompteDocumentsDeTest("dl.absent")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("dl.auteur"))

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.absent"), dossier.NomDocument)

        Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode)
        Assert.AreEqual("Document introuvable", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UnSousEpisodeInconnuRepond404MemeSiLeFichierExiste()
        CreerCompteDocumentsDeTest("dl.inconnu")
        Dim nom = NomDocumentSousEpisodeDeTest(987654, 876543)
        DeposerFichierDeTest(nom, OctetsDocumentDeTest(20))

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.inconnu"), nom)

        Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode)
        Assert.AreEqual("Document introuvable", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UnSousEpisodeRattacheAUnAutreEpisodeRepond404()
        Dim idAuteur = CreerCompteDocumentsDeTest("dl.croise")
        Dim dossierA = CreerDossierDocumentDeTest(idAuteur, "PATIENTA")
        Dim dossierB = CreerDossierDocumentDeTest(idAuteur, "PATIENTB")
        ' Épisode de B, sous-épisode de A : combinaison forgée. Le fichier existe
        ' pourtant sous ce nom, déposé par un appelant d'avant la correction.
        Dim forge = NomDocumentSousEpisodeDeTest(dossierB.EpisodeId, dossierA.SousEpisodeId)
        DeposerFichierDeTest(forge, OctetsDocumentDeTest(20))

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.croise"), forge)

        Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode)
        Assert.AreEqual(0, CompterJournalCommencantParDeTest(idAuteur, "CONSULTATION"))
    End Sub

    <TestMethod()> Public Sub LaRemonteeDeRepertoireEstRefusee()
        CreerCompteDocumentsDeTest("dl.remonte")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("dl.auteur"))
        ' Un fichier que la traversée viserait, placé à la racine de la zone.
        DeposerFichierDeTest("secret.txt", OctetsDocumentDeTest(20))

        For Each nom In {"..\..\Windows\win.ini",
                         "SousEpisode\..\secret.txt",
                         "SousEpisode\..\..\Documents\secret.txt",
                         dossier.NomDocument & "\..\..\secret.txt",
                         "C:\OasisTests\Documents\secret.txt",
                         "\\serveur\partage\secret.txt",
                         "secret.txt"}
            Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.remonte"), nom)
            Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode, nom)
            Assert.AreEqual("Document introuvable", CorpsDe(reponse), nom)
        Next
    End Sub

    <TestMethod()> Public Sub UneExtensionHorsListeEstRefuseeMemeSurUnDossierReel()
        CreerCompteDocumentsDeTest("dl.exe")
        Dim idAuteur = CreerCompteDocumentsDeTest("dl.auteur")
        Dim dossier = CreerDossierDocumentDeTest(idAuteur)
        Dim nom = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "exe")
        DeposerFichierDeTest(nom, OctetsDocumentDeTest(20))

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.exe"), nom)

        ' Le nom désigne un sous-épisode réel (HabilitationsDocuments l'accepte) mais
        ' CheminsDocuments ne sert pas cette extension.
        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.AreEqual("Nom de fichier invalide", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UnNomVideRepond404()
        CreerCompteDocumentsDeTest("dl.nomvide")

        Assert.AreEqual(HttpStatusCode.NotFound, Telecharger(EnteteDocumentsDeTest("dl.nomvide"), Nothing).StatusCode)
        Assert.AreEqual(HttpStatusCode.NotFound, Telecharger(EnteteDocumentsDeTest("dl.nomvide"), "   ").StatusCode)
    End Sub

    <TestMethod()> Public Sub UnIdentifiantDEpisodeDemesureRepond500()
        CreerCompteDocumentsDeTest("dl.long")

        Dim reponse = Telecharger(EnteteDocumentsDeTest("dl.long"),
                                  "SousEpisode\Episode_99999999999999999999_SousEpisode_1_SousEpisodeSousType_1.DOCX")

        ' Comportement actuel : CLng déborde dans ResoudreDocument (OverflowException,
        ' pas UnauthorizedAccessException) et l'appel finit en 500 au lieu de 404.
        ' Le message reste générique : rien de la configuration ne fuit.
        Assert.AreEqual(HttpStatusCode.InternalServerError, reponse.StatusCode)
        Assert.AreEqual("Erreur interne au serveur", CorpsDe(reponse))
    End Sub

End Class
