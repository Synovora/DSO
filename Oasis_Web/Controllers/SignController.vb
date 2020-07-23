Imports Microsoft.IdentityModel.Tokens
Imports Oasis_Common

Namespace Controllers
    Public Class SignController
        Inherits Controller

        Function Index() As ActionResult
            Return View()
        End Function

        'GET: /Sign/Check/
        Function Check(id As String) As ActionResult
            Dim ordonnanceDao As New OrdonnanceDaoBase
            Dim patientDao As New PatientDaoBase
            Dim utilisateurDao As New UserDao
            Dim ordonnanceDetailDao As New OrdonnanceDetailDaoBase
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
                Dim ordonnanceDetail = ordonnanceDetailDao.GetOrdonnanceLigneByOrdonnanceId(ordonnance.Id)
                ViewBag.OrdonnanceDetail = ordonnanceDetail
                Dim traitements As New List(Of Traitement)
                For Each detail In ordonnanceDetail
                    traitements.Add(traitementDao.GetTraitementById(detail.TraitementId))
                Next
                ViewBag.Traitements = traitements

                Dim patient = patientDao.GetPatientById(ordonnance.PatientId)
                If patient Is Nothing Then
                    Return View()
                End If
                ViewBag.Patient = patient

                Dim user = utilisateurDao.getUserById(ordonnance.UtilisateurCreation)
                If user Is Nothing Then
                    Return View()
                End If
                ViewBag.User = user
            Catch
                Return View("~/Views/Shared/Error.vbhtml")
            End Try

            Return View()
        End Function
    End Class
End Namespace