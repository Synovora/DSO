<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Ordonnance</title>
</head>
<body class="bg-light">
    <div class="container">
        <div class="py-5 text-center">
            @*<img class="d-block mx-auto mb-4" src="https://getbootstrap.com/docs/4.0/assets/brand/bootstrap-solid.svg" alt="" width="72" height="72">*@
            <h2>Verification d'Ordonnance</h2>
            <p class="lead">Verifier rapidement et simplement l'authenticite d'une ordonnance</p>
        </div>
        <div class="row">
            <div class="col-md-6 order-md-2 mb-6">
                <h4 class="d-flex justify-content-between align-items-center mb-3">
                    <span class="text-muted">Details</span>
                    '<span class="badge badge-secondary badge-pill">@ViewBag.OrdonnanceDetail.Count</span>
                </h4>
                <ul class="list-group mb-3">
                    @For Each traitement In ViewBag.Traitements

                    @<li class="list-group-item d-flex justify-content-between lh-condensed">
                        <div>
                        <h6 class="my-0">@traitement.DenominationLongue</h6>
                        <small class="text-muted">Posologie: @traitement.PosologieMatin / @traitement.PosologieMidi / @traitement.PosologieApresMidi / @traitement.PosologieSoir </small>
                        </div>
                        <span class="text-muted">@traitement.PosologieRythme @traitement.PosologieBase</span>
                    </li>
                    Next

                    @*<li class="list-group-item d-flex justify-content-between bg-light">
                        <div class="text-success">
                            <h6 class="my-0">Traitement 4</h6>
                            <small>test</small>
                        </div>
                        <span class="text-success">5</span>
                    </li>*@
                    @*<li class="list-group-item d-flex justify-content-between">
                        <span>Total</span>
                        <strong>20</strong>
                    </li>*@
                </ul>

                @*<form class="card p-2">
                    <div class="input-group">
                        <input type="text" class="form-control" placeholder="Promo code">
                        <div class="input-group-append">
                            <button type="submit" class="btn btn-secondary">Redeem</button>
                        </div>
                    </div>
                </form>*@
            </div>
            <div class="col-md-6 order-md-1">
                <form class="needs-validation" novalidate>
                    <div class="row">
                        <h4 class="mb-3">Patient</h4>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="firstName">Nom</label>
                                <input type="text" class="form-control" id="firstName" placeholder="" value=@ViewBag.Patient.PatientNom required disabled>
                                <div class="invalid-feedback">
                                    Valid first name is required.
                                </div>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="lastName">Prenom</label>
                                <input type="text" class="form-control" id="lastName" placeholder="" value=@ViewBag.Patient.PatientPrenom required disabled>
                                <div class="invalid-feedback">
                                    Valid last name is required.
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="birthDate">Date de naissance</label>
                                <input type="text" class="form-control" id="birthDate" placeholder="" value=@ViewBag.Patient.PatientDateNaissance required disabled>
                                <div class="invalid-feedback">
                                    Valid birthDate is required.
                                </div>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="CPAM">Immatriculation CPAM</label>
                                <input type="text" class="form-control" id="CPAM" placeholder="" value=@ViewBag.Patient.INS required disabled>
                                <div class="invalid-feedback">
                                    Valid CPAM is required.
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <h4 class="mb-3">Medecin</h4>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="firstName">Nom</label>
                                <input type="text" class="form-control" id="firstName" placeholder="" value=@ViewBag.User.UtilisateurNom required disabled>
                                <div class="invalid-feedback">
                                    Valid first name is required.
                                </div>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="lastName">Prenom</label>
                                <input type="text" class="form-control" id="lastName" placeholder="" value=@ViewBag.User.UtilisateurPrenom required disabled>
                                <div class="invalid-feedback">
                                    Valid last name is required.
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <h4 class="mb-3">Information</h4>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="date">Date</label>
                                <input type="text" class="form-control" id="date" placeholder="" value=@ViewBag.Ordonnance.DateValidation required disabled>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="renouvelable">Renouvelable</label>
                                <input type="text" class="form-control" id="renouvelable" placeholder="" value=@(If(ViewBag.Ordonnance.Renouvellement, ViewBag.Ordonnance.Renouvellement, "Non")) required disabled>
                            </div>
                        </div>
                    </div>
                    <hr />
                </form>
            </div>
        </div>
    </div>
</body>