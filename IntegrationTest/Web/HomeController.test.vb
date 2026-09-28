Imports System.Web.Mvc

''' <summary>HomeController : page d'accueil du modèle de projet, sans donnée.</summary>
<TestClass()> Public Class HomeControllerTest
    Inherits TestIntegration

    <TestMethod()> Public Sub LAccueilRendSaVueAvecSonTitre()
        Dim vue = VuePortail(ControleurPortail(Of Global.Oasis_Web.HomeController)(1).Index())

        Assert.AreEqual("", vue.ViewName)
        Assert.AreEqual("Home Page", CStr(vue.ViewData("Title")))
    End Sub

    <TestMethod()> Public Sub LAccueilExigeUneAuthentification()
        ' Comportement actuel : pas d'AllowAnonymous, l'Authorize global s'applique.
        VerifierNonAuthentifiePortail(AutorisationPortail(ControleurPortailAnonyme(Of Global.Oasis_Web.HomeController)(), "Index"))
        Assert.IsNull(AutorisationPortail(ControleurPortail(Of Global.Oasis_Web.HomeController)(1), "Index"))
    End Sub

End Class
