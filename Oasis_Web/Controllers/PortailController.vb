Imports System.Web.Mvc
Imports Oasis_Common

Namespace Oasis_Web.Controllers

    ''' <summary>
    ''' Classe de base des écrans du portail patient. Le patient et l'internaute
    ''' affichés sont résolus à partir de l'identité authentifiée (le ticket Forms),
    ''' jamais d'un cookie ou d'un paramètre fourni par le client. Cela empêche un
    ''' internaute de consulter le dossier d'un autre patient en modifiant un cookie.
    ''' </summary>
    Public MustInherit Class PortailController
        Inherits Controller

        ''' <summary>
        ''' Identifiant de l'internaute connecté, tiré du ticket d'authentification.
        ''' Nothing si l'identité est absente ou illisible.
        ''' </summary>
        Protected Function GetInternauteIdConnecte() As Integer?
            Dim id As Integer
            If User Is Nothing OrElse User.Identity Is Nothing OrElse Not User.Identity.IsAuthenticated Then
                Return Nothing
            End If
            If Not Integer.TryParse(User.Identity.Name, id) Then
                Return Nothing
            End If
            Return id
        End Function

        ''' <summary>
        ''' Patient rattaché à l'internaute connecté. Nothing si l'internaute n'a
        ''' aucune permission ou n'est pas authentifié.
        ''' </summary>
        Protected Function GetPatientConnecte() As Patient
            Dim internauteId = GetInternauteIdConnecte()
            If Not internauteId.HasValue Then Return Nothing

            Dim permissionDao As New InternautePermissionDao
            Dim permissions = permissionDao.GetPermissionsByInternaute(internauteId.Value)
            If permissions Is Nothing OrElse permissions.Count = 0 Then Return Nothing

            Dim patientDao As New PatientDao
            Return patientDao.GetPatient(permissions(0).Patient)
        End Function

        ''' <summary>
        ''' Résultat standard d'accès refusé (patient non résolu / non autorisé).
        ''' </summary>
        Protected Function AccesRefuse() As ActionResult
            Return New HttpStatusCodeResult(403, "Accès refusé")
        End Function
    End Class

End Namespace
