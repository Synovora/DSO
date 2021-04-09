﻿<header id="page-topbar">
    <div class="navbar-header">
        <div class="d-flex">
            <!-- LOGO -->
            <div class="navbar-brand-box">
                <a href="@Url.Action("Index","Dashboard")" class="logo logo-dark">
                    <span class="logo-sm">
                        <img src="~/assets/images/logo.svg" alt="" height="22">
                    </span>
                    <span class="logo-lg">
                        <img src="~/assets/images/logo-dark.png" alt="" height="17">
                    </span>
                </a>

                <a href="@Url.Action("Index","Dashboard")" class="logo logo-light">
                    <span class="logo-sm">
                        <img src="~/assets/images/logo-light.svg" alt="" height="22">
                    </span>
                    <span class="logo-lg">
                        <img src="~/assets/images/logo-light.png" alt="" height="19">
                    </span>
                </a>
            </div>

            <button type="button" class="btn btn-sm px-3 font-size-16 d-lg-none header-item waves-effect waves-light" data-toggle="collapse" data-target="#topnav-menu-content">
                <i class="fa fa-fw fa-bars"></i>
            </button>

            <!-- App Search-->
            <form class="app-search d-none d-lg-block">
                <div class="position-relative">
                    <input type="text" class="form-control" placeholder="Search...">
                    <span class="bx bx-search-alt"></span>
                </div>
            </form>

            <div class="dropdown dropdown-mega d-none d-lg-block ml-2">
                <button type="button" class="btn header-item waves-effect" data-toggle="dropdown" aria-haspopup="false" aria-expanded="false">
                    Mega Menu
                    <i class="mdi mdi-chevron-down"></i>
                </button>
                <div class="dropdown-menu dropdown-megamenu">
                    <div class="row">
                        <div class="col-sm-8">

                            <div class="row">
                                <div class="col-md-4">
                                    <h5 class="font-size-14 mt-0">UI Components</h5>
                                    <ul class="list-unstyled megamenu-list">
                                        <li>
                                            <a href="javascript:void(0);">Lightbox</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Range Slider</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Sweet Alert</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Rating</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Forms</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Tables</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Charts</a>
                                        </li>
                                    </ul>
                                </div>

                                <div class="col-md-4">
                                    <h5 class="font-size-14 mt-0">Applications</h5>
                                    <ul class="list-unstyled megamenu-list">
                                        <li>
                                            <a href="javascript:void(0);">Ecommerce</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Calendar</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Email</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Projects</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Tasks</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Contacts</a>
                                        </li>
                                    </ul>
                                </div>

                                <div class="col-md-4">
                                    <h5 class="font-size-14 mt-0">Extra Pages</h5>
                                    <ul class="list-unstyled megamenu-list">
                                        <li>
                                            <a href="javascript:void(0);">Light Sidebar</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Compact Sidebar</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Horizontal layout</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Maintenance</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Coming Soon</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Timeline</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">FAQs</a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="row">
                                <div class="col-sm-6">
                                    <h5 class="font-size-14 mt-0">UI Components</h5>
                                    <ul class="list-unstyled megamenu-list">
                                        <li>
                                            <a href="javascript:void(0);">Lightbox</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Range Slider</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Sweet Alert</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Rating</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Forms</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Tables</a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">Charts</a>
                                        </li>
                                    </ul>
                                </div>

                                <div class="col-sm-5">
                                    <div>
                                        <img src="~/assets/images/megamenu-img.png" alt="" class="img-fluid mx-auto d-block">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div class="d-flex">

            <div class="dropdown d-inline-block d-lg-none ml-2">
                <button type="button" class="btn header-item noti-icon waves-effect" id="page-header-search-dropdown"
                        data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="mdi mdi-magnify"></i>
                </button>
                <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right p-0"
                     aria-labelledby="page-header-search-dropdown">

                    <form class="p-3">
                        <div class="form-group m-0">
                            <div class="input-group">
                                <input type="text" class="form-control" placeholder="Search ..." aria-label="Recipient's username">
                                <div class="input-group-append">
                                    <button class="btn btn-primary" type="submit"><i class="mdi mdi-magnify"></i></button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>

            <div class="dropdown d-inline-block">
                <button type="button" class="btn header-item waves-effect"
                        data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <img class="" src="~/assets/images/flags/us.jpg" alt="Header Language" height="16">
                </button>
                <div class="dropdown-menu dropdown-menu-right">

                    <!-- item-->
                    <a href="javascript:void(0);" class="dropdown-item notify-item">
                        <img src="~/assets/images/flags/spain.jpg" alt="user-image" class="mr-1" height="12"> <span class="align-middle">Spanish</span>
                    </a>

                    <!-- item-->
                    <a href="javascript:void(0);" class="dropdown-item notify-item">
                        <img src="~/assets/images/flags/germany.jpg" alt="user-image" class="mr-1" height="12"> <span class="align-middle">German</span>
                    </a>

                    <!-- item-->
                    <a href="javascript:void(0);" class="dropdown-item notify-item">
                        <img src="~/assets/images/flags/italy.jpg" alt="user-image" class="mr-1" height="12"> <span class="align-middle">Italian</span>
                    </a>

                    <!-- item-->
                    <a href="javascript:void(0);" class="dropdown-item notify-item">
                        <img src="~/assets/images/flags/russia.jpg" alt="user-image" class="mr-1" height="12"> <span class="align-middle">Russian</span>
                    </a>
                </div>
            </div>

            <div class="dropdown d-none d-lg-inline-block ml-1">
                <button type="button" class="btn header-item noti-icon waves-effect"
                        data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="bx bx-customize"></i>
                </button>
                <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
                    <div class="px-lg-2">
                        <div class="row no-gutters">
                            <div class="col">
                                <a class="dropdown-icon-item" href="#">
                                    <img src="~/assets/images/brands/github.png" alt="Github">
                                    <span>GitHub</span>
                                </a>
                            </div>
                            <div class="col">
                                <a class="dropdown-icon-item" href="#">
                                    <img src="~/assets/images/brands/bitbucket.png" alt="bitbucket">
                                    <span>Bitbucket</span>
                                </a>
                            </div>
                            <div class="col">
                                <a class="dropdown-icon-item" href="#">
                                    <img src="~/assets/images/brands/dribbble.png" alt="dribbble">
                                    <span>Dribbble</span>
                                </a>
                            </div>
                        </div>

                        <div class="row no-gutters">
                            <div class="col">
                                <a class="dropdown-icon-item" href="#">
                                    <img src="~/assets/images/brands/dropbox.png" alt="dropbox">
                                    <span>Dropbox</span>
                                </a>
                            </div>
                            <div class="col">
                                <a class="dropdown-icon-item" href="#">
                                    <img src="~/assets/images/brands/mail_chimp.png" alt="mail_chimp">
                                    <span>Mail Chimp</span>
                                </a>
                            </div>
                            <div class="col">
                                <a class="dropdown-icon-item" href="#">
                                    <img src="~/assets/images/brands/slack.png" alt="slack">
                                    <span>Slack</span>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="dropdown d-none d-lg-inline-block ml-1">
                <button type="button" class="btn header-item noti-icon waves-effect" data-toggle="fullscreen">
                    <i class="bx bx-fullscreen"></i>
                </button>
            </div>

            <div class="dropdown d-inline-block">
                <button type="button" class="btn header-item noti-icon waves-effect" id="page-header-notifications-dropdown"
                        data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="bx bx-bell bx-tada"></i>
                    <span class="badge badge-danger badge-pill">3</span>
                </button>
                <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right p-0"
                     aria-labelledby="page-header-notifications-dropdown">
                    <div class="p-3">
                        <div class="row align-items-center">
                            <div class="col">
                                <h6 class="m-0"> Notifications </h6>
                            </div>
                            <div class="col-auto">
                                <a href="#!" class="small"> View All</a>
                            </div>
                        </div>
                    </div>
                    <div data-simplebar style="max-height: 230px;">
                        <a href="" class="text-reset notification-item">
                            <div class="media">
                                <div class="avatar-xs mr-3">
                                    <span class="avatar-title bg-primary rounded-circle font-size-16">
                                        <i class="bx bx-cart"></i>
                                    </span>
                                </div>
                                <div class="media-body">
                                    <h6 class="mt-0 mb-1">Your order is placed</h6>
                                    <div class="font-size-12 text-muted">
                                        <p class="mb-1">If several languages coalesce the grammar</p>
                                        <p class="mb-0"><i class="mdi mdi-clock-outline"></i> 3 min ago</p>
                                    </div>
                                </div>
                            </div>
                        </a>
                        <a href="" class="text-reset notification-item">
                            <div class="media">
                                <img src="~/assets/images/users/avatar-3.jpg"
                                     class="mr-3 rounded-circle avatar-xs" alt="user-pic">
                                <div class="media-body">
                                    <h6 class="mt-0 mb-1">James Lemire</h6>
                                    <div class="font-size-12 text-muted">
                                        <p class="mb-1">It will seem like simplified English.</p>
                                        <p class="mb-0"><i class="mdi mdi-clock-outline"></i> 1 hours ago</p>
                                    </div>
                                </div>
                            </div>
                        </a>
                        <a href="" class="text-reset notification-item">
                            <div class="media">
                                <div class="avatar-xs mr-3">
                                    <span class="avatar-title bg-success rounded-circle font-size-16">
                                        <i class="bx bx-badge-check"></i>
                                    </span>
                                </div>
                                <div class="media-body">
                                    <h6 class="mt-0 mb-1">Your item is shipped</h6>
                                    <div class="font-size-12 text-muted">
                                        <p class="mb-1">If several languages coalesce the grammar</p>
                                        <p class="mb-0"><i class="mdi mdi-clock-outline"></i> 3 min ago</p>
                                    </div>
                                </div>
                            </div>
                        </a>

                        <a href="" class="text-reset notification-item">
                            <div class="media">
                                <img src="~/assets/images/users/avatar-4.jpg"
                                     class="mr-3 rounded-circle avatar-xs" alt="user-pic">
                                <div class="media-body">
                                    <h6 class="mt-0 mb-1">Salena Layfield</h6>
                                    <div class="font-size-12 text-muted">
                                        <p class="mb-1">As a skeptical Cambridge friend of mine occidental.</p>
                                        <p class="mb-0"><i class="mdi mdi-clock-outline"></i> 1 hours ago</p>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <div class="p-2 border-top">
                        <a class="btn btn-sm btn-link font-size-14 btn-block text-center" href="javascript:void(0)">
                            <i class="mdi mdi-arrow-right-circle mr-1"></i> View More..
                        </a>
                    </div>
                </div>
            </div>

            <div class="dropdown d-inline-block">
                <button type="button" class="btn header-item waves-effect" id="page-header-user-dropdown"
                        data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <img class="rounded-circle header-profile-user" src="~/assets/images/users/avatar-1.jpg"
                         alt="Header Avatar">
                    <span class="d-none d-xl-inline-block ml-1">Henry</span>
                    <i class="mdi mdi-chevron-down d-none d-xl-inline-block"></i>
                </button>
                <div class="dropdown-menu dropdown-menu-right">
                    <!-- item-->
                    <a class="dropdown-item" href="#"><i class="bx bx-user font-size-16 align-middle mr-1"></i> Profile</a>
                    <a class="dropdown-item" href="#"><i class="bx bx-wallet font-size-16 align-middle mr-1"></i> My Wallet</a>
                    <a class="dropdown-item d-block" href="#"><span class="badge badge-success float-right">11</span><i class="bx bx-wrench font-size-16 align-middle mr-1"></i> Settings</a>
                    <a class="dropdown-item" href="#"><i class="bx bx-lock-open font-size-16 align-middle mr-1"></i> Lock screen</a>
                    <div class="dropdown-divider"></div>
                    <a class="dropdown-item text-danger" href="#"><i class="bx bx-power-off font-size-16 align-middle mr-1 text-danger"></i> Logout</a>
                </div>
            </div>

            <div class="dropdown d-inline-block">
                <button type="button" class="btn header-item noti-icon right-bar-toggle waves-effect">
                    <i class="bx bx-cog bx-spin"></i>
                </button>
            </div>

        </div>
    </div>
