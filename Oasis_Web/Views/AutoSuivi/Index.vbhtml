﻿@Imports Oasis_Common
@Code
    ViewBag.Title = "Auto-Suivi"
    ViewBag.pTitle = "Auto-Suivi"
    ViewBag.pageTitle = "Synovora"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


@Section Styles
    <!-- Sweet Alert-->
    <link href="~/assets/libs/sweetalert2/sweetalert2.min.css" rel="stylesheet" type="text/css" />
End Section

<div class="row">
    <div class="col-xl-4">
        <div class="card">
            <div class="card-body">
            </div>
        </div>
    </div>
    <div class="col-xl-8">
        @If (Session("autosuivi") = False) Then
            @<div Class="card" id="autoSuiviCard">
                <div Class="card-body">
                    <h4 Class="card-title mb-4 float-sm-left">Paramatere d'Auto-Suivi</h4>
                    <div Class="clearfix"></div>
                    <div>
                        @Using (Html.BeginForm(Nothing, Nothing, FormMethod.Post, New With {.id = "__AjaxAntiForgeryForm"}))
                            @Html.AntiForgeryToken()
                        End Using
                        <form id="myDiv" Class="form-horizontal" data-url="@Url.Action("AutoSuiviValidate", "AutoSuivi")">
                            <div class="d-flex flex-column flex-wrap justify-flex-start">
                                @For i As Integer = 0 To ViewBag.ParametresAutoSuivi.Count - 1
                                    @<div class="boxAutoSuiviItem d-flex flex-row justify-content-between lh-condensed">
                                        <h6 class="my-0">@(If(ViewBag.ParametresAutoSuivi(i).DescriptionPatient = "", ViewBag.ParametresAutoSuivi(i).Description, ViewBag.ParametresAutoSuivi(i).DescriptionPatient))</h6>
                                        <div Class="">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text" id="validationTooltipUsernamePrepend">@ViewBag.ParametresAutoSuivi(i).Unite</span>
                                                </div>
                                                <input type="text" class="form-control" maxlength="11" id=@("ParametreAutoSuivi-" & ViewBag.ParametresAutoSuivi(i).Id) name=@ViewBag.ParametresAutoSuivi(i).Id placeholder="">
                                            </div>
                                        </div>
                                    </div>
                                Next
                            </div>
                            <div class="clearfix"></div>
                            <div Class="mt-3">
                                <button Class="btn btn-primary btn-block waves-effect waves-light" id="btnParametreAutoSuiviValidate">Valider</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        End If
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
                    }
                })
            }
        });
    })
</script>
