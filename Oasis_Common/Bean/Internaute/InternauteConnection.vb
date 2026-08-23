Imports System.Data.SqlClient
Imports System.Security.Cryptography

Public Class InternautePermission

    Public Property Id As Integer
    Public Property Patient As Integer
    Public Property Internaute As Integer
    Public Property Permission As Integer

    Public Sub New()
    End Sub
    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Patient = reader("patient")
        Me.Internaute = reader("internaute")
        Me.Permission = reader("permission")
    End Sub

    Public Function Clone() As InternautePermission
        Dim newInstance As InternautePermission = DirectCast(Me.MemberwiseClone(), InternautePermission)
        Return newInstance
    End Function

End Class
