@Imports Oasis_Common
@Imports System.Globalization
@Code
    ViewBag.Title = "Accueil"
    ViewBag.pTitle = "Accueil"
    ViewBag.pageTitle = "Synovora"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="grid">
    <div class="card overflow-hidden g-col-8">
        <div class="bg-primary bg-soft">
            <div class="row">
                <div class="col-7">
                    <div class="text-primary p-3">
                        <h5 class="text-primary">Bienvenue sur le DSO en ligne !</h5>
                    </div>
                </div>
                <div class="col-5 align-self-end">
                    <img src="~/assets/images/profile-img.png" alt="" class="img-fluid">
                </div>
            </div>
        </div>
        <div class="card-body pt-0">
            <div class="row">
                <div class="col-sm-4">
                    <div class="avatar-md profile-user-wid mb-4">
                        <img src="https://th.bing.com/th/id/R.dc8b1732c919ca17845aab44dc3afb27?rik=qOkrlNPk9Y4cBg&pid=ImgRaw&r=0" alt="" class="img-thumbnail rounded-circle">
                    </div>
                    <h5 class="font-size-15 text-truncate">@ViewBag.Patient.PatientPrenom @ViewBag.Patient.PatientNom</h5>
                    <p class="text-muted mb-0 text-truncate">Profil Patient Nº @Request.Cookies("patientId").Value</p>
                </div>

                <div class="col-sm-8">
                    <div class="pt-4">
                        <div class="row">
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
    </div>

    <div class="card d-md-flex g-col-4">
        <div class="card-body">
            <h4 class="card-title mb-4">Mes dernières connections</h4>
            <ul class="verti-timeline list-unstyled">
                @For i As Integer = 0 To ViewBag.Connections.Count - 1
                    @<li class="event-list">
                        <div class="event-timeline-dot">
                            <i class="bx bx-right-arrow-circle font-size-18"></i>
                        </div>
                        <div class="d-flex">
                            <div class="flex-shrink-0 me-3">
                                <h5 class="font-size-14">@System.DateTime.Now.ToString("dddd d MMM yyyy à HH\hmm", CultureInfo.GetCultureInfoByIetfLanguageTag("fr-FR"))></h5>
                            </div>
                        </div>
                    </li>
                Next
            </ul>

        </div>
    </div>
</div>

<div class="grid gap-4 mb-4">
    <div class="card g-col-6 mb-0">
        <div class="card-body">
            <h4 class="card-title mb-4">
                Ma Synthèse
            </h4>
            <p class="text-muted">
                C'est un résumé des principaux éléments de votre dossier de santé. Il facilitera grandement votre prise en charge par tout personnel de santé qui ne vous connaîtrait pas.
            </p>
            <button type="button" class="btn btn-primary btn-sm" onClick="document.location.href='/Synthese'">Accéder à ma synthese</button>
        </div>
    </div>
    <div class="card g-col-6 mb-0">
        <div class="card-body">
            <h4 class="card-title mb-4">Mon Auto-Suivi</h4>
            <p class="text-muted">
                C’est un outil qui vous permet de partager avec vos soignants les éléments chiffrés de la surveillance que vous réalisez vous même.
            </p>
            <button type="button" class="btn btn-primary btn-sm" onClick="document.location.href='/AutoSuivi'">Accéder à mon auto-suivi</button>
        </div>
    </div>
    <div class="card g-col-6 mb-0">
        <div class="card-body">
            <h4 class="card-title mb-4">Mes Vaccins</h4>
            <p class="text-muted">
                C’est votre « carnet vaccinal » où sont résumés tous les vaccins dont vous avez bénéficié.
            </p>
            <button type="button" class="btn btn-primary btn-sm" onClick="document.location.href='/CarnetVaccinal'">Accéder à mes vaccins</button>
        </div>
    </div>
    <div class="card g-col-6 mb-0">
        <div class="card-body">
            <h4 class="card-title mb-4">Mes Rendez-Vous</h4>
            <p class="text-muted">
                C’est un résumé de vos rendez-vous avec des professionnels de santé.
            </p>
            <button type="button" class="btn btn-primary btn-sm" onClick="document.location.href='/RDV'">Accéder à mes rendez-vous</button>
        </div>
    </div>
    <div class="card g-col-6 mb-0">
        <div class="card-body">
            <h4 class="card-title mb-4">Mes Résultats</h4>
            <p class="text-muted">
                C’est un accès à l’ensemble de vos résultats d’analyses biologiques, d’examens radiologiques, d’avis spécialisés réalisés et communiqués à votre référent de santé.
            </p>
            <button type="button" class="btn btn-primary btn-sm" onClick="document.location.href='/Resultats'">Acceder a mes résultats</button>
        </div>
    </div>
</div>
</div>

@Section Scripts
    <!-- apexcharts -->
    @*<script src="~/assets/libs/apexcharts/apexcharts.min.js"></script>

        <script src="~/assets/js/pages/dashboard.init.js"></script>*@

    <!-- App js -->
    <script src="~/assets/js/app.js"></script>
End section

<script type="text/javascript">

    $('#myDiv').submit(function (e) {
        e.preventDefault();
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            url: $(this).data('url'),
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                data: $("#myDiv").serialize()
            },
            success: function (result) {
                alert(result.someValue);
            }
        });
        return false;
    });
</script>
