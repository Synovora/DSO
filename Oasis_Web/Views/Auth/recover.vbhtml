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
                                 @If ViewBag.Internaute IsNot Nothing AndAlso ViewBag.Recovery IsNot Nothing Then
                                        @<div class="alert alert-success text-center mb-4" role="alert">
                                            Vous venez de vous connecter pour la premiere fois avec un lien temporaire, veuillez entrer votre nouveau mot de passe.
                                        </div>

                                        @<form Class="form-horizontal" action="@Url.Action("Recover", "Auth")" method="post">
                                            <div class="form-group">
                                                <label for="useremail">Email:</label>
                                                <input type="email" class="form-control" id="email" placeholder=@ViewBag.Internaute.Email disabled>
                                            </div>
                                            <div Class="form-group">
                                                <label for="userpassword">Recovery Key:</label>
                                                <input type="text" name="Recovery" value="@ViewBag.Recovery" />
                                            </div>

                                            <div Class="form-group">
                                                <Label for="usercode">Code SMS:</Label>
                                                @Html.PasswordFor(Function(u) u.Code, New With {Key .Class = "form-control", .placeholder = "Entrez le code SMS"})
                                                @Html.ValidationMessageFor(Function(u) u.Code)
                                            </div>
                                            <div Class="form-group">
                                                <label for="userpassword">Mot de passe:</label>
                                                @Html.PasswordFor(Function(u) u.Password, New With {Key .Class = "form-control", .placeholder = "Entrez votre mot de passe"})
                                                @Html.ValidationMessageFor(Function(u) u.Password)
                                            </div>
                                            <div Class="form-group">
                                                <Label for="userpassword">Confirmation mot de passe:</Label>
                                                @Html.PasswordFor(Function(u) u.PasswordBis, New With {Key .Class = "form-control", .placeholder = "Confirmez votre mot de passe"})
                                                @Html.ValidationMessageFor(Function(u) u.PasswordBis)
                                            </div>
                                            <div Class="form-group">
                                                <Label>@ViewBag.Message</Label>
                                            </div>
                                            <div Class="form-group row mb-0">
                                                <div Class="col-12 text-right">
                                                    <Button Class="btn btn-primary w-md waves-effect waves-light" type="submit">Valider</Button>
                                                </div>
                                            </div>
                                        </form>

                                 Else
                                    @<div class="alert alert-danger text-center" role="alert">
                                         <Label>@ViewBag.Message</Label>
                                     </div>
                                 End If
                            </div>

                        </div>
                    </div>
                    <div class="mt-5 text-center">
                        <p>© 2020 Synovora.</p>
                    </div>

                </div>
            </div>
        </div>
    </div>                  End Using
</div>