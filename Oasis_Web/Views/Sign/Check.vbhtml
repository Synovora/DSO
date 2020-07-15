<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Ordonnance</title>
</head>
<body class="bg-light">
    <div class="container">
        <div class="py-5 text-center">
            <h2>Verification d'Ordonnance</h2>
            <p class="lead">Verifier rapidement et simplement l'authenticite d'une ordonnance</p>
        </div>
        <div class="row">
            <div class="col-md-6 order-md-2 mb-6">
                <h4 class="d-flex justify-content-between align-items-center mb-3">
                    <span class="text-muted">Details</span>
                    '<span class="badge badge-secondary badge-pill">@ViewBag.Traitements.Count</span>
                </h4>
                <ul class="list-group mb-3">
                    @For Each traitement In ViewBag.Traitements

                    @<li class="list-group-item d-flex justify-content-between lh-condensed">
                        <div>
                        <h6 class="my-0">@traitement.DenominationLongue</h6>
                        <small class="text-muted">Posologie: @(If(traitement.PosologieBase = "J", traitement.PosologieMatin & "." & traitement.PosologieMidi & "." & traitement.PosologieApresMidi & "." & traitement.PosologieSoir & "/" & traitement.PosologieBase, traitement.PosologieRythme & "/" & traitement.PosologieBase))</small>
                        </div>
                        <span Class="text-muted">---</span>
                    </li>
                    Next
                </ul>
            </div>
            <div Class="col-md-6 order-md-1">
                <form Class="needs-validation" novalidate>
                    <div Class="row">
                        <h4 Class="mb-3">Patient</h4>
                        <div Class="row">
                            <div Class="col-md-6 mb-3">
                                <Label for="firstName">Nom</label>
                                <input type = "text" Class="form-control" id="firstName" placeholder="" value=@ViewBag.Patient.PatientNom required disabled>
                                <div Class="invalid-feedback">
                                    Valid first name Is required.
                                </div>
                            </div>
                            <div Class="col-md-6 mb-3">
                                <Label for="lastName">Prenom</label>
                                <input type = "text" Class="form-control" id="lastName" placeholder="" value=@ViewBag.Patient.PatientPrenom required disabled>
                                <div Class="invalid-feedback">
                                    Valid last name Is required.
                                </div>
                            </div>
                        </div>
                        <div Class="row">
                            <div Class="col-md-6 mb-3">
                                <Label for="birthDate">Date de naissance</label>
                                <input type = "text" Class="form-control" id="birthDate" placeholder="" value=@ViewBag.Patient.PatientDateNaissance required disabled>
                                <div Class="invalid-feedback">
                                    Valid birthDate Is required.
                                </div>
                            </div>
                            <div Class="col-md-6 mb-3">
                                <Label for="CPAM">Immatriculation CPAM</label>
                                <input type = "text" Class="form-control" id="CPAM" placeholder="" value=@ViewBag.Patient.INS required disabled>
                                <div Class="invalid-feedback">
                                    Valid CPAM Is required.
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div Class="row">
                        <h4 Class="mb-3">Medecin</h4>
                        <div Class="row">
                            <div Class="col-md-6 mb-3">
                                <Label for="firstName">Nom</label>
                                <input type = "text" Class="form-control" id="firstName" placeholder="" value=@ViewBag.User.UtilisateurNom required disabled>
                                <div Class="invalid-feedback">
                                    Valid first name Is required.
                                </div>
                            </div>
                            <div Class="col-md-6 mb-3">
                                <Label for="lastName">Prenom</label>
                                <input type = "text" Class="form-control" id="lastName" placeholder="" value=@ViewBag.User.UtilisateurPrenom required disabled>
                                <div Class="invalid-feedback">
                                    Valid last name Is required.
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div Class="row">
                        <h4 Class="mb-3">Information</h4>
                        <div Class="row">
                            <div Class="col-md-6 mb-3">
                                <Label for="date">Date</label>
                                <input type = "text" Class="form-control" id="date" placeholder="" value=@ViewBag.Ordonnance.DateValidation required disabled>
                            </div>
                            <div Class="col-md-6 mb-3">
                                <Label for="renouvelable">Renouvelable</label>
                                <input type = "text" Class="form-control" id="renouvelable" placeholder="" value=@(If(ViewBag.Ordonnance.Renouvellement, ViewBag.Ordonnance.Renouvellement, "Non")) required disabled>
                            </div>
                        </div>
                    </div>
                    <hr />
                </form>
            </div>
        </div>
    </div>
</body>