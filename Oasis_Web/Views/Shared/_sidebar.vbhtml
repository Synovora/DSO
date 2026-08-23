<div class="vertical-menu">
    <div data-simplebar class="h-100">
        <div id="sidebar-menu">
            <!-- Left Menu Start -->
            <ul class="metismenu list-unstyled" id="side-menu">
                <li class="menu-title" key="t-menu">Menu</li>
                <li>
                    <a href="@Url.Action("Index", "Dashboard")" class="waves-effect">
                        <i class="mdi mdi-home"></i>
                        <span key="t-Dashboard">Mon Accueil</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "Synthese") class="waves-effect">
                        <i class="mdi mdi-file"></i>
                        <span key="t-Synthese">Ma Synthese</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "RDV") class="waves-effect">
                        <i class="mdi mdi-clock"></i>
                        <span key="t-RDV">Mes Rendez-Vous</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "AutoSuivi") class="waves-effect">
                        <i class="mdi mdi-human-capacity-increase"></i>
                        <span key="t-AutoSuivi">Mon Auto-Suivi</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "CarnetVaccinal") class="waves-effect">
                        <i class="mdi mdi-book"></i>
                        <span key="t-CarnetVaccinal">Mon Carnet Vaccinal</span>
                    </a>
                </li>
                <li>
                    <a href=@Url.Action("Index", "Resultats") class="waves-effect">
                        <i class="mdi mdi-inbox"></i><span class="badge rounded-pill bg-success float-end">Nouveau</span>
                        <span key="t-Resultats">Mes Resultats</span>
                    </a>
                </li>
            </ul>
        </div>
    </div>
</div>
