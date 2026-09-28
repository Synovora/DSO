Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Oasis_Common
Imports Oasis_Web

''' <summary>
''' /api/rename : renommage d'un document de la zone de dépôt par le client lourd
''' (validation d'une réponse, déplacement vers le dossier définitif), sous le
''' compte du serveur. Les deux noms doivent renvoyer à des documents enregistrés
''' du même patient.
''' </summary>
<TestClass()> Public Class RenameControllerTest
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

    Private Shared Function Renommer(entete As AuthenticationHeaderValue, ancien As String, nouveau As String) As HttpResponseMessage
        UtiliserCompte(Compte.Web)
        Return AppelerApi(Of RenameController)(entete, "PostValue",
            Function(c) c.PostValue(New RenameRequest With {.OldName = ancien, .NewName = nouveau}))
    End Function

    ''' <summary>Deux noms de réponse du même sous-épisode, le premier déposé sur le disque.</summary>
    Private Shared Function DeuxNomsDeReponse(dossier As DossierDocumentDeTest, contenu As Byte()) As String()
        Dim ancien = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "pdf")
        Dim nouveau = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 2, "pdf")
        DeposerFichierDeTest(ancien, contenu)
        Return {ancien, nouveau}
    End Function

    ' --- Cas nominal ---------------------------------------------------------------

    <TestMethod()> Public Sub LeDocumentChangeDeNomSansChangerDeContenu()
        Dim idMedecin = CreerCompteDocumentsDeTest("rn.medecin")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        Dim contenu = OctetsDocumentDeTest(20000, 6)
        Dim noms = DeuxNomsDeReponse(dossier, contenu)

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.medecin"), noms(0), noms(1))

        ' Comportement actuel : 202 Accepted, seul code que ApiOasis.renameFile accepte.
        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode, CorpsDe(reponse))
        Assert.IsFalse(ExisteDansZoneDeTest(noms(0)))
        CollectionAssert.AreEqual(contenu, LireDansZoneDeTest(noms(1)))
    End Sub

    <TestMethod()> Public Sub LeRenommageVersUnAutreDossierDuMemePatientEstAccepte()
        CreerCompteDocumentsDeTest("rn.dossier")
        Dim idAuteur = CreerCompteDocumentsDeTest("rn.auteur")
        Dim dossier = CreerDossierDocumentDeTest(idAuteur)
        Dim idAutreSousEpisode = CreerSousEpisode(dossier.EpisodeId, idAuteur)
        Dim ancien = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "pdf")
        Dim nouveau = NomDocumentSousEpisodeDeTest(dossier.EpisodeId, idAutreSousEpisode, extension:="pdf")
        Dim contenu = OctetsDocumentDeTest(100)
        DeposerFichierDeTest(ancien, contenu)

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.dossier"), ancien, nouveau)

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        CollectionAssert.AreEqual(contenu, LireDansZoneDeTest(nouveau))
    End Sub

    <TestMethod()> Public Sub LeRenommageEstJournaliseAvecLePatient()
        Dim idMedecin = CreerCompteDocumentsDeTest("rn.journal")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        Dim noms = DeuxNomsDeReponse(dossier, OctetsDocumentDeTest(10))

        Renommer(EnteteDocumentsDeTest("rn.journal"), noms(0), noms(1))

        Assert.AreEqual(1, CompterJournalDocumentsDeTest(idMedecin, dossier.PatientId,
                                                         "MODIFICATION : Renommage document " & noms(0) & " vers " & noms(1)))
    End Sub

    <TestMethod()> Public Sub UnModeleSeRenommeEnModeleSansJournal()
        Dim idMedecin = CreerCompteDocumentsDeTest("rn.modele")
        Dim ancien = NomModeleDocumentDeTest(TypeSeCourrier, SousTypeSeAdressage)
        Dim nouveau = NomModeleDocumentDeTest(TypeSeCourrier, SousTypeSeCompteRendu)
        Dim contenu = OctetsDocumentDeTest(70)
        DeposerFichierDeTest(ancien, contenu)

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.modele"), ancien, nouveau)

        Assert.AreEqual(HttpStatusCode.Accepted, reponse.StatusCode)
        CollectionAssert.AreEqual(contenu, LireDansZoneDeTest(nouveau))
        Assert.AreEqual(0, CompterJournalCommencantParDeTest(idMedecin, "MODIFICATION"))
    End Sub

    ' --- Changement de dossier ------------------------------------------------------

    <TestMethod()> Public Sub LeRenommageVersLeDossierDUnAutrePatientEstRefuseEtTrace()
        Dim idMedecin = CreerCompteDocumentsDeTest("rn.croise")
        Dim dossierA = CreerDossierDocumentDeTest(idMedecin, "PATIENTA")
        Dim dossierB = CreerDossierDocumentDeTest(idMedecin, "PATIENTB")
        Dim contenu = OctetsDocumentDeTest(60)
        Dim ancien = NomDocumentReponseDeTest(dossierA.EpisodeId, dossierA.SousEpisodeId, 1, "pdf")
        Dim cible = NomDocumentReponseDeTest(dossierB.EpisodeId, dossierB.SousEpisodeId, 1, "pdf")
        DeposerFichierDeTest(ancien, contenu)

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.croise"), ancien, cible)

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual("Accès refusé", CorpsDe(reponse))
        CollectionAssert.AreEqual(contenu, LireDansZoneDeTest(ancien), "le document de A reste en place")
        Assert.IsFalse(ExisteDansZoneDeTest(cible))
        Assert.AreEqual(1, CompterJournalDocumentsDeTest(idMedecin, dossierA.PatientId,
                                                         "REFUS : Renommage vers un autre dossier refusé : " & ancien & " vers " & cible))
    End Sub

    <TestMethod()> Public Sub UnDocumentNeDevientPasUnModele()
        Dim idMedecin = CreerCompteDocumentsDeTest("rn.versmodele")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        Dim ancien = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "DOCX")
        Dim modele = NomModeleDocumentDeTest(TypeSeCourrier, SousTypeSeAdressage)
        DeposerFichierDeTest(ancien, OctetsDocumentDeTest(60))

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.versmodele"), ancien, modele)

        ' Un modèle est servi à tous les patients : y verser un compte rendu le diffuserait.
        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.IsTrue(ExisteDansZoneDeTest(ancien))
        Assert.IsFalse(ExisteDansZoneDeTest(modele))
        Assert.AreEqual(1, CompterJournalCommencantParDeTest(idMedecin, "REFUS : Renommage vers un autre dossier refusé"))
    End Sub

    <TestMethod()> Public Sub UnModeleNeDevientPasUnDocumentDeDossier()
        Dim idMedecin = CreerCompteDocumentsDeTest("rn.depuismod")
        Dim dossier = CreerDossierDocumentDeTest(idMedecin)
        Dim modele = NomModeleDocumentDeTest(TypeSeCourrier, SousTypeSeAdressage)
        DeposerFichierDeTest(modele, OctetsDocumentDeTest(60))

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.depuismod"), modele, dossier.NomDocument)

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.IsTrue(ExisteDansZoneDeTest(modele))
        Assert.IsFalse(ExisteDansZoneDeTest(dossier.NomDocument))
        ' La trace de refus porte le patient de la source : 0 pour un modèle.
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_action WHERE utilisateur_id = @p0 AND patient_id = 0" &
                                         " AND action LIKE 'REFUS : %'", idMedecin)))
    End Sub

    ' --- Authentification et profil ------------------------------------------------

    <TestMethod()> Public Sub SansEnTeteRienNeBouge()
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("rn.auteur"))
        Dim noms = DeuxNomsDeReponse(dossier, OctetsDocumentDeTest(10))

        Dim reponse = Renommer(Nothing, noms(0), noms(1))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.IsTrue(ExisteDansZoneDeTest(noms(0)))
        Assert.IsFalse(ExisteDansZoneDeTest(noms(1)))
    End Sub

    <TestMethod()> Public Sub UnMauvaisMotDePasseNeBougeRien()
        CreerCompteDocumentsDeTest("rn.mdp")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("rn.auteur"))
        Dim noms = DeuxNomsDeReponse(dossier, OctetsDocumentDeTest(10))

        Dim reponse = Renommer(EnteteBasic("rn.mdp", "Mauvais!2026"), noms(0), noms(1))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.IsTrue(ExisteDansZoneDeTest(noms(0)))
    End Sub

    <TestMethod()> Public Sub UnProfilDeGestionNeRenommeRien()
        CreerCompteDocumentsDeTest("rn.gestion", "GESTION")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("rn.auteur"))
        Dim noms = DeuxNomsDeReponse(dossier, OctetsDocumentDeTest(10))

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.gestion"), noms(0), noms(1))

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.IsTrue(ExisteDansZoneDeTest(noms(0)))
        Assert.IsFalse(ExisteDansZoneDeTest(noms(1)))
    End Sub

    <TestMethod()> Public Sub SansCorpsLaRequeteEstIncomplete()
        CreerCompteDocumentsDeTest("rn.vide")

        UtiliserCompte(Compte.Web)
        Dim reponse = AppelerApi(Of RenameController)(EnteteDocumentsDeTest("rn.vide"), "PostValue",
                                                      Function(c) c.PostValue(Nothing))

        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.AreEqual("Requête incomplète", CorpsDe(reponse))
    End Sub

    ' --- Noms refusés et fichiers absents ------------------------------------------

    <TestMethod()> Public Sub UneCibleEnRemonteeDeRepertoireEstRefusee()
        CreerCompteDocumentsDeTest("rn.remonte")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("rn.auteur"))
        Dim ancien = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "pdf")
        DeposerFichierDeTest(ancien, OctetsDocumentDeTest(10))
        Dim hors = Path.Combine(Path.GetDirectoryName(RacineZoneDocumentsDeTest()), "Intrus.pdf")

        For Each cible In {"..\Intrus.pdf", "SousEpisodeReponse\..\..\Intrus.pdf", ancien & "\..\..\..\Intrus.pdf", "C:\OasisTests\Intrus.pdf"}
            Dim reponse = Renommer(EnteteDocumentsDeTest("rn.remonte"), ancien, cible)
            Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode, cible)
            Assert.IsTrue(ExisteDansZoneDeTest(ancien), cible)
            Assert.IsFalse(File.Exists(hors), cible)
        Next
    End Sub

    <TestMethod()> Public Sub UneSourceEnRemonteeDeRepertoireEstRefusee()
        CreerCompteDocumentsDeTest("rn.source")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("rn.auteur"))
        DeposerFichierDeTest("secret.pdf", OctetsDocumentDeTest(10))
        Dim cible = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "pdf")

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.source"), "SousEpisodeReponse\..\secret.pdf", cible)

        Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode)
        Assert.IsTrue(ExisteDansZoneDeTest("secret.pdf"))
        Assert.IsFalse(ExisteDansZoneDeTest(cible))
    End Sub

    <TestMethod()> Public Sub UneCibleAvecUneExtensionHorsListeEstRefusee()
        CreerCompteDocumentsDeTest("rn.exe")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("rn.auteur"))
        Dim ancien = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "pdf")
        Dim cible = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 2, "exe")
        DeposerFichierDeTest(ancien, OctetsDocumentDeTest(10))

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.exe"), ancien, cible)

        ' Même patient, noms bien formés pour HabilitationsDocuments, mais CheminsDocuments
        ' refuse l'extension : un document ne peut pas devenir exécutable.
        Assert.AreEqual(HttpStatusCode.BadRequest, reponse.StatusCode)
        Assert.AreEqual("Nom de fichier invalide", CorpsDe(reponse))
        Assert.IsTrue(ExisteDansZoneDeTest(ancien))
        Assert.IsFalse(ExisteDansZoneDeTest(cible))
    End Sub

    <TestMethod()> Public Sub UnSousEpisodeInconnuRepond404()
        CreerCompteDocumentsDeTest("rn.inconnu")
        Dim ancien = NomDocumentSousEpisodeDeTest(987654, 876543)
        DeposerFichierDeTest(ancien, OctetsDocumentDeTest(10))

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.inconnu"), ancien, NomDocumentSousEpisodeDeTest(987654, 876544))

        Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode)
        Assert.IsTrue(ExisteDansZoneDeTest(ancien))
    End Sub

    <TestMethod()> Public Sub UneSourceAbsenteDuDisqueRepond404()
        CreerCompteDocumentsDeTest("rn.absent")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("rn.auteur"))
        Dim ancien = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 1, "pdf")
        Dim nouveau = NomDocumentReponseDeTest(dossier.EpisodeId, dossier.SousEpisodeId, 2, "pdf")

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.absent"), ancien, nouveau)

        Assert.AreEqual(HttpStatusCode.NotFound, reponse.StatusCode)
        Assert.AreEqual("Document introuvable", CorpsDe(reponse))
        Assert.IsFalse(ExisteDansZoneDeTest(nouveau))
    End Sub

    <TestMethod()> Public Sub UneCibleDejaPresenteFaitEchouerSansRienEcraser()
        CreerCompteDocumentsDeTest("rn.occupe")
        Dim dossier = CreerDossierDocumentDeTest(CreerCompteDocumentsDeTest("rn.auteur"))
        Dim source = OctetsDocumentDeTest(30, 1)
        Dim present = OctetsDocumentDeTest(40, 2)
        Dim noms = DeuxNomsDeReponse(dossier, source)
        DeposerFichierDeTest(noms(1), present)

        Dim reponse = Renommer(EnteteDocumentsDeTest("rn.occupe"), noms(0), noms(1))

        ' Comportement actuel : File.Move lève IOException quand la cible existe, et
        ' l'appel finit en 500 générique. Aucun des deux fichiers n'est perdu.
        Assert.AreEqual(HttpStatusCode.InternalServerError, reponse.StatusCode)
        Assert.AreEqual("Erreur interne au serveur", CorpsDe(reponse))
        CollectionAssert.AreEqual(source, LireDansZoneDeTest(noms(0)))
        CollectionAssert.AreEqual(present, LireDansZoneDeTest(noms(1)))
    End Sub

End Class
