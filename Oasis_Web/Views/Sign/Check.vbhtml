@Code
    ViewBag.Title = "Ordonnance Page"
    Layout = "~/Views/Shared/_BlankLayout.cshtml"
End Code

<div class="container">
    <div class="py-5 text-center">
        <h2> Verification d'Ordonnance</h2>
        <p class="lead">Verifier rapidement et simplement l'authenticite d'une ordonnance</p>
    </div>
    <div class="row">
        <div class="col-md-6 order-md-2 mb-6">
            <h4 class="d-flex justify-content-between align-items-center mb-3">
                <span class="text-muted">Details</span>
                <span class="badge badge-secondary badge-pill">@ViewBag.Traitements.Count</span>
            </h4>
            <ul class="list-group mb-3">
                @For i As Integer = 0 To ViewBag.Traitements.Count - 1
                    @<li class="list-group-item d-flex justify-content-between lh-condensed">
                        <div>
                            <h6 class="my-0">@ViewBag.Traitements(i).DenominationLongue</h6>
                            <small class="text-muted">Posologie @ViewBag.OrdonnanceDetail(i).Posologie</small>
                            @If ViewBag.Traitements(i).PosologieBase = "C" Then
                                @<small Class="text-muted">@ViewBag.Traitements(i).PosologieCommentaire</small>
                            End If
                        </div>

                        <span class="text-muted">
                            @If (ViewBag.OrdonnanceDetail(i).ADelivrer) Then
                                @<svg width="1em" height="1em" viewBox="0 0 16 16" Class="bi bi-arrow-right-circle" fill="currentColor" xmlns="http:  //www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" d="M8 15A7 7 0 1 0 8 1a7 7 0 0 0 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                                    <path fill-rule="evenodd" d="M7.646 11.354a.5.5 0 0 1 0-.708L10.293 8 7.646 5.354a.5.5 0 1 1 .708-.708l3 3a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0z" />
                                    <path fill-rule="evenodd" d="M4.5 8a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1H5a.5.5 0 0 1-.5-.5z" />
                                </svg>
                            Else
                                @<svg width="1em" height="1em" viewBox="0 0 16 16" Class="bi bi-slash-circle" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" d="M8 15A7 7 0 1 0 8 1a7 7 0 0 0 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                                    <path fill-rule="evenodd" d="M11.854 4.146a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708-.708l7-7a.5.5 0 0 1 .708 0z" />
                                </svg>
                            End If

                        </span>
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
                            <Label for="firstName">Nom</Label>
                            <input type="text" Class="form-control" id="firstName" placeholder="" value=@ViewBag.Patient.PatientNom required disabled>
                            <div Class="invalid-feedback">
                                Valid first name Is required.
                            </div>
                        </div>
                        <div Class="col-md-6 mb-3">
                            <Label for="lastName">Prenom</Label>
                            <input type="text" Class="form-control" id="lastName" placeholder="" value=@ViewBag.Patient.PatientPrenom required disabled>
                            <div Class="invalid-feedback">
                                Valid last name Is required.
                            </div>
                        </div>
                    </div>
                    <div Class="row">
                        <div Class="col-md-6 mb-3">
                            <Label for="birthDate">Date de naissance</Label>
                            <input type="text" Class="form-control" id="birthDate" placeholder="" value=@Format(ViewBag.Patient.PatientDateNaissance, "dd/MM/yyyy") required disabled>
                            <div Class="invalid-feedback">
                                Valid birthDate Is required.
                            </div>
                        </div>
                        <div Class="col-md-6 mb-3">
                            <Label for="CPAM">Immatriculation CPAM</Label>
                            <input type="text" Class="form-control" id="CPAM" placeholder="" value=@ViewBag.Patient.PatientNir required disabled>
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
                            <Label for="firstName">Nom</Label>
                            <input type="text" Class="form-control" id="firstName" placeholder="" value=@ViewBag.User.UtilisateurNom required disabled>
                            <div Class="invalid-feedback">
                                Valid first name Is required.
                            </div>
                        </div>
                        <div Class="col-md-6 mb-3">
                            <Label for="lastName">Prenom</Label>
                            <input type="text" Class="form-control" id="lastName" placeholder="" value=@ViewBag.User.UtilisateurPrenom required disabled>
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
                            <Label for="date">Date</Label>
                            <input type="text" Class="form-control" id="date" placeholder="" value=@Format(ViewBag.Ordonnance.DateValidation, "dd/MM/yyyy") required disabled>
                        </div>
                        <div Class="col-md-6 mb-3">
                            <Label for="renouvelable">Renouvelable</Label>
                            <input type="text" Class="form-control" id="renouvelable" placeholder="" value=@(If(ViewBag.Ordonnance.Renouvellement, ViewBag.Ordonnance.Renouvellement, "Non")) required disabled>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</div>