<div class="vertical-menu">
    <div data-simplebar class="h-100">
        <div id="sidebar-menu">
            <!-- Left Menu Start -->
            <ul class="metismenu list-unstyled" id="side-menu">
                <li class="menu-title" key="t-menu">Menu</li>
                <li>
                    <a href="@Url.Action("Index", "Dashboard")" class="waves-effect">
                        <i class="bx bx-home-circle"></i><span class="badge rounded-pill bg-success float-end">Nouveau</span>
                        <span key="t-dashboards">Mon Accueil</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "Synthese") class="waves-effect">
                        <i class="bx bx-file"></i>
                        <span key="t-calendar">Ma Synthese</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "RDV") class="waves-effect">
                        <i class="bx bx-book"></i>
                        <span key="t-calendar">Mes Rendez-Vous</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "AutoSuivi") class="waves-effect">
                        <i class="bx bx-file"></i>
                        <span key="t-calendar">Mon Auto-Suivi</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "CarnetVaccinal") class="waves-effect">
                        <i class="bx bx-book"></i>
                        <span key="t-calendar">Mon Carnet Vaccinal</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "Resultats") class="waves-effect">
                        <i class="bx bx-user-circle"></i>
                        <span key="t-calendar">Mes Resultats</span>
                    </a>
                </li>
            </ul>
        </div>
    </div>
</div>
