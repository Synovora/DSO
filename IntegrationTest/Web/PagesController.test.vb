Imports System.Reflection
Imports System.Web.Mvc
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' PagesController : gabarits statiques du thème du portail, sans donnée. Chaque
''' action rend sa vue par défaut ; aucune n'est AllowAnonymous.
''' </summary>
<TestClass()> Public Class PagesControllerTest
    Inherits TestIntegration

    Private Shared ReadOnly NomsMethodes As String() = {
        "pageslogin2", "pagesregister", "pagesregister2", "pagesrecoverpw", "pagesrecoverpw2",
        "pageslockscreen", "pageslockscreen2", "pagesstarter", "pagesmaintenance", "pagescomingsoon",
        "pagestimeline", "pagesfaqs", "pagespricing", "pages404", "pages500"}

    <TestMethod()> Public Sub LaListeDesActionsEstComplete()
        Dim methodes = GetType(PagesController).GetMethods(BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.DeclaredOnly)
        Dim publiques = methodes.Where(Function(m) GetType(ActionResult).IsAssignableFrom(m.ReturnType)).Select(Function(m) m.Name).ToArray()
        CollectionAssert.AreEquivalent(NomsMethodes, publiques)
    End Sub

    <TestMethod()> Public Sub ChaqueActionRendSaVueParDefaut()
        Dim controleur = ControleurPortail(Of PagesController)(1)
        For Each nom In NomsMethodes
            Dim resultat = DirectCast(GetType(PagesController).GetMethod(nom).Invoke(controleur, Nothing), ActionResult)
            Assert.AreEqual("", VuePortail(resultat).ViewName, nom)
        Next
    End Sub

    <TestMethod()> Public Sub ChaqueActionExigeUneAuthentification()
        ' Comportement actuel : même les gabarits de connexion et d'inscription
        ' (pages-login-2, pages-register...) tombent sous l'Authorize global.
        Dim anonyme = ControleurPortailAnonyme(Of PagesController)()
        Dim connecte = ControleurPortail(Of PagesController)(1)
        For Each nom In NomsMethodes
            VerifierNonAuthentifiePortail(AutorisationPortail(anonyme, nom))
            Assert.IsNull(AutorisationPortail(connecte, nom), nom)
        Next
    End Sub

End Class
