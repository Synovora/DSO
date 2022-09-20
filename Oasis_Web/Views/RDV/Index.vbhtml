@Imports Oasis_Common
@Code
    ViewBag.Title = "RDV"
    ViewBag.pTitle = "Rendez-Vous"
    ViewBag.pageTitle = "Synovora"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


@Section Styles
    <!-- Sweet Alert-->
    <link href="~/assets/libs/sweetalert2/sweetalert2.min.css" rel="stylesheet" type="text/css" />
End Section

<div class="row my-4">
    <div class="col-xl-6">
        <div class="card">
            <div class="card-header bg-soft-primary">
                <div class="text-primary">
                    <h5 class="text-primary">@ViewBag.Patient.PatientPrenom @ViewBag.Patient.PatientNom</h5>
                </div>
            </div>
            <div class="card-body">
                <div class="row mb-3">
                    <div class="col-md-6">
                        <p class="text-muted">Date de naissance: @String.Format(ViewBag.Patient.PatientDateNaissance, "dd/MM/yyyy")</p>
                        <p class="text-muted">Genre: @ViewBag.patient.PatientGenre</p>
                    </div>
                    <div class="col-md-6">
                        <p class="text-muted">Age: @ViewBag.patient.PatientAge</p>
                        <p class="text-muted">NIR: @ViewBag.patient.PatientNir</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-xl-12">
        <div class="card">
            <div class="card-header bg-soft-primary">
                <h5>Mes Rendez-Vous</h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table mb-0">
                        <thead>
                        <thead>
                            <tr>
                                <th>Spécialité</th>
                                <th>Nom</th>
                                <th>Structure</th>
                                <th>Prochain RDV</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For i As Integer = 0 To ViewBag.ParcoursDeSoin.Count - 1
                                @<tr>
                                    <td>@ViewBag.ParcoursDeSoin(i)(1)</td>
                                    <td>@ViewBag.ParcoursDeSoin(i)(2)</td>
                                    <td>@ViewBag.ParcoursDeSoin(i)(3)</td>
                                    <td>@ViewBag.ParcoursDeSoin(i)(4)</td>

                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>

@Section Scripts
    <!-- Sweet Alerts js -->
    <script src="~/assets/libs/sweetalert2/sweetalert2.min.js"></script>

    <!-- Sweet alert init js-->
    <script src="~/assets/js/pages/sweet-alerts.init.js"></script>

    <script src="~/assets/js/app.js"></script>
End section
