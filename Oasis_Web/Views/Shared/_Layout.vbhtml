<!DOCTYPE html>
<html lang="en">

<head>

    <script src="https://code.jquery.com/jquery-3.5.1.min.js"
            integrity="sha256-9/aliU8dGd2tb6OSsuzixeV4y/faTqgFtohetphbbj0="
            crossorigin="anonymous"></script>

    @Html.Partial("~/Views/Shared/_title_meta.cshtml")
    @RenderSection("styles", False)
    @Html.Partial("~/Views/Shared/_head_css.cshtml")

</head>

<body data-sidebar="dark">

    <!-- Begin page -->
    <div id="layout-wrapper">
        @Html.Partial("~/Views/Shared/_topbar.vbhtml")
        @Html.Partial("~/Views/Shared/_sidebar.vbhtml")

        <!-- ============================================================== -->
        <!-- Start right Content here -->
        <!-- ============================================================== -->

        <div class="main-content">
            <div class="page-content">
                <div class="container-fluid">
                    @Html.Partial("~/Views/Shared/_page_title.vbhtml")

                    @RenderBody()
                </div> <!-- container-fluid -->
            </div>
            <!-- End Page-content -->

            @Html.Partial("~/Views/Shared/_footer.vbhtml")
        </div>
        <!-- end main content-->

    </div>

    @RenderSection("externalhtml", False)

    <!-- END layout-wrapper -->
    @Html.Partial("~/Views/Shared/_right_sidebar.vbhtml")

    @Html.Partial("~/Views/Shared/_vendor_scripts.vbhtml")

    @RenderSection("scripts", False)


</body>

</html>
