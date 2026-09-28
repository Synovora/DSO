Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' Prochains rendez-vous du portail (RDVController.Index), sous oasis_web. Même
''' requête de parcours que le tableau de bord, en [oasis].[oasis] : les tests qui
''' dépassent le contrôle d'accès commencent par ExigerBaseOasis. Les dates sont
''' formatées puis relues selon la culture courante : fr-FR, celle du serveur.
''' </summary>
<TestClass()> Public Class RDVControllerTest
    Inherits TestIntegration

    Private Shared Function RendezVous(idInternaute As Long) As List(Of List(Of String))
        Dim vue = EnFrancaisPortail(Function() VuePortail(ControleurPortail(Of RDVController)(idInternaute).Index()))
        Return DirectCast(vue.ViewData("ParcoursDeSoin"), List(Of List(Of String)))
    End Function

    ' --- Contrôle d'accès ---------------------------------------------------------------

    <TestMethod()> Public Sub LeFiltreRefuseUnVisiteurAnonyme()
        VerifierNonAuthentifiePortail(AutorisationPortail(ControleurPortailAnonyme(Of RDVController)(), "Index"))
    End Sub

    <TestMethod()> Public Sub LeFiltreLaissePasserUnInternauteAuthentifie()
        Dim idInternaute = CreerComptePortail(CreerPatient())
        Assert.IsNull(AutorisationPortail(ControleurPortail(Of RDVController)(idInternaute), "Index"))
    End Sub

    <TestMethod()> Public Sub SansFiltreUnAnonymeEstQuandMemeRefuse()
        VerifierAccesRefusePortail(ControleurPortailAnonyme(Of RDVController)().Index())
    End Sub

    <TestMethod()> Public Sub UnInternauteSansPatientEstRefuse()
        VerifierAccesRefusePortail(ControleurPortail(Of RDVController)(CreerInternaute()).Index())
    End Sub

    ' --- Contenu ------------------------------------------------------------------------

    <TestMethod()> Public Sub SeulsLesRendezVousDuPatientConnecteSontMontres()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim internauteA = CreerComptePortail(patientA)
        CreerComptePortail(patientB)
        Dim parcoursA = CreerParcoursPortail(patientA, idUtilisateur)
        Dim parcoursB = CreerParcoursPortail(patientB, idUtilisateur)

        Dim liste = RendezVous(internauteA)

        CollectionAssert.AreEqual(New String() {CStr(parcoursA)}, liste.Select(Function(r) r(0)).ToArray())
        Assert.IsFalse(liste.Any(Function(r) r(0) = CStr(parcoursB)))
    End Sub

    <TestMethod()> Public Sub SansRendezVousPlanifieLaDateEstCalculeeDepuisLaCreation()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        CreerParcoursPortail(idPatient, idUtilisateur, ParcoursDao.EnumParcoursBaseCode.ParAn, 1)

        Dim ligne = RendezVous(idInternaute).Single()

        Assert.IsTrue(ligne(4).StartsWith("automatique prevu en "), ligne(4))
        Dim prevue = EnFrancaisPortail(Function() Date.Parse(ligne(5)))
        Assert.AreEqual(Date.Today.AddDays(365), prevue.Date)
    End Sub

    <TestMethod()> Public Sub LesRendezVousSontTriesParDate()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim dansCinqAns = CreerParcoursPortail(idPatient, idUtilisateur, ParcoursDao.EnumParcoursBaseCode.TousLes5Ans)
        Dim dansDeuxAns = CreerParcoursPortail(idPatient, idUtilisateur, ParcoursDao.EnumParcoursBaseCode.TousLes2Ans)
        Dim dansUnAn = CreerParcoursPortail(idPatient, idUtilisateur, ParcoursDao.EnumParcoursBaseCode.ParAn)

        Dim liste = RendezVous(idInternaute)

        CollectionAssert.AreEqual(New String() {CStr(dansUnAn), CStr(dansDeuxAns), CStr(dansCinqAns)},
                                  liste.Select(Function(r) r(0)).ToArray())
    End Sub

    <TestMethod()> Public Sub UnParcoursSansRythmeNestPasMontre()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        CreerParcoursPortail(idPatient, idUtilisateur, ParcoursDao.EnumParcoursBaseCode.ParAn, 0)
        CreerParcoursPortail(idPatient, idUtilisateur, "", 1)

        Assert.AreEqual(0, RendezVous(idInternaute).Count)
    End Sub

    <TestMethod()> Public Sub LaMiseEnPageParDefautEstVerticale()
        ExigerBaseOasis()
        Dim idInternaute = CreerComptePortail(CreerPatient())

        Dim vue = EnFrancaisPortail(Function() VuePortail(ControleurPortail(Of RDVController)(idInternaute).Index()))

        Assert.AreEqual("LAYOUT_VERTICAL", CStr(vue.ViewData("ModeName")))
        Assert.AreEqual("RDV", CStr(vue.ViewData("WelcomeText")))
        Assert.AreEqual(0, DirectCast(vue.ViewData("ParcoursDeSoin"), List(Of List(Of String))).Count)
    End Sub

End Class
