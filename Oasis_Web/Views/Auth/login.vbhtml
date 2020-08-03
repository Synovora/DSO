﻿@ModelType Oasis_Web.Models.UserLogin
@Code
    ViewBag.Title = "Login"
    Layout = "~/Views/Shared/_BlankLayout.cshtml"
End Code

<div class="home-btn d-none d-sm-block">
    <a href="@Url.Action("Index", "Dashboard")" class="text-dark"><i class="fas fa-home h2"></i></a>
</div>
<div>
    @Using (Html.BeginForm())

        @Html.AntiForgeryToken()
        @<div Class="account-pages my-5 pt-sm-5">
            <div Class="container">
                <div Class="row justify-content-center">
                    <div Class="col-md-8 col-lg-6 col-xl-5">
                        <div Class="card overflow-hidden">
                            <div Class="bg-soft-primary">
                                <div Class="row">
                                    <div Class="col-7">
                                        <div Class="text-primary p-4">
                                            <h5 Class="text-primary">Bienvenue !</h5>
                                            <p> Connectez-vous pour continuer vers Synovora.</p>
                                        </div>
                                    </div>
                                    <div Class="col-5 align-self-end">
                                        <img src="assets/images/profile-img.png" alt="" Class="img-fluid">
                                    </div>
                                </div>
                            </div>
                            <div Class="card-body pt-0">
                                <div>
                                    <a href="@Url.Action("Index", "Dashboard")">
                                        <div Class="avatar-md profile-user-wid mb-4">
                                            <span Class="avatar-title rounded-circle bg-light">
                                                <img src="assets/images/logo.svg" alt="" Class="rounded-circle" height="34">
                                            </span>
                                        </div>
                                    </a>
                                </div>
                                <div Class="p-2">
                                    <form Class="form-horizontal" action="@Url.Action("Index")" method="post">

                                        <div Class="form-group">
                                            <label for="username">Nom d'utilisateur</label>
                                            @*<input type="text" Class="form-control" id="username" placeholder="Entrez votre nom d'utilisateur">*@
                                            @Html.TextBoxFor(Function(u) u.Username, New With {Key .Class = "form-control", .placeholder = "Entrez votre nom d'utilisateur"})
                                        </div>

                                        <div Class="form-group">
                                            <label for="userpassword">Mot de passe</label>
                                            @*<input type="password" Class="form-control" id="userpassword" placeholder="Entrez votre mot de passe">*@
                                            @Html.PasswordFor(Function(u) u.Password, New With {Key .Class = "form-control", .placeholder = "Entrez votre mot de passe"})
                                            @Html.ValidationMessageFor(Function(u) u.Password)
                                        </div>

                                        <div Class="custom-control custom-checkbox">
                                            <input type="checkbox" Class="custom-control-input" id="customControlInline">
                                            <label Class="custom-control-label" for="customControlInline">Enregistrer la connexion</label>
                                        </div>
                                            <label>@ViewBag.Message</label>

                        <div Class="mt-3">
                            <button Class="btn btn-primary btn-block waves-effect waves-light" type="submit" id="btnLogin">Se connecter</button>
                        </div>

                        <div Class="mt-4 text-center">
                            <a href="@Url.Action("auth-recoverpw", "Auth")" Class="text-muted"><i Class="mdi mdi-lock mr-1"></i> Mot de passe oublie?</a>
                        </div>
                    </form>
                </div>

            </div>
        </div>
        <div Class="mt-5 text-center">
            @*<p>Don't have an account ? <a href="@Url.Action("auth-register","Auth")" class="font-weight-medium text-primary"> Signup now </a> </p>*@
                                            <p>© 2020 Synovora.</p>
                                        </div>

                    </div>
                </div>
            </div>
        </div>
    End Using
</div>
