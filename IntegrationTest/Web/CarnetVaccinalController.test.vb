Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' Carnet vaccinal du portail (CarnetVaccinalController.Index), sous oasis_web :
''' une ligne par vaccin programmé au calendrier du patient et administré (lot et
''' péremption saisis). La liste des vaccins est lue en [oasis].[oasis] : les
''' tests qui dépassent le contrôle d'accès commencent par ExigerBaseOasis. Les
''' dates de réalisation sont formatées selon la culture courante : fr-FR.
''' </summary>
<TestClass()> Public Class CarnetVaccinalControllerTest
    Inherits TestIntegration

    Private idOperateur As Long

    <TestInitialize>
    Public Sub PreparerOperateur()
        idOperateur = CreerUtilisateur(avecCle:=False)
    End Sub

    ''' <summary>
    ''' Vaccin rattaché à une valence, programmé au calendrier du patient et, si
    ''' lot est donné, administré. Renvoie l'id de la programmation.
    ''' </summary>
    Private Function Programmer(idPatient As Long, dci As String, Optional lot As String = Nothing) As Long
        Dim code = NouveauCodeVaccin()
        Dim idVaccin = CreerVaccin(code, dci, idOperateur)
        LierVaccinValence(code, CreerValence(utilisateurId:=idOperateur))
        Dim idDate = CreerDateCgv(60, idPatient)
        Dim idProgramme = ProgrammerVaccin(idDate, idPatient, idVaccin, code)
        If lot IsNot Nothing Then CreerAdministrationVaccin(idProgramme, lot)
        Return idProgramme
    End Function

    ''' <summary>
    ''' Réalisation saisie comme RadFVaccinInput (VaccinDao.UpdateVaccinProgramRelation) :
    ''' opérateur Oasis, ou 0 et un texte libre.
    ''' </summary>
    Private Shared Sub Realiser(idProgramme As Long, quand As Date, Optional operateur As Long = 0, Optional texte As String = "")
        Dim idDate = CLng(Scalaire("SELECT date FROM oasis.oa_vaccin_program_relation WHERE id = @p0", idProgramme))
        Dim idPatient = CLng(Scalaire("SELECT patient FROM oasis.oa_vaccin_program_relation WHERE id = @p0", idProgramme))
        Dim dao As New VaccinDao
        Dim ligne = dao.GetVaccinProgramRelationListDatePatient(idDate, idPatient).Single(Function(p) p.Id = idProgramme)
        ligne.RealisationDate = quand
        ligne.RealisationOperator = operateur
        ligne.RealisationOperatorRor = 0
        ligne.RealisationOperatorText = texte
        dao.UpdateVaccinProgramRelation(ligne)
    End Sub

    Private Shared Function Carnet(idInternaute As Long) As ViewResult
        Return EnFrancaisPortail(Function() VuePortail(ControleurPortail(Of CarnetVaccinalController)(idInternaute).Index()))
    End Function

    Private Shared Function Colonne(vue As ViewResult, nom As String) As String()
        Return DirectCast(vue.ViewData(nom), List(Of String)).ToArray()
    End Function

    ' --- Contrôle d'accès ---------------------------------------------------------------

    <TestMethod()> Public Sub LeFiltreRefuseUnVisiteurAnonyme()
        VerifierNonAuthentifiePortail(AutorisationPortail(ControleurPortailAnonyme(Of CarnetVaccinalController)(), "Index"))
    End Sub

    <TestMethod()> Public Sub LeFiltreLaissePasserUnInternauteAuthentifie()
        Dim idInternaute = CreerComptePortail(CreerPatient())
        Assert.IsNull(AutorisationPortail(ControleurPortail(Of CarnetVaccinalController)(idInternaute), "Index"))
    End Sub

    <TestMethod()> Public Sub SansFiltreUnAnonymeEstQuandMemeRefuse()
        VerifierAccesRefusePortail(ControleurPortailAnonyme(Of CarnetVaccinalController)().Index())
    End Sub

    <TestMethod()> Public Sub UnInternauteSansPatientEstRefuse()
        VerifierAccesRefusePortail(ControleurPortail(Of CarnetVaccinalController)(CreerInternaute()).Index())
    End Sub

    ' --- Contenu ------------------------------------------------------------------------

    <TestMethod()> Public Sub LeCarnetNeMontreQueLesVaccinsDuPatientConnecte()
        ExigerBaseOasis()
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim internauteA = CreerComptePortail(patientA)
        CreerComptePortail(patientB)
        Realiser(Programmer(patientA, "VACCIN ALPHA", "LOT-ALPHA"), New Date(2026, 2, 3), texte:="Dr Alpha")
        Realiser(Programmer(patientB, "VACCIN BETA", "LOT-BETA"), New Date(2026, 4, 5), texte:="Dr Beta")

        Dim vue = Carnet(internauteA)

        Assert.AreEqual(CInt(patientA), DirectCast(vue.ViewData("Patient"), Patient).PatientId)
        CollectionAssert.AreEqual(New String() {"03/02/2026 - Dr Alpha"}, Colonne(vue, "Realisation"))
        CollectionAssert.AreEqual(New String() {"VACCIN ALPHA"}, Colonne(vue, "Dci"))
        CollectionAssert.AreEqual(New String() {"LOT-ALPHA"}, Colonne(vue, "Lot"))
        CollectionAssert.AreEqual(New String() {"06/2027"}, Colonne(vue, "Exp"))
    End Sub

    <TestMethod()> Public Sub UnVaccinSansAdministrationNestPasMontre()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Programmer(idPatient, "VACCIN SANS LOT")

        Assert.AreEqual(0, Colonne(Carnet(idInternaute), "Dci").Length)
    End Sub

    <TestMethod()> Public Sub UnVaccinAdministreMaisNonRealiseAUneDateVide()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Programmer(idPatient, "VACCIN EN ATTENTE", "LOT-ATTENTE")

        ' Comportement actuel : la date nulle est affichée telle quelle, 01/01/0001.
        CollectionAssert.AreEqual(New String() {"01/01/0001 - "}, Colonne(Carnet(idInternaute), "Realisation"))
    End Sub

    <TestMethod()> Public Sub UnOperateurOasisEstAfficheParSonNom()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Realiser(Programmer(idPatient, "VACCIN OASIS", "LOT-OASIS"), New Date(2026, 3, 9), operateur:=idOperateur)

        Dim realisation = Colonne(Carnet(idInternaute), "Realisation").Single()

        Assert.IsTrue(realisation.StartsWith("09/03/2026 - (Utilisateur TEST"), realisation)
    End Sub

    <TestMethod()> Public Sub SansVaccinLeCarnetEstVide()
        ExigerBaseOasis()
        Dim idInternaute = CreerComptePortail(CreerPatient())

        Dim vue = Carnet(idInternaute)

        Assert.AreEqual(0, Colonne(vue, "Realisation").Length)
        Assert.AreEqual("Carnet Vaccinal", CStr(vue.ViewData("WelcomeText")))
    End Sub

End Class
