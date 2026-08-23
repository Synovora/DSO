@Code
    ViewBag.Title = "Signature Invalide"
    Layout = "~/Views/Shared/_BlankLayout.cshtml"
End Code

<div class="account-pages my-5 pt-5">
    <div class="container">
        <div class="row">
            <div class="col-lg-12">
                <div class="text-center mb-5">
                    <h1 class="display-2 font-weight-medium"><i class="bx bx-error-circle text-danger display-3"></i></h1>
                    <h4 class="text-uppercase">La signature de cette ordonnance n'est pas valide</h4>
                    <p class="mt-3">
                        Le contenu de l'ordonnance ne correspond pas à la signature du prescripteur.
                        Ce document ne doit pas être honoré. Contactez le prescripteur.
                    </p>
                </div>
            </div>
        </div>
        <div class="row justify-content-center">
            <div class="col-md-8 col-xl-6">
                <div>
                    <img src="~/assets/images/error-img.png" alt="" class="img-fluid">
                </div>
            </div>
        </div>
    </div>
</div>
