Imports System.Globalization
Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class CarnetVaccinalController
        Inherits Controller

        ReadOnly parametreDao As New ParametreDao
        ReadOnly ordonnanceDao As New OrdonnanceDao
        ReadOnly patientDao As New PatientDao
        ReadOnly rorDao As New RorDao
        ReadOnly userDao As New UserDao
        ReadOnly ordonnanceDetailDao As New OrdonnanceDetailDao
        ReadOnly traitementDao As New TraitementDao
        ReadOnly autoSuiviDao As New AutoSuiviDao
        ReadOnly episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
        ReadOnly episodeDao As New EpisodeDao
        ReadOnly vaccinDao As New VaccinDao
        ReadOnly cgvValenceDao As New CGVValenceDao
        ReadOnly cgvDateDao As New CGVDateDao
        Dim episodeParametreDao As New EpisodeParametreDao

        <Authorize>
        Public Function Index() As ActionResult
            Dim strName As String = Constants.LAYOUT_VERTICAL
            Dim strWelcomeText As String = "Carnet Vaccinal"
            If TempData("ModeName") IsNot Nothing Then strName = TempData("ModeName").ToString()
            If TempData("WelcomeText") IsNot Nothing Then strWelcomeText = TempData("WelcomeText").ToString()
            ViewBag.ModeName = strName
            ViewBag.WelcomeText = strWelcomeText

            If Request.Cookies("patientId") Is Nothing Then
                Return View("~/Views/Pages/pages-500.cshtml")
            End If

            Dim patient = patientDao.GetPatient(Request.Cookies("patientId").Value)
            Dim cgvDates = cgvDateDao.GetListFromPatient(patient.PatientId)
            Dim Vaccins = vaccinDao.GetListVaccinValence()
            ViewBag.Patient = patient
            'ViewBag.Valences = cgvValenceDao.GetListFromPatient(patient.PatientId)
            'ViewBag.Relations = cgvDateDao.GetRelationListFromPatient(patient.PatientId)
            Dim Realisation = New List(Of String)
            Dim Dci = New List(Of String)
            Dim Exp = New List(Of String)
            Dim Lot = New List(Of String)

            For Each cgvDate As CGVDate In cgvDates
                Dim VaccinProgram = vaccinDao.GetFirstVaccinProgramRelationListDatePatient(cgvDate.Id, patient.PatientId)
                Dim VaccinPrograms = vaccinDao.GetVaccinProgramRelationListDatePatient(cgvDate.Id, patient.PatientId)
                Dim isFirstLine = True
                For Each VaccinProgram In VaccinPrograms
                    Dim row As New TableRow()
                    Dim vaccinProgramAdmin = vaccinDao.GetVaccinProgramAdministrationByRelation(VaccinPrograms.Find(Function(x) x.Vaccin = VaccinProgram.Vaccin).Id)
                    If (vaccinProgramAdmin Is Nothing) Then
                        Continue For
                    End If
                    Dim Text1 = ""
                    If (VaccinProgram IsNot Nothing AndAlso VaccinProgram.RealisationDate <> Nothing) Then
                        If (VaccinProgram.RealisationOperator <> Nothing) Then
                            Text1 = GetProfilUserString(userDao.GetUserById(VaccinProgram.RealisationOperator))
                        ElseIf (VaccinProgram.RealisationOperatorRor <> Nothing) Then
                            Text1 = GetProfilUserString(rorDao.GetRorById(VaccinProgram.RealisationOperatorRor))
                        ElseIf (VaccinProgram.RealisationOperatorText <> Nothing) Then
                            Text1 = VaccinProgram.RealisationOperatorText
                        End If
                    End If
                    Realisation.Add(String.Format("{0} - {1}", VaccinProgram.RealisationDate.ToString("dd/MM/yyyy"), Text1))
                    Dci.Add(Vaccins.Find(Function(x) x.Id = VaccinProgram.Vaccin).Dci)
                    Lot.Add(vaccinProgramAdmin.Lot)
                    Exp.Add(vaccinProgramAdmin.Expiration.ToString("MM/yyyy", CultureInfo.GetCultureInfoByIetfLanguageTag("fr-FR")))
                Next
            Next
            ViewBag.Realisation = Realisation
            ViewBag.Dci = Dci
            ViewBag.Exp = Exp
            ViewBag.Lot = Lot
            Return View()
        End Function

    End Class
End Namespace