</header>

<div class="topnav">
    <div class="container-fluid">
        <nav class="navbar navbar-light navbar-expand-lg topnav-menu">

            <div class="collapse navbar-collapse" id="topnav-menu-content">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" href="@Url.Action("Index","Dashboard")">
                            <i class="bx bx-home-circle mr-2"></i>Dashboard
                        </a>
                    </li>

                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle arrow-none" href="#" id="topnav-components" role="button"
                           data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="bx bx-tone mr-2"></i>UI Elements <div class="arrow-down"></div>
                        </a>

                        <div class="dropdown-menu mega-dropdown-menu px-2 dropdown-mega-menu-xl"
                             aria-labelledby="topnav-components">
                            <div class="row">
                                <div class="col-lg-4">
                                    <div>
                                        <a href="@Url.Action("ui-alerts","UI")" class="dropdown-item">Alerts</a>
                                        <a href="@Url.Action("ui-buttons","UI")" class="dropdown-item">Buttons</a>
                                        <a href="@Url.Action("ui-cards","UI")" class="dropdown-item">Cards</a>
                                        <a href="@Url.Action("ui-carousel","UI")" class="dropdown-item">Carousel</a>
                                        <a href="@Url.Action("ui-dropdowns","UI")" class="dropdown-item">Dropdowns</a>
                                        <a href="@Url.Action("ui-grid","UI")" class="dropdown-item">Grid</a>
                                        <a href="@Url.Action("ui-images","UI")" class="dropdown-item">Images</a>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div>
                                        <a href="@Url.Action("ui-lightbox","UI")" class="dropdown-item">Lightbox</a>
                                        <a href="@Url.Action("ui-modals","UI")" class="dropdown-item">Modals</a>
                                        <a href="@Url.Action("ui-rangeslider","UI")" class="dropdown-item">Range Slider</a>
                                        <a href="@Url.Action("ui-session-timeout","UI")" class="dropdown-item">Session Timeout</a>
                                        <a href="@Url.Action("ui-progressbars","UI")" class="dropdown-item">Progress Bars</a>
                                        <a href="@Url.Action("ui-sweet-alert","UI")" class="dropdown-item">Sweet-Alert</a>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div>
                                        <a href="@Url.Action("ui-tabs-accordions","UI")" class="dropdown-item">Tabs & Accordions</a>
                                        <a href="@Url.Action("ui-typography","UI")" class="dropdown-item">Typography</a>
                                        <a href="@Url.Action("ui-video","UI")" class="dropdown-item">Video</a>
                                        <a href="@Url.Action("ui-general","UI")" class="dropdown-item">General</a>
                                        <a href="@Url.Action("ui-colors","UI")" class="dropdown-item">Colors</a>
                                        <a href="@Url.Action("ui-rating","UI")" class="dropdown-item">Rating</a>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </li>

                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle arrow-none" href="#" id="topnav-pages" role="button"
                           data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="bx bx-customize mr-2"></i>Apps <div class="arrow-down"></div>
                        </a>
                        <div class="dropdown-menu" aria-labelledby="topnav-pages">

                            <a href="@Url.Action("Index","Calendar")" class="dropdown-item">Calendar</a>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-email"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Email <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-email">
                                    <a href="@Url.Action("email-inbox","Email")" class="dropdown-item">Inbox</a>
                                    <a href="@Url.Action("email-read","Email")" class="dropdown-item">Read Email</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-ecommerce"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Ecommerce <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-ecommerce">
                                    <a href="@Url.Action("ecommerce-products","Ecommerce")" class="dropdown-item">Products</a>
                                    <a href="@Url.Action("ecommerce-productdetail","Ecommerce")" class="dropdown-item">Product Detail</a>
                                    <a href="@Url.Action("ecommerce-orders","Ecommerce")" class="dropdown-item">Orders</a>
                                    <a href="@Url.Action("ecommerce-customers","Ecommerce")" class="dropdown-item">Customers</a>
                                    <a href="@Url.Action("ecommerce-cart","Ecommerce")" class="dropdown-item">Cart</a>
                                    <a href="@Url.Action("ecommerce-checkout","Ecommerce")" class="dropdown-item">Checkout</a>
                                    <a href="@Url.Action("ecommerce-shops","Ecommerce")" class="dropdown-item">Shops</a>
                                    <a href="@Url.Action("ecommerce-addproduct","Ecommerce")" class="dropdown-item">Add Product</a>
                                </div>
                            </div>

                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-project"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Projects <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-project">
                                    <a href="@Url.Action("projects-grid","Projects")" class="dropdown-item">Projects Grid</a>
                                    <a href="@Url.Action("projects-list","Projects")" class="dropdown-item">Projects List</a>
                                    <a href="@Url.Action("projects-overview","Projects")" class="dropdown-item">Project Overview</a>
                                    <a href="@Url.Action("projects-create","Projects")" class="dropdown-item">Create New</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-task"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Tasks <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-task">
                                    <a href="@Url.Action("tasks-list","Tasks")" class="dropdown-item">Task List</a>
                                    <a href="@Url.Action("tasks-kanban","Tasks")" class="dropdown-item">Kanban Board</a>
                                    <a href="@Url.Action("tasks-create","Tasks")" class="dropdown-item">Create Task</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-contact"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Contacts <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-contact">
                                    <a href="@Url.Action("contacts-grid","Contacts")" class="dropdown-item">User Grid</a>
                                    <a href="@Url.Action("contacts-list","Contacts")" class="dropdown-item">User List</a>
                                    <a href="@Url.Action("contacts-profile","Contacts")" class="dropdown-item">Profile</a>
                                </div>
                            </div>
                        </div>
                    </li>

                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle arrow-none" href="#" id="topnav-charts" role="button"
                           data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="bx bx-collection mr-2"></i>Components <div class="arrow-down"></div>
                        </a>
                        <div class="dropdown-menu" aria-labelledby="topnav-charts">
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-form"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Forms <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-form">
                                    <a href="@Url.Action("form-elements","Forms")" class="dropdown-item">Form Elements</a>
                                    <a href="@Url.Action("form-validation","Forms")" class="dropdown-item">Form Validation</a>
                                    <a href="@Url.Action("form-advanced","Forms")" class="dropdown-item">Form Advanced</a>
                                    <a href="@Url.Action("form-editors","Forms")" class="dropdown-item">Form Editors</a>
                                    <a href="@Url.Action("form-uploads","Forms")" class="dropdown-item">Form File Upload</a>
                                    <a href="@Url.Action("form-xeditable","Forms")" class="dropdown-item">Form Xeditable</a>
                                    <a href="@Url.Action("form-repeater","Forms")" class="dropdown-item">Form Repeater</a>
                                    <a href="@Url.Action("form-wizard","Forms")" class="dropdown-item">Form Wizard</a>
                                    <a href="@Url.Action("form-mask","Forms")" class="dropdown-item">Form Mask</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-table"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Tables <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-table">
                                    <a href="@Url.Action("tables-basic","Tables")" class="dropdown-item">Basic Tables</a>
                                    <a href="@Url.Action("tables-datatable","Tables")" class="dropdown-item">Data Tables</a>
                                    <a href="@Url.Action("tables-responsive","Tables")" class="dropdown-item">Responsive Table</a>
                                    <a href="@Url.Action("tables-editable","Tables")" class="dropdown-item">Editable Table</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-table"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Charts <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-table">
                                    <a href="@Url.Action("charts-apex","Charts")" class="dropdown-item">Apex charts</a>
                                    <a href="@Url.Action("charts-chartjs","Charts")" class="dropdown-item">Chartjs Chart</a>
                                    <a href="@Url.Action("charts-flot","Charts")" class="dropdown-item">Flot Chart</a>
                                    <a href="@Url.Action("charts-tui","Charts")" class="dropdown-item">Toast UI Chart</a>
                                    <a href="@Url.Action("charts-knob","Charts")" class="dropdown-item">Jquery Knob Chart</a>
                                    <a href="@Url.Action("charts-sparkline","Charts")" class="dropdown-item">Sparkline Chart</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-table"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Icons <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-table">
                                    <a href="@Url.Action("icons-boxicons","Icons")" class="dropdown-item">Boxicons</a>
                                    <a href="@Url.Action("icons-materialdesign","Icons")" class="dropdown-item">Material Design</a>
                                    <a href="@Url.Action("icons-dripicons","Icons")" class="dropdown-item">Dripicons</a>
                                    <a href="@Url.Action("icons-fontawesome","Icons")" class="dropdown-item">Font awesome</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-map"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Maps <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-map">
                                    <a href="@Url.Action("maps-google","Maps")" class="dropdown-item">Google Maps</a>
                                    <a href="@Url.Action("maps-vector","Maps")" class="dropdown-item">Vector Maps</a>
                                </div>
                            </div>
                        </div>
                    </li>



                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle arrow-none" href="#" id="topnav-more" role="button"
                           data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="bx bx-file mr-2"></i>Extra pages <div class="arrow-down"></div>
                        </a>
                        <div class="dropdown-menu" aria-labelledby="topnav-more">
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-invoice"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Invoices <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-invoice">
                                    <a href="@Url.Action("invoices-list","Invoices")" class="dropdown-item">Invoice List</a>
                                    <a href="@Url.Action("invoices-detail","Invoices")" class="dropdown-item">Invoice Detail</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-auth"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Authentication <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-auth">
                                    <a href="@Url.Action("auth-login","Auth")" class="dropdown-item">Login</a>
                                    <a href="@Url.Action("auth-register","Auth")" class="dropdown-item">Register</a>
                                    <a href="@Url.Action("auth-recoverpw","Auth")" class="dropdown-item">Recover Password</a>
                                    <a href="@Url.Action("auth-lock-screen","Auth")" class="dropdown-item">Lock Screen</a>
                                </div>
                            </div>
                            <div class="dropdown">
                                <a class="dropdown-item dropdown-toggle arrow-none" href="#" id="topnav-utility"
                                   role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Utility <div class="arrow-down"></div>
                                </a>
                                <div class="dropdown-menu" aria-labelledby="topnav-utility">
                                    <a href="@Url.Action("pages-starter","Pages")" class="dropdown-item">Starter Page</a>
                                    <a href="@Url.Action("pages-maintenance","Pages")" class="dropdown-item">Maintenance</a>
                                    <a href="@Url.Action("pages-comingsoon","Pages")" class="dropdown-item">Coming Soon</a>
                                    <a href="@Url.Action("pages-timeline","Pages")" class="dropdown-item">Timeline</a>
                                    <a href="@Url.Action("pages-faqs","Pages")" class="dropdown-item">FAQs</a>
                                    <a href="@Url.Action("pages-pricing","Pages")" class="dropdown-item">Pricing</a>
                                    <a href="@Url.Action("pages-404","Pages")" class="dropdown-item">Error 404</a>
                                    <a href="@Url.Action("pages-500","Pages")" class="dropdown-item">Error 500</a>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle arrow-none" href="#" id="topnav-layout" role="button"
                           data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="bx bx-layout mr-2"></i>Layouts <div class="arrow-down"></div>
                        </a>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="topnav-layout">
                            <a href="@Url.Action("layout-horizontal","Layouts")" class="dropdown-item">Horizontal</a>
                            <a href="@Url.Action("layout-light-sidebar","Layouts")" class="dropdown-item">Light Sidebar</a>
                            <a href="@Url.Action("layout-compact-sidebar","Layouts")" class="dropdown-item">Compact Sidebar</a>
                            <a href="@Url.Action("layout-icon-sidebar","Layouts")" class="dropdown-item">Icon Sidebar</a>
                            <a href="@Url.Action("layout-boxed","Layouts")" class="dropdown-item">Boxed Layout</a>
                            <a href="@Url.Action("layout-preloader","Layouts")" class="dropdown-item">Preloader</a>
                            <a href="@Url.Action("layout-colored-sidebar","Layouts")" class="dropdown-item">Colored Sidebar</a>
                        </div>
                    </li>

                </ul>
            </div>
        </nav>
    </div>
</div>