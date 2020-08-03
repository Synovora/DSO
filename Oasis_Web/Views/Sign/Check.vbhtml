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
                            <input type="text" Class="form-control" id="birthDate" placeholder="" value=@ViewBag.Patient.PatientDateNaissance required disabled>
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
                            <input type="text" Class="form-control" id="date" placeholder="" value=@ViewBag.Ordonnance.DateValidation required disabled>
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