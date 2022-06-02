Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Models
Imports Nethereum.Signer

Namespace Oasis_Web.Controllers

    Public Class AuthController
        Inherits Controller

        <AllowAnonymous>
        Public Function Index() As ActionResult
            Return View("login")
        End Function

        <AllowAnonymous>
        <ActionName("login")>
        Public Function Login() As ActionResult
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("forgot")>
        Public Function Forgot() As ActionResult
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("recover")>
        Public Function Recover(key As String) As ActionResult
            Dim message As String = Nothing
            Dim internauteDao As New InternauteDao
            Dim patientDao As New PatientDao
            Dim internautePermissionDao As New InternautePermissionDao

            Try
                If key Is Nothing OrElse Not Regex.IsMatch(key, "^[A-F0-9]{64}") Then
                    Throw New ArgumentException("La recovery key n'est pas valide.")
                End If

                Dim internaute As Internaute = internauteDao.GetInternauteByRecoveryKey(key)
                If internaute Is Nothing Then
                    Throw New ArgumentException("Internaute introuvable.")
                End If

                ViewBag.Internaute = internaute
                ViewBag.Recovery = key
            Catch ex As Exception
                message = ex.Message
            End Try

            ViewBag.Message = message
            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("register")>
        Public Function Register() As ActionResult
            Return View()
        End Function


        <HttpPost>
        <ValidateAntiForgeryToken>
        <AllowAnonymous>
        Public Function Recover(user As UserRecover) As ActionResult
            Dim message As String
            Dim internauteDao As New InternauteDao
            Dim internautePermissionDao As New InternautePermissionDao

            Try
                Dim internaute As Internaute = internauteDao.GetInternauteByRecoveryKey(user.Recovery)
                If internaute.Code <> user.Code Then
                    Throw New Exception("Le code SMS n'est pas le bon")
                End If
                internaute.Password = user.Password
                internaute.Recovery = ""
                internaute.Code = ""
                internaute.CryptePwd()
                internauteDao.Update(internaute)
                Return RedirectToAction("Login", "Auth")

            Catch ex As Exception
                message = ex.Message
            End Try

            ViewBag.Message = message

            Return View()
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        <AllowAnonymous>
        Public Function Forgot(user As UserForgot) As ActionResult
            Dim message As String
            Dim internauteDao As New InternauteDao
            Dim internautePermissionDao As New InternautePermissionDao

            Try
                Dim internaute = internauteDao.GetInternauteByEmail(user.Email)
                If (internaute Is Nothing) Then
                    Return RedirectToAction("Login", "Auth")
                End If
                Dim internautePermission = internautePermissionDao.GetPermissionsByInternaute(internaute.Id)
                Dim ecKey As String = BitConverter.ToString(EthECKey.GenerateKey().GetPrivateKeyAsBytes()).Replace("-", "")
                Dim internautePermissions = internautePermissionDao.GetPermissionsByPatient(internautePermission(0).Patient)
                Dim internauteId = internauteDao.Update(New Internaute With {
                    .Id = internautePermissions(0).Internaute,
                    .Password = "",
                .Recovery = ecKey,
                    .Code = "0000"
                })
                If internauteId > 0 Then
                    Dim mailOasis As New MailOasis
                    mailOasis.IsSousEpisode = False
                    mailOasis.Type = ParametreMail.TypeMailParams.PWD_GENERATE
                    mailOasis.Send(New LoginRequest With {
                                   .login = "Bertrand.Gambet",
                                   .password = "a"})
                End If
                Return RedirectToAction("Login", "Auth")

            Catch ex As Exception
                message = ex.Message
            End Try

            ViewBag.Message = message

            Return View()
        End Function

        <AllowAnonymous>
        <ActionName("lock-screen")>
        Public Function LockScreen() As ActionResult
            Return View()
        End Function

        'Register POST
        '<HttpPost>
        '<ValidateAntiForgeryToken>
        '<AllowAnonymous>
        'Public Function Register(user As UserLogin, ReturnUrl As String) As ActionResult
        '    Dim message As String
        '    Dim internauteDao As New InternauteDao
        '    Dim patientDao As New PatientDao
        '    Dim internautePermissionDao As New InternautePermissionDao

        '    Try
        '        'TODO: check if the NIR is valid
        '        'TODO: May have problem with Dep that contain Alpha char
        '        If IsNumeric(user.NIR) And Oasis_Common.Patient.IsValidNIR(CDec(user.NIR)) = False Then
        '            Throw New ArgumentException("Le NIR n'est pas valide.")
        '        End If
        '        'TODO: check if internaute already exist
        '        If internauteDao.GetInternauteByNIR(user.NIR) IsNot Nothing Then
        '            Throw New ArgumentException("Le NIR est deja attribue a un internaute existant.")
        '        End If
        '        'TODO: check patient NIR
        '        Dim patient As Patient = patientDao.GetPatientByNIR(user.NIR)
        '        If patient Is Nothing Then
        '            Throw New ArgumentException("Le NIR ne correspond a aucun patient.")
        '        End If
        '        'TODO: Check the patient's Name
        '        If RemoveDiacritics(UCase(patient.PatientNom)) <> RemoveDiacritics(UCase(user.Nom)) Then
        '            Throw New ArgumentException("Le nom ne correspond pas au nom du patient.")
        '        End If

        '        'TODO: Check validity
        '        'If Check patient oasis OR (date entre exist et inf actuel AND date de sortie sup actuel) OR (date ouverture moins d'un an)
        '        If (patient.PatientDateEntree > DateTime.Now And patient.PatientDateSortie < DateTime.Now) Then
        '            Throw New ArgumentException("Les dates d'entrees et de sorties du patient ne sont pas correct.")
        '        End If

        '        Dim internaute As New Internaute

        '        'Create User
        '        Dim r As New Random
        '        Debug.WriteLine(internaute.Password)
        '        internauteDao.Create(Internaute)

        '        If (Url.IsLocalUrl(ReturnUrl)) Then
        '            Return Redirect(ReturnUrl)
        '        Else
        '            Return RedirectToAction("Index", "Dashboard")
        '        End If

        '    Catch ex As Exception
        '        message = ex.Message
        '    End Try
        '    ViewBag.Message = message
        '    Return View()
        'End Function

        'Login POST
        <HttpPost>
        <ValidateAntiForgeryToken>
        <AllowAnonymous>
        Public Function Login(user As UserLogin, ReturnUrl As String) As ActionResult
            Dim message As String
            Dim internauteDao As New InternauteDao
            Dim internautePermissionDao As New InternautePermissionDao
            Dim internauteConnectionDao As New InternauteConnectionDao

            Try
                Dim internaute As Internaute = internauteDao.GetInternauteByLoginPassword(user.Email, user.Password)
                Dim timeout As Integer = If(user.RememberMe, 525600, 20)
                Dim ticket = New FormsAuthenticationTicket(user.Email, user.RememberMe, timeout)
                FormsAuthentication.SetAuthCookie(internaute.Id, True)
                Dim internautePermission = internautePermissionDao.GetPermissionsByInternaute(internaute.Id)
                Response.Cookies("patientId").Value = internautePermission(0).Patient
                Response.Cookies("patientId").Expires = DateTime.Now.AddDays(90)
                Response.Cookies("internauteId").Value = internaute.Id
                Response.Cookies("internauteId").Expires = DateTime.Now.AddDays(90)
                'Dim strHostName = System.Net.Dns.GetHostName()
                Dim outputIP As String
                Using wClient As New WebClient
                    outputIP = Regex.Match(wClient.DownloadString("http://www.ip-adress.com/"), "(?<=<h2>My IP address is: )[0-9.]*?(?=</h2>)", RegexOptions.Compiled).Value

                End Using
                internauteConnectionDao.Create(New InternauteConnection With {
                    .Internaute = internaute.Id,
                    .Datetime = Date.Now(),
                    .Ip = outputIP
                })
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
