@ModelType Oasis_Web.Models.UserForgot
@Code
    ViewBag.Title = "Forgot Password"
    Layout = "~/Views/Shared/_BlankLayout.cshtml"
End Code

<div>
    @Using (Html.BeginForm("Forgot", "Auth", method:=FormMethod.Post))

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
                                        <h5 class="text-primary">Mot de passe oublie</h5>
                                        <p>Synovora.</p>
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
                                <div class="alert alert-success text-center mb-4" role="alert">
                                    Vous avez oublie votre mot de passe ? Entrez votre adresse email.
                                </div>

                                <form Class="form-horizontal" action="@Url.Action("Forgot", "Auth")" method="post">
                                    <div Class="form-group">
                                        <label for="username">Adresse mail</label>
                                        @Html.TextBoxFor(Function(u) u.Email, New With {Key .Class = "form-control", .placeholder = "Entrez votre adresse mail"})
                                    </div>

                                    <div class="form-group row mb-0">
                                        <div class="col-12 text-right">
                                            <button class="btn btn-primary w-md waves-effect waves-light" type="submit">Valider</button>
                                        </div>
                                    </div>

                                </form>
                            </div>

                        </div>
                    </div>
                    <div class="mt-5 text-center">
                        <p>© 2020 Synovora.</p>
                    </div>

                </div>
            </div>
        </div>
    </div>
    End Using
</div>