Imports System.Web.Http
Imports Oasis_Web.Filters

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        ' Configuration et services API Web

        ' Authentification et autorisation de toute l'API, en un seul endroit.
        '
        ' FilterConfig pose un AuthorizeAttribute global, mais celui-ci appartient à
        ' System.Web.Mvc et ne s'applique qu'aux contrôleurs MVC : les
        ' ApiController n'étaient couverts par rien. Chaque route se défendait
        ' seule, et une route qui oubliait de le faire était ouverte en silence.
        '
        ' Ici le refus est le comportement par défaut. Une route accessible sans
        ' authentification doit porter <AllowAnonymous> explicitement, ce qui rend
        ' l'exception visible en relecture.
        config.Filters.Add(New AuthentificationApiAttribute())
        config.Filters.Add(New AuthorizeAttribute())

        ' Itinéraires de l'API Web
        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )
    End Sub
End Module
