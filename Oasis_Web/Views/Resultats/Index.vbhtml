@Imports Oasis_Common
@Code
    ViewBag.Title = "Resultats"
    ViewBag.pTitle = "Resultats de " & ViewBag.Patient.PatientPrenom & " " & ViewBag.Patient.PatientNom & " (" & Format(ViewBag.Patient.PatientDateNaissance, "dd/MM/yyyy") & ")"
    ViewBag.pageTitle = "Synovora"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    Dim sousEpisodeLibelles As List(Of SelectListItem) = ViewData("sousEpisodeLibelles")
    Dim SousEpisodeSousLibelle As List(Of SelectListItem) = ViewData("SousEpisodeSousLibelle")
End Code

@Section styles
    <link href="~/assets/libs/select2/css/select2.min.css" rel="stylesheet" type="text/css" />
    <link href="~/assets/libs/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet">
    <link href="~/assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    <link href="~/assets/libs/datatables.net-responsive-bs4/css/responsive.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    <link href="~/assets/css/app.css" id="app-style" rel="stylesheet" type="text/css" />
End section

<style>
    .cursor-pointer {
        cursor: pointer;
    }
</style>

<div class="my-4">
    @Using Html.BeginForm("index", "Resultats")
        @<div class="row mb-4">
            <div Class="w-100">
                <div Class="card">
                    <div class="card-header bg-soft-primary">
                        <h5>Filtre</h5>
                    </div>
                    <div Class="card-body d-flex flex-row gap-2">
                        <div>
                            <p Class="mb-2">Type:</p>
                            @Html.DropDownList("MySousEpisodeLibelles", sousEpisodeLibelles, New With {.Class = "form-control", .onchange = "submit();", .autopostback = "true"})
                        </div>
                        @if SousEpisodeSousLibelle.Count > 0 Then
                            @<div>
                                <p Class="mb-2">Sous Type:</p>
                                @Html.DropDownList("MySousEpisodeSousLibelle", SousEpisodeSousLibelle, New With {.Class = "form-control", .onchange = "submit();", .autopostback = "true"})
                            </div>
                        End If
                    </div>
                </div>
            </div>
        </div>
    End Using
    <div class="row">
        <div class="col-xl-12">
            <div class="card">
                <div class="card-header bg-soft-primary">
                    <h5>Liste des resultats</h5>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table mb-0 table-hover">
                            <thead>
                            <thead>
                                <tr>
                                    <th> Type</th>
                                    <th> Sous Type</th>
                                    <th>Date</th>
                                    <th> Pathologie</th>
                                    <th> Conclusion</th>
                                </tr>
                            </thead>
                            <tbody>
                                @For Each item In ViewBag.Resultats
                                    @<div>
                                    <tr data-toggle="collapse" data-target=@("#accordion" + item.Value.ToString) class="clickable cursor-pointer">
                                        <td>@item.Element(0).SousEpisodeLibelle</td>
                                        <td>@item.Element(0).SousEpisodeSousLibelle</td>
                                        <td>@Format(item.Element(0).HorodateCreation, "dd/MM/yyyy")</td>
                                        <td>@item.Element(0).TypeActivite</td>
                                        <td>@item.Element(0).Conclusion</td>
                                    </tr>
                                    <tr id=@("accordion" + item.Value.ToString()) class="collapse border border-primary">
                                        <td colspan="6" class="bg-light">
                                            <table class="table mb-0">
                                                <thead>
                                                <thead>
                                                    <tr>
                                                        <th>Nom du fichier</th>
                                                        <th>Date</th>
                                                        <th>Actions</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    @For Each resultat In item.Element
                                                        @<tr>
                                                            <td>@resultat.NomFichier</td>
                                                            <td>@Format(resultat.HorodateCreation, "le dd/MM/yyyy a HH\hMM")</td>
                                                            <td><a onclick="location.href='@Url.Action("download", "Resultats", New With {Key .fileName = resultat.NomFichier})'" class="btn btn-primary btn-sm w-xs">Voir</a></td>
                                                        </tr>
                                                    Next
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                        </div>

                    Next
                                </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
