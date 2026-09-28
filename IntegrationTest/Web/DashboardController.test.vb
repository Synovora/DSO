Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' Tableau de bord du portail patient (DashboardController.Index), sous oasis_web :
''' fiche du patient, dernières connexions de l'internaute, parcours de soins. Le
''' parcours est lu par une requête qui nomme la base en dur ([oasis].[oasis]) :
''' les tests qui dépassent le contrôle d'accès commencent par ExigerBaseOasis.
''' </summary>
<TestClass()> Public Class DashboardControllerTest
    Inherits TestIntegration

    ''' <summary>
    ''' Connexion enregistrée par InternauteConnectionDao.Create sous le compte Web,
    ''' comme AuthController.Login. Le compte reste Web ensuite : à appeler en dernier.
    ''' </summary>
    Private Shared Function EnregistrerConnexion(idInternaute As Long, ip As String) As Long
        UtiliserCompte(Compte.Web)
        Dim dao As New InternauteConnectionDao
        Return dao.Create(New InternauteConnection With {.Internaute = idInternaute, .Datetime = Date.Now, .Ip = ip})
    End Function

    Private Shared Function Tableau(idInternaute As Long) As ViewResult
        Return VuePortail(ControleurPortail(Of DashboardController)(idInternaute).Index())
    End Function

    Private Shared Function ParcoursAffiches(vue As ViewResult) As List(Of List(Of String))
        Return DirectCast(vue.ViewData("ParcoursDeSoin"), List(Of List(Of String)))
    End Function

    ' --- Contrôle d'accès ---------------------------------------------------------------

    <TestMethod()> Public Sub LeFiltreRefuseUnVisiteurAnonyme()
        VerifierNonAuthentifiePortail(AutorisationPortail(ControleurPortailAnonyme(Of DashboardController)(), "Index"))
    End Sub

    <TestMethod()> Public Sub LeFiltreLaissePasserUnInternauteAuthentifie()
        Dim idInternaute = CreerComptePortail(CreerPatient())
        Assert.IsNull(AutorisationPortail(ControleurPortail(Of DashboardController)(idInternaute), "Index"))
    End Sub

    <TestMethod()> Public Sub SansFiltreUnAnonymeEstQuandMemeRefuse()
        VerifierAccesRefusePortail(ControleurPortailAnonyme(Of DashboardController)().Index())
    End Sub

    <TestMethod()> Public Sub UnTicketIllisibleEstRefuse()
        VerifierAccesRefusePortail(ControleurPortailPour(Of DashboardController)(PrincipalTicketPortail("admin")).Index())
    End Sub

    <TestMethod()> Public Sub UnInternauteSansPatientEstRefuse()
        Dim idInternaute = CreerInternaute()
        VerifierAccesRefusePortail(ControleurPortail(Of DashboardController)(idInternaute).Index())
    End Sub

    ' --- Contenu ------------------------------------------------------------------------

    <TestMethod()> Public Sub LeTableauNeMontreQueLeDossierDuPatientConnecte()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim internauteA = CreerComptePortail(patientA)
        Dim internauteB = CreerComptePortail(patientB)
        Dim parcoursA = CreerParcoursPortail(patientA, idUtilisateur)
        Dim parcoursB = CreerParcoursPortail(patientB, idUtilisateur)
        Dim connexionA = EnregistrerConnexion(internauteA, "192.0.2.1")
        EnregistrerConnexion(internauteB, "192.0.2.2")

        Dim vue = Tableau(internauteA)

        Assert.AreEqual("", vue.ViewName)
        Assert.AreEqual(CInt(patientA), DirectCast(vue.ViewData("Patient"), Patient).PatientId)
        Dim connexions = DirectCast(vue.ViewData("Connections"), List(Of InternauteConnection))
        Assert.AreEqual(1, connexions.Count)
        Assert.AreEqual(connexionA, connexions(0).Id)
        Assert.AreEqual(internauteA, connexions(0).Internaute)
        Dim parcours = ParcoursAffiches(vue)
        CollectionAssert.AreEqual(New String() {CStr(parcoursA)}, parcours.Select(Function(p) p(0)).ToArray())
        Assert.IsFalse(parcours.Any(Function(p) p(0) = CStr(parcoursB)), "parcours du patient B absent")
    End Sub

    <TestMethod()> Public Sub LesSixDernieresConnexionsSontMontreesDeLaPlusRecenteALaPlusAncienne()
        ExigerBaseOasis()
        Dim idInternaute = CreerComptePortail(CreerPatient())
        Dim ids As New List(Of Long)
        For i = 1 To 7
            ids.Add(EnregistrerConnexion(idInternaute, "192.0.2." & i))
        Next

        Dim connexions = DirectCast(Tableau(idInternaute).ViewData("Connections"), List(Of InternauteConnection))

        ids.Reverse()
        CollectionAssert.AreEqual(ids.Take(6).ToArray(), connexions.Select(Function(c) c.Id).ToArray())
    End Sub

    <TestMethod()> Public Sub UnParcoursMasqueOuAnnuleSuitLaRequete()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim actif = CreerParcoursPortail(idPatient, idUtilisateur)
        Dim annule = CreerParcoursPortail(idPatient, idUtilisateur, ParcoursDao.EnumParcoursBaseCode.TousLes2Ans)
        Executer("UPDATE oasis.oa_patient_parcours SET oa_parcours_inactif = 1 WHERE oa_parcours_id = @p0", annule)

        Dim parcours = ParcoursAffiches(Tableau(idInternaute))

        CollectionAssert.AreEqual(New String() {CStr(actif)}, parcours.Select(Function(p) p(0)).ToArray())
        ' Sans rendez-vous ni demande en cours, les deux dernières colonnes sont vides.
        Assert.IsNull(parcours(0)(4))
        Assert.IsNull(parcours(0)(5))
    End Sub

    <TestMethod()> Public Sub SansParcoursLaListeEstVide()
        ExigerBaseOasis()
        Dim idInternaute = CreerComptePortail(CreerPatient())

        Dim vue = Tableau(idInternaute)

        Assert.AreEqual(0, ParcoursAffiches(vue).Count)
        Assert.AreEqual(0, DirectCast(vue.ViewData("Connections"), List(Of InternauteConnection)).Count)
    End Sub

    <TestMethod()> Public Sub LaMiseEnPageVientDeTempDataSinonDesValeursParDefaut()
        ExigerBaseOasis()
        Dim idInternaute = CreerComptePortail(CreerPatient())

        Dim parDefaut = Tableau(idInternaute)
        Assert.AreEqual("LAYOUT_VERTICAL", CStr(parDefaut.ViewData("ModeName")))
        Assert.AreEqual("Dashboard", CStr(parDefaut.ViewData("WelcomeText")))

        Dim controleur = ControleurPortail(Of DashboardController)(idInternaute)
        controleur.TempData("ModeName") = "LAYOUT_HORIZONTAL"
        controleur.TempData("WelcomeText") = "Bienvenue"
        Dim vue = VuePortail(controleur.Index())
        Assert.AreEqual("LAYOUT_HORIZONTAL", CStr(vue.ViewData("ModeName")))
        Assert.AreEqual("Bienvenue", CStr(vue.ViewData("WelcomeText")))
    End Sub

End Class
