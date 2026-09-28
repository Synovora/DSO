Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Oasis_Common
Imports Oasis_Web

''' <summary>
''' /api/sendMail : envoi de courriel pour le client lourd, sous le compte du serveur.
'''
''' Aucun courriel ne part. Chaque test retire d'abord tout paramètre SMTP de la
''' base : un envoi qui franchit les contrôles échoue en lisant sa configuration,
''' avant que MailUtil ne soit construit, et répond « Erreur interne au serveur lors
''' de l'envoi du mail ». Ce message distingue l'étape atteinte : il n'apparaît
''' qu'une fois les destinataires et la pièce jointe acceptés. L'envoi SMTP
''' lui-même et la trace SORTIE qui le suit ne sont pas atteignables ici.
''' </summary>
<TestClass()> Public Class SendMailControllerTest
    Inherits TestIntegration

    Private Const EchecEnvoi As String = "Erreur interne au serveur lors de l'envoi du mail"

    <TestInitialize>
    Public Sub PreparerZone()
        RetirerParametresSmtpDeTest()
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

    Private Shared Function Envoyer(entete As AuthenticationHeaderValue, corps As MultipartFormDataContent) As HttpResponseMessage
        ' Garde-fou : rien ne doit pouvoir joindre un serveur SMTP.
        Assert.AreEqual(0, CompterParametresSmtpDeTest(), "paramètre SMTP présent : test interrompu avant tout envoi")
        UtiliserCompte(Compte.Web)
        Return AppelerApiAvecCorpsDeTest(Of SendMailController)(entete, "SendMail",
            Function(c) c.SendMail().Result, corps)
    End Function

    Private Shared Function EnvoyerA(login As String, adresses As String, Optional patientId As Long = 0) As HttpResponseMessage
        Return Envoyer(EnteteDocumentsDeTest(login), CorpsCourrielDeTest(adresses, patientId.ToString()))
    End Function

    ' --- Destinataires acceptés : l'appel va jusqu'à la configuration SMTP ------------

    <TestMethod()> Public Sub UnDomaineConfigurePasseLesControles()
        Dim idMedecin = CreerCompteDocumentsDeTest("ml.domaine")

        Dim reponse = EnvoyerA("ml.domaine", "secretariat@exemple.fr")

        Assert.AreEqual(HttpStatusCode.InternalServerError, reponse.StatusCode)
        Assert.AreEqual(EchecEnvoi, CorpsDe(reponse))
        ' Rien n'est parti, rien n'est tracé comme sorti.
        Assert.AreEqual(0, CompterJournalCommencantParDeTest(idMedecin, "SORTIE"))
        Assert.AreEqual(0, CompterJournalCommencantParDeTest(idMedecin, "REFUS"))
    End Sub

    <TestMethod()> Public Sub LeDomaineConfigureAvecArobaseEtCasseDifferentePasse()
        CreerCompteDocumentsDeTest("ml.casse")

        Dim reponse = EnvoyerA("ml.casse", "Contact@AUTORISE.test")

        Assert.AreEqual(EchecEnvoi, CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub LAdresseDuPatientConcernePasse()
        CreerCompteDocumentsDeTest("ml.patient")
        Dim idPatient = CreerPatientJoignableDeTest("patiente.joignable@messagerie.test")

        Dim reponse = EnvoyerA("ml.patient", "Patiente.Joignable@messagerie.test", idPatient)

        Assert.AreEqual(EchecEnvoi, CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UneAdresseDeLAnnuaireProfessionnelPasse()
        CreerCompteDocumentsDeTest("ml.annuaire")
        InscrireAdresseAnnuaireDeTest("dr.correspondant@mssante.test")

        Dim reponse = EnvoyerA("ml.annuaire", "dr.correspondant@mssante.test")

        Assert.AreEqual(EchecEnvoi, CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub PlusieursDestinatairesAutorisesPassent()
        CreerCompteDocumentsDeTest("ml.plusieurs")
        InscrireAdresseAnnuaireDeTest("dr.correspondant@mssante.test")

        Dim reponse = EnvoyerA("ml.plusieurs", " secretariat@exemple.fr , dr.correspondant@mssante.test ,")

        Assert.AreEqual(EchecEnvoi, CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UnePieceJointeDe25MoPilePasse()
        CreerCompteDocumentsDeTest("ml.limite")
        Dim corps = CorpsCourrielDeTest("secretariat@exemple.fr", "0", OctetsDocumentDeTest(25 * 1024 * 1024))

        Dim reponse = Envoyer(EnteteDocumentsDeTest("ml.limite"), corps)

        Assert.AreEqual(EchecEnvoi, CorpsDe(reponse))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest(), "la pièce jointe reçue est supprimée")
    End Sub

    <TestMethod()> Public Sub AucuneExtensionDePieceJointeNEstRefusee()
        CreerCompteDocumentsDeTest("ml.exe")
        Dim corps = CorpsCourrielDeTest("secretariat@exemple.fr", "0", OctetsDocumentDeTest(100), "..\..\outil.exe")

        Dim reponse = Envoyer(EnteteDocumentsDeTest("ml.exe"), corps)

        ' Comportement actuel : le nom est réduit par Path.GetFileName mais aucune
        ' extension n'est refusée ; un exécutable joint passe les contrôles.
        Assert.AreEqual(EchecEnvoi, CorpsDe(reponse))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
    End Sub

    <TestMethod()> Public Sub UnProfilDeGestionPeutEnvoyer()
        CreerCompteDocumentsDeTest("ml.gestion", "GESTION")

        Dim reponse = EnvoyerA("ml.gestion", "secretariat@exemple.fr")

        ' Comportement actuel : SendMail ne consulte pas PeutAccederAuxDocuments. Un
        ' profil de gestion, refusé en lecture sur les documents, peut les joindre à un
        ' envoi vers un destinataire autorisé.
        Assert.AreEqual(EchecEnvoi, CorpsDe(reponse))
    End Sub

    ' --- Destinataires refusés -------------------------------------------------------

    <TestMethod()> Public Sub UneAdresseInconnueEstRefuseeEtTracee()
        Dim idMedecin = CreerCompteDocumentsDeTest("ml.inconnue")
        Dim idPatient = CreerPatient("DESTINATAIRE", "Refuse")

        Dim reponse = EnvoyerA("ml.inconnue", "quelquun@ailleurs.test", idPatient)

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual("Destinataire non autorisé : quelquun@ailleurs.test", CorpsDe(reponse))
        Assert.AreEqual(1, CompterJournalDocumentsDeTest(idMedecin, idPatient, "REFUS : Envoi refusé vers quelquun@ailleurs.test"))
    End Sub

    <TestMethod()> Public Sub UnSeulDestinataireRefuseBloqueLEnvoi()
        CreerCompteDocumentsDeTest("ml.melange")

        Dim reponse = EnvoyerA("ml.melange", "secretariat@exemple.fr, fuite@ailleurs.test")

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual("Destinataire non autorisé : fuite@ailleurs.test", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub LAdresseDUnAutrePatientEstRefusee()
        CreerCompteDocumentsDeTest("ml.autre")
        CreerPatientJoignableDeTest("autre.patient@messagerie.test")
        Dim idPatientConcerne = CreerPatient("CONCERNE", "Courriel")

        Dim reponse = EnvoyerA("ml.autre", "autre.patient@messagerie.test", idPatientConcerne)

        ' Seule l'adresse du patient du dossier est admise, pas celle de n'importe quel patient.
        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub LAdresseDuPatientSansDossierIndiqueEstRefusee()
        CreerCompteDocumentsDeTest("ml.sansdossier")
        CreerPatientJoignableDeTest("patiente.joignable@messagerie.test")

        Dim reponse = EnvoyerA("ml.sansdossier", "patiente.joignable@messagerie.test", 0)

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub UnPatientInexistantFaitRefuserMemeUneAdresseDeLAnnuaire()
        CreerCompteDocumentsDeTest("ml.fantome")
        InscrireAdresseAnnuaireDeTest("dr.correspondant@mssante.test")

        Dim reponse = EnvoyerA("ml.fantome", "dr.correspondant@mssante.test", 987654321)

        ' Comportement actuel : GetPatient lève pour un id inconnu, et l'exception saute
        ' la recherche dans l'annuaire. Une adresse de correspondant, acceptée sans
        ' dossier (voir UneAdresseDeLAnnuaireProfessionnelPasse), est alors refusée.
        ' Le refus est sûr ; il n'est pas cohérent.
        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub UnSousDomaineDuDomaineAutoriseEstRefuse()
        CreerCompteDocumentsDeTest("ml.sousdom")

        Assert.AreEqual(HttpStatusCode.Forbidden, EnvoyerA("ml.sousdom", "pirate@mal.exemple.fr").StatusCode)
    End Sub

    <TestMethod()> Public Sub UneAdresseMalFormeeEstRefusee()
        CreerCompteDocumentsDeTest("ml.forme")

        For Each adresse In {"pas-une-adresse", "secretariat@exemple", "secretariat@exemple.fr."}
            Assert.AreEqual(HttpStatusCode.Forbidden, EnvoyerA("ml.forme", adresse).StatusCode, adresse)
        Next
    End Sub

    <TestMethod()> Public Sub UnSautDeLigneDansLAdresseEstRefuse()
        CreerCompteDocumentsDeTest("ml.crlf")

        Dim reponse = EnvoyerA("ml.crlf", "secretariat@exemple.fr" & vbCrLf & "Bcc: fuite@ailleurs.test")

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
    End Sub

    <TestMethod()> Public Sub SansDestinataireLEnvoiEstRefuse()
        CreerCompteDocumentsDeTest("ml.personne")

        Dim sansChamp = Envoyer(EnteteDocumentsDeTest("ml.personne"), CorpsCourrielDeTest(Nothing))
        Dim videsSeulement = EnvoyerA("ml.personne", " , ,")

        Assert.AreEqual(HttpStatusCode.BadRequest, sansChamp.StatusCode)
        Assert.AreEqual("Aucun destinataire", CorpsDe(sansChamp))
        Assert.AreEqual(HttpStatusCode.BadRequest, videsSeulement.StatusCode)
    End Sub

    ' --- Pièce jointe et formulaire ---------------------------------------------------

    <TestMethod()> Public Sub UnePieceJointeDePlusDe25MoEstRefusee()
        CreerCompteDocumentsDeTest("ml.gros")
        Dim corps = CorpsCourrielDeTest("secretariat@exemple.fr", "0", OctetsDocumentDeTest(25 * 1024 * 1024 + 1))

        Dim reponse = Envoyer(EnteteDocumentsDeTest("ml.gros"), corps)

        Assert.AreEqual(HttpStatusCode.RequestEntityTooLarge, reponse.StatusCode)
        Assert.AreEqual("Pièce jointe trop volumineuse", CorpsDe(reponse))
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
    End Sub

    <TestMethod()> Public Sub UnePieceJointeVersUnDestinataireRefuseNeResteNullePart()
        CreerCompteDocumentsDeTest("ml.pjrefus")
        Dim corps = CorpsCourrielDeTest("fuite@ailleurs.test", "0", OctetsDocumentDeTest(5000))

        Dim reponse = Envoyer(EnteteDocumentsDeTest("ml.pjrefus"), corps)

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
    End Sub

    <TestMethod()> Public Sub SansIndicateursLeFormulaireEchoueAvantLesControles()
        CreerCompteDocumentsDeTest("ml.indic")

        Dim reponse = Envoyer(EnteteDocumentsDeTest("ml.indic"),
                              CorpsCourrielDeTest("fuite@ailleurs.test", "0", avecIndicateurs:=False))

        ' Comportement actuel : isSousEpisode absent donne Nothing, que la conversion
        ' implicite en Boolean refuse. L'appel finit en 500 générique, avant même le
        ' contrôle des destinataires (d'où l'absence de 403).
        Assert.AreEqual(HttpStatusCode.InternalServerError, reponse.StatusCode)
        Assert.AreEqual("Erreur interne au serveur", CorpsDe(reponse))
    End Sub

    <TestMethod()> Public Sub UnIdentifiantDePatientIllisibleVautZero()
        Dim idMedecin = CreerCompteDocumentsDeTest("ml.idpat")

        Dim reponse = Envoyer(EnteteDocumentsDeTest("ml.idpat"), CorpsCourrielDeTest("fuite@ailleurs.test", "douze"))

        Assert.AreEqual(HttpStatusCode.Forbidden, reponse.StatusCode)
        Assert.AreEqual(1, CompterJournalDocumentsDeTest(idMedecin, 0, "REFUS : Envoi refusé vers fuite@ailleurs.test"))
    End Sub

    ' --- Authentification ------------------------------------------------------------

    <TestMethod()> Public Sub SansEnTeteLEnvoiEstRefuse()
        Dim reponse = Envoyer(Nothing, CorpsCourrielDeTest("secretariat@exemple.fr"))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
        Assert.AreEqual(0, FichiersTemporairesRestantsDeTest())
    End Sub

    <TestMethod()> Public Sub UnMauvaisMotDePasseEstRefuse()
        CreerCompteDocumentsDeTest("ml.mdp")

        Dim reponse = Envoyer(EnteteBasic("ml.mdp", "Mauvais!2026"), CorpsCourrielDeTest("secretariat@exemple.fr"))

        Assert.AreEqual(HttpStatusCode.Unauthorized, reponse.StatusCode)
    End Sub

End Class
