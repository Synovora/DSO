@ModelType Oasis_Web.Models.UserLogin
@Code
    ViewBag.Title = "Register"
    Layout = "~/Views/Shared/_BlankLayout.cshtml"
End Code

<div class="home-btn d-none d-sm-block">
    <a href="@Url.Action("Index","Dashboard")" class="text-dark"><i class="fas fa-home h2"></i></a>
</div>

<div>
    @Using (Html.BeginForm("Register", "Auth", method:=FormMethod.Post))
    @Html.AntiForgeryToken()
    @<div class="account-pages my-5 pt-sm-5">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-8 col-lg-6 col-xl-5">
                    <div class="card overflow-hidden">
                        <div class="bg-soft-primary">
                            <div class="row">
                                <div class="col-7">
                                    <div class="text-primary p-4">
                                        <h5 class="text-primary">Creation de compte</h5>
                                        <p>Creer un compte Oasis des maintenant.</p>
                                    </div>
                                </div>
                                <div class="col-5 align-self-end">
                                    <img src="~/assets/images/profile-img.png" alt="" class="img-fluid">
                                </div>
                            </div>
                        </div>
                        <div class="card-body pt-0">
                            <div>
                                <a href="@Url.Action("Index", "Dashboard")">
                                    <div class="avatar-md profile-user-wid mb-4">
                                        <span class="avatar-title rounded-circle bg-light">
                                            <img src="~/assets/images/logo.svg" alt="" class="rounded-circle" height="34">
                                        </span>
                                    </div>
                                </a>
                            </div>
                            <div class="p-2">
                                <form class="form-horizontal" action="@Url.Action("Register", "Auth")" method="post">

                                    <div class="form-group">
                                        <label for="useremail">Prenom</label>
                                        @Html.TextBoxFor(Function(u) u.Prenom, New With {Key .Class = "form-control", .placeholder = "Entrez votre prenom"})
                                    </div>

                                    <div class="form-group">
                                        <label for="username">Nom</label>
                                        @Html.TextBoxFor(Function(u) u.Nom, New With {Key .Class = "form-control", .placeholder = "Entrez votre nom"})
                                    </div>

                                    <div class="form-group">
                                        <label for="userpassword">NIR</label>
                                        @Html.TextBoxFor(Function(u) u.NIR, New With {Key .Class = "form-control", .placeholder = "Entrez votre NIR"})
                                    </div>

                                    <div class="mt-4">
                                        <button class="btn btn-primary btn-block waves-effect waves-light" type="submit" id="btnLogin">Creer un compte</button>
                                    </div>

                                    <p>@ViewBag.Message</p>

                                    <div class="mt-4 text-center">
                                        <p class="mb-0">En vous enregistrant vous acceptez les <a href="#" class="text-primary">conditions d'utilisation</a> de Synovora</p>
                                    </div>
                                </form>
                            </div>

                        </div>
                    </div>
                    <div class="mt-5 text-center">
                        <p>Vous avez deja un compte ? <a href="@Url.Action("auth-login", "Auth")" class="font-weight-medium text-primary"> Login</a> </p>
                        <p>© 2020 Synovora.</p>
                    </div>

                </div>
            </div>
        </div>
    </div>
    End Using
</div>