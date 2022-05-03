@Imports Oasis_Common
@Code
    ViewBag.Title = "Carnet Vaccinal"
    ViewBag.pTitle = "Carnet Vaccinal de " & ViewBag.Patient.PatientPrenom & " " & ViewBag.Patient.PatientNom & " (" & ViewBag.Patient.PatientDateNaissance & ")"
    ViewBag.pageTitle = "Synovora"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="row">
    <div class="col-xl-12">
        <div class="card">
            <div class="card-body">
                <h4 class="card-title mb-2">Liste des vaccins au @Date.Now().ToString("dd/MM/yyyy")</h4>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <thead>
                        <thead>
                            <tr>
                                <th>Nom</th>
                                <th>Realisation</th>
                                <th>Lot</th>
                                <th>Expiration</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For i As Integer = 0 To ViewBag.Realisation.Count - 1
                                @<tr>
                                    <td>@ViewBag.Dci(i)</td>
                                    <td>@ViewBag.Realisation(i)</td>
                                    <td>@ViewBag.Lot(i)</td>
                                    <td>@ViewBag.Exp(i)</td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
