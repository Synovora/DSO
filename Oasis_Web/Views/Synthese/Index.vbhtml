@Imports Oasis_Common
@Code
    ViewBag.Title = "Synthese"
    ViewBag.pTitle = "Synthese"
    ViewBag.pageTitle = "Synovora"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


@Section Styles
    <!-- Sweet Alert-->
    <link href="~/assets/libs/sweetalert2/sweetalert2.min.css" rel="stylesheet" type="text/css" />
End Section

<div class="row">
    <div class="col-xl-6">
        <div class="card text-center">
            <div class="card-body">
                <h5 class="font-size-15"><a href="#" class="text-dark">@ViewBag.patient.PatientPrenom @ViewBag.patient.PatientNom</a></h5>
                <div class="row mb-3">
                    <div class="col-md-6">
                        <div>
                            <p class="text-muted">Date de naissance: @ViewBag.patient.PatientDateNaissance.ToShortDateString()</p>
                            <p class="text-muted">Genre: @ViewBag.patient.PatientGenre</p>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div>
                            <p class="text-muted">Age: @ViewBag.patient.PatientAge</p>
                            <p class="text-muted">NIR: @ViewBag.patient.PatientNir</p>
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
                <h4 class="card-title">Antecedents</h4>
                <div Class="clearfix"></div>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <thead>
                            <tr>
                                @*<th>#</th>*@
                                <th>Description</th>

                            </tr>
                        </thead>
                        <tbody>
                            @For i As Integer = 0 To ViewBag.Antecedents.Count - 1
                                @<tr>
                                    @*<th scope="row">@i</th>*@
                                    <td>@ViewBag.Antecedents(i)(1)</td>
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
    <div class="col-xl-12">
        <div Class="card">
            <div Class="card-body">
                <h4 Class="card-title mb-4">Contexte</h4>
                <div Class="clearfix"></div>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <thead>
                            <tr>
                                @*<th>#</th>*@
                                <th>Description</th>
                                <th>Domaine</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For i As Integer = 0 To ViewBag.Contexts.Count - 1
                                @<tr>
                                    @*<th scope="row">@i</th>*@
                                    <td>@ViewBag.Contexts(i)(1)</td>
                                    <td>@ViewBag.Contexts(i)(0)</td>
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
            //type: "info",
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
