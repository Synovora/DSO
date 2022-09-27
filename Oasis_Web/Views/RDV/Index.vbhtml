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

<div class="grid">
    <div class="card overflow-hidden g-col-6 mb-0">
        <div class="card-body">
            <div class="d-flex gap-2 justify-content-between align-items-center">
                <div class="">
                    <div class="avatar-md profile-user-wid m-auto">
                        <img src="https://th.bing.com/th/id/R.dc8b1732c919ca17845aab44dc3afb27?rik=qOkrlNPk9Y4cBg&pid=ImgRaw&r=0" alt="" class="img-thumbnail rounded-circle">
                    </div>
                </div>
                <div class="">
                    <h5 class="font-size-15 text-truncate">@ViewBag.Patient.PatientPrenom @ViewBag.Patient.PatientNom</h5>
                    <p class="text-muted mb-0 text-truncate">Profil Patient Nº @Request.Cookies("patientId").Value</p>
                </div>
                <div class="mx-auto d-flex flex-column">
                    <p>Date de naissance: <span class="text-muted">@String.Format(ViewBag.Patient.PatientDateNaissance, "dd/MM/yyyy")</span></p>
                    <p>Genre: <span class="text-muted">@ViewBag.patient.PatientGenre</span></p>
                </div>
                <div class="mx-auto d-flex flex-column">
                    <p>Age: <span class="text-muted">@ViewBag.patient.PatientAge</span></p>
                    <p>NIR: <span class="text-muted">@ViewBag.patient.PatientNir</span></p>
                </div>
            </div>
        </div>
    </div>
    <div class="card g-col-12">
        <div class="card-body">
            <div class="card-title bg-soft-primary">
                <h5>Mes Rendez-Vous</h5>
            </div>
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

@Section Scripts
    <!-- Sweet Alerts js -->
    <script src="~/assets/libs/sweetalert2/sweetalert2.min.js"></script>

    <!-- Sweet alert init js-->
    <script src="~/assets/js/pages/sweet-alerts.init.js"></script>

    <script src="~/assets/js/app.js"></script>
End section
