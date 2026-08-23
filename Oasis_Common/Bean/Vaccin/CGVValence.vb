Imports System.Data.SqlClient

Public Class CGVValence

    Property Id As Long
    Property Code As String
    Property Description As String
    Property Precaution As String
    Property Valence As Long
    Property Ordre As Integer
    Property Patient As Long

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Code = reader("code")
        Me.Description = reader("description")
        Me.Precaution = reader("precaution")
        Me.Valence = reader("valence")
        Me.Ordre = reader("ordre")
        Me.Patient = reader("patient")
    End Sub

End Class

