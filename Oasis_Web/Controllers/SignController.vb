Imports System.Web.Mvc
Imports Oasis_Common

Namespace Controllers
    Public Class SignController
        Inherits Controller

        Function Index() As ActionResult
            Return View()
        End Function

        'GET: /Sign/Check/
        Function Check(Optional id As Integer = 10105) As ActionResult

            Dim ordonnanceDao As New OrdonnanceDaoBase
            Dim patientDao As New PatientDaoBase
            Dim utilisateurDao As New UserDao
            Dim ordonnanceDetailDao As New OrdonnanceDetailDaoBase
            Dim traitementDao As New TraitementDaoBase

            Dim ordonnance = ordonnanceDao.GetOrdonnaceById(id)
            'If ordonnance Is Nothing Then
            'Return View()
            'End If
            If ordonnance.Inactif = True Then
                Return View()
            End If
            ViewBag.Ordonnance = ordonnance

            Dim ordonnanceDetail = ordonnanceDetailDao.GetOrdonnanceLigneByOrdonnanceId(id)
            'If ordonnanceDetail Is Nothing Then
            'Return View()
            'End If
            ViewBag.OrdonnanceDetail = ordonnanceDetail
            Dim traitements As New List(Of TraitementBase)
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
            Return View()
        End Function
    End Class
End Namespace