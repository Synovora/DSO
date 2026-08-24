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
                If key Is Nothing OrElse Not Regex.IsMatch(key, "^[A-F0-9]{64}$") Then
                    Throw New ArgumentException("La recovery key n'est pas valide.")
                End If

                Dim internaute As Internaute = internauteDao.GetInternauteByRecoveryKey(key)
                If internaute Is Nothing Then
                    Throw New ArgumentException("Internaute introuvable.")
                End If

                If String.IsNullOrEmpty(internaute.Recovery) OrElse internaute.RecoveryExpiration < DateTime.Now Then
                    Throw New ArgumentException("Lien de récupération invalide ou expiré.")
                End If

                ViewBag.Internaute = internaute
                ViewBag.Recovery = key
            Catch ex As ArgumentException
                message = ex.Message
            Catch ex As Exception
                message = "Une erreur est survenue, veuillez réessayer."
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
            Dim message As String = Nothing
            Dim internauteDao As New InternauteDao

            Try
                ' La clé de récupération doit être valide (comme au GET) : on ne
                ' peut pas réinitialiser un mot de passe avec une clé vide, ce qui
                ' permettait de prendre le contrôle du premier compte ayant déjà
                ' effectué une réinitialisation.
                If user.Recovery Is Nothing OrElse Not Regex.IsMatch(user.Recovery, "^[A-F0-9]{64}$") Then
                    Throw New ArgumentException("Le lien de récupération n'est pas valide.")
                End If
                If Not ModelState.IsValid OrElse user.Password <> user.PasswordBis Then
                    Throw New ArgumentException("Les deux mots de passe ne correspondent pas.")
                End If
                If Not isValidePassword(user.Password) Then
                    Throw New ArgumentException("Le mot de passe est trop faible : " & messageFormatPassword)
                End If

                Dim internaute As Internaute = internauteDao.GetInternauteByRecoveryKey(user.Recovery)
                If internaute Is Nothing OrElse String.IsNullOrEmpty(internaute.Recovery) OrElse internaute.RecoveryExpiration < DateTime.Now Then
                    Throw New ArgumentException("Lien de récupération invalide ou expiré.")
                End If

                internaute.Password = user.Password
                internaute.CryptePwd()
                internaute.Recovery = Nothing
                internaute.Code = Nothing
                internaute.RecoveryExpiration = Nothing
                internauteDao.Update(internaute)
                Return RedirectToAction("Login", "Auth")

            Catch ex As ArgumentException
                message = ex.Message
            Catch ex As Exception
                message = "Une erreur est survenue, veuillez réessayer."
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
            Dim patientDao As New PatientDao

            Try
                ' Réponse identique que le compte existe ou non (pas d'énumération).
                Dim internaute = internauteDao.GetInternauteByEmail(user.Email)
                If (internaute Is Nothing) Then
                    Return RedirectToAction("Login", "Auth")
                End If

                ' Clé à usage unique, valable une heure. Le mot de passe existant
                ' n'est PAS effacé : le compte reste utilisable tant que la
                ' réinitialisation n'est pas confirmée.
                Dim ecKey As String = BitConverter.ToString(EthECKey.GenerateKey().GetPrivateKeyAsBytes()).Replace("-", "")
                internauteDao.UpdateRecovery(internaute.Id, ecKey, DateTime.Now.AddHours(1), Nothing)

                ' L'objet transmis au mail doit porter la NOUVELLE clé, sinon le lien
                ' envoyé pointe sur l'ancienne demande.
                internaute.Recovery = ecKey
                internaute.RecoveryExpiration = DateTime.Now.AddHours(1)

                ' Le constructeur charge le modèle de message et effectue les
                ' substitutions (@InternauteRecovery, ...) ; New MailOasis seul
                ' produisait un mail vide.
                Dim mailOasis As New MailOasis(ParametreMail.TypeMailParams.INTERNAUTE_RESET, _Internaute:=internaute)
                mailOasis.AddressTo = user.Email
                EnvoyerMailService(mailOasis)

                Return RedirectToAction("Login", "Auth")
            Catch ex As Exception
                message = "Une erreur est survenue, veuillez réessayer."
            End Try

            ViewBag.Message = message

            Return View()
        End Function

        ''' <summary>
        ''' Envoi d'un mail par le serveur.
        '''
        ''' Le serveur passait par MailOasis.Send, c'est-à-dire par une requête HTTP
        ''' vers sa propre API, émise sur le thread de la requête en cours et bloquée
        ''' sur .Result : un aller-retour réseau vers lui-même, dans le contexte de
        ''' synchronisation d'ASP.NET, avec le risque d'interblocage que cela comporte
        ''' et sans délai maximal. Le serveur a MailUtil et les paramètres SMTP sous la
        ''' main : il envoie directement.
        '''
        ''' Cela supprime du même coup le compte de service : plus personne n'a besoin
        ''' de s'authentifier auprès de l'API pour que le portail envoie un courriel,
        ''' et MailServiceLogin / MailServicePassword deviennent inutiles.
        ''' </summary>
        Private Shared Sub EnvoyerMailService(mailOasis As MailOasis)
            ' La variable ne peut pas s'appeler parametreMail : VB ne distingue pas
            ' la casse, donc ParametreMail dans l'initialiseur désignerait la
            ' variable en cours de déclaration et non le type.
            Dim parametreMailDao As New ParametreMailDao
            Dim parametresSmtp = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(
                Nothing, ParametreMail.TypeMailParams.SMTP_PARAMETERS, inclureSmtp:=True)

            Dim mailUtil As New MailUtil(parametresSmtp.GetSMTPServerUrl(),
                                         parametresSmtp.GetSMTPPort(),
                                         parametresSmtp.GetSMTPUser(False),
                                         parametresSmtp.GetSMTPPassword(False),
                                         parametresSmtp.GetSMTPFrom(False))
            mailUtil.SendMail(Nothing, mailOasis)
        End Sub

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

                ' L'identité authentifiée est la seule source du patient affiché :
                ' plus de cookie patientId/internauteId modifiable par le client.
                FormsAuthentication.SetAuthCookie(internaute.Id.ToString(), user.RememberMe)

                ' Adresse réelle de l'appelant. L'ancienne version interrogeait un
                ' site externe en HTTP à chaque connexion et enregistrait l'IP du
                ' serveur, pas celle du patient.
                internauteConnectionDao.Create(New InternauteConnection With {
                    .Internaute = internaute.Id,
                    .Datetime = Date.Now(),
                    .Ip = AdresseAppelant()
                })
                If (Url.IsLocalUrl(ReturnUrl)) Then
                    Return Redirect(ReturnUrl)
                Else
                    Return RedirectToAction("Index", "Dashboard")
                End If

            Catch ex As ArgumentException
                ' Message identique pour compte inconnu et mot de passe erroné.
                message = ex.Message
            Catch ex As Exception
                message = "Une erreur est survenue, veuillez réessayer."
            End Try

            ViewBag.Message = message
            Return View()
        End Function

        ''' <summary>
        ''' Adresse IP de l'appelant, en tenant compte d'un éventuel reverse proxy.
        ''' </summary>
        Private Function AdresseAppelant() As String
            Dim transmise = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If Not String.IsNullOrWhiteSpace(transmise) Then
                Return transmise.Split(","c)(0).Trim()
            End If
            Return Request.UserHostAddress
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        <Authorize>
        Public Function Logout() As ActionResult
            FormsAuthentication.SignOut()
            Session.Abandon()
            ' Purge des anciens cookies chez les clients qui les ont encore.
            For Each nom In {"patientId", "internauteId"}
                Response.Cookies.Add(New HttpCookie(nom, "") With {.Expires = DateTime.Now.AddDays(-1)})
            Next
            Return RedirectToAction("Login", "Auth")
        End Function
    End Class
End Namespace
