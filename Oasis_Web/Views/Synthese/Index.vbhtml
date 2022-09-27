@Imports Oasis_Common
@Code
    ViewBag.Title = "Synthese"
    ViewBag.pTitle = "Synthese"
    ViewBag.pageTitle = "Synovora"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


@Section Styles
    <link href="~/assets/libs/sweetalert2/sweetalert2.min.css" rel="stylesheet" type="text/css" />
End Section

<div class="grid mb-4">
    <div class="card overflow-hidden g-col-8 mb-0">
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
    <div class="g-col-4">
        <div class="card h-100">
            <div class="card-body">
                <div class="card-title bg-soft-primary">
                    <h5>Mes Actions</h5>
                </div>
                <div class="text-center">
                    <a href="@Url.Action("Index", "CarnetVaccinal")" class="btn btn-primary btn-sm btn-rounded waves-effect waves-light">
                        Mes Vaccins
                    </a>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-xl-12">
        <div class="card">
            <div class="card-body">
                <div class="card-title bg-soft-primary">
                    <h5>Mes Antecedents</h5>
                </div>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <tbody>
                            @For i As Integer = 0 To ViewBag.Antecedents.Count - 1
                                @<tr>
                                    <td style="@ViewBag.Antecedents(i)(0)">@ViewBag.Antecedents(i)(2)</td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-xl-9">
        <div class="card">
            <div class="card-body">
            <div class="card-title bg-soft-primary">
                <h5>Mes Traitements</h5>
            </div>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <thead>
                            <tr>
                                <th>DCI</th>
                                <th>Posologie</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For i As Integer = 0 To ViewBag.Traitements.Count - 1
                                @<tr>
                                    <td>@ViewBag.Traitements(i)(0)</td>
                                    <td>@ViewBag.Traitements(i)(1)</td>
                                    <td>@(ViewBag.Traitements(i)(2) & ViewBag.Traitements(i)(3))</td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div class="col-xl-3">
        <div class="card">
            <div class="card-body">
                <ul class="nav nav-pills bg-light rounded" role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" data-toggle="tab" href="#transactions-all-tab" role="tab">Allergies</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#transactions-buy-tab" role="tab">Contres indications</a>
                    </li>
                </ul>
                <div class="tab-content mt-4">
                    <div class="tab-pane active" id="transactions-all-tab" role="tabpanel">
                        <div class="table-responsive" data-simplebar style="max-height: 400px;">
                            @ViewBag.Allergies
                        </div>
                    </div>
                    <div class="tab-pane" id="transactions-buy-tab" role="tabpanel">
                        <div class="table-responsive" data-simplebar style="max-height: 400px;">
                            <table class="table table-centered table-nowrap">
                                @ViewBag.ContreIndication
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-xl-12">
        <div class="card">
            <div class="card-body">
            <div class="card-title bg-soft-primary">
                <h5>Mon Parcours de Soin</h5>
            </div>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <thead>
                            <tr>
                                <th>Spécialité</th>
                                <th>Nom</th>
                                <th>Structure</th>
                                <th>Derniere consultation</th>
                                <th>Prochaine consultation</th>
                                <th>Remarque</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For i As Integer = 0 To ViewBag.PS.Count - 1
                                @<tr>
                                    <td>@ViewBag.PS(i)(1)</td>
                                    <td>@ViewBag.PS(i)(3)</td>
                                    <td>@ViewBag.PS(i)(4)</td>
                                    <td>@ViewBag.PS(i)(5)</td>
                                    <td>@ViewBag.PS(i)(6)</td>
                                    <td>@ViewBag.PS(i)(7)</td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-xl-6">
        <div Class="card" id="autoSuiviCard">
            <div Class="card-body">
            <div class="card-title bg-soft-primary">
                <h5>Mes Contextes</h5>
            </div>
                <div Class="clearfix"></div>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <thead>
                            <tr>
                                <th>Description</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For i As Integer = 0 To ViewBag.Contexts.Count - 1
                                @<tr>
                                    <td>@ViewBag.Contexts(i)(1)</td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div class="col-xl-6">
        <div class="card">
            <div class="card-body">
            <div class="card-title bg-soft-primary">
                <h5>Mon Plan Personnalisé de Soin</h5>
            </div>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <tbody>
                            @For i As Integer = 0 To ViewBag.PPS.Count - 1
                                @<tr>
                                    <td>@ViewBag.PPS(i)(0)</td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Modal -->
<div class="modal fade exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Mes Vaccins</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="table-responsive">
                    <table class="table mb-0">
                        <tbody>
                            @For i As Integer = 0 To ViewBag.Vaccins.Count - 1
                                @<tr>
                                    <td>@ViewBag.Vaccins(i)(0)</td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Fermer</button>
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

<script type="text/javascript">
    var att = '@Session["AutoSuivi"]'
    console.log(att)
    if (att === true) {
        $("#autoSuiviCard").hide();
        $("#btnParametreAutoSuiviAdd").removeAttr("disabled");
    }
    $("#btnParametreAutoSuiviValidate").click(function (e) {
        e.preventDefault();
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var that = this;
        Swal.fire({
            title: "Etes-vous sure?",
            text: "Vous ne pourrez pas annuler cette action!",
            showCancelButton: !0,
            confirmButtonColor: "#34c38f",
            cancelButtonColor: "#f46a6a",
            cancelButtonText: "Non",
            confirmButtonText: "Oui, envoyer les parametres!",
        }).then(function (t) {
            if (t.value) {
                console.log('send', $("#myDiv").serialize(), token, $("myDiv").data('url'))
                $.ajax({
                    url: $("#myDiv").data('url'),
                    type: 'POST',
                    data: {
                        __RequestVerificationToken: token,
                        data: $("#myDiv").serialize()
                    },
                    success: function (result) {
                        Swal.fire("Envoye", "Vos parametres d'auto-suivi ont ete envoyes.", "success");
                        $("#autoSuiviCard").hide();
                        $("#btnParametreAutoSuiviAdd").removeAttr("disabled");
                    }
                })
            }
        });
    })
    $("#btnParametreAutoSuiviAdd").click(function (e) {
        e.preventDefault();
        $("#autoSuiviCard").show();
        $("#btnParametreAutoSuiviAdd").attr("disabled", true);
    })
</script>
