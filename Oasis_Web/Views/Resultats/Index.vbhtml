@Imports Oasis_Common
@Code
    ViewBag.Title = "Resultats"
    ViewBag.pTitle = "Resultats de " & ViewBag.Patient.PatientPrenom & " " & ViewBag.Patient.PatientNom & " (" & ViewBag.Patient.PatientDateNaissance & ")"
    ViewBag.pageTitle = "Synovora"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="">
    <div class="row">
            <div class="col-xl-3">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title mb-2">Type</h4>
                    @Html.DropDownList("Items", New SelectList(String.Empty, "Value", "Text"), "Tous", htmlAttributes:=New With {.class = "btn btn-secondary dropdown-toggle"})
                </div>
            </div>
        </div>
            <div class="col-xl-3">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title mb-2">Sous Type</h4>
                    @Html.DropDownList("Items", New SelectList(String.Empty, "Value", "Text"), "Tous", htmlAttributes:=New With {.class = "btn btn-secondary dropdown-toggle"})
                </div>
            </div>
        </div>
        </div>
    <div class="row">
    <div class="col-xl-12">
        <div class="card">
            <div class="card-body">
                <h4 class="card-title mb-2">Liste des resultats</h4>
                <div class="table-responsive">
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
                            @For Each item In ViewBag.Resultats
                            @<tr>
                                <td>@item.NomFichier</td>
                                <td>@item.HorodateCreation.ToString()</td>
                            </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
        </div>
</div>
