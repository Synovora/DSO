Imports System.Security.Principal
Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' PortailController, base de tous les écrans du portail patient : de l'identité
''' authentifiée à l'internaute, puis au patient affiché. C'est la seule barrière
''' entre un internaute et le dossier d'un autre patient ; les écrans n'acceptent
''' aucun identifiant de patient venu du client. Sous oasis_web.
''' </summary>
<TestClass()> Public Class PortailControllerTest
    Inherits TestIntegration

    ''' <summary>Expose les membres protégés de la classe de base, sans rien y changer.</summary>
    Private NotInheritable Class SondePortail
        Inherits PortailController

        Public Function InternauteConnecte() As Integer?
            Return GetInternauteIdConnecte()
        End Function

        Public Function PatientConnecte() As Patient
            Return GetPatientConnecte()
        End Function

        Public Function Refus() As ActionResult
            Return AccesRefuse()
        End Function
    End Class

    ''' <summary>Identité qui porte un nom mais se déclare non authentifiée.</summary>
    Private NotInheritable Class IdentiteNonAuthentifiee
        Implements IIdentity

        Private ReadOnly _nom As String

        Public Sub New(nom As String)
            _nom = nom
        End Sub

        Public ReadOnly Property AuthenticationType As String Implements IIdentity.AuthenticationType
            Get
                Return "Forms"
            End Get
        End Property

        Public ReadOnly Property IsAuthenticated As Boolean Implements IIdentity.IsAuthenticated
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property Name As String Implements IIdentity.Name
            Get
                Return _nom
            End Get
        End Property
    End Class

    Private Shared Function Sonde(principal As IPrincipal) As SondePortail
        Return ControleurPortailPour(Of SondePortail)(principal)
    End Function

    ' --- GetInternauteIdConnecte --------------------------------------------------------

    <TestMethod()> Public Sub LeTicketDonneLIdDeLInternaute()
        Assert.AreEqual(4242, Sonde(PrincipalInternautePortail(4242)).InternauteConnecte())
    End Sub

    <TestMethod()> Public Sub SansIdentiteAucunInternaute()
        Assert.IsFalse(Sonde(Nothing).InternauteConnecte().HasValue, "User Nothing")
        Assert.IsFalse(Sonde(PrincipalAnonymePortail()).InternauteConnecte().HasValue, "anonyme")
        Assert.IsFalse(Sonde(New GenericPrincipal(New IdentiteNonAuthentifiee("12"), New String() {})).InternauteConnecte().HasValue,
                       "nom numérique mais non authentifié")
    End Sub

    <TestMethod()> Public Sub UnNomDeTicketNonNumeriqueNeDonneAucunInternaute()
        For Each nomTicket In {"abc", "12abc", "99999999999", "1.5", ""}
            Assert.IsFalse(Sonde(PrincipalTicketPortail(nomTicket)).InternauteConnecte().HasValue, "ticket : " & nomTicket)
        Next
    End Sub

    <TestMethod()> Public Sub UnNomDeTicketEntoureDEspacesEstLuQuandMeme()
        ' Comportement actuel : Integer.TryParse tolère les espaces et le signe.
        Assert.AreEqual(12, Sonde(PrincipalTicketPortail(" 12 ")).InternauteConnecte())
        Assert.AreEqual(-5, Sonde(PrincipalTicketPortail("-5")).InternauteConnecte())
    End Sub

    ' --- GetPatientConnecte -------------------------------------------------------------

    <TestMethod()> Public Sub LInternauteVoitLePatientDeSaPermission()
        Dim idPatient = CreerPatient("PORTAIL", "Alpha")
        Dim idInternaute = CreerComptePortail(idPatient)

        Dim patient = Sonde(PrincipalInternautePortail(idInternaute)).PatientConnecte()

        Assert.AreEqual(CInt(idPatient), patient.PatientId)
        Assert.AreEqual("PORTAIL", patient.PatientNom.Trim())
    End Sub

    <TestMethod()> Public Sub DeuxInternautesVoientChacunLeurPatient()
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim internauteA = CreerComptePortail(patientA)
        Dim internauteB = CreerComptePortail(patientB)

        Assert.AreEqual(CInt(patientA), Sonde(PrincipalInternautePortail(internauteA)).PatientConnecte().PatientId)
        Assert.AreEqual(CInt(patientB), Sonde(PrincipalInternautePortail(internauteB)).PatientConnecte().PatientId)
    End Sub

    <TestMethod()> Public Sub SansPermissionAucunPatient()
        Dim idInternaute = CreerInternaute()
        CreerPatient()

        Assert.IsNull(Sonde(PrincipalInternautePortail(idInternaute)).PatientConnecte())
    End Sub

    <TestMethod()> Public Sub UnTicketVersUnInternauteInexistantNeDonneAucunPatient()
        CreerComptePortail(CreerPatient())
        Dim inexistant = CLng(Scalaire("SELECT COALESCE(MAX(id), 0) + 1000 FROM oasis.oa_internaute"))

        Assert.IsNull(Sonde(PrincipalInternautePortail(inexistant)).PatientConnecte())
        Assert.IsNull(Sonde(PrincipalTicketPortail("-5")).PatientConnecte())
    End Sub

    <TestMethod()> Public Sub SansIdentiteValableAucunPatient()
        Assert.IsNull(Sonde(Nothing).PatientConnecte())
        Assert.IsNull(Sonde(PrincipalAnonymePortail()).PatientConnecte())
        Assert.IsNull(Sonde(PrincipalTicketPortail("abc")).PatientConnecte())
    End Sub

    <TestMethod()> Public Sub AvecPlusieursPermissionsSeuleLaPremiereCompte()
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim idInternaute = CreerComptePortail(patientA)
        AutoriserPatientPortail(idInternaute, patientB)

        ' Comportement actuel : la lecture des permissions n'a pas d'ORDER BY et
        ' l'écran ne montre que la première, la plus ancienne dans l'ordre de la clé.
        ' Le second dossier est inaccessible depuis le portail.
        Assert.AreEqual(CInt(patientA), Sonde(PrincipalInternautePortail(idInternaute)).PatientConnecte().PatientId)
    End Sub

    ' --- AccesRefuse ---------------------------------------------------------------------

    <TestMethod()> Public Sub LeRefusEstUn403()
        VerifierAccesRefusePortail(Sonde(PrincipalAnonymePortail()).Refus())
    End Sub

End Class
