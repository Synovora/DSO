<!DOCTYPE html>
<html lang="en">

<head>

    @Html.Partial("~/Views/Shared/_title_meta.cshtml")
    @RenderSection("styles", false)
    @Html.Partial("~/Views/Shared/_head_css.cshtml")
    <link href="~/assets/css/app.css" id="app-style" rel="stylesheet" type="text/css" />

</head>

<body data-sidebar="dark">
    <!-- Begin page -->
    <div id="layout-wrapper">
        @Html.Partial("~/Views/Shared/_topbar.vbhtml")
        @Html.Partial("~/Views/Shared/_sidebar.vbhtml")

        <!-- ============================================================== -->
        <!-- Start right Content here -->
        <!-- ============================================================== -->
        @Scripts.Render("~/bundles/jquery")
        <div class="main-content">
            <div class="page-content">
                <div class="container-fluid">
                    @Html.Partial("~/Views/Shared/_page_title.vbhtml")

                    @RenderBody()

                </div> <!-- container-fluid -->
            </div>
            <!-- End Page-content -->

            @Html.Partial("~/Views/Shared/_topbar.vbhtml")
        </div>
        <!-- end main content-->

    </div>

    @Html.Partial("~/Views/Shared/_sidebar.vbhtml")


    @*#End ExternalSource

        #ExternalSource ("\\Mac\Home\Documents\synovora\oasis\Oasis_Web\Views\Shared\_Layout.vbhtml", 7)
        __o = Html.Partial("~/Views/Shared/_page_title.vbhtml")


        #End ExternalSource

        #ExternalSource ("\\Mac\Home\Documents\synovora\oasis\Oasis_Web\Views\Shared\_Layout.vbhtml", 8)
        __o = RenderBody()


        #End ExternalSource

        #ExternalSource ("\\Mac\Home\Documents\synovora\oasis\Oasis_Web\Views\Shared\_Layout.vbhtml", 9)
        __o = Html.Partial("~/Views/Shared/_footer.vbhtml")


        #End ExternalSource

        #ExternalSource ("\\Mac\Home\Documents\synovora\oasis\Oasis_Web\Views\Shared\_Layout.vbhtml", 10)
        __o = RenderSection("externalhtml", required:   false)*@

    <!-- END layout-wrapper -->
    @Html.Partial("~/Views/Shared/_right_sidebar.vbhtml")

    @Html.Partial("~/Views/Shared/_vendor_scripts.vbhtml")

    @RenderSection("scripts", False)
    <strong>@Html.Encode(User.Identity.Name)</strong>
    @Html.ActionLink("Sign Out", "Logout", "User")

</body>


</html>
