Imports Microsoft.IdentityModel.Tokens
Imports Oasis_Common

Namespace Controllers
    Public Class SignController
        Inherits Controller

        <AllowAnonymous>
        Function Check(id As String) As ActionResult
            Dim ordonnanceDao As New OrdonnanceDao
            Dim patientDao As New PatientDao
            Dim utilisateurDao As New UserDao
            Dim ordonnanceDetailDao As New OrdonnanceDetailDao
            Dim traitementDao As New TraitementDao

            ViewBag.traitementDao = traitementDao

            Try
                Dim signatue As Byte() = Base64UrlEncoder.DecodeBytes(id)
                Dim sigHex As String = "0x" & LCase(BitConverter.ToString(signatue).Replace("-", String.Empty))
                Dim ordonnance = ordonnanceDao.GetOrdonnaceBySignature(sigHex)

                If ordonnance.Inactif = True Then
                    Return View("~/Views/Sign/Inactif.vbhtml")
                End If

                ViewBag.Ordonnance = ordonnance
                Dim ordonnanceDetails = ordonnanceDetailDao.GetOrdonnanceLigneByOrdonnanceId(ordonnance.Id)
                ViewBag.OrdonnanceDetail = ordonnanceDetails
                Dim traitements As New List(Of Traitement)
                For Each detail In ordonnanceDetails
                    traitements.Add(traitementDao.GetTraitementById(detail.TraitementId))
                Next
                ViewBag.Traitements = traitements

                Dim patient = patientDao.GetPatient(ordonnance.PatientId)
                If patient Is Nothing Then
                    Return View()
                End If
                ViewBag.Patient = patient
                Dim user = utilisateurDao.GetUserById(ordonnance.UserValidation)
                If user Is Nothing Then
                    Return View()
                End If
                ViewBag.User = user
            Catch
                Return View("~/Views/Sign/Inactif.vbhtml")
            End Try

            Return View()
        End Function
    End Class
End Namespace