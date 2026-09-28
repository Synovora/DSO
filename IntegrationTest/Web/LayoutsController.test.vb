Imports System.Web.Mvc
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' LayoutsController : choix de la mise en page du portail. Chaque action range le
''' mode et le texte d'accueil dans TempData puis renvoie au tableau de bord, qui
''' les relit (voir DashboardControllerTest).
''' </summary>
<TestClass()> Public Class LayoutsControllerTest
    Inherits TestIntegration

    ''' <summary>Méthode, ModeName attendu, WelcomeText attendu.</summary>
    Private Shared ReadOnly Attendus As String()() = {
        New String() {"layoutvertical", "LAYOUT_VERTICAL", "LAYOUT_VERTICAL"},
        New String() {"layouthorizontal", "LAYOUT_HORIZONTAL", "LAYOUT_HORIZONTAL"},
        New String() {"layoutlightsidebar", "LAYOUT_LIGHT_SIDEBAR", "LAYOUT_LIGHT_SIDEBAR"},
        New String() {"layoutcompactsidebar", "LAYOUT_COMPACT_SIDEBAR", "LAYOUT_COMPACT_SIDEBAR"},
        New String() {"layouticonsidebar", "LAYOUT_ICON_SIDEBAR", "LAYOUT_ICON_SIDEBAR"},
        New String() {"layoutboxed", "LAYOUTS_BOXED", "LAYOUTS_BOXED"},
        New String() {"layoutpreloader", "LAYOUTS_PRELOADER", "LAYOUTS_PRELOADER"},
        New String() {"layoutcoloredsidebar", "LAYOUTS_COLORED_SIDEBAR", "LAYOUTS_COLORED_SIDEBAR"}}

    <TestMethod()> Public Sub ChaqueMiseEnPageEstRangeeDansTempDataPuisRenvoieAuTableauDeBord()
        For Each attendu In Attendus
            Dim controleur = ControleurPortail(Of LayoutsController)(1)
            Dim resultat = DirectCast(GetType(LayoutsController).GetMethod(attendu(0)).Invoke(controleur, Nothing), ActionResult)

            VerifierRedirectionPortail(resultat, "Dashboard", "Index")
            Assert.AreEqual(attendu(1), CStr(controleur.TempData("ModeName")), attendu(0))
            Assert.AreEqual(attendu(2), CStr(controleur.TempData("WelcomeText")), attendu(0))
        Next
    End Sub

    <TestMethod()> Public Sub ChaqueMiseEnPageExigeUneAuthentification()
        Dim anonyme = ControleurPortailAnonyme(Of LayoutsController)()
        Dim connecte = ControleurPortail(Of LayoutsController)(1)
        For Each attendu In Attendus
            VerifierNonAuthentifiePortail(AutorisationPortail(anonyme, attendu(0)))
            Assert.IsNull(AutorisationPortail(connecte, attendu(0)), attendu(0))
        Next
    End Sub

End Class
