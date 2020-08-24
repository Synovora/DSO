Imports System.Security.Cryptography

Public Class InternautePermission

    Public Property InternauteId As Integer
    Public Property PatientId As Integer
    Public Property PermissionId As Integer
    Public Property PermissionLevel As Integer

    Public Function Clone() As InternautePermission
        Dim newInstance As InternautePermission = DirectCast(Me.MemberwiseClone(), InternautePermission)
        Return newInstance
    End Function

End Class
