
Public Module RouteConfig
    Public Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapRoute(
                name:="Dashboard",
                url:="{controller}/{action}/{id}",
                defaults:=New With {.controller = "Dashboard", .action = "Index", .id = UrlParameter.Optional}
            )
    End Sub
End Module