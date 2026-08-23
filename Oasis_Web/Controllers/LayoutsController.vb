Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc

Namespace Oasis_Web.Controllers
    Public Class LayoutsController
        Inherits Controller

        <ActionName("layout-vertical")>
        Public Function layoutvertical() As ActionResult
            TempData("ModeName") = Constants.LAYOUT_VERTICAL
            TempData("WelcomeText") = "LAYOUT_VERTICAL"
            Return RedirectToAction("Index", "Dashboard")
        End Function

        <ActionName("layout-horizontal")>
        Public Function layouthorizontal() As ActionResult
            TempData("ModeName") = Constants.LAYOUT_HORIZONTAL
            TempData("WelcomeText") = "LAYOUT_HORIZONTAL"
            Return RedirectToAction("Index", "Dashboard")
        End Function

        <ActionName("layout-light-sidebar")>
        Public Function layoutlightsidebar() As ActionResult
            TempData("ModeName") = Constants.LAYOUT_LIGHT_SIDEBAR
            TempData("WelcomeText") = "LAYOUT_LIGHT_SIDEBAR"
            Return RedirectToAction("Index", "Dashboard")
        End Function

        <ActionName("layout-compact-sidebar")>
        Public Function layoutcompactsidebar() As ActionResult
            TempData("ModeName") = Constants.LAYOUT_COMPACT_SIDEBAR
            TempData("WelcomeText") = "LAYOUT_COMPACT_SIDEBAR"
            Return RedirectToAction("Index", "Dashboard")
        End Function

        <ActionName("layout-icon-sidebar")>
        Public Function layouticonsidebar() As ActionResult
            TempData("ModeName") = Constants.LAYOUT_ICON_SIDEBAR
            TempData("WelcomeText") = "LAYOUT_ICON_SIDEBAR"
            Return RedirectToAction("Index", "Dashboard")
        End Function

        <ActionName("layout-boxed")>
        Public Function layoutboxed() As ActionResult
            TempData("ModeName") = Constants.LAYOUT_BOXED
            TempData("WelcomeText") = "LAYOUTS_BOXED"
            Return RedirectToAction("Index", "Dashboard")
        End Function

        <ActionName("layout-preloader")>
        Public Function layoutpreloader() As ActionResult
            TempData("ModeName") = Constants.LAYOUT_PRELOADER
            TempData("WelcomeText") = "LAYOUTS_PRELOADER"
            Return RedirectToAction("Index", "Dashboard")
        End Function

        <ActionName("layout-colored-sidebar")>
        Public Function layoutcoloredsidebar() As ActionResult
            TempData("ModeName") = Constants.LAYOUT_COLORED_SIDEBAR
            TempData("WelcomeText") = "LAYOUTS_COLORED_SIDEBAR"
            Return RedirectToAction("Index", "Dashboard")
        End Function
    End Class
End Namespace
