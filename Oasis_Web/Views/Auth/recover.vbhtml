@ModelType Oasis_Web.Models.UserRecover
@Code
    ViewBag.Title = "Recover Password"
    Layout = "~/Views/Shared/_BlankLayout.cshtml"
End Code

<div>
    @Using (Html.BeginForm("Recover", "Auth", method:=FormMethod.Post))

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
                                        <h5 class="text-primary"> Changement de mot de passe</h5>
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
                                <a href="@Url.Action("Index","Dashboard")">
                                    <div class="avatar-md profile-user-wid mb-4">
                                        <span class="avatar-title rounded-circle bg-light">
                                            <img src="~/assets/images/logo.svg" alt="" class="rounded-circle" height="34">
                                        </span>
                                    </div>
                                </a>
                            </div>

                            <div class="p-2">
                                <div class="alert alert-success text-center mb-4" role="alert">
                                    Vous venez de vous connecter pour la premiere fois avec un lien temporaire, veuillez entrer votre nouveau mot de passe.
                                </div>

                                <form Class="form-horizontal" action="@Url.Action("Recover", "Auth")" method="post">
                                    @If ViewBag.Internaute IsNot Nothing Then
                                        @<div class="form-group">
                                            <label for="useremail">Email</label>
                                            <input type="email" class="form-control" id="email" placeholder=@ViewBag.Internaute.Email disabled>
                                        </div>
                                    End If
                                    @If ViewBag.Recovery IsNot Nothing Then
                                        @<div Class="form-group">
                                            <label for="userpassword">Recovery</label>
                                            <input type="text" name="Recovery" value="@ViewBag.Recovery" />
                                        </div>
                                    End If
                                    <div Class="form-group">
                                        <label for="usercode">Code</label>
                                        @Html.PasswordFor(Function(u) u.Code, New With {Key .Class = "form-control", .placeholder = "Entrez le code SMS"})
                                        @Html.ValidationMessageFor(Function(u) u.Code)
                                    </div>
                                    <div Class="form-group">
                                        <label for="userpassword">Mot de passe</label>
                                        @Html.PasswordFor(Function(u) u.Password, New With {Key .Class = "form-control", .placeholder = "Entrez votre mot de passe"})
                                        @Html.ValidationMessageFor(Function(u) u.Password)
                                    </div>

                                    <div Class="form-group">
                                        <label for="userpassword">Confirmation mot de passe</label>
                                        @Html.PasswordFor(Function(u) u.PasswordBis, New With {Key .Class = "form-control", .placeholder = "Confirmez votre mot de passe"})
                                        @Html.ValidationMessageFor(Function(u) u.PasswordBis)
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