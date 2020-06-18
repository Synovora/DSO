Imports System.Web.Mvc

Namespace Controllers
    Public Class SignController
        Inherits Controller

        Function Index() As ActionResult
            Return View()
        End Function

        'GET: /Sign/Check/
        Function Check(Optional id As Integer = 1) As ActionResult
            Dim db As oasisEntities = New oasisEntities()
            Dim ordonnance As oa_patient_ordonnance = db.oa_patient_ordonnance.SingleOrDefault(Function(x As oa_patient_ordonnance) x.oa_ordonnance_id = id)
            If ordonnance Is Nothing Then
                Return View()
            End If
            Dim patient As oa_patient = db.oa_patient.SingleOrDefault(Function(x As oa_patient) x.oa_patient_id = ordonnance.oa_ordonnance_patient_id)
            If patient Is Nothing Then
                Return View()
            End If
            ViewBag.Ordonnance = ordonnance
            ViewBag.Patient = patient
            Return View()
        End Function
    End Class
End Namespace