Imports System.Web.Http
Imports System.Web.Optimization

Public Class WebApiApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        ' Aucune zone MVC n'est enregistrée : la documentation d'API (Areas/HelpPage)
        ' a été retirée. Elle décrivait les routes et les paramètres attendus par
        ' Login, DocFileUpload, Rename et SendMail, et n'était fermée que par
        ' l'absence de fournisseur de rôles, ce qui aurait suffi à la rouvrir en
        ' silence le jour où l'un serait configuré.
        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        ' inhibe le serializer xml (on sera toujours en json)
        GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear()
    End Sub
End Class
