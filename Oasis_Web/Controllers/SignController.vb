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

                ' Vérification cryptographique : on retrouve l'adresse du signataire à
                ' partir de la charge signée et on la compare à celle enregistrée lors
                ' de la validation. Retrouver la ligne en base ne prouve rien par
                ' lui-même : n'importe quelle ligne portant cette signature s'affichait
                ' auparavant comme authentique, quel qu'en soit le contenu.
                Dim resultat = VerificationSignature.Verifier(ordonnance)
                If resultat = VerificationSignature.ResultatVerification.Invalide Then
                    Return View("~/Views/Sign/Invalide.vbhtml")
                End If
                ViewBag.SignatureVerifiee = (resultat = VerificationSignature.ResultatVerification.Valide)

                ' On affiche le contenu tel qu'il a été signé, pas la ligne vivante,
                ' qui a pu être modifiée depuis la signature.
                Dim ordonnanceSignee = VerificationSignature.OrdonnanceSignee(ordonnance)
                Dim ordonnanceAffichee = If(ordonnanceSignee IsNot Nothing, ordonnanceSignee.Ordonnance, ordonnance)
                ViewBag.Ordonnance = ordonnanceAffichee

                Dim ordonnanceDetails As List(Of OrdonnanceDetail)
                If ordonnanceSignee IsNot Nothing Then
                    ordonnanceDetails = ordonnanceSignee.Details
                Else
                    ordonnanceDetails = ordonnanceDetailDao.GetOrdonnanceLigneByOrdonnanceId(ordonnance.Id)
                End If
                ViewBag.OrdonnanceDetail = ordonnanceDetails

                Dim traitements As New List(Of Traitement)
                For Each detail In ordonnanceDetails
                    traitements.Add(traitementDao.GetTraitementById(detail.TraitementId))
                Next
                ViewBag.Traitements = traitements

                Dim patient = patientDao.GetPatient(ordonnanceAffichee.PatientId)
                If patient Is Nothing Then
                    Return View("~/Views/Sign/Inactif.vbhtml")
                End If
                ' DTO : la vue n'a besoin que de l'identité, et surtout pas du NIR ni
                ' de l'objet Utilisateur complet, qui porte l'empreinte du mot de passe
                ' et la clé privée de signature.
                ViewBag.Patient = New With {
                    .PatientNom = patient.PatientNom,
                    .PatientPrenom = patient.PatientPrenom,
                    .AnneeNaissance = patient.PatientDateNaissance.Year
                }

                Dim user = utilisateurDao.GetUserById(ordonnanceAffichee.UserValidation)
                If user Is Nothing Then
                    Return View("~/Views/Sign/Inactif.vbhtml")
                End If
                ViewBag.User = New With {
                    .UtilisateurNom = user.UtilisateurNom,
                    .UtilisateurPrenom = user.UtilisateurPrenom
                }
            Catch
                Return View("~/Views/Sign/Inactif.vbhtml")
            End Try

            Return View()
        End Function
    End Class
End Namespace
