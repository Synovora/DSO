Imports System.Configuration
Imports System.IO
Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' Résultats du portail (ResultatsController), sous oasis_web : liste paginée et
''' filtrée des réponses reçues aux sous-épisodes du patient, et téléchargement du
''' document. Les réponses sont lues par une requête en [oasis].[oasis] : les tests
''' qui dépassent le contrôle d'accès commencent par ExigerBaseOasis.
''' </summary>
<TestClass()> Public Class ResultatsControllerTest
    Inherits TestIntegration

    Private idUtilisateur As Long

    <TestInitialize>
    Public Sub PreparerReferentiel()
        ' La requête joint le type d'activité de l'épisode (oa_r_activite_episode).
        CreerActiviteEpisode("PATHOLOGIE_AIGUE", 1)
        idUtilisateur = CreerUtilisateur(avecCle:=False)
    End Sub

    ''' <summary>Réponse à un nouveau sous-épisode de l'épisode donné. Renvoie l'id de la réponse.</summary>
    Private Function Reponse(idEpisode As Long, Optional sousType As Long = SousTypeSeAdressage,
                             Optional horodate As Date? = Nothing) As Long
        Dim idSousEpisode = CreerSousEpisode(idEpisode, idUtilisateur, sousType)
        Return CreerReponseSousEpisode(idSousEpisode, idUtilisateur, horodate:=horodate)
    End Function

    ''' <summary>Nom du document côté serveur, tel que GetFilenameServer le calcule.</summary>
    Private Shared Function NomServeur(idReponse As Long) As String
        Dim idSousEpisode = CLng(Scalaire("SELECT id_sous_episode FROM oasis.oa_sous_episode_reponse WHERE id = @p0", idReponse))
        Dim idEpisode = CLng(Scalaire("SELECT episode_id FROM oasis.oa_sous_episode_reponse WHERE id = @p0", idReponse))
        Return $"SousEpisodeReponse\Episode_{idEpisode}_SousEpisode_{idSousEpisode}_SousEpisodeReponse_{idReponse}.pdf"
    End Function

    Private Shared Function SousEpisodeDe(idReponse As Long) As Long
        Return CLng(Scalaire("SELECT id_sous_episode FROM oasis.oa_sous_episode_reponse WHERE id = @p0", idReponse))
    End Function

    Private Shared Function Liste(idInternaute As Long, Optional libelle As String = Nothing,
                                  Optional sousLibelle As String = Nothing, Optional page As Integer = 0) As ViewResult
        Return VuePortail(ControleurPortail(Of ResultatsController)(idInternaute).Index(libelle, sousLibelle, page))
    End Function

    ''' <summary>Ids des sous-épisodes affichés, dans l'ordre des groupes.</summary>
    Private Shared Function Groupes(vue As ViewResult) As Long()
        Dim resultats = DirectCast(vue.ViewData("Resultats"), IEnumerable)
        Return resultats.Cast(Of Object)().Select(Function(g) CLng(ProprietePortail(g, "Value"))).ToArray()
    End Function

    Private Shared Function Telecharger(idInternaute As Long, nomFichier As String) As ActionResult
        Return ControleurPortail(Of ResultatsController)(idInternaute).Download(nomFichier)
    End Function

    ''' <summary>Dépose un document sous FileUploadLocation et renvoie son chemin complet.</summary>
    Private Shared Function Deposer(nomRelatif As String) As String
        Dim chemin = Path.GetFullPath(Path.Combine(ConfigurationManager.AppSettings("FileUploadLocation"), nomRelatif))
        Directory.CreateDirectory(Path.GetDirectoryName(chemin))
        File.WriteAllBytes(chemin, New Byte() {&H25, &H50, &H44, &H46})
        Return chemin
    End Function

    ' --- Contrôle d'accès ---------------------------------------------------------------

    <TestMethod()> Public Sub LeFiltreRefuseUnVisiteurAnonyme()
        Dim controleur = ControleurPortailAnonyme(Of ResultatsController)()
        VerifierNonAuthentifiePortail(AutorisationPortail(controleur, "Index", GetType(String), GetType(String), GetType(Integer)))
        VerifierNonAuthentifiePortail(AutorisationPortail(controleur, "Download", GetType(String)))
        VerifierNonAuthentifiePortail(AutorisationPortail(controleur, "Pagination", GetType(Integer)))
    End Sub

    <TestMethod()> Public Sub LeFiltreLaissePasserUnInternauteAuthentifie()
        Dim controleur = ControleurPortail(Of ResultatsController)(CreerComptePortail(CreerPatient()))
        Assert.IsNull(AutorisationPortail(controleur, "Index", GetType(String), GetType(String), GetType(Integer)))
        Assert.IsNull(AutorisationPortail(controleur, "Download", GetType(String)))
        Assert.IsNull(AutorisationPortail(controleur, "Pagination", GetType(Integer)))
    End Sub

    <TestMethod()> Public Sub SansFiltreUnAnonymeEstQuandMemeRefuse()
        VerifierAccesRefusePortail(ControleurPortailAnonyme(Of ResultatsController)().Index(Nothing, Nothing))
        VerifierAccesRefusePortail(ControleurPortailAnonyme(Of ResultatsController)().Download("SousEpisodeReponse\x.pdf"))
    End Sub

    <TestMethod()> Public Sub UnInternauteSansPatientEstRefuse()
        Dim idInternaute = CreerInternaute()
        VerifierAccesRefusePortail(ControleurPortail(Of ResultatsController)(idInternaute).Index(Nothing, Nothing))
        VerifierAccesRefusePortail(ControleurPortail(Of ResultatsController)(idInternaute).Download("SousEpisodeReponse\x.pdf"))
    End Sub

    ' --- Liste --------------------------------------------------------------------------

    <TestMethod()> Public Sub LaListeNeMontreQueLesResultatsDuPatientConnecte()
        ExigerBaseOasis()
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim internauteA = CreerComptePortail(patientA)
        CreerComptePortail(patientB)
        Dim reponseA = Reponse(CreerEpisode(patientA, idUtilisateur))
        Dim reponseB = Reponse(CreerEpisode(patientB, idUtilisateur))

        Dim vue = Liste(internauteA)

        Assert.AreEqual(CInt(patientA), DirectCast(vue.ViewData("Patient"), Patient).PatientId)
        CollectionAssert.AreEqual(New Long() {SousEpisodeDe(reponseA)}, Groupes(vue))
        Assert.AreEqual(1, CInt(vue.ViewData("PageTotal")))
        Dim groupe = DirectCast(vue.ViewData("Resultats"), IEnumerable).Cast(Of Object)().Single()
        Dim affichee = DirectCast(ProprietePortail(groupe, "Element"), IEnumerable(Of SousEpisodeReponse)).Single()
        Assert.AreEqual(reponseA, affichee.Id)
        Assert.AreEqual(NomServeur(reponseA), affichee.NomFichier)
        Assert.AreNotEqual(SousEpisodeDe(reponseB), affichee.IdSousEpisode)
    End Sub

    <TestMethod()> Public Sub SansExtensionConnueLeTypeDeFichierEstInconnu()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Reponse(CreerEpisode(idPatient, idUtilisateur))
        Executer("DELETE FROM oasis.oa_r_file_extension")

        Dim groupe = DirectCast(Liste(idInternaute).ViewData("Resultats"), IEnumerable).Cast(Of Object)().Single()

        Assert.AreEqual("fichier inconnu",
                        DirectCast(ProprietePortail(groupe, "Element"), IEnumerable(Of SousEpisodeReponse)).Single().Commentaire)
    End Sub

    <TestMethod()> Public Sub LesFiltresProposentLesTypesDuPatientEtFiltrent()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim adressage = Reponse(idEpisode, SousTypeSeAdressage)
        Dim compteRendu = Reponse(idEpisode, SousTypeSeCompteRendu)
        Dim certificat = Reponse(idEpisode, SousTypeSeCertificat)

        Dim tous = Liste(idInternaute)
        Dim libelles = DirectCast(tous.ViewData("sousEpisodeLibelles"), List(Of SelectListItem)).Select(Function(i) i.Value).ToList()
        Assert.AreEqual("Tous", libelles(0))
        CollectionAssert.AreEquivalent(New String() {"Tous", LibelleTypeSeCourrier, LibelleTypeSeCertificat}, libelles)
        Assert.AreEqual(0, DirectCast(tous.ViewData("SousEpisodeSousLibelle"), List(Of SelectListItem)).Count)
        Assert.AreEqual(3, Groupes(tous).Length)

        Dim courriers = Liste(idInternaute, LibelleTypeSeCourrier)
        CollectionAssert.AreEquivalent(New Long() {SousEpisodeDe(adressage), SousEpisodeDe(compteRendu)}, Groupes(courriers))
        CollectionAssert.AreEquivalent(New String() {"Tous", LibelleSousTypeSeAdressage, LibelleSousTypeSeCompteRendu},
            DirectCast(courriers.ViewData("SousEpisodeSousLibelle"), List(Of SelectListItem)).Select(Function(i) i.Value).ToArray())

        CollectionAssert.AreEqual(New Long() {SousEpisodeDe(compteRendu)},
                                  Groupes(Liste(idInternaute, LibelleTypeSeCourrier, LibelleSousTypeSeCompteRendu)))
        CollectionAssert.AreEqual(New Long() {SousEpisodeDe(certificat)}, Groupes(Liste(idInternaute, LibelleTypeSeCertificat)))
        Assert.AreEqual(3, Groupes(Liste(idInternaute, "Tous")).Length)
    End Sub

    <TestMethod()> Public Sub UnSousLibelleEtrangerAuTypeEstIgnore()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim adressage = Reponse(idEpisode, SousTypeSeAdressage)
        Reponse(idEpisode, SousTypeSeCertificat)

        CollectionAssert.AreEqual(New Long() {SousEpisodeDe(adressage)},
                                  Groupes(Liste(idInternaute, LibelleTypeSeCourrier, LibelleSousTypeSeCertificat)))
    End Sub

    <TestMethod()> Public Sub LaListeEstPagineeParDixDuPlusRecentAuPlusAncien()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim sousEpisodes As New List(Of Long)
        For i = 1 To 11
            sousEpisodes.Add(SousEpisodeDe(Reponse(idEpisode, horodate:=New Date(2026, 1, i, 9, 0, 0))))
        Next
        sousEpisodes.Reverse()

        Dim premiere = Liste(idInternaute, page:=0)
        Dim seconde = Liste(idInternaute, page:=1)

        CollectionAssert.AreEqual(sousEpisodes.Take(10).ToArray(), Groupes(premiere))
        CollectionAssert.AreEqual(sousEpisodes.Skip(10).ToArray(), Groupes(seconde))
        Assert.AreEqual(11, CInt(premiere.ViewData("PageTotal")))
        Assert.AreEqual(10, CInt(premiere.ViewData("PageCount")))
        Assert.AreEqual(1, CInt(seconde.ViewData("Page")))
    End Sub

    <TestMethod()> Public Sub LaPaginationRepond204()
        Dim controleur = ControleurPortail(Of ResultatsController)(CreerComptePortail(CreerPatient()))

        Dim resultat = controleur.Pagination(3)

        VerifierStatutPortail(resultat, 204)
        Assert.AreEqual(3, CInt(controleur.ViewData("Page")))
    End Sub

    ' --- Téléchargement -----------------------------------------------------------------

    <TestMethod()> Public Sub LePatientTelechargeSonDocument()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim nom = NomServeur(Reponse(CreerEpisode(idPatient, idUtilisateur)))
        Dim chemin = Deposer(nom)
        Try
            For Each demande In {nom, nom.Replace("\"c, "/"c), "\" & nom}
                Dim resultat = Telecharger(idInternaute, demande)
                Assert.IsInstanceOfType(resultat, GetType(FilePathResult), demande)
                Dim fichier = DirectCast(resultat, FilePathResult)
                Assert.AreEqual(chemin, fichier.FileName, True)
                Assert.AreEqual("application/pdf", fichier.ContentType)
                Assert.AreEqual(Path.GetFileName(chemin), fichier.FileDownloadName)
            Next
        Finally
            File.Delete(chemin)
        End Try
    End Sub

    <TestMethod()> Public Sub LeDocumentDUnAutrePatientEstIntrouvableMemeSIlExiste()
        ExigerBaseOasis()
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim internauteA = CreerComptePortail(patientA)
        Dim internauteB = CreerComptePortail(patientB)
        Reponse(CreerEpisode(patientA, idUtilisateur))
        Dim nomB = NomServeur(Reponse(CreerEpisode(patientB, idUtilisateur)))
        Dim cheminB = Deposer(nomB)
        Try
            VerifierStatutPortail(Telecharger(internauteA, nomB), 404, "Document introuvable")
            ' Le propriétaire, lui, l'obtient : c'est bien l'appartenance qui refuse.
            Assert.IsInstanceOfType(Telecharger(internauteB, nomB), GetType(FilePathResult))
        Finally
            File.Delete(cheminB)
        End Try
    End Sub

    <TestMethod()> Public Sub UnDocumentAbsentDuDisqueEstIntrouvable()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim nom = NomServeur(Reponse(CreerEpisode(idPatient, idUtilisateur)))
        Dim chemin = Path.Combine(ConfigurationManager.AppSettings("FileUploadLocation"), nom)
        If File.Exists(chemin) Then File.Delete(chemin)

        VerifierStatutPortail(Telecharger(idInternaute, nom), 404, "Document introuvable")
    End Sub

    <TestMethod()> Public Sub UnNomHorsListeEstIntrouvable()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim nom = NomServeur(Reponse(CreerEpisode(idPatient, idUtilisateur)))

        For Each demande In {Nothing, "", "..\..\Windows\win.ini", "SousEpisodeReponse\..\" & nom, "C:\Windows\win.ini",
                             nom & ".pdf", nom.Replace("SousEpisodeReponse", "SousEpisode")}
            VerifierStatutPortail(Telecharger(idInternaute, demande), 404, "Document introuvable")
        Next
    End Sub

End Class
