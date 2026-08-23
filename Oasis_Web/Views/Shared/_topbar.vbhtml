<script type="text/javascript">
    function logout() {
        $.post("/Auth/Logout").done(function (data) {
            location.reload();
        });
    }
</script>


<header id="page-topbar">
    <div class="navbar-header">
        <div class="d-flex">
            <!-- LOGO -->
            <div class="navbar-brand-box">
                <a href=@Url.Action("Index", "Dashboard") class="">
                    <img src="http://synovora.com/wp-content/uploads/2020/05/logo-300x137.png" alt="" height="60" class="my-auto pt-2">
                </a>
            </div>

            <button type="button" class="btn btn-sm px-3 font-size-16 header-item waves-effect" id="vertical-menu-btn">
                <i class="fa fa-fw fa-flash"></i>
            </button>

            <!-- App Search-->
            @*<form class="app-search d-none d-lg-block">
                    <div class="position-relative">
                        <input type="text" class="form-control" placeholder="Rechercher...">
                        <span class="bx bx-search-alt"></span>
                    </div>
                </form>*@
        </div>

        <div class="d-flex">
            <div class="dropdown d-inline-block d-lg-none ms-2">
                <button type="button" class="btn header-item noti-icon waves-effect" id="page-header-search-dropdown"
                        data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="mdi mdi-magnify"></i>
                </button>
                <div class="dropdown-menu dropdown-menu-lg dropdown-menu-end p-0"
                     aria-labelledby="page-header-search-dropdown">

                    <form class="p-3">
                        <div class="form-group m-0">
                            <div class="input-group">
                                <input type="text" class="form-control" placeholder="Rechercher ..." aria-label="Recipient's username">
                                <div class="input-group-append">
                                    <button class="btn btn-primary" type="submit"><i class="mdi mdi-magnify"></i></button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>

            <div class="dropdown d-inline-block">
                <button class="btn header-item waves-effect" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <img class="rounded-circle header-profile-user" src="https://th.bing.com/th/id/R.dc8b1732c919ca17845aab44dc3afb27?rik=qOkrlNPk9Y4cBg&pid=ImgRaw&r=0"
                         alt="Header Avatar">
                    @*<span class="d-none d-xl-inline-block ms-1" key="t-henry">---</span>*@
                    <i class="mdi mdi-chevron-down d-none d-xl-inline-block"></i>
                </button>
                <div class="dropdown-menu dropdown-menu-end">
                    <!-- item-->
                    @*<a class="dropdown-item" href="#"><i class="bx bx-user font-size-16 align-middle me-1"></i> <span key="t-profile">Mon Profil</span></a>*@
                    <div class="dropdown-divider"></div>
                    <a class="dropdown-item text-danger" href="javascript:logout()"><i class="mdi mdi-power-plug-off font-size-16 align-middle me-1 text-danger"></i> <span key="t-logout">Se Deconnecter</span></a>
                </div>
            </div>
        </div>
    </div>
</header>