Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Models

Namespace Oasis_Web.Controllers

    Public Class AuthController
        Inherits Controller

        <AllowAnonymous>
        Public Function Index() As ActionResult
            Return View("login")
        End Function

        <AllowAnonymous>
        <ActionName("login")>
        Public Function authlogin() As ActionResult
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("auth-register")>
        Public Function authregister() As ActionResult
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("auth-recoverpw")>
        Public Function authrecoverpw() As ActionResult
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("auth-lock-screen")>
        Public Function authlockscreen() As ActionResult
            Return View()
        End Function

        Function RandomString(r As Random) As String
            Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
            Dim sb As New StringBuilder
            Dim cnt As Integer = r.Next(15, 33)
            For i As Integer = 1 To cnt
                Dim idx As Integer = r.Next(0, s.Length)
                sb.Append(s.Substring(idx, 1))
            Next
            Return sb.ToString()
        End Function

        Function RemoveDiacritics(ByVal text As String) As String
            Dim normalizedString = text.Normalize(NormalizationForm.FormD)
            Dim stringBuilder = New StringBuilder()

            For Each c In normalizedString
                Dim unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c)

                If unicodeCategory <> UnicodeCategory.NonSpacingMark Then
                    stringBuilder.Append(c)
                End If
            Next

            Return stringBuilder.ToString().Normalize(NormalizationForm.FormC)
        End Function

        'Register POST
        <HttpPost>
        <ValidateAntiForgeryToken>
        <AllowAnonymous>
        Public Function Register(user As UserLogin, ReturnUrl As String) As ActionResult
            Dim message As String
            Dim internauteDao As InternauteDao = New InternauteDao
            Dim patientDao As PatientDao = New PatientDao
            Dim internautePermissionDao As InternautePermissionDao = New InternautePermissionDao

            Try
                'TODO: check if the NIR is valid
                'TODO: May have problem with Dep that contain Alpha char
                If IsNumeric(user.NIR) And Oasis_Common.Patient.IsValidNIR(CDec(user.NIR)) = False Then
                    Throw New ArgumentException("Le NIR n'est pas valide.")
                End If
                'TODO: check if internaute already exist
                If internauteDao.GetInternauteByNIR(user.NIR) IsNot Nothing Then
                    Throw New ArgumentException("Le NIR est deja attribue a un internaute existant.")
                End If
                'TODO: check patient NIR
                Dim patient As Patient = patientDao.GetPatientByNIR(user.NIR)
                If patient Is Nothing Then
                    Throw New ArgumentException("Le NIR ne correspond a aucun patient.")
                End If
                'TODO: Check the patient's Name
                If RemoveDiacritics(UCase(patient.PatientNom)) <> RemoveDiacritics(UCase(user.Nom)) Then
                    Throw New ArgumentException("Le nom ne correspond pas au nom du patient.")
                End If

                'TODO: Check validity
                'If Check patient oasis OR (date entre exist et inf actuel AND date de sortie sup actuel) OR (date ouverture moins d'un an)
                If (patient.PatientDateEntree > DateTime.Now And patient.PatientDateSortie < DateTime.Now) Then
                    Throw New ArgumentException("Les dates d'entrees et de sorties du patient ne sont pas correct.")
                End If

                Dim internaute As Internaute = New Internaute
                'With {
                '.PatientId = patient.PatientId
                '}

                'Create User
                Dim r As New Random
                Internaute.Password = RandomString(r)
                Debug.WriteLine(Internaute.Password)
                internauteDao.Create(Internaute)

                If (Url.IsLocalUrl(ReturnUrl)) Then
                    Return Redirect(ReturnUrl)
                Else
                    Return RedirectToAction("Index", "Dashboard")
                End If

            Catch ex As Exception
                message = ex.Message
            End Try
            ViewBag.Message = message
            Return View()
        End Function

        'Login POST
        <HttpPost>
        <ValidateAntiForgeryToken>
        <AllowAnonymous>
        Public Function Login(user As UserLogin, ReturnUrl As String) As ActionResult
            Dim message As String
            Dim internauteDao As InternauteDao = New InternauteDao
            Dim internautePermissionDao As InternautePermissionDao = New InternautePermissionDao
            Dim internaute As Internaute
            Try
                internaute = internauteDao.GetInternauteByLoginPassword(user.Username, user.Password)
                Dim timeout As Integer = If(user.RememberMe, 525600, 20)
                Dim ticket = New FormsAuthenticationTicket(user.Username, user.RememberMe, timeout)
                Dim encrypted As String = FormsAuthentication.Encrypt(ticket)
                Dim cookie = New HttpCookie(FormsAuthentication.FormsCookieName, encrypted) With {
                    .Expires = DateTime.Now.AddMinutes(timeout),
                    .HttpOnly = True
                }
                Response.Cookies.Add(cookie)
                Response.Cookies("internauteId").Value = internaute.Id
                Response.Cookies("internauteId").Expires = DateTime.Now.AddDays(90)
                Session("internauteId") = internaute.Id
                Dim internautePermission = internautePermissionDao.GetPermissionsByInternaute(internaute.Id)
                'Session("patientId") = internautePermission(0).PatientId
                Response.Cookies("patientId").Value = internautePermission(0).PatientId
                Response.Cookies("patientId").Expires = DateTime.Now.AddDays(90)
                If (Url.IsLocalUrl(ReturnUrl)) Then
                    Return Redirect(ReturnUrl)
                Else
                    Return RedirectToAction("Index", "Dashboard")
                End If

            Catch ex As Exception
                message = ex.Message
            End Try

            ViewBag.Message = message
            Return View()
        End Function

        <HttpPost>
        <Authorize>
        Public Function Logout() As ActionResult
            FormsAuthentication.SignOut()
            Session.Abandon()
            Return RedirectToAction("Login", "Auth")
        End Function
    End Class
End Namespace
